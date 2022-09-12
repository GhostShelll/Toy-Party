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
            //p.checkAssetTxt = "**���̺� üũ��";
            //p.downloadAssetTxt = "**{0} ���̺� �ٿ�ε���";
            //p.refreshBtnTxt = "**���̺� ����";

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

        private void SetStateWaitDone()
        {
            this.SetState((int)STATE.WaitDone);
        }
    }
}
