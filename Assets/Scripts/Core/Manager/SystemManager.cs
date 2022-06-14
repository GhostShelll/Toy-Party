using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

namespace com.jbg.core.manager
{
    using Manager = SystemManager;

    public class SystemManager
    {
        public static bool IsOpened { get; private set; }
        public static bool IsPaused { get; private set; }

        private static readonly List<string> openList = new(1024);

        private const string CLASSNAME = "SystemManager";

        public static void Open()
        {
            DebugEx.Log("SYSTEMMANAGER::OPEN");

            Manager.Close();
            Manager.IsOpened = true;

            Manager.AddOpenList(CLASSNAME);

            // 각종 매니저 오픈 함수 실행
        }

        public static void Close()
        {
            if (Manager.IsOpened)
            {
                DebugEx.Log("SYSTEMMANAGER::CLOSE");

                Manager.IsOpened = false;

                Manager.RemoveOpenList(CLASSNAME);

                // 각종 매니저 클로즈 함수 실행
            }

            // 닫히지 않은 매니저를 알아낸다
            DebugEx.Log(Manager.OpenListToString);
            Manager.ClearOpenList();
        }

        public static void Update()
        {
            if (Manager.IsOpened == false)
                return;

            try
            {
                // 각종 매니저 업데이트 함수 실행
            }
            catch (System.Exception e)
            {
                DebugEx.LogError(e);
#if LOG_DEBUG
                // TODO[jbg] : 에러 팝업 연출
                //ViewControl.OpenDebugPopup(e.GetType().Name, e.Message, e.StackTrace, (pop, btn) => { SystemControl.Reset(); });
#endif  // LOG_DEBUG
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // TODO[jbg] : 백버튼 처리
            }
        }

        public static void OnApplicationPause()
        {
            if (Manager.IsPaused == false)
            {
                DebugEx.Log("SYSTEMMANAGER::ON_APPLICATION_PAUSE");

                Manager.IsPaused = true;
            }
        }

        public static void OnApplicationResume()
        {
            if (Manager.IsPaused)
            {
                DebugEx.Log("SYSTEMMANAGER::ON_APPLICATION_RESUME");

                Manager.IsPaused = false;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////// 열려있는 매니저 목록
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Conditional("LOG_DEBUG")]
        public static void AddOpenList(string tag)
        {
            Manager.openList.Add(tag);
        }

        [Conditional("LOG_DEBUG")]
        public static void RemoveOpenList(string tag)
        {
            int index = Manager.openList.IndexOf(tag);
            if (index != -1)
            {
                Manager.openList.RemoveAt(index);
                return;
            }
        }

        [Conditional("LOG_DEBUG")]
        private static void ClearOpenList()
        {
            Manager.openList.Clear();
        }

        private static string OpenListToString
        {
            get
            {
#if LOG_DEBUG
                System.Text.StringBuilder result = new(1024);

                result.Append("SYSTEMMANAGER::OPEN_CONTROL_LIST : ").AppendLine(Manager.openList.Count.ToString());
                result.Append("TAGS : ");

                if (Manager.openList.Count == 0)
                {
                    result.AppendLine("is empty.");
                }
                else
                {
                    for (int i = 0; i < Manager.openList.Count; i++)
                    {
                        string name = Manager.openList[i];
                        result.Append(i).Append(". ").AppendLine(name);
                    }
                }

                return result.ToString();
#else   // LOG_DEBUG
                return string.Empty;
#endif  // LOG_DEBUG
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
