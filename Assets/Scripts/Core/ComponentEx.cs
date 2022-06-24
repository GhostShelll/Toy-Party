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

        [SerializeField]
        RectTransform cachedRectTransform;

        public void Active()
        {
            this.gameObject.SetActive(true);
        }

        public void Inactive()
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
                return this.transform;
            }
        }

        public RectTransform CachedRectTransform
        {
            get
            {
                return this.cachedRectTransform;
            }
        }

        public void Show()
        {
            this.transform.ShowTransform();
        }

        public void Hide()
        {
            this.transform.HideTransform();
        }

        public bool IsShow
        {
            get
            {
                return this.transform.IsHideTransform() == false;
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

                ComponentEx[] childs = this.transform.GetComponentsInChildren<ComponentEx>();
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
            this.cachedRectTransform = this.GetComponent<RectTransform>();

            this.OnSetComponent();

            UnityEx.SetDirtyAll(this.transform);
        }

        protected virtual void OnSetComponent() { }
        protected virtual void OnDrawGizmosEx() { }
#endif  // UNITY_EDITOR
    }
}
