using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using com.jbg.core.manager;
using com.jbg.content.scene;

namespace com.jbg.core.scene
{
    using Manager = SceneExManager;

    public static class SceneExManager
    {
        public enum SceneType
        {
            Invalid,
            Title,
        }

        private static readonly string[] SceneName = new string[]
        {
            "Game",
            "TitleScene",
        };

        private delegate SceneEx FactoryFunc();
        private static readonly Dictionary<SceneType, FactoryFunc> SingleFactory = new()
        {
            { SceneType.Title, () => { return new TitleScene(); } },
        };

        public static bool IsOpened { get; private set; }
        public static int SceneChangeCount { get; private set; }
        public static SceneType CurSceneType { get; private set; }
        public static SceneType BeforeSceneType { get; private set; }
        public static SceneEx SceneInstance { get; private set; }
        public static bool IsEnableBackButton { get; set; }         // TODO[jbg] : 백버튼 활성화 여부

        private static GameObject curSceneObj = null;
        private static int hideCount = 0;
        private static Action completeCallback = null;
        private static Action changedCallback = null;
        private static AsyncOperation asyncLoadScene = null;
        private static AsyncOperation asyncLoadNextScene = null;
        private static bool appSuspendOn = false;
        private static bool appResumeOn = false;

        private static WaitAsyncData waitAsyncData = null;

        private const string CLASSNAME = "SceneExManager";

        public static void Open()
        {
            Manager.Close();
            Manager.ResetValues();

            Manager.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);
        }

        public static void Close()
        {
            if (Manager.IsOpened)
            {
                Manager.IsOpened = false;

                SystemManager.RemoveOpenList(CLASSNAME);

                Manager.ResetValues();
            }
        }

        private static void ResetValues()
        {
            DebugEx.Log(string.Format("RESET_VALUES (CURRENT : {0}, NEXT : 0)", Manager.hideCount));

            Manager.SceneChangeCount = 0;
            Manager.CurSceneType = SceneType.Invalid;
            Manager.BeforeSceneType = SceneType.Invalid;

            if (Manager.SceneInstance != null)
                Manager.SceneInstance.Close();
            Manager.SceneInstance = null;

            if (Manager.curSceneObj)
                UnityEngine.Object.Destroy(Manager.curSceneObj);
            Manager.curSceneObj = null;

            Manager.hideCount = 0;
            Manager.completeCallback = null;
            Manager.changedCallback = null;
            Manager.asyncLoadScene = null;
            Manager.asyncLoadNextScene = null;
            Manager.appSuspendOn = false;
            Manager.appResumeOn = false;
        }

        public static void OpenScene(Manager.SceneType sceneType, Action completeCallback = null, Action changedCallback = null)
        {
            DebugEx.Log("OPEN_SCENE:" + sceneType);

            Manager.SceneChangeCount++;

            if (Manager.asyncLoadNextScene != null)
            {
                DebugEx.Log("OPEN_SCENE_DUPLICATED#1:" + sceneType);
                Manager.OpenSceneWait(sceneType, completeCallback, changedCallback);
                return;
            }
            else if (Manager.asyncLoadScene != null)
            {
                DebugEx.Log("OPEN_SCENE_DUPLICATED#2:" + sceneType);
                Manager.OpenSceneWait(sceneType, completeCallback, changedCallback);
                return;
            }
            else if (Manager.waitAsyncData != null)
            {
                DebugEx.LogError("OPEN_SCENE_DUPLICATED_EXCEPTION#2:" + sceneType + ", WAIT:" + Manager.waitAsyncData.sceneType + ", CURRENT:" + Manager.CurSceneType);
                return;
            }

            if (Manager.CurSceneType != sceneType)
                Manager.BeforeSceneType = Manager.CurSceneType;

            Manager.CurSceneType = sceneType;

            Manager.curSceneObj = null;

            if (Manager.SceneInstance != null)
                Manager.SceneInstance.Close();
            Manager.SceneInstance = null;

            Manager.completeCallback = completeCallback;
            Manager.changedCallback = changedCallback;

            Manager.asyncLoadNextScene = SceneManager.LoadSceneAsync("NextScene", LoadSceneMode.Single);
        }

        private static void NextSceneLoadComplete()
        {
            Manager.changedCallback?.Invoke();
            Manager.changedCallback = null;

            Manager.asyncLoadScene = SceneManager.LoadSceneAsync(Manager.SceneName[(int)Manager.CurSceneType], LoadSceneMode.Single);
        }

