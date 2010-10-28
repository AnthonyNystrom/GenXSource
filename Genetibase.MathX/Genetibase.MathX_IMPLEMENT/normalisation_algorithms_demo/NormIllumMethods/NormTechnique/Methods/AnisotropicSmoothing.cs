using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace NormIllumMethods.Techniques
{
    public class AnisotropicSmoothing : ImageOperations
    {
        double smooth; //smoothness constraint

        public AnisotropicSmoothing(double c)
        {
            this.smooth = c;
        }

        public override unsafe Bitmap Apply(Bitmap bitmap) 
        {
            Bitmap = bitmap;
            Point size = PixelSize;
            double[,] src = new double[size.X,size.Y];
            bool first = true;
            byte N, S, E, W, A;
            double Lw, Le, Ls, Ln, pw, pe, ps, pn, eps = .1, tmp, min = 0, max = 0;
            
            LockBitmap();
            
            for (int y = 0; y < size.Y; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < size.X; x++)
                {
                    tmp = pPixel->gray;

                    //Applying the process to all pixels of image except to the borders
                    if ((x > 0) && (x < size.X-1) && (y > 0) && (y < size.Y-1))
                    {
                        //Adjacent neighbouring pixels
                        A = pPixel->gray;           //current
                        E = PixelAt(x, y+1)->gray;  //east 
                        S = PixelAt(x+1, y)->gray;  //south 
                        N = PixelAt(x-1, y)->gray;  //north 
                        W = PixelAt(x, y-1)->gray;  //west  

                        //Ld refers to the derivative with respect to 
                        //each of the four adjacent neighbouring pixels
                        Lw = A - W;
                        Le = A - E;
                        Ln = A - N;
                        Ls = A - S;

                        //Weber’s contrast inverse
                        pw = Math.Min(A, W) / (Math.Abs(A - W) + eps);
                        pe = Math.Min(A, E) / (Math.Abs(A - E) + eps);
                        pn = Math.Min(A, N) / (Math.Abs(A - N) + eps);
                        ps = Math.Min(A, S) / (Math.Abs(A - S) + eps);

                        //Anisotropic smoothing
                        tmp = A + smooth * (Ln * pn + Ls * ps + Le * pe + Lw * pw);
                    }

                    src[x, y] = tmp;

                    //Computing the min and max values from all pixels of image
                    if (first) { min = max = tmp; first = false; }
                    else
                    {
                        if (tmp < min) min = tmp;
                        else
                            if (tmp > max) max = tmp;
                    }
                    pPixel++;
                }
            }
            UnlockBitmap();
            return Normalise(src,min, max);
        }

    }
}
