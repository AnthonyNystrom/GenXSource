using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Genetibase.UI.NuGenImageWorks.WaitCursor;
using Genetibase.UI.FileSaveDialogEx;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class PhotoMenu : RibbonPopup
    {
        private bool cancelclose;
        private Form parent = null;

        public PhotoMenu(Form parent)
        {
            InitializeComponent();

            this.parent = parent;
        }

        

        private void PhotoMenu_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(115, 115, 115)), 0, 0, this.Width - 1, this.Height - 1);
        }
        //Document _psd = null;
        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.cancelclose = true;

                if (this.openFileDialog1.ShowDialog(this.parent) == DialogResult.OK)
                {
                    OpenFile(this.openFileDialog1.FileName);
                }
                this.Close();
            }
            catch { }
        }

        private void OpenFile(string FileName)
        {
            this.Hide();
            Application.DoEvents();
            //((MainForm)this.parent).Clear();               
            String ext = Path.GetExtension(FileName).ToLower();
            MainForm frmMain = ((MainForm)this.parent);

            try
            {
                Directory.Delete(frmMain.TempDirectory, true);
            }
            catch { }


            //if (ext == ".psd")
            //{
            //    try
            //    {
            //        PSD.PSDDocument psdDoc = new PSD.PSDDocument();
            //        psdDoc.LoadFile(this.openFileDialog1.FileName);                            
            //        frmMain.imageViewer1.SourceDirectory = psdDoc.TempDirectory;
            //        frmMain.TempDirectory = psdDoc.TempDirectory;
            //    }
            //    catch { }
            //    finally
            //    {
            //        this.Close();
            //    }
            //    return;
            //}

            string fileSize = Utility.GetFileSize(FileName);

            FileStream fs = File.Open(FileName, FileMode.Open, FileAccess.Read);
            Program.Photo = new Bitmap(fs);
            fs.Close();

            string[] fn = FileName.Split('\\');
            Program.Title.Text = "File: " + fn[fn.Length - 1] + " Resolution: " + Program.Photo.Width.ToString() + " x " + Program.Photo.Height.ToString() + " Size: " + fileSize;
            Program.FileName = FileName;

            ((MainForm)this.parent).Open();


            Program.Source.Size = Program.Destination.Size;
            Program.Optimize2();
            //Program.Optimize(Program.Source.Width, Program.Source.Height);

            Program.Destination.Image = (Bitmap)Program.Source.Image.Clone();
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            //Save AS
            MainForm mainForm = (MainForm)this.parent;
            Cursor oldCursor = mainForm.Cursor;
            mainForm.Cursor = ApplicationWaitCursor.Cursor;
            string fileName = "";
            DialogResult res = DialogResult.None;
            int filterIndex = 0;

            try
            {
                this.cancelclose = true;

                if (Effects2.BoxFilter.Angle > 0)
                {
                    FormSaveFileDialog saveDlg = new FormSaveFileDialog(mainForm);
                    saveDlg.SaveDialog.Filter = this.saveFileDialog1.Filter;
                    res = saveDlg.ShowDialog();                    
                    fileName = saveDlg.SaveDialog.FileName;
                    filterIndex = saveDlg.SaveDialog.FilterIndex;
                }
                else
                {
                    res = this.saveFileDialog1.ShowDialog();
                    fileName = this.saveFileDialog1.FileName;
                    filterIndex = this.saveFileDialog1.FilterIndex;
                }

                if (res != DialogResult.OK)
                {
                    Extra.BackgroundColor = Color.Empty;
                    Extra.Enable = false;
                }

                if ( res == DialogResult.OK)
                {
                    this.Hide();
                    Application.DoEvents();
                    MainForm frmMain = ((MainForm)this.parent);
                    Bitmap photo = frmMain.FilterEffectsForSave();

                    switch (filterIndex)
                    {
                        case 1:
                            photo.Save(fileName, ImageFormat.Bmp);
                            break;
                        case 2:
                            ImageCodecInfo codecinfo = null;

                            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
                            for (int i = 0; i < encoders.Length; i++)
                            {
                                if (encoders[i].MimeType == "image/jpeg")
                                    codecinfo = encoders[i];
                            }

                            EncoderParameters ep = new EncoderParameters(1);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                            photo.Save(fileName, codecinfo, ep);
                            break;
                        case 3:
                            photo.Save(fileName, ImageFormat.Gif);
                            break;
                        case 4:
                            photo.Save(fileName, ImageFormat.Tiff);
                            break;
                        case 5:
                            photo.Save(fileName, ImageFormat.Png);
                            break;
                        default:
                            photo.Save(fileName);
                            break;
                    }

                    photo.Dispose();
                }

                Extra.Enable = false;
                Extra.BackgroundColor = Color.Empty;

                this.Close();
            }
            catch { }
            finally
            {
                mainForm.Cursor = oldCursor;
            }
        }

        private void Backup(string FileName)
        {
            try
            {
                if(File.Exists(FileName + ".iwbak") )
                    return;

                File.Copy(FileName,FileName + ".iwbak");
            }
            catch { }
        }

        private void ribbonButton4_Click(object sender, EventArgs e)
        {
            // Save
            MainForm mainForm = (MainForm)this.parent;
            Cursor oldCursor = mainForm.Cursor;
            try
            {
                mainForm.Cursor = ApplicationWaitCursor.Cursor;

                this.Hide();
                Application.DoEvents();

                if (Program.Destination != null && Program.Destination.Image != null)
                {
                    MainForm frmMain = ((MainForm)this.parent);

                    if (Effects2.BoxFilter.Angle > 0)
                    {
                        frmSelectBackGround frmSelectBG = new frmSelectBackGround(mainForm);
                        DialogResult res = frmSelectBG.ShowDialog();

                        if (res == DialogResult.Cancel)
                        {
                            Extra.Enable = false;
                            Extra.BackgroundColor = Color.Empty;
                            return;
                        }
                    }

                    Bitmap photo = frmMain.FilterEffectsForSave();
                    //Program.Destination.Image.Save(Program.FileName, Program.Source.Image.RawFormat);
                    ImageFormat format = Program.Photo.RawFormat;

                    //Create a backup if one already doesn't exist
                    this.Backup(Program.FileName);

                    photo.Save(Program.FileName, format);


                    mainForm.imageViewer1.UpdateThumbnail(Program.FileName);

                    Image old = Program.Source.Image;

                    Program.Source.Image = (Image)Program.Destination.Image.Clone();
                    Program.Source.Refresh();

                    OpenFile(Program.FileName);

                    Extra.Enable = false;
                    Extra.BackgroundColor = Color.Empty;

                    this.Close();
                }
            }
            catch { }
            finally
            {
                mainForm.Cursor = oldCursor;
                Extra.Enable = false;
                Extra.BackgroundColor = Color.Empty;
            }
            
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PhotoMenu_Deactivate(object sender, EventArgs e)
        {
            if (!this.cancelclose)
                this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            this.Hide();
            System.Diagnostics.Process.Start("http://www.genetibase.com/nugenimageworks.php");
            this.Close();
        }

        private void ribbonButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Splash.ShowSplash(true);
            this.Close();
        }
    }
}
