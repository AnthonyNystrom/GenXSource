using System;
using System.Collections.Generic;
using System.Text;

namespace UserInterface
{
    class MoleculePropertyWrapper
    {
        private double mass;
        private string formula;

        public string Formula
        {
            get { return formula; }
        }

        public double Mass
        {
            get { return mass; }
        }

    }
}
