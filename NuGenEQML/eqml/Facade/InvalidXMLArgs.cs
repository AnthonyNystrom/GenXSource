namespace Facade
{
    using System;

    public class InvalidXMLArgs : EventArgs
    {
        public InvalidXMLArgs(string sError, string sFileName, int nLine, int nPos)
        {
            this.error = sError;
            this.filename = sFileName;
            this.line = nLine;
            this.pos = nPos;
        }


        public string Filename
        {
            get
            {
                return this.filename;
            }
        }

        public string Error
        {
            get
            {
                return this.error;
            }
        }

        public int Pos
        {
            get
            {
                return this.pos;
            }
        }

        public int Line
        {
            get
            {
                return this.line;
            }
        }


        private int line;
        private int pos;
        private string error;
        private string filename;
    }
}

