using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis
{
    class DataSourceItem
    {
        public string Name;
        public Bitmap Thumbnail;
        public Texture Texture;
        public IDEMDataSampler DEMSampler;

        /// <summary>
        /// Initializes a new instance of the DataSourceItem class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="thumbnail"></param>
        public DataSourceItem(string name, Bitmap thumbnail)
        {
            Name = name;
            Thumbnail = thumbnail;
        }

        /// <summary>
        /// Initializes a new instance of the DataSourceItem class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="thumbnail"></param>
        /// <param name="texture"></param>
        public DataSourceItem(string name, Bitmap thumbnail, Texture texture)
        {
            Name = name;
            Thumbnail = thumbnail;
            Texture = texture;
        }
    }
}