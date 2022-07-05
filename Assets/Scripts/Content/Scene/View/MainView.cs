using UnityEngine;

using com.jbg.core;
using com.jbg.core.scene;

namespace com.jbg.content.scene.view
{
    public class MainView : SceneView
    {
        public enum Event
        {
            LottoSelect,
        };

        public class Params
        {
            public string lottoBtnTxt;
        }

        [Header("Main View")]
        [SerializeField]
        ButtonComponent lottoBtn;

        public void OnOpen(Params p)
        {
            this.lottoBtn.Text = p.lottoBtnTxt;
        }

        public void OnClickLottoSelect()
        {
            this.DoEvent(Event.LottoSelect);
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            Transform cached = this.CachedTransform.Find("Canvas/Padding");
            Transform t;

            t = cached.Find("BtnLottoSelect");
            this.lottoBtn = new ButtonComponent(t);
        }
#endif  // UNITY_EDITOR
    }
}
