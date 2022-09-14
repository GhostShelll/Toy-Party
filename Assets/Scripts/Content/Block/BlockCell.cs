using UnityEngine;
using UnityEngine.UI;

using com.jbg.core;

namespace com.jbg.content.block
{
    using Manager = BlockManager;

    public class BlockCell : ComponentEx
    {
        [Header("Block Cell")]
        [SerializeField]
        Image imgBg;
        [SerializeField]
        Image imgDebug;
        [SerializeField]
        Image imgDebugForward;

        private int colIndex;
        private int rowIndox;

        private bool isEnable;
        public bool IsEnable { get { return this.isEnable; } }

        private BlockCell[] surroundCells;  // 0:좌상단, 1:상단, 2:우상단, 3:좌하단, 4:하단, 5:우하단
        private Block block;

        private bool isChanged;
        public bool IsChanged { get { return this.isChanged; } }

        private bool isChecked;
        public bool IsChecked { get { return this.isChecked; } }

        public string GetName()
        {
            return string.Format("{0}-{1}", this.colIndex, this.rowIndox);
        }

        public void SetSurroundCells(BlockCell[] surroundCells)
        {
            this.surroundCells = surroundCells;
        }

        public void Initialize(int col, int row, bool isEnable)
        {
            this.colIndex = col;
            this.rowIndox = row;
            this.isEnable = isEnable;

            if (isEnable)
            {
                this.block = Manager.Instance.LoadBlock(this);

                this.isChanged = true;
                this.isChecked = false;

                Manager.BlkColor color = Manager.Instance.GetRandomColor();
                Sprite blkImg = Manager.Instance.GetNormalSprite(color);

                this.block.Initialize(color, Manager.BlkType.Normal, blkImg);

                this.imgBg.canvasRenderer.SetAlpha(1f);
#if LOG_DEBUG
                this.imgDebug.enabled = true;
                this.imgDebug.sprite = blkImg;
                this.imgDebugForward.enabled = false;
#else   // LOG_DEBUG
                this.imgDebug.enabled = false;
                this.imgDebugForward.enabled = false;
#endif  // LOG_DEBUG
            }
            else
            {
                this.block = null;

                this.isChanged = false;
                this.isChecked = true;

                this.imgBg.canvasRenderer.SetAlpha(0.2f);

                this.imgDebug.enabled = false;
                this.imgDebugForward.enabled = false;
            }
        }

        public void CheckMatch()
        {
            this.isChecked = true;
        }

        public void SetBlock(Manager.BlkColor color, Sprite mainImg, Manager.BlkType type, Sprite forwardImg)
        {
#if LOG_DEBUG
            this.imgDebug.sprite = mainImg;

            this.imgDebugForward.enabled = forwardImg != null;
            if (forwardImg != null)
            {
                this.imgDebugForward.sprite = forwardImg;

                // 앞 레이어의 이미지 회전
                switch (type)
                {
                    case Manager.BlkType.Normal:
                    case Manager.BlkType.Pack:
                        break;

                    case Manager.BlkType.Line6to12: this.imgDebugForward.transform.localEulerAngles = Vector3.zero; break;
                    case Manager.BlkType.Line1to7: this.imgDebugForward.transform.localEulerAngles = new Vector3(0f, 0f, -45f); break;
                    case Manager.BlkType.Line3to9: this.imgDebugForward.transform.localEulerAngles = new Vector3(0f, 0f, 90f); break;
                    case Manager.BlkType.Line5to11: this.imgDebugForward.transform.localEulerAngles = new Vector3(0f, 0f, 45f); break;

                    case Manager.BlkType.UFO:
                    case Manager.BlkType.Turtle:
                        break;
                }
            }
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
