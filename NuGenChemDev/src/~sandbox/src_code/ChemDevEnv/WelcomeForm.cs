using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace ChemDevEnv
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();

            Assembly asm = Assembly.GetExecutingAssembly();
            //label4.Text = asm.ManifestModule.GetField("AssemblyConfiguration").ToString();

            AssemblyName asmName = asm.GetName();
            label3.Text = asmName.Version.ToString();
            
            ListAsms(asm, treeView1.Nodes);
        }

        private void ListAsms(Assembly asm, TreeNodeCollection treeNodeCollection)
        {
            AssemblyName[] asms = asm.GetReferencedAssemblies();
            foreach (AssemblyName name in asms)
            {
                if (name.Name.StartsWith("NuGen"))
                {
                    TreeNode node = new TreeNode(string.Format("{0} : [{1}]", name.Name, name.Version.ToString()));
                    try
                    {
                        Assembly subAsm = Assembly.Load(name);
                        ListAsms(subAsm, node.Nodes);
                    }
                    catch { }
                    treeNodeCollection.Add(node);
                }
            }
        }
    }
}