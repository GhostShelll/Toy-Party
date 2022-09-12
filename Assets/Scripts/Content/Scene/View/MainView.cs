using UnityEngine;

using com.jbg.core;
using com.jbg.core.scene;
using com.jbg.content.block;

namespace com.jbg.content.scene.view
{
    public class MainView : SceneView
    {
        public class Params
        {

        }

        private Params paramBuffer = null;
        public Params ParamBuffer { get { return this.paramBuffer; } }

        [Header("Main View")]
        [SerializeField]
        BlockManager blockManager;

        public void OnOpen(Params p)
        {
            this.paramBuffer = p;

            this.blockManager.OnOpen();
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform.Find("Canvas/Padding");
            Transform t;

            t = cached.Find("Center");
            this.blockManager = t.FindComponent<BlockManager>("BlockManager");
        }
#endif  // UNITY_EDITOR
    }
}
