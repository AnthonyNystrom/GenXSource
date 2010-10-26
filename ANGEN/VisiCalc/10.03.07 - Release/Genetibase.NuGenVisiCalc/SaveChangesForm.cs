/* -----------------------------------------------
 * SaveChangesForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.Controls;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed partial class SaveChangesForm : Form
	{
		public void AddDocument(Canvas canvas)
		{
			NuGenTreeNode treeNode = new NuGenTreeNode(canvas.ParentTabPage.Text, true, true);
			treeNode.Tag = canvas;
			_documents.Nodes.Add(treeNode);
		}

		public IList<Canvas> GetCanvasCollection()
		{
			List<Canvas> canvasList = new List<Canvas>();

			foreach (NuGenTreeNode treeNode in _documents.Nodes)
			{
				if (treeNode.Checked)
				{
					canvasList.Add((Canvas)treeNode.Tag);
				}
			}

			return canvasList;
		}

		private void _abortButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		public SaveChangesForm()
		{
			InitializeComponent();
			SetStyle(ControlStyles.Opaque, true);
		}
	}
}
