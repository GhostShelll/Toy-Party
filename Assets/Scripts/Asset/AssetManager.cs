using System.Collections;
using System.Collections.Generic;

using com.jbg.asset.control;
using com.jbg.core.manager;

namespace com.jbg.asset
{
    using Manager = AssetManager;

    public class AssetManager
    {
        public static bool IsOpened { get; private set; }

        public static bool LoadingDone { get; private set; }
        public static string CurrentAsset { get; private set; }
        public static float CurrentProgress { get; private set; }

        private const string CLASSNAME = "AssetManager";
        public const string ASSOCIATED_SHEET = "1tJmnVzNu45UkLEqM_6hAP-3sPcxtFnBBknsPM-7wpJA";

        public static Dictionary<string, IEnumerator> loadList = new();

        public static void Open()
        {
            Manager.Close();
            Manager.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            Manager.LoadingDone = false;
            Manager.CurrentAsset = string.Empty;
            Manager.CurrentProgress = 0f;

            // 로딩할 것들 목록 만들기
            Manager.loadList.Add(LocaleControl.ASSOCIATED_SHEET_NAME, LocaleControl.LoadAsync());
            Manager.loadList.Add(LottoResultControl.ASSOCIATED_SHEET_NAME, LottoResultControl.LoadAsync());

            // 각종 에셋 오픈
            LocaleControl.Open();
            LottoResultControl.Open();
        }

        public static IEnumerator LoadAsync()
        {
            Manager.LoadingDone = false;

            // 로딩 과정 시작
            float currentCount = 0f;
            float totalCount = Manager.loadList.Count;

            Dictionary<string, IEnumerator>.Enumerator enumerator = Manager.loadList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string assetName = enumerator.Current.Key;
                IEnumerator task = enumerator.Current.Value;

                Manager.CurrentAsset = assetName;
                Manager.CurrentProgress = currentCount / totalCount;

                yield return task;

                currentCount++;
            }

            Manager.CurrentAsset = string.Empty;
            Manager.CurrentProgress = currentCount / totalCount;

            Manager.LoadingDone = true;

            yield break;
        }

        public static void Close()
        {
            if (Manager.IsOpened)
            {
                Manager.IsOpened = false;

                SystemManager.RemoveOpenList(CLASSNAME);

                // 각종 에셋 클로즈
                LocaleControl.Close();
                LottoResultControl.Close();
            }
        }
    }
}
