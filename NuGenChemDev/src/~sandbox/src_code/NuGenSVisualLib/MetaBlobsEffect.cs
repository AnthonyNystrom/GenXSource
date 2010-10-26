using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Devices;
using Microsoft.DirectX;
using NuGenSVisualLib.Rendering.Pipelines;
using System.Drawing;
using NuGenSVisualLib.Rendering.Chem;
using NuGenSVisualLib.Rendering.Shading;
using Org.OpenScience.CDK;
using NuGenSVisualLib.Rendering.Lighting;
using NuGenSVisualLib.Settings;

namespace NuGenSVisualLib.Rendering.Effects
{
    class MetaBlobsEffectSettings : RenderingEffectSettings
    {
        public override RenderingEffect GetEffect(Device refDevice)
        {
            return new MetaBlobsEffect(refDevice, HashTableSettings.Instance);
        }
    }

    class MetaBlobsEffect : ScreenSpaceRenderingEffect
    {
        #region structures and statics

        static int GaussianTexsize = 64;
        static int GaussianHeight = 1;
        static float GaussianDeviation = 0.1f;//25f;

        public struct PointVertex
        {
            public Vector3 pos;
            public float size;
            public Vector3 color;
        }
        //VertexFormats PointVertexFormat = VertexFormats.Position | VertexFormats.PointSize | VertexFormats.Diffuse;

        struct ScreenVertex
        {
            public Vector4 pos;
            public Vector2 tCurr;
            public Vector2 tBack;
            public float fSize;
            public Vector3 vColor;
        }
        // NOTE: seems to work
        VertexFormats ScreenVertexFormat = (VertexFormats)7341060;// VertexFormats.Transformed | VertexFormats.Texture1 | VertexFormats.Texture2 | VertexFormats.Texture3 | VertexFormats.Texture4;

        struct RenderTargetSet
        {
            public Surface[] apCopyRT;
            public Surface[] apBlendRT;

            public RenderTargetSet(int numSurfaces)
            {
                apBlendRT = new Surface[numSurfaces];
                apCopyRT = new Surface[numSurfaces];
            }
        }

        #endregion
        
    //    PointVertex[] blobPoints;
        Format blobTexFormat;
        Effect pEffect;
        int nPasses;
        int nRtUsed;
        EffectHandle hBlendTech;
        VertexBuffer pBlobVB;
        Texture pTexScratch;
        Texture pTexBlob;
        Texture[] pTexGBuffer = new Texture[4];
        RenderTargetSet[] aRTSet = new RenderTargetSet[] { new RenderTargetSet(2), new RenderTargetSet(2) };
        CubeTexture pEnvMap;

        bool needsReset;

        int numPoints;

        public MetaBlobsEffect(Device device, HashTableSettings settings)
            : base("Meta-Blobs", device, settings, 0, 0, 0)
        {
            efxType = EffectType.ScreenSpace;
        }

        public override void LoadResources()
        {
            string base_path = (string)settings["Base.Path"];

            string errors;
            pEffect = Effect.FromFile(device, base_path + "Media/Effects/Blobs.fx", null, null, ShaderFlags.NotCloneable, null, out errors);

            if (errors.Length > 0)
                throw new Exception("HLSL compile error");

            // Initialize the technique for blending
            if (nPasses == 1)
            {
                // Multiple RT available
                hBlendTech = pEffect.GetTechnique("BlobBlend");
            }
            else
            {
                // Single RT. Multiple passes.
                hBlendTech = pEffect.GetTechnique("BlobBlendTwoPasses");
            }

            // Create the environment map
            pEnvMap = TextureLoader.FromCubeFile(device, base_path + "Media/Effects/LobbyCube.dds");
        }

        public override void UnLoadResources()
        {
            if (pEffect != null)
                pEffect.Dispose();
            if (pEnvMap != null)
                pEnvMap.Dispose();
        }

        public override void CheckDeviceCompatibility(OutputSettings settings)
        {
        }

