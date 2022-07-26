#if CODE_EDIT_JBG
#else
using UnityEngine;
using System.Collections;
#endif  // CODE_EDIT_JBG
using System;

namespace GoogleSheetsToUnity
{
#if CODE_EDIT_JBG
    public class GoogleSheetsToUnityConfig
    {

    }
#else   // CODE_EDIT_JBG
    public class GoogleSheetsToUnityConfig : ScriptableObject
    {
        public string CLIENT_ID = "";
        public string CLIENT_SECRET = "";
        public string ACCESS_TOKEN = "";

        [HideInInspector]
        public string REFRESH_TOKEN;

        public string API_Key = "";

        public int PORT;

        public GoogleDataResponse gdr;
    }
#endif  // CODE_EDIT_JBG

    [System.Serializable]
    public class GoogleDataResponse
    {
        public string access_token = "";
        public string refresh_token = "";
        public string token_type = "";
        public int expires_in = 0; //just a place holder to work the the json and caculate the next refresh time
        public DateTime nextRefreshTime;
    }
}