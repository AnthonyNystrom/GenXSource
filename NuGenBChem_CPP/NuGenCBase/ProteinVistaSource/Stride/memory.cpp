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
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <errno.h>
#include <stdarg.h>
#include <ctype.h>

void *ckalloc(size_t bytes)
{
  void *ret;
  void die(char *format, ... );
  
  if( !(ret = malloc(bytes)) ) die("Out of  memory\n");

  return ret;	
}

float **FloatMatrix(int M, int N)
{
  int m;
  float **Matrix;

  Matrix = (float **)ckalloc(M*sizeof(float *));

  for( m=0; m<M; m++ ) Matrix[m] = (float *)ckalloc(N*sizeof(float));

  return(Matrix);
}

float ***FloatCube(int M, int N, int K)
{
  int m, n, k;
  float ***Cube;

  Cube = (float ***)ckalloc(M*sizeof(float **));

  for( m=0; m<M; m++ ) {
    Cube[m] = (float **)ckalloc(N*sizeof(float *));
    for( n=0; n<N; n++ )
      Cube[m][n] = (float *)ckalloc(K*sizeof(float));
  }

  for( m=0; m<M; m++ )
    for( n=0; n<N; n++ )
      for( k=0; k<K; k++ )
	Cube[m][n][k] = 0.0;

  return(Cube);
}

float ****Float4Dim(int M, int N, int K, int L)
{
  int m, n, k, l;
  float ****FourDim;

  FourDim = (float ****)ckalloc(M*sizeof(float ***));


  for( m=0; m<M; m++ ) {
    FourDim[m] = (float ***)ckalloc(N*sizeof(float **));
    for( n=0; n<N; n++ ) {
      FourDim[m][n] = (float **)ckalloc(K*sizeof(float*));
      for( k=0; k<K; k++ )
	FourDim[m][n][k] = (float *)ckalloc(L*sizeof(float));
    }
  }

  for( m=0; m<M; m++ )
    for( n=0; n<N; n++ )
      for( k=0; k<K; k++ )
	for( l=0; l<L; l++ )
	  FourDim[m][n][k][l] = 0.0;

  return(FourDim);
}

void FreeFloatMatrix(float **Matrix, int M)
{
  int m;

  for( m=0; m<M; m++ ) free(Matrix[m]);

  free(Matrix);

}

int ***IntCube(int M, int N, int K)
{
  int m, n, k;
  int ***Cube;

  Cube = (int ***)ckalloc(M*sizeof(int **));

  for( m=0; m<M; m++ ) {
    Cube[m] = (int **)ckalloc(N*sizeof(int *));
    for( n=0; n<N; n++ ) Cube[m][n] = (int *)ckalloc(K*sizeof(int));
  }
  
  for( m=0; m<M; m++ )
    for( n=0; n<N; n++ )
      for( k=0; k<K; k++ )
	Cube[m][n][k] = 0;

  return(Cube);
}


int **IntMatrix(int M, int N)
{
  int m;
  int **Matrix;

  Matrix = (int **)ckalloc(M*sizeof(int *));

  for( m=0; m<M; m++ ) Matrix[m] = (int *)ckalloc(N*sizeof(int));

  return(Matrix);
}

int ****Int4Dim(int M, int N, int K, int L)
{
  int m, n, k, l;
  int ****FourDim;

  FourDim = (int ****)ckalloc(M*sizeof(int ***));


  for( m=0; m<M; m++ ) {
    FourDim[m] = (int ***)ckalloc(N*sizeof(int **));
    for( n=0; n<N; n++ ) {
      FourDim[m][n] = (int **)ckalloc(K*sizeof(int*));
      for( k=0; k<K; k++ )
	FourDim[m][n][k] = (int *)ckalloc(L*sizeof(int));
    }
  }

  for( m=0; m<M; m++ )
    for( n=0; n<N; n++ )
      for( k=0; k<K; k++ )
	for( l=0; l<L; l++ )
	  FourDim[m][n][k][l] = 0;

  return(FourDim);
}

void FreeIntMatrix(int **Matrix, int M)
{
  int m;

  for( m=0; m<M; m++ ) free(Matrix[m]);

  free(Matrix);

}

char **CharMatrix(int M, int N)
{
  int m;
  char **Matrix;

  Matrix = (char **)ckalloc(M*sizeof(char *));

  for( m=0; m<M; m++ ) Matrix[m] = (char *)ckalloc(N*sizeof(char));

  return(Matrix);
}

void FreeCharMatrix(char **Matrix, int M)
{
  int m;

  for( m=0; m<M; m++ ) free(Matrix[m]);

  free(Matrix);

}

void FreeIntCube(int ***Cube, int M, int N)
{
  int m, n;

  for( m=0; m<M; m++ ) {
    for( n=0; n<N; n++ )
      free(Cube[m][n]);
    free(Cube[m]);
  }

  free(Cube);
}

void FreeFloatCube(float ***Cube, int M, int N)
{
  int m, n;

  for( m=0; m<M; m++ ) {
    for( n=0; n<N; n++ )
      free(Cube[m][n]);
    free(Cube[m]);
  }

  free(Cube);
}


