using UnityEngine;
using UnityEngine.UI;

using com.jbg.core;

namespace com.jbg.content.block
{
    public class BlockCell : ComponentEx
    {
        [Header("Block Cell")]
        [SerializeField]
        Image imgBg;
        [SerializeField]
        Image imgDebug;
        [SerializeField]
        Image imgDebugForward;

        private void Awake()
        {
#if LOG_DEBUG
            this.imgDebug.transform.parent.gameObject.SetActive(true);
#else
            this.imgDebug.transform.parent.gameObject.SetActive(false);
#endif  // LOG_DEBUG
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform;
            Transform t;

            t = cached.Find("Bg");
            this.imgBg = t.GetComponent<Image>();

            t = cached.Find("DebugImage");
            this.imgDebug = t.FindComponent<Image>("Main");
            this.imgDebugForward = t.FindComponent<Image>("Forward");
        }
#endif  // UNITY_EDITOR
    }
}
