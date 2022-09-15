using System.Collections.Generic;

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
        Image imgHighlight;

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

        private System.Action<string> onClickCallback;

        public string GetName()
        {
            return string.Format("{0}-{1}", this.colIndex, this.rowIndox);
        }

        public void SetBlock(Block block)
        {
            this.block = block;

            Transform trans = this.block.transform;

            trans.SetParent(this.transform);
            trans.name = this.GetName();
            trans.localPosition = Vector3.zero;
        }

        public void SetNewBlock()
        {
            this.block = Manager.Instance.LoadBlock(this);

            Manager.BlkColor color = Manager.Instance.GetRandomColor();
            Sprite blkImg = Manager.Instance.GetNormalSprite(color);

            this.block.Initialize(color, Manager.BlkType.Normal, blkImg);
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

        public bool IsSurroundCell(string cellName)
        {
            bool result = false;

            for (int i = 0; i < this.surroundCells.Length; i++)
            {
                BlockCell cell = this.surroundCells[i];
                if (cell == null || cell.IsEnable == false)
                    continue;

                result = cell.GetName().Equals(cellName);
                if (result)
                    break;
            }

            return result;
        }

        public void SetHighlight(bool isEnable)
        {
            this.imgHighlight.enabled = isEnable;
        }

        public void Initialize(int col, int row, bool isEnable, System.Action<string> onClickCallback)
        {
            this.colIndex = col;
            this.rowIndox = row;
            this.isEnable = isEnable;
            this.isNeedDestroy = false;
            this.onClickCallback = onClickCallback;

            if (isEnable)
            {
                this.SetNewBlock();

                this.imgBg.canvasRenderer.SetAlpha(1f);
            }
            else
            {
                this.block = null;

                this.imgBg.canvasRenderer.SetAlpha(0.2f);
            }

            this.imgHighlight.enabled = false;
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

        public void DestroyBlock()
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
                this.SetNewBlock();

                return true;
            }

            return false;
        }

        public bool CheckMatchPossible()
        {
            Manager.BlkColor thisColor = this.block.Color;

            List<int> matchIndex = new();
            for (int i = 0; i < this.surroundCells.Length; i++)
            {
                BlockCell cell = this.surroundCells[i];
                if (cell == null)
                    continue;
                if (cell.IsEnable == false || cell.IsEmpty)
                    continue;

                if (cell.IsSameColor(thisColor))
                    matchIndex.Add(i);
            }

            if (matchIndex.Count > 2)   // 주위의 같은 색 블럭이 3개 이상이면 모든 상황 매칭 가능함
                return true;

            if (matchIndex.Count == 2)
            {
                // 중앙 블럭 기준으로 주위의 같은색 블럭은 2개뿐 일 때
                // 총 3개의 블럭이 삼각형 모양으로 뭉친 경우 매칭 불가함
                if (matchIndex.Contains(0) && matchIndex.Contains(1))       // 북서-북
                    return false;
                else if (matchIndex.Contains(1) && matchIndex.Contains(2))  // 북-북동
                    return false;
                else if (matchIndex.Contains(2) && matchIndex.Contains(5))  // 북동-남동
                    return false;
                else if (matchIndex.Contains(5) && matchIndex.Contains(4))  // 남동-남
                    return false;
                else if (matchIndex.Contains(4) && matchIndex.Contains(3))  // 남-남서
                    return false;
                else if (matchIndex.Contains(3) && matchIndex.Contains(0))  // 남서-북서
                    return false;
                else
                    return true;    // 위 예외를 제외한 모든 상황은 매칭 가능함
            }

            return false;
        }

        public void OnClickBlockCell()
        {
            this.onClickCallback?.Invoke(this.GetName());
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform;
            Transform t;

            t = cached.Find("Bg");
            this.imgBg = t.GetComponent<Image>();

            t = cached.Find("Highlight");
            this.imgHighlight = t.GetComponent<Image>();
        }
#endif  // UNITY_EDITOR
    }
}
