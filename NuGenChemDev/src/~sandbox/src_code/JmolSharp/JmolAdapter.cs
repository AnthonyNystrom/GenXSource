/* $RCSfile: JmolAdapter.java,v $
* $Author: migueljmol $
* $Date: 2005/06/05 15:51:17 $
* $Revision: 1.19 $
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
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
*  02111-1307  USA.
*/
using System;

namespace Org.Jmol.Api
{
    /// <summary>*************************************************************
    /// The JmolAdapter interface defines the API used by the JmolViewer to
    /// read external files and fetch atom properties necessary for rendering.
    /// 
    /// A client of the JmolViewer implements this interface on top of their
    /// existing molecular model representation. The JmolViewer then requests
    /// information from the implementation using this API. 
    /// 
    /// Jmol will automatically calculate some atom properties if the client
    /// is not capable or does not want to supply them.
    /// 
    /// Note: If you are seeing pink atoms that have lots of bonds, then your
    /// methods for getElementNumber(clientAtom) or getElementSymbol(clientAtom)
    /// are probably returning stray values. Therefore, these atoms are getting
    /// mapped to element 0 (Xx), which has color pink and a relatively large
    /// covalent bonding radius. 
    /// </summary>
    /// <seealso cref="org.jmol.api.JmolViewer">
    /// **************************************************************
    /// </seealso>
    public abstract class JmolAdapter
    {
        public const short ORDER_COVALENT_SINGLE = 1;
        public const short ORDER_COVALENT_DOUBLE = 2;
        public const short ORDER_COVALENT_TRIPLE = 3;
        public const short ORDER_AROMATIC = (short)((1 << 2));
        public const short ORDER_HBOND = (short)((1 << 6));
        public const short ORDER_STEREO_NEAR = (short)(((1 << 3) | 1));
        public const short ORDER_STEREO_FAR = (short)(((2 << 3) | 2));
        public const short ORDER_PARTIAL01 = (short)((1 << 10));
        public const short ORDER_PARTIAL12 = (short)((1 << 11));

        //////////////////////////////////////////////////////////////////
        // file related
        //////////////////////////////////////////////////////////////////


        internal System.String adapterName;
        //public Logger //logger;

        public JmolAdapter(System.String adapterName)//, Logger //logger)
        {
            this.adapterName = adapterName;
            //this.//logger = (//logger == null?new Logger(this)://logger);
        }

        /// <summary> Associate a clientFile object with a bufferedReader.
        /// 
        /// <p>Given the BufferedReader, return an object which represents the file
        /// contents. The parameter <code>name</code> is assumed to be the
        /// file name or URL which is the source of reader. Note that this 'file'
        /// may have been automatically decompressed. Also note that the name
        /// may be 'String', representing a string constant. Therefore, few
        /// assumptions should be made about the <code>name</code> parameter.
        /// 
        /// The return value is an object which represents a <code>clientFile</code>.
        /// This <code>clientFile</code> will be passed back in to other methods.
        /// If the return value is <code>instanceof String</code> then it is
        /// considered an error condition and the returned String is the error
        /// message. 
        /// 
        /// </summary>
        /// <param name="name">File name, String or URL acting as the source of the reader
        /// </param>
        /// <param name="bufferedReader">The BufferedReader
        /// </param>
        /// <returns> The clientFile or String with an error message
        /// </returns>
        public virtual System.Object openBufferedReader(System.String name, System.IO.StreamReader bufferedReader)
        {
            return openBufferedReader(name, bufferedReader);
        }

        /// <param name="name">File name, String or URL acting as the source of the reader
        /// </param>
        /// <param name="bufferedReader">The BufferedReader
        /// </param>
        /// <param name="//logger">The //logger
        /// </param>
        /// <returns> The clientFile or String with an error message
        /// </returns>
        /// <seealso cref="openBufferedReader(String, BufferedReader)">
        /// </seealso>
        //public virtual System.Object openBufferedReader(System.String name, System.IO.StreamReader bufferedReader)//, Logger //logger)
        //{
        //    return null;
        //}

        public virtual void finish(System.Object clientFile)
        {
        }

        /// <summary> Get the type of this file or molecular model, if known.</summary>
        /// <param name="clientFile"> The client file
        /// </param>
        /// <returns> The type of this file or molecular model, default
        /// <code>"unknown"</code>
        /// </returns>
        public virtual System.String getFileTypeName(System.Object clientFile)
        {
            return "unknown";
        }

        /// <summary> Get the name of the atom set collection, if known.
        /// 
        /// <p>Some file formats contain a formal name of the molecule in the file.
        /// If this method returns <code>null</code> then the JmolViewer will
        /// automatically supply the file/URL name as a default.
        /// </summary>
        /// <param name="clientFile">
        /// </param>
        /// <returns> The atom set collection name or <code>null</code>
        /// </returns>
        public virtual System.String getAtomSetCollectionName(System.Object clientFile)
        {
            return null;
        }

