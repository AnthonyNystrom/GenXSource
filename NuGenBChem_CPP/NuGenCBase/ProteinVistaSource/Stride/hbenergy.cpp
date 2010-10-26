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

/*************************************************
 Calculate the hydrogen bond energy as defined by
 Boobbyer et al., 1989
**************************************************/

void GRID_Energy(float *CA2, float *C, float *O, float *H, float *N, COMMAND *Cmd, HBOND *HBond)
{

  float ProjH[3];

 /***** Distance dependence ( 8-6 potential ) ****/

  if( Cmd->Truncate && HBond->AccDonDist < RmGRID ) 
    HBond->AccDonDist = RmGRID;
  HBond->Er = (float)(CGRID/pow((double)(HBond->AccDonDist),8.0) - 
	  DGRID/pow((double)(HBond->AccDonDist),6.0));

 /************** Angular dependance ****************/

 /* Find projection of the hydrogen on the O-C-CA plane */
  Project4_123(O,C,CA2,H,ProjH); 


 /* Three angles determining the direction of the hydrogen bond */
  HBond->ti = (float)(fabs(180.0 - Ang(ProjH,O,C))); 
  HBond->to = Ang(H,O,ProjH);             
  HBond->p  = Ang(N,H,O);

 /* Calculate both angle-dependent HB energy components Et and Ep */ 
  if( HBond->ti >= 0.0 && HBond->ti < 90.0 )   
    HBond->Et = (float)(cos(RAD(HBond->to))*(0.9+0.1*sin(RAD(2*HBond->ti))));
  else
  if( HBond->ti >= 90.0 && HBond->ti < 110.0 ) 
    HBond->Et = (float)(K1GRID*cos(RAD(HBond->to))*
      (pow((K2GRID-pow(cos(RAD(HBond->ti)),2.0)),3.0)));
  else
    HBond->Et = 0.0;

  if( HBond->p > 90.0 && HBond->p < 270.0 )
    HBond->Ep = (float)(pow(cos(RAD(HBond->p)),2.0));
  else
    HBond->Ep = 0.0;

    /******** Full hydrogen bond energy *********************/
  HBond->Energy = (float)(1000.0*HBond->Er*HBond->Et*HBond->Ep);
}

#define Q -27888.0

/********************************************************
 Calculate the energy of polar interaction as defined by
 Kabsch and Sander (1983) 
*********************************************************/

void DSSP_Energy(float *Dummy, float *C, float *O, float *H, float *N, COMMAND *Cmd, 
		 HBOND *HBond)
                     
/* Dummy not used, for compatibility with GRID_Energy */
{ 
	HBond->Energy = (float)(Q/Dist(O,H) + Q/Dist(C,N) - 
		Q/HBond->AccDonDist - Q/Dist(C,H)); 
}












