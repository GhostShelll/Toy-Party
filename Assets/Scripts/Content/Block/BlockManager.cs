using System.Collections.Generic;

using UnityEngine;

using com.jbg.core;

namespace com.jbg.content.block
{
    public class BlockManager : ComponentEx
    {
        private const string BLOCK_BASE_PATH = "Sprites/Blocks/";

        public enum Color
        {
            Blue,
            Green,
            Orange,
            Purple,
            Red,
            Yellow,
        };

        public enum Special
        {
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

        public void OnOpen()
        {
            Transform cached = this.CachedTransform;

            this.blockMap = new BlockCell[cached.childCount][];
            for (int i = 0; i < cached.childCount; i++)
            {
                Transform child = cached.GetChild(i);

                this.blockMap[i] = new BlockCell[child.childCount];

                for (int j = 0; j < child.childCount; j++)
                    this.blockMap[i][j] = child.GetChild(j).FindComponent<BlockCell>();
            }
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            //Transform cached = this.CachedTransform;
            //Transform t;

            this.normalBlockSprites = new();
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_b"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_g"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_o"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_p"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_r"));
            this.normalBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_y"));

            this.packBlockSprites = new();
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_b_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_g_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_o_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_p_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_r_pack"));
            this.packBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_y_pack"));

            this.blockLineSprites = new();
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_b_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_g_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_o_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_p_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_r_vertical_line"));
            this.blockLineSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_y_vertical_line"));

            this.specialBlockSprites = new();
            this.specialBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_z_munchkin"));
            this.specialBlockSprites.Add(Resources.Load<Sprite>(BlockManager.BLOCK_BASE_PATH + "blk_z_rainbow"));
        }
#endif  // UNITY_EDITOR
    }
}
