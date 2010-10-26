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

#include "NuGenPSurfaceMatrix5D.h" 

namespace NuGenPSurfaceMatrices {
/** Create a new unit matrix */
Matrix5D::Matrix5D() {
    
    pi = Math::PI;
	xx = 1.0f;
	yy = 1.0f;
	zz = 1.0f;
	ww = 1.0f;
	tt = 1.0f;
	ko = 1.0f;
	xy = xz = xw = xt = xo = 0;
	yx = yz = yw = yt = yo = 0;
	zx = zy = zw = zt = zo = 0;
	wx = wy = wz = wt = wo = 0;
	tx = ty = tz = tw = to = 0;
	kx = ky = kz = kw = kt = 0;
}


void Matrix5D::mult(Matrix5D^ rhs) {
	double lxx = xx * rhs->xx + yx * rhs->xy + zx * rhs->xz + wx * rhs->xw + tx * rhs->xt;
	double lxy = xy * rhs->xx + yy * rhs->xy + zy * rhs->xz + wy * rhs->xw + ty * rhs->xt;
	double lxz = xz * rhs->xx + yz * rhs->xy + zz * rhs->xz + wz * rhs->xw + tz * rhs->xt;
	double lxw = xw * rhs->xx + yw * rhs->xy + zw * rhs->xz + ww * rhs->xw + tw * rhs->xt;
	double lxt = xt * rhs->xx + yt * rhs->xy + zt * rhs->xz + wt * rhs->xw + tt * rhs->xt;
	double lxo = xo * rhs->xx + yo * rhs->xy + zo * rhs->xz + wo * rhs->xw + to * rhs->xt + rhs->xo;

	double lyx = xx * rhs->yx + yx * rhs->yy + zx * rhs->yz + wx * rhs->yw + tx * rhs->yt;
	double lyy = xy * rhs->yx + yy * rhs->yy + zy * rhs->yz + wy * rhs->yw + ty * rhs->yt;
	double lyz = xz * rhs->yx + yz * rhs->yy + zz * rhs->yz + wz * rhs->yw + tz * rhs->yt;
	double lyw = xw * rhs->yx + yw * rhs->yy + zw * rhs->yz + ww * rhs->yw + tw * rhs->yt;
	double lyt = xt * rhs->yx + yt * rhs->yy + zt * rhs->yz + wt * rhs->yw + tt * rhs->yt;
	double lyo = xo * rhs->yx + yo * rhs->yy + zo * rhs->yz + wo * rhs->yw + to * rhs->yt + rhs->yo;

	double lzx = xx * rhs->zx + yx * rhs->zy + zx * rhs->zz + wx * rhs->zw + tx * rhs->zt;
	double lzy = xy * rhs->zx + yy * rhs->zy + zy * rhs->zz + wy * rhs->zw + ty * rhs->zt;
	double lzz = xz * rhs->zx + yz * rhs->zy + zz * rhs->zz + wz * rhs->zw + tz * rhs->zt;
	double lzw = xw * rhs->zx + yw * rhs->zy + zw * rhs->zz + ww * rhs->zw + tw * rhs->zt;
	double lzt = xt * rhs->zx + yt * rhs->zy + zt * rhs->zz + wt * rhs->zw + tt * rhs->zt;
	double lzo = xo * rhs->zx + yo * rhs->zy + zo * rhs->zz + wo * rhs->zw + to * rhs->zt + rhs->zo;
	
	double lwx = xx * rhs->wx + yx * rhs->wy + zx * rhs->wz + wx * rhs->ww + tx * rhs->wt;
	double lwy = xy * rhs->wx + yy * rhs->wy + zy * rhs->wz + wy * rhs->ww + ty * rhs->wt;
	double lwz = xz * rhs->wx + yz * rhs->wy + zz * rhs->wz + wz * rhs->ww + tz * rhs->wt;
	double lww = xw * rhs->wx + yw * rhs->wy + zw * rhs->wz + ww * rhs->ww + tw * rhs->wt;
	double lwt = xt * rhs->wx + yt * rhs->wy + zt * rhs->wz + wt * rhs->ww + tt * rhs->wt;
	double lwo = xo * rhs->wx + yo * rhs->wy + zo * rhs->wz + wo * rhs->ww + to * rhs->wt + rhs->wo;
	
	double ltx = xx * rhs->tx + yx * rhs->ty + zx * rhs->tz + wx * rhs->tw + tx * rhs->tt;
	double lty = xy * rhs->tx + yy * rhs->ty + zy * rhs->tz + wy * rhs->tw + ty * rhs->tt;
	double ltz = xz * rhs->tx + yz * rhs->ty + zz * rhs->tz + wz * rhs->tw + tz * rhs->tt;
	double ltw = xw * rhs->tx + yw * rhs->ty + zw * rhs->tz + ww * rhs->tw + tw * rhs->tt;
	double ltt = xt * rhs->tx + yt * rhs->ty + zt * rhs->tz + wt * rhs->tw + tt * rhs->tt;
	double lto = xo * rhs->tx + yo * rhs->ty + zo * rhs->tz + wo * rhs->tw + to * rhs->tt + rhs->to;		

	xx = lxx;
	xy = lxy;
	xz = lxz;
	xw = lxw;
	xt = lxt;
	xo = lxo;

	yx = lyx;
	yy = lyy;
	yz = lyz;
	yw = lyw;
	yt = lyt;
	yo = lyo;

	zx = lzx;
	zy = lzy;
	zz = lzz;
	zw = lzw;
	zt = lzt;
	zo = lzo;

	wx = lwx;
	wy = lwy;
	wz = lwz;
	ww = lww;
	wt = lwt;
	wo = lwo;
	
	tx = ltx;
	ty = lty;
	tz = ltz;
	tw = ltw;
	tt = ltt;
	to = lto;	
	
	
		
}

/** rotate theta degrees about the yz plan */
void Matrix5D::yzrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Nyx = (double) (yx * ct + zx * st);
	double Nyy = (double) (yy * ct + zy * st);
	double Nyz = (double) (yz * ct + zz * st);
	double Nyw = (double) (yw * ct + zw * st);
	double Nyt = (double) (yt * ct + zt * st);
	double Nyo = (double) (yo * ct + zo * st);

