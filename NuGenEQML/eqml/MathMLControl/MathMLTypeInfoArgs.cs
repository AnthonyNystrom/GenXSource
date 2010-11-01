namespace Genetibase.MathX
{
    using System;

    public class MathMLTypeInfoArgs : EventArgs
    {
        public MathMLTypeInfoArgs(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }

        private string text;
    }
}

