using System.Drawing;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Maths.Volumes
{
    /// <summary>
    /// Encapsulates a scene of volumetric data
    /// </summary>
    public interface IVolumeScene
    {
        float GetPotentialAtPoint(Vector3 point);
        bool IsOutside(Vector3 point);
        float EstimateVolumeMaxSize();
        Color ColourizePoint(Vector3 point);

        IVolume[] Volumes { get; }
    }

    /// <summary>
    /// Encapsulates just a regular group of volumes
    /// </summary>
    public class GenericVolumeScene : IVolumeScene
    {
        IVolume[] volumes;

        public GenericVolumeScene(IVolume[] volumes)
        {
            this.volumes = volumes;
        }

        #region IVolumeScene Members

        public float GetPotentialAtPoint(Vector3 point)
        {
            float potential = 0;
            foreach (IVolume volume in volumes)
            {
                potential += volume.GetPotentialAtPoint(point);
            }
            return potential;
        }

        public IVolume[] Volumes
        {
            get { return volumes; }
        }

        public bool IsOutside(Vector3 point)
        {
            return (GetPotentialAtPoint(point) < 0.1f);
        }

        public float EstimateVolumeMaxSize()
        {
            float max = volumes[0].Origin.X;
            float min = volumes[0].Origin.Y;
            foreach (IVolume volume in volumes)
            {
                float size = volume.GetBoundingSphere();
                Vector3 pos = volume.Origin;
                if (pos.X + size > max)
                    max = pos.X + size;
                if (pos.X - size < min)
                    min = pos.X - size;

                if (pos.Y + size > max)
                    max = pos.Y + size;
                if (pos.Y - size < min)
                    min = pos.Y - size;

                if (pos.Z + size > max)
                    max = pos.Z + size;
                if (pos.Z - size < min)
                    min = pos.Z - size;
            }
            if (max < 0 && min < 0)
            {
                float temp = max;
                max = -min;
                min = -temp;
            }
            else
            {
                if (max < 0)
                    max = -max;
                if (min < 0)
                    min = -min;
            }
            float largest = max;
            if (max < min)
                largest = min;

            // add 5%
            float fivepercent = ((largest * 2f) / 100f) * 20f;
            return (largest * 2f) + fivepercent;
        }

        public Color ColourizePoint(Vector3 point)
        {
            /*int R=0, G=0, B=0, A=0;
            float numClrs = 0;
            foreach (IVolume volume in volumes)
            {
                float pot = volume.GetPotentialAtPoint(point);
                if (pot >= 0.01f)
                {
                    if (pot > 0.02f)
                        pot = 0.02f;
                    float rPot = pot * 50f;

                    R += (int)(volume.Colour.R * rPot);
                    G += (int)(volume.Colour.G * rPot);
                    B += (int)(volume.Colour.B * rPot);
                    A += (int)(volume.Colour.A * rPot);

                    numClrs += rPot;
                }
            }
            if (R > 0)
                R = (int)(R / numClrs);
            if (G > 0)
                G = (int)(G / numClrs);
            if (B > 0)
                B = (int)(B / numClrs);
            if (A > 0)
                A = (int)(A / numClrs);

            if (R > 255)
                R = 255;
            if (G > 255)
                G = 255;
            if (B > 255)
                B = 255;
            if (A > 255)
                A = 255;
            return Color.FromArgb(A, R, G, B);*/

            // find nearest point for direct 1-way colour
            float nVolumeDist = float.MaxValue;
            IVolume nVolume = null;
            foreach (IVolume volume in volumes)
            {
                float dist = (point - volume.Origin).Length();
                if (dist < nVolumeDist)
                {
                    nVolumeDist = dist;
                    nVolume = volume;
                }
            }
            return nVolume.Colour;
        }

        #endregion
    }
}