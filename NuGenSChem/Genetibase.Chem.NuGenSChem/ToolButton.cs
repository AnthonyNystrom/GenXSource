using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Text;

namespace Genetibase.Chem.NuGenSChem
{
	
	// Subclassed version of the ToolButton which supports tool-tips with multiple lines.
	
	[Serializable]
	public class ToolButton:System.Windows.Forms.CheckBox
	{
		public ToolButton(System.Drawing.Image icon):base()
		{
			this.Appearance = System.Windows.Forms.Appearance.Button;
			this.Image = icon;
		}
		
		//UPGRADE_NOTE: The equivalent of method 'javax.swing.JComponent.createToolTip' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public System.Windows.Forms.ToolTip createToolTip()
		{
			MultiLineToolTip tip = new MultiLineToolTip();
			//UPGRADE_TODO: Method 'javax.swing.JToolTip.setComponent' was converted to 'System.Windows.Forms.ToolTip.SetToolTip' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJToolTipsetComponent_javaxswingJComponent'"
			tip.SetToolTip(this, "");
			return tip;
		}
	}
	
	//UPGRADE_TODO: The class 'ToolTip' is marked as Sealed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1147'"
	[Serializable]
	class MultiLineToolTip:System.Windows.Forms.ToolTip
	{
		public MultiLineToolTip()
		{
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setUI' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetUI_javaxswingplafComponentUI'"
// 			setUI(new MultiLineToolTipUI());
		}
	}
	
	//UPGRADE_TODO: Class 'javax.swing.plaf.ToolTipUI' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
	class MultiLineToolTipUI // :ToolTipUI
	{
		internal System.String[] strs;
		internal int maxWidth = 0;
		
		public void  paint(System.Drawing.Graphics g, System.Windows.Forms.Control c)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Graphics.getFont' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Drawing.Font font = SupportClass.GraphicsManager.manager.GetFont(g);
			System.Drawing.Font metrics = SupportClass.GraphicsManager.manager.GetFont(g);
			//UPGRADE_TODO: Class 'java.awt.font.FontRenderContext' was converted to 'System.Windows.Forms.Control' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			//UPGRADE_ISSUE: Constructor 'java.awt.font.FontRenderContext.FontRenderContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtfontFontRenderContextFontRenderContext_javaawtgeomAffineTransform_boolean_boolean'"
            System.Drawing.Text.TextRenderingHint frc = TextRenderingHint.AntiAlias; //  new FontRenderContext(null, false, false);
			System.Drawing.Size size = c.Size;
			SupportClass.GraphicsManager.manager.SetColor(g, c.BackColor);
			g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 0, 0, size.Width, size.Height);
			SupportClass.GraphicsManager.manager.SetColor(g, c.ForeColor);
			g.DrawRectangle(SupportClass.GraphicsManager.manager.GetPen(g), 0, 0, size.Width - 1, size.Height - 1);
			if (strs != null)
			{
				int y = 0;
				for (int i = 0; i < strs.Length; i++)
				{
					// TODO: use render hint? frc
					y += (int) g.MeasureString(strs[i], font).Height + 2;
					//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
					g.DrawString(strs[i], SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), 3, y - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
					//g.drawString(strs[i],3,(metrics.getHeight())*(i+1));
				}
			}
		}
		
        //public System.Drawing.Size getPreferredSize(System.Windows.Forms.Control c)
        //{
        //    System.Drawing.Font font = c.Font;
        //    System.Drawing.Text.TextRenderingHint frc = TextRenderingHint.AntiAlias; // (null, false, false);
        //    //UPGRADE_TODO: Method 'javax.swing.JToolTip.getTipText' was converted to 'System.Windows.Forms.ToolTip.GetToolTip' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJToolTipgetTipText'"
        //    System.String tipText = ((System.Windows.Forms.ToolTip) c).GetToolTip(null);
        //    if (tipText == null)
        //        tipText = "";
        //    while (tipText.EndsWith("\n"))
        //    {
        //        tipText = tipText.Substring(0, (tipText.Length - 1) - (0));
        //    }
        //    //UPGRADE_ISSUE: Constructor 'java.io.StreamReader.StreamReader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReaderBufferedReader_javaioReader'"
        //    System.IO.StringReader br = new System.IO.StringReader(tipText);
        //    string line;
        //    int maxWidth = 0, totalHeight = 0;
        //    //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        //    // Vector, using List as well 
        //    List<String> v = new List<String>();
        //    try
        //    {
        //        while ((line = br.ReadLine()) != null)
        //        {
        //            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //            //UPGRADE_TODO: Method 'java.awt.Font.getStringBounds' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
        //            int width = (int) font.getStringBounds(line, frc).Width;
        //            maxWidth = (maxWidth < width)?width:maxWidth;
        //            v.Add(line);
        //            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //            //UPGRADE_ISSUE: Method 'java.awt.font.LineMetrics.getHeight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtfontLineMetricsgetHeight'"
        //            //UPGRADE_TODO: Method 'java.awt.Font.getLineMetrics' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
        //            totalHeight += (int) font.getLineMetrics(line, frc).getHeight() + 2;
        //        }
        //    }
        //    catch (System.IO.IOException ex)
        //    {
        //        SupportClass.WriteStackTrace(ex, Console.Error);
        //    }
        //    int lines = v.Count;
        //    if (lines < 1)
        //    {
        //        strs = null;
        //        lines = 1;
        //    }
        //    else
        //    {
        //        strs = new System.String[lines];
        //        int i = 0;
        //        //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
        //        for (System.Collections.IEnumerator e = v.GetEnumerator(); e.MoveNext(); i++)
        //        {
        //            //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
        //            strs[i] = ((System.String) e.Current);
        //        }
        //    }
        //    this.maxWidth = maxWidth;
        //    return new System.Drawing.Size(maxWidth + 6, totalHeight + 4);
        //}
	}
}