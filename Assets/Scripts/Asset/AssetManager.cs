using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine.Networking;

using com.jbg.asset.control;
using com.jbg.core;
using com.jbg.core.manager;

namespace com.jbg.asset
{
    using Manager = AssetManager;

    public class AssetManager
    {
        public static bool IsOpened { get; private set; }

        public static bool DownloadDone { get; private set; }
        public static string CurrentAsset { get; private set; }

        private static UnityWebRequest currentRequest = new();

        private const string CLASSNAME = "AssetManager";

        public static readonly string PATH_ASSET = UnityEngine.Application.persistentDataPath + "/Asset/";
        public static readonly string PATH_ASSET_TABLE_VERSION_DATA = Manager.PATH_ASSET + TableVersionControl.TABLE_NAME + ".csv";

        private struct DownloadData
        {
            public string tableName;
            public string localPath;
            public UnityWebRequest request;
            public System.Action<string> updateDataCallback;
        }

        public static void Open()
        {
            Manager.Close();
            Manager.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            Manager.DownloadDone = false;
            Manager.CurrentAsset = string.Empty;
            Manager.currentRequest = null;

            // 각종 에셋 오픈
            TableVersionControl.Open();
        }

        public static void Close()
        {
            if (Manager.IsOpened)
            {
                Manager.IsOpened = false;

                SystemManager.RemoveOpenList(CLASSNAME);

                // 각종 에셋 클로즈
                TableVersionControl.Close();
            }
        }

        public static IEnumerator DownloadAsset()
        {
            // 로딩 과정 시작
            Manager.DownloadDone = false;

            // TODO[jbg] : 로딩할 것들 목록 만들기
            List<DownloadData> downloadList = new();

            // 리스트 순회하면서 다운로드 순차 진행
            for (int i = 0; i < downloadList.Count; i++)
            {
                DownloadData downloadData = downloadList[i];
                string tableName = downloadData.tableName;
                string localPath = downloadData.localPath;

                Manager.CurrentAsset = tableName;

                // 다운로드 필요한가 검사
                if (TableVersionControl.NeedDownload(tableName) == false && File.Exists(localPath))
                    continue;

                // 다운로드 경로 가져오기
                string downloadPath = GoogleSheetConfig.GetDownloadPath(tableName);
                if (string.IsNullOrEmpty(downloadPath))
                {
                    string message = string.Format("[ASSET_DOWNLOAD] {0} Download Path is null. Check DLL File.", tableName);
                    DebugEx.LogColor(message, "red");
                    continue;
                }

                // 다운로드 시작
                downloadData.request = UnityWebRequest.Get(downloadPath);
                Manager.currentRequest = downloadData.request;

                yield return downloadData.request.SendWebRequest();

                // 다운로드 완료
                if (downloadData.request.result != UnityWebRequest.Result.Success)
                {
                    string message = string.Format("[ASSET_DOWNLOAD] {0} Download not success. Result : {1}, Error : {2}", tableName, downloadData.request.result.ToString(), downloadData.request.error);
                    DebugEx.LogColor(message, "red");
                    continue;
                }

                // 다운로드한 파일 검사
                string csvData = downloadData.request.downloadHandler.text;
                if (string.IsNullOrEmpty(csvData))
                {
                    string message = string.Format("[ASSET_DOWNLOAD] {0} Download data not vaild.", tableName);
                    DebugEx.LogColor(message, "red");
                    continue;
                }

                // 다운로드한 파일 저장
                string directoryName = Path.GetDirectoryName(localPath);
                if (Directory.Exists(directoryName) == false)
                    Directory.CreateDirectory(directoryName);

                File.WriteAllText(localPath, csvData, System.Text.Encoding.UTF8);

                // 에셋 갱신하기
                downloadData.updateDataCallback(csvData);
            }

            // 다운로드 요청 객체 정리
            for (int i = 0; i < downloadList.Count; i++)
            {
                DownloadData downloadData = downloadList[i];

                if (downloadData.request != null)
                    downloadData.request.Dispose();
            }

            Manager.DownloadDone = true;
            Manager.CurrentAsset = string.Empty;
            Manager.currentRequest = null;

            yield break;
        }

        public static float GetRequestProgress()
        {
            if (Manager.currentRequest != null)
                return Manager.currentRequest.downloadProgress;

            return 1f;
        }
    }
}