        public override void SetupForDevice(OutputSettings settings)
        {
            if (Manager.GetDeviceCaps(settings.Adapter, settings.DeviceType).NumberSimultaneousRts < 2)
                //{
                nPasses = 2;
            nRtUsed = 1;
            //}
            //else
            //{
            //    nPasses = 1;
            //    nRtUsed = 2;
            //}
            if (!Manager.CheckDeviceFormat(settings.Adapter, settings.DeviceType, Format.R16F,
                Usage.RenderTarget, ResourceType.Surface, DepthFormat.D16))
                blobTexFormat = Format.R32F;
            else
                blobTexFormat = Format.R16F;

            OnReset();
        }

        public override void SetupWithLights(LightingSetup setup)
        {
        }

        public override void OnReset()
        {
            if (numPoints == 0)
            {
                needsReset = true;
                return;
            }
            needsReset = false;

            GenerateGaussianTexture();

            // Create the blob vertex buffer
            pBlobVB = new VertexBuffer(typeof(ScreenVertex), numPoints * 6, device,
                                       Usage.WriteOnly | Usage.Dynamic, ScreenVertexFormat,
                                       Pool.Default);
            // Create the blank texture
            pTexScratch = new Texture(device, 1, 1, 1, Usage.RenderTarget, Format.A16B16G16R16F, Pool.Default);
            // Create buffer textures
            Surface backBufferSurfaceDesc = device.GetBackBuffer(0, 0, BackBufferType.Left);
            for (int i = 0; i < 4; ++i)
            {
                pTexGBuffer[i] = new Texture(device, backBufferSurfaceDesc.Description.Width,
                                             backBufferSurfaceDesc.Description.Height,
                                             1, Usage.RenderTarget, Format.A16B16G16R16F,
                                             Pool.Default);
            }

            // Initialize the render targets
            if (nPasses == 1)
            {
                // Multiple RT
                aRTSet[0].apCopyRT[0] = pTexGBuffer[2].GetSurfaceLevel(0);
                aRTSet[0].apCopyRT[1] = pTexGBuffer[3].GetSurfaceLevel(0);
                aRTSet[0].apBlendRT[0] = pTexGBuffer[0].GetSurfaceLevel(0);
                aRTSet[0].apBlendRT[1] = pTexGBuffer[1].GetSurfaceLevel(0);

                // 2nd pass is not needed. Therefore all RTs are NULL for this pass.
                aRTSet[1].apCopyRT[0] = null;
                aRTSet[1].apCopyRT[1] = null;
                aRTSet[1].apBlendRT[0] = null;
                aRTSet[1].apBlendRT[1] = null;
            }
            else
            {
                // Single RT, multiple passes
                aRTSet[0].apCopyRT[0] = pTexGBuffer[2].GetSurfaceLevel(0);
                aRTSet[1].apCopyRT[0] = pTexGBuffer[3].GetSurfaceLevel(0);
                aRTSet[0].apBlendRT[0] = pTexGBuffer[0].GetSurfaceLevel(0);
                aRTSet[1].apBlendRT[0] = pTexGBuffer[1].GetSurfaceLevel(0);

                // RT 1 is not available. Therefore all RTs are NULL for this index.
                aRTSet[0].apCopyRT[1] = null;
                aRTSet[1].apCopyRT[1] = null;
                aRTSet[0].apBlendRT[1] = null;
                aRTSet[1].apBlendRT[1] = null;
            }
        }

        public override void RenderFrame(GraphicsPipeline3D pipeline)
        {
            pEffect.SetValue("g_mWorldViewProjection", pipeline.ProjectionMatrix);

            Surface[] apSurfOldRenderTarget = new Surface[2];
            Surface pSurfOldDepthStencil = null;
            Surface[] pGBufSurf = new Surface[4];

            // Clear the render target and the zbuffer 
            //device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.FromArgb(0, 45, 50, 170), 1.0f, 0);
            device.BeginScene();

            // Get the initial device surfaces
            apSurfOldRenderTarget[0] = device.GetRenderTarget(0);  // Only RT 0 should've been set.
            pSurfOldDepthStencil = device.DepthStencilSurface;

            // Turn off Z
            device.RenderState.ZBufferEnable = false;
            device.DepthStencilSurface = null;

            // Clear the blank texture
            Surface pSurfBlank = pTexScratch.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurfBlank);
            device.Clear(ClearFlags.Target, Color.Black, 1.0f, 0);
            pSurfBlank.Dispose();

