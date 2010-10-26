using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DEffectLib;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DEffect effect = DEffect.LoadEffect("../../../DEffectLib/Example.xml");
            string efxCode = effect.ProduceEffectForTechnique("metaball");
        }
    }
}