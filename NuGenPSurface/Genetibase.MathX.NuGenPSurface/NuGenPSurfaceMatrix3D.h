/***************************************************************************
*   Copyright (C) 2005 by Abderrahman Taha                                *
*                                                                         *
*                                                                         *
*   This program is free software; you can redistribute it and/or modify  *
*   it under the terms of the GNU General Public License as published by  *
*   the Free Software Foundation; either version 2 of the License, or     *
*   (at your option) any later version.                                   *
*                                                                         *
*   This program is distributed in the hope that it will be useful,       *
*   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
*   GNU General Public License for more details.                          *
*                                                                         *
*   You should have received a copy of the GNU General Public License     *
*   along with this program; if not, write to the                         *
*   Free Software Foundation, Inc.,                                       *
*   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
***************************************************************************/
#using <system.dll>

using namespace System;

namespace NuGenPSurfaceMatrices
{
    public ref class Matrix3D 
    {
    public:
        double xx, xy, xz, xo;
        double yx, yy, yz, yo;
        double zx, zy, zz, zo;
        double wx, wy, wz, wo;
        double pi;
    public:
        Matrix3D();
        inline  void scale(double );
        inline  void scale(double, double, double);
        inline  void translate(double, double, double);
        inline  void yrot(double);
        inline  void xrot(double);
        inline  void zrot(double);
        inline  void mult(Matrix3D^ rhs);
        inline  void unit();
        void transform(array<double>^, array<double>^, int);
        void CalcProdVect(array<double>^, array<double>^, array<double>^);
        double  prodscalaire(array<double>^, array<double>^);
    };

    inline void Matrix3D::mult(Matrix3D^ rhs)
    {
        double lxx = xx * rhs->xx + yx * rhs->xy + zx * rhs->xz;
        double lxy = xy * rhs->xx + yy * rhs->xy + zy * rhs->xz;
        double lxz = xz * rhs->xx + yz * rhs->xy + zz * rhs->xz;
        double lxo = xo * rhs->xx + yo * rhs->xy + zo * rhs->xz + rhs->xo;

        double lyx = xx * rhs->yx + yx * rhs->yy + zx * rhs->yz;
        double lyy = xy * rhs->yx + yy * rhs->yy + zy * rhs->yz;
        double lyz = xz * rhs->yx + yz * rhs->yy + zz * rhs->yz;
        double lyo = xo * rhs->yx + yo * rhs->yy + zo * rhs->yz + rhs->yo;

        double lzx = xx * rhs->zx + yx * rhs->zy + zx * rhs->zz;
        double lzy = xy * rhs->zx + yy * rhs->zy + zy * rhs->zz;
        double lzz = xz * rhs->zx + yz * rhs->zy + zz * rhs->zz;
        double lzo = xo * rhs->zx + yo * rhs->zy + zo * rhs->zz + rhs->zo;

        xx = lxx;
        xy = lxy;
        xz = lxz;
        xo = lxo;

        yx = lyx;
        yy = lyy;
        yz = lyz;
        yo = lyo;

        zx = lzx;
        zy = lzy;
        zz = lzz;
        zo = lzo;
    }

