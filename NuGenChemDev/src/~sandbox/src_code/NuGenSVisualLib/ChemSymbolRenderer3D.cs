
using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Helpers;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Settings;
using Org.OpenScience.CDK.Interfaces;
using Microsoft.DirectX;
using NuGenSVisualLib.Rendering.Shading;
using NuGenSVisualLib.Rendering.Pipelines;

namespace NuGenSVisualLib.Rendering.Chem
{
    //class ChemSymbolRenderer3D : ITypeRenderer3DDx9
    //{
    //    private ChemSymbolTextures textures;
    //    private Device device;
    //    FillMode fillMode;

    //    float[] point = new float[3];

    //    #region ITypeRenderer3DDx9 Members

    //    public void Init(Device device, ISettings settings, CompleteOutputDescription coDesc)
    //    {
    //        this.device = device;

    //        textures = ChemSymbolTextures.Instance;
    //    }

    //    public void BeginDraw(GraphicsPipeline3D pipeline, GeneralShadingDesc gShading,
    //                          AtomShadingDesc aShading)
    //    {
    //        fillMode = device.RenderState.FillMode;

    //        PointSpriteHelper.InitDeviceReady(device, true, 1.0f, 10.0f);
    //        device.VertexFormat = CustomVertex.PositionOnly.Format;
    //        device.RenderState.ZBufferEnable = false;
    //        device.RenderState.AlphaBlendEnable = true;
    //        device.RenderState.SourceBlend = Blend.SourceAlpha;
    //        device.RenderState.DestinationBlend = Blend.DestinationAlpha;

    //        device.RenderState.Lighting = false;
    //    }

    //    public void EndDraw()
    //    {
    //        device.RenderState.FillMode = fillMode;

    //        PointSpriteHelper.DeInitDevice(device);
    //        device.SetTexture(0, null);
    //        device.RenderState.ZBufferEnable = true;
    //        device.RenderState.AlphaBlendEnable = false;
    //        device.SetTexture(0, null);
    //    }

    //    #endregion

    //    public void DrawSymbol(string chSymbol, float[] point)
    //    {
    //        device.SetTexture(0, textures[chSymbol]);
    //        device.DrawUserPrimitives(PrimitiveType.PointList, 1, point);
    //    }

    //    public void DrawAtomSymbol(IAtom atom, Matrix rotation, Vector3 camPos)
    //    {
    //        int period = (int)atom.Properties["Period"];
    //        float scale = 0.2f * period;

    //        Matrix world = Matrix.Multiply(device.Transform.World, Matrix.Identity);

    //        device.Transform.World = Matrix.Translation((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d) * world;
            
    //        // calc pos
    //        //Vector3 atomV = new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d);

    //        //Vector3 atomP = new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d);
    //        //Matrix view = device.Transform.View;// *device.Transform.Projection;
    //        //Vector3 viewV = new Vector3(view.M13, view.M23, view.M33);
    //        ////viewV.TransformCoordinate(world);
    //        //viewV = Vector3.Project(viewV, device.Viewport, device.Transform.Projection, device.Transform.View, world);
    //        ////Vector3 up = new Vector3(0, 1, 0);
    //        ////Vector3 r = Vector3.Cross(up, viewV);

    //        //Vector3 los = viewV;
    //        ////los.Normalize();
    //        //point[0] = (scale * los.X) + 0.1f;
    //        //point[1] = (scale * los.Y) + 0.1f;
    //        //point[2] = (scale * los.Z) + 0.1f;

    //        //CustomVertex.PositionOnly[] verts = new CustomVertex.PositionOnly[2];
    //        //verts[0].Position = new Vector3(0, 0, 0);
    //        //verts[1].Position = new Vector3(point[0], point[1], point[2]);

    //        //device.DrawUserPrimitives(PrimitiveType.LineList, 1, verts);

    //        device.SetTexture(0, textures[atom.Symbol]);
    //        device.DrawUserPrimitives(PrimitiveType.PointList, 1, point);

    //        device.Transform.World = world;
    //    }

    //    #region IDisposable Members

    //    public void Dispose()
    //    {
    //    }

    //    #endregion

    //    #region ITypeRenderer3DDx9 Members


    //    public void InitBuffer(Device device, ISettings settings, CompleteOutputDescription coDesc, int numItems, IAtom[] dataPreiview)
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }

    //    public void BeginDrawToBuffer(GraphicsPipeline3D pipeline, GeneralShadingDesc gShading, AtomShadingDesc aShading)
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }

    //    public void EndDrawToBuffer()
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }

    //    public void DrawBuffer()
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }

    //    #endregion
    //}
}
