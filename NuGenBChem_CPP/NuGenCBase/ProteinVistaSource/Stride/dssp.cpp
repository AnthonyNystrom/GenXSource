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

int ReadDSSP(CHAIN **Chain, DSSP **Dssp, COMMAND *Cmd)
{

  FILE *fi;
  int ChainNumber = -1, Start = 0, i;
  STRIDE_BOOL DuplicateChain = NO;
  BUFFER Buffer, Tmp1, Tmp2;
  char *Fields[MAX_FIELD];
  
  if( strlen(Cmd->DsspFile) == 0 ) {
    strcpy(Cmd->DsspFile,DSSPPATH);
    GetFileNameFromPath(Chain[0]->File,Tmp1);
    StripPathFromLastExtention(Tmp1,Tmp2);
    strcat(Cmd->DsspFile,Tmp2);
    strcat(Cmd->DsspFile,".dssp");
  }

  if( (fi = fopen(Cmd->DsspFile,"r")) ) {

    while( fgets(Buffer,BUFSZ,fi) != NULL && !DuplicateChain ) {
      if( Buffer[2] == '#' ) Start = 1;
      else
      if( Start == 1 ) {
	if( Buffer[13] == '!' ) continue;
	if( ChainNumber > 0 ) {
	  for( i=0; i<ChainNumber; i++ ) 
	    if( Dssp[i]->Id == Buffer[11] ) {
	      DuplicateChain = YES;
	      fprintf(stderr,"# Duplicate chain(s) in DSSP file %s\n",Cmd->DsspFile);
	    break;
	  }
	}
	if( ( ChainNumber == -1 || Buffer[11] != Dssp[ChainNumber]->Id ) ) {
	  ChainNumber++;
	  Dssp[ChainNumber]       = (DSSP *)ckalloc(sizeof(DSSP));

	  /* Removed fixed size memory limitations */
	  /*Dssp[ChainNumber]->ResType = CharMatrix(MAX_RES,RES_FIELD);*/
	  Dssp[ChainNumber]->ResType = CharMatrix(Cmd->MaxLength,RES_FIELD);

	  /* Removed fixed size memory limitations */
	  /*Dssp[ChainNumber]->PDB_ResNumb = CharMatrix(MAX_RES,RES_FIELD);*/
	  Dssp[ChainNumber]->PDB_ResNumb = CharMatrix(Cmd->MaxLength,RES_FIELD);

	  /* Removed fixed size memory limitations */
	  /*Dssp[ChainNumber]->SecondStr = (char *)ckalloc(MAX_RES*sizeof(char));*/
	  Dssp[ChainNumber]->SecondStr = (char *)ckalloc(Cmd->MaxLength*sizeof(char));

	  /* Removed fixed size memory limitations */
	  /*Dssp[ChainNumber]->Accessibility = (float *)ckalloc(MAX_RES*sizeof(float));*/
	  Dssp[ChainNumber]->Accessibility = (float *)ckalloc(Cmd->MaxLength*sizeof(float));

	  strcpy(Dssp[ChainNumber]->File,Cmd->DsspFile);
	  Dssp[ChainNumber]->Id   = Buffer[11];
	  Dssp[ChainNumber]->NRes = 0;
	}
	
	if( islower(Buffer[13]) ) Buffer[13] = 'C';
	strcpy(Dssp[ChainNumber]->ResType[ Dssp[ChainNumber]->NRes ],OneToThree(Buffer[13]));
	if( Buffer[16] == ' ' )
	  Dssp[ChainNumber]->SecondStr[ Dssp[ChainNumber]->NRes ] = 'C';
	else
	  Dssp[ChainNumber]->SecondStr[ Dssp[ChainNumber]->NRes ] = Buffer[16];
	SplitString(Buffer+6,Fields,1);
	strcpy(Dssp[ChainNumber]->PDB_ResNumb[Dssp[ChainNumber]->NRes],Fields[0]);
	SplitString(Buffer+34,Fields,1);
	Dssp[ChainNumber]->Accessibility[ Dssp[ChainNumber]->NRes ] = (float)(atof(Fields[0]));
	Dssp[ChainNumber]->NRes++;
      }
    }
    fclose(fi);
  }

  ChainNumber++;

  for( i=0; i<ChainNumber; i++ ) 
    Dssp[i]->Id = SpaceToDash(Dssp[i]->Id);

  return(ChainNumber);
}
	      
/*************************************************************************
**                                                                      **
** Check whether PDB and DSSP files correspond to each other            **
**                                                                      **
** INPUT:   *Chain     Pointer to a PDB chain                           **
**          *Dssp      Pointer to a DSSP chain                          **
**                                                                      **
*************************************************************************/

int CompPdbDssp(CHAIN *Chain, DSSP *Dssp)
{

  int Res, CA;

  /* If the first or the last PDB residue does not have Ca, insert one residue in
     coil conformation in the beginning or in the end of the DSSP chain */
  if( !stride_FindAtom(Chain,0,"CA",&CA) ) 
    InsertFirst(Dssp,Chain);

  if( Chain->NRes-1 > 0 && !stride_FindAtom(Chain,Chain->NRes-1,"CA",&CA) ) 
    InsertLast(Dssp,Chain);

  /* Check correspondence of residue types */
  for( Res=0; Res<Chain->NRes; Res++ )
    if( strcmp(Chain->Rsd[Res]->ResType,Dssp->ResType[Res]) )
      return(FAILURE);

  /* If DSSP chain is still longer than PDB chain, shrink it */
  if( Chain->NRes != Dssp->NRes ) 
    Dssp->NRes = Chain->NRes;

  return(SUCCESS);
}

