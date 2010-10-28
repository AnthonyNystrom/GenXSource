using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NUnit.Framework;
using CPI.Plot3D;

namespace Tests
{

    [TestFixture]
    public class Point3DTests
    {
        # region Sample Test Values

        float[] goodValues = new float[]{
            float.MinValue, 
            float.MinValue / 2f,
            -382.23783982f,
            -5,
            -float.Epsilon * 2,
            -float.Epsilon,
            0,
            float.Epsilon,
            float.Epsilon * 2,
            5,
            382.23783982f,
            float.MaxValue / 2f,
            float.MaxValue
        };

        float[] badValues = new float[]{
            float.PositiveInfinity,
            float.NegativeInfinity,
            float.NaN
        };

        # endregion 

        # region Constructor Tests

        [Test]
        public void ConstructorGoodValues()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        ConstructorTest(x, y, z);
        }


        [Test]
        public void ConstructorTestBadXValue()
        {
            string expectedMessage = "Argument x cannot be infinity or NAN";

            ConstructorTestBadValue(badValues, goodValues, goodValues, expectedMessage);
        }

        [Test]
        public void ConstructorTestBadYValue()
        {
            string expectedMessage = "Argument y cannot be infinity or NAN";

            ConstructorTestBadValue(goodValues, badValues, goodValues, expectedMessage);
        }

        [Test]
        public void ConstructorTestBadZValue()
        {
            string expectedMessage = "Argument z cannot be infinity or NAN";

            ConstructorTestBadValue(goodValues, goodValues, badValues, expectedMessage);
        }

        public void ConstructorTestBadValue(float[] xValues, float[] yValues, float[] zValues, string expectedMessage)
        {
            foreach (float x in xValues)
                foreach (float y in yValues)
                    foreach (float z in zValues)
                    {
                        bool caught = false;
                        try
                        {
                            ConstructorTest(x, y, z);
                        }
                        catch (ArgumentException ex)
                        {
                            caught = true;
                            Assert.AreSame(expectedMessage, ex.Message, "Expected exception message [{0}], but got [{1}] when called with parameters [{2}, {3}, {4}]", expectedMessage, ex.Message, x.ToString(), y.ToString(), z.ToString());
                        }

                        if (caught == false)
                            Assert.Fail("Expected constructor to throw ArgumentException, but no exception thrown, when called with parameters [{0}, {1}, {2}].", x.ToString(), y.ToString(), z.ToString());
                    }
        }

        public void ConstructorTest(float x, float y, float z)
        {
            TestLogger.Log(x.ToString(), y.ToString(), z.ToString());

            Point3D point = new Point3D(x, y, z);

            Assert.AreEqual(x, point.X, Point3D.Tolerance, "point.X [{0}] is not approximately equal to parameter x [{1}] when called with parameters [{2}, {3}, {4}].", point.X.ToString(), x.ToString(), x.ToString(), y.ToString(), z.ToString());
            Assert.AreEqual(y, point.Y, Point3D.Tolerance, "point.Y [{0}] is not approximately equal to parameter y [{1}] when called with parameters [{2}, {3}, {4}].", point.Y.ToString(), y.ToString(), x.ToString(), y.ToString(), z.ToString());
            Assert.AreEqual(z, point.Z, Point3D.Tolerance, "point.Z [{0}] is not approximately equal to parameter z [{1}] when called with parameters [{2}, {3}, {4}].", point.Z.ToString(), z.ToString(), x.ToString(), y.ToString(), z.ToString());
        }

        [Test]
        public void EmptyConstructorTest()
        {
            TestLogger.Log();

            Point3D emptyConstructor = new Point3D();

            Point3D explicitZeroConstructor = new Point3D(0f, 0f, 0f);

            Assert.AreEqual(emptyConstructor, explicitZeroConstructor, "Expected an empty constructor to yield the same point as one expicitly instantiated with [0f, 0f, 0f]");
        }

        # endregion

        # region ToString() Tests

