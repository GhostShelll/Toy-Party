using System.Collections.Generic;

using UnityEngine;

namespace com.jbg.core.popup
{
    public class PopupManager : MonoBehaviour
    {
        // Singleton
        private PopupManager() { }
        private static readonly System.Lazy<PopupManager> instance = new(() => new());
        public static PopupManager Instance { get { return instance.Value; } }

        [SerializeField]
        List<Popup> popupList = new();

        public void AddPopup(Popup popup)
        {
            this.popupList.Add(popup);
        }

        public void RemovePopup(Popup popup)
        {
            this.popupList.Remove(popup);
        }

        public bool IsPopupOn()
        {
            return (this.popupList.Count > 0);
        }

        public void CloseAllPopup()
        {
            for (int i = this.popupList.Count - 1; i >= 0; i--)
            {
                if (this.popupList[i] != null)
                    this.popupList[i].Close();
            }
        }

        public void CloseAllPopup_OneExcept<T>() where T : Popup
        {
            for (int i = this.popupList.Count - 1; i >= 0; i--)
            {
                if (this.popupList[i] != null)
                {
                    if (this.popupList[i] as T)
                        continue;

                    this.popupList[i].Close();
                }
            }
        }

        public void ShowAllPopup()
        {
            for (int i = 0; i < this.popupList.Count; i++)
                this.popupList[i].Show();
        }

        public void HideAllPopup()
        {
            for (int i = 0; i < this.popupList.Count; i++)
                this.popupList[i].Hide();
        }
    }
}
