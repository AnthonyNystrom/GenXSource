#include "StdAfx.h"

/*
* Stride has been included in Qmol with the kind permission of 
* 
* Dmitrij Frishman, PhD
* Institute for Bioinformatics
* GSF - Forschungszentrum f? Umwelt und Gesundheit, GmbH
* Ingolst?ter Landstra? 1,
* D-85764 Neuherberg, Germany
*
* Telephone: +49-89-3187-4201
* Fax: +49-89-31873585
* e-mail: d.frishman@gsf.de
* WWW: http://mips.gsf.de/mips/staff/frishman/
*
* Stride copyright (see http://www.embl-heidelberg.de/stride/stride.html):
*
* All rights reserved, whether the whole  or  part  of  the  program  is
* concerned.  Permission  to use, copy, and modify this software and its
* documentation is granted for academic use, provided that:
*
*
* i.	this copyright notice appears in all copies of the software  and
*		related documentation;
*
* ii.  the reference given below (Frishman and  Argos,  1995)  must  be
*		cited  in any publication of scientific results based in part or
*		completely on the use of the program;
*
* iii.  bugs will be reported to the authors.
*
* The use of the  software  in  commercial  activities  is  not  allowed
* without a prior written commercial license agreement.
* 
* WARNING: STRIDE is provided "as-is" and without warranty of any  kind,
* express,  implied  or  otherwise,  including  without  limitation  any
* warranty of merchantability or fitness for a particular purpose. In no
* event will the authors be liable for any special, incidental, indirect
* or consequential damages  of  any  kind,  or  any  damages  whatsoever
* resulting  from loss of data or profits, whether or not advised of the
* possibility of damage, and on any theory of liability, arising out  of
* or in connection with the use or performance of this software.
* 
* For calculation of the residue solvent accessible area the program NSC
* [3,4]   is   used   and   was  kindly  provided  by  Dr.  F.Eisenhaber
* (EISENHABER@EMBL-HEIDELBERG.DE). Please direct to  him  all  questions
* concerning specifically accessibility calculations.
* 
* Stride References:
* 
* 1.	Frishman,D & Argos,P. (1995) Knowledge-based secondary structure
* 		assignment.  Proteins:  structure, function and genetics, 23,
* 		566-579.
* 
* 2.	Kabsch,W. & Sander,C. (1983)  Dictionary  of  protein  secondary
* 		structure:    pattern   recognition   of   hydrogen-bonded   and
* 		geometrical features. Biopolymers, 22: 2577-2637.
* 
* 3.	Eisenhaber,  F.  and  Argos,  P.  (1993)  Improved  strategy  in
* 		analytic  surface calculation for molecular systems: handling of
* 		singularities and computational efficiency. J. comput. Chem. 14,
* 		1272-1280.
* 
* 4.	Eisenhaber, F., Lijnzaad, P., Argos, P., Sander, C., and Scharf,
* 		M.  (1995) The double cubic lattice method: efficient approaches
* 		to numerical integration of surface area and volume and  to  dot
* 		surface contouring of molecular assemblies. J. comput. Chem. 16,
* 		273-284.
* 
* 5.	Bernstein, F.C., Koetzle, T.F.,  Williams,  G.J.,  Meyer,  E.F.,
* 		Brice,  M.D.,  Rodgers,  J.R., Kennard, O., Shimanouchi, T., and
* 		Tasumi, M.  (1977)  The  protein  data  bank:  a  computer-based
* 		archival  file for macromolecular structures. J. Mol. Biol. 112,
* 		535-542.
* 
* 6.	Kraulis, P.J.  (1991)  MOLSCRIPT:  a  program  to  produce  both
* 		detailed  and  schematic  plots  of protein structures. J. Appl.
* 		Cryst. 24, 946-950.
* 
* 7.	Pearson, W.R. (1990) Rapid  and  sensitive  sequence  comparison
* 		with FASTP and FASTA. Methods. Enzymol. 183, 63-98.
* 
*/
#include "stride.h"

