using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

namespace com.jbg.core
{
    public class View : ComponentEx
    {
        private Dictionary<int, Action<int, object>> Events { get; set; }

        public virtual void Refresh()
        {
            DebugEx.Log(0);
            Assert.IsTrue(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////// 이벤트 기능
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void DoEvent<T>(T key, object go = null)
        {
            DebugEx.Log(string.Format("VIEW::DO_EVENT EVENT:{0}, GO:{1}, VIEW_NAME:{2}, VIEW_TYPE:{3}", key, go, this.name, this.GetType().Name));

            if (this.Events != null)
            {
                int keyi = key.GetHashCode();
                if (this.Events.TryGetValue(keyi, out Action<int, object> action))
                    action?.Invoke(keyi, go);
            }
        }

        public void BindEvent<T>(T key, Action<int, object> action)
        {
            DebugEx.Log(string.Format("VIEW::BIND_EVENT EVENT:{0}, VIEW_NAME:{1}, VIEW_TYPE:{2}", key, this.name, this.GetType().Name));

            if (this.Events == null)
                this.Events = new Dictionary<int, Action<int, object>>();

            this.Events.Add(key.GetHashCode(), action);
        }

        public bool RemoveEvent<T>(T key)
        {
            if (this.Events == null)
                return false;

            bool ret = this.Events.Remove(key.GetHashCode());

            if (this.Events.Count == 0)
                this.Events = null;

            return ret;
        }

        public Action<int, object> FindEvent<T>(T key)
        {
            if (this.Events == null)
                return null;

            if (this.Events.TryGetValue(key.GetHashCode(), out Action<int, object> action))
                return action;

            return null;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
