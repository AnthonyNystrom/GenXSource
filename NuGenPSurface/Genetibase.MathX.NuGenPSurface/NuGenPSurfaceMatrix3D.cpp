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

#include "NuGenPSurfaceMatrix3D.h" 

namespace NuGenPSurfaceMatrices 
{
/** Create a new unit matrix */
Matrix3D::Matrix3D() 
{

    pi = Math::PI;

    xx = 1.0f;
    yy = 1.0f;
    zz = 1.0f;
    wo = 1.0f;
    xy= xz =xo =yx =yz= yo =zx = zy = wx= wy= wz= 0;

}
/*********** PRODUIT  VECTORIEL  **************/

void Matrix3D::CalcProdVect(array<double>^ v1, array<double>^ v2, array<double>^ v)
{
    v[1]=v1[2]*v2[3]-v2[2]*v1[3];
    v[2]=v2[1]*v1[3]-v2[3]*v1[1];
    v[3]=v1[1]*v2[2]-v2[1]*v1[2];

}



/********* Produit scalaire *******************/
double  Matrix3D::prodscalaire( array<double>^ v1, array<double>^  v2)
{
    return (v1[1]*v2[1]+v1[2]*v2[2]+v1[3]*v2[3]);
} 
}
