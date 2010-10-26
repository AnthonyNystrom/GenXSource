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

void FillAsnAntiPar(char *Asn1, char *Asn2, CHAIN **Chain, int Cn1, int Cn2, 
		    STRIDE_PATTERN **Pat, int NPat, COMMAND *Cmd)
{
  register int i, j;
  int Beg1, Beg2, End1, End2;
  int B1D, B1A, B2D, B2A, E1D, E1A, E2D, E2A;
  char B1DCn, B1ACn, B2DCn, B2ACn, E1DCn, E1ACn, E2DCn, E2ACn, Beg1Cn, Beg2Cn; 
  STRIDE_PATTERN *CurrPat, *PrevPat;;

  for( i=0; i<NPat; i++ ) {

    if( Pat[i]->Nei1 != NULL && Pat[i]->Nei2 == NULL )
      CurrPat = Pat[i]->Nei1;
    else
    if( Pat[i]->Nei2 != NULL && Pat[i]->Nei1 == NULL ) 
      CurrPat = Pat[i]->Nei2;
    else 
      continue;
    
    if( Cmd->Info ) {
      fprintf(stdout,"From: %c %c ",
	      Pat[i]->Hb1->Dnr->Chain->Id,Pat[i]->Hb2->Dnr->Chain->Id);
      if( Pat[i]->Hb1->Dnr->Chain->Id == Chain[Cn1]->Id )
	fprintf(stdout,"%s %s %s %s \n",
	    Chain[Cn1]->Rsd[Pat[i]->Hb1->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[Pat[i]->Hb1->Acc->A_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[Pat[i]->Hb2->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[Pat[i]->Hb2->Acc->A_Res]->PDB_ResNumb);
      else
	fprintf(stdout,"%s %s %s %s \n",
	    Chain[Cn2]->Rsd[Pat[i]->Hb1->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[Pat[i]->Hb1->Acc->A_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[Pat[i]->Hb2->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[Pat[i]->Hb2->Acc->A_Res]->PDB_ResNumb);
    }

    PrevPat = Pat[i];
    while( CurrPat->Nei1 != NULL && CurrPat->Nei2 != NULL ) {
      
      if( (CurrPat->Nei1->Nei1 == CurrPat || CurrPat->Nei1->Nei2 == CurrPat) && 
	 CurrPat->Nei1 != PrevPat ) {
	PrevPat = CurrPat;
	CurrPat = CurrPat->Nei1;
      }
      else 
      if( (CurrPat->Nei2->Nei1 == CurrPat || CurrPat->Nei2->Nei2 == CurrPat) && 
	 CurrPat->Nei2 != PrevPat ) {
	PrevPat = CurrPat;
	CurrPat = CurrPat->Nei2;
      }
      else {
	fprintf(stdout,"Cycle Anti%s%c i = %d \n",Chain[Cn1]->File,Chain[Cn1]->Id,i);
	break;
      }
    }  
    
    if( Cmd->Info ) {
      fprintf(stdout,"To: %c %c ",
	      CurrPat->Hb1->Dnr->Chain->Id,CurrPat->Hb2->Dnr->Chain->Id);
      if( CurrPat->Hb1->Dnr->Chain->Id == Chain[Cn1]->Id )
	fprintf(stdout,"%s %s %s %s \n",
	    Chain[Cn1]->Rsd[CurrPat->Hb1->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[CurrPat->Hb1->Acc->A_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[CurrPat->Hb2->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[CurrPat->Hb2->Acc->A_Res]->PDB_ResNumb);
      else
	fprintf(stdout,"%s %s %s %s \n",
	    Chain[Cn2]->Rsd[CurrPat->Hb1->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[CurrPat->Hb1->Acc->A_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[CurrPat->Hb2->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[CurrPat->Hb2->Acc->A_Res]->PDB_ResNumb);
    }

    Alias(&B1D,&B1A,&B2D,&B2A,&B1DCn,&B1ACn,&B2DCn,&B2ACn,Pat[i]);
    Alias(&E1D,&E1A,&E2D,&E2A,&E1DCn,&E1ACn,&E2DCn,&E2ACn,CurrPat);

    if( (Cn1 != Cn2 || E1D - B2A <  E2D - B2A ) &&
        ( MakeEnds(&Beg1,B1D,B2A,&Beg1Cn,B1DCn,&End1,E2A,E1D,E2ACn,&Beg2,E2D,E1A,&Beg2Cn,E2DCn,
		   &End2,B1A,B2D,B1ACn,Pat,NPat) ||
          MakeEnds(&Beg1,B1D,B2A,&Beg1Cn,B1DCn,&End1,E1D,E2A,E1DCn,&Beg2,E1A,E2D,&Beg2Cn,E1ACn,
		   &End2,B1A,B2D,B1ACn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || E2D - B2A <  E1D - B2A ) && 
        ( MakeEnds(&Beg1,B1D,B2A,&Beg1Cn,B1DCn,&End1,E1A,E2D,E1ACn,&Beg2,E1D,E2A,&Beg2Cn,E1DCn,
		   &End2,B1A,B2D,B1ACn,Pat,NPat) ||
          MakeEnds(&Beg1,B1D,B2A,&Beg1Cn,B1DCn,&End1,E2D,E1A,E2DCn,&Beg2,E2A,E1D,&Beg2Cn,E2ACn,
		   &End2,B1A,B2D,B1ACn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || B2A - E1D < B2A - E2D ) && 
        ( MakeEnds(&Beg1,B1A,B2D,&Beg1Cn,B1ACn,&End1,E2D,E1A,E2DCn,&Beg2,E2A,E1D,&Beg2Cn,E2ACn,
		   &End2,B1D,B2A,B1DCn,Pat,NPat) ||
          MakeEnds(&Beg1,B1A,B2D,&Beg1Cn,B1ACn,&End1,E1A,E2D,E1ACn,&Beg2,E1D,E2A,&Beg2Cn,E1DCn,
		   &End2,B1D,B2A,B1DCn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || B2A - E2D < B2A - E1D ) && 
        ( MakeEnds(&Beg1,B1A,B2D,&Beg1Cn,B1ACn,&End1,E1D,E2A,E1DCn,&Beg2,E1A,E2D,&Beg2Cn,E1ACn,
		   &End2,B1D,B2A,B1DCn,Pat,NPat) ||
          MakeEnds(&Beg1,B1A,B2D,&Beg1Cn,B1ACn,&End1,E2A,E1D,E2ACn,&Beg2,E2D,E1A,&Beg2Cn,E2DCn,
		   &End2,B1D,B2A,B1DCn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || B1D - E2A <  B2D - E2A ) && 
        ( MakeEnds(&Beg1,E1D,E2A,&Beg1Cn,E1DCn,&End1,B2A,B1D,B2ACn,&Beg2,B2D,B1A,&Beg2Cn,B2DCn,
		   &End2,E1A,E2D,E1ACn,Pat,NPat) ||
          MakeEnds(&Beg1,E1D,E2A,&Beg1Cn,E1DCn,&End1,B1D,B2A,B1DCn,&Beg2,B1A,B2D,&Beg2Cn,B1ACn,
		   &End2,E1A,E2D,E1ACn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || B2D - E2A <  B1D - E2A ) && 
        ( MakeEnds(&Beg1,E1D,E2A,&Beg1Cn,E1DCn,&End1,B1A,B2D,B1ACn,&Beg2,B1D,B2A,&Beg2Cn,B1DCn,
		   &End2,E1A,E2D,E1ACn,Pat,NPat) ||
          MakeEnds(&Beg1,E1D,E2A,&Beg1Cn,E1DCn,&End1,B2D,B1A,B2DCn,&Beg2,B2A,B1D,&Beg2Cn,B2ACn,
		   &End2,E1A,E2D,E1ACn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || E2A - B1D < E2A - B2D ) && 
        ( MakeEnds(&Beg1,E1A,E2D,&Beg1Cn,E1ACn,&End1,B2D,B1A,B2DCn,&Beg2,B2A,B1D,&Beg2Cn,B2ACn,
		   &End2,E1D,E2A,E1DCn,Pat,NPat) ||
          MakeEnds(&Beg1,E1A,E2D,&Beg1Cn,E1ACn,&End1,B1A,B2D,B1ACn,&Beg2,B1D,B2A,&Beg2Cn,B1DCn,
		   &End2,E1D,E2A,E1DCn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || E2A - B2D < E2A - B1D ) && 
        ( MakeEnds(&Beg1,E1A,E2D,&Beg1Cn,E1ACn,&End1,B1D,B2A,B1DCn,&Beg2,B1A,B2D,&Beg2Cn,B1ACn,
		   &End2,E1D,E2A,E1DCn,Pat,NPat) ||
          MakeEnds(&Beg1,E1A,E2D,&Beg1Cn,E1ACn,&End1,B2A,B1D,B2ACn,&Beg2,B2D,B1A,&Beg2Cn,B2DCn,
		   &End2,E1D,E2A,E1DCn,Pat,NPat) ) )
      ;
    else {
      fprintf(stdout,"Ne tot variant.. Anti.. %s%c\n",Chain[Cn1]->File,Chain[Cn1]->Id);
      continue;
    }


    if( Beg1Cn == Chain[Cn1]->Id ) {
      for( j=Beg1; j<=End1; j++ ) 
	Asn1[j] = 'N';
      for( j=Beg2; j<=End2; j++ ) 
	Asn2[j] = 'N';
    }
    else {
      for( j=Beg1; j<=End1; j++ ) 
	Asn2[j] = 'N';
      for( j=Beg2; j<=End2; j++ ) 
	Asn1[j] = 'N';
    }

    Pat[i]->Nei1 = NULL;
    Pat[i]->Nei2 = NULL;
    CurrPat->Nei1 = NULL;
    CurrPat->Nei2 = NULL;

  }
}
  

void FillAsnPar(char *Asn1, char *Asn2, CHAIN **Chain, int Cn1, int Cn2, 
		STRIDE_PATTERN **Pat, int NPat, COMMAND *Cmd)
{
  register int i, j;
  int Beg1, Beg2, End1, End2;
  int B1D, B1A, B2D, B2A, E1D, E1A, E2D, E2A; 
  char B1DCn, B1ACn, B2DCn, B2ACn, E1DCn, E1ACn, E2DCn, E2ACn, Beg1Cn, Beg2Cn; 
  STRIDE_PATTERN *CurrPat, *PrevPat;;

  for( i=0; i<NPat; i++ ) {
    
    if( Pat[i]->Nei1 != NULL && Pat[i]->Nei2 == NULL )
      CurrPat = Pat[i]->Nei1;
    else
    if( Pat[i]->Nei2 != NULL && Pat[i]->Nei1 == NULL ) 
      CurrPat = Pat[i]->Nei2;
    else 
      continue;
    
    if( Cmd->Info ) {
      fprintf(stdout,"From: %c %c ",
	      Pat[i]->Hb1->Dnr->Chain->Id,Pat[i]->Hb2->Dnr->Chain->Id);
      if( Pat[i]->Hb1->Dnr->Chain->Id == Chain[Cn1]->Id )
	fprintf(stdout,"%s %s %s %s \n",
	    Chain[Cn1]->Rsd[Pat[i]->Hb1->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[Pat[i]->Hb1->Acc->A_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[Pat[i]->Hb2->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[Pat[i]->Hb2->Acc->A_Res]->PDB_ResNumb);
      else
	fprintf(stdout,"%s %s %s %s \n",
	    Chain[Cn2]->Rsd[Pat[i]->Hb1->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[Pat[i]->Hb1->Acc->A_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[Pat[i]->Hb2->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[Pat[i]->Hb2->Acc->A_Res]->PDB_ResNumb);
    }

    PrevPat = Pat[i];
    while( CurrPat->Nei1 != NULL && CurrPat->Nei2 != NULL ) {
      
      if( (CurrPat->Nei1->Nei1 == CurrPat || CurrPat->Nei1->Nei2 == CurrPat) && 
	 CurrPat->Nei1 != PrevPat ) {
	PrevPat = CurrPat;
	CurrPat = CurrPat->Nei1;
      }
      else {
	PrevPat = CurrPat;
	CurrPat = CurrPat->Nei2;
      }
    }  

    if( Cmd->Info ) {
      fprintf(stdout,"To: %c %c ",
	      CurrPat->Hb1->Dnr->Chain->Id,CurrPat->Hb2->Dnr->Chain->Id);
      if( CurrPat->Hb1->Dnr->Chain->Id == Chain[Cn1]->Id )
	fprintf(stdout,"%s %s %s %s \n",
	    Chain[Cn1]->Rsd[CurrPat->Hb1->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[CurrPat->Hb1->Acc->A_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[CurrPat->Hb2->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[CurrPat->Hb2->Acc->A_Res]->PDB_ResNumb);
      else
	fprintf(stdout,"%s %s %s %s \n",
	    Chain[Cn2]->Rsd[CurrPat->Hb1->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[CurrPat->Hb1->Acc->A_Res]->PDB_ResNumb,
	    Chain[Cn1]->Rsd[CurrPat->Hb2->Dnr->D_Res]->PDB_ResNumb,
	    Chain[Cn2]->Rsd[CurrPat->Hb2->Acc->A_Res]->PDB_ResNumb);
    }

    Alias(&B1D,&B1A,&B2D,&B2A,&B1DCn,&B1ACn,&B2DCn,&B2ACn,Pat[i]);
    Alias(&E1D,&E1A,&E2D,&E2A,&E1DCn,&E1ACn,&E2DCn,&E2ACn,CurrPat);

    if( ( Cn1 != Cn2 || abs(E1D-B2A) < abs(E2D-B2A) ) && 
        ( MakeEnds(&Beg1,B1D,B2A,&Beg1Cn,B1DCn,&End1,E2A,E1D,E2ACn,&Beg2,B1A,B2D,&Beg2Cn,B1ACn,
		   &End2,E2D,E1A,E2DCn,Pat,NPat) ||
          MakeEnds(&Beg1,B1D,B2A,&Beg1Cn,B1DCn,&End1,E1D,E2A,E1DCn,&Beg2,B1A,B2D,&Beg2Cn,B1ACn,
		   &End2,E1A,E2D,E1ACn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || abs(E2D-B2A) < abs(E1D-B2A) ) && 
        ( MakeEnds(&Beg1,B1D,B2A,&Beg1Cn,B1DCn,&End1,E1A,E2D,E1ACn,&Beg2,B1A,B2D,&Beg2Cn,B1ACn,
		   &End2,E1D,E2A,E1DCn,Pat,NPat) ||
          MakeEnds(&Beg1,B1D,B2A,&Beg1Cn,B1DCn,&End1,E2D,E1A,E2DCn,&Beg2,B1A,B2D,&Beg2Cn,B1ACn,
		   &End2,E2A,E1D,E2ACn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || abs(B2A-E1D) < abs(B2A-E2D) ) && 
        ( MakeEnds(&Beg1,B1A,B2D,&Beg1Cn,B1ACn,&End1,E2D,E1A,E2DCn,&Beg2,B1D,B2A,&Beg2Cn,B1DCn,
		   &End2,E2A,E1D,E2ACn,Pat,NPat) ||
          MakeEnds(&Beg1,B1A,B2D,&Beg1Cn,B1ACn,&End1,E1A,E2D,E1ACn,&Beg2,B1D,B2A,&Beg2Cn,B1DCn,
		   &End2,E1D,E2A,E1DCn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || abs(B2A-E2D) < abs(B2A-E1D) ) && 
        ( MakeEnds(&Beg1,B1A,B2D,&Beg1Cn,B1ACn,&End1,E1D,E2A,E1DCn,&Beg2,B1D,B2A,&Beg2Cn,B1DCn,
		   &End2,E1A,E2D,E1ACn,Pat,NPat) ||
          MakeEnds(&Beg1,B1A,B2D,&Beg1Cn,B1ACn,&End1,E2A,E1D,E2ACn,&Beg2,B1D,B2A,&Beg2Cn,B1DCn,
		   &End2,E2D,E1A,E2DCn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || abs(B1D-E2A) < abs(B2D-E2A) ) && 
        ( MakeEnds(&Beg1,E1D,E2A,&Beg1Cn,E1DCn,&End1,B2A,B1D,B2ACn,&Beg2,E1A,E2D,&Beg2Cn,E1ACn,
		   &End2,B2D,B1A,B2DCn,Pat,NPat) ||
          MakeEnds(&Beg1,E1D,E2A,&Beg1Cn,E1DCn,&End1,B1D,B2A,B1DCn,&Beg2,E1A,E2D,&Beg2Cn,E1ACn,
		   &End2,B1A,B2D,B1ACn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || abs(B2D-E2A) < abs(B1D-E2A) ) && 
        ( MakeEnds(&Beg1,E1D,E2A,&Beg1Cn,E1DCn,&End1,B1A,B2D,B1ACn,&Beg2,E1A,E2D,&Beg2Cn,E1ACn,
		   &End2,B1D,B2A,B1DCn,Pat,NPat) ||
          MakeEnds(&Beg1,E1D,E2A,&Beg1Cn,E1DCn,&End1,B2D,B1A,B2DCn,&Beg2,E1A,E2D,&Beg2Cn,E1ACn,
		   &End2,B2A,B1D,B2ACn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || abs(E2A-B1D) < abs(E2A-B2D) ) && 
        ( MakeEnds(&Beg1,E1A,E2D,&Beg1Cn,E1ACn,&End1,B2D,B1A,B2DCn,&Beg2,E1D,E2A,&Beg2Cn,E1DCn,
		   &End2,B2A,B1D,B2ACn,Pat,NPat) ||
          MakeEnds(&Beg1,E1A,E2D,&Beg1Cn,E1ACn,&End1,B1A,B2D,B1ACn,&Beg2,E1D,E2A,&Beg2Cn,E1DCn,
		   &End2,B1D,B2A,B1DCn,Pat,NPat) ) )
      ;
    else
    if( ( Cn1 != Cn2 || abs(E2A-B2D) < abs(E2A-B1D) ) && 
        ( MakeEnds(&Beg1,E1A,E2D,&Beg1Cn,E1ACn,&End1,B1D,B2A,B1DCn,&Beg2,E1D,E2A,&Beg2Cn,E1DCn,
		   &End2,B1A,B2D,B1ACn,Pat,NPat) ||
          MakeEnds(&Beg1,E1A,E2D,&Beg1Cn,E1ACn,&End1,B2A,B1D,B2ACn,&Beg2,E1D,E2A,&Beg2Cn,E1DCn,
		   &End2,B2D,B1A,B2DCn,Pat,NPat) ) )
      ;
    else {
      fprintf(stdout,"Ne tot variant.. Par %s%c\n",Chain[Cn1]->File,Chain[Cn1]->Id);
      continue;
    }

    if( Beg1Cn == Chain[Cn1]->Id ) {
      for( j=Beg1; j<=End1; j++ ) Asn1[j] = 'P';
      for( j=Beg2; j<=End2; j++ ) Asn2[j] = 'P';
    }
    else {
      for( j=Beg1; j<=End1; j++ ) Asn2[j] = 'P';
      for( j=Beg2; j<=End2; j++ ) Asn1[j] = 'P';
    }

    Pat[i]->Nei1 = NULL;
    Pat[i]->Nei2 = NULL;
    CurrPat->Nei1 = NULL;
    CurrPat->Nei2 = NULL;

  }
}
  

int MakeEnds(int *Beg1, int ResBeg1, int NeiBeg1, char *Beg1Cn, char ResBeg1Cn, int *End1, 
	     int ResEnd1, int NeiEnd1, char ResEnd1Cn, int *Beg2, int ResBeg2, int NeiBeg2, 
	     char *Beg2Cn, char ResBeg2Cn, int *End2, int ResEnd2, int NeiEnd2, 
	     char ResEnd2Cn, STRIDE_PATTERN **Pat, int NPat)
{

  register int i;
  int Flag1 = 0, Flag2 = 0;


  if( ResBeg1 <= NeiBeg1 && NeiBeg1 <= NeiEnd1 && NeiEnd1 <= ResEnd1 &&
      ResBeg2 <= NeiBeg2 && NeiBeg2 <= NeiEnd2 && NeiEnd2 <= ResEnd2 &&
      ResBeg1Cn == ResEnd1Cn && ResBeg2Cn == ResEnd2Cn ) {

    *Beg1 = ResBeg1;
    *End1 = ResEnd1;
    *Beg2 = ResBeg2;
    *End2 = ResEnd2;
    *Beg1Cn = ResBeg1Cn;
    *Beg2Cn = ResBeg2Cn;
    
    for( i=0; i<NPat && (Flag1 == 0 || Flag2 == 0); i++ ) {
      if( ( (Pat[i]->Hb1->Dnr->D_Res == (*Beg1) 
	     && Pat[i]->Hb1->Acc->A_Res == (*End2)
	     && Pat[i]->Hb1->Dnr->Chain->Id == (*Beg1Cn)
	     && Pat[i]->Hb1->Acc->Chain->Id == (*Beg2Cn) ) 
	   ||
	   (Pat[i]->Hb1->Acc->A_Res == (*Beg1) 
	    && Pat[i]->Hb1->Dnr->D_Res == (*End2) 
	    && Pat[i]->Hb1->Acc->Chain->Id == (*Beg1Cn)
	    && Pat[i]->Hb1->Dnr->Chain->Id == (*Beg2Cn) ) ) 
	 && Pat[i]->Hb1->Dnr->D_Res == Pat[i]->Hb2->Acc->A_Res 
	 && Pat[i]->Hb2->Dnr->D_Res == Pat[i]->Hb1->Acc->A_Res )
	Flag1 = 1; 
      if( ( (Pat[i]->Hb1->Dnr->D_Res == (*Beg2) 
	     && Pat[i]->Hb1->Acc->A_Res == (*End1) 
	     && Pat[i]->Hb1->Dnr->Chain->Id == (*Beg2Cn)
	     && Pat[i]->Hb1->Acc->Chain->Id == (*Beg1Cn) ) 
	   ||
	   (Pat[i]->Hb1->Acc->A_Res == (*Beg2) 
	    && Pat[i]->Hb1->Dnr->D_Res == (*End1) 
	    && Pat[i]->Hb1->Acc->Chain->Id == (*Beg2Cn)
	    && Pat[i]->Hb1->Dnr->Chain->Id == (*Beg1Cn) ) ) 
	 && Pat[i]->Hb1->Dnr->D_Res == Pat[i]->Hb2->Acc->A_Res 
	 && Pat[i]->Hb2->Dnr->D_Res == Pat[i]->Hb1->Acc->A_Res )
	Flag2 = 1; 
    }
    
    if( !Flag1 ) {
      if( *Beg1 != NeiBeg1 ) (*Beg1)++;
      if( *End2 != NeiEnd2 ) (*End2)--;
    }
    
    if( !Flag2 ) {
      if( *End1 != NeiEnd1 ) (*End1)--;
      if( *Beg2 != NeiBeg2 ) (*Beg2)++;
    }
    return(SUCCESS);
  }
  
  return(FAILURE);
}
  
  
void FilterAntiPar(STRIDE_PATTERN **Pat, int NPat)
{
 
  register int i, j;
  int I1A, I1D, I2A, I2D, J1A, J1D, J2A, J2D;
  char I1ACn, I1DCn, I2ACn, I2DCn, J1ACn, J1DCn, J2ACn, J2DCn;

  for( i=0; i<NPat; i++ ) {
    
    if( !Pat[i]->ExistPattern ) continue;

    Alias(&I1D,&I1A,&I2D,&I2A,&I1DCn,&I1ACn,&I2DCn,&I2ACn,Pat[i]);
    
    for( j=0; j<NPat; j++ ) {
      
      if( j == i || !Pat[j]->ExistPattern ) continue;
      
      Alias(&J1D,&J1A,&J2D,&J2A,&J1DCn,&J1ACn,&J2DCn,&J2ACn,Pat[j]);
      
      if( J1D == J2A && J2D == J1A && I1D != I2A && I2D != I1A &&
	 ( (J1D == I1D && J1A == I1A) ||  (J1D == I1A && J1A == I1D) || 
	   (J1D == I2A && J1A == I2D) ||  (J1D == I2D && J1A == I2A) ) ) continue;

      if( ( ( I1D < I2A || I2D < I1A ) && 
	   ( (J1A <= I2A && J1A >= I1D && J2D <= I2A && J2D >= I1D && J2DCn == I1DCn &&
	      J2A <= I1A && J2A >= I2D && J1D <= I1A && J1D >= I2D && J1DCn == I2DCn) ||
	     (J2A <= I2A && J2A >= I1D && J1D <= I2A && J1D >= I1D && J1DCn == I1DCn &&
	      J1A <= I1A && J1A >= I2D && J2D <= I1A && J2D >= I2D && J2DCn == I2DCn) ) ) || 
	  ( ( I1D > I2A || I2D > I1A ) && 
	   ( (J1A >= I2A && J1A <= I1D && J2D >= I2A && J2D <= I1D && J2DCn == I1DCn &&
	      J2A >= I1A && J2A <= I2D && J1D >= I1A && J1D <= I2D && J1DCn == I2DCn) ||
	     (J2A >= I2A && J2A <= I1D && J1D >= I2A && J1D <= I1D && J1DCn == I1DCn &&
	      J1A >= I1A && J1A <= I2D && J2D >= I1A && J2D <= I2D && J2DCn == I2DCn) ) ) ) {
	Pat[j]->ExistPattern = NO;
      }
    }
  }
}

void FilterPar(STRIDE_PATTERN **Pat, int NPat)
{
 
  register int i, j;
  int I1A, I1D, I2A, I2D, J1A, J1D, J2A, J2D;
  char I1ACn, I1DCn, I2ACn, I2DCn, J1ACn, J1DCn, J2ACn, J2DCn;

  for( i=0; i<NPat; i++ ) {
    
    if( !Pat[i]->ExistPattern ) continue;

    Alias(&I1D,&I1A,&I2D,&I2A,&I1DCn,&I1ACn,&I2DCn,&I2ACn,Pat[i]);
    
    for( j=0; j<NPat; j++ ) {
      
      if( j == i || !Pat[j]->ExistPattern ) continue;
      
      Alias(&J1D,&J1A,&J2D,&J2A,&J1DCn,&J1ACn,&J2DCn,&J2ACn,Pat[j]);
      
      if( ( ( I1A >= I2D && I1D >= I2A ) && 
	   ( (J1A >= I2A && J1A <= I1D && J2D >= I2A && J2D <= I1D && J2DCn == I1DCn &&
	      J2A <= I1A && J2A >= I2D && J1D <= I1A && J1D >= I2D && J1DCn == I2DCn) ||
	     (J2A >= I2A && J2A <= I1D && J1D >= I2A && J1D <= I1D && J1DCn == I1DCn &&
	      J1A <= I1A && J1A >= I2D && J2D <= I1A && J2D >= I2D && J2DCn == I2DCn) ) ) || 

	  ( I2A >= I1D && I2D >= I1A  && 
	   ( (J1A <= I2A && J1A >= I1D && J2D <= I2A && J2D >= I1D && J2DCn == I1DCn &&
	      J2A >= I1A && J2A <= I2D && J1D >= I1A && J1D <= I2D && J1DCn == I2DCn) ||

	     (J2A <= I2A && J2A >= I1D && J1D <= I2A && J1D >= I1D && J1DCn == I1DCn &&
	      J1A >= I1A && J1A <= I2D && J2D >= I1A && J2D <= I2D && J2DCn == I2DCn) ) ) ) {
	Pat[j]->ExistPattern = NO;
      }
    }
  }
}
      
void Alias(int *D1,int *A1,int *D2,int *A2,char *D1Cn,char *A1Cn,char *D2Cn,char *A2Cn,
	  STRIDE_PATTERN *Pat)
{
    *D1 = Pat->Hb1->Dnr->D_Res;
    *A1 = Pat->Hb1->Acc->A_Res; 
    *D2 = Pat->Hb2->Dnr->D_Res;
    *A2 = Pat->Hb2->Acc->A_Res;
    *D1Cn = Pat->Hb1->Dnr->Chain->Id;
    *A1Cn = Pat->Hb1->Acc->Chain->Id;
    *D2Cn = Pat->Hb2->Dnr->Chain->Id;
    *A2Cn = Pat->Hb2->Acc->Chain->Id;
}		      

