using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using NuGenCRBase.AvalonBridge;
using NuGenCRBase.Managed.MDX1.Direct3D;
using System.IO;
using NuGenCRBase.SceneFormats.ThreeDS;

namespace NuGenCRBase.Managed.MDX1.Direct3D
{
    /// <summary>
    /// Encapsulates a basic control base code for rendering avalon-bridge in Managed DirectX
    /// </summary>
    public abstract partial class ABRenderViewControl : UserControl
    {
        protected Device device;
        protected PresentParameters pParams;
        protected string deviceFailMsg = "Managed DirectX: No device created";
        protected Matrix projMat, viewMat;

        internal ABScene3D abScene;

        internal GraphicsDevice3DRequirements minReqs, desiredReqs;

        private bool rotatingView = false;
        private Vector2 startMousePos;
        private Vector2 mousePosScaled;

        internal ABRenderViewControl(bool delayCreateDx)
        {
            InitializeComponent();

            minReqs = new GraphicsDevice3DRequirements(MultiSampleType.None, DeviceType.Hardware,
                                                       new Format[] { Format.X8R8G8B8 }, 1, true,
                                                       new DepthFormat[] { DepthFormat.D16 },
                                                       false, false, null, null, Format.X8R8G8B8 );
            desiredReqs = new GraphicsDevice3DRequirements(MultiSampleType.FourSamples, DeviceType.Hardware,
                                                           new Format[] { Format.X8R8G8B8 }, 1, true,
                                                           new DepthFormat[] { DepthFormat.D16 },
                                                           false, true, null, null, Format.X8R8G8B8);

            if (!delayCreateDx)
                CreateDxDevice();
        }

        internal bool LoadABSupportedScene(string file)
        {
            // determine file type
            string ext = Path.GetExtension(file).ToLower();
            ABScene3D scene = null;
            if (ext == ".3ds")
            {
                SceneLoader3ds loader = new SceneLoader3ds();
                scene = loader.LoadScene(file);
            }
            if (scene != null)
            {
                if (abScene != null)
                    abScene.Dispose();
                abScene = scene;
                abScene.CalcBounds();
                foreach (ABModel3D model in scene.Models)
                {
                    model.BuildBuffers(device);
                }

                // set defaults if none loaded
                if (abScene.Cameras == null)
                {
                    abScene.Cameras = new ABCamera[1];
                    abScene.Cameras[0] = new ABCameraSpherical(abScene.BoundingSphere * 2f, abScene.Origin);
                    abScene.Cameras[0].Scroll(new Vector3());
                    abScene.CurrentCamera = 0;
                }
                if (abScene.Lights == null)
                {
                    abScene.Lights = new ABLight[] { new ABDirectionalLight(new Vector3(-1, -1, -1), Color.White) };
                }

                return true;
            }
            return false;
        }

        protected void CreateDxDevice()
        {
            if (GraphicsDeviceManager.CheckAdapterMeetsRequirements(0, minReqs))
            {
                GraphicsDevice3DOutputDescription outDesc = GraphicsDeviceManager.CreateOutputDescription(0, minReqs, desiredReqs);
                if (!GraphicsDeviceManager.CreateGraphicsDevice3D(outDesc, this, out outDesc, out device, out pParams))
                    deviceFailMsg = "Managed DirectX: Failed to create device";

                if (device != null)
                {
                    SetupView();
                }
            }
            else
                deviceFailMsg = "Managed DirectX: Adapter does not meet minimum requirements";
        }

        protected virtual void SetupView()
        {
            float near = 0.1f;
            float far = 1000f;
            if (abScene != null)
            {
                far = abScene.BoundingSphere * 5;
                near = far * 0.01f;
            }
            projMat = Matrix.PerspectiveFovLH((float)Math.PI / 4f, (float)Width / (float)Height, near, far);
            //viewMat = Matrix.LookAtLH(new Vector3(40, 20, 20), new Vector3(), new Vector3(0, 1, 0));
        }

        protected void ResetDevice()
        {
            device.Reset(pParams);
        }

