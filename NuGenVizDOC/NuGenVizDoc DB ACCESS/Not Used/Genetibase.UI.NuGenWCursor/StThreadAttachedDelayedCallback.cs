using System;
using System.Runtime.InteropServices;

namespace Genetibase.UI
{
	/// <summary>
	/// Base class for StDelayedCallback classes that require ThreadInput from the Main thread
	/// 
	/// This class is a client of itself in that it implements the IDelayedCallbackHandler interface.
	/// </summary>
	public class StThreadAttachedDelayedCallback : StDelayedCallback, IDelayedCallbackHandler
	{
		#region Member Variables
		/// <summary>
		/// GUI Thread Id 
		/// </summary>
		private uint _mainThreadId;

		/// <summary>
		/// Callback Thread Id
		/// </summary>
		private uint _callbackThreadId;
		#endregion

		#region PInvoke imports
		[DllImport("USER32.DLL")]
		private static extern uint AttachThreadInput(uint attachTo, uint attachFrom, bool attach);

		[DllImport("KERNEL32.DLL")]
		private static extern uint GetCurrentThreadId();
		#endregion

		#region Constructors
		/// <summary>
		/// Member Initialising Constructor.
		/// </summary>
		/// <param name="delay">Delay to wait for</param>
		/// <param name="enabled">Enabled or not</param>
		public StThreadAttachedDelayedCallback(TimeSpan delay, bool enabled)
		{
			// Constructor is called from (what is treated as) the Main thread, grab its Thread Id
			_mainThreadId = GetCurrentThreadId();
			base.Init(this, delay, enabled);
		}

		/// <summary>
		/// Member Initialising Constructor.
		/// </summary>
		/// <param name="delay">Delay to wait for</param>
		public StThreadAttachedDelayedCallback(TimeSpan delay) : this(delay, true)
		{
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Start.  Called when the Delay has expired and operation is to begin.
		/// This implementation attaches this Thread to the Main Thread's Input.
		/// </summary>
		public virtual void Start()
		{
			// Start is called in a new Thread, grab the new Thread Id so we can attach to Main thread's input
			_callbackThreadId = GetCurrentThreadId();
			AttachThreadInput(_callbackThreadId, _mainThreadId, true);
		}

		/// <summary>
		/// Finish.  Called when the operation is to finish (usually IDispose)
		/// This implementation detaches this Thread from the Main Thread's Input.
		/// </summary>
		public virtual void Finish()
		{
			// Detach from Main thread input
			AttachThreadInput(_callbackThreadId, _mainThreadId, false);
		}
		#endregion
	}
}
