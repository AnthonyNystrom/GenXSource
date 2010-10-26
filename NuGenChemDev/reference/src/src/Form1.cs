using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TextureBlending
{
	public class Form1 : System.Windows.Forms.Form
	{	
		private Device device = null;
		
		private Texture t1;
		private Texture t2;
		
		private Effect effect = null;

		public static Matrix viewMatrix;
		public static Matrix projMatrix;

		private Terrain t = null;

        private bool FullScreen = false;

		private Random rand = new Random();

		private float ambient = 0.5f;

		private float ElapsedTime;

		private bool camMode;


		private Vector3 CamPoint = new Vector3();

		#region EffectHandles
		/// <summary>
		/// Percentage Ambient light
		/// </summary>
		private EffectHandle handle1;
		/// <summary>
		/// The world view projection Matrix
		/// </summary>
		private EffectHandle handle2;
		/// <summary>
		/// The location of the light
		/// </summary>
		private EffectHandle handle3;	
		#endregion
		
		private Microsoft.DirectX.Direct3D.Font font;

		public Form1()
		{
			this.Size = new Size(640,480);
			this.KeyUp += new KeyEventHandler(keyup);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
		}

		private void InitializeGraphics()
		{
			Cursor.Hide();
			PresentParameters presentParams = new PresentParameters();

			presentParams.Windowed = ! FullScreen;
			presentParams.SwapEffect = SwapEffect.Discard;
			presentParams.AutoDepthStencilFormat = DepthFormat.D24X8;
			presentParams.EnableAutoDepthStencil = true;
			// If we want a Fullscreen device we need to set up a backbuffer
			if ( FullScreen)
			{
				presentParams.BackBufferCount = 1;
				presentParams.BackBufferFormat = Format.X8R8G8B8; 
				presentParams.BackBufferWidth = 800;
				presentParams.BackBufferHeight = 600;
			}
			Caps hardware = Manager.GetDeviceCaps(0, DeviceType.Hardware);
			
			
			CreateFlags flags = CreateFlags.SoftwareVertexProcessing;

			if (hardware.DeviceCaps.SupportsHardwareTransformAndLight)
				flags = CreateFlags.HardwareVertexProcessing;

			if (hardware.DeviceCaps.SupportsPureDevice)
				flags |= CreateFlags.PureDevice;

			// Pixelshader 2.0 is required
			// If not available create a Reference device ( must have SDK installed )
			if (hardware.PixelShaderVersion >= new Version(2,0) && hardware.VertexShaderVersion >= new Version(1,1))				
				device = new Device(0, DeviceType.Hardware, this, flags, presentParams);
			else
				device = new Device(0, DeviceType.Reference, this, flags, presentParams);

			String s = null;
			
			effect = Effect.FromFile(device, @"shader.fx", null,null,ShaderFlags.None, null, out s);			
            //if ( s != null) 
            //{
            //    // There are Compilation errors show them and then close app
            //    Cursor.Show();
            //    device.Dispose();
            //    this.Visible = false;
            //    MessageBox.Show(s);
            //    return;
            //}

			effect.Technique = "TransformTexture";

			projMatrix = Matrix.PerspectiveFovLH((float)Math.PI/4f, 
				this.Width / this.Height, 1f, 250f);

			t1 = TextureLoader.FromFile(device,@"..\..\grass.bmp");
			t2 = TextureLoader.FromFile(device,@"..\..\rock.bmp");

			effect.SetValue("Texture1", t1);
			effect.SetValue("Texture2", t2);

			handle1 = effect.GetParameter(null,"ambient");
			handle2 = effect.GetParameter(null,"WorldViewProj");
			handle3 = effect.GetParameter(null,"light");

			t = new Terrain(device,0,28);

			font = new Microsoft.DirectX.Direct3D.Font(device,new System.Drawing.Font("Arial",18));
	}


		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{			
			if ( device.Disposed ) return;
			
			// Get elapsed time to calculate FPS
			ElapsedTime = DXUtil.Timer(DirectXTimer.GetElapsedTime);
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.CornflowerBlue, 1.0f, 0);

			device.BeginScene();

			Draw();
			
			// Drawing multiple lines of text on the same sprite is more efficient
			using ( Sprite s = new Sprite(device) )
			{
				s.Begin(SpriteFlags.AlphaBlend);
				int y = 5;				
				font.DrawText(s,string.Format("FPS: {0}",( (int) (1f / ElapsedTime) ).ToString() ),new Rectangle(4,y,0,0),DrawTextFormat.NoClip,Color.FromArgb(64,255,255,255) );
				y += 22;
				font.DrawText(s,string.Format("Ambient light: {0}%",(int) (ambient*100)),new Rectangle(4,y,0,0),DrawTextFormat.NoClip,Color.FromArgb(64,255,255,255));
				s.End();
			}

			device.EndScene();

			device.Present();
			
			// The onPaint event must be called again 
			this.Invalidate();
		}

		private void Draw()
		{
			float AppTime = DXUtil.Timer(DirectXTimer.GetApplicationTime); 
			Vector3 campos = new Vector3(80f*(float) Math.Cos(AppTime),38,80f*(float) Math.Sin(AppTime));
			if ( camMode )
			campos = new Vector3(0,200,-30);

			viewMatrix = Matrix.LookAtLH(campos, new Vector3(), 
				new Vector3(0,1,0));
			Matrix worldViewProj = viewMatrix * projMatrix;
			
			// Update the effect's variables
			effect.SetValue(handle1,ambient);
			effect.SetValue(handle2, worldViewProj);
			effect.SetValue(handle3,new float[] { Terrain.size *0.5f* (float) Math.Sin(AppTime),80,Terrain.size *0.5f* (float)  Math.Cos(AppTime) });
			
			// Begin rendering with the effect
			effect.Begin(0);           
			// There's only one pass
			effect.BeginPass(0);
			t.Draw();
			effect.EndPass();            
			effect.End();
		}

		static void Main() 
		{
			using (Form1 frm = new Form1())
			{
				frm.InitializeGraphics();				
				Application.Run(frm);
			}
			
		}

		private void keyup(object sender, KeyEventArgs e)
		{
			if ( e.KeyCode == Keys.Escape ) 
				this.Close();
			if ( e.KeyCode == Keys.Up ) ambient += 0.1f; 
			if ( e.KeyCode == Keys.Down ) ambient -= 0.1f; 
			if ( e.KeyCode == Keys.Return ) t.renderNormals = ! t.renderNormals; 
			if ( e.KeyCode == Keys.Space) camMode = ! camMode;
			if ( ambient < 0 ) ambient = 0;
			if ( ambient > 1 ) ambient = 1;
		}

	}
}
