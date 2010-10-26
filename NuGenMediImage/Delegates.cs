using System;

namespace Genetibase.NuGenMediImage
{
	public delegate void CollapsedEventHandler(object sender, EventArgs e);
    public delegate void ShowProgressCallback(int max, int value, string text);
    public delegate void SimpleDelegate();
    public delegate void SimpleDelegateDouble(double val);
    public delegate void AddSliceDelegate(System.Drawing.Bitmap b,int idx,int total);
    internal delegate void DicomSliceLoadEventHandler(object sender, DicomSliceLoadEventArgs e);
    internal delegate void DicomLoadEventHandler(object sender, DicomLoadEventArgs e);
    //internal delegate void LoadDicomDelegate(string Path);
    internal delegate void LoadImageHelperDelegate(ImageArray images);
    internal delegate void SliceProcessedEventHandler(object sender, SliceEventArgs e);
}
