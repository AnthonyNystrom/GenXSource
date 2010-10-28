using System;
using System.IO;
using System.Drawing;
using FPlotLibrary;

namespace FPlotDemo {

	class WAVLoader {
		
		private struct Header {
			public UInt32 ChunkID;
			public UInt32 ChunkSize;
			public UInt32 Format;
			public UInt32 Subchunk1ID;
			public UInt32 Subchunk1Size;
			public UInt16 AudioFormat;
			public UInt16 Channels;
			public UInt32 SampleRate;
			public UInt32 ByteRate;
			public UInt16 BlockAlign;
			public UInt16 BitsPerSample;
			public UInt32 Subchunk2ID;
			public UInt32 Subchunk2Size;
		}

		public static void LoadFile(GraphControl graph, string filename) {
			Header h;
		
			using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
				using (BinaryReader r = new BinaryReader(stream)) {
					//Read WAV header
					h.ChunkID = r.ReadUInt32();
					h.ChunkSize = r.ReadUInt32();
					h.Format = r.ReadUInt32();
					h.Subchunk1ID = r.ReadUInt32();
					h.Subchunk1Size = r.ReadUInt32();
					h.AudioFormat = r.ReadUInt16();
					h.Channels = r.ReadUInt16();
					h.SampleRate = r.ReadUInt32();
					h.ByteRate = r.ReadUInt32();
					h.BlockAlign = r.ReadUInt16();
					h.BitsPerSample = r.ReadUInt16();
					h.Subchunk2ID = r.ReadUInt32();
					h.Subchunk2Size = r.ReadUInt32();
					int channels = h.Channels;
					int bytes = h.BitsPerSample / 8;
					int samples = (int)(h.Subchunk2Size / channels / bytes);
					int m, n;
					byte[] d;
					DataItem[] items = new DataItem[channels];
					for (m = 0; m < channels; m++) {
						items[m] = new DataItem();
						items[m].lines = true; // join points with a line.
						items[m].marks = false; // draw no error marks.
						items[m].Length = samples; // Set the length of the arrays.
						// set formula for x-array
						items[m].x.source = "n/((float)Length)*" + (samples / (float)h.SampleRate).ToString(); 
						items[m].dx.source = "0"; // set formula for dx-array
						items[m].dy.source = "0"; // set formula for dy-array
						items[m].name = "channel " + m.ToString(); // set the name of the DataItem.
						items[m].Compile(true);
						switch (m % 3) {
						case 0: items[m].Color = Color.Blue; break;
						case 1: items[m].Color = Color.Red; break;
						case 2: items[m].Color = Color.Green; break;
						}
						graph.Model.Items.Add(items[m]);
					}
					for (n = 0; n < samples; n++) {
						for (m = 0; m < channels; m++) {
							// Set y.AutoResize to true, so the size of the y-array will be adapted automatically.
							items[m].y.AutoResize = true;
							d = r.ReadBytes(bytes);
							switch (bytes) {
							case 1:	items[m].y[n] = (double)((int)d[0])/0x100; break;
							case 2: items[m].y[n] = (double)BitConverter.ToInt16(d, 0)/0x10000; break;
							case 3: items[m].y[n] = (double)(d[0]*0x10000 + d[1]*0x100 + d[3])/0x1000000; break;
							case 4: items[m].y[n] = (double)BitConverter.ToInt32(d, 0)/0x100000000; break;
							}
							items[m].y.AutoResize = false;
						}
					}

				}
			}
		}
	}

}