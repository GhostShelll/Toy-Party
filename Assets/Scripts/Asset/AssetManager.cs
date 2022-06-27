using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using com.jbg.asset.control;
using com.jbg.core;
using com.jbg.core.manager;

namespace com.jbg.asset
{
    using Manager = AssetManager;

    public class AssetManager
    {
        public static bool IsOpened { get; private set; }

        private const string CLASSNAME = "AssetManager";
        private const string DISPOSE_TAG = "Asset";

        public static void Open()
        {
            Manager.Close();
            Manager.IsOpened = true;

            SystemManager.AddOpenList(CLASSNAME);

            // 각종 에셋 오픈
            LocaleControl.Open();

#if LOG_DEBUG && UNITY_EDITOR
            // 에셋 에러에 대한 로그
            if (Manager.notFounds.Count > 0)
            {
                StringBuilder sbNotFounds = new(1024);
                foreach (string s in notFounds)
                    sbNotFounds.Append(s).Append('\n');

                if (sbNotFounds.Length > 0)
                    DebugEx.LogWarning(sbNotFounds.ToString());

                Manager.notFounds.Clear();
            }
#endif  // LOG_DEBUG && UNITY_EDITOR
        }

        public static void Close()
        {
            if (Manager.IsOpened)
            {
                Manager.IsOpened = false;

                SystemManager.RemoveOpenList(CLASSNAME);

                // 각종 에셋 클로즈
                LocaleControl.Close();
            }
        }

        public static bool OpenStorage<K, V>(string jsonText, Storage<K, V>.GetKeyCallback getKey, string filePath = null) where V : class
        {
            try
            {
                JArray jarr = JArray.Parse(jsonText);
                V[] array = jarr.ToObject<V[]>();

                Manager.OpenStorageManually<K, V>(jsonText, jarr, array, getKey, filePath);
                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning(e);
                UnityEngine.Debug.LogWarning("TYPE:" + typeof(V).Name + ", DECRIPT IN:\n" + jsonText.FormatJson());

                Storage<K, V>.Open(null, getKey, Manager.DISPOSE_TAG);
                return false;
            }
        }

        public static void OpenStorageManually<K, V>(string jsonText, JArray jarr, V[] array, Storage<K, V>.GetKeyCallback getKey, string filePath = null) where V : class
        {
#if LOG_DEBUG && UNITY_EDITOR
            Manager.DebugStorage(jarr, array);
#endif  // LOG_DEBUG && UNITY_EDITOR
            Storage<K, V>.Open(array, getKey, Manager.DISPOSE_TAG);
        }

#if LOG_DEBUG && UNITY_EDITOR
        class NotFoundTest
        {
            public int count;
            public string status;

            public NotFoundTest(int c, string s)
            {
                this.count = c;
                this.status = s;
            }
        }

        private static readonly LinkedList<string> notFounds = new();

        private static void DebugStorage<V>(JArray jarr, V[] array) where V : class
        {
            Dictionary<string, NotFoundTest> test = new();
            Type type = typeof(V);
            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] props = type.GetProperties();
            int count = (array.Length > 10) ? 10 : array.Length;

            for (int n = 0; n < count; ++n)
            {
                V v = array[n];
                foreach (FieldInfo f in fields)
                {
                    JsonIgnoreAttribute[] ignoreAttrs = (JsonIgnoreAttribute[])f.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                    if (ignoreAttrs == null || ignoreAttrs.Length == 0)
                    {
                        object var = f.GetValue(v);
                        string text = var?.ToString();
                        if (string.IsNullOrEmpty(text))
                        {
                            if (test.TryGetValue(f.Name, out NotFoundTest notFound) == false)
                                test.Add(f.Name, notFound = new NotFoundTest(0, f.FieldType + ", " + type.Name + "." + f.Name + " is null (or empty)"));
                            ++notFound.count;
                        }
                    }
                }

                foreach (PropertyInfo p in props)
                {
                    JsonIgnoreAttribute[] ignoreAttrs = (JsonIgnoreAttribute[])p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                    if (ignoreAttrs == null || ignoreAttrs.Length == 0)
                    {
                        object var = p.GetValue(v, null);
                        string text = var?.ToString();
                        if (string.IsNullOrEmpty(text))
                        {
                            if (test.TryGetValue(p.Name, out NotFoundTest notFound) == false)
                                test.Add(p.Name, notFound = new NotFoundTest(0, p.PropertyType + ", " + type.Name + "." + p.Name + "(prop) is null (or empty)"));
                            ++notFound.count;
                        }
                    }
                }

                Dictionary<string, object> dict = jarr[n].ToObject<Dictionary<string, object>>();

                foreach (KeyValuePair<string, object> pair in dict)
                {
                    bool hasKey = false;
                    foreach (FieldInfo f in fields)
                    {
                        if (f.Name == pair.Key)
                        {
                            hasKey = true;
                            break;
                        }

                        JsonPropertyAttribute[] propAttrs = (JsonPropertyAttribute[])f.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
                        if (propAttrs != null && propAttrs.Length > 0)
                        {
                            foreach (JsonPropertyAttribute attr in propAttrs)
                            {
                                if (attr.PropertyName == pair.Key)
                                {
                                    hasKey = true;
                                    break;
                                }
                            }

                            if (hasKey)
                                break;
                        }
                    }

                    if (hasKey == false)
                    {
                        foreach (PropertyInfo p in props)
                        {
                            if (p.Name == pair.Key)
                            {
                                hasKey = true;
                                break;
                            }

                            JsonPropertyAttribute[] propAttrs = (JsonPropertyAttribute[])p.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
                            if (propAttrs != null && propAttrs.Length > 0)
                            {
                                foreach (JsonPropertyAttribute attr in propAttrs)
                                {
                                    if (attr.PropertyName == pair.Key)
                                    {
                                        hasKey = true;
                                        break;
                                    }
                                }

                                if (hasKey)
                                    break;
                            }
                        }
                    }

                    if (hasKey == false)
                    {
                        if (test.TryGetValue(pair.Key, out NotFoundTest notFound) == false)
                            test.Add(pair.Key, notFound = new NotFoundTest(0, type.Name + "." + pair.Key + "(json) is not found. value:" + pair.Value));
                        ++notFound.count;
                    }
                }
            }

            foreach (NotFoundTest notFound in test.Values)
            {
                if (notFound.count == array.Length)
                {
                    notFounds.AddLast(notFound.status);
                }
            }
        }
#endif  // LOG_DEBUG && UNITY_EDITOR
    }
}
