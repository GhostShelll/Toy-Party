using UnityEngine;

using com.jbg.content.block;
using com.jbg.content.popup;
using com.jbg.content.scene.view;
using com.jbg.core.scene;

namespace com.jbg.content.scene
{
    public class MainScene : SceneEx
    {
        private MainView sceneView;

        public enum STATE
        {
            Initialize,
            CheckMatch,
            DestroyMatched,
            ProcessBlockMove,
            ProcessDone,
        }

        private float waitTime;

        protected override void OnOpen()
        {
            base.OnOpen();

            this.sceneView = (MainView)this.SceneView;

            MainView.Params p = new();
            p.initializeTxt = "**�ʱ�ȭ ��";
            p.checkMatchTxt = "**��Ī �˻� ��";
            p.destroyMatchedTxt = "**��Ī�� �� ���� ��";
            p.processBlockMoveTxt = "**�� �̵� ��";
            p.processDoneTxt = "**�Է� ��� ��";

            this.sceneView.OnOpen(p);

            this.waitTime = 0f;

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

            this.sceneView.SetStateInitialize();

            BlockManager.Instance.Initialize();

            this.SetStateCheckMatch();
        }

        private void SetStateCheckMatch()
        {
            this.SetState((int)STATE.CheckMatch);

            this.sceneView.SetStateCheckMatch();

            BlockManager.Instance.CheckMatch();

            this.waitTime = 0f;
            this.AddUpdateFunc(() =>
            {
                this.waitTime += Time.deltaTime;
                if (this.waitTime >= 1f)
                    this.SetStateDestroyMatched();
            });
        }

        private void SetStateDestroyMatched()
        {
            this.SetState((int)STATE.DestroyMatched);

            this.sceneView.SetStateDestroyMatched();

            bool cellDestroyed = BlockManager.Instance.DestroyMatched();

            if (cellDestroyed)
            {
                this.waitTime = 0f;
                this.AddUpdateFunc(() =>
                {
                    this.waitTime += Time.deltaTime;
                    if (this.waitTime >= 1f)
                        this.SetStateProcessBlockMove();
                });
            }
            else
            {
                this.SetStateProcessDone();
            }
        }

        private void SetStateProcessBlockMove()
        {
            this.SetState((int)STATE.ProcessBlockMove);

            this.sceneView.SetStateProcessBlockMove();

            bool isMoved = BlockManager.Instance.ProcessBlockMove();

            if (isMoved)
            {
                this.waitTime = 0f;
                this.AddUpdateFunc(() =>
                {
                    this.waitTime += Time.deltaTime;
                    if (this.waitTime >= 1f)
                        this.SetStateProcessBlockMove();
                });
            }
            else
            {
                this.SetStateCheckMatch();
            }
        }

        private void SetStateProcessDone()
        {
            this.SetState((int)STATE.ProcessDone);

            this.sceneView.SetStateProcessDone();
        }
    }
}