        /// <summary> Get the properties for this atomSetCollection.
        /// 
        /// <p>Not yet implemented everywhere, it is in the smarterJmolAdapter
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> The properties for this atomSetCollection or <code>null</code>
        /// </returns>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public virtual System.Collections.Specialized.NameValueCollection getAtomSetCollectionProperties(System.Object clientFile)
        {
            return null;
        }

        /// <summary> Get number of atomSets in the file.
        /// 
        /// <p>NOTE WARNING:
        /// <br>Not yet implemented everywhere, it is in the smarterJmolAdapter
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> The number of atomSets in the file, default 1
        /// </returns>
        public virtual int getAtomSetCount(System.Object clientFile)
        {
            return 1;
        }

        /// <summary> Get the number identifying each atomSet.
        /// 
        /// <p>For a PDB file, this is is the model number. For others it is
        /// a 1-based atomSet number.
        /// <p>
        /// <i>Note that this is not currently implemented in PdbReader</i>
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <param name="atomSetIndex">The atom set's index for which to get
        /// the atom set number
        /// </param>
        /// <returns> The number identifying each atom set, default atomSetIndex+1.
        /// </returns>
        public virtual int getAtomSetNumber(System.Object clientFile, int atomSetIndex)
        {
            return atomSetIndex + 1;
        }

        /// <summary> Get the name of an atomSet.
        /// 
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <param name="atomSetIndex">The atom set index
        /// </param>
        /// <returns> The name of the atom set, default the string representation
        /// of atomSetIndex
        /// </returns>
        public virtual System.String getAtomSetName(System.Object clientFile, int atomSetIndex)
        {
            return "" + getAtomSetNumber(clientFile, atomSetIndex);
        }

        /// <summary> Get the properties for an atomSet.
        /// 
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <param name="atomSetIndex">The atom set index
        /// </param>
        /// <returns> The properties for an atom set or <code>null</code>
        /// </returns>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public virtual System.Collections.Specialized.NameValueCollection getAtomSetProperties(System.Object clientFile, int atomSetIndex)
        {
            return null;
        }

        /// <summary> Get the estimated number of atoms contained in the file.
        /// 
        /// <p>Just return -1 if you don't know (or don't want to figure it out)
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> The estimated number of atoms in the file
        /// </returns>
        abstract public int getEstimatedAtomCount(System.Object clientFile);


        /// <summary> Get the boolean whether coordinates are fractional.</summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> true if the coordinates are fractional, default <code>false</code>
        /// </returns>
        public virtual bool coordinatesAreFractional(System.Object clientFile)
        {
            return false;
        }

        /// <summary> Get the notional unit cell.
        /// 
        /// <p>This method returns the parameters that define a crystal unitcell
        /// the parameters are returned in a float[] in the following order
        /// <code>a, b, c, alpha, beta, gamma</code>
        /// <br><code>a, b, c</code> : angstroms
        /// <br><code>alpha, beta, gamma</code> : degrees
        /// <br>if there is no unit cell data then return null
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> The array of the values or <code>null</code>
        /// </returns>
        public virtual float[] getNotionalUnitcell(System.Object clientFile)
        {
            return null;
        }

        /// <summary> Get the PDB scale matrix.
        /// 
        /// <p>Does not seem to be overriden by any descendent
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> The array of 9 floats for the matrix or <code>null</code>
        /// </returns>
        public virtual float[] getPdbScaleMatrix(System.Object clientFile)
        {
            return null;
        }

        /// <summary> Get the PDB scale translation vector.
        /// <p>Does not seem to be overriden by any descendent
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> The x, y and z translation values or <code>null</code>
        /// </returns>
        public virtual float[] getPdbScaleTranslate(System.Object clientFile)
        {
            return null;
        }

        /// <summary> Get a property from a clientAtom.
        /// 
        /// </summary>
        /// <param name="clientAtom">The clientAtom
        /// </param>
        /// <param name="propertyName">the key of the property
        /// </param>
        /// <returns> The value of the property
        /// </returns>
        public virtual System.String getClientAtomStringProperty(System.Object clientAtom, System.String propertyName)
        {
            return null;
        }

