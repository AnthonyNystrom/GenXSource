using System;
using System.Collections.Generic;
using System.Text;
using NuGenVideoEnc;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Recording
{
    interface IRecordingRenderer
    {
        void Rotate(Matrix rotation, float zoomLevel);
        void Render(int index, Surface renderTarget);
        Device Device { get; }
    }

    public class RecordingSettings
    {
        private static RecordingSettings defaultsInstance = null;

        ICodec codec;
        int height;
        int width;
        int fps;

        public ICodec Codec
        {
            get { return codec; }
            set { codec = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int FramesPerSecond
        {
            get { return fps; }
            set { fps = value; }
        }

        public static RecordingSettings DefaultsInstance
        {
            get
            {
                if (defaultsInstance == null)
                {
                    StringReader stream = new StringReader(Properties.Resources.defaultRecSettings);
                    defaultsInstance = RecordingSettings.FromXml(stream);
                    stream.Close();
                    stream.Dispose();
                }

                return defaultsInstance;
            }
        }

        public static RecordingSettings FromXml(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open);
            RecordingSettings settings = FromXml(file);
            file.Close();
            return settings;
        }

        public static RecordingSettings FromXml(TextReader stream)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(stream);
            return FromXml(xml);
        }

        public static RecordingSettings FromXml(Stream stream)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(stream);
            return FromXml(xml);
        }

        private static RecordingSettings FromXml(XmlDocument xml)
        {
            RecordingSettings settings = new RecordingSettings();

            XmlNode sNode = xml.SelectSingleNode("recordingSettings");
            settings.Width = int.Parse(sNode.SelectSingleNode("@width").InnerText);
            settings.Height = int.Parse(sNode.SelectSingleNode("@height").InnerText);
            settings.FramesPerSecond = int.Parse(sNode.SelectSingleNode("@fps").InnerText);
            string codecName = sNode.SelectSingleNode("@codec").InnerText;
            foreach (ICodec codec in VideoEncodingInterface.AvailableCodecs)
            {
                if (codec.Name == codecName)
                {
                    settings.Codec = codec;
                    break;
                }
            }

            return settings;
        }

        public void ToXmlFile(string filename)
        {
            XmlWriter xml = XmlWriter.Create(filename);

            xml.WriteStartDocument();

            xml.WriteStartElement("recordingSettings");

            xml.WriteAttributeString("width", this.Width.ToString());
            xml.WriteAttributeString("height", this.Height.ToString());
            xml.WriteAttributeString("codec", this.Codec.Name);
            xml.WriteAttributeString("fps", this.FramesPerSecond.ToString());

            xml.WriteEndElement();

            xml.WriteEndDocument();

            xml.Close();
        }
    }
}