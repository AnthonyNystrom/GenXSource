using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Settings;
using Genetibase.VisUI.Rendering;
using Genetibase.VisUI.UI;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Rendering
{
    public class VisRenderingContext3D : RenderingContext3D
    {
        DigitalElevationMap deMap;
//        TargetCamera camera;
        FillMode demFillMode = FillMode.Solid;

        readonly LoadingLayer loadingLayer;
        bool showLoading;

        public VisRenderingContext3D(ISettings settings, Control targetRenderArea,
                                     CommonDeviceInterface cdi, GraphicsProfile gProfile)
            : base(targetRenderArea, gProfile, cdi, (HashTableSettings)settings)
        {
            /*List<ISharableResource> shared = new List<ISharableResource>();
            ILayer layer = (ILayer)devIf.GetSharedResource("file://media/ui/common/WelcomeLayer.xml", ref shared);
            layers.InsertLayer(layer, uint.MinValue);*/

            //deMap = new DigitalElevationMap(new Size(), null);
            //deMap.Init(devIf.Device);
            
            loadingLayer = new LoadingLayer(devIf, new Point(), new Size(300, 300), "LoadingLayer", null, null);
            layers.InsertLayer(loadingLayer, uint.MinValue);
        }

        public void SetDEM(DigitalElevationMap dem)
        {
            lock (gDevice)
            {
                deMap = dem;
                deMap.SetProfile((DEMGraphicsProfile)outProfile);
                sManager.AddSceneEntity(deMap);
                deMap.Init(devIf, sManager);
            }
        }

        public void SetDEMFillMode(FillMode fillMode)
        {
            lock (gDevice)
            {
                demFillMode = fillMode;
            }
        }

        protected override void Render()
        {
            Render(0, gDevice.GetRenderTarget(0));
        }

        public void Render(int index, Surface renderTarget)
        {
            lock (gDevice)
            {
                Surface bb = gDevice.GetRenderTarget(0);
                gDevice.SetRenderTarget(index, renderTarget);
                gDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, background/*Color.Blue*/, 1.0f, 0);

                // setup view
                gPipeline.ProjectionMatrix = view3D.Projection;
                gPipeline.WorldMatrix = Matrix.Scaling(1 - zoomLevel, 1 - zoomLevel, 1 - zoomLevel) * rotation;
                /*pipeline.ViewMatrix = view3D.ViewMatrix;
                pipeline.WorldMatrix = view3D.WorldMatrix * rotation;*/

                view3D.ViewMatrix = gPipeline.ViewMatrix = Matrix.LookAtLH(new Vector3(-5, 5, -5), new Vector3(), new Vector3(0, 1, 0));

                gDevice.RenderState.Lighting = false;

                /*if (renderSource != null)
                {
                    Surface bb = device.GetBackBuffer(0, 0, BackBufferType.Mono);
                    sceneManager.RenderSceneFrame(pipeline, bb.Description.Width, bb.Description.Height);
                }*/
                gDevice.RenderState.FillMode = demFillMode;
                sManager.RenderSceneFrame(gPipeline, -1, -1);
                gDevice.RenderState.FillMode = FillMode.Solid;

                //if (cStatus == Status.Transition)
                //    loadingLayer.RenderLayer(pipeline, renderTarget.Description.Width, renderTarget.Description.Height);

                // draw layers
                if (layers.LayerCount > 0)
                {
                    gDevice.RenderState.ZBufferEnable = false;
                    foreach (ILayer layer in layers)
                    {
                        if (layer.Visible)
                            layer.Draw();
                    }
                    gDevice.RenderState.ZBufferEnable = true;
                }

                gDevice.Present();
                gDevice.SetRenderTarget(0, bb);
            }
        }

        public LoadingLayer LoadingLayer
        {
            get { return loadingLayer; }
        }
    }
}