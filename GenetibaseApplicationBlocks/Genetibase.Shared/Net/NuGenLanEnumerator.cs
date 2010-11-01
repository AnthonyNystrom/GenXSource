/* -----------------------------------------------
 * LanEnumerator.cs
 * Copyright © 2005 Alex Nesterov
 * --------------------------------------------- */

using lms = Genetibase.WinApi.LMServer;
using net = Genetibase.WinApi.NetApi32;

using Genetibase.WinApi;

using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;

namespace Genetibase.Shared.Net
{
	/// <summary>
	/// Enumerates servers in the LAN.
	/// </summary>
	public class NuGenLanEnumerator : IEnumerator
	{
		#region Declarations

		private IntPtr serverInfoPtr;
		private int currentItem;
		private uint itemCount;
		private string currentServerName;
		private static int SERVER_INFO_101_SIZE;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the current element in the collection.
		/// </summary>
		/// <value></value>
		/// <exception cref="T:System.InvalidOperationException">
		/// The enumerator is positioned before the first element of the collection or after the last element.
		/// </exception>
		public object Current
		{
			get
			{
				return currentServerName;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the enumerator was successfully advanced to the next element;
		/// <see langword="false"/> if the enumerator has passed the end of the collection.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		public bool MoveNext()
		{
			bool result = false;

			if (++currentItem < itemCount)
			{
				int newOffset = serverInfoPtr.ToInt32() + SERVER_INFO_101_SIZE * currentItem;
				SERVER_INFO_101 si = (SERVER_INFO_101)Marshal.PtrToStructure(new IntPtr(newOffset), typeof(SERVER_INFO_101));
				currentServerName = Marshal.PtrToStringAuto(si.sv101_name);
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before
		/// the first element in the collection.
		/// </summary>
		public void Reset()
		{
			IPHostEntry host = Dns.GetHostEntry("");
			currentItem = -1;
			currentServerName = null;
		}

		#endregion

		#region Constructors

		static NuGenLanEnumerator()
		{
			SERVER_INFO_101_SIZE = Marshal.SizeOf(typeof(SERVER_INFO_101));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:NuGenLanEnumerator"/> class.
		/// </summary>
		/// <param name="serverType">Type of the server.</param>
		protected internal NuGenLanEnumerator(uint serverType)
			: this(serverType, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:NuGenLanEnumerator"/> class.
		/// </summary>
		/// <param name="serverType">Type of the server.</param>
		/// <param name="domainName">Name of the domain.</param>
		protected internal NuGenLanEnumerator(uint serverType, string domainName)
		{
			uint level = 101, prefmaxlen = 0xFFFFFFFF, entriesread = 0, totalentries = 0;
			Reset();
			serverInfoPtr = IntPtr.Zero;

			int nRes = net.NetServerEnum(IntPtr.Zero, // Server Name: Reserved; must be NULL. 		
				level, // Return server names, types, and associated software. The bufptr parameter points to an array of SERVER_INFO_101 structures.			
				ref serverInfoPtr, // Pointer to the buffer that receives the data.			
				prefmaxlen, // Specifies the preferred maximum length of returned data, in bytes.			
				ref entriesread, // Count of elements actually enumerated.			
				ref totalentries, // Total number of visible servers and workstations on the network			
				(uint)serverType, // Value that filters the server entries to return from the enumeration			
				null, // Pointer to a constant string that specifies the name of the domain for which a list of servers is to be returned.			
				IntPtr.Zero); // Reserved; must be set to zero. 		

			itemCount = entriesread;
		}

		#endregion

		#region Destructors

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="T:Genetibase.Shared.Service.NuGenLanEnumerator"/> is reclaimed by garbage collection.
		/// </summary>
		~NuGenLanEnumerator()
		{
			if (!serverInfoPtr.Equals(IntPtr.Zero))
			{
				net.NetApiBufferFree(serverInfoPtr);
				serverInfoPtr = IntPtr.Zero;
			}
		}

		#endregion
	}

	/// <summary>
	/// Enumerates the servers in the LAN.
	/// </summary>
	public class NuGenLanItems : IEnumerable
	{
		#region Properties

		/*
		 * DomainName
		 */

		private string _domainName;

		/// <summary>
		/// Gets or sets the name of the domain.
		/// </summary>
		/// <value>The name of the domain.</value>
		public string DomainName
		{
			get
			{
				return _domainName;
			}
			set
			{
				_domainName = value;
			}
		}

		/*
		 * ServerType
		 */

		private uint _serverType;

		/// <summary>
		/// Gets or sets the server type.
		/// </summary>
		/// <value></value>
		public uint ServerType
		{
			get
			{
				return _serverType;
			}
			set
			{
				_serverType = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/>
		/// that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			return new NuGenLanEnumerator(_serverType, _domainName);
		}

		/// <summary>
		/// Gets the type of the server.
		/// </summary>
		/// <param name="serverName">Name of the server.</param>
		/// <returns></returns>
		public static int GetServerType(string serverName)
		{
			int result = 0;
			IntPtr serverInfoPtr = IntPtr.Zero;
			int rc = net.NetServerGetInfo(serverName, 101, ref serverInfoPtr);

			if (rc != 0)
			{
				SERVER_INFO_101 si = (SERVER_INFO_101)Marshal.PtrToStructure(serverInfoPtr, typeof(SERVER_INFO_101));
				result = si.sv101_type;
				net.NetApiBufferFree(serverInfoPtr);
				serverInfoPtr = IntPtr.Zero;
			}

			return result;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLanItems"/> class.
		/// </summary>
		public NuGenLanItems()
			: this(0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLanItems"/> class.
		/// </summary>
		/// <param name="aServerType">Type of a server.</param>
		public NuGenLanItems(uint aServerType)
		{
			ServerType = aServerType;
		}

		#endregion
	}
}
