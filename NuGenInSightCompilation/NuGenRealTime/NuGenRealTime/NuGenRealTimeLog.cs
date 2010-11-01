using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.UI.NuGenMeters;

namespace NuGenRealTime
{
    class NuGenRealTimeLog
    {
        private NuGenLog log;        
        private List<INuGenCounter> items = new List<INuGenCounter>();

        public NuGenRealTimeLog(NuGenLog log)
        {
            this.log = log;
        }

        public NuGenLog Log
        {
            get
            {
                return log;
            }
        }

        public List<INuGenCounter> Items
        {
            get
            {
                return items;
            }
        }

        public void LogGeneric(INuGenCounter item)
        {
            items.Add(item);
            log.SetLog(item, true);
        }

        public void RemoveGeneric(INuGenCounter item)
        {
            items.Remove(item);
            log.SetLog(item, false);
        }

        public void StopLogging()
        {
            foreach (INuGenCounter item in items)
            {
                if (log.GetLog(item))
                {
                    log.SetLog(item, false);
                }
            }
        }

        public void StartLogging()
        {
            foreach (INuGenCounter item in items)
            {
                if (!log.GetLog(item))
                {
                    log.SetLog(item, true);
                }
            }
        }

        public void RemoveGenericAt(int p)
        {
            log.SetLog(items[p], false);
            items.RemoveAt(p);            
        }
    }
}
