using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenCRBase.SceneFormats.ThreeDS
{
    class DataReader3DS
    {
        #region Fields
        protected byte[] sourceData;				//array of source data
        protected MemoryStream sourceStream;		//stream of source data
        protected ulong myStreamOffset;				//the true position in the stream relative to start of sourceData
        protected ushort tag;						//specifies what is contained in the data subsegment
        protected ulong size;						//specifies the size of the data subsegment
        protected BinaryReader dataReader;			//used to read data from the input System.IO.Stream
        protected BinaryReader currentSegment;		//contains the current data segment
        protected DataReader3DS currentSubSegment;	//contains the current data subsegment

        protected const ushort HeaderSize = 6;	// = (size of 'tag') + (size of 'size')
        #endregion

        #region Constructor(s)
        public DataReader3DS(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open);
            byte[] data = new byte[file.Length];
            file.Read(data, 0, (int)file.Length);
            file.Close();

            sourceData = data;
            myStreamOffset = 0;
            sourceStream = new MemoryStream(sourceData);
            dataReader = new BinaryReader(sourceStream);
            tag = dataReader.ReadUInt16();
            size = (ulong)dataReader.ReadInt32();
            currentSubSegment = null;
            currentSegment = dataReader;
        }

        /// <summary>
        /// Creates a DataReader3DS object.
        /// </summary>
        /// <param name="inputData">Contains array of data bytes.</param>
        public DataReader3DS(byte[] inputData)
        {
            sourceData = inputData;
            myStreamOffset = 0;
            sourceStream = new MemoryStream(sourceData);
            dataReader = new BinaryReader(sourceStream);
            tag = dataReader.ReadUInt16();
            size = (ulong)dataReader.ReadInt32();
            currentSubSegment = null;
            currentSegment = dataReader;
        }

        /// <summary>
        /// Internal constructor that creates a DataReader3DS object.
        /// </summary>
        /// <param name="inputData">Contains array of data bytes.</param>
        /// <param name="startingPoint">Specified starting location.</param>
        /// <param name="length">Specified length.</param>
        protected DataReader3DS(byte[] inputData, int startingPoint, int length)
        {
            sourceData = inputData;
            myStreamOffset = (ulong)startingPoint;
            sourceStream = new MemoryStream(sourceData, startingPoint, length);
            dataReader = new BinaryReader(sourceStream);
            tag = dataReader.ReadUInt16();
            size = (ulong)dataReader.ReadInt32();
            currentSubSegment = null;
            currentSegment = dataReader;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the tag denoting the type of data in the current data segment.
        /// </summary>
        public ushort Tag
        {
            get
            {
                return tag;
            }
        }

        /// <summary>
        /// Returns the size of the current data reading segment.
        /// </summary>
        public ulong Size
        {
            get
            {
                return size;
            }
        }

        /// <summary>
        /// Returns the BinaryReader being used to read 3DS data from the stream.
        /// </summary>
        public BinaryReader DataReader
        {
            get
            {
                return dataReader;
            }
        }

        /*// <summary>
		/// Returns a reader that is looking at the current segment of 3DS data.
		/// </summary>
		public DataReader3DS CurrentSegment
        {
            get {
                return currentSegment;
            }
        }*/

                /// <summary>
                /// Returns a reader that is looking at the current subsegment of 3DS data.
        /// </summary>
        public DataReader3DS CurrentSubSegment
        {
            get
            {
                return currentSubSegment;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Moves to the next subsegment of data in the data stream.
        /// </summary>
        /// <returns>Returns the next subsegment of data in the data stream.</returns>
        public DataReader3DS GetNextSubSegment()
        {
            if (currentSegment.BaseStream.Position > 3700000)
                System.Diagnostics.Debug.WriteLine("Breaking...");
            if ((ulong)currentSegment.BaseStream.Position < (size - HeaderSize))
            {
                if (currentSubSegment != null)
                {
                    currentSegment.BaseStream.Position += (long)currentSubSegment.Size;
                    currentSubSegment = null;
                }
                //currentSubSegment = new DataReader3DS(sourceData,
                //									  (int)currentSegment.BaseStream.Position,
                //									  (int)(currentSegment.BaseStream.Length - currentSegment.BaseStream.Position));
                if ((currentSegment.BaseStream.Length - currentSegment.BaseStream.Position) > 0)
                {
                    currentSubSegment = new DataReader3DS(sourceData, (int)(myStreamOffset +
                                                          (ulong)currentSegment.BaseStream.Position),
                                                          (int)(currentSegment.BaseStream.Length -
                                                          currentSegment.BaseStream.Position));
                }
            }
            else
            {
                currentSubSegment = null;
            }
            return currentSubSegment;
        }

        /// <summary>
        /// Reads a System.UInt16 from binary data in 3DS data stream.
        /// Assumes that the .NET implementation preserves correct endian-ness.
        /// </summary>
        /// <returns>Returns the next binary System.UInt16.</returns>
        public ushort GetUShort()
        {
            return currentSegment.ReadUInt16();
        }

        /// <summary>
        /// Reads a System.Byte from binary data in 3DS data stream.
        /// </summary>
        /// <returns>Returns the next binary System.Byte.</returns>
        public byte GetByte()
        {
            return currentSegment.ReadByte();
        }

        /// <summary>
        /// Reads a System.Single from binary data in 3DS data stream.
        /// Assumes that the .NET implementation preserves correct endian-ness.
        /// </summary>
        /// <returns>Returns the next binary System.Single.</returns>
        public float GetFloat()
        {
            return currentSegment.ReadSingle();
        }

        /// <summary>
        /// Reads a System.String from binary data in 3DS data stream.
        /// </summary>
        /// <returns>Returns the next binary System.String</returns>
        public string GetString()
        {
            string result = "";
            byte streamByte;

            do
            {
                streamByte = currentSegment.ReadByte();
                if (streamByte != '\0')
                {
                    result += ((char)streamByte).ToString();
                }
            } while (streamByte != '\0');
            return result;
        }
        #endregion
    }
}