int FindDnr(CHAIN *Chain, DONOR **Dnr, int *NDnr, COMMAND *Cmd)
{

	int Res, dc;
	char Rsd[RES_FIELD];

	dc = *NDnr;

	for( Res=0; Res<Chain->NRes; Res++ ) {

		strcpy(Rsd,Chain->Rsd[Res]->ResType);

		DefineDnr(Chain,Dnr,&dc,Res,Nsp2,Peptide,1.90f,0);

		if( !Cmd->SideChainHBond ) continue;

		if( !strcmp(Rsd,"TRP") ) 
			DefineDnr(Chain,Dnr,&dc,Res,Nsp2,Trp,1.90f,0); 
		else if( !strcmp(Rsd,"ASN") ) DefineDnr(Chain,Dnr,&dc,Res,Nsp2,Asn,1.90f,0); 
		else if( !strcmp(Rsd,"GLN") ) DefineDnr(Chain,Dnr,&dc,Res,Nsp2,Gln,1.90f,0); 
		else if( !strcmp(Rsd,"ARG") ) {
			DefineDnr(Chain,Dnr,&dc,Res,Nsp2,Arg,1.90f,1);
			DefineDnr(Chain,Dnr,&dc,Res,Nsp2,Arg,1.90f,2);
			DefineDnr(Chain,Dnr,&dc,Res,Nsp2,Arg,1.90f,3);
		}
		else if( !strcmp(Rsd,"HIS") ) {
			DefineDnr(Chain,Dnr,&dc,Res,Nsp2,His,1.90f,1);
			DefineDnr(Chain,Dnr,&dc,Res,Nsp2,His,1.90f,2);
		}
		else if( !strcmp(Rsd,"LYS") ) DefineDnr(Chain,Dnr,&dc,Res,Nsp3,Lys,2.10f,0); 
		else if( !strcmp(Rsd,"SER") ) DefineDnr(Chain,Dnr,&dc,Res,Osp3,Ser,1.70f,0);
		else if( !strcmp(Rsd,"THR") ) DefineDnr(Chain,Dnr,&dc,Res,Osp3,Thr,1.70f,0);
		else if( !strcmp(Rsd,"TYR") ) DefineDnr(Chain,Dnr,&dc,Res,Osp2,Tyr,1.70f,0); 
	}

	*NDnr = dc;
	return(dc);
}

int DefineDnr(CHAIN *Chain, DONOR **Dnr, int *dc, int Res, enum HYBRID Hybrid, enum GROUP Group,
			  float HB_Radius, int N)
{

	/* Prevent memory leaks */
	if(Dnr[*dc])
	free(Dnr[*dc]);

	Dnr[*dc] = (DONOR *)ckalloc(sizeof(DONOR));

	Dnr[*dc]->Chain = Chain;
	Dnr[*dc]->D_Res = Res;
	if( Group != Peptide )
		Dnr[*dc]->DD_Res = Res;
	else
		Dnr[*dc]->DD_Res = Res-1;
	Dnr[*dc]->DDI_Res = Res;
	Dnr[*dc]->Hybrid = Hybrid;
	Dnr[*dc]->Group = Group;
	Dnr[*dc]->HB_Radius = HB_Radius;

	if( Group == Peptide ) {
		if( Res != 0 ) {
			stride_FindAtom(Chain,Res,"N",&Dnr[*dc]->D_At);
			stride_FindAtom(Chain,Res-1,"C",&Dnr[*dc]->DD_At);
		}
		else {
			Dnr[*dc]->D_At  = ERR;
			Dnr[*dc]->DD_At = ERR;
		}
		stride_FindAtom(Chain,Res,"CA",&Dnr[*dc]->DDI_At);
		stride_FindAtom(Chain,Res,"H",&Dnr[*dc]->H);
	}
	else if( Group == Trp ) {
		stride_FindAtom(Chain,Res,"NE1",&Dnr[*dc]->D_At);
		stride_FindAtom(Chain,Res,"CE2",&Dnr[*dc]->DD_At);
		stride_FindAtom(Chain,Res,"CD1",&Dnr[*dc]->DDI_At);
	}
	else if( Group == Asn ) {
		stride_FindAtom(Chain,Res,"ND1",&Dnr[*dc]->D_At);
		stride_FindAtom(Chain,Res,"CG",&Dnr[*dc]->DD_At);
		stride_FindAtom(Chain,Res,"CB",&Dnr[*dc]->DDI_At);
	}
	else if( Group == Gln ) {
		stride_FindAtom(Chain,Res,"NE2",&Dnr[*dc]->D_At);
		stride_FindAtom(Chain,Res,"CD",&Dnr[*dc]->DD_At);
		stride_FindAtom(Chain,Res,"CG",&Dnr[*dc]->DDI_At);
	}
	else if( Group == Arg ) {
		if( N == 1 ) {
			stride_FindAtom(Chain,Res,"NE",&Dnr[*dc]->D_At);
			stride_FindAtom(Chain,Res,"CZ",&Dnr[*dc]->DD_At);
			stride_FindAtom(Chain,Res,"CD",&Dnr[*dc]->DDI_At);
		}
		else
			if( N == 2 ) {
				stride_FindAtom(Chain,Res,"NH1",&Dnr[*dc]->D_At);
				stride_FindAtom(Chain,Res,"CZ",&Dnr[*dc]->DD_At);
				stride_FindAtom(Chain,Res,"NE",&Dnr[*dc]->DDI_At);
			}
			else
				if( N == 3 ) {
					stride_FindAtom(Chain,Res,"NH2",&Dnr[*dc]->D_At);
					stride_FindAtom(Chain,Res,"CZ",&Dnr[*dc]->DD_At);
					stride_FindAtom(Chain,Res,"NE",&Dnr[*dc]->DDI_At);
				}
	}
	else if( Group == His ) {
		if( N == 1 ) {
			stride_FindAtom(Chain,Res,"ND1",&Dnr[*dc]->D_At);
			stride_FindAtom(Chain,Res,"CG",&Dnr[*dc]->DD_At);
			stride_FindAtom(Chain,Res,"CE1",&Dnr[*dc]->DDI_At);
		}
		else if( N == 2 ) {
			stride_FindAtom(Chain,Res,"NE2",&Dnr[*dc]->D_At);
			stride_FindAtom(Chain,Res,"CE1",&Dnr[*dc]->DD_At);
			stride_FindAtom(Chain,Res,"CD2",&Dnr[*dc]->DDI_At);
		}
	}
	else if( Group == Tyr ) {
		stride_FindAtom(Chain,Res,"OH",&Dnr[*dc]->D_At);
		stride_FindAtom(Chain,Res,"CZ",&Dnr[*dc]->DD_At);
		stride_FindAtom(Chain,Res,"CE1",&Dnr[*dc]->DDI_At);
	}
	else if( Group == Lys ) {
		stride_FindAtom(Chain,Res,"NZ",&Dnr[*dc]->D_At);
		stride_FindAtom(Chain,Res,"CE",&Dnr[*dc]->DD_At);
	}
	else if( Group == Ser ) {
		stride_FindAtom(Chain,Res,"OG",&Dnr[*dc]->D_At);
		stride_FindAtom(Chain,Res,"CB",&Dnr[*dc]->DD_At);
	}
	else if( Group == Thr ) {
		stride_FindAtom(Chain,Res,"OG1",&Dnr[*dc]->D_At);
		stride_FindAtom(Chain,Res,"CB",&Dnr[*dc]->DD_At);
	}      

	if( Dnr[*dc]->H == ERR || Dnr[*dc]->D_At   == ERR || Dnr[*dc]->DD_At  == ERR ||
		(Dnr[*dc]->DDI_At == ERR && (Hybrid == Nsp2 || Hybrid == Osp2 )) ) {
			free(Dnr[*dc]); 
			/* Prevent memory leaks! */
			Dnr[*dc] = NULL;
			return(FAILURE);
	}
	else (*dc)++;
	return(SUCCESS);
}


