namespace Facade
{
    using System;

    public class FileInfoArgs : EventArgs
    {
        public FileInfoArgs(string sFileName, bool bMathImages, string sEncoding)
        {
            this.filename = sFileName;
            this.isImages = bMathImages;
            this.encoding = sEncoding;
        }
        
        public string filename;
        public bool isImages;
        public string encoding;
    }
}

