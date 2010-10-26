/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6672 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
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

namespace Org.OpenScience.CDK
{
    /// <summary>  Implements the concept of a covalent bond between two atoms. A bond is
    /// considered to be a number of electrons connecting a two of atoms.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2003-10-02 </cdk.created>
    /// <cdk.keyword>     bond </cdk.keyword>
    /// <cdk.keyword>     atom </cdk.keyword>
    /// <cdk.keyword>     electron </cdk.keyword>
    [Serializable]
    public class Bond : ElectronContainer, IBond, System.ICloneable
    {
        /// <summary>  Returns the number of Atoms in this Bond.
        /// 
        /// </summary>
        /// <returns>    The number of Atoms in this Bond
        /// </returns>
        virtual public int AtomCount
        {
            get
            {
                return atomCount;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Returns the bond order of this bond.
        /// 
        /// </summary>
        /// <returns>    The bond order of this bond
        /// </returns>
        /// <seealso cref="org.openscience.cdk.CDKConstants org.openscience.cdk.CDKConstants">
        /// for predefined values.
        /// </seealso>
        /// <seealso cref="setOrder">
        /// </seealso>
        /// <summary>  Sets the bond order of this bond.
        /// 
        /// </summary>
        /// <param name="order"> The bond order to be assigned to this bond
        /// </param>
        /// <seealso cref="org.openscience.cdk.CDKConstants">
        /// org.openscience.cdk.CDKConstants for predefined values.
        /// </seealso>
        /// <seealso cref="getOrder">
        /// </seealso>
        virtual public double Order
        {
            get
            {
                return this.order;
            }

            set
            {
                this.order = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Returns the stereo descriptor for this bond.
        /// 
        /// </summary>
        /// <returns>    The stereo descriptor for this bond
        /// </returns>
        /// <seealso cref="setStereo">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        /// <summary>  Sets the stereo descriptor for this bond.
        /// 
        /// </summary>
        /// <param name="stereo"> The stereo descriptor to be assigned to this bond.
        /// </param>
        /// <seealso cref="getStereo">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        virtual public int Stereo
        {
            get
            {
                return this.stereo;
            }

            set
            {
                this.stereo = value;
                notifyChanged();
            }

        }
        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 7057060562283387384L;

        /// <summary>  The bond order of this bond.</summary>
        protected internal double order;

        /// <summary>  Number of atoms contained by this object.</summary>
        protected internal int atomCount = 2;

        /// <summary>  A list of atoms participating in this bond.</summary>
        protected internal IAtom[] atoms;

        /// <summary>  A descriptor the stereochemical orientation of this bond.
        /// 
        /// </summary>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values to be used">
        /// here.
        /// </seealso>
        protected internal int stereo;


        /// <summary>  Constructs an empty bond.</summary>
        public Bond()
            : this(null, null, 0.0, CDKConstants.STEREO_BOND_NONE)
        {
        }


        /// <summary>  Constructs a bond with a single bond order..
        /// 
        /// </summary>
        /// <param name="atom1"> the first Atom in the bond
        /// </param>
        /// <param name="atom2"> the second Atom in the bond
        /// </param>
        public Bond(IAtom atom1, IAtom atom2)
            : this(atom1, atom2, 1.0, CDKConstants.STEREO_BOND_NONE)
        {
        }


        /// <summary>  Constructs a bond with a given order.
        /// 
        /// </summary>
        /// <param name="atom1"> the first Atom in the bond
        /// </param>
        /// <param name="atom2"> the second Atom in the bond
        /// </param>
        /// <param name="order"> the bond order
        /// </param>
        public Bond(IAtom atom1, IAtom atom2, double order)
            : this(atom1, atom2, order, CDKConstants.STEREO_BOND_NONE)
        {
        }


        /// <summary>  Constructs a bond with a given order and stereo orientation from an array
        /// of atoms.
        /// 
        /// </summary>
        /// <param name="atom1">  the first Atom in the bond
        /// </param>
        /// <param name="atom2">  the second Atom in the bond
        /// </param>
        /// <param name="order">  the bond order
        /// </param>
        /// <param name="stereo"> a descriptor the stereochemical orientation of this bond
        /// </param>
        public Bond(IAtom atom1, IAtom atom2, double order, int stereo)
        {
            atoms = new Atom[2];
            atoms[0] = atom1;
            atoms[1] = atom2;
            this.order = order;
            this.stereo = stereo;
        }


        /// <summary>  Returns the array of atoms making up this bond.
        /// 
        /// </summary>
        /// <returns>    An array of atoms participating in this bond
        /// </returns>
        /// <seealso cref="setAtoms">
        /// </seealso>
        public virtual IAtom[] getAtoms()
        {
            /*IAtom[] returnAtoms = new IAtom[AtomCount];
            Array.Copy(this.atoms, 0, returnAtoms, 0, returnAtoms.Length);
            return returnAtoms;*/
            return atoms;
        }

        /// <summary>  Sets the array of atoms making up this bond.
        /// 
        /// </summary>
        /// <param name="atoms"> An array of atoms that forms this bond
        /// </param>
        /// <seealso cref="getAtoms">
        /// </seealso>
        public virtual void setAtoms(IAtom[] atoms)
        {
            this.atoms = atoms;
            notifyChanged();
        }


        /// <summary>  Returns an Atom from this bond.
        /// 
        /// </summary>
        /// <param name="position"> The position in this bond where the atom is
        /// </param>
        /// <returns>           The atom at the specified position
        /// </returns>
        /// <seealso cref="setAtomAt">
        /// </seealso>
        public virtual IAtom getAtomAt(int position)
        {
            return (IAtom)atoms[position];
        }



        /// <summary>  Returns the atom connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the bond partner is searched of
        /// </param>
        /// <returns>       the connected atom or null
        /// </returns>
        public virtual IAtom getConnectedAtom(IAtom atom)
        {
            if (atoms[0] == atom)
            {
                return (Atom)atoms[1];
            }
            else if (atoms[1] == atom)
            {
                return (Atom)atoms[0];
            }
            return null;
        }


        /// <summary>  Returns true if the given atom participates in this bond.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be tested if it participates in this bond
        /// </param>
        /// <returns>       true if the atom participates in this bond
        /// </returns>
        public virtual bool contains(IAtom atom)
        {
            if (atoms[0] == atom)
            {
                return true;
            }
            else if (atoms[1] == atom)
            {
                return true;
            }
            return false;
        }


        /// <summary>  Sets an Atom in this bond.
        /// 
        /// </summary>
        /// <param name="atom">     The atom to be set
        /// </param>
        /// <param name="position"> The position in this bond where the atom is to be inserted
        /// </param>
        /// <seealso cref="getAtomAt">
        /// </seealso>
        public virtual void setAtomAt(IAtom atom, int position)
        {
            atoms[position] = atom;
            notifyChanged();
        }


        /// <summary>  Returns the geometric 2D center of the bond.
        /// 
        /// </summary>
        /// <returns>    The geometric 2D center of the bond
        /// </returns>
        public virtual Point2d get2DCenter()
        {
            double xOfCenter = 0;
            double yOfCenter = 0;
            for (int f = 0; f < AtomCount; f++)
            {
                xOfCenter += getAtomAt(f).X2d;
                yOfCenter += getAtomAt(f).Y2d;
            }
            return new Point2d(xOfCenter / ((double)AtomCount), yOfCenter / ((double)AtomCount));
        }



        /// <summary>  Returns the geometric 3D center of the bond.
        /// 
        /// </summary>
        /// <returns>    The geometric 3D center of the bond
        /// </returns>
        public virtual Point3d get3DCenter()
        {
            double xOfCenter = 0;
            double yOfCenter = 0;
            double zOfCenter = 0;
            for (int f = 0; f < AtomCount; f++)
            {
                xOfCenter += getAtomAt(f).X3d;
                yOfCenter += getAtomAt(f).Y3d;
                zOfCenter += getAtomAt(f).Z3d;
            }
            return new Point3d(xOfCenter / AtomCount, yOfCenter / AtomCount, zOfCenter / AtomCount);
        }

        /// <summary>  Compares a bond with this bond.
        /// 
        /// </summary>
        /// <param name="object"> Object of type Bond
        /// </param>
        /// <returns>         Return true, if the bond is equal to this bond
        /// </returns>
        public override bool compare(System.Object object_Renamed)
        {
            if (object_Renamed is IBond)
            {
                Bond bond = (Bond)object_Renamed;
                for (int i = 0; i < atoms.Length; i++)
                {
                    if (!bond.contains(atoms[i]))
                    {
                        return false;
                    }
                }

                // not important ??!!
                //if (order==bond.order)
                //  return false;

                return true;
            }
            return false;
        }


        /// <summary> Checks wether a bond is connected to another one.
        /// This can only be true if the bonds have an Atom in common.
        /// 
        /// </summary>
        /// <param name="bond"> The bond which is checked to be connect with this one
        /// </param>
        /// <returns>       True, if the bonds share an atom, otherwise false
        /// </returns>
        public virtual bool isConnectedTo(IBond bond)
        {
            for (int f = 0; f < AtomCount; f++)
            {
                if (bond.contains(getAtomAt(f)))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary> Clones this bond object, including clones of the atoms between which the
        /// bond is defined.
        /// 
        /// </summary>
        /// <returns>    The cloned object
        /// </returns>
        public override System.Object Clone()
        {
            Bond clone = (Bond)base.Clone();
            // clone all the Atoms
            if (atoms != null)
            {
                clone.atoms = new IAtom[atoms.Length];
                for (int f = 0; f < atoms.Length; f++)
                {
                    if (atoms[f] != null)
                    {
                        clone.atoms[f] = (IAtom)((IAtom)atoms[f]).Clone();
                    }
                }
            }
            return clone;
        }


        /// <summary>  Returns a one line string representation of this Container. This method is
        /// conform RFC #9.
        /// 
        /// </summary>
        /// <returns>    The string representation of this Container
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder resultString = new System.Text.StringBuilder(32);
            resultString.Append("Bond(");
            resultString.Append(this.GetHashCode());
            resultString.Append(", #O:").Append(Order);
            resultString.Append(", #S:").Append(Stereo);
            IAtom[] atoms = getAtoms();
            resultString.Append(", #A:").Append(atoms.Length);
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i] == null)
                {
                    resultString.Append(", null");
                }
                else
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    resultString.Append(", ").Append(atoms[i].ToString());
                }
            }
            resultString.Append(')');
            return resultString.ToString();
        }
    }
}