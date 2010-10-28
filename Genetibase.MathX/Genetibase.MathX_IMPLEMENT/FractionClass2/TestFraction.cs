using System;
using System.Globalization;

using Mehroz;

namespace TestFraction
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class TestFraction
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Fraction frac=new Fraction(); // we'll get NaN
			AssertEqual(frac, Fraction.NaN);
			AssertEqual(frac.ToString() , NumberFormatInfo.CurrentInfo.NaNSymbol);

			frac = new Fraction(1,5);       // we'll get 1/5
			AssertEqual(frac.ToString() , "1/5");

			frac=new Fraction(25);        // we'll get 25
			AssertEqual(frac.ToString() , "25");

			frac=new Fraction(0.0);		// we'll get 0
			AssertEqual(frac.ToString() , "0");

			frac=new Fraction(0.25);      // we'll get 1/4
			AssertEqual(frac.ToString() , "1/4");

			frac=new Fraction(9.25);      // we'll get 37/4
			AssertEqual(frac.ToString() , "37/4");

			frac=new Fraction(long.MaxValue, 1);
			string compareTo = string.Format("{0}", long.MaxValue);
			AssertEqual(frac.ToString() , compareTo);

			frac=new Fraction(1, long.MaxValue);
			compareTo = string.Format("1/{0}", long.MaxValue);
			AssertEqual(frac.ToString() , compareTo);

			frac=new Fraction(long.MaxValue, long.MaxValue);
			AssertEqual(frac.ToString(), "1");

			// the plus-one issue is because of twos-complement representing one more negtive value than
			// positive
			frac=new Fraction(long.MinValue + 1, 1);
			compareTo = string.Format("{0}", long.MinValue + 1);
			AssertEqual(frac.ToString() , compareTo);

			frac=new Fraction(1, long.MinValue + 1);
			compareTo = string.Format("-1/{0}", Math.Abs(long.MinValue + 1));
			AssertEqual(frac.ToString() , compareTo);

			frac=new Fraction(long.MinValue + 1, long.MinValue + 1);
			AssertEqual(frac.ToString(), "1");

			frac=new Fraction(long.MaxValue, long.MinValue + 1);
			AssertEqual(frac.ToString(), "-1");

			frac=new Fraction(long.MinValue + 1, long.MaxValue);
			AssertEqual(frac.ToString(), "-1");

			frac=new Fraction(0.025);     // we'll get 1/40
			AssertEqual(frac.ToString() , "1/40");

			frac=new Fraction(1 / 2.0);   // we'll get 1/2
			AssertEqual(frac.ToString() , "1/2");
			frac=new Fraction(1 / 3.0);   // we'll get 1/3
			AssertEqual(frac.ToString() , "1/3");
			frac=new Fraction(1 / 4.0);   // we'll get 1/4
			AssertEqual(frac.ToString() , "1/4");
			frac=new Fraction(1 / 5.0);   // we'll get 1/5
			AssertEqual(frac.ToString() , "1/5");
			frac=new Fraction(1 / 6.0);   // we'll get 1/6
			AssertEqual(frac.ToString() , "1/6");
			frac=new Fraction(1 / 7.0);   // we'll get 1/7
			AssertEqual(frac.ToString() , "1/7");
			frac=new Fraction(1 / 8.0);   // we'll get 1/8
			AssertEqual(frac.ToString() , "1/8");
			frac=new Fraction(1 / 9.0);   // we'll get 1/9
			AssertEqual(frac.ToString() , "1/9");
			frac=new Fraction(1 / 10.0);   // we'll get 1/10
			AssertEqual(frac.ToString() , "1/10");
			frac=new Fraction(1 / 49.0);   // we'll get 1/49
			AssertEqual(frac.ToString() , "1/49");

			frac=new Fraction(6);
			AssertEqual(frac.ToString() , "6");

			Fraction divisor = new Fraction(4);
			AssertEqual(divisor.ToString() , "4");

			frac %= divisor;
			AssertEqual(frac.ToString(), "2");

			frac=new Fraction(9,4);
			AssertEqual(frac.ToString() , "9/4");

			divisor = new Fraction(2);
			AssertEqual(divisor.ToString() , "2");

			frac %= divisor;
			AssertEqual(frac.ToString() , "1/4");

			frac=new Fraction(5,12);
			AssertEqual(frac.ToString() , "5/12");

			divisor = new Fraction(1,4);
			AssertEqual(divisor.ToString() , "1/4");

			frac %= divisor;
			AssertEqual(frac.ToString() , "1/6");

			frac=new Fraction(1.0);     // we'll get 1
			AssertEqual(frac.ToString() , "1");

			frac=new Fraction(2.0);     // we'll get 2
			AssertEqual(frac.ToString() , "2");

			frac=new Fraction(-2.0);    // we'll get -2
			AssertEqual(frac.ToString() , "-2");

			frac=new Fraction(-1.0);    // we'll get -1
			AssertEqual(frac.ToString() , "-1");

			frac=new Fraction(0.5);		// we'll get 1/2
			AssertEqual(frac.ToString() , "1/2");

			frac=new Fraction(1.5);     // we'll get 3/2
			AssertEqual(frac.ToString() , "3/2");

			Console.WriteLine("Doing the loop check");
			for (int numerator = -100; numerator < 100; numerator++)
			{
				Console.Write("{0} ", numerator);

				for (int denominator = -100; denominator < 100; denominator++)
				{
					Fraction frac1 = new Fraction(numerator, denominator);

					double dbl = (double)numerator / (double)denominator;
					Fraction frac2 = new Fraction(dbl);

					AssertEqual(frac1, frac2);
				}
			}
			Console.WriteLine();

			frac=new Fraction("6.25");    // we'll get 25/4
			AssertEqual(frac.ToString() , "25/4");

			frac = 0;
			AssertEqual(frac.ToString(), "0");

			frac = 1;
			AssertEqual(frac.ToString(), "1");

			frac /= new Fraction(0);
			AssertEqual(frac, Fraction.PositiveInfinity);
			AssertEqual(frac.ToString(), NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol);

			frac = -1;
			AssertEqual(frac.ToString(), "-1");

			frac /= new Fraction(0);
			AssertEqual(frac, Fraction.NegativeInfinity);
			AssertEqual(frac.ToString(), NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol);

			// we can enter anything like "213" or 
			// "23/3" or "4.27"
			Console.Write("Enter a Fraction: ");
			frac = new Fraction( System.Console.ReadLine() );

			Console.WriteLine( frac );     // displays the current value of frac object;

			frac=new Fraction("1/2"); // initialize a fraction with 1/2
			AssertEqual(frac.ToString() , "1/2");

			Console.WriteLine( frac+2.5 );     // will display 3
			AssertEqual((frac + 2.5).ToString() , "3");

			frac="1/2";			// implicit cast from string to 
			AssertEqual(frac.ToString() , "1/2");

			frac="22.5";         // implicit cast from string to fraction
			AssertEqual(frac.ToString() , "45/2");

			frac=10.25;         // implicit cast from double to fraction
			AssertEqual(frac.ToString() , "41/4");

			frac=15;             // implicit cast from integer/long to fraction
			AssertEqual(frac.ToString() , "15");
																																		
			frac = 0.5;                 // initialize frac=1/2
			AssertEqual(frac.ToString() , "1/2");

			Console.WriteLine( frac - 0.25 );    // Yes, you are right. "1/4" is displayed
			AssertEqual((frac - 0.25).ToString(),  "1/4");

			Console.WriteLine(frac + "1/4");
			AssertEqual((frac + "1/4").ToString(), "3/4");

			if (frac.Equals(0.5))
				Console.WriteLine("seems that frac == 0.5");

			frac += 0.5;
			AssertEqual(frac.ToString(), "1");

			if (frac.Equals(1))
				Console.WriteLine("seems that now frac == 1");

			frac = double.NaN;
			Console.WriteLine(frac.ToString());
			AssertEqual(frac.ToString(), NumberFormatInfo.CurrentInfo.NaNSymbol);

			frac = double.PositiveInfinity;
			Console.WriteLine(frac.ToString());
			AssertEqual(frac.ToString(), NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol);

			frac = double.NegativeInfinity;
			Console.WriteLine(frac.ToString());
			AssertEqual(frac.ToString(), NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol);

			frac = "33";
			frac += "1/3";
			Console.WriteLine(frac.ToString());

			frac *= 3;
			Console.WriteLine(frac.ToString());

			Console.Write("Any key to quit");
			Console.ReadLine();
		}

		static void AssertEqual(Fraction left, Fraction right)
		{
			if (left == right)
				return;

			throw new Exception(string.Format("AssertFailed {0} != {1}", left, right));
		}

		static void AssertEqual(string left, string right)
		{
			if (left == right)
				return;

			throw new Exception(string.Format("AssertFailed {0} != {1}", left, right));
		}
	}
}
