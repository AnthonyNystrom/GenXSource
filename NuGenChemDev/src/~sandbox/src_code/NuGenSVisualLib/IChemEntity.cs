using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NuGenJmol;
using NuGenSVisualLib.Settings;
using OcTree;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenSVisualLib.Rendering.Chem.Structures
{
    public interface IChemEntity : IEntity
    {
        IChemObject CdkObject { get; }
    }

    public abstract class ViewSpaceEntity : IEntity
    {
        protected Device device;
        protected BoundingBox bBox;

        public abstract void UpdateView(Matrix world, Matrix view);

        #region IEntity Members
        public abstract void Render();
        public abstract void Init(Device device);
        public abstract BoundingBox BoundingBox
        {
            get;
        }
        #endregion
        #region IDisposable Members
        public abstract void Dispose();
        #endregion
    }

    public abstract class ChemEntity : OcTreeItem, IChemEntity
    {
        protected BoundingBox bBox;

        public ChemEntity(Vector3 pos, Vector3 dim, float radius)
            : base(pos, dim, radius)
        {
            bBox = new BoundingBox();
            bBox.Centre = pos;
            bBox.Dimensions = dim * 0.5f;
            bBox.Position = pos - bBox.Dimensions;
            bBox.Extent = dim;
        }

        public abstract IChemObject CdkObject
        {
            get;
        }

        #region IEntity Members

        public void Render() { }

        public void Init(Device device) { }

        public BoundingBox BoundingBox
        {
            get { return bBox; }
        }

        #endregion
    }

    /// <summary>
    /// Encapsulates a CDK atom, providing all local frequently used info.
    /// </summary>
    public class AtomEntity : ChemEntity
    {
        protected IAtom atom;
        AtomSelectionEntity selection;

//        public AtomEntity(IAtom atom)
//            : base(new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d), new Vector3(), 0)
//        {
//            this.atom = atom;
//        }

        public AtomEntity(IAtom atom, Vector3 dimensions, float radius)
            : base(new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d), dimensions, radius)
        {
            this.atom = atom;
        }

        #region Properties

        public IAtom Atom
        {
            get { return atom; }
            set { atom = value; }
        }

        public override IChemObject CdkObject
        {
            get { return atom; }
        }

        public AtomSelectionEntity SelectionEntity
        {
            get { return selection; }
            set { selection = value; }
        }
        #endregion

        public static AtomEntity BuildEntity(IAtom atom)
        {
            // calc dimensions based on standard for now
            int period = 1;
            if (atom.Properties.ContainsKey("Period"))
                period = (int)atom.Properties["Period"];
            float radius = period * 0.2f;
            Vector3 dim = new Vector3(radius, radius, radius) * 2;
            return new AtomEntity(atom, dim, radius);
        }
    }

    public class AtomSelectionEntity : ViewSpaceEntity
    {
        AtomEntity atom;
        CustomVertex.PositionTextured[] texQuad;
        Texture tex;

        public AtomSelectionEntity(AtomEntity atom)
        {
            this.atom = atom;
            atom.SelectionEntity = this;
            bBox = new BoundingBox((Vector3)atom.Position3D, (Vector3)atom.Position3D);
        }

        public override void UpdateView(Matrix world, Matrix view)
        {
            // transform to world space
            float radius = atom.ItemRadius * 4;
            Matrix final = world * view;
            Vector3 rightVect = Vector3.Normalize(new Vector3(final.M11, final.M21, final.M31)) * radius * 0.5f;
            Vector3 upVect = Vector3.Normalize(new Vector3(final.M12, final.M22, final.M32)) * radius * 0.5f;
            Vector3 pos = (Vector3)atom.Position3D;// -(rightVect * 0.5f) - (upVect * 0.5f);

            texQuad[0].Position = pos - rightVect;
            texQuad[1].Position = pos + upVect;
            texQuad[2].Position = pos - upVect;
            texQuad[3].Position = pos + rightVect;
        }

        public override void Render()
        {
            device.TextureState[0].ColorOperation = TextureOperation.Modulate;
            device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
            device.TextureState[0].ColorArgument0 = TextureArgument.TextureColor;
            device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;

            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.SourceBlend = Blend.SourceAlpha;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            device.BeginScene();
            device.SetTexture(0, tex);
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, texQuad);
            device.EndScene();

            device.RenderState.AlphaBlendEnable = false;
        }

        public override void Init(Device device)
        {
            this.device = device;

            texQuad = new CustomVertex.PositionTextured[4];
            texQuad[0] = new CustomVertex.PositionTextured(new Vector3(), 0, 0);
            texQuad[1] = new CustomVertex.PositionTextured(new Vector3(), 1, 0);
            texQuad[2] = new CustomVertex.PositionTextured(new Vector3(), 0, 1);
            texQuad[3] = new CustomVertex.PositionTextured(new Vector3(), 1, 1);

            tex = TextureLoader.FromFile(device, (string)HashTableSettings.Instance["Base.Path"] + @"Media/AtomSelection.png");
        }

        public override BoundingBox BoundingBox
        {
            get { return bBox; }
        }

        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public AtomEntity Atom
        {
            get { return atom; }
        }
    }

    class AtomSymbolEntity : ViewSpaceEntity
    {
        AtomEntity atom;
        Texture texture;
        CustomVertex.PositionTextured[] texQuad;

        public AtomSymbolEntity(AtomEntity atom)
        {
            this.atom = atom;
            bBox = new BoundingBox((Vector3)atom.Position3D, (Vector3)atom.Position3D);
        }

        public override void UpdateView(Matrix world, Matrix view)
        {
            // transform to world space
            float radius = atom.ItemRadius * 2;
            Matrix final = world * view;
            Vector3 rightVect = Vector3.Normalize(new Vector3(final.M11, final.M21, final.M31)) * radius * 0.5f;
            Vector3 upVect = Vector3.Normalize(new Vector3(final.M12, final.M22, final.M32)) * radius * 0.5f;
            Vector3 zVect = Vector3.Normalize(Vector3.Cross(rightVect, upVect)) * -radius;
            Vector3 pos = (Vector3)atom.Position3D;

            texQuad[0].Position = pos - rightVect + upVect + zVect;
            texQuad[1].Position = pos + rightVect + upVect + zVect;
            texQuad[2].Position = pos - rightVect - upVect + zVect;
            texQuad[3].Position = pos + rightVect - upVect + zVect;
        }

        public override void Render()
        {
            device.TextureState[0].ColorOperation = TextureOperation.Modulate;
            device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
            device.TextureState[0].ColorArgument0 = TextureArgument.TextureColor;
            device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;

            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.SourceBlend = Blend.SourceAlpha;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            device.BeginScene();
            device.SetTexture(0, texture);
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, texQuad);
            device.EndScene();
            device.SetTexture(0, null);

            device.RenderState.AlphaBlendEnable = false;
        }

        public override void Init(Device device)
        {
            this.device = device;

            texQuad = new CustomVertex.PositionTextured[4];
            texQuad[0] = new CustomVertex.PositionTextured(new Vector3(), 0, 0);
            texQuad[1] = new CustomVertex.PositionTextured(new Vector3(), 1, 0);
            texQuad[2] = new CustomVertex.PositionTextured(new Vector3(), 0, 1);
            texQuad[3] = new CustomVertex.PositionTextured(new Vector3(), 1, 1);

            texture = ChemSymbolTextures.Instance[atom.Atom.Symbol];
        }

        public override BoundingBox BoundingBox
        {
            get { return bBox; }
        }

        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public abstract class ChemEntityStructure : IChemEntity
    {
        protected BoundingBox boundingBox;
        protected NuSceneBuffer3D sbLocalCopy;

        public ChemEntityStructure(NuSceneBuffer3D sceneBuffer) { sbLocalCopy = sceneBuffer; }

        #region IEntity Members

        public abstract void Render();
        public abstract void Init(Device device);
        public abstract void Init(Device device, GeneralStructuresShadingDesc shading);
        
        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        #endregion

        public static BoundingBox GenerateBoundingBoxFromPoints(List<Vector3[]> pointsList)
        {
            // find extremes
            float minX = pointsList[0][0].X;
            float minY = pointsList[0][0].Y;
            float minZ = pointsList[0][0].Z;
            float maxX = pointsList[0][0].X;
            float maxY = pointsList[0][0].Y;
            float maxZ = pointsList[0][0].Z;

            foreach (Vector3[] points in pointsList)
            {
                foreach (Vector3 point in points)
                {
                    if (point.X < minX)
                        minX = point.X;
                    else if (point.X > maxX)
                        maxX = point.X;
                    if (point.Y < minY)
                        minY = point.Y;
                    else if (point.Y > maxY)
                        maxY = point.Y;
                    if (point.Z < minZ)
                        minZ = point.Z;
                    else if (point.Z > maxZ)
                        maxZ = point.Z;
                }
            }

            return new BoundingBox(new float[] { minX, maxX, minY, maxY, minZ, maxZ });
        }

        public static void FlipNormalsVB(VertexBuffer vb, VertexFormats vFormat, int size)
        {
            if (vFormat == CustomVertex.PositionNormal.Format)
            {
                CustomVertex.PositionNormal[] verts = (CustomVertex.PositionNormal[])
                                                      vb.Lock(0, LockFlags.None);
                for (int v = 0; v < verts.Length; v++)
                {
                    verts[v].Normal = new Vector3(-verts[v].Normal.X, -verts[v].Normal.Y, -verts[v].Normal.Z);
                }
                vb.Unlock();
            }
            else if (vFormat == CustomVertex.PositionNormalColored.Format)
            {
                CustomVertex.PositionNormalColored[] verts = (CustomVertex.PositionNormalColored[])
                                                             vb.Lock(0, LockFlags.None);
                for (int v = 0; v < verts.Length; v++)
                {
                    verts[v].Normal = new Vector3(-verts[v].Normal.X, -verts[v].Normal.Y, -verts[v].Normal.Z);
                }
                vb.Unlock();
            }
            else if (vFormat == CustomVertex.PositionNormalTextured.Format)
            {
                CustomVertex.PositionNormalTextured[] verts = (CustomVertex.PositionNormalTextured[])
                                                              vb.Lock(0, LockFlags.None);
                for (int v = 0; v < verts.Length; v++)
                {
                    verts[v].Normal = new Vector3(-verts[v].Normal.X, -verts[v].Normal.Y, -verts[v].Normal.Z);
                }
                vb.Unlock();
            }
            else
                throw new Exception("Unsupported format to flip normals with");
        }

        #region IDisposable Members

        public abstract void Dispose();

        #endregion

        #region IChemEntity Members

        public IChemObject CdkObject
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}