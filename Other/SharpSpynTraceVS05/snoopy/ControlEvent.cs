using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Debug
{
	public class ControlEvent 
	{
		string eventName;
		string controlName;

		public string EventName 
		{
			get 
			{
				return eventName;
			}
			set 
			{
				eventName = value;
			}
		}

		public string ControlName {
			get {
				return controlName;
			}
			set {
				controlName = value;
			}
		}

		object eventArgs;
		public object EventArgs {
			get {
				return eventArgs;
			}
		}

		object sender;
		public object Sender {
			get {
				return sender;
			}
		}

		EventTrackInfo trackInfo;
		public EventTrackInfo EventTrackInfo {
			get {
				return trackInfo;
			}
			set {
				trackInfo = value;
			}
		}

		bool trackEnabled = true;
		public bool TrackEnabled {
			get {
				return trackEnabled;
			}
			set {
				trackEnabled = value;
			}
		}

		public void GenericHandleEvent(object sender, object eventArgs) 
		{
			this.sender = sender;
			this.eventArgs = eventArgs;

			OnEventFired(System.EventArgs.Empty);
		}

		public event EventHandler EventFired;

		protected virtual void OnEventFired(EventArgs e) 
		{
			if (EventFired != null) 
			{
				EventFired(this, e);
			}
		}
	}
}