        private static void OpenSceneWait(Manager.SceneType sceneType, Action completeCallback = null, Action changedCallback = null)
        {
            if (Manager.waitAsyncData != null)
            {
                DebugEx.LogError("OPEN_SCENE_DUPLICATED_EXCEPTION#1:" + sceneType + ", WAIT:" + Manager.waitAsyncData.sceneType + ", CURRENT:" + Manager.CurSceneType);
                return;
            }

            Manager.waitAsyncData = new WaitAsyncData(sceneType, completeCallback, changedCallback);
        }

        public static void Update()
        {
            if (Manager.curSceneObj == null)
            {
                if (Manager.asyncLoadNextScene != null)
                {
                    if (Manager.asyncLoadNextScene.isDone)
                    {
                        Manager.asyncLoadNextScene = null;
                        Manager.NextSceneLoadComplete();
                    }
                    return;
                }
                else if (Manager.asyncLoadScene != null && Manager.asyncLoadScene.isDone)
                {
                    Manager.curSceneObj = GameObject.Find(Manager.SceneName[(int)Manager.CurSceneType]);

                    if (Manager.curSceneObj != null)
                    {
                        Camera[] cams = Manager.curSceneObj.GetComponentsInChildren<Camera>();
                        foreach (Camera cam in cams)
                        {
                            if (cam && cam.gameObject)
                                GameObject.Destroy(cam.gameObject);
                        }

                        if (0 < Manager.hideCount)
                        {
                            DebugEx.Log(string.Format("UPDATE (HideCount : {0})", Manager.hideCount));

                            Manager.curSceneObj.transform.HideTransform();
                        }

                        SceneView sceneView = Manager.curSceneObj.GetComponent<SceneView>();

                        if (sceneView != null && sceneView.CanvasArr != null)
                        {
                            foreach (Canvas canvas in sceneView.CanvasArr)
                            {
                                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                                canvas.worldCamera = MainCamera.Camera;

                                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                                if (scaler)
                                    scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                            }
                        }

                        if (Manager.SceneInstance != null && Manager.SceneInstance.SceneView != null)
                            Manager.SceneInstance.SceneView.Inactive();

                        Manager.SceneInstance = Manager.SingleFactory[Manager.CurSceneType]();
                        Manager.SceneInstance.Open(sceneView);
                    }
                }
            }

            if (Manager.appResumeOn)
            {
                Manager.appSuspendOn = false;
                Manager.appResumeOn = false;

                if (Manager.SceneInstance != null)
                    Manager.SceneInstance.AppResume();
            }
            else if (Manager.appSuspendOn)
            {
                return;
            }

            bool isChanged = false;
            if (Manager.asyncLoadScene != null && Manager.asyncLoadScene.isDone)
            {
                isChanged = true;

                Manager.completeCallback?.Invoke();
                Manager.completeCallback = null;

                Manager.asyncLoadScene = null;
            }

            if (Manager.SceneInstance != null)
                Manager.SceneInstance.Update();

            if (isChanged)
            {
                if (Manager.waitAsyncData != null)
                {
                    Manager.WaitAsyncData nextSceneData = Manager.waitAsyncData;
                    Manager.waitAsyncData = null;
                    Manager.OpenScene(nextSceneData.sceneType, nextSceneData.completeCallback, nextSceneData.changedCallback);
                }
            }
        }

        public static void Back()
        {
            bool enableBackButton = Manager.IsEnableBackButton;

            DebugEx.Log("ENABLE_BACK_BUTTON : " + enableBackButton);

            if (enableBackButton == false)
                return;

            // TODO[jbg] : 팝업의 백 버튼 처리부터 진행
            //if (Popup.Focused)
            //{
            //    Popup.Focused.OnBack();
            //    return;
            //}

            if (Manager.SceneInstance != null)
                Manager.SceneInstance.Back();
        }

        public static void AppSuspend()
        {
            if (Manager.appSuspendOn)
                return;

            Manager.appSuspendOn = true;

            if (Manager.SceneInstance != null)
                Manager.SceneInstance.AppSuspend();
        }

        public static void AppResume()
        {
            if (Manager.appSuspendOn == false)
                return;

            Manager.appResumeOn = true;
        }

        public class WaitAsyncData
        {
            public Manager.SceneType sceneType;
            public Action completeCallback;
            public Action changedCallback;

            public WaitAsyncData(Manager.SceneType sceneType, Action completeCallback, Action changedCallback)
            {
                this.sceneType = sceneType;
                this.completeCallback = completeCallback;
                this.changedCallback = changedCallback;
            }
        }
    }
}
