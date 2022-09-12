using UnityEngine;
using UnityEngine.UI;

using com.jbg.core;

namespace com.jbg.content.block
{
    public class Block : ComponentEx
    {
        [Header("Block")]
        [SerializeField]
        Image imgMain;
        [SerializeField]
        Image imgForward;

        public void SetImage(Sprite mainImg, Sprite forwardImg)
        {
            this.imgMain.sprite = mainImg;

            this.imgForward.enabled = forwardImg != null;
            if (forwardImg != null)
            {
                this.imgForward.sprite = forwardImg;

                // TODO[jbg] : 회전 시켜야함
            }
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform;
            Transform t;

            t = cached.Find("ImageMain");
            this.imgMain = t.GetComponent<Image>();

            t = cached.Find("ImageForward");
            this.imgForward = t.GetComponent<Image>();
        }
#endif  // UNITY_EDITOR
    }
}
