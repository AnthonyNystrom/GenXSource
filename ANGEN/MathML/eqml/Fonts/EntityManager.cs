namespace Fonts
{
    using System;
    using System.Collections;

    public class EntityManager
    {
        public EntityManager(FontCollection FontCollection)
        {
            this.fonts_ = null;
            
            this.fonts_ = FontCollection;
            this.mapper_ = new Entity2FontMapper(this.fonts_, true);
            this.list_ = new ArrayList(5);
            this.fontFamilyInfos_ = new Hashtable(5, 0.5f);
            for (int i = 0; i < this.mapper_.FontTable_Count; i++)
            {
                FontFamilyInfo fontFamilyInfo = new FontFamilyInfo();
                FontTable fontTable = this.mapper_.GetFontTable(i);
                fontFamilyInfo.FontFamily = fontTable.FontFamily;
                fontFamilyInfo.Name = fontTable.Name;
                fontFamilyInfo.CorrectB = fontTable.CorrectB;
                fontFamilyInfo.CorrectH = fontTable.CorrectH;
                fontFamilyInfo.CorrectW = fontTable.CorrectW;
                fontFamilyInfo.CorrectY = fontTable.CorrectY;
                fontFamilyInfo.CorrectX = fontTable.CorrectX;
                this.list_.Add(fontFamilyInfo);
                this.fontFamilyInfos_.Add(fontFamilyInfo.FontFamily, fontFamilyInfo);
            }
            this.mapper_.ClearFontTables();
            this.ids_ = new MapItems();
            this.ops_ = new MapItems();
            try
            {
                for (int i = 0; i < this.mapper_.uniCats_.Count; i++)
                {
                    bool isActive = false;
                    UnicodeCategory unicodeCategory = this.mapper_.uniCats_.Indexer(i);
                    isActive = false;
                    switch (unicodeCategory.ID)
                    {
                        case 1:
                        case 3:
                        case 4:
                        case 7:
                        case 9:
                        case 11:
                        case 12:
                        case 14:
                        case 0x16:
                        case 0x1d:
                        case 0x1f:
                        case 0x20:
                            isActive = true;
                            break;
                    }
                    this.ids_.Put(unicodeCategory.ID, unicodeCategory.Name, isActive);
                    
                    isActive = false;
                    switch (unicodeCategory.ID)
                    {
                        case 1:
                        case 11:
                        case 12:
                        case 0x10:
                        case 0x11:
                        case 0x12:
                        case 0x18:
                        case 0x19:
                        case 0x1d:
                        case 0x20:
                            isActive = true;
                            break;
                    }
                    this.ops_.Put(unicodeCategory.ID, unicodeCategory.Name, isActive);
                }
                for (int g = 0; g < this.Count; g++)
                {
                    Glyph glyph = this.Get(g);
                    if ((glyph != null) && glyph.IsVisible)
                    {
                        this.ops_.Ref(glyph.Category.ID, true);
                        this.ids_.Ref(glyph.Category.ID, true);
                        if (glyph.FontFamily.Length > 0)
                        {
                            this.ops_.Ref(glyph.Category.ID, false);
                            this.ids_.Ref(glyph.Category.ID, false);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public Glyph ByUnicode(string unicode)
        {
            Glyph glyph = null;
            EntityInfo entityInfo = null;
            try
            {
                entityInfo = this.mapper_.ItemFromHex(unicode.PadLeft(5, '0'));
                if (entityInfo != null)
                {
                    glyph = new Glyph(entityInfo);
                }
            }
            catch
            {
            }
            return glyph;
        }

        public Glyph ByName(string name)
        {
            Glyph glyph = null;
            EntityInfo entityInfo = null;
            try
            {
                entityInfo = this.mapper_.ItemForEntity(name);
                if (entityInfo != null)
                {
                    glyph = new Glyph(entityInfo);
                }
            }
            catch
            {
            }
            return glyph;
        }

        public Glyph Get(int index)
        {
            Glyph glyph = null;
            EntityInfo entityInfo = null;
            try
            {
                entityInfo = this.mapper_.Get(index);
                if (entityInfo != null)
                {
                    glyph = new Glyph(entityInfo);
                }
            }
            catch
            {
            }
            return glyph;
        }

        public string GenerateXMLEntities(string xml)
        {
            string s = "";
            try
            {
                int count = 0;
                s = "<!DOCTYPE entities [" + Environment.NewLine;
                int index = 0;
                int semiIndex = 0;
                while ((index != -1) && (index < xml.Length))
                {
                    index = xml.IndexOf("&", index);
                    if ((index != -1) && (index < xml.Length))
                    {
                        semiIndex = xml.IndexOf(";", index);
                        if ((semiIndex < xml.Length) && ((semiIndex - index) < 100))
                        {
                            int startIndex = index + 1;
                            int e = semiIndex;
                            if (((e > startIndex) && ((e - startIndex) > 1)) && ((e < xml.Length) && (startIndex < xml.Length)))
                            {
                                try
                                {
                                    string entityName = xml.Substring(startIndex, e - startIndex);
                                    EntityInfo entityInfo = this.mapper_.ItemForEntity(entityName);
                                    if (entityInfo != null)
                                    {
                                        s = s + string.Concat(new string[] { "<!ENTITY ", entityInfo.Name, " '&#x", entityInfo.Code, ";'>", Environment.NewLine });
                                        count++;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        index++;
                    }
                }
                s = s + "]>";
                if (count > 0)
                {
                    return s;
                }
            }
            catch
            {
            }
            return "";
        }

        public bool TryConvertHexToEntities(string sSource, ref string xml)
        {
            try
            {
                string s = sSource;
                int count = 0;
                int index = 0;
                int semiIndex = 0;
                while ((index != -1) && (index < s.Length))
                {
                    index = s.IndexOf("&", index);
                    if ((index != -1) && (index < s.Length))
                    {
                        semiIndex = s.IndexOf(";", index);
                        if ((semiIndex < s.Length) && ((semiIndex - index) < 100))
                        {
                            int p = index + 1;
                            int e = semiIndex;
                            if (((e > p) && ((e - p) > 1)) && ((e < s.Length) && (p < s.Length)))
                            {
                                try
                                {
                                    string entityName = s.Substring(p, e - p);
                                    if (!this.InTag(s, p) && (entityName != "lt"))
                                    {
                                        EntityInfo entityInfo = this.mapper_.ItemForEntity(entityName);
                                        if (entityInfo != null)
                                        {
                                            s = s.Remove(p, e - p);
                                            s = s.Insert(p, "#x" + entityInfo.Code);
                                            count++;
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        index++;
                    }
                }
                if (count > 0)
                {
                    xml = s;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public bool ReplaceEntities(string sSource, ref string outXml)
        {
            try
            {
                string s = sSource;
                int count = 0;
                int index = 0;
                int semiIndex = 0;
                while ((index != -1) && (index < s.Length))
                {
                    index = s.IndexOf("&", index);
                    if ((index != -1) && (index < s.Length))
                    {
                        semiIndex = s.IndexOf(";", index);
                        if ((((semiIndex < s.Length) && ((semiIndex - index) > 2)) && (((semiIndex - index) < 100) && (s[index + 1] == '#'))) && (s[index + 2] == 'x'))
                        {
                            int p = index + 1;
                            int e = semiIndex;
                            if (((e > p) && ((e - p) > 1)) && ((e < s.Length) && (p < s.Length)))
                            {
                                try
                                {
                                    string unicode = s.Substring(p + 2, (e - p) - 2);
                                    unicode = unicode.PadLeft(5, '0');
                                    if (!this.InTag(s, p))
                                    {
                                        if (unicode.Length > 0)
                                        {
                                            EntityInfo entityInfo = this.mapper_.ItemFromHex(unicode);
                                            if (entityInfo != null)
                                            {
                                                if (entityInfo.Name.Length > 0)
                                                {
                                                    s = s.Remove(p, e - p);
                                                    s = s.Insert(p, entityInfo.Name);
                                                }
                                                else
                                                {
                                                    s = s.Remove(p, e - p);
                                                    s = s.Insert(p, "quest");
                                                }
                                                count++;
                                            }
                                            else
                                            {
                                                s = s.Remove(p, e - p);
                                                s = s.Insert(p, "quest");
                                                count++;
                                            }
                                        }
                                        else
                                        {
                                            s = s.Remove(p, e - p);
                                            s = s.Insert(p, "quest");
                                            count++;
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        index++;
                    }
                }
                if (count > 0)
                {
                    outXml = s;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public bool TryConvertCommonOperators(string sSource, ref string outXml)
        {
            try
            {
                string s = sSource;
                int p = s.IndexOf("<mo>+</mo>", 0, s.Length);
                int m = s.IndexOf("<mo>-</mo>", 0, s.Length);
                int e = s.IndexOf("<mo>=</mo>", 0, s.Length);
                if (((p != -1) || (m != -1)) || (e != -1))
                {
                    s = s.Replace("<mo>+</mo>", "<mo>&plus;</mo>");
                    s = s.Replace("<mo>-</mo>", "<mo>&minus;</mo>");
                    s = s.Replace("<mo>=</mo>", "<mo>&equals;</mo>");
                    outXml = s;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public FontFamilyInfo GetFontFamilyInfo(string sFontFamily)
        {
            FontFamilyInfo familyInfo = null;
            try
            {
                object o = this.fontFamilyInfos_[sFontFamily];
                if (o != null)
                {
                    familyInfo = (FontFamilyInfo) o;
                }
            }
            catch
            {
            }
            return familyInfo;
        }

        private bool InTag(string sXML, int p)
        {
            try
            {
                int c = sXML.IndexOf(">", p, sXML.Length - p);
                int o = sXML.IndexOf("<", p, sXML.Length - p);
                if (((c != -1) && (o != -1)) && (c < o))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }


        public int Count
        {
            get
            {
                int r = 0;
                try
                {
                    if (this.mapper_ == null)
                    {
                        return r;
                    }
                    return this.mapper_.ItemsCount;
                }
                catch
                {
                }
                return r;
            }
        }

        public Entity2FontMapper mapper_;
        private ArrayList list_;
        private Hashtable fontFamilyInfos_;
        public MapItems ops_;
        public MapItems ids_;
        private FontCollection fonts_;
    }
}

