using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using com.jbg.core;
using com.jbg.core.scene;

namespace com.jbg.content.scene.view
{
    public class MainView : SceneView
    {
        public enum Event
        {

        };

        public class Params
        {

        }

        private Params paramBuffer = null;
        public Params ParamBuffer { get { return this.paramBuffer; } }

        //[Header("Main View")]

        public void OnOpen(Params p)
        {
            this.paramBuffer = p;
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform.Find("Canvas/Padding");
            Transform t;

            //t = cached.Find("BtnRefresh");
            //this.refreshBtn = new ButtonComponent(t);
        }
#endif  // UNITY_EDITOR
    }
}
