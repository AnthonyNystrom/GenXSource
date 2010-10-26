/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-07-14 06:14:13 +0000 (Fri, 14 Jul 2006) $
* $Revision: 6659 $
* 
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <summary> Class with convenience methods that provide methods from
    /// methods from ChemObjects within the ChemFile.
    /// 
    /// </summary>
    /// <seealso cref="org.openscience.cdk.AtomContainer.removeAtomAndConnectedElectronContainers(IAtom)">
    /// 
    /// </seealso>
    /// <cdk.module>  standard </cdk.module>
    public class ChemFileManipulator
    {
        public static int getAtomCount(IChemFile file)
        {
            int count = 0;
            for (int i = 0; i < file.ChemSequenceCount; i++)
            {
                count += ChemSequenceManipulator.getAtomCount(file.getChemSequence(i));
            }
            return count;
        }

        public static int getBondCount(IChemFile file)
        {
            int count = 0;
            for (int i = 0; i < file.ChemSequenceCount; i++)
            {
                count += ChemSequenceManipulator.getBondCount(file.getChemSequence(i));
            }
            return count;
        }

        /// <summary> Puts all the Molecules of this container together in one 
        /// AtomCcntainer.
        /// 
        /// </summary>
        /// <returns>  The AtomContainer with all the Molecules of this container
        /// 
        /// </returns>
        /// <deprecated> This method has a serious performace impact. Try to use
        /// other methods.
        /// </deprecated>
        public static IAtomContainer getAllInOneContainer(IChemFile file)
        {
            IAtomContainer container = file.Builder.newAtomContainer();
            for (int i = 0; i < file.ChemSequenceCount; i++)
            {
                IChemSequence sequence = file.getChemSequence(i);
                container.add(ChemSequenceManipulator.getAllInOneContainer(sequence));
            }
            return container;
        }

        /// <summary> Returns a List of all IChemObject in this ChemFile.
        /// 
        /// </summary>
        /// <returns>  A list of all ChemObjects
        /// </returns>
        public static System.Collections.IList getAllChemObjects(IChemFile file)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            list.Add(file);
            for (int i = 0; i < file.ChemSequenceCount; i++)
            {
                list.AddRange(ChemSequenceManipulator.getAllChemObjects(file.getChemSequence(i)));
            }
            return list;
        }

        /// <summary> Returns all the AtomContainer's of a ChemFile.</summary>
        public static IAtomContainer[] getAllAtomContainers(IChemFile file)
        {
            IChemSequence[] sequences = file.ChemSequences;
            int acCount = 0;
            System.Collections.ArrayList acArrays = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int i = 0; i < sequences.Length; i++)
            {
                IAtomContainer[] sequenceContainers = ChemSequenceManipulator.getAllAtomContainers(sequences[i]);
                acArrays.Add(sequenceContainers);
                acCount += sequenceContainers.Length;
            }
            IAtomContainer[] containers = new IAtomContainer[acCount];
            int arrayOffset = 0;
            //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
            for (System.Collections.IEnumerator acArraysElements = acArrays.GetEnumerator(); acArraysElements.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                IAtomContainer[] modelContainers = (IAtomContainer[])acArraysElements.Current;
                Array.Copy(modelContainers, 0, containers, arrayOffset, modelContainers.Length);
                arrayOffset += modelContainers.Length;
            }
            return containers;
        }

        public static IChemModel[] getAllChemModels(IChemFile file)
        {
            IChemSequence[] sequences = file.ChemSequences;
            int modelCounter = 0;
            int counter = 0;
            IChemModel[] tempModels = null;
            for (int f = 0; f < sequences.Length; f++)
            {
                modelCounter += sequences[f].ChemModelCount;
            }
            IChemModel[] models = new IChemModel[modelCounter];
            for (int f = 0; f < sequences.Length; f++)
            {
                tempModels = sequences[f].ChemModels;
                for (int g = 0; g < tempModels.Length; g++)
                {
                    models[counter] = tempModels[g];
                    counter++;
                }
            }
            return models;
        }
    }
}