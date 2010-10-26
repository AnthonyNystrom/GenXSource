/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-02 13:48:44 +0200 (Sun, 02 Jul 2006) $
* $Revision: 6537 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using javax.vecmath;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Represents the idea of an chemical atom.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       egonw
    /// </author>
    /// <cdk.created>  2005-08-24 </cdk.created>
    /// <cdk.keyword>  atom </cdk.keyword>
    /// <cdk.bug>      1514483 </cdk.bug>
    public interface IAtom : IAtomType
    {
        /// <summary> Returns the x coordinate for of the 2D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the x coordinate for of the 2D location of this atom
        /// </returns>
        /// <seealso cref="setX2d">
        /// </seealso>
        /// <summary> Sets the x coordinate for of the 2D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="xCoord"> the new x coordinate for of the 2D location of this atom
        /// </param>
        /// <seealso cref="getX2d">
        /// </seealso>
        double X2d
        {
            get;
            set;
        }
        
        /// <summary> Returns the y coordinate for of the 2D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the y coordinate for of the 2D location of this atom
        /// </returns>
        /// <seealso cref="setY2d">
        /// </seealso>
        /// <summary> Sets the y coordinate for of the 2D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="yCoord"> the new y coordinate for of the 2D location of this atom
        /// </param>
        /// <seealso cref="getY2d">
        /// </seealso>
        double Y2d
        {
            get;
            set;
        }
        
        /// <summary> Returns the x coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the x coordinate for of the 3D location of this atom
        /// </returns>
        /// <seealso cref="setX3d">
        /// </seealso>
        /// <summary> Sets the x coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="xCoord"> the new x coordinate for of the 3D location of this atom
        /// </param>
        /// <seealso cref="getX3d">
        /// </seealso>
        double X3d
        {
            get;
            set;
        }
        
        /// <summary> Returns the y coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the y coordinate for of the 3D location of this atom
        /// </returns>
        /// <seealso cref="setY3d">
        /// </seealso>
        /// <summary> Sets the y coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="yCoord"> the new y coordinate for of the 3D location of this atom
        /// </param>
        /// <seealso cref="getY3d">
        /// </seealso>
        double Y3d
        {
            get;
            set;
        }
        
        /// <summary> Returns the z coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the z coordinate for of the 3D location of this atom
        /// </returns>
        /// <seealso cref="setZ3d">
        /// </seealso>
        /// <summary> Sets the z coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="zCoord"> the new z coordinate for of the 3d location of this atom
        /// </param>
        /// <seealso cref="getZ3d">
        /// </seealso>
        double Z3d
        {
            get;
            set;
        }
        
        /// <summary> Returns the x coordinate for of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <returns> the x coordinate for of the fractional coordinate of this atom.
        /// </returns>
        /// <seealso cref="setFractX3d">
        /// </seealso>
        /// <summary> Sets the x coordinate of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <param name="xFract">The x coordinate of the fractional coordinate of this atom.
        /// </param>
        /// <seealso cref="getFractX3d">
        /// </seealso>
        double FractX3d
        {
            get;
            set;
        }
        
        /// <summary> Returns the y coordinate for of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <returns> the y coordinate for of the fractional coordinate of this atom.
        /// </returns>
        /// <seealso cref="setFractY3d">
        /// </seealso>
        /// <summary> Sets the y coordinate of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <param name="yFract">The y coordinate of the fractional coordinate of this atom.
        /// </param>
        /// <seealso cref="getFractY3d">
        /// </seealso>
        double FractY3d
        {
            get;
            set;
        }
        
        /// <summary> Returns the z coordinate for of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <returns> the z coordinate for of the fractional coordinate of this atom.
        /// </returns>
        /// <seealso cref="setFractZ3d">
        /// </seealso>
        /// <summary> Sets the z coordinate of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <param name="zFract">The z coordinate of the fractional coordinate of this atom.
        /// </param>
        /// <seealso cref="getFractZ3d">
        /// </seealso>
        double FractZ3d
        {
            get;
            set;
        }

        /// <summary> Sets the partial charge of this atom.
        /// 
        /// </summary>
        /// <param name="charge"> The partial charge
        /// </param>
        /// <seealso cref="getCharge">
        /// </seealso>
        void setCharge(double charge);

        /// <summary> Returns the partial charge of this atom.
        /// 
        /// </summary>
        /// <returns> the charge of this atom
        /// </returns>
        /// <seealso cref="setCharge">
        /// </seealso>
        double getCharge();

        /// <summary> Sets the hydrogen count of this atom.
        /// 
        /// </summary>
        /// <param name="hydrogenCount"> The number of hydrogen atoms bonded to this atom.
        /// </param>
        /// <seealso cref="getHydrogenCount">
        /// </seealso>
        void setHydrogenCount(int hydrogenCount);

        /// <summary> Returns the hydrogen count of this atom.
        /// 
        /// </summary>
        /// <returns>    The hydrogen count of this atom.
        /// </returns>
        /// <seealso cref="setHydrogenCount">
        /// </seealso>
        int getHydrogenCount();

        /// <summary> Sets a point specifying the location of this
        /// atom in a 2D space.
        /// 
        /// </summary>
        /// <param name="point2d"> A point in a 2D plane
        /// </param>
        /// <seealso cref="getPoint2d">
        /// </seealso>
        void setPoint2d(Point2d point2d);

        /// <summary> Sets a point specifying the location of this
        /// atom in 3D space.
        /// 
        /// </summary>
        /// <param name="point3d"> A point in a 3-dimensional space
        /// </param>
        /// <seealso cref="getPoint3d">
        /// </seealso>
        void setPoint3d(Point3d point3d);

        /// <summary> Sets a point specifying the location of this
        /// atom in a Crystal unit cell.
        /// 
        /// </summary>
        /// <param name="point3d"> A point in a 3d fractional unit cell space
        /// </param>
        /// <seealso cref="getFractionalPoint3d">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.Crystal">
        /// </seealso>
        void setFractionalPoint3d(Point3d point3d);

        /// <summary> Sets the stereo parity for this atom.
        /// 
        /// </summary>
        /// <param name="stereoParity"> The stereo parity for this atom
        /// </param>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        /// <seealso cref="getStereoParity">
        /// </seealso>
        void setStereoParity(int stereoParity);

        /// <summary> Returns a point specifying the location of this
        /// atom in a 2D space.
        /// 
        /// </summary>
        /// <returns>    A point in a 2D plane. Null if unset.
        /// </returns>
        /// <seealso cref="setPoint2d">
        /// </seealso>
        Point2d getPoint2d();

        /// <summary> Returns a point specifying the location of this
        /// atom in a 3D space.
        /// 
        /// </summary>
        /// <returns>    A point in 3-dimensional space. Null if unset.
        /// </returns>
        /// <seealso cref="setPoint3d">
        /// </seealso>
        Point3d getPoint3d();

        /// <summary> Returns a point specifying the location of this
        /// atom in a Crystal unit cell.
        /// 
        /// </summary>
        /// <returns>    A point in 3d fractional unit cell space. Null if unset.
        /// </returns>
        /// <seealso cref="setFractionalPoint3d">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        Point3d getFractionalPoint3d();

        /// <summary> Returns the stereo parity of this atom. It uses the predefined values
        /// found in CDKConstants.
        /// 
        /// </summary>
        /// <returns>    The stereo parity for this atom
        /// </returns>
        /// <seealso cref="org.openscience.cdk.CDKConstants">
        /// </seealso>
        /// <seealso cref="setStereoParity">
        /// </seealso>
        int getStereoParity();
    }
}