namespace Genetibase.MathX
{
    using System;

    public class ValidationErrorEventArgs : EventArgs
    {
        public ValidationErrorEventArgs(string message, int line, int pos)
        {
            this.ValidationErrorEventArgs_f_0 = "";
            this.ValidationErrorEventArgs_f_1 = 0;
            this.ValidationErrorEventArgs_f_2 = 0;
            this.ValidationErrorEventArgs_f_0 = message;
            this.ValidationErrorEventArgs_f_1 = line;
            this.ValidationErrorEventArgs_f_2 = pos;
        }


        public int ErrorLine
        {
            get
            {
                return this.ValidationErrorEventArgs_f_1;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.ValidationErrorEventArgs_f_0;
            }
        }

        public int ErrorPos
        {
            get
            {
                return this.ValidationErrorEventArgs_f_2;
            }
        }


        private string ValidationErrorEventArgs_f_0;
        private int ValidationErrorEventArgs_f_1;
        private int ValidationErrorEventArgs_f_2;
    }
}

