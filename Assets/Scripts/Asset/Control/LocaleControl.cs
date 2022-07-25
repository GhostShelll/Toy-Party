using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Newtonsoft.Json;
using GoogleSheetsToUnity;

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
        public static bool LoadingDone { get; private set; }

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
        private static Dictionary<int, LocaleData> downloadedLocale = new();

        private const string CLASSNAME = "LocaleControl";
        public const string ASSOCIATED_SHEET_NAME = "LocaleData";
        private const string START_CELL = "A1";
        private const string END_CELL = "C100";

        public static void Open()
        {
            Control.Close();
            Control.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            Control.LoadingDone = false;

            Control.builtInLocale = new();
            Control.downloadedLocale = new();

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

            if (Control.LanguageCode == Language.Invaild)
            {
                Control.LanguageCode = Language.enUS;

                DebugEx.Log("AUTO SET LANGUAGE:" + Control.LanguageCode);
            }
        }

        public static IEnumerator LoadAsync()
        {
            Control.LoadingDone = false;

            // 로딩 과정 시작
            SpreadsheetManager.Read(new GSTU_Search(AssetManager.ASSOCIATED_SHEET, Control.ASSOCIATED_SHEET_NAME, Control.START_CELL, Control.END_CELL), (spreadSheet) =>
            {
                Dictionary<int, List<GSTU_Cell>>.Enumerator enumerator = spreadSheet.rows.primaryDictionary.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    int rowNum = enumerator.Current.Key;
                    if (rowNum == 1)        // 1번 행은 '열의 제목' 행이기 때문에 생략
                        continue;

                    List<GSTU_Cell> cellList = enumerator.Current.Value;        // code, koKR, enUS 순으로 정렬됨

                    bool codeIsCorrect = int.TryParse(cellList[0].value, out int code);
                    if (codeIsCorrect == false)
                    {
                        DebugEx.LogColor(string.Format("[LOCALE CHECK] {0}번째 행의 Code 값 int.Parse()를 실패했습니다.", rowNum), "red");
                        continue;
                    }

                    if (cellList.Count != 3)
                    {
                        DebugEx.LogColor(string.Format("[LOCALE CHECK] Code {0}의 열 갯수가 맞지 않습니다.", code), "red");
                        continue;
                    }

                    if (Control.downloadedLocale.ContainsKey(code) == false)
                    {
                        Control.downloadedLocale.Add(code, new LocaleData()
                        {
                            code = code,
                            koKR = cellList[1].value,
                            enUS = cellList[2].value,
                        });
                    }
                }

                Control.LoadingDone = true;
            });

            while (Control.LoadingDone == false)
                yield return null;

            yield break;
        }

        public static void Close()
        {
            if (Control.IsOpened)
            {
                Control.IsOpened = false;

                if (Control.builtInLocale != null)
                    Control.builtInLocale.Clear();
                Control.builtInLocale = null;

                if (Control.downloadedLocale != null)
                    Control.downloadedLocale.Clear();
                Control.downloadedLocale = null;

                SystemManager.RemoveOpenList(CLASSNAME);
            }
        }

        public static string GetString(int code)
        {
            if (Control.downloadedLocale.ContainsKey(code))
            {
                LocaleData localeData = Control.downloadedLocale[code];
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
