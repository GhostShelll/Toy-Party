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
        private System.Action resultCallback;

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

            LottoSelectPopup.Params p = new();
            p.countTxt = new();
            p.highestToggleTxt = LocaleControl.GetString(LocaleCodes.LOTTO_SELECT_POPUP_TOGGLE_TEXT);

            SystemPopupAssist.OpenPopup("Popup_LottoSelect", p, this.ResultCallback, this.LoadedCallback);
        }

        private void ResultCallback(Popup popup)
        {
            this.popupView.RemoveEvent(LottoSelectPopup.Event.HighestNumOn);
            this.popupView.RemoveEvent(LottoSelectPopup.Event.NumMinus);
            this.popupView.RemoveEvent(LottoSelectPopup.Event.NumPlus);

            this.resultCallback?.Invoke();
            this.resultCallback = null;

            Assist.Close();
        }

        private void LoadedCallback(Popup popup)
        {
            this.popupView = (LottoSelectPopup)popup;

            this.popupView.BindEvent(LottoSelectPopup.Event.HighestNumOn, this.OnClickHighestNumOn);
            this.popupView.BindEvent(LottoSelectPopup.Event.NumMinus, this.OnClickNumMinus);
            this.popupView.BindEvent(LottoSelectPopup.Event.NumPlus, this.OnClickNumPlus);
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

            // TODO[jbg]
        }

        private void OnClickNumMinus(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            // TODO[jbg]
        }

        private void OnClickNumPlus(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            // TODO[jbg]
        }
        #endregion
    }
}
