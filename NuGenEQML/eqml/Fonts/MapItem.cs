namespace Fonts
{
    using System;

    public class MapItem
    {
        public MapItem(int id, string name, bool bActive)
        {
            this.id_ = 0;
            this.isActive_ = true;
            this.name_ = "";
            this.ref2_ = 0;
            this.ref1_ = 0;
            this.id_ = id;
            this.name_ = name;
            this.isActive_ = bActive;
        }

        public override string ToString()
        {
            return this.Name;
        }


        public int ref1
        {
            get
            {
                return this.ref1_;
            }
            set
            {
                this.ref1_ = value;
            }
        }

        public int ref2
        {
            get
            {
                return this.ref2_;
            }
            set
            {
                this.ref2_ = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name_;
            }
        }

        public int ID
        {
            get
            {
                return this.id_;
            }
        }

        public bool IsActive
        {
            get
            {
                return this.isActive_;
            }
            set
            {
                this.isActive_ = value;
            }
        }


        private int id_;
        private bool isActive_;
        private string name_;
        private int ref2_;
        private int ref1_;
    }
}

