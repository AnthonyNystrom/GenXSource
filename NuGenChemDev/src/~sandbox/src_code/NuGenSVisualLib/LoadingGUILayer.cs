using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Pipelines;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using NuGenSVisualLib.Settings;
using System.Drawing;
using NuGenSVisualLib.Rendering.Devices;

namespace NuGenSVisualLib.Rendering.Layers
{
    /// <summary>
    /// Encapsulates a GUI layer capable of displaying the loading status of the scene
    /// </summary>
//    class LoadingGUILayer : SimpleGUILayer
//    {
//        Texture bg, geomClr, geomGs, efxClr, efxGs, timer;
//        CustomVertex.TransformedColoredTextured[] bgQuad, geomQuad, efxQuad;
//        Microsoft.DirectX.Direct3D.Font font;
//
//        bool loadedGeom;
//        bool loadedEfx;
//
//        int alpha = 255;
//
//        public LoadingGUILayer(DeviceInterface devIf, int width, int height)
//            : base(devIf, width, height)
//        { }
//
//        public void Reset()
//        {
//            loadedGeom = false;
//            loadedEfx = false;
//            alpha = 255;
//        }
//
//        #region Properties
//
//        public int Alpha
//        {
//            get { return alpha; }
//            set { alpha = value; }
//        }
//
//        public bool LoadedGeometry
//        {
//            get { return loadedGeom; }
//            set { loadedGeom = value; }
//        }
//
//        public bool LoadedEffects
//        {
//            get { return loadedEfx; }
//            set { loadedEfx = value; }
//        }
//        #endregion
//
//        public override 
//        {
//            // TODO: need optimize
//            float borderV = (1f - (384f / 512f)) / 2;
//            float borderX = ((float)width / 2f) - 256f;
//            float borderY = ((float)height / 2f) - 192f;
//            
//            bgQuad[0].Position = new Vector4(borderX, borderY, 0, 1);
//            bgQuad[0].Tu = 0; bgQuad[0].Tv = borderV;
//            bgQuad[1].Position = new Vector4(borderX + 512, borderY, 0, 1);
//            bgQuad[1].Tu = 1; bgQuad[1].Tv = borderV;
//            bgQuad[2].Position = new Vector4(borderX, borderY + 384, 0, 1);
//            bgQuad[2].Tu = 0; bgQuad[2].Tv = 1 - borderV;
//            bgQuad[3].Position = new Vector4(borderX + 512, borderY + 384, 0, 1);
//            bgQuad[3].Tu = 1; bgQuad[3].Tv = 1 - borderV;
//            bgQuad[0].Color = bgQuad[1].Color = bgQuad[2].Color = bgQuad[3].Color = Color.FromArgb(alpha, 255, 255, 255).ToArgb();
//            
//            geomQuad[0].Position = new Vector4(borderX + 40, borderY + 300, 0, 1);
//            geomQuad[0].Tu = 0; geomQuad[0].Tv = 0;
//            geomQuad[1].Position = new Vector4(borderX + 104, borderY + 300, 0, 1);
//            geomQuad[1].Tu = 1; geomQuad[1].Tv = 0;
//            geomQuad[2].Position = new Vector4(borderX + 40, borderY + 364, 0, 1);
//            geomQuad[2].Tu = 0; geomQuad[2].Tv = 1;
//            geomQuad[3].Position = new Vector4(borderX + 104, borderY + 364, 0, 1);
//            geomQuad[3].Tu = 1; geomQuad[3].Tv = 1;
//            geomQuad[0].Color = geomQuad[1].Color = geomQuad[2].Color = geomQuad[3].Color = Color.FromArgb(alpha, 255, 255, 255).ToArgb();
//
//            Array.Copy(geomQuad, efxQuad, 4);
//            for (int i = 0; i < efxQuad.Length; i++)
//            {
//                efxQuad[i].Position = efxQuad[i].Position + new Vector4(100, 0, 0, 0);
//            }
//
//            device.RenderState.Lighting = false;
//            device.RenderState.ZBufferEnable = false;
//
//            device.BeginScene();
//
//            device.RenderState.CullMode = Cull.None;
//            device.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
//
//            device.TextureState[0].ColorOperation = TextureOperation.Modulate;
//            device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
//            device.TextureState[0].ColorArgument0 = TextureArgument.TextureColor;
//            device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
//
//            device.RenderState.AlphaBlendEnable = true;
//            device.RenderState.SourceBlend = Blend.SourceAlpha;
//            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
//
//            device.SetTexture(0, bg);
//            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, bgQuad);
//
//            if (loadedGeom)
//                device.SetTexture(0, geomClr);
//            else
//            {
//                device.SetTexture(0, geomGs);
//                font.DrawText(null, "Loading Geometry", new Point((int)borderX + 200, (int)borderY + 192), Color.White);
//            }
//            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, geomQuad);
//
//            if (loadedEfx)
//            {
//                device.SetTexture(0, efxClr);
//                font.DrawText(null, "Complete!", new Point((int)borderX + 200, (int)borderY + 192), Color.White);
//            }
//            else
//            {
//                device.SetTexture(0, efxGs);
//                if (loadedGeom)
//                    font.DrawText(null, "Loading Effects", new Point((int)borderX + 200, (int)borderY + 192), Color.White);
//            }
//            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, efxQuad);
//
//            device.RenderState.AlphaBlendEnable = false;
//            device.TextureState[0].AlphaOperation = TextureOperation.SelectArg1;
//            device.TextureState[0].ColorArgument0 = TextureArgument.Current;
//            device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
//
//            device.EndScene();
//
//            device.RenderState.ZBufferEnable = true;
//        }
//
//        public override void LoadResources()
//        {
//            // load loading screen resources
//            string base_path = (string)HashTableSettings.Instance["Base.Path"];
//
//            bg = TextureLoader.FromFile(device, base_path + "/Media/UI/loadingScreen/bg.jpg");
//            geomClr = TextureLoader.FromFile(device, base_path + "/Media/UI/loadingScreen/geomIcon-clr.png");
//            geomGs = TextureLoader.FromFile(device, base_path + "/Media/UI/loadingScreen/geomIcon-gs.png");
//            efxClr = TextureLoader.FromFile(device, base_path + "/Media/UI/loadingScreen/efxIcon-clr.png");
//            efxGs = TextureLoader.FromFile(device, base_path + "/Media/UI/loadingScreen/efxIcon-gs.png");
//
//            bgQuad = new CustomVertex.TransformedColoredTextured[4];
//            geomQuad = new CustomVertex.TransformedColoredTextured[4];
//            efxQuad = new CustomVertex.TransformedColoredTextured[4];
//
//            font = new Microsoft.DirectX.Direct3D.Font(device, 0, 9, FontWeight.Normal, 1, false, CharacterSet.Ansi, Precision.Default, FontQuality.AntiAliased, PitchAndFamily.DefaultPitch, "Segoe UI");
//        }
//
//        public override void UnloadResources()
//        {
//            if (bg != null) { bg.Dispose(); bg = null; }
//            if (geomClr != null) { geomClr.Dispose(); geomClr = null; }
//            if (geomGs != null) { geomGs.Dispose(); geomGs = null; }
//            if (efxClr != null) { efxClr.Dispose(); efxClr = null; }
//            if (efxGs != null) { efxGs.Dispose(); efxGs = null; }
//
//            if (bgQuad != null)
//                bgQuad = null;
//            if (geomQuad != null)
//                geomQuad = null;
//            if (efxQuad != null)
//                efxQuad = null;
//        }
//
//        public override void Dispose()
//        {
//            UnloadResources();
//        }
//    }
}