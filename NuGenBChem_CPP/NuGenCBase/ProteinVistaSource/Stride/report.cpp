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

void Report(CHAIN **Chain, int NChain, HBOND **HBond, COMMAND *Cmd) 
{

  FILE *Out;

  if( !strlen(Cmd->OutFile) )
    Out = stdout;
  else 
  if( !(Out = fopen(Cmd->OutFile,"w")) )
    die("Can not open output file %s\n",Cmd->OutFile);
  
  if( !Cmd->ReportSummaryOnly )
    ReportGeneral(Chain,Out);

  ReportSummary(Chain,NChain,Out,Cmd);
  ReportShort(Chain,NChain,Out,Cmd);
  ReportTurnTypes(Chain,NChain,Out,Cmd);
  ReportSSBonds(Chain,Out);

  if( !Cmd->ReportSummaryOnly )
    ReportDetailed(Chain,NChain,Out,Cmd);

  if( Cmd->ReportBonds )
    ReportHydrBonds(Chain,NChain,HBond,Out,Cmd);


  if( Cmd->Measure ) {
    //	Measure(Chain,NChain,0,Cmd,Out);
    //	Measure(Chain,NChain,1,Cmd,Out);
  }

  if( Out != stdout )
    fclose(Out);

}


void ReportGeneral(CHAIN **Chain, FILE *Out)
{
  register int i;
  BUFFER Bf, Tmp;
  
  if( !Chain[0]->NInfo )
    return;

  PrepareBuffer(Bf,Chain);

  Glue(Bf,"REM  --------------------------------------------------------------------",Out);
  Glue(Bf,"REM",Out);
  Glue(Bf,"REM  STRIDE: Knowledge-based secondary structure assignment",Out);
  Glue(Bf,"REM  Please cite: D.Frishman & P.Argos, Proteins XX, XXX-XXX, 1995",Out);
  Glue(Bf,"REM",Out);
  Glue(Bf,"REM  Residue accessible surface area calculation",Out);
  Glue(Bf,"REM  Please cite: F.Eisenhaber & P.Argos, J.Comp.Chem. 14, 1272-1280, 1993 ",Out);
  Glue(Bf,"REM               F.Eisenhaber et al., J.Comp.Chem., 1994, submitted",Out);
  Glue(Bf,"REM",Out);

  sprintf(Tmp,"REM  ------------------------ ");
  strcat(Tmp,"General information");
  strcat(Tmp," -----------------------"); 
  Glue(Bf,Tmp,Out);
  Glue(Bf,"REM",Out);

  for( i=0; i<Chain[0]->NInfo; i++ ) {
    strcpy(Tmp,Chain[0]->Info[i]);
    Tmp[66] = '\0';
    Replace(Tmp,'\n',' ');
    Glue(Bf,Tmp,Out);
  }
  Glue(Bf,"REM",Out);
}

