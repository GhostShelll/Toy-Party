using UnityEngine;

using com.jbg.asset.control;
using com.jbg.asset.data;
using com.jbg.core.popup;

namespace com.jbg.content.popup
{
    using Assist = PopupAssist;

    public class PopupAssist
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////// OPEN POPUP FUNCTION
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Popup OpenPopup(string popupName, Popup.Params p, System.Action<Popup> resultCallback, System.Action<Popup> loadedCallback = null)
        {
            Popup popup = PopupManager.Instance.Load(popupName);
            if (popup != null)
            {
                popup.Open(p, resultCallback, loadedCallback);
                return popup;
            }
            else
            {
                Debug.Assert(false, popupName);
                resultCallback?.Invoke(null);
            }

            return null;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Popup OpenNoticeOneBtnPopup(string title, string message, System.Action<Popup> resultCallback)
        {
            Popup.Params p = new();
            p.title = title;
            p.message = message;
            p.btnOkText = LocaleControl.GetString(LocaleCodes.BTN_OK);

            return Assist.OpenPopup("Popup_Notice_1Btn", p, resultCallback);
        }
    }
}
