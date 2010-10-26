using System.Drawing;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Genetibase.NuGenRenderCore.Settings;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.VisUI.Controls
{
    public delegate void RenderUpdateDelegate(double time);
    public delegate void EntitySelectionDelegate(IEntity[] entities, bool add);
    public delegate void SceneEntityUpdate(SceneEntity entity, bool added);

    public enum ControlMode
    {
        ViewMovement,
        Selection,
        SelectionMovement,
        SelectionRotation
    }

    public enum ControlStatus
    {
        Loading,
        Idle,
        Movement
    }

    public struct LayerInfo
    {
        public string Name;
        public Size Size;
        public Image Preview;
    }

    public struct ScreenshotSettings
    {
        public enum OutputDestination
        {
            File,
            Bitmap,
            Clipboard
        }

        public OutputDestination Destination;
        public string File;
        public ImageFileFormat Format;
        public Size Resolution;
    }

    public interface IUI3DControl
    {
        ControlMode ControlMode { get; set; }
        ControlStatus Status { get; }

        void Init(HashTableSettings settings, ICommonDeviceInterface cdi);
        Color BackColor { get; set; }
        string Title { get; }

        HashTableSettings Settings { get; }

        // display
        void TakeScreenshot(string file, ImageFileFormat format);
        void TakeScreenshot(ScreenshotSettings settings);
        bool LowDetailMovement { get; set; }
        double LastRenderTime { get; }
        RenderUpdateDelegate OnRenderUpdate { get; set; }

        // layers
        LayerInfo[] GetLayersInfo();

        // entities
        event EntitySelectionDelegate OnEntitySelected;
        event SceneEntityUpdate OnSceneModified;

        SceneEntity[] GetSceneEntities();
    }
}