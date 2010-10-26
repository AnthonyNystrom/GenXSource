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
    
int SSBond(CHAIN **Chain, int NChain)
{

  register int Res1, Res2, Cn1, Cn2;
  int S1, S2, Bn, Cnt=0;
  
  for( Cn1=0; Cn1<NChain; Cn1++ )
    for( Res1=0; Res1<Chain[Cn1]->NRes; Res1++ ) {
      if( strcmp(Chain[Cn1]->Rsd[Res1]->ResType,"CYS") )
	continue;
      for( Cn2=Cn1; Cn2<NChain; Cn2++ )
	for( Res2 = ( (Cn2 == Cn1)? Res1+1 : 0) ; Res2<Chain[Cn2]->NRes; Res2++ ) {
	  if( strcmp(Chain[Cn2]->Rsd[Res2]->ResType,"CYS") )
	    continue;
	  
	  if( !ExistSSBond(Chain,NChain,Cn1,Cn2,
			   Chain[Cn1]->Rsd[Res1]->PDB_ResNumb,
			   Chain[Cn2]->Rsd[Res2]->PDB_ResNumb) && 
	     stride_FindAtom(Chain[Cn1],Res1,"SG",&S1) && stride_FindAtom(Chain[Cn2],Res2,"SG",&S2) &&
	     Dist(Chain[Cn1]->Rsd[Res1]->Coord[S1],
		  Chain[Cn2]->Rsd[Res2]->Coord[S2]) <= SSDIST ) {
	    Bn = Chain[0]->NBond;
	    Chain[0]->SSbond[Bn] =  (SSBOND *)ckalloc(sizeof(SSBOND));
	    strcpy(Chain[0]->SSbond[Bn]->PDB_ResNumb1,Chain[Cn1]->Rsd[Res1]->PDB_ResNumb);
	    strcpy(Chain[0]->SSbond[Bn]->PDB_ResNumb2,Chain[Cn2]->Rsd[Res2]->PDB_ResNumb);
	    Chain[0]->SSbond[Bn]->ChainId1 = Chain[Cn1]->Id;
	    Chain[0]->SSbond[Bn]->ChainId2 = Chain[Cn2]->Id;
	    Chain[0]->SSbond[Bn]->AsnSource = Stride;
	    Chain[0]->NBond++;
	    Cnt++;
	  }
	}
    }
  
  return(Cnt);
}

STRIDE_BOOL ExistSSBond(CHAIN **Chain,int NChain, int Cn1,int Cn2,char *Res1,char *Res2)
{

  register int i;
  SSBOND *ptr;

  for( i=0; i<Chain[0]->NBond; i++ ) {
    ptr = Chain[0]->SSbond[i];
    if( ( !strcmp(Res1,ptr->PDB_ResNumb1) && 
	  !strcmp(Res2,ptr->PDB_ResNumb2) &&
	  FindChain(Chain,NChain,ptr->ChainId1) == Cn1 &&
	  FindChain(Chain,NChain,ptr->ChainId2) == Cn2 ) ||
        ( !strcmp(Res2,ptr->PDB_ResNumb1) && 
          !strcmp(Res1,ptr->PDB_ResNumb2) &&
	  FindChain(Chain,NChain,ptr->ChainId1) == Cn2 &&
	  FindChain(Chain,NChain,ptr->ChainId2) == Cn1 ) )
      return(SUCCESS);
  }

  return(FAILURE);
}
