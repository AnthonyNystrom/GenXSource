using System;
using System.Collections.Generic;
using System.Text;
using Netron.Diagramming.Core;
namespace Netron.Cobalt
{
    /// <summary>
    /// Sample plugin, see the DiagramForm.cs for usage.
    /// </summary>
    public class MyPlugin : IMouseListener
    {
        ComplexRectangle shape;
        public MyPlugin(ComplexRectangle shape)
        {
            this.shape = shape;
        }



        #region IClickListener Members

        public bool MouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            //this.shape.ShapeColor = ArtPallet.RandomColor;
            //(this.shape.Children[0] as LabelMaterial).Text = this.shape.ShapeColor.ToString();
            //foreach (IPaintable material in shape.Children)
            //{
            //    //material.;
            //}
            return false;
        }

        #endregion


        public void MouseMove(System.Windows.Forms.MouseEventArgs e)
        {

        }

        public void MouseUp(System.Windows.Forms.MouseEventArgs e)
        {

        }


    }
}
