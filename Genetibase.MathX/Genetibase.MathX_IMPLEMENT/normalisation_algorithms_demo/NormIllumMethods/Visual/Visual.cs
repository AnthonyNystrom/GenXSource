using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using NormIllumMethods.Techniques;
using ComparisonGreyScaleImage;

namespace NormIllumMethods.Visual
{
    public partial class Visual : Form
    {

        Bitmap bitmap;//image loaded

        public Visual()
        {
            InitializeComponent();
        }

        #region File Options
        // Menu File/OpenImage
        private void mnuOpenImage_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        // Menu File/SaveImage
        private void mnuSaveImage_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        // Menu  File/Exit
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        // Shortcut Open 
        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        // Shortcut Save
        private void toolStripSave_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        /// <summary>
        /// Open an image
        /// </summary>
        public void OpenImage()
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                bitmap = (Bitmap)Image.FromFile(ofd.FileName);
                if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                    bitmap = BitmapGrey.GrayScale8bpp(bitmap);
                picOriginal.Image = bitmap;
                SetVisibleMethods();
            }
        }

        /// <summary>
        /// Save the image
        /// </summary>
        public void SaveImage()
        {   
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap copy = (Bitmap)picFiltered.Image.Clone();
                switch (sfd.FilterIndex)
                {
                    case 1:
                        copy.Save(sfd.FileName, ImageFormat.Bmp);
                        break;

                    case 2:
                        copy.Save(sfd.FileName, ImageFormat.Gif);
                        break;

                    case 3:
                        copy.Save(sfd.FileName, ImageFormat.Jpeg);
                        break;

                    case 4:
                        copy.Save(sfd.FileName, ImageFormat.Png);
                        break;
                }
            }
        }

        /// <summary>
        /// Close application
        /// </summary>
        public void Exit()
        {
            this.Close();
        }

        /// <summary>
        /// Set visible the methods to apply to the image
        /// </summary>
        private void SetVisibleMethods()
        {
            mnuMethods.Visible =      //Enable the menu Methods in main window
            labInitial.Visible = true;//View the initial label
        }

        /// <summary>
        /// Set visible the options to save the image filtered
        /// </summary>
        private void SetVisibleSave()
        {
            mnuSaveImage.Visible =       //Enable the menu SaveImage in main window
            toolStripSave.Visible =      //Enable the shortcut SaveImage in main window
            labNormalised.Visible = true;//View the image normalised
        }

        #endregion 

        #region Normalisation Algorithms
        // Menu Methods/MultiscaleRetinex
        private void mnuMultiscaleRetinex_Click(object sender, EventArgs e)
        {
            MultiscaleRetinex();
        }

        // Menu Methods/Isotrophic
        private void mnuIsotropic_Click(object sender, EventArgs e)
        {
            Isotropic();
        }

        // Menu Methods/Anisotrophic
        private void mnuAnisotropic_Click(object sender, EventArgs e)
        {
            Anisotropic();
        }

        //Context Menu/Anisotropic Smoothing
        private void contMnuAnisotropic_Click(object sender, EventArgs e)
        {
            Anisotropic();
        }

        //Context Menu/Multiscale Retinex
        private void contMnuMultRetinex_Click(object sender, EventArgs e)
        {
            MultiscaleRetinex();
        }

        //Context Menu/Isotropic Smoothing
        private void contMnuIsotropic_Click(object sender, EventArgs e)
        {
            Isotropic();
        }
       
        /// <summary>
        /// Apply Multiscale Retinex Algorithm
        /// </summary>
        public void MultiscaleRetinex() 
        {
            RetinexParameter param = new RetinexParameter();
            param.ShowDialog();
            if (param.OK)
            {
                MultiscaleRetinex retinex = new MultiscaleRetinex(param.Sigmas, param.Widths, param.FilterSize);
                picFiltered.Image = retinex.Apply((Bitmap)bitmap.Clone());
                SetVisibleSave();
            }
        }

        /// <summary>
        /// Apply Isotropic Smoothing Algorithm
        /// </summary>
        public void Isotropic() 
        {
            IsoAniParameter param = new IsoAniParameter();
            param.ShowDialog();
            if (param.OK)
            {
                IsotropicSmoothing iso = new IsotropicSmoothing(param.Value);
                picFiltered.Image = iso.Apply((Bitmap)bitmap.Clone());
                SetVisibleSave();
            }
        }

        /// <summary>
        /// Apply Anisotropic Smoothing Algorithm
        /// </summary>
        public void Anisotropic() 
        {
            IsoAniParameter param = new IsoAniParameter();
            param.ShowDialog();
            if (param.OK)
            {
                AnisotropicSmoothing anis = new AnisotropicSmoothing(param.Value);
                picFiltered.Image = anis.Apply((Bitmap)bitmap.Clone());
                SetVisibleSave();
            }
        }
        #endregion 
    }
}