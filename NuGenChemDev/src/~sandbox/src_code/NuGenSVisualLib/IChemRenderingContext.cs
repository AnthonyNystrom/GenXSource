using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Chem.Structures;

namespace NuGenSVisualLib.Rendering
{
    /// <summary>
    /// Encapsulates the rendering of chemistry specific source material
    /// </summary>
    public interface IChemRenderingContext : IRenderingContext
    {
        ChemEntity TrySelectAtPoint(int x, int y);
    }
}
