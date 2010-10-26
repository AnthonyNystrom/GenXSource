/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-31 02:27:49 +0200 (ven., 31 mars 2006) $
* $Revision: 4858 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
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
using System.Collections;

namespace Org.Jmol.Viewer
{
	class Bond
	{
		public virtual bool Covalent
		{
            get { return (order & JmolConstants.BOND_COVALENT_MASK) != 0; }
		}

		public virtual bool Stereo
		{
            get { return (order & JmolConstants.BOND_STEREO_MASK) != 0; }
		}

		public virtual bool Aromatic
		{
            get { return (order & JmolConstants.BOND_AROMATIC_MASK) != 0; }
		}

		public virtual short Mad
		{
            set { this.mad = value; }
		}

		public virtual short Colix
		{
            set { this.colix = value; }
		}

		public virtual bool Translucent
		{
            set { }//colix = Graphics3D.Translucent = colix; }
		}

		public virtual short Order
		{
            get { return order; }
            set { this.order = value; }
		}

		public virtual Atom Atom1
		{
            get { return atom1; }
		}

		public virtual Atom Atom2
		{
            get { return atom2; }
		}

		public virtual float Radius
		{
            get { return mad / 2000f; }
		}

		public virtual string OrderName
		{
			get
			{
				switch (order)
				{
					case 1: 
						return "single";
					
					case 2: 
						return "double";
					
					case 3: 
						return "triple";
					
					case 4: 
						return "aromatic";
				}
				if ((order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
					return "hbond";
				return "unknown";
			}
		}

		public virtual short Colix1
		{
            get { return 0; }// Graphics3D.inheritColix(colix, atom1.colixAtom);
		}

		public virtual int Argb1
		{
			get { return 0; }//atom1.group.chain.frame.viewer.getColixArgb(Colix1);
		}

		public virtual short Colix2
		{
            get { return 0; } // Graphics3D.inheritColix(colix, atom2.colixAtom);
		}

		public virtual int Argb2
		{
            get {return 0; }// atom1.group.chain.frame.viewer.getColixArgb(Colix2);
		}

		public virtual Hashtable PublicProperties
		{
			////////////////////////////////////////////////////////////////
			get
			{
				Hashtable ht = Hashtable.Synchronized(new System.Collections.Hashtable());
				ht["atomIndexA"] = atom1.atomIndex;
				ht["atomIndexB"] = atom2.atomIndex;
				ht["argbA"] = Argb1;
				ht["argbB"] = Argb2;
				ht["order"] = OrderName;
				ht["radius"] = (double) Radius;
				ht["modelIndex"] = atom1.modelIndex;
				ht["xA"] = (double)atom1.point3f.x;
				ht["yA"] = (double)atom1.point3f.y;
				ht["zA"] = (double)atom1.point3f.z;
				ht["xB"] = (double)atom2.point3f.x;
				ht["yB"] = (double)atom2.point3f.y;
				ht["zB"] = (double)atom2.point3f.z;
				return ht;
			}
		}

        public Atom atom1;
        public Atom atom2;
        public short order;
        public short mad;
        public short colix;

        public Bond(Atom atom1, Atom atom2, short order, short mad, short colix)
		{
			if (atom1 == null)
				throw new System.NullReferenceException();
			if (atom2 == null)
				throw new System.NullReferenceException();
			this.atom1 = atom1;
			this.atom2 = atom2;
			if (atom1.elementNumber == 16 && atom2.elementNumber == 16)
				order |= JmolConstants.BOND_SULFUR_MASK;
			if (order == JmolConstants.BOND_AROMATIC_MASK)
				order = JmolConstants.BOND_AROMATIC;
			this.order = order;
			this.mad = mad;
			this.colix = colix;
		}

        public Bond(Atom atom1, Atom atom2, short order, Frame frame)
            :this(atom1, atom2, order, /*((order & JmolConstants.BOND_HYDROGEN_MASK) != 0?*/1/*:frame.viewer.MadBond)*/, (short)0)
		{ }

        public virtual void deleteAtomReferences()
		{
			if (atom1 != null)
				atom1.deleteBond(this);
			if (atom2 != null)
				atom2.deleteBond(this);
			atom1 = atom2 = null;
		}
	}
}