using UnityEngine;

namespace com.jbg.core.scene
{
    public class SceneView : View
    {
        [SerializeField]
        Canvas[] canvasArr;

        public Canvas[] CanvasArr { get { return this.canvasArr; } }

        public void ShowCanvases()
        {
            if (this.canvasArr == null)
                return;

            foreach (Canvas canvas in this.canvasArr)
            {
                CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1;
                    canvasGroup.blocksRaycasts = true;
                }
                else
                {
                    canvas.enabled = true;
                }
            }
        }

        public void HideCanvases()
        {
            if (this.canvasArr == null)
                return;

            foreach (Canvas canvas in this.canvasArr)
            {
                CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.alpha = 0;
                }
                else
                {
                    canvas.enabled = false;
                }
            }
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            this.canvasArr = this.gameObject.GetComponentsInChildren<Canvas>();
        }
#endif  // UNITY_EDITOR
    }
}
