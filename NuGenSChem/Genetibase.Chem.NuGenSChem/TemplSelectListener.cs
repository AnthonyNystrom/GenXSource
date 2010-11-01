using System;
namespace Genetibase.Chem.NuGenSChem
{
	
	
	public interface ITemplSelectListener
	{
		void  TemplSelected(Molecule mol, int idx);
	}
}