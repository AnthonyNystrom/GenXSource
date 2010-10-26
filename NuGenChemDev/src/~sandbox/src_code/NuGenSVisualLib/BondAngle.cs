using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Geometry;


namespace NuGenSVisualLib.Rendering.ThreeD
{
    public class BondAngle : IScreenSpaceEntity
    {
        Device device;
        IAtom atom1, atom2, atom3;
        double angle;
        string angleText;

        Line line;
        Vector2[] points;
        Microsoft.DirectX.Direct3D.Font font;
        Point textPos;
        Vector3[] arcWorldPoints;
        Vector2[] arcPoints;

        public BondAngle(Device device, IAtom atom1, IAtom atom2, IAtom atom3)
        {
            this.device = device;
            this.atom1 = atom1;
            this.atom2 = atom2;
            this.atom3 = atom3;
        }

        #region IScreenSpaceEntity Members

        public void Update(Matrix world, Matrix view, Matrix proj)
        {
            // unproject atoms to 2D screen space
            Vector3 pos = Vector3.Project(new Vector3((float)atom1.X3d, (float)atom1.Y3d, (float)atom1.Z3d),
                                          device.Viewport, proj, view, world);
            points[0] = new Vector2(pos.X, pos.Y);
            pos = Vector3.Project(new Vector3((float)atom2.X3d, (float)atom2.Y3d, (float)atom2.Z3d),
                                  device.Viewport, proj, view, world);
            points[1] = new Vector2(pos.X, pos.Y);
            pos = Vector3.Project(new Vector3((float)atom3.X3d, (float)atom3.Y3d, (float)atom3.Z3d),
                                  device.Viewport, proj, view, world);
            points[2] = new Vector2(pos.X, pos.Y);

            textPos = new Point((int)points[1].X, (int)points[1].Y);

            // update arc positions
            for (int i = 0; i < arcWorldPoints.Length; i++)
            {
                pos = Vector3.Project(arcWorldPoints[i], device.Viewport, proj, view, world);
                arcPoints[i] = new Vector2(pos.X, pos.Y);
            }
        }

        public void Render()
        {
            // draw angle lines
            line.Begin();
            line.Draw(points, Color.Yellow);
            line.Draw(arcPoints, Color.Yellow);
            line.End();

            // draw angle arc?

            // draw angle text
            font.DrawText(null, angleText, textPos, Color.Yellow);
        }

        public void Init(Device device)
        {
            // create angle lines
            line = new Line(device);
            line.Antialias = true;
            line.Width = 1;
            //line.Pattern = (int)0xFFF0;
            //line.PatternScale = 1.0f;

            // create text sprite
            font = new Microsoft.DirectX.Direct3D.Font(device, 12, 0, FontWeight.Bold, 1, false,
                                                       CharacterSet.Ansi, Precision.Default, FontQuality.ClearType,
                                                       PitchAndFamily.DefaultPitch, "Tahoma");

            points = new Vector2[3];

            // calc angle to -/+ degrees
            angle = 90;// (180 / Math.PI) * BondTools.giveAngleFromMiddle(atom2, atom1, atom3);
            // round for text
            angleText = Math.Round(angle, 1).ToString() + (char)176;

            // calc world arc points
            arcWorldPoints = new Vector3[3];
            arcPoints = new Vector2[3];
            Vector3 a1 = new Vector3((float)atom1.X3d, (float)atom1.Y3d, (float)atom1.Z3d);
            Vector3 a2 = new Vector3((float)atom2.X3d, (float)atom2.Y3d, (float)atom2.Z3d);
            Vector3 a3 = new Vector3((float)atom3.X3d, (float)atom3.Y3d, (float)atom3.Z3d);
            
            Vector3 arm1 = a2 - a1;
            float arm1Len = arm1.Length();
            arm1.Normalize();
            Vector3 arcStartPos = arm1 * (arm1Len * 0.3f);

            Vector3 arm2 = a2 - a3;
            arm2.Normalize();
            Vector3 arcEndPos = arm2 * (arm1Len * 0.3f);

            arcWorldPoints[0] = a2 - arcStartPos;
            arcWorldPoints[2] = a2 - arcEndPos;

            Vector3 arm3 = ((arcWorldPoints[0] - arcWorldPoints[2]) * 0.5f) + arcWorldPoints[2];
            arm3 = a2 - arm3;
            arm3.Normalize();
            arcWorldPoints[1] = a2 - (arm3 * (arm1Len * 0.35f));
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (line != null)
                line.Dispose();
            points = null;
        }
        #endregion
    }
}