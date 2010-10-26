using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.IO.Formats;

namespace NuGenSVisualLib
{
    class MoleculeLoadingResults
    {
        public enum Results
        {
            Success,
            Warnings,
            Problems,
            Errors
        }

        string filename;
        Results result;
        IChemFormat fileFormat;
        int numSequences;
        int numModels;
        int numMolecules;
        int numAtoms;
        int numBonds;

        int num2DCoords;
        int num3DCoords;

        public int Num2DCoords
        {
            get { return num2DCoords; }
            set { num2DCoords = value; }
        }

        public int Num3DCoords
        {
            get { return num3DCoords; }
            set { num3DCoords = value; }
        }

        public int NumBonds
        {
            get { return numBonds; }
            set { numBonds = value; }
        }

        public int NumAtoms
        {
            get { return numAtoms; }
            set { numAtoms = value; }
        }

        public int NumMolecules
        {
            get { return numMolecules; }
            set { numMolecules = value; }
        }

        public int NumModels
        {
            get { return numModels; }
            set { numModels = value; }
        }

        public int NumSequences
        {
            get { return numSequences; }
            set { numSequences = value; }
        }

        public IChemFormat FileFormat
        {
            get { return fileFormat; }
            set { fileFormat = value; }
        }

        public Results Result
        {
            get { return result; }
            set { result = value; }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        
    }
}