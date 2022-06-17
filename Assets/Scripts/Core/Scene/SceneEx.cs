using System;

namespace com.jbg.core.scene
{
    public abstract class SceneEx
    {
        private Action onUpdate;

        public SceneView SceneView { get; private set; }
        public int State { get; private set; }

        public void Open(SceneView sceneView)
        {
            this.SceneView = sceneView;

            this.OnOpen();
        }

        public void Close()
        {
            this.onUpdate = null;

            this.OnClose();

            this.SceneView = null;
        }

        public void Update()
        {
            this.OnUpdate();

            this.onUpdate?.Invoke();
        }

        public void Back()
        {
            this.OnBack();
        }

        public void AppSuspend()
        {
            this.OnAppSuspend();
        }

        public void AppResume()
        {
            this.OnAppResume();
        }

        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnBack() { }
        protected virtual void OnAppSuspend() { }
        protected virtual void OnAppResume() { }
        protected virtual void Refresh() { }

        protected void AddUpdateFunc(Action func)
        {
            this.onUpdate += func;
        }
        protected void RemoveUpdateFunc(Action func)
        {
            this.onUpdate -= func;
        }
        protected void ResetUpdateFunc(Action func)
        {
            this.onUpdate = func;
        }
        protected void ClearUpdateFunc()
        {
            this.onUpdate = null;
        }

        protected void SetState<T>(T state)
        {
            DebugEx.Log("SCENE_STATE:" + state + ", SCENE_NAME:" + this.GetType().Name);

            this.State = state.GetHashCode();

            this.onUpdate = null;
        }
    }
}
