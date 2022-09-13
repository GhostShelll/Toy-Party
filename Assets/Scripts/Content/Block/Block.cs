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

        private BlockManager.BlkColor color;
        public BlockManager.BlkColor Color { get { return this.color; } }
        private BlockManager.BlkType type;
        public BlockManager.BlkType Type { get { return this.type; } }

        public void Initialize(BlockManager.BlkColor color, BlockManager.BlkType type, Sprite mainImg)
        {
            this.imgMain.sprite = mainImg;
            this.imgForward.enabled = false;

            this.color = color;
            this.type = type;
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
