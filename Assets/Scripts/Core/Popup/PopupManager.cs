using System.Collections.Generic;

using UnityEngine;

namespace com.jbg.core.popup
{
    public class PopupManager : MonoBehaviour
    {
        private const string POPUP_BASE_PATH = "Prefabs/Popups/";

        public static PopupManager Instance { get; private set; }

        [SerializeField]
        LinkedList<Popup> popupList = new();
        public int OpenCount { get { return this.popupList.Count; } }
        public Popup TopPopup
        {
            get
            {
                LinkedListNode<Popup> lastPopup = this.popupList.Last;
                if (lastPopup == null)
                    return null;

                return lastPopup.Value;
            }
        }

        private void Awake()
        {
            PopupManager.Instance = this;
        }

        private void OnDestroy()
        {
            PopupManager.Instance = null;
        }

        public void AddPopup(Popup popup)
        {
            this.popupList.AddLast(popup);
        }

        public void RemovePopup(Popup popup)
        {
            this.popupList.Remove(popup);
        }

        public void CloseAllPopup()
        {
            LinkedListNode<Popup> node = this.popupList.First;
            while (node != null)
            {
                Popup popup = node.Value;
                if (popup != null)
                    popup.Close();

                node = node.Next;
            }
        }

        public void CloseAllPopup_OneExcept<T>() where T : Popup
        {
            LinkedListNode<Popup> node = this.popupList.First;
            while (node != null)
            {
                Popup popup = node.Value;
                if (popup != null)
                {
                    if (popup as T)
                        continue;

                    popup.Close();
                }

                node = node.Next;
            }
        }

        public void ShowAllPopup()
        {
            LinkedListNode<Popup> node = this.popupList.First;
            while (node != null)
            {
                Popup popup = node.Value;
                if (popup != null)
                    popup.Show();

                node = node.Next;
            }
        }

        public void HideAllPopup()
        {
            LinkedListNode<Popup> node = this.popupList.First;
            while (node != null)
            {
                Popup popup = node.Value;
                if (popup != null)
                    popup.Hide();

                node = node.Next;
            }
        }

        public Popup Load(string popupName)
        {
            DebugEx.Log("POPUP_LOAD:" + popupName);

            if (this == null)
            {
                DebugEx.Log("POPUP_LOAD_FAILED:" + popupName + ", MANAGER_IS_NULL");
                return null;
            }

            string popupPath = PopupManager.POPUP_BASE_PATH + popupName;

            Popup prefab = Resources.Load<Popup>(popupPath);
            if (prefab == null)
            {
                DebugEx.Log("POPUP_LOAD_FAILED:" + popupName + ", PATH:" + popupPath + ", RESOURCES_LOAD_FAILED");
                return null;
            }

            GameObject popupObj = GameObject.Instantiate(prefab.gameObject);
            if (popupObj == null)
            {
                DebugEx.Log("POPUP_LOAD_FAILED:" + popupName + ", PATH:" + popupPath + ", GAMEOBJECT_INSTANTIATE_FAILED");

                Object.Destroy(prefab);
                return null;
            }

            Popup popup = popupObj.GetComponent<Popup>();
            if (popup == null)
            {
                DebugEx.Log("POPUP_LOAD_FAILED:" + popupName + ", PATH:" + popupPath + ", GETCOMPONENT_FAILED");

                GameObject.Destroy(popupObj);
                Object.Destroy(prefab);
                return null;
            }

            RectTransform trans = popup.CachedRectTransform;
            RectTransform transPrefab = prefab.CachedRectTransform;
            trans.SetParent(this.transform);
            trans.name = popupName;
            trans.anchoredPosition = transPrefab.anchoredPosition;
            trans.localPosition = transPrefab.localPosition;
            trans.localScale = transPrefab.localScale;
            trans.offsetMax = transPrefab.offsetMax;
            trans.offsetMin = transPrefab.offsetMin;
            trans.HideTransform();

            this.AddPopup(popup);

            return popup;
        }
    }
}
