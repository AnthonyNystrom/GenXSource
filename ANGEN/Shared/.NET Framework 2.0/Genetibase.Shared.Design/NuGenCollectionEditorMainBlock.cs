/* -----------------------------------------------
 * NuGenCollectionEditorMainBlock.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Design.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Represents a customizable collection editor block with a title and basic action buttons.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenCollectionEditorMainBlock : UserControl
	{
		private Panel _actionPanel;
		private Panel _populatePanel;
		private Button _moveDownButton;
		private Button _moveUpButton;
		private NuGenCollectionEditorTitle _title;

		/// <summary>
		/// </summary>
		public ControlCollection GetActionControls()
		{
			return _actionPanel.Controls;
		}

		/// <summary>
		/// </summary>
		public ControlCollection GetPopulateControls()
		{
			return _populatePanel.Controls;
		}

		/// <summary>
		/// </summary>
		public Button GetMoveDownButton()
		{
			return _moveDownButton;
		}

		/// <summary>
		/// </summary>
		public Button GetMoveUpButton()
		{
			return _moveUpButton;
		}

		/// <summary>
		/// </summary>
		public string GetTitle()
		{
			return _title.Text;
		}

		/// <summary>
		/// </summary>
		public void SetTitle(string value)
		{
			_title.Text = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCollectionEditorMainBlock"/> class.
		/// </summary>
		public NuGenCollectionEditorMainBlock()
		{
			_title = new NuGenCollectionEditorTitle();
			_actionPanel = new Panel();
			_populatePanel = new Panel();
			_moveDownButton = new Button();
			_moveUpButton = new Button();

			_populatePanel.Dock = DockStyle.Bottom;
			_populatePanel.Height = 29;
			_populatePanel.Padding = new Padding(0, 5, 0, 0);
			_populatePanel.Parent = this;

			_moveDownButton.Image = Resources.Down;
			_moveDownButton.Parent = _actionPanel;
			_moveDownButton.TabIndex = 20;

			_moveUpButton.Image = Resources.Up;
			_moveUpButton.Parent = _actionPanel;
			_moveUpButton.TabIndex = 10;

			_actionPanel.Dock = DockStyle.Right;
			_actionPanel.Padding = new Padding(5, 0, 0, 0);
			_actionPanel.Parent = this;
			_actionPanel.Width = 35;

			foreach (Control ctrl in _actionPanel.Controls)
			{
				ctrl.Dock = DockStyle.Top;
			}

			_title.Dock = DockStyle.Top;
			_title.Parent = this;
		}
	}
}
