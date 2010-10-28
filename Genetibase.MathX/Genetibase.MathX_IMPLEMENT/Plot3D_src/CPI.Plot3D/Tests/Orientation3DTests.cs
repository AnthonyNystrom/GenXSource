using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CPI.Plot3D;

namespace Tests
{
    [TestFixture]
    public class Orientation3DTests
    {
        # region Helper Methods

        private void CheckInverseVectors(Orientation3D orientation)
        {
            Assert.IsTrue(orientation.BackwardVector.ApproximatelyEquals(-orientation.ForwardVector), "Expected the backward vector to be the inverse of the forward vector.");
            Assert.IsTrue(orientation.UpVector.ApproximatelyEquals(-orientation.DownVector), "Expected the up vector to be the inverse of the down vector.");
            Assert.IsTrue(orientation.LeftVector.ApproximatelyEquals(-orientation.RightVector), "Expected the left vector to be the inverse of the right vector.");
        }

        # endregion

        # region Sample Test Values

        Dictionary<double, double> degreeConversions = new Dictionary<double, double>();

        # endregion

        # region Setup Method

        [TestFixtureSetUp]
        public void VariableSetup()
        {
            double circleDegrees = 360.0;
            double circleRadians = 2.0 * Math.PI;

            // This loop produces concecutive powers of 2.
            // 1, 2, 4, 8, 16, 32
            for (int i = 1; i <= 32; i *= 2)
            {
                degreeConversions.Add(circleDegrees / i, circleRadians / i);
                degreeConversions.Add(-circleDegrees / i, -circleRadians / i);
            }
        }

        # endregion

        # region Property Tests

        [Test]
        public void DefaultPropertyTests()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            Assert.IsTrue(orientation.AngleMeasurement == AngleMeasurement.Degrees, "Expected default angle measurement to be degrees.");

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected default forward vector to be PositiveX.");
            Assert.IsTrue(orientation.BackwardVector.ApproximatelyEquals(Vector3D.NegativeX), "Expected default backward vector to be NegativeX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected default right vector to be PositiveY.");
            Assert.IsTrue(orientation.LeftVector.ApproximatelyEquals(Vector3D.NegativeY), "Expected default left vector to be NegativeY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected default down vector to be PositiveZ.");
            Assert.IsTrue(orientation.UpVector.ApproximatelyEquals(Vector3D.NegativeZ), "Expected default up vector to be NegativeZ.");
        }

        [Test]
        public void AngleMeasurementTestGoodValues()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            orientation.AngleMeasurement = AngleMeasurement.Radians;

            Assert.IsTrue(orientation.AngleMeasurement == AngleMeasurement.Radians, "Expected get_AngleMeasurement to return Radians after setting it to radians.");

            orientation.AngleMeasurement = AngleMeasurement.Degrees;

