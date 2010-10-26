/* -----------------------------------------------
 * NuGenCollectionEditorPropertyBlock.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Represents a <see cref="PropertyGrid"/> with a title above.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenCollectionEditorPropertyBlock : UserControl
	{
		private NuGenPropertyGrid _propertyGrid;
		private NuGenCollectionEditorTitle _title;

		private static readonly object _selectedObjectsChanged = new object();

		/// <summary>
		/// Occurs when the <see cref="E:System.Windows.Forms.PropertyGrid.SelectedObjectsChanged"/> event occurs.
		/// </summary>
		public event EventHandler SelectedObjectsChanged
		{
			add
			{
				this.Events.AddHandler(_selectedObjectsChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedObjectsChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Design.NuGenCollectionEditorPropertyBlock.SelectedObjectsChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectedObjectsChanged(EventArgs e)
		{
			EventHandler handler = this.Events[_selectedObjectsChanged] as EventHandler;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// </summary>
		public string GetTitle()
		{
			return _title.Text;
		}

		/// <summary>
		/// </summary>
		public void SetSelectedObject(object node)
		{
			_propertyGrid.SelectedObject = node;
		}

		/// <summary>
		/// </summary>
		public void SetTitle(string value)
		{
			_title.Text = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCollectionEditorPropertyBlock"/> class.
		/// </summary>
		public NuGenCollectionEditorPropertyBlock()
		{
			_propertyGrid = new NuGenPropertyGrid();
			_title = new NuGenCollectionEditorTitle();

			_propertyGrid.Dock = DockStyle.Fill;
			_propertyGrid.Parent = this;
			_propertyGrid.SelectedObjectsChanged += delegate
			{
				this.OnSelectedObjectsChanged(EventArgs.Empty);
			};

			_title.Dock = DockStyle.Top;
			_title.Parent = this;
		}
	}
}
