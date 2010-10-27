/***************************************************************************
                          profiler.cpp  -  description
                             -------------------
    begin                : Sat Jan 18 2003
    copyright            : (C) 2003,2004,2005,2006 by Matthew Mastracci, Christian Staudenmeyer
    email                : mmastrac@canada.com
 ***************************************************************************/

/***************************************************************************
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 ***************************************************************************/

using System;
using System.Threading;

namespace TestProfilee
{
	public class TestClass
	{
		public static int Main( string[] astrArguments )
		{
			TestClass tc = new TestClass();
			int nType = 1;
			if ( astrArguments.Length > 0 )
				nType = Convert.ToInt32( astrArguments[ 0 ] );

			switch ( nType )
			{
				case 1:
					new TestClass1().Run();
					break;
				case 2:
					new TestClass2().Run();
					break;
				case 3:
					new TestClass3().Run();
					break;
				default:
					Console.WriteLine( "Unknown test: " + nType );
					break;
			}

			return 0;
		}
	}
}
namespace TestProfilee
{
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

namespace TestProfilee
{
	public class TestClass2
	{
		public ManualResetEvent _mreA = new ManualResetEvent( false );
		public ManualResetEvent _mreB = new ManualResetEvent( false );

		public TestClass2()
		{
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

namespace TestProfilee
{
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
