namespace UI
{
    using System;

    public class ValidationErrorArgs : EventArgs
    {
        public ValidationErrorArgs(string message, int line, int pos)
        {
            this.message = "";
            this.line = 0;
            this.pos = 0;
            this.message = message;
            this.line = line;
            this.pos = pos;
        }
        
        public string Message
        {
            get
            {
                return this.message;
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


        private string message;
        private int line;
        private int pos;
    }
}