	double Nzx = (double) (zx * ct - yx * st);
	double Nzy = (double) (zy * ct - yy * st);
	double Nzz = (double) (zz * ct - yz * st);
	double Nzw = (double) (zw * ct - yw * st);
	double Nzt = (double) (zt * ct - yt * st);
	double Nzo = (double) (zo * ct - yo * st);
	

	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
	
	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
}
        
/** rotate theta degrees about the xz plan */
void Matrix5D::xzrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Nxx = (double) (xx * ct + zx * st);
	double Nxy = (double) (xy * ct + zy * st);
	double Nxz = (double) (xz * ct + zz * st);
	double Nxw = (double) (xw * ct + zw * st);
	double Nxt = (double) (xt * ct + zt * st);
	double Nxo = (double) (xo * ct + zo * st);

	double Nzx = (double) (zx * ct - xx * st);
	double Nzy = (double) (zy * ct - xy * st);
	double Nzz = (double) (zz * ct - xz * st);
	double Nzw = (double) (zw * ct - xw * st);
	double Nzt = (double) (zt * ct - xt * st);
	double Nzo = (double) (zo * ct - xo * st);

	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
	
	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
}

/** rotate theta degrees about the  xy plan */
void Matrix5D::xyrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Nyx = (double) (yx * ct + xx * st);
	double Nyy = (double) (yy * ct + xy * st);
	double Nyz = (double) (yz * ct + xz * st);
	double Nyw = (double) (yw * ct + xw * st);
	double Nyt = (double) (yt * ct + xt * st);
	double Nyo = (double) (yo * ct + xo * st);

	double Nxx = (double) (xx * ct - yx * st);
	double Nxy = (double) (xy * ct - yy * st);
	double Nxz = (double) (xz * ct - yz * st);
	double Nxw = (double) (xw * ct - yw * st);
	double Nxt = (double) (xt * ct - yt * st);
	double Nxo = (double) (xo * ct - yo * st);

	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
	
	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
}
    
/** rotate theta degrees about the  xw plan */   
     
void Matrix5D::xwrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Nwx = (double) (wx * ct + xx * st);
	double Nwy = (double) (wy * ct + xy * st);
	double Nwz = (double) (wz * ct + xz * st);
	double Nww = (double) (ww * ct + xw * st);
	double Nwt = (double) (wt * ct + xt * st);
	double Nwo = (double) (wo * ct + xo * st);

	double Nxx = (double) (xx * ct - wx * st);
	double Nxy = (double) (xy * ct - wy * st);
	double Nxz = (double) (xz * ct - wz * st);
	double Nxw = (double) (xw * ct - ww * st);
	double Nxt = (double) (xt * ct - wt * st);
	double Nxo = (double) (xo * ct - wo * st);

	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
	
	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
} 
/** rotate theta degrees about the  xt plan */   
     
void Matrix5D::xtrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Ntx = (double) (tx * ct + xx * st);
	double Nty = (double) (ty * ct + xy * st);
	double Ntz = (double) (tz * ct + xz * st);
	double Ntw = (double) (tw * ct + xw * st);
	double Ntt = (double) (tt * ct + xt * st);
	double Nto = (double) (to * ct + xo * st);

	double Nxx = (double) (xx * ct - tx * st);
	double Nxy = (double) (xy * ct - ty * st);
	double Nxz = (double) (xz * ct - tz * st);
	double Nxw = (double) (xw * ct - tw * st);
	double Nxt = (double) (xt * ct - tt * st);
	double Nxo = (double) (xo * ct - to * st);

	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
	
	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
}  
    
/** rotate theta degrees about the  yt plan */   
     
