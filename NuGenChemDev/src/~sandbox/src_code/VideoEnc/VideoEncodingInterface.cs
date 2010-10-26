using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NuGenVideoEnc.Effects;

namespace NuGenVideoEnc
{
    /// <summary>
    /// Encapsulates the main public interface for creating video streams
    /// </summary>
    public class VideoEncodingInterface
    {
        private static readonly ICodec[] codecs = new ICodec[] { new XVidCodec() };

        /// <summary>
        /// Lists all the available codecs
        /// </summary>
        public static ICodec[] AvailableCodecs
        {
            get { return (ICodec[])codecs.Clone(); }
        }

        /// <summary>
        /// Creates a video output stream based on the parameters
        /// </summary>
        /// <param name="outStream">The stream to output data to</param>
        /// <param name="codec">The codec to encode with</param>
        /// <param name="width">The viedo width</param>
        /// <param name="height">The video height</param>
        /// <param name="depth">The video colour bit-depth</param>
        /// <param name="fps">The number of frames to encode per second</param>
        /// <param name="length">The length of the video</param>
        /// <param name="effects">The effects to apply to the video (null for none)</param>
        /// <returns>The video stream to write to</returns>
        public static IOutputStream CreateVideoStream(Stream outStream, ICodec codec,
                                                      int width, int height, int depth,
                                                      int fps, TimeSpan length,
                                                      IFrameEffect[] effects)
        {
            if ((int)(codec.Capabilities.OutputType & CodecCapabilities.OutputTypes.Stream) > 0)
                return ((Codec)codec).CreateStream(outStream, width, height, depth, fps, effects);
            return null;
        }

        /// <summary>
        /// Creates a video output directly to file based on the parameters
        /// </summary>
        /// <param name="filename">The file to output to</param>
        /// <param name="codec">The codec to encode with</param>
        /// <param name="width">The viedo width</param>
        /// <param name="height">The video height</param>
        /// <param name="depth">The video colour bit-depth</param>
        /// <param name="fps">The number of frames to encode per second</param>
        /// <param name="length">The length of the video</param>
        /// <param name="effects">The effects to apply to the video (null for none)</param>
        /// <returns>The video stream to write to</returns>
        public static IOutputStream CreateVideoStream(string filename, ICodec codec,
                                                      int width, int height, int depth,
                                                      int fps, TimeSpan length,
                                                      IFrameEffect[] effects)
        {
            if ((int)(codec.Capabilities.OutputType & CodecCapabilities.OutputTypes.File) > 0)
                return ((Codec)codec).CreateStream(filename, width, height, depth, fps, effects);
            return null;
        }
    }
}