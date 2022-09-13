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

        private BlockManager.Color color;
        public BlockManager.Color Color { get { return this.color; } }
        private BlockManager.Type type;
        public BlockManager.Type Type { get { return this.type; } }

        public void SetImage(BlockManager.Color color, Sprite mainImg, BlockManager.Type type, Sprite forwardImg)
        {
            this.color = color;
            this.imgMain.sprite = mainImg;

            this.type = type;
            this.imgForward.enabled = forwardImg != null;
            if (forwardImg != null)
            {
                this.imgForward.sprite = forwardImg;

                // 앞 레이어의 이미지 회전
                switch (this.type)
                {
                    case BlockManager.Type.Normal:
                    case BlockManager.Type.Pack:
                        break;
                    case BlockManager.Type.Line6to12: this.imgForward.transform.localEulerAngles = Vector3.zero; break;
                    case BlockManager.Type.Line1to7: this.imgForward.transform.localEulerAngles = new Vector3(0f, 0f, -45f); break;
                    case BlockManager.Type.Line3to9: this.imgForward.transform.localEulerAngles = new Vector3(0f, 0f, 90f); break;
                    case BlockManager.Type.Line5to11: this.imgForward.transform.localEulerAngles = new Vector3(0f, 0f, 45f); break;
                    case BlockManager.Type.UFO:
                    case BlockManager.Type.Turtle:
                        break;
                }
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
