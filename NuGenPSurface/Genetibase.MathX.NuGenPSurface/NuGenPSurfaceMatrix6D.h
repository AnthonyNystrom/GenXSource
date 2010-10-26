/***************************************************************************
 *   Copyright (C) 2005 by Abderrahman Taha                                *
 *   taha_ab@yahoo.fr                                                      *
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
#using  <system.dll>

using namespace System;

namespace NuGenPSurfaceMatrices {

public ref class Matrix6D {
public:
    double xx, xy, xz, xw, xt, xs, xo;
    double yx, yy, yz, yw, yt, ys, yo;
    double zx, zy, zz, zw, zt, zs, zo;
    double wx, wy, wz, ww, wt, ws, wo;
    double tx, ty, tz, tw, tt, ts, to;
    double sx, sy, sz, sw, st, ss, so;
    double kx, ky, kz, kw, kt, ks, ko;
    
    double pi;
    public:
    Matrix6D();
//    void scale(double );
//    void scale(double, double, double, double, double);
//    void translate(double, double, double, double, double);
    
    void xzrot(double);
    void yzrot(double);
    void xyrot(double);
    
    void xwrot(double);
    void ywrot(double);
    void zwrot(double);
    
    void xtrot(double);
    void ytrot(double);
    void ztrot(double);
    void wtrot(double);
    
    void xsrot(double);
    void ysrot(double);
    void zsrot(double);
    void wsrot(double);
    void tsrot(double);    
    
    void mult(Matrix6D^);
    void unit();
};
}
