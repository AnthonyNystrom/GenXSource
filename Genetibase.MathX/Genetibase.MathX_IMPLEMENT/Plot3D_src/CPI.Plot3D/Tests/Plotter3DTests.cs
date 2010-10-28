using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using NUnit.Framework;
using CPI.Plot3D;

namespace Tests
{
    [TestFixture]
    public class Plotter3DTests
    {
        # region Private Fields

        private Bitmap testBitmap;
        private Graphics testGraphics;

        # endregion

        # region Setup Method

        [SetUp]
        public void VariableSetup()
        {
            testBitmap = new Bitmap(100, 100);
            testGraphics = Graphics.FromImage(testBitmap);
            testGraphics.Clear(Color.White);
        }

        # endregion

        # region TearDown Method

        [TearDown]
        public void VariableTearDown()
        {
            if (testGraphics != null)
                testGraphics.Dispose();

            if (testBitmap != null)
                testBitmap.Dispose();
        }

        # endregion

        # region Constructor Tests

        [Test]
        public void ConstructorTest1()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                Assert.AreEqual(p.Canvas, testGraphics, "Expected the Canvas property to equal our testGraphics variable.");
                Assert.IsTrue(p.PenColor.A == 255 && p.PenColor.R == 0 && p.PenColor.G == 0 && p.PenColor.B == 0, "Expected the PenColor property to be black.");
                Assert.AreEqual(p.PenWidth, 1f, .000001, "Expected the PenWidth property to be 1f.");
                Assert.IsTrue(p.IsPenDown, "Expected the IsPenDown property to be true.");
                Assert.IsTrue(p.AngleMeasurement == AngleMeasurement.Degrees, "Expected AngleMeasurement property to be Degrees.");
                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(0, 0, 0)), "Expected Location property to be [0, 0, 0].");
                Assert.IsTrue(p.CameraLocation.ApproximatelyEquals(new Point3D(-30, 0, -600)), "Expected CameraLocation property to be [-30, 0, -600].");
                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected Orientation.ForwardVector to equal Vector3D.PositiveX.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected Orientation.DownVector to equal Vector3D.PositiveZ.");
            }
        }

        [Test]
        public void ConstructorTest2()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Green, 3);

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                Assert.AreEqual(p.Canvas, testGraphics, "Expected the Canvas property to equal our testGraphics variable.");
                Assert.AreEqual(p.Pen, pen, "Expected the Pen property to equal our pen variable.");
                Assert.IsTrue(p.IsPenDown, "Expected the IsPenDown property to be true.");
                Assert.IsTrue(p.AngleMeasurement == AngleMeasurement.Degrees, "Expected AngleMeasurement property to be Degrees.");
                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(0, 0, 0)), "Expected Location property to be [0, 0, 0].");
                Assert.IsTrue(p.CameraLocation.ApproximatelyEquals(new Point3D(-30, 0, -600)), "Expected CameraLocation property to be [-30, 0, -600].");
                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected Orientation.ForwardVector to equal Vector3D.PositiveX.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected Orientation.DownVector to equal Vector3D.PositiveZ.");
            }

            bool caughtException = false;

            try
            {
                float f = pen.Width;
            }
            catch (ArgumentException)
            {
                caughtException = true;
            }

            Assert.IsTrue(caughtException, "Expected pen variable to be disposed, which should have thrown an exception.");
        }

        [Test]
        public void ConstructorTest3()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics, new Point3D(-2, -2, -2)))
            {
                Assert.AreEqual(p.Canvas, testGraphics, "Expected the Canvas property to equal our testGraphics variable.");
                Assert.IsTrue(p.PenColor.A == 255 && p.PenColor.R == 0 && p.PenColor.G == 0 && p.PenColor.B == 0, "Expected the PenColor property to be black.");
                Assert.AreEqual(p.PenWidth, 1f, .000001, "Expected the PenWidth property to be 1f.");
                Assert.IsTrue(p.IsPenDown, "Expected the IsPenDown property to be true.");
                Assert.IsTrue(p.AngleMeasurement == AngleMeasurement.Degrees, "Expected AngleMeasurement property to be Degrees.");
                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(0, 0, 0)), "Expected Location property to be [0, 0, 0].");
                Assert.IsTrue(p.CameraLocation.ApproximatelyEquals(new Point3D(-2, -2, -2)), "Expected CameraLocation property to be [-2, -2, -2].");
                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected Orientation.ForwardVector to equal Vector3D.PositiveX.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected Orientation.DownVector to equal Vector3D.PositiveZ.");
            }
        }

        [Test]
        public void ConstructorTest4()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Orange, 15);

            using (Plotter3D p = new Plotter3D(testGraphics, pen, new Point3D(5, 6, 23)))
            {
                Assert.AreEqual(p.Canvas, testGraphics, "Expected the Canvas property to equal our testGraphics variable.");
                Assert.AreEqual(p.Pen, pen, "Expected the Pen property to equal our pen variable.");
                Assert.IsTrue(p.IsPenDown, "Expected the IsPenDown property to be true.");
                Assert.IsTrue(p.AngleMeasurement == AngleMeasurement.Degrees, "Expected AngleMeasurement property to be Degrees.");
                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(0, 0, 0)), "Expected Location property to be [0, 0, 0].");
                Assert.IsTrue(p.CameraLocation.ApproximatelyEquals(new Point3D(5, 6, 23)), "Expected CameraLocation property to be [5, 6, 23].");
                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected Orientation.ForwardVector to equal Vector3D.PositiveX.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected Orientation.DownVector to equal Vector3D.PositiveZ.");
            }

            bool caughtException = false;

            try
            {
                float f = pen.Width;
            }
            catch (ArgumentException)
            {
                caughtException = true;
            }

            Assert.IsTrue(caughtException, "Expected pen variable to be disposed, which should have thrown an exception.");
        }

        # endregion

        # region Dispose Tests

        [Test]
        public void DisposeWithRegularPenTest()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Orange);
            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                // Don't to anything; we're just going to let the dispose method 
                // run, and make sure it disposes the pen.
            }

            bool caughtException = false;

            try
            {
                float width = pen.Width;
            }
            catch
            {
                caughtException = true;
            }

            Assert.IsTrue(caughtException, "Expected pen variable to be disposed in Plotter3D.Dispose().");
        }


        [Test]
        public void DisposeWithImmutabePenTest()
        {
            TestLogger.Log();

            Pen pen = Pens.Beige;

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                // Don't to anything; we're just going to let the dispose method 
                // run, and make sure it disposes the pen.
            }

            bool caughtException = false;

            try
            {
                float width = pen.Width;
            }
            catch
            {
                caughtException = true;
            }

            Assert.IsFalse(caughtException, "Did not expect pen variable to be disposed in Plotter3D.Dispose().");
        }

        # endregion

        # region Orientation Tests

        [Test]
        public void OrientationGetSetTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                Orientation3D newOrientation = new Orientation3D();

                // verify that the orientation in the plotter is not the same as the one we just created
                // (We're not checking its contents; we're just verifying that they're two different instances.
                Assert.IsFalse(Object.ReferenceEquals(p.Orientation, newOrientation), "Expected existing orientation to be different from new orientation.");

                // Set the orientation to the new one that we created
                p.Orientation = newOrientation;

                // Verify that the Orientation property returns a reference to our newOrientation now.
                Assert.IsTrue(Object.ReferenceEquals(p.Orientation, newOrientation), "Expected p.Orientation to equal newOrientation.");
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrientationSetNullTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.Orientation = null;
            }
        }

        # endregion

        # region Pen Tests

        [Test]
        public void PenGetSetRegularTest()
        {
            TestLogger.Log();

            Pen pen1 = new Pen(Color.BurlyWood);

            Pen pen2 = new Pen(Color.MintCream);

            using (Plotter3D p = new Plotter3D(testGraphics, pen1))
            {
                Assert.IsTrue(Object.ReferenceEquals(p.Pen, pen1), "Expected p.Pen to equal pen1.");

                p.Pen = pen2;

                Assert.IsTrue(Object.ReferenceEquals(p.Pen, pen2), "Expected p.Pen to equal pen2.");

                bool pen1Disposed = false;

                try
                {
                    float f = pen1.Width;
                }
                catch (ArgumentException)
                {
                    pen1Disposed = true;
                }

                Assert.IsTrue(pen1Disposed, "Expected pen1 to be disposed when p.Pen is set to pen2.");
            }

            bool pen2Disposed = false;

            try
            {
                float f = pen2.Width;
            }
            catch (ArgumentException)
            {
                pen2Disposed = true;
            }

            Assert.IsTrue(pen2Disposed, "Expected pen2 to be disposed when p is disposed.");
        }

        [Test]
        public void PenGetSetImmutableTest()
        {
            TestLogger.Log();

            Pen pen1 = Pens.Azure;

            Pen pen2 = Pens.LightSlateGray;

            using (Plotter3D p = new Plotter3D(testGraphics, pen1))
            {
                Assert.IsTrue(Object.ReferenceEquals(p.Pen, pen1), "Expected p.Pen to equal pen1.");

                p.Pen = pen2;

                Assert.IsTrue(Object.ReferenceEquals(p.Pen, pen2), "Expected p.Pen to equal pen2.");

                bool pen1Disposed = false;

                try
                {
                    float f = pen1.Width;
                }
                catch (ArgumentException)
                {
                    pen1Disposed = true;
                }

                Assert.IsFalse(pen1Disposed, "Expected immutable pen1 not to be disposed when p.Pen is set to pen2.");
            }

            bool pen2Disposed = false;

            try
            {
                float f = pen2.Width;
            }
            catch (ArgumentException)
            {
                pen2Disposed = true;
            }

            Assert.IsFalse(pen2Disposed, "Expected immutable pen2 not to be disposed when p is disposed.");
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PenSetNullTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.Pen = null;
            }
        }

        # endregion           

        # region Pen Color Tests

        [Test]
        public void PenColorGetSetRegularTest()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Moccasin);

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                Assert.IsTrue(p.PenColor == Color.Moccasin, "Expected p.PenColor to be Color.Moccasin.");

                p.PenColor = Color.Linen;

                Assert.IsTrue(p.PenColor == Color.Linen, "Expected p.PenColor to be Color.Linen.");

                Assert.IsFalse(object.ReferenceEquals(p.Pen, pen), "Expected a new Pen object to be created when the pen color changes.");

                bool penDisposed = false;

                try
                {
                    float f = pen.Width;
                }
                catch (ArgumentException)
                {
                    penDisposed = true;
                }

                Assert.IsTrue(penDisposed, "Expected pen to be disposed when p.PenColor changes.");
            }
        }

        [Test]
        public void PenColorGetSetImmutableTest()
        {
            TestLogger.Log();

            Pen pen = Pens.Orchid;

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                Assert.IsTrue(p.PenColor == Color.Orchid, "Expected p.PenColor to be Color.Orchid.");

                p.PenColor = Color.PaleGoldenrod;

                Assert.IsTrue(p.PenColor == Color.PaleGoldenrod, "Expected p.PenColor to be Color.PaleGoldenrod.");

                Assert.IsFalse(object.ReferenceEquals(p.Pen, pen), "Expected a new Pen object to be created when the pen color changes.");

                bool penDisposed = false;

                try
                {
                    float f = pen.Width;
                }
                catch (ArgumentException)
                {
                    penDisposed = true;
                }

                Assert.IsFalse(penDisposed, "Expected immutable pen not to be disposed when p.PenColor changes.");
            }
        }

        # endregion

        # region Pen Width Tests


        [Test]
        public void PenWidthGetSetRegularTest()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Peru, 1f);

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                Assert.AreEqual(1f, p.PenWidth, .00001, "Expected p.PenWidth to equal 1f.");

                p.PenWidth = 2f;

                Assert.AreEqual(2f, p.PenWidth, .00001, "Expected p.PenWidth to equal 2f.");

                Assert.IsFalse(object.ReferenceEquals(p.Pen, pen), "Expected a new Pen object to be created when the pen width changes.");

                bool penDisposed = false;

                try
                {
                    float f = pen.Width;
                }
                catch (ArgumentException)
                {
                    penDisposed = true;
                }

                Assert.IsTrue(penDisposed, "Expected pen to be disposed when p.PenWidth changes.");
            }
        }

        [Test]
        public void PenWidthGetSetImmutableTest()
        {
            TestLogger.Log();

            Pen pen = Pens.PapayaWhip;

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                Assert.AreEqual(1f, p.PenWidth, .00001, "Expected p.PenWidth to equal 1f.");

                p.PenWidth = 2f;

                Assert.AreEqual(2f, p.PenWidth, .00001, "Expected p.PenWidth to equal 2f.");

                Assert.IsFalse(object.ReferenceEquals(p.Pen, pen), "Expected a new Pen object to be created when the pen color changes.");

                bool penDisposed = false;

                try
                {
                    float f = pen.Width;
                }
                catch (ArgumentException)
                {
                    penDisposed = true;
                }

                Assert.IsFalse(penDisposed, "Expected immutable pen not to be disposed when p.PenWidth changes.");
            }
        }


        # endregion

        # region IsPenDown Test

        [Test]
        public void IsPenDownGetSetTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                Assert.IsTrue(p.IsPenDown, "Expected default pen position to be down.");

                p.IsPenDown = false;

                Assert.IsFalse(p.IsPenDown, "Expected p.IsPenDown to be false.");

                p.IsPenDown = true;

                Assert.IsTrue(p.IsPenDown, "Expected p.IsPenDown to be true.");
            }
        }

        # endregion

        # region Location Test

        [Test]
        public void LocationGetSetTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(0f, 0f, 0f)), "Expected default location to be [0, 0, 0].");

                Point3D newPoint = new Point3D(-1, 2, 4.5f);

                p.Location = newPoint;

                Assert.IsTrue(p.Location.ApproximatelyEquals(newPoint), "Expected location to equal what we just set it to, namely [-1, 2, 4.5].");
            }
        }

        # endregion

        # region Canvas Test

        [Test]
        public void CanvasGetTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                Assert.IsTrue(object.ReferenceEquals(testGraphics, p.Canvas), "Expected p.Canvas to equal testGraphics.");
            }
        }

        # endregion

        # region Bounding Box Test

        [Test]
        public void BoundingBoxTest()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Blue, 1);

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                Assert.IsTrue(p.BoundingBox == Rectangle.Empty, "Expected bounding box to be empty initially.");

                p.Location = new Point3D(50, 50, 0);

                // Just changing the location by setting the Location property doesn't increase
                // the bounding box.
                Assert.IsTrue(p.BoundingBox == Rectangle.Empty, "Expected bounding box to remain empty after repositioning pen.");

                p.Forward(10);

                Assert.AreEqual(p.BoundingBox.Left, 49 - 1 - (int)(pen.Width / 2), .00001, "Expected BoundingBox.Left to be 48 after drawing a line.");
                Assert.AreEqual(p.BoundingBox.Right, 61 + (int)(pen.Width / 2), .00001, "Expected BoundingBox.Right to be 61 after drawing a line.");
                Assert.AreEqual(p.BoundingBox.Top, 49 - 1 - (int)(pen.Width / 2), .00001, "Expected BoundingBox.Top to be 48 after drawing a line.");
                Assert.AreEqual(p.BoundingBox.Bottom, 51 + (int)(pen.Width / 2), .00001, "Expected BoundingBox.Bottom to be 51 after drawing a line.");

                p.TurnRight(90);

                p.Forward(10);

                Assert.AreEqual(p.BoundingBox.Left, 49 - 1 - (int)(pen.Width / 2), .00001, "Expected BoundingBox.Left to be 48 after drawing 2 lines.");
                Assert.AreEqual(p.BoundingBox.Right, 61 + (int)(pen.Width / 2), .00001, "Expected BoundingBox.Right to be 61 after drawing 2 lines.");
                Assert.AreEqual(p.BoundingBox.Top, 49 - 1 - (int)(pen.Width / 2), .00001, "Expected BoundingBox.Top to be 48 after drawing 2 lines.");
                Assert.AreEqual(p.BoundingBox.Bottom, 61 + (int)(pen.Width / 2), .00001, "Expected BoundingBox.Bottom to be 61 after drawing 2 lines.");


                p.TurnRight(90);

                p.PenUp();

                p.Forward(20);

                // The bounding box should stay the same because the pen was up.
                Assert.AreEqual(p.BoundingBox.Left, 49 - 1 - (int)(pen.Width / 2), .00001, "Expected BoundingBox.Left to stay the same.");
                Assert.AreEqual(p.BoundingBox.Right, 61 + (int)(pen.Width / 2), .00001, "Expected BoundingBox.Right to stay the same.");
                Assert.AreEqual(p.BoundingBox.Top, 49 - 1 - (int)(pen.Width / 2), .00001, "Expected BoundingBox.Top to stay the same.");
                Assert.AreEqual(p.BoundingBox.Bottom, 61 + (int)(pen.Width / 2), .00001, "Expected BoundingBox.Bottom to stay the same.");

                p.PenDown();

                p.MoveTo(new Point3D(55, 30, 0));

                Assert.AreEqual(p.BoundingBox.Left, 39 - 1 - (int)(pen.Width / 2), .00001, "Expected BoundingBox.Left to be 38 after drawing 3 lines.");
                Assert.AreEqual(p.BoundingBox.Right, 61 + (int)(pen.Width / 2), .00001, "Expected BoundingBox.Right to be 61 after drawing 3 lines.");
                Assert.AreEqual(p.BoundingBox.Top, 29 - 1 - (int)(pen.Width / 2), .00001, "Expected BoundingBox.Top to be 28 after drawing 3 lines.");
                Assert.AreEqual(p.BoundingBox.Bottom, 61 + (int)(pen.Width / 2), .00001, "Expected BoundingBox.Bottom to be 61 after drawing 3 lines.");

            }
        }


        # endregion

        # region Angle Measurement Tests

        [Test]
        public void AngleMeasurementTestGoodValues()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.AngleMeasurement = AngleMeasurement.Radians;

                Assert.IsTrue(p.AngleMeasurement == AngleMeasurement.Radians, "Expected p.AngleMeasurement to equal AngleMeasurement.Radians.");

                p.AngleMeasurement = AngleMeasurement.Degrees;

                Assert.IsTrue(p.AngleMeasurement == AngleMeasurement.Degrees, "Expected p.AngleMeasurement to equal AngleMeasurement.Degrees.");
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AngleMeasurementtestBadValue()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.AngleMeasurement = (AngleMeasurement)(-5);
            }
        }

        # endregion

        # region Camera Location Test

        [Test]
        public void CameraLocationTest()
        {
            TestLogger.Log();

            Point3D cameraLocation = new Point3D(20, 30, 40);

            using (Plotter3D p = new Plotter3D(testGraphics, cameraLocation))
            {
                Assert.IsTrue(p.CameraLocation.ApproximatelyEquals(cameraLocation), "Expected p.CameraLocation to equal cameraLocation.");
            }
        }

        # endregion

        # region Forward() Tests

        [Test]
        public void ForwardWithPenUpTest()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Blue, 3f);

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                p.PenUp();

                p.Location = new Point3D(0, 5, 0);

                p.Forward(100);

                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(100, 5, 0)), "Expected p.Location to be [100, 5, 0] after move.");

                // Make sure that the pen didn't actually draw anything, what with the pen being up and all.
                // We're going to look at all the points in a vertical line crossing the path the the plotter
                // took when it moved forward, and verify that none of the pixels have been colored on.
                for (int i = 0; i < 10; i++)
                {
                    Color c = testBitmap.GetPixel(50, i);

                    Assert.IsTrue(c.A == 255 && c.R == 255 && c.G == 255 && c.B == 255, "Expected all the pixels to be white.");
                }

            }
        }

        [Test]
        public void ForwardWithPenDownTest()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Blue, 3f);

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                p.PenDown();

                p.Location = new Point3D(0, 5, 0);

                p.Forward(100);

                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(100, 5, 0)), "Expected p.Location to be [100, 5, 0] after move.");

                // Make sure that the pixels above our line are still white
                for (int i = 0; i < 4; i++)
                {
                    Color c = testBitmap.GetPixel(50, i);

                    Assert.IsTrue(c.A == 255 && c.R == 255 && c.G == 255 && c.B == 255, "Expected the pixels above the line to be white.");
                }

                for (int i = 4; i < 7; i++)
                {
                    Color c = testBitmap.GetPixel(50, i);

                    Assert.IsTrue(c.A == 255 && c.R == 0 && c.G == 0 && c.B == 255, "Expected the pixels in the path of the line to be blue.");
                }

                // Make sure that the pixels below our line are still white
                for (int i = 7; i < 10; i++)
                {
                    Color c = testBitmap.GetPixel(50, i);

                    Assert.IsTrue(c.A == 255 && c.R == 255 && c.G == 255 && c.B == 255, "Expected the pixels below the line to be white.");
                }

            }
        }

        [Test]
        public void ForwardAtAWeirdAngleTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.AngleMeasurement = AngleMeasurement.Radians;

                p.TurnRight(Math.PI / 4);
                p.TurnDown(Math.Atan(1 / Math.Sqrt(2)));

                p.Forward(100);

                float oneHundredOverTheSquareRootOfThree = (float)(100.0 / Math.Sqrt(3.0));

                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(oneHundredOverTheSquareRootOfThree, oneHundredOverTheSquareRootOfThree, oneHundredOverTheSquareRootOfThree)),
                    "Expected position after move to be [100/Math.Sqrt(3), 100/Math.Sqrt(3), 100/Math.Sqrt(3)].");
            }
        }

        # endregion

        # region MoveTo() Tests

        [Test]
        public void MoveToWithPenUpTest()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Blue, 3f);

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                p.PenUp();

                p.Location = new Point3D(0, 5, 0);

                Point3D newLocation = new Point3D(100, 5, 0);

                p.MoveTo(newLocation);

                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(100, 5, 0)), "Expected p.Location to be [100, 5, 0] after move.");

                // Make sure that the pen didn't actually draw anything, what with the pen being up and all.
                // We're going to look at all the points in a vertical line crossing the path the the plotter
                // took when it moved forward, and verify that none of the pixels have been colored on.
                for (int i = 0; i < 10; i++)
                {
                    Color c = testBitmap.GetPixel(50, i);

                    Assert.IsTrue(c.A == 255 && c.R == 255 && c.G == 255 && c.B == 255, "Expected all the pixels to be white.");
                }
            }
        }

        [Test]
        public void MoveToWithPenDownTest()
        {
            TestLogger.Log();

            Pen pen = new Pen(Color.Blue, 3f);

            using (Plotter3D p = new Plotter3D(testGraphics, pen))
            {
                p.PenDown();

                p.Location = new Point3D(0, 5, 0);

                Point3D newLocation = new Point3D(100, 5, 0);

                p.MoveTo(newLocation);

                Assert.IsTrue(p.Location.ApproximatelyEquals(new Point3D(100, 5, 0)), "Expected p.Location to be [100, 5, 0] after move.");

                // Make sure that the pixels above our line are still white
                for (int i = 0; i < 4; i++)
                {
                    Color c = testBitmap.GetPixel(50, i);

                    Assert.IsTrue(c.A == 255 && c.R == 255 && c.G == 255 && c.B == 255, "Expected the pixels above the line to be white.");
                }

                for (int i = 4; i < 7; i++)
                {
                    Color c = testBitmap.GetPixel(50, i);

                    Assert.IsTrue(c.A == 255 && c.R == 0 && c.G == 0 && c.B == 255, "Expected the pixels in the path of the line to be blue.");
                }

                // Make sure that the pixels below our line are still white
                for (int i = 7; i < 10; i++)
                {
                    Color c = testBitmap.GetPixel(50, i);

                    Assert.IsTrue(c.A == 255 && c.R == 255 && c.G == 255 && c.B == 255, "Expected the pixels below the line to be white.");
                }

            }
        }

        # endregion

        # region PenUp(), PenDown() Test

        [Test]
        public void PenUpDownTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                Assert.IsTrue(p.IsPenDown, "Expected default pen position to be down.");

                p.PenUp();

                Assert.IsFalse(p.IsPenDown, "Expected pen position to be up after call to p.PenUp().");

                p.PenDown();

                Assert.IsTrue(p.IsPenDown, "Expected pen position to be down after call to p.PenDown().");
            }
        }

        # endregion

        # region TurnLeft(), TurnRight() Tests

        [Test]
        public void TurnLeftTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.TurnLeft(90);

                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.NegativeY), "Expected rotated forward vector to equal NegativeY.");
                Assert.IsTrue(p.Orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated right vector to equal PositiveX.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");

                p.AngleMeasurement = AngleMeasurement.Radians;
                p.TurnLeft(-Math.PI / 2);

                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
                Assert.IsTrue(p.Orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            }
        }

        [Test]
        public void TurnRightTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.TurnRight(90);

                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated forward vector to equal PositiveY.");
                Assert.IsTrue(p.Orientation.RightVector.ApproximatelyEquals(Vector3D.NegativeX), "Expected rotated right vector to equal NegativeX.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");


                p.AngleMeasurement = AngleMeasurement.Radians;
                p.TurnRight(-Math.PI / 2);

                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
                Assert.IsTrue(p.Orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");

            }
        }

        # endregion

        # region TurnUp(), TurnDown() Tests


        [Test]
        public void TurnUpTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.TurnUp(90);

                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.NegativeZ), "Expected rotated forward vector to equal NegativeZ.");
                Assert.IsTrue(p.Orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated down vector to equal PositiveX.");

                p.AngleMeasurement = AngleMeasurement.Radians;
                p.TurnUp(-Math.PI / 2);

                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
                Assert.IsTrue(p.Orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            }
        }

        [Test]
        public void TurnDownTest()
        {
            TestLogger.Log();

            using (Plotter3D p = new Plotter3D(testGraphics))
            {
                p.TurnDown(90);

                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated forward vector to equal PositiveZ.");
                Assert.IsTrue(p.Orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.NegativeX), "Expected rotated down vector to equal NegativeX.");

                p.AngleMeasurement = AngleMeasurement.Radians;
                p.TurnDown(-Math.PI / 2);

                Assert.IsTrue(p.Orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
                Assert.IsTrue(p.Orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
                Assert.IsTrue(p.Orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            }
        }


        # endregion
    }
}
