/* -----------------------------------------------
 * NodePort.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.Expression;

namespace Genetibase.NuGenVisiCalc
{
	[Serializable]
	internal sealed class NodePort
	{
		/*
		 * Connection
		 */

		private NodePort _connection;

		public NodePort Connection
		{
			get
			{
				return _connection;
			}
			set
			{
				NodePort oldConnection = _connection;
				_connection = value;

				if (_node != null && _connection != null)
				{
					if (ExpressionSchemaBuilder.HasCircularLinks(_node, new List<NodeBase>()))
					{
						_connection = oldConnection;
					}
				}
			}
		}

		/*
		 * Data
		 */

		private Object _data;

		public Object Data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}

		/*
		 * Index
		 */

		private Int32 _index;

		public Int32 Index
		{
			get
			{
				return _index;
			}
			set
			{
				_index = value;
			}
		}

		/*
		 * Name
		 */

		private String _name;

		public String Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		/*
		 * Node
		 */

		private NodeBase _node;

		public NodeBase Node
		{
			get
			{
				return _node;
			}
			set
			{
				_node = value;
			}
		}

		/*
		 * IsCompatibleConnection
		 */

		/// <summary>
		/// </summary>
		/// <param name="nodePortToCheck"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="nodePortToCheck"/> is <see langword="null"/>.</para>
		/// </exception>
		public Boolean IsCompatibleConnection(NodePort nodePortToCheck)
		{
			if (nodePortToCheck == null)
			{
				throw new ArgumentNullException("nodePortToCheck");
			}

			try
			{
				if (_data != null && nodePortToCheck._data != null)
				{
					Type dataType = _data.GetType();
					Type anotherDataType = nodePortToCheck._data.GetType();

					return dataType == anotherDataType;
				}
			}
			catch
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NodePort"/> class.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="index"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="node"/> is <see langword="null"/>.</para>
		/// </exception>
		public NodePort(NodeBase node, Int32 index)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			_node = node;
			_index = index;
		}
	}
}
