using System;
using System.Threading;

namespace Genetibase.UI
{
	/// <summary>
	/// Implement this interface to participate with a StDelayedCallback instance
	/// </summary>
	public interface IDelayedCallbackHandler
	{
		void Start();
		void Finish();
	}

	/// <summary>
	/// This class manages a IDelayedCallbackHandler.  After a specified delay the 
	/// <see cref="IDelayedCallbackHandler.Start"/> method is called.
	/// If the <see cref="IDelayedCallbackHandler.Start"/> is called then this guarantees that the 
	/// <see cref="IDelayedCallbackHandler.Finish"/> method is called when this instance is Disposed.
	/// <seealso cref="StCursor"/> for an implementation
	/// </summary>
	public class StDelayedCallback : IDisposable
	{
		#region Member Variables
		/// <summary>
		/// The callback
		/// </summary>
		private IDelayedCallbackHandler			_callbackHandler;			

		/// <summary>
		/// Delay to wait before calling back
		/// </summary>
		private TimeSpan						_delay;

		/// <summary>
		/// Thread to perform the wait and callback
		/// </summary>
		private Thread							_callbackThread;		

		/// <summary>
		/// Have we been Disposed or not ?
		/// </summary>
		private bool							_disposed					= false;	
		
		/// <summary>
		/// Has callback Start been called ?
		/// </summary>
		private bool							_startCalled				= false;

		/// <summary>
		/// WaitHandle for notifications
		/// </summary>
		private ManualResetEvent				_resetEvent					= new ManualResetEvent(false);
		
		/// <summary>
		/// Enabled or not ?
		/// </summary>
		private bool							_enabled					= true;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default Constructor.  Hidden.  
		/// </summary>
		protected StDelayedCallback()
		{
			// Derived Class MUST call Init in their constructor	
		}

		/// <summary>
		/// Creates a StDelayedCallback instance prepared with a <see cref="IDelayedCallbackHandler"/> and the specified <see cref="TimeSpan"/> delay
		/// </summary>
		/// <param name="callbackHandler">The CallbackHandler to use</param>
		/// <param name="delay">Initial Delay value</param>
		/// <param name="enabled">Initial Enabled state</param>
		public StDelayedCallback(IDelayedCallbackHandler callbackHandler, TimeSpan delay, bool enabled)
		{
			Init(callbackHandler, delay, enabled);
		}
		#endregion

		#region Protected Methods
		/// <summary>
		/// Prepares the class.  Creates the Thread that will call Start & Finish
		/// </summary>
		/// <param name="callbackHandler"></param>
		/// <param name="delay"></param>
		/// <param name="enabled"></param>
		protected void Init(IDelayedCallbackHandler callbackHandler, TimeSpan delay, bool enabled)
		{
			_callbackHandler	= callbackHandler;
			_delay				= delay;
			_enabled			= enabled;

			_callbackThread = new Thread(new ThreadStart(CallbackThread));
			_callbackThread.Name = this.GetType().Name + " DelayedCallback Thread";
			_callbackThread.IsBackground = true;
			_callbackThread.Start();
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Thread method.  Loops calling Start & Finish until Disposed, honours the Enabled flag
		/// </summary>
		private void CallbackThread()
		{
			do
			{
				// Initial State around loop
				_startCalled = false;

				WaitToStart();
				if (_startCalled)
					WaitForReset();

			} while (!_disposed);
		}

		/// <summary>
		/// Waits for either the ResetEvent or the Wait period to expire.  If Wait period expires then Start is called
		/// </summary>
		private void WaitToStart()
		{
			bool waited = _resetEvent.WaitOne(_delay, false);
			_resetEvent.Reset();

			if (!waited)
			{
				if (_enabled)
				{
					try
					{
						_callbackHandler.Start();
					}
					finally
					{
						_startCalled = true;
					}
				}
			}
		}

		/// <summary>
		/// Waits for the ResetEvent (set by Dispose & Reset), since Start has been called we *have* to call Finish
		/// </summary>
		private void WaitForReset()
		{
			_resetEvent.WaitOne();
			_resetEvent.Reset();

			// Always calls Finish even if we are Disabled or we Aborted since Start/Finish *always* go in Pairs
			_callbackHandler.Finish();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Resets the Wait period to start Waiting again
		/// </summary>
		public void Reset()
		{
			_resetEvent.Set();
		}

		/// <summary>
		/// On Disposal terminates the Thread, calls Finish (on thread) if Start has been called
		/// </summary>
		public void Dispose()
		{
			if (_disposed)
				return;

			_disposed = true;			// Kills the Thread loop
			_resetEvent.Set();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Enable/Disable the call to Start (note, once Start is called it *always* calls the paired Finish)
		/// </summary>
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		/// <summary>
		/// Get/Set the period of Time to wait before calling the Start method
		/// </summary>
		public TimeSpan Delay
		{
			get { return _delay; }
			set { _delay = value; }
		}
		#endregion
	}
}