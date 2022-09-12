using UnityEngine;

using com.jbg.content.popup;
using com.jbg.content.scene.view;
using com.jbg.core;
using com.jbg.core.manager;
using com.jbg.core.scene;

namespace com.jbg.content.scene
{
    public class MainScene : SceneEx
    {
        private MainView sceneView;

        public enum STATE
        {
            WaitDone,
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            this.sceneView = (MainView)this.SceneView;

            //this.sceneView.BindEvent(MainView.Event.RefreshAsset, this.OnClickRefreshAsset);

            MainView.Params p = new();
            //p.checkAssetTxt = "**테이블 체크중";
            //p.downloadAssetTxt = "**{0} 테이블 다운로드중";
            //p.refreshBtnTxt = "**테이블 갱신";

            this.sceneView.OnOpen(p);

            this.SetStateWaitDone();
        }

        protected override void OnClose()
        {
            base.OnClose();

            //this.sceneView.RemoveEvent(MainView.Event.RefreshAsset);
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

        private void SetStateWaitDone()
        {
            this.SetState((int)STATE.WaitDone);
        }
    }
}
