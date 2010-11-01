using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace Genetibase.Chem.NuGenSChem
{
	
	/*
	A popup window which displays available templates within a grid of molecule widgets, adding page navigation and reporting of the
	selection of individual templates.*/
	
	[Serializable]
    class TemplateSelector : NuGenPopupMenu, MolSelectListener // WindowFocusListener
	{
		internal Templates templ;
		internal ITemplSelectListener selectListen;
		
		internal const int MOL_COL = 5;
		internal const int MOL_ROW = 5;
		internal const int MOL_WIDTH = 100;
		internal const int MOL_HEIGHT = 75;
		internal const int FRAME_SIZE = 1;
		internal const int ARROW_WIDTH = 30;
		internal const int ARROW_HEIGHT = 15;
		//UPGRADE_NOTE: Final was removed from the declaration of 'WIDTH '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int WIDTH = MOL_COL * MOL_WIDTH + 2 * FRAME_SIZE;
		//UPGRADE_NOTE: Final was removed from the declaration of 'HEIGHT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int HEIGHT = MOL_ROW * MOL_HEIGHT + 2 * FRAME_SIZE + ARROW_HEIGHT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'NUM_WIDGETS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int NUM_WIDGETS = MOL_COL * MOL_ROW;
		
		internal EditorPane[] pics = new EditorPane[NUM_WIDGETS];
		//UPGRADE_TODO: Class 'javax.swing.plaf.basic.Button' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		internal int curPage = 0, numPages;

        private TemplateBorder content;               

        public TemplateSelector(NuGenEventHandler handler, NuGenPopupMenu parent):base(handler, parent)
		{
            this.templ = handler.GetTemplates();
			selectListen = handler.GetTemplListener();
			
			// setUndecorated(true);
			System.Windows.Forms.ContainerControl temp_Container;
			temp_Container = new System.Windows.Forms.ContainerControl();
			temp_Container.Parent = this;
			// temp_Container.setWindowDecorationStyle(JRootPane.NONE);
			//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_int_int'"
			Size = new System.Drawing.Size(WIDTH + 10, HEIGHT + 5);
			
			content = new TemplateBorder();
			content.Dock = System.Windows.Forms.DockStyle.Fill;
			Controls.Add(content);
			
			System.Drawing.Color bckgr = BackColor;
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRed' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getGreen' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getBlue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            System.Drawing.Color shade1 = Color.WhiteSmoke;// System.Drawing.Color.FromArgb(System.Math.Max((int)bckgr.R - 8, 0), System.Math.Max((int)bckgr.G - 8, 0), (int)bckgr.B);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRed' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getGreen' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getBlue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Drawing.Color shade2 = System.Drawing.Color.FromArgb(System.Math.Max((int) bckgr.R - 16, 0), System.Math.Max((int) bckgr.G - 16, 0), (int) bckgr.B);
			
			for (int n = 0; n < NUM_WIDGETS; n++)
				if (n < templ.NumTemplates())
				{
					pics[n] = new EditorPane(MOL_WIDTH, MOL_HEIGHT, true);
					pics[n].SetEditable(false);
					pics[n].BackColor = shade1;
					pics[n].Replace(templ.GetTemplate(n));
					pics[n].ScaleToFit();
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
					content.Controls.Add(pics[n]);
					//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_int_int'"
					pics[n].Location = new System.Drawing.Point(FRAME_SIZE + MOL_WIDTH * (n % MOL_COL) + 5, FRAME_SIZE + MOL_HEIGHT * (n / MOL_COL) + 5);
					pics[n].SetToolCursor();
					pics[n].SetMolSelectListener((MolSelectListener)this);
				}
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			numPages = (int) System.Math.Ceiling(templ.NumTemplates() / (double) NUM_WIDGETS);

            content.BackColor = Color.DimGray;

            this.Size = new Size(content.Size.Width, content.Size.Height - 10);
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
			
			// addWindowFocusListener(this);
		}

        // ------------------ event functions --------------------

        public virtual void windowGainedFocus(System.Object event_sender, System.EventArgs e)
		{
		}
		public virtual void  windowLostFocus(System.Object event_sender, System.EventArgs e)
		{
			Dispose();
		}
		
		public virtual void  MolSelected(EditorPane source, int idx, bool dblclick)
		{
			if (idx == 0)
				return ;
			selectListen.TemplSelected(source.MolData().Clone(), idx);
            Hide();
		}
		public virtual void  DirtyChanged(bool isdirty)
		{
		}

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            for (int i = 0; i < pics.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    pics[i].RePaint();
                }
            }
        }
    }

    [Serializable]
    class TemplateBorder : System.Windows.Forms.Control
	{
		public TemplateBorder()
		{
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setOpaque' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetOpaque_boolean'"
			// setOpaque(true);
            
		}
		
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs gr_EventArg)
		{
			System.Drawing.Graphics gr = null;
			if (gr_EventArg != null)
				gr = gr_EventArg.Graphics;
			System.Drawing.Graphics g = (System.Drawing.Graphics) gr;
			
			SupportClass.GraphicsManager.manager.SetColor(g, BackColor);
			g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 0, 0, Width, Height);
		}
	}
}