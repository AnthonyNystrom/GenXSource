/* -----------------------------------------------
 * NuGenFormStateStore.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// Stores the state of the specified <see cref="Form"/>. Change its state to whatever you need. Restore
	/// the original state using <see cref="RestoreFormState"/> method.
	/// </summary>
	public class NuGenFormStateStore : INuGenFormStateStore
	{
		#region INuGenFormStateStore Members

		/*
		 * RestoreFormState
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="formToRestore"/> is <see langword="null"/>.
		/// </exception>
		public void RestoreFormState(Form formToRestore)
		{
			if (formToRestore == null)
			{
				throw new ArgumentNullException("formToRestore");
			}

			Debug.Assert(this.StoredForms != null, "this.StoredForms != null");

			if (this.StoredForms.ContainsKey(formToRestore))
			{
				NuGenFormStateDescriptor stateDescriptor = this.StoredForms[formToRestore];
				ControlStyles[] formControlStyles = NuGenEnum.ToArray<ControlStyles>();

				for (int i = 0; i < formControlStyles.Length; i++)
				{
					NuGenControlPaint.SetStyle(
						formToRestore,
						formControlStyles[i],
						stateDescriptor.Styles[formControlStyles[i]]
					);

					formToRestore.BackColor = stateDescriptor.BackColor;
					formToRestore.Padding = stateDescriptor.Padding;
				}
			}
		}

		/*
		 * StoreFormState
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="formToStore"/> is <see langword="null"/>.
		/// </exception>
		public void StoreFormState(Form formToStore)
		{
			if (formToStore == null)
			{
				throw new ArgumentNullException("formToStore");
			}

			NuGenFormStateDescriptor stateDescriptor = new NuGenFormStateDescriptor();

			stateDescriptor.BackColor = formToStore.BackColor;
			stateDescriptor.Padding = formToStore.Padding;

			ControlStyles[] formControlStyles = NuGenEnum.ToArray<ControlStyles>();
			Debug.Assert(formControlStyles != null, "formControlStyles != null");

			for (int i = 0; i < formControlStyles.Length; i++)
			{
				stateDescriptor.Styles.Add(
					formControlStyles[i],
					NuGenControlPaint.GetStyle(formToStore, formControlStyles[i])
				);
			}

			Debug.Assert(this.StoredForms != null, "this.StoredForms != null");

			if (this.StoredForms.ContainsKey(formToStore))
			{
				this.StoredForms[formToStore] = stateDescriptor;
			}
			else
			{
				this.StoredForms.Add(formToStore, stateDescriptor);
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * StoredForms
		 */

		private Dictionary<Form, NuGenFormStateDescriptor> _StoredForms = null;

		/// <summary>
		/// </summary>
		protected Dictionary<Form, NuGenFormStateDescriptor> StoredForms
		{
			get
			{
				if (_StoredForms == null)
				{
					_StoredForms = new Dictionary<Form, NuGenFormStateDescriptor>();
				}

				return _StoredForms;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFormStateStore"/> class.
		/// </summary>
		public NuGenFormStateStore()
		{

		}

		#endregion
	}
}
