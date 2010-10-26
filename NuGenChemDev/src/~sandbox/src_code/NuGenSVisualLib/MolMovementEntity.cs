using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Chem.Structures;

namespace NuGenSVisualLib.Rendering.Entities
{
    /// <summary>
    /// Encapsulates an entity that represents a set of chem objects that are being moved
    /// </summary>
    public class MolMovementEntity : IEntity
    {
        public enum DisplayMode
        {
            Points,
            Nodes,
            Sticks,
            SceneScheme
        }

        DisplayMode displayMode;
        AtomEntity[] atoms;

        Matrix movement;
        BoundingBox bBox;

        public MolMovementEntity(DisplayMode displayMode, AtomEntity[] atoms)
        {
            this.displayMode = displayMode;
            this.atoms = atoms;

            // calc bounding box
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (AtomEntity item in atoms)
            {
                if (item.BoundingBox.Centre.X - item.ItemRadius < min.X)
                    min.X = item.BoundingBox.Centre.X - item.ItemRadius;
                if (item.BoundingBox.Centre.X + item.ItemRadius > max.X)
                    max.X = item.BoundingBox.Centre.X + item.ItemRadius;

                if (item.BoundingBox.Centre.Y - item.ItemRadius < min.Y)
                    min.Y = item.BoundingBox.Centre.Y - item.ItemRadius;
                if (item.BoundingBox.Centre.Y + item.ItemRadius > max.Y)
                    max.Y = item.BoundingBox.Centre.Y + item.ItemRadius;

                if (item.BoundingBox.Centre.Z - item.ItemRadius < min.Z)
                    min.Z = item.BoundingBox.Centre.Z - item.ItemRadius;
                if (item.BoundingBox.Centre.Z + item.ItemRadius > max.Z)
                    max.Z = item.BoundingBox.Centre.Z + item.ItemRadius;
            }
            bBox = new BoundingBox(min, max);
        }

        public void Move(Vector3 toPosition)
        {
            Vector3 diff = toPosition - Movement;

            movement.M41 = toPosition.X;
            movement.M42 = toPosition.Y;
            movement.M43 = toPosition.Z;

            // shift bounds
            bBox.Centre += diff;
        }

        public void Rotate()
        {
        }

        #region Properties

        public DisplayMode CutrrentDisplayMode
        {
            get { return displayMode; }
        }

        public Vector3 Movement
        {
            get { return new Vector3(movement.M41, movement.M42, movement.M43); }
        }
        #endregion

        #region IEntity Members

        public void Render()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Init(Device device)
        {
            if (displayMode == DisplayMode.Points)
            {
                
            }
        }

        public BoundingBox BoundingBox
        {
            get { return bBox; }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}