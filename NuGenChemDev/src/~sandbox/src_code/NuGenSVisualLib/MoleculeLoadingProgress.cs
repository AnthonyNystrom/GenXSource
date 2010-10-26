using System;
using System.Collections.Generic;
using System.Text;

namespace NuGenSVisualLib.Logging.Chem
{
    class MoleculeLoadingProgress : ProcessLoadingProgress
    {
        protected string molName;

        public MoleculeLoadingProgress(string molName, LoadingProgress loadingProgress)
            : base("Molecule Loading", loadingProgress)
        {
            this.molName = molName;
        }
    }

    class MoleculeProcessingProgress : ProcessLoadingProgress
    {
        public MoleculeProcessingProgress(LoadingProgress loadingProgress)
            : base("Molecule Processing", loadingProgress)
        {
        }
    }
}
