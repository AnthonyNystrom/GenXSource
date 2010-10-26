/* -----------------------------------------------
 * NuGenHotKeyPopup.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenHotKeyPopup
	{
		private sealed class KeyCombo : NuGenComboBox
		{
			public Keys SelectedKey
			{
				get
				{
					if (this.SelectedIndex == -1)
					{
						return Keys.None;
					}

					return (Keys)this.KeysConverter.ConvertFromString(this.SelectedItem.ToString());
				}
				set
				{
					if (KeyCombo.IsValidKey(value))
					{
						this.SelectedItem = this.KeysConverter.ConvertToString(value);
					}
					else
					{
						this.SelectedIndex = -1;
					}
				}
			}

			private TypeConverter _keysConverter;

			private TypeConverter KeysConverter
			{
				get
				{
					if (_keysConverter == null)
					{
						_keysConverter = TypeDescriptor.GetConverter(typeof(Keys));
						Debug.Assert(_keysConverter != null, "_keysConverter != null");
					}

					return _keysConverter;
				}
			}

			private static bool IsValidKey(Keys keyCode)
			{
				foreach (Keys keys in _validKeys)
				{
					if (keys == keyCode)
					{
						return true;
					}
				}

				return false;
			}

			#region ValidKeys

			private static readonly Keys[] _validKeys = new Keys[]
			{ 
				Keys.A,
				Keys.B,
				Keys.C,
				Keys.D, Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.Delete, Keys.Down, 
				Keys.E, Keys.End, Keys.Escape,
				Keys.F, Keys.F1, Keys.F10, Keys.F11, Keys.F12, Keys.F13, Keys.F14, Keys.F15, Keys.F16, Keys.F17, Keys.F18, Keys.F19, Keys.F2, Keys.F20, Keys.F21, Keys.F22, Keys.F23, Keys.F24, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9,
				Keys.G,
				Keys.H,
				Keys.I,
				Keys.Insert,
				Keys.J, 
				Keys.K,
				Keys.L, Keys.Left,
				Keys.M,
				Keys.N, Keys.NumLock, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, 
				Keys.O, Keys.OemBackslash, Keys.OemClear, Keys.OemCloseBrackets, Keys.Oemcomma, Keys.OemMinus, Keys.OemOpenBrackets, Keys.OemPeriod, Keys.OemPipe, Keys.Oemplus, Keys.OemQuestion, Keys.OemQuotes, Keys.OemSemicolon, Keys.Oemtilde,
				Keys.P, Keys.Pause, 
				Keys.Q,
				Keys.R, Keys.Right,
				Keys.S, Keys.Space,
				Keys.T, Keys.Tab,
				Keys.U, Keys.Up,
				Keys.V,
				Keys.W,
				Keys.X,
				Keys.Y,
				Keys.Z
			};

			#endregion

			/// <summary>
			/// Initializes a new instance of the <see cref="KeyCombo"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// <para>Requires:</para>
			/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			/// 	<para><see cref="INuGenImageListService"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public KeyCombo(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				foreach (Keys key in _validKeys)
				{
					this.Items.Add(this.KeysConverter.ConvertToString(key));
				}

				this.DropDownStyle = ComboBoxStyle.DropDownList;
			}
		}
	}
}
