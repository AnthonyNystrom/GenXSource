using System;
using System.Windows.Forms;

namespace Genetibase.UI
{
	/// <summary>
	/// Singleton Utility class which is used to show a Wait Cursor when the Application is busy.  
	/// If the Application is busy then the Idle event will not be called during the busy period 
	/// and hence the Screen Cursor is automatically changed to a (by default) WaitCursor.
	///
	/// To use, simply insert the following line in your Application startup code
	/// 
	///		ApplicationWaitCursor.Cursor = Cursors.Wait;
	///		ApplicationWaitCursor.Delay  = new TimeSpan(0, 0, 0, 0, 100);
	///		
	/// This installs a StCursor to activate after 100ms of 'work' (Application.Idle not being called)
	/// 
	/// </summary>
	public class ApplicationWaitCursor : IMessageFilter
	{
		#region Member Variables
		/// <summary>
		/// The Cursor to use during busy periods 
		/// </summary>
		private static StCursor					_cursor							= new StCursor(Cursors.WaitCursor, false);
		
		private static EventHandler				_applicationIdleEventHandler	= null;
		private static ApplicationWaitCursor	_singleton						= null;

		/// <summary>
		/// None Client Area Button Down Windows Message
		/// </summary>
		private const int						WM_NCLBUTTONDOWN				= 0xa1;
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor.  Hidden
		/// </summary>
		private ApplicationWaitCursor()
		{	
		}

		/// <summary>
		/// Static constructor which attaches to the Singleton Application instance
		/// </summary>
		static ApplicationWaitCursor()
		{
			_singleton							= new ApplicationWaitCursor();
			_applicationIdleEventHandler		= new EventHandler(OnApplicationIdle);
		}
		#endregion

		#region Public Static Properties
		/// <summary>
		/// Gets and Sets the Cursor to use during Application busy periods.  Setting this to NULL will disable the
		/// monitoring of busy periods.
		/// </summary>
		public static Cursor Cursor
		{
			get { return _cursor.Cursor; }
			set
			{
				if ((value != null) && !_cursor.Enabled)
				{
					Application.Idle += _applicationIdleEventHandler;
					Application.AddMessageFilter(_singleton);
				}
				else if ((value == null) && _cursor.Enabled)
				{
					Application.Idle -= _applicationIdleEventHandler;
					Application.RemoveMessageFilter(_singleton);
				}
				
				_cursor.Cursor = value;
				_cursor.Enabled = value != null;
			}
		}

		/// <summary>
		/// Get/Set the period of Time to wait before showing the WaitCursor whilst Application is working
		/// </summary>
		public static TimeSpan Delay
		{
			get { return _cursor.Delay; }
			set { _cursor.Delay = value; }
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Process the Idle event.  Simply reset the StWaitCursor 
		/// </summary>
		private static void OnApplicationIdle(object sender, EventArgs e)
		{
			_cursor.Reset();
		}

		/// <summary>
		/// Pre-Filters Windows messages.  During Window Moves/Resizes the Application Idle is not called (appears busy)
		/// so we filter for these events so we can temporarily turn off the WaitCursor
		/// </summary>
		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_NCLBUTTONDOWN)
				_cursor.Enabled = false;
			else
				_cursor.Enabled = true;

			// Always let the real Message through
			return false;
		}
		#endregion
	}
}
