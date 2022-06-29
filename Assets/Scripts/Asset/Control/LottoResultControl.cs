//#define CHECK_LOTTO_NUMBERS

using System.Collections.Generic;

using UnityEngine;
using Newtonsoft.Json;

using com.jbg.asset.data;
using com.jbg.core;
using com.jbg.core.manager;

namespace com.jbg.asset.control
{
    using Control = LottoResultControl;
    using DataStorage = Storage<int, LottoResultData>;

    public class LottoResultControl
    {
        public static bool IsOpened { get; private set; }

        private static readonly Dictionary<int, LottoResultData> builtInData = new();

        private const string CLASSNAME = "LottoResultControl";

        public static void Open()
        {
            Control.Close();
            Control.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            string csvToJSON = Resources.Load<TextAsset>("BuiltInAsset/LottoResultData").text.CsvToJson(new CSVParser.Info[]
            {
                new CSVParser.Info("code", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num1", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num2", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num3", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num4", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num5", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num6", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("bonus", CSVParser.Info.TYPE.Plain),
            }, true, 1);

            AssetManager.OpenStorage<int, LottoResultData>(csvToJSON, (v) => { return v.code; });

            // 빌트인 정보 저장
            List<LottoResultData> builtInDataList = JsonConvert.DeserializeObject<List<LottoResultData>>(csvToJSON);

            Control.builtInData.Clear();
            for (int i = 0; i < builtInDataList.Count; i++)
            {
                if (Control.builtInData.ContainsKey(builtInDataList[i].code) == false)
                {
                    Control.builtInData.Add(builtInDataList[i].code, new LottoResultData()
                    {
                        code = builtInDataList[i].code,
                        num1 = builtInDataList[i].num1,
                        num2 = builtInDataList[i].num2,
                        num3 = builtInDataList[i].num3,
                        num4 = builtInDataList[i].num4,
                        num5 = builtInDataList[i].num5,
                        num6 = builtInDataList[i].num6,
                        bonus = builtInDataList[i].bonus,
                    });
                }
            }

#if CHECK_LOTTO_NUMBERS
            string originDataJSON = Resources.Load<TextAsset>("BuiltInAsset/LottoOriginResultData").text.CsvToJson(new CSVParser.Info[]
            {
                new CSVParser.Info("code", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num1", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num2", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num3", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num4", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num5", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("num6", CSVParser.Info.TYPE.Plain),
                new CSVParser.Info("bonus", CSVParser.Info.TYPE.Plain),
            }, true, 1);

            List<LottoResultData> originDataList = JsonConvert.DeserializeObject<List<LottoResultData>>(originDataJSON);
            Dictionary<int, LottoResultData> originDataDic = new();

            for (int i = 0; i < originDataList.Count; i++)
            {
                if (originDataDic.ContainsKey(originDataList[i].code) == false)
                {
                    originDataDic.Add(originDataList[i].code, new LottoResultData()
                    {
                        code = originDataList[i].code,
                        num1 = originDataList[i].num1,
                        num2 = originDataList[i].num2,
                        num3 = originDataList[i].num3,
                        num4 = originDataList[i].num4,
                        num5 = originDataList[i].num5,
                        num6 = originDataList[i].num6,
                        bonus = originDataList[i].bonus,
                    });
                }
            }

            Dictionary<int, LottoResultData>.Enumerator enumerator = originDataDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int code = enumerator.Current.Key;
                if (Control.builtInData.ContainsKey(code) == false)
                {
                    DebugEx.LogColor(string.Format("[LOTTO CHECK] Code : {0}에 대한 빌트인 정보가 없습니다.", code), "red");
                    return;
                }

                LottoResultData originData = enumerator.Current.Value;
                LottoResultData checkData = Control.builtInData[code];

                int[] numArr = new int[] { originData.num1, originData.num2, originData.num3, originData.num4, originData.num5, originData.num6 };
                for (int i = 0; i < numArr.Length; i++)
                {
                    int num = numArr[i];
                    bool numCheck = checkData.num1 == num || checkData.num2 == num || checkData.num3 == num || checkData.num4 == num || checkData.num5 == num || checkData.num6 == num;
                    if (numCheck == false)
                    {
                        DebugEx.LogColor(string.Format("[LOTTO CHECK] Code {0}에서 {1}가 없습니다.", code, num), "red");
                        return;
                    }
                }

                if (originData.bonus != checkData.bonus)
                {
                    DebugEx.LogColor(string.Format("[LOTTO CHECK] Code {0}의 보너스 번호가 {1}가 아닙니다.", code, originData.bonus), "red");
                    return;
                }
            }
#endif  // CHECK_LOTTO_NUMBERS
        }

        public static void Close()
        {
            if (Control.IsOpened)
            {
                Control.IsOpened = false;

                SystemManager.RemoveOpenList(CLASSNAME);

                AssetManager.CloseStorage<int, LottoResultData>();
            }
        }
    }
}
