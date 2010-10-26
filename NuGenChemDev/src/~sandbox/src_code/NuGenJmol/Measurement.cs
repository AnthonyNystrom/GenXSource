/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2002-2005  The Jmol Development Team
*
* Contact: jmol-developers@lists.sf.net
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using javax.vecmath;

namespace Org.Jmol.Viewer
{
	class Measurement
	{
        public Frame frame;
        public int count;
        public int[] countPlusIndices;
        public string strMeasurement;

        public AxisAngle4f aa;
        public Point3f pointArc;

        public Measurement(Frame frame, int[] atomCountPlusIndices)
		{
            this.frame = frame;
			if (atomCountPlusIndices == null)
				count = 0;
			else
			{
				count = atomCountPlusIndices[0];
				this.countPlusIndices = new int[count + 1];
				Array.Copy(atomCountPlusIndices, 0, countPlusIndices, 0, count + 1);
			}
			formatMeasurement();
		}

        public virtual void formatMeasurement()
		{
			for (int i = count; --i >= 0; )
				if (countPlusIndices[i + 1] < 0)
				{
					strMeasurement = null;
					return ;
				}
			if (count < 2)
				return ;
			switch (count)
			{
				
				case 2:
                    float distance = frame.getDistance(countPlusIndices[1], countPlusIndices[2]);
                    strMeasurement = formatDistance(distance);
					break;
				
				case 3:
                    float degrees = frame.getAngle(countPlusIndices[1], countPlusIndices[2], countPlusIndices[3]);
                    strMeasurement = formatAngle(degrees);
					
                    if (degrees == 180)
                    {
                        aa = null;
                        pointArc = null;
                    }
                    else
                    {
                        Point3f pointA = getAtomPoint3f(1);
                        Point3f pointB = getAtomPoint3f(2);
                        Point3f pointC = getAtomPoint3f(3);
						
                        Vector3f vectorBA = new Vector3f();
                        Vector3f vectorBC = new Vector3f();
                        vectorBA.sub(pointA, pointB);
                        vectorBC.sub(pointC, pointB);
                        float radians = vectorBA.angle(vectorBC);
						
                        Vector3f vectorAxis = new Vector3f();
                        vectorAxis.cross(vectorBA, vectorBC);
                        aa = new AxisAngle4f(vectorAxis.x, vectorAxis.y, vectorAxis.z, radians);

                        vectorBA.normalize();
                        vectorBA.scale(0.5f);
                        pointArc = new Point3f(vectorBA);
                    }
					
					break;
				
				case 4:
                    float torsion = frame.getTorsion(countPlusIndices[1], countPlusIndices[2], countPlusIndices[3], countPlusIndices[4]);

                    strMeasurement = formatAngle(torsion);
					break;
				
				default: 
					System.Console.Out.WriteLine("Invalid count to measurement shape:" + count);
					throw new System.IndexOutOfRangeException();
				
			}
		}

        public virtual void reformatDistanceIfSelected()
		{
			if (count != 2)
				return ;
			//Viewer viewer = frame.viewer;
            //if (viewer.isSelected(countPlusIndices[1]) && viewer.isSelected(countPlusIndices[2]))
            //    formatMeasurement();
		}

        public virtual Point3f getAtomPoint3f(int i)
		{
            return frame.getAtomPoint3f(countPlusIndices[i]);
		}

        public virtual string formatDistance(float dist)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int nDist = (int) (dist * 100 + 0.5f);
            //string units = frame.viewer.getMeasureDistanceUnits();
            //if ((object) units == (object) "nanometers")
            //    return "" + (nDist / 1000.0) + " nm";
            //if ((object) units == (object) "picometers")
            //    return "" + nDist + " pm";
			return "" + (nDist / 100.0) + '\u00C5'; // angstroms
		}

        public virtual string formatAngle(float angle)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			angle = (int) (angle * 10 + (angle >= 0?0.5f:- 0.5f));
			angle /= 10;
			return "" + angle + '\u00B0';
		}

        public virtual bool sameAs(int[] atomCountPlusIndices)
		{
			if (count != atomCountPlusIndices[0])
				return false;
			if (count == 2)
				return ((atomCountPlusIndices[1] == this.countPlusIndices[1] && atomCountPlusIndices[2] == this.countPlusIndices[2]) || (atomCountPlusIndices[1] == this.countPlusIndices[2] && atomCountPlusIndices[2] == this.countPlusIndices[1]));
			if (count == 3)
				return (atomCountPlusIndices[2] == this.countPlusIndices[2] && ((atomCountPlusIndices[1] == this.countPlusIndices[1] && atomCountPlusIndices[3] == this.countPlusIndices[3]) || (atomCountPlusIndices[1] == this.countPlusIndices[3] && atomCountPlusIndices[3] == this.countPlusIndices[1])));
			return ((atomCountPlusIndices[1] == this.countPlusIndices[1] && atomCountPlusIndices[2] == this.countPlusIndices[2] && atomCountPlusIndices[3] == this.countPlusIndices[3] && atomCountPlusIndices[4] == this.countPlusIndices[4]) || (atomCountPlusIndices[1] == this.countPlusIndices[4] && atomCountPlusIndices[2] == this.countPlusIndices[3] && atomCountPlusIndices[3] == this.countPlusIndices[2] && atomCountPlusIndices[4] == this.countPlusIndices[1]));
		}

        public static float computeTorsion(Point3f p1, Point3f p2, Point3f p3, Point3f p4)
		{
			
			float ijx = p1.x - p2.x;
			float ijy = p1.y - p2.y;
			float ijz = p1.z - p2.z;
			
			float kjx = p3.x - p2.x;
			float kjy = p3.y - p2.y;
			float kjz = p3.z - p2.z;
			
			float klx = p3.x - p4.x;
			float kly = p3.y - p4.y;
			float klz = p3.z - p4.z;
			
			float ax = ijy * kjz - ijz * kjy;
			float ay = ijz * kjx - ijx * kjz;
			float az = ijx * kjy - ijy * kjx;
			float cx = kjy * klz - kjz * kly;
			float cy = kjz * klx - kjx * klz;
			float cz = kjx * kly - kjy * klx;
			
			float ai2 = 1f / (ax * ax + ay * ay + az * az);
			float ci2 = 1f / (cx * cx + cy * cy + cz * cz);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float ai = (float) System.Math.Sqrt(ai2);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float ci = (float) System.Math.Sqrt(ci2);
			float denom = ai * ci;
			float cross = ax * cx + ay * cy + az * cz;
			float cosang = cross * denom;
			if (cosang > 1)
			{
				cosang = 1;
			}
			if (cosang < - 1)
			{
				cosang = - 1;
			}
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float torsion = toDegrees((float) System.Math.Acos(cosang));
			float dot = ijx * cx + ijy * cy + ijz * cz;
			float absDot = System.Math.Abs(dot);
			torsion = (dot / absDot > 0)?torsion:- torsion;
			return torsion;
		}

        public static float toDegrees(float angrad)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return angrad * 180 / (float) System.Math.PI;
		}
	}
}