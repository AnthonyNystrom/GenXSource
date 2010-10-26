using Facade;

namespace Fonts
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Xml;

    public class Entity2FontMapper
    {
        public Entity2FontMapper(FontCollection FontCollection, bool bRuntime)
        {
            this.mapFilename = "";
            this.fonts_ = null;
            this.fonts_ = FontCollection;

            try
            {
                this.uniCats_ = new UnicodeCategories("Binary");
                
                this.MasterFile = "FontMapping.xml";
                this.list_ = new ArrayList(0x834);
                this.codes_ = new Hashtable(0x834, 0.5f);
                this.names_ = new Hashtable(0x834, 0.5f);
                this.fontTablesHash_ = new Hashtable(10, 0.5f);
                this.fontTables_ = new ArrayList(10);

                this.ParseFontTables("Binary", "FontMapping.xml");
                this.ParseEntities("Binary", "FontMapping.xml");
            }
            catch
            {
            }
        }

        public void ClearFontTables()
        {
            for (int i = 0; i < this.ItemsCount; i++)
            {
                try
                {
                    if (((EntityInfo) this.list_[i]).fontTables != null)
                    {
                        ((EntityInfo) this.list_[i]).fontTables.Clear();
                    }
                    ((EntityInfo) this.list_[i]).fontTables = null;
                }
                catch
                {
                }
            }
            try
            {
                for (int i = 0; i < this.fontTables_.Count; i++)
                {
                    ((FontTable) this.fontTables_[i]).Clear();
                    try
                    {
                        this.fontTables_[i] = null;
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            try
            {
                this.fontTables_.Clear();
                this.fontTablesHash_.Clear();
                this.fontTables_ = null;
                this.fontTablesHash_ = null;
            }
            catch
            {
                return;
            }
        }

        public EntityInfo Get(int index)
        {
            try
            {
                if ((this.list_ != null) && (index < this.list_.Count))
                {
                    object o = this.list_[index];
                    return (EntityInfo) o;
                }
            }
            catch (Exception )
            {
                // FIXME: report unknown unicode cat
                
            }
            return null;
        }

        private bool HasFontFamily(string sFontFamily)
        {
            bool found = false;
            try
            {
                if (this.fonts_ != null)
                {
                    
                    if (this.fonts_.Get(sFontFamily) != null)
                    {
                        found = true;
                    }
                    
                }
                if (found)
                {
                    return found;
                }
                Font font = new Font(sFontFamily, 10f);
                if (font.FontFamily.Name == sFontFamily)
                {
                    found = true;
                }
                font = null;
            }
            catch
            {
            }
            return found;
        }

        public void ParseFamilyList(EntityInfo item, string sFontFamilyList)
        {
            try
            {
                if ((item == null) || (sFontFamilyList.Length <= 0))
                {
                    return;
                }
                item.FontTables = null;
                string[] strings = sFontFamilyList.Split(new char[] { ';' }, 10);
                if (strings.Length <= 0)
                {
                    return;
                }
                bool hasFf = false;
                for (int i = 0; i < strings.Length; i++)
                {
                    try
                    {
                        FontTable table = (FontTable) this.fontTablesHash_[strings[i]];
                        if (table != null)
                        {
                            
                                if (this.HasFontFamily(strings[i]) && !hasFf)
                                {
                                    Symbol symbol;
                                    hasFf = true;
                                    item.FontFamily = table.FontFamily;
                                    if (item.CodeAlias.Length > 0)
                                    {
                                        symbol = table.ByCode(item.CodeAlias);
                                        if (symbol != null)
                                        {
                                            item.CharPos = symbol.CharPos;
                                        }
                                    }
                                    else
                                    {
                                        symbol = table.ByCode(item.Code);
                                        if (symbol != null)
                                        {
                                            item.CharPos = symbol.CharPos;
                                        }
                                    }
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

        public EntityInfo ItemForEntity(string entityName)
        {
            try
            {
                object o = this.names_[entityName];
                if (o != null)
                {
                    return (EntityInfo) o;
                }
            }
            catch (Exception )
            {
                // FIXME: report unknown unicode cat
            }
            return null;
        }

        public EntityInfo ItemFromHex(string unicode)
        {
            try
            {
                object o = this.codes_[unicode];
                if (o != null)
                {
                    return (EntityInfo) o;
                }
            }
            catch (Exception )
            {
                // FIXME: report unknown unicode cat
            }
            return null;
        }

        public FontTable GetFontTable(int index)
        {
            try
            {
                if ((this.fontTables_ != null) && (index < this.fontTables_.Count))
                {
                    object o = this.fontTables_[index];
                    return (FontTable) o;
                }
            }
            catch
            {
            }
            return null;
        }

        public UnicodeCategory GetCategory(string unicode)
        {
            UnicodeCategory r = null;
            try
            {
                return this.uniCats_.Indexer(unicode);
            }
            catch
            {
				return r;
            }
        }

        public void ParseFontTables(string path, string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                Stream stream = ResourceLoader.GetStream(path, fileName);
                if (stream == null)
                {
                    return;
                }
                doc.Load(stream);
                stream.Close();
                
                XmlNode node = doc.DocumentElement;
                if (!node.HasChildNodes)
                {
                    return;
                }
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes[i].Name == "FontTables")
                    {
                        node = node.ChildNodes[i];
                        for (int j = 0; j < node.ChildNodes.Count; j++)
                        {
                            try
                            {
                                XmlNode child = node.ChildNodes[j];
                                if (child != null)
                                {
                                    XmlNode filenode = child.Attributes.GetNamedItem("file");
                                    string name = filenode.Value;
                                    try
                                    {
                                        if (name != this.mapFilename)
                                        {
                                            FontTable fontTable = new FontTable(path, name);
                                            if (fontTable != null)
                                            {
                                                
                                                    if (this.HasFontFamily(fontTable.FontFamily))
                                                    {
                                                        this.fontTables_.Add(fontTable);
                                                        this.fontTablesHash_.Add(fontTable.FontFamily, fontTable);
                                                    }
                                                
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
                    }
                }
            }
            catch
            {
            }
        }

        public void ParseEntities(string path, string fileName)
        {
            this.names_ = new Hashtable(0x834, 0.5f);
            try
            {
                XmlDocument doc = new XmlDocument();

                Stream stream = ResourceLoader.GetStream(path, fileName);
                if (stream == null)
                {
                    return;
                }
                doc.Load(stream);
                stream.Close();
               
               
                XmlNode n = doc.DocumentElement;
                if (n.HasChildNodes)
                {
                    for (int i = 0; i < n.ChildNodes.Count; i++)
                    {
                        if (n.ChildNodes[i].Name == "MathML_Entities")
                        {
                            n = n.ChildNodes[i];
                            int count = n.ChildNodes.Count;
                            for (int j = 0; j < count; j++)
                            {
                                try
                                {
                                    XmlNode xmlNode = n.ChildNodes[j];
                                    if (xmlNode != null)
                                    {
                                        EntityInfo entityInfo = this.ParseEntity(j, xmlNode);
                                        
                                        try
                                        {
                                            if (!this.names_.Contains(entityInfo.Name))
                                            {
                                                this.names_.Add(entityInfo.Name, entityInfo);
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        try
                                        {
                                            if (!this.codes_.Contains(entityInfo.Code))
                                            {
                                                this.codes_.Add(entityInfo.Code, entityInfo);
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        this.list_.Add(entityInfo);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                try
                {
                    doc.LoadXml("<root/>");
                    doc = null;
                }
                catch
                {
                }
                
            }
            catch
            {
            }
           
        }

        private EntityInfo ParseEntity(int index, XmlNode node)
        {
            EntityInfo info = null;
            try
            {
                XmlNode nameNode = node.Attributes.GetNamedItem("name");
                XmlNode uniNode = node.Attributes.GetNamedItem("unicode");
                XmlNode viNode = node.Attributes.GetNamedItem("visible");
                XmlNode uniAlias = node.Attributes.GetNamedItem("unicodeAlias");
                XmlNode ffNode = node.Attributes.GetNamedItem("font_family");
                XmlNode descNode = node.Attributes.GetNamedItem("description");
                XmlNode descFont = node.Attributes.GetNamedItem("description_font");
                string name = "";
                string code = "";
                string codeAlias = "";
                string description = "";
                string description_font = "";
                bool visible = true;
                if (nameNode != null)
                {
                    name = nameNode.Value;
                }
                if (uniNode != null)
                {
                    code = uniNode.Value;
                }
                if (viNode != null)
                {
                    visible = Convert.ToBoolean(viNode.Value);
                }
                if (uniAlias != null)
                {
                    codeAlias = uniAlias.Value;
                }
                if (descNode != null)
                {
                    description = descNode.Value;
                }
                if (descFont != null)
                {
                    description_font = descFont.Value;
                }
                info = new EntityInfo(index, name, code, codeAlias, description, description_font, visible);
                try
                {
                    info.Category = this.GetCategory(info.Code);
                }
                catch
                {
                }
                if (ffNode != null)
                {
                    string familyList = ffNode.Value;
                    if (familyList.Length <= 0)
                    {
                        return info;
                    }
                    this.ParseFamilyList(info, familyList);
                }
            }
            catch
            {
            }
            return info;
        }


        public string MasterFile
        {
            set
            {
                this.mapFilename = value.Trim();
            }
        }

        public int ItemsCount
        {
            get
            {
                int r = 0;
                try
                {
                    if (this.list_ != null)
                    {
                        r = this.list_.Count;
                    }
                }
                catch
                {
                    r = 0;
                }
                return r;
            }
        }

        public int FontTable_Count
        {
            get
            {
                int r = 0;
                try
                {
                    if (this.fontTables_ != null)
                    {
                        r = this.fontTables_.Count;
                    }
                }
                catch
                {
                }
                return r;
            }
        }
        
        private string mapFilename;
        private Hashtable names_;
        private Hashtable codes_;
        private ArrayList list_;
        private ArrayList fontTables_;
        private Hashtable fontTablesHash_;
        public UnicodeCategories uniCats_;
        private FontCollection fonts_;
    }
}

