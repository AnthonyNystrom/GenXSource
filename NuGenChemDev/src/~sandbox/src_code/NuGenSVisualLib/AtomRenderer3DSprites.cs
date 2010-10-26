using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Shading;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Helpers;
using System.IO;
using System.Reflection;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Pipelines;
using Microsoft.DirectX;


namespace NuGenSVisualLib.Rendering.Chem
{
    /// <summary>
    /// Encapsulates the rendering of atoms as sprites
    /// </summary>
    //class AtomRenderer3DSprites : IAtomRenderer3DDx9
    //{
    //    private Device device;
    //    private FillMode fillMode;

    //    private Texture spriteTexture;
    //    private bool scale;

    //    private VertexBuffer[] atomBuffers;
    //    private int[] bufferAtomSizes;
    //    private int[] vertexIndices;
    //    private CustomVertex.PositionOnly[][] atomVerts;
    //    private int[] numAtoms;

    //    #region IAtomRenderer3DDx9 Members

    //    public void Init(Device device, ISettings settings, CompleteOutputDescription coDesc)
    //    {
    //        this.device = device;

    //        Stream texstm = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenSVisualLib.Atom.PNG");
    //        spriteTexture = TextureLoader.FromStream(device, texstm);

    //        scale = false;
    //        try
    //        {
    //            scale = (bool)settings["Rendering.Atoms.Sprites.Scale"];
    //        }
    //        catch { }
    //    }

    //    public void DrawAtom(GraphicsPipeline3D pipeline, IAtom atom,
    //                         GeneralShadingDesc gShading, AtomShadingDesc aShading)
    //    {
    //        device.DrawUserPrimitives(PrimitiveType.PointList, 1, PointSpriteHelper.zeroPoint);
    //    }

    //    public void BeginDraw(GraphicsPipeline3D pipeline, GeneralShadingDesc gShading,
    //                          AtomShadingDesc aShading)
    //    {
    //        fillMode = device.RenderState.FillMode;
    //        device.RenderState.FillMode = FillMode.Solid;

    //        PointSpriteHelper.InitDeviceReady(device, true, 1.0f, 1.0f);

    //        device.SetTexture(0, spriteTexture);
    //        device.VertexFormat = CustomVertex.PositionOnly.Format;

    //        device.RenderState.AlphaBlendEnable = true;
    //        device.RenderState.SourceBlend = Blend.SourceAlpha;
    //        device.RenderState.DestinationBlend = Blend.DestinationAlpha;
    //    }

    //    public void EndDraw()
    //    {
    //        PointSpriteHelper.DeInitDevice(device);

    //        device.RenderState.FillMode = fillMode;

    //        device.RenderState.AlphaBlendEnable = false;
    //    }

    //    #endregion

    //    #region IDisposable Members

    //    public void Dispose()
    //    {
    //        spriteTexture.Dispose();
    //    }

    //    #endregion

    //    #region ITypeRenderer3DDx9 Members

    //    public void EndDrawToBuffer()
    //    {
    //        for (int size = 0; size < bufferAtomSizes.Length; size++)
    //        {
    //            atomBuffers[size].Unlock();
    //        }
    //    }

    //    public void DrawBuffer()
    //    {
    //        fillMode = device.RenderState.FillMode;
    //        device.RenderState.FillMode = FillMode.Solid;

    //        device.SetTexture(0, spriteTexture);
    //        device.VertexFormat = CustomVertex.PositionOnly.Format;

    //        device.RenderState.AlphaBlendEnable = true;
    //        device.RenderState.SourceBlend = Blend.SourceAlpha;
    //        device.RenderState.DestinationBlend = Blend.DestinationAlpha;

    //        // draw each size set
    //        for (int size = 0; size < bufferAtomSizes.Length; size++)
    //        {
    //            // setup this size
    //            device.SetStreamSource(0, atomBuffers[size], 0);
    //            PointSpriteHelper.InitDeviceReady(device, true, bufferAtomSizes[size], 1.0f);

    //            device.DrawUserPrimitives(PrimitiveType.PointList, 1, PointSpriteHelper.zeroPoint);
    //        }

    //        PointSpriteHelper.DeInitDevice(device);
    //        device.RenderState.FillMode = fillMode;
    //        device.RenderState.AlphaBlendEnable = false;
    //    }

    //    #endregion

    //    #region IAtomRenderer3DDx9 Members

    //    public void DrawAtomToBuffer(GraphicsPipeline3D pipeline, IAtom atom, GeneralShadingDesc gShading,
    //                                 AtomShadingDesc bShading)
    //    {
    //        // TODO: Change over to allow coloured atom sprites
            
    //        // find right size
    //        int period = 1;
    //        if (atom.Properties.ContainsKey("Period"))
    //            period = (int)atom.Properties["Period"];

    //        // NOTE: Do via sorted list?
    //        int szIdx;
    //        for (szIdx = 0; szIdx < bufferAtomSizes.Length; szIdx++)
    //        {
    //            if (bufferAtomSizes[szIdx] == period)
    //                break;
    //        }

    //        // add point to right buffer
    //        atomVerts[szIdx][vertexIndices[szIdx]++].Position = new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d);
    //    }

    //    public void InitBuffer(Device device, ISettings settings, CompleteOutputDescription coDesc,
    //                           int numItems, List<IAtom[]> dataPreview)
    //    {
    //        this.device = device;

    //        // count how many sizes and how many of each size
    //        SortedList<int, int> sizes = new SortedList<int,int>();
    //        foreach (IAtom[] atoms in dataPreview)
    //        {
    //            foreach (IAtom atom in atoms)
    //            {
    //                int period = 1;
    //                if (atom.Properties.ContainsKey("Period"))
    //                    period = (int)atom.Properties["Period"];

    //                if (sizes.ContainsKey(period))
    //                    sizes[period]++;
    //                else
    //                    sizes.Add(period, 1);
    //            }
    //        }

    //        // create buffer for each size
    //        atomBuffers = new VertexBuffer[sizes.Count];
    //        bufferAtomSizes = new int[sizes.Count];
    //        numAtoms = new int[sizes.Count];
    //        atomVerts = new CustomVertex.PositionOnly[sizes.Count][];
    //        vertexIndices = new int[sizes.Count];

    //        int bIdx = 0;
    //        foreach (KeyValuePair<int, int> size in sizes)
    //        {
    //            atomBuffers[bIdx] = new VertexBuffer(typeof(CustomVertex.PositionOnly), size.Value, device,
    //                                                 Usage.None, CustomVertex.PositionOnly.Format, Pool.Managed);
    //            bufferAtomSizes[bIdx] = size.Key;
    //            numAtoms[bIdx] = size.Value;
    //            bIdx++;
    //        }
    //        sizes.Clear();
    //    }

    //    public void BeginDrawToBuffer(GraphicsPipeline3D pipeline, GeneralShadingDesc gShading,
    //                                  AtomShadingDesc bShading)
    //    {
    //        for (int b=0; b < atomBuffers.Length; b++)
    //        {
    //            atomVerts[b] = (CustomVertex.PositionOnly[])atomBuffers[b].Lock(0, LockFlags.None);
    //            vertexIndices[b] = 0;
    //        }
    //    }

    //    #endregion

    //    #region IAtomRenderer3DDx9 Members

    //    public bool NeedsPreviewData
    //    {
    //        get { return true; }
    //    }

    //    #endregion
    //}
}
