using System;
namespace Genetibase.Chem.NuGenSChem
{
	
	
	public interface MolSelectListener
	{
		void  MolSelected(EditorPane source, int idx, bool dblclick);
	}
}