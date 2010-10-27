using System;
using System.Threading;

namespace TestProfilee
{
	/// <summary>
	/// Summary description for TestClass3.
	/// </summary>
	public class TestClass3
	{
		public TestClass3()
		{
		}

		public void Run()
		{
			B( 0 );
		}

		void B( int nLevel )
		{
			C( nLevel );
		}
	
		void C( int nLevel )
		{
			Console.WriteLine( "ProjectRun Start" );
			if ( nLevel == 0 )
			{
				D( 1 );
				D( 2 );
			}
			else
			{
				E( nLevel );
			}

			Console.WriteLine( "ProjectRun End" );
		}

		void D( int nLevel )
		{
			B( nLevel + 1 );
		}

		void E( int nLevel )
		{
			Console.WriteLine( "DoSomethingElse: " + 1000 );
			Thread.Sleep( 1000 );
		}
	}
}