            // clear temp textures
            for (int i = 0; i < 2; i++)
            {
                pGBufSurf[i] = pTexGBuffer[i].GetSurfaceLevel(0);
                device.SetRenderTarget(0, pGBufSurf[i]);
                device.Clear(ClearFlags.Target, Color.Black, 1.0f, 0);
            }

            for (int i = 2; i < 4; i++)
                pGBufSurf[i] = pTexGBuffer[i].GetSurfaceLevel(0);

            device.SetStreamSource(0, pBlobVB, 0, System.Runtime.InteropServices.Marshal.SizeOf(typeof(ScreenVertex)));
            device.VertexFormat = ScreenVertexFormat;

            // Render the blobs
            pEffect.Technique = hBlendTech;
            int nNumPasses;
            for (int i = 0; i < numPoints; i++)
            {
                // Copy bits off of render target into scratch surface [for blending].
                pEffect.SetValue("g_tSourceBlob", pTexScratch);
                pEffect.SetValue("g_tNormalBuffer", pTexGBuffer[0]);
                pEffect.SetValue("g_tColorBuffer", pTexGBuffer[1]);

                nNumPasses = pEffect.Begin(FX.None);
                for (int iPass = 0; iPass < nNumPasses; iPass++)
                {
                    for (int rt = 0; rt < nRtUsed; rt++)
                        device.SetRenderTarget(rt, aRTSet[iPass].apCopyRT[rt]);

                    pEffect.BeginPass(iPass);

                    device.DrawPrimitives(PrimitiveType.TriangleList, i * 6, 2);
                    pEffect.EndPass();
                }
                pEffect.End();

                // Render the blob
                pEffect.SetValue("g_tSourceBlob", pTexBlob);
                pEffect.SetValue("g_tNormalBuffer", pTexGBuffer[2]);
                pEffect.SetValue("g_tColorBuffer", pTexGBuffer[3]);

                nNumPasses = pEffect.Begin(FX.None);
                for (int iPass = 0; iPass < nNumPasses; iPass++)
                {
                    for (int rt = 0; rt < nRtUsed; rt++)
                        device.SetRenderTarget(rt, aRTSet[iPass].apBlendRT[rt]);

                    pEffect.BeginPass(iPass);

                    device.DrawPrimitives(PrimitiveType.TriangleList, i * 6, 2);
                    pEffect.EndPass();
                }

                pEffect.End();
            }

            // Restore initial device surfaces
            device.DepthStencilSurface = pSurfOldDepthStencil;

            for (int rt = 0; rt < nRtUsed; rt++)
                device.SetRenderTarget(rt, apSurfOldRenderTarget[rt]);

            // Light and composite blobs into backbuffer
            pEffect.Technique = "BlobLight";

            nNumPasses = pEffect.Begin(FX.None);

            for (int iPass = 0; iPass < nNumPasses; iPass++)
            {
                pEffect.BeginPass(iPass);

                for (int i = 0; i < numPoints; i++)
                {
                    pEffect.SetValue("g_tSourceBlob", pTexGBuffer[0]);
                    pEffect.SetValue("g_tColorBuffer", pTexGBuffer[1]);
                    pEffect.SetValue("g_tEnvMap", pEnvMap);
                    pEffect.CommitChanges();

                    RenderFullScreenQuad(1.0f);
                }
                pEffect.EndPass();
            }

            pEffect.End();
            device.EndScene();

            for (int rt = 0; rt < nRtUsed; rt++)
            {
                if (apSurfOldRenderTarget[rt] != null)
                    apSurfOldRenderTarget[rt].Dispose();
            }
            pSurfOldDepthStencil.Dispose();

            for (int i = 0; i < 4; i++)
            {
                pGBufSurf[i].Dispose();
            }
        }

//        public override bool DesiredGeomDataFields(out DataFields[] fields, bool exclusive)
//        {
//            fields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
//                                        new DataFields(VertexFormats.PointSize, "SIZEFLOAT"),
//                                        new DataFields(VertexFormats.Diffuse, "DIFFUSE") };
//            return true;
//        }

