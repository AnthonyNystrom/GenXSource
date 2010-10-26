using System;
using System.Collections.Generic;
using System.Text;
using Org.Jmol.Viewer;

namespace NuGenJmol
{
    public class ChemModel
    {
        public enum ShapesType
        {
            Ribbons,
            Cartoon
        }

        public static ChemModel[] CreateNewFromCDKSequence(Org.OpenScience.CDK.Interfaces.IChemSequence sequence)
        {
            //ChemModel[] models = new ChemModel[sequence.ChemModelCount];
            //int idx = 0;
            //foreach (Org.OpenScience.CDK.Interfaces.IChemModel model in sequence.ChemModels)
            //{
            //    ChemModel cModel = models[idx++] = new ChemModel();
            //    // add frames
            //    ChemFrame[] frames = new ChemFrame[model.SetOfMolecules.MoleculeCount];
            //    int mIdx = 0;
            //    foreach (Org.OpenScience.CDK.Interfaces.IMolecule molecule in model.SetOfMolecules.Molecules)
            //    {
            //        //frames[mIdx++] = new ChemFrame(molecule);
            //    }
            //}
            return null;
        }

        public static object CreateJMOLFrameFromCDKFile(Org.OpenScience.CDK.Interfaces.IChemFile file,
                                                        Microsoft.DirectX.Direct3D.Device graphicsDevice)
        {
            return new Frame(file, graphicsDevice);
        }

        public static NuSceneBuffer3D RenderShapes(object frame, ShapesType shapes)
        {
            Frame f = (Frame)frame;
            f.Render((shapes == ShapesType.Ribbons), (shapes == ShapesType.Cartoon));
            return f.GetG3DSceneBuffer();
        }
    }
}