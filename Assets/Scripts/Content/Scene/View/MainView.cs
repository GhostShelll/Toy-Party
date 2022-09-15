using UnityEngine;
using UnityEngine.UI;

using com.jbg.core;
using com.jbg.core.scene;

namespace com.jbg.content.scene.view
{
    public class MainView : SceneView
    {
        public class Params
        {
            public string initializeTxt;
            public string checkMatchTxt;
            public string destroyMatchedTxt;
            public string processBlockMoveTxt;
            public string processBlockSwapTxt;
            public string checkMatchPossibleTxt;
            public string processBlockAllSwapTxt;
            public string processDoneTxt;
        }

        private Params paramBuffer = null;
        public Params ParamBuffer { get { return this.paramBuffer; } }

        [Header("Main View")]
        [SerializeField]
        Text stateTxt;

        public void OnOpen(Params p)
        {
            this.paramBuffer = p;
        }

        public void SetStateInitialize()
        {
            Params p = this.paramBuffer;

            this.stateTxt.text = p.initializeTxt;
        }

        public void SetStateCheckMatch()
        {
            Params p = this.paramBuffer;

            this.stateTxt.text = p.checkMatchTxt;
        }

        public void SetStateDestroyMatched()
        {
            Params p = this.paramBuffer;

            this.stateTxt.text = p.destroyMatchedTxt;
        }

        public void SetStateProcessBlockMove()
        {
            Params p = this.paramBuffer;

            this.stateTxt.text = p.processBlockMoveTxt;
        }

        public void SetStateProcessBlockSwap()
        {
            Params p = this.paramBuffer;

            this.stateTxt.text = p.processBlockSwapTxt;
        }
        
        public void SetStateCheckMatchPossible()
        {
            Params p = this.paramBuffer;

            this.stateTxt.text = p.checkMatchPossibleTxt;
        }

        public void SetStateProcessBlockAllSwap()
        {
            Params p = this.paramBuffer;

            this.stateTxt.text = p.processBlockAllSwapTxt;
        }

        public void SetStateProcessDone()
        {
            Params p = this.paramBuffer;

            this.stateTxt.text = p.processDoneTxt;
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform.Find("Canvas/Padding");
            Transform t;

            t = cached.Find("Top");
            this.stateTxt = t.FindComponent<Text>("TextState");
        }
#endif  // UNITY_EDITOR
    }
}