        protected void RenderDxScene()
        {
            //device.RenderState.FillMode = FillMode.WireFrame;
            //device.RenderState.CullMode = Cull.None;

            if (abScene.Lights != null)
            {
                for (int light = 0; light < abScene.Lights.Length; light++)
                {
                    abScene.Lights[light].SetupDx(device.Lights[light]);
                }

                device.RenderState.Lighting = true;

                Microsoft.DirectX.Direct3D.Material mt = new Microsoft.DirectX.Direct3D.Material();
                mt.AmbientColor = ColorValue.FromArgb(Color.Black.ToArgb());
                mt.DiffuseColor = ColorValue.FromArgb(Color.White.ToArgb());
                device.RenderState.DiffuseMaterialSource = ColorSource.Material;
                device.Material = mt;
                device.RenderState.AmbientColor = Color.Black.ToArgb();
            }
            else
                device.RenderState.Lighting = false;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackColor, 1, 0);

            if (abScene.CurrentCamera != -1)
            {
                viewMat = Matrix.LookAtLH(abScene.Cameras[abScene.CurrentCamera].Position,
                                          abScene.Cameras[abScene.CurrentCamera].Target,
                                          new Vector3(0, 1, 0));
            }
            else
                viewMat = Matrix.LookAtLH(new Vector3(40, 20, 20), new Vector3(), new Vector3(0, 1, 0));

            device.Transform.Projection = projMat;
            device.Transform.View = viewMat;
            device.Transform.World = Matrix.Identity;

            // render avalon view
            foreach (ABModel3D model in abScene.Models)
            {
                device.BeginScene();
                model.Render(device);
                device.EndScene();
            }

            device.Present();
        }

        private void ABRenderViewControl_MouseDown(object sender, MouseEventArgs e)
        {
            rotatingView = (e.Button == MouseButtons.Left);
            if (rotatingView)
                startMousePos = new Vector2(e.X, e.Y);
        }

