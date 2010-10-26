/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */
using System;
using NUnit.Framework;
using Geotools.Geometries;
using Geotools.SystemTests.TestRunner;
namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	[TestFixture]
	public class SystemTests
	{
		string _usiDir;
		string _vividTests;
		string _validateTests;

		public SystemTests()
		{
			string testRoot = @"C:\CC\GeotoolsNetCVSBuild\GeotoolsNet\";

			_usiDir= testRoot + @"Geotools.SystemTests\Tests\UrbanScience\";
			_vividTests= testRoot+ @"Geotools.SystemTests\Tests\vivid\";
			_validateTests = testRoot+ @"Geotools.SystemTests\Tests\validate\";
		}

		#region Vidid system tests
		public void TestVividTestBoundary()
		{
			PerformSystemTest(_vividTests+@"TestBoundary");
		}
		public void TestVividTestConvexHull()
		{
			PerformSystemTest(_vividTests+@"TestConvexHull");
		}
		public void TestVividTestFunctionAA()
		{
			PerformSystemTest(_vividTests+@"TestFunctionAA");
		}
		public void TestVividTestFunctionAAPrec()
		{
			PerformSystemTest(_vividTests+@"TestFunctionAAPrec");
		}
		public void TestVividestFunctionLA()
		{
			PerformSystemTest(_vividTests+@"TestFunctionLA");
		}
		public void TestVividTestFunctionLAPrec()
		{
			PerformSystemTest(_vividTests+@"TestFunctionLAPrec");
		}
		public void TestVividTestFunctionLL()
		{
			PerformSystemTest(_vividTests+@"TestFunctionLL");
		}
		public void TestVividTestFunctionLLPrec()
		{
			PerformSystemTest(_vividTests+@"TestFunctionLLPrec");
		}
		
		public void TestVividTestFunctionPL()
		{
			PerformSystemTest(_vividTests+@"TestFunctionPL");
		}
		public void TestVividTestFunctionPLPrec()
		{
			PerformSystemTest(_vividTests+@"TestFunctionPLPrec");
		}
		public void TestVividTestFunctionPP()
		{
			PerformSystemTest(_vividTests+@"TestFunctionPP");
		}
		public void TestVividTestRelateAA()
		{
			PerformSystemTest(_vividTests+@"TestRelateAA");
		}
		public void TestVividTestRelateAC()
		{
			PerformSystemTest(_vividTests+@"TestRelateAC");
		}
		public void TestVividTestRelateLA()
		{
			PerformSystemTest(_vividTests+@"TestRelateLA");
		}
		public void TestVividTestRelateLC()
		{
			PerformSystemTest(_vividTests+@"TestRelateLC");
		}
		public void TestVividTestRelateLL()
		{
			PerformSystemTest(_vividTests+@"TestRelateLL");
		}
		public void TestVividTestRelatePA()
		{
			PerformSystemTest(_vividTests+@"TestRelatePA");
		}
		public void TestVividTestRelatePL()
		{
			PerformSystemTest(_vividTests+@"TestRelatePL");
		}
		public void TestVividTestRelatePP()
		{
			PerformSystemTest(_vividTests+@"TestRelatePP");
		}
		public void TestVividTestSimple()
		{
			PerformSystemTest(_vividTests+@"TestSimple");
		}
		public void TestVividTestValid()
		{
			PerformSystemTest(_vividTests+@"TestValid");
		}
		public void TestVividTestValid2()
		{
			PerformSystemTest(_vividTests+@"TestValid2");
		}
		#endregion 

		#region Validate Tests
		public void TestRelateAA()
		{
			PerformSystemTest(_validateTests+"TestRelateAA");
		}
		public void TestRelateAC()
		{
			PerformSystemTest(_validateTests+"TestRelateAC");
		}
		public void TestRelateLA()
		{
			PerformSystemTest(_validateTests+"TestRelateLA");
		}
		public void TestRelateLC()
		{
			PerformSystemTest(_validateTests+"TestRelateLC");
		}
		public void TestTestRelateLL()
		{
			PerformSystemTest(_validateTests+"TestRelateLL");
		}
		public void Test()
		{												 
			PerformSystemTest(_validateTests+"TestRelatePA");
		}
		public void TestTestRelatePL()
		{
			PerformSystemTest(_validateTests+"TestRelatePL");
		}
		public void TestTestRelatePP()
		{
			PerformSystemTest(_validateTests+"TestRelatePP");

		}
		#endregion

		#region UrbanScience
		public void Test2DisjointPolygons2()
		{
			PerformSystemTest(_usiDir+@"2DisjointPolygons2");
		}
		public void Test2DisjointPolygons()
		{
			PerformSystemTest(_usiDir+@"2DisjointPolygons");
		}
		
		public void Test2PointsTest()
		{
			PerformSystemTest(_usiDir+@"2PointsTest");
		}
		public void TestAllDistinctIntersection()
		{
			PerformSystemTest(_usiDir+@"AllDistinctIntersection");
		}
		public void TestBufferOfGeometricCollection()
		{
			PerformSystemTest(_usiDir+@"BufferOfGeometricCollection");
		}
		public void TestbufferOfLineString()
		{
			PerformSystemTest(_usiDir+@"bufferOfLineString");
		}
		public void TestbufferOfPoint()
		{
			PerformSystemTest(_usiDir+@"bufferOfPoint");
		}
		public void TestBufferOfPolygon()
		{
			PerformSystemTest(_usiDir+@"BufferOfPolygon");
		}
		public void TestBufferTests()
		{
			PerformSystemTest(_usiDir+@"BufferTests");
		}
		public void TestBufferTests2()
		{
			PerformSystemTest(_usiDir+@"BufferTests2");
		}
		public void TestDimensionIntOfPoint()
		{
			PerformSystemTest(_usiDir+@"DimensionIntOfPoint");
		}
		public void TestDimensionOfGeometry()
		{
			PerformSystemTest(_usiDir+@"DimensionOfGeometry");
		}
		public void TestDimensionOfintersectionOfPoly()
		{
			PerformSystemTest(_usiDir+@"DimensionOfintersectionOfPoly");
		}
		public void TestDimensionOfIntLines()
		{
			PerformSystemTest(_usiDir+@"DimensionOfIntLines");
		}
		public void TestDimensionOfIntofPolygons()
		{//PerformSystemTest(@"DimensionOfIntofPolygons");
		}
		public void TestEquals_Polygon()
		{
			PerformSystemTest(_usiDir+@"Equals_Polygon");
		}
		public void TestLinestringBufferTest()
		{
			PerformSystemTest(_usiDir+@"LinestringBufferTest");
		}
		public void TestNumberOfPoints()
		{
			PerformSystemTest(_usiDir+@"NumberOfPoints");
		}
		public void TestReallyBigPolygon()
		{
			PerformSystemTest(_usiDir+@"ReallyBigPolygon");
		}		
		public void TestTest1()
		{
			PerformSystemTest(_usiDir+@"Test1");
		}
		public void TestTest2()
		{
			PerformSystemTest(_usiDir+@"Test2");
		}	
		public void TestTest3()
		{
			PerformSystemTest(_usiDir+@"Test3");
		}
		#endregion

		#region Testing code

		private void PerformSystemTest(string filename)
		{
				
			//Geometries.PrecisionModel pm = new Geometries.PrecisionModel();
			Run run = GetTestRun(filename);
			int testCount=0;
			int failCount=0;
			int opCount=0;
			int resultCount=0;
			foreach(TestRunner.TestCase testcase in run)
			{
				foreach(TestRunner.Test test in testcase)
				{
					Geotools.Geometries.PrecisionModel pm;
					
					if (run.PrecisionModel.Type=="FLOATING")
					{
						pm = new Geotools.Geometries.PrecisionModel();
					}
					else
					{
						pm = new Geotools.Geometries.PrecisionModel(double.Parse(run.PrecisionModel.Scale), double.Parse(run.PrecisionModel.OffSetX), double.Parse(run.PrecisionModel.OffSetY));
					}

					testCount++;
					Geometry a = null;
					Geometry b = null;
					if (testcase.A.Trim()!="")
					{
						a= (Geometry)testcase.CreateAGeometry(pm);
					}
					if (testcase.B.Trim()!="")
					{
						b= (Geometry)testcase.CreateBGeometry(pm);
					}
					test.Run(a, b, true);

					if(test.Passed == false)
					{
						Console.WriteLine(test.Operation.Name + " failed."+filename);
						failCount++;
					}
					else
					{
						//Console.WriteLine(t.Operation.Name + " passed.");
						//passedTests++;
					}
				}	
			}
			//Console.WriteLine(filename);
			//Console.WriteLine("tests: "+testCount);
			//Console.WriteLine("failed "+failCount);
			//Console.WriteLine("Ops "+opCount);
			//Console.WriteLine("Creates "+resultCount);
			//Console.WriteLine("-----------------------------");
			if (failCount>0)
			{
				Assertion.Fail(String.Format("{0} had {1} fails.",filename, failCount));
			}
		}

		private Run GetTestRun(string filename)
		{
			TestFileReader tReader = new TestFileReader();
			Run r = tReader.ReadTestFile(filename + ".xml");
			return r;
		}
		#endregion
	}
}
