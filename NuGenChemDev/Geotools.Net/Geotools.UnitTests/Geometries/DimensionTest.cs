#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/DimensionTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: DimensionTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 8     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 7     11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 6     10/21/02 11:04a Rabergman
 * Made test match new formats for methods & properties.
 * 
 * 5     10/02/02 2:05p Rabergman
 * Daily check in
 * 
 * 4     9/25/02 12:30p Rabergman
 * Daily check in
 * 
 * 3     8/28/02 10:02a Lakoeppe
 * Modified tests to match changes made to Dimension class.
 * ToDimensionSymbol, ToDimensionValue, TodimensionSting were modified to
 * use char instead of string.
 * 
 * 2     8/23/02 11:12a Rabergman
 * removed extra s from namespace
 * 
 * 1     8/21/02 4:06p Rabergman
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.Geometries;
#endregion

namespace Geotools.UnitTests.Geometries
{
	/// <summary>
	/// Summary description for DimensionTest.
	/// </summary>
	[TestFixture]
	public class DimensionTest 
	{
		

		public void test_P()
		{
			//test that a zero is returned
			//AssertEquals("P: ", 0, Dimension.P);
		}

		public void test_L()
		{
			//test that a one is returned
			//AssertEquals("L: ", 1, Dimension.L);
		}

		public void test_A()
		{
			//test that a two is returned
			//AssertEquals("A: ", 2, Dimension.A);
		}

		public void test_False()
		{
			//test that a negative one is returned
			//AssertEquals("False: ", -1, Dimension.False);
		}

		public void test_True()
		{
			//test that a negative two is returned
			//AssertEquals("True: ", -2, Dimension.True);
		}

		public void test_DontCare()
		{
			//test that a negative three is returned
			//AssertEquals("DontCare: ", -3, Dimension.DontCare);
		}

		public void test_ToDimensionSymbol()
		{
			//AssertEquals("ToDimensionSymbol1: ", 'F', Dimension.ToDimensionSymbol(-1));
			//AssertEquals("ToDimensionSymbol2: ", 'T', Dimension.ToDimensionSymbol(-2));
			//AssertEquals("ToDimensionSymbol3: ", '*', Dimension.ToDimensionSymbol(-3));
			//AssertEquals("ToDimensionSymbol4: ", '0', Dimension.ToDimensionSymbol(0));
			//AssertEquals("ToDimensionSymbol5: ", '1', Dimension.ToDimensionSymbol(1));
			//AssertEquals("ToDimensionSymbol6: ", '2', Dimension.ToDimensionSymbol(2));
			
			try
			{
				//Dimension.ToDimensionSymbol(3);
				//Fail("ArgumentException should have been thrown");
			}
			catch(ArgumentException)
			{
			}
		}

		public void test_ToDimensionValue()
		{
			//AssertEquals("ToDimensionValue1: ", -1, Dimension.ToDimensionValue('F'));
			//AssertEquals("ToDimensionValue2: ", -2, Dimension.ToDimensionValue('t'));
			//AssertEquals("ToDimensionValue3: ", -3, Dimension.ToDimensionValue('*'));
			//AssertEquals("ToDimensionValue4: ", 0, Dimension.ToDimensionValue('0'));
			//AssertEquals("ToDimensionValue5: ", 1, Dimension.ToDimensionValue('1'));
			//AssertEquals("ToDimensionValue6: ", 2, Dimension.ToDimensionValue('2'));
			
			try
			{
				//Dimension.ToDimensionValue('3');
				//Fail("ArgumentException should have been thrown");
			}
			catch(ArgumentException)
			{
			}
		}

		public void test_ToDimensionString()
		{
			//AssertEquals("ToDimensionString1: ", "False", Dimension.ToDimensionString('F'));
			//AssertEquals("ToDimensionString2: ", "True", Dimension.ToDimensionString('t'));
			//AssertEquals("ToDimensionString3: ", "DontCare", Dimension.ToDimensionString('*'));
			//AssertEquals("ToDimensionString4: ", "P", Dimension.ToDimensionString('0'));
			//AssertEquals("ToDimensionString5: ", "L", Dimension.ToDimensionString('1'));
			//AssertEquals("ToDimensionString6: ", "A", Dimension.ToDimensionString('2'));
			
			try
			{
			//	Dimension.ToDimensionString('3');
			//	Fail("ArgumentException should have been thrown");
			}
			catch(ArgumentException)
			{
			}
		}

		public void test_ToDimensionString2()
		{
			//AssertEquals("ToDimensionString1: ", "False", Dimension.ToDimensionString(-1));
			//AssertEquals("ToDimensionString2: ", "True", Dimension.ToDimensionString(-2));
			//AssertEquals("ToDimensionString3: ", "DontCare", Dimension.ToDimensionString(-3));
			//AssertEquals("ToDimensionString4: ", "P", Dimension.ToDimensionString(0));
			//AssertEquals("ToDimensionString5: ", "L", Dimension.ToDimensionString(1));
			//AssertEquals("ToDimensionString6: ", "A", Dimension.ToDimensionString(2));
			
			try
			{
				//Dimension.ToDimensionSymbol(3);
				//Fail("ArgumentException should have been thrown");
			}
			catch(ArgumentException)
			{
			}
		}
	}
}
