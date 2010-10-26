using System;
using System.Collections.Generic;
using System.Text;

namespace NuGenSVisualLib.Rendering
{
    public interface IRenderingSource
    {
        /// <summary>
        /// Called as notification of the source being modified
        /// </summary>
        void SourceModified();
    }
}
