/*!
@mainpage   Protein Insight .Net Interface

@htmlonly
<map id="FPMap0" name="FPMap0">
<area coords="607, 117, 699, 159" href="interface_protein_insight_interface_1_1_i_property.html" shape="rect" />
<area coords="200, 153, 309, 206" href="interface_protein_insight_interface_1_1_i_protein_insight.html" shape="rect" />
<area coords="41, 171, 119, 212" href="interface_protein_insight_interface_1_1_i_movie.html" shape="rect" />
<area coords="20, 255, 125, 295" href="interface_protein_insight_interface_1_1_i_property_scene.html" shape="rect" />
<area coords="27, 340, 120, 382" href="interface_protein_insight_interface_1_1_i_light.html" shape="rect" />
<area coords="217, 296, 292, 339" href="interface_protein_insight_interface_1_1_i_p_d_b.html" shape="rect" />
<area coords="209, 394, 299, 435" href="interface_protein_insight_interface_1_1_i_model.html" shape="rect" />
<area coords="207, 489, 298, 532" href="interface_protein_insight_interface_1_1_i_chain.html" shape="rect" />
<area coords="209, 583, 300, 626" href="interface_protein_insight_interface_1_1_i_residue.html" shape="rect" />
<area coords="210, 682, 295, 722" href="interface_protein_insight_interface_1_1_i_atom.html" shape="rect" />
<area coords="412, 301, 498, 343" href="interface_protein_insight_interface_1_1_i_v_p.html" shape="rect" />
<area coords="827, 195, 919, 239" href="interface_protein_insight_interface_1_1_i_annotation.html" shape="rect" />
<area coords="826, 138, 918, 182" href="interface_protein_insight_interface_1_1_i_annotation.html" shape="rect" />
<area coords="825, 79, 917, 123" href="interface_protein_insight_interface_1_1_i_annotation.html" shape="rect" />
<area coords="825, 19, 917, 63" href="interface_protein_insight_interface_1_1_i_clipping.html" shape="rect" />
<area coords="606, 181, 732, 222" href="interface_protein_insight_interface_1_1_i_property_wireframe.html" shape="rect" />
<area coords="606, 242, 726, 285" href="interface_protein_insight_interface_1_1_i_property_balln_stick.html" shape="rect" />
<area coords="608, 303, 700, 343" href="interface_protein_insight_interface_1_1_i_property_stick.html" shape="rect" />
<area coords="608, 360, 719, 404" href="interface_protein_insight_interface_1_1_i_property_space_fill.html" shape="rect" />
<area coords="608, 425, 712, 467" href="interface_protein_insight_interface_1_1_i_property_ribbon.html" shape="rect" />
<area coords="607, 483, 715, 526" href="interface_protein_insight_interface_1_1_i_property_surface.html" shape="rect" />
<area coords="797, 360, 889, 401" href="interface_protein_insight_interface_1_1_i_property_helix.html" shape="rect" />
<area coords="797, 427, 896, 466" href="interface_protein_insight_interface_1_1_i_property_sheet.html" shape="rect" />
<area coords="795, 489, 888, 532" href="interface_protein_insight_interface_1_1_i_property_coil.html" shape="rect" />
<area coords="20, 453, 128, 496" href="interface_protein_insight_interface_1_1_i_utility.html" shape="rect" />
</map>
<center>
<br>
<p>Click the class. You can see class description</p>
<img alt="" src="ProteinInsightInterfaceClass.png" usemap="#FPMap0" class="style1" /><p>
</center>

@endhtmlonly

*/


#include "stdafx.h"

using namespace System;				
using namespace System::Drawing;
using namespace System::Collections;				
using namespace System::Collections::Generic;		//	List namespace
using namespace Microsoft::DirectX;					
using namespace Microsoft::DirectX::Direct3D;

using namespace System::ComponentModel;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing::Imaging;
/**
	Root interface of Protein Insight.

	You have to use it for accessing the Protein Insight.
*/
namespace NuGenCbaseInterface
{
	//	Predefine
	interface class IPDB;
	interface class IModel;
	interface class IChain;
	interface class IResidue;
	interface class IProteinInsight;
	interface class IVP;
 
	/**
		Atom interface.

		@b Example 
		@code
//	Deselect all
pi.SetSelect(false, true);

IChain Chain = pi.PDBs[0].Chains[0];
for (int i = 0; i < Chain.Atoms.Count; i++)
{
	//	find CA atoms and select those.
	if (Chain.Atoms[i].Type == IAtom.IType.CA)
		Chain.Atoms[i].SetSelect(true, true);
}
//	Show selected atoms
pi.AddVP(IProteinInsight.IDisplayStyle.SpaceFill);
		@endcode
		@sa IResidue, IChain, IVP, IProteinInsight
	*/
	public interface class IAtom
	{
	public:
		/**
		Atom serial index in a PDB.

		@b Example
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
int num = Atom.Num;
		@endcode
		@sa IAtom, IProteinInsight
		*/
		property	long	Num { virtual long get(); }

		/**
		Atom name in a PDB.

		@b Example
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
if ( Atom.Name == "C" ) { 
	Atom.Select(true, true); 
}
		@endcode
		@sa IAtom, IProteinInsight
		*/
		property	String ^ Name { virtual String ^ get(); }

		/**
		Name of residue which include the Atom.

		@b Example
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
if ( Atom.ResidueName == "ALA" ) { 
	Atom.Select(true, true); 
}
		@endcode
		@sa IAtom, IProteinInsight
		*/
		property	String ^ ResidueName { virtual String ^ get(); }

		/**
		Atom occupancy

		@sa IAtom, IProteinInsight
		*/
		property	float	Occupancy { virtual float get(); virtual void set(float value); }

		/**
		Atom temperature.

		@sa IAtom, IProteinInsight
		*/
		property	float	Temperature { virtual float get(); virtual void set(float value); }

		/**
		Atom hydropathy.

		@sa IAtom, IProteinInsight
		*/
		property	float	Hydropathy { virtual float get(); virtual void set(float value); }

		/**
		Get the atom is side chain or not.

		@b Example
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
if ( Atom.SideChain == true ) { 
	Atom.Select(false, true); 
}
		@endcode
		@sa IAtom, IProteinInsight
		*/
		property	bool	SideChain { virtual bool get(); }

		/**
		Type of Atom.

		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
if ( Atom.Type == IAtom.IType.CA ) {	
	Atom.Select(true,true); 
}
		@endcode
		@sa IAtom, IAtom::Type, IProteinInsight
		*/
		enum class IType	{ N, CA, CB, C, O, SIDECHAIN };

		/**
		Get type of Atom.

		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
if ( Atom.Type == IAtom.IType.CA ) {	
	Atom.Select(true,true); 
}
		@endcode
		@sa IAtom::IType, IAtom, IProteinInsight
		*/
		property	IType Type { virtual IType get(); }

		/**
		Get atom position in a PDB. This value is not transformed.

		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
pi.Utility.OutputMsg(Atom.Position.ToString());
		@endcode
		@sa PositionTransformed, IAtom, IProteinInsight
		*/
		property	Vector3		Position { virtual Vector3  get(); }

		/**
		Get 3D Transformed atom position.

		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
pi.Utility.OutputMsg(Atom.PositionTransformed.ToString());
		@endcode
		@sa Position, IAtom, IProteinInsight
		*/
		property	Vector3		PositionTransformed { virtual Vector3  get(); }

		/**
		Get Chain Object which include Atom.

		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
IChain Chain = Atom.ParentChain;
		@endcode
		@sa ParentPDB, ParentResidue, IChain, IAtom, IProteinInsight
		*/
		property	IChain	^	ParentChain { virtual IChain ^ get(); }

		/**
		PDB Object which include this Atom.

		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
IPDB PDB = Atom.ParentPDB;
		@endcode
		@sa ParentChain, ParentResidue, IChain, IAtom, IProteinInsight
		*/
		property	IPDB ^		ParentPDB { virtual IPDB ^ get(); }

		/**
		Residue Object which include this Atom.

		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
IResidue Residue = Atom.ParentResidue;
		@endcode
		@sa ParentChain, ParentPDB, IChain, IAtom, IProteinInsight
		*/
		property	IResidue ^	ParentResidue { virtual IResidue ^ get(); }

		/**[Category("Common Option")]
		Set Atom Select.

		@param select select or de-select the atom
		@param bNeedUpdate apply selection to all user interface pane
		
		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
Atom.SetSelect(true, true);
		@endcode
		@sa IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		virtual void	SetSelect(bool select, bool bNeedUpdate);

		/**
		Get/Set Atom Select. It's a little bit slow operation.

		@b Example:
		@code
IAtom Atom = pi.PDBs[0].Chains[0].Residues[0].Atoms[0];
Atom.Select = true;		//	Same as Atom.SetSelect(true,true);
		@endcode
		@sa IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		property	bool		Select;
	};

	/**
	Residue interface.

	@b Example 
	@code
//	Deselect all
pi.SetSelect(false, true);

IChain Chain = pi.PDBs[0].Chains[0];
for (int i = 0; i < Chain.Residues.Count; i++)
{
	//	find Helix residue.
	if (Chain.Residues[i].SSType == IResidue.ISSType.Helix)
		Chain.Residues[i].SetSelect(true, true);
}
//	Show selected atoms
pi.AddVP(IProteinInsight.IDisplayStyle.SpaceFill);
	@endcode
	@sa IAtom, IChain, IVP, IProteinInsight
	*/
	public interface class IResidue
	{
	public:
		/**
		Name of Residue.
		ALA, ARG, ASN, ASP, CYS, GLU, GLN, GLY, HIS, ILE, LEU, LYS, MET, PHE, PRO, SER, THR, TRP, TYR, VAL or
		A, G, T, C, U

		@sa IReisude
		*/
		property	String ^	Name { virtual String ^ get(); }

