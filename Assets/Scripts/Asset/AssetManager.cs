using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

using com.jbg.asset.control;
using com.jbg.core;
using com.jbg.core.manager;

namespace com.jbg.asset
{
    using Manager = AssetManager;

    public class AssetManager
    {
        public static bool IsOpened { get; private set; }

        private const string CLASSNAME = "AssetManager";
        private const string DISPOSE_TAG = "Asset";

        public static void Open()
        {
            Manager.Close();
            Manager.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            // 각종 에셋 오픈
            LocaleControl.Open();
            LottoResultControl.Open();
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