        public override bool DesiredFrameData(out DataFields[] fields)
        {
            fields = new DataFields[] { new DataFields(VertexFormats.Transformed, "POSITION"),
                                        new DataFields(VertexFormats.Texture1, "TCURR"),
                                        new DataFields(VertexFormats.Texture2, "TBACK"),
                                        new DataFields(VertexFormats.PointSize, "FSIZE"),
                                        new DataFields(VertexFormats.Texture3, "VCOLOR") };
            return true;
        }

        public override void UpdateFrameData(BufferedGeometryData[] geomData, GraphicsPipeline3D pipeline)
        {
            PointVertex[] blobPoints = (PointVertex[])geomData[0].vBuffers[0].Buffer.Lock(0, LockFlags.None);
            numPoints = blobPoints.Length;

            if (needsReset)
                OnReset();

            FillBlobVB(pipeline.WorldMatrix * pipeline.ViewMatrix, pipeline.ProjectionMatrix, ref blobPoints);
            
            geomData[0].vBuffers[0].Buffer.Unlock();
        }

        private void FillBlobVB(Matrix pmatWorldView, Matrix pmatProjection, ref PointVertex[] blobPoints)
        {
            ScreenVertex[] pBlobVertex = (ScreenVertex[])pBlobVB.Lock(0, LockFlags.None);
            PointVertex[] blobPos = new PointVertex[blobPoints.Length];

            for (int i = 0; i < blobPoints.Length; i++)
            {
                //transform point to camera space
                Vector4 blobPosCamera = Vector3.Transform(blobPoints[i].pos, pmatWorldView);

                blobPos[i] = blobPoints[i];
                blobPos[i].pos.X = blobPosCamera.X;
                blobPos[i].pos.Y = blobPosCamera.Y;
                blobPos[i].pos.Z = blobPosCamera.Z;
            }

            int posCount = 0;
            for (int i = 0; i < blobPoints.Length; i++)
            {
                Vector4 blobScreenPos;

                // For calculating billboarding
                Vector4 billOfs = new Vector4(blobPos[i].size, blobPos[i].size, blobPos[i].pos.Z, 1);
                Vector4 billOfsScreen;

                // Transform to screenspace
                blobScreenPos = Vector3.Transform(blobPos[i].pos, pmatProjection);
                billOfsScreen = Vector4.Transform(billOfs, pmatProjection);

                // Project
                blobScreenPos = Vector4.Scale(blobScreenPos, 1.0f / blobScreenPos.W);
                billOfsScreen = Vector4.Scale(billOfsScreen, 1.0f / billOfsScreen.W);

                Vector2[] vTexCoords = new Vector2[]
                {
                    new Vector2(0.0f, 0.0f),
                    new Vector2(1.0f, 0.0f),
                    new Vector2(0.0f, 1.0f),
                    new Vector2(0.0f, 1.0f),
                    new Vector2(1.0f, 0.0f),
                    new Vector2(1.0f, 1.0f)
                };

                Vector4[] vPosOffset = new Vector4[]
                {
                    new Vector4(-billOfsScreen.X,-billOfsScreen.Y, 0.0f, 0.0f),
                    new Vector4( billOfsScreen.X,-billOfsScreen.Y, 0.0f, 0.0f),
                    new Vector4(-billOfsScreen.X, billOfsScreen.Y, 0.0f, 0.0f),
                    new Vector4(-billOfsScreen.X, billOfsScreen.Y, 0.0f, 0.0f),
                    new Vector4( billOfsScreen.X,-billOfsScreen.Y, 0.0f, 0.0f),
                    new Vector4( billOfsScreen.X, billOfsScreen.Y, 0.0f, 0.0f)
                };

                SurfaceDescription pBackBufferSurfaceDesc = device.GetBackBuffer(0, 0, BackBufferType.Left).Description;

                // Set constants across quad
                for (int j = 0; j < 6; j++)
                {
                    // Scale to pixels
                    pBlobVertex[posCount].pos = Vector4.Add(blobScreenPos, vPosOffset[j]);

                    pBlobVertex[posCount].pos.X *= pBackBufferSurfaceDesc.Width;
                    pBlobVertex[posCount].pos.Y *= pBackBufferSurfaceDesc.Height;
                    pBlobVertex[posCount].pos.X += 0.5f * pBackBufferSurfaceDesc.Width;
                    pBlobVertex[posCount].pos.Y += 0.5f * pBackBufferSurfaceDesc.Height;

                    pBlobVertex[posCount].tCurr = vTexCoords[j];
                    pBlobVertex[posCount].tBack = new Vector2((0.5f + pBlobVertex[posCount].pos.X) * (1.0f / pBackBufferSurfaceDesc.Width),
                                                           (0.5f + pBlobVertex[posCount].pos.Y) * (1.0f / pBackBufferSurfaceDesc.Height));
                    pBlobVertex[posCount].fSize = blobPos[i].size;
                    pBlobVertex[posCount].vColor = blobPoints[i].color;

                    posCount++;
                }
            }
            pBlobVB.Unlock();
        }

