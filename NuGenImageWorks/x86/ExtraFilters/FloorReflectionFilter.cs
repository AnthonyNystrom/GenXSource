using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.BasicFilters
{
    class FloorReflectionFilter : BasicFilter
    {
        private int alphaStart = 150;
        private int alphaEnd = 50;
        private DockStyle dockPosition = DockStyle.None;
        private int offset = 50;
        
		public int Offset {
			get {
				return offset;
			}
			set {
				offset = value;
			}
		}
        
		public DockStyle DockPosition {
			get {
				return dockPosition;
			}
			set {
				dockPosition = value;
			}
		}
        
		public int AlphaStart {
			get {
				return alphaStart;
			}
			set {
				alphaStart = value;
			}
		}        
        
		public int AlphaEnd {
			get {
				return alphaEnd;
			}
			set {
				alphaEnd = value;
			}
		}

        public override Image ExecuteFilter(Image input )
        {
        	DockStyle pos = this.dockPosition;
            Bitmap b = (Bitmap)input;
            int width = b.Width;
            int height = b.Height;

            if (pos == DockStyle.Bottom || pos == DockStyle.Top)
            {
                height += Offset;
            }
            else if (pos == DockStyle.Left || pos == DockStyle.Right)
            {
                width += Offset;
            }

            Bitmap newB = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(newB);

            int alphaDecreaseRate =(int)( (float)(alphaStart - alphaEnd) / (float)Offset);
            int alpha = alphaStart;

            if (pos == DockStyle.Top)
            {
                g.DrawImage(b, 0, Offset);
                int k = Offset;
                
                for (int y = 0; y < Offset; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        newB.SetPixel(x,k,Color.FromArgb(alpha,b.GetPixel(x,y)));
                    }

                    alpha -= alphaDecreaseRate;
                    if( alpha < 0 )
                    	alpha = 0;
                    k--;
                }
            }
            else if (pos == DockStyle.Bottom)
            {
                g.DrawImage(b, 0, 0);

                int k = b.Height;

                for (int y = b.Height - 1; y > b.Height - Offset ; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        newB.SetPixel(x, k, Color.FromArgb(alpha, b.GetPixel(x, y)));
                    }

                    alpha -= alphaDecreaseRate;
                    if( alpha < 0 )
                    	alpha = 0;
                    k++;
                }
            }
            else if (pos == DockStyle.Left)
            {
                g.DrawImage(b, Offset, 0);
                
                int k = Offset;
                
                for (int x = 0; x < Offset; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        newB.SetPixel(k,y,Color.FromArgb(alpha,b.GetPixel(x,y)));
                    }

                    alpha -= alphaDecreaseRate;
                    if( alpha < 0 )
                    	alpha = 0;
                    k--;
                }
            }
            else if (pos == DockStyle.Right)
            {
                g.DrawImage(b, 0, 0);
                
                int k = b.Width;
                
                for (int x = b.Width - 1; x > b.Width - Offset ; x--)
                {
                    for (int y = 0; y < height; y++)
                    {
                        newB.SetPixel(k,y,Color.FromArgb(alpha,b.GetPixel(x,y)));
                    }

                    alpha -= alphaDecreaseRate;
                    if( alpha < 0 )
                    	alpha = 0;
                    
                    k++;
                }
            }

            return newB;
        }
    }
}
