using com.jbg.asset.control;
using com.jbg.asset.data;
using com.jbg.content.popup;
using com.jbg.content.scene.view;
using com.jbg.core.manager;
using com.jbg.core.scene;

namespace com.jbg.content.scene
{
    public class MainScene : SceneEx
    {
        private MainView sceneView;

        public enum STATE
        {
            Wait,
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            this.sceneView = (MainView)this.SceneView;

            this.sceneView.BindEvent(MainView.Event.LottoSelect, this.OnClickLottoSelect);

            MainView.Params p = new();
            p.lottoBtnTxt = LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_TITLE_TEXT);

            this.sceneView.OnOpen(p);

            this.SetStateWait();
        }

        protected override void OnClose()
        {
            base.OnClose();

            this.sceneView.RemoveEvent(MainView.Event.LottoSelect);
        }

        private void SetStateWait()
        {
            this.SetState((int)STATE.Wait);

            this.OnClickLottoSelect(0, 0);
        }

        private void OnClickLottoSelect(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            LottoPopupAssist.Open(() =>
            {

            });
        }
    }
}
