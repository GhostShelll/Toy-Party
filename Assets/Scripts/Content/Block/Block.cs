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

        private bool isDestroy;

        public void Initialize(BlockManager.BlkColor color, BlockManager.BlkType type, Sprite mainImg)
        {
            this.imgMain.sprite = mainImg;
            this.imgForward.enabled = false;

            this.color = color;
            this.type = type;
            this.isDestroy = false;
        }

        public void DoDestroy()
        {
            if (this.isDestroy)
                return;
            this.isDestroy = true;

            GameObject.Destroy(this.gameObject);
        }

        //// 앞 레이어의 이미지 회전
        //switch (type)
        //{
        //    case Manager.BlkType.Normal:
        //    case Manager.BlkType.Pack:
        //        break;

        //    case Manager.BlkType.Line6to12: this.imgDebugForward.transform.localEulerAngles = Vector3.zero; break;
        //    case Manager.BlkType.Line1to7: this.imgDebugForward.transform.localEulerAngles = new Vector3(0f, 0f, -45f); break;
        //    case Manager.BlkType.Line3to9: this.imgDebugForward.transform.localEulerAngles = new Vector3(0f, 0f, 90f); break;
        //    case Manager.BlkType.Line5to11: this.imgDebugForward.transform.localEulerAngles = new Vector3(0f, 0f, 45f); break;

        //    case Manager.BlkType.UFO:
        //    case Manager.BlkType.Turtle:
        //        break;
        //}

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
