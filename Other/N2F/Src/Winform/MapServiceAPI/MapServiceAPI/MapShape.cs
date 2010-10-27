using System;
using System.Collections.Generic;
using System.Text;

namespace MapServiceAPI
{
    public abstract class MapShapes : IMapShape
    {
        #region IMapShape Members

        public String Id
        {
            get
            {
                return id;
            }
        }
        #endregion

        private static int shapeIDCount;
        private String id;
        private Boolean isDisposed = false;
        private Boolean isClickable = false;

        public Boolean IsClickable
        {
            get { return isClickable; }
            set { isClickable = value; }
        }
        private MapShapes parent;

        public MapShapes Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        protected IMapServiceJS mapServiceJS;
        public MapShapes(IMapServiceJS mapServiceJS)
        {
            id = "shape_" + shapeIDCount.ToString("0000");
            shapeIDCount++;
            this.mapServiceJS = mapServiceJS;
        }

        virtual public void Redraw() { }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Map.MapJS.ShapeDispose(this);
            }
            isDisposed = true;

        }

        ~MapShapes()
        {
            Dispose(false);
        }

    }
}