int FindAcc(CHAIN *Chain, ACCEPTOR **Acc, int *NAcc, COMMAND *Cmd)
{

	int Res, ac;
	char Rsd[RES_FIELD];

	ac = *NAcc;

	for( Res=0; Res<Chain->NRes; Res++ ) {
		strcpy(Rsd,Chain->Rsd[Res]->ResType);

		DefineAcceptor(Chain,Acc,&ac,Res,Osp2,Peptide,1.60f,0);

		if( !Cmd->SideChainHBond ) continue;

		if( !strcmp(Rsd,"HIS") ) {
			DefineAcceptor(Chain,Acc,&ac,Res,Nsp2,His,1.60f,0);
			DefineAcceptor(Chain,Acc,&ac,Res,Nsp2,His,1.60f,0);
		}
		else if( !strcmp(Rsd,"SER") ) DefineAcceptor(Chain,Acc,&ac,Res,Osp3,Ser,1.70f,0);
		else if( !strcmp(Rsd,"THR") ) DefineAcceptor(Chain,Acc,&ac,Res,Osp3,Thr,1.70f,0);
		else if( !strcmp(Rsd,"ASN") ) DefineAcceptor(Chain,Acc,&ac,Res,Osp2,Asn,1.60f,0);
		else if( !strcmp(Rsd,"GLN") ) DefineAcceptor(Chain,Acc,&ac,Res,Osp2,Gln,1.60f,0);
		else if( !strcmp(Rsd,"ASP") ) {
			DefineAcceptor(Chain,Acc,&ac,Res,Osp2,Asp,1.60f,1);
			DefineAcceptor(Chain,Acc,&ac,Res,Osp2,Asp,1.60f,2);
		}
		else if( !strcmp(Rsd,"GLU") ) {
			DefineAcceptor(Chain,Acc,&ac,Res,Osp2,Glu,1.60f,1);
			DefineAcceptor(Chain,Acc,&ac,Res,Osp2,Glu,1.60f,2);
		}
		else if( !strcmp(Rsd,"TYR") ) DefineAcceptor(Chain,Acc,&ac,Res,Osp2,Tyr,1.70f,0);
		else if( !strcmp(Rsd,"MET") ) DefineAcceptor(Chain,Acc,&ac,Res,Ssp3,Met,1.95f,0);
		else if( !strcmp(Rsd,"CYS") ) DefineAcceptor(Chain,Acc,&ac,Res,Ssp3,Cys,1.70f,0);
	}

	*NAcc = ac;
	return(ac);
}


