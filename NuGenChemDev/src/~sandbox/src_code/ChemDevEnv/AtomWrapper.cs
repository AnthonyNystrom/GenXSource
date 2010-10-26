using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Chem;

namespace ChemDevEnv
{
    class AtomWrapper
    {
        protected IAtom atom;
        PeriodicTableElement pe;

        public AtomWrapper(IAtom atom)
        {
            this.atom = atom;

            // rip some stats from the atom
            pe = (PeriodicTableElement)atom.Properties["PeriodicTableElement"];
        }

        public ListViewItem[] GetConnectedItemsForListView(IChemControl chemControl, ListViewGroup atomGroup, ListViewGroup bondGroup)
        {
            IChemObject[] objs = chemControl.QueryConnected(atom);
            if (objs != null && objs.Length > 0)
            {
                ListViewItem[] items = new ListViewItem[objs.Length];
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] is IAtom)
                        items[i] = new ListViewItem("atom:" + ((IAtom)objs[i]).ID, atomGroup);
                    else if (objs[i] is IBond)
                        items[i] = new ListViewItem("bond", bondGroup);
                    items[i].Tag = objs[i];
                }
                return items;
            }
            return null;
        }

        public string Symbol
        {
            get { return atom.Symbol; }
        }

        public int AtomicNumber
        {
            get { return atom.AtomicNumber; }
        }

        public string AtomicTypeName
        {
            get { return atom.AtomTypeName; }
        }

        public int MassNumber
        {
            get { return atom.MassNumber; }
        }

        public double NaturalAbundance
        {
            get { return atom.NaturalAbundance; }
        }

        public int Valency
        {
            get { return atom.Valency; }
        }

        public double VanderwaalsRadius
        {
            get { return atom.VanderwaalsRadius; }
        }

        public string ChemicalSerie
        {
            get { return pe.ChemicalSerie; }
        }

        public string Group
        {
            get { return pe.Group; }
        }

        public string ElementName
        {
            get { return pe.Name; }
        }
    }
}