        [Test]
        public void ToStringTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        ToStringTest(new Point3D(x, y, z), string.Format("[{0}, {1}, {2}]", x.ToString(), y.ToString(), z.ToString()));
        }

        [Test]
        public void ToStringRoundingTestGoodValues()
        {
            // Try all possible valid combinations of digits, because hey...what the hell?
            for (int digits = 0; digits <= 15; digits++)
                foreach (float x in goodValues)
                    foreach (float y in goodValues)
                        foreach (float z in goodValues)
                            ToStringRoundingTest(new Point3D(x, y, z), digits, string.Format("[{0}, {1}, {2}]", ((float)Math.Round(x, digits)).ToString(), ((float)Math.Round(y, digits)).ToString(), ((float)Math.Round(z, digits)).ToString()));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToStringRoundingToNegativeOneTest()
        {
            ToStringRoundingTest(new Point3D(goodValues[0], goodValues[1], goodValues[2]), -1, "Actually, we expect to throw an exception");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToStringRoundingToSixteenTest()
        {
            ToStringRoundingTest(new Point3D(goodValues[3], goodValues[4], goodValues[5]), 16, "Actually, we expect to throw an exception");
        }

        public void ToStringTest(Point3D point, string expectedValue)
        {
            TestLogger.Log(point.ToString(), expectedValue.ToString());

            string toString = point.ToString();
            Assert.AreEqual(toString, expectedValue, "Expected ToString() to return [{0}], instead got [{1}], when called with point [{2},{3},{4}].", expectedValue, toString, point.X.ToString(), point.Y.ToString(), point.Z.ToString());
        }

        public void ToStringRoundingTest(Point3D point, int digits, string expectedValue)
        {
            TestLogger.Log(point.ToString(), digits.ToString(), expectedValue.ToString());

            string toString = point.ToString(digits);
            Assert.AreEqual(toString, expectedValue, "Expected ToString() to return [{0}], instead got [{1}], when called with point [{2},{3},{4}] and [{5}] digits.", expectedValue, toString, point.X.ToString(), point.Y.ToString(), point.Z.ToString(), digits.ToString());
        }

        # endregion

        # region Equals() Tests

        [Test]
        public void EqualsTests()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        Point3D point1 = new Point3D(x, y, z);

                        // point2 should be equal to point1
                        Point3D point2 = new Point3D(x, y, z);

                        EqualsTest(point1, point2);

                        // point3's x value is off, and should not be equal to point1
                        Point3D point3 = new Point3D(x + (x < 0 ? (float.MaxValue / 2): (float.MinValue / 2)), y, z);

                        NotEqualsTest(point1, point3);

                        // point4's y value is off, and should not be equal to point1
                        Point3D point4 = new Point3D(x, y + (y < 0 ? (float.MaxValue / 2) : (float.MinValue / 2)), z);

                        NotEqualsTest(point1, point4);

                        // point5's z value is off, and should not be equal to point1
                        Point3D point5 = new Point3D(x, y, z + (z < 0 ? (float.MaxValue / 2) : (float.MinValue / 2)));

                        NotEqualsTest(point1, point5);
                    }
        }


        public void EqualsTest(Point3D a, Point3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            // check IEquatable<Point3D>.Equals(Point3D)
            Assert.IsTrue(a.Equals(b), "Expected IEquatable<Point3D>.Equals(Point3D) to return true when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsTrue(b.Equals(a), "Expected IEquatable<Point3D>.Equals(Point3D) to return true when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check Point3D.Equals(object)
            Assert.IsTrue(a.Equals((object)b), "Expected Point3D.Equals(object) to return true when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsTrue(b.Equals((object)a), "Expected Point3D.Equals(object) to return true when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check op_equality(Point3D, Point3D)
            Assert.IsTrue(a == b, "Expected op_equality(Point3D, Point3D) to return true when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsTrue(b == a, "Expected op_equality(Point3D, Point3D) to return true when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check op_inequality(Point3D, Point3D)
            Assert.IsFalse(a != b, "Expected op_inequality(Point3D, Point3D) to return false when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsFalse(b != a, "Expected op_inequality(Point3D, Point3D) to return false when called with parameters [{0}, {1}].", b.ToString(), a.ToString());
        }

        public void NotEqualsTest(Point3D a, Point3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            // check IEquatable<Point3D>.Equals(Point3D)
            Assert.IsFalse(a.Equals(b), "Expected IEquatable<Point3D>.Equals(Point3D) to return false when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsFalse(b.Equals(a), "Expected IEquatable<Point3D>.Equals(Point3D) to return false when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check Point3D.Equals(object)
            Assert.IsFalse(a.Equals((object)b), "Expected Point3D.Equals(object) to return false when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsFalse(b.Equals((object)a), "Expected Point3D.Equals(object) to return false when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check Point3D.Equals(object) with a non Point3D parameter.
            // In this case we'll test a potentially inflammatory string object.
            Assert.IsFalse(a.Equals("Why haven't the police found Tupac's murderer yet?"));

            // check op_equality(Point3D, Point3D)
            Assert.IsFalse(a == b, "Expected op_equality(Point3D, Point3D) to return false when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsFalse(b == a, "Expected op_equality(Point3D, Point3D) to return false when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check op_inequality(Point3D, Point3D)
            Assert.IsTrue(a != b, "Expected op_inequality(Point3D, Point3D) to return true when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsTrue(b != a, "Expected op_inequality(Point3D, Point3D) to return true when called with parameters [{0}, {1}].", b.ToString(), a.ToString());
        }

        # endregion

        # region ApproximatelyEquals() Tests

        [Test]
        public void ApproximatelyEqualsTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        Point3D point = new Point3D(x, y, z);

                        // This represents 90% of tolerance.  If we add or subtract
                        // this value from our points, we should still be approximately
                        // equal.
                        float nearToleranceLevel = Point3D.Tolerance * .9f;

                        Point3D nearbyPoint = new Point3D(
                            x + (x < 0 ? nearToleranceLevel : -nearToleranceLevel),
                            y + (y < 0 ? nearToleranceLevel : -nearToleranceLevel),
                            z + (z < 0 ? nearToleranceLevel : -nearToleranceLevel)
                        );

                        ApproximatelyEqualsTest(point, nearbyPoint);

                        // You might be thinking, let's add 110% of the tolerance and verify that 
                        // the resulting point is NOT approximately equal.  Well, we can't, because
                        // of precision problems with really big numbers.  For example,
                        // float.MaxValue + 2 * Point3D.Tolerance == float.MaxValue
                        // (at least for any reasonable tolerance value that we would use) because
                        // the small precision numbers get rounded away when you're dealing with a 
                        // very large number.  So it's possible that adding a number larger than our
                        // tolerance will return a number that is still within tolerance.  That's just
                        // the way floating point numbers work. So we'll just verify that adding a 
                        // Really Big Number to each of the coordinates makes it be no longer approximately
                        // equal.

                        float bigDistance = float.MaxValue / 2;

                        Point3D farAwayPointX = new Point3D(x + (x < 0 ? bigDistance : -bigDistance), y, z);
                        Point3D farAwayPointY = new Point3D(x, y + (y < 0 ? bigDistance : -bigDistance), z);
                        Point3D farAwayPointZ = new Point3D(x, y, z + (z < 0 ? bigDistance : -bigDistance));

                        NotApproximatelyEqualsTest(point, farAwayPointX);
                        NotApproximatelyEqualsTest(point, farAwayPointY);
                        NotApproximatelyEqualsTest(point, farAwayPointZ);


                    }
        }

        public void ApproximatelyEqualsTest(Point3D a, Point3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            // Verify that the two points are approximately equal
            Assert.IsTrue(a.ApproximatelyEquals(b), "Expected a.ApproximatelyEquals(b) to return true with parameters {0} and {1}.", a.ToString(), b.ToString());

            // Switch up the parameters and verify that it still works
            Assert.IsTrue(b.ApproximatelyEquals(a), "Expected b.ApproximatelyEquals(a) to return true with parameters {0} and {1}.", a.ToString(), b.ToString());
        }

        public void NotApproximatelyEqualsTest(Point3D a, Point3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            // Verify that the two points are NOT approximately equal
            Assert.IsFalse(a.ApproximatelyEquals(b), "Expected a.ApproximatelyEquals(b) to return false with parameters {0} and {1}.", a.ToString(), b.ToString());

            // Switch up the parameters and verify that it still works
            Assert.IsFalse(b.ApproximatelyEquals(a), "Expected b.ApproximatelyEquals(a) to return false with parameters {0} and {1}.", a.ToString(), b.ToString());

        }

        # endregion

        # region GetHashCode() Tests

        [Test]
        public void HashCodeConflictTest()
        {
            TestLogger.Log();

            int count = 0;
            Dictionary<int, int>codes = new Dictionary<int, int>();

            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        count++;

                        int hashCode = new Point3D(x, y, z).GetHashCode();

                        if (codes.ContainsKey(hashCode))
                            codes[hashCode]++;
                        else
                            codes.Add(hashCode, 1);
                    }

            // The hashing algorithm isn't particluarly good, but let's verify that it's at least doing
            // sort of what it should be.  Here we're making sure that at least 95% of the tested hash
            // codes are unique.
            Assert.IsTrue((count - codes.Count) < count / 20, "Expected at least 95% of hash codes to be unique.  Instead, there were only [{0}] unique hash codes out of [{1}] total points tested.", codes.Count.ToString(), count.ToString());
        }

        [Test]
        public void GetHashCodeTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        Point3D point1 = new Point3D(x, y, z);
                        Point3D point2 = new Point3D(x, y, z);

                        GetHashCodeTest(point1, point2);
                    }
        }

        public void GetHashCodeTest(Point3D a, Point3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "Expected Point a and Point b (which are equal) to have the same hash code, with parameters [{0}, {1}].", a.ToString(), b.ToString());
        }

        # endregion

        # region GetScreenPosition() Tests

        [Test]
        public void GetScreenPositionTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        // Here's the object that we're going to project.
                        // We'll try with a bunch of different camera positions.
                        Point3D objectPoint = new Point3D(x, y, z);

                        CameraPosition1Test(objectPoint);

                        CameraPosition2Test(objectPoint);
                    }
        }

        [Test]
        public void GetScreenPositionProjectOverflowTest()
        {
            // Try to project a point past float.MaxValue on the x axis
            GetScreenPositionProjectOverflowTest(new Point3D(float.MaxValue, 0, -3), new Point3D(float.MinValue, 0, -4));

            // Try to project a point past float.MinValue on the x axis
            GetScreenPositionProjectOverflowTest(new Point3D(float.MinValue, 0, -3), new Point3D(float.MaxValue, 0, -4));

            // Try to project a point past float.MaxValue on the y axis
            GetScreenPositionProjectOverflowTest(new Point3D(0, float.MaxValue, -3), new Point3D(0, float.MinValue, -4));

            // Try to project a point past float.MinValue on the y axis
            GetScreenPositionProjectOverflowTest(new Point3D(0, float.MinValue, -3), new Point3D(0, float.MaxValue, -4));
        }

        public void GetScreenPositionProjectOverflowTest(Point3D objectPoint, Point3D cameraPoint)
        {
            TestLogger.Log(objectPoint.ToString(), cameraPoint.ToString());

            bool exceptionThrown = false;

            try
            {
                objectPoint.GetScreenPosition(cameraPoint);
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected ArgumentOutOfRangeException, but no exception was thrown with parameters {0}, {1}", objectPoint.ToString(), cameraPoint.ToString());
        }

        [Test]
        public void GetScreenPositionInvalidZTest()
        {

            Point3D objectPoint = new Point3D();
            Point3D cameraPoint = new Point3D();

            // Verify that things fall apart when the object point's z coordinate is the 
            // same as the camera point's z coordinate.
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        bool failed = false;

                        try
                        {
                            objectPoint = new Point3D(x, y, z);
                            cameraPoint = new Point3D(-x, -y, z);

                            PointF projectedPoint = objectPoint.GetScreenPosition(cameraPoint);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            failed = true;
                        }

                        Assert.IsTrue(failed, "Expected an exception, but didn't get one, when projecting object point {0} with camera point {1}.", objectPoint.ToString(), cameraPoint.ToString());
                    }
        }

        private void CameraPosition1Test(Point3D objectPoint)
        {
            TestLogger.Log(objectPoint.ToString());

            // This test doesn't make sense if Z is zero, so if it is, we just return.
            if (objectPoint.Z == 0f)
                return;

            // This camera position is the inverse of the object position,
            // so when we project it, the projected point should pass through
            // the origin, [0,0].
            Point3D camera = new Point3D(-objectPoint.X, -objectPoint.Y, -objectPoint.Z);

            PointF projectedPoint = objectPoint.GetScreenPosition(camera);

            Assert.AreEqual(projectedPoint.X, 0f, Point3D.Tolerance, "Expected the projected point's X value to be approximately 0.  Instead, it was [{0}], with parameter {1}", projectedPoint.X.ToString(), objectPoint.ToString());
            Assert.AreEqual(projectedPoint.Y, 0f, Point3D.Tolerance, "Expected the projected point's Y value to be approximately 0.  Instead, it was [{0}], with parameter {1}", projectedPoint.X.ToString(), objectPoint.ToString());
        }

        private void CameraPosition2Test(Point3D objectPoint)
        {
            TestLogger.Log(objectPoint.ToString());

            // This test doesn't make sense if Z is zero, so if it is, we just return.
            if (objectPoint.Z == 0F)
                return;

            // This camera position is the same as the object postion, except with an 
            // inverse Z.  The projected point should equal the X,Y coordinates of the 
            // object point.
            Point3D camera = new Point3D(objectPoint.X, objectPoint.Y, -objectPoint.Z);

            PointF projectedPoint = objectPoint.GetScreenPosition(camera);

            Assert.AreEqual(projectedPoint.X, objectPoint.X, Point3D.Tolerance, "Expected the projected point's X value to be approximately [{0}].  Instead, it was [{1}], with parameter {2}", objectPoint.X.ToString(), projectedPoint.X.ToString(), objectPoint.ToString());
            Assert.AreEqual(projectedPoint.Y, objectPoint.Y, Point3D.Tolerance, "Expected the projected point's Y value to be approximately [{0}].  Instead, it was [{1}], with parameter {2}", objectPoint.Y.ToString(), projectedPoint.Y.ToString(), objectPoint.ToString());
        }

        # endregion
    }
}
