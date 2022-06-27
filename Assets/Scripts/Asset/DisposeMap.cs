using System;
using System.Collections.Generic;

namespace com.jbg.asset
{
    public static class DisposeMap
    {
        private static Dictionary<string, LinkedList<IDisposable>> Map = new();

        public static void AddObject(string tag, IDisposable itf)
        {
            if (DisposeMap.Map.TryGetValue(tag, out LinkedList<IDisposable> list) == false)
            {
                list = new LinkedList<IDisposable>();
                DisposeMap.Map.Add(tag, list);
            }

            list.AddLast(itf);
        }

        public static bool RemoveObject(string tag, IDisposable itf)
        {
            if (DisposeMap.Map.TryGetValue(tag, out LinkedList<IDisposable> list))
            {
                LinkedListNode<IDisposable> node = list.Find(itf);
                if (node != null)
                {
                    list.Remove(node);
                    return true;
                }
            }

            return false;
        }

        public static void DisposeAll()
        {
            Dictionary<string, LinkedList<IDisposable>> mapOld = DisposeMap.Map;
            DisposeMap.Reset();

            foreach (KeyValuePair<string, LinkedList<IDisposable>> pair in mapOld)
            {
                foreach (IDisposable itf in pair.Value)
                {
                    itf.Dispose();
                }
            }

            mapOld.Clear();
        }

        public static bool DisposeAllByTag(string tag)
        {
            if (DisposeMap.Map.TryGetValue(tag, out LinkedList<IDisposable> list))
            {
                DisposeMap.Map.Remove(tag);

                foreach (IDisposable itf in list)
                {
                    itf.Dispose();
                }

                return true;
            }

            return false;
        }

        public static void Reset()
        {
            DisposeMap.Map = new Dictionary<string, LinkedList<IDisposable>>();
        }
    }
}
