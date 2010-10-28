using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NUnit.Framework;
using CPI.Plot3D;

namespace Tests
{
    [TestFixture]
    public class Vector3DTests
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

        Point3D[] extremePoints = new Point3D[]{
            new Point3D(float.MinValue, 0, 0),
            new Point3D(float.MaxValue, 0, 0),
            new Point3D(0, float.MinValue, 0),
            new Point3D(0, float.MaxValue, 0),
            new Point3D(0, 0, float.MinValue),
            new Point3D(0, 0, float.MaxValue)
        };


        List<double> rotationAngles;

        Dictionary<Vector3D, double> knownVectorLengths;


        struct CrossProductResult
        {
            public readonly Vector3D A;
            public readonly Vector3D B;
            public readonly Vector3D Result;

            public CrossProductResult(Vector3D a, Vector3D b, Vector3D result)
            {
                this.A = a;
                this.B = b;
                this.Result = result;
            }
        }

        List<CrossProductResult> knownCrossProducts;

        # endregion 

        # region Setup Method

        [TestFixtureSetUp]
        public void VariableSetup()
        {
            # region Set up known vector lengths

            knownVectorLengths = new Dictionary<Vector3D, double>();

            # region All these values have a length of 0, 1, Sqrt(2), or Sqrt(3)
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; y <= 1; y++)
                        knownVectorLengths.Add(new Vector3D(x, y, z), Math.Sqrt(Math.Abs(x) + Math.Abs(y) + Math.Abs(z)));
            # endregion

            # region All these values have a length of Sqrt(1460.054404)

            double specificLength = Math.Sqrt(1460.054404);

            for (int xMult = -1; xMult <= 1; xMult += 2)
                for (int yMult = -1; yMult <= 1; yMult += 2)
                    for (int zMult = -1; zMult <= 1; zMult += 2)
                    {
                        knownVectorLengths.Add(new Vector3D(xMult * 15f, yMult * 34.752f, zMult * 5.23f), specificLength);
                        knownVectorLengths.Add(new Vector3D(xMult * 34.752f, yMult * 5.23f, zMult * 15f), specificLength);
                    }

            # endregion

            # region These values have a length of float.MaxValue

            knownVectorLengths.Add(new Vector3D(float.MaxValue, 0, 0), float.MaxValue);
            knownVectorLengths.Add(new Vector3D(float.MinValue, 0, 0), float.MaxValue);
            knownVectorLengths.Add(new Vector3D(0, float.MaxValue, 0), float.MaxValue);
            knownVectorLengths.Add(new Vector3D(0, float.MinValue, 0), float.MaxValue);
            knownVectorLengths.Add(new Vector3D(0, 0, float.MaxValue), float.MaxValue);
            knownVectorLengths.Add(new Vector3D(0, 0, float.MinValue), float.MaxValue);

            # endregion

            # region These values have a length of float.Epsilon * Math.Sqrt(3)

            // float.Epsilon * Math.Sqrt(3) cannot be accurately represented as a float, because it requires
            // more precision than a float has.  But since we calculate our lengths as doubles, we should have
            // plenty of precision to spare.
            knownVectorLengths.Add(new Vector3D(float.Epsilon, float.Epsilon, float.Epsilon), (double)float.Epsilon * Math.Sqrt(3));
            knownVectorLengths.Add(new Vector3D(-float.Epsilon, -float.Epsilon, -float.Epsilon), (double)float.Epsilon * Math.Sqrt(3));

            # endregion

            # region These values have a length of float.MaxValue * Math.Sqrt(3)

            // float.MaxValue * Math.Sqrt(3) cannot be represented as a float because a float doesn't
            // have enough precision.  But because we're using doubles for our lengths, this shouldn't
            // be any kind of problem.
            knownVectorLengths.Add(new Vector3D(float.MaxValue, float.MaxValue, float.MaxValue), (double)float.MaxValue * Math.Sqrt(3));
            knownVectorLengths.Add(new Vector3D(-float.MaxValue, -float.MaxValue, -float.MaxValue), (double)float.MaxValue * Math.Sqrt(3));

            # endregion

            # endregion

            # region Set up known cross products

            knownCrossProducts = new List<CrossProductResult>();

