using System.Collections;
using System.Text;
using GoogleSheetsToUnity;
using GoogleSheetsToUnity.ThirdPary;
using TinyJSON;
using UnityEngine;
#if CODE_EDIT_JBG
using UnityEngine.Networking;
#endif

public delegate void OnSpreedSheetLoaded(GstuSpreadSheet sheet);
namespace GoogleSheetsToUnity
{
    /// <summary>
    /// Partial class for the spreadsheet manager to handle all Public functions
    /// </summary>
    public partial class SpreadsheetManager
    {
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

        /// <summary>
        /// Read a public accessable spreadsheet
        /// </summary>
        /// <param name="searchDetails"></param>
        /// <param name="callback">event that will fire after reading is complete</param>
        public static void ReadPublicSpreadsheet(GSTU_Search searchDetails, OnSpreedSheetLoaded callback)
        {
            if (string.IsNullOrEmpty(Config.API_Key))
            {
                Debug.Log("Missing API Key, please enter this in the confie settings");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
            sb.Append("/" + searchDetails.sheetId);
            sb.Append("/values");
            sb.Append("/" + searchDetails.worksheetName + "!" + searchDetails.startCell + ":" + searchDetails.endCell);
            sb.Append("?key=" + Config.API_Key);

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