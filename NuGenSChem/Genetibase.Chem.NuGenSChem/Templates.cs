using System;
using System.Collections.Generic;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.IO;
namespace Genetibase.Chem.NuGenSChem
{
	
	// For obtaining the template list.
	
	public class Templates
	{
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		List<Molecule> templ = new List<Molecule>();
		
		public Templates()
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			List<String> list = new List<String>();
			
			// read the list of molecules from the directory file, then create each one of them
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resources = assembly.GetManifestResourceNames();

			try
			{
 
                // Build the string of resources.
                foreach (string resource in resources)
                {
                    if (resource.StartsWith("Genetibase.Chem.NuGenSChem.templ."))
                        list.Add(resource); 
                }
			}
			catch (IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Failed to obtain list of templates:\n" + e.ToString());
				return ;
			}
			
			try
			{
				for (int n = 0; n < list.Count; n++)
				{
                    Stream istr = assembly.GetManifestResourceStream(list[n]);
					Molecule mol = MoleculeStream.ReadNative(istr);
					templ.Add(mol);
					istr.Close();
				}
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Failed to obtain particular template:\n" + e.ToString());
				return ;
			}
			
			// sort the molecules by an index of "complexity" (smaller molecules first, carbon-only favoured)

            int[] complex = new int[templ.Count];
            for (int n = 0; n < templ.Count; n++)
			{
				Molecule mol = templ[n];
				complex[n] = mol.NumAtoms() * 100;
				bool nonCH = false;
				for (int i = 1; i <= mol.NumAtoms(); i++)
					if (String.CompareOrdinal(mol.AtomElement(i), "C") != 0 && String.CompareOrdinal(mol.AtomElement(i), "H") != 0)
						nonCH = true;
				if (!nonCH)
					complex[n] -= 1000;
				for (int i = 1; i <= mol.NumBonds(); i++)
					complex[n] = complex[n] + mol.BondOrder(i);
			}
			
			int p = 0;
			while (p < templ.Count - 1)
			{
				if (complex[p] > complex[p + 1])
				{
					int i = complex[p]; complex[p] = complex[p + 1]; complex[p + 1] = i;
					Molecule mol = templ[p]; templ[p] =  templ[p + 1]; templ[p + 1] =  mol;
					if (p > 0)
						p--;
				}
				else
					p++;
			}
		}
		
		public virtual int NumTemplates()
		{
			return templ.Count;
		}
		public virtual Molecule GetTemplate(int N)
		{
			return templ[N];
		}
		public virtual void  AddTemplate(Molecule Mol)
		{
			templ.Add(Mol);
		}
	}
}