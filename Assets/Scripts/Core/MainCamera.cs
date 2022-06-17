using UnityEngine;

namespace com.jbg.core
{
    public class MainCamera : ComponentEx
    {
        [SerializeField]
        Camera mainCamera;

        [SerializeField]
        bool autoResize;

        [SerializeField]
        int manualWidth;

        [SerializeField]
        int manualHeight;

#if UNITY_EDITOR
        [SerializeField]
        bool editorReset = false;
#endif  // UNITY_EDITOR

        // Singleton
        private MainCamera() { }
        private static readonly System.Lazy<MainCamera> instance = new(() => new());
        public static MainCamera Instance { get { return instance.Value; } }

        public static Camera Camera
        {
            get
            {
                return MainCamera.Instance != null ? MainCamera.Instance.mainCamera : null;
            }
        }

        private void Awake()
        {
            this.DoResize(this.manualWidth, this.manualHeight);
        }

        private void Update()
        {
            if (this.autoResize)
                this.DoResize(this.manualWidth, this.manualHeight);
        }

        private void DoResize(int newWidth, int newHeight)
        {
            if (newWidth == 0)
                newWidth = this.manualWidth;
            else
                this.manualWidth = newWidth;

            if (newHeight == 0)
                newHeight = this.manualHeight;
            else
                this.manualHeight = newHeight;

            int width = Screen.width;
            int height = Screen.height;

#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == false)
            {
                Vector2 realScreenRes = UnityEx.GetSizeOfMainGameView;
                width = (int)realScreenRes.x;
                height = (int)realScreenRes.y;
            }
#endif  // UNITY_EDITOR

            float manualOrthoSize = this.manualHeight * 0.5f;
            float nextOrthographicSize = manualOrthoSize * ((height / (float)this.manualHeight) * (this.manualWidth / (float)width));
            if (this.mainCamera.orthographicSize != nextOrthographicSize)
                this.mainCamera.orthographicSize = nextOrthographicSize;
        }

#if UNITY_EDITOR
        protected override void OnSetComponent()
        {
            base.OnSetComponent();

            this.mainCamera = this.GetComponent<Camera>();
        }

        protected override void OnDrawGizmosEx()
        {
            base.OnDrawGizmosEx();

            if (this.editorReset)
            {
                this.editorReset = false;

                this.DoResize(this.manualWidth, this.manualHeight);

                UnityEx.RepaintAll();
            }
        }
#endif  // UNITY_EDITOR
    }
}
