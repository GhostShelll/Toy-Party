using System;

using UnityEngine;
using UnityEngine.UI;

namespace com.jbg.core
{
    [Serializable]
    public class ButtonComponent : BaseComponent
    {
        [SerializeField]
        Button button;
        public Button Button
        {
            get { return this.button; }
        }

        [SerializeField]
        Image box;

        [SerializeField]
        Image disabled;

        public bool Interactable
        {
            get
            {
                if (this.button != null)
                    return this.button.interactable;

                return false;
            }
            set
            {
                if (this.button != null)
                    this.button.interactable = value;

                if (value)
                {
                    if (this.box != null)
                        this.box.gameObject.SetActive(true);

                    if (this.disabled != null)
                        this.disabled.gameObject.SetActive(false);
                }
                else
                {
                    if (this.box != null)
                        this.box.gameObject.SetActive(false);

                    if (this.disabled != null)
                        this.disabled.gameObject.SetActive(true);
                }
            }
        }

        [SerializeField]
        Text label;
        public Text Label
        {
            get { return this.label; }
        }

        public string Text
        {
            get
            {
                if (this.label != null)
                    return this.label.text;

                return string.Empty;
            }
            set
            {
                if (this.label != null)
                    this.label.text = value;
            }
        }

        public ButtonComponent(GameObject target) : base(target) { }

        public ButtonComponent(Transform targetTransform) : base(targetTransform) { }

        public static implicit operator bool(ButtonComponent thiz)
        {
            return thiz != null;
        }

        public override void Assign(GameObject target)
        {
            base.Assign(target);

            if (target != null)
            {
                Transform t = this.Transform;

                this.button = t.FindComponent<Button>();

                GameObject baseObject = t.FindObject("Base");
                if (baseObject != null)
                {
                    this.box = t.FindComponent<Image>("Base/Box");
                    this.disabled = t.FindComponent<Image>("Base/Disabled");
                    this.label = t.FindComponent<Text>("Base/Text");
                }
                else
                {
                    this.box = t.FindComponent<Image>("Box");
                    this.disabled = t.FindComponent<Image>("Disabled");
                    this.label = t.FindComponent<Text>("Text");
                }
            }
            else
            {
                this.button = null;
                this.box = null;
                this.disabled = null;
                this.label = null;
            }
        }
    }
}
