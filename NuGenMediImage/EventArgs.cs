/*
 * Created by SharpDevelop.
 * User: hq230002
 * Date: 4/18/2007
 * Time: 10:35 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Genetibase.NuGenMediImage
{
    internal class DicomSliceLoadEventArgs : EventArgs
    {
        private System.Drawing.Bitmap b;
        private int idx = 0;
        private int total = 0;

        public int Idx
        {
            get { return idx; }
            //set { idx = value; }
        }

        public int Total
        {
            get { return total; }
            //set { total = value; }
        }


        public System.Drawing.Bitmap Thumbnail
        {
            get { return b; }
            //set { b = value; }
        }

        public DicomSliceLoadEventArgs(System.Drawing.Bitmap b,int idx,int total)
        {            
            this.b = b;
            this.idx = idx;
            this.total = total;
        }
    }

    internal class DicomLoadEventArgs : EventArgs
    {
        private int idx = 0;
        private int total = 0;

        public int Idx
        {
            get { return idx; }
            //set { idx = value; }
        }

        public int Total
        {
            get { return total; }
            //set { total = value; }
        }

        public DicomLoadEventArgs(int idx, int total)
        {
            this.idx = idx;
            this.total = total;
        }
    }

    internal class SliceEventArgs : EventArgs
    {
        private int frameNumber;        

        public int FrameNumber
        {
            get { return frameNumber; }
        }

        public SliceEventArgs(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }
    }
}