        private void RenderFullScreenQuad(float fDepth)
        {
            ScreenVertex[] aVertices = new ScreenVertex[4];

            SurfaceDescription pBackBufferSurfaceDesc = device.GetBackBuffer(0, 0, BackBufferType.Left).Description;

            aVertices[0].pos = new Vector4(-0.5f, -0.5f, fDepth, fDepth);
            aVertices[1].pos = new Vector4(pBackBufferSurfaceDesc.Width - 0.5f, -0.5f, fDepth, fDepth);
            aVertices[2].pos = new Vector4(-0.5f, pBackBufferSurfaceDesc.Height - 0.5f, fDepth, fDepth);
            aVertices[3].pos = new Vector4(pBackBufferSurfaceDesc.Width - 0.5f, pBackBufferSurfaceDesc.Height - 0.5f, fDepth, fDepth);

            aVertices[0].tCurr = new Vector2(0.0f, 0.0f);
            aVertices[1].tCurr = new Vector2(1.0f, 0.0f);
            aVertices[2].tCurr = new Vector2(0.0f, 1.0f);
            aVertices[3].tCurr = new Vector2(1.0f, 1.0f);

            for (int i = 0; i < 4; i++)
            {
                aVertices[i].tBack = aVertices[i].tCurr;
                aVertices[i].tBack.X += (1.0f / pBackBufferSurfaceDesc.Width);
                aVertices[i].tBack.Y += (1.0f / pBackBufferSurfaceDesc.Height);
                aVertices[i].fSize = 0.0f;
            }

            device.VertexFormat = ScreenVertexFormat;
            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, aVertices);
        }

        private void GenerateGaussianTexture()
        {
            Surface pBlobTemp = null;
            Surface pBlobNew = null;

            // Create a temporary texture
            Texture texTemp;
            texTemp = new Texture(device, GaussianTexsize, GaussianTexsize, 1, Usage.Dynamic, Format.R32F, Pool.Default);
            // Create the gaussian texture
            pTexBlob = new Texture(device, GaussianTexsize, GaussianTexsize, 1, Usage.Dynamic, blobTexFormat, Pool.Default);

            // Fill in the gaussian texture data
            GraphicsStream Rect = texTemp.LockRectangle(0, LockFlags.None);
            float dx, dy, I;

            int margin = (int)(GaussianTexsize * 0.1f);
            float size = GaussianTexsize - margin;

            //Rect.Seek(4 * margin * GaussianTexsize, System.IO.SeekOrigin.Begin);
            for (int v = 0; v < GaussianTexsize; v++)
            {
                for (int u = 0; u < GaussianTexsize; u++)
                {
                    dx = 2.0f * (float)u / (float)GaussianTexsize - 1.0f;
                    dy = 2.0f * (float)v / (float)GaussianTexsize - 1.0f;
                    I = GaussianHeight * (float)Math.Exp(-(dx * dx + dy * dy) / GaussianDeviation);

                    byte[] data = BitConverter.GetBytes(I);
                    Rect.Write(data, 0, data.Length);
                }
            }

            texTemp.UnlockRectangle(0);

            // Copy the temporary surface to the stored gaussian texture
            pBlobTemp = texTemp.GetSurfaceLevel(0);
            pBlobNew = pTexBlob.GetSurfaceLevel(0);
            SurfaceLoader.FromSurface(pBlobNew, pBlobTemp, Filter.None, 0);

            pBlobTemp.Dispose();
            pBlobNew.Dispose();
            texTemp.Dispose();
        }
    }
}