void Matrix5D::ytrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Ntx = (double) (tx * ct + yx * st);
	double Nty = (double) (ty * ct + yy * st);
	double Ntz = (double) (tz * ct + yz * st);
	double Ntw = (double) (tw * ct + yw * st);
	double Ntt = (double) (tt * ct + yt * st);
	double Nto = (double) (to * ct + yo * st);

	double Nyx = (double) (yx * ct - tx * st);
	double Nyy = (double) (yy * ct - ty * st);
	double Nyz = (double) (yz * ct - tz * st);
	double Nyw = (double) (yw * ct - tw * st);
	double Nyt = (double) (yt * ct - tt * st);
	double Nyo = (double) (yo * ct - to * st);

	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
	
	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
}  
    
/** rotate theta degrees about the  zt plan */   
     
void Matrix5D::ztrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Ntx = (double) (tx * ct + zx * st);
	double Nty = (double) (ty * ct + zy * st);
	double Ntz = (double) (tz * ct + zz * st);
	double Ntw = (double) (tw * ct + zw * st);
	double Ntt = (double) (tt * ct + zt * st);
	double Nto = (double) (to * ct + zo * st);

	double Nzx = (double) (zx * ct - tx * st);
	double Nzy = (double) (zy * ct - ty * st);
	double Nzz = (double) (zz * ct - tz * st);
	double Nzw = (double) (zw * ct - tw * st);
	double Nzt = (double) (zt * ct - tt * st);
	double Nzo = (double) (zo * ct - to * st);

	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
	
	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
}  
    
        
/** rotate theta degrees about the  wt plan */   
     
void Matrix5D::wtrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Ntx = (double) (tx * ct + wx * st);
	double Nty = (double) (ty * ct + wy * st);
	double Ntz = (double) (tz * ct + wz * st);
	double Ntw = (double) (tw * ct + ww * st);
	double Ntt = (double) (tt * ct + wt * st);
	double Nto = (double) (to * ct + wo * st);

	double Nwx = (double) (wx * ct - tx * st);
	double Nwy = (double) (wy * ct - ty * st);
	double Nwz = (double) (wz * ct - tz * st);
	double Nww = (double) (ww * ct - tw * st);
	double Nwt = (double) (wt * ct - tt * st);
	double Nwo = (double) (wo * ct - to * st);

	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
	
	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
}    
/** rotate theta degrees about the  yw plan */   
     
void Matrix5D::ywrot(double theta) {
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Nwx = (double) (wx * ct + yx * st);
	double Nwy = (double) (wy * ct + yy * st);
	double Nwz = (double) (wz * ct + yz * st);
	double Nww = (double) (ww * ct + yw * st);
	double Nwt = (double) (wt * ct + yt * st);
	double Nwo = (double) (wo * ct + yo * st);

	double Nyx = (double) (yx * ct - wx * st);
	double Nyy = (double) (yy * ct - wy * st);
	double Nyz = (double) (yz * ct - wz * st);
	double Nyw = (double) (yw * ct - ww * st);
	double Nyt = (double) (yt * ct - wt * st);
	double Nyo = (double) (yo * ct - wo * st);

	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
	
	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
}     

      
/** rotate theta degrees about the  yw plan */   
     
void Matrix5D::zwrot(double theta) {
    
	theta *= (pi / 180);
    double ct = Math::Cos(theta);
    double st = Math::Sin(theta);

	double Nwx = (double) (wx * ct + zx * st);
	double Nwy = (double) (wy * ct + zy * st);
	double Nwz = (double) (wz * ct + zz * st);
	double Nww = (double) (ww * ct + zw * st);
	double Nwt = (double) (wt * ct + zt * st);
	double Nwo = (double) (wo * ct + zo * st);

	double Nzx = (double) (zx * ct - wx * st);
	double Nzy = (double) (zy * ct - wy * st);
	double Nzz = (double) (zz * ct - wz * st);
	double Nzw = (double) (zw * ct - ww * st);
	double Nzt = (double) (zt * ct - wt * st);
	double Nzo = (double) (zo * ct - wo * st);

	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
	
	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
}     

/** Reinitialize to the unit matrix */
void Matrix5D::unit() {
    
	xo = 0;
	xx = 1;
	xy = 0;
	xz = 0;
	xw = 0;
	xt = 0;
	
	yo = 0;
	yx = 0;
	yy = 1;
	yz = 0;
	yw = 0;
	yt = 0;
	
	zo = 0;
	zx = 0;
	zy = 0;
	zz = 1;
	zw = 0;
	zt = 0;
	
	wo = 0;
	wx = 0;
	wy = 0;
	wz = 0;
	ww = 1;
	wt = 0;
	
	to = 0;
	tx = 0;
	ty = 0;
	tz = 0;
	tw = 0; 	
	tt = 1;

	ko = 1;
	kx = 0;
	ky = 0;
	kz = 0;
	kw = 0;
	kt = 0;
}  
}
