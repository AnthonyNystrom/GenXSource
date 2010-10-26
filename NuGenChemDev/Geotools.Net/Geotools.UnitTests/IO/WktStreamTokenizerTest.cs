#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/WktStreamTokenizerTest.cs,v 1.2 2003/01/02 20:37:30 awcoats Exp $
 * $Log: WktStreamTokenizerTest.cs,v $
 * Revision 1.2  2003/01/02 20:37:30  awcoats
 * *** empty log message ***
 *
 * 
 * 3     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 2     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 1     9/17/02 4:15p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using System.IO;
using NUnit.Framework;
using Geotools.IO;
#endregion

namespace Geotools.UnitTests
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystemWktWriterTest class
	/// </summary>
	[TestFixture]
	public class WktStreamTokenizerTest  
	{
		
	

		
		public void TestReadQuotedString1() 
		{
			StringReader reader = new StringReader("\"testA\"..");
			WktStreamTokenizer tokenizer = new WktStreamTokenizer(reader);
			string word=tokenizer.ReadDoubleQuotedWord();
			Assertion.AssertEquals("test1","testA",word);
		}
		public void TestReadQuotedString2() 
		{
			StringReader reader = new StringReader("\"Two words\"..");
			WktStreamTokenizer tokenizer = new WktStreamTokenizer(reader);
			string word=tokenizer.ReadDoubleQuotedWord();
			Assertion.AssertEquals("test1","Two words",word);
		}
		public void TestReadAuthority1() 
		{
			StringReader reader = new StringReader("AUTHORITY[\"EPSG\",\"9102\"]");
			WktStreamTokenizer tokenizer = new WktStreamTokenizer(reader);
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			Assertion.AssertEquals("test 1","EPSG",authority);
			Assertion.AssertEquals("test 2","9102",authorityCode);
		}
		public void TestReadAuthority2() 
		{
			StringReader reader = new StringReader("OPPS[\"EPSG\",\"9102\"]");
			WktStreamTokenizer tokenizer = new WktStreamTokenizer(reader);
			string authority="";
			string authorityCode=""; 
			try
			{
				tokenizer.ReadAuthority(ref authority, ref authorityCode);
			}
			catch(ParseException)
			{

			}
		}
	
		
		

	}
}
