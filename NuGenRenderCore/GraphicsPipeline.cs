using System.Collections.Generic;
using Genetibase.NuGenRenderCore.Shaders;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering
{
    public class GraphicsPipeline
    {
        readonly Device gDevice;
        readonly Stack<Matrix> matrixStack;
        
        bool matrixUpdateFrozen;
        bool shaderEnabled;

        Matrix worldMatrix, viewMatrix, projectionMatrix;
        ShaderInterface shaderIf;

        bool startedScene;
        bool keepSceneState;

        public GraphicsPipeline(Device gDevice)
        {
            this.gDevice = gDevice;

            matrixStack = new Stack<Matrix>();
            startedScene = false;
        }

        #region Properties

        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
            set
            {
                worldMatrix = value;
                if (!matrixUpdateFrozen)
                {
                    if (!shaderEnabled)
                        gDevice.Transform.World = worldMatrix;
                    else
                    {
                        shaderIf.WorldMatrix = worldMatrix;
                        shaderIf.WorldViewProjectionMatrix = worldMatrix * viewMatrix * projectionMatrix;
                    }
                }
            }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set
            {
                viewMatrix = value;
                if (!matrixUpdateFrozen)
                {
                    if (!shaderEnabled)
                        gDevice.Transform.View = viewMatrix;
                    else
                    {
                        shaderIf.ViewMatrix = viewMatrix;
                        shaderIf.WorldInverseTransposeMatrix = Matrix.TransposeMatrix(Matrix.Invert(viewMatrix));
                        shaderIf.WorldViewProjectionMatrix = worldMatrix * viewMatrix * projectionMatrix;
                    }
                }
            }
        }

        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
            set
            {
                projectionMatrix = value;
                if (!matrixUpdateFrozen)
                {
                    if (!shaderEnabled)
                        gDevice.Transform.Projection = projectionMatrix;
                    else
                    {
                        shaderIf.ProjectionMatrix = projectionMatrix;
                        shaderIf.WorldViewProjectionMatrix = worldMatrix * viewMatrix * projectionMatrix;
                    }
                }
            }
        }

        public void UpdateMatrices()
        {
            if (!shaderEnabled)
            {
                gDevice.Transform.World = worldMatrix;
                gDevice.Transform.View = viewMatrix;
                gDevice.Transform.Projection = projectionMatrix;
            }
            else
            {
                shaderIf.WorldMatrix = worldMatrix;
                shaderIf.ViewMatrix = viewMatrix;
                shaderIf.ProjectionMatrix = projectionMatrix;
                shaderIf.WorldInverseTransposeMatrix = Matrix.TransposeMatrix(Matrix.Invert(viewMatrix));
                shaderIf.WorldViewProjectionMatrix = worldMatrix * viewMatrix * projectionMatrix;
            }
        }

        public bool ShaderEnabled
        {
            get { return shaderEnabled; }
            set { shaderEnabled = value; UpdateMatrices(); }
        }

        public bool MatrixUpdatesFrozen
        {
            get { return matrixUpdateFrozen; }
            set { matrixUpdateFrozen = value; }
        }

        public ShaderInterface ShaderIf
        {
            get { return shaderIf; }
            set { shaderIf = value; UpdateMatrices(); }
        }
        #endregion

        public void PushAll()
        {
            matrixStack.Push(ProjectionMatrix);
            matrixStack.Push(ViewMatrix);
            matrixStack.Push(WorldMatrix);
        }

        public void PopAll()
        {
            bool frozen = matrixUpdateFrozen;
            matrixUpdateFrozen = true;

            WorldMatrix = matrixStack.Pop();
            ViewMatrix = matrixStack.Pop();
            ProjectionMatrix = matrixStack.Pop();

            matrixUpdateFrozen = frozen;
            UpdateMatrices();
        }

        public void Push()
        {
            matrixStack.Push(worldMatrix);
        }

        public void Pop()
        {
            bool frozen = matrixUpdateFrozen;
            matrixUpdateFrozen = true;

            worldMatrix = matrixStack.Pop();

            matrixUpdateFrozen = frozen;
            UpdateWorldMatrix();
        }

        private void UpdateWorldMatrix()
        {
            if (!shaderEnabled)
            {
                gDevice.Transform.World = worldMatrix;
            }
            else
            {
                shaderIf.WorldMatrix = worldMatrix;
                shaderIf.WorldViewProjectionMatrix = worldMatrix * viewMatrix * projectionMatrix;
            }
        }

        public void BeginScene()
        {
            if (!startedScene)
            {
                gDevice.BeginScene();
                startedScene = true;
            }
        }

        public void EndScene()
        {
            if (startedScene)
            {
                gDevice.EndScene();
                startedScene = false;
            }
        }

        public void BeginSceneKeepState()
        {
            keepSceneState = startedScene;
            BeginScene();
        }

        public void EndSceneKeepState()
        {
            if (!keepSceneState)
                EndScene();
        }
    }
}