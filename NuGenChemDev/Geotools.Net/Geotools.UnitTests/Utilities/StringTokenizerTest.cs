#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Utilities/StringTokenizerTest.cs,v 1.2 2003/01/02 20:38:20 awcoats Exp $
 * $Log: StringTokenizerTest.cs,v $
 * Revision 1.2  2003/01/02 20:38:20  awcoats
 * *** empty log message ***
 *
 * 
 * 3     12/27/02 1:18p Awcoats
 * 
 * 2     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 1     9/19/02 9:39a Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using System.IO;
using NUnit.Framework;
using Geotools.Utilities;
#endregion

namespace Geotools.UnitTests.Utilities
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.Utilities.StreamTokenizerTest class
	/// </summary>
	[TestFixture]
	public class StreamTokenizerTest  
	{

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void TestTokenize() 
		{
			string test1="this,.is  123 \"test\"\"kk 456.789 test123 123.2 12*2 /* hello */ \n hello";
			TextReader reader = new StringReader(test1);
			StreamTokenizer tokenizer = new StreamTokenizer(reader,true);
			TokenType tokentype = tokenizer.NextToken();
			int iTokenCount=0;
			while (tokentype != TokenType.Eof)
			{
				//Console.WriteLine("token:"+tokentype+"("+tokenizer.GetStringValue()+")");
				tokentype = tokenizer.NextToken();
				iTokenCount++;
			}
			//Console.WriteLine("token count="+iTokenCount);
			Assertion.AssertEquals("token count",22,iTokenCount);
		}

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void TestTokenize2() 
		{
			string test1="this,.is  123 test 456.789 test123 123.2 12*2 /* hello */ \n hello";
			TextReader reader = new StringReader(test1);
			StreamTokenizer tokenizer = new StreamTokenizer(reader,false);
			TokenType tokentype = tokenizer.NextToken();
			int iTokenCount=0;
			while (tokentype != TokenType.Eof)
			{
				//Console.WriteLine("token:"+tokentype+"("+tokenizer.GetStringValue()+")");
				tokentype = tokenizer.NextToken();
				iTokenCount++;
			}
			//Console.WriteLine("token count ignore=false="+iTokenCount);
			Assertion.AssertEquals("token count",30,iTokenCount);
		}

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void TestTokenize3() 
		{
			string test1="this is a very_long_word and long123 long123longer ok";
			TextReader reader = new StringReader(test1);
			StreamTokenizer tokenizer = new StreamTokenizer(reader,true);
			TokenType tokentype = tokenizer.NextToken();
			int iTokenCount=0;
			while (tokentype != TokenType.Eof)
			{
				//Console.WriteLine("token:"+tokentype+"("+tokenizer.GetStringValue()+")");
				tokentype = tokenizer.NextToken();
				iTokenCount++;
			}
			//Console.WriteLine("token count ignore=false="+iTokenCount);
			Assertion.AssertEquals("token count",8,iTokenCount);
		}

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void TestTokenize5() 
		{
			string test1="-2.5 -2 4";
			TextReader reader = new StringReader(test1);
			StreamTokenizer tokenizer = new StreamTokenizer(reader,true);
			TokenType tokentype = tokenizer.NextToken();
			int iTokenCount=0;
			while (tokentype != TokenType.Eof)
			{
				tokentype = tokenizer.NextToken();
				iTokenCount++;
			}
			// first token will be -2.5
			Assertion.AssertEquals("token count",3,iTokenCount);
		}
		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void TestTokenize4() 
		{
			string test1="this34.3is a good test";
			TextReader reader = new StringReader(test1);
			StreamTokenizer tokenizer = new StreamTokenizer(reader,true);
			TokenType tokentype = tokenizer.NextToken();
			int iTokenCount=0;
			while (tokentype != TokenType.Eof)
			{
				//Console.WriteLine("token:"+tokentype+"("+tokenizer.GetStringValue()+")");
				tokentype = tokenizer.NextToken();
				iTokenCount++;
			}
			//Console.WriteLine("token count ignore=false="+iTokenCount);
			Assertion.AssertEquals("token count",7,iTokenCount);
		}
		public void TestReadNumber() 
		{
			string test1="this is a 123.5 notAnumber";
			TextReader reader = new StringReader(test1);
			StreamTokenizer tokenizer = new StreamTokenizer(reader,true);
			TokenType tokentype1 = tokenizer.NextToken(); //this
			TokenType tokentype2 = tokenizer.NextToken(); //is
			TokenType tokentype3 = tokenizer.NextToken(); //a
			tokenizer.NextToken();
		
			double number = tokenizer.GetNumericValue();
			Assertion.AssertEquals("test1",123.5,number);

			tokenizer.NextToken();
		
			try
			{
				double number2 = tokenizer.GetNumericValue();
				Assertion.Fail("This should fail because the token is not a number.");
			}
			catch
			{
			}	
		}
		/// <summary>
		/// Test getting and setting the properties
		/// </summary>
		public void Test_TestProperties() 
		{
			//TODO: Unit test getting and setting properties
		}
	}
}

