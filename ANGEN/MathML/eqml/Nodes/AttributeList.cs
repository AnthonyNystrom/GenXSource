namespace Nodes
{
    using System;
    using System.Collections;

    public class AttributeList
    {
        public AttributeList()
        {
            this.list = new ArrayList();
            this.iterator = 0;
            this.count = 0;
        }

        public void Add(Attribute attribute)
        {
            try
            {
                this.list.Add(attribute);
                this.count++;
            }
            catch
            {
            }
        }

        public void CopyTo(AttributeList list)
        {
            if (list == null)
            {
                list = new AttributeList();
            }
            this.Reset();
            for (Attribute attribute = this.Next(); attribute != null; attribute = this.Next())
            {
                Attribute a = new Attribute(attribute.name, attribute.val, attribute.ns, attribute.system);
                list.Add(a);
            }
            this.Reset();
            list.Reset();
        }

        public void Remove(Attribute attribute)
        {
            try
            {
                this.list.Remove(attribute);
                if (this.count > 0)
                {
                    this.count--;
                }
            }
            catch
            {
            }
        }

        public Attribute Next()
        {
            try
            {
                if (this.iterator < this.count)
                {
                    object o = this.list[this.iterator];
                    Attribute attribute = (Attribute) o;
                    this.iterator++;
                    return attribute;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public void Reset()
        {
            this.iterator = 0;
        }

        public Attribute Get(string name)
        {
            int i = 0;
            int count = this.count;
            bool found = false;
            Attribute attribute = null;
            try
            {
                if (count <= 0)
                {
                    return attribute;
                }
                while ((i < count) && !found)
                {
                    object o = this.list[i];
                    if (o != null)
                    {
                        attribute = (Attribute) o;
                        if ((attribute != null) && (attribute.name == name))
                        {
                            found = true;
                        }
                    }
                    i++;
                }
                if (!found)
                {
                    attribute = null;
                }
            }
            catch
            {
                attribute = null;
            }
            return attribute;
        }

        public string GetValue(string name)
        {
            Attribute attribute = null;
            attribute = this.Get(name);
            if (attribute != null)
            {
                return attribute.val;
            }
            return "";
        }

        public void Add(string name, string val)
        {
            try
            {
                if ((((name != null) && (val != null)) && ((name.Length > 0) && (val.Length > 0))) && (name.ToUpper() == "CLASS"))
                {
                    val = val.Replace("_", "-");
                }
                Attribute attribute = this.Get(name);
                if (attribute != null)
                {
                    attribute.val = val;
                }
                else
                {
                    attribute = new Attribute(name, val, "");
                    this.Add(attribute);
                }
            }
            catch
            {
            }
        }

        public int Count
        {
            get
            {
                if (this.list == null)
                {
                    return 0;
                }
                try
                {
                    return this.list.Count;
                }
                catch
                {
                    return 0;
                }
            }
        }


        private int iterator;
        private int count;
        private ArrayList list;
    }
}

