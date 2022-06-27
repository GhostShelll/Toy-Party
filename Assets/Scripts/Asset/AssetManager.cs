using com.jbg.core.manager;

namespace com.jbg.asset
{
    using Manager = AssetManager;

    public class AssetManager
    {
        public static bool IsOpened { get; private set; }

        private const string CLASSNAME = "AssetManager";

        public static void Open()
        {
            Manager.Close();
            Manager.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            // 각종 에셋 매니저 오픈
        }

        public static void Close()
        {
            if (Manager.IsOpened)
            {
                Manager.IsOpened = false;

                SystemManager.RemoveOpenList(CLASSNAME);

                // 각종 에셋 매니저 클로즈
            }
        }
    }
}
