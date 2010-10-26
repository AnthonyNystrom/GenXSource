using System.Drawing;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Resources;

namespace Genetibase.VisUI.UI
{
    public class LoadingLayer : SimpleGUILayer
    {
        GUIProgressBar progressBar;
        GUILabel loadingTitle;

        /// <summary>
        /// Encapsulates a layer attached to a loading thread
        /// </summary>
        /// <param name="devIf"></param>
        /// <param name="position"></param>
        /// <param name="dimensions"></param>
        /// <param name="id"></param>
        /// <param name="dependants"></param>
        /// <param name="dependancies"></param>
        public LoadingLayer(DeviceInterface devIf, Point position, Size dimensions, string id, IResource[] dependants,
                            ISharableResource[] dependancies)
            : base(devIf, position, dimensions, id, dependants, dependancies)
        {
            AddItem(LayoutManager.AlignItem(progressBar = new GUIProgressBar(new Point(0, 0), new Size(200, 20), Color.Blue),
                                            LayoutRules.Positioning.Center, LayoutRules.Positioning.Center));
            progressBar.Progress = 0;
        }

        public void SetProgress(int progress)
        {
            progressBar.Progress = progress;
        }

        public void SetText(string text)
        {
            // TODO: Allow text resizing w/layout etc.
            // just do a hack for now I guess
            if (loadingTitle != null)
                RemoveItem(loadingTitle);

            AddItem(LayoutManager.AlignItem(loadingTitle = new GUILabel(text, new Font("Verdana", 12, FontStyle.Bold), Color.Blue, new Point(0, -30), Size.Empty),
                                            LayoutRules.Positioning.Center, LayoutRules.Positioning.Center));
        }
    }
}