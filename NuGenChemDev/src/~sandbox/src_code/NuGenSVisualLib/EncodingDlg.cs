using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Recording;
using System.Threading;
using NuGenVideoEnc;
using Microsoft.DirectX.Direct3D;
using System.Drawing.Imaging;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Recording
{
    partial class EncodingDlg : Form
    {
        ViewRecording recording;
        RecordingSettings settings;
        IRecordingRenderer renderer;

        Thread workThread;
        bool complete;
        string outFilename;

        public EncodingDlg(ViewRecording recording, RecordingSettings settings,
                           string outFilename, IRecordingRenderer renderer)
        {
            InitializeComponent();

            this.recording = recording;
            this.settings = settings;
            this.renderer = renderer;
            this.outFilename = outFilename;

            workThread = new Thread(this.EncodeProcess);
        }

        private void EncodingDlg_Load(object sender, EventArgs e)
        {
            // start thread
            workThread.Start();
        }

        private void EncodeProcess()
        {
            try
            {
                // calc info
                int totalFrames = (int)((float)settings.FramesPerSecond * (float)recording.Duration.Seconds);
                // start encoding
                IOutputStream stream = VideoEncodingInterface.CreateVideoStream(outFilename, settings.Codec,
                                                                                settings.Width, settings.Height,
                                                                                16, settings.FramesPerSecond, TimeSpan.MinValue, null);
                // create local buffer
                int pixels = settings.Width * settings.Height;
                byte[] buffer = new byte[settings.Width * settings.Height * 4];
                Device device = renderer.Device;
                Surface bb = device.GetBackBuffer(0, 0, BackBufferType.Mono);
                Texture t = new Texture(device, settings.Width, settings.Height, 1, bb.Description.Usage, bb.Description.Format, bb.Description.Pool);
                Surface target = t.GetSurfaceLevel(0);

                Texture sysT = new Texture(device, settings.Width, settings.Height, 1, bb.Description.Usage, bb.Description.Format, bb.Description.Pool);
                Surface sysTarget = sysT.GetSurfaceLevel(0);

                byte[] pBuf = new byte[4];
                for (int frame = 0; frame < totalFrames; frame++)
                {
                    // Draw frame
                    renderer.Render(0, target);

                    //Bitmap b = new Bitmap(TextureLoader.SaveToStream(ImageFileFormat.Bmp, t));
                    //BitmapData data = b.LockBits(new Rectangle(0, 0, settings.Width, settings.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    device.GetRenderTargetData(target, sysTarget);

                    GraphicsStream gStream = sysTarget.LockRectangle(LockFlags.ReadOnly);
                    // copy over to right format
                    // Format.X8R8G8B8 > RGBA
                    int bIdx = 0;
                    for (int pixel = 0; pixel < pixels; pixel++)
                    {
                        gStream.Read(pBuf, 0, 4);
                        buffer[bIdx++] = pBuf[1]; // r
                        buffer[bIdx++] = pBuf[2]; // g
                        buffer[bIdx++] = pBuf[3]; // b
                        buffer[bIdx++] = pBuf[0]; // a
                    }

                    // encode frame
                    stream.PutFrame(buffer);
                }

                int totalBytes = 0;
                stream.Close(out totalBytes);
            }
            catch (ThreadAbortException) { }

            complete = true;
        }
    }
}