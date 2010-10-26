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
    
void BetaTurn(CHAIN **Chain, int Cn)
{

  register int i;
  RESIDUE **r;
  TURN *t;
  int CA1, CA4, Tn;
  float Phi2, Phi3, Psi2, Psi3, Range1 = 30.0, Range2 = 45.0;
  char TurnType;

  for( i=0; i<Chain[Cn]->NRes-4; i++ ) {

    r = &Chain[Cn]->Rsd[i];
    
    if( r[1]->Prop->Asn == 'H' || r[2]->Prop->Asn == 'H' || 
        r[1]->Prop->Asn == 'G' || r[2]->Prop->Asn == 'G' || 
        r[1]->Prop->Asn == 'I' || r[2]->Prop->Asn == 'G' || 
       !stride_FindAtom(Chain[Cn],i,"CA",&CA1) || !stride_FindAtom(Chain[Cn],i+3,"CA",&CA4) ||
       Dist(r[0]->Coord[CA1],r[3]->Coord[CA4]) > 7.0 )
      continue;
    
    Phi2 = r[1]->Prop->Phi;
    Psi2 = r[1]->Prop->Psi;
    Phi3 = r[2]->Prop->Phi;
    Psi3 = r[2]->Prop->Psi;
    
    if( TurnCondition(Phi2,-60.0,Psi2,-30,Phi3,-90.0,Psi3,0,Range1,Range2) )
      TurnType = '1';
    else
    if( TurnCondition(Phi2,60.0,Psi2,30,Phi3,90.0,Psi3,0,Range1,Range2) )
      TurnType = '2';
    else
    if( TurnCondition(Phi2,-60.0,Psi2,120,Phi3,80.0,Psi3,0,Range1,Range2) )
      TurnType = '3';
    else
    if( TurnCondition(Phi2,60.0,Psi2,-120,Phi3,-80.0,Psi3,0,Range1,Range2) )
      TurnType = '4';
    else
    if( TurnCondition(Phi2,-60.0,Psi2,120,Phi3,-90.0,Psi3,0,Range1,Range2) )
      TurnType = '5';
    else
    if( TurnCondition(Phi2,-120.0,Psi2,120,Phi3,-60.0,Psi3,0,Range1,Range2) )
      TurnType = '6';
    else
    if( TurnCondition(Phi2,-60.0,Psi2,-30,Phi3,-120.0,Psi3,120,Range1,Range2) )
      TurnType = '7';
    else
      TurnType = '8';
  
    if( r[0]->Prop->Asn == 'C' ) 
      r[0]->Prop->Asn = 'T';
    
    if( r[1]->Prop->Asn == 'C' )
      r[1]->Prop->Asn = 'T';
    
    if( r[2]->Prop->Asn == 'C' )
      r[2]->Prop->Asn = 'T';
    
    if( r[3]->Prop->Asn == 'C' )
      r[3]->Prop->Asn = 'T';
    
    Tn = Chain[Cn]->NAssignedTurn;
    Chain[Cn]->AssignedTurn[Tn] = (TURN *)ckalloc(sizeof(TURN));
    t = Chain[Cn]->AssignedTurn[Tn];
    strcpy(t->Res1,r[0]->ResType);
    strcpy(t->Res2,r[3]->ResType);
    strcpy(t->PDB_ResNumb1,r[0]->PDB_ResNumb);
    strcpy(t->PDB_ResNumb2,r[3]->PDB_ResNumb);
    t->TurnType = TurnType;
    Chain[Cn]->NAssignedTurn++;

  }
}


void GammaTurn(CHAIN **Chain, int Cn, HBOND **HBond)
{

  register int i;
  RESIDUE **r;
  TURN *t;
  int Tn;
  float Phi2, Psi2;
  char TurnType, Asn;

  for( i=0; i<Chain[Cn]->NRes-2; i++ ) {

    r = &Chain[Cn]->Rsd[i-1];

    Asn = r[2]->Prop->Asn;

    if( Asn == 'H' || Asn == 'T' || Asn == 'G' || Asn == 'I' ||
        FindBnd(HBond,r[3],r[1]) == ERR ||
        (i > 0 && FindBnd(HBond,r[3],r[0]) != ERR) || 
        (i < Chain[Cn]->NRes-3 && FindBnd(HBond,r[4],r[1]) != ERR) )
      continue;
    
    Phi2 = r[2]->Prop->Phi;
    Psi2 = r[2]->Prop->Psi;
    
    if( Phi2 > 0.0 && Psi2 < 0.0 )
      TurnType = '@';
    else
    if( Phi2 < 0.0 && Psi2 > 0.0 )
      TurnType = '&';
    else 
      continue;

    if( r[1]->Prop->Asn == 'C' )
      r[1]->Prop->Asn = 'T';
    
    if( r[2]->Prop->Asn == 'C' )
      r[2]->Prop->Asn = 'T';
    
    if( r[3]->Prop->Asn == 'C' )
      r[3]->Prop->Asn = 'T';
    
    Tn = Chain[Cn]->NAssignedTurn;
    Chain[Cn]->AssignedTurn[Tn] = (TURN *)ckalloc(sizeof(TURN));
    t = Chain[Cn]->AssignedTurn[Tn];
    strcpy(t->Res1,r[1]->ResType);
    strcpy(t->Res2,r[3]->ResType);
    strcpy(t->PDB_ResNumb1,r[1]->PDB_ResNumb);
    strcpy(t->PDB_ResNumb2,r[3]->PDB_ResNumb);
    t->TurnType = TurnType;
    Chain[Cn]->NAssignedTurn++;
  }
}


int TurnCondition(float Phi2,float Phi2S,float Psi2,float Psi2S,
		  float Phi3,float Phi3S,float Psi3,float Psi3S,
		  float Range1,float Range2)
{
  if((IN(Phi2,Phi2S,Range2)==YES && IN(Psi2,Psi2S,Range1)==YES && 
      IN(Phi3,Phi3S,Range1)==YES && IN(Psi3,Psi3S,Range1)==YES)
     ||
     (IN(Phi2,Phi2S,Range1)==YES && IN(Psi2,Psi2S,Range2)==YES && 
      IN(Phi3,Phi3S,Range1)==YES && IN(Psi3,Psi3S,Range1)==YES)
     ||
     (IN(Phi2,Phi2S,Range1)==YES && IN(Psi2,Psi2S,Range1)==YES && 
      IN(Phi3,Phi3S,Range2)==YES && IN(Psi3,Psi3S,Range1)==YES)
     ||
     (IN(Phi2,Phi2S,Range1)==YES && IN(Psi2,Psi2S,Range1)==YES && 
      IN(Phi3,Phi3S,Range1)==YES && IN(Psi3,Psi3S,Range2)==YES)
     )
    return(SUCCESS);
  
  return(FAILURE);
}
    


