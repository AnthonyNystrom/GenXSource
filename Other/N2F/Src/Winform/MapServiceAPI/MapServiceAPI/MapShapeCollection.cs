using System;
using System.Collections.Generic;

namespace MapServiceAPI
{
    public class MapShapeCollection:Dictionary<String, MapShapes>
    {
        public void Add(MapShapes shape)
        {
            Add(shape.Id, shape);
        }
        public Boolean Remove(MapShapes shape)
        {
            return Remove(shape.Id);
        }

    }
}
