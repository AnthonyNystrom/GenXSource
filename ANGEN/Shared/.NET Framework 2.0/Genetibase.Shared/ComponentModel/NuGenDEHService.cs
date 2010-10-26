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
		private List<INuGenDEHClient> _dehClients = new List<INuGenDEHClient>();
		private ReadOnlyCollection<INuGenDEHClient> _dehClientsRO;

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
			clientToAdd.EventToBeDelayed += _client_EventToBeDelayed;
			_dehClients.Add(clientToAdd);
		}

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
			clientToRemove.EventToBeDelayed -= _client_EventToBeDelayed;
			_dehClients.Remove(clientToRemove);
		}

		private void _client_EventToBeDelayed(object sender, NuGenDEHEventArgs e)
		{
			_currentSender = sender;
			_currentEventArgs = e;

			Debug.Assert(_eventRaiseTimer != null, "_eventRaiseTimer != null");
			_eventRaiseTimer.Start();
		}

		private void _eventRaiseTimer_Tick(object sender, EventArgs e)
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

		private INuGenTimer _eventRaiseTimer;
		private NuGenDEHEventArgs _currentEventArgs;
		private object _currentSender;

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
			_eventRaiseTimer.Tick += _eventRaiseTimer_Tick;
		}
	}
}
