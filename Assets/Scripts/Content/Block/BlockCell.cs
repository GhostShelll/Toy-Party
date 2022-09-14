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

        private bool isChecked;
        public bool IsChecked { get { return this.isChecked; } }

        private bool isNeedDestroy;
        public bool IsNeedDestroy { get { return this.isNeedDestroy; } set { this.isNeedDestroy = value; } }

        private Block block;
        public Block Block { get { return this.block; } }

        private BlockCell[] surroundCells;  // 0:�»��, 1:���, 2:����, 3:���ϴ�, 4:�ϴ�, 5:���ϴ�

        public string GetName()
        {
            return string.Format("{0}-{1}", this.colIndex, this.rowIndox);
        }

        public bool IsSameColor(BlockManager.BlkColor color)
        {
            if (this.block == null)
                return false;

            return this.block.Color == color;
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
            this.isChecked = false;
            this.isNeedDestroy = false;

            if (isEnable)
            {
                this.block = Manager.Instance.LoadBlock(this);

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

                this.imgBg.canvasRenderer.SetAlpha(0.2f);

                this.imgDebug.enabled = false;
                this.imgDebugForward.enabled = false;
            }
        }

        public void CheckMatch()
        {
            Manager.BlkColor thisColor = this.block.Color;

            this.isChecked = true;

            // ������ Cell�� �������� �ΰ� �� �������� �˻��Ͽ� 3���� ��Ī�Ǿ����� �˻�
            BlockCell cellLT = this.surroundCells[0];
            BlockCell cellT = this.surroundCells[1];
            BlockCell cellRT = this.surroundCells[2];
            BlockCell cellLB = this.surroundCells[3];
            BlockCell cellB = this.surroundCells[4];
            BlockCell cellRB = this.surroundCells[5];

            // �ϼ�-���� ���� ��Ī �˻�
            if (cellLT != null && cellRB != null)
            {
                bool isMatch = cellLT.IsSameColor(thisColor) && cellRB.IsSameColor(thisColor);
                if (isMatch)
                    cellLT.IsNeedDestroy = cellRB.IsNeedDestroy = true;

                this.isNeedDestroy |= isMatch;
            }

            // ��-�� ���� ��Ī �˻�
            if (cellT != null && cellB != null)
            {
                bool isMatch = cellT.IsSameColor(thisColor) && cellB.IsSameColor(thisColor);
                if (isMatch)
                    cellT.IsNeedDestroy = cellB.IsNeedDestroy = true;

                this.isNeedDestroy |= isMatch;
            }

            // �ϵ�-���� ���� ��Ī �˻�
            if (cellRT != null && cellLB != null)
            {
                bool isMatch = cellRT.IsSameColor(thisColor) && cellLB.IsSameColor(thisColor);
                if (isMatch)
                    cellRT.IsNeedDestroy = cellLB.IsNeedDestroy = true;

                this.isNeedDestroy |= isMatch;
            }
        }

        public void DestroyMatched()
        {
            // �� �ı��ϱ�
            if (this.block != null)
                this.block.DoDestroy();

            this.block = null;

            this.imgDebug.enabled = false;
            this.imgDebugForward.enabled = false;
        }

        public void SetBlock(Manager.BlkColor color, Sprite mainImg, Manager.BlkType type, Sprite forwardImg)
        {
#if LOG_DEBUG
            this.imgDebug.sprite = mainImg;

            this.imgDebugForward.enabled = forwardImg != null;
            if (forwardImg != null)
            {
                this.imgDebugForward.sprite = forwardImg;

                // �� ���̾��� �̹��� ȸ��
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
