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
** Calculate the number of individual secondary structure elements of   **
** type SecStrType and length not less than ElemLength that are:        **
**  - Present in Asn1 and Asn2 and absent  in Asn3 (YYN)                **
**  - Present in Asn2 and Asn3 and absent  in Asn1 (NYY)                **
**  - Absent  in Asn2 and Asn3 and present in Asn1 (YYN)                **
**  - Absent  in Asn1 and Asn2 and present in Asn3 (YYN)                **
**                                                                      **
*************************************************************************/

int FullElement(char *Asn1, char *Asn2, char *Asn3, int Length, char SecStrType, int ElemLength,
		 char EditChar, int *YYN, int *NYY, int *YNN, int *NNY)
{

  register int i, j, Count1, Count2, Count3;
  int Beg, ElLength;

  *YYN = 0;
  *NYY = 0;
  *YNN = 0;
  *NNY = 0;

  if( ElemLength >= Length )
    return(0);

  ElLength = ElemLength-1;
  Count1 = 0;
  Count2 = 0;
  Count3 = 0;

  Beg = -1;
  
  for( i=1; i<Length; i++ ) {
    if( ( i == 0 && 
	  ( Asn1[i] == SecStrType && Asn2[i] == SecStrType && Asn3[i] == SecStrType) ||
          ( Asn1[i] != SecStrType && Asn2[i] != SecStrType && Asn3[i] != SecStrType) )
       ||
        ( i  > 0 && 
	  ( Asn1[i] != Asn1[i-1] || Asn2[i] != Asn2[i-1] || Asn3[i] != Asn3[i-1] ) )
       ||
        i == Length-1 ) {
      
      if( Count1 >= ElLength && Count2 >= ElLength && Count3 <  ElLength ) 
	(*YYN)++;
      else
      if( Count1 <  ElLength && Count2 >= ElLength && Count3 >= ElLength ) 
	(*NYY)++;
      else
      if( Count1 >= ElLength && Count2 <  ElLength && Count3 <  ElLength ) 
	(*YNN)++;
      else
      if( Count1 <  ElLength && Count2 <  ElLength && Count3 >= ElLength ) 
	(*NNY)++;
      
/*       if( Count1 >= ElLength || Count2 >= ElLength || Count3 >= ElLength ) {
 * 	for( j=Beg-1; j<i; j++ ) {
 * 	  Asn1[j] = 'X';
 * 	  Asn2[j] = 'X';
 * 	  Asn3[j] = 'X';
 * 	}
 *       }
 * 
 */
      if( Count1 >= ElLength && ( Count2 < ElLength || Count3 < ElLength ) )
	for( j=Beg-1; j<i; j++ )
	  Asn1[j] = EditChar;

      if( Count2 >= ElLength && ( Count1 < ElLength || Count3 < ElLength ) )
	for( j=Beg-1; j<i; j++ )
	  Asn2[j] = EditChar;

      if( Count3 >= ElLength && ( Count1 < ElLength || Count2 < ElLength ) )
	for( j=Beg-1; j<i; j++ )
	  Asn3[j] = EditChar;

      Count1 = 0;
      Count2 = 0;
      Count3 = 0;
      Beg = -1;
      
    }
    else {
      if( Asn1[i] == SecStrType ) Count1++;
      if( Asn2[i] == SecStrType ) Count2++;
      if( Asn3[i] == SecStrType ) Count3++;
      if( Beg == -1 && (Count1 == 1 || Count2 == 1 || Count3 == 1) ) Beg = i;
    }
  }	

  CorrectAsn(Asn1,Length,SecStrType,EditChar,ElLength);
  CorrectAsn(Asn2,Length,SecStrType,EditChar,ElLength);
  CorrectAsn(Asn3,Length,SecStrType,EditChar,ElLength);

  return( (*YYN) * (*NYY) * (*YNN) * (*NNY) );
}


/*************************************************************************
**                                                                      **
** Calculate the number of individual secondary structure elements of   **
** type SecStrType in the known assignment Asn2 that are:               **
**  - Reproduced in Asn1 better than in Asn3 (Better)                   **
**  - Reproduced in Asn1 worse  than in Asn3 (Worse)                    **
**                                                                      **
*************************************************************************/

int CompareElements(char *Asn1, char *Asn2, char *Asn3, int Length, 
		   char SecStrType, int *Better, int *Worse)
{

  register int i, j, Count1, Count2;
  int TotalNumber = 0, Beg;

  *Better = 0;
  *Worse = 0;

  Beg = -1;
  
  for( i=0; i<Length; i++ ) {
    if( (Asn1[i] == SecStrType || Asn2[i] == SecStrType || Asn3[i] == SecStrType) &&
	(i == 0 || 
	 ( Asn1[i-1] != SecStrType && Asn2[i-1] != SecStrType && Asn3[i-1] != SecStrType) ) ) {
      TotalNumber++;
      Beg = i;
    }
    else
    if( Beg != -1 && ( i == Length-1 || 
	 ( Asn1[i] != SecStrType && Asn2[i] != SecStrType && Asn3[i] != SecStrType ) ) ) {
      Count1 = Count2 = 0;
      for( j=Beg; j<=i; j++ ) {
	if( (Asn1[j] == SecStrType || Asn2[j] == SecStrType) && Asn1[j] != Asn2[j] ) 
	  Count1++;
	if( (Asn3[j] == SecStrType || Asn2[j] == SecStrType) && Asn3[j] != Asn2[j] ) 
	  Count2++;
      }
      if( Count1 > Count2 ) 
	(*Worse)++;
      else
      if( Count2 > Count1 ) 
	(*Better)++;
      Beg = -1;
    }
  }
  return(TotalNumber);
}