int DefineAcceptor(CHAIN *Chain, ACCEPTOR **Acc, int *ac, int Res, enum HYBRID Hybrid, 
				   enum GROUP Group, float HB_Radius, int N)
{

	/* Prevent memory leaks */
	if(Acc[*ac])
	free(Acc[*ac]);

	Acc[*ac] = (ACCEPTOR *)ckalloc(sizeof(ACCEPTOR));

	Acc[*ac]->Chain = Chain;
	Acc[*ac]->A_Res    = Res;
	Acc[*ac]->AA_Res   = Res;
	Acc[*ac]->AA2_Res   = Res;
	Acc[*ac]->Hybrid    = Hybrid;
	Acc[*ac]->Group     = Group;
	Acc[*ac]->HB_Radius = HB_Radius;

	if( Group == Peptide ) {
		if( Res != Chain->NRes-1 ) {
			stride_FindAtom(Chain,Res,"O",&Acc[*ac]->A_At);
			stride_FindAtom(Chain,Res,"C",&Acc[*ac]->AA_At);
		}
		else {
			Acc[*ac]->A_At = ERR;
			Acc[*ac]->AA_At = ERR;
		}
		stride_FindAtom(Chain,Res,"CA",&Acc[*ac]->AA2_At);
	}
	else if( Group == His ) {
		if( N == 1 ) {
			stride_FindAtom(Chain,Res,"ND1",&Acc[*ac]->A_At);
			stride_FindAtom(Chain,Res,"CG",&Acc[*ac]->AA_At);
			stride_FindAtom(Chain,Res,"CE1",&Acc[*ac]->AA2_At);
		}
		else if( N == 2 ) {
			stride_FindAtom(Chain,Res,"NE2",&Acc[*ac]->A_At);
			stride_FindAtom(Chain,Res,"CE1",&Acc[*ac]->AA_At);
			stride_FindAtom(Chain,Res,"CD2",&Acc[*ac]->AA2_At);
		}
	}
	else if( Group == Asn ) {
		stride_FindAtom(Chain,Res,"OD1",&Acc[*ac]->A_At);
		stride_FindAtom(Chain,Res,"CG",&Acc[*ac]->AA_At);
		stride_FindAtom(Chain,Res,"CB",&Acc[*ac]->AA2_At);
	}
	else if( Group == Gln ) {
		stride_FindAtom(Chain,Res,"OE1",&Acc[*ac]->A_At);
		stride_FindAtom(Chain,Res,"CD",&Acc[*ac]->AA_At);
		stride_FindAtom(Chain,Res,"CG",&Acc[*ac]->AA2_At);
	}
	else if( Group == Asp ) {
		if( N == 1 ) {
			stride_FindAtom(Chain,Res,"OD1",&Acc[*ac]->A_At);
			stride_FindAtom(Chain,Res,"CG",&Acc[*ac]->AA_At);
			stride_FindAtom(Chain,Res,"CB",&Acc[*ac]->AA2_At);
		}
		else if( N == 2 ) {
			stride_FindAtom(Chain,Res,"ND2",&Acc[*ac]->A_At);
			stride_FindAtom(Chain,Res,"CG",&Acc[*ac]->AA_At);
			stride_FindAtom(Chain,Res,"CB",&Acc[*ac]->AA2_At);
		}
	}
	else if( Group == Glu ) {
		if( N == 1 ) {
			stride_FindAtom(Chain,Res,"OE1",&Acc[*ac]->A_At);
			stride_FindAtom(Chain,Res,"CD",&Acc[*ac]->AA_At);
			stride_FindAtom(Chain,Res,"CG",&Acc[*ac]->AA2_At);
		}
		else if( N == 2 ) {
			stride_FindAtom(Chain,Res,"NE2",&Acc[*ac]->A_At);
			stride_FindAtom(Chain,Res,"CD",&Acc[*ac]->AA_At);
			stride_FindAtom(Chain,Res,"CG",&Acc[*ac]->AA2_At);
		}
	}
	else if( Group == Tyr ) {
		stride_FindAtom(Chain,Res,"OH",&Acc[*ac]->A_At);
		stride_FindAtom(Chain,Res,"CZ",&Acc[*ac]->AA_At);
		stride_FindAtom(Chain,Res,"CE1",&Acc[*ac]->AA2_At);
	}
	else if( Group == Ser ) {
		stride_FindAtom(Chain,Res,"OG",&Acc[*ac]->A_At);
		stride_FindAtom(Chain,Res,"CB",&Acc[*ac]->AA_At);
	}
	else if( Group == Thr ) {
		stride_FindAtom(Chain,Res,"OG1",&Acc[*ac]->A_At);
		stride_FindAtom(Chain,Res,"CB",&Acc[*ac]->AA_At);
	}
	else if( Group == Met ) {
		stride_FindAtom(Chain,Res,"SD",&Acc[*ac]->A_At);
		stride_FindAtom(Chain,Res,"CG",&Acc[*ac]->AA_At);
	}
	else if( Group == Cys ) {
		stride_FindAtom(Chain,Res,"SG",&Acc[*ac]->A_At);
		stride_FindAtom(Chain,Res,"CB",&Acc[*ac]->AA_At);
	}

