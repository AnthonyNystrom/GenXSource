using System.Drawing;
using Genetibase.VisUI.UI;

namespace NuGenVisUI
{
    public abstract class VisLayer
    {
        readonly string name;
        Image preview;
        bool enabled;

        /// <summary>
        /// Initializes a new instance of the VisLayer class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="preview"></param>
        /// <param name="enabled"></param>
        public VisLayer(string name, Image preview, bool enabled)
        {
            this.name = name;
            this.preview = preview;
            this.enabled = enabled;
        }

        public string Name
        {
            get { return name; }
        }

        public Image Preview
        {
            get { return preview; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
    }

    public class ScreenVisLayer : VisLayer
    {
        readonly ILayer actualLayer;

        public ScreenVisLayer(string name, Image preview, bool enabled, ILayer actualLayer)
            : base(name, preview, enabled)
        {
            this.actualLayer = actualLayer;
        }

        public ILayer ActualLayer
        {
            get { return actualLayer; }
        }
    }

    public class GeometryVisLayer : VisLayer
    {
        public GeometryVisLayer(string name, Image preview, bool enabled)
            : base(name, preview, enabled)
        {
        }
    }
}