		/**
		One character name of Residue.
		ALA -> A
		ARG -> R
		ASN -> N
		ASP -> D
		CYS -> C
		GLU -> E
		GLN -> Q
		GLY -> G
		HIS -> H
		ILE -> I
		LEU -> L
		LYS -> K
		MET -> M
		PHE -> F
		PRO -> P
		SER -> S
		THR -> T
		TRP -> W
		TYR -> Y
		VAL -> V

		@sa IReisude
		*/
		property	String ^	NameOneChar { virtual String ^ get(); }

		/**
		Residue Index in a PDB.

		@sa IReisude
		*/
		property	long		Num { virtual long get(); }

		/**
		Type definition of secondary structure of residue.

		@b Example:
		@code
if (Chain.Residues[i].SSType == IResidue.ISSType.Helix)
	Chain.Residues[i].SetSelect(true, true);
		@endcode
		@sa IReisude, ISSType, SSType, IHelixType, HelixType
		*/
		enum	class ISSType	{ None, Helix, Sheet };

		/**
		Get secondary structure type of residue.

		@b Example:
		@code
if (Chain.Residues[i].SSType == IResidue.ISSType.Helix)
	Chain.Residues[i].SetSelect(true, true);
		@endcode
		@sa IReisude, ISSType, SSType, IHelixType, HelixType
		*/
		property	ISSType		SSType { virtual ISSType  get(); }

		/**
		Helix type definition.

		@b Example:
		@code
if (Chain.Residues[i].SSType == IResidue.ISSType.Helix)
	if ( Chain.Residues[i].HelixType == IResidue.IHelixType.PI )
			Chain.Residues[i].SetSelect(true, true);
		@endcode
		@sa IReisude, ISSType, SSType, IHelixType, HelixType
		*/
		enum	class IHelixType { Default, PI, _310 };

		/**
		Get Helix type of the residue.

		@b Example:
		@code
if (Chain.Residues[i].SSType == IResidue.ISSType.Helix)
	if ( Chain.Residues[i].HelixType == IResidue.IHelixType.PI )
		Chain.Residues[i].SetSelect(true, true);
		@endcode
		@sa IReisude, ISSType, SSType, IHelixType, HelixType
		*/
		property	IHelixType	HelixType { virtual IHelixType  get(); }

		/**
		Get Atom N in a reisude.

		@sa IReisude::AtomN, IReisude::AtomCa, IReisude::AtomCb, IReisude::AtomC, IReisude::AtomO
		*/
		property	IAtom ^		AtomN { virtual IAtom ^ get(); }
		/**
		Get Atom Ca in a reisude.

		@sa IReisude::AtomN, IReisude::AtomCa, IReisude::AtomCb, IReisude::AtomC, IReisude::AtomO
		*/
		property	IAtom ^		AtomCa{ virtual IAtom ^ get(); }
		/**
		Get Atom Cb in a reisude.

		@sa IReisude::AtomN, IReisude::AtomCa, IReisude::AtomCb, IReisude::AtomC, IReisude::AtomO
		*/
		property	IAtom ^		AtomCb{ virtual IAtom ^ get(); }
		/**
		Get Atom C in a reisude.

		@sa IReisude::AtomN, IReisude::AtomCa, IReisude::AtomCb, IReisude::AtomC, IReisude::AtomO
		*/
		property	IAtom ^		AtomC { virtual IAtom ^ get(); }
		/**
		Get Atom O in a reisude.

		@sa IReisude::AtomN, IReisude::AtomCa, IReisude::AtomCb, IReisude::AtomC, IReisude::AtomO
		*/
		property	IAtom ^		AtomO { virtual IAtom ^ get(); }
		/**
		True if this residue is main chain.

		@sa IResidue
		*/
		property	bool		ExistMainChain { virtual bool get(); }

		/**
		Get chain which include the this residue.

		@sa IResidue, IChain
		*/
		property	IChain	^	ParentChain { virtual IChain ^ get(); }
		/**
		Get PDB which include the this residue.

		@sa IResidue, IPDB
		*/
		property	IPDB ^		ParentPDB { virtual IPDB ^ get(); }
		/**
		Get Atoms of which this residue is composed.

		@b Example:
@code
IResidue residue = Chain.Residues[i];

for ( int j = 0 ; j < residue.Atoms.Count ; j++ )	{
	pi.Utility.OutputMsg(residue.Atoms[j].Name);
}	
@endcode
		@sa List, IAtom, IResidue, IPDB
		*/
		property List<IAtom ^ > ^ Atoms { virtual List<IAtom ^> ^ get(); }

		/**
		Set Atom Select.

		@param	select	select or de-select the atom
		@param	bNeedUpdate	apply selection to all user interface pane

		@sa IAtom::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		virtual void	SetSelect(bool select, bool bNeedUpdate);

		/**
		Get/Set Residue Select. It's a little bit slow operation.

		@b Example:
		@code
IResidue Residue = pi.PDBs[0].Chains[0].Residues[0];
Residue.Select = true;		//	Same as Residue.SetSelect(true,true);
		@endcode
		@sa IAtom::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		property	bool		Select;
	};

	/**
	Chain interface.

	@b Example 
	@code
//	enumerate and display all chains of PDB[0]
IPDB pdb = pi.PDBs[0];
for (int i = 0; i < pdb.Chains.Count; i++) {
	String strMsg = String.Format("PDB ID:{0:s}, Chain ID:{1:s}\n", pdb.Chains[i].PDBID, pdb.Chains[i].ID);
	pi.Utility.OutputMsg(strMsg);
}
@endcode
	@sa IAtom, IResidue, IVP, IProteinInsight
	*/
	public interface class IChain
	{
	public:
		/**
		Get ID of chain.

		@sa IChain, IPDB
		*/
		property String ^ ID { virtual String ^ get(); }
		/**
		Get PDB ID of chain.

		@sa IChain, IPDB
		*/
		property String ^ PDBID { virtual String ^ get(); }
		/**
		Get Model of chain.

		@sa IChain::ParentPDB, IModel, IPDB
		*/
		property	IModel ^	ParentModel { virtual IModel ^ get(); }
		/**
		Get PDB of chain.

		@sa IChain::ParentModel, IModel, IPDB
		*/
		property	IPDB ^		ParentPDB { virtual IPDB ^ get(); }

		/**
		Get residue sequence string of chain.
		for example, DARNDCREQG...

		@sa IChain::IChain, IResidue
		*/
		property String ^ Sequence { virtual String ^ get(); }

		/**
		Set Atom Select.

		@param	select select or de-select the atom
		@param	bNeedUpdate apply selection to all user interface pane

		@b Example:
		@code
pi.PDBs[0].Chains[0].SetSelect(true, true);
		@endcode
		@sa IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		virtual void	SetSelect(bool select, bool bNeedUpdate);

		/**
		Get/Set Chain Select. It's a little bit slow operation.

		@b Example:
		@code
IChain Chain = pi.PDBs[0].Chains[0];
Chain.Select = true;		//	Same as Chain.SetSelect(true,true);
		@endcode
		@sa IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		property	bool		Select;

		/**
		Get specified residues of this chain.

		@param	residueName name of residue you want to get

		@b Example:
		@code
foreach(IResidue r in pi.PDBs[0].Chains[0].GetResidues("ALA") )
if ( r.SSType == IResidue.ISSType.Helix ) {
	r.SetSelect(true, true);
}
		@endcode
		@sa IChain::GetResidues, IChain::GetAtoms
		*/
		virtual List<IResidue ^ > ^ GetResidues(String ^ residueName);

		/**
		Get specified atoms of this chain.

		@param	atomName name of atom you want to get

		@b Example:
		@code
foreach (IAtom atom in pi.PDBs[0].Chains[0].GetAtoms("CA"))
	atom.SetSelect(true, true);
		@endcode
		@sa IChain::GetResidues, IChain::GetAtoms
		*/		
		virtual List<IAtom ^ > ^ GetAtoms(String ^ atomName);

		/**
		Get specified residues of this chain.

		@param	type name of secondaty structure type you want to get (None, Helix or Sheet)

		@b Example:
		@code
pi.AddVP(pi.PDBs[0].Chains[0].GetSSResidues(IResidue.ISSType.Helix), IProteinInsight.IDisplayStyle.Surface);
		@endcode
		@sa IChain::GetResidues, IChain::GetAtoms
		*/			
		virtual List<IResidue ^ > ^ GetSSResidues(IResidue::ISSType type);

		/**
		Get all atoms of this chain.

		@sa IChain::Atoms, IChain::HETAtoms, IChain::Residues
		*/
		property List<IAtom ^ > ^ Atoms { virtual List<IAtom ^> ^ get(); }

		/**
		Get all hetero atoms of this chain.

		@sa IChain::Atoms, IChain::HETAtoms, IChain::Residues
		*/
		property List<IAtom ^ > ^ HETAtoms { virtual List<IAtom ^> ^ get(); }

		/**
		Get all residues of this chain.

		@sa IChain::Atoms, IChain::HETAtoms, IChain::Residues
		*/
		property List<IResidue ^ > ^ Residues { virtual List<IResidue ^> ^ get(); }

		/**
		True of this chain is DNA.

		@sa IChain
		*/
		property bool	 IsDNA { virtual bool get(); }
	};

