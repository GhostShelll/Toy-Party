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
            p.initializeTxt = "**초기화 중";
            p.checkMatchTxt = "**매칭 검사 중";
            p.destroyMatchedTxt = "**매칭된 블럭 삭제 중";
            p.processBlockMoveTxt = "**블럭 이동 중";
            p.processDoneTxt = "**입력 대기 중";

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