	if( Acc[*ac]->A_At   == ERR || Acc[*ac]->AA_At  == ERR ||
		(Acc[*ac]->AA2_At == ERR && (Hybrid == Nsp2 || Hybrid == Osp2 )) ) {
			free(Acc[*ac]); 
			/* Prevent memory leaks! */
			Acc[*ac] = NULL;
			return(FAILURE); 
	}
	else (*ac)++;
	return(SUCCESS);
}


int FindHydrogenBonds(CHAIN **Chain, int NChain, HBOND **HBond, COMMAND *Cmd,
					  DONOR ***Dnr, ACCEPTOR ***Acc)
{
	STRIDE_BOOL *BondedDonor = NULL, *BondedAcceptor = NULL;
	int NDnr=0, NAcc=0;
	int dc, ac, ccd, cca, cc, hc=0, i;
	/* Make the declaration of HBOND_Energy C++ compatible */
	/* void (*HBOND_Energy)(); */
	void (*HBOND_Energy)(float *CA2, float *C, float *O, float *H, float *N, COMMAND *Cmd, 
		HBOND *HBond);
	BUFFER Text;

	/* Remove fixed memroy limitations */
	/*(*Dnr) = (DONOR **)ckalloc(MAXDONOR*sizeof(DONOR *));*/
	/*(*Acc) = (ACCEPTOR **)ckalloc(MAXACCEPTOR*sizeof(ACCEPTOR *));*/

	(*Dnr) = (DONOR **)ckalloc(MAXDONOR(Cmd->MaxLength)*sizeof(DONOR *));
	(*Acc) = (ACCEPTOR **)ckalloc(MAXACCEPTOR(Cmd->MaxLength)*sizeof(ACCEPTOR *));

	for(i = 0;i < MAXDONOR(Cmd->MaxLength);i++){
		(*Dnr)[i] = NULL;
	}

	for(i = 0;i < MAXACCEPTOR(Cmd->MaxLength);i++){
		(*Acc)[i] = NULL;
	}

	for( cc=0; cc<NChain; cc++ ) { 
		FindDnr(Chain[cc],*Dnr,&NDnr,Cmd); 
		FindAcc(Chain[cc],*Acc,&NAcc,Cmd);
	}

	BondedDonor    = (STRIDE_BOOL *)ckalloc(NDnr*sizeof(STRIDE_BOOL));
	BondedAcceptor = (STRIDE_BOOL *)ckalloc(NAcc*sizeof(STRIDE_BOOL));
	for( i=0; i<NDnr; i++ )
		BondedDonor[i] = NO;
	for( i=0; i<NAcc; i++ )
		BondedAcceptor[i] = NO;

	if( Cmd->EnergyType == 'D' ) 
		HBOND_Energy = DSSP_Energy; 
	else 
		HBOND_Energy = GRID_Energy;

	for( dc=0; dc<NDnr; dc++ ) {

		if( (*Dnr)[dc]->Group != Peptide && !Cmd->SideChainHBond ) continue;

		for( ac=0; ac<NAcc; ac++ ) {

			if( abs((*Acc)[ac]->A_Res - (*Dnr)[dc]->D_Res) < 2 && (*Acc)[ac]->Chain->Id == (*Dnr)[dc]->Chain->Id ) 
				continue;

			if( (*Acc)[ac]->Group != Peptide && !Cmd->SideChainHBond ) continue;

			/* Remove fixed memory limiations */
			/*if( hc == MAXHYDRBOND ) */
			if( hc == MAXHYDRBOND(Cmd->MaxLength) ) 
				die("Number of hydrogen bonds exceeds current limit of %d in %s\n",
				MAXHYDRBOND(Cmd->MaxLength),Chain[0]->File);
			HBond[hc] = (HBOND *)ckalloc(sizeof(HBOND));

			/* 
			* Allow the deallocation of memory later by initializing 
			* pointer to NULL.
			*/
			HBond[hc]->Dnr = NULL;
			HBond[hc]->Acc = NULL;
			HBond[hc]->ExistHydrBondRose = NO;
			HBond[hc]->ExistHydrBondBaker = NO;
			HBond[hc]->ExistPolarInter = NO;

			if( (HBond[hc]->AccDonDist = 
				Dist((*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->D_At],
				(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At]) ) <=
				Cmd->DistCutOff ) {


					if( Cmd->MainChainPolarInt && (*Dnr)[dc]->Group == Peptide && 
						(*Acc)[ac]->Group == Peptide && (*Dnr)[dc]->H != ERR) {
							HBOND_Energy((*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->AA2_Res]->Coord[(*Acc)[ac]->AA2_At],
								(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->AA_Res]->Coord[(*Acc)[ac]->AA_At],
								(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At],
								(*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->H],
								(*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->D_At],
								Cmd,HBond[hc]);

							if( HBond[hc]->Energy < -10.0 &&
								( (Cmd->EnergyType == 'G' && fabs(HBond[hc]->Et) > Eps && 
								fabs(HBond[hc]->Ep) > Eps ) || Cmd->EnergyType != 'G' ) )
								HBond[hc]->ExistPolarInter = YES;
					}

					if( Cmd->MainChainHBond && 
						(HBond[hc]->OHDist = 
						Dist((*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->H],
						(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At])) <= 2.5 &&
						(HBond[hc]->AngNHO = 
						Ang((*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->D_At],
						(*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->H],
						(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At])) >= 90.0 &&
						HBond[hc]->AngNHO <= 180.0 &&
						(HBond[hc]->AngCOH = 
						Ang((*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->AA_Res]->Coord[(*Acc)[ac]->AA_At],
						(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At],
						(*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->H])) >= 90.0 &&

						HBond[hc]->AngCOH <= 180.0 )
						HBond[hc]->ExistHydrBondBaker = YES;

					if( Cmd->MainChainHBond && 
						HBond[hc]->AccDonDist <= (*Dnr)[dc]->HB_Radius+(*Acc)[ac]->HB_Radius ) {

							HBond[hc]->AccAng = 
								Ang((*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->D_At],
								(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At],
								(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->AA_Res]->Coord[(*Acc)[ac]->AA_At]);

							if( ( ( (*Acc)[ac]->Hybrid == Nsp2 || (*Acc)[ac]->Hybrid == Osp2 ) && 
								( HBond[hc]->AccAng >= MINACCANG_SP2 && 
								HBond[hc]->AccAng <= MAXACCANG_SP2 ) ) || 
								( ( (*Acc)[ac]->Hybrid == Ssp3 ||  (*Acc)[ac]->Hybrid == Osp3 ) && 
								( HBond[hc]->AccAng >= MINACCANG_SP3 && 
								HBond[hc]->AccAng <= MAXACCANG_SP3 ) ) ) {

									HBond[hc]->DonAng = 
										Ang((*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At],
										(*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->D_At],
										(*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->DD_Res]->Coord[(*Dnr)[dc]->DD_At]);

									if( ( ( (*Dnr)[dc]->Hybrid == Nsp2 || (*Dnr)[dc]->Hybrid == Osp2 ) && 
										( HBond[hc]->DonAng >= MINDONANG_SP2 && 
										HBond[hc]->DonAng <= MAXDONANG_SP2 ) ) || 
										( ( (*Dnr)[dc]->Hybrid == Nsp3 || (*Dnr)[dc]->Hybrid == Osp3 ) && 
										( HBond[hc]->DonAng >= MINDONANG_SP3 &&
										HBond[hc]->DonAng <= MAXDONANG_SP3 ) ) ) {

											if( (*Dnr)[dc]->Hybrid == Nsp2 || (*Dnr)[dc]->Hybrid == Osp2 ) {
												HBond[hc]->AccDonAng = 
													(float)(fabs(Torsion((*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->DDI_Res]->Coord[(*Dnr)[dc]->DDI_At],
													(*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->D_At],
													(*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->DD_Res]->Coord[(*Dnr)[dc]->DD_At],
													(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At])));

												if( HBond[hc]->AccDonAng > 90.0 && HBond[hc]->AccDonAng < 270.0 )
													HBond[hc]->AccDonAng = (float)(fabs(180.0 - HBond[hc]->AccDonAng));

											}

											if( (*Acc)[ac]->Hybrid == Nsp2 || (*Acc)[ac]->Hybrid == Osp2 ) {
												HBond[hc]->DonAccAng = 
													(float)(fabs(Torsion((*Dnr)[dc]->Chain->Rsd[(*Dnr)[dc]->D_Res]->Coord[(*Dnr)[dc]->D_At],
													(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->A_Res]->Coord[(*Acc)[ac]->A_At],
													(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->AA_Res]->Coord[(*Acc)[ac]->AA_At],
													(*Acc)[ac]->Chain->Rsd[(*Acc)[ac]->AA2_Res]->Coord[(*Acc)[ac]->AA2_At])));

												if(HBond[hc]->DonAccAng > 90.0 && HBond[hc]->DonAccAng < 270.0)
													HBond[hc]->DonAccAng = (float)(fabs(180.0 - HBond[hc]->DonAccAng));

											}

											if( ( (*Dnr)[dc]->Hybrid != Nsp2 && (*Dnr)[dc]->Hybrid != Osp2 && 
												(*Acc)[ac]->Hybrid != Nsp2 && (*Acc)[ac]->Hybrid != Osp2 ) ||
												( (*Acc)[ac]->Hybrid != Nsp2 && (*Acc)[ac]->Hybrid != Osp2 &&
												( (*Dnr)[dc]->Hybrid == Nsp2 || (*Dnr)[dc]->Hybrid == Osp2 ) &&
												HBond[hc]->AccDonAng <= ACCDONANG ) ||
												( (*Dnr)[dc]->Hybrid != Nsp2 && (*Dnr)[dc]->Hybrid != Osp2 &&
												( (*Acc)[ac]->Hybrid == Nsp2 || (*Acc)[ac]->Hybrid == Osp2 ) &&
												HBond[hc]->DonAccAng <= DONACCANG ) ||
												( ( (*Dnr)[dc]->Hybrid == Nsp2 || (*Dnr)[dc]->Hybrid == Osp2 ) && 
												( (*Acc)[ac]->Hybrid == Nsp2 || (*Acc)[ac]->Hybrid == Osp2 ) &&
												HBond[hc]->AccDonAng <= ACCDONANG &&
												HBond[hc]->DonAccAng <= DONACCANG ) )
												HBond[hc]->ExistHydrBondRose = YES;
									}
							}
					}

			}

			if( (HBond[hc]->ExistPolarInter && HBond[hc]->Energy < 0.0) 
				|| HBond[hc]->ExistHydrBondRose || HBond[hc]->ExistHydrBondBaker ) {
					HBond[hc]->Dnr = (*Dnr)[dc];
					HBond[hc]->Acc = (*Acc)[ac];
					BondedDonor[dc] = YES;
					BondedAcceptor[ac] = YES;
					if( (ccd = FindChain(Chain,NChain,(*Dnr)[dc]->Chain->Id)) != ERR ) {
						if( Chain[ccd]->Rsd[(*Dnr)[dc]->D_Res]->Inv->NBondDnr < MAXRESDNR )
							Chain[ccd]->Rsd[(*Dnr)[dc]->D_Res]->Inv->
							HBondDnr[Chain[ccd]->Rsd[(*Dnr)[dc]->D_Res]->Inv->NBondDnr++] = hc;
						/*
						else
						printf("Residue %s %s of chain %s%c is is involved in more than %d hydrogen bonds (%d)\n",
						Chain[ccd]->Rsd[(*Dnr)[dc]->D_Res]->ResType,
						Chain[ccd]->Rsd[(*Dnr)[dc]->D_Res]->PDB_ResNumb,
						Chain[ccd]->File,SpaceToDash(Chain[ccd]->Id),
						MAXRESDNR,Chain[ccd]->Rsd[(*Dnr)[dc]->D_Res]->Inv->NBondDnr);
						*/
					}
					if( (cca  = FindChain(Chain,NChain,(*Acc)[ac]->Chain->Id)) != ERR ) {
						if( Chain[cca]->Rsd[(*Acc)[ac]->A_Res]->Inv->NBondAcc < MAXRESACC )
							Chain[cca]->Rsd[(*Acc)[ac]->A_Res]->Inv->
							HBondAcc[Chain[cca]->Rsd[(*Acc)[ac]->A_Res]->Inv->NBondAcc++] = hc;
						/*	  
						else
						printf("Residue %s %s of chain %s%c is is involved in more than %d hydrogen bonds (%d)\n",
						Chain[cca]->Rsd[(*Acc)[ac]->A_Res]->ResType,
						Chain[cca]->Rsd[(*Acc)[ac]->A_Res]->PDB_ResNumb,
						Chain[cca]->File,SpaceToDash(Chain[cca]->Id),
						MAXRESDNR,Chain[cca]->Rsd[(*Acc)[ac]->A_Res]->Inv->NBondAcc);
						*/
					}
					if( ccd != cca && ccd != ERR ) {
						Chain[ccd]->Rsd[(*Dnr)[dc]->D_Res]->Inv->InterchainHBonds = YES;
						Chain[cca]->Rsd[(*Acc)[ac]->A_Res]->Inv->InterchainHBonds = YES;
						if( HBond[hc]->ExistHydrBondRose ) {
							Chain[0]->NHydrBondInterchain++;
							Chain[0]->NHydrBondTotal++;
						}
					}
					else
						if( ccd == cca && ccd != ERR && HBond[hc]->ExistHydrBondRose ) {
							Chain[ccd]->NHydrBond++;
							Chain[0]->NHydrBondTotal++;
						}
						hc++;
			}
			else{
				free(HBond[hc]);

				/* Prevent memory leaks! */
				HBond[hc] = NULL;
			}
		}
	}
	if( Cmd->Info )
		for( i=0; i<hc; i++ ) { 
			if( HBond[i]->Energy < 0.0 ) {
				sprintf(Text,"%3d ",i); 
				PrintHydrBond(Text,HBond[i]); 
			}
		} 

		/*
		for( i=0; i<NDnr; i++ )
		if( !BondedDonor[i] )
		free(Dnr[i]);

		for( i=0; i<NAcc; i++ )
		if( !BondedAcceptor[i] )
		free(Acc[i]);
		*/

		if(BondedDonor)
			free(BondedDonor);

		if(BondedAcceptor)
			free(BondedAcceptor);

		return(hc);
}

