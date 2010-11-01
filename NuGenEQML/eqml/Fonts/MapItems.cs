namespace Fonts
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;

    public class MapItems
    {
        public MapItems()
        {
            this.hash_ = new Hashtable();
            this.list_ = new ArrayList();
        }

        public bool Find(int id)
        {
            MapItem item = null;
            try
            {
                item = (MapItem) this.hash_[id];
                if ((item != null) && (item.ID == id))
                {
                    return item.IsActive;
                }
            }
            catch
            {
            }
            return false;
        }

        public void Ref(int id, bool bAll)
        {
            MapItem item = null;
            try
            {
                item = (MapItem) this.hash_[id];
                if ((item == null) || (item.ID != id))
                {
                    return;
                }
                if (bAll)
                {
                    item.ref1++;
                }
                else
                {
                    item.ref2++;
                }
            }
            catch
            {
            }
        }

        public void Put(int id, string name, bool bActive)
        {
            bool exists = false;
            if (this.hash_.Count > 0)
            {
                try
                {
                    if (this.hash_[id] != null)
                    {
                        exists = true;
                    }
                }
                catch
                {
                }
            }
            if (!exists)
            {
                MapItem item = new MapItem(id, name, bActive);
                this.hash_.Add(id, item);
                this.list_.Add(item);
            }
        }

        public MapItem Get(int index)
        {
            MapItem r = null;
            if (index < this.Count)
            {
                object o = this.list_[index];
                if (o != null)
                {
                    r = (MapItem) o;
                }
            }
            return r;
        }

        public int Count
        {
            get
            {
                if (this.list_ != null)
                {
                    return this.list_.Count;
                }
                return 0;
            }
        }

        private Hashtable hash_;
        private ArrayList list_;
    }
}

