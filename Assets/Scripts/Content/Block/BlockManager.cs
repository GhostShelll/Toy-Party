using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using com.jbg.core;

namespace com.jbg.content.block
{
    public class BlockManager : ComponentEx
    {
        private const string BLOCK_PREFAB_PATH = "Prefabs/Blocks/Block";
        private const string BLOCK_IMAGE_PATH = "Sprites/Blocks/";

        private const string TEST_MAP_INFO = "1-6,1-7,1-8,2-5,2-6,2-7,2-8,3-5,3-6,3-7,3-8,3-9,4-4,4-5,4-6,4-7,4-8,4-9,5-5,5-6,5-7,5-8,5-9,6-5,6-6,6-7,6-8,7-6,7-7,7-8";

        public static BlockManager Instance { get; private set; }

        public enum BlkColor
        {
            Blue = 0,
            Green,
            Orange,
            Purple,
            Red,
            Yellow,
        };

        public enum BlkType
        {
            Normal = 0,
            Pack,
            Line6to12,
            Line1to7,
            Line3to9,
            Line5to11,
            UFO,
            Turtle,
        };

        [Header("Block Manager")]
        [SerializeField]
        List<Sprite> normalBlockSprites;
        [SerializeField]
        List<Sprite> packBlockSprites;
        [SerializeField]
        List<Sprite> blockLineSprites;

        [SerializeField]
        List<Sprite> specialBlockSprites;

        private BlockCell[][] blockMap;

        private void Awake()
        {
            BlockManager.Instance = this;
        }

        private void OnDestroy()
        {
            BlockManager.Instance = null;
        }

        public Block LoadBlock(BlockCell cell)
        {
            string cellName = cell.GetName();
            DebugEx.Log("BLOCK_LOAD:" + cellName);

            if (this == null)
            {
                DebugEx.Log("BLOCK_LOAD_FAILED:" + cellName + ", MANAGER_IS_NULL");
                return null;
            }

            string blockPath = BlockManager.BLOCK_PREFAB_PATH;

            Block prefab = Resources.Load<Block>(blockPath);
            if (prefab == null)
            {
                DebugEx.Log("BLOCK_LOAD_FAILED:" + cellName + ", PATH:" + blockPath + ", RESOURCES_LOAD_FAILED");
                return null;
            }

            GameObject blockObj = GameObject.Instantiate(prefab.gameObject);
            if (blockObj == null)
            {
                DebugEx.Log("BLOCK_LOAD_FAILED:" + cellName + ", PATH:" + blockPath + ", GAMEOBJECT_INSTANTIATE_FAILED");

                Object.Destroy(prefab);
                return null;
            }

            Block block = blockObj.GetComponent<Block>();
            if (block == null)
            {
                DebugEx.Log("BLOCK_LOAD_FAILED:" + cellName + ", PATH:" + blockPath + ", GETCOMPONENT_FAILED");

                GameObject.Destroy(blockObj);
                Object.Destroy(prefab);
                return null;
            }

            RectTransform trans = block.CachedRectTransform;
            RectTransform transPrefab = prefab.CachedRectTransform;
            trans.SetParent(cell.transform);
            trans.name = cellName;
            trans.anchoredPosition = transPrefab.anchoredPosition;
            trans.localPosition = transPrefab.localPosition;
            trans.localScale = transPrefab.localScale;
            trans.offsetMax = transPrefab.offsetMax;
            trans.offsetMin = transPrefab.offsetMin;

            return block;
        }

        public BlkColor GetRandomColor()
        {
            System.Random random = new();

            System.Array colorArray = System.Enum.GetValues(typeof(BlkColor));
            BlkColor result = (BlkColor)colorArray.GetValue(random.Next(colorArray.Length));

            return result;
        }

        public Sprite GetNormalSprite(BlkColor color)
        {
            if (this.normalBlockSprites == null)
                return null;

            int index = (int)color;
            if (index < 0 || this.normalBlockSprites.Count <= index)
                return null;

            return this.normalBlockSprites[(int)color];
        }

        public Sprite GetPackSprite(BlkColor color)
        {
            if (this.packBlockSprites == null)
                return null;

            int index = (int)color;
            if (index < 0 || this.packBlockSprites.Count <= index)
                return null;

            return this.packBlockSprites[(int)color];
        }

        public Sprite GetLineSprite(BlkColor color)
        {
            if (this.blockLineSprites == null)
                return null;

            int index = (int)color;
            if (index < 0 || this.blockLineSprites.Count <= index)
                return null;

            return this.blockLineSprites[(int)color];
        }