	/**
	Model interface.
	In some PDBs, there is no model. In this case, pi.PDBs[0].Models[0].Chains[0] is same as pi.PDBs[0].Chains[0].

	@b Example 
@code
//  enumeration all atoms in PDB.
foreach(IPDB pdb in pi.PDBs)
	foreach ( IModel model in pdb.Models)
		foreach (IChain chain in model.Chains)
			foreach (IResidue residue in chain.Residues)
				foreach (IAtom atom in residue.Atoms){
					String strMsg = String.Format("Atom Position: {0:s}", atom.Position.ToString());
					pi.Utility.OutputMsg(strMsg);
				}      
@endcode
	@sa IAtom, IResidue, IChain, IPDB, IVP, IProteinInsight
	*/
	public interface class IModel
	{
	public:
		/**
		The number of Model.

		@sa	IModel
		*/
		property	long		Num { virtual long get(); }
		/**
		The PDB of model

		@sa	IModel, IPDB
		*/
		property	IPDB ^		ParentPDB { virtual IPDB ^ get(); }

		/**
		Set Model Select.

		@param	select select or de-select the atom
		@param	bNeedUpdate apply selection to all user interface pane

		@b Example:
		@code
pi.PDBs[0].Models[0].SetSelect(true, true);
		@endcode
		@sa IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		virtual void	SetSelect(bool select, bool bNeedUpdate);

		/**
		Get/Set Model Select. It's a little bit slow operation.

		@b Example:
		@code
IModel Model = pi.PDBs[0].Models[0];
Model.Select = true;		//	Same as Model.SetSelect(true,true);
		@endcode
		@sa IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		property	bool		Select;

		/**
		Get specific chain of this model.

		@param chainID	ID of chain you want to get. \n

		@sa IModel, IPDB::GetChains
		*/
		virtual IChain ^ GetChain(String ^ chainID);

		/**
		Get all chains of this model.

		@sa IModel, IModel::GetChain, IPDB::GetChains
		*/
		property List<IChain ^> ^ Chains { virtual List<IChain ^> ^ get(); }
	};

	/**
	PDB interface.
	PDB interface gets from IProteinInsight

	@b Example 
	@code
//  enumeration all atoms in PDB.
foreach(IPDB pdb in pi.PDBs)
	foreach ( IModel model in pdb.Models)
		foreach (IChain chain in model.Chains)
			foreach (IResidue residue in chain.Residues)
				foreach (IAtom atom in residue.Atoms){
					String strMsg = String.Format("Atom Position: {0:s}", atom.Position.ToString());
					pi.Utility.OutputMsg(strMsg);
				}      
	@endcode
	@sa IAtom, IResidue, IChain, IModel, IVP, IProteinInsight
	*/
	public interface class IPDB
	{
	public:
		/**
		ID of PDB

		@sa IPDB, IChain::ID, IChain::PDBID
		*/
		property String ^ ID { virtual String ^ get(); }

		/**
		Get file name of this PDB

		@sa IPDB
		*/
		property String ^ Filename { virtual String ^ get(); }

		/**
		Set PDB Select.

		@param	select select or de-select the atom
		@param	bNeedUpdate apply selection to all user interface pane

		@b Example:
		@code
foreach(IPDB pdb in pi.PDBs)
	pdb.SetSelect(true, true);
		@endcode
		@sa IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		virtual void	SetSelect(bool select, bool bNeedUpdate);

		/**
		Get/Set PDB Select. It's a little bit slow operation.

		@b Example:
		@code
IPDB PDB = pi.PDBs[0];
PDB.Select = true;		//	Same as PDB.SetSelect(true,true);
		@endcode
		@sa IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		property	bool		Select;

		/**
		True of this PDB is active.

		@sa IPDB
		*/
		property bool	Active;

		/**
		Shows Biological Molecule if it exists.

		@sa IPDB
		*/
		//	virtual	 void	ShowBioUnit(IProteinInsight::IDisplayStyle displayStyle);
		virtual	 void	ShowBioUnit();

		/**
		True if this PDB is one of biounit molecule.

		@sa IPDB
		*/
		property bool	IsBioUnit	{ virtual bool get(); }

		/**
		True if this PDB is child of biounit molecule.

		@sa IPDB
		*/
		property bool	IsBioUnitChild	{ virtual bool get(); }

		/**
		True if this PDB is parent of biounit molecule.

		@sa IPDB
		*/
		property bool	IsBioUnitParent		{ virtual bool get(); }

		/**
		Manipulate Biological Molecules as one protein.

		@sa IPDB
		*/
		property bool	AttatchBioUnit;

		/**
		Move this PDB to center of screen.

		@sa IPDB
		*/
		virtual	void	MoveCenter();

		/**
		Rotate PDB geometry based on specific axis. Origin is (0,0,0)

		@sa IPDB
		*/
		virtual void	RotationAxis(Vector3 axis, float angle);

		/**
		Rotate PDB geometry based on X axis. Origin is (0,0,0)

		@sa IPDB
		*/
		virtual void	RotationX(float angle);
		/**
		Rotate PDB geometry based on Y axis. Origin is (0,0,0)

		@sa IPDB
		*/
		virtual void	RotationY(float angle);
		/**
		Rotate PDB geometry based on Z axis. Origin is (0,0,0)

		@sa IPDB
		*/
		virtual void	RotationZ(float angle);
		/**
		Move PDB geometry in screen coordinate

		@sa IPDB
		*/
		virtual	void	Move(float x, float y, float z);

		/**
		Get/Set local transform

		@b Example
@code
IPDB pdb = pi.PDBs[3];
Microsoft.DirectX.Matrix tr= pdb.TransformLocal;
tr.M41++;
pdb.TransformLocal = tr;
@endcode

		@sa IPDB
		*/
		property	Microsoft::DirectX::Matrix	TransformLocal;

		/**
		Get all chains of PDB. 
		
		@sa IPDB, IModel::GetChain
		*/
		virtual List<IChain ^> ^ GetChains(String ^ chainID);

		/**
		Get model of PDB. 

		@sa IPDB, IModel
		*/
		virtual IModel ^	GetModel(int modelNum);

		//	MODEL 이 없을때에는 Chains 를 사용해도 되고, Models[0].Chains를 사용해도 된다.
		/**
		Get chains of PDB. 

		@sa IPDB, IChain
		*/
		property List<IChain ^> ^ Chains { virtual List<IChain ^> ^ get(); }

		/**
		Get models of PDB. 

		@sa IPDB, IModel
		*/
		property List<IModel ^> ^ Models { virtual List<IModel ^> ^ get(); }
	};

	/**
	Clipping interface.
	With IClipping interface, you can visualize clipped geometry of PDB 

	@b Example 
	@code
pi.Open("1a31");             // Load Protein ID 1a31

IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Surface);
vp1.Property.Clippings[0].Enable = true;                            //  Turn on clipping plane  

for (float height = 20.0f; height > -20.0f; height -= 0.5f)
{
	String strEqu = String.Format("0, -1, 0, {0:f}", height);           //    Make clipping plane equation
	vp1.Property.Clippings[0].Equation = strEqu;                 //    Set new equation
	pi.Idle(10);
}
	@endcode
	@sa IProperty, IPropertyScene, IVP, IProperty::Clippings, IPropertyScene::Clipping
	*/
	public interface class IClipping
	{
	public:
		/**
		True if this clipping is enable.

		@sa IClipping
		*/
		property bool	Enable;

		/**
		True if this clipping pane is shown.

		@sa IClipping
		*/
		property bool	Show;

		/**
		Clipping plane color

		@a Example
@code
IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Surface);
vp1.Property.Clippings[0].Enable = true;                //  Turn on clipping plane  
vp1.Property.Clippings[0].Color = Color.Red;			//	Set clipping plane color to red
@endcode
		@sa IClipping
		*/
		property Color	Color;

		/**
		Set Transparency of clipping plane.

		@sa IClipping
		*/
		property int	Transparency;

		/**
		Set clip direction.

		@sa IClipping
		*/
		property bool	Direction;

		/**
		Set equation of clipping plane
		Format is "%f,%f,%f,%f". First 3 number is normal vector and last number is length.
		
		@b Example
@code
IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Surface);
vp1.Property.Clippings[0].Enable = true;            //  Turn on clipping plane  
String strEqu = "0,-1,0,5";							//    Make clipping plane equation
vp1.Property.Clippings[0].Equation = strEqu;        //    Set new equation
@endcode
		@sa IClipping
		*/
		property String ^ Equation;
	};

	/**
	IAnnotation interface.
	With IAnnotation interface, you can visualize name of VP, Atom and(or) Residue

@b Example
@code
IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Stick);
vp1.Property.AnnotationAtom.Show = true;            //  Show Annotation
vp1.Property.AnnotationAtom.FontHeight = 16;		//	Font Height(default font height is 10, font face is Arial)
vp1.Property.AnnotationAtom.DepthBias = 5;			//	Annotation text show in the front of model
@endcode

	@sa IProperty, IPropertyScene
	*/
	public interface class IAnnotation
	{
	public:
		/**
		Show/Hide Annotation

		@sa IAnnotation
		*/
		property	bool		Show;

		/**
		Get/Set annotation text

		@sa IAnnotation
		*/
		property	String ^	Text;

		/**
		Get/Set Annotation Font Face
		Default is Arial

		@sa IAnnotation
		*/
		property	String  ^		FontName;

		/**
		Get/Set Annotation Font Height
		Default is 10

		@sa IAnnotation
		*/
		property	int				FontHeight;