            knownCrossProducts.Add(new CrossProductResult(Vector3D.PositiveX, Vector3D.NegativeY, Vector3D.NegativeZ));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.NegativeY, Vector3D.NegativeX, Vector3D.NegativeZ));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.NegativeX, Vector3D.PositiveY, Vector3D.NegativeZ));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.PositiveY, Vector3D.PositiveX, Vector3D.NegativeZ));

            knownCrossProducts.Add(new CrossProductResult(Vector3D.PositiveX, Vector3D.NegativeZ, Vector3D.PositiveY));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.NegativeZ, Vector3D.NegativeX, Vector3D.PositiveY));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.NegativeX, Vector3D.PositiveZ, Vector3D.PositiveY));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.PositiveZ, Vector3D.PositiveX, Vector3D.PositiveY));

            knownCrossProducts.Add(new CrossProductResult(Vector3D.PositiveY, Vector3D.PositiveZ, Vector3D.PositiveX));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.PositiveZ, Vector3D.NegativeY, Vector3D.PositiveX));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.NegativeY, Vector3D.NegativeZ, Vector3D.PositiveX));
            knownCrossProducts.Add(new CrossProductResult(Vector3D.NegativeZ, Vector3D.PositiveY, Vector3D.PositiveX));

            # endregion

            # region Set up rotation angles

            rotationAngles = new List<double>(33);

            for (int i = 0; i <= 16; i++)
            {
                rotationAngles.Add(i * Math.PI / 8);
            }

            # endregion
        }

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

            // Instantiate a Vector3D using x, y, and z coordinates
            Vector3D vector = new Vector3D(x, y, z);

            Assert.AreEqual(x, vector.X, Vector3D.Tolerance, "vector.X [{0}] is not approximately equal to parameter x [{1}] when called with parameters [{2}, {3}, {4}].", vector.X.ToString(), x.ToString(), x.ToString(), y.ToString(), z.ToString());
            Assert.AreEqual(y, vector.Y, Vector3D.Tolerance, "vector.Y [{0}] is not approximately equal to parameter y [{1}] when called with parameters [{2}, {3}, {4}].", vector.Y.ToString(), y.ToString(), x.ToString(), y.ToString(), z.ToString());
            Assert.AreEqual(z, vector.Z, Vector3D.Tolerance, "vector.Z [{0}] is not approximately equal to parameter z [{1}] when called with parameters [{2}, {3}, {4}].", vector.Z.ToString(), z.ToString(), x.ToString(), y.ToString(), z.ToString());

            // Instantiate a Vector3D using a Point3D structure
            Vector3D vector2 = new Vector3D(new Point3D(x, y, z));

            Assert.AreEqual(vector, vector2, "Expected Vector3D(x, y, z) to yield the same Vector3D as Vector3D(Point3D(x, y, z)) when called with parameters [{0}, {1}, {2}].", x.ToString(), y.ToString(), z.ToString());
        }

        [Test]
        public void EmptyConstructorTest()
        {
            TestLogger.Log();

            Vector3D emptyConstructor = new Vector3D();

            Vector3D explicitZeroConstructor = new Vector3D(0f, 0f, 0f);

            Assert.AreEqual(emptyConstructor, explicitZeroConstructor, "Expected an empty constructor to yield the same vector as one expicitly instantiated with [0f, 0f, 0f]");
        }

        # endregion

        # region GetHashCode() Tests

        [Test]
        public void GetHashCodeTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        Vector3D vector = new Vector3D(x, y, z);
                        GetHashCodeTest(new Vector3D(x, y, z));
                    }
        }

        public void GetHashCodeTest(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            // A Vector3D's hash code is the binary complement of the hash code of its EndPoint property.
            Assert.AreEqual(vector.GetHashCode(), ~vector.EndPoint.GetHashCode(), "Expected Vector3D's hash code to be the binary complement of its EndPoint when called with parameter {0}.", vector.ToString());
        }

        # endregion

        # region ToString() Tests

        [Test]
        public void ToStringTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        Vector3D vector = new Vector3D(x, y, z);
                        ToStringTest(vector, vector.EndPoint.ToString());
                    }
        }

        [Test]
        public void ToStringRoundingTestGoodValues()
        {
            // Try all possible valid combinations of digits, because hey...what the hell?
            for (int digits = 0; digits <= 15; digits++)
                foreach (float x in goodValues)
                    foreach (float y in goodValues)
                        foreach (float z in goodValues)
                        {
                            Vector3D vector = new Vector3D(x, y, z);
                            ToStringRoundingTest(vector, digits, vector.EndPoint.ToString(digits));
                        }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToStringRoundingToNegativeOneTest()
        {
            ToStringRoundingTest(new Vector3D(goodValues[3], goodValues[1], goodValues[2]), -1, "Actually, we expect to throw an exception");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToStringRoundingToSixteenTest()
        {
            ToStringRoundingTest(new Vector3D(goodValues[3], goodValues[4], goodValues[5]), 16, "Actually, we expect to throw an exception");
        }

        public void ToStringTest(Vector3D vector, string expectedValue)
        {
            TestLogger.Log(vector.ToString(), expectedValue.ToString());

            string toString = vector.ToString();
            Assert.AreEqual(toString, expectedValue, "Expected ToString() to return [{0}], instead got [{1}], when called with point [{2},{3},{4}].", expectedValue, toString, vector.X.ToString(), vector.Y.ToString(), vector.Z.ToString());
        }

        public void ToStringRoundingTest(Vector3D vector, int digits, string expectedValue)
        {
            TestLogger.Log(vector.ToString(), digits.ToString(), expectedValue.ToString());

            string toString = vector.ToString(digits);
            Assert.AreEqual(toString, expectedValue, "Expected ToString() to return [{0}], instead got [{1}], when called with vector [{2},{3},{4}] and [{5}] digits.", expectedValue, toString, vector.X.ToString(), vector.Y.ToString(), vector.Z.ToString(), digits.ToString());
        }

        # endregion

        # region Coordinate Property Tests

        [Test]
        public void CoordinatePropertyTests()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        Vector3D vector = new Vector3D(x, y, z);
                        CoordinatePropertyTest(vector, x, y, z);
                    }
        }

        public void CoordinatePropertyTest(Vector3D vector, float expectedX, float expectedY, float expectedZ)
        {
            TestLogger.Log(vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());

            Assert.AreEqual(vector.X, expectedX, Vector3D.Tolerance, "Expected vector.X to equal expectedX parameter when called with parameters [{0}, {1}, {2}, {3}].", vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
            Assert.AreEqual(vector.Y, expectedY, Vector3D.Tolerance, "Expected vector.Y to equal expectedY parameter when called with parameters [{0}, {1}, {2}, {3}].", vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
            Assert.AreEqual(vector.Z, expectedZ, Vector3D.Tolerance, "Expected vector.Z to equal expectedZ parameter when called with parameters [{0}, {1}, {2}, {3}].", vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());

            Point3D point = new Point3D(expectedX, expectedY, expectedZ);

            Assert.AreEqual(vector.EndPoint, point, "Expected vector.EndPoint to equal Point3D(expectedX, expectedY, expectedZ) when called with parameters [{0}, {1}, {2}, {3}].", vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
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
                        Vector3D vector1 = new Vector3D(x, y, z);

                        // vector2 should be equal to vector1
                        Vector3D vector2 = new Vector3D(x, y, z);

                        EqualsTest(vector1, vector2);

                        // vector3's x value is off, and should not be equal to vector1
                        Vector3D vector3 = new Vector3D(x + (x < 0 ? (float.MaxValue / 2) : (float.MinValue / 2)), y, z);
                        NotEqualsTest(vector1, vector3);

                        // vector4's y value is off, and should not be equal to vector1
                        Vector3D vector4 = new Vector3D(x, y + (y < 0 ? (float.MaxValue / 2) : (float.MinValue / 2)), z);
                        NotEqualsTest(vector1, vector4);

                        // vector5's z value is off, and should not be equal to vector1
                        Vector3D vector5 = new Vector3D(x, y, z + (z < 0 ? (float.MaxValue / 2) : (float.MinValue / 2)));
                        NotEqualsTest(vector1, vector5);
                    }
        }


        public void EqualsTest(Vector3D a, Vector3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            // check IEquatable<Vector3D>.Equals(Vector3D)
            Assert.IsTrue(a.Equals(b), "Expected IEquatable<Vector3D>.Equals(Vector3D) to return true when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsTrue(b.Equals(a), "Expected IEquatable<Vector3D>.Equals(Vector3D) to return true when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check Vector3D.Equals(object)
            Assert.IsTrue(a.Equals((object)b), "Expected Vector3D.Equals(object) to return true when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsTrue(b.Equals((object)a), "Expected Vector3D.Equals(object) to return true when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check op_equality(Vector3D, Vector3D)
            Assert.IsTrue(a == b, "Expected op_equality(Vector3D, Vector3D) to return true when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsTrue(b == a, "Expected op_equality(Vector3D, Vector3D) to return true when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check op_inequality(Vector3D, Vector3D)
            Assert.IsFalse(a != b, "Expected op_inequality(Vector3D, Vector3D) to return false when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsFalse(b != a, "Expected op_inequality(Vector3D, Vector3D) to return false when called with parameters [{0}, {1}].", b.ToString(), a.ToString());
        }

        public void NotEqualsTest(Vector3D a, Vector3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            // check IEquatable<Vector3D>.Equals(Vector3D)
            Assert.IsFalse(a.Equals(b), "Expected IEquatable<Vector3D>.Equals(Vector3D) to return false when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsFalse(b.Equals(a), "Expected IEquatable<Vector3D>.Equals(Vector3D) to return false when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check Vector3D.Equals(object)
            Assert.IsFalse(a.Equals((object)b), "Expected Vector3D.Equals(object) to return false when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsFalse(b.Equals((object)a), "Expected Vector3D.Equals(object) to return false when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check Vector3D.Equals(object) with a non Vector3D parameter.
            // In this case we'll test a Point3D instance.
            Assert.IsFalse(a.Equals(new Point3D(a.X, a.Y, a.Z)));

            // check op_equality(Vector3D, Vector3D)
            Assert.IsFalse(a == b, "Expected op_equality(Vector3D, Vector3D) to return false when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsFalse(b == a, "Expected op_equality(Vector3D, Vector3D) to return false when called with parameters [{0}, {1}].", b.ToString(), a.ToString());

            // check op_inequality(Vector3D, Vector3D)
            Assert.IsTrue(a != b, "Expected op_inequality(Vector3D, Vector3D) to return true when called with parameters [{0}, {1}].", a.ToString(), b.ToString());

            // check with reverse parameters
            Assert.IsTrue(b != a, "Expected op_inequality(Vector3D, Vector3D) to return true when called with parameters [{0}, {1}].", b.ToString(), a.ToString());
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
                        Vector3D vector = new Vector3D(x, y, z);

                        // This represents 90% of tolerance.  If we add or subtract
                        // this value from our points, we should still be approximately
                        // equal.
                        float nearToleranceLevel = Vector3D.Tolerance * .9f;

                        Vector3D nearbyVector = new Vector3D(
                            x + (x < 0 ? nearToleranceLevel : -nearToleranceLevel),
                            y + (y < 0 ? nearToleranceLevel : -nearToleranceLevel),
                            z + (z < 0 ? nearToleranceLevel : -nearToleranceLevel)
                        );

                        ApproximatelyEqualsTest(vector, nearbyVector);

                        // You might be thinking, let's add 110% of the tolerance and verify that 
                        // the resulting vector is NOT approximately equal.  Well, we can't, because
                        // of precision problems with really big numbers.  For example,
                        // float.MaxValue + 2 * Vector3D.Tolerance == float.MaxValue
                        // (at least for any reasonable tolerance value that we would use) because
                        // the small precision numbers get rounded away when you're dealing with a 
                        // very large number.  So it's possible that adding a number larger than our
                        // tolerance will return a number that is still within tolerance.  That's just
                        // the way floating point numbers work. So we'll just verify that adding a 
                        // Really Big Number to each of the coordinates makes it be no longer approximately
                        // equal.

                        float bigDistance = float.MaxValue / 2;

                        Vector3D farAwayVectorX = new Vector3D(x + (x < 0 ? bigDistance : -bigDistance), y, z);
                        NotApproximatelyEqualsTest(vector, farAwayVectorX);

                        Vector3D farAwayVectorY = new Vector3D(x, y + (y < 0 ? bigDistance : -bigDistance), z);
                        NotApproximatelyEqualsTest(vector, farAwayVectorY);

                        Vector3D farAwayVectorZ = new Vector3D(x, y, z + (z < 0 ? bigDistance : -bigDistance));
                        NotApproximatelyEqualsTest(vector, farAwayVectorZ);
                    }
        }

        public void ApproximatelyEqualsTest(Vector3D a, Vector3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            // Verify that the two vectors are approximately equal
            Assert.IsTrue(a.ApproximatelyEquals(b), "Expected a.ApproximatelyEquals(b) to return true with parameters {0} and {1}.", a.ToString(), b.ToString());

            // Switch up the parameters and verify that it still works
            Assert.IsTrue(b.ApproximatelyEquals(a), "Expected b.ApproximatelyEquals(a) to return true with parameters {0} and {1}.", a.ToString(), b.ToString());
        }

        public void NotApproximatelyEqualsTest(Vector3D a, Vector3D b)
        {
            TestLogger.Log(a.ToString(), b.ToString());

            // Verify that the two points are NOT approximately equal
            Assert.IsFalse(a.ApproximatelyEquals(b), "Expected a.ApproximatelyEquals(b) to return false with parameters {0} and {1}.", a.ToString(), b.ToString());

            // Switch up the parameters and verify that it still works
            Assert.IsFalse(b.ApproximatelyEquals(a), "Expected b.ApproximatelyEquals(a) to return false with parameters {0} and {1}.", a.ToString(), b.ToString());

        }

        # endregion

        # region Addition Tests

        [Test]
        public void AdditionTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        foreach (Point3D point in extremePoints)
                        {
                            Vector3D vector1 = new Vector3D(x, y, z);

                            // Vector and point addition
                            AdditionTest(point, vector1, x + point.X, y + point.Y, z + point.Z);

                            // Vector and vector addition
                            Vector3D vector2 = new Vector3D(point);
                            AdditionTest(vector1, vector2, vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z); 
                        }
        }

        public void AdditionTest(Point3D point, Vector3D vector, float expectedX, float expectedY, float expectedZ)
        {
            TestLogger.Log(point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());

            bool expectException = (
                float.IsInfinity(expectedX) || float.IsNaN(expectedX)
                || float.IsInfinity(expectedY) || float.IsNaN(expectedY)
                || float.IsInfinity(expectedZ) || float.IsNaN(expectedZ)
            );


            bool gotException = false;

            try
            {
                Point3D result = point + vector;

                Assert.AreEqual(result.X, expectedX, "Expected the X coordinate of Point3D + Vector3D to equal expectedX when called with parameters [{0}, {1}, {2}, {3}, {4}].", point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
                Assert.AreEqual(result.Y, expectedY, "Expected the Y coordinate of Point3D + Vector3D to equal expectedY when called with parameters [{0}, {1}, {2}, {3}, {4}].", point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
                Assert.AreEqual(result.Z, expectedZ, "Expected the Z coordinate of Point3D + Vector3D to equal expectedZ when called with parameters [{0}, {1}, {2}, {3}, {4}].", point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
            }
            catch (OverflowException)
            {
                gotException = true;
            }

            if (expectException)
                Assert.IsTrue(gotException, "Expected an OverflowException in Point3D + Vector3D, when called with parameters [{0}, {1}, {2}, {3}, {4}]", point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
        }

        public void AdditionTest(Vector3D vector1, Vector3D vector2, float expectedX, float expectedY, float expectedZ)
        {
            TestLogger.Log(vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());

            bool expectException = (
                float.IsInfinity(expectedX) || float.IsNaN(expectedX) 
                || float.IsInfinity(expectedY) || float.IsNaN(expectedY) 
                || float.IsInfinity(expectedZ) || float.IsNaN(expectedZ)
            );

            bool gotException = false;

            try
            {
                Vector3D result = vector1 + vector2;

                Assert.AreEqual(result.X, expectedX, "Expected the X coordinate of Vector3D + Vector3D to equal expectedX when called with parameters [{0}, {1}, {2}, {3}, {4}].", vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
                Assert.AreEqual(result.Y, expectedY, "Expected the Y coordinate of Vector3D + Vector3D to equal expectedY when called with parameters [{0}, {1}, {2}, {3}, {4}].", vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
                Assert.AreEqual(result.Z, expectedZ, "Expected the Z coordinate of Vector3D + Vector3D to equal expectedZ when called with parameters [{0}, {1}, {2}, {3}, {4}].", vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
            }
            catch (OverflowException)
            {
                gotException = true;
            }

            if (expectException)
                Assert.IsTrue(gotException, "Expected an OverflowException in Vector3D + Vector3D, when called with parameters [{0}, {1}, {2}, {3}, {4}]", vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());

        }

        # endregion

        # region Subtraction Tests

        [Test]
        public void SubtractionTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        foreach (Point3D point in extremePoints)
                        {
                            Vector3D vector1 = new Vector3D(x, y, z);

                            // Vector and point subtraction
                            SubtractionTest(point, vector1, point.X - x, point.Y - y, point.Z - z);

                            // Vector and vector subtraction
                            Vector3D vector2 = new Vector3D(point);

                            SubtractionTest(vector1, vector2, vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z);
                            SubtractionTest(vector2, vector1, vector2.X - vector1.X, vector2.Y - vector1.Y, vector2.Z - vector1.Z);
                        }
        }

        public void SubtractionTest(Point3D point, Vector3D vector, float expectedX, float expectedY, float expectedZ)
        {
            TestLogger.Log(point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());

            bool expectException = (
                float.IsInfinity(expectedX) || float.IsNaN(expectedX)
                || float.IsInfinity(expectedY) || float.IsNaN(expectedY)
                || float.IsInfinity(expectedZ) || float.IsNaN(expectedZ)
            );


            bool gotException = false;

            try
            {
                Point3D result = point - vector;

                Assert.AreEqual(result.X, expectedX, "Expected the X coordinate of Point3D - Vector3D to equal expectedX when called with parameters [{0}, {1}, {2}, {3}, {4}].", point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
                Assert.AreEqual(result.Y, expectedY, "Expected the Y coordinate of Point3D - Vector3D to equal expectedY when called with parameters [{0}, {1}, {2}, {3}, {4}].", point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
                Assert.AreEqual(result.Z, expectedZ, "Expected the Z coordinate of Point3D - Vector3D to equal expectedZ when called with parameters [{0}, {1}, {2}, {3}, {4}].", point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
            }
            catch (OverflowException)
            {
                gotException = true;
            }

            if (expectException)
                Assert.IsTrue(gotException, "Expected an OverflowException in Point3D - Vector3D, when called with parameters [{0}, {1}, {2}, {3}, {4}]", point.ToString(), vector.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());

        }

        public void SubtractionTest(Vector3D vector1, Vector3D vector2, float expectedX, float expectedY, float expectedZ)
        {
            TestLogger.Log(vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());

            bool expectException = (
                float.IsInfinity(expectedX) || float.IsNaN(expectedX)
                || float.IsInfinity(expectedY) || float.IsNaN(expectedY)
                || float.IsInfinity(expectedZ) || float.IsNaN(expectedZ)
            );

            bool gotException = false;

            try
            {
                Vector3D result = vector1 - vector2;

                Assert.AreEqual(result.X, expectedX, "Expected the X coordinate of Vector3D - Vector3D to equal expectedX when called with parameters [{0}, {1}, {2}, {3}, {4}].", vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
                Assert.AreEqual(result.Y, expectedY, "Expected the Y coordinate of Vector3D - Vector3D to equal expectedY when called with parameters [{0}, {1}, {2}, {3}, {4}].", vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
                Assert.AreEqual(result.Z, expectedZ, "Expected the Z coordinate of Vector3D - Vector3D to equal expectedZ when called with parameters [{0}, {1}, {2}, {3}, {4}].", vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
            }
            catch (OverflowException)
            {
                gotException = true;
            }

            if (expectException)
                Assert.IsTrue(gotException, "Expected an OverflowException in Vector3D - Vector3D, when called with parameters [{0}, {1}, {2}, {3}, {4}]", vector1.ToString(), vector2.ToString(), expectedX.ToString(), expectedY.ToString(), expectedZ.ToString());
        }

        # endregion

        # region Length Tests

        [Test]
        public void LengthTestsAgainstKnownValues()
        {
            foreach (KeyValuePair<Vector3D, double> kvp in knownVectorLengths)
            {
                LengthTest(kvp.Key, kvp.Value);
            }
        }

        public void LengthTest(Vector3D vector, double expectedLength)
        {
            TestLogger.Log(vector.ToString(), expectedLength.ToString());

            Assert.AreEqual(vector.Length, expectedLength, Vector3D.Tolerance, "vector.Length does not match expectedLength with parameters [{0}, {1}].", vector.ToString(), expectedLength.ToString());
        }

        # endregion

        # region Normalization Tests

        [Test]
        public void NormalizationTestsAgainstKnownValues()
        {
            foreach (KeyValuePair<Vector3D, double> kvp in knownVectorLengths)
            {
                NormalizationTest(kvp.Key);
            }
        }

        public void NormalizationTest(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            Vector3D normalizedVector = vector.Normalize();

            // verify that the vector's length is now 1
            Assert.AreEqual(normalizedVector.Length, 1.0, Vector3D.Tolerance, "Expected length of normalized vector to be 1, when called with parameter {0}.", vector.ToString());

            // Now, to verify that the normalized vector has the same direction as the original, we'll
            // multiply each of the normalized coordinates by the length of the original vector, and make
            // sure that the resulting vector approximately equals the original.  Follow me?
            Vector3D extendedVector = new Vector3D(
                (float)(normalizedVector.X * vector.Length),
                (float)(normalizedVector.Y * vector.Length),
                (float)(normalizedVector.Z * vector.Length)
            );

            Assert.IsTrue(extendedVector.ApproximatelyEquals(vector), "Expected extended normalized vector to be approximately equal to the original vector when called with parameter {0}.", vector.ToString());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NormalizeZeroVectorTest()
        {
            TestLogger.Log();

            Vector3D zeroVector = new Vector3D(0, 0, 0);

            zeroVector.Normalize();
        }

        # endregion

        # region Scalar Multiplication Tests

        [Test]
        public void ScalarMultiplicationTestsGoodValues()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        foreach (float multiplier in goodValues)
                        {
                            ScalarMultiplicationTestGoodValues(new Vector3D(x, y, z), multiplier);
                        }

        }

        [Test]
        public void ScalarMultiplicationTestsBadValues()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        foreach (float multiplier in badValues)
                        {
                            ScalarMultiplicationTestBadValues(new Vector3D(x, y, z), multiplier);
                        }

        }

        public void ScalarMultiplicationTestBadValues(Vector3D vector, double multiplier)
        {
            TestLogger.Log(vector.ToString(), multiplier.ToString());

            bool caughtException = false;

            try
            {
                Vector3D multiplied = vector * multiplier;
            }
            catch (ArgumentException)
            {
                caughtException = true;
            }

            Assert.IsTrue(caughtException, "Expected ArgumentException but didn't get one when called with [{0}, {1}].", vector.ToString(), multiplier.ToString());
        }


        public void ScalarMultiplicationTestGoodValues(Vector3D vector, double multiplier)
        {
            TestLogger.Log(vector.ToString(), multiplier.ToString());

            float newX = (float)(multiplier * vector.X);
            float newY = (float)(multiplier * vector.Y);
            float newZ = (float)(multiplier * vector.Z);

            bool expectException = false;
            bool caughtException = false;

            if (float.IsInfinity(newX) || float.IsNaN(newX)
                || float.IsInfinity(newY) || float.IsNaN(newY)
                || float.IsInfinity(newZ) || float.IsNaN(newZ))
            {
                expectException = true;
            }

            try
            {
                Vector3D multiplied = vector * multiplier;

                Assert.AreEqual(multiplied.X, newX, Vector3D.Tolerance, "Expected X coordinate of multiplied vector to equal original X coordinate multiplied by the multiplier, when called with parameters [{0}, {1}].", vector.ToString(), multiplier.ToString());
                Assert.AreEqual(multiplied.Y, newY, Vector3D.Tolerance, "Expected Y coordinate of multiplied vector to equal original Y coordinate multiplied by the multiplier, when called with parameters [{0}, {1}].", vector.ToString(), multiplier.ToString());
                Assert.AreEqual(multiplied.Z, newZ, Vector3D.Tolerance, "Expected Z coordinate of multiplied vector to equal original Z coordinate multiplied by the multiplier, when called with parameters [{0}, {1}].", vector.ToString(), multiplier.ToString());

                // Test that vector * double == double * vector
                Assert.IsTrue(multiplied.ApproximatelyEquals(multiplier * vector), "Expected Vector3D * double == double * Vector3D when called with parameters [{0}, {1}].", vector.ToString(), multiplier.ToString());
            }
            catch (ArgumentException)
            {
                caughtException = true;
            }

            if (expectException)
                Assert.IsTrue(caughtException, "Expected an exception but didn't get one when called with parameters [{0}, {1}].", vector.ToString(), multiplier.ToString());
            else
                Assert.IsFalse(caughtException, "Unexpected ArgumentException when called with parameters [{0}, {1}].", vector.ToString(), multiplier.ToString());
        }



        # endregion

        # region Scalar Division Tests

        [Test]
        public void ScalarDivisionTestGoodValues()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        foreach (float divisor in goodValues)
                        {
                            // 0 is not a valid divisor
                            if (divisor == 0f)
                                continue;

                            ScalarDivisionTestGoodValues(new Vector3D(x, y, z), divisor);
                        }
        }

        public void ScalarDivisionTestGoodValues(Vector3D vector, double divisor)
        {
            TestLogger.Log(vector.ToString(), divisor.ToString());

            float newX = (float)(vector.X / divisor);
            float newY = (float)(vector.Y / divisor);
            float newZ = (float)(vector.Z / divisor);

            bool expectException = false;
            bool caughtException = false;

            if (float.IsInfinity(newX) || float.IsNaN(newX)
                || float.IsInfinity(newY) || float.IsNaN(newY)
                || float.IsInfinity(newZ) || float.IsNaN(newZ))
            {
                expectException = true;
            }

            try
            {
                Vector3D divided = vector / divisor;

                Assert.AreEqual(divided.X, newX, Vector3D.Tolerance, "Expected X coordinate of divided vector to equal original X coordinate multiplied by the multiplier, when called with parameters [{0}, {1}].", vector.ToString(), divisor.ToString());
                Assert.AreEqual(divided.Y, newY, Vector3D.Tolerance, "Expected Y coordinate of divided vector to equal original Y coordinate multiplied by the multiplier, when called with parameters [{0}, {1}].", vector.ToString(), divisor.ToString());
                Assert.AreEqual(divided.Z, newZ, Vector3D.Tolerance, "Expected Z coordinate of divided vector to equal original Z coordinate multiplied by the multiplier, when called with parameters [{0}, {1}].", vector.ToString(), divisor.ToString());
            }
            catch (ArgumentException)
            {
                caughtException = true;
            }

            if (expectException)
                Assert.IsTrue(caughtException, "Expected an exception but didn't get one when called with parameters [{0}, {1}].", vector.ToString(), divisor.ToString());
            else
                Assert.IsFalse(caughtException, "Unexpected ArgumentException when called with parameters [{0}, {1}].", vector.ToString(), divisor.ToString());

        }

        [Test]
        public void ScalarDivisionByZeroTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        ScalarDivisionByZeroTest(new Vector3D(x, y, z));
        }


        public void ScalarDivisionByZeroTest(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            bool caughtException = false;

            // This should ALWAYS fail.
            try
            {
                Vector3D divided = vector / 0f;
            }
            catch (DivideByZeroException)
            {
                caughtException = true;
            }

            Assert.IsTrue(caughtException, "Expected division by zero to throw an exception when called with parameter {0}.", vector.ToString());
        }

        [Test]
        public void ScalarDivisionByInfinityTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        ScalarDivisionByInfinityTest(new Vector3D(x, y, z));
        }

        public void ScalarDivisionByInfinityTest(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            Vector3D dividedByPositiveInfinity = vector / double.PositiveInfinity;
            Vector3D dividedByNegativeInfinity = vector / double.NegativeInfinity;

            Assert.AreEqual(dividedByPositiveInfinity, Vector3D.Zero, "Expected to get a zero vector when dividing any vector by positive infinity, when called with parameter {0}.", vector.ToString());
            Assert.AreEqual(dividedByNegativeInfinity, Vector3D.Zero, "Expected to get a zero vector when dividing any vector by negative infinity, when called with parameter {0}.", vector.ToString());
        }

        [Test]
        public void ScalarDivisionByNaNTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        ScalarDivisionByNaNTest(new Vector3D(x, y, z));
        }

        public void ScalarDivisionByNaNTest(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            bool caughtException = false;

            try
            {
                Vector3D dividedByPositiveInfinity = vector / double.NaN;
            }
            catch (ArgumentException)
            {
                caughtException = true;
            }

            Assert.IsTrue(caughtException, "Expected an ArgumentException when dividing by NaN, when called with parameter {0}.", vector.ToString());
        }

        # endregion

        # region Unary Negation Tests

        [Test]
        public void UnaryNegationTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        ScalarDivisionByNaNTest(new Vector3D(x, y, z));

        }

        public void UnaryNegationTest(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            Vector3D negatedVector = -vector;

            Assert.AreEqual(vector.X * -1, negatedVector.X, Vector3D.Tolerance, "Expected X coordinate of negated vector to be -1 * vector.X, with parameter {0}.", vector.ToString());
            Assert.AreEqual(vector.Y * -1, negatedVector.Y, Vector3D.Tolerance, "Expected Y coordinate of negated vector to be -1 * vector.Y, with parameter {0}.", vector.ToString());
            Assert.AreEqual(vector.Z * -1, negatedVector.Z, Vector3D.Tolerance, "Expected Z coordinate of negated vector to be -1 * vector.Z, with parameter {0}.", vector.ToString());
        }

        # endregion

        # region Cross Product Tests

        [Test]
        public void CrossProductTestKnownValues()
        {
            foreach (CrossProductResult c in knownCrossProducts)
            {
                CrossProductTestKnownValues(c.A, c.B, c.Result);
                CrossProductTestKnownValues(c.B, c.A, -c.Result);
            }
        }

        public void CrossProductTestKnownValues(Vector3D vectorA, Vector3D vectorB, Vector3D expectedResult)
        {
            TestLogger.Log(vectorA.ToString(), vectorB.ToString(), expectedResult.ToString());

            Assert.IsTrue(vectorA.CrossProduct(vectorB).ApproximatelyEquals(expectedResult), "Expected vectorA.CrossProduct(vectorB) to equal expectedResult when called with parameters [{0}, {1}, {2}].", vectorA.ToString(), vectorB.ToString(), expectedResult.ToString());
        }

        [Test]
        public void CrossProductTestZeroVector()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        CrossProductTestZeroVector(new Vector3D(x, y, z));
                    }
        }

        public void CrossProductTestZeroVector(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            Assert.IsTrue(vector.CrossProduct(Vector3D.Zero).ApproximatelyEquals(Vector3D.Zero), "Expected vector.CrossProduct(Vector3D.Zero) to equal a zero vector when called with parameter {0}.", vector.ToString());

            Assert.IsTrue(Vector3D.Zero.CrossProduct(vector).ApproximatelyEquals(Vector3D.Zero), "Expected Vector3D.Zero.CrossProduct(vector) to equal a zero vector when called with parameter {0}.", vector.ToString());
        }

        [Test]
        public void CrossProductTestInverse()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        CrossProductTestInverse(new Vector3D(x, y, z));
                    }
        }

        public void CrossProductTestInverse(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            Assert.IsTrue(vector.CrossProduct(-vector).ApproximatelyEquals(Vector3D.Zero), "Expected vector.CrossProduct(-vector) to equal a zero vector when called with parameter {0}.", vector.ToString());
        }

        # endregion

        # region Dot Product Tests

        [Test]
        public void DotProductTestRightAngles()
        {
            foreach (CrossProductResult result in knownCrossProducts)
            {
                DotProductTestGoodValues(result.A, result.B, Math.PI / 2);

                DotProductTestGoodValues(result.A * 8.32, result.B / 4.12, Math.PI / 2);

                DotProductTestGoodValues(result.A, -result.A, Math.PI);
            }
        }

        /// <summary>
        /// We're going to test the dot product using a geometric interpretation of just what exactly
        /// the dot product is.  Effectively, the angle between two vectors can be calcualted like so:
        /// 
        /// double angle = Math.Acos((vectorA * vectorB) / (vectorA.Length * vectorB.Length));
        /// 
        /// So we give this function some vectors and the angle between them, and we make sure our 
        /// calculation comes up with the same answer.
        /// 
        /// This falls apart when one or the other is a zero vector, though, because angles become 
        /// meaningless in that situation.  So we only use this function to test non-zero vectors.
        /// </summary>
        public void DotProductTestGoodValues(Vector3D vectorA, Vector3D vectorB, double expectedAngle)
        {
            TestLogger.Log(vectorA.ToString(), vectorB.ToString(), expectedAngle.ToString());

            double cos = (vectorA * vectorB) / (vectorA.Length * vectorB.Length);
            double angle = Math.Acos(cos);

            Assert.AreEqual(angle, expectedAngle, Vector3D.Tolerance, "Expected angle to equal expectedAngle when called with parameters [{0}, {1}, {2}].", vectorA.ToString(), vectorB.ToString(), expectedAngle.ToString());
        }

        [Test]
        public void DotProductTestZeroVector()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                    {
                        DotProductTestZeroVector(new Vector3D(x, y, z));
                    }

        }

        public void DotProductTestZeroVector(Vector3D vector)
        {
            TestLogger.Log(vector.ToString());

            double dotProduct = vector * Vector3D.Zero;

            Assert.IsTrue(dotProduct == 0.0, "Expected vector * Vector3D.Zero to be 0.0 when called with parameter {0}.", vector.ToString());

            Assert.IsTrue((vector * Vector3D.Zero) == (Vector3D.Zero * vector), "Expected vector * Vector3D.Zero to equal Vector3D.Zero * vector when called with parameter {0}.", vector.ToString());
        }

        # endregion

        # region Rotation Tests

        [Test]
        public void RotateTest()
        {
            foreach (float x in goodValues)
                foreach (float y in goodValues)
                    foreach (float z in goodValues)
                        foreach (double rotationAngle in rotationAngles)
                        {
                            // You can't rotate around a zero vector.  That'd be crazy.
                            if (x == 0f && y == 0f && z == 0f)
                                continue;
                                
                            RotateTest(new Vector3D(15, -313, 1123), new Vector3D(x, y, z), rotationAngle);
                        }
        }

        public void RotateTest(Vector3D vectorToRotate, Vector3D rotationAxis, double angle)
        {
            TestLogger.Log(vectorToRotate.ToString(), rotationAxis.ToString(), angle.ToString());

            Vector3D rotatedVector = vectorToRotate.Rotate(rotationAxis, angle);

            // Make sure the rotated vector's length is the same as the original's.
            // (Rotation is kind of a sloppy calculation by nature, so we're using 
            // a larger-than-normal tolerance for error here.)
            Assert.AreEqual(vectorToRotate.Length, rotatedVector.Length, .001, "Expected the rotated vector's length to be equal to the original vector's length when called with parameters [{0}, {1}, {2}].", vectorToRotate.ToString(), rotationAxis.ToString(), angle.ToString());

            // Here's what we're going to to.  We're going to get the cross product of the original vector
            // and the rotation axis, then we're going to get the cross product of the rotated vector and 
            // the rotation axis.  Then we're going to figure out the angle between the two cross products
            // using the dot product.  That angle should be the same as the rotation angle.

            Vector3D originalCrossProduct = vectorToRotate.CrossProduct(rotationAxis.Normalize()).Normalize();
            Vector3D rotatedCrossProduct = rotatedVector.CrossProduct(rotationAxis.Normalize()).Normalize();

            // Seriously, this math is very sloppy, so we're rounding to six digits here to save ourselves
            // some trouble.  We just want to be sure that we're coming reasonably close to the answer we
            // expect.
            double computedAngle = Math.Acos(Math.Round(originalCrossProduct * rotatedCrossProduct, 6));

            double normalizedAngle = angle;

            while (normalizedAngle >= 2 * Math.PI)
            {
                normalizedAngle -= 2 * Math.PI;
            }

            while (normalizedAngle < 0.0)
            {
                normalizedAngle += 2 * Math.PI;
            }

            if (normalizedAngle > Math.PI)
                computedAngle = 2 * Math.PI - computedAngle;

            Assert.AreEqual(normalizedAngle, computedAngle, .001, "Expected computed rotation angle to equal actual rotation angle when called with parameters [{0}, {1}, {2}].", vectorToRotate.ToString(), rotationAxis.ToString(), angle.ToString());

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RotateAroundZeroVectorTest()
        {
            TestLogger.Log();

            Vector3D.PositiveZ.Rotate(Vector3D.Zero, (float)Math.PI);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RotateByPositiveInfinityTest()
        {
            TestLogger.Log();

            Vector3D.PositiveX.Rotate(Vector3D.PositiveY, double.PositiveInfinity);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RotateByNegativeInfinityTest()
        {
            TestLogger.Log();

            Vector3D.PositiveX.Rotate(Vector3D.PositiveY, double.NegativeInfinity);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RotateByNaNTest()
        {
            TestLogger.Log();

            Vector3D.PositiveX.Rotate(Vector3D.PositiveY, double.NaN);
        }

        # endregion

     }
}
