using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TID.Plugin.Resource.Event
{
    /// <summary>
    ///事件聚合器
    /// </summary>
     public class RresourceEventAggregator

    {
        private static EventAggregator eventAggregator = null;
        private static readonly object syncLock = new object();
        private RresourceEventAggregator() { }
        public static EventAggregator GetEventAggregator()
        {
            lock (syncLock)
            {
                if (eventAggregator == null)
                {
                    eventAggregator = new EventAggregator();
                }
                return eventAggregator;
            }
        }
    }
}
