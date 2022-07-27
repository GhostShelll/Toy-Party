using System.Collections;
using System.Collections.Generic;
using System.Linq;

using com.jbg.core;
using com.jbg.core.manager;

namespace com.jbg.asset.control
{
    using Control = TableVersionControl;

    public class TableVersionControl
    {
        public static bool IsOpened { get; private set; }
        public static bool LoadingDone { get; private set; }

        private const string CLASSNAME = "TableVersionControl";
        public const string TABLENAME = "TableVersionData";

        public static void Open()
        {
            Control.Close();
            Control.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            Control.LoadingDone = false;
        }

        public static IEnumerator LoadAsync()
        {
            Control.LoadingDone = false;

            // TODO[jbg] : 로딩 과정 시작

            Control.LoadingDone = true;

            while (Control.LoadingDone == false)
                yield return null;

            yield break;
        }

        public static void Close()
        {
            if (Control.IsOpened)
            {
                Control.IsOpened = false;

                SystemManager.RemoveOpenList(CLASSNAME);
            }
        }
    }
}
