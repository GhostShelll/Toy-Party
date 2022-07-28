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
            LottoSelect,
            RefreshAsset,
            ChangeLanguage,
        };

        public class Params
        {
            public string lottoBtnTxt;
            public string checkAssetTxt;
            public string downloadAssetTxt;
            public string refreshBtnTxt;
            public List<string> languagesList;
        }

        private Params paramBuffer = null;

        [Header("Main View")]
        [SerializeField]
        ButtonComponent lottoBtn;

        [SerializeField]
        GameObject progressObj;
        [SerializeField]
        Image progress;
        [SerializeField]
        Text progressTxt;

        [SerializeField]
        ButtonComponent refreshBtn;

        [SerializeField]
        Dropdown languageBtn;

        private int dotCount = 1;

        public void OnOpen(Params p)
        {
            this.paramBuffer = p;

            this.lottoBtn.Text = p.lottoBtnTxt;

            this.progress.fillAmount = 0f;
            this.progressTxt.text = string.Empty;

            this.refreshBtn.Text = p.refreshBtnTxt;

            for (int i = 0; i < p.languagesList.Count; i++)
                this.languageBtn.options.Add(new(p.languagesList[i]));
            this.languageBtn.onValueChanged.AddListener(this.OnClickChangeLanguage);
        }

        public void SetStateCheckAsset()
        {
            this.lottoBtn.GameObject.SetActive(false);
            this.progressObj.SetActive(true);
            this.refreshBtn.Interactable = false;
            this.languageBtn.enabled = false;
        }

        public void SetStateWaitDone()
        {
            this.lottoBtn.GameObject.SetActive(true);
            this.progressObj.SetActive(false);
            this.refreshBtn.Interactable = true;
            this.languageBtn.enabled = true;
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

        public void SetLanguageState(int optionNum)
        {
            this.languageBtn.SetValueWithoutNotify(optionNum);
            this.languageBtn.RefreshShownValue();
        }

        public void OnClickLottoSelect()
        {
            this.DoEvent(Event.LottoSelect);
        }

        public void OnClickRefreshAsset()
        {
            this.DoEvent(Event.RefreshAsset);
        }

        public void OnClickChangeLanguage(int option)
        {
            this.DoEvent(Event.ChangeLanguage, option);
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform.Find("Canvas/Padding");
            Transform t;

            t = cached.Find("BtnLottoSelect");
            this.lottoBtn = new ButtonComponent(t);

            t = cached.Find("ProgressSlider");
            this.progressObj = t.gameObject;
            this.progress = t.FindComponent<Image>("Fill");
            this.progressTxt = t.FindComponent<Text>("Text");

            t = cached.Find("BtnRefresh");
            this.refreshBtn = new ButtonComponent(t);

            t = cached.Find("BtnLanguage");
            this.languageBtn = t.GetComponent<Dropdown>();
        }
#endif  // UNITY_EDITOR
    }
}
