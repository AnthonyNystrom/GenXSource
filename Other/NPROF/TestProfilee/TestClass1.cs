using System;

namespace TestProfilee
{
	/// <summary>
	/// Summary description for TestClass1.
	/// </summary>
	public class TestClass1
	{
		public TestClass1()
		{
		}

		public void Run()
		{
			A( true );
		}

		public void A( bool bRecurse )
		{
			if ( bRecurse )
			{
				Console.WriteLine( "A" );
				B();
				B();
				C( false );
				D();
			}
			else
			{
				Console.WriteLine( "        A" );
				System.Threading.Thread.Sleep( 1000 );
			}
		}

		public void B()
		{
			Console.WriteLine( "    B" );
			C( true );
		}

		public void C( bool bDeep )
		{
			if ( bDeep )
			{
				Console.WriteLine( "        C" );
				A( false );
			}
			else
			{
				Console.WriteLine( "    C" );
			}

			A( false );
		}

		public void D()
		{
			Console.WriteLine( "    D" );
			System.Threading.Thread.Sleep( 5000 );
		}

	}
}
