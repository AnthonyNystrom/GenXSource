/* -----------------------------------------------
 * NuGenTargetEventArgsGeneric.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared
{
	/// <summary>
	/// Contains target specific data.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="D"></typeparam>
	public class NuGenTargetEventArgsT<T, D> : EventArgs
	{
		#region Properties.Public

		/*
		 * Target
		 */

		private T _target = default(T);

		/// <summary>
		/// </summary>
		public T Target
		{
			get
			{
				return _target;
			}
		}

		/*
		 * TargetData
		 */

		private D _targetData = default(D);

		/// <summary>
		/// </summary>
		public D TargetData
		{
			get
			{
				return _targetData;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.Shared.NuGenTargetEventArgsT`2"/>
		/// class that is empty, has the default initial capacity, and uses the default equality comparer
		/// for the key type.
		/// </summary>
		public NuGenTargetEventArgsT(T target, D targetData)
		{
			_target = target;
			_targetData = targetData;
		}

		#endregion
	}
}
