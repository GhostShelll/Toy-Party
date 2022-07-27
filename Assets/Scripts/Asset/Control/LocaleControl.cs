using System.Collections.Generic;
using System.IO;

using UnityEngine;
using Newtonsoft.Json;

using com.jbg.asset.data;
using com.jbg.core;
using com.jbg.core.manager;

namespace com.jbg.asset.control
{
    using Control = LocaleControl;

    public class LocaleControl
    {
        public enum Language
        {
            Invaild = -1,
            koKR = 0,
            enUS = 1,
        }

        public static bool IsOpened { get; private set; }

        public static Language LanguageCode
        {
            set
            {
                PlayerPrefs.SetInt("language", (int)value);
            }
            get
            {
                int savedValue = PlayerPrefs.GetInt("language", -1);
                bool enableParse = System.Enum.TryParse<Language>(savedValue.ToString(), out Language result);

                if (enableParse)
                    return result;
                else
                    return Language.Invaild;
            }
        }

        private static Dictionary<int, LocaleData> builtInLocale = new();
        private static Dictionary<int, LocaleData> assetData = new();

        private const string CLASSNAME = "LocaleControl";
        public const string TABLENAME = "LocaleData";

        public static void Open()
        {
            Control.Close();
            Control.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            if (Control.builtInLocale == null)
                Control.builtInLocale = new();
            Control.builtInLocale.Clear();
            if (Control.assetData == null)
                Control.assetData = new();
            Control.assetData.Clear();

            string jsonBuiltInLocale = Resources.Load<TextAsset>("BuiltInAsset/LocaleData").text.CsvToJson(new CSVParser.Info[]
            {
                new CSVParser.Info("code", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("koKR", CSVParser.Info.TYPE.String),
                new CSVParser.Info("enUS", CSVParser.Info.TYPE.String),
            }, true, 1);

            // 빌트인 로케일 정보 저장
            List<LocaleData> builtInLocaleList = JsonConvert.DeserializeObject<List<LocaleData>>(jsonBuiltInLocale);
            for (int i = 0; i < builtInLocaleList.Count; i++)
            {
                if (Control.builtInLocale.ContainsKey(builtInLocaleList[i].code) == false)
                {
                    Control.builtInLocale.Add(builtInLocaleList[i].code, new LocaleData()
                    {
                        code = builtInLocaleList[i].code,
                        koKR = builtInLocaleList[i].koKR,
                        enUS = builtInLocaleList[i].enUS,
                    });
                }
            }

            string localPath = AssetManager.PATH_ASSET_LOCALE_DATA;
            if (File.Exists(localPath))
            {
                string csvData = File.ReadAllText(localPath, System.Text.Encoding.UTF8);
                Control.UpdateData(csvData);
            }

            if (Control.LanguageCode == Language.Invaild)
            {
                Control.LanguageCode = Language.enUS;

                DebugEx.Log("AUTO SET LANGUAGE:" + Control.LanguageCode);
            }
        }

        public static void Close()
        {
            if (Control.IsOpened)
            {
                Control.IsOpened = false;

                if (Control.builtInLocale != null)
                    Control.builtInLocale.Clear();
                Control.builtInLocale = null;

                if (Control.assetData != null)
                    Control.assetData.Clear();
                Control.assetData = null;

                SystemManager.RemoveOpenList(CLASSNAME);
            }
        }

        public static void UpdateData(string csvData)
        {
            // 에셋 정보 갱신
            Control.assetData.Clear();

            string jsonData = csvData.CsvToJson(new CSVParser.Info[]
            {
                new CSVParser.Info("code", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("koKR", CSVParser.Info.TYPE.String),
                new CSVParser.Info("enUS", CSVParser.Info.TYPE.String),
            }, true, 1);

            List<LocaleData> dataList = JsonConvert.DeserializeObject<List<LocaleData>>(jsonData);
            for (int i = 0; i < dataList.Count; i++)
            {
                int curDataCode = dataList[i].code;
                if (Control.assetData.ContainsKey(curDataCode) == false)
                {
                    Control.assetData.Add(curDataCode, new LocaleData()
                    {
                        code = curDataCode,
                        koKR = dataList[i].koKR,
                        enUS = dataList[i].enUS,
                    });
                }
            }
        }

        public static string GetString(int code)
        {
            if (Control.assetData.ContainsKey(code))
            {
                LocaleData localeData = Control.assetData[code];
                if (localeData != null)
                {
                    switch (Control.LanguageCode)
                    {
                        case Language.koKR:
                            return localeData.koKR;
                        case Language.enUS:
                            return localeData.enUS;
                    }
                }
            }

            return Control.GetString_BuiltIn(code);
        }

        private static string GetString_BuiltIn(int code)
        {
            if (Control.builtInLocale == null || Control.builtInLocale.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                Control.builtInLocale.TryGetValue(code, out LocaleData localeData);

                if (localeData != null)
                {
                    switch (Control.LanguageCode)
                    {
                        case Language.koKR:
                            return localeData.koKR;
                        case Language.enUS:
                            return localeData.enUS;
                        case Language.Invaild:
                        default:
                            return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
