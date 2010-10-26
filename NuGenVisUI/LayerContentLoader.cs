using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Genetibase.NuGenRenderCore.Logging;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Resources;
using Genetibase.VisUI.Scripting;
using Genetibase.VisUI.UI;

namespace Genetibase.VisUI.Resources
{
    public class LayerContentLoader : RzContentTypeLoader
    {
        class GUIItemType_Config
        {
            // TODO: Change to allow optional/req checks?
            public bool usePos;
            public bool useSize;
            public bool useLayout;

            /// <summary>
            /// Initializes a new instance of the GUIItemType_Config class.
            /// </summary>
            /// <param name="usePos"></param>
            /// <param name="useSize"></param>
            /// <param name="useLayout"></param>
            public GUIItemType_Config(bool usePos, bool useSize, bool useLayout)
            {
                this.usePos = usePos;
                this.useSize = useSize;
                this.useLayout = useLayout;
            }
        }
        static readonly Dictionary<string, GUIItemType_Config> guiItemsConfig = new Dictionary<string, GUIItemType_Config>();

        public LayerContentLoader()
            : base(new string[] { "text/xml" },
                   new string[] { ".xml" },
                   new int[] { 0 })
        {
            if (guiItemsConfig.Count == 0)
            {
                guiItemsConfig["label"] = new GUIItemType_Config(true, true, true);
                guiItemsConfig["icon"] = new GUIItemType_Config(true, true, true);
            }
        }

        public override IResource LoadContent(string filePath, string rzPath, string subPath,
                                              string contentType, XmlNodeList rzNode,
                                              out IResource[] loadedDependants,
                                              out IResource[] loadedDependancies, DeviceInterface devIf)
        {
            ILog log = devIf.CDI.GeneralLog;
            log.AddItem(new LogItem(string.Format("Starting loading layer rz ([{0}]{1})", contentType, rzPath), LogItem.ItemLevel.DebugInfo));

            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);

            List<ISharableResource> sharedRzs = new List<ISharableResource>();

            // look for layer info
            XmlNode layerNode = xml.SelectSingleNode("/layer");
            Point position = ParsePoint(layerNode.Attributes["position"].InnerText);
            Size dimensions = ParseSize(layerNode.Attributes["size"].InnerText);
            string booClass = null, booFile = null;
            if (layerNode.Attributes["classFile"] != null)
                booFile = layerNode.Attributes["classFile"].InnerText;
            if (layerNode.Attributes["className"] != null)
                booClass = layerNode.Attributes["className"].InnerText;

            // parse boo script file
            BooScript script = null;
            StringBuilder scriptText;
            if (booFile != null && booClass != null)
            {
                string scriptRzPath = rzPath.Substring(0, rzPath.Length - Path.GetFileName(rzPath).Length) +  booFile;
                log.AddItem(new LogItem(string.Format("Found Layer Script ({0})", scriptRzPath), LogItem.ItemLevel.DebugInfo));
                script = (BooScript)devIf.GetSharedResource(scriptRzPath, ref sharedRzs);
                scriptText = new StringBuilder();
                scriptText.Append(script.Script);
            }
            else
            {
                log.AddItem(new LogItem("Generating Layer Script", LogItem.ItemLevel.DebugInfo));
                // build boo script for layer
                BooScriptBuilder.CreateClass(new string[] { "import Genetibase.VisUI.Scripting from \"NuGenVisUI\"" },
                                             "AutoGenLayer", "ScriptLayer", out scriptText);

                script = new BooScript(scriptText.ToString(), null);
                booClass = "AutoGenLayer";
            }
            
            // load resources
            Dictionary<string, IResource> refRzs = new Dictionary<string, IResource>();
            XmlElement resourcesNode = (XmlElement)layerNode.SelectSingleNode("resources");
            foreach (XmlNode node in resourcesNode.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    string rzName = node.Attributes["name"].InnerText;
                    string rzUrl = node.Attributes["url"].InnerText;
                    IResource rz = devIf.GetSharedResource(rzUrl, ref sharedRzs);
                    refRzs[rzName] = rz;
                }
            }

            SimpleGUILayer layer = new SimpleGUILayer(devIf, position, dimensions, rzPath, null, sharedRzs.ToArray());

            // pre-load script for layer
            XmlNode itemsNode = layerNode.SelectSingleNode("items");
            /*foreach (XmlNode node in itemsNode.ChildNodes)
            {
                // parse common events
                if (node.Attributes["onclick"] != null)
                {
                    string booCode = node.Attributes["onclick"].InnerText;
                    // insert into script
                    BooScriptBuilder.InsertFunction("", node.Name + "_onclick", "obj as object, args as MouseEventArgs", booCode, ref scriptText);
                }
            }*/

