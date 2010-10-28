using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenMatrix
{
    /// <summary>
    /// NuGenMatrix provides basic operations with matrices
    /// </summary>
    public class NuGenMatrix
    {

        public decimal[,] Data;
        public NuGenMatrix(decimal[,] data)
        {

            if (data == null)
            {
                throw new System.ArgumentNullException("Cannot create the matrix!");
            }
            Data = data;
        }

        public NuGenMatrix(int rowsCount, int colsCount, decimal[] data)
        {

            if (data == null)
            {
                throw new System.ArgumentNullException("Cannot create the matrix!");
            }
            int r = data.GetLength(0);
            int c = data.GetLength(1);
            decimal[,] dataM = new decimal[r, c];

            for (int i = 0; i < r; i++)
            {

                for (int j = 0; j < c; j++)
                {
                    dataM[i, j] = data[i * c + j];
                }
            }
            Data = dataM;
        }

        public static NuGenMatrix operator +(NuGenMatrix M1, NuGenMatrix M2)
        {
            int r1 = M1.Data.GetLength(0); int r2 = M2.Data.GetLength(0);
            int c1 = M1.Data.GetLength(1); int c2 = M2.Data.GetLength(1);

            if ((r1 != r2) || (c1 != c2))
            {
                throw new System.ArgumentException("Matrices dimensions are not equal!");
            }
            decimal[,] res = new decimal[r1, c1];

            for (int i = 0; i < r1; i++)
            {

                for (int j = 0; j < c1; j++)
                {
                    res[i, j] = M1.Data[i, j] + M2.Data[i, j];
                }
            }
            return new NuGenMatrix(res);
        }

        public static NuGenMatrix operator -(NuGenMatrix M1, NuGenMatrix M2)
        {
            int r1 = M1.Data.GetLength(0); int r2 = M2.Data.GetLength(0);
            int c1 = M1.Data.GetLength(1); int c2 = M2.Data.GetLength(1);

            if ((r1 != r2) || (c1 != c2))
            {
                throw new System.ArgumentException("Matrices dimensions are not equal!");
            }
            decimal[,] res = new decimal[r1, c1];

            for (int i = 0; i < r1; i++)
            {

                for (int j = 0; j < c1; j++)
                {
                    res[i, j] = M1.Data[i, j] - M2.Data[i, j];
                }
            }
            return new NuGenMatrix(res);
        }

        public static NuGenMatrix operator *(NuGenMatrix M1, NuGenMatrix M2)
        {
            int r1 = M1.Data.GetLength(0); int r2 = M2.Data.GetLength(0);
            int c1 = M1.Data.GetLength(1); int c2 = M2.Data.GetLength(1);

            if (c1 != r2)
            {
                throw new System.ArgumentException("Matrices dimensions do not correspond!");
            }
            decimal[,] res = new decimal[r1, c2];

            for (int i = 0; i < r1; i++)
            {

                for (int j = 0; j < c2; j++)
                {

                    for (int k = 0; k < r2; k++)
                    {
                        res[i, j] = res[i, j] + (M1.Data[i, k] * M2.Data[k, j]);
                    }
                }
            }
            return new NuGenMatrix(res);
        }

        public static NuGenMatrix operator /(decimal D, NuGenMatrix M)
        {
            return ScaleMultiply(D, NuGenMatrix.INV(M));
        }

        public static bool operator ==(NuGenMatrix M1, NuGenMatrix M2)
        {
            bool B = true;
            int r1 = M1.Data.GetLength(0); int r2 = M2.Data.GetLength(0);
            int c1 = M1.Data.GetLength(1); int c2 = M2.Data.GetLength(1);

            if ((r1 != r2) || (c1 != c2))
            {
                return false;
            }

            else
            {

                for (int i = 0; i < r1; i++)
                {

                    for (int j = 0; j < c1; j++)
                    {

                        if (M1.Data[i, j] != M2.Data[i, j])
                            B = false;
                    }
                }
            }
            return B;
        }

        public static bool operator !=(NuGenMatrix M1, NuGenMatrix M2)
        {
            return !(M1 == M2);
        }

        public override bool Equals(object obj)
        {

            if (!(obj is NuGenMatrix))
            {
                return false;
            }
            return this == (NuGenMatrix)obj;
        }

        /*

        public void display()
        {
            int r1 = this.Data.GetLength(0);int c1 = this.Data.GetLength(1);

            for (int i=0;i<r1;i++)
            {

                for (int j=0;j<c1;j++)
                {
                    Console.Write(this.Data[i,j].ToString("N2")+"   " );				
                }
                Console.WriteLine(); 
            }
            Console.WriteLine(); 
        }
        */

        public static NuGenMatrix INV(NuGenMatrix M)
        {
            decimal[,] a = M.Data;
            int ro = a.GetLength(0);
            int co = a.GetLength(1);

            if (ro != co)
            {
                throw new System.ArgumentException("Cannot find inverse for an non square matrix!");
            }

            int q; decimal[,] b = new decimal[ro, co]; decimal[,] I = Identity(ro).Data;

            for (int p = 0; p < ro; p++) { for (q = 0; q < co; q++) { b[p, q] = a[p, q]; } }
            int i; decimal det = 1;

            if (a[0, 0] == 0)
            {
                i = 1;

                while (i < ro)
                {

                    if (a[i, 0] != 0)
                    {
                        NuGenMatrix.interrow(a, 0, i);
                        NuGenMatrix.interrow(I, 0, i);
                        det *= -1;
                        break;
                    }
                    i++;
                }
            }
            det *= a[0, 0];
            NuGenMatrix.rowdiv(I, 0, a[0, 0]);
            NuGenMatrix.rowdiv(a, 0, a[0, 0]);

            for (int p = 1; p < ro; p++)
            {
                q = 0;

                while (q < p)
                {
                    NuGenMatrix.rowsub(I, p, q, a[p, q]);
                    NuGenMatrix.rowsub(a, p, q, a[p, q]);
                    q++;
                }

                if (a[p, p] != 0)
                {
                    det *= a[p, p];
                    NuGenMatrix.rowdiv(I, p, a[p, p]);
                    NuGenMatrix.rowdiv(a, p, a[p, p]);
                }

                if (a[p, p] == 0)
                {

                    for (int j = p + 1; j < co; j++)
                    {

                        if (a[p, j] != 0)
                        {
                            throw new System.Exception("Unable to determine the Inverse!");
                        }
                    }

                }
            }

            for (int p = ro - 1; p > 0; p--)
            {

                for (q = p - 1; q >= 0; q--)
                {
                    NuGenMatrix.rowsub(I, q, p, a[q, p]);
                    NuGenMatrix.rowsub(a, q, p, a[q, p]);
                }
            }

            for (int p = 0; p < ro; p++)
            {

                for (q = 0; q < co; q++)
                {
                    a[p, q] = b[p, q];
                }
            }

            return (new NuGenMatrix(I));
        }
        static void rowdiv(decimal[,] a, int r, decimal s)
        {
            int co = a.GetLength(1);

            for (int q = 0; q < co; q++)
            {
                a[r, q] = a[r, q] / s;
            }
        }
        static void rowsub(decimal[,] a, int i, int j, decimal s)
        {
            int co = a.GetLength(1);

            for (int q = 0; q < co; q++)
            {
                a[i, q] = a[i, q] - (s * a[j, q]);
            }
        }
        static decimal[,] interrow(decimal[,] a, int i, int j)
        {
            int ro = a.GetLength(0);
            int co = a.GetLength(1);
            decimal temp = 0;

            for (int q = 0; q < co; q++)
            {
                temp = a[i, q];
                a[i, q] = a[j, q];
                a[j, q] = temp;
            }
            return (a);
        }

        public static NuGenMatrix RowSwitch(NuGenMatrix M, int i, int j)
        {
            NuGenMatrix MS = NuGenMatrix.DeepCopy(M);
            decimal[,] a = MS.Data;
            int ro = a.GetLength(0);
            int co = a.GetLength(1);
            decimal temp = 0;

            for (int q = 0; q < co; q++)
            {
                temp = a[i, q];
                a[i, q] = a[j, q];
                a[j, q] = temp;
            }
            return (new NuGenMatrix(a));
        }

        public static NuGenMatrix Identity(int n)
        {
            return Diagonal(n, 1);
        }

        public static NuGenMatrix Diagonal(int n, decimal D)
        {
            decimal[,] a = new decimal[n, n];

            for (int p = 0; p < n; p++)
            {

                for (int q = 0; q < n; q++)
                {

                    if (p == q)
                    {
                        a[p, q] = D;
                    }

                    else
                    {
                        a[p, q] = 0;
                    }
                }
            }
            return (new NuGenMatrix(a));
        }

        public static NuGenMatrix ScaleMultiply(decimal scalar, NuGenMatrix M)
        {
            decimal[,] A = M.Data;
            int ro = A.GetLength(0);
            int co = A.GetLength(1);
            decimal[,] B = new decimal[ro, co];

            for (int p = 0; p < ro; p++)
            {

                for (int q = 0; q < co; q++)
                {
                    B[p, q] = scalar * A[p, q];
                }
            }
            return (new NuGenMatrix(B));
        }

        public static NuGenMatrix DeepCopy(NuGenMatrix M)
        {
            return ScaleMultiply(1, M);
        }

    }
}
