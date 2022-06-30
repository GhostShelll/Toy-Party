//#define CHECK_LOTTO_NUMBERS

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Newtonsoft.Json;

using com.jbg.asset.data;
using com.jbg.core;
using com.jbg.core.manager;

namespace com.jbg.asset.control
{
    using Control = LottoResultControl;

    public class LottoResultControl
    {
        public static bool IsOpened { get; private set; }

        private static readonly Dictionary<int, LottoResultData> builtInData = new();
        private static readonly Dictionary<int, List<int>> lottoNumberMap = new();       // 추첨 순서 별 로또번호 등장 횟수

        private const string CLASSNAME = "LottoResultControl";
        private const int MAX_NUMBER = 45;

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

            Dictionary<int, LottoResultData>.Enumerator enumerator1 = originDataDic.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                int code = enumerator1.Current.Key;
                if (Control.builtInData.ContainsKey(code) == false)
                {
                    DebugEx.LogColor(string.Format("[LOTTO CHECK] Code : {0}에 대한 빌트인 정보가 없습니다.", code), "red");
                    return;
                }

                LottoResultData originData = enumerator1.Current.Value;
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

            // 추첨 순서 별 번호 나온 횟수를 0으로 초기화
            Control.lottoNumberMap.Clear();
            Control.lottoNumberMap.Add(1, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(2, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(3, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(4, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(5, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(6, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(7, Enumerable.Repeat(0, 45).ToList());

            Dictionary<int, LottoResultData>.Enumerator enumerator2 = Control.builtInData.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                int code = enumerator2.Current.Key;
                LottoResultData data = enumerator2.Current.Value;

                Control.lottoNumberMap[1][data.num1 - 1]++;
                Control.lottoNumberMap[2][data.num2 - 1]++;
                Control.lottoNumberMap[3][data.num3 - 1]++;
                Control.lottoNumberMap[4][data.num4 - 1]++;
                Control.lottoNumberMap[5][data.num5 - 1]++;
                Control.lottoNumberMap[6][data.num6 - 1]++;
                Control.lottoNumberMap[7][data.bonus - 1]++;
            }

            // TODO[jbg] : 아래 내용 지워야함
            System.Text.StringBuilder log = new();
            log.AppendLine();
            log.Append('\t').Append('1').Append('\t').Append('2').Append('\t').Append('3').Append('\t').Append('4').Append('\t').Append('5').Append('\t').Append('6').Append('\t').Append("Bonus");
            log.AppendLine();

            for (int i = 0; i < Control.MAX_NUMBER; i++)
            {
                log.Append(i + 1);
                log.Append('\t').Append(Control.lottoNumberMap[1][i]).Append('\t').Append(Control.lottoNumberMap[2][i]).Append('\t').Append(Control.lottoNumberMap[3][i]);
                log.Append('\t').Append(Control.lottoNumberMap[4][i]).Append('\t').Append(Control.lottoNumberMap[5][i]).Append('\t').Append(Control.lottoNumberMap[6][i]);
                log.Append('\t').Append(Control.lottoNumberMap[7][i]);
                log.AppendLine();
            }

            DebugEx.LogColor(log.ToString(), "red");
        }

        public static int RecentPeriod { get { return Control.builtInData.Count; } }        // 가장 최근 진행한 회차

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
