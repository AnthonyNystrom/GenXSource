using System;
using System.Collections.Generic;
using System.Text;
using NuGenVideoEnc.Effects;

namespace NuGenVideoEnc
{
    class XVidCodec : Codec
    {
        CodecCapabilities capabilities;

        public XVidCodec()
        {
            capabilities = new CodecCapabilities(-1, -1, new int[] { -1 }, new int[] { 1, 2 },
                                                 CodecCapabilities.OutputTypes.File);
        }

        #region ICodec Members

        public override string Name
        {
            get { return "XVid"; }
        }

        public override string MIMEType
        {
            get { return "video/x-msvideo"; }
        }

        public override CodecCapabilities Capabilities
        {
            get { return capabilities; }
        }

        public override string FileExtension
        {
            get { return "avi"; }
        }

        public override string CompressionType
        {
            get { return "MPEG-4"; }
        }

        public override string Provider
        {
            get { return "Revel 1.1.0-win32"; }
        }

        #endregion

        internal override IOutputStream CreateStream(System.IO.Stream stream, int width, int height, int depth, int fps, IFrameEffect[] effects)
        {
            return null;
        }

        internal override IOutputStream CreateStream(string filename, int width, int height, int depth, int fps, IFrameEffect[] effects)
        {
            return new XvidOutputStream(filename, width, height, depth, fps);
        }
    }
}