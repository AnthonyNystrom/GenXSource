/* $RCSfile: SmarterJmolAdapter.java,v $
* $Author: migueljmol $
* $Date: 2005/06/05 15:51:17 $
* $Revision: 1.14 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
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
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
*  02111-1307  USA.
*/
using System;
using Org.Jmol.Api;

namespace Org.Jmol.Adapter.Smarter
{
    public class SmarterJmolAdapter : JmolAdapter
    {

        public SmarterJmolAdapter()
            : base("SmarterJmolAdapter")
        {
        }

        /* **************************************************************
        * the file related methods
        * **************************************************************/

        internal const int UNKNOWN = -1;
        internal const int XYZ = 0;
        internal const int MOL = 1;
        internal const int JME = 2;
        internal const int PDB = 3;

        public const System.String PATH_KEY = ".PATH";
        //UPGRADE_NOTE: Final was removed from the declaration of 'PATH_SEPARATOR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        //UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.IO.Path.PathSeparator.ToString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
        public static readonly System.String PATH_SEPARATOR = System.IO.Path.PathSeparator.ToString();



        public override void finish(System.Object clientFile)
        {
            ((AtomSetCollection)clientFile).finish();
        }

        public override System.Object openBufferedReader(System.String name, System.IO.StreamReader bufferedReader)
        {
            try
            {
                System.Object atomSetCollectionOrErrorMessage = Resolver.resolve(name, bufferedReader);
                if (atomSetCollectionOrErrorMessage is System.String)
                    return atomSetCollectionOrErrorMessage;
                if (atomSetCollectionOrErrorMessage is AtomSetCollection)
                {
                    AtomSetCollection atomSetCollection = (AtomSetCollection)atomSetCollectionOrErrorMessage;
                    if (atomSetCollection.errorMessage != null)
                        return atomSetCollection.errorMessage;
                    return atomSetCollection;
                }
                return "unknown reader error";
            }
            catch (System.Exception e)
            {
                //SupportClass.WriteStackTrace(e, Console.Error);
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                return "" + e;
            }
        }

        public override System.String getFileTypeName(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).fileTypeName;
        }

