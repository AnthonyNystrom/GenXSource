using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Genetibase.NuGenRenderCore.Settings
{
    /// <summary>
    /// Encapsulates a settings dictionary
    /// </summary>
    public class HashTableSettings : ISettings
    {
        public static Dictionary<string, object> globalTable = new Dictionary<string,object>();
        readonly Dictionary<string, object> overridesTable;
        bool globalOnly;

        private static HashTableSettings instance;

        public static HashTableSettings Instance
        {
            get { return instance; }
        }

        public HashTableSettings()
        {
            instance = this;
            overridesTable = new Dictionary<string, object>();
            globalOnly = false;
        }

        public bool GlobalOnly
        {
            get { return globalOnly; }
            set { globalOnly = value; }
        }

        public static HashTableSettings LoadFromXml(StreamReader stream)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(stream);
            return LoadFromXml(xml);
        }

        public static HashTableSettings LoadFromXml(string xmlText, bool isFilePath)
        {
            if (isFilePath)
            {
                StreamReader reader = new StreamReader(xmlText);
                HashTableSettings settings = LoadFromXml(reader);
                reader.Close();
                return settings;
            }
            else
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlText);
                return LoadFromXml(xml);
            }
        }

        public static HashTableSettings LoadFromXml(XmlDocument xml)
        {
            HashTableSettings settings = new HashTableSettings();

            XmlNodeList values = xml.SelectNodes("settings/values/valueType");

            foreach (XmlNode value in values)
            {
                string token = value.SelectSingleNode("@token").InnerText;
                XmlNode typeNode = value.SelectSingleNode("@type");
                //if (typeNode != null)
                //{
                    string type = typeNode.InnerText;
                    string data = value.SelectSingleNode("@data").InnerText;
                    object obj = null;

                    if (type == "Int32")
                        obj = Int32.Parse(data);
                    else if (type == "String")
                        obj = data;
                    else if (type == "Float")
                        obj = float.Parse(data);
                    else if (type == "Bool")
                        obj = bool.Parse(data);
                    else if (type == "Color")
                        obj = Color.FromArgb(Int32.Parse(data));
                    else if (type == "Byte")
                        obj = byte.Parse(data);

                    if (obj != null)
                        globalTable[token] = obj;
                //}
                //else
                //{
                //    string asmName = value.SelectSingleNode("@assembly").InnerText;
                //    string typeName = value.SelectSingleNode("@typeName").InnerText;

                //    ObjectHandle handle = System.Activator.CreateInstance(asmName, typeName);
                //    settings[token] = handle.Unwrap();
                //}
            }
            return settings;
        }

        public void ExportToXml(string filename)
        {
            using (FileStream stream = File.Open(filename, FileMode.Create))
            {
                ExportToXml(stream);
            }
        }

        public void ExportToXml(Stream stream)
        {
            XmlWriter xml = XmlWriter.Create(stream);

            xml.WriteStartDocument();

            xml.WriteStartElement("settings");
            xml.WriteStartElement("values");

            foreach (KeyValuePair<string, object> setting in overridesTable)
            {
                Type type = setting.Value.GetType();
                xml.WriteStartElement("valueType");
                xml.WriteAttributeString("token", setting.Key);
                xml.WriteAttributeString("type", type.Name);

                string data;
                if (type == typeof(Color))
                    data = ((Color)setting.Value).ToArgb().ToString();
                else
                    data = setting.Value.ToString();

                xml.WriteAttributeString("data", data);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
            xml.WriteEndElement();

            xml.WriteEndDocument();

            xml.Close();
        }

        #region ISettings Members

        public object GetSettingObj(string token)
        {
            return overridesTable[token];
        }

        public int GetSettingInt32(string token)
        {
            return (int)overridesTable[token];
        }

        public string GetSettingString(string token)
        {
            return (string)overridesTable[token];
        }

        public object this[string key]
        {
            get
            {
                // check overrides first
                object obj;
                if (!globalOnly && overridesTable.TryGetValue(key, out obj))
                    return obj;
                // fallback to globals
                return globalTable[key];
            }
            set { if (!globalOnly) overridesTable[key] = value; else globalTable[key] = value; }
        }

        public bool TryGetValue(string key, out object value)
        {
            try
            {
                value = this[key];
                return true;
            }
            catch (Exception)
            { }
            value = null;
            return false;
        }
        #endregion
    }
}
