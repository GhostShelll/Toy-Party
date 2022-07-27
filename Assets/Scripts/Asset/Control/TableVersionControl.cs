using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine.Networking;
using Newtonsoft.Json;

using com.jbg.asset.data;
using com.jbg.content.popup;
using com.jbg.core.manager;

namespace com.jbg.asset.control
{
    using Control = TableVersionControl;

    public class TableVersionControl
    {
        public static bool IsOpened { get; private set; }
        public static bool CheckDone { get; private set; }

        private static Dictionary<string, TableVersionData> assetData = new();
        private static UnityWebRequest request = new();
        private static List<string> needDownloadAsset = new();

        private const string CLASS_NAME = "TableVersionControl";
        public const string TABLE_NAME = "TableVersionData";

        public static void Open()
        {
            Control.Close();
            Control.IsOpened = true;

            SystemManager.AddOpenList(CLASS_NAME);

            Control.CheckDone = false;

            if (Control.assetData == null)
                Control.assetData = new();
            Control.assetData.Clear();
            if (Control.needDownloadAsset == null)
                Control.needDownloadAsset = new();
            Control.needDownloadAsset.Clear();

            string localPath = AssetManager.PATH_ASSET_TABLE_VERSION_DATA;
            if (File.Exists(localPath))
            {
                string csvData = File.ReadAllText(localPath, System.Text.Encoding.UTF8);
                if (string.IsNullOrEmpty(csvData) == false)
                {
                    string jsonData = csvData.CsvToJson(new CSVParser.Info[]
                    {
                        new CSVParser.Info("name", CSVParser.Info.TYPE.String),
                        new CSVParser.Info("version", CSVParser.Info.TYPE.Plain),
                    }, true, 1);

                    List<TableVersionData> dataList = JsonConvert.DeserializeObject<List<TableVersionData>>(jsonData);
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        if (Control.assetData.ContainsKey(dataList[i].name) == false)
                        {
                            Control.assetData.Add(dataList[i].name, new TableVersionData()
                            {
                                name = dataList[i].name,
                                version = dataList[i].version,
                            });
                        }
                    }
                }
            }
        }

        public static void Close()
        {
            if (Control.IsOpened)
            {
                Control.IsOpened = false;

                if (Control.assetData != null)
                    Control.assetData.Clear();
                Control.assetData = null;

                if (Control.request != null)
                    Control.request.Dispose();

                if (Control.needDownloadAsset != null)
                    Control.needDownloadAsset.Clear();
                Control.needDownloadAsset = null;

                SystemManager.RemoveOpenList(CLASS_NAME);
            }
        }

        public static IEnumerator CheckAsset()
        {
            Control.CheckDone = false;

            // 다운로드 경로 가져오기
            string downloadPath = GoogleSheetConfig.GetDownloadPath(Control.TABLE_NAME);
            if (string.IsNullOrEmpty(downloadPath))
            {
                string message = string.Format("[TABLE_VERSION_CONTROL] {0} Download Path is null. Check DLL File.", Control.TABLE_NAME);
#if UNITY_EDITOR
                PopupAssist.OpenNoticeOneBtnPopup("DEBUG ERROR", message, (popup) =>
                {
                    GameRestart.StartScene();
                });
#else   // UNITY_EDITOR
                UnityEngine.Debug.LogError(message);
                UnityEngine.Application.Quit();
#endif  // UNITY_EDITOR
                yield break;
            }

            // 다운로드 시작
            Control.request = UnityWebRequest.Get(downloadPath);
            yield return Control.request.SendWebRequest();

            // 다운로드 완료
            if (request.result != UnityWebRequest.Result.Success)
            {
                string message = string.Format("[TABLE_VERSION_CONTROL] {0} Download not success. Result : {1}, Error : {2}", Control.TABLE_NAME, request.result.ToString(), request.error);
#if UNITY_EDITOR
                PopupAssist.OpenNoticeOneBtnPopup("DEBUG ERROR", message, (popup) =>
                {
                    GameRestart.StartScene();
                });
#else   // UNITY_EDITOR
                UnityEngine.Debug.LogError(message);
                UnityEngine.Application.Quit();
#endif  // UNITY_EDITOR
                yield break;
            }

            // 다운로드한 파일 검사
            string csvData = request.downloadHandler.text;
            if (string.IsNullOrEmpty(csvData))
            {
                string message = string.Format("[TABLE_VERSION_CONTROL] {0} Download data not vaild.", Control.TABLE_NAME);
#if UNITY_EDITOR
                PopupAssist.OpenNoticeOneBtnPopup("DEBUG ERROR", message, (popup) =>
                {
                    GameRestart.StartScene();
                });
#else   // UNITY_EDITOR
                UnityEngine.Debug.LogError(message);
                UnityEngine.Application.Quit();
#endif  // UNITY_EDITOR
                yield break;
            }

            // 다운로드한 파일 저장
            string localPath = AssetManager.PATH_ASSET_TABLE_VERSION_DATA;

            string directoryName = Path.GetDirectoryName(localPath);
            if (Directory.Exists(directoryName) == false)
                Directory.CreateDirectory(directoryName);

            File.WriteAllText(localPath, csvData, System.Text.Encoding.UTF8);

            // 이전 정보와 비교하기
            string jsonData = csvData.CsvToJson(new CSVParser.Info[]
            {
                new CSVParser.Info("name", CSVParser.Info.TYPE.String),
                new CSVParser.Info("version", CSVParser.Info.TYPE.Plain),
            }, true, 1);

            List<TableVersionData> dataList = JsonConvert.DeserializeObject<List<TableVersionData>>(jsonData);
            for (int i = 0; i < dataList.Count; i++)
            {
                string curDataName = dataList[i].name;
                int curDataVersion = dataList[i].version;

                if (Control.assetData.ContainsKey(curDataName) == false)
                {
                    Control.assetData.Add(curDataName, new TableVersionData()
                    {
                        name = curDataName,
                        version = curDataVersion,
                    });
                }
                else
                {
                    TableVersionData originData = Control.assetData[curDataName];
                    if (originData.version != curDataVersion)
                    {
                        originData.version = curDataVersion;
                        if (Control.needDownloadAsset.Contains(curDataName) == false)
                            Control.needDownloadAsset.Add(curDataName);
                    }
                }
            }

            Control.CheckDone = true;

            yield break;
        }

        public static float GetRequestProgress()
        {
            if (Control.request != null)
                return Control.request.downloadProgress;

            return 1f;
        }

        public static bool NeedDownload(string assetName)
        {
            return Control.needDownloadAsset.Contains(assetName);
        }
    }
}
