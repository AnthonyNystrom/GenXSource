using System;
using System.Collections.Generic;
using System.Text;

namespace NuGenSVisualLib.Caching
{
    enum CachingLevel
    {
        Highest,
        High,
        Low,
        Lowest,
        None
    }

    interface CacheableResource
    {
        void ClearCache(CachingLevel toLevel);
    }

    /// <summary>
    /// Controls the global caching levels through cache interface objects
    /// </summary>
    class CacheManager
    {
        private long maxSize;
        private long currentSize;

        public long MaxSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }

        public long CurrentSize
        {
            get { return currentSize; }
        }
    }
}