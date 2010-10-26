using Genetibase.NuGenRenderCore.Rendering;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Scene
{
    public abstract class SceneManager
    {
        protected Device gDevice;

        public SceneManager(Device gDevice)
        {
            this.gDevice = gDevice;
        }

        public abstract void OnSourceDataModified();
        public abstract void RenderSceneFrame(GraphicsPipeline gPipeline, int width, int height);
        public abstract void AddEntity(object entity);
        public abstract void RemoveEntity(object entity);
    }
}