void ReportSummary(CHAIN **Chain, int NChain, FILE *Out, COMMAND *Cmd)
{
  int Cn, Width, CurrWidth, NBlocks, Tail, i, j, From, To;
  BUFFER Bf, Tmp, Tmp1;

  PrepareBuffer(Bf,Chain);

  sprintf(Tmp,"REM  -------------------- ");
  strcat(Tmp,"Secondary structure summary");
  strcat(Tmp," -------------------");
  Glue(Bf,Tmp,Out);

  for( Cn=0; Cn<NChain; Cn++ ) {

    if( !Chain[Cn]->Valid )
      continue;

    Width = 50;

    Glue(Bf,"REM",Out);

    strncpy(Tmp1,Chain[Cn]->File,40);
    Tmp1[40] = '\0';
    sprintf(Tmp,"CHN  %s %c",Tmp1,SpaceToDash(Chain[Cn]->Id));
    Glue(Bf,Tmp,Out);

    NBlocks = Chain[Cn]->NRes/Width;
    Tail = Chain[Cn]->NRes % Width;
    if( Tail ) NBlocks++;
    
    for( i=0; i<NBlocks; i++ ) {

      Glue(Bf,"REM",Out);
      From = i*Width;
      if( i == NBlocks-1 && Tail ) 
	CurrWidth = Tail;
      else
	CurrWidth = Width;
      To   = From+CurrWidth;

      sprintf(Tmp,"REM       ");
      for( j=0; j<CurrWidth; j++ )
	if( j && (j+1)%10 == 0 )
	  strcat(Tmp,".");
	else
	  strcat(Tmp," ");
      Glue(Bf,Tmp,Out);

      sprintf(Tmp,"SEQ  %-4d ",From+1);
      for( j=From; j<From+Width; j++ ) {
	if( j < To )
	  sprintf(Tmp1,"%c",ThreeToOne(Chain[Cn]->Rsd[j]->ResType));
	else
	  sprintf(Tmp1," ");
	strcat(Tmp,Tmp1);
      }
      sprintf(Tmp1," %4d",To);
      strcat(Tmp,Tmp1);
      Glue(Bf,Tmp,Out);

      sprintf(Tmp,"STR       ");
      for( j=From; j<To; j++ ) {
	if( Chain[Cn]->Rsd[j]->Prop->Asn == 'C' )
	  sprintf(Tmp1," ");
	else
	  sprintf(Tmp1,"%c",Chain[Cn]->Rsd[j]->Prop->Asn);
	strcat(Tmp,Tmp1);
      }
      strcat(Tmp,"     ");
      Glue(Bf,Tmp,Out);
    }

  }
}


void ReportDetailed(CHAIN **Chain, int NChain, FILE *Out, COMMAND *Cmd)
{
  register int i, Cn;
  RESIDUE *p;
  BUFFER Bf, Tmp, Tmp1;

  PrepareBuffer(Bf,Chain);

  Glue(Bf,"REM",Out);
  sprintf(Tmp,"REM  --------------- ");
  strcat(Tmp,"Detailed secondary structure assignment");
  strcat(Tmp,"-------------");
  Glue(Bf,Tmp,Out);
  Glue(Bf,"REM",Out);
  Glue(Bf,"REM  |---Residue---|    |--Structure--|   |-Phi-|   |-Psi-|  |-Area-|      ",Out);

  for( Cn=0; Cn<NChain; Cn++ ) {

    if( !Chain[Cn]->Valid )
      continue;
    
    for( i=0; i<Chain[Cn]->NRes; i++ ) {
      p = Chain[Cn]->Rsd[i];
      sprintf(Tmp,"ASG  %3s %c %4s %4d    %c   %11s   %7.2f   %7.2f   %7.1f",
	      p->ResType,SpaceToDash(Chain[Cn]->Id),p->PDB_ResNumb,i+1,
	      p->Prop->Asn,Translate(p->Prop->Asn),p->Prop->Phi,
	      p->Prop->Psi,p->Prop->Solv);

      if( Cmd->BrookhavenAsn ) {
	Tmp[26] = p->Prop->PdbAsn;
	Tmp[25] = ' ';
	Tmp[27] = ' ';
      }

      if( Cmd->DsspAsn ) {
	Tmp[28] = p->Prop->DsspAsn;
	Tmp[27] = ' ';
	Tmp[29] = ' ';
	sprintf(Tmp1," %6.1f ",p->Prop->DsspSolv);
	strcat(Tmp,Tmp1);
      }
      Glue(Bf,Tmp,Out);
    }
  }
  
}