        public void Initialize()
        {
            Transform cached = this.CachedTransform;

            List<string> mapInfo = BlockManager.TEST_MAP_INFO.Split(',').ToList();

            this.blockMap = new BlockCell[cached.childCount][];
            for (int i = 0; i < cached.childCount; i++)
            {
                Transform child = cached.GetChild(i);

                this.blockMap[i] = new BlockCell[child.childCount];

                for (int j = 0; j < child.childCount; j++)
                {
                    string cellName = string.Format("{0}-{1}", i, j);
                    bool isEnableCell = mapInfo.Contains(cellName);

                    this.blockMap[i][j] = child.GetChild(j).FindComponent<BlockCell>();
                    this.blockMap[i][j].Initialize(i, j, isEnableCell);
                }
            }

            for (int i = 0; i < this.blockMap.Length; i++)
            {
                BlockCell[] rows = this.blockMap[i];

                bool firstCol = i == 0;
                bool lastCol = i == this.blockMap.Length - 1;

                for (int j = 0; j < rows.Length; j++)
                {
                    BlockCell cell = rows[j];
                    BlockCell[] surroundCells = new BlockCell[6];

                    bool firstRow = j == 0;
                    bool lastRow = j == rows.Length - 1;

                    if (i % 2 == 0)
                    {
                        surroundCells[0] = firstCol ? null : this.blockMap[i - 1][j];
                        surroundCells[1] = firstRow ? null : this.blockMap[i][j - 1];
                        surroundCells[2] = lastCol ? null : this.blockMap[i + 1][j];
                        surroundCells[3] = firstCol ? null : this.blockMap[i - 1][j + 1];
                        surroundCells[4] = lastRow ? null : this.blockMap[i][j + 1];
                        surroundCells[5] = lastCol ? null : this.blockMap[i + 1][j + 1];
                    }
                    else
                    {
                        surroundCells[0] = firstRow ? null : this.blockMap[i - 1][j - 1];
                        surroundCells[1] = firstRow ? null : this.blockMap[i][j - 1];
                        surroundCells[2] = firstRow ? null : this.blockMap[i + 1][j - 1];
                        surroundCells[3] = lastRow ? null : this.blockMap[i - 1][j];
                        surroundCells[4] = lastRow ? null : this.blockMap[i][j + 1];
                        surroundCells[5] = lastRow ? null : this.blockMap[i + 1][j];
                    }

                    cell.SetSurroundCells(surroundCells);
                }
            }
        }

        public void CheckMatch()
        {
            for (int i = 0; i < this.blockMap.Length; i++)
            {
                BlockCell[] columns = this.blockMap[i];

                for (int j = 0; j < columns.Length; j++)
                {
                    BlockCell cell = columns[j];
                    if (cell.IsEnable == false)
                        continue;

                    cell.CheckMatch();
                }
            }
        }

        public bool DestroyMatched()
        {
            bool cellDestroyed = false;

            for (int i = 0; i < this.blockMap.Length; i++)
            {
                BlockCell[] columns = this.blockMap[i];

                for (int j = 0; j < columns.Length; j++)
                {
                    BlockCell cell = columns[j];
                    if (cell.IsEnable == false)
                        continue;
                    if (cell.IsNeedDestroy == false)
                        continue;

                    cell.DestroyMatched();

                    if (cellDestroyed == false)
                        cellDestroyed = true;
                }
            }

            return cellDestroyed;
        }

        public bool ProcessBlockMove()
        {
            bool isMoved = false;

            for (int i = 0; i < this.blockMap.Length; i++)
            {
                BlockCell[] columns = this.blockMap[i];

                for (int j = 0; j < columns.Length; j++)
                {
                    BlockCell cell = columns[j];
                    if (cell.IsEnable == false)
                        continue;
                    if (cell.IsEmpty == false)
                        continue;

                    bool oneStep = cell.ProcessBlockMove();
                    isMoved |= oneStep;

                    if (oneStep)
                        break;
                }
            }

            return isMoved;
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            this.normalBlockSprites = new();
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_b"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_g"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_o"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_p"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_r"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_y"));

            this.packBlockSprites = new();
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_b_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_g_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_o_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_p_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_r_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_y_pack"));

            this.blockLineSprites = new();
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_b_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_g_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_o_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_p_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_r_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_y_vertical_line"));

            this.specialBlockSprites = new();
            this.specialBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_z_munchkin"));
            this.specialBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_IMAGE_PATH + "blk_z_rainbow"));
        }
#endif  // UNITY_EDITOR
    }
}
