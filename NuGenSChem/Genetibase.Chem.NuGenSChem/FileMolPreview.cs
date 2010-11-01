using System;
using System.Drawing;
namespace Genetibase.Chem.NuGenSChem
{
	
	// Previewing molecule-type files within the file choose mechanism.
	
	[Serializable]
	public class FileMolPreview:EditorPane
	{
		//UPGRADE_TODO: Class 'javax.swing.Image' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
		internal System.Drawing.Image thumbnail = null;
		internal System.IO.FileInfo file = null;
		
		public FileMolPreview(System.Windows.Forms.FileDialog fc):base(200, 200, true)
		{
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentaddPropertyChangeListener_javabeansPropertyChangeListener'"
			// TODO: fc.addPropertyChangeListener(this);
			// setBackground(Color.White);
            this.BackColor = Color.White; 
			SetBorder(true);
			SetToolCursor();
			SetEditable(false);
		}
		
		public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs ev)
		{
			bool update = false;
			System.String prop = ev.PropertyName;
			
			if ("directoryChanged".Equals(prop))
			// changed directory, do nothing much
			{
				file = null;
				update = true;
			}
			else if ("SelectedFilesChangedProperty".Equals(prop))
			// file just got selected
			{
				file = (System.IO.FileInfo) ev.NewValue;
				update = true;
			}
			
			if (update)
			{
				thumbnail = null;
				Molecule mol = null;
				if (file != null && System.IO.File.Exists(file.FullName))
				{
					try
					{
						//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javaioFile'"
						System.IO.FileStream istr = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
						mol = MoleculeStream.ReadUnknown(istr);
						istr.Close();
					}
                    catch (System.IO.IOException) 
					{
						mol = null;
					}
				}
				if (mol == null)
					mol = new Molecule();
				Replace(mol);
				ScaleToFit();
				if (Visible)
				{
					//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
					Refresh();
				}
			}
		}
		
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs g_EventArg)
		{
			System.Drawing.Graphics g = null;
			if (g_EventArg != null)
				g = g_EventArg.Graphics;
			System.Drawing.Size sz = Size;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Dimension.getWidth' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Dimension.getHeight' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			int width = (int) sz.Width, height = (int) sz.Height;
			SupportClass.GraphicsManager.manager.SetColor(g, Color.White);
			g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 0, 0, width, height);
			SupportClass.GraphicsManager.manager.SetColor(g, Color.Black);
			g.DrawRectangle(SupportClass.GraphicsManager.manager.GetPen(g), 0, 0, width, height);
			
			base.OnPaint(g_EventArg);
		}
	}
}