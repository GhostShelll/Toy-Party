using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace com.jbg.core
{
    public class DebugEx
    {
        private static readonly string tag = "TODO[jbg]";

        [Conditional("LOG_DEBUG")]
        public static void Log(object msg)
        {
            UnityEngine.Debug.unityLogger.Log(tag, msg);
        }

        [Conditional("LOG_DEBUG")]
        public static void Log(string msg, params object[] args)
        {
            UnityEngine.Debug.unityLogger.Log(tag, string.Format(msg, args));
        }

        [Conditional("LOG_DEBUG")]
        public static void Logs<T>(IList<T> list, Func<int, T, object> function)
        {
            if (function != null)
            {
                for (int i = 0; i < list.Count; ++i)
                    UnityEngine.Debug.unityLogger.Log(tag, function(i, list[i]));
            }
        }

        [Conditional("LOG_DEBUG")]
        public static void LogColor(object msg, string color)
        {
            UnityEngine.Debug.unityLogger.Log(tag, string.Format("<color={0}>{1}</color>", color, msg));
        }

        public static void LogWarning(object msg)
        {
            UnityEngine.Debug.unityLogger.LogWarning(tag, msg);
        }

        public static void LogWarning(string msg, params object[] args)
        {
            UnityEngine.Debug.unityLogger.LogWarning(tag, string.Format(msg, args));
        }

        public static void LogError(object msg)
        {
            UnityEngine.Debug.unityLogger.LogError(tag, msg);
        }

        public static void LogError(string msg, params object[] args)
        {
            UnityEngine.Debug.unityLogger.LogError(tag, string.Format(msg, args));
        }
    }
}
