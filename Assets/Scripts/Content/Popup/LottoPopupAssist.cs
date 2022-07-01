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

            LottoPopup.Params p = new();
            p.lottoInfoTxt = "@@회차별 로또 번호를 선택해주세요.";        // TODO[jbg] : 로케일 데이터 사용
            p.defaultSelectTxt = "@@Select";
            p.btnShuffleTxt = "@@Shuffle";

            SystemPopupAssist.OpenPopup("Popup_Lotto", p, this.ResultCallback, this.LoadedCallback);
        }

        private void ResultCallback(Popup popup)
        {
            this.popupView.RemoveEvent(LottoPopup.Event.Select);
            this.popupView.RemoveEvent(LottoPopup.Event.Shuffle);

            this.resultCallback?.Invoke();
            this.resultCallback = null;

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

            DebugEx.Log("TODO[jbg] OnClickSelect Number:" + btnNum);
        }

        private void OnClickShuffle(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            DebugEx.Log("TODO[jbg] OnClickShuffle");
        }
        #endregion
    }
}
