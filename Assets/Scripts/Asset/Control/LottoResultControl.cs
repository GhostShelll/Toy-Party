using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public static int RecentPeriod { get { return Control.assetData.Count; } }        // 가장 최근 진행한 회차

        private static Dictionary<int, LottoResultData> assetData = new();
        private static Dictionary<int, List<int>> lottoNumberMap = new();       // 추첨 순서 별 로또번호 등장 횟수

        private const string CLASS_NAME = "LottoResultControl";
        public const string TABLE_NAME = "LottoResultData";

        public static void Open()
        {
            Control.Close();
            Control.IsOpened = true;

            SystemManager.AddOpenList(CLASS_NAME);

            if (Control.assetData == null)
                Control.assetData = new();
            Control.assetData.Clear();
            if (Control.lottoNumberMap == null)
                Control.lottoNumberMap = new();
            Control.lottoNumberMap.Clear();

            string localPath = AssetManager.PATH_ASSET_LOTTO_RESULT_DATA;
            if (File.Exists(localPath))
            {
                string csvData = File.ReadAllText(localPath, System.Text.Encoding.UTF8);
                Control.UpdateData(csvData);
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

                if (Control.lottoNumberMap != null)
                    Control.lottoNumberMap.Clear();
                Control.lottoNumberMap = null;

                SystemManager.RemoveOpenList(CLASS_NAME);
            }
        }

        public static void UpdateData(string csvData)
        {
            // 에셋 정보 갱신
            Control.assetData.Clear();

            string jsonData = csvData.CsvToJson(new CSVParser.Info[]
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

            List<LottoResultData> dataList = JsonConvert.DeserializeObject<List<LottoResultData>>(jsonData);
            for (int i = 0; i < dataList.Count; i++)
            {
                int curDataCode = dataList[i].code;
                if (Control.assetData.ContainsKey(curDataCode) == false)
                {
                    Control.assetData.Add(curDataCode, new LottoResultData()
                    {
                        code = curDataCode,
                        num1 = dataList[i].num1,
                        num2 = dataList[i].num2,
                        num3 = dataList[i].num3,
                        num4 = dataList[i].num4,
                        num5 = dataList[i].num5,
                        num6 = dataList[i].num6,
                        bonus = dataList[i].bonus,
                    });
                }
            }

            // 추첨 순서 별 번호 나온 횟수를 0으로 초기화
            Control.lottoNumberMap.Clear();
            Control.lottoNumberMap.Add(1, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(2, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(3, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(4, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(5, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(6, Enumerable.Repeat(0, 45).ToList());
            Control.lottoNumberMap.Add(7, Enumerable.Repeat(0, 45).ToList());

            Dictionary<int, LottoResultData>.Enumerator enumerator2 = Control.assetData.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                LottoResultData data = enumerator2.Current.Value;

                Control.lottoNumberMap[1][data.num1 - 1]++;
                Control.lottoNumberMap[2][data.num2 - 1]++;
                Control.lottoNumberMap[3][data.num3 - 1]++;
                Control.lottoNumberMap[4][data.num4 - 1]++;
                Control.lottoNumberMap[5][data.num5 - 1]++;
                Control.lottoNumberMap[6][data.num6 - 1]++;
                Control.lottoNumberMap[7][data.bonus - 1]++;
            }
        }

        public static List<int> GetLottoNumbers(int choiceNum)
        {
            if (Control.lottoNumberMap.ContainsKey(choiceNum) == false)
            {
                DebugEx.LogColor(string.Format("로또 추첨 순서 중에서 {0}번째 순서는 없습니다.", choiceNum), "red");
                return null;
            }

            return Control.lottoNumberMap[choiceNum].ToList();
        }
    }
}
