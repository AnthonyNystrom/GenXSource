using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Genetibase.NuGenTransform.Properties;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.NuGenTransform
{
    public class UnitTests
    {
        public static void Test()
        {
            //Tests go here

            //TestDefaultSettings();

            //TestDocumentSerialize();
            //TestDocumentDeserialize();
            //TestDocumentSerialization();
            //TestDigitizerGetBackgroundColor();

            //TestSetPixelAt();
            //TestGetPixels();

            //TestDiscretize();
            //TestMatrixMultiplication();
            //TestMatrixInverse();
            //TestLineIntersect();
            TestCoordinatesDialog();
        }

        private static void TestCoordinatesDialog()
        {
            //CoordinatesDialog dlg = new CoordinatesDialog();
            //dlg.ShowDialog();
            // System.Threading.Thread.Sleep(10000);
        }

        private static void TestLineIntersect()
        {
            SegmentLine line = new SegmentLine(new NuGenSegment(0));
            line.StartPoint = new Point(13,27);
            line.EndPoint = new Point(19, 55);

            line.Intersects(16, 36, 2.0);
        }

        private static void TestMatrixMultiplication()
        {

            double[][] mat1 = { new double[3] { 3, 3, 3 }, new double[3] { 3, 3, 3 }, new double[3] { 3, 3, 3 } };
            double[,] mat2 = {{3,3,3},{3,3,3},{3,3,3}};
            double[,] res = new double[3,3];
            NuGenMath.MatrixMultiply3x3(res, mat1, mat2);         
        }

        private static void TestMatrixInverse()
        {
            NuGenMath.Test();
        }

        private static void TestDigitizerGetBackgroundColor()
        {
            Image img = Image.FromFile("samples\\gridlines.gif");
            NuGenDiscretize discretize = new NuGenDiscretize(img, new DiscretizeSettings());
            discretize.GetBackgroundColor();
        }

        private static void TestDefaultSettings()
        {

            NuGenDefaultSettings s = NuGenDefaultSettings.GetInstance();
            GridRemovalSettings settings = s.GridRemovalSettings;
        }

        private static void TestDocumentSerialize()
        {
            NuGenDocument doc = new NuGenDocument(DigitizeState.SegmentState);

            Stream stream = File.Open("Test.osl", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, doc);
            stream.Close();
        }

        private static void TestDocumentDeserialize()
        {            
            Stream stream = File.Open("Test.osl", FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();

            NuGenDocument doc = (NuGenDocument)   bformatter.Deserialize(stream);
            stream.Close();
        }

        //Full featured test of serialization
        private static void TestDocumentSerialization()
        {
            NuGenDocument doc = new NuGenDocument(DigitizeState.SegmentState);

            Stream stream = File.Open("Test.osl", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, doc);
            stream.Close();

            //Now read back from the stream we just wrote

            stream = File.Open("Test.osl", FileMode.Open);

            NuGenDocument docDeserialized = (NuGenDocument)bformatter.Deserialize(stream);
            stream.Close();

            //examine the two docs for identicality
        }

        private static void TestSetPixelAt()
        {
            Image img = Image.FromFile("samples\\gridlines.gif");
            Bitmap b = new Bitmap(img);
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadWrite, b.PixelFormat);

            NuGenImageProcessor.SetPixelAt(bmData, 100, 100, 255, 0, 0);

            b.UnlockBits(bmData);

            Color c = b.GetPixel(100, 100);
        }

        private static void TestGetPixels()
        {            
            Image img = Image.FromFile("samples\\gridlines.gif");
            Bitmap b = new Bitmap(img);
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadOnly, b.PixelFormat);

            NuGenImageProcessor.SetPixelAt(bmData, 242,133, 64,65,66);
            int r, g, blue;
            NuGenImageProcessor.GetPixelAt(bmData, 242,133, out r, out g, out blue);

            Color c = Color.FromArgb(r, g, blue);

            b.UnlockBits(bmData);

            Color cb = b.GetPixel(242,133);

            if (!(c.R == 64 && c.G == 65 && c.B == 66))
            {
                throw new Exception("Failure");
            }

            if (!(c.R == cb.R && c.G == cb.G && c.B == cb.B))
            {
                throw new Exception("Failure");
            }
        }

        private static void TestDiscretize()
        {
            Image img = Image.FromFile("samples\\gridlines.gif");
            NuGenDiscretize discretize = new NuGenDiscretize(img, NuGenDefaultSettings.GetInstance().DiscretizeSettings);

            discretize.Discretize();

            Form f = new Form();
            f.Size = new Size(img.Width, img.Height);

            f.BackgroundImage = discretize.GetImage();

            f.ShowDialog();

            System.Threading.Thread.Sleep(10000);
        }

        private static void TestRemoveColor()
        {                                              
        }

        private static void TestRemoveGridlines()
        {
        }

        private static void TestRemoveThinLines()
        {
        }

        private static void TestRemoveGaps()
        {
        }
    }
}
