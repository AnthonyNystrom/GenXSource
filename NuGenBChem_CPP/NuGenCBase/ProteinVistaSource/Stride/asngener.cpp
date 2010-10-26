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

/*************************************************************************
**                                                                      **
** Remove short stretches of secondary structure from the assignment    **
**                                                                      **
** INPUT:   *Asn       String with one letter secondary structure       **
**                     assignment                                       **
**          Length     Length of the string                             **
**          SecStrType Type of the secondary structure to which this    **
**                     operation should be applied                      **
**          EditChar   Character to be used instead of removed symbols  **
**          MaxLength  Maximal length of secondary struture segments to **
**                     be removed                                       **
**                                                                      **
** OUTPUT:  *Asn       Edited secondary structure assignment            **
**                                                                      **
*************************************************************************/
void CorrectAsn(char *Asn, int Length, char SecStrType, char EditChar, int MaxLength)
{

  int NStr = 0, Res, Flag = 0, Bound[MAX_ASSIGN][2], i;

  for( Res=0; Res<Length; Res++ ) {
    if( Asn[Res] == SecStrType && Flag == 0 ) {
      Flag = 1; 
      Bound[NStr][0] = Res;
    }
    else
    if( Asn[Res] != SecStrType && Flag == 1 ) {
      Flag = 0; 
      Bound[NStr++][1] = Res-1;
    }
  }

  for( i=0; i<NStr; i++ )
    if( Bound[i][1]-Bound[i][0]+1 <= MaxLength )
      for( Res=Bound[i][0]; Res<=Bound[i][1]; Res++ ) 
	Asn[Res] = EditChar;
}

void CorrectAsnDouble(char *Asn1, char *Asn2, char *KnownAsn, int Length, 
		      char SecStrType, char EditChar)
{

  register int Res;

  for( Res=0; Res<Length; Res++ )
    if( (Asn1[Res] == SecStrType || Asn2[Res] == SecStrType) && KnownAsn[Res] != SecStrType &&
        ( (Res == 0 && Asn1[Res+1] != SecStrType && Asn2[Res+1] != SecStrType) ||
	  (Res == Length-1 && Asn1[Res-1] != SecStrType && Asn2[Res-1] != SecStrType) ||
          (Res > 0 && Res < Length-1 && 
	   Asn1[Res-1] != SecStrType && Asn2[Res-1] != SecStrType && 
	   Asn1[Res+1] != SecStrType && Asn2[Res+1] != SecStrType) ) )
      Asn1[Res] = Asn2[Res] = EditChar;
      
}

/*************************************************************************
**                                                                      **
** Calculate the number of true positives, true negatives, false        **
** negatives and false positives resulting from comparison of test and  **
** known secondary structure assignments for a particular secondary     **
** structure type                                                       **
**                                                                      **
** INPUT:   *TestAsn   String with one letter test secondary structure  **
**                     assignment                                       **
**          *KnownAsn  String with one letter known secondary structure **
**                     assignment                                       **
**          Length     Length of the assignment                         **
**          SecStrType Type of the secondary structure to which this    **
**                     operation should be applied                      **
**                                                                      **
** OUTPUT:  *Quality   Pointer to the structure with quality assessment **
**                                                                      **
*************************************************************************/
int Difference(char *TestAsn, char *KnownAsn, int Length, char SecStrType, QUALITY *Qual)
{
  register int Res;

  Qual->TP = Qual->TN = Qual->FP = Qual->FN = 0;

  for( Res=0; Res<Length; Res++ ) {
    if( KnownAsn[Res] != 'X' ) { 

      if( KnownAsn[Res] == SecStrType && TestAsn[Res]  == SecStrType ) Qual->TP++;
      else 
      if( KnownAsn[Res] != SecStrType && TestAsn[Res]  != SecStrType ) Qual->TN++;
      else 
      if( KnownAsn[Res] != SecStrType && TestAsn[Res]  == SecStrType ) Qual->FP++;
      else 
      if( KnownAsn[Res] == SecStrType && TestAsn[Res]  != SecStrType ) Qual->FN++;
    }
  }

  if( Qual->TP == 0 && Qual->TN == 0 && Qual->FP == 0 && Qual->FN == 0 )  {
    Qual->Perc = 0.0;
    return(FAILURE);
  }

  Qual->Perc = 
    ((float)Qual->TP+(float)Qual->TN)/
      ((float)Qual->TP+(float)Qual->TN+(float)Qual->FP+(float)Qual->FN);

  return(SUCCESS);
}

/*************************************************************************
**                                                                      **
** Calculate percent of the correctly assigned residues                 **
**                                                                      **
** INPUT:   *TestAsn   String with one letter test secondary structure  **
**                     assignment                                       **
**          *KnownAsn  String with one letter known secondary structure **
**                     assignment                                       **
**          Length     Length of the assignment                         **
**                                                                      **
** RETURNS:            Percent correct                                  **
**                                                                      **
*************************************************************************/
float PercentCorrect(char *TestAsn, char *KnownAsn, int Length)
{
  int Res, Count=0;;

  for( Res=0; Res<Length; Res++ )
    if( KnownAsn[Res] == TestAsn[Res] )
      Count++;

  return( ((float)Count/(float)Length) );
}

/*************************************************************************
**                                                                      **
** Calculate measures of secondary structure assignment quality based   **
** on the number of true positives, true negatives, false negatives and **
** false positives resulting from comparison of test and known          **
** assignments                                                          **
**                                                                      **
** INPUT:   *Quality   Pointer to the structure with quality assessment **
**                     assignment                                       **
** OUTPUT:  Quality->Corr  Correlation coefficient between the two      **
**                         assignments as suggested by B.Matthews       **
**                         (1975) Biochim. Biophys. Acta, 405, 442-451  **
**          Quality->Perc  Percent correct                              **
**                                                                      **
*************************************************************************/
int AssessCorr(QUALITY *Qual)
{

  float TP, TN, FP, FN;

  if( (Qual->TP == 0 && Qual->FN == 0) || (Qual->TP == 0 && Qual->FP == 0) ) return(FAILURE);
  else {
    TP = (float)Qual->TP; 
    TN = (float)Qual->TN; 
    FP = (float)Qual->FP; 
    FN =(float)Qual->FN;

    Qual->Corr = (float)((TP*TN - FN*FP)/sqrt((TN+FN)*(TN+FP)*(TP+FN)*(TP+FP)));

    return(SUCCESS);
  }
}

int AssessPerc(QUALITY *Qual)
{

  float TP, TN, FP, FN;

  TP = (float)Qual->TP; 
  TN = (float)Qual->TN; 
  FP = (float)Qual->FP; 
  FN =(float)Qual->FN;

  Qual->Perc = (TP+TN)/(TP+TN+FP+FN);

  return(SUCCESS);
}

void ExcludeObvious(char *Asn1, char *Asn2, char *KnownAsn, int Length)
{
  register int i;

  for( i=0; i<Length; i++ )
    if( Asn1[i] == Asn2[i] ) {
      KnownAsn[i] = 'X';
      Asn1[i] = 'X';
      Asn2[i] = 'X';
    }
}


