using System.Collections.Generic;

using UnityEngine;

using com.jbg.asset.control;
using com.jbg.asset.data;
using com.jbg.content.popup.view;
using com.jbg.core;
using com.jbg.core.manager;
using com.jbg.core.popup;

namespace com.jbg.content.popup
{
    using Assist = LottoSelectPopupAssist;

    public class LottoSelectPopupAssist
    {
        private static Assist Instance = null;

        private LottoSelectPopup popupView;
        private System.Action<int[]> resultCallback;

        private List<int> currentNumbers;
        private List<int> selectedIndex;
        private bool highestMode;
        private int numberRange;

        private const int MAX_RANGE = 10;

        #region Static members
        public static void Open(int choiceNum, System.Action<int[]> resultCallback)
        {
            if (Assist.Instance == null)
                Assist.Instance = new Assist();

            Assist.Instance.OpenPopup(choiceNum, resultCallback);
        }

        public static void Close()
        {
            Assist.Instance = null;
        }
        #endregion

        #region Non-Static members
        private void OpenPopup(int choiceNum, System.Action<int[]> resultCallback)
        {
            this.resultCallback = resultCallback;

            if (this.currentNumbers != null)
                this.currentNumbers.Clear();
            this.currentNumbers = LottoResultControl.GetLottoNumbers(choiceNum);

            if (this.selectedIndex != null)
                this.selectedIndex.Clear();
            this.selectedIndex = new();

            this.highestMode = false;
            this.numberRange = 0;

            LottoSelectPopup.Params p = new();
            p.title = LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_TITLE_TEXT);
            p.countTxt = new();
            for (int i = 0; i < this.currentNumbers.Count; i++)
                p.countTxt.Add(this.currentNumbers[i].ToString());
            p.highestToggleTxt = LocaleControl.GetString(LocaleCodes.LOTTO_SELECT_POPUP_TOGGLE_TEXT);
            p.btnOkText = LocaleControl.GetString(LocaleCodes.BTN_OK);

            PopupAssist.OpenPopup("Popup_LottoSelect", p, this.ResultCallback, this.LoadedCallback);
        }

        private void ResultCallback(Popup popup)
        {
            this.popupView.RemoveEvent(LottoSelectPopup.Event.HighestNumOn);
            this.popupView.RemoveEvent(LottoSelectPopup.Event.NumMinus);
            this.popupView.RemoveEvent(LottoSelectPopup.Event.NumPlus);

            this.resultCallback?.Invoke(this.selectedIndex.ToArray());
            this.resultCallback = null;

            this.currentNumbers.Clear();
            this.currentNumbers = null;

            this.selectedIndex.Clear();
            this.selectedIndex = null;

            Assist.Close();
        }

        private void LoadedCallback(Popup popup)
        {
            this.popupView = (LottoSelectPopup)popup;

            this.popupView.BindEvent(LottoSelectPopup.Event.HighestNumOn, this.OnClickHighestNumOn);
            this.popupView.BindEvent(LottoSelectPopup.Event.NumMinus, this.OnClickNumMinus);
            this.popupView.BindEvent(LottoSelectPopup.Event.NumPlus, this.OnClickNumPlus);

            this.UpdateSelect();
        }

        private void UpdateSelect()
        {
            // 최소값 혹은 최대값 찾기
            int pinPoint;
            if (this.highestMode)
                pinPoint = Mathf.Max(this.currentNumbers.ToArray());
            else
                pinPoint = Mathf.Min(this.currentNumbers.ToArray());

            // 숫자 범위에 따른 갯수 목록 나열
            this.selectedIndex.Clear();
            for (int i = 0; i < this.currentNumbers.Count; i++)
            {
                int num = this.currentNumbers[i];
                if (this.highestMode)
                {
                    if (pinPoint - this.numberRange <= num && num <= pinPoint)
                        this.selectedIndex.Add(i);
                }
                else
                {
                    if (pinPoint <= num && num <= pinPoint + this.numberRange)
                        this.selectedIndex.Add(i);
                }
            }

            this.popupView.SetCountText(this.selectedIndex);
            this.popupView.SetCurrentNum(this.numberRange.ToString());
        }

        private void OnClickHighestNumOn(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            bool parse = bool.TryParse(obj.ToString(), out bool isOn);
            if (parse == false)
            {
                DebugEx.LogColor("LOTTO_SELECT_POPUP_ASSIST PARSE ERROR", "red");
                return;
            }

            DebugEx.Log("LOTTO_SELECT_POPUP_ASSIST Highest Mode is " + isOn.ToString());
            this.highestMode = isOn;
            this.UpdateSelect();
        }

        private void OnClickNumMinus(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            if (this.numberRange == 0)
                return;

            this.numberRange--;
            this.UpdateSelect();
        }

        private void OnClickNumPlus(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            if (this.numberRange == Assist.MAX_RANGE)
                return;

            this.numberRange++;
            this.UpdateSelect();
        }
        #endregion
    }
}