		/**
		Enumeraton of Annotaton Renderng Technique

		@sa IAnnotation
		*/
		enum class IDisplayMethod { EnableZBuffer, DisableZBuffer };

		/**
		Get/Set Annotation Rendering Technique

		@sa IAnnotation
		*/
		property	IDisplayMethod		DisplayMethod;

		/**
		Enumeration of Annotation Text format

		@sa IAnnotation
		*/
		enum class	ITextType { ThreeLetter, OneLetter };

		/**
		Get/Set Annotation Text format

		@sa IAnnotation
		*/
		property	ITextType	TextType;

		/**
		Enumeration Annotation Color

		@sa IAnnotation
		*/
		enum class	IColorScheme { FollowAtom, SingleColor };

		/**
		Get/Set Annotation Text Color Scheme

		@sa IAnnotation
		*/
		property	IColorScheme ColorScheme;

		/**
		Get/Set Annotation Text Color

		@sa IAnnotation
		*/
		property	Color		Color;

		/**
		Get/Set relative X position of annotation text 

		@sa IAnnotation
		*/
		property	int			RelativeXPos;

		/**
		Get/Set relative Y position of annotation text 

		@sa IAnnotation
		*/
		property	int			RelativeYPos;
		/**
		Get/Set relative Z position of annotation text 

		@sa IAnnotation
		*/
		property	int			RelativeZPos;
		/**
		Get/Set horizontal alignment of annotation text 

		@sa IAnnotation
		*/
		property	int			TextXPos;
		/**
		Get/Set vertical alignment of annotation text 

		@sa IAnnotation
		*/
		property	int			TextYPos;

		/**
		Get/Set depth bias of annotation text

		@sa IAnnotation
		*/
		property	int			DepthBias;

		/**
		Get/Set transparency of annotation text 

		@sa IAnnotation
		*/
		property	int			Transparency;
	};

	/**
	Visualization property interface.
	It's a common visualization property.

	@b Example 
	@code
IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Surface);
vp1.Property.ColorScheme = IProperty.IColorScheme.Residue;
	@endcode
	@sa IVP, IProperty, IPropertyScene
	*/
	public interface class IProperty
	{
	public:
		/**
		Name of Visualization Part(VP)

		@sa IProperty
		*/
		[Category("Common Option")]
		property String ^ Name { virtual String ^ get(); virtual void set(String ^ name); }

		/**
		visualization of side chain

		@sa IProperty
		*/
		[Category("Common Option")]
		property bool DisplaySideChain { virtual bool get(); virtual void set(bool display); }

		/**
		Enumeration of color scheme

		@b Example
		@code
vp1.Property.ColorScheme = IProperty.IColorScheme.CPK;
pi.Idle(2000);								//	pause 2 sec(2000 millisec)
vp1.Property.ColorScheme = IProperty.IColorScheme.Residue;
pi.Idle(2000);
vp1.Property.ColorScheme = IProperty.IColorScheme.Chain;
		@endcode
		@sa IProperty, IProperty::ColorScheme
		*/
		[Category("Common Option")]
		enum	class IColorScheme { CPK, Residue, Chain, Occupancy, Temparature, Progressive, Hydropathy, SingleColor , CustomColor };

		/**
		Color scheme of visualization of VP
		@b Example
		@code
		vp1.Property.ColorScheme = IProperty.IColorScheme.CPK;
		pi.Idle(2000);								//	pause 2 sec(2000 millisec)
		vp1.Property.ColorScheme = IProperty.IColorScheme.Residue;
		pi.Idle(2000);
		vp1.Property.ColorScheme = IProperty.IColorScheme.Chain;
		@endcode
		@sa IProperty, IProperty::IColorScheme
		*/
		[Category("Common Option")]
		property IColorScheme ColorScheme { virtual IColorScheme get(); virtual void set(IColorScheme colorScheme); }

		/**
		Customize color scheme 

		@b Example
		@code
		//	This is not allowed.
		//  pi.VPs[0].Property.CustomizeColors[0] = Color.FromArgb(255,255,0);

		//	These are OK.
		List<Color> colors = pi.VPs[0].Property.CustomizeColors;
		colors[0] = Color.FromArgb(255,255,0);
		pi.VPs[0].Property.CustomizeColors = colors;

		//	another example
		foreach ( IVP vp in pi.VPs )
		{
			vp.Property.ColorScheme = IProperty.IColorScheme.SingleColor;
			List<Color> colors = vp.Property.CustomizeColors;
			colors[0] = Color.FromArgb(255,255,0);
			vp.Property.CustomizeColors = colors;
		}

		@endcode
		@sa IProperty, IProperty::IColorScheme
		*/
		[Category("Common Option")]
		property List<Color> ^ CustomizeColors;
	
		/**
		Get clippings interface
		Clipping panes are two.

		@sa IProperty, IClipping, IPropertyScene::Clipping
		*/
		[Category("Common Option")]
		property array < IClipping ^ > ^ Clippings { virtual array < IClipping ^ > ^ get(); }

		/**
		Enumeration of shader quality

		@sa IProperty, IProperty::ShaderQuality, IPropertyScene::ShaderQuality
		*/
		[Category("Common Option")]
		enum class IShaderQuality { Low , High };

		/**
		Get/Set shader quality

		@sa IProperty, IProperty::IShaderQuality, IPropertyScene::ShaderQuality
		*/
		[Category("Common Option")]
		property	IShaderQuality	ShaderQuality;

		/**
		Enumeration of geometry quality

		@sa IProperty, IProperty::GeometryQuality, IPropertyScene::GeometryQuality
		*/

		enum class IGeometryQuality { VeryLow, Low, Medium, High, VeryHigh };		//	 0..4 5 step, vertex 갯수를 선택한다.

		/**
		Get/Set geometry quality

		@sa IProperty, IProperty::IGeometryQuality, IPropertyScene::GeometryQuality
		*/
		[Category("Common Option")]
		property	IGeometryQuality	GeometryQuality;

		/**
		Show/Hide selection mark

		@sa IProperty
		*/
		[Category("Common Option")]
		property	bool			ShowSelectionMark;

		/**
		Show/Hide Indicate selection mark

		@sa IProperty
		*/
		[Category("Common Option")]
		property	bool			ShowIndicateSelectionMark;

		/**
		Color of Indicate selection mark

		@sa IProperty
		*/
		[Category("Common Option")]
		property	Color			IndicateSelectionMarkColor;

		/**
		Get interface of Annotation of VP

		@sa IProperty
		*/
		[Category("Common Option")]
		property	IAnnotation	^	AnnotationVP { virtual IAnnotation	^ get(); }

		/**
		Get interface of Annotation of Atom

		@sa IProperty
		*/
		[Category("Common Option")]
		property	IAnnotation	^	AnnotationAtom{ virtual IAnnotation	^ get(); }

		/**
		Get interface of Annotation of Residue

		@sa IProperty
		*/
		[Category("Common Option")]
		property	IAnnotation	^	AnnotationResidue{ virtual IAnnotation	^ get(); }

		/**
		Get/Set ClipPS.
		If clipping plane is used, this option changes which shader geometry is cut.
		If ClipPS is true, pixel shader is used. If ClipPs is false, vertex shader is used.

		@sa IProperty
		*/
		//	Obsolete. 항상 TRUE
		//	property bool ClipPs;

		/**
		Show/Hide Axis geometry

		@sa IPropertyScene
		*/
		[Category("Common Option")]
		property	bool  DisplayAxis { virtual bool get(); virtual void set(bool display); }

		/**
		Size of Axis geometry

		@sa IPropertyScene
		*/
		[Category("Common Option")]
		property	int		AxisSize {  virtual int get(); virtual void set(int size); }

		/**
		Get/Set Ambient Intensity

		@sa IProperty
		*/
		[Category("Common Option")]
		property int IntensityAmbient;	

		/**
		Get/Set Diffuse Intensity

		@sa IProperty
		*/
		[Category("Common Option")]
		property int IntensityDiffuse;

		/**
		Get/Set Specular Intensity

		@sa IProperty
		*/
		[Category("Common Option")]
		property int IntensitySpecular;
	};

	/**
	Wireframe visualization interface

	@sa IVP, IProperty
	*/
	public interface class IPropertyWireframe
	{
	public:
		/**
		Enumeration of wireframe visualization mode.

		@sa IPropertyWireframe
		*/   
		enum class IDisplayMode { All, MainChain, Ca };
		/**
		Get/Set wireframe visualization mode.

		@sa IPropertyWireframe
		*/  
		[Category("Wireframe Display")]
		property IDisplayMode DisplayMode;

		/**
		Get/Set wireframe line width.
		Range is from 1 to 100. 1 is thin and 100 is thick

		@sa IPropertyWireframe
		*/ 
		[Category("Wireframe Display")]
		property int		LineWidth;
	};

	/**
	Stick visualization interface

	@sa IVP, IProperty
	*/
	public interface class IPropertyStick
	{
	public:

		/**
		Sphere resolution.
		Range is from 2 to 64

		@sa IPropertyStick
		*/
		[Category("Stick Display")]
		property int SphereResolution;
		/**
		Cylinder resolution.
		Range is from 2 to 64

		@sa IPropertyStick
		*/
		[Category("Stick Display")]
		property int CylinderResolution;
		/**
		Stick size
		
		@sa IPropertyStick
		*/
		[Category("Stick Display")]
		property double	StickSize;
	};

	/**
	Stick visualization interface

	@sa IVP, IProperty
	*/
	public interface class IPropertySpaceFill
	{
	public:
		/**
		Sphere resolution.
		Range is from 2 to 64

		@sa IPropertySpaceFill
		*/
		[Category("Spacefill Display")]
		property int	SphereResolution;
	};

