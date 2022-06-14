using UnityEngine;

namespace com.jbg.core
{
    public class ComponentEx : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        bool setComponent = false;
        [SerializeField]
        bool setComponentChild = false;
#endif  // UNITY_EDITOR

        private Transform cachedTransform = null;

        public virtual void Active()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Inactive()
        {
            this.gameObject.SetActive(false);
        }

        public bool IsActive
        {
            get
            {
                if (this.gameObject != null)
                    return this.gameObject.activeSelf;
                else
                    return false;
            }
        }

        public Transform CachedTransform
        {
            get
            {
                this.InitCachedTransform();

                return this.cachedTransform;
            }
        }

        private void InitCachedTransform()
        {
            if (this.cachedTransform != null)
                return;

            this.cachedTransform = this.transform;
        }

        public virtual void Show()
        {
            this.cachedTransform.ShowTransform();
        }

        public virtual void Hide()
        {
            this.cachedTransform.HideTransform();
        }

        public bool IsShow
        {
            get
            {
                return this.cachedTransform.IsHideTransform() == false;
            }
        }

        public RectTransform GetRectTransform
        {
            get
            {
                return this.GetComponent<RectTransform>();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (this.setComponent)
            {
                this.setComponent = false;

                this.SetComponent();
            }

            if (this.setComponentChild)
            {
                this.setComponentChild = false;

                ComponentEx[] childs = this.cachedTransform.GetComponentsInChildren<ComponentEx>();
                for (int i = 0; i < childs.Length; i++)
                {
                    ComponentEx child = childs[i];
                    if (child != null)
                        child.SetComponent();
                }
            }

            this.OnDrawGizmosEx();
        }

        public void SetComponent()
        {
            this.OnSetComponent();

            UnityEx.SetDirtyAll(this.cachedTransform);
        }

        protected virtual void OnSetComponent() { }
        protected virtual void OnDrawGizmosEx() { }
#endif  // UNITY_EDITOR
    }
}
