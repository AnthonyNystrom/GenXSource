using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NuGenVideoEnc.Effects;

namespace NuGenVideoEnc
{
    public class CodecCapabilities
    {
        public enum OutputTypes
        {
            File        = 1,
            Memory      = 2,
            Stream      = 4
        }

        int[] passes;
        int maxWidth;
        int maxHeight;
        int[] depths;
        OutputTypes outputTypes;

        public CodecCapabilities(int maxWidth, int maxHeight,
                                 int[] depths, int[] passes, OutputTypes output)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            this.depths = depths;
            this.passes = passes;
            this.outputTypes = output;
        }

        public OutputTypes OutputType
        {
            get { return outputTypes; }
        }

        public int[] Depths
        {
            get { return depths; }
        }

        public int MaxHeight
        {
            get { return maxHeight; }
        }

        public int MaxWidth
        {
            get { return maxWidth; }
        }

        public int[] Passes
        {
            get { return passes; }
        }
    }

    public interface ICodec
    {
        string Name { get; }
        string MIMEType { get; }
        string FileExtension { get; }
        string CompressionType { get; }
        string Provider { get; }
        CodecCapabilities Capabilities { get; }
    }

    abstract class Codec : ICodec
    {
        internal abstract IOutputStream CreateStream(Stream stream, int width, int height, int depth, int fps, IFrameEffect[] effects);
        internal abstract IOutputStream CreateStream(string filename, int width, int height, int depth, int fps, IFrameEffect[] effects);

        #region ICodec Members

        public abstract string Name { get; }
        public abstract string MIMEType { get; }
        public abstract string FileExtension { get; }
        public abstract string CompressionType { get; }
        public abstract string Provider { get; }
        public abstract CodecCapabilities Capabilities { get; }

        #endregion
    }
}