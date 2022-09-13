using System.Collections.Generic;

using UnityEngine;

using com.jbg.core;

namespace com.jbg.content.block
{
    public class BlockManager : ComponentEx
    {
        private const string BLOCK_PREFAB_PATH = "Prefabs/Blocks/Block";
        private const string BLOCK_IMAGE_PATH = "Sprites/Blocks/";

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

            this.blockMap = new BlockCell[cached.childCount][];
            for (int i = 0; i < cached.childCount; i++)
            {
                Transform child = cached.GetChild(i);

                this.blockMap[i] = new BlockCell[child.childCount];

                for (int j = 0; j < child.childCount; j++)
                {
                    this.blockMap[i][j] = child.GetChild(j).FindComponent<BlockCell>();
                    this.blockMap[i][j].Initialize(i, j);
                }
            }
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
