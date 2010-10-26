/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 11:39:20 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6669 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using javax.vecmath;
using Org.OpenScience.CDK.Tools.Manipulator;
using Org.OpenScience.CDK.Exception;
using Support;
using Org.OpenScience.CDK.Isomorphism.MCSS;
using Org.OpenScience.CDK.Isomorphism;

namespace Org.OpenScience.CDK.Geometry
{
    /// <summary> A set of static utility classes for geometric calculations and operations.
    /// This class is extensively used, for example, by JChemPaint to edit molecule.
    /// 
    /// </summary>
    /// <author>         seb
    /// </author>
    /// <author>         Stefan Kuhn
    /// </author>
    /// <author>         Egon Willighagen
    /// </author>
    /// <author>         Ludovic Petain
    /// </author>
    /// <author>         Christian Hoppe
    /// 
    /// </author>
    /// <cdk.module>     standard </cdk.module>
    public class GeometryTools
    {

        //UPGRADE_NOTE: The initialization of  '//logger' was moved to static method 'org.openscience.cdk.geometry.GeometryTools'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        //private static LoggingTool //logger;


        /// <summary>  Adds an automatically calculated offset to the coordinates of all atoms
        /// such that all coordinates are positive and the smallest x or y coordinate
        /// is exactly zero, using an external set of coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> AtomContainer for which all the atoms are translated to
        /// positive coordinates
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static void translateAllPositive(IAtomContainer atomCon, System.Collections.Hashtable renderingCoordinates)
        {
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minX = System.Double.MaxValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minY = System.Double.MaxValue;
            IAtom[] atoms = atomCon.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[atoms[i]] == null && atoms[i].getPoint2d() != null)
                {
                    renderingCoordinates[atoms[i]] = new Point2d(atoms[i].getPoint2d().x, atoms[i].getPoint2d().y);
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[atoms[i]] != null)
                {
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    if (((Point2d)renderingCoordinates[atoms[i]]).x < minX)
                    {
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        minX = ((Point2d)renderingCoordinates[atoms[i]]).x;
                    }
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    if (((Point2d)renderingCoordinates[atoms[i]]).y < minY)
                    {
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        minY = ((Point2d)renderingCoordinates[atoms[i]]).y;
                    }
                }
            }
            //logger.debug("Translating: minx=" + minX + ", minY=" + minY);
            translate2D(atomCon, minX * (-1), minY * (-1), renderingCoordinates);
        }


        /// <summary>  Adds an automatically calculated offset to the coordinates of all atoms
        /// such that all coordinates are positive and the smallest x or y coordinate
        /// is exactly zero.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> AtomContainer for which all the atoms are translated to
        /// positive coordinates
        /// </param>
        public static void translateAllPositive(IAtomContainer atomCon)
        {
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minX = System.Double.MaxValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minY = System.Double.MaxValue;
            IAtom[] atoms = atomCon.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint2d() != null)
                {
                    if (atoms[i].getPoint2d().x < minX)
                    {
                        minX = atoms[i].getPoint2d().x;
                    }
                    if (atoms[i].getPoint2d().y < minY)
                    {
                        minY = atoms[i].getPoint2d().y;
                    }
                }
            }
            //logger.debug("Translating: minx=" + minX + ", minY=" + minY);
            translate2D(atomCon, minX * (-1), minY * (-1));
        }


        /// <summary>  Translates the given molecule by the given Vector.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> The molecule to be translated
        /// </param>
        /// <param name="transX">  translation in x direction
        /// </param>
        /// <param name="transY">  translation in y direction
        /// </param>
        public static void translate2D(IAtomContainer atomCon, double transX, double transY)
        {
            translate2D(atomCon, new Vector2d(transX, transY));
        }


        /// <summary>  Scales a molecule such that it fills a given percentage of a given
        /// dimension
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon">    The molecule to be scaled
        /// </param>
        /// <param name="areaDim">    The dimension to be filled
        /// </param>
        /// <param name="fillFactor"> The percentage of the dimension to be filled
        /// </param>
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
        public static void scaleMolecule(IAtomContainer atomCon, ref System.Drawing.Size areaDim, double fillFactor)
        {
            System.Drawing.Size molDim = get2DDimension(atomCon);
            double widthFactor = (double)areaDim.Width / (double)molDim.Width;
            double heightFactor = (double)areaDim.Height / (double)molDim.Height;
            double scaleFactor = System.Math.Min(widthFactor, heightFactor) * fillFactor;
            scaleMolecule(atomCon, scaleFactor);
        }


        /// <summary>  Multiplies all the coordinates of the atoms of the given molecule with the
        /// scalefactor.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon">     The molecule to be scaled
        /// </param>
        /// <param name="scaleFactor"> Description of the Parameter
        /// </param>
        public static void scaleMolecule(IAtomContainer atomCon, double scaleFactor)
        {
            for (int i = 0; i < atomCon.AtomCount; i++)
            {
                if (atomCon.getAtomAt(i).getPoint2d() != null)
                {
                    atomCon.getAtomAt(i).getPoint2d().x *= scaleFactor;
                    atomCon.getAtomAt(i).getPoint2d().y *= scaleFactor;
                }
            }
        }


        /// <summary>  Centers the molecule in the given area
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> molecule to be centered
        /// </param>
        /// <param name="areaDim"> dimension in which the molecule is to be centered
        /// </param>
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
        public static void center(IAtomContainer atomCon, ref System.Drawing.Size areaDim)
        {
            System.Drawing.Size molDim = get2DDimension(atomCon);
            int transX = (int)((areaDim.Width - molDim.Width) / 2);
            int transY = (int)((areaDim.Height - molDim.Height) / 2);
            translateAllPositive(atomCon);
            translate2D(atomCon, new Vector2d(transX, transY));
        }


        /// <summary>  Translates a molecule from the origin to a new point denoted by a vector.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> molecule to be translated
        /// </param>
        /// <param name="vector">  dimension that represents the translation vector
        /// </param>
        public static void translate2D(IAtomContainer atomCon, Vector2d vector)
        {
            IAtom[] atoms = atomCon.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint2d() != null)
                {
                    atoms[i].getPoint2d().add(vector);
                }
                else
                {
                    //logger.warn("Could not translate atom in 2D space");
                }
            }
        }


        /// <summary>  Translates the given molecule by the given Vector, using an external set of coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> The molecule to be translated
        /// </param>
        /// <param name="transX">  translation in x direction
        /// </param>
        /// <param name="transY">  translation in y direction
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static void translate2D(IAtomContainer atomCon, double transX, double transY, System.Collections.Hashtable renderingCoordinates)
        {
            translate2D(atomCon, new Vector2d(transX, transY), renderingCoordinates);
        }


        /// <summary>  Multiplies all the coordinates of the atoms of the given molecule with the
        /// scalefactor, using an external set of coordinates..
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon">     The molecule to be scaled
        /// </param>
        /// <param name="scaleFactor"> Description of the Parameter
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static void scaleMolecule(IAtomContainer atomCon, double scaleFactor, System.Collections.Hashtable renderingCoordinates)
        {
            for (int i = 0; i < atomCon.AtomCount; i++)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[atomCon.getAtomAt(i)] != null)
                {
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    ((Point2d)renderingCoordinates[atomCon.getAtomAt(i)]).x *= scaleFactor;
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    ((Point2d)renderingCoordinates[atomCon.getAtomAt(i)]).y *= scaleFactor;
                }
            }
        }


        /// <summary>  Rotates a molecule around a given center by a given angle
        /// 
        /// </summary>
        /// <param name="atomCon"> The molecule to be rotated
        /// </param>
        /// <param name="center">  A point giving the rotation center
        /// </param>
        /// <param name="angle">   The angle by which to rotate the molecule
        /// </param>
        public static void rotate(IAtomContainer atomCon, Point2d center, double angle)
        {
            Point2d p = null;
            double distance;
            double offsetAngle;
            IAtom atom = null;
            for (int i = 0; i < atomCon.AtomCount; i++)
            {
                atom = atomCon.getAtomAt(i);
                p = atom.getPoint2d();
                offsetAngle = GeometryTools.getAngle(p.x - center.x, p.y - center.y);
                distance = p.distance(center);
                p.x = center.x + (System.Math.Sin(angle + offsetAngle) * distance);
                p.y = center.y - (System.Math.Cos(angle + offsetAngle) * distance);
            }
        }

        /// <summary> Rotates a 3D point about a specified line segment by a specified angle.
        /// 
        /// The code is based on code available <a href="http://astronomy.swin.edu.au/~pbourke/geometry/rotate/source.c">here</a>.
        /// Positive angles are anticlockwise looking down the axis towards the origin.
        /// Assume right hand coordinate system.
        /// 
        /// </summary>
        /// <param name="atom">The atom to rotate
        /// </param>
        /// <param name="p1"> The  first point of the line segment
        /// </param>
        /// <param name="p2"> The second point of the line segment
        /// </param>
        /// <param name="angle"> The angle to rotate by (in degrees)
        /// </param>
        public static void rotate(IAtom atom, Point3d p1, Point3d p2, double angle)
        {
            double costheta, sintheta;

            Point3d r = new Point3d();

            r.x = p2.x - p1.x;
            r.y = p2.y - p1.y;
            r.z = p2.z - p1.z;
            normalize(r);


            angle = angle * System.Math.PI / 180.0;
            costheta = System.Math.Cos(angle);
            sintheta = System.Math.Sin(angle);

            Point3d p = atom.getPoint3d();
            p.x -= p1.x;
            p.y -= p1.y;
            p.z -= p1.z;

            Point3d q = new Point3d(0, 0, 0);
            q.x += (costheta + (1 - costheta) * r.x * r.x) * p.x;
            q.x += ((1 - costheta) * r.x * r.y - r.z * sintheta) * p.y;
            q.x += ((1 - costheta) * r.x * r.z + r.y * sintheta) * p.z;

            q.y += ((1 - costheta) * r.x * r.y + r.z * sintheta) * p.x;
            q.y += (costheta + (1 - costheta) * r.y * r.y) * p.y;
            q.y += ((1 - costheta) * r.y * r.z - r.x * sintheta) * p.z;

            q.z += ((1 - costheta) * r.x * r.z - r.y * sintheta) * p.x;
            q.z += ((1 - costheta) * r.y * r.z + r.x * sintheta) * p.y;
            q.z += (costheta + (1 - costheta) * r.z * r.z) * p.z;

            q.x += p1.x;
            q.y += p1.y;
            q.z += p1.z;

            atom.setPoint3d(q);
        }

        /// <summary> Normalizes a point.
        /// 
        /// </summary>
        /// <param name="point">The point to normalize
        /// </param>
        public static void normalize(Point3d point)
        {
            double sum = System.Math.Sqrt(point.x * point.x + point.y * point.y + point.z * point.z);
            point.x = point.x / sum;
            point.y = point.y / sum;
            point.z = point.z / sum;
        }


        /// <summary>  Scales a molecule such that it fills a given percentage of a given
        /// dimension, using an external set of coordinates
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon">    The molecule to be scaled
        /// </param>
        /// <param name="areaDim">    The dimension to be filled
        /// </param>
        /// <param name="fillFactor"> The percentage of the dimension to be filled
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
        public static void scaleMolecule(IAtomContainer atomCon, ref System.Drawing.Size areaDim, double fillFactor, System.Collections.Hashtable renderingCoordinates)
        {
            System.Drawing.Size molDim = get2DDimension(atomCon, renderingCoordinates);
            double widthFactor = (double)areaDim.Width / (double)molDim.Width;
            double heightFactor = (double)areaDim.Height / (double)molDim.Height;
            double scaleFactor = System.Math.Min(widthFactor, heightFactor) * fillFactor;
            scaleMolecule(atomCon, scaleFactor, renderingCoordinates);
        }


        /// <summary>  Returns the java.awt.Dimension of a molecule
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> of which the dimension should be returned
        /// </param>
        /// <returns>          The java.awt.Dimension of this molecule
        /// </returns>
        public static System.Drawing.Size get2DDimension(IAtomContainer atomCon)
        {
            double[] minmax = getMinMax(atomCon);
            double maxX = minmax[2];
            double maxY = minmax[3];
            double minX = minmax[0];
            double minY = minmax[1];
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            return new System.Drawing.Size((int)(maxX - minX + 1), (int)(maxY - minY + 1));
        }


        /// <summary>  Returns the minimum and maximum X and Y coordinates of the atoms in the
        /// AtomContainer. The output is returned as: <pre>
        /// minmax[0] = minX;
        /// minmax[1] = minY;
        /// minmax[2] = maxX;
        /// minmax[3] = maxY;
        /// </pre>
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="container"> Description of the Parameter
        /// </param>
        /// <returns>            An four int array as defined above.
        /// </returns>
        public static double[] getMinMax(IAtomContainer container)
        {
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double maxX = System.Double.MinValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double maxY = System.Double.MinValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minX = System.Double.MaxValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minY = System.Double.MaxValue;
            for (int i = 0; i < container.AtomCount; i++)
            {
                IAtom atom = container.getAtomAt(i);
                if (atom.getPoint2d() != null)
                {
                    if (atom.X2d > maxX)
                    {
                        maxX = atom.X2d;
                    }
                    if (atom.X2d < minX)
                    {
                        minX = atom.X2d;
                    }
                    if (atom.Y2d > maxY)
                    {
                        maxY = atom.Y2d;
                    }
                    if (atom.Y2d < minY)
                    {
                        minY = atom.Y2d;
                    }
                }
            }
            double[] minmax = new double[4];
            minmax[0] = minX;
            minmax[1] = minY;
            minmax[2] = maxX;
            minmax[3] = maxY;
            return minmax;
        }


        /// <summary>  Returns the java.awt.Dimension of a molecule
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> of which the dimension should be returned
        /// </param>
        /// <returns>          The java.awt.Dimension of this molecule
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static System.Drawing.Size get2DDimension(IAtomContainer atomCon, System.Collections.Hashtable renderingCoordinates)
        {
            double[] minmax = getMinMax(atomCon, renderingCoordinates);
            double maxX = minmax[2];
            double maxY = minmax[3];
            double minX = minmax[0];
            double minY = minmax[1];
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            return new System.Drawing.Size((int)(maxX - minX + 1), (int)(maxY - minY + 1));
        }

        /// <summary>  Returns the java.awt.Dimension of a SetOfMolecules
        /// See comment for center(ISetOfMolecules setOfMolecules, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="setOfMolecules">Of which the dimension should be returned
        /// </param>
        /// <returns> The java.awt.Dimension of this SetOfMolecules
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static System.Drawing.Size get2DDimension(ISetOfMolecules setOfMolecules, System.Collections.Hashtable renderingCoordinates)
        {
            double[] minmax = getMinMax(setOfMolecules, renderingCoordinates);
            double maxX = minmax[2];
            double maxY = minmax[3];
            double minX = minmax[0];
            double minY = minmax[1];
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            return new System.Drawing.Size((int)(maxX - minX + 1), (int)(maxY - minY + 1));
        }


        /// <summary>  Returns the minimum and maximum X and Y coordinates of the atoms in the
        /// AtomContainer. The output is returned as: <pre>
        /// minmax[0] = minX;
        /// minmax[1] = minY;
        /// minmax[2] = maxX;
        /// minmax[3] = maxY;
        /// </pre>
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="container"> Description of the Parameter
        /// </param>
        /// <returns>            An four int array as defined above.
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static double[] getMinMax(IAtomContainer container, System.Collections.Hashtable renderingCoordinates)
        {
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double maxX = System.Double.MinValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double maxY = System.Double.MinValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minX = System.Double.MaxValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minY = System.Double.MaxValue;
            for (int i = 0; i < container.AtomCount; i++)
            {
                IAtom atom = container.getAtomAt(i);
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[atom] != null)
                {
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    if (((Point2d)renderingCoordinates[atom]).x > maxX)
                    {
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        maxX = ((Point2d)renderingCoordinates[atom]).x;
                    }
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    if (((Point2d)renderingCoordinates[atom]).x < minX)
                    {
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        minX = ((Point2d)renderingCoordinates[atom]).x;
                    }
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    if (((Point2d)renderingCoordinates[atom]).y > maxY)
                    {
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        maxY = ((Point2d)renderingCoordinates[atom]).y;
                    }
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    if (((Point2d)renderingCoordinates[atom]).y < minY)
                    {
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        minY = ((Point2d)renderingCoordinates[atom]).y;
                    }
                }
            }
            double[] minmax = new double[4];
            minmax[0] = minX;
            minmax[1] = minY;
            minmax[2] = maxX;
            minmax[3] = maxY;
            return minmax;
        }

        /// <summary>  Returns the minimum and maximum X and Y coordinates of the molecules in the
        /// SetOfMolecules. The output is returned as: <pre>
        /// minmax[0] = minX;
        /// minmax[1] = minY;
        /// minmax[2] = maxX;
        /// minmax[3] = maxY;
        /// </pre>
        /// See comment for center(ISetOfMolecules setOfMolecules, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <returns>            An four int array as defined above.
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static double[] getMinMax(ISetOfMolecules setOfMolecules, System.Collections.Hashtable renderingCoordinates)
        {
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double maxX = System.Double.MinValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double maxY = System.Double.MinValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minX = System.Double.MaxValue;
            //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            double minY = System.Double.MaxValue;
            for (int j = 0; j < setOfMolecules.AtomContainerCount; j++)
            {
                IAtomContainer container = setOfMolecules.getAtomContainer(j);
                for (int i = 0; i < container.AtomCount; i++)
                {
                    IAtom atom = container.getAtomAt(i);
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    if (renderingCoordinates[atom] != null)
                    {
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        if (((Point2d)renderingCoordinates[atom]).x > maxX)
                        {
                            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                            maxX = ((Point2d)renderingCoordinates[atom]).x;
                        }
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        if (((Point2d)renderingCoordinates[atom]).x < minX)
                        {
                            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                            minX = ((Point2d)renderingCoordinates[atom]).x;
                        }
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        if (((Point2d)renderingCoordinates[atom]).y > maxY)
                        {
                            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                            maxY = ((Point2d)renderingCoordinates[atom]).y;
                        }
                        //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                        if (((Point2d)renderingCoordinates[atom]).y < minY)
                        {
                            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                            minY = ((Point2d)renderingCoordinates[atom]).y;
                        }
                    }
                }
            }
            double[] minmax = new double[4];
            minmax[0] = minX;
            minmax[1] = minY;
            minmax[2] = maxX;
            minmax[3] = maxY;
            return minmax;
        }


        /// <summary>  Translates a molecule from the origin to a new point denoted by a vector, using an external set of coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> molecule to be translated
        /// </param>
        /// <param name="vector">  dimension that represents the translation vector
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static void translate2D(IAtomContainer atomCon, Vector2d vector, System.Collections.Hashtable renderingCoordinates)
        {
            IAtom[] atoms = atomCon.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[atoms[i]] == null && atoms[i].getPoint2d() != null)
                {
                    renderingCoordinates[atoms[i]] = new Point2d(atoms[i].getPoint2d().x, atoms[i].getPoint2d().y);
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (((Point2d)renderingCoordinates[atoms[i]]) != null)
                {
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    ((Point2d)renderingCoordinates[atoms[i]]).add(vector);
                }
                else
                {
                    //logger.warn("Could not translate atom in 2D space");
                }
            }
        }


        /// <summary>  Translates a molecule from the origin to a new point denoted by a vector.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atomCon"> molecule to be translated
        /// </param>
        /// <param name="p">       Description of the Parameter
        /// </param>
        public static void translate2DCentreOfMassTo(IAtomContainer atomCon, Point2d p)
        {
            Point2d com = get2DCentreOfMass(atomCon);
            Vector2d translation = new Vector2d(p.x - com.x, p.y - com.y);
            IAtom[] atoms = atomCon.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint2d() != null)
                {
                    atoms[i].getPoint2d().add(translation);
                }
            }
        }


        /// <summary>  Centers the molecule in the given area, using an external set of coordinates
        /// Attention: Many methods in this class working on coordinates exist in two versions: One with a HashMap as last parameter, one without
        /// this. The difference is as follows: The methods without the HashMap change the coordinates in the Atoms of the AtomContainer. The methods with the HashMaps
        /// expect in this HashMaps pairs of atoms and Point2ds. They work on the Point2ds associated with a particular atom and leave the atom itself
        /// unchanged. If there is no entry in the HashMap for an atom, they put the coordinates from the Atom in this HashMap and then work on the HashMap.
        /// 
        /// 
        /// </summary>
        /// <param name="atomCon"> molecule to be centered
        /// </param>
        /// <param name="areaDim"> dimension in which the molecule is to be centered
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
        public static void center(IAtomContainer atomCon, ref System.Drawing.Size areaDim, System.Collections.Hashtable renderingCoordinates)
        {
            System.Drawing.Size molDim = get2DDimension(atomCon, renderingCoordinates);
            int transX = (int)((areaDim.Width - molDim.Width) / 2);
            int transY = (int)((areaDim.Height - molDim.Height) / 2);
            translateAllPositive(atomCon, renderingCoordinates);
            translate2D(atomCon, new Vector2d(transX, transY), renderingCoordinates);
        }


        /// <summary>  Calculates the center of the given atoms and returns it as a Point2d, using
        /// an external set of coordinates
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atoms"> The vector of the given atoms
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        /// <returns>        The center of the given atoms as Point2d
        /// 
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static Point2d get2DCenter(IAtom[] atoms, System.Collections.Hashtable renderingCoordinates)
        {
            IAtom atom;
            double x = 0;
            double y = 0;
            for (int f = 0; f < atoms.Length; f++)
            {
                atom = (IAtom)atoms[f];
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[atom] != null)
                {
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    x += ((Point2d)renderingCoordinates[atom]).x;
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    y += ((Point2d)renderingCoordinates[atom]).y;
                }
            }
            return new Point2d(x / (double)atoms.Length, y / (double)atoms.Length);
        }


        /// <summary>  Calculates the center of the given atoms and returns it as a Point2d
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="atoms"> The vector of the given atoms
        /// </param>
        /// <returns>        The center of the given atoms as Point2d
        /// </returns>
        public static Point2d get2DCenter(IAtom[] atoms)
        {
            IAtom atom;
            double x = 0;
            double y = 0;
            for (int f = 0; f < atoms.Length; f++)
            {
                atom = (IAtom)atoms[f];
                if (atom.getPoint2d() != null)
                {
                    x += atom.X2d;
                    y += atom.Y2d;
                }
            }
            return new Point2d(x / (double)atoms.Length, y / (double)atoms.Length);
        }


        /// <summary>  Returns the geometric center of all the rings in this ringset.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="ringSet"> Description of the Parameter
        /// </param>
        /// <returns>          the geometric center of the rings in this ringset
        /// </returns>
        public static Point2d get2DCenter(IRingSet ringSet)
        {
            double centerX = 0;
            double centerY = 0;
            for (int i = 0; i < ringSet.AtomContainerCount; i++)
            {
                Point2d centerPoint = GeometryTools.get2DCenter((IRing)ringSet.getAtomContainer(i));
                centerX += centerPoint.x;
                centerY += centerPoint.y;
            }
            Point2d point = new Point2d(centerX / ((double)ringSet.AtomContainerCount), centerY / ((double)ringSet.AtomContainerCount));
            return point;
        }


        /// <summary>  Calculates the center of mass for the <code>Atom</code>s in the
        /// AtomContainer for the 2D coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="ac">     AtomContainer for which the center of mass is calculated
        /// </param>
        /// <returns>         Description of the Return Value
        /// </returns>
        /// <cdk.keyword>     center of mass </cdk.keyword>
        public static Point2d get2DCentreOfMass(IAtomContainer ac)
        {
            double x = 0.0;
            double y = 0.0;

            double totalmass = 0.0;

            System.Collections.IEnumerator atoms = ac.atoms();
            //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
            while (atoms.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                IAtom a = (IAtom)atoms.Current;
                double mass = a.getExactMass();
                totalmass += mass;
                x += mass * a.X2d;
                y += mass * a.Y2d;
            }

            return new Point2d(x / totalmass, y / totalmass);
        }


        /// <summary>  Returns the geometric center of all the atoms in the atomContainer.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="container"> Description of the Parameter
        /// </param>
        /// <returns>            the geometric center of the atoms in this atomContainer
        /// </returns>
        public static Point2d get2DCenter(IAtomContainer container)
        {
            double centerX = 0;
            double centerY = 0;
            double counter = 0;
            IAtom[] atoms = container.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint2d() != null)
                {
                    centerX += atoms[i].getPoint2d().x;
                    centerY += atoms[i].getPoint2d().y;
                    counter++;
                }
            }
            Point2d point = new Point2d(centerX / (counter), centerY / (counter));
            return point;
        }


        /// <summary>  Returns the geometric center of all the atoms in the atomContainer.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="container"> Description of the Parameter
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        /// <returns>            the geometric center of the atoms in this atomContainer
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static Point2d get2DCenter(IAtomContainer container, System.Collections.Hashtable renderingCoordinates)
        {
            double centerX = 0;
            double centerY = 0;
            double counter = 0;
            IAtom[] atoms = container.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint2d() != null)
                {
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    centerX += ((Point2d)renderingCoordinates[atoms[i]]).x;
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    centerY += ((Point2d)renderingCoordinates[atoms[i]]).y;
                    counter++;
                }
            }
            Point2d point = new Point2d(centerX / (counter), centerY / (counter));
            return point;
        }


        /// <summary>  Translates the geometric 2DCenter of the given
        /// AtomContainer container to the specified Point2d p.
        /// 
        /// </summary>
        /// <param name="container"> AtomContainer which should be translated.
        /// </param>
        /// <param name="p">         New Location of the geometric 2D Center.
        /// </param>
        /// <seealso cref="get2DCenter">
        /// </seealso>
        /// <seealso cref="translate2DCentreOfMassTo">
        /// </seealso>
        public static void translate2DCenterTo(IAtomContainer container, Point2d p)
        {
            Point2d com = get2DCenter(container);
            Vector2d translation = new Vector2d(p.x - com.x, p.y - com.y);
            IAtom[] atoms = container.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint2d() != null)
                {
                    atoms[i].getPoint2d().add(translation);
                }
            }
        }


        /// <summary>  Calculates the center of mass for the <code>Atom</code>s in the
        /// AtomContainer for the 2D coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="ac">     AtomContainer for which the center of mass is calculated
        /// </param>
        /// <returns>         Description of the Return Value
        /// </returns>
        /// <cdk.keyword>     center of mass </cdk.keyword>
        /// <cdk.dictref>    blue-obelisk:calculate3DCenterOfMass </cdk.dictref>
        public static Point3d get3DCentreOfMass(IAtomContainer ac)
        {
            double x = 0.0;
            double y = 0.0;
            double z = 0.0;

            double totalmass = 0.0;

            System.Collections.IEnumerator atoms = ac.atoms();
            //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
            while (atoms.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                IAtom a = (IAtom)atoms.Current;
                double mass = a.getExactMass();
                totalmass += mass;
                x += mass * a.X3d;
                y += mass * a.Y3d;
                z += mass * a.Z3d;
            }

            return new Point3d(x / totalmass, y / totalmass, z / totalmass);
        }


        /// <summary>  Returns the geometric center of all the atoms in this atomContainer.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="ac"> Description of the Parameter
        /// </param>
        /// <returns>     the geometric center of the atoms in this atomContainer
        /// </returns>
        public static Point3d get3DCenter(IAtomContainer ac)
        {
            double centerX = 0;
            double centerY = 0;
            double centerZ = 0;
            double counter = 0;
            IAtom[] atoms = ac.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint3d() != null)
                {
                    centerX += atoms[i].getPoint3d().x;
                    centerY += atoms[i].getPoint3d().y;
                    centerZ += atoms[i].getPoint3d().z;
                    counter++;
                }
            }
            Point3d point = new Point3d(centerX / (counter), centerY / (counter), centerZ / (counter));
            return point;
        }


        /// <summary>  Gets the angle attribute of the GeometryTools class
        /// 
        /// </summary>
        /// <param name="xDiff"> Description of the Parameter
        /// </param>
        /// <param name="yDiff"> Description of the Parameter
        /// </param>
        /// <returns>        The angle value
        /// </returns>
        public static double getAngle(double xDiff, double yDiff)
        {
            double angle = 0;
            //		System.out.println("getAngle->xDiff: " + xDiff);
            //		System.out.println("getAngle->yDiff: " + yDiff);
            if (xDiff >= 0 && yDiff >= 0)
            {
                angle = System.Math.Atan(yDiff / xDiff);
            }
            else if (xDiff < 0 && yDiff >= 0)
            {
                angle = System.Math.PI + System.Math.Atan(yDiff / xDiff);
            }
            else if (xDiff < 0 && yDiff < 0)
            {
                angle = System.Math.PI + System.Math.Atan(yDiff / xDiff);
            }
            else if (xDiff >= 0 && yDiff < 0)
            {
                angle = 2 * System.Math.PI + System.Math.Atan(yDiff / xDiff);
            }
            return angle;
        }


        /// <summary>  Gets the coordinates of two points (that represent a bond) and calculates
        /// for each the coordinates of two new points that have the given distance
        /// vertical to the bond.
        /// 
        /// </summary>
        /// <param name="coords"> The coordinates of the two given points of the bond like this
        /// [point1x, point1y, point2x, point2y]
        /// </param>
        /// <param name="dist">   The vertical distance between the given points and those to
        /// be calculated
        /// </param>
        /// <returns>         The coordinates of the calculated four points
        /// </returns>
        public static int[] distanceCalculator(int[] coords, double dist)
        {
            double angle;
            if ((coords[2] - coords[0]) == 0)
            {
                angle = System.Math.PI / 2;
            }
            else
            {
                angle = System.Math.Atan(((double)coords[3] - (double)coords[1]) / ((double)coords[2] - (double)coords[0]));
            }
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            int begin1X = (int)(System.Math.Cos(angle + System.Math.PI / 2) * dist + coords[0]);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            int begin1Y = (int)(System.Math.Sin(angle + System.Math.PI / 2) * dist + coords[1]);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            int begin2X = (int)(System.Math.Cos(angle - System.Math.PI / 2) * dist + coords[0]);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            int begin2Y = (int)(System.Math.Sin(angle - System.Math.PI / 2) * dist + coords[1]);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            int end1X = (int)(System.Math.Cos(angle - System.Math.PI / 2) * dist + coords[2]);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            int end1Y = (int)(System.Math.Sin(angle - System.Math.PI / 2) * dist + coords[3]);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            int end2X = (int)(System.Math.Cos(angle + System.Math.PI / 2) * dist + coords[2]);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            int end2Y = (int)(System.Math.Sin(angle + System.Math.PI / 2) * dist + coords[3]);

            int[] newCoords = new int[] { begin1X, begin1Y, begin2X, begin2Y, end1X, end1Y, end2X, end2Y };
            return newCoords;
        }


        /// <summary>  Writes the coordinates of the atoms participating the given bond into an
        /// array, using renderingCoordinates, using an external set of coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="bond"> The given bond
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        /// <returns>       The array with the coordinates
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static int[] getBondCoordinates(IBond bond, System.Collections.Hashtable renderingCoordinates)
        {
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            if (renderingCoordinates[bond.getAtomAt(0)] == null && bond.getAtomAt(0).getPoint2d() != null)
            {
                renderingCoordinates[bond.getAtomAt(0)] = new Point2d(bond.getAtomAt(0).getPoint2d().x, bond.getAtomAt(0).getPoint2d().y);
            }
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            if (renderingCoordinates[bond.getAtomAt(1)] == null && bond.getAtomAt(1).getPoint2d() != null)
            {
                renderingCoordinates[bond.getAtomAt(1)] = new Point2d(bond.getAtomAt(1).getPoint2d().x, bond.getAtomAt(1).getPoint2d().y);
            }
            if (bond.getAtomAt(0).getPoint2d() == null || bond.getAtomAt(1).getPoint2d() == null)
            {
                //logger.error("getBondCoordinates() called on Bond without 2D coordinates!");
                return new int[0];
            }
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            int beginX = (int)((Point2d)renderingCoordinates[bond.getAtomAt(0)]).x;
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            int endX = (int)((Point2d)renderingCoordinates[bond.getAtomAt(1)]).x;
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            int beginY = (int)((Point2d)renderingCoordinates[bond.getAtomAt(0)]).y;
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            int endY = (int)((Point2d)renderingCoordinates[bond.getAtomAt(1)]).y;
            int[] coords = new int[] { beginX, beginY, endX, endY };
            return coords;
        }


        /// <summary>  Writes the coordinates of the atoms participating the given bond into an
        /// array.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="bond"> The given bond
        /// </param>
        /// <returns>       The array with the coordinates
        /// </returns>
        public static int[] getBondCoordinates(IBond bond)
        {
            if (bond.getAtomAt(0).getPoint2d() == null || bond.getAtomAt(1).getPoint2d() == null)
            {
                //logger.error("getBondCoordinates() called on Bond without 2D coordinates!");
                return new int[0];
            }
            int beginX = (int)bond.getAtomAt(0).getPoint2d().x;
            int endX = (int)bond.getAtomAt(1).getPoint2d().x;
            int beginY = (int)bond.getAtomAt(0).getPoint2d().y;
            int endY = (int)bond.getAtomAt(1).getPoint2d().y;
            int[] coords = new int[] { beginX, beginY, endX, endY };
            return coords;
        }


        /// <summary>  Returns the atom of the given molecule that is closest to the given
        /// coordinates, using an external set of coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="xPosition"> The x coordinate
        /// </param>
        /// <param name="yPosition"> The y coordinate
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        /// <returns>            The atom that is closest to the given coordinates
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static IAtom getClosestAtom(int xPosition, int yPosition, IChemModel model, IAtom ignore, System.Collections.Hashtable renderingCoordinates)
        {
            IAtom closestAtom = null;
            IAtom currentAtom;
            double smallestMouseDistance = -1;
            double mouseDistance;
            double atomX;
            double atomY;
            IAtomContainer all = ChemModelManipulator.getAllInOneContainer(model);
            for (int i = 0; i < all.AtomCount; i++)
            {
                currentAtom = all.getAtomAt(i);
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[currentAtom] == null && currentAtom.getPoint2d() != null)
                {
                    renderingCoordinates[currentAtom] = new Point2d(currentAtom.getPoint2d().x, currentAtom.getPoint2d().y);
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (currentAtom != ignore && renderingCoordinates[currentAtom] != null)
                {
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    atomX = ((Point2d)renderingCoordinates[currentAtom]).x;
                    //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                    atomY = ((Point2d)renderingCoordinates[currentAtom]).y;
                    mouseDistance = System.Math.Sqrt(System.Math.Pow(atomX - xPosition, 2) + System.Math.Pow(atomY - yPosition, 2));
                    if (mouseDistance < smallestMouseDistance || smallestMouseDistance == -1)
                    {
                        smallestMouseDistance = mouseDistance;
                        closestAtom = currentAtom;
                    }
                }
            }
            return closestAtom;
        }


        /// <summary>  Returns the atom of the given molecule that is closest to the given
        /// coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="xPosition"> The x coordinate
        /// </param>
        /// <param name="yPosition"> The y coordinate
        /// </param>
        /// <param name="atomCon">   The molecule that is searched for the closest atom
        /// </param>
        /// <returns>            The atom that is closest to the given coordinates
        /// </returns>
        public static IAtom getClosestAtom(int xPosition, int yPosition, IAtomContainer atomCon)
        {
            IAtom closestAtom = null;
            IAtom currentAtom;
            double smallestMouseDistance = -1;
            double mouseDistance;
            double atomX;
            double atomY;
            for (int i = 0; i < atomCon.AtomCount; i++)
            {
                currentAtom = atomCon.getAtomAt(i);
                atomX = currentAtom.X2d;
                atomY = currentAtom.Y2d;
                mouseDistance = System.Math.Sqrt(System.Math.Pow(atomX - xPosition, 2) + System.Math.Pow(atomY - yPosition, 2));
                if (mouseDistance < smallestMouseDistance || smallestMouseDistance == -1)
                {
                    smallestMouseDistance = mouseDistance;
                    closestAtom = currentAtom;
                }
            }
            return closestAtom;
        }


        /// <summary>  Returns the bond of the given molecule that is closest to the given
        /// coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="xPosition"> The x coordinate
        /// </param>
        /// <param name="yPosition"> The y coordinate
        /// </param>
        /// <param name="atomCon">   The molecule that is searched for the closest bond
        /// </param>
        /// <returns>            The bond that is closest to the given coordinates
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static IBond getClosestBond(int xPosition, int yPosition, IAtomContainer atomCon, System.Collections.Hashtable renderingCoordinates)
        {
            Point2d bondCenter;
            IBond closestBond = null;
            IBond currentBond;
            double smallestMouseDistance = -1;
            double mouseDistance;
            IBond[] bonds = atomCon.Bonds;
            for (int i = 0; i < bonds.Length; i++)
            {
                currentBond = bonds[i];
                bondCenter = get2DCenter(currentBond.getAtoms(), renderingCoordinates);
                mouseDistance = System.Math.Sqrt(System.Math.Pow(bondCenter.x - xPosition, 2) + System.Math.Pow(bondCenter.y - yPosition, 2));
                if (mouseDistance < smallestMouseDistance || smallestMouseDistance == -1)
                {
                    smallestMouseDistance = mouseDistance;
                    closestBond = currentBond;
                }
            }
            return closestBond;
        }


        /// <summary>  Returns the bond of the given molecule that is closest to the given
        /// coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="xPosition"> The x coordinate
        /// </param>
        /// <param name="yPosition"> The y coordinate
        /// </param>
        /// <param name="atomCon">   The molecule that is searched for the closest bond
        /// </param>
        /// <returns>            The bond that is closest to the given coordinates
        /// </returns>
        public static IBond getClosestBond(int xPosition, int yPosition, IAtomContainer atomCon)
        {
            Point2d bondCenter;
            IBond closestBond = null;
            IBond currentBond;
            double smallestMouseDistance = -1;
            double mouseDistance;
            IBond[] bonds = atomCon.Bonds;
            for (int i = 0; i < bonds.Length; i++)
            {
                currentBond = bonds[i];
                bondCenter = get2DCenter(currentBond.getAtoms());
                mouseDistance = System.Math.Sqrt(System.Math.Pow(bondCenter.x - xPosition, 2) + System.Math.Pow(bondCenter.y - yPosition, 2));
                if (mouseDistance < smallestMouseDistance || smallestMouseDistance == -1)
                {
                    smallestMouseDistance = mouseDistance;
                    closestBond = currentBond;
                }
            }
            return closestBond;
        }


        /// <summary>  Sorts a Vector of atoms such that the 2D distances of the atom locations
        /// from a given point are smallest for the first atoms in the vector
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="point"> The point from which the distances to the atoms are measured
        /// </param>
        /// <param name="atoms"> The atoms for which the distances to point are measured
        /// </param>
        public static void sortBy2DDistance(IAtom[] atoms, Point2d point)
        {
            double distance1;
            double distance2;
            IAtom atom1 = null;
            IAtom atom2 = null;
            bool doneSomething = false;
            do
            {
                doneSomething = false;
                for (int f = 0; f < atoms.Length - 1; f++)
                {
                    atom1 = atoms[f];
                    atom2 = atoms[f + 1];
                    distance1 = point.distance(atom1.getPoint2d());
                    distance2 = point.distance(atom2.getPoint2d());
                    if (distance2 < distance1)
                    {
                        atoms[f] = atom2;
                        atoms[f + 1] = atom1;
                        doneSomething = true;
                    }
                }
            }
            while (doneSomething);
        }


        /// <summary>  Determines the scale factor for displaying a structure loaded from disk in
        /// a frame. An average of all bond length values is produced and a scale
        /// factor is determined which would scale the given molecule such that its
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="ac">         The AtomContainer for which the ScaleFactor is to be
        /// calculated
        /// </param>
        /// <param name="bondLength"> The target bond length
        /// </param>
        /// <returns>             The ScaleFactor with which the AtomContainer must be
        /// scaled to have the target bond length
        /// </returns>

        public static double getScaleFactor(IAtomContainer ac, double bondLength)
        {
            double currentAverageBondLength = getBondLengthAverage(ac);
            if (currentAverageBondLength == 0 || System.Double.IsNaN(currentAverageBondLength))
                return 1;
            return bondLength / currentAverageBondLength;
        }


        /// <summary>  Determines the scale factor for displaying a structure loaded from disk in
        /// a frame, using an external set of coordinates. An average of all bond length values is produced and a scale
        /// factor is determined which would scale the given molecule such that its
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="ac">         The AtomContainer for which the ScaleFactor is to be
        /// calculated
        /// </param>
        /// <param name="bondLength"> The target bond length
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        /// <returns>             The ScaleFactor with which the AtomContainer must be
        /// scaled to have the target bond length
        /// </returns>

        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static double getScaleFactor(IAtomContainer ac, double bondLength, System.Collections.Hashtable renderingCoordinates)
        {
            IAtom[] atoms = ac.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[atoms[i]] == null && atoms[i].getPoint2d() != null)
                {
                    renderingCoordinates[atoms[i]] = new Point2d(atoms[i].getPoint2d().x, atoms[i].getPoint2d().y);
                }
            }
            double currentAverageBondLength = getBondLengthAverage(ac, renderingCoordinates);
            if (currentAverageBondLength == 0 || System.Double.IsNaN(currentAverageBondLength))
                return 1;
            return bondLength / currentAverageBondLength;
        }


        /// <summary>  An average of all 2D bond length values is produced, using an external set of coordinates. Bonds which have
        /// Atom's with no coordinates are disregarded.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="ac"> The AtomContainer for which the average bond length is to be
        /// calculated
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        /// <returns>     the average bond length
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static double getBondLengthAverage(IAtomContainer ac, System.Collections.Hashtable renderingCoordinates)
        {
            double bondLengthSum = 0;
            IBond[] bonds = ac.Bonds;
            int bondCounter = 0;
            for (int f = 0; f < bonds.Length; f++)
            {
                IBond bond = bonds[f];
                IAtom atom1 = bond.getAtomAt(0);
                IAtom atom2 = bond.getAtomAt(1);
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (renderingCoordinates[atom1] != null && renderingCoordinates[atom2] != null)
                {
                    bondCounter++;
                    bondLengthSum += getLength2D(bond, renderingCoordinates);
                }
            }
            return bondLengthSum / bondCounter;
        }


        /// <summary>  An average of all 2D bond length values is produced. Bonds which have
        /// Atom's with no coordinates are disregarded.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="ac"> The AtomContainer for which the average bond length is to be
        /// calculated
        /// </param>
        /// <returns>     the average bond length
        /// </returns>
        public static double getBondLengthAverage(IAtomContainer ac)
        {
            double bondLengthSum = 0;
            IBond[] bonds = ac.Bonds;
            int bondCounter = 0;
            for (int f = 0; f < bonds.Length; f++)
            {
                IBond bond = bonds[f];
                IAtom atom1 = bond.getAtomAt(0);
                IAtom atom2 = bond.getAtomAt(1);
                if (atom1.getPoint2d() != null && atom2.getPoint2d() != null)
                {
                    bondCounter++;
                    bondLengthSum += getLength2D(bond);
                }
            }
            return bondLengthSum / bondCounter;
        }


        /// <summary>  Returns the geometric length of this bond in 2D space, using an external set of coordinates
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="bond"> Description of the Parameter
        /// </param>
        /// <param name="renderingCoordinates"> The set of coordinates to use coming from RendererModel2D
        /// </param>
        /// <returns>       The geometric length of this bond
        /// </returns>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static double getLength2D(IBond bond, System.Collections.Hashtable renderingCoordinates)
        {
            if (bond.getAtomAt(0) == null || bond.getAtomAt(1) == null)
            {
                return 0.0;
            }
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            Point2d p1 = ((Point2d)renderingCoordinates[bond.getAtomAt(0)]);
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            Point2d p2 = ((Point2d)renderingCoordinates[bond.getAtomAt(1)]);
            if (p1 == null || p2 == null)
            {
                return 0.0;
            }
            return p1.distance(p2);
        }

        /// <summary>  Returns the geometric length of this bond in 2D space.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="bond"> Description of the Parameter
        /// </param>
        /// <returns>       The geometric length of this bond
        /// </returns>
        public static double getLength2D(IBond bond)
        {
            if (bond.getAtomAt(0) == null || bond.getAtomAt(1) == null)
            {
                return 0.0;
            }
            Point2d p1 = bond.getAtomAt(0).getPoint2d();
            Point2d p2 = bond.getAtomAt(1).getPoint2d();
            if (p1 == null || p2 == null)
            {
                return 0.0;
            }
            return p1.distance(p2);
        }


        /// <summary>  Determines if this AtomContainer contains 2D coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="m"> Description of the Parameter
        /// </param>
        /// <returns>    boolean indication that 2D coordinates are available
        /// </returns>
        public static bool has2DCoordinates(IAtomContainer m)
        {
            return has2DCoordinatesNew(m) > 0;
        }


        /// <summary>  Determines if this AtomContainer contains 2D coordinates for some or all molecules.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="m"> Description of the Parameter
        /// </param>
        /// <returns>    0 no 2d, 1=some, 2= for each atom
        /// </returns>
        public static int has2DCoordinatesNew(IAtomContainer m)
        {
            bool no2d = false;
            bool with2d = false;
            IAtom[] atoms = m.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint2d() == null)
                {
                    no2d = true;
                }
                else
                {
                    with2d = true;
                }
            }
            if (!no2d && with2d)
            {
                return 2;
            }
            else if (no2d && with2d)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>  Determines if this Atom contains 2D coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="a"> Description of the Parameter
        /// </param>
        /// <returns>    boolean indication that 2D coordinates are available
        /// </returns>
        public static bool has2DCoordinates(IAtom a)
        {
            return (a.getPoint2d() != null);
        }


        /// <summary>  Determines if this Bond contains 2D coordinates.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="b"> Description of the Parameter
        /// </param>
        /// <returns>    boolean indication that 2D coordinates are available
        /// </returns>
        public static bool has2DCoordinates(IBond b)
        {
            IAtom[] atoms = b.getAtoms();
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint2d() == null)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>  Determines if this model contains 3D coordinates
        /// 
        /// </summary>
        /// <param name="m"> Description of the Parameter
        /// </param>
        /// <returns>    boolean indication that 3D coordinates are available
        /// </returns>
        public static bool has3DCoordinates(IAtomContainer m)
        {
            bool hasinfo = true;
            IAtom[] atoms = m.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].getPoint3d() == null)
                {
                    return false;
                }
            }
            return hasinfo;
        }


        /// <summary>  Determines the normalized vector orthogonal on the vector p1->p2.
        /// 
        /// </summary>
        /// <param name="p1"> Description of the Parameter
        /// </param>
        /// <param name="p2"> Description of the Parameter
        /// </param>
        /// <returns>     Description of the Return Value
        /// </returns>
        public static Vector2d calculatePerpendicularUnitVector(Point2d p1, Point2d p2)
        {
            Vector2d v = new Vector2d();
            v.sub(p2, p1);
            v.normalize();

            // Return the perpendicular vector
            return new Vector2d((-1.0) * v.y, v.x);
        }


        /// <summary>  Calculates the normalization factor in order to get an average bond length
        /// of 1.5. It takes only into account Bond's with two atoms.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="container"> Description of the Parameter
        /// </param>
        /// <returns>            The normalizationFactor value
        /// </returns>
        public static double getNormalizationFactor(IAtomContainer container)
        {
            IBond[] bonds = container.Bonds;
            double bondlength = 0.0;
            double ratio = 0.0;
            /*
            *  Desired bond length for storing structures in MDL mol files
            *  This should probably be set externally (from system wide settings)
            */
            double desiredBondLength = 1.5;
            // loop over all bonds and determine the mean bond distance
            int counter = 0;
            for (int f = 0; f < bonds.Length; f++)
            {
                // only consider two atom bonds into account
                if (bonds[f].AtomCount == 2)
                {
                    counter++;
                    IAtom atom1 = bonds[f].getAtomAt(0);
                    IAtom atom2 = bonds[f].getAtomAt(1);
                    bondlength += System.Math.Sqrt(System.Math.Pow(atom1.X2d - atom2.X2d, 2) + System.Math.Pow(atom1.Y2d - atom2.Y2d, 2));
                }
            }
            bondlength = bondlength / counter;
            ratio = desiredBondLength / bondlength;
            return ratio;
        }


        /// <summary>  Determines the best alignment for the label of an atom in 2D space. It
        /// returns 1 if left aligned, and -1 if right aligned.
        /// See comment for center(IAtomContainer atomCon, Dimension areaDim, HashMap renderingCoordinates) for details on coordinate sets
        /// 
        /// </summary>
        /// <param name="container"> Description of the Parameter
        /// </param>
        /// <param name="atom">      Description of the Parameter
        /// </param>
        /// <returns>            The bestAlignmentForLabel value
        /// </returns>
        public static int getBestAlignmentForLabel(IAtomContainer container, IAtom atom)
        {
            IAtom[] connectedAtoms = container.getConnectedAtoms(atom);
            int overallDiffX = 0;
            for (int i = 0; i < connectedAtoms.Length; i++)
            {
                IAtom connectedAtom = connectedAtoms[i];
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                overallDiffX = overallDiffX + (int)(connectedAtom.X2d - atom.X2d);
            }
            if (overallDiffX <= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }


        /// <summary>  Returns the atoms which are closes to an atom in an AtomContainer by
        /// distance in 3d.
        /// 
        /// </summary>
        /// <param name="ac">               The AtomContainer to examine
        /// </param>
        /// <param name="a">                the atom to start from
        /// </param>
        /// <param name="max">              the number of neighbours to return
        /// </param>
        /// <returns>                   the average bond length
        /// </returns>
        /// <exception cref="CDKException"> Description of the Exception
        /// </exception>
        public static System.Collections.ArrayList findClosestInSpace(IAtomContainer ac, IAtom a, int max)
        {
            IAtom[] atoms = ac.Atoms;
            Point3d originalPoint = a.getPoint3d();
            if (originalPoint == null)
            {
                throw new CDKException("No point3d, but findClosestInSpace is working on point3ds");
            }
            //UPGRADE_TODO: Constructor 'java.util.TreeMap.TreeMap' was converted to 'System.Collections.SortedList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapTreeMap'"
            //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.TreeMap' and 'System.Collections.SortedList' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            System.Collections.IDictionary hm = new System.Collections.SortedList();
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i] != a)
                {
                    if (atoms[i].getPoint3d() == null)
                    {
                        throw new CDKException("No point3d, but findClosestInSpace is working on point3ds");
                    }
                    double distance = atoms[i].getPoint3d().distance(originalPoint);
                    hm[(double)distance] = atoms[i];
                }
            }
            //UPGRADE_TODO: Method 'java.util.Map.keySet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilMapkeySet'"
            CSGraphT.SupportClass.SetSupport ks = new CSGraphT.SupportClass.HashSetSupport(hm.Keys);
            System.Collections.IEnumerator it = ks.GetEnumerator();
            System.Collections.ArrayList returnValue = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            int i2 = 0;
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (it.MoveNext() && i2 < max)
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                returnValue.Add(hm[it.Current]);
                i2++;
            }
            return (returnValue);
        }
        /// <summary>  Returns a Map with the AtomNumbers, the first number corresponds to the first (or the largest
        /// AtomContainer) atomcontainer. It is recommend to sort the atomContainer due to their number of atoms before
        /// calling this function.
        /// 
        /// The molecules needs to be aligned before! (coordinates are needed)
        /// 
        /// </summary>
        /// <param name="firstAtomContainer">               the (largest) first aligned AtomContainer which is the reference
        /// </param>
        /// <param name="secondAtomContainer">              the second aligned AtomContainer
        /// </param>
        /// <param name="searchRadius">              		the radius of space search from each atom
        /// </param>
        /// <returns>                   				a Map of the mapped atoms
        /// </returns>
        /// <exception cref="CDKException"> Description of the Exception
        /// </exception>
        public static System.Collections.IDictionary mapAtomsOfAlignedStructures(IAtomContainer firstAtomContainer, IAtomContainer secondAtomContainer, double searchRadius, System.Collections.IDictionary mappedAtoms)
        {
            //to return the mapping setProperty("MappedAtom",AtomNumber)
            //System.out.println("**** MAP ATOMS ****");
            getLargestAtomContainer(firstAtomContainer, secondAtomContainer);
            double[][] distanceMatrix = new double[firstAtomContainer.AtomCount][];
            for (int i = 0; i < firstAtomContainer.AtomCount; i++)
            {
                distanceMatrix[i] = new double[secondAtomContainer.AtomCount];
            }
            for (int i = 0; i < firstAtomContainer.AtomCount; i++)
            {
                Point3d firstAtomPoint = firstAtomContainer.getAtomAt(i).getPoint3d();
                //System.out.println("Closest atoms of "+firstAtomContainer.getAtoms()[i].getSymbol()+" :");
                for (int j = 0; j < secondAtomContainer.AtomCount; j++)
                {
                    distanceMatrix[i][j] = firstAtomPoint.distance(secondAtomContainer.getAtomAt(j).getPoint3d());
                    //System.out.println("Distance "+i+" "+j+":"+distanceMatrix[i][j]);
                }
                //System.out.println(" Atoms from the secondAtomContainer");
            }

            //System.out.println();
            //System.out.print("\t");
            //for (int j=0;j<secondAtomContainer.getAtomCount();j++){
            //System.out.print(j+" "+secondAtomContainer.getAtomAt(j).getSymbol()+"\t");
            //}
            double tmp = 0;
            for (int i = 0; i < firstAtomContainer.AtomCount; i++)
            {
                //System.out.print(i+" "+firstAtomContainer.getAtomAt(i).getSymbol()+"\t");
                for (int j = 0; j < secondAtomContainer.AtomCount; j++)
                {
                    tmp = System.Math.Floor(distanceMatrix[i][j] * 10);
                    //System.out.println(tmp/10+"\t");
                }
            }

            double minimumDistance = searchRadius;
            int countMappedAtoms = 0;
            for (int i = 0; i < firstAtomContainer.AtomCount; i++)
            {
                minimumDistance = searchRadius;
                for (int j = 0; j < secondAtomContainer.AtomCount; j++)
                {
                    if (distanceMatrix[i][j] < searchRadius && distanceMatrix[i][j] < minimumDistance)
                    {
                        //System.out.println("Distance OK "+i+" "+j+":"+distanceMatrix[i][j]+" AtomCheck:"+checkAtomMapping(firstAtomContainer,secondAtomContainer, i, j));
                        //check atom properties
                        if (checkAtomMapping(firstAtomContainer, secondAtomContainer, i, j))
                        {
                            minimumDistance = distanceMatrix[i][j];
                            mappedAtoms[(System.Int32)firstAtomContainer.getAtomNumber(firstAtomContainer.getAtomAt(i))] = (System.Int32)secondAtomContainer.getAtomNumber(secondAtomContainer.getAtomAt(j));
                            //firstAtomContainer.getAtomAt(i).setProperty("MappedAtom",new Integer(secondAtomContainer.getAtomNumber(secondAtomContainer.getAtomAt(j))));
                            countMappedAtoms++;
                            //System.out.println("#:"+countMappedAtoms+" Atom:"+i+" is mapped to Atom"+j);
                            //System.out.println(firstAtomContainer.getConnectedAtoms(firstAtomContainer.getAtomAt(i)).length);
                        }
                    }
                }
            }
            return mappedAtoms;
        }

        /// <summary>  Returns a Map with the AtomNumbers, the first number corresponds to the first (or the largest
        /// AtomContainer) atomContainer. 
        /// 
        /// Only for similar and aligned molecules with coordinates!
        /// 
        /// </summary>
        /// <param name="firstAtomContainer">               the (largest) first aligned AtomContainer which is the reference
        /// </param>
        /// <param name="secondAtomContainer">              the second aligned AtomContainer
        /// </param>
        /// <returns>                   				a Map of the mapped atoms
        /// </returns>
        /// <exception cref="CDKException"> Description of the Exception
        /// </exception>
        public static System.Collections.IDictionary mapAtomsOfAlignedStructures(IAtomContainer firstAtomContainer, IAtomContainer secondAtomContainer, System.Collections.IDictionary mappedAtoms)
        {
            //System.out.println("**** GT MAP ATOMS ****");
            //Map atoms onto each other
            if (firstAtomContainer.AtomCount < 1 & secondAtomContainer.AtomCount < 1)
            {
                return mappedAtoms;
            }
            RMap map;
            IAtom atom1;
            IAtom atom2;
            int countMappedAtoms = 0;
            System.Collections.IList list;
            try
            {
                list = UniversalIsomorphismTester.getSubgraphAtomsMap(firstAtomContainer, secondAtomContainer);
                //System.out.println("ListSize:"+list.size());
                for (int i = 0; i < list.Count; i++)
                {
                    map = (RMap)list[i];
                    atom1 = firstAtomContainer.getAtomAt(map.Id1);
                    atom2 = secondAtomContainer.getAtomAt(map.Id2);
                    if (checkAtomMapping(firstAtomContainer, secondAtomContainer, firstAtomContainer.getAtomNumber(atom1), secondAtomContainer.getAtomNumber(atom2)))
                    {
                        mappedAtoms[(System.Int32)firstAtomContainer.getAtomNumber(atom1)] = (System.Int32)secondAtomContainer.getAtomNumber(atom2);
                        countMappedAtoms++;
                        //System.out.println("#:"+countMappedAtoms+" Atom:"+firstAtomContainer.getAtomNumber(atom1)+" is mapped to Atom:"+secondAtomContainer.getAtomNumber(atom2));
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Error: Atoms are not similar !!");
                    }
                }
            }
            catch (CDKException e)
            {
                // TODO Auto-generated catch block
                System.Console.Out.WriteLine("Error in UniversalIsomorphismTester due to:");
                SupportClass.WriteStackTrace(e, Console.Error);
            }
            return mappedAtoms;
        }


        private static void getLargestAtomContainer(IAtomContainer firstAC, IAtomContainer secondAC)
        {
            if (firstAC.AtomCount < secondAC.AtomCount)
            {
                IAtomContainer tmp;
                try
                {
                    tmp = (IAtomContainer)firstAC.Clone();
                    firstAC = (IAtomContainer)secondAC.Clone();
                    secondAC = (IAtomContainer)tmp.Clone();
                }
                //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                catch (System.Exception e)
                {
                    // TODO Auto-generated catch block
                    SupportClass.WriteStackTrace(e, Console.Error);
                }
            }
        }

        private static bool checkAtomMapping(IAtomContainer firstAC, IAtomContainer secondAC, int posFirstAtom, int posSecondAtom)
        {
            IAtom firstAtom = firstAC.getAtomAt(posFirstAtom);
            IAtom secondAtom = secondAC.getAtomAt(posSecondAtom);
            if (firstAtom.Symbol.Equals(secondAtom.Symbol) && firstAC.getConnectedAtoms(firstAtom).Length == secondAC.getConnectedAtoms(secondAtom).Length && firstAtom.BondOrderSum == secondAtom.BondOrderSum && firstAtom.MaxBondOrder == secondAtom.MaxBondOrder)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static IAtomContainer setVisitedFlagsToFalse(IAtomContainer atomContainer)
        {
            for (int i = 0; i < atomContainer.AtomCount; i++)
            {
                atomContainer.getAtomAt(i).setFlag(CDKConstants.VISITED, false);
            }
            return atomContainer;
        }

        /// <summary>  Return the RMSD of bonds length between the 2 aligned molecules.
        /// 
        /// </summary>
        /// <param name="firstAtomContainer">               the (largest) first aligned AtomContainer which is the reference
        /// </param>
        /// <param name="secondAtomContainer">              the second aligned AtomContainer
        /// </param>
        /// <param name="mappedAtoms">            			Map: a Map of the mapped atoms
        /// </param>
        /// <param name="Coords3d">           			    boolean: true if moecules has 3D coords, false if molecules has 2D coords
        /// </param>
        /// <returns>                   				double: all the RMSD of bonds length
        /// </returns>
        /// <exception cref="CDK">*
        /// 
        /// </exception>
        public static double getBondLengthRMSD(IAtomContainer firstAtomContainer, IAtomContainer secondAtomContainer, System.Collections.IDictionary mappedAtoms, bool Coords3d)
        {
            //System.out.println("**** GT getBondLengthRMSD ****");
            //UPGRADE_TODO: Method 'java.util.Map.keySet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilMapkeySet'"
            System.Collections.IEnumerator firstAtoms = new CSGraphT.SupportClass.HashSetSupport(mappedAtoms.Keys).GetEnumerator();
            IAtom centerAtomFirstMolecule = null;
            IAtom centerAtomSecondMolecule = null;
            IAtom[] connectedAtoms = null;
            double sum = 0;
            double n = 0;
            double distance1 = 0;
            double distance2 = 0;
            setVisitedFlagsToFalse(firstAtomContainer);
            setVisitedFlagsToFalse(secondAtomContainer);
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (firstAtoms.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                centerAtomFirstMolecule = firstAtomContainer.getAtomAt(((System.Int32)firstAtoms.Current));
                centerAtomFirstMolecule.setFlag(CDKConstants.VISITED, true);
                centerAtomSecondMolecule = secondAtomContainer.getAtomAt(((System.Int32)mappedAtoms[(System.Int32)firstAtomContainer.getAtomNumber(centerAtomFirstMolecule)]));
                connectedAtoms = firstAtomContainer.getConnectedAtoms(centerAtomFirstMolecule);
                for (int i = 0; i < connectedAtoms.Length; i++)
                {
                    //this step is built to know if the program has already calculate a bond length (so as not to have duplicate values)
                    if (!connectedAtoms[i].getFlag(CDKConstants.VISITED))
                    {
                        if (Coords3d)
                        {
                            distance1 = ((Point3d)centerAtomFirstMolecule.getPoint3d()).distance(connectedAtoms[i].getPoint3d());
                            distance2 = ((Point3d)centerAtomSecondMolecule.getPoint3d()).distance(secondAtomContainer.getAtomAt(((System.Int32)mappedAtoms[(System.Int32)firstAtomContainer.getAtomNumber(connectedAtoms[i])])).getPoint3d());
                            sum = sum + System.Math.Pow((distance1 - distance2), 2);
                            n++;
                        }
                        else
                        {
                            distance1 = ((Point2d)centerAtomFirstMolecule.getPoint2d()).distance(connectedAtoms[i].getPoint2d());
                            distance2 = ((Point2d)centerAtomSecondMolecule.getPoint2d()).distance(secondAtomContainer.getAtomAt(((System.Int32)mappedAtoms[(System.Int32)firstAtomContainer.getAtomNumber(connectedAtoms[i])])).getPoint2d());
                            sum = sum + System.Math.Pow((distance1 - distance2), 2);
                            n++;
                        }
                    }
                }
            }
            setVisitedFlagsToFalse(firstAtomContainer);
            setVisitedFlagsToFalse(secondAtomContainer);
            return System.Math.Sqrt(sum / n);
        }
        /// <summary>  Return the variation of each angle value between the 2 aligned molecules.
        /// 
        /// </summary>
        /// <param name="firstAtomContainer">               the (largest) first aligned AtomContainer which is the reference
        /// </param>
        /// <param name="secondAtomContainer">              the second aligned AtomContainer
        /// </param>
        /// <param name="mappedAtoms">            			Map: a Map of the mapped atoms
        /// </param>
        /// <returns>                   				double: the value of the RMSD 
        /// </returns>
        /// <exception cref="CDK">*
        /// 
        /// </exception>
        public static double getAngleRMSD(IAtomContainer firstAtomContainer, IAtomContainer secondAtomContainer, System.Collections.IDictionary mappedAtoms)
        {
            //System.out.println("**** GT getAngleRMSD ****");
            //UPGRADE_TODO: Method 'java.util.Map.keySet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilMapkeySet'"
            System.Collections.IEnumerator firstAtoms = new CSGraphT.SupportClass.HashSetSupport(mappedAtoms.Keys).GetEnumerator();
            //System.out.println("mappedAtoms:"+mappedAtoms.toString());
            IAtom firstAtomfirstAC = null;
            IAtom centerAtomfirstAC = null;
            IAtom firstAtomsecondAC = null;
            IAtom secondAtomsecondAC = null;
            IAtom centerAtomsecondAC = null;
            double angleFirstMolecule = 0;
            double angleSecondMolecule = 0;
            double sum = 0;
            double n = 0;
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (firstAtoms.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                int firstAtomNumber = ((System.Int32)firstAtoms.Current);
                centerAtomfirstAC = firstAtomContainer.getAtomAt(firstAtomNumber);
                IAtom[] connectedAtoms = firstAtomContainer.getConnectedAtoms(centerAtomfirstAC);
                if (connectedAtoms.Length > 1)
                {
                    //System.out.println("If "+centerAtomfirstAC.getSymbol()+" is the center atom :");
                    for (int i = 0; i < connectedAtoms.Length - 1; i++)
                    {
                        firstAtomfirstAC = connectedAtoms[i];
                        for (int j = i + 1; j < connectedAtoms.Length; j++)
                        {
                            angleFirstMolecule = getAngle(centerAtomfirstAC, firstAtomfirstAC, connectedAtoms[j]);
                            centerAtomsecondAC = secondAtomContainer.getAtomAt(((System.Int32)mappedAtoms[(System.Int32)firstAtomContainer.getAtomNumber(centerAtomfirstAC)]));
                            firstAtomsecondAC = secondAtomContainer.getAtomAt(((System.Int32)mappedAtoms[(System.Int32)firstAtomContainer.getAtomNumber(firstAtomfirstAC)]));
                            secondAtomsecondAC = secondAtomContainer.getAtomAt(((System.Int32)mappedAtoms[(System.Int32)firstAtomContainer.getAtomNumber(connectedAtoms[j])]));
                            angleSecondMolecule = getAngle(centerAtomsecondAC, firstAtomsecondAC, secondAtomsecondAC);
                            sum = sum + System.Math.Pow(angleFirstMolecule - angleSecondMolecule, 2);
                            n++;
                            //System.out.println("Error for the "+firstAtomfirstAC.getSymbol().toLowerCase()+"-"+centerAtomfirstAC.getSymbol()+"-"+connectedAtoms[j].getSymbol().toLowerCase()+" Angle :"+deltaAngle+" degrees");
                        }
                    }
                } //if
            }
            return System.Math.Sqrt(sum / n);
        }

        private static double getAngle(IAtom atom1, IAtom atom2, IAtom atom3)
        {

            Vector3d centerAtom = new Vector3d();
            centerAtom.x = atom1.X3d;
            centerAtom.y = atom1.Y3d;
            centerAtom.z = atom1.Z3d;
            Vector3d firstAtom = new Vector3d();
            Vector3d secondAtom = new Vector3d();

            firstAtom.x = atom2.X3d;
            firstAtom.y = atom2.Y3d;
            firstAtom.z = atom2.Z3d;

            secondAtom.x = atom3.X3d;
            secondAtom.y = atom3.Y3d;
            secondAtom.z = atom3.Z3d;

            firstAtom.sub(centerAtom);
            secondAtom.sub(centerAtom);

            return firstAtom.angle(secondAtom);
        }

        /// <summary>  Return the RMSD between the 2 aligned molecules.
        /// 
        /// </summary>
        /// <param name="firstAtomContainer">               the (largest) first aligned AtomContainer which is the reference
        /// </param>
        /// <param name="secondAtomContainer">              the second aligned AtomContainer
        /// </param>
        /// <param name="mappedAtoms">            			Map: a Map of the mapped atoms
        /// </param>
        /// <param name="Coords3d">           			    boolean: true if moecules has 3D coords, false if molecules has 2D coords
        /// </param>
        /// <returns>                   				double: the value of the RMSD 
        /// </returns>
        /// <exception cref="CDK">*
        /// 
        /// </exception>
        public static double getAllAtomRMSD(IAtomContainer firstAtomContainer, IAtomContainer secondAtomContainer, System.Collections.IDictionary mappedAtoms, bool Coords3d)
        {
            //System.out.println("**** GT getAllAtomRMSD ****");
            double sum = 0;
            double RMSD = 0;
            //UPGRADE_TODO: Method 'java.util.Map.keySet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilMapkeySet'"
            System.Collections.IEnumerator firstAtoms = new CSGraphT.SupportClass.HashSetSupport(mappedAtoms.Keys).GetEnumerator();
            int firstAtomNumber = 0;
            int secondAtomNumber = 0;
            int n = 0;
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (firstAtoms.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                firstAtomNumber = ((System.Int32)firstAtoms.Current);
                try
                {
                    secondAtomNumber = ((System.Int32)mappedAtoms[(System.Int32)firstAtomNumber]);
                    IAtom firstAtom = firstAtomContainer.getAtomAt(firstAtomNumber);
                    if (Coords3d)
                    {
                        sum = sum + System.Math.Pow(((Point3d)firstAtom.getPoint3d()).distance(secondAtomContainer.getAtomAt(secondAtomNumber).getPoint3d()), 2);
                        n++;
                    }
                    else
                    {
                        sum = sum + System.Math.Pow(((Point2d)firstAtom.getPoint2d()).distance(secondAtomContainer.getAtomAt(secondAtomNumber).getPoint2d()), 2);
                        n++;
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
            RMSD = System.Math.Sqrt(sum / n);
            return RMSD;
        }
        /// <summary>  Return the RMSD of the heavy atoms between the 2 aligned molecules.
        /// 
        /// </summary>
        /// <param name="firstAtomContainer">               the (largest) first aligned AtomContainer which is the reference
        /// </param>
        /// <param name="secondAtomContainer">              the second aligned AtomContainer
        /// </param>
        /// <param name="mappedAtoms">            			Map: a Map of the mapped atoms
        /// </param>
        /// <param name="Coords3d">           			    boolean: true if moecules has 3D coords, false if molecules has 2D coords
        /// </param>
        /// <returns>                   				double: the value of the RMSD 
        /// </returns>
        /// <exception cref="CDK">*
        /// 
        /// </exception>
        public static double getHeavyAtomRMSD(IAtomContainer firstAtomContainer, IAtomContainer secondAtomContainer, System.Collections.IDictionary mappedAtoms, bool Coords3d)
        {
            //System.out.println("**** GT getAllAtomRMSD ****");
            double sum = 0;
            double RMSD = 0;
            //UPGRADE_TODO: Method 'java.util.Map.keySet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilMapkeySet'"
            System.Collections.IEnumerator firstAtoms = new CSGraphT.SupportClass.HashSetSupport(mappedAtoms.Keys).GetEnumerator();
            int firstAtomNumber = 0;
            int secondAtomNumber = 0;
            int n = 0;
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (firstAtoms.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                firstAtomNumber = ((System.Int32)firstAtoms.Current);
                try
                {
                    secondAtomNumber = ((System.Int32)mappedAtoms[(System.Int32)firstAtomNumber]);
                    IAtom firstAtom = firstAtomContainer.getAtomAt(firstAtomNumber);
                    if (!firstAtom.Symbol.Equals("H"))
                    {
                        if (Coords3d)
                        {
                            sum = sum + System.Math.Pow(((Point3d)firstAtom.getPoint3d()).distance(secondAtomContainer.getAtomAt(secondAtomNumber).getPoint3d()), 2);
                            n++;
                        }
                        else
                        {
                            sum = sum + System.Math.Pow(((Point2d)firstAtom.getPoint2d()).distance(secondAtomContainer.getAtomAt(secondAtomNumber).getPoint2d()), 2);
                            n++;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
            RMSD = System.Math.Sqrt(sum / n);
            return RMSD;
        }
        static GeometryTools()
        {
            //logger = new LoggingTool(typeof(GeometryTools));
        }
    }
}