void PrintHydrBond(char *Text, HBOND *HBond)
{

	fprintf(stdout,"HB %s %20s %3s %4s %4d %c <> %3s %4s %4d %c ",Text,
		HBond->Dnr->Chain->File,
		HBond->Dnr->Chain->Rsd[HBond->Dnr->D_Res]->ResType,
		HBond->Dnr->Chain->Rsd[HBond->Dnr->D_Res]->PDB_ResNumb,
		HBond->Dnr->D_Res,HBond->Dnr->Chain->Id,
		HBond->Acc->Chain->Rsd[HBond->Acc->A_Res]->ResType,
		HBond->Acc->Chain->Rsd[HBond->Acc->A_Res]->PDB_ResNumb,
		HBond->Acc->A_Res,HBond->Acc->Chain->Id);

	fprintf(stdout," %7.1f ",HBond->AccDonDist);
	if( HBond->ExistPolarInter ) 
		fprintf(stdout,"%7.1f ",HBond->Energy);
	else
		fprintf(stdout,"XXXXXXX ");

	if( HBond->ExistHydrBondRose ) 
		fprintf(stdout,"YES ");
	else
		fprintf(stdout,"NO ");

	if( HBond->ExistHydrBondBaker ) 
		fprintf(stdout,"YES\n");
	else
		fprintf(stdout,"NO\n");



}

