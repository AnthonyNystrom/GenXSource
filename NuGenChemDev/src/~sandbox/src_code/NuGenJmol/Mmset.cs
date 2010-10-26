/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-02 21:10:27 +0200 (dim., 02 avr. 2006) $
* $Revision: 4881 $
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
using System.Collections.Specialized;
using System.Collections;

namespace Org.Jmol.Viewer
{
	// Mmset == Molecular Model set
	sealed class Mmset
	{
		private void InitBlock()
		{
			structures = new Structure[10];
		}

		public NameValueCollection ModelSetProperties
		{
            get { return modelSetProperties; }
            set { this.modelSetProperties = value; }
		}

        public int ModelCount
		{
            get { return modelCount; }
			set
			{
				//    System.out.println("setModelCount(" + modelCount + ")");
				if (this.modelCount != 0)
					throw new System.NullReferenceException();
				this.modelCount = value;
				models = (Model[]) Util.setLength(models, value);
				modelNames = Util.setLength(modelNames, value);
				modelNumbers = Util.setLength(modelNumbers, value);
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				modelProperties = (System.Collections.Specialized.NameValueCollection[]) Util.setLength(modelProperties, value);
			}
		}

        public Model[] Models
		{
            get { return models; }
		}

        public int ChainCount
		{
			get
			{
				int chainCount = 0;
				for (int i = modelCount; --i >= 0; )
					chainCount += models[i].ChainCount;
				return chainCount;
			}
		}

        public int PolymerCount
        {
            get
            {
                int polymerCount = 0;
                for (int i = modelCount; --i >= 0; )
                    polymerCount += models[i].PolymerCount;
                return polymerCount;
            }
        }

        public int GroupCount
		{
			get
			{
				int groupCount = 0;
				for (int i = modelCount; --i >= 0; )
					groupCount += models[i].GroupCount;
				return groupCount;
			}
		}

        public Frame frame;

        public NameValueCollection modelSetProperties;
		
		private int modelCount = 0;
		private string[] modelNames = new string[1];
		private int[] modelNumbers = new int[1];
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private NameValueCollection[] modelProperties = new NameValueCollection[1];
		private Model[] models = new Model[1];
		
		private int structureCount = 0;
		//UPGRADE_NOTE: The initialization of  'structures' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private Structure[] structures;

        public Mmset(Frame frame)
		{
			InitBlock();
            this.frame = frame;
		}

        public void defineStructure(string structureType, char startChainID, int startSequenceNumber, char startInsertionCode, char endChainID, int endSequenceNumber, char endInsertionCode)
		{
			/*
			System.out.println("Mmset.defineStructure(" + structureType + "," +
			chainID + "," +
			startSequenceNumber + "," + startInsertionCode + "," +
			endSequenceNumber + "," + endInsertionCode + ")" );
			*/
			if (structureCount == structures.Length)
				structures = (Structure[]) Util.setLength(structures, structureCount + 10);
			structures[structureCount++] = new Structure(structureType, startChainID, Group.getSeqcode(startSequenceNumber, startInsertionCode), endChainID, Group.getSeqcode(endSequenceNumber, endInsertionCode));
		}

        public void calculateStructures()
		{
			//    System.out.println("Mmset.calculateStructures()");
			for (int i = modelCount; --i >= 0; )
				models[i].calculateStructures();
		}

        public void freeze()
		{
			//    System.out.println("Mmset.freeze() modelCount=" + modelCount);
			for (int i = modelCount; --i >= 0; )
			{
				//      System.out.println(" model " + i);
				models[i].freeze();
			}
			propogateSecondaryStructure();
		}

        public string getModelSetProperty(string propertyName)
		{
			return (modelSetProperties == null?null:modelSetProperties.Get(propertyName));
		}

        public string getModelName(int modelIndex)
		{
			return modelNames[modelIndex];
		}

        public int getModelNumber(int modelIndex)
		{
			return modelNumbers[modelIndex];
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public NameValueCollection getModelProperties(int modelIndex)
		{
			return modelProperties[modelIndex];
		}

        public string getModelProperty(int modelIndex, string property)
		{
			NameValueCollection props = modelProperties[modelIndex];
			return props == null?null:props.Get(property);
		}

        public Model getModel(int modelIndex)
		{
			return models[modelIndex];
		}

        public int getModelNumberIndex(int modelNumber)
		{
			int i;
			for (i = modelCount; --i >= 0 && modelNumbers[i] != modelNumber; )
			{
			}
			return i;
		}
		
		public void setModelNameNumberProperties(int modelIndex, string modelName, int modelNumber, NameValueCollection modelProperties)
		{
			modelNames[modelIndex] = modelName;
			modelNumbers[modelIndex] = modelNumber;
			this.modelProperties[modelIndex] = modelProperties;
			models[modelIndex] = new Model(this, modelIndex, modelName);
		}
		
		private void  propogateSecondaryStructure()
		{
			for (int i = structureCount; --i >= 0; )
			{
				Structure structure = structures[i];
				for (int j = modelCount; --j >= 0; )
					models[j].addSecondaryStructure(structure.type, structure.startChainID, structure.startSeqcode, structure.endChainID, structure.endSeqcode);
			}
		}

        public int getPolymerCountInModel(int modelIndex)
		{
			return models[modelIndex].PolymerCount;
		}

        public Polymer getPolymerAt(int modelIndex, int polymerIndex)
		{
			return models[modelIndex].getPolymer(polymerIndex);
		}

        public void calcSelectedGroupsCount(BitArray bsSelected)
		{
			for (int i = modelCount; --i >= 0; )
				models[i].calcSelectedGroupsCount(bsSelected);
		}

        public void calcSelectedMonomersCount(BitArray bsSelected)
		{
			for (int i = modelCount; --i >= 0; )
				models[i].calcSelectedMonomersCount(bsSelected);
		}

        public void calcHydrogenBonds(System.Collections.BitArray bsA, BitArray bsB)
		{
			for (int i = modelCount; --i >= 0; )
				models[i].calcHydrogenBonds(bsA, bsB);
		}

        public void selectSeqcodeRange(int seqcodeA, int seqcodeB, BitArray bs)
		{
			for (int i = modelCount; --i >= 0; )
				models[i].selectSeqcodeRange(seqcodeA, seqcodeB, bs);
		}

        public class Structure
		{
            public string typeName;
            public sbyte type;
            public char startChainID;
            public int startSeqcode;
            public char endChainID;
            public int endSeqcode;

            public Structure(string typeName, char startChainID, int startSeqcode, char endChainID, int endSeqcode)
			{
				this.typeName = typeName;
				this.startChainID = startChainID;
				this.startSeqcode = startSeqcode;
				this.endChainID = endChainID;
				this.endSeqcode = endSeqcode;
				if ("helix".Equals(typeName))
					type = JmolConstants.PROTEIN_STRUCTURE_HELIX;
				else if ("sheet".Equals(typeName))
					type = JmolConstants.PROTEIN_STRUCTURE_SHEET;
				else if ("turn".Equals(typeName))
					type = JmolConstants.PROTEIN_STRUCTURE_TURN;
				else
					type = JmolConstants.PROTEIN_STRUCTURE_NONE;
			}
		}
	}
}