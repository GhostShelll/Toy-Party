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
#endif  // UNITY_EDITOR
    }
}
