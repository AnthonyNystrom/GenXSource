using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Controls;
using System.Collections;


namespace Dile.UI
{
	public partial class NuGenDocumentSelectorForm : Form
	{
        private Panel dockPanel;
        public Panel DockPanel
		{
			get
			{
				return dockPanel;
			}
			set
			{
				dockPanel = value;
			}
		}

		private List<NuGenHighlightLabel> labels;
		private List<NuGenHighlightLabel> Labels
		{
			get
			{
				return labels;
			}
			set
			{
				labels = value;
			}
		}

		private List<NuGenCodeEditorForm> activeCodeEditors;
		private List<NuGenCodeEditorForm> ActiveCodeEditors
		{
			get
			{
				return activeCodeEditors;
			}
			set
			{
				activeCodeEditors = value;
			}
		}

		public NuGenDocumentSelectorForm()
		{
			InitializeComponent();
		}

		private void AddDocuments(Graphics graphics)
		{
			int documentIndex = 0;
			bool isActiveContentCodeEditor = DockPanel is NuGenCodeEditorForm;

			for (int index = 0; index < ActiveCodeEditors.Count; index++)
			{
				NuGenCodeEditorForm codeEditor = ActiveCodeEditors[index];

				if (codeEditor.Visible)
				{
					NuGenHighlightLabel label = CreateLabel(graphics, codeEditor);

					Labels.Add(label);
					documentsPanel.Controls.Add(label, 1, documentIndex++);

					if ((isActiveContentCodeEditor && index == 1) || index == 0)
					{
						documentsPanel.Tag = label;
					}
				}
			}
		}

		private void AddPanels(Graphics graphics)
		{
			int panelIndex = 0;

			NuGenHighlightLabel label = CreateLabel(graphics, DockPanel);

			Labels.Add(label);
			documentsPanel.Controls.Add(label, 0, panelIndex++);

			if (ActiveCodeEditors.Count == 0)
			{
				documentsPanel.Tag = label;
			}
		}

        private NuGenHighlightLabel CreateLabel(Graphics graphics, Panel content)
		{
			NuGenHighlightLabel result = new NuGenHighlightLabel();

			result.Dock = DockStyle.Fill;
			result.Tag = content;
			result.Text = content.Text;
			result.TextAlign = ContentAlignment.MiddleLeft;

			SizeF measuredSize = graphics.MeasureString(result.Text, result.Font);
			result.Height = 17;
			result.Width = Convert.ToInt32(measuredSize.Width) + 5;

			result.Click += new EventHandler(contentLabel_Click);

			return result;
		}

		private void contentLabel_Click(object sender, EventArgs e)
		{
			documentsPanel.Tag = sender;
			DisplayChosenContent();
		}

		public void Display(Form owner, List<NuGenCodeEditorForm> activeCodeEditors)
		{
			ActiveCodeEditors = activeCodeEditors;

			if (DockPanel != null)
			{
				documentsPanel.Controls.Clear();
				Labels = new List<NuGenHighlightLabel>(1);

				using (Graphics graphics = CreateGraphics())
				{
					AddPanels(graphics);
					AddDocuments(graphics);
				}

				Left = (owner.Width - owner.Left - Width) / 2;
				Top = (owner.Height - owner.Top - Height) / 2;
				Show(owner);

				NuGenHighlightLabel labelOfActiveContent = documentsPanel.Tag as NuGenHighlightLabel;

				if (labelOfActiveContent != null)
				{
					labelOfActiveContent.Focus();
				}
			}
		}

		private void DocumentSelectorForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Control)
			{
				DisplayChosenContent();
			}
		}

		private void DisplayChosenContent()
		{
			Hide();

			NuGenHighlightLabel label = (NuGenHighlightLabel)documentsPanel.Tag;
			//PETETODO: What?
            //DockContent content = (DockContent)label.Tag;
			//content.Activate();
		}

		private void DocumentSelectorForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
			{
				DisplayChosenContent();
			}
			else
			{
				int index = Labels.IndexOf((NuGenHighlightLabel)documentsPanel.Tag);

				switch (e.KeyCode)
				{
					case Keys.Tab:
						index++;

						if (index >= Labels.Count)
						{
							index = Labels.Count - ActiveCodeEditors.Count;
						}
						break;

					case Keys.Down:
						index++;
						break;

					case Keys.Up:
						index--;
						break;

					case Keys.Left:
					case Keys.Right:
						int panelCount = Labels.Count - ActiveCodeEditors.Count;

						if (index >= panelCount)
						{
							index -= panelCount;

							if (index >= panelCount)
							{
								index = panelCount - 1;
							}
						}
						else
						{
							index += panelCount;
						}
						break;
				}

				if (index >= Labels.Count)
				{
					index = 0;
				}
				else if (index < 0)
				{
					index = Labels.Count - 1;
				}

				Label nextLabel = Labels[index];
				documentsPanel.Tag = nextLabel;
				nextLabel.Focus();
			}
		}

		private void DocumentSelectorForm_Deactivate(object sender, EventArgs e)
		{
			Owner.Activate();
			Hide();
		}
	}
}