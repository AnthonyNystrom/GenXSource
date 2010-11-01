namespace Nodes
{
    using System;
    using System.Collections.Generic;

    public enum ElementType
    {
        Math,
        Mi,
        Mn,
        Mo,
        Mtext,
        Mspace,
        Ms,
        Mglyph,
        Mfrac,
        Msqrt,
        Mroot,
        Mrow,
        Mfenced,
        Mpadded,
        Mstyle,
        Mphantom,
        Merror,
        Menclose,
        Msup,
        Msub,
        Msubsup,
        Mover,
        Munder,
        Munderover,
        Mmultiscripts,
        Mprescripts,
        Mnone,
        Mtable,
        Mtr,
        Mtd,
        Mlabeledtr,
        Maligngroup,
        Malignmark,
        Maction,
        Entity,
        UNKNOWN
    }

    public class Type
    {
        public Type(string pXmlTag, ElementType type, int minChilds, int maxChilds)
        {
            this.type = type;
            this.xmlTag = pXmlTag;
            this.minChilds = minChilds;
            this.maxChilds = maxChilds;
        }

        public ElementType type;
        public string xmlTag;
        public int minChilds;
        public int maxChilds;
    }

    public class Types
    {
        public Types()
        {
            Register(new Type("math", ElementType.Math, 1, -1));
            Register(new Type("mi", ElementType.Mi, 0, 0));
            Register(new Type("mn", ElementType.Mn, 0, 0));
            Register(new Type("mo", ElementType.Mo, 0, 0));
            Register(new Type("mtext", ElementType.Mtext, 0, -1));
            Register(new Type("mspace", ElementType.Mspace, 0, 0));
            Register(new Type("ms", ElementType.Ms, 0, -1));
            Register(new Type("mglyph", ElementType.Mglyph, 0, 0));
            Register(new Type("mfrac", ElementType.Mfrac, 2, 2));
            Register(new Type("msqrt", ElementType.Msqrt, 1, 1));
            Register(new Type("mroot", ElementType.Mroot, 2, 2));
            Register(new Type("mrow", ElementType.Mrow, 0, -1));
            Register(new Type("maction", ElementType.Maction, 1, -1));
            Register(new Type("mfenced", ElementType.Mfenced, 1, -1));
            Register(new Type("mpadded", ElementType.Mpadded, 1, -1));
            Register(new Type("mstyle", ElementType.Mstyle, 1, -1));
            Register(new Type("mphantom", ElementType.Mphantom, 1, -1));
            Register(new Type("merror", ElementType.Merror, 0, -1));
            Register(new Type("menclose", ElementType.Menclose, 1, -1));
            Register(new Type("msup", ElementType.Msup, 2, 2));
            Register(new Type("msub", ElementType.Msub, 2, 2));
            Register(new Type("msubsup", ElementType.Msubsup, 3, 3));
            Register(new Type("mover", ElementType.Mover, 2, 2));
            Register(new Type("munder", ElementType.Munder, 2, 2));
            Register(new Type("munderover", ElementType.Munderover, 3, 3));
            Register(new Type("mmultiscripts", ElementType.Mmultiscripts, 3, 6));
            Register(new Type("mprescripts", ElementType.Mprescripts, 0, 0));
            Register(new Type("none", ElementType.Mnone, 0, 0));
            Register(new Type("mtable", ElementType.Mtable, 0, -1));
            Register(new Type("mtr", ElementType.Mtr, 0, -1));
            Register(new Type("mtd", ElementType.Mtd, 1, -1));
            Register(new Type("mlabeledtr", ElementType.Mlabeledtr, 1, -1));
            Register(new Type("maligngroup", ElementType.Maligngroup, 1, -1));
            Register(new Type("malignmark", ElementType.Malignmark, 0, 0));
            Register(new Type("entity", ElementType.Entity, 0, 0));
            
        }

        public Type this[string XMLTag]
        {
            get
            {
                Type type = this.hash_[XMLTag];
                if (type == null)
                {
                    return new Type(XMLTag, ElementType.UNKNOWN, 0, -1);
                }
                return type;
            }
        }

        private void Register(Type type)
        {
            hash_[type.xmlTag] = type;
        }
        
        private IDictionary<string, Type> hash_ = new Dictionary<string, Type>();
    }
}


