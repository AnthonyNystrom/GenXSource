using System;
using System.Collections.Generic;
namespace Genetibase.Chem.NuGenSChem
{
	
	// Selecting files by extension (strangely absent from Java).
	
	//UPGRADE_ISSUE: Class 'javax.swing.filechooser.FileFilter' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingfilechooserFileFilter'"
	public class FileExtFilter // :javax.swing.filechooser.FileFilter
	{
		internal System.String descr;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		List<String> exts;
		
		public FileExtFilter(System.String descroot, System.String suffixes)
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			exts = new List<String>();
			SupportClass.Tokenizer tok = new SupportClass.Tokenizer(suffixes, ";");
			while (tok.HasMoreTokens())
				exts.Add(tok.NextToken());
			
			descr = descroot + " (";
			for (int n = 0; n < exts.Count; n++)
				descr = descr + (n > 0?" ":"") + "*" + exts[n];
			descr = descr + ")";
		}
		
		public System.String getDescription()
		{
			return descr;
		}
		
		public bool accept(System.IO.FileInfo f)
		{
			if (System.IO.Directory.Exists(f.FullName))
				return true;
			for (int n = 0; n < exts.Count; n++)
				if (f.Name.EndsWith(exts[n]))
					return true;
			return false;
		}
	}
}