int FindPolInt(HBOND **HBond, RESIDUE *Res1, RESIDUE *Res2)
{

	register int i, j, hb;
	INVOLVED *p1, *p2;

	p1 = Res1->Inv;
	p2 = Res2->Inv;

	if( p1->NBondDnr && p2->NBondAcc ) {
		for( i=0; i<p1->NBondDnr; i++ ) {
			hb = p1->HBondDnr[i];
			for( j=0; j<p2->NBondAcc; j++ )
				if( hb == p2->HBondAcc[j] && HBond[hb]->ExistPolarInter ) 
					return(hb);
		}
	}

	return(ERR);
}

int FindBnd(HBOND **HBond, RESIDUE *Res1, RESIDUE *Res2)
{

	register int i, j, hb;
	INVOLVED *p1, *p2;

	p1 = Res1->Inv;
	p2 = Res2->Inv;

	if( p1->NBondDnr && p2->NBondAcc ) {
		for( i=0; i<p1->NBondDnr; i++ ) {
			hb = p1->HBondDnr[i];
			for( j=0; j<p2->NBondAcc; j++ )
				if( hb == p2->HBondAcc[j] && HBond[hb]->ExistHydrBondRose ) 
					return(hb);
		}
	}

	return(ERR);
}

int NoDoubleHBond(HBOND **HBond, int NHBond)
{

	int i, j, NExcl=0;

	for( i=0; i<NHBond-1; i++ )
		for( j=i+1; j<NHBond; j++ ) 
			if( HBond[i]->Dnr->D_Res == HBond[j]->Dnr->D_Res &&
				HBond[i]->Dnr->Chain->Id == HBond[j]->Dnr->Chain->Id &&
				HBond[i]->ExistPolarInter && HBond[j]->ExistPolarInter ) {
					if( HBond[i]->Energy < 5.0*HBond[j]->Energy ) {
						HBond[j]->ExistPolarInter = NO;
						NExcl++;
					}
					else 
						if( HBond[j]->Energy < 5.0*HBond[i]->Energy ) {
							HBond[i]->ExistPolarInter = NO;
							NExcl++;
						}
			}

			return(NExcl);
}

