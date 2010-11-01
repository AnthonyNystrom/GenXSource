using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenTransform
{
    //A single cell in the geometry window screen
    public class NuGenGeometryWindowItem
    {

        private int row;
        private int column;
        private string entry;

        public NuGenGeometryWindowItem(int row, int column, string entry)
        {
            this.row = row;
            this.column = column;
            this.entry = entry;
        }

        public NuGenGeometryWindowItem(NuGenGeometryWindowItem old)
        {
            row = old.row;
            column = old.column;
            entry = old.entry;
        }

        public override bool Equals(object obj)
        {
            return this == (NuGenGeometryWindowItem)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator==(NuGenGeometryWindowItem item, NuGenGeometryWindowItem other)
        {
            return((item.row == other.row) &&
                    (item.column == other.column) &&
                    (item.entry == other.entry));
        }

        public static bool operator !=(NuGenGeometryWindowItem item, NuGenGeometryWindowItem other)
        {
            return !(item == other);
        }

        public int Row
        {
            get
            {
                return row;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
        }

        public string Entry
        {
            get
            {
                return entry;
            }
        }
    }
}
