namespace Operators
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;

    public class OpDictImpl
    {
        public OpDictImpl()
        {
            this.operators = null;
            this.entities = null;
            this.texts = null;
            this.unicodes = null;
        }

        public Operator ByUnicode(string sUnicode)
        {
            sUnicode = sUnicode.Trim();
            Operator op = null;
            if (sUnicode.Length > 0)
            {
                try
                {
                    sUnicode = "00000" + sUnicode;
                    sUnicode = sUnicode.Substring(sUnicode.Length - 5, 5);
                    string key = "";
                    key = sUnicode + "_infix";
                    try
                    {
                        op = (Operator) this.unicodes[key];
                    }
                    catch
                    {
                    }
                    if (op == null)
                    {
                        key = sUnicode + "_prefix";
                        try
                        {
                            op = (Operator) this.unicodes[key];
                        }
                        catch
                        {
                        }
                    }
                    if (op != null)
                    {
                        return op;
                    }
                    key = sUnicode + "_postfix";
                    try
                    {
                        return (Operator) this.unicodes[key];
                    }
                    catch
                    {
                        return op;
                    }
                }
                catch
                {
                }
            }
            return op;
        }

        public Operator ByUnicode(string sUnicode, Form form)
        {
            Operator op = null;
            try
            {
                if (sUnicode.Length <= 0)
                {
                    return op;
                }
                sUnicode = "00000" + sUnicode;
                sUnicode = sUnicode.Substring(sUnicode.Length - 5, 5);
                string key = "";
                string frm = "";
                switch (form)
                {
                    case Form.PREFIX:
                    {
                        frm = "prefix";
                        break;
                    }
                    case Form.INFIX:
                    {
                        frm = "infix";
                        break;
                    }
                    case Form.POSTFIX:
                    {
                        frm = "postfix";
                        break;
                    }
                    default:
                    {
                        frm = "";
                        break;
                    }
                }
                key = sUnicode + "_" + frm;
                return (Operator) this.unicodes[key];
            }
            catch
            {
				return op;
            }
        }

        public Operator ByText(string sText)
        {
            sText = sText.Trim();
            Operator op = null;
            string key = "";
            key = sText + "_infix";
            try
            {
                op = (Operator) this.texts[key];
            }
            catch
            {
            }
            if (op == null)
            {
                key = sText + "_prefix";
                try
                {
                    op = (Operator) this.texts[key];
                }
                catch
                {
                }
            }
            if (op != null)
            {
                return op;
            }
            key = sText + "_postfix";
            try
            {
                return (Operator) this.texts[key];
            }
            catch
            {
				return op;
            }
        }

        public Operator ByText(string sText, Form form)
        {
            sText = sText.Trim();
            
            try
            {
                string key = "";
                string frm = "";
                switch (form)
                {
                    case Form.PREFIX:
                    {
                        frm = "prefix";
                        break;
                    }
                    case Form.INFIX:
                    {
                        frm = "infix";
                        break;
                    }
                    case Form.POSTFIX:
                    {
                        frm = "postfix";
                        break;
                    }
                    default:
                    {
                        frm = "";
                        break;
                    }
                }
                key = sText + "_" + frm;
                return (Operator) this.texts[key];
            }
            catch
            {
				return null;
            }
        }

        public void LoadInternalDictionary()
        {
            this.LoadDictionary("", true);
        }

        public void LoadDictionary(string path, bool bFromResource)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                if (bFromResource)
                {
                    Stream stream = Facade.ResourceLoader.GetStream("Binary", "Operator_Dictionary.xml");
                    document.Load(stream);
                }
                else
                {
                    document.Load(path);
                }
                XmlNode root = document.DocumentElement;
                if (!root.HasChildNodes)
                {
                    return;
                }
                
                this.operators = new ArrayList(root.ChildNodes.Count);
                this.entities = new Hashtable();
                this.texts = new Hashtable();
                this.unicodes = new Hashtable();
                
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    try
                    {
                        XmlNode opNode = root.ChildNodes[i];
                        Operator op = new Operator();
                        if (opNode.HasChildNodes && (opNode.FirstChild.NodeType == XmlNodeType.Text))
                        {
                            op.text = opNode.FirstChild.Value.Trim();
                        }
                        
                        int startIndex = 0;
                        int endIndex = 0;
                        startIndex = op.text.IndexOf("@", 0, op.text.Length);
                        if ((startIndex != -1) && (op.text.Length > startIndex))
                        {
                            startIndex++;
                            endIndex = op.text.IndexOf(";", startIndex, op.text.Length - startIndex);
                            if (endIndex != -1)
                            {
                                op.entity = op.text.Substring(startIndex, endIndex - startIndex);
                            }
                        }
                        
                        for (int attrIndex = 0; attrIndex < opNode.Attributes.Count; attrIndex++)
                        {
                            string formval;
                            XmlAttribute xmlAttribute = opNode.Attributes[attrIndex];
                            
                            switch (xmlAttribute.Name)
                            {
                                case "unicode":
                                {
                                    op.unicode = xmlAttribute.Value;
                                    break;
                                }
                                case "entity":
                                {
                                    op.entity = xmlAttribute.Value;
                                    break;
                                }
                                case "lspace":
                                {
                                    op.lspace = xmlAttribute.Value;
                                    break;
                                }
                                case "rspace":
                                {
                                    op.rspace = xmlAttribute.Value;
                                    break;
                                }
                                case "form":
                                    if ((formval = xmlAttribute.Value) == null)
                                    {
                                        op.form = Form.UNKNOWN;
                                        break;
                                    }
                                    formval = string.IsInterned(formval);
                                    if (formval == "prefix")
                                    {
                                        op.form = Form.PREFIX;
                                        break;
                                    }
                                    else if (formval == "infix")
                                    {
                                        op.form = Form.INFIX;
                                        break;
                                    }
                                    else if (formval == "postfix")
                                    {
                                        op.form = Form.POSTFIX;
                                        break;
                                    }
                                    else
                                    {
                                        op.form = Form.UNKNOWN;
                                        break;
                                    }

                                case "fence":
                                {
                                    op.fence = Convert.ToBoolean(xmlAttribute.Value);
                                    break;
                                }
                                case "separator":
                                {
                                    op.separator = Convert.ToBoolean(xmlAttribute.Value);
                                    break;
                                }
                                case "stretchy":
                                {
                                    op.stretchy = Convert.ToBoolean(xmlAttribute.Value);
                                    break;
                                }
                                case "symmetric":
                                {
                                    op.symmetric = Convert.ToBoolean(xmlAttribute.Value);
                                    break;
                                }
                                case "maxsize":
                                {
                                    op.maxsize = xmlAttribute.Value;
                                    break;
                                }
                                case "minsize":
                                {
                                    op.minsize = xmlAttribute.Value;
                                    break;
                                }
                                case "largeop":
                                {
                                    op.largeop = Convert.ToBoolean(xmlAttribute.Value);
                                    break;
                                }
                                case "movablelimits":
                                {
                                    op.movablelimits = Convert.ToBoolean(xmlAttribute.Value);
                                    break;
                                }
                                case "accent":
                                {
                                    op.accent = Convert.ToBoolean(xmlAttribute.Value);
                                    break;
                                }
                                case "active":
                                {
                                    op.active = Convert.ToBoolean(xmlAttribute.Value);
                                    break;
                                }
                            }
                        }
                        string form = "";
                        try
                        {
                            switch (op.form)
                            {
                                case Form.PREFIX:
                                {
                                    form = "prefix";
                                    break;
                                }
                                case Form.INFIX:
                                {
                                    form = "infix";
                                    break;
                                }
                                case Form.POSTFIX:
                                {
                                    form = "postfix";
                                    break;
                                }
                            }
                            form = "";
                        }
                        catch
                        {
                        }
                    
                        this.operators.Add(op);
                        if (op.entity.Length > 0)
                        {
                            try
                            {
                                this.entities.Add(op.entity + "_" + form, op);
                            }
                            catch
                            {
                            }
                        }
                        if (op.text.Length > 0)
                        {
                            try
                            {
                                if (!this.texts.Contains(op.text + "_" + form))
                                {
                                    this.texts.Add(op.text + "_" + form, op);
                                }
                            }
                            catch
                            {
                            }
                        }
                        if (op.unicode.Length > 0)
                        {
                            try
                            {
                                if (!this.unicodes.Contains(op.unicode + "_" + form))
                                {
                                    this.unicodes.Add(op.unicode + "_" + form, op);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        private ArrayList operators;
        private Hashtable entities;
        private Hashtable texts;
        private Hashtable unicodes;
    }
}

