#include "StdAfx.h"
#include "PDB.h"
#include "stride\stride.h"

BOOL CPDB::StrideWrapper()
{
	long first_chain = 0;
	long NChain = GetNumChain(0);

	BOOL return_value = TRUE;

	CHAIN **Chain = NULL;
	HBOND **HBond = NULL;
	DONOR **Dnr = NULL;
	ACCEPTOR **Acc = NULL;
	RESIDUE *r;
	COMMAND *Cmd;
	int Cn, NHBond = 0, ValidChain = 0;
	float **PhiPsiMapHelix = NULL, 
		**PhiPsiMapSheet = NULL;
	int i, j, k;

	// The total number of residues
	int num_residues = 0;

	// Count the total number of residues
	/*
	for(i = 0;i < NChain;i++){
		array<amino> &sequence_ref = secondaryStruc[i + first_chain].sequence;
		num_residues += sequence_ref.Length();
	}
	*/
	for ( i = 0; i < GetNumChain(0); i++ )
	{
		num_residues += GetChain(0,i)->m_arrayResidue.size();
	}


	// Dynamically allocate memory for the required number of
	// chains.
	Chain = (CHAIN  **)ckalloc(NChain*sizeof(CHAIN *));
	//Chain = (CHAIN  **)ckalloc(MAX_CHAIN*sizeof(CHAIN *));

	// Dynamically allocate memory for the required number of
	// chains.
	//HBond = (HBOND  **)ckalloc(MAXHYDRBOND*sizeof(HBOND *));
	HBond = (HBOND  **)ckalloc(MAXHYDRBOND(num_residues)*sizeof(HBOND *));
	Cmd   = (COMMAND *)ckalloc(sizeof(COMMAND));

	// Allocate memory within the COMMAND structure
	/* char Active[MAX_CHAIN+1]; */
	/* char Processed[MAX_CHAIN+1]; */
	Cmd->Active = (char *)ckalloc((NChain+1)*sizeof(char));
	Cmd->Processed = (char *)ckalloc((NChain+1)*sizeof(char));

	// Initialize arrays to make memory clean up easier.
	//for(i = 0;i < MAX_CHAIN;i++)
	for(i = 0;i < NChain;i++)
		Chain[i] = NULL;

	for(i = 0;i < MAXHYDRBOND(num_residues);i++)
		HBond[i] = NULL;

	try{
		////////////// Set the COMMAND structure //////////////
		DefaultCmd(Cmd, num_residues);

		strcpy(Cmd->InputFile, ""); // <-- There is not default input file!

		Cmd->NActive = (int)strlen(Cmd->Active);
		Cmd->NProcessed = (int)strlen(Cmd->Processed);

		if( Cmd->Measure ) {
			Cmd->BrookhavenAsn = YES;
			Cmd->DsspAsn = YES;
		}
		///////////////////////////////////////////////////////

		for(i = 0;i < NChain;i++)
		{
			//	array<amino> &sequence_ref = secondaryStruc[i + first_chain].sequence;
			CChain * pChain = GetChain(0,i);

			StrideInitChain(&(Chain[i]), Cmd);

			// The number of animoacids in the chain
			Chain[i]->NRes = pChain->m_arrayResidue.size();
			Chain[i]->NHetRes = 0;
			Chain[i]->NonStandRes = 0;
			Chain[i]->Ter = 0;

			Chain[i]->Id = pChain->m_chainID;
			Chain[i]->Published = TRUE; 
			Chain[i]->DsspAssigned = FALSE;

			// Translate the atomic level detail of the atoms array into
			// the CHAIN structure used by Stride.

			for(j = 0;j < Chain[i]->NRes;j++)
			{
				CResidue * pResidue = pChain->m_arrayResidue[j];

				// Only assign the residue memory, name and number once per residue
				Chain[i]->Rsd[j] = (RESIDUE *)ckalloc(sizeof(RESIDUE));

				r = Chain[i]->Rsd[j];

				// Initialize these pointers to NULL so we can clean up in the event of 
				// an error.
				r->Inv =  NULL;
				r->Prop = NULL;

				sprintf(r->PDB_ResNumb,"%d", pResidue->GetResidueNum() );
				strcpy(r->ResType, pResidue->GetResidueName());

				r->NAtom = pResidue->m_arrayAtom.size();

				if(r->NAtom > MAX_AT_IN_RES)
					throw "Too many atoms in residue!";

				for(k = 0;k < r->NAtom;k++)
				{
					int cur_atom = k;
					CString atomName = pResidue->m_arrayAtom[cur_atom]->m_atomName;
					atomName.TrimLeft();
					atomName.TrimRight();

					int str_len = strlen(atomName);

					if(str_len >= AT_FIELD)
					{
						strncpy(r->AtomType[k], atomName, AT_FIELD - 1);
						r->AtomType[k][AT_FIELD - 1] = '\0';
					}
					else{
						strcpy(r->AtomType[k], atomName);
					}

					r->Coord[k][0] = pResidue->m_arrayAtom[cur_atom]->m_pos.x;
					r->Coord[k][1] = pResidue->m_arrayAtom[cur_atom]->m_pos.y;
					r->Coord[k][2] = pResidue->m_arrayAtom[cur_atom]->m_pos.z;

					r->Occupancy[k] = pResidue->m_arrayAtom[cur_atom]->m_occupancy;
					r->TempFactor[k] = pResidue->m_arrayAtom[cur_atom]->m_temperature;
				}

				r->Inv =  (INVOLVED *)ckalloc(sizeof(INVOLVED));
				r->Prop = (PROPERTY *)ckalloc(sizeof(PROPERTY));
				r->Inv->NBondDnr = 0;
				r->Inv->NBondAcc = 0;
				r->Inv->InterchainHBonds = NO;
				r->Prop->Asn     = 'C';
				r->Prop->PdbAsn  = 'C';
				r->Prop->DsspAsn = 'C';
				r->Prop->Solv    = 0.0;
				r->Prop->Phi     = 360.0;
				r->Prop->Psi     = 360.0;
			}
		}
		///////////////////////////////////////////////////////

		for( Cn=0; Cn<NChain; Cn++ )
			ValidChain += CheckChain(Chain[Cn],Cmd);

		if( !ValidChain ) 
			die("No valid chain\n");

		if( Cmd->BrookhavenAsn )
			GetPdbAsn(Chain,NChain);

		if( Cmd->DsspAsn )
			GetDsspAsn(Chain,NChain,Cmd);

		BackboneAngles(Chain,NChain);

		if( Cmd->OutSeq )
			OutSeq(Chain,NChain,Cmd);

		if( !strlen(Cmd->MapFileHelix) )
			PhiPsiMapHelix = DefaultHelixMap(Cmd);
		else
			ReadPhiPsiMap(Cmd->MapFileHelix,&PhiPsiMapHelix,Cmd);

		if( !strlen(Cmd->MapFileSheet) )
			PhiPsiMapSheet = DefaultSheetMap(Cmd);
		else
			ReadPhiPsiMap(Cmd->MapFileSheet,&PhiPsiMapSheet,Cmd);

		for( Cn=0; Cn<NChain; Cn++ )
			PlaceHydrogens(Chain[Cn]);

		if( (NHBond = FindHydrogenBonds(Chain,Cn,HBond,Cmd,&Dnr,&Acc)) == 0 ) 
			die("No hydrogen bonds found in %s\n",Cmd->InputFile);

		NoDoubleHBond(HBond,NHBond);

		DiscrPhiPsi(Chain,NChain,Cmd);

		if(Cmd->ExposedArea)
			Area(Chain,NChain,Cmd);

		for(Cn = 0;Cn < NChain;Cn++){

			if(Chain[Cn]->Valid){

				Helix(Chain,Cn,HBond,Cmd,PhiPsiMapHelix);

				for(i = 0;i < NChain;i++) 
					if(Chain[i]->Valid)
						Sheet(Chain,Cn,i,HBond,Cmd,PhiPsiMapSheet);    

				BetaTurn(Chain,Cn);
				GammaTurn(Chain,Cn,HBond);
			}
		}

		// Parse the Stride data to obtain secondary structure information
		for(i = 0;i < NChain;i++) 
		{
			CChain * pChain = GetChain(0,i);

			for(j = 0;j < Chain[i]->NRes;j++ )
			{
				RESIDUE *ptr = Chain[i]->Rsd[j];

				long secondaryStructure = SS_NONE;
				long typeHelix = 0;

				if ( ptr->Prop->Asn == 'H' )
				{
					secondaryStructure = SS_HELIX;
					typeHelix = SS_HELIX_DEFAULT;
				}
				else if ( ptr->Prop->Asn == 'G' )
				{
					secondaryStructure = SS_HELIX;
					typeHelix = SS_HELIX_310;
				}
				else if ( ptr->Prop->Asn == 'I' )
				{
					secondaryStructure = SS_HELIX;
					typeHelix = SS_HELIX_PI;
				}
				else if ( ptr->Prop->Asn == 'E' )
				{
					secondaryStructure = SS_SHEET;
				}

				if ( secondaryStructure != SS_NONE )
				{
					CResidue * pResidue = pChain->m_arrayResidue[j];
					for ( int k = 0 ; k < pResidue->m_arrayAtom.size() ; k++ )
					{
						CAtom * pAtom = pResidue->m_arrayAtom[k];
						pAtom->m_secondaryStructure = secondaryStructure;
						pAtom->m_typeHelix = typeHelix;

						//pAtom->m_bBeginHelix
						//pAtom->m_bEndHelix
						//pAtom->m_bBeginSheet
						//pAtom->m_bEndSheet

					}
				}
			}
		}
	}
	catch(char *){
		// Catch a thrown "char *" error (Stride has been modified to
		// throw such an error). Note that the ellipsis version of catch
		// (i.e. catch (...)) will not catch a "throw;" -- must throw a 
		// variable (i.e. throw "Stride Error").
		return_value = FALSE;
	}

	// Clean up memory allocated by Stride
	for(i = 0;i < NChain;i++){
		if(Chain[i]){
			if(Chain[i]->File)
				free(Chain[i]->File);

			if(Chain[i]->Rsd){
				for(j = 0;j < Chain[i]->NRes;j++)
					if(Chain[i]->Rsd[j]){
						free(Chain[i]->Rsd[j]->Inv);

						free(Chain[i]->Rsd[j]->Prop);

						free(Chain[i]->Rsd[j]);
					}

					free(Chain[i]->Rsd);
			}

			if(Chain[i]->HetRsd){
				for(j = 0;j < MAX_HETRES;j++)
					if(Chain[i]->HetRsd[j])
						free(Chain[i]->HetRsd[j]);

				free(Chain[i]->HetRsd);
			}

			if(Chain[i]->Het){
				for(j = 0;j < MAX_HET;j++)
					if(Chain[i]->Het[j])
						free(Chain[i]->Het[j]);

				free(Chain[i]->Het);
			}

			if(Chain[i]->Helix){
				for(j = 0;j < MAX_HELIX;j++)
					if(Chain[i]->Helix[j])
						free(Chain[i]->Helix[j]);

				free(Chain[i]->Helix);
			}

			if(Chain[i]->Sheet){
				for(j = 0;j < MAX_SHEET;j++)
					if(Chain[i]->Sheet[j])
						free(Chain[i]->Sheet[j]);

				free(Chain[i]->Sheet);
			}

			if(Chain[i]->Turn){
				for(j = 0;j < MAX_TURN;j++)
					if(Chain[i]->Turn[j])
						free(Chain[i]->Turn[j]);

				free(Chain[i]->Turn);
			}

			if(Chain[i]->AssignedTurn){
				for(j = 0;j < MAX_TURN;j++){
					if(Chain[i]->AssignedTurn[j])
						free(Chain[i]->AssignedTurn[j]);
				}

				free(Chain[i]->AssignedTurn);
			}

			if(Chain[i]->SSbond){
				for(j = 0;j < MAX_BOND;j++)
					if(Chain[i]->SSbond[j])
						free(Chain[i]->SSbond[j]);

				free(Chain[i]->SSbond);
			}

			if(Chain[i]->Info){
				for(j = 0;j < MAX_INFO;j++)
					if(Chain[i]->Info[j])
						free(Chain[i]->Info[j]);

				free(Chain[i]->Info);
			}

			free(Chain[i]);
		}
	}

	for(i = 0;i < NHBond;i++)
		if(HBond[i]){
			free(HBond[i]);
		}

		free(Chain);
		free(HBond);

		if(Dnr){
			for(i = 0;i < MAXDONOR(num_residues);i++)
				if(Dnr[i])
					free(Dnr[i]);

			free(Dnr);
		}

		if(Acc){
			for(i = 0;i < MAXACCEPTOR(num_residues);i++)
				if(Acc[i])
					free(Acc[i]);

			free(Acc);
		}

		if(PhiPsiMapHelix)
			free(PhiPsiMapHelix);

		if(PhiPsiMapSheet)
			free(PhiPsiMapSheet);

		if(Cmd){
			if(Cmd->Active){
				free(Cmd->Active);
			}

			if(Cmd->Processed){
				free(Cmd->Processed);
			}

			free(Cmd);
		}

		return return_value;
}
