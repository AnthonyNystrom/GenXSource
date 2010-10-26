using System;
using System.Collections.Generic;
using System.Text;
using VidEnc.Revel;

namespace NuGenVideoEnc
{
    class XvidOutputStream : OutputStream
    {
        RevelOutput output;

        public XvidOutputStream(string filename, int width, int height,
                                int depth, int fps)
            : base(width, height, depth, fps)
        {
            output = new RevelOutput();
            output.Init(width, height, fps, filename);
        }

        public override void PutFrame(byte[] data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void PutFrame(int[] data)
        {
            output.DrawFrame(data);
        }

        public override void Close(out int totalBytes)
        {
            RevelOutputStats stats = output.Close();
            totalBytes = stats.totalBytes;
        }
    }
}