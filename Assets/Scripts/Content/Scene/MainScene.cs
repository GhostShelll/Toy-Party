using com.jbg.content.block;
using com.jbg.content.popup;
using com.jbg.core.scene;

namespace com.jbg.content.scene
{
    public class MainScene : SceneEx
    {
        public enum STATE
        {
            Initialize,
            ProcessDone,
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            this.SetStateInitialize();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        protected override void OnBack()
        {
            base.OnBack();

            string title = "**����";
            string message = "**�����Ͻðڽ��ϱ�?";
            PopupAssist.OpenNoticeTwoBtnPopup(title, message, (popup) =>
            {
                if (popup.IsOK)
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#else
                    Application.Quit();
#endif  // UNITY_EDITOR
            });
        }

        private void SetStateInitialize()
        {
            this.SetState((int)STATE.Initialize);

            BlockManager.Instance.Initialize();

            this.SetStateProcessDone();
        }

        private void SetStateProcessDone()
        {
            this.SetState((int)STATE.ProcessDone);
        }
    }
}
