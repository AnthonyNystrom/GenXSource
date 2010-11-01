/* -----------------------------------------------
 * NuGenDEHService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Timers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Provides service for Delayed Event Handling infrastructure.
	/// </summary>
	public class NuGenDEHService : INuGenDEHService
	{
		#region Declarations.Fields

		private INuGenTimer _eventRaiseTimer = null;
		private INuGenDEHEventArgs _currentEventArgs = null;
		private object _currentSender = null;

		#endregion

		#region Properties.Public

		/*
		 * DEHClients
		 */

		private List<INuGenDEHClient> _dehClients = new List<INuGenDEHClient>();
		private ReadOnlyCollection<INuGenDEHClient> _dehClientsRO = null;

		/// <summary>
		/// Gets the collection of <see cref="INuGenDEHClient"/> inheritors that use this
		/// <see cref="NuGenDEHService"/>.
		/// </summary>
		public ReadOnlyCollection<INuGenDEHClient> DEHClients
		{
			get
			{
				if (_dehClientsRO == null)
				{
					_dehClientsRO = new ReadOnlyCollection<INuGenDEHClient>(_dehClients);
				}

				return _dehClientsRO;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * AddClient
		 */

		/// <summary>
		/// </summary>
		/// <param name="clientToAdd"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="clientToAdd"/> is <see langword="null"/>.
		/// </exception>
		public void AddClient(INuGenDEHClient clientToAdd)
		{
			if (clientToAdd == null)
			{
				throw new ArgumentNullException("clientToAdd");
			}

			Debug.Assert(_dehClients != null, "_dehClients != null");
			clientToAdd.EventToBeDelayed += this.client_EventToBeDelayed;
			_dehClients.Add(clientToAdd);
		}

		/*
		 * RemoveClient
		 */

		/// <summary>
		/// </summary>
		/// <param name="clientToRemove"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="clientToRemove"/> is <see langword="null"/>.
		/// </exception>
		public void RemoveClient(INuGenDEHClient clientToRemove)
		{
			if (clientToRemove == null)
			{
				throw new ArgumentNullException("clientToRemove");
			}

			Debug.Assert(_dehClients != null, "_dehClients != null");
			clientToRemove.EventToBeDelayed -= this.client_EventToBeDelayed;
			_dehClients.Remove(clientToRemove);
		}

		#endregion

		#region EventHandlers

		private void client_EventToBeDelayed(object sender, INuGenDEHEventArgs e)
		{
			_currentSender = sender;
			_currentEventArgs = e;

			Debug.Assert(_eventRaiseTimer != null, "_eventRaiseTimer != null");
			_eventRaiseTimer.Start();
		}

		private void eventRaiseTimer_Tick(object sender, EventArgs e)
		{
			Debug.Assert(_eventRaiseTimer != null, "_timer != null");
			_eventRaiseTimer.Stop();

			foreach (INuGenDEHClient client in this.DEHClients)
			{
				if (_currentSender != client)
				{
					client.HandleDelayedEvent(_currentSender, _currentEventArgs);
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDEHService"/> class.
		/// </summary>
		public NuGenDEHService(INuGenTimer eventRaiseTimer)
		{
			if (eventRaiseTimer == null)
			{
				throw new ArgumentNullException("eventRaiseTimer");
			}

			_eventRaiseTimer = eventRaiseTimer;
			_eventRaiseTimer.Tick += this.eventRaiseTimer_Tick;
		}

		#endregion
	}
}