            Assert.IsTrue(orientation.AngleMeasurement == AngleMeasurement.Degrees, "Expected get_AngleMeasurement to return Degrees after setting it to degrees.");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AngleMeasurementTestBadValue()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            orientation.AngleMeasurement = (AngleMeasurement)2;
        }

        # endregion

        # region Yaw Tests

        [Test]
        public void YawLeftTest()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            orientation.YawLeft(90);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.NegativeY), "Expected rotated forward vector to equal NegativeY.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated right vector to equal PositiveX.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            CheckInverseVectors(orientation);

            orientation.AngleMeasurement = AngleMeasurement.Radians;
            orientation.YawLeft(-Math.PI / 2);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            CheckInverseVectors(orientation);
        }

        [Test]
        public void YawRightTest()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            orientation.YawRight(90);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated forward vector to equal PositiveY.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.NegativeX), "Expected rotated right vector to equal NegativeX.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            CheckInverseVectors(orientation);

            orientation.AngleMeasurement = AngleMeasurement.Radians;
            orientation.YawRight(-Math.PI / 2);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            CheckInverseVectors(orientation);
        }

        # endregion

        # region Pitch Tests

        [Test]
        public void PitchUpTest()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            orientation.PitchUp(90);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.NegativeZ), "Expected rotated forward vector to equal NegativeZ.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated down vector to equal PositiveX.");
            CheckInverseVectors(orientation);

            orientation.AngleMeasurement = AngleMeasurement.Radians;
            orientation.PitchUp(-Math.PI / 2);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            CheckInverseVectors(orientation);
        }

        [Test]
        public void PitchDownTest()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            orientation.PitchDown(90);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated forward vector to equal PositiveZ.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.NegativeX), "Expected rotated down vector to equal NegativeX.");
            CheckInverseVectors(orientation);

            orientation.AngleMeasurement = AngleMeasurement.Radians;
            orientation.PitchDown(-Math.PI / 2);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            CheckInverseVectors(orientation);
        }

        # endregion

        # region Roll Tests

        [Test]
        public void RollLeftTest()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            orientation.RollLeft(90);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.NegativeZ), "Expected rotated right vector to equal NegativeZ.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated down vector to equal PositiveY.");
            CheckInverseVectors(orientation);

            orientation.AngleMeasurement = AngleMeasurement.Radians;
            orientation.RollLeft(-Math.PI / 2);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            CheckInverseVectors(orientation);
        }

        [Test]
        public void RollRightTest()
        {
            TestLogger.Log();

            Orientation3D orientation = new Orientation3D();

            orientation.RollRight(90);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated right vector to equal PositiveZ.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.NegativeY), "Expected rotated down vector to equal NegativeY.");
            CheckInverseVectors(orientation);

            orientation.AngleMeasurement = AngleMeasurement.Radians;
            orientation.RollRight(-Math.PI / 2);

            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(Vector3D.PositiveX), "Expected rotated forward vector to equal PositiveX.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(Vector3D.PositiveY), "Expected rotated right vector to equal PositiveY.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(Vector3D.PositiveZ), "Expected rotated down vector to equal PositiveZ.");
            CheckInverseVectors(orientation);
        }


        # endregion

        # region Conversion Tests

        [Test]
        public void DegreesToRadiansTest()
        {
            foreach (KeyValuePair<double, double> kvp in degreeConversions)
            {
                DegreesToRadiansTest(kvp.Key, kvp.Value);
            }
        }

        [Test]
        public void RadiansToDegreesTest()
        {
            foreach (KeyValuePair<double, double> kvp in degreeConversions)
            {
                RadiansToDegreesTest(kvp.Value, kvp.Key);
            }
        }

        public void DegreesToRadiansTest(double degrees, double expectedRadians)
        {
            TestLogger.Log(degrees.ToString(), expectedRadians.ToString());

            Assert.AreEqual(Orientation3D.DegreesToRadians(degrees), expectedRadians, Vector3D.Tolerance, "Expected DegreesToRadians(double, double) return value to equal precomputed radian value when called with [{0}, {1}].", degrees.ToString(), expectedRadians.ToString());
        }

        public void RadiansToDegreesTest(double radians, double expectedDegrees)
        {
            TestLogger.Log(radians.ToString(), expectedDegrees.ToString());

            Assert.AreEqual(Orientation3D.RadiansToDegrees(radians), expectedDegrees, Vector3D.Tolerance, "Expected RadiansToDegrees(double, double) return value to equal precomputed degree value when called with [{0}, {1}].", radians.ToString(), expectedDegrees.ToString());
        }


        # endregion

        # region Clone Tests

        [Test]
        public void CloneTestClassMethod()
        {
            TestLogger.Log();

            // Create an Orientation3D object
            Orientation3D orientation = new Orientation3D();

            // Mess with it to make sure that it's no longer set to default.
            orientation.YawLeft(45);
            orientation.PitchDown(45);
            orientation.AngleMeasurement = AngleMeasurement.Radians;

            // Now make a clone
            Orientation3D clone = orientation.Clone();

            // Check that it has the same settings as the original
            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(clone.ForwardVector), "Expected cloned forward vector to equal original forward vector.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(clone.RightVector), "Expected cloned right vector to equal original right vector.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(clone.DownVector), "Expected cloned down vector to equal original down vector.");
            Assert.IsTrue(orientation.AngleMeasurement == clone.AngleMeasurement, "Expected cloned angle measurement to equal original angle measurement.");

            // Now verify that we're dealing with a deep copy by messing with the clone and verifying
            // that it's different from the original now.

            clone.AngleMeasurement = AngleMeasurement.Degrees;
            clone.YawLeft(45);
            clone.PitchDown(45);

            Assert.IsFalse(orientation.ForwardVector.ApproximatelyEquals(clone.ForwardVector), "Expected modified clone's forward vector not to equal original forward vector.");
            Assert.IsFalse(orientation.RightVector.ApproximatelyEquals(clone.ForwardVector), "Expected modified clone's right vector not to equal original right vector.");
            Assert.IsFalse(orientation.DownVector.ApproximatelyEquals(clone.DownVector), "Expected modified clone's down vector not to equal original down vector.");
            Assert.IsFalse(orientation.AngleMeasurement == clone.AngleMeasurement, "Expected modified clone's angle measurement not to equal original angle measurement.");
        }

        [Test]
        public void CloneTestExplicitInterfaceMethod()
        {
            TestLogger.Log();

            // Create an Orientation3D object
            Orientation3D orientation = new Orientation3D();

            // Mess with it to make sure that it's no longer set to default.
            orientation.YawLeft(45);
            orientation.PitchDown(45);
            orientation.AngleMeasurement = AngleMeasurement.Radians;

            // Now make a clone
            Orientation3D clone = (Orientation3D)((ICloneable)orientation).Clone();

            // Check that it has the same settings as the original
            Assert.IsTrue(orientation.ForwardVector.ApproximatelyEquals(clone.ForwardVector), "Expected cloned forward vector to equal original forward vector.");
            Assert.IsTrue(orientation.RightVector.ApproximatelyEquals(clone.RightVector), "Expected cloned right vector to equal original right vector.");
            Assert.IsTrue(orientation.DownVector.ApproximatelyEquals(clone.DownVector), "Expected cloned down vector to equal original down vector.");
            Assert.IsTrue(orientation.AngleMeasurement == clone.AngleMeasurement, "Expected cloned angle measurement to equal original angle measurement.");

            // Now verify that we're dealing with a deep copy by messing with the clone and verifying
            // that it's different from the original.

            clone.AngleMeasurement = AngleMeasurement.Degrees;
            clone.YawLeft(45);
            clone.PitchDown(45);

            Assert.IsFalse(orientation.ForwardVector.ApproximatelyEquals(clone.ForwardVector), "Expected modified clone's forward vector not to equal original forward vector.");
            Assert.IsFalse(orientation.RightVector.ApproximatelyEquals(clone.ForwardVector), "Expected modified clone's right vector not to equal original right vector.");
            Assert.IsFalse(orientation.DownVector.ApproximatelyEquals(clone.DownVector), "Expected modified clone's down vector not to equal original down vector.");
            Assert.IsFalse(orientation.AngleMeasurement == clone.AngleMeasurement, "Expected modified clone's angle measurement not to equal original angle measurement.");
        }

        # endregion
    }
}
