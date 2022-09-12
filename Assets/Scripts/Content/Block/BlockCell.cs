using UnityEngine;
using UnityEngine.UI;

using com.jbg.core;

namespace com.jbg.content.block
{
    public class BlockCell : ComponentEx
    {
        [SerializeField]
        Image bgImg;
        [SerializeField]
        Image debugImg;
        [SerializeField]
        Image debugForwardImg;

        private void Awake()
        {
#if LOG_DEBUG
            this.debugImg.transform.parent.gameObject.SetActive(true);
#else
            this.debugImg.transform.parent.gameObject.SetActive(false);
#endif  // LOG_DEBUG
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform;
            Transform t;

            t = cached.Find("Bg");
            this.bgImg = t.GetComponent<Image>();

            t = cached.Find("DebugImg");
            this.debugImg = t.FindComponent<Image>("Img");
            this.debugForwardImg = t.FindComponent<Image>("Img_Forward");
        }
#endif  // UNITY_EDITOR
    }
}