        public override System.String getAtomSetCollectionName(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).collectionName;
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override System.Collections.Specialized.NameValueCollection getAtomSetCollectionProperties(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).atomSetCollectionProperties;
        }

        public override int getAtomSetCount(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).atomSetCount;
        }

        public override int getAtomSetNumber(System.Object clientFile, int atomSetIndex)
        {
            return ((AtomSetCollection)clientFile).getAtomSetNumber(atomSetIndex);
        }

        public override System.String getAtomSetName(System.Object clientFile, int atomSetIndex)
        {
            return ((AtomSetCollection)clientFile).getAtomSetName(atomSetIndex);
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override System.Collections.Specialized.NameValueCollection getAtomSetProperties(System.Object clientFile, int atomSetIndex)
        {
            return ((AtomSetCollection)clientFile).getAtomSetProperties(atomSetIndex);
        }

        /* **************************************************************
        * The frame related methods
        * **************************************************************/

        public override int getEstimatedAtomCount(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).atomCount;
        }

        public override bool coordinatesAreFractional(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).coordinatesAreFractional;
        }

        public override float[] getNotionalUnitcell(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).notionalUnitcell;
        }

        public override float[] getPdbScaleMatrix(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).pdbScaleMatrix;
        }

        public override float[] getPdbScaleTranslate(System.Object clientFile)
        {
            return ((AtomSetCollection)clientFile).pdbScaleTranslate;
        }

        /*
        // not redefined for the smarterJmolAdapter, but we probably 
        // should do something similar like that. This would required
        // us to add a Properties to the Atom, I guess...
        public String getClientAtomStringProperty(Object clientAtom,
        String propertyName) {
        return null;
        }*/

        public override JmolAdapter.AtomIterator getAtomIterator(System.Object clientFile)
        {
            return new AtomIterator((AtomSetCollection)clientFile);
        }

        public override JmolAdapter.BondIterator getBondIterator(System.Object clientFile)
        {
            return new BondIterator((AtomSetCollection)clientFile);
        }

        public override JmolAdapter.StructureIterator getStructureIterator(System.Object clientFile)
        {
            AtomSetCollection atomSetCollection = (AtomSetCollection)clientFile;
            return atomSetCollection.structureCount == 0 ? null : new StructureIterator(atomSetCollection);
        }

        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AtomIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        /* **************************************************************
        * the frame iterators
        * **************************************************************/
        new internal class AtomIterator : JmolAdapter.AtomIterator
        {
            AtomSetCollection atomSetCollection;
            int iatom;
            Atom atom;

            public AtomIterator(AtomSetCollection atomSetCollection)
            {
                this.atomSetCollection = atomSetCollection;
                iatom = 0;
            }
            public override bool hasNext()
            {
                if (iatom == atomSetCollection.atomCount)
                    return false;
                atom = atomSetCollection.atoms[iatom++];
                return true;
            }
            public int getAtomSetIndex() { return atom.atomSetIndex; }
            public override object UniqueID { get { return atom; } }
            public string getElementSymbol()
            {
                if (atom.elementSymbol != null)
                    return atom.elementSymbol;
                return atom.ElementSymbol;
            }
            public int getElementNumber() { return atom.elementNumber; }
            public string getAtomName() { return atom.atomName; }
            public int getFormalCharge() { return atom.formalCharge; }
            public float getPartialCharge() { return atom.partialCharge; }
            public override float X { get { return atom.x; } }
            public override float Y { get { return atom.y; } }
            public override float Z { get { return atom.z; } }
            public float getVectorX() { return atom.vectorX; }
            public float getVectorY() { return atom.vectorY; }
            public float getVectorZ() { return atom.vectorZ; }
            public float getBfactor() { return atom.bfactor; }
            public int getOccupancy() { return atom.occupancy; }
            public bool getIsHetero() { return atom.isHetero; }
            public int getAtomSerial() { return atom.atomSerial; }
            public char getChainID() { return canonizeChainID(atom.chainID); }
            public char getAlternateLocationID()
            { return canonizeAlternateLocationID(atom.alternateLocationID); }
            public string getGroup3() { return atom.group3; }
            public int getSequenceNumber() { return atom.sequenceNumber; }
            public char getInsertionCode()
            { return canonizeInsertionCode(atom.insertionCode); }
        }

        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'BondIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        new internal class BondIterator : JmolAdapter.BondIterator
        {
            AtomSetCollection atomSetCollection;
            Atom[] atoms;
            Bond[] bonds;
            int ibond;
            Bond bond;

            public BondIterator(AtomSetCollection atomSetCollection)
            {
                this.atomSetCollection = atomSetCollection;
                atoms = atomSetCollection.atoms;
                bonds = atomSetCollection.bonds;
                ibond = 0;
            }
            public override bool hasNext()
            {
                if (ibond == atomSetCollection.bondCount)
                    return false;
                bond = bonds[ibond++];
                return true;
            }
            public override object AtomUniqueID1
            {
                get
                {
                    return atoms[bond.atomIndex1];
                }
            }
            public override object AtomUniqueID2
            {
                get
                {
                    return atoms[bond.atomIndex2];
                }
            }
            public override int EncodedOrder
            {
                get
                {
                    return bond.order;
                }
            }
        }

        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'StructureIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        new internal class StructureIterator : JmolAdapter.StructureIterator
        {
            int structureCount;
            Structure[] structures;
            Structure structure;
            int istructure;

            public StructureIterator(AtomSetCollection atomSetCollection)
            {
                structureCount = atomSetCollection.structureCount;
                structures = atomSetCollection.structures;
                istructure = 0;
            }

            public override bool hasNext()
            {
                if (istructure == structureCount)
                    return false;
                structure = structures[istructure++];
                return true;
            }

            public override string StructureType
            {
                get
                {
                    return structure.structureType;
                }
            }

            public override char StartChainID
            {
                get
                {
                    return canonizeChainID(structure.startChainID);
                }
            }

            public override int StartSequenceNumber
            {
                get
                {
                    return structure.startSequenceNumber;
                }
            }

            public override char StartInsertionCode
            {
                get
                {
                    return canonizeInsertionCode(structure.startInsertionCode);
                }
            }

            public override char EndChainID
            {
                get
                {
                    return canonizeChainID(structure.endChainID);
                }
            }

            public override int EndSequenceNumber
            {
                get
                {
                    return structure.endSequenceNumber;
                }
            }

            public override char EndInsertionCode
            {
                get
                {
                    return structure.endInsertionCode;
                }
            }
        }
    }
}