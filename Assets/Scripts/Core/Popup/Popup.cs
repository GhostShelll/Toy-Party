using UnityEngine;
using UnityEngine.UI;

using com.jbg.core.scene;

namespace com.jbg.core.popup
{
    using Manager = PopupManager;

    public class Popup : View
    {
        public class Params
        {
            public string title;

            public string message;
            public string[] messageArr;

            public string btnOkText;
            public string btnCancelText;

            public bool useBlind = false;
            public bool useBtnX = true;
        }

        protected Params paramBuffer = null;

        public Params GetParam()
        {
            return this.paramBuffer;
        }

        [SerializeField]
        protected BaseComponent popupBase;

        [SerializeField]
        protected BaseComponent touchLocked;

        [SerializeField]
        protected BaseComponent blindBase;
        public bool Blind
        {
            set
            {
                if (this.blindBase != null && this.blindBase.GameObject != null)
                    this.blindBase.GameObject.SetActive(value);
            }
            get
            {
                if (this.blindBase != null && this.blindBase.GameObject != null)
                    return this.blindBase.GameObject.activeSelf;

                return false;
            }
        }


        [SerializeField]
        protected BaseComponent contentsBase;
        public RectTransform ContentsBaseRectTransform
        {
            get
            {
                return this.contentsBase.RectTransform;
            }
        }

        [SerializeField]
        protected BaseComponent titleBase;
        [SerializeField]
        protected Text titleText;
        public string TitleText
        {
            set
            {
                if (this.titleText != null)
                    this.titleText.text = string.IsNullOrEmpty(value) == false ? value : string.Empty;

                this.TitleVisible = string.IsNullOrEmpty(this.TitleText) == false;
            }
            get
            {
                if (this.titleText != null)
                    return this.titleText.text;

                return string.Empty;
            }
        }
        public bool TitleVisible
        {
            set
            {
                if (this.titleBase != null && this.titleBase.GameObject != null)
                    this.titleBase.GameObject.SetActive(value);
            }
            get
            {
                if (this.titleBase != null && this.titleBase.GameObject != null)
                    return this.titleBase.GameObject.activeSelf;

                return false;
            }
        }

        [SerializeField]
        protected BaseComponent messageBase;
        [SerializeField]
        protected Text[] messageText;
        public string MessageText
        {
            set
            {
                this.SetMessageText(0, value);
            }
            get
            {
                return this.GetMessageText(0);
            }
        }
        public bool MessageVisible
        {
            set
            {
                if (this.messageBase != null && this.messageBase.GameObject != null)
                    this.messageBase.GameObject.SetActive(value);
            }
            get
            {
                if (this.messageBase != null && this.messageBase.GameObject != null)
                    return this.messageBase.GameObject.activeSelf;

                return false;
            }
        }
        public void SetMessageText(int index, string value)
        {
            if (this.messageText != null && index >= 0 && index < this.messageText.Length)
            {
                Text message = this.messageText[index];
                if (message)
                {
                    message.text = value ?? string.Empty;
                    message.gameObject.SetActive(string.IsNullOrEmpty(message.text) == false);
                }
            }
        }
        public string GetMessageText(int index)
        {
            if (this.messageText != null && index >= 0 && index < this.messageText.Length)
            {
                Text message = this.messageText[index];
                if (message)
                    return message.text;
            }

            return string.Empty;
        }
        public void ClearMessagesText()
        {
            if (this.messageText != null)
            {
                for (int i = 0; i < this.messageText.Length; i++)
                {
                    Text message = this.messageText[i];
                    if (message)
                        message.gameObject.SetActive(false);
                }
            }
        }

        [SerializeField]
        protected ButtonComponent btnOK;
        public string BtnOKText
        {
            set
            {
                if (this.btnOK != null && this.btnOK.Label != null)
                    this.btnOK.Text = value ?? string.Empty;

                this.BtnOKVisible = string.IsNullOrEmpty(this.BtnOKText) == false;
            }
            get
            {
                if (this.btnOK && this.btnOK.Label)
                    return this.btnOK.Text;

                return string.Empty;
            }
        }
        public bool BtnOKVisible
        {
            set
            {
                if (this.btnOK != null && this.btnOK.GameObject != null)
                    this.btnOK.GameObject.SetActive(value);
            }
            get
            {
                if (this.btnOK != null && this.btnOK.GameObject != null)
                    return this.btnOK.GameObject.activeSelf;

                return false;
            }
        }
        public bool BtnOKEnable
        {
            set
            {
                if (this.btnOK != null && this.btnOK.Button != null)
                    this.btnOK.Interactable = value;
            }
            get
            {
                if (this.btnOK != null && this.btnOK.Button != null)
                    return this.btnOK.Interactable;

                return false;
            }
        }