	/**
	Ball and Stick visualization interface

	@sa IVP, IProperty
	*/
	public interface class IPropertyBallnStick
	{
	public:
		/**
		Sphere resolution.
		Range is from 2 to 64

		@sa IPropertyBallnStick
		*/
		[Category("Ball & Stick Display")]
		property int SphereResolution;
		/**
		Cylinder resolution.
		Range is from 2 to 64

		@sa IPropertyBallnStick
		*/
		[Category("Ball & Stick Display")]
		property int CylinderResolution;
		/**
		Sphere radius.

		@sa IPropertyBallnStick
		*/
		[Category("Ball & Stick Display")]
		property double SphereRadius;
		/**
		Cylinder size.

		@sa IPropertyBallnStick
		*/
		[Category("Ball & Stick Display")]
		property double CylinderSize;
	};

	/**
	Helix Property in ribbon visualization 

	@b Example
	@code
IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Ribbon);
vp1.PropertyRibbon.Helix.Show = true;
vp1.PropertyRibbon.Helix.Size = new Size(10, 10);
vp1.PropertyRibbon.Helix.ShowTexture = false;
	@endcode
	@sa IPropertyRibbon, IPropertySheet, IPropertyCoil
	*/
	public interface class IPropertyHelix
	{
	public:
		/**
		Show/Hide helix structure in ribbon visualization

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	bool		Show;

		/**
		Enable/Disable texture mapping in helix structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	bool		ShowTexture;

		/**
		Texture filename in helix structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	String ^	TextureFilename;

		/**
		Texture coordinate U in helix structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	int			TextureCoordU;

		/**
		Texture coordinate V in helix structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	int			TextureCoordV;

		/**
		Helix base color.

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	Color		Color;

		/**
		Helix size

		@b Example
@code
vp1.PropertyRibbon.helix.Size = new Size(100,100);
Size s = vp1.PropertyRibbon.helix.Size;
s.Width = s.Height = 100;

//	vp1.PropertyRibbon.helix.Size.Height = 100;		//	compile error CS1612.
@endcode
		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	Size		Size;

		/**
		Enumeration of Fitting method.
		Optimal : Visualize helix structure as cylinder geometry which places with optimal position.
		BeginEnd : Visualize helix structure as cylinder geometry which places in begin, end atom position
		Curve: Visualize helix structure as curve geometry such as coil geometry

		@sa IPropertyHelix::Fitting, IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		enum	class IFitting { Optimal, BeginEnd, Curve };
		/**
		Fitting method

		@sa IPropertyHelix::IFitting, IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	IFitting	Fitting;

		/**
		Enumeration of helix shape 

		@sa IPropertyHelix::Shape, IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		enum	class IShape { Ellipse, Triangle, Rectangle, _5Poly, _6Poly, _8Poly, _10Poly, _15Poly, _20Poly, _30Poly };

		/**
		Helix shape

		@sa IPropertyHelix::IShape, IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	IShape		Shape;

		/**
		Show/Hide Coil geometry in Helix geometry

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	bool		ShowCoilOnHelix;
	};

	/**
	Sheet Property in ribbon visualization 

	@b Example
	@code
IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Ribbon);
vp1.PropertyRibbon.Sheet.Show = true;
vp1.PropertyRibbon.Sheet.Size = new Size(10, 10);
vp1.PropertyRibbon.Sheet.ShowTexture = false;
	@endcode
	@sa IPropertyRibbon, IPropertyHelix, IPropertyCoil
	*/
	public interface class IPropertySheet
	{
	public:
		/**
		Show/Hide sheet structure in ribbon visualization

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	bool		Show;

		/**
		Enable/Disable texture mapping in sheet structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	bool		ShowTexture;

		/**
		Texture filename in sheet structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	String ^	TextureFilename;

		/**
		Texture coordinate U in sheet structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	int			TextureCoordU;

		/**
		Texture coordinate U in sheet structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	int			TextureCoordV;

		/**
		sheet base color.

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	Color		Color;

		/**
		Helix size

		@b Example
		@code
vp1.PropertyRibbon.Sheet.Size = new Size(100,100);
Size s = vp1.PropertyRibbon.Sheet.Size;
s.Width = s.Height = 100;
		@endcode
		@sa IPropertyRibbon, IPropertyHeix, IPropertyHeix::Size, IPropertySheet, IPropertyCoil
		*/
		property	Size		Size;

		/**
		Enumeration of sheet shape 

		@sa IPropertySheet::Shape, IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		enum class IShape { Ellipse, Triangle, Rectangle, _5Poly, _6Poly, _8Poly, _10Poly, _15Poly, _20Poly, _30Poly };

		/**
		Sheet shape

		@sa IPropertySheet::IShape, IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	IShape		Shape;

		/**
		Show/Hide Coil geometry in sheet geometry

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	bool		ShowCoilOnSheet;		
	};

	/**
	Coil Property in ribbon visualization 

	@b Example
	@code
IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Ribbon);
vp1.PropertyRibbon.Coil.Show = true;
vp1.PropertyRibbon.Coil.Size = new Size(10, 10);
vp1.PropertyRibbon.Coil.ShowTexture = false;
	@endcode
	@sa IPropertyRibbon, IPropertyHelix, IPropertySheet
	*/
	public interface class IPropertyCoil
	{
	public:
		/**
		Show/Hide coil structure in ribbon visualization

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	bool		Show;

		/**
		Enable/Disable texture mapping in coil structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	bool		ShowTexture;

		/**
		Texture filename in coil structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	String ^	TextureFilename;

		/**
		Texture coordinate U in coil structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	int			TextureCoordU;

		/**
		Texture coordinate V in coil structure

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	int			TextureCoordV;

		/**
		Coil base color.

		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	Color		Color;

		/**
		Coil size

		@b Example
		@code
		vp1.PropertyRibbon.Coil.Size = new Size(100,100);
		Size s = vp1.PropertyRibbon.Coil.Size;
		s.Width = s.Height = 100;
		@endcode
		@sa IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	Size		Size;

		/**
		Enumeration of coil shape 

		@sa IPropertyCoil::Shape, IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		enum	class IShape { Ellipse, Triangle, Rectangle, _5Poly, _6Poly, _8Poly, _10Poly, _15Poly, _20Poly, _30Poly };

		/**
		Coil shape

		@sa IPropertyCoil::IShape, IPropertyRibbon, IPropertyHeix, IPropertySheet, IPropertyCoil
		*/
		property	IShape		Shape;
	};

	/**
	Ribbon visualization property

	@sa IVP, IPropertyHelix, IPropertySheet, IPropertyCoil
	*/
	public interface class IPropertyRibbon
	{
	public:
		/**
		Curve tension of Ribbon geometry
		Curve tension range from 0 to 100

		@sa IPropertyRibbon
		*/
		[Category("Ribbon Display")] 
		property int	CurveTension;

		/**
		Curve resolution of Ribbon geometry
		Curve tension range from 1 to 20

		@sa IPropertyRibbon
		*/	
		[Category("Ribbon Display")] 
		property int	CurveResolution;

		/**
		Show sugar atoms of DNA if the VP is DNA

		@sa IPropertyRibbon
		*/		
		virtual void	SelectSugarInDNA();

		/**
		Select back bone atoms of DNA if the VP is DNA

		@sa IPropertyRibbon
		*/		
		virtual void	SelectBackBoneInDNA();

		/**
		Select inner atoms of DNA if the VP is DNA

		@sa IPropertyRibbon
		*/		
		virtual void	SelectInnerAtomsInDNA();

		/**
		Helix interface

		@sa IPropertyRibbon, IPropertyHelix
		*/
		[Category("Helix Display")] 
		property IPropertyHelix ^	Helix { virtual IPropertyHelix ^ get(); }

		/**
		Sheet interface

		@sa IPropertyRibbon, IPropertySheet
		*/
		[Category("Sheet Display")] 
		property IPropertySheet ^	Sheet { virtual IPropertySheet^ get(); }

		/**
		Coil interface

		@sa IPropertyRibbon, IPropertyCoil
		*/
		[Category("Coil Display")] 
		property IPropertyCoil ^	Coil { virtual IPropertyCoil ^ get(); }
	};

	/**
	Surface visualization property

	@sa IProperty
	*/
	public interface class IPropertySurface
	{
	public:
		/**
		Enumeration of Surface Display Method

		@sa IPropertySurface, IPropertySurface::DisplayMethod
		*/
		enum class IDisplayMethod { Solid, Wireframe, Point };

		/**
		visualize surface to wire frame geometry

		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	IDisplayMethod	DisplayMethod;

		/**
		Transparency of surface geometry
		0 is opaque, 100 is fully transparent(invisible)

		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	int		Transparency;

		/**	
		Radius of probe sphere in generating surface

		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	double	ProbeSphereRadius;

		/**
		Enumeration of geometry quality

		@sa IPropertySurface, IPropertySurface::GeometryQuality
		*/
		enum class IQuality { _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10 };

		/**
		Geometry quality of surface

		@sa IPropertySurface, IPropertySurface::IQuality
		*/
		[Category("Surface Display")] 
		property	IQuality		GeometryQuality;

		/**
		Enumeration of surface generation algorithm

		@sa IPropertySurface, IPropertySurface::Algorithm
		*/
		enum class IAlgorithm { MQ, MSMS };

