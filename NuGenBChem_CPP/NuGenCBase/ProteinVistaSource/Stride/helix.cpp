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

void Helix(CHAIN **Chain, int Cn, HBOND **HBond, COMMAND *Cmd, float **PhiPsiMap)
{

  int BondNumb, i;
  float *Prob, _CONST;
  RESIDUE **r;

  _CONST = 1+Cmd->C1_H;

  /* Removed fixed size memory limitations */
  /*Prob = (float *)ckalloc(MAX_RES*sizeof(float));*/
  Prob = (float *)ckalloc(Cmd->MaxLength*sizeof(float));

  for( i=0; i<Chain[Cn]->NRes; i++ )
    Prob[i] = 0.0; 


  for( i=0; i<Chain[Cn]->NRes-5; i++ ) {

    r = &Chain[Cn]->Rsd[i];

    if( r[0]->Prop->PhiZn != ERR && r[0]->Prop->PsiZn != ERR &&
        r[1]->Prop->PhiZn != ERR && r[1]->Prop->PsiZn != ERR &&
        r[2]->Prop->PhiZn != ERR && r[2]->Prop->PsiZn != ERR &&
        r[3]->Prop->PhiZn != ERR && r[3]->Prop->PsiZn != ERR &&
        r[4]->Prop->PhiZn != ERR && r[4]->Prop->PsiZn != ERR ) {

      if( (BondNumb = FindPolInt(HBond,r[4],r[0])) != ERR ) {
	Prob[i] = (float)(HBond[BondNumb]->Energy*(_CONST+Cmd->C2_H*
	    0.5*(PhiPsiMap[r[0]->Prop->PhiZn][r[0]->Prop->PsiZn]+
		 PhiPsiMap[r[4]->Prop->PhiZn][r[4]->Prop->PsiZn])));

      }
    }
  }
  
  for( i=0; i<Chain[Cn]->NRes-5; i++ ) {

    if( Prob[i] < Cmd->Treshold_H1 && Prob[i+1] < Cmd->Treshold_H1 ) {

      r = &Chain[Cn]->Rsd[i];

      r[1]->Prop->Asn = 'H'; 
      r[2]->Prop->Asn = 'H'; 
      r[3]->Prop->Asn = 'H'; 
      r[4]->Prop->Asn = 'H';
      if( r[0]->Prop->PhiZn!= ERR && r[0]->Prop->PsiZn != ERR &&
	  PhiPsiMap[r[0]->Prop->PhiZn][r[0]->Prop->PsiZn] > Cmd->Treshold_H3 )
	r[0]->Prop->Asn = 'H';
      if( r[5]->Prop->PhiZn != ERR && r[5]->Prop->PsiZn != ERR &&
	  PhiPsiMap[r[5]->Prop->PhiZn][r[5]->Prop->PsiZn] > Cmd->Treshold_H4 )
	r[5]->Prop->Asn = 'H';
    }
  }

  for( i=0; i<Chain[Cn]->NRes-4; i++ ) {

    r = &Chain[Cn]->Rsd[i];

    if(  FindBnd(HBond,r[3],r[0]) != ERR && FindBnd(HBond,r[4],r[1]) != ERR &&
       /*************************** This should be improved **************************/
         ( (r[1]->Prop->Asn != 'H' && r[2]->Prop->Asn != 'H') ||
	   (r[2]->Prop->Asn != 'H' && r[3]->Prop->Asn != 'H') ) ) 
       /******************************************************************************/
      {
	r[1]->Prop->Asn = 'G'; 
	r[2]->Prop->Asn = 'G'; 
	r[3]->Prop->Asn = 'G'; 
      }
  }

  for( i=0; i<Chain[Cn]->NRes-6; i++ ) {

    r = &Chain[Cn]->Rsd[i];

    if( FindBnd(HBond,r[5],r[0]) != ERR && FindBnd(HBond,r[6],r[1]) != ERR &&
        r[1]->Prop->Asn == 'C' && r[2]->Prop->Asn == 'C' && 
        r[3]->Prop->Asn == 'C' && r[4]->Prop->Asn == 'C' && 
        r[5]->Prop->Asn == 'C' ) {
      r[1]->Prop->Asn = 'I'; 
      r[2]->Prop->Asn = 'I';
      r[3]->Prop->Asn = 'I'; 
      r[4]->Prop->Asn = 'I'; 
      r[5]->Prop->Asn = 'I'; 
    }
  }

  if( Cmd->Info ) {
    fprintf(stdout,"%s%c\n",Chain[Cn]->File,Chain[Cn]->Id);

    for( i=0; i<Chain[Cn]->NRes-4; i++ ) {

      r = &Chain[Cn]->Rsd[i];

      if( r[0]->Prop->PhiZn != ERR && r[0]->Prop->PsiZn != ERR &&
	  r[4]->Prop->PhiZn != ERR && r[4]->Prop->PsiZn != ERR ) {
	
	fprintf(stdout,"%s (%d) %c %10.7f %8.5f %8.5f | %4d %4d\n",
		r[0]->PDB_ResNumb,i,r[0]->Prop->Asn,Prob[i],
		PhiPsiMap[r[0]->Prop->PhiZn][r[0]->Prop->PsiZn],
		PhiPsiMap[r[4]->Prop->PhiZn][r[4]->Prop->PsiZn],
		r[4]->Prop->PhiZn,r[4]->Prop->PsiZn);
      }
    }
  }
  free(Prob);
}







