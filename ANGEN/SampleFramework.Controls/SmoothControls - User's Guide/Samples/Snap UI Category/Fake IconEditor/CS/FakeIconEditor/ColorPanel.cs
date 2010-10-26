using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FakeIconEditor
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal sealed partial class ColorPanel : UserControl
	{
		[DefaultValue(typeof(Color), "Window")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		private Color _selectedColor;

		public Color SelectedColor
		{
			get
			{
				if (_selectedColor == Color.Empty)
				{
					return Color.Transparent;
				}
				
				return _selectedColor;
			}
			set
			{
				if (_selectedColor != value)
				{
					_selectedColor = value;
					this.Invalidate();
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (this.Width > 0 && this.Height > 0)
			{
				using (SolidBrush sb = new SolidBrush(this.SelectedColor))
				{
					e.Graphics.FillRectangle(sb, this.ClientRectangle);
				}
			}
		}

		public ColorPanel()
		{
			this.InitializeComponent();
			
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.BackColor = SystemColors.Window;
		}
	}
}
