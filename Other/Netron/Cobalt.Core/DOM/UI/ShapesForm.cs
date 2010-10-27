using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Netron.Neon.WinFormsUI;
using System.Diagnostics;
using System.Reflection;
using Netron.Diagramming.Core;
namespace Netron.Cobalt
{
    public partial class ShapesForm : DockContent
    {
        public ShapesForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ListViewItem item;
            string[] names = Enum.GetNames(typeof(ShapeTypes));
            Type type = null;
            ShapeAttribute sha = null;
            ListViewGroup group = null;
            for (int i = 0; i < names.Length; i++)
            {  

                type = typeof(ShapeTypes).Assembly.GetType("Netron.Diagramming.Core." + names[i]);
                if (type != null)
                {
                    object[] catt = type.GetCustomAttributes(false);
                    if (catt != null)
                    {
                        for (int m = 0; m < catt.Length; m++)
                        {
                            if (catt[m] is ShapeAttribute)
                            {
                                sha = catt[m] as ShapeAttribute;
                                group = GetGroup(sha.Category);                                
                                item = new ListViewItem(sha.Name);
                                item.ToolTipText = sha.Description;
                                item.Group = group;
                                item.Tag = names[i];
                                item.ImageIndex = 0;
                                this.ShapesListView.Items.Add(item);
                            }
                        }
                        
                    }
                }
            }                  
        }

        private ListViewGroup GetGroup(string name)
        {
            foreach (ListViewGroup group in this.ShapesListView.Groups)
            {
                if (group.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return group;
            }
            ListViewGroup g = new ListViewGroup(name, name);
            this.ShapesListView.Groups.Add(g);
            return g;
        }

        /// <summary>
        /// Handles the MouseDown event of the shapesListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void shapesListView_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewItem item = this.ShapesListView.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                if (item.Tag != null)
                    Application.Diagram.DoDragDrop(item.Tag.ToString(), DragDropEffects.All);
                else
                    Trace.WriteLine("The shape or entity '" + item.Text + "' does not have a tag and cannot be dropped onto the canvas.");
            }

        }
        private void shapesListView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Cursor.Current = Cursors.HSplit;
        }
    }
}