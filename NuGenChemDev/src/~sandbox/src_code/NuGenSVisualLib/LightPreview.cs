using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.UI.Lighting;
using NuGenSVisualLib.Rendering;
using NuGenSVisualLib.Rendering.Devices;

namespace NuGenSVisualLib.UI
{
//	/// <summary>
//	/// Summary description for LightPreview.
//	/// </summary>
//	class LightPreview : System.Windows.Forms.UserControl
//	{
//		/// <summary> 
//		/// Required designer variable.
//		/// </summary>
//		private System.ComponentModel.Container components = null;
//
//		Device device;
//		PresentParameters presentParams;
//		Mesh sphere;
//
//        GeneralLightingDesc lightingDesc;
//		LightWrapper editLight;
//		Material previewMat;
//        bool backwards;
//
//        bool lostDev;
//
//		public LightPreview()
//		{
//			// This call is required by the Windows.Forms Form Designer.
//			InitializeComponent();
//
//			presentParams = new PresentParameters();
//			previewMat = new Material();
//
//			previewMat.Diffuse = Color.White;
//			previewMat.Ambient = Color.Gray;
//			previewMat.Emissive = Color.Black;
//			previewMat.Specular = Color.White;
//			previewMat.SpecularSharpness = 50f;
//		}
//
//		/// <summary> 
//		/// Clean up any resources being used.
//		/// </summary>
//		protected override void Dispose( bool disposing )
//		{
//			/*active_  = false;
//			isValid_ = false;*/
//		
//			if (device != null)
//				device.Dispose();
//
//			device = null;
//
//			if( disposing )
//			{
//				if(components != null)
//				{
//					components.Dispose();
//				}
//			}
//			base.Dispose( disposing );
//		}
//
//		public bool InitializeGraphics(OutputSettings outSettings, OutputCaps caps)
//		{
//            if (device == null)
//            {
//                // Set up the presentation parameters
//                presentParams.Windowed = true;// outSettings.Windowed;
//                presentParams.SwapEffect = SwapEffect.Discard;
//                presentParams.AutoDepthStencilFormat = outSettings.DepthFormat;
//                presentParams.EnableAutoDepthStencil = true;
//                // try to bum up AA if pos
//                presentParams.MultiSample = caps.AntiAliasing.MaxSupported;
//                presentParams.BackBufferFormat = outSettings.DeviceFormat;
//
//                CreateDevice(outSettings);
//
//                device.RenderState.Lighting = true;
//
//                sphere = Mesh.Sphere(device, 1.0f, 32, 32);
//
//                device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, (float)this.Width / (float)this.Height, 0.5f, 10.0f);
//                device.Transform.View = Matrix.LookAtLH(new Vector3(2, 2, 2), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
//                //device.Transform.Projection = Matrix.PerspectiveLH(128, 128, 0.1f, 150.0f);
//                device.Transform.World = Matrix.Identity;
//            }
//            else
//            {
//                device.Reset(presentParams);
//            }
//
//			return true;
//		}
//
//        private void CreateDevice(OutputSettings outSettings)
//        {
//			try
//			{
//				// Create the device
//				device = new Device(outSettings.Adapter, outSettings.DeviceType, this, outSettings.CreateFlags, presentParams);
//
//                lostDev = false;
//
//				// setup matrix
//				device.Transform.World = Matrix.Identity;
//				device.Transform.View = Matrix.Identity;
//				device.Transform.Projection = Matrix.Identity;
//
//				// Setup the event handlers for our device
//				device.DeviceLost     += new System.EventHandler(this.DeviceLost);
//				device.DeviceReset    += new System.EventHandler(this.DeviceReset);
//				device.Disposing      += new System.EventHandler(this.DeviceDisposing);
//				device.DeviceResizing += new System.ComponentModel.CancelEventHandler(this.DeviceResizing);
//
//				// Initialize the app's device-dependent objects
//				try
//				{
//					DeviceReset(null, null);
//					return;
//				}
//				catch
//				{
//					// Cleanup before we try again
//					DeviceLost(null, null);
//					DeviceDisposing(null, null);
//
//					device.Dispose();
//					device = null;
//					if (this.Disposing)
//						return;
//				}
//			}
//			catch
//			{
//				// FIXME: If that failed, fall back to the reference rasterizer
//				MessageBox.Show("Failed to create desired device");
//			}
//		}
//
//		#region DX callbacks
//
//		protected virtual void DeviceLost(System.Object sender, System.EventArgs e)
//		{
//            lostDev = true;
//        }
//
//		protected virtual void DeviceDisposing(System.Object sender, System.EventArgs e)
//		{}
//
//		protected virtual void DeviceReset(System.Object sender, System.EventArgs e)
//		{}
//
//		protected virtual void DeviceResizing(object sender, System.ComponentModel.CancelEventArgs e)
//		{
//			// Check to see if we're closing or changing the form style
//			/*if ((isClosing_) || (isChangingFormStyle))
//			{
//				// We are, cancel our reset, and exit
//				e.Cancel = true;
//				return;
//			}
//
//			if (!isWindowActive_)
//				e.Cancel = true;*/
//		}
//
//		#endregion
//
//		protected override void OnGotFocus(System.EventArgs e)
//		{
//			//isWindowActive_ = true;
//			//hasFocus_ = true;
//
//			base.OnGotFocus(e);
//		}
//
//		protected override void OnLostFocus(System.EventArgs e)
//		{
//			//hasFocus_ = false;
//
//			base.OnLostFocus(e);
//		}
//
//		#region Component Designer generated code
//		/// <summary> 
//		/// Required method for Designer support - do not modify 
//		/// the contents of this method with the code editor.
//		/// </summary>
//		private void InitializeComponent()
//		{
//			// 
//			// LightPreview
//			// 
//			this.Name = "LightPreview";
//			this.Paint += new System.Windows.Forms.PaintEventHandler(this.LightPreview_Paint);
//
//		}
//		#endregion
//
//		private void LightPreview_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
//		{
//			if (device != null)
//				DrawPreview();
//		}
//
//		#region Properties
//
//		public GeneralLightingDesc Lights
//		{
//			get
//			{
//				return lightingDesc;
//			}
//			set
//			{
//                lightingDesc = value;
//			}
//		}
//
//		public LightWrapper EditingLight
//		{
//			get
//			{
//				return editLight;
//			}
//			set
//			{
//				editLight = value;
//			}
//		}
//
//        public bool RenderBackwards
//        {
//            get
//            {
//                return backwards;
//            }
//            set
//            {
//                backwards = value;
//            }
//        }
//
//		#endregion
//
//		public void DrawPreview()
//		{
//            try
//            {
//                if (device != null)
//                {
//                    device.TestCooperativeLevel();
//
//                    device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, this.BackColor, 1.0f, 0);
//                    device.BeginScene();
//
//                    if (Enabled)
//                    {
//                        if (lightingDesc != null)
//                        {
//                            device.RenderState.Lighting = true;
//                            lightingDesc.ApplyToDevice(device);
//                        }
//                        else if (editLight != null)
//                        {
//                            device.RenderState.Lighting = true;
//                            device.Lights[0].Enabled = true;
//                            device.Lights[0].Type = LightType.Directional;
//                            if (!backwards)
//                                device.Lights[0].Direction = new Vector3(editLight.DirectionX, editLight.DirectionY,
//                                                                         editLight.DirectionZ);
//                            else
//                                device.Lights[0].Direction = new Vector3(-editLight.DirectionX, -editLight.DirectionY,
//                                                                         editLight.DirectionZ);
//
//                            device.Lights[0].Diffuse = editLight.LightColor;
//
//                            // FIXME: use max lights
//                            device.Lights[1].Enabled = false;
//                            device.Lights[2].Enabled = false;
//                            device.Lights[3].Enabled = false;
//                            device.Lights[4].Enabled = false;
//                            device.Lights[5].Enabled = false;
//                            device.Lights[6].Enabled = false;
//                            device.Lights[7].Enabled = false;
//                        }
//                        else
//                            device.RenderState.Lighting = false;
//
//                        device.Material = previewMat;
//
//                        sphere.DrawSubset(0);
//                    }
//
//                    device.EndScene();
//                    device.Present();
//                }
//            }
//            catch (Exception)
//            {
//                device.Reset(presentParams);
//            }
//		}
//
//        internal void DisposeDevice()
//        {
//            device.Dispose();
//            device = null;
//        }
//    }
}
