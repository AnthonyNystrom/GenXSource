using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.IO;
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK;
using Org.OpenScience.CDK.Tools.Manipulator;
using Org.OpenScience.CDK.Geometry;
using Org.OpenScience.CDK.Config;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Chem;
using Org.OpenScience.CDK.IO.Formats;
using NuGenSVisualLib.Exceptions;
using NuGenSVisualLib.Logging;
using NuGenSVisualLib.Logging.Chem;

namespace NuGenSVisualLib.Chem
{
    public class IChemFileWrapper
    {
        internal IChemFile chemFile;
        internal MoleculeLoadingResults results;
        public int threeDCoords;
        public int twoDCoords;
        public string filename;
        internal LoadingProgress progress;
    }

    /// <summary>
    /// Facilitates the loading of molecules
    /// </summary>
    public class MoleculeLoader
    {
        private static string filterCache = null;
        public static IChemFormatMatcher[] supportedFormats;

        public enum FileUsage
        {
            None    = 0,
            TwoD    = 1,
            ThreeD  = 2
        }

        class IChemFormatMatcherComparer : IComparer<string>
        {
            #region IComparer<string> Members

            public int Compare(string x, string y)
            {
                return x.CompareTo(y);
            }

            #endregion
        }

        /// <summary>
        /// Builds a filter for an open file dialog for the supported file formats
        /// </summary>
        /// <returns></returns>
        public static string CreateOpenFilter(out int recFilterIdx, string initialDir)
        {
            // use cache if possible
            if (filterCache == null)
            {
                IChemFormatMatcher[] formats = new ReaderFactory().GetFormats();

                // sort formats alphabetically ascending
                SortedList<string, IChemFormatMatcher> sFormats = new SortedList<string, IChemFormatMatcher>(new IChemFormatMatcherComparer());
                foreach (IChemFormatMatcher format in formats)
                {
                    sFormats.Add(format.FormatName, format);
                }

                StringBuilder filter = new StringBuilder();

                int formatsInserted = 0;
                foreach (KeyValuePair<string, IChemFormatMatcher> format in sFormats)
                {
                    if (format.Value.NameExtensions.Length > 0)
                    {
                        if (formatsInserted > 0)
                            filter.Append("|");

                        filter.Append(format.Value.FormatName);
                        filter.Append(" (");
                        for (int ext = 0; ext < format.Value.NameExtensions.Length; ext++)
                        {
                            if (ext > 0)
                                filter.Append(",");
                            filter.Append("*.");
                            filter.Append(format.Value.NameExtensions[ext]);
                        }
                        filter.Append(")|");

                        for (int ext = 0; ext < format.Value.NameExtensions.Length; ext++)
                        {
                            if (ext > 0)
                                filter.Append(";");
                            filter.Append("*.");
                            filter.Append(format.Value.NameExtensions[ext]);
                        }
                        formatsInserted++;
                    }
                }

                if (formatsInserted > 0)
                    filter.Append("|");
                filter.Append("All Files (*.*)|*.*");

                supportedFormats = new IChemFormatMatcher[sFormats.Count];
                int idx = 0;
                foreach (KeyValuePair<string, IChemFormatMatcher> format in sFormats)
                {
                    supportedFormats[idx++] = format.Value;
                }

                filterCache = filter.ToString();
            }

            // find recommended filter to use for initial dir
            if (initialDir != null)
            {
                int[] hits = new int[supportedFormats.Length];
                string[] files = Directory.GetFiles(initialDir);
                foreach (string file in files)
                {
                    string ext = Path.GetExtension(file).Substring(0);
                    // match
                    for (int i = 0; i < hits.Length; i++)
                    {
                        bool matched = false;
                        foreach (string nameExt in supportedFormats[i].NameExtensions)
                        {
                            if (nameExt == ext)
                            {
                                matched = true;
                                break;
                            }
                        }
                        if (matched)
                        {
                            hits[i]++;
                            break;
                        }
                    }
                }

                // see which has highest matches
                int highestValue = 0;
                int highestIdx = 0;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i] > highestValue)
                    {
                        highestValue = hits[i];
                        highestIdx = i;
                    }
                }

