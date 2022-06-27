using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using com.jbg.core;
using com.jbg.core.manager;

namespace com.jbg.asset
{
    public class Storage<K, V> : System.IDisposable where V : class
    {
        protected static Storage<K, V> Instance;

        public class Data
        {
            public K Key;
            public V Value;
            [JsonIgnore]
            public LinkedListNode<KeyValuePair<K, V>> Node;

            public Data(K key, V value, LinkedListNode<KeyValuePair<K, V>> node)
            {
                this.Key = key;
                this.Value = value;
                this.Node = node;
            }
        }

        protected Dictionary<K, Data> Map_ = new();
        protected LinkedList<KeyValuePair<K, V>> OrderedValues_ = new();
        protected string DisposeTag = "Storage";

        public static bool IsOpened
        {
            get
            {
                return Storage<K, V>.Instance != null;
            }
        }

        public static int Count
        {
            get
            {
                if (Storage<K, V>.IsOpened)
                {
                    return Storage<K, V>.Instance.Map_.Count;
                }

                return 0;
            }
        }

        public static IEnumerator GetEnumerator()
        {
            if (Storage<K, V>.IsOpened)
            {
                return Storage<K, V>.Instance.OrderedValues_.GetEnumerator();
            }

            return null;
        }

        public static IEnumerable Values
        {
            get
            {
                if (Storage<K, V>.IsOpened)
                {
                    LinkedList<KeyValuePair<K, V>> linkedList = Storage<K, V>.Instance.OrderedValues_;

                    foreach (KeyValuePair<K, V> pair in linkedList)
                    {
                        yield return pair.Value;
                    }
                }
            }
        }

        public static IEnumerable Keys
        {
            get
            {
                if (Storage<K, V>.IsOpened)
                {
                    LinkedList<KeyValuePair<K, V>> linkedList = Storage<K, V>.Instance.OrderedValues_;

                    foreach (KeyValuePair<K, V> pair in linkedList)
                    {
                        yield return pair.Key;
                    }
                }
            }
        }

        public static IEnumerable KeyValues
        {
            get
            {
                if (Storage<K, V>.IsOpened)
                {
                    LinkedList<KeyValuePair<K, V>> linkedList = Storage<K, V>.Instance.OrderedValues_;

                    foreach (KeyValuePair<K, V> pair in linkedList)
                    {
                        yield return pair;
                    }
                }
            }
        }

        public static LinkedListNode<KeyValuePair<K, V>> First
        {
            get
            {
                if (Storage<K, V>.IsOpened)
                {
                    return Storage<K, V>.Instance.OrderedValues_.First;
                }

                return null;
            }
        }

        public static LinkedListNode<KeyValuePair<K, V>> Last
        {
            get
            {
                if (Storage<K, V>.IsOpened)
                {
                    return Storage<K, V>.Instance.OrderedValues_.Last;
                }

                return null;
            }
        }

        public delegate K GetKeyCallback(V value);

        private static string CLASSNAME { get { return string.Format("Storage<{0}, {1}>", typeof(K).Name, typeof(V).Name); } }

        public static void Open(V[] array, Storage<K, V>.GetKeyCallback getKey, string disposeTag = "Storage")
        {
            Storage<K, V>.Close();

            SystemManager.AddOpenList(CLASSNAME);

            Storage<K, V>.Instance = new Storage<K, V>();
            Storage<K, V>.AddArray(array, getKey);

            if (disposeTag != null)
                Storage<K, V>.Instance.DisposeTag = disposeTag;

            DisposeMap.AddObject(Storage<K, V>.Instance.DisposeTag, Storage<K, V>.Instance);
        }

        public static void Close()
        {
            if (Storage<K, V>.Instance != null)
            {
                DisposeMap.RemoveObject(Storage<K, V>.Instance.DisposeTag, Storage<K, V>.Instance);
                Storage<K, V>.Instance = null;

                SystemManager.RemoveOpenList(CLASSNAME);
            }
        }

        public static void Clear()
        {
            if (Storage<K, V>.Instance != null)
            {
                Storage<K, V> inst = Storage<K, V>.Instance;

                inst.Map_.Clear();
                inst.OrderedValues_.Clear();
            }
        }

