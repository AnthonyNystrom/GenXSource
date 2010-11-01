/* -----------------------------------------------
 * NuGenEventHandlerListProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Provides a list of handlers for classes that fire events.
	/// </summary>
	/// <example>
	/// using System.ComponentModel;
	/// ...
	/// private NuGenEventHandlerListProvider _eventHandlerList = null;
	/// private NuGenEventInitiatorService _initiator = null;
	/// ...
	/// protected EventHandlerList Events
	/// {
	///		get
	///		{
	///			if (_eventHandlerList == null)
	///			{
	///				_eventHandlerList = new NuGenEventHandlerListProvider();
	///			}
	///			
	///			return _eventHandlerList.Events;
	///		}
	/// }
	/// 
	/// public void PerformClick()
	/// {
	///		this.OnClick(EventArgs.Empty);
	/// }
	/// 
	/// private static readonly object _click = new object();
	/// 
	/// public event EventHandler Click
	/// {
	///		add
	///		{
	///			this.Events.AddHandler(_click, value);
	///		}
	///		remove
	///		{
	///			this.Events.RemoveHandler(_click, value);
	///		}
	/// }
	/// 
	/// protected virtual void OnClick(EventArgs e)
	/// {
	///		_initiator.InvokeAction(_click, e);
	/// }
	/// 
	/// public ...() // Constructor
	/// {
	///		_initiator = new NuGenEventInitiatorService(this, this.Events);
	/// }
	/// </example>
	public class NuGenEventHandlerListProvider : INuGenEventHandlerListProvider
	{
		#region Properties.Public

		private EventHandlerList _events = null;

		/// <summary>
		/// Gets the list of handlers for the events defined.
		/// </summary>
		public EventHandlerList Events
		{
			get
			{
				if (_events == null)
				{
					_events = new EventHandlerList();
				}

				return _events;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventHandlerListProvider"/> class.
		/// </summary>
		public NuGenEventHandlerListProvider()
		{
		}

		#endregion
	}
}