        private void ABRenderViewControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                rotatingView = false;
        }

        private void ABRenderViewControl_MouseMove(object sender, MouseEventArgs e)
        {
            if ((int)e.Button > 0)
            {
                float scaleX = 1f / (float)Width;
                float scaleY = 1f / (float)Height;

                float rMoveX = (float)(e.X - startMousePos.X) * scaleX;
                float rMoveY = (float)(e.Y - startMousePos.Y) * scaleY;

                mousePosScaled.X += rMoveX;
                mousePosScaled.Y += rMoveY;

                if (mousePosScaled.X >= 1)
                    mousePosScaled.X -= 1;
                else if (mousePosScaled.X < 0)
                    mousePosScaled.X += 1;
                if (mousePosScaled.Y >= 1)
                    mousePosScaled.Y -= 1;
                else if (mousePosScaled.Y < 0)
                    mousePosScaled.Y += 1;

                abScene.Cameras[abScene.CurrentCamera].Scroll(new Vector3(mousePosScaled.X, mousePosScaled.Y, 0));

                startMousePos.X = e.X;
                startMousePos.Y = e.Y;

                Invalidate();
            }
        }

        internal bool TrySelectPolygon(int x, int y)
        {
            Vector3 pickRay, pickRayOrigin, pickRayDir;

	        Matrix projection = projMat;
	        Matrix view = viewMat;
	        Matrix world = Matrix.Identity;

	        // Compute the vector of the pick ray in screen space
	        pickRay.X = (((2.0f * x) / (float)this.Width) - 1f) / projection.M11;
	        pickRay.Y = -(((2.0f * y) / (float)this.Height) - 1f) /  projection.M22;
	        pickRay.Z = 1.0f;

	        Matrix matInverseView = Matrix.Invert(view);

	        // Transform the screen space pick ray into 3D space
	        pickRayDir.X  = pickRay.X * matInverseView.M11 + pickRay.Y * matInverseView.M21 + pickRay.Z * matInverseView.M31;
	        pickRayDir.Y  = pickRay.X * matInverseView.M12 + pickRay.Y * matInverseView.M22 + pickRay.Z * matInverseView.M32;
	        pickRayDir.Z  = pickRay.X * matInverseView.M13 + pickRay.Y * matInverseView.M23 + pickRay.Z * matInverseView.M33;
	        pickRayDir.Normalize();

	        pickRayOrigin.X = matInverseView.M41;
	        pickRayOrigin.Y = matInverseView.M42;
	        pickRayOrigin.Z = matInverseView.M43;

	        pickRayOrigin.Add(Vector3.Multiply(pickRayDir, 1.0f));	//	near plane

	        // transform trough world space
	        Matrix inverseWorld = Matrix.Invert(world); // scene-world == object-world

	        Vector3 localOrigin = Vector3.TransformCoordinate(pickRayOrigin, inverseWorld);
	        Vector3 localDir = Vector3.TransformNormal(pickRayDir, inverseWorld);

	        return TrySelect(localOrigin, localDir);
        }

        private bool TrySelect(Vector3 origin, Vector3 dir)
        {
            Vector3 rayOrigin = origin;
            Vector3 rayDir = dir;
            // try intersections with ALL triangles
            // do slow way for simplicity
            Plane testPlane;
            Vector3 triP1, triP2, triP3;
            IntersectInformation interInf;
            int nearestIdx = -1;
            float nearestDist = -1;

            foreach (ABModel3D model in abScene.Models)
            {
                if (model.Geometry.PrimIndices == null)
                {
                    for (int vertex = 0; vertex < model.Geometry.Vertices.Length; vertex += 3)
                    {
                        Geometry.IntersectTri(model.Geometry.Vertices[vertex],
                                              model.Geometry.Vertices[vertex + 1],
                                              model.Geometry.Vertices[vertex + 2], rayOrigin, rayDir, out interInf);

                        if (interInf.Dist != 0)
                        {
                            // check dist
                            if (nearestDist == -1 || interInf.Dist < nearestDist)
                            {
                                // check not back facing
                                // set as nearest intersection
                                nearestDist = interInf.Dist;
                                nearestIdx = vertex;
                            }
                        }

                        if (interInf.Dist != 0)
                        {
                            // check dist
                            if (nearestDist == -1 || interInf.Dist < nearestDist)
                            {
                                // check not back facing
                                // set as nearest intersection
                                nearestDist = interInf.Dist;
                                nearestIdx = vertex;
                            }
                        }
                    }
                }
            }

            if (nearestIdx != -1)
            {
                //selectedTri = nearestIdx;
                //PolyIsSelected = true;

                // build outline buffer
                //if (selectedPolyOutlineVB != nullptr)
                //    delete selectedPolyOutlineVB;

                //selectedPolyOutlineVB = gcnew VertexBuffer(CustomVertex::PositionOnly::typeid, 5, graphicsDevice,
                //                                           Usage::None, CustomVertex::PositionOnly::Format,
                //                                           Pool::Managed);
                //array<CustomVertex::PositionOnly>^ verts = (array<CustomVertex::PositionOnly>^)selectedPolyOutlineVB->Lock(0, LockFlags::None);

                //int i = hIdx;
                //int j = vIdx;

                //verts[0].Position = Vector3((float)Tre2[i*3, j],
                //                            (float)Tre2[i*3 + 1, j],
                //                            (float)Tre2[i*3 + 2, j]);
                //selectedPolyPoints[0] = verts[0].Position;
                //SelectedPolygon[0] = Point3F(verts[0].Position);

                //verts[1].Position = Vector3((float)Tre2[(i+1)*3, j],
                //                            (float)Tre2[(i+1)*3 + 1, j],
                //                            (float)Tre2[(i+1)*3 + 2, j]);
                //selectedPolyPoints[1] = verts[1].Position;
                //SelectedPolygon[1] = Point3F(verts[1].Position);

                //verts[2].Position = Vector3((float)Tre2[(i+1)*3, j+1],
                //                            (float)Tre2[(i+1)*3 + 1, j+1],
                //                            (float)Tre2[(i+1)*3 + 2, j+1]);
                //selectedPolyPoints[2] = verts[2].Position;
                //SelectedPolygon[2] = Point3F(verts[2].Position);

                //verts[3].Position = Vector3((float)Tre2[i*3, j+1],
                //                            (float)Tre2[i*3 + 1, j+1],
                //                            (float)Tre2[i*3 + 2, j+1]);
                //selectedPolyPoints[3] = verts[3].Position;
                //SelectedPolygon[3] = Point3F(verts[3].Position);

                //verts[4].Position = verts[0].Position;

                //selectedPolyOutlineVB->Unlock();
                return true;
            }
            return false;
        }
    }
}