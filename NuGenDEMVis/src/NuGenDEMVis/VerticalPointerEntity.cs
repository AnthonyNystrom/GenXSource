using System;
using System.Drawing;
using Genetibase.NuGenDEMVis.Raster;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Genetibase.RasterDatabase;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Rendering
{
    public class VerticalPointerEntity : ShapeEntity
    {
        private readonly float maxDataValue;
        private ScreenSpaceText text;
        private readonly RasterDatabaseLookup heightLookup;

        public VerticalPointerEntity(Shape shape, DataLayer lookupLayer, float maxDataValue)
            : base(shape, new Vector3())
        {
            this.maxDataValue = maxDataValue;

            Scaling(new Vector3(0.25f, 0.25f, 0.25f));

            heightLookup = new RasterDatabaseLookup(lookupLayer);
        }

        public override void Init(DeviceInterface devIf, SceneManager sManager)
        {
 	        base.Init(devIf, sManager);

            // do a lookup to get y position
            float height = heightLookup.ValueLookup(0.25f, 0.25f) / maxDataValue;
            
            // load text
            text = new ScreenSpaceText(Math.Round((decimal)height * 10, 1) + "m", Color.Yellow, "Tahoma", FontWeight.Bold,
                                       11, new Vector3(0, 2.2f, 0), this);
            text.Init(devIf, sManager);
            sManager.AddEntity(text);
            AddDependant(text);

            Move(new Vector3(1.25f, height, 1.25f));
        }

        public string Text
        {
            get { return text.Text; }
            set { text.Text = value; }
        }
    }
}