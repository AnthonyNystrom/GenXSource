#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2005 Dr. Dirk Lellinger
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////
#endregion

/*
 * Unghr.cs
 * 
 * Copyright (c) 2003-2004, dnAnalytics. All rights reserved.
*/

#if !MANAGED
using System;
using System.Runtime.InteropServices;



namespace Altaxo.Calc.LinearAlgebra.Lapack{
  [System.Security.SuppressUnmanagedCodeSecurityAttribute]
  internal sealed class Unghr{
    private Unghr(){}
    
    private static void ArgumentCheck(int n, int ilo, int ihi, object A, int lda, object tau){
      if (n<0) 
        throw new ArgumentException("n must be at least zero.", "n");
      if (n>0) {
        if (ilo<1 || ilo>n || ilo>ihi)
          throw new ArgumentException("ilo must be between 1 and min(ihi,n) if n>0");
        if (ihi<1 || ihi>n)
          throw new ArgumentException("ihi must be between 1 and n if n>0");
      } else {
        if (ilo!=1)
          throw new ArgumentException("ilo must be 1 if n=0", "ilo");
        if (ihi!=0)
          throw new ArgumentException("ihi must be 0 if n=0", "ihi");
      }
      if (A==null) 
        throw new ArgumentNullException("A","A cannot be null.");
      if ( tau == null )
        throw new ArgumentNullException("tau","tau cannot be null.");
      if (lda<n || lda<1) 
        throw new ArgumentException("lda must be at least max(1,n)", "lda");
    }
    
    internal static int Compute( int n, int ilo, int ihi, ComplexFloat[] A, int lda, ComplexFloat[] tau ){
      ArgumentCheck(n, ilo, ihi, A, lda, tau);
      return dna_lapack_cunghr(Configuration.BlockSize, n, ilo, ihi, A, lda, tau);
    }

    internal static int Compute( int n, int ilo, int ihi, Complex[] A, int lda, Complex[] tau ){
      ArgumentCheck(n, ilo, ihi, A, lda, tau);
      return dna_lapack_zunghr(Configuration.BlockSize, n, ilo, ihi, A, lda, tau);
    }

    [DllImport(Configuration.BLASLibrary, ExactSpelling=true, SetLastError=false)]
    private static extern int dna_lapack_cunghr( int block_size, int n, int ilo, int ihi, ComplexFloat[] A, int lda, ComplexFloat[] tau);

    [DllImport(Configuration.BLASLibrary, ExactSpelling=true, SetLastError=false)]
    private static extern int dna_lapack_zunghr( int block_size, int n, int ilo, int ihi, Complex[] A, int lda, Complex[] tau);

  }
}
#endif