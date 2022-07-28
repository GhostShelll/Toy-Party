using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using com.jbg.core;
using com.jbg.core.popup;

namespace com.jbg.content.popup.view
{
    public class LottoSelectPopup : Popup
    {
        public enum Event
        {
            HighestNumOn,
            MiddleNumOn,
            NumMinus,
            NumPlus,
        }

        public new class Params : Popup.Params
        {
            public List<string> countTxt;
            public string highestToggleTxt;
            public string middleToggleTxt;
        }

        [Header("Lotto Select Popup")]
        [SerializeField]
        Text[] countTxt;
        [SerializeField]
        Toggle highestToggle;
        [SerializeField]
        Text highestToggleTxt;
        [SerializeField]
        Toggle middleToggle;
        [SerializeField]
        Text middleToggleTxt;
        [SerializeField]
        Text currentNumTxt;

        protected override void OnBuild()
        {
            base.OnBuild();

            Params p = (Params)this.paramBuffer;

            for (int i = 0; i < this.countTxt.Length; i++)
            {
                if (p.countTxt.Count <= i)
                    this.countTxt[i].text = "XX";
                else
                    this.countTxt[i].text = p.countTxt[i];
            }

            this.highestToggle.isOn = false;
            this.highestToggleTxt.text = p.highestToggleTxt;
            this.middleToggle.isOn = false;
            this.middleToggleTxt.text = p.middleToggleTxt;

            this.currentNumTxt.text = "0";
        }

        public void SetCountText(List<int> selectIndex)
        {
            bool highToggleOn = this.highestToggle.isOn;
            bool middleToggleOn = this.middleToggle.isOn;

            for (int i = 0; i < this.countTxt.Length; i++)
            {
                if (selectIndex.Contains(i))
                    this.countTxt[i].color = highToggleOn ? Color.red : middleToggleOn ? Color.yellow : Color.blue;
                else
                    this.countTxt[i].color = UnityEx.DefaultTextColor;
            }
        }

        public void SetCurrentNum(string value)
        {
            this.currentNumTxt.text = value;
        }

        public void OnClickHighestNumOn()
        {
            this.DoEvent(Event.HighestNumOn, this.highestToggle.isOn);
        }
        public void OnClickMiddleNumOn()
        {
            this.DoEvent(Event.MiddleNumOn, this.middleToggle.isOn);
        }

        public void OnClickNumMinus()
        {
            this.DoEvent(Event.NumMinus);
        }

        public void OnClickNumPlus()
        {
            this.DoEvent(Event.NumPlus);
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform contents = this.ContentsBaseRectTransform;
            Transform t;

            this.countTxt = new Text[45];

            int index = 0;
            t = contents.Find("NumberInfo");
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child1 = t.GetChild(i);
                for (int j = 0; j < child1.childCount; j++)
                {
                    Transform child2 = child1.GetChild(j);
                    for (int k = 0; k < child2.childCount; k++)
                    {
                        this.countTxt[index] = child2.GetChild(k).FindComponent<Text>("Count/Text");
                        index++;
                    }
                }
            }

            t = contents.Find("Toggle/ToggleUpDown");
            this.highestToggle = t.FindComponent<Toggle>();
            this.highestToggleTxt = t.FindComponent<Text>("Text");

            t = contents.Find("Toggle/ToggleMiddle");
            this.middleToggle = t.FindComponent<Toggle>();
            this.middleToggleTxt = t.FindComponent<Text>("Text");

            t = contents.Find("BtnGroup");
            this.currentNumTxt = t.FindComponent<Text>("CurNumSize/Text");
        }
#endif  // UNITY_EDITOR
    }
}