        public void Dispose()
        {
            Storage<K, V>.Close();
        }

        public static V Find(K key)
        {
            if (Storage<K, V>.IsOpened == false)
                throw new System.Exception("Storage<" + typeof(K).Name + ", " + typeof(V).Name + ">.Find(" + key + ") FAILED: NOT OPENED");

            if (Storage<K, V>.Instance.Map_.TryGetValue(key, out Data data))
                return data.Value;

            return null;
        }

        public static bool Contains(K key)
        {
            if (Storage<K, V>.IsOpened == false)
                throw new System.Exception("Storage<" + typeof(K).Name + ", " + typeof(V).Name + ">.Contains(" + key + ") FAILED: NOT OPENED");

            return Storage<K, V>.Instance.Map_.ContainsKey(key);
        }

        public static void Add(K key, V value)
        {
            if (Storage<K, V>.IsOpened == false)
                throw new System.Exception("Storage<" + typeof(K).Name + ", " + typeof(V).Name + ">.Add(" + key + ", " + JsonConvert.SerializeObject(value, Formatting.Indented).ToString() + ") FAILED: NOT OPENED");

            Storage<K, V> instance = Storage<K, V>.Instance;

            if (instance.Map_.ContainsKey(key))
                throw new System.Exception("Storage<" + typeof(K).Name + ", " + typeof(V).Name + ">.Add(" + key + ", " + JsonConvert.SerializeObject(value, Formatting.Indented).ToString() + ") FAILED: DUPLICATE:" + JsonConvert.SerializeObject(instance.Map_[key], Formatting.Indented).ToString());

            instance.Map_.Add(key, new Data(key, value, instance.OrderedValues_.AddLast(new KeyValuePair<K, V>(key, value))));
        }

        public static void AddArray(V[] array, Storage<K, V>.GetKeyCallback getKey)
        {
            if (Storage<K, V>.IsOpened == false)
                throw new System.Exception("Storage<" + typeof(K).Name + ", " + typeof(V).Name + ">.AddArray(" + array + ", " + getKey + ") FAILED: NOT OPENED");

            Storage<K, V> instance = Storage<K, V>.Instance;

            int count = array != null ? array.Length : 0;
            for (int n = 0; n < count; ++n)
            {
                try
                {
                    V value = array[n];
                    K key = getKey(value);
                    instance.Map_.Add(key, new Data(key, value, instance.OrderedValues_.AddLast(new KeyValuePair<K, V>(key, value))));
                }
                catch (System.ArgumentException e)
                {
                    DebugEx.LogWarning("Storage<" + typeof(K).Name + ", " + typeof(V).Name + ">.AddArray(" + array + ", " + getKey + ") FAILED: EXCEPTION, IDX:" + n);
                    DebugEx.LogError(e);
                    V value = array[n];
                    K key = getKey(value);
                    DebugEx.LogWarning("K:" + key + ", V:" + JsonConvert.SerializeObject(value, Formatting.Indented).ToString());
                    if (instance.Map_.ContainsKey(key))
                        DebugEx.Log("DUPLICATED:" + JsonConvert.SerializeObject(instance.Map_[key], Formatting.Indented).ToString());
                    else
                        DebugEx.Log("???");
                }
                catch (System.Exception e)
                {
                    DebugEx.LogWarning("Storage<" + typeof(K).Name + ", " + typeof(V).Name + ">.AddArray(" + array + ", " + getKey + ") FAILED: EXCEPTION, IDX:" + n);
                    DebugEx.LogError(e);
                }
            }
        }

        public static bool Remove(K key)
        {
            if (Storage<K, V>.IsOpened == false)
                throw new System.Exception("Storage<" + typeof(K).Name + ", " + typeof(V).Name + ">.Remove(" + key + ") FAILED: NOT OPENED");

            Storage<K, V> instance = Storage<K, V>.Instance;

            if (instance.Map_.TryGetValue(key, out Data data))
            {
                instance.OrderedValues_.Remove(data.Node);
                instance.Map_.Remove(key);
                return true;
            }

            return false;
        }
    }
}
