using System.Xml;
using Genetibase.NuGenRenderCore.Logging;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Resources;
using Genetibase.VisUI.Scripting;

namespace Genetibase.VisUI.Resources
{
    class BooScriptContentLoader : RzContentTypeLoader
    {
        public BooScriptContentLoader()
            : base(new string[] { "text/boo" },
                   new string[] { ".boo" },
                   new int[] { 0 })
        {
        }

        public override IResource LoadContent(string filePath, string rzPath, string subPath, string contentType,
                                              XmlNodeList rzNode, out IResource[] loadedDependants,
                                              out IResource[] loadedDependancies, DeviceInterface devIf)
        {
            ILog log = devIf.CDI.GeneralLog;
            log.AddItem(new LogItem(string.Format("Starting loading script rz ([{0}]{1})", contentType, rzPath), LogItem.ItemLevel.DebugInfo));

            BooScript script = BooScript.LoadBooScript(rzPath, filePath);

            log.AddItem(new LogItem(string.Format("Loaded script rz ([{0}]{1})", contentType, rzPath), LogItem.ItemLevel.DebugInfo));

            loadedDependancies = null;
            loadedDependants = null;

            return script;
        }
    }
}