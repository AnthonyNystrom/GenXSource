using System;
using System.Threading;

namespace TestProfilee
{
	/// <summary>
	/// Summary description for TestClass2.
	/// </summary>
	public class TestClass2
	{
		public ManualResetEvent _mreA = new ManualResetEvent( false );
		public ManualResetEvent _mreB = new ManualResetEvent( false );

		public TestClass2()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Run()
		{
			Thread tA = new Thread( new ThreadStart( ThreadA ) );
			Thread tB = new Thread( new ThreadStart( ThreadB ) );

			Thread.CurrentThread.Name = "Main thread";
			tA.Name = "Thread A";
			tB.Name = "Thread B";

			tA.Start();
			tB.Start();
			System.Console.WriteLine( "Suspending thread A" );
			Thread.Sleep( 100 );
			tA.Suspend();
			Thread.Sleep( 2500 );
			System.Console.WriteLine( "Suspending thread B" );
			tB.Suspend();
			Thread.Sleep( 2500 );
			System.Console.WriteLine( "Running GC..." );
			GC.Collect();
			System.Console.WriteLine( "Suspending main thread" );
			Thread.Sleep( 2500 );
			tA.Resume();
			_mreA.Set();
			Thread.Sleep( 2500 );
			tB.Resume();
			_mreB.Set();
		}

		public void ThreadA()
		{
			System.Console.WriteLine( "Thread A Started" );
			_mreA.WaitOne();
			System.Console.WriteLine( "Thread A Ended" );
		}

		public void ThreadB()
		{
			System.Console.WriteLine( "Thread B Started" );
			_mreB.WaitOne();
			System.Console.WriteLine( "Thread B Ended" );
		}
	}
}
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