void ReportHydrBonds(CHAIN **Chain, int NChain, HBOND **HBond, FILE *Out,
		     COMMAND *Cmd)
{
  register int i, k, Cn;
  int Cnt, Res;
  BUFFER Bf, Tmp, Tmp1;
  HBOND *p;
  RESIDUE *r;

  PrepareBuffer(Bf,Chain);

  Glue(Bf,"REM",Out);
  sprintf(Tmp,"REM  ------------------ ");
  strcat(Tmp,"Mainchain hydrogen bonds");
  strcat(Tmp," ------------------------");
  Glue(Bf,Tmp,Out);
  Glue(Bf,"REM",Out);

  Glue(Bf,"REM  Definition of Stickle et al., J.Mol.Biol. 226:1143-1159, 1992",Out);
  Glue(Bf,"REM  A1 is the angle between the planes of donor complex and O..N-C",Out);
  Glue(Bf,"REM  A2 is the angle between the planes of acceptor complex and N..O=C",Out);
  Glue(Bf,"REM",Out);

  sprintf(Tmp,"HBT  %-6d",Chain[0]->NHydrBondTotal);
  Glue(Bf,Tmp,Out);
  sprintf(Tmp,"HBI  %-6d",Chain[0]->NHydrBondInterchain);
  Glue(Bf,Tmp,Out);
  for( Cn=0; Cn<NChain; Cn++ )
    if( Chain[Cn]->Valid ) {

      sprintf(Tmp,"HBC  %-6d  %s %c %4d",
	      Chain[Cn]->NHydrBond,Chain[Cn]->File,SpaceToDash(Chain[Cn]->Id),Chain[Cn]->NRes);
      Glue(Bf,Tmp,Out);
    }
  Glue(Bf,"REM",Out);

  Glue(Bf,"REM  |--Residue 1--|     |--Residue 2--|  N-O N..O=C O..N-C     A1     A2",Out);

  for( Cn=0; Cn<NChain; Cn++ ) {

    if( !Chain[Cn]->Valid )
      continue;
    
    for( i=0; i<Chain[Cn]->NRes; i++ ) {

      r = Chain[Cn]->Rsd[i];

      Cnt = 0;
      for( k=0; k<r->Inv->NBondDnr; k++ ) {
	p = HBond[r->Inv->HBondDnr[k]];
	if( p->ExistHydrBondRose ) {
	  Res = p->Acc->A_Res;
	  sprintf(Tmp,"DNR %4s %c %4s %4d -> ",
		  r->ResType,SpaceToDash(Chain[Cn]->Id),r->PDB_ResNumb,i);

	  sprintf(Tmp1,"%4s %c %4s %4d %4.1f %6.1f %6.1f %6.1f %6.1f ",
		  p->Acc->Chain->Rsd[Res]->ResType,SpaceToDash(Chain[Cn]->Id),
		  p->Acc->Chain->Rsd[Res]->PDB_ResNumb,Res,p->AccDonDist,p->AccAng,
		  p->DonAng,p->AccDonAng,p->DonAccAng);
	  strcat(Tmp,Tmp1);
	  Glue(Bf,Tmp,Out);
	  Cnt++;
	}
      }

      Cnt = 0;
      for( k=0; k<r->Inv->NBondAcc; k++ ) {
	p = HBond[r->Inv->HBondAcc[k]];
	if( p->ExistHydrBondRose ) {
	  Res = p->Dnr->D_Res;
	  sprintf(Tmp,"ACC %4s %c %4s %4d -> ",
		  r->ResType,SpaceToDash(Chain[Cn]->Id),r->PDB_ResNumb,i);

	  sprintf(Tmp1,"%4s %c %4s %4d %4.1f %6.1f %6.1f %6.1f %6.1f ",
		  p->Dnr->Chain->Rsd[Res]->ResType,SpaceToDash(Chain[Cn]->Id),
		  p->Dnr->Chain->Rsd[Res]->PDB_ResNumb,Res,p->AccDonDist,
		  p->AccAng,p->DonAng,p->AccDonAng,p->DonAccAng);
	  strcat(Tmp,Tmp1);
	  Glue(Bf,Tmp,Out);
	  Cnt++;
	}
      }

    }
  }

}