		/**
		Set surface generation algorithm

		@sa IPropertySurface, IPropertySurface::IAlgorithm
		*/
		[Category("Surface Display")] 
		property IAlgorithm			Algorithm;

		/**
		Make surface included HETATM

		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	bool				AddHETATM;	

		/**
		Show/Hide curvature visualize

		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	bool				ShowCurvature;	

		/**
		enumeration of curvature depth
		_1 means narrow curvature(Faster).
		_5 means wide curvature(Slower).

		@sa IPropertySurface, IPropertySurface::CurvatureRingSize
		*/
		enum class ICurvatureRingSize { _1, _2, _3, _4, _5 , _6, _7, _8 };

		/**
		CurvatureRingSize
		_1 means narrow curvature(Faster).
		_5 means wide curvature(Slower).

		@sa IPropertySurface, IPropertySurface::CurvatureRingSize
		*/
		property	ICurvatureRingSize	CurvatureRingSize;

		/**
		Enumeration of color smoothing
		_5 is more smooth than _1

		@sa IPropertySurface, IPropertySurface::ColorSmoothing
		*/
		enum class IColorSmoothing { _1, _2, _3, _4, _5 };

		/**
		Enumeration of color smoothing factor

		@sa IPropertySurface, IPropertySurface::IColorSmoothing
		*/
		[Category("Surface Display")] 
		property	IColorSmoothing		ColorSmoothing;

		/**
		Enable/Disable Depth Sort of surface.
		
		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	bool				DepthSort;

		/**
		Enable/Disable Inner face color

		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	bool				UseInnerFaceColor;

		/**
		Inner face color blending factor

		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	int					InnerFaceColorBlend;

		/**
		Inner face color 

		@sa IPropertySurface
		*/
		[Category("Surface Display")] 
		property	Color				InnerFaceColor;

		/**
		Select atoms for surface

		@sa IPropertySurface
		*/
		virtual void SelectSurfaceAtoms();
	};

	/**
	Light Property

	@sa IPropertyScene::Lights
	*/
	public interface class ILight
	{
	public:
		/**
		Enable/Disable Light

		@sa ILight
		*/
		property	bool	Enable;
		/**
		Show/Hide Light geometry

		@sa ILight
		*/
		property	bool	Show;
		/**
		Get/Set Light Color

		@sa ILight
		*/
		property	Color	Color;
		/**
		Get/Set Light Intensity

		@sa ILight
		*/
		property	int		Intensity;
		/**
		Get/Set Light Position

		@sa ILight
		*/
		property	Vector3 Position;
	};

	/**
	Interface of property of rendering scene
	Global rendering property such as background color, lights, camera and so on.

	@b Example
@code
pi.Open("1NME");
pi.Property.ShaderQuality = IPropertyScene.IShaderQuality.High;
pi.Property.GeometryQuality = IPropertyScene.IGeometryQuality.VeryHigh;

IVP VP1 = pi.AddVP(pi.PDBs[0].GetChains("A"), IProteinInsight.IDisplayStyle.Ribbon);
IVP VP2 = pi.AddVP(pi.PDBs[0].GetChains("B"), IProteinInsight.IDisplayStyle.BallnStick);

for (Double degree = 0; degree < 360.0f; degree += 5.0)
{
	Double radius = 30.0f;
	Double xPos = radius * Math.Cos(Math.PI * degree / 180.0);
	Double zPos = radius * Math.Sin(Math.PI * degree / 180.0);
	pi.Property.CameraPosition = new Vector3((float)xPos, 0, (float)zPos);

	pi.Idle(50);
}
@endcode

	@sa IProteinInsight, IProteinInsight::Property
	*/
	public interface class IPropertyScene
	{
	public:
		/**
		Background color of rendering scene

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	Color BackgroundColor { virtual Color get(); virtual void set(Color color); }
		/**
		Show/Hide Background texture of rendering scene

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	bool  ShowBackgroundTexture { virtual bool get(); virtual void set(bool show); }
		/**
		Background texture filename 

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	String ^ BackgroundTextureFilename { virtual String ^ get(); virtual void set(String ^ show); }

		/**
		Enumeration of shader quality

		@sa IPropertyScene, IProperty::ShaderQuality, IPropertyScene::ShaderQuality
		*/
		enum class IShaderQuality { Low , High };						//	0,1 2 step. shader를 선택한다.

		/**
		Get/Set shader quality

		@sa IPropertyScene, IProperty::IShaderQuality, IPropertyScene::ShaderQuality
		*/
		[Category("Rendering Option")]
		property	IShaderQuality	ShaderQuality;

		/**
		Enumeration of geometry quality

		@sa IPropertyScene, IProperty::GeometryQuality, IPropertyScene::GeometryQuality
		*/
		enum class IGeometryQuality { VeryLow, Low, Medium, High, VeryHigh };		

		/**
		Get/Set geometry quality

		@sa IPropertyScene, IProperty::IGeometryQuality, IPropertyScene::GeometryQuality
		*/
		[Category("Rendering Option")]
		property	IGeometryQuality	GeometryQuality;

		/**
		Show/Hide selection mark

		@sa IProperty
		*/
		[Category("Rendering Option")]
		property	bool			ShowSelectionMark;

		/**
		Enable Ambient Occlusion

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	bool			EnableAO;

		/**
		The number of sampling of Ambient Occlusion(AO). Range is from 1 to 64

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	int				AOSampling;

		/**
		Range of Ambient Occlusion(AO). Range is from 0 to 200

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	int				AORange;

		/**
		Intensity of Ambient Occlusion(AO). Range is from 0 to 100

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	int				AOIntensity;

		/**
		Blur Type of Ambient Occlusion(AO). { None, Blur4Pixel, Blur16Pixel }

		@sa IPropertyScene
		*/
		enum class IAOBlurType { None, Blur4Pixel, Blur16Pixel };

		/**
		Blur Type of Ambient Occlusion(AO). One of { None, Blur4Pixel, Blur16Pixel }

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	IAOBlurType		AOBlurType;

		/**
		Buffer size of Ambient Occlusion(AO)
		True is full size. Quality is high, but use large video memory.

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	bool			AOFullSizeBuffer;

		/**
		Show/Hide depth of field(Fog)

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	bool  DepthOfField { virtual bool get(); virtual void set(bool dof); }
		/**
		Get/Set fog color

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	Color FogColor { virtual Color get(); virtual void set(Color color); }
		/**
		Get/Set fog start.

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	int	  FogStart { virtual int get(); virtual void set(int start); }
		/**
		Get/Set fog end.

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	int	  FogEnd   { virtual int get(); virtual void set(int end); }

		/**
		Get light interface
		The maximum number of light is two.

		@sa IPropertyScene, ILight
		*/
		[Category("Rendering Option")]
		property	array < ILight ^ > ^ Lights { virtual array < ILight ^ > ^ get(); }

		/**
		Get global clipping interface

		@sa IPropertyScene, IVP, IVP::Clippings
		*/
		[Category("Rendering Option")]
		property	IClipping ^		Clipping { virtual IClipping ^ get(); }

		/**
		Show/Hide Axis geometry

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	bool  DisplayAxis { virtual bool get(); virtual void set(bool display); }

		/**
		Size of Axis geometry

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	int		AxisSize {  virtual int get(); virtual void set(int size); }

		/**
		Enumeration of antialising quality

		@sa IPropertyScene, IPropertyScene::AntiAliasing
		*/
		[Category("Rendering Option")]
		enum class IAntiAliasing { None, Low, Medium, High  };

		/**
		Enable/Disable full screen anti-aliasing

		@sa IPropertyScene, IPropertyScene::IAntiAlising
		*/
		[Category("Rendering Option")]
		property	IAntiAliasing  AntiAliasing;

		/**
		Get/Set nearest clip view volume

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	double	ClipPlaneNear { virtual double	get(); virtual void set(double	near); }
		/**
		Get/Set farest clip view volume

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	double	ClipPlaneFar { virtual double	get(); virtual void set(double	far); }

		/**
		Camera position

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	Vector3	CameraPosition;

		/**
		Enumeration of camera type.

		@sa IPropertyScene, IPropertyScene::CameraType
		*/
		enum class ICameraType { Orthographic , Perspective };

		/**
		Camera type (Orthographic or Perspective camera).

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	ICameraType	CameraType;

		/**
		Perspective Camera FOV value.

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	int	FOV;

		/**
		Orthographic Camera size of view volume.

		@sa IPropertyScene
		*/
		[Category("Rendering Option")]
		property	int	SizeViewVol;

		/**
		Camera animation. Move camera to specified atom

		@param atom			atom camera move to
		@param time			animation time(sec)

		@sa IPropertyScene
		*/
		virtual void CameraAnimation(IAtom ^ atom, float time);

		/**
		Camera animation. Move camera to specified residue

		@param residue		residue camera move to
		@param time			animation time(sec)

		@sa IPropertyScene
		*/
		virtual void CameraAnimation(IResidue ^ residue, float time);

		/**
		Camera animation. Move camera to specified position

		@param pos			position camera move to
		@param time			animation time(sec)

		@sa IPropertyScene
		*/
		virtual void CameraAnimation(Vector3 pos , float time);

		/**
		Camera animation. Move camera to specified position

		@param vp			VP camera move to
		@param time			animation time(sec)

		@sa IPropertyScene, IVP
		*/
		virtual void CameraAnimation(IVP ^ vp, float time);

		/**
		Camera animation. Move camera to select atom(s)
		If there are one more atoms selected, target position of camera is mean position of atoms.

		@sa IPropertyScene, IVP
		*/
		virtual void CameraAnimation();
	};

	/**
	Utility Function Interface.

@code
for ( int i = 0 ; i < pi.VPs.Count ; i++ )
{
	IVP vp = pi.VPs[i];
	vp.Property.ColorScheme = IProperty.IColorScheme.SingleColor;
	List<Color> colors = vp.Property.CustomizeColors;
	colors[0] = pi.Utilities.GetGradientColor(i, pi.VPs.Count);
	vp.Property.CustomizeColors = colors;
}
@endcode
	@sa IProteinInsight
	*/
	public interface class IUtility
	{
	public:
		/**
		Get a iStep color in total step colors

		@sa	IUtility
		*/
		virtual Color GetGradientColor(int iStep, int nTotalStep);

		/**
		Get a iStep color in total step colors between color1 and color2

		@sa	IUtility
		*/
		virtual Color GetGradientColor(Color color1, Color color2, int iStep, int nTotalStep);

		/**
		Output text message in the output pane.

		@param msg output message
		@sa	IUtility
		*/
		virtual void OutputMsg(String ^ msg);

		/**
		Output text message in the status bar 

		@param msg output message
		@sa	IUtility
		*/
		virtual void OutputMsgInStatusBar(String ^ msg);

		/**
		Output progress in the status bar 

		@param progress progress status from 0 to 100
		@sa	IUtility
		*/
		virtual void SetProgressInStatusBar(int progress);

		/**
		Get Direct3D device interface

		@sa IProteinInsight, IUtility, Microsoft::DirectX::Direct3D::Device
		*/
		virtual Microsoft::DirectX::Direct3D::Device ^ GetDirect3DDevice9();
	};

	/**
	Movie interface.
	Movie interface provide simple and poweful interface for making movie.

@code
//    Set movie recording, filename is "rotation.wmv", size is 800x600, frame rate is 10
pi.Movie.BeginMovie("rotation.wmv", 800, 600, 10);

pi.Open("1d66");
pi.Property.BackgroundColor = Color.White;
pi.Property.ShowBackgroundTexture = false;
pi.Property.ShowSelectionMark = false;

pi.DeleteVP(pi.VPs[0]);

IVP vp1 = pi.AddVP(pi.PDBs[0].GetChains("D"), IProteinInsight.IDisplayStyle.Ribbon);
IVP vp2 = pi.AddVP(pi.PDBs[0].GetChains("E"), IProteinInsight.IDisplayStyle.Ribbon);

vp1.PropertyRibbon.Coil.Size = new Size(100,50);
vp1.PropertyRibbon.SelectInnerAtomsInDNA();
pi.AddVP(IProteinInsight.IDisplayStyle.BallnStick);
vp2.PropertyRibbon.Coil.Size = new Size(100,50);
vp2.PropertyRibbon.SelectInnerAtomsInDNA();
pi.AddVP(IProteinInsight.IDisplayStyle.BallnStick);

vp1.MoveCenter();

for (int i = 0; i < 360; i += 2)
{
	vp1.RotationY((float)Math.PI*2 /180.0f);
	vp1.RotationZ((float)Math.PI / 360.0f);
	pi.Movie.Capture(1);					//    make this screen to movie. 1 means one frame.
}

pi.Movie.EndMovie();						//    save and make movie. 
@endcode
	@sa IProteinInsight
	*/
	public interface class IMovie
	{
	public:
		/**
		Start movie making

		@param filename		movie filename
		@param width		movie width
		@param height		movie height
		@param fps			movie fps(frame per second)

		@sa IMovie
		*/
		virtual void BeginMovie(String ^ filename, int width, int height, int fps );
		/**
		Make movie with this rendering scene

		@param frame		The number of frames to make movie
		@sa IMovie
		*/
		virtual void Capture(int frame);

		/**
		Camera animation. Move camera to specified atom and capture movie

		@param atom			atom camera move to
		@param frame		total camera animation frame

		@sa IMovie, IPropertyScene
		*/
		virtual void CaptureCameraAnimation(IAtom ^ atom, int frame);

		/**
		Camera animation. Move camera to specified residue and capture movie

		@param residue		residue camera move to
		@param frame		total camera animation frame

		@sa IMovie, IPropertyScene
		*/
		virtual void CaptureCameraAnimation(IResidue ^ residue, int frame);

		/**
		Camera animation. Move camera to specified position and capture movie

		@param pos			position camera move to
		@param frame		total camera animation frame

		@sa IMovie, IPropertyScene
		*/
		virtual void CaptureCameraAnimation(Vector3 pos , int frame);

		/**
		Camera animation. Move camera to specified VP and capture movie

		@param vp			position camera move to
		@param frame		total camera animation frame

		@sa IMovie, IPropertyScene
		*/
		virtual void CaptureCameraAnimation(IVP ^ vp, int frame);

		/**
		Enumberation of caption position
		Not yet implemented.
		*/
		enum class ICaptionPosition{ Top, Bottom };
		/**
		Caption information
		Not yet implemented.
		*/
		virtual void Caption(String ^ strCaption, int frame, ICaptionPosition pos, String ^ fontFamily, int fontHeight );
		/**
		End making movie.
		Make movie with captured rendering scene

		@sa IMovie
		*/
		virtual void EndMovie();

		/**
		Cancel making movie.
		If you want to make new movie, you have to use BeginMovie function

		@sa IMovie
		*/
		virtual void CancelMovie();
	};

	/**
	Protein Insight Interface.

	Root Interface of all other interface
	@b Example
@code
pi.Open("1NME");
pi.AddPDB("1A31");

IPDB pdb1 = pi.PDBs[0];
IPDB pdb2 = pi.PDBs[1];

IVP vp1 = pi.AddVP(pdb1, IProteinInsight.IDisplayStyle.SpaceFill);
IVP vp2 = pi.AddVP(pdb2, IProteinInsight.IDisplayStyle.Ribbon);

vp2.Show = false;
pi.SaveImage("1NME", 1280, 1024, IProteinInsight.IImageFormat.PNG);
vp2.Show = true;
vp1.Show = false;

pi.SaveImage("1A31", 1280, 1024, IProteinInsight.IImageFormat.PNG);
@endcode
	@sa IVP, IPDB, IMovie, IPropertyScene
	*/
	public interface class IProteinInsight
	{
	public:
		/**
		Open PDB.
		
		@param filename filename or PDB ID. Available file formats are pdb, ent, piw
		
		@sa IProteinInsight
		*/
		virtual bool Open ( String ^ filename );	

		/**
		Add pdb in this workspace

		@param filename filename or PDB ID. Available file formats are pdb, ent 

		@sa IProteinInsight
		*/
		virtual bool AddPDB ( String ^ filename );

		/**
		Save this workspace to specified file

		@sa IProteinInsight
		*/
		virtual bool SaveWorkspace( String ^ filename );

		/**
		Close specified pdb
		@b Example
@code
pi.Open("1NME");
pi.AddPDB("1A31");

pi.ClosePDB(pi.PDBs[0]);
@endcode
		@sa IProteinInsight
		*/
		virtual bool ClosePDB ( IPDB ^ pdb );

		/**
		Close current workspace

		@sa IProteinInsight
		*/
		virtual bool CloseWorkspace();

		/**
		Set select all PDB 

		@param	select select or de-select the atom
		@param	bNeedUpdate apply selection to all user interface pane. refer to IProteinInsight::UpdateSelect

		@sa UpdateSelect, IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		virtual void SetSelect (bool select, bool bNeedUpdate);		//	전체를 Select/Deselect.

		/**
		Selection Type

		@sa UpdateSelect, IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		enum class ISelectType { C, N, O, Na, MG, P, S, HETATMwithWater , HETATMwithoutWater, CA, Backbone, SideChain, Hydrophilic, Hydrophobic, Helix, Sheet };

		/**
		Select Specific atoms in selected atoms.

		@param type one value of ISelectType

		@sa ISelectType, UpdateSelect, IAtom::SetSelect, IResidue::SetSelect, IChain::SetSelect, IModel::SetSelect, IPDB::SetSelect, Residue::SetSelect, IProteinInsight::SetSelect
		*/
		virtual void SetSelect ( ISelectType type );

		/**
		Full sync of selection 

		It is little slow, so you have to use it carefully.

		@b Example
Two code fragment has same function. But second code fragment is much faster than first.
@code
foreach (IAtom atom in chain.Atoms)
{
	atom.SetSelect(true, true);
}
@endcode
@code
foreach (IAtom atom in chain.Atoms)
{
	atom.SetSelect(true, false);
}
pi.UpdateSelect();
@endcode
		@sa IProteinInsight, SetSelect		
		*/
		virtual void UpdateSelect();
		
		/**
		Enumeration of display style

		@sa IProteinInsight
		*/
		enum	class IDisplayStyle { Wireframe, Stick, SpaceFill, BallnStick, Ribbon, Surface };

		/**
		Add current selection to new VP.
		
		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(IDisplayStyle displayStyle);
		
		/**
		Add specified PDB to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(IPDB ^ pdb, IDisplayStyle displayStyle);
		/**
		Add specified PDBs to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(List<IPDB ^> ^ pdbs, IDisplayStyle displayStyle);

		/**
		Add specified model to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(IModel ^ model, IDisplayStyle displayStyle);
		/**
		Add specified models to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(List<IModel ^> ^  models, IDisplayStyle displayStyle);

		/**
		Add specified chain to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(IChain ^ chain, IDisplayStyle displayStyle);
		/**
		Add specified chains to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(List<IChain ^> ^ chains, IDisplayStyle displayStyle);

		/**
		Add specified residue to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(IResidue ^ residue, IDisplayStyle displayStyle);
		/**
		Add specified residues to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(List<IResidue ^> ^ residues, IDisplayStyle displayStyle);

		/**
		Add specified atom to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(IAtom ^ atom, IDisplayStyle displayStyle);
		/**
		Add specified atoms to new VP.

		@sa IProteinInsight
		*/
		virtual IVP ^ AddVP(List<IAtom ^> ^ atoms, IDisplayStyle displayStyle);