        /// <summary> Get an AtomIterator for retrieval of all atoms in the file.
        /// 
        /// <p>This method may not return <code>null</code>.
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> An AtomIterator
        /// </returns>
        /// <seealso cref="AtomIterator">
        /// </seealso>
        abstract public AtomIterator getAtomIterator(System.Object clientFile);
        /// <summary> Get a BondIterator for retrieval of all bonds in the file.
        /// 
        /// <p>If this method returns <code>null</code> and no
        /// bonds are defined then the JmolViewer will automatically apply its
        /// rebonding code to build bonds between atoms.
        /// </summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> A BondIterator or <code>null</code>
        /// </returns>
        /// <seealso cref="BondIterator">
        /// </seealso>
        public virtual BondIterator getBondIterator(System.Object clientFile)
        {
            return null;
        }

        /// <summary> Get a StructureIterator.</summary>
        /// <param name="clientFile">The client file
        /// </param>
        /// <returns> A StructureIterator or <code>null</code>
        /// </returns>

        public virtual StructureIterator getStructureIterator(System.Object clientFile)
        {
            return null;
        }

        /// <summary>*************************************************************
        /// AtomIterator is used to enumerate all the <code>clientAtom</code>
        /// objects in a specified frame. 
        /// Note that Java 1.1 does not have java.util.Iterator
        /// so we will define our own AtomIterator
        /// **************************************************************
        /// </summary>
        public abstract class AtomIterator
        {
            virtual public int AtomSetIndex
            {
                get
                {
                    return 0;
                }

            }
            abstract public System.Object UniqueID { get;}
            virtual public int ElementNumber
            {
                get
                {
                    return -1;
                }

            }
            virtual public System.String ElementSymbol
            {
                get
                {
                    return null;
                }

            }
            virtual public System.String AtomName
            {
                get
                {
                    return null;
                }

            }
            virtual public int FormalCharge
            {
                get
                {
                    return 0;
                }

            }
            virtual public float PartialCharge
            {
                get
                {
                    return System.Single.NaN;
                }

            }
            abstract public float X { get;}
            abstract public float Y { get;}
            abstract public float Z { get;}
            virtual public float VectorX
            {
                get
                {
                    return System.Single.NaN;
                }

            }
            virtual public float VectorY
            {
                get
                {
                    return System.Single.NaN;
                }

            }
            virtual public float VectorZ
            {
                get
                {
                    return System.Single.NaN;
                }

            }
            virtual public float Bfactor
            {
                get
                {
                    return System.Single.NaN;
                }

            }
            virtual public int Occupancy
            {
                get
                {
                    return 100;
                }

            }
            virtual public bool IsHetero
            {
                get
                {
                    return false;
                }

            }
            virtual public int AtomSerial
            {
                get
                {
                    return System.Int32.MinValue;
                }

            }
            virtual public char ChainID
            {
                get
                {
                    return (char)0;
                }

            }
            virtual public char AlternateLocationID
            {
                get
                {
                    return (char)0;
                }

            }
            virtual public System.String Group3
            {
                get
                {
                    return null;
                }

            }
            virtual public int SequenceNumber
            {
                get
                {
                    return System.Int32.MinValue;
                }

            }
            virtual public char InsertionCode
            {
                get
                {
                    return (char)0;
                }

            }
            virtual public System.Object ClientAtomReference
            {
                get
                {
                    return null;
                }

            }
            public abstract bool hasNext();
        }

        /// <summary>*************************************************************
        /// BondIterator is used to enumerate all the bonds
        /// **************************************************************
        /// </summary>

        public abstract class BondIterator
        {
            public abstract System.Object AtomUniqueID1 { get;}
            public abstract System.Object AtomUniqueID2 { get;}
            public abstract int EncodedOrder { get;}
            public abstract bool hasNext();
        }

        /// <summary>*************************************************************
        /// StructureIterator is used to enumerate Structures
        /// Helix, Sheet, Turn
        /// **************************************************************
        /// </summary>

        public abstract class StructureIterator
        {
            public abstract System.String StructureType { get;}
            public abstract char StartChainID { get;}
            public abstract int StartSequenceNumber { get;}
            public abstract char StartInsertionCode { get;}
            public abstract char EndChainID { get;}
            public abstract int EndSequenceNumber { get;}
            public abstract char EndInsertionCode { get;}
            public abstract bool hasNext();
        }

        //////////////////////////////////////////////////////////////////
        // range-checking routines
        /////////////////////////////////////////////////////////////////

        public static char canonizeAlphaDigit(char ch)
        {
            if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9'))
                return ch;
            return '\x0000';
        }

        public static char canonizeChainID(char chainID)
        {
            return canonizeAlphaDigit(chainID);
        }

        public static char canonizeInsertionCode(char insertionCode)
        {
            return canonizeAlphaDigit(insertionCode);
        }

        public static char canonizeAlternateLocationID(char altLoc)
        {
            // pdb altLoc
            return canonizeAlphaDigit(altLoc);
        }
    }
}