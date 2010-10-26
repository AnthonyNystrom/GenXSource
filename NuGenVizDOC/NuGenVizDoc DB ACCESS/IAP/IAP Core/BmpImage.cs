using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IAP_Core
{
    public class BmpImage : Image
    {
        public BmpImage(System.Drawing.Bitmap bitmap)
            : base(bitmap)
        {
        }
        public BmpImage(string filename)
        {
            BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));

            // Read header

            br.ReadUInt16();
            br.ReadUInt32();
            br.ReadUInt16();
            br.ReadUInt16();
            br.ReadUInt32();

            // Read infoheader

            br.ReadUInt32();
            this.Create(br.ReadInt32(), br.ReadInt32());
            br.ReadUInt16();
            br.ReadUInt16();
            br.ReadUInt32();
            br.ReadUInt32();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadUInt32();
            br.ReadUInt32();

            // Read palette

            System.Drawing.Imaging.ColorPalette palette = this.Bitmap.Palette;

            for (int i = 0; i < palette.Entries.Length; i++)
            {
                byte b = br.ReadByte();
                byte g = br.ReadByte();
                byte r = br.ReadByte();
                br.ReadByte();

                palette.Entries[i] = System.Drawing.Color.FromArgb(r, g, b);
            }

            this.Bitmap.Palette = palette;

            // Read data

            this.OpenData(System.Drawing.Imaging.ImageLockMode.WriteOnly, true);

            for (int y = 0; y < this.Height; y++)
            {
                this.DecLine();

                for (int x = 0; x < this.Width; x++)
                    this.SetPixel(x, br.ReadByte());
            }

            this.CloseData();

            br.Close();
        }

        public override void Save(string filename)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Create));

            // Store header

            bw.Write((ushort)19778);
            bw.Write((uint)1078);
            bw.Write((ushort)0);
            bw.Write((ushort)0);
            bw.Write((uint)1078);

            // Store infoheader

            bw.Write((uint)40);
            bw.Write((int)this.Width);
            bw.Write((int)this.Height);
            bw.Write((ushort)1);
            bw.Write((ushort)8);
            bw.Write((uint)0);
            bw.Write((uint)(this.Width * this.Height));
            bw.Write((int)0);
            bw.Write((int)0);
            bw.Write((uint)256);
            bw.Write((uint)0);

            // Store palette

            System.Drawing.Color[] colortable = this.Bitmap.Palette.Entries;
            for (int i = 0; i < colortable.Length; i++)
            {
                bw.Write((byte)colortable[i].B);
                bw.Write((byte)colortable[i].G);
                bw.Write((byte)colortable[i].R);
                bw.Write((byte)0);
            }

            // Store data

            this.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, true);

            for (int y = 0; y < this.Height; y++)
            {
                this.DecLine();

                for (int x = 0; x < this.Width; x++)
                    bw.Write((byte)this.GetPixel(x));
            }

            this.CloseData();

            bw.Close();
        }
    }
}
