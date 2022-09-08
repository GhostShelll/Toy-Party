using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using com.jbg.core;
using com.jbg.core.scene;
using com.jbg.asset.control;

namespace com.jbg.content.scene.view
{
    public class MainView : SceneView
    {
        public enum Event
        {
            RefreshAsset,
        };

        public class Params
        {
            public string checkAssetTxt;
            public string downloadAssetTxt;
            public string refreshBtnTxt;
        }

        private Params paramBuffer = null;
        public Params ParamBuffer { get { return this.paramBuffer; } }

        [Header("Main View")]
        [SerializeField]
        GameObject progressObj;
        [SerializeField]
        Image progress;
        [SerializeField]
        Text progressTxt;

        [SerializeField]
        ButtonComponent refreshBtn;

        private int dotCount = 1;

        public void OnOpen(Params p)
        {
            this.paramBuffer = p;

            this.progress.fillAmount = 0f;
            this.progressTxt.text = string.Empty;

            this.refreshBtn.Text = p.refreshBtnTxt;
        }

        public void SetStateCheckAsset()
        {
            this.progressObj.SetActive(true);
            this.refreshBtn.Interactable = false;
        }

        public void SetStateWaitDone()
        {
            this.progressObj.SetActive(false);
            this.refreshBtn.Interactable = true;
        }

        public void UpdateCheckAsset(float progress)
        {
            Params p = this.paramBuffer;

            this.progress.fillAmount = progress;

            string checkAssetTxt = p.checkAssetTxt;
            for (int i = 0; i < this.dotCount; i++)
                checkAssetTxt += '.';

            this.dotCount++;
            if (this.dotCount > 3)
                this.dotCount = 1;

            this.progressTxt.text = checkAssetTxt;
        }

        public void UpdateDownloadAsset(string assetName, float progress)
        {
            Params p = this.paramBuffer;

            this.progress.fillAmount = progress;

            string downloadAssetTxt = string.Format(p.downloadAssetTxt, assetName);
            for (int i = 0; i < this.dotCount; i++)
                downloadAssetTxt += '.';

            this.dotCount++;
            if (this.dotCount > 3)
                this.dotCount = 1;

            this.progressTxt.text = downloadAssetTxt;
        }

        public void OnClickRefreshAsset()
        {
            this.DoEvent(Event.RefreshAsset);
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform.Find("Canvas/Padding");
            Transform t;

            t = cached.Find("ProgressSlider");
            this.progressObj = t.gameObject;
            this.progress = t.FindComponent<Image>("Fill");
            this.progressTxt = t.FindComponent<Text>("Text");

            t = cached.Find("BtnRefresh");
            this.refreshBtn = new ButtonComponent(t);
        }
#endif  // UNITY_EDITOR
    }
}
