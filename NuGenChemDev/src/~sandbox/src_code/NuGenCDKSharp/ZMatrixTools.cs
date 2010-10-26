/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
* 
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the beginning
* of your source code files, and to any copyright notice that you may distribute
* with programs based on this work.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*  */
using System;
using javax.vecmath;

namespace Org.OpenScience.CDK.Geometry
{
    /// <summary> A set of static utility classes for dealing with Z matrices.
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    /// <cdk.keyword>     Z Matrix </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <cdk.created>  2004-02-09 </cdk.created>
    public class ZMatrixTools
    {

        /// <summary> Takes the given Z Matrix coordinates and converts them to cartesian coordinates.
        /// The first Atom end up in the origin, the second on on the x axis, and the third
        /// one in the XY plane. The rest is added by applying the Zmatrix distances, angles
        /// and dihedrals.
        /// 
        /// </summary>
        /// <param name="distances">    Array of distance variables of the Z matrix
        /// </param>
        /// <param name="angles">       Array of angle variables of the Z matrix
        /// </param>
        /// <param name="dihedrals">    Array of distance variables of the Z matrix
        /// </param>
        /// <param name="first_atoms">  Array of atom ids of the first involed atom in distance, angle and dihedral
        /// </param>
        /// <param name="second_atoms"> Array of atom ids of the second involed atom in angle and dihedral
        /// </param>
        /// <param name="third_atoms">  Array of atom ids of the third involed atom in dihedral
        /// 
        /// </param>
        /// <cdk.dictref>  blue-obelisk:zmatrixCoordinatesIntoCartesianCoordinates </cdk.dictref>
        public static Point3d[] zmatrixToCartesian(double[] distances, int[] first_atoms, double[] angles, int[] second_atoms, double[] dihedrals, int[] third_atoms)
        {
            Point3d[] cartesianCoords = new Point3d[distances.Length];
            for (int index = 0; index < distances.Length; index++)
            {
                if (index == 0)
                {
                    cartesianCoords[index] = new Point3d(0d, 0d, 0d);
                }
                else if (index == 1)
                {
                    cartesianCoords[index] = new Point3d(distances[1], 0d, 0d);
                }
                else if (index == 2)
                {
                    cartesianCoords[index] = new Point3d((-System.Math.Cos((angles[2] / 180) * System.Math.PI)) * distances[2] + distances[1], System.Math.Sin((angles[2] / 180) * System.Math.PI) * distances[2], 0d);
                }
                else
                {
                    Vector3d cd = new Vector3d();
                    cd.sub(cartesianCoords[third_atoms[index]], cartesianCoords[second_atoms[index]]);

                    Vector3d bc = new Vector3d();
                    bc.sub(cartesianCoords[second_atoms[index]], cartesianCoords[first_atoms[index]]);

                    Vector3d n1 = new Vector3d();
                    n1.cross(cd, bc);
                    n1.normalize();

                    Vector3d n2 = rotate(n1, bc, dihedrals[index]);
                    n2.normalize();
                    Vector3d ba = rotate(bc, n2, -angles[index]);

                    ba.normalize();

                    Vector3d ban = new Vector3d(ba);
                    ban.scale(distances[index]);

                    Point3d result = new Point3d();
                    result.add(cartesianCoords[first_atoms[index]], ba);
                    cartesianCoords[index] = result;
                }
            }
            return cartesianCoords;
        }

        private static Vector3d rotate(Vector3d vector, Vector3d axis, double angle)
        {
            Matrix3d rotate = new Matrix3d();
            rotate.set_Renamed(new AxisAngle4d(axis.x, axis.y, axis.z, (angle / 180) * System.Math.PI));
            Vector3d result = new Vector3d();
            rotate.transform(vector, result);
            return result;
        }
    }
}