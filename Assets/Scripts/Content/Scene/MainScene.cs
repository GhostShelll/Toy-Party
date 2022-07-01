using com.jbg.content.popup;
using com.jbg.core.scene;

namespace com.jbg.content.scene
{
    public class MainScene : SceneEx
    {
        // TODO[jbg] : 필요한 것만 사용해야함

        protected override void OnOpen()
        {
            base.OnOpen();

            LottoPopupAssist.Open(() =>
            {

            });
        }
    }
}
