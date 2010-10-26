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

void StrideInitChain(CHAIN **Chain, COMMAND *Cmd)
{
  int i;

  *Chain = (CHAIN *)ckalloc(sizeof(CHAIN));

  (*Chain)->NRes                = 0;
  (*Chain)->NHetRes             = 0;
  (*Chain)->NonStandRes         = 0;
  (*Chain)->NHet                = 0;
  (*Chain)->NonStandAtom        = 0;
  (*Chain)->NHelix              = 0;
  (*Chain)->NSheet              = -1;
  (*Chain)->NTurn               = 0;
  (*Chain)->NAssignedTurn       = 0;
  (*Chain)->NBond               = 0;
  (*Chain)->NHydrBond           = 0;
  (*Chain)->NHydrBondTotal      = 0;
  (*Chain)->NHydrBondInterchain = 0;
  (*Chain)->Method              = XRay;
  (*Chain)->Ter                 = 0;
  (*Chain)->Resolution          = 0.0;

  (*Chain)->File = (char *)ckalloc(BUFSZ*sizeof(char));
  for(i = 0;i < BUFSZ;i++)
	(*Chain)->File[i] = '\0';

  /* Removed fixed size memory limitations */
  /*(*Chain)->Rsd = (RESIDUE **)ckalloc(MAX_RES*sizeof(RESIDUE *));*/
  (*Chain)->Rsd = (RESIDUE **)ckalloc(Cmd->MaxLength*sizeof(RESIDUE *));

  /* Removed fixed size memory limitations */
  /*for(i = 0;i < MAX_RES;i++)*/
  for(i = 0;i < Cmd->MaxLength;i++)
	(*Chain)->Rsd[i] = NULL;

  (*Chain)->HetRsd = (HETERORESIDUE **)ckalloc(MAX_HETRES*sizeof(HETERORESIDUE *));

  for(i = 0;i < MAX_HETRES;i++)
	(*Chain)->HetRsd[i] = NULL;

  (*Chain)->Het = (HET **)ckalloc(MAX_HET*sizeof(HET *));

  for(i = 0;i < MAX_HET;i++)
	(*Chain)->Het[i] = NULL;

  (*Chain)->Helix = (HELIX **)ckalloc(MAX_HELIX*sizeof(HELIX *));

  for(i = 0;i < MAX_HELIX;i++)
	(*Chain)->Helix[i] = NULL;

  (*Chain)->Sheet = (SHEET **)ckalloc(MAX_SHEET*sizeof(SHEET *));

  for(i = 0;i < MAX_SHEET;i++)
	(*Chain)->Sheet[i] = NULL;

  (*Chain)->Turn = (TURN **)ckalloc(MAX_TURN*sizeof(TURN *));

  for(i = 0;i < MAX_TURN;i++)
	(*Chain)->Turn[i] = NULL;

  (*Chain)->AssignedTurn = (TURN **)ckalloc(MAX_TURN*sizeof(TURN *));

  for(i = 0;i < MAX_TURN;i++)
	(*Chain)->AssignedTurn[i] = NULL;

  (*Chain)->SSbond = (SSBOND **)ckalloc(MAX_BOND*sizeof(SSBOND *));

  for(i = 0;i < MAX_BOND;i++)
	(*Chain)->SSbond[i] = NULL;

  (*Chain)->Info = (char **)ckalloc(MAX_INFO*sizeof(char *));

  for(i = 0;i < MAX_INFO;i++)
	(*Chain)->Info[i] = NULL;

  (*Chain)->Valid = YES;
}

