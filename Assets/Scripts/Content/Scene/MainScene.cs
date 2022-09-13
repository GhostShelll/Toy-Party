using com.jbg.content.block;
using com.jbg.content.popup;
using com.jbg.core.scene;

namespace com.jbg.content.scene
{
    public class MainScene : SceneEx
    {
        public enum STATE
        {
            Initial,
            WaitDone,
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            this.SetStateInitial();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        protected override void OnBack()
        {
            base.OnBack();

            string title = "**종료";
            string message = "**종료하시겠습니까?";
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

        private void SetStateInitial()
        {
            this.SetState((int)STATE.Initial);

            BlockManager.Instance.Initialize();

            this.SetStateWaitDone();
        }

        private void SetStateWaitDone()
        {
            this.SetState((int)STATE.WaitDone);
        }
    }
}
