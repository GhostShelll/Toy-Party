using UnityEngine;

namespace com.jbg.core
{
    public static class UnityEx
    {
        public static void ShowTransform(this Transform trans)
        {
            if (trans == null)
                return;

            Vector3 pos = trans.localPosition;
            if (pos.y >= 1000000)
            {
                pos.y -= 1100000;
                trans.localPosition = pos;
            }
        }

        public static void HideTransform(this Transform trans)
        {
            if (trans == null)
                return;

            Vector3 pos = trans.localPosition;
            if (pos.y < 1000000)
            {
                pos.y += 1100000;
                trans.localPosition = pos;
            }
        }

        public static bool IsHideTransform(this Transform trans)
        {
            if (trans == null)
                return true;

            Vector3 pos = trans.localPosition;
            return pos.y >= 1000000;
        }

        public static Transform FindTransform(this GameObject go, string path)
        {
            if (go != null)
                return go.transform.Find(path);

            return null;
        }

        public static GameObject FindObject(this GameObject go, string path)
        {
            Transform t = UnityEx.FindTransform(go, path);
            return t != null ? t.gameObject : null;
        }

        public static Transform FindTransform(this Transform t, string path)
        {
            if (t != null)
                return t.Find(path);

            return null;
        }

        public static GameObject FindObject(this Transform t, string path)
        {
            t = UnityEx.FindTransform(t, path);
            return t != null ? t.gameObject : null;
        }

        public static Transform FindTransform(this Component c, string path)
        {
            if (c != null)
                return c.transform.Find(path);

            return null;
        }

        public static GameObject FindObject(this Component c, string path)
        {
            Transform t = UnityEx.FindTransform(c, path);
            return t != null ? t.gameObject : null;
        }

        public static Transform FindTransform(this ComponentEx c, string path)
        {
            if (c != null)
                return c.CachedTransform.Find(path);

            return null;
        }

        public static GameObject FindObject(this ComponentEx c, string path)
        {
            Transform t = UnityEx.FindTransform(c, path);
            return t != null ? t.gameObject : null;
        }

        public static T FindComponent<T>(this GameObject go, string path = null) where T : Component
        {
            if (go != null)
            {
                if (string.IsNullOrEmpty(path) == false)
                {
                    Transform t = UnityEx.FindTransform(go, path);
                    if (t != null)
                        return t.GetComponent<T>();
                }
                else
                {
                    return go.GetComponent<T>();
                }
            }

            return null;
        }

        public static T FindComponent<T>(this Component c, string path = null) where T : Component
        {
            if (c != null)
            {
                if (string.IsNullOrEmpty(path) == false)
                {
                    Transform t = UnityEx.FindTransform(c, path);
                    if (t != null)
                        return t.GetComponent<T>();
                }
                else
                {
                    return c.GetComponent<T>();
                }
            }

            return null;
        }

        public static T FindComponentInChildren<T>(this GameObject go, string path = null) where T : Component
        {
            if (go != null)
            {
                if (string.IsNullOrEmpty(path) == false)
                {
                    Transform t = UnityEx.FindTransform(go, path);
                    if (t != null)
                        return t.GetComponentInChildren<T>();
                }
                else
                {
                    return go.GetComponentInChildren<T>();
                }
            }

            return null;
        }

        public static T FindComponentInChildren<T>(this Component c, string path = null) where T : Component
        {
            if (c != null)
            {
                if (string.IsNullOrEmpty(path) == false)
                {
                    Transform t = UnityEx.FindTransform(c, path);
                    if (t != null)
                        return t.GetComponentInChildren<T>();
                }
                else
                {
                    return c.GetComponentInChildren<T>();
                }
            }

            return null;
        }

#if UNITY_EDITOR
        public static void SetDirtyAll(Transform trans)
        {
            Component[] components = trans.GetComponentsInChildren<Component>(true);
            for (int i = 0; i < components.Length; i++)
            {
                Component component = components[i];
                if (component != null)
                    UnityEditor.EditorUtility.SetDirty(component);
                else
                    DebugEx.LogWarning("MISSING COMPONENT AT : " + i);
            }

            UnityEx.SetDirtyTransform(trans);
        }

        private static void SetDirtyTransform(Transform trans)
        {
            if (trans != null)
            {
                UnityEditor.EditorUtility.SetDirty(trans);

                foreach (Transform transform in trans)
                    UnityEx.SetDirtyTransform(transform);
            }
            else
            {
                DebugEx.LogWarning("NULL TRANSFORM!!");
            }
        }

        public static UnityEditor.EditorWindow GameView
        {
            get
            {
                System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
                System.Type type = assembly.GetType("UnityEditor.GameView");
                return UnityEditor.EditorWindow.GetWindow(type);
            }
        }

        public static Vector2 GetSizeOfMainGameView
        {
            get
            {
                System.Type typr = System.Type.GetType("UnityEditor.GameView,UnityEditor");
                System.Reflection.MethodInfo method = typr.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                object result = method.Invoke(null, null);
                return (Vector2)result;
            }
        }

        public static void RepaintAll()
        {
            UnityEditor.SceneView.RepaintAll();
            UnityEx.GameView.Repaint();
        }
#endif  // UNITY_EDITOR
    }
}
