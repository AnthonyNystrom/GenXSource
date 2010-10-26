using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Shaders
{
    public class ShaderInterface
    {
        ShaderHLSL shader;

        EffectHandle worldViewProjection, worldInverseTranspose, world, view, proj;

        /// <summary>
        /// Initializes a new instance of the ShaderInterface class.
        /// </summary>
        /// <param name="shader"></param>
        public ShaderInterface(ShaderHLSL shader)
        {
            this.shader = shader;

            BuildMappings();
        }

        private void BuildMappings()
        {
            worldViewProjection = GetParameter("WorldViewProjection");
            worldInverseTranspose = GetParameter("WorldInverseTranspose");
            world = GetParameter("World");
            view = GetParameter("View");
            proj = GetParameter("Proj");
        }

        private EffectHandle GetParameter(string semantic)
        {
            return shader.Effect.GetParameterBySemantic(null, semantic);
        }

        public Matrix WorldMatrix
        {
            set
            {
                if (world != null)
                {
                    shader.Effect.SetValue(world, value);
                    shader.Effect.CommitChanges();
                }
            }
        }

        public Matrix ViewMatrix
        {
            set
            {
                if (view != null)
                {
                    shader.Effect.SetValue(view, value);
                    shader.Effect.CommitChanges();
                }
            }
        }

        public Matrix ProjectionMatrix
        {
            set
            {
            	if (proj != null)
                {
                    shader.Effect.SetValue(proj, value);
                    shader.Effect.CommitChanges();
                }
            }
        }

        public Matrix WorldInverseTransposeMatrix
        {
            set
            {
                if (worldInverseTranspose != null)
                {
                    shader.Effect.SetValue(worldInverseTranspose, value);
                    shader.Effect.CommitChanges();
                }
            }
        }

        public Matrix WorldViewProjectionMatrix
        {
            set
            {
                if (worldViewProjection != null)
                {
                    shader.Effect.SetValue(worldViewProjection, value);
                    shader.Effect.CommitChanges();
                }
            }
        }

        public Effect Effect
        {
            get { return shader.Effect; }
        }
    }
}