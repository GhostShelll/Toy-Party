using UnityEngine;

using com.jbg.asset;
using com.jbg.asset.control;
using com.jbg.asset.data;
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
            CheckAsset,
            DownloadAsset,
            WaitDone,
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            this.sceneView = (MainView)this.SceneView;

            this.sceneView.BindEvent(MainView.Event.RefreshAsset, this.OnClickRefreshAsset);

            MainView.Params p = new();
            p.checkAssetTxt = "**���̺� üũ��";
            p.downloadAssetTxt = "**{0} ���̺� �ٿ�ε���";
            p.refreshBtnTxt = "**���̺� ����";

            this.sceneView.OnOpen(p);

            this.SetStateCheckAsset();
        }

        protected override void OnClose()
        {
            base.OnClose();

            this.sceneView.RemoveEvent(MainView.Event.RefreshAsset);
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

        private void SetStateCheckAsset()
        {
            this.SetState((int)STATE.CheckAsset);

            this.sceneView.SetStateCheckAsset();

            // ���� üũ ����
            Coroutine task = CoroutineManager.AddTask(TableVersionControl.CheckAsset());

            this.AddUpdateFunc(() =>
            {
                if (TableVersionControl.CheckDone)
                {
                    // ���� üũ �Ϸ���
                    if (task != null)
                        CoroutineManager.RemoveTask(task);

                    this.SetStateDownloadAsset();
                }
                else
                {
                    // ���� üũ ��
                    float currentProgress = TableVersionControl.GetRequestProgress();

                    this.sceneView.UpdateCheckAsset(currentProgress);
                }
            });
        }

        private void SetStateDownloadAsset()
        {
            this.SetState((int)STATE.DownloadAsset);

            // ���� �ε� ����
            Coroutine task = CoroutineManager.AddTask(AssetManager.DownloadAsset());

            this.AddUpdateFunc(() =>
            {
                if (AssetManager.DownloadDone)
                {
                    // ���� �ε� �Ϸ���
                    if (task != null)
                        CoroutineManager.RemoveTask(task);

                    this.SetStateWaitDone();
                }
                else
                {
                    // ���� �ε���
                    string currentAsset = AssetManager.CurrentAsset;
                    float currentProgress = AssetManager.GetRequestProgress();

                    this.sceneView.UpdateDownloadAsset(currentAsset, currentProgress);
                }
            });
        }

        private void SetStateWaitDone()
        {
            this.SetState((int)STATE.WaitDone);

            this.sceneView.SetStateWaitDone();
        }

        private void OnClickRefreshAsset(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            // ���� ���� ����
            this.SetStateCheckAsset();
        }
    }
}
