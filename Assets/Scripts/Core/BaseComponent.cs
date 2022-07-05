using System;

using UnityEngine;

namespace com.jbg.core
{
    [Serializable]
    public class BaseComponent
    {
        [SerializeField]
        GameObject gameObject;
        public GameObject GameObject
        {
            get { return this.gameObject; }
        }

        [SerializeField]
        Transform transform;
        public Transform Transform
        {
            get { return this.transform; }
        }

        [SerializeField]
        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get { return this.rectTransform; }
        }

        public BaseComponent(GameObject target)
        {
            this.Assign(target);
        }

        public BaseComponent(Transform targetTransform)
        {
            GameObject go = targetTransform ? targetTransform.gameObject : null;
            this.Assign(go);
        }

        public static implicit operator bool(BaseComponent thiz)
        {
            return thiz != null;
        }

        public virtual void Assign(GameObject target)
        {
            if (target != null)
            {
                this.gameObject = target;
                this.transform = target.transform;
                this.rectTransform = target.GetComponent<RectTransform>();
            }
            else
            {
                this.gameObject = null;
                this.transform = null;
                this.rectTransform = null;
            }
        }
    }
}
