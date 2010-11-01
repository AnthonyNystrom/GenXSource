namespace Operators
{
    using System;

    public class OperatorDictionary
    {
        public OperatorDictionary()
        {
            this.dict_ = null;
            this.dict_ = new OpDictImpl();
            try
            {
                this.dict_.LoadInternalDictionary();
            }
            catch
            {
            }
        }

        public Operator Indexer(string sUnicode, string sText, Form form)
        {
            Operator op = null;
            try
            {
                if (this.dict_ != null)
                {
                    if (sUnicode.Length > 0)
                    {
                        if (((form == Form.PREFIX) || (form == Form.INFIX)) || (form == Form.POSTFIX))
                        {
                            op = this.dict_.ByUnicode(sUnicode, form);
                        }
                        else
                        {
                            op = this.dict_.ByUnicode(sUnicode);
                        }
                    }
                    else if (sText.Length > 0)
                    {
                        if (((form == Form.PREFIX) || (form == Form.INFIX)) || (form == Form.POSTFIX))
                        {
                            op = this.dict_.ByText(sText, form);
                        }
                        else
                        {
                            op = this.dict_.ByText(sText);
                        }
                    }
                }
                if (op == null)
                {
                    op = new Operator();
                }
            }
            catch
            {
            }
            return op;
        }


        private OpDictImpl dict_;
    }
}