                recFilterIdx = highestIdx;
            }
            else
                recFilterIdx = -1;

            return filterCache;
        }

        public static IChemFileWrapper LoadFromFile(string filename, LoadingProgress progress)
        {
            IChemFileWrapper chemFile = new IChemFileWrapper();
            chemFile.progress = progress;
            chemFile.chemFile = LoadFromFile(filename, null, FileUsage.None, chemFile.progress, out chemFile.results);
            chemFile.filename = Path.GetFileName(filename);
            return chemFile;
        }

        internal static IChemFile LoadFromFile(string filename, ISettings settings, FileUsage usage,
                                               LoadingProgress progress, out MoleculeLoadingResults results)
        {
            IChemObjectReader reader;
            results = new MoleculeLoadingResults();
            results.Filename = filename;

            MoleculeLoadingProgress molLoading = new MoleculeLoadingProgress(Path.GetFileName(filename), progress);
            progress.MoveToNextProcess(molLoading);

            try
            {
                molLoading.Log("Reading file", LogItem.ItemLevel.StageInfo);

                StreamReader file = new StreamReader(filename, System.Text.Encoding.Default);
                ReaderFactory readerFactory = new ReaderFactory();

                molLoading.Log("Creating Reader", LogItem.ItemLevel.DebugInfo);

                reader = readerFactory.createReader(filename, file);
                file.Close();
                
                if (reader != null)
                    reader.setReader(new StreamReader(filename, System.Text.Encoding.Default));
                else
                    throw new UserLevelException("Format not recognised", UserLevelException.ExceptionType.FileLoading,
                                                 typeof(MoleculeLoader), null);
                results.FileFormat = (IChemFormat)reader.Format;

                molLoading.Log("Found File Format: " + reader.Format.FormatName, LogItem.ItemLevel.UserInfo);
            }
            catch (FileNotFoundException fnfe)
            {
                throw new UserLevelException("Target file no found", UserLevelException.ExceptionType.FileLoading,
                                             typeof(MoleculeLoader), fnfe);
            }
            molLoading.Log("Loaded Molecule File", LogItem.ItemLevel.Success);

            //catch (IOException ioe)
            //{
            //    throw new UserLevelException("Problem reading from file", UserLevelException.ExceptionType.FileLoading,
            //                                 typeof(MoleculeLoader), ioe);
            //}
            //catch (UserLevelException ule)
            //{
            //    results.Result = MoleculeLoadingResults.Results.Problems;
            //    throw ule;
            //}
            //catch (Exception e)
            //{
            //    results.Result = MoleculeLoadingResults.Results.Errors;
            //    throw new UserLevelException("Problem loading file", UserLevelException.ExceptionType.FileLoading,
            //                                 typeof(MoleculeLoader), e);
            //}

            IChemFile chemFile;
            ChemModel chemModel;

            if (reader.accepts(typeof(IChemFile)))
            {
                // try to read a ChemFile
                //try
                //{
                    chemFile = (IChemFile)reader.read((IChemObject)new ChemFile());
                    molLoading.UpdateProgress(1);
                    if (chemFile != null)
                        ProcessChemFile(chemFile, settings, usage, results, progress);
                    return chemFile;
                //}
                //catch (UserLevelException ule)
                //{
                //    throw ule;
                //}
                //catch (Exception e)
                //{
                //    results.Result = MoleculeLoadingResults.Results.Errors;

                //    throw new UserLevelException("Problem reading/processing file", UserLevelException.ExceptionType.FileLoading,
                //                                 typeof(MoleculeLoader), e);
                //}
            }
            if (reader.accepts(typeof(ChemModel)))
            {
                // try to read a ChemModel
                //try
                //{
                    chemModel = (ChemModel)reader.read((IChemObject)new ChemModel());
                    molLoading.UpdateProgress(1);
                    if (chemModel != null)
                    {
                        MoleculeProcessingProgress molProcessing = new MoleculeProcessingProgress(progress);
                        progress.MoveToNextProcess(molProcessing);
                        ProcessChemModel(chemModel, settings, usage, results, molProcessing, 1);
                    }
                //}
                //catch (UserLevelException ule)
                //{
                //    throw ule;
                //}
                //catch (Exception e)
                //{
                //    throw new UserLevelException("Problem reading/processing file", UserLevelException.ExceptionType.FileLoading,
                //                                 typeof(MoleculeLoader), e);
                //}
            }
            else
                throw new UserLevelException("Unable to process reading", UserLevelException.ExceptionType.FileLoading,
                                             typeof(MoleculeLoader), null);
            return null;
        }

        private static void ProcessChemModel(IChemModel chemModel, ISettings settings, FileUsage usage,
                                             MoleculeLoadingResults results, MoleculeProcessingProgress progress,
                                             float sectionSz)
        {
            //if (ChemModelManipulator.getAllInOneContainer(chemModel).getBondCount() == 0)
            //{
            //    return;
            //}

            // check for coordinates
            if ((GeometryTools.has2DCoordinatesNew(ChemModelManipulator.getAllInOneContainer(chemModel)) != 0))//
                results.Num2DCoords++;
            //    usage == FileUsage.TwoD)
            //{
            //    throw new UserLevelException("File has no 2D coords", UserLevelException.ExceptionType.FileLoading,
            //                                 typeof(MoleculeLoader), null);
            //}
            if ((GeometryTools.has2DCoordinatesNew(ChemModelManipulator.getAllInOneContainer(chemModel)) != 0))// &&
                results.Num3DCoords++;
            //    usage == FileUsage.ThreeD)
            //{
            //    throw new UserLevelException("File has no 3D coords", UserLevelException.ExceptionType.FileLoading,
            //                                 typeof(MoleculeLoader), null);
            //}

            ElementPTFactory elements = ElementPTFactory.Instance;

            // calc item sz
            int numItems = 0;
            if (chemModel.SetOfMolecules != null)
            {
                results.NumMolecules += chemModel.SetOfMolecules.MoleculeCount;
                for (int mol = 0; mol < chemModel.SetOfMolecules.MoleculeCount; mol++)
                {
                    numItems += chemModel.SetOfMolecules.Molecules[mol].Atoms.Length;
                    //numItems += chemModel.SetOfMolecules.Molecules[mol].Bonds.Length;
                }
            }
            float itemSz = sectionSz / (float)numItems;

            if (chemModel.SetOfMolecules != null)
            {
                results.NumMolecules += chemModel.SetOfMolecules.MoleculeCount;
                for (int mol = 0; mol < chemModel.SetOfMolecules.MoleculeCount; mol++)
                {
                    IMolecule molecule = chemModel.SetOfMolecules.Molecules[mol];
                    results.NumAtoms += molecule.Atoms.Length;
                    results.NumBonds += molecule.Bonds.Length;
                    foreach (IAtom atom in molecule.Atoms)
                    {
                        PeriodicTableElement pe = elements.getElement(atom.Symbol);
                        if (pe != null)
                        {
                            atom.AtomicNumber = pe.AtomicNumber;
                            atom.Properties["PeriodicTableElement"] = pe;
                            atom.Properties["Period"] = int.Parse(pe.Period);
                        }
                        else
                            progress.Log(string.Format("Failed to find periodic element: {0}", atom.Symbol), LogItem.ItemLevel.Failure);
                        progress.UpdateProgress(itemSz);
                    }
                    progress.Log(string.Format("Processed {0} atoms", molecule.Atoms.Length), LogItem.ItemLevel.Info);
                }
            }
            progress.Log("Processed Model", LogItem.ItemLevel.Success);
        }

        private static void ProcessChemFile(IChemFile chemFile, ISettings settings, FileUsage usage,
                                            MoleculeLoadingResults results, LoadingProgress progress)
        {
            MoleculeProcessingProgress molProcessing = new MoleculeProcessingProgress(progress);
            progress.MoveToNextProcess(molProcessing);

            // calc size
            int numModels = 0;
            for (int seq = 0; seq < chemFile.ChemSequenceCount; seq++)
            {
                IChemSequence chemSeq = chemFile.getChemSequence(seq);
                numModels += chemSeq.ChemModelCount;
            }
            float modelSz = 1.0f / (float)numModels;

            results.NumSequences = chemFile.ChemSequenceCount;
            for (int seq = 0; seq < chemFile.ChemSequenceCount; seq++)
            {
                IChemSequence chemSeq = chemFile.getChemSequence(seq);
                results.NumModels += chemSeq.ChemModelCount;
                for (int model = 0; model < chemSeq.ChemModelCount; model++)
                {
                    ProcessChemModel(chemSeq.getChemModel(model), settings, usage, results, molProcessing, modelSz);
                }
                molProcessing.Log("Processed Sequence", LogItem.ItemLevel.Success);
            }
            molProcessing.Log("Processed File", LogItem.ItemLevel.Success);
        }
    }
}