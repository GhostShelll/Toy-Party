using System.Collections;
using System.Text;
using GoogleSheetsToUnity;
using GoogleSheetsToUnity.ThirdPary;
using TinyJSON;
using UnityEngine;
#if CODE_EDIT_JBG
using UnityEngine.Networking;
using com.jbg.core;
#endif

public delegate void OnSpreedSheetLoaded(GstuSpreadSheet sheet);
namespace GoogleSheetsToUnity
{
    /// <summary>
    /// Partial class for the spreadsheet manager to handle all Public functions
    /// </summary>
    public partial class SpreadsheetManager
    {
#if CODE_EDIT_JBG
        public static string GDRData
        {
            get
            {
                return PlayerPrefs.GetString("GDRData", string.Empty);
            }
            set
            {
                PlayerPrefs.SetString("GDRData", value);

                SpreadsheetManager._gdr = JsonUtility.FromJson<GoogleDataResponse>(value);
                SpreadsheetManager._gdr.nextRefreshTime = System.DateTime.Now.AddSeconds(SpreadsheetManager._gdr.expires_in);
            }
        }

        private static GoogleDataResponse _gdr = null;
        public static GoogleDataResponse GDR
        {
            get
            {
                if (SpreadsheetManager._gdr == null)
                {
                    SpreadsheetManager._gdr = new GoogleDataResponse()
                    {
                        access_token = string.Empty,
                        refresh_token = string.Empty,
                        token_type = string.Empty,
                        expires_in = 0,
                        nextRefreshTime = System.DateTime.MinValue,
                    };

                    string jsonData = SpreadsheetManager.GDRData;
                    if (string.IsNullOrEmpty(jsonData) == false)
                    {
                        SpreadsheetManager._gdr = JsonUtility.FromJson<GoogleDataResponse>(jsonData);
                        SpreadsheetManager._gdr.nextRefreshTime = System.DateTime.Now.AddSeconds(SpreadsheetManager._gdr.expires_in);
                    }
                }

                return SpreadsheetManager._gdr;
            }
        }
#else   // CODE_EDIT_JBG
        static GoogleSheetsToUnityConfig _config;
        /// <summary>
        /// Reference to the config for access to the auth details
        /// </summary>
        public static GoogleSheetsToUnityConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = (GoogleSheetsToUnityConfig)Resources.Load("GSTU_Config");
                }

                return _config;
            }
            set { _config = value; }
        }
#endif  // CODE_EDIT_JBG

        /// <summary>
        /// Read a public accessable spreadsheet
        /// </summary>
        /// <param name="searchDetails"></param>
        /// <param name="callback">event that will fire after reading is complete</param>
        public static void ReadPublicSpreadsheet(GSTU_Search searchDetails, OnSpreedSheetLoaded callback)
        {
#if CODE_EDIT_JBG
            string apiKey = GSTU_Config.GetAPIKey();
            if (string.IsNullOrEmpty(apiKey))
            {
                DebugEx.LogColor("Missing API Key, Check 'GSTU_Config.dll'", "red");
                return;
            }
#else   // CODE_EDIT_JBG
            if (string.IsNullOrEmpty(Config.API_Key))
            {
                Debug.Log("Missing API Key, please enter this in the confie settings");
                return;
            }
#endif  // CODE_EDIT_JBG

            StringBuilder sb = new StringBuilder();
            sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
            sb.Append("/" + searchDetails.sheetId);
            sb.Append("/values");
            sb.Append("/" + searchDetails.worksheetName + "!" + searchDetails.startCell + ":" + searchDetails.endCell);
#if CODE_EDIT_JBG
            sb.Append("?key=" + apiKey);
#else
            sb.Append("?key=" + Config.API_Key);
#endif  // CODE_EDIT_JBG

            if (Application.isPlaying)
            {
#if CODE_EDIT_JBG
                new Task(Read(new UnityWebRequest(sb.ToString()), searchDetails.titleColumn, searchDetails.titleRow, callback));
#else
                new Task(Read(new WWW(sb.ToString()), searchDetails.titleColumn, searchDetails.titleRow, callback));
#endif  // CODE_EDIT_JBG
            }
#if UNITY_EDITOR
            else
            {
#if CODE_EDIT_JBG
                EditorCoroutineRunner.StartCoroutine(Read(new UnityWebRequest(sb.ToString()), searchDetails.titleColumn, searchDetails.titleRow, callback));
#else
                EditorCoroutineRunner.StartCoroutine(Read(new WWW(sb.ToString()), searchDetails.titleColumn, searchDetails.titleRow, callback));
#endif  // CODE_EDIT_JBG
            }
#endif
        }

        /// <summary>
        /// Wait for the Web request to complete and then process the results
        /// </summary>
        /// <param name="www"></param>
        /// <param name="titleColumn"></param>
        /// <param name="titleRow"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
#if CODE_EDIT_JBG
        static IEnumerator Read(UnityWebRequest www, string titleColumn, int titleRow, OnSpreedSheetLoaded callback)
#else
        static IEnumerator Read(WWW www, string titleColumn, int titleRow, OnSpreedSheetLoaded callback)
#endif  // CODE_EDIT_JBG
        {
            yield return www;

#if CODE_EDIT_JBG
            ValueRange rawData = JSON.Load(www.downloadHandler.text).Make<ValueRange>();
#else
            ValueRange rawData = JSON.Load(www.text).Make<ValueRange>();
#endif  // CODE_EDIT_JBG
            GSTU_SpreadsheetResponce responce = new GSTU_SpreadsheetResponce(rawData);

            GstuSpreadSheet spreadSheet = new GstuSpreadSheet(responce, titleColumn, titleRow);

            if (callback != null)
            {
                callback(spreadSheet);
            }
        }
    }
}