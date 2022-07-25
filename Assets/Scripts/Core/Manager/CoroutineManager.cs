using System.Collections;

using UnityEngine;

namespace com.jbg.core.manager
{
    public sealed class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager Inst { get; set; }

        private void Awake()
        {
            CoroutineManager.Inst = this;
        }

        private void OnDestroy()
        {
            CoroutineManager.ClearTasks();

            if (CoroutineManager.Inst == this)
                CoroutineManager.Inst = null;
        }

        public static Coroutine AddTask(IEnumerator routine)
        {
            try
            {
                if (CoroutineManager.Inst != null && CoroutineManager.Inst.gameObject.activeSelf)
                    return CoroutineManager.Inst.StartCoroutine(routine);
            }
            catch (System.Exception e)
            {
                DebugEx.LogWarning(e.StackTrace);
                DebugEx.LogError(e);
            }
            return null;
        }

        public static void RemoveTask(Coroutine co)
        {
            try
            {
                if (CoroutineManager.Inst != null)
                    CoroutineManager.Inst.StopCoroutine(co);
            }
            catch (System.Exception e)
            {
                DebugEx.LogWarning(e.StackTrace);
                DebugEx.LogError(e);
            }
        }

        public static void ClearTasks()
        {
            if (CoroutineManager.Inst != null)
                CoroutineManager.Inst.StopAllCoroutines();
        }
    }
}
