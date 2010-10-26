using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering
{
    public abstract class MatrixStack3D
    {
        readonly Stack<Matrix> stack;

        public MatrixStack3D()
        {
            stack = new Stack<Matrix>();
        }

        public abstract Matrix WorldMatrix
        {
            get;
            set;
        }

        public abstract Matrix ViewMatrix
        {
            get;
            set;
        }

        public abstract Matrix ProjectionMatrix
        {
            get;
            set;
        }

        public abstract Material Material
        {
            get;
            set;
        }

        public void PushAll()
        {
            stack.Push(ProjectionMatrix);
            stack.Push(ViewMatrix);
            stack.Push(WorldMatrix);
        }

        public void PopAll()
        {
            WorldMatrix = stack.Pop();
            ViewMatrix = stack.Pop();
            ProjectionMatrix = stack.Pop();
        }
    }

    public class DeviceMatrixStack3D : MatrixStack3D
    {
        readonly Device device;

        public DeviceMatrixStack3D(Device device)
        {
            this.device = device;
        }

        public override Matrix WorldMatrix
        {
            get { return device.Transform.World; }
            set { device.Transform.World = value; }
        }

        public override Matrix ViewMatrix
        {
            get { return device.Transform.View; }
            set { device.Transform.View = value; }
        }

        public override Matrix ProjectionMatrix
        {
            get { return device.Transform.Projection; }
            set { device.Transform.Projection = value; }
        }

        public override Material Material
        {
            get { return device.Material; }
            set { device.Material = value; }
        }
    }
}