    /** Scale by f in all dimensions */
    inline void Matrix3D::scale(double f) {
        xx *= f;
        xy *= f;
        xz *= f;
        xo *= f;
        yx *= f;
        yy *= f;
        yz *= f;
        yo *= f;
        zx *= f;
        zy *= f;
        zz *= f;
        zo *= f;
    }
    /** Scale along each axis independently */
    inline void Matrix3D::scale(double xf, double yf, double zf) {
        xx *= xf;
        xy *= xf;
        xz *= xf;
        xo *= xf;
        yx *= yf;
        yy *= yf;
        yz *= yf;
        yo *= yf;
        zx *= zf;
        zy *= zf;
        zz *= zf;
        zo *= zf;
    }
    /** Translate the origin */
    inline void Matrix3D::translate(double x, double y, double z) {
        xo += x;
        yo += y;
        zo += z;
    }
    /** rotate theta degrees about the y axis */
    inline void Matrix3D::yrot(double theta) {
        theta *= (pi / 180);
        double ct = Math::Cos(theta);
        double st = Math::Sin(theta);

        double Nxx = (double) (xx * ct + zx * st);
        double Nxy = (double) (xy * ct + zy * st);
        double Nxz = (double) (xz * ct + zz * st);
        double Nxo = (double) (xo * ct + zo * st);

        double Nzx = (double) (zx * ct - xx * st);
        double Nzy = (double) (zy * ct - xy * st);
        double Nzz = (double) (zz * ct - xz * st);
        double Nzo = (double) (zo * ct - xo * st);

        xo = Nxo;
        xx = Nxx;
        xy = Nxy;
        xz = Nxz;
        zo = Nzo;
        zx = Nzx;
        zy = Nzy;
        zz = Nzz;
    }
    /** rotate theta degrees about the x axis */
    inline void Matrix3D::xrot(double theta) {
        theta *= (pi / 180);
        double ct = Math::Cos(theta);
        double st = Math::Sin(theta);

        double Nyx = (double) (yx * ct + zx * st);
        double Nyy = (double) (yy * ct + zy * st);
        double Nyz = (double) (yz * ct + zz * st);
        double Nyo = (double) (yo * ct + zo * st);

        double Nzx = (double) (zx * ct - yx * st);
        double Nzy = (double) (zy * ct - yy * st);
        double Nzz = (double) (zz * ct - yz * st);
        double Nzo = (double) (zo * ct - yo * st);

        yo = Nyo;
        yx = Nyx;
        yy = Nyy;
        yz = Nyz;
        zo = Nzo;
        zx = Nzx;
        zy = Nzy;
        zz = Nzz;
    }
    /** rotate theta degrees about the z axis */
    inline void Matrix3D::zrot(double theta) {
        theta *= (pi / 180);
        double ct = Math::Cos(theta);
        double st = Math::Sin(theta);

        double Nyx = (double) (yx * ct + xx * st);
        double Nyy = (double) (yy * ct + xy * st);
        double Nyz = (double) (yz * ct + xz * st);
        double Nyo = (double) (yo * ct + xo * st);

        double Nxx = (double) (xx * ct - yx * st);
        double Nxy = (double) (xy * ct - yy * st);
        double Nxz = (double) (xz * ct - yz * st);
        double Nxo = (double) (xo * ct - yo * st);

        yo = Nyo;
        yx = Nyx;
        yy = Nyy;
        yz = Nyz;
        xo = Nxo;
        xx = Nxx;
        xy = Nxy;
        xz = Nxz;
    }
    /** Multiply this matrix by a second: M = M*R */


    /** Reinitialize to the unit matrix */
    inline void Matrix3D::unit() {
        xo = 0;
        xx = 1;
        xy = 0;
        xz = 0;
        yo = 0;
        yx = 0;
        yy = 1;
        yz = 0;
        zo = 0;
        zx = 0;
        zy = 0;
        zz = 1;
        wx = 0;
        wy = 0;
        wz = 0;
        wo = 1;
    }


    inline  void Matrix3D::transform(array<double>^ v, array<double>^ tv, int nvert) {
        double lxx = xx, lxy = xy, lxz = xz, lxo = xo;
        double lyx = yx, lyy = yy, lyz = yz, lyo = yo;
        double lzx = zx, lzy = zy, lzz = zz, lzo = zo;
        for (int i = nvert * 3; (i -= 3) >= 0;) {
            double x = v[i];
            double y = v[i + 1];
            double z = v[i + 2];
            tv[i    ] = (x * lxx + y * lxy + z * lxz + lxo);
            tv[i + 1] = (x * lyx + y * lyy + z * lyz + lyo);
            tv[i + 2] = (x * lzx + y * lzy + z * lzz + lzo);
        }
    }

} 