            // compile new script
            log.AddItem(new LogItem(string.Format("Compiling Layer Script (Length: {0})", scriptText.Length), LogItem.ItemLevel.DebugInfo));
            script = new BooScript(scriptText.ToString(), null);
            if (script.Compile())
                log.AddItem(new LogItem("Compiled Layer Script", LogItem.ItemLevel.Success));
            else
                log.AddItem(new LogItem("Failed to Compile Layer Script", LogItem.ItemLevel.Failure));

            // create class instance
            Type layerClass = script.GeneratedAssembly.GetType(booClass);
            ScriptLayer sLayer = (ScriptLayer)script.GeneratedAssembly.CreateInstance(booClass);
            
            // load items
            Font font = new Font("Tahoma", 10);
            foreach (XmlNode node in itemsNode.ChildNodes)
            {
                // TODO: Parse layout instructions
                // TODO: Log building?
                // Parse common attributes
                GUIItemType_Config itemConf;
                Point pos = Point.Empty;
                Size size = Size.Empty;
                LayoutRules.Positioning xLayout = LayoutRules.Positioning.Near;
                LayoutRules.Positioning yLayout = LayoutRules.Positioning.Near;
                MouseEventHandler onclickHandler = null;

                // parse and map common events
                if (node.Attributes["onclick"] != null)
                {
                    string methodName = node.Name + "_onclick";
                    BooScriptEventsBridge evBridge = new BooScriptEventsBridge(sLayer, layerClass.GetMethod(methodName));
                    onclickHandler = evBridge.MouseHandler;
                }

                if (guiItemsConfig.TryGetValue(node.Name, out itemConf))
                {
                    if (itemConf.usePos && node.Attributes["position"] != null)
                        pos = ParsePoint(node.Attributes["position"].InnerText);
                    if (itemConf.useSize && node.Attributes["size"] != null)
                        size = ParseSize(node.Attributes["size"].InnerText);
                    if (itemConf.useLayout && node.Attributes["layout"] != null)
                        ParseLayout(node.Attributes["layout"].InnerText, out xLayout, out yLayout);
                }
                GUILayerItem item = null;
                if (node.Name == "label")
                {
                    string text = node.Attributes["text"].InnerText;
                    
                    item = new GUILabel(text, font, Color.Red, pos, Size.Empty);
                }
                else if (node.Name == "icon")
                {
                    string imgRz = node.Attributes["img"].InnerText;
                    TextureResource.Icon icon = (TextureResource.Icon)refRzs[imgRz];
                    bool highlight = (node.Attributes["highlight"].InnerText == bool.TrueString);
                    bool enabled = (node.Attributes["enabled"].InnerText == bool.TrueString);

                    item = new GUIIcon(pos, size, icon, highlight, enabled);
                }

                if (item != null)
                {
                    item = LayoutManager.AlignItem(item, xLayout, yLayout);
                    if (onclickHandler != null)
                        layer.AddItem(item, null, null, onclickHandler);
                    else
                        layer.AddItem(item);
                }
            }

            log.AddItem(new LogItem(string.Format("Loaded layer rz ([{0}]{1})", contentType, rzPath), LogItem.ItemLevel.DebugInfo));

            loadedDependancies = null;
            loadedDependants = null;
            
            return layer;
        }

        private static void ParseLayout(string text, out LayoutRules.Positioning xLayout, out LayoutRules.Positioning yLayout)
        {
            string[] match = text.Split(' ');
            //Match match = Regex.Match(text, "[a-z]+ [a-z]+");
            if (match/*.Groups*/[0]/*.Value*/ == "near")
                xLayout = LayoutRules.Positioning.Near;
            else /*(match.Groups[0].Value == "far")*/
                xLayout = LayoutRules.Positioning.Far;

            if (match/*.Groups*/[1]/*.Value*/ == "near")
                yLayout = LayoutRules.Positioning.Near;
            else /*(match.Groups[1].Value == "far")*/
                yLayout = LayoutRules.Positioning.Far;
        }

        private static Size ParseSize(string text)
        {
            int[] ints = ParseIntArray(text);
            return new Size(ints[0], ints[1]);
        }

        private static Point ParsePoint(string text)
        {
            int[] ints = ParseIntArray(text);
            return new Point(ints[0], ints[1]);
        }

        private static int[] ParseIntArray(string text)
        {
            MatchCollection matches = Regex.Matches(text, "[0-9]+");
            if (matches.Count > 0)
            {
                int[] array = new int[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                {
                    array[i] = int.Parse(matches[i].Groups[0].Value);
                }
                return array;
            }
            return null;
        }
    }
}