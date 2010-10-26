using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenSVisualLib.Rendering
{
    public interface IChemRenderingSource
    {
        IChemFile ChemFile
        {
            get;
        }
    }
}
