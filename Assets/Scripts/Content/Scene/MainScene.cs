using UnityEngine;

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
            CheckMatch,
            DestroyMatched,
            ProcessDone,
        }

        private float waitTime;

        protected override void OnOpen()
        {
            base.OnOpen();

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

            BlockManager.Instance.Initialize();

            this.SetStateCheckMatch();
        }

        private void SetStateCheckMatch()
        {
            this.SetState((int)STATE.CheckMatch);

            BlockManager.Instance.CheckMatch();

            this.waitTime = 0f;
            this.AddUpdateFunc(() =>
            {
                this.waitTime += Time.deltaTime;
                if (this.waitTime >= 0.5f)
                    this.SetStateDestroyMatched();
            });
        }

        private void SetStateDestroyMatched()
        {
            this.SetState((int)STATE.DestroyMatched);

            BlockManager.Instance.DestroyMatched();

            this.waitTime = 0f;
            this.AddUpdateFunc(() =>
            {
                this.waitTime += Time.deltaTime;
                if (this.waitTime >= 0.5f)
                    this.SetStateProcessDone();
            });
        }

        private void SetStateProcessDone()
        {
            this.SetState((int)STATE.ProcessDone);
        }
    }
}
