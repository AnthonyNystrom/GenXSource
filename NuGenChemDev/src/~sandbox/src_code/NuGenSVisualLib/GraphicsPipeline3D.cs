using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.Pipelines
{
    public abstract class GraphicsPipeline3D
    {
        Stack<Matrix> matrixStack;

        public GraphicsPipeline3D()
        {
            this.matrixStack = new Stack<Matrix>();
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
            matrixStack.Push(ProjectionMatrix);
            matrixStack.Push(ViewMatrix);
            matrixStack.Push(WorldMatrix);
        }

        public void PopAll()
        {
            WorldMatrix = matrixStack.Pop();
            ViewMatrix = matrixStack.Pop();
            ProjectionMatrix = matrixStack.Pop();
        }
    }
}
