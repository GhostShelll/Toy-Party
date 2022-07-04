using System.Collections.Generic;
using System.Text;

using com.jbg.asset.control;
using com.jbg.asset.data;
using com.jbg.content.popup.view;
using com.jbg.core;
using com.jbg.core.manager;
using com.jbg.core.popup;

namespace com.jbg.content.popup
{
    using Assist = LottoPopupAssist;

    public class LottoPopupAssist
    {
        private static Assist Instance = null;

        private LottoPopup popupView;
        private System.Action resultCallback;

        private Dictionary<int, List<int>> selectedNumbers;     // 각 슬롯 별로 선택된 번호들

        #region Static members
        public static void Open(System.Action resultCallback)
        {
            if (Assist.Instance == null)
                Assist.Instance = new Assist();

            Assist.Instance.OpenPopup(resultCallback);
        }

        public static void Close()
        {
            Assist.Instance = null;
        }
        #endregion

        #region Non-Static members
        private void OpenPopup(System.Action resultCallback)
        {
            this.resultCallback = resultCallback;

            if (this.selectedNumbers != null)
                this.selectedNumbers.Clear();
            this.selectedNumbers = new();

            LottoPopup.Params p = new();
            p.lottoInfoTxt = LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_MSG);
            p.defaultSelectTxt = LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_DEFAULT_SELECT);
            p.btnShuffleTxt = LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_SHUFFLE_BTN_TEXT);

            SystemPopupAssist.OpenPopup("Popup_Lotto", p, this.ResultCallback, this.LoadedCallback);
        }

        private void ResultCallback(Popup popup)
        {
            this.popupView.RemoveEvent(LottoPopup.Event.Select);
            this.popupView.RemoveEvent(LottoPopup.Event.Shuffle);

            this.resultCallback?.Invoke();
            this.resultCallback = null;

            this.selectedNumbers.Clear();
            this.selectedNumbers = null;

            Assist.Close();
        }

        private void LoadedCallback(Popup popup)
        {
            this.popupView = (LottoPopup)popup;

            this.popupView.BindEvent(LottoPopup.Event.Select, this.OnClickSelect);
            this.popupView.BindEvent(LottoPopup.Event.Shuffle, this.OnClickShuffle);
        }

        private void OnClickSelect(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            bool parse = int.TryParse(obj.ToString(), out int btnNum);
            if (parse == false)
            {
                DebugEx.LogColor("LOTTO_POPUP_ASSIST PARSE ERROR. Object:" + obj.ToString(), "red");
                return;
            }

            LottoSelectPopupAssist.Open(btnNum, (selectedIndex) =>
            {
                if (this.selectedNumbers.ContainsKey(btnNum) == false)
                    this.selectedNumbers.Add(btnNum, new());
                else
                    this.selectedNumbers[btnNum].Clear();

                StringBuilder selectInfo = new();
                for (int i = 0; i < selectedIndex.Length; i++)
                {
                    int num = selectedIndex[i] + 1;
                    selectInfo.Append(num);
                    if (i + 1 < selectedIndex.Length)
                        selectInfo.Append(", ");

                    this.selectedNumbers[btnNum].Add(num);
                }

                this.popupView.SetSelectInfoText(btnNum - 1, selectInfo.ToString());
            });
        }

        private void OnClickShuffle(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            DebugEx.Log("TODO[jbg] OnClickShuffle");
        }
        #endregion
    }
}