/*************************************************************************
**                                                                      **
** Insert one residue in the beginning of a DSSP chain and assign coil  **
** conformation to it. This is neccessary to get rid of descrepancy     **
** between the DSSP and PDB protein chains resulting from the fact      **
** that DSSP does not consider residues without Ca atoms. Such residues,**
** e.g. ACE, often occur as first residues in PDB files                 **
**                                                                      **
** INPUT:   *Chain     Pointer to protein chain                         **
**          *Dssp      Pointer to DSSP chain                            **
**                                                                      **
** OUTPUT:  *DSSP      Pointer to modified DSSP chain                   **
**                                                                      **
*************************************************************************/
void InsertFirst(DSSP *Dssp, CHAIN *Chain)
{
  int Res;

  for( Res=Dssp->NRes; Res>=1; Res-- ) {
    strcpy(Dssp->ResType[Res],Dssp->ResType[Res-1]);
    strcpy(Dssp->PDB_ResNumb[Res],Dssp->PDB_ResNumb[Res-1]);
    Dssp->SecondStr[Res] = Dssp->SecondStr[Res-1];
  }

  strcpy(Dssp->ResType[0],Chain->Rsd[0]->ResType);
  strcpy(Dssp->PDB_ResNumb[0],Chain->Rsd[0]->PDB_ResNumb);
  Dssp->SecondStr[0] = 'C';
  
  Dssp->NRes++;
}

/*************************************************************************
**                                                                      **
** Insert one residue in the end a DSSP chain and assign coil           **
** conformation to it. This is neccessary to get rid of descrepancy     **
** between the DSSP and PDB protein chains resulting from the fact      **
** that DSSP does not consider residues without Ca atoms. Such residues,**
** often occur as last residues in PDB files                            **
**                                                                      **
** INPUT:   *Chain     Pointer to protein chain                         **
**          *Dssp      Pointer to DSSP chain                            **
**                                                                      **
** OUTPUT:  *DSSP      Pointer to modified DSSP chain                   **
**                                                                      **
*************************************************************************/
void InsertLast(DSSP *Dssp, CHAIN *Chain)
{

  strcpy(Dssp->ResType[Dssp->NRes],Chain->Rsd[Dssp->NRes]->ResType);
  strcpy(Dssp->PDB_ResNumb[Dssp->NRes],Chain->Rsd[Dssp->NRes]->PDB_ResNumb);
  Dssp->SecondStr[Dssp->NRes] = 'C';
  
  Dssp->NRes++;
}

/*************************************************************************
**                                                                      **
** Get DSSP secondary structure assignment for every residue            **
**                                                                      **
*************************************************************************/
void GetDsspAsn(CHAIN **Chain, int NChain, COMMAND *Cmd)
{
  DSSP **Dssp;

  int NDsspChain=0, DsspCn, Cn, i;

  /* 
   * Dynamically allocate memory for the required number of 
   * chains!
   */
  Dssp  = (DSSP **)ckalloc(NChain*sizeof(DSSP *));
  /*Dssp  = (DSSP **)ckalloc(MAX_CHAIN*sizeof(DSSP *));*/
  
  if( (NDsspChain = ReadDSSP(Chain,Dssp,Cmd)) == 0 )
    die("NODSSP Dssp file for %s not found\n",Chain[0]->File);
  
  for( Cn=0; Cn<NChain; Cn++ ) {
    
    if( !Chain[Cn]->Valid )
      continue;
    
    for( DsspCn=0; DsspCn<NDsspChain; DsspCn++ )
      if( SpaceToDash(Chain[Cn]->Id) == Dssp[DsspCn]->Id )
	break;

    if( DsspCn == NDsspChain ) {
      fprintf(stderr,"No DSSP chain corresponding to %s%c\n",
	      Chain[Cn]->File,SpaceToDash(Chain[Cn]->Id));
      continue;
    }

    if( !CompPdbDssp(Chain[Cn],Dssp[DsspCn]) )
      die("PDBDSSPDIF Chain %s%c differs from %s%c\n",
	  Dssp[DsspCn]->File,Dssp[DsspCn]->Id,Chain[Cn]->File,SpaceToDash(Chain[Cn]->Id));
    
    for( i=0; i<Chain[Cn]->NRes; i++ ) {
      Chain[Cn]->Rsd[i]->Prop->DsspAsn  = Dssp[DsspCn]->SecondStr[i];
      Chain[Cn]->Rsd[i]->Prop->DsspSolv = Dssp[DsspCn]->Accessibility[i];
    }
  }

  if( Cmd->DsspAsn ) {
    for( i=0; i<NDsspChain; i++ ) free(Dssp[i]);
    free(Dssp);
  }
  
}