void ReportSSBonds(CHAIN **Chain, FILE *Out)
{
  register int i;
  BUFFER Bf, Tmp;
  SSBOND *s;

  if( !Chain[0]->NBond ) return;

  PrepareBuffer(Bf,Chain);

  for( i=0; i<Chain[0]->NBond; i++ ) {
    s = Chain[0]->SSbond[i];
    sprintf(Tmp,"LOC  Disulfide    CYS  %4s %c      CYS   %4s %c         ",
	    s->PDB_ResNumb1,SpaceToDash(s->ChainId1),
	    s->PDB_ResNumb2,SpaceToDash(s->ChainId2));

    if( s->AsnSource == Pdb )
      strcat(Tmp,"   PDB");
    else
      strcat(Tmp,"STRIDE\n");
    Glue(Bf,Tmp,Out);
  }
}

void ReportTurnTypes(CHAIN **Chain, int NChain, FILE *Out, COMMAND *Cmd)
{

  register int Cn, Tn;
  BUFFER Bf, Tmp;
  TURN *t;

  Tn = 0;
  for( Cn=0; Cn<NChain; Cn++ )
    if( Chain[Cn]->Valid )
       Tn += Chain[Cn]->NAssignedTurn;

  if( !Tn ) return;

  PrepareBuffer(Bf,Chain);

  for( Cn=0; Cn<NChain; Cn++ ) {
    if( !Chain[Cn]->Valid )
      continue;
    for( Tn=0; Tn<Chain[Cn]->NAssignedTurn; Tn++ ) {
      t = Chain[Cn]->AssignedTurn[Tn];
      sprintf(Tmp,"LOC  %-11s  %3s  %4s %c      %3s   %4s %c",
	      Translate(t->TurnType),t->Res1,t->PDB_ResNumb1,
	      SpaceToDash(Chain[Cn]->Id),t->Res2,
	      t->PDB_ResNumb2,SpaceToDash(Chain[Cn]->Id));

      Glue(Bf,Tmp,Out);
    }
  }
}


void ReportShort(CHAIN **Chain, int NChain, FILE *Out, COMMAND *Cmd)
{
  
  register int Cn, i;
  BUFFER Bf, Tmp;
  char *Asn;
  static char *StrTypes = "HGIE";
  int Bound[MAX_ASSIGN][2], NStr;

  if( !ExistsSecStr(Chain,NChain) )
    return;

  PrepareBuffer(Bf,Chain);

  Glue(Bf,"REM",Out);
  Glue(Bf,"REM",Out);
  Glue(Bf,"REM",Out);

  for( ; *StrTypes!= '\0'; StrTypes++ ) {

    for( Cn=0; Cn<NChain; Cn++ ) {

      if( !Chain[Cn]->Valid )
	continue;

      Asn = (char *)ckalloc(Chain[Cn]->NRes*sizeof(char));
      ExtractAsn(Chain,Cn,Asn);
      NStr = Boundaries(Asn,Chain[Cn]->NRes,(*StrTypes),Bound);

      for( i=0; i<NStr; i++ ) {
	sprintf(Tmp,"LOC  %-10s   %3s  %4s %c      %3s   %4s %c",Translate(*StrTypes),
		Chain[Cn]->Rsd[Bound[i][0]]->ResType,
		Chain[Cn]->Rsd[Bound[i][0]]->PDB_ResNumb,
		SpaceToDash(Chain[Cn]->Id),
		Chain[Cn]->Rsd[Bound[i][1]]->ResType,
		Chain[Cn]->Rsd[Bound[i][1]]->PDB_ResNumb,
		SpaceToDash(Chain[Cn]->Id));
	Glue(Bf,Tmp,Out);
      }
      
      free(Asn);
    }    
  }

}


void PrepareBuffer(BUFFER Bf, CHAIN **Chain)
{

  memset(Bf,' ',OUTPUTWIDTH);

  strcpy(Bf+OUTPUTWIDTH-5,Chain[0]->PdbIdent);
  Bf[OUTPUTWIDTH] = '\0';
  Bf[OUTPUTWIDTH-1]   = '\n';
	   
}

void Glue(char *String1, char *String2, FILE *Out)
{

  BUFFER Bf;

  strcpy(Bf,String1);
  strncpy(Bf,String2,(int)strlen(String2));

  fprintf(Out,"%s",Bf);
}






