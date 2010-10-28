/* *****************************************************************************
 * AUTHOR       : Coskun OBA
 * EMAIL        : oba.coskun@hotmail.com
 * 
 * DATE         : JANUARY 2007
 * *****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Sci.Math;
using Sci.Physics.Interaction;
using Sci.Physics.Entity.Microscobic;

namespace SciWIN
{
    public partial class Form1 : Form
    {
        List<Particle> particles = new List<Particle>();

        Red red = (Red) ColorFactory.GetColourFactory().GetColour(ColorCharge.RED);
        Green green = (Green) ColorFactory.GetColourFactory().GetColour(ColorCharge.GREEN);
        Blue blue = (Blue) ColorFactory.GetColourFactory().GetColour(ColorCharge.BLUE);
        AntiRed antired = (AntiRed) ColorFactory.GetColourFactory().GetColour(ColorCharge.ANTIRED);
        AntiGreen antigreen = (AntiGreen) ColorFactory.GetColourFactory().GetColour(ColorCharge.ANTIGREEN);
        AntiBlue antiblue = (AntiBlue) ColorFactory.GetColourFactory().GetColour(ColorCharge.ANTIBLUE);

        public Form1()
        {
            InitializeComponent();

            // Initialize the web browser
            this.webBrowser.Navigate("about:blank");

            InitializeParticles();

            TreeNode root = new TreeNode("Subatomic Particles");
            TreeNode gaugeBosonNode = new TreeNode("Gauge Bosons");
            TreeNode leptonNode = new TreeNode("Leptons");
            TreeNode quarkNode = new TreeNode("Quarks");
            TreeNode hadronNode = new TreeNode("Hadrons"); ;
            TreeNode baryonNode = new TreeNode("Baryons");
            foreach (Particle p in particles)
            {
                if (p is IGaugeBoson)
                    gaugeBosonNode.Nodes.Add(p.Symbol);
                if (p is Lepton)
                    leptonNode.Nodes.Add(p.Symbol);
                if (p is Quark)
                    quarkNode.Nodes.Add(p.Symbol);
                if (p is Baryon)
                    baryonNode.Nodes.Add(p.Symbol);
            }
            hadronNode.Nodes.Add(baryonNode);

            root.Nodes.Add(gaugeBosonNode);
            root.Nodes.Add(leptonNode);
            root.Nodes.Add(quarkNode);
            root.Nodes.Add(hadronNode);

            treeView.Nodes.Add(root);
        }
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Particle particle = particles.Find(delegate(Particle p) { return p.Symbol == treeView.SelectedNode.Text; });
            if (particle != null)
            {   
                // DISPLAY THE FAMILY
                if (particle is GaugeBoson)
                    this.webBrowser.DocumentText = DisplayGaugeBosonsTable();
                if (particle is Lepton)
                    this.webBrowser.DocumentText = DisplayLeptonsTable();
                if (particle is Quark)
                    this.webBrowser.DocumentText = DisplayQuarksTable();
                if (particle is Baryon)
                    this.webBrowser.DocumentText = DisplayBaryonsTable();                

                // PARTICLE
                this.textBox1.Text = particle.GetType().Name.ToUpper();
                propertyGrid.SelectedObject = particle;

                // ANTI PARTICLE
                this.textBox2.Text = particle.Antiparticle.GetType().Name.ToUpper();
                propertyGridAnti.SelectedObject = particle.Antiparticle;
            }
        }
        private void InitializeParticles()
        {
            AddGaugeBosons();
            AddLeptons();
            AddQuarks();
            AddBaryons();
        }
        private void AddGaugeBosons()
        {
            // ADD PHOTON
            Photon photon = new Photon();
            particles.Add(photon);
            // ADD GLUON
            Gluon g = new Gluon();
            particles.Add(g);
            // ADD W
            W w = new W();
            particles.Add(w);
            particles.Add(w.Antiparticle);
            // ADD Z
            Z z = new Z();
            particles.Add(z);
        }
        private void AddLeptons()
        {
            // ADD ELECTRON
            Electron e = new Electron();
            particles.Add(e);
            // ADD MUON
            Muon muon = new Muon();
            particles.Add(muon);
            // ADD TAUON
            Tauon tau = new Tauon();
            particles.Add(tau);

            // ADD ELECTRON NEUTRINO
            ElectronNeutrino nu_e = new ElectronNeutrino();
            particles.Add(nu_e);
            // ADD MUON NEUTRINO
            MuonNeutrino nu_muon = new MuonNeutrino();
            particles.Add(nu_muon);
            // ADD TAUON NEUTRINO
            TauonNeutrino nu_tau = new TauonNeutrino();
            particles.Add(nu_tau);
        }
        private void AddQuarks()
        {
            particles.Add(new UpQuark(red));
            particles.Add(new DownQuark(green));
            particles.Add(new CharmQuark(green));
            particles.Add(new StrangeQuark(blue));
            particles.Add(new TopQuark(blue));
            particles.Add(new BottomQuark(red));
        }
        private void AddBaryons()
        {
            // N BARYONS
            particles.Add(new Proton());
            particles.Add(new Neutron());

            // DELTA BARYONS
            particles.Add(Delta.PLUSPLUS);
            particles.Add(Delta.PLUS);
            particles.Add(Delta.ZERO);
            particles.Add(Delta.MINUS);

            // LAMBDA BARYONS
            particles.Add(new Lambda());
            particles.Add(new BottomLambda());
            particles.Add(new CharmedLambda());

            // SIGMA BARYONS
            particles.Add(new Sigma(new DownQuark(red), new DownQuark(green), new StrangeQuark(blue)));
            particles.Add(new Sigma(new UpQuark(red), new DownQuark(green), new StrangeQuark(blue)));
            particles.Add(new Sigma(new UpQuark(red), new UpQuark(green), new StrangeQuark(blue)));

            // XI BARONS
            particles.Add(new Xi(new DownQuark(red), new StrangeQuark(green), new StrangeQuark(blue)));
            particles.Add(new Xi(new UpQuark(red), new StrangeQuark(green), new StrangeQuark(blue)));
            particles.Add(new CharmedXi(new DownQuark(red), new StrangeQuark(green), new CharmQuark(blue)));
            particles.Add(new CharmedXi(new UpQuark(red), new StrangeQuark(green), new CharmQuark(blue)));

            // OMEGA BARYONS
            particles.Add(new Omega());
            particles.Add(new CharmedOmega());
        }

        // TABLES
        private string DisplayGaugeBosonsTable()
        {
            StringBuilder str = new StringBuilder();
            str.Append("GAUGE BOSONS");
            str.Append("<TABLE BORDER=1 BGCOLOR=#CFCFCF STYLE='COLOR=#000000'>");
            str.Append("<TR ALIGN=CENTER BGCOLOR=#AAAAAA STYLE='COLOR=#FFFFFF'>");
            str.Append("<TD>NAME</TD>");
            str.Append("<TD>SYMBOL</TD>");
            str.Append("<TD>CHARGE</TD>");
            str.Append("<TD>SPIN</TD>");
            str.Append("<TD>OBSERVED</TD>");
            str.Append("<TR>");

            foreach (Particle particle in particles)
            {
                if (particle is GaugeBoson)
                {
                    GaugeBoson p = (GaugeBoson)particle;
                    str.Append("<TR ALIGN=CENTER BGCOLOR=#EAEAEA>");
                    str.Append("<TD>" + p.GetType().Name + "</TD>");
                    str.Append("<TD>" + p.Symbol + "</TD>");

                    IInteractElectromagnetic e = p as IInteractElectromagnetic;
                    if (e != null)
                    {
                        str.Append("<TD>" + e.ElectricCharge + "</TD>");
                    }
                    else
                    {
                        str.Append("<TD>0</TD>");
                    }
                    str.Append("<TD>" + p.Spin + "</TD>");
                    str.Append("<TD>" + p.Observed + "</TD>");
                    str.Append("<TR>");
                }
            }
            str.Append("</TABLE>");
            return str.ToString();
        }
        private string DisplayLeptonsTable()
        {
            StringBuilder str = new StringBuilder();
            str.Append("LEPTONS");
            str.Append("<TABLE BORDER=1 BGCOLOR=#CFCFCF STYLE='COLOR=#000000'>");
            str.Append("<TR ALIGN=CENTER BGCOLOR=#AAAAAA STYLE='COLOR=#FFFFFF'>");
            str.Append("<TD>SYMBOL</TD>");
            str.Append("<TD>NAME</TD>");
            str.Append("<TD>CHARGE</TD>");
            str.Append("<TD>T<SUB>Z</SUB></TD>");
            str.Append("<TD>Y<SUB>W</SUB></TD>");
            str.Append("<TR>");

            foreach (Particle particle in particles)
            {
                if (particle is Lepton)
                {
                    Lepton p = (Lepton)particle;
                    str.Append("<TR ALIGN=CENTER BGCOLOR=#EAEAEA>");
                    str.Append("<TD>" + p.Symbol + "</TD>");
                    str.Append("<TD>" + p.GetType().Name + "</TD>");
                    IInteractElectromagnetic e = p as IInteractElectromagnetic;
                    if (e != null)
                    {
                        str.Append("<TD>" + e.ElectricCharge + "</TD>");
                    }
                    else
                    {
                        str.Append("<TD>0</TD>");
                    }
                    str.Append("<TD>" + p.WeakIsospin + "</TD>");
                    str.Append("<TD>" + p.WeakHypercharge + "</TD>");
                    str.Append("<TR>");
                }
            }
            str.Append("</TABLE>");
            return str.ToString();
        }
        private string DisplayQuarksTable()
        {
            StringBuilder str = new StringBuilder();
            str.Append("QUARKS");
            str.Append("<TABLE BORDER=1 BGCOLOR=#CFCFCF STYLE='COLOR=#000000'>");
            str.Append("<TR ALIGN=CENTER BGCOLOR=#AAAAAA STYLE='COLOR=#FFFFFF'>");
            str.Append("<TD>SYMBOL</TD>");
            str.Append("<TD>FLAVOR</TD>");
            str.Append("<TD>SPIN</TD>");
            str.Append("<TD>CHARGE</TD>");
            str.Append("<TD>T<SUB>Z</SUB></TD>");
            str.Append("<TD>Y<SUB>W</SUB></TD>");
            str.Append("<TD>I<SUB>Z</SUB></TD>");
            str.Append("<TD>Y</TD>");
            str.Append("<TD>B</TD>");
            str.Append("<TD>C</TD>");
            str.Append("<TD>S</TD>");
            str.Append("<TD>T</TD>");
            str.Append("<TR>");

            foreach (Particle particle in particles)
            {
                if (particle is Quark)
                {
                    Quark q = (Quark)particle;
                    str.Append("<TR ALIGN=CENTER BGCOLOR=#EAEAEA>");
                    str.Append("<TD>" + q.Symbol + "</TD>");
                    str.Append("<TD>" + q.GetType().Name.Replace("Quark", "") + "</TD>");
                    str.Append("<TD>" + q.Spin + "</TD>");
                    str.Append("<TD>" + q.ElectricCharge + "</TD>");
                    str.Append("<TD>" + q.WeakIsospin + "</TD>");
                    str.Append("<TD>" + q.WeakHypercharge + "</TD>");
                    str.Append("<TD>" + q.IsospinZ + "</TD>");
                    str.Append("<TD>" + q.Hypercharge + "</TD>");
                    str.Append("<TD>" + q.Bottomness + "</TD>");
                    str.Append("<TD>" + q.Charmness + "</TD>");
                    str.Append("<TD>" + q.Strangeness + "</TD>");
                    str.Append("<TD>" + q.Topness + "</TD>");
                    str.Append("<TR>");
                }
            }
            str.Append("</TABLE>");
            return str.ToString();
        }
        private string DisplayBaryonsTable()
        {
            StringBuilder str = new StringBuilder();
            str.Append("BARYONS");
            str.Append("<TABLE BORDER=1 WIDTH=100% BGCOLOR=#CFCFCF STYLE='COLOR=#000000'>");
            str.Append("<TR ALIGN=CENTER BGCOLOR=#AAAAAA STYLE='COLOR=#FFFFFF'>");
            str.Append("<TD>NAME</TD>");
            str.Append("<TD>SYMBOL</TD>");
            str.Append("<TD>MAKEUP</TD>");
            str.Append("<TD>CHARGE</TD>");
            str.Append("<TD>SPIN</TD>");
            str.Append("<TD>B</TD>");
            str.Append("<TD>C</TD>");
            str.Append("<TD>S</TD>");
            str.Append("<TD>I<SUB>Z</SUB></TD>");
            str.Append("<TD>Y</TD>");
            str.Append("<TR>");

            foreach (Particle particle in this.particles)
            {
                if (particle is Baryon)
                {
                    Baryon p = (Baryon)particle;
                    str.Append("<TR ALIGN=CENTER BGCOLOR=#EAEAEA>");
                    str.Append("<TD>" + p.GetType().Name + "</TD>");
                    str.Append("<TD>" + p.Symbol + "</TD>");
                    str.Append("<TD>" + p.Makeup + "</TD>");

                    str.Append("<TD>" + p.ElectricCharge + "</TD>");
                    str.Append("<TD>" + p.Spin + "</TD>");
                    str.Append("<TD>" + p.Bottomness + "</TD>");
                    str.Append("<TD>" + p.Charmness + "</TD>");
                    str.Append("<TD>" + p.Strangeness + "</TD>");
                    str.Append("<TD>" + p.IsospinZ + "</TD>");
                    str.Append("<TD>" + p.Hypercharge + "</TD>");
                }
            }
            str.Append("</TABLE>");
            return str.ToString();
        }
    }
}