        [SerializeField]
        protected ButtonComponent btnCancel;
        public string BtnCancelText
        {
            set
            {
                if (this.btnCancel != null && this.btnCancel.Label != null)
                    this.btnCancel.Text = value ?? string.Empty;

                this.BtnCancelVisible = string.IsNullOrEmpty(this.BtnCancelText) == false;
            }
            get
            {
                if (this.btnCancel != null && this.btnCancel.Label != null)
                    return this.btnCancel.Text;

                return string.Empty;
            }
        }
        public bool BtnCancelVisible
        {
            set
            {
                if (this.btnCancel != null && this.btnCancel.GameObject != null)
                    this.btnCancel.GameObject.SetActive(value);
            }
            get
            {
                if (this.btnCancel != null && this.btnCancel.GameObject != null)
                    return this.btnCancel.GameObject.activeSelf;

                return false;
            }
        }
        public bool BtnCancelEnable
        {
            set
            {
                if (this.btnCancel != null && this.btnCancel.Button != null)
                    this.btnCancel.Interactable = value;
            }
            get
            {
                if (this.btnCancel != null && this.btnCancel.Button != null)
                    return this.btnCancel.Interactable;

                return false;
            }
        }

        [SerializeField]
        protected ButtonComponent btnX;
        public bool BtnXVisible
        {
            set
            {
                if (this.btnX != null && this.btnX.GameObject != null)
                    this.btnX.GameObject.SetActive(value);
            }
            get
            {
                if (this.btnX != null && this.btnX.GameObject != null)
                    return this.btnX.GameObject.activeSelf;

                return false;
            }
        }

        private bool startOn = false;
        private bool afterBuildOn = false;
        private bool isDestroy = false;

        public bool IsOpened { get; private set; }

        public System.Action<Popup> LoadedCallback { get; private set; }
        public System.Action<Popup> ResultCallback { get; private set; }

        public bool IsOK { get; set; }
        public bool IsX { get; set; }

        public void Open(Params p, System.Action<Popup> resultCallback, System.Action<Popup> loadedCallback = null)
        {
            DebugEx.Log("POPUP_OPEN:" + this.name + ", IS OPENED:" + this.IsOpened + ", POPUP_OPEN_COUNT:" + Manager.Instance.OpenCount + ", POPUP_ON:" + Manager.Instance.TopPopup);

            if (this.IsOpened)
                this.Close();
            this.IsOpened = true;

            this.paramBuffer = p;
            this.LoadedCallback = loadedCallback;
            this.ResultCallback = resultCallback;

            this.popupBase.GameObject.SetActive(true);

            if (this.startOn)
                this.Build();
            else
                this.afterBuildOn = true;
        }

        public void Close()
        {
            if (this.IsOpened == false)
                return;
            this.IsOpened = false;

            DebugEx.Log("POPUP_CLOSE:" + this.name + ", POPUP_OPEN_COUNT:" + Manager.Instance.OpenCount + ", POPUP_ON:" + Manager.Instance.TopPopup);

            this.popupBase.Transform.HideTransform();

            this.LoadedCallback = null;

            DebugEx.Log("Close: IsOK - " + this.IsOK);

            System.Action<Popup> resultCallback = this.ResultCallback;
            this.ResultCallback = null;

            resultCallback?.Invoke(this);

            this.OnClose();

            SceneExManager.ShowScene();

            this.DoDestroy();

            Manager.Instance.RemovePopup(this);
        }

        private void DoDestroy()
        {
            if (this.isDestroy)
                return;
            this.isDestroy = true;

            this.paramBuffer = null;

            GameObject.Destroy(this.gameObject);
        }

        public void OnClick(GameObject go)
        {
            if (this.IsOpened == false)
                return;

            this.IsOK = (go != null) && (go == this.btnOK.GameObject);
            this.IsX = (go != null) && (go == this.btnX.GameObject);

            // TODO[jbg] : 효과음 기능 구현
            //if (this.IsOK)
            //{
            //    if (this.BtnCancelVisible)
            //    {
            //        if (string.IsNullOrEmpty(Manager.BtnYesSound) == false)
            //            SoundFx.Play(Manager.BtnYesSound);
            //    }
            //    else
            //    {
            //        if (string.IsNullOrEmpty(Manager.BtnOKSound) == false)
            //            SoundFx.Play(Manager.BtnOKSound);
            //    }
            //}
            //else if (go == this.btnCancel.GameObject)
            //{
            //    if (string.IsNullOrEmpty(Manager.BtnNoSound) == false)
            //        SoundFx.Play(Manager.BtnNoSound);
            //}
            //else if (this.IsX)
            //{
            //    if (string.IsNullOrEmpty(Manager.BtnXSound) == false)
            //        SoundFx.Play(Manager.BtnXSound);
            //}

            if (this.OnClickOverride(go))
                this.Close();
        }

        private void Awake()
        {
            this.IsOK = false;
            this.IsX = false;

            this.OnAwake();
        }

        private void Start()
        {
            this.startOn = true;
            this.OnStart();

            if (this.afterBuildOn)
            {
                this.afterBuildOn = false;
                this.Build();
            }

            System.Action<Popup> loadedCallback = this.LoadedCallback;
            this.LoadedCallback = null;

            loadedCallback?.Invoke(this);
        }

        private void Build()
        {
            Params p = this.paramBuffer;

            if (this.popupBase.Transform.IsHideTransform() == false)
                this.popupBase.Transform.HideTransform();

            SceneExManager.HideScene();

            this.OnBuild();

            this.OnRefresh();

            this.popupBase.Transform.ShowTransform();

            if (this.startOn)
            {
                System.Action<Popup> loadedCallback = this.LoadedCallback;
                this.LoadedCallback = null;

                loadedCallback?.Invoke(this);
            }
        }

        public override void Refresh()
        {
            this.OnRefresh();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////// OVERRIDE
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnBuild()
        {
            Params p = this.paramBuffer;

            this.TitleText = p.title;

            this.ClearMessagesText();
            if (p.messageArr != null && p.messageArr.Length > 0)
            {
                for (int i = 0; i < p.messageArr.Length; i++)
                    this.SetMessageText(i, p.messageArr[i]);

                this.MessageVisible = true;
            }
            else
            {
                this.MessageText = p.message;
                this.MessageVisible = string.IsNullOrEmpty(this.MessageText) == false;
            }

            this.BtnOKText = p.btnOkText;
            this.BtnCancelText = p.btnCancelText;

            this.Blind = p.useBlind;
            this.BtnXVisible = p.useBtnX;
        }
        protected virtual void OnRefresh() { }
        protected virtual void OnClose() { }
        protected virtual bool OnClickOverride(GameObject go) { return true; }
        public virtual void OnBack()
        {
            if (this.paramBuffer == null)
                return;

            this.OnClick(this.btnX.GameObject);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform;
            Transform t;

            this.popupBase = new BaseComponent(cached);
            this.touchLocked = new BaseComponent(cached.Find("TouchLock"));
            this.blindBase = new BaseComponent(cached.Find("Blind"));

            t = cached.Find("Contents");
            this.contentsBase = new BaseComponent(t);

            cached = this.ContentsBaseRectTransform;

            t = cached.Find("Title");
            this.titleBase = new BaseComponent(t);
            this.titleText = t.Find("Text").GetComponent<Text>();

            t = cached.Find("Message");
            this.messageBase = new BaseComponent(t);
            Text[] text = t.GetComponentsInChildren<Text>();
            if (text != null)
                System.Array.Sort<Text>(text, (l, r) => { return l.name.CompareTo(r.name); });
            this.messageText = text;

            this.btnOK = new ButtonComponent(cached.Find("BtnOk"));
            this.btnCancel = new ButtonComponent(cached.Find("BtnCancel"));
            this.btnX = new ButtonComponent(cached.Find("BtnX"));
        }
#endif  // UNITY_EDITOR
    }
}
