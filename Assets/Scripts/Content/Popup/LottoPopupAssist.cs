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
        private List<int> selectedNumbersAll;                   // 슬롯 상관없이 선택된 모든 번호들
        private Dictionary<int, int> resultNumbers;             // 각 슬롯 별로 지정된 번호들

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

            if (this.selectedNumbersAll != null)
                this.selectedNumbersAll.Clear();
            this.selectedNumbersAll = new();

            if (this.resultNumbers != null)
                this.resultNumbers.Clear();
            this.resultNumbers = new();

            LottoPopup.Params p = new();
            p.title = string.Format(LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_TITLE_TEXT), LottoResultControl.RecentPeriod + 1);
            p.lottoInfoTxt = LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_MSG);
            p.defaultSelectTxt = LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_DEFAULT_SELECT);
            p.btnShuffleTxt = LocaleControl.GetString(LocaleCodes.LOTTO_POPUP_SHUFFLE_BTN_TEXT);

            PopupAssist.OpenPopup("Popup_Lotto", p, this.ResultCallback, this.LoadedCallback);
        }

        private void ResultCallback(Popup popup)
        {
            this.popupView.RemoveEvent(LottoPopup.Event.Select);
            this.popupView.RemoveEvent(LottoPopup.Event.Shuffle);

            this.resultCallback?.Invoke();
            this.resultCallback = null;

            if (this.selectedNumbers != null)
                this.selectedNumbers.Clear();
            this.selectedNumbers = null;

            if (this.selectedNumbersAll != null)
                this.selectedNumbersAll.Clear();
            this.selectedNumbersAll = null;

            if (this.resultNumbers != null)
                this.resultNumbers.Clear();
            this.resultNumbers = null;

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

            this.popupView.Hide();
            LottoSelectPopupAssist.Open(btnNum, (selectedIndex) =>
            {
                if (this.selectedNumbers.ContainsKey(btnNum) == false)
                    this.selectedNumbers.Add(btnNum, new());
                else
                    this.selectedNumbers[btnNum].Clear();

                this.selectedNumbersAll.Clear();

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

                Dictionary<int, List<int>>.Enumerator enumerator = this.selectedNumbers.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    List<int> selectedNumbersOneSlot = enumerator.Current.Value;
                    for (int i = 0; i < selectedNumbersOneSlot.Count; i++)
                    {
                        if (this.selectedNumbersAll.Contains(selectedNumbersOneSlot[i]))
                            continue;

                        this.selectedNumbersAll.Add(selectedNumbersOneSlot[i]);
                    }
                }

                this.selectedNumbersAll.Sort();

                selectInfo.Clear();
                for (int i = 0; i < this.selectedNumbersAll.Count; i++)
                {
                    int num = this.selectedNumbersAll[i];
                    selectInfo.Append(num);
                    if (i + 1 < this.selectedNumbersAll.Count)
                        selectInfo.Append(", ");
                }

                this.popupView.SetSelectInfoAllText(selectInfo.ToString());

                this.popupView.Show();
            });
        }

        private void OnClickShuffle(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            List<int> containCheck = new();
            List<int> overlapCheck = new();

            this.resultNumbers.Clear();

            Dictionary<int, List<int>>.Enumerator enumerator1 = this.selectedNumbers.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                int key = enumerator1.Current.Key;
                List<int> value = enumerator1.Current.Value;
                value.Shuffle();

                int pickNumber = value[0];
                if (containCheck.Contains(pickNumber) == false)
                    containCheck.Add(pickNumber);
                else
                    overlapCheck.Add(pickNumber);

                this.resultNumbers.Add(key, pickNumber);
            }

            Dictionary<int, int>.Enumerator enumerator2 = this.resultNumbers.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                int key = enumerator2.Current.Key;
                int value = enumerator2.Current.Value;
                bool isOverlap = overlapCheck.Contains(value);

                this.popupView.SetResultInfoText(key - 1, value.ToString(), isOverlap);
            }
        }
        #endregion
    }
}
