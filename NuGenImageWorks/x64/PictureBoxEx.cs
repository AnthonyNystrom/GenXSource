using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Genetibase.UI.NuGenImageWorks
{
    public delegate void ImageChangedEventHandler(object sender, EventArgs e);

    public class PictureBoxEx:System.Windows.Forms.PictureBox
    {

        [Category("Property Changed")]
        public event ImageChangedEventHandler ImageChanged;

        public new Image Image
        {
            get
            {
                return base.Image;
            }

            set
            {
                if (value != null)
                {
                    base.Image = value;

                    if (ImageChanged != null)
                    {
                        ImageChanged(this, EventArgs.Empty); 
                    }
                }
            }
        }
    }
}
