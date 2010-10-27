using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Next2Friends.ImageWorks.EffectsFilters
{
    public class OilPainting : EffectFilter
    {
        public int BrushSize { get; set; }
        public byte Coarseness { get; set; }

        public OilPainting(int brushSize, byte coarseness)
        {
            BrushSize = brushSize;
            Coarseness = coarseness;
        }

        public static new EffectFilter Default
        {
            get
            {
                return new OilPainting(3, 20);
            }
        }

        public override BitmapSource ExecuteFilter(BitmapSource inputImage)
        {
            int width = inputImage.PixelWidth;
            int height = inputImage.PixelHeight;

            int arrayLens = 1 + Coarseness;

            int[] intensityCount;
            uint[] avgRed;
            uint[] avgGreen;
            uint[] avgBlue;
            uint[] avgAlpha;

            byte[] sourceBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];
            byte[] destBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];

            inputImage.CopyPixels(sourceBytes, inputImage.PixelWidth * 4, 0);            

            byte maxIntensity = Coarseness;
            double scaleFactor = (double)maxIntensity / 255.0;

            for (int y = 0; y < height; y++)
            {
                int top = y - BrushSize;
                int bottom = y + BrushSize + 1;

                if (top < 0)
                {
                    top = 0;
                }

                if (bottom > height)
                {
                    bottom = height;
                }

                for (int x = 0; x < width; x++)
                {
                    int index = y * width * 4 + x * 4;
                    intensityCount = new int[arrayLens];
                    avgRed = new uint[arrayLens];
                    avgGreen = new uint[arrayLens];
                    avgBlue = new uint[arrayLens];
                    avgAlpha = new uint[arrayLens];

                    int left = x - BrushSize;
                    int right = x + BrushSize + 1;

                    if (left < 0)
                    {
                        left = 0;
                    }

                    if (right > width)
                    {
                        right = width;
                    }

                    int numInt = 0;

                    for (int j = top; j < bottom; j++)
                    {
                        int sourceIndex = j * width * 4 + left * 4;

                        for (int i = left; i < right; i++)
                        {
                            byte srcA = sourceBytes[sourceIndex + 3];
                            byte srcR = sourceBytes[sourceIndex + 2];
                            byte srcG = sourceBytes[sourceIndex + 1];
                            byte srcB = sourceBytes[sourceIndex];

                            double iDouble = srcB * 0.11 + srcG * 0.59 + srcR * 0.30;
                            //byte iByte = (byte)iDouble;
                            //byte intensity = Utility.FastScaleByteByByte(iByte, maxIntensity);
                            byte intensity = (byte)(iDouble * scaleFactor);

                            intensityCount[intensity]++;
                            numInt++;

                            avgRed[intensity] += srcR;
                            avgGreen[intensity] += srcG;
                            avgBlue[intensity] += srcB;
                            avgAlpha[intensity] += srcA;
                        }
                    }

                    byte chosenIntensity = 0;
                    int maxInstance = 0;

                    for (int i = 0; i <= maxIntensity; ++i)
                    {
                        if (intensityCount[i] > maxInstance)
                        {
                            chosenIntensity = (byte)i;
                            maxInstance = intensityCount[i];
                        }
                    }

                    // TODO: correct handling of alpha values?

                    destBytes[index + 2] = (byte)(avgRed[chosenIntensity] / maxInstance);
                    destBytes[index + 1] = (byte)(avgGreen[chosenIntensity] / maxInstance);
                    destBytes[index] = (byte)(avgBlue[chosenIntensity] / maxInstance);
                    destBytes[index + 3] = (byte)(avgAlpha[chosenIntensity] / maxInstance);
                }
            }

            return BitmapSource.Create(inputImage.PixelWidth, inputImage.PixelHeight, 96, 96,
                inputImage.Format, null, destBytes, inputImage.PixelWidth * 4);
        }
    }
}
