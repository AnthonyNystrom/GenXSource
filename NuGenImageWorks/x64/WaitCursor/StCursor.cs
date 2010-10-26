using System;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks.WaitCursor
{
	/// <summary>
	/// Utility class to make showing (usually) a Wait Cursor much simpler and to remove the
	/// possibility of the Cursor not being restored due to an uncaught exception or forgetfulness to restore
	/// the cursor manually.
	/// 
	/// 2 Possible uses for this class :-
	/// 
	/// 1.  Single instance usage of the StCursor ..
	/// Instead of
	/// 
	/// public void DoSomeLengthyWork()
	/// {
	///		try
	///		{
	///			Screen.Cursor = Cursors.Wait;
	///			
	///			SlowlyCountToTenBillion();
	///		}
	///		finally
	///		{
	///			Screen.Cursor = Cursors.Default;
	///		}
	/// }
	/// 
	/// do this ..
	/// 
	/// public void DoSomeLengthyWork()
	/// {
	///		using (new StCursor(Cursors.Wait, new TimeSpan(0, 0, 0, 0, 100)))
	///		{
	///			SlowlyCountToTenBillion();
	///		}
	/// }
	/// 
	/// Above code will show the Wait cursor after 100ms of 'work'.  
	/// It makes use of the 'using' statement and IDispose to *make sure* the Cursor is always restored
	///
	/// 2.  Global usage of the StCursor (<see cref="ApplicationWaitCursor"/> class for usage)
	/// 
	/// </summary>
	public class StCursor : StThreadAttachedDelayedCallback, IDelayedCallbackHandler
	{
		#region Consts
		public static readonly TimeSpan DEFAULT_DELAY = new TimeSpan(0, 0, 0, 0, 500);
		#endregion

		#region Member Variables
		private Cursor _oldCursor;				// Remember old cursor
		private Cursor _newCursor;				// New cursor to show
		#endregion

		#region Constructors
		/// <summary>
		/// Member initialising Constructor
		/// </summary>
		/// <param name="newCursor">The Cursor to use</param>
		/// <param name="delay">Delay period before showing Cursor</param>
		/// <param name="enabled">Enable or Not</param>
		public StCursor(Cursor newCursor, TimeSpan delay, bool enabled) : base(delay, enabled)
		{
			_newCursor = newCursor;
		}

		/// <summary>
		/// Member initialising Constructor
		/// </summary>
		/// <param name="newCursor">The Cursor to use</param>
		/// <param name="delay">Delay period before showing Cursor</param>
		public StCursor(Cursor newCursor, TimeSpan delay) : this(newCursor, delay, true)
		{
		}

		/// <summary>
		/// Member initialising Constructor
		/// </summary>
		/// <param name="newCursor">The Cursor to use</param>
		public StCursor(Cursor newCursor) : this(newCursor, DEFAULT_DELAY)
		{
		}

		/// <summary>
		/// Member initialising Constructor
		/// </summary>
		/// <param name="newCursor">The Cursor to use</param>
		/// <param name="enabled">Enable or Not</param>
		public StCursor(Cursor newCursor, bool enabled) : this(newCursor, DEFAULT_DELAY, enabled)
		{
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Start showing the Cursor now
		/// </summary>
		public override void Start()
		{
			base.Start();
			_oldCursor = Cursor.Current;
			Cursor.Current = _newCursor;
		}

		/// <summary>
		/// Finish showing the Cursor (switch back to previous Cursor)
		/// </summary>
		public override void Finish()
		{
			Cursor.Current = _oldCursor;
			base.Finish();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Get/Set the Cursor to show
		/// </summary>
		public Cursor Cursor
		{
			get { return _newCursor; }
			set { _newCursor = value; }
		}
		#endregion
	}

}
