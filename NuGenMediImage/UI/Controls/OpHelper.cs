using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public class OpHelper
    {
        public enum MultiFrameThreadDirection
        {
            None,Forward,Backward
        }

        internal delegate void SetImageDelegate(Bitmap img);

        internal event SliceProcessedEventHandler SliceProccesed;        

        private int selectedIndex = 0;
        internal bool _dirty = false;
        
        private Viewer viewer;
        private ImageArray originalBitmap;

        private ImageArray operatedBitmap;
        private ImageArray operatedBitmapOrig;
        private ImageArray toBeDisposed;
        private int progress = 0;
        private List<Op> operations = new List<Op>();

        bool initOrigRequired = true;
        bool processing = false;

        bool onlineMode = false;

        Bitmap onlineBitmap = null;

        public Bitmap OnlineBitmap
        {
            get { return onlineBitmap; }            
        }

        public bool OnlineMode
        {
            get { return onlineMode; }
            set { onlineMode = value; }
        }

        public bool Processing
        {
            get { return processing; }
            set { processing = value; }
        }

        public bool DoAllSliceOP
        {
            get
            {
                if (operations.Count == 0 || !_dirty)
                {
                    return false;
                }
                return true;
            }
        }

        internal List<Op> Operations
        {
            get { return operations; }            
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set 
            { 
                selectedIndex = value;
                if( operatedBitmap == null )
                    ReInitOperatedBitmaps();
                viewer.picBoxMain.Image = this.operatedBitmap[selectedIndex];
            }
        }

        public int Progress
        {
            get { return progress; }
        }


        public ImageArray OriginalBitmap
        {
            get { return originalBitmap; }
            //set 
            //{ 
            //    originalBitmap = value;
            //    InitOperatedOrigBitmaps();
            //    ReInitOperatedBitmaps();
            //    this.operatedBitmap = this.operatedBitmapOrig;
            //}
        }


        public Bitmap GetOperatedBitmap(int idx)
        {
            if (idx >= this.originalBitmap.Count || idx < 0)
                throw new ArgumentException("Index out of bound");

            if (operatedBitmap != null)
                return (Bitmap)operatedBitmap[idx];

            else if (originalBitmap != null)
                return (Bitmap)originalBitmap[idx];

            else
                return null;
        }

        //public Bitmap OperatedBitmap2
        //{
        //    get 
        //    {
        //        if (operatedBitmap != null)
        //            return (Bitmap)operatedBitmap[selectedIndex];

        //        else if (originalBitmap != null)
        //            return (Bitmap)originalBitmap[selectedIndex];

        //        else
        //            return null;
        //    }
        //}

        //public ImageArray OperatedBitmaps
        //{
        //    get
        //    {
        //        if (operatedBitmap != null)
        //            return operatedBitmap;

        //        else
        //            return originalBitmap;
        //    }
        //}

        public void Dispose()
        {
            if (operatedBitmap != null)
            {
                foreach (Image img in operatedBitmap)
                {
                    if (img != null)
                    {
                        img.Dispose();
                    }
                }

                operatedBitmap.Clear();
                operatedBitmap = null;
            }

            if (originalBitmap != null)
            {
                foreach (Image img in originalBitmap)
                {
                    if (img != null)
                    {
                        img.Dispose();
                    }
                }

                originalBitmap.Clear();
                originalBitmap = null;
            }

            try
            {
                DisposeToBeDisposed();
            }
            catch { }
        }

        public void AddLUT(string LutName)
        {
            lock (operations)
            {
                bool updated = false;

                foreach (Op o in operations)
                {
                    if (o.OpName == OperatorName.LUT)
                    { 
                        Op oper = o;

                        if (oper.param3 == LutName)
                            return;

                        oper.param3 = LutName;
                        updated = true;
                        break;
                    }
                }

                if (!updated)
                {                    
                    this.Operations.Add(new Op(OperatorName.LUT, 0, 0, LutName));
                }

                _dirty = true;
            }
        }

        public void ClearLUT()
        {
            lock (operations)
            {
                Op oper = null;
                bool updated = false;
                        
                foreach (Op o in operations)
                {
                    if (o.OpName == OperatorName.LUT)
                    {
                        oper = o;
                        updated = true;
                        break;
                    }
                }

                if (updated)
                {
                    operations.Remove(oper);
                }

                _dirty = true;
            }
        }

        public void AddSmooth(int smooth)
        {
            lock (operations)
            {
                this.Operations.Add(new Op(OperatorName.SMOOTH, smooth, 0, ""));                
                _dirty = true;
            }
        }


        public void AddEmboss(int offset)
        {
            lock (operations)
            {
                bool updated = false;

                foreach (Op o in operations)
                {
                    if (o.OpName == OperatorName.EMBOSS)
                    {
                        Op oper = o;
                        if (oper.param1 == offset)
                            return;
                        oper.param1 = offset;
                        updated = true;
                        break;
                    }
                }

                if (!updated)
                {
                    this.Operations.Add(new Op(OperatorName.EMBOSS, offset, 0, ""));
                }

                _dirty = true;
            }
        }

        //public void AddZoom(double ZoomFactor)
        //{
        //    lock (operations)
        //    {
        //        bool updated = false;

        //        foreach (Op o in operations)
        //        {
        //            if (o.OpName == OperatorName.ZOOM)
        //            {
        //                Op oper = o;
        //                oper.param2 = ZoomFactor;
        //                updated = true;
        //                break;
        //            }
        //        }

        //        if (!updated)
        //        {
        //            this.Operations.Add(new Op(OperatorName.ZOOM, 0, ZoomFactor, ""));
        //        }

        //        Dirty++;
        //    }
        //}

        public void AddRotate(RotateFlipType rotFlipType)
        {
            lock (operations)
            {
                bool updated = false;

                foreach (Op o in operations)
                {
                    if (o.OpName == OperatorName.ROTATE)
                    {
                        Op oper = o;
                        RotateFlipType newRot = (RotateFlipType)(((int)oper.param1 + (int)rotFlipType) % 4);
                        oper.param1 = (int)newRot;
                        updated = true;
                        break;
                    }
                }

                if (!updated)
                {
                    this.Operations.Add(new Op(OperatorName.ROTATE, (int)rotFlipType, 0, ""));
                }

                _dirty = true;
            }
        }

        public void AddFlip(RotateFlipType rotFlipType)
        {
            lock (operations)
            {
                bool updated = false;

                foreach (Op o in operations)
                {
                    if (o.OpName == OperatorName.FLIP)
                    {

                        Op oper = o;

                        if (oper.param1 == (int)rotFlipType)
                            oper.param1 = (int)RotateFlipType.RotateNoneFlipNone;

                        else if ((int)rotFlipType == 6)
                        {
                            oper.param1 = ((int)rotFlipType - (int)oper.param1);
                        }

                        else if ((int)rotFlipType == 4)
                        {
                            if ((int)oper.param1 > 4)
                                oper.param1 = ((int)oper.param1 - (int)rotFlipType);
                            else
                                oper.param1 = ((int)rotFlipType + (int)oper.param1);
                        }

                        updated = true;
                        break;
                    }
                }

                if (!updated)
                {
                    this.Operations.Add(new Op(OperatorName.FLIP, (int)rotFlipType, 0, ""));
                }

                _dirty = true;
            }
        }

        public void AddBrightness(float brightness)
        {
            lock (operations)
            {
                bool updated = false;

                foreach (Op o in operations)
                {
                    if (o.OpName == OperatorName.BRIGHTNESS)
                    {
                        Op oper = o;
                        if (oper.param1 == brightness)
                            return;
                        oper.param1 = brightness;
                        updated = true;
                        break;
                    }
                }

                if (!updated)
                {
                    this.Operations.Add(new Op(OperatorName.BRIGHTNESS, brightness, 0, ""));
                }

                _dirty = true;
            }
        }

        public void AddContrast(float contrast)
        {
            lock (operations)
            {
                bool updated = false;

                foreach (Op o in operations)
                {
                    if (o.OpName == OperatorName.CONTRAST)
                    {
                        Op oper = o;
                        if (oper.param1 == contrast)
                            return;

                        oper.param1 = contrast;


                        updated = true;
                        break;
                    }
                }

                if (!updated)
                {
                    this.Operations.Add(new Op(OperatorName.CONTRAST, contrast, 0, ""));
                }

                _dirty = true;
            }
        }


        public OpHelper(ImageArray originalBitmap, Viewer viewer)
        {
            this.originalBitmap = originalBitmap;

            InitOperatedOrigBitmaps();
            //ReInitOperatedBitmaps();
            
            this.viewer = viewer;
        }

        private void ReInitOperatedBitmaps()
        {
            toBeDisposed = this.operatedBitmap;
            this.operatedBitmap = this.operatedBitmapOrig;
        }

        private void InitOperatedOrigBitmaps()
        {
            this.operatedBitmapOrig = new ImageArray();

            for (int i = 0; i < this.originalBitmap.Count; i++)
            {
                this.operatedBitmapOrig.Add((Bitmap)this.originalBitmap[i].Clone());
            }
        }

        private void DisposeToBeDisposed()
        {
            if (toBeDisposed != null)
            {
                for (int i = 0; i < this.toBeDisposed.Count; i++)
                {
                    toBeDisposed[i].Dispose();
                }
            }
        }

        public void PerformCBOps(Bitmap img, float contrast, float brightness)
        {
            try
            {
                Filters.Contrast(img, contrast);
                Filters.Brightness(img, brightness);
            }
            catch{}
        }


        internal void SetupOnlineBmp()
        {
            if (originalBitmap == null && originalBitmap.Count == 0)
            {
                if (onlineBitmap != null)
                    onlineBitmap.Dispose();

                onlineBitmap = null;
                return;
            }

            int w = 0;
            int h = 0;

            double factor = (double)this.originalBitmap[selectedIndex].Height / (double)this.originalBitmap[selectedIndex].Width;
            w = 800;
            h = (int)(800 * factor);

            if (this.originalBitmap[selectedIndex].Width > 800)
            {
                onlineBitmap = (Bitmap)this.originalBitmap[selectedIndex].GetThumbnailImage(w, h, null, IntPtr.Zero);
            }
            else
            {
                onlineBitmap = (Bitmap)this.originalBitmap[selectedIndex].Clone();
            }

            if (operations.Count == 0 || !_dirty)
            {                
                return;
            }
           
            List<Op> currentList = new List<Op>();

            lock (operations)
            {
                foreach (Op o in operations)
                {
                    currentList.Add(o);
                }
            }

            foreach (Op o in currentList)
            {

                switch (o.OpName)
                {
                    case OperatorName.ROTATE:
                        Filters.iRotate(onlineBitmap, (RotateFlipType)o.param1);
                        break;

                    case OperatorName.FLIP:
                        Filters.iRotate(onlineBitmap, (RotateFlipType)o.param1);
                        break;

                    case OperatorName.SMOOTH:
                        Filters.Smooth(onlineBitmap, (int)o.param1);
                        break;

                    case OperatorName.EMBOSS:
                        if (o.param1 > 0)
                        {
                            onlineBitmap = Filters.Emboss(onlineBitmap, (int)o.param1);
                        }
                        break;

                    case OperatorName.LUT:
                        if (o.param3.Trim().ToUpper() != "NONE")
                        {
                            string resourceName = "Genetibase.NuGenMediImage" + ".LUTs." + o.param3 + ".LUT";
                            Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

                            Filters.ReadAndApplyLUT(stream, onlineBitmap);

                            stream.Close();
                        }
                        break;
                }
            }
        }

        public void PerformOperations(bool selectedOnly, MultiFrameThreadDirection direction)
        {
            try
            {
                processing = true;

                if (operations.Count == 0 || !_dirty)
                {
                    processing = false;
                    return;
                }

                if (originalBitmap == null && originalBitmap.Count == 0)
                {
                    processing = false;
                    return;
                }

                if (initOrigRequired)
                {
                    InitOperatedOrigBitmaps();
                    DisposeToBeDisposed();
                }

                initOrigRequired = true;

                List<Op> currentList = new List<Op>();

                lock (operations)
                {
                    foreach (Op o in operations)
                    {
                        currentList.Add(o);
                    }
                }

                int total = currentList.Count;
                int step = 100 / total;
                progress = 0;

                if (selectedOnly && direction == MultiFrameThreadDirection.None)
                {
                    ReInitOperatedBitmaps();
                }
                else if (direction == MultiFrameThreadDirection.Backward)
                {
                    _dirty = false;
                }

                
                int start = 0;
                int end = operatedBitmap.Count;

                if (!selectedOnly && direction == MultiFrameThreadDirection.Forward)
                {
                    start = selectedIndex + 1;
                }
                else if (!selectedOnly && direction == MultiFrameThreadDirection.Backward)
                {
                    end = selectedIndex;
                }

                for (int i = start; i < end; i++)
                {
                    if (selectedOnly && i != selectedIndex)
                        continue;

                    foreach (Op o in currentList)
                    {
                        Bitmap oldImage = (Bitmap)operatedBitmap[i];

                        switch (o.OpName)
                        {
                            case OperatorName.ROTATE:
                                Filters.iRotate(oldImage, (RotateFlipType)o.param1);
                                break;

                            case OperatorName.FLIP:
                                Filters.iRotate(oldImage, (RotateFlipType)o.param1);
                                break;

                            case OperatorName.SMOOTH:
                                Filters.Smooth(oldImage, (int)o.param1);
                                break;

                            case OperatorName.EMBOSS:
                                if (o.param1 > 0)
                                {
                                    operatedBitmap[i] = Filters.Emboss(oldImage, (int)o.param1);
                                }
                                break;

                            case OperatorName.CONTRAST:
                                Filters.Contrast(oldImage, o.param1);
                                operatedBitmap[i] = (Bitmap)oldImage.Clone();
                                break;

                            case OperatorName.BRIGHTNESS:
                                Filters.Brightness(oldImage, o.param1);
                                break;

                            //case OperatorName.ZOOM:
                            //if(o.param2 != 0)
                            //    operatedBitmap[selectedIndex] = Filters.Resize(oldImage, (float)o.param2);
                            //break;

                            case OperatorName.LUT:
                                if (o.param3.Trim().ToUpper() != "NONE")
                                {
                                    string resourceName = "Genetibase.NuGenMediImage" + ".LUTs." + o.param3 + ".LUT";
                                    Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

                                    Filters.ReadAndApplyLUT(stream, oldImage);

                                    stream.Close();
                                }
                                break;
                        }
                        progress += step;
                    }

                    if (direction == MultiFrameThreadDirection.Forward || direction == MultiFrameThreadDirection.Backward)
                    {
                        if (SliceProccesed != null)
                        {
                            SliceProccesed(this, new SliceEventArgs(i));
                        }
                    }
                }

                try
                {
                    SetPicBoxImage(); //picBox.Image = operatedBitmap[selectedIndex];
                }
                catch { }
                //picBox.Refresh();
                Application.DoEvents();                

            }
            catch (System.Threading.ThreadAbortException)
            {                
                SetPicBoxImage();
            }

            processing = false;

            //DisposeToBeDisposed();
            //currentList.Clear();
            //InitOperatedOrigBitmaps();
            //initOrigRequired = false;
        }

        private void SetPicBoxImage()
        {
            viewer.picBoxMain.Image = operatedBitmap[selectedIndex];
            //viewer.picBoxMain.Invoke(new SetImageDelegate(viewer.picBoxMain.SetImage), new object[] { operatedBitmap[selectedIndex] });
            if (viewer.ZoomFit)
                viewer.PerformZoomFit();
            //viewer.Invoke(new SimpleDelegate(viewer.PerformZoomFit));
            else
                viewer.DoZoom(viewer.Zoom);
                //viewer.Invoke(new SimpleDelegateDouble(viewer.DoZoom), new object[] { viewer.Zoom });
            //viewer.Invoke(new SimpleDelegate(viewer.PerformZoomFit));           
            viewer.picBoxMain.Refresh();
            //viewer.picBoxMain.Invoke(new SimpleDelegate(viewer.picBoxMain.Refresh));

        }
    }

    class Op
    {
        public OperatorName OpName;
        public float param1;
        public double param2;
        public string param3;

        public Op(OperatorName OpName, float param1, double param2, string param3)
        {
            this.OpName = OpName;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
        }
    }

    enum OperatorName
    {
        SMOOTH
        ,EMBOSS
        ,CONTRAST
        ,BRIGHTNESS
        ,LUT
        ,ROTATE
        ,FLIP
        
    }
}
