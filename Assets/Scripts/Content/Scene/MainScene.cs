using com.jbg.content.popup;
using com.jbg.core.scene;

namespace com.jbg.content.scene
{
    public class MainScene : SceneEx
    {
        // TODO[jbg] : 필요한 것만 사용해야함

        protected override void OnOpen()
        {
            base.OnOpen();

            SystemPopupAssist.OpenNoticeOneBtnPopup("JBG TITLE", "JBG MESSAGE\nJBG MESSAGE\nJBG MESSAGE\n", (popup) =>
            {
                SystemPopupAssist.OpenNoticeOneBtnPopup("JBG TITLE22222", "JBG MESSAGE\nJBG MESSAGE\nJBG MESSAGE\n", null);
            });
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnBack()
        {
            base.OnBack();
        }

        protected override void OnAppSuspend()
        {
            base.OnAppSuspend();
        }

        protected override void OnAppResume()
        {
            base.OnAppResume();
        }

        protected override void Refresh()
        {
            base.Refresh();
        }
    }
}
