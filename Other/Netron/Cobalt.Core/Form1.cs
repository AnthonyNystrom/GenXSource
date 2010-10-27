using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Netron.Diagramming.Core;
namespace Netron.Cobalt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ClassShape cls1 = new ClassShape();
            cls1.HeadColor = Color.SteelBlue;
            cls1.Location = new Point(100, 10);
            cls1.BodyType = BodyType.None;
            cls1.Text = "What is this?";
            cls1.Title = "This is the title";
            cls1.SubTitle = "The subtitle";
            this.diagramControl1.AddShape(cls1);

            ClassShape cls2 = new ClassShape();
            cls2.HeadColor = Color.SlateGray;
            cls2.Location = new Point(100, 10);
            cls2.BodyType = BodyType.None;
            cls2.Text = "What is this?";
            cls2.Title = "This is the title";
            cls2.SubTitle = "The subtitle";
            this.diagramControl1.AddShape(cls2);

            this.diagramControl1.AddConnection(cls1.Connectors[0], cls2.Connectors[0]);


            
        }
    }
}