		/**
		Delete specified VP

		@sa IProteinInsight
		*/
		virtual void DeleteVP(IVP ^ selection);

		/**
		result = selection of vp1 - selection of vp2

		@sa IProteinInsight
		*/
		virtual void VPSubtrctVP(IVP ^ vp1, IVP ^ vp2);

		/**
		result = union( selection of vp1 , selection of vp2 )

		@sa IProteinInsight
		*/
		virtual void VPUnionVP(IVP ^ vp1, IVP ^ vp2);

		/**
		result = intersect ( selection of vp1 , selection of vp2 )

		@sa IProteinInsight
		*/
		virtual void VPIntersectVP(IVP ^ vp1, IVP ^ vp2);

		/**
		Idle 

		@param millisecond idle time(1 second = 1000 millisecond)

		@sa IProteinInsight
		*/
		virtual void Idle(double millisecond);

		/**
		Save image file format

		@sa IProteinInsight, IProteinInsight::SaveImage
		*/
		enum class IImageFormat { PNG, BMP, JPG, DIB };

		/**
		Save current rendering scene to image file

		@param filename	file name
		@param width	image file width
		@param height	image file height
		@param format	image file format

		@sa	IProteinInsight, IProteinInsight::IImageFormat
		*/
		virtual void SaveImage(String ^ filename, int width, int height, IImageFormat format);

		/**
		Script directory.

		@sa IProteinInsight::ScriptPath
		*/
		property String ^ ScriptDir { virtual String ^ get(); }

		/**
		Script full path.

		ScriptDir + filename + .cs

		@sa IProteinInsight::ScriptDir
		*/
		property String ^ ScriptPath { virtual String ^ get(); }

		/**
		Plug in directory

		@sa IProteinInsight::PlugInPath
		*/
		property String ^ PlugInDir { virtual String ^ get(); }	

		/**
		Plug in full path.

		@sa IProteinInsight::PlugInDir
		*/
		property String ^ PlugInPath { virtual String ^ get(); }				

		/**
		Protein Insight directory

		@sa IProteinInsight
		*/
		property String ^ ProteinInsightDir { virtual String ^ get(); }	

		/**
		Default PDB download directory

		@sa IProteinInsight
		*/
		property String ^ PDBDir { virtual String ^ get(); }	

		/**
		Default workspace directory

		@sa IProteinInsight
		*/
		property String ^ WorkspaceDir { virtual String ^ get(); }	

		/**
		Default movie directory

		@sa IProteinInsight
		*/
		property String ^ MovieDir { virtual String ^ get(); }	

		/**
		Get VP list

		@sa IProteinInsight, IVP
		*/
		property List <IVP ^> ^ VPs { virtual List<IVP ^> ^ get(); }

		/**
		Get PDB list 

		@sa IProteinInsight, IPDB
		*/
		property List <IPDB^> ^ PDBs	{ virtual List<IPDB^> ^ get(); }

		/**
		Get scene property interface

		@sa IProteinInsight, IPropertyScene
		*/
		property IPropertyScene ^ Property { virtual IPropertyScene ^ get(); }

		/**
		Get Movie interface

		@sa IProteinInsight, IMovie
		*/
		property IMovie ^ Movie { virtual IMovie ^ get(); }
		
		/**
		Move camera to view all protein

		@sa IProteinInsight, IVP::ViewAll
		*/
		virtual void ViewAll();

		/**
		Get Utilty function interface

		@sa IProteinInsight, IColor
		*/
		property IUtility ^ Utility { virtual IUtility ^ get(); }
	};

	/**
	Visualization Part(VP) Interface.
	
	@b Example
	@code
pi.Open("1NME");
pi.AddPDB("1A31");

IPDB pdb1 = pi.PDBs[0];
IPDB pdb2 = pi.PDBs[1];

IVP vp1 = pi.AddVP(pdb1, IProteinInsight.IDisplayStyle.SpaceFill);
IVP vp2 = pi.AddVP(pdb2, IProteinInsight.IDisplayStyle.Ribbon);
	@endcode
	@sa IProteinInsight, IProperty, IPropertyWireframe, IPropertyStick, IPropertySpaceFill, IPropertyBallnStick, IPropertyRibbon, IPropertySurface
	*/
	public interface class IVP
	{
	public:
		/**
		Get common visualization property interface

		@sa IVP
		*/
		property	IProperty			^ Property { virtual IProperty	^ get(); }
		/**
		Get wireframe visualization property interface

		@sa IVP
		*/
		property	IPropertyWireframe	^ PropertyWireframe { virtual IPropertyWireframe	^ get(); }
		/**
		Get stick visualization property interface

		@sa IVP
		*/
		property	IPropertyStick		^ PropertyStick { virtual IPropertyStick	^ get(); }
		/**
		Get space fill visualization property interface

		@sa IVP
		*/
		property	IPropertySpaceFill	^ PropertySpaceFill { virtual IPropertySpaceFill	^ get(); }

		/**
		Get ball and stick visualization property interface

		@sa IVP
		*/
		property	IPropertyBallnStick	^ PropertyBallnStick { virtual IPropertyBallnStick	^ get(); }

		/**
		Get ribbon visualization property interface

		@sa IVP
		*/
		property	IPropertyRibbon		^ PropertyRibbon { virtual IPropertyRibbon	^ get(); }

		/**
		Get surface visualization property interface

		@sa IVP
		*/
		property	IPropertySurface	^ PropertySurface { virtual IPropertySurface	^ get(); }

		/**
		Get/Set display style of current VP.

		Display style is one of 
		Wireframe(IProteinInsight.IDisplayStyle.Wireframe), 
		Stick(IProteinInsight.IDisplayStyle.Stick), 
		SpaceFill(IProteinInsight.IDisplayStyle.SpaceFill),
		BallnStick(IProteinInsight.IDisplayStyle.BallnStick), 
		Ribbon(IProteinInsight.IDisplayStyle.Ribbon) or 
		Surface(IProteinInsight.IDisplayStyle.Surface).

		@sa IVP, IProteinInsight::IDisplayStyle
		*/
		property	IProteinInsight::IDisplayStyle	DisplayStyle;

		/**
		Get PDB ID of current VP.

		@sa IProteinInsight
		*/
		property	String ^ PDBID { virtual String ^ get(); }

		/**
		Get PDB file name of current VP.

		@sa IProteinInsight
		*/
		property	String ^ Filename { virtual String ^ get(); }

		/**
		Show/Hide current VP

		@sa IProteinInsight
		*/
		property	bool	Show { virtual bool get(); virtual void set(bool show); }

		/**
		Name of current VP

		@sa IProteinInsight
		*/
		property	String ^ Name;

		/**
		select/deselect current VP

		@sa IProteinInsight
		*/
		property	bool	Select;

		/**
		Move current VP to center of screen

		@sa IProteinInsight
		*/
		virtual	void MoveCenter();

		/**
		Rotate current VP based on specified rotation axis. Origin is center of VP

		@param axis rotation axis
		@param angle radian angle

		@sa IProteinInsight
		*/
		virtual void RotationAxis(Vector3 axis, float angle);

		/**
		Rotate current VP based on X axis. Origin is center of VP

		@param angle radian angle

		@sa IProteinInsight
		*/
		virtual void RotationX(float angle);
		/**
		Rotate current VP based on Y axis. Origin is center of VP

		@param angle radian angle

		@sa IProteinInsight
		*/
		virtual void RotationY(float angle);

		/**
		Rotate current VP based on Z axis. Origin is center of VP

		@param angle radian angle

		@sa IProteinInsight
		*/
		virtual void RotationZ(float angle);

		/**
		Move current VP to specified position in screen coordinate

		@sa IProteinInsight
		*/
		virtual	void Move(float x, float y, float z);

		/**
		Move Camera to view current VP

		@param time animation time. 0 is no animation

		@sa IProteinInsight, IProteinInsight::ViewAll
		*/
		virtual	void ViewAll(float time);

		/**
		Get/Set local transform of current VP

		@b Example
		@code
Microsoft.DirectX.Matrix tr= pi.TransformLocal;
tr.M41++;
pi.TransformLocal = tr;
		@endcode
		@sa IProteinInsight
		*/
		property	Microsoft::DirectX::Matrix	TransformLocal;

		/**
		Get atom list in current VP

		@sa IProteinInsight
		*/
		property List<IAtom ^ > ^ Atoms { virtual List<IAtom ^> ^ get(); }

		/**
		Get PDB in current VP

		@sa IProteinInsight
		*/
		property IPDB ^ PDB { virtual IPDB ^ get(); }
	};

	//	plugIn Interface.
	public interface class IProteinInsightPlugin
	{
	public:
		property String ^ Name { String ^ get(); }
		void Run(IProteinInsight ^ ProteinInsight);
	};

	//	Render Plug-in
	//public interface class IProteinInsightRenderPlugin
	//{
	//public:
	//	property String ^ Name { String ^ get(); }
	//	void Init(IProteinInsight ^ ProteinInsight);
	//	void Render(IProteinInsight ^ ProteinInsight);
	//	void Destroy(IProteinInsight ^ ProteinInsight);
	//};
}

