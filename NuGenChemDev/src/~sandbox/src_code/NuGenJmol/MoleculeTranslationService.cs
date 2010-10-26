using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;
using Org.Jmol.Viewer;
using System.Collections;
using Org.OpenScience.CDK;

namespace NuGenJmol
{
    /// <summary>
    /// Should be used to get hard rendering structures from CDK molecules?
    /// </summary>
    class MoleculeTranslationService
    {
        public Org.Jmol.Viewer.Polymer TranslateCDKPolymer(Org.OpenScience.CDK.Polymer polymer)
        {
            // translate all monomers
            ICollection monomers = polymer.MonomerNames;
            Group[] groups = new Group[monomers.Count];

            IEnumerator iter = monomers.GetEnumerator();
            int idx = 0;
            while (iter.MoveNext())
            {
                groups[idx++] = TranslateCDKMonomer((Org.OpenScience.CDK.Interfaces.IMonomer)iter.Current);
            }

            Org.Jmol.Viewer.Polymer lPolymer = Org.Jmol.Viewer.Polymer.allocatePolymer(groups, 0);
            return lPolymer;
        }

        private Group TranslateCDKMonomer(IMonomer iMonomer)
        {
            // find type
            Type cdkType = iMonomer.GetType();
            if (cdkType is AminoAcid)
            {
                AminoAcid amino = (AminoAcid)iMonomer;
                
                //AminoMonomer monomer = new AminoMonomer();
            }
            return null;
        }
    }
}