using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace Genetibase.NuGenDEMVis
{
    /// <summary>
    /// Provides Digital Elevation Model parsing support.
    /// USGS DEM - http://en.wikipedia.org/wiki/USGS_DEM
    /// </summary>
    public class DEMReader
    {
        public static void ReadFile(string file, out Bitmap image)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            int nRows = 6000;
            int nCols = 4800;

            int sampleRate = 5;
            int spr = nCols / sampleRate;
            int spc = nRows / sampleRate;

            image = new Bitmap(spr, spc);
            HeightBandRange hbr = new HeightBandRange();
            hbr.AddBand(-0.1f, Color.Red);
            hbr.AddBand(0.0f, Color.DarkBlue);
            hbr.AddBand(0.01f, Color.DarkGreen);
            hbr.AddBand(0.2f, Color.LightGreen);
            hbr.AddBand(0.5f, Color.SlateGray);
            hbr.AddBand(1.0f, Color.White);

            // read a row at a time
            byte[] data = new byte[2];
            byte[] dataRev = new byte[2];
            for (int row = 0; row < spc; row++)
            {
                long pos = row * nCols * 2 * sampleRate;
                fs.Seek(pos, SeekOrigin.Begin);
                
                for (int col = 0; col < spr; col++)
                {
                    int avrValue = 0;
                    for (int i = 0; i < sampleRate; i++)
                    {
                        fs.Read(data, 0, 2);
                        dataRev[0] = data[1];
                        dataRev[1] = data[0];
                        int value = BitConverter.ToInt16(dataRev, 0);
                        if (value != -9999)
                            avrValue += value;
                    }
                    avrValue /= sampleRate;
                    if (avrValue == 0)
                        image.SetPixel(col, row, Color.Blue);
                    else
                    {
                        // encode to greyscale
                        /*byte clr = (byte)((avrValue * 0.05f) + 32);
                        image.SetPixel(col, row, Color.FromArgb(255, clr, clr, clr));*/
                        image.SetPixel(col, row, hbr[avrValue / 5000f]);
                    }
                    //fs.Seek((col * 2 * sampleRate) + pos, SeekOrigin.Begin);
                }
            }
        }
    }
}