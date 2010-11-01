/* -----------------------------------------------
 * NuGenInt32.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using Genetibase.Shared.Properties;

namespace Genetibase.Shared
{
	/// <summary>
	/// <see cref="Int32"/> wrapper with bounds check. Throws <see cref="ArgumentException"/>
	/// if the specified <see cref="Value"/> is out of the specified bounds.
	/// </summary>
	public class NuGenInt32
	{
		#region Properties.Public

		/*
		 * Default
		 */

		private int _default;

		/// <summary>
		/// Gets or sets the default value for this <see cref="NuGenInt32"/>.
		/// </summary>
		public int Default
		{
			get
			{
				return _default;
			}
			set
			{
				_default = value;
			}
		}

		/*
		 * Maximum
		 */

		/// <summary>
		/// Determines the upper bound for this <see cref="NuGenInt32"/>.
		/// </summary>
		private int _maximum;

		/// <summary>
		/// Gets or sets the upper bound for this <see cref="NuGenInt32"/>.
		/// </summary>
		public int Maximum
		{
			get
			{
				return _maximum;
			}
			set
			{
				_maximum = value;
			}
		}

		/*
		 * Minimum
		 */

		/// <summary>
		/// Determines the lower bound for this <see cref="NuGenInt32"/>.
		/// </summary>
		private int _minimum;

		/// <summary>
		/// Gets or sets the lower bound for this <see cref="NuGenInt32"/>.
		/// </summary>
		public int Minimum
		{
			get
			{
				return _minimum;
			}
			set
			{
				_minimum = value;
			}
		}

		/*
		 * Value
		 */

		/// <summary>
		/// Determines the value this <see cref="NuGenInt32"/> contains.
		/// </summary>
		private int _value;

		/// <summary>
		/// Gets or sets the value this <see cref="NuGenInt32"/> contains.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> is out of the specified bounds.
		/// </exception>
		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (!this.CheckLBound(value))
				{
					throw new ArgumentException(
						string.Format(Resources.Argument_InvalidCheckLBound, this.Minimum),
						"value"
					);
				}
				else if (!this.CheckUBound(value))
				{
					throw new ArgumentException(
						string.Format(Resources.Argument_InvalidCheckUBound, this.Maximum),
						"value"
						);
				}

				_value = value;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// Resets the <see cref="P:NuGenInt32.Value"/> to its default state.
		/// </summary>
		public void Reset()
		{
			this.Value = this.Default;
		}

		/// <summary>
		/// Indicates whether the <see cref="P:NuGenInt32.Value"/> is in its default state.
		/// </summary>
		/// <returns><see langword="true"/> if the <see cref="P:NuGenInt32.Value"/> differs from
		/// the <see cref="P:NuGenInt32.Default"/>; otherwise, <see langword="false"/>.</returns>
		public bool ShouldSerialize()
		{
			return this.Value != this.Default;
		}

		#endregion

		#region Methods.Private

		private bool CheckLBound(int value)
		{
			if (value < this.Minimum)
			{
				return false;
			}

			return true;
		}

		private bool CheckUBound(int value)
		{
			if (value > this.Maximum)
			{
				return false;
			}

			return true;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInt32"/> class.<para/>
		/// <c>Default = 0</c>.
		/// </summary>
		public NuGenInt32(int minimum, int maximum)
			: this(minimum, maximum, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInt32"/> class.
		/// </summary>
		public NuGenInt32(int minimum, int maximum, int @default)
		{
			this.Minimum = (int)Math.Min(minimum, maximum);
			this.Maximum = (int)Math.Max(minimum, maximum);

			if (!this.CheckLBound(@default) || !this.CheckUBound(@default))
			{
				throw new ArgumentException(
					string.Format(Resources.Argument_InvalidDefaultValue, this.Minimum, this.Maximum),
					"default"
				);
			}

			this.Default = @default;
			this.Value = @default;
		}

		#endregion
	}
}
