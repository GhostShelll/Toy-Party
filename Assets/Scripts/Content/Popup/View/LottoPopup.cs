using UnityEngine;
using UnityEngine.UI;

using com.jbg.core;
using com.jbg.core.popup;

namespace com.jbg.content.popup.view
{
    public class LottoPopup : Popup
    {
        public enum Event
        {
            Select,
            CombineSelectMode,
            Shuffle,
        }

        public new class Params : Popup.Params
        {
            public string lottoInfoTxt;
            public string defaultSelectTxt;
            public string combineSelectToggleTxt;
            public string btnShuffleTxt;
        }

        [Header("Lotto Popup")]
        [SerializeField]
        Text lottoInfoTxt;
        [SerializeField]
        GameObject selectInfoObj;
        [SerializeField]
        Text[] selectInfoTxt;
        [SerializeField]
        GameObject combineSelectInfoObj;
        [SerializeField]
        Text combineSelectInfoTxt;
        [SerializeField]
        Text[] resultInfoTxt;
        [SerializeField]
        Toggle combineSelectToggle;
        [SerializeField]
        Text combineSelectToggleTxt;
        [SerializeField]
        ButtonComponent btnShuffle;

        private const string DEFAULT_NUMBER = "00";

        protected override void OnBuild()
        {
            base.OnBuild();

            Params p = (Params)this.paramBuffer;

            this.lottoInfoTxt.text = p.lottoInfoTxt;

            for (int i = 0; i < this.selectInfoTxt.Length; i++)
                this.selectInfoTxt[i].text = p.defaultSelectTxt;

            this.combineSelectInfoTxt.text = p.defaultSelectTxt;

            for (int i = 0; i < this.resultInfoTxt.Length; i++)
                this.resultInfoTxt[i].text = LottoPopup.DEFAULT_NUMBER;

            this.combineSelectToggle.isOn = false;
            this.combineSelectToggleTxt.text = p.combineSelectToggleTxt;

            this.btnShuffle.Text = p.btnShuffleTxt;
        }

        public void SetStateSelectInfo(bool isCombineSelectMode)
        {
            this.selectInfoObj.SetActive(isCombineSelectMode == false);
            this.combineSelectInfoObj.SetActive(isCombineSelectMode);
        }

        public void SetSelectInfoText(int index, string text)
        {
            if (this.selectInfoTxt.Length <= index)
            {
                DebugEx.LogColor("LOTTO_POPUP SELECT INFO TEXT SET ERROR. Index:" + index, "red");
                return;
            }

            this.selectInfoTxt[index].text = text;
        }

        public void SetCombineSelectInfoText(string text)
        {
            this.combineSelectInfoTxt.text = text;
        }

        public void SetResultInfoText(int index, string text, bool isOverlap)
        {
            if (this.resultInfoTxt.Length <= index)
            {
                DebugEx.LogColor("LOTTO_POPUP RESULT INFO TEXT SET ERROR. Index:" + index, "red");
                return;
            }

            this.resultInfoTxt[index].text = text;
            this.resultInfoTxt[index].color = isOverlap ? Color.red : UnityEx.DefaultTextColor;
        }

        public void OnClickSelect(GameObject go)
        {
            string btnName = go.name;
            bool nameParse = int.TryParse(btnName, out int num);
            if (nameParse == false)
            {
                DebugEx.LogColor("LOTTO_POPUP PARSE ERROR. Name:" + btnName, "red");
                return;
            }

            this.DoEvent(Event.Select, num);
        }

        public void OnClickCombineSelectMode()
        {
            this.DoEvent(Event.CombineSelectMode, this.combineSelectToggle.isOn);
        }

        public void OnClickShuffle()
        {
            this.DoEvent(Event.Shuffle);
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform contents = this.ContentsBaseRectTransform;
            Transform t;

            t = contents.Find("LottoInfo");
            this.lottoInfoTxt = t.FindComponent<Text>("Text");

            t = contents.Find("SelectInfo");
            this.selectInfoObj = t.gameObject;
            this.selectInfoTxt = new Text[t.childCount];
            for (int i = 0; i < t.childCount; i++)
                this.selectInfoTxt[i] = t.GetChild(i).FindComponent<Text>("Text");

            t = contents.Find("CombineSelectInfo");
            this.combineSelectInfoObj = t.gameObject;
            this.combineSelectInfoTxt = t.FindComponent<Text>("Text");

            t = contents.Find("ResultInfo");
            this.resultInfoTxt = new Text[t.childCount];
            for (int i = 0; i < t.childCount; i++)
                this.resultInfoTxt[i] = t.GetChild(i).FindComponent<Text>("Text");

            t = contents.Find("ToggleCombineSelectMode");
            this.combineSelectToggle = t.GetComponent<Toggle>();
            this.combineSelectToggleTxt = t.FindComponent<Text>("Text");

            t = contents.Find("BtnShuffle");
            this.btnShuffle = new ButtonComponent(t);
        }
#endif  // UNITY_EDITOR
    }
}
