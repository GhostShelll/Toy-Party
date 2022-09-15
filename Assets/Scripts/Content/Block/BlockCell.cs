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

        private int colIndex;
        private int rowIndox;

        private bool isEnable;
        public bool IsEnable { get { return this.isEnable; } }

        private bool isNeedDestroy;
        public bool IsNeedDestroy { get { return this.isNeedDestroy; } set { this.isNeedDestroy = value; } }

        private Block block;
        public Block Block { get { return this.block; } set { this.block = value; } }
        public bool IsEmpty { get { return this.block == null; } }

        private BlockCell[] surroundCells;  // 0:좌상단, 1:상단, 2:우상단, 3:좌하단, 4:하단, 5:우하단

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
            this.isNeedDestroy = false;

            if (isEnable)
            {
                this.block = Manager.Instance.LoadBlock(this);

                Manager.BlkColor color = Manager.Instance.GetRandomColor();
                Sprite blkImg = Manager.Instance.GetNormalSprite(color);

                this.block.Initialize(color, Manager.BlkType.Normal, blkImg);

                this.imgBg.canvasRenderer.SetAlpha(1f);
            }
            else
            {
                this.block = null;

                this.imgBg.canvasRenderer.SetAlpha(0.2f);
            }
        }

        public void CheckMatch()
        {
            Manager.BlkColor thisColor = this.block.Color;

            // 현재의 Cell을 중점으로 두고 세 방향으로 검사하여 3개가 매칭되었는지 검사
            BlockCell cellLT = this.surroundCells[0];
            BlockCell cellT = this.surroundCells[1];
            BlockCell cellRT = this.surroundCells[2];
            BlockCell cellLB = this.surroundCells[3];
            BlockCell cellB = this.surroundCells[4];
            BlockCell cellRB = this.surroundCells[5];

            // 북서-남동 방향 매칭 검사
            if (cellLT != null && cellRB != null)
            {
                bool isMatch = cellLT.IsSameColor(thisColor) && cellRB.IsSameColor(thisColor);
                if (isMatch)
                    cellLT.IsNeedDestroy = cellRB.IsNeedDestroy = true;

                this.isNeedDestroy |= isMatch;
            }

            // 북-남 방향 매칭 검사
            if (cellT != null && cellB != null)
            {
                bool isMatch = cellT.IsSameColor(thisColor) && cellB.IsSameColor(thisColor);
                if (isMatch)
                    cellT.IsNeedDestroy = cellB.IsNeedDestroy = true;

                this.isNeedDestroy |= isMatch;
            }

            // 북동-남서 방향 매칭 검사
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
            // 블럭 파괴하기
            if (this.block != null)
                this.block.DoDestroy();

            this.isNeedDestroy = false;
            this.block = null;
        }

        public bool ProcessBlockMove()
        {
            // 상단에 위치한 Cell 중에서 북, 북서, 북동 순으로 검사
            BlockCell[] topCells = new BlockCell[3] { this.surroundCells[1], this.surroundCells[0], this.surroundCells[2] };

            for (int i = 0; i < topCells.Length; i++)
            {
                BlockCell cell = topCells[i];

                if (cell != null && cell.IsEnable && cell.IsEmpty == false)
                {
                    this.SetBlock(cell.Block);
                    cell.Block = null;
                    cell.ProcessBlockMove();

                    return true;
                }
            }

            bool nonTopCell = true;

            for (int i = 0; i < topCells.Length; i++)
            {
                BlockCell cell = topCells[i];

                if (cell != null && cell.IsEnable)
                {
                    nonTopCell = false;
                    break;
                }
            }

            if (nonTopCell)
            {
                this.block = Manager.Instance.LoadBlock(this);

                Manager.BlkColor color = Manager.Instance.GetRandomColor();
                Sprite blkImg = Manager.Instance.GetNormalSprite(color);

                this.block.Initialize(color, Manager.BlkType.Normal, blkImg);

                return true;
            }

            return false;
        }

        public void SetBlock(Block block)
        {
            this.block = block;

            Transform trans = this.block.transform;

            trans.SetParent(this.transform);
            trans.name = this.GetName();
            trans.localPosition = Vector3.zero;
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform;
            Transform t;

            t = cached.Find("Bg");
            this.imgBg = t.GetComponent<Image>();
        }
#endif  // UNITY_EDITOR
    }
}
