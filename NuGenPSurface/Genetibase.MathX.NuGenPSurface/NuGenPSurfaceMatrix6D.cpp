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

#include "NuGenPSurfaceMatrix6D.h" 

namespace NuGenPSurfaceMatrices {
/** Create a new unit matrix */
Matrix6D::Matrix6D() {
    
    pi = Math::PI;
	xx = 1.0f;
	yy = 1.0f;
	zz = 1.0f;
	ww = 1.0f;
	tt = 1.0f;
    ss = 1.0f;
	ko = 1.0f;
	xy = xz = xw = xt = xs = xo = 0;
	yx = yz = yw = yt = ys = yo = 0;
	zx = zy = zw = zt = zs = zo = 0;
	wx = wy = wz = wt = ws = wo = 0;
	tx = ty = tz = tw = ts = to = 0;
    sx = sy = sz = sw = st = so = 0;
	kx = ky = kz = kw = kt = ks = 0;
}


void Matrix6D::mult(Matrix6D^ rhs) {
	double lxx = xx * rhs->xx + yx * rhs->xy + zx * rhs->xz + wx * rhs->xw + tx * rhs->xt + sx * rhs->xs;
	double lxy = xy * rhs->xx + yy * rhs->xy + zy * rhs->xz + wy * rhs->xw + ty * rhs->xt + sy * rhs->xs;
	double lxz = xz * rhs->xx + yz * rhs->xy + zz * rhs->xz + wz * rhs->xw + tz * rhs->xt + sz * rhs->xs;
	double lxw = xw * rhs->xx + yw * rhs->xy + zw * rhs->xz + ww * rhs->xw + tw * rhs->xt + sw * rhs->xs;
	double lxt = xt * rhs->xx + yt * rhs->xy + zt * rhs->xz + wt * rhs->xw + tt * rhs->xt + st * rhs->xs;
	double lxs = xs * rhs->xx + ys * rhs->xy + zs * rhs->xz + ws * rhs->xw + ss * rhs->xt + ss * rhs->xs;
	double lxo = xo * rhs->xx + yo * rhs->xy + zo * rhs->xz + wo * rhs->xw + to * rhs->xt + so * rhs->xs + rhs->xo;

	double lyx = xx * rhs->yx + yx * rhs->yy + zx * rhs->yz + wx * rhs->yw + tx * rhs->yt + sx * rhs->ys;
	double lyy = xy * rhs->yx + yy * rhs->yy + zy * rhs->yz + wy * rhs->yw + ty * rhs->yt + sy * rhs->ys;
	double lyz = xz * rhs->yx + yz * rhs->yy + zz * rhs->yz + wz * rhs->yw + tz * rhs->yt + sz * rhs->ys;
	double lyw = xw * rhs->yx + yw * rhs->yy + zw * rhs->yz + ww * rhs->yw + tw * rhs->yt + sw * rhs->ys;
	double lyt = xt * rhs->yx + yt * rhs->yy + zt * rhs->yz + wt * rhs->yw + tt * rhs->yt + st * rhs->ys;
    double lys = xs * rhs->yx + ys * rhs->yy + zs * rhs->yz + ws * rhs->yw + ts * rhs->yt + ss * rhs->ys;
	double lyo = xo * rhs->yx + yo * rhs->yy + zo * rhs->yz + wo * rhs->yw + to * rhs->yt + so * rhs->ys + rhs->yo;

	double lzx = xx * rhs->zx + yx * rhs->zy + zx * rhs->zz + wx * rhs->zw + tx * rhs->zt + sx * rhs->zs;
	double lzy = xy * rhs->zx + yy * rhs->zy + zy * rhs->zz + wy * rhs->zw + ty * rhs->zt + sy * rhs->zs;
	double lzz = xz * rhs->zx + yz * rhs->zy + zz * rhs->zz + wz * rhs->zw + tz * rhs->zt + sz * rhs->zs;
	double lzw = xw * rhs->zx + yw * rhs->zy + zw * rhs->zz + ww * rhs->zw + tw * rhs->zt + sw * rhs->zs;
	double lzt = xt * rhs->zx + yt * rhs->zy + zt * rhs->zz + wt * rhs->zw + tt * rhs->zt + st * rhs->zs;
	double lzs = xs * rhs->zx + ys * rhs->zy + zs * rhs->zz + ws * rhs->zw + ts * rhs->zt + ss * rhs->zs;
	double lzo = xo * rhs->zx + yo * rhs->zy + zo * rhs->zz + wo * rhs->zw + to * rhs->zt + so * rhs->zs + rhs->zo;
	
	double lwx = xx * rhs->wx + yx * rhs->wy + zx * rhs->wz + wx * rhs->ww + tx * rhs->wt + sx * rhs->ws;
	double lwy = xy * rhs->wx + yy * rhs->wy + zy * rhs->wz + wy * rhs->ww + ty * rhs->wt + sy * rhs->ws;
	double lwz = xz * rhs->wx + yz * rhs->wy + zz * rhs->wz + wz * rhs->ww + tz * rhs->wt + sz * rhs->ws;
	double lww = xw * rhs->wx + yw * rhs->wy + zw * rhs->wz + ww * rhs->ww + tw * rhs->wt + sw * rhs->ws;
	double lwt = xt * rhs->wx + yt * rhs->wy + zt * rhs->wz + wt * rhs->ww + tt * rhs->wt + st * rhs->ws;
	double lws = xs * rhs->wx + ys * rhs->wy + zs * rhs->wz + ws * rhs->ww + ts * rhs->wt + ss * rhs->ws;
	double lwo = xo * rhs->wx + yo * rhs->wy + zo * rhs->wz + wo * rhs->ww + to * rhs->wt + so * rhs->ws + rhs->wo;
	
	double ltx = xx * rhs->tx + yx * rhs->ty + zx * rhs->tz + wx * rhs->tw + tx * rhs->tt + sx * rhs->ts;
	double lty = xy * rhs->tx + yy * rhs->ty + zy * rhs->tz + wy * rhs->tw + ty * rhs->tt + sy * rhs->ts;
	double ltz = xz * rhs->tx + yz * rhs->ty + zz * rhs->tz + wz * rhs->tw + tz * rhs->tt + sz * rhs->ts;
	double ltw = xw * rhs->tx + yw * rhs->ty + zw * rhs->tz + ww * rhs->tw + tw * rhs->tt + sw * rhs->ts;
	double ltt = xt * rhs->tx + yt * rhs->ty + zt * rhs->tz + wt * rhs->tw + tt * rhs->tt + st * rhs->ts;
	double lts = xs * rhs->tx + ys * rhs->ty + zs * rhs->tz + ws * rhs->tw + ts * rhs->tt + ss * rhs->ts;
	double lto = xo * rhs->tx + yo * rhs->ty + zo * rhs->tz + wo * rhs->tw + to * rhs->tt + so * rhs->ts + rhs->to;		
	
	double lsx = xx * rhs->sx + yx * rhs->sy + zx * rhs->sz + wx * rhs->sw + tx * rhs->st + sx * rhs->ss;
	double lsy = xy * rhs->sx + yy * rhs->sy + zy * rhs->sz + wy * rhs->sw + ty * rhs->st + sy * rhs->ss;
	double lsz = xz * rhs->sx + yz * rhs->sy + zz * rhs->sz + wz * rhs->sw + tz * rhs->st + sz * rhs->ss;
	double lsw = xw * rhs->sx + yw * rhs->sy + zw * rhs->sz + ww * rhs->sw + tw * rhs->st + sw * rhs->ss;
	double lst = xt * rhs->sx + yt * rhs->sy + zt * rhs->sz + wt * rhs->sw + tt * rhs->st + st * rhs->ss;
	double lss = xs * rhs->sx + ys * rhs->sy + zs * rhs->sz + ws * rhs->sw + ts * rhs->st + ss * rhs->ss;
	double lso = xo * rhs->sx + yo * rhs->sy + zo * rhs->sz + wo * rhs->sw + to * rhs->st + so * rhs->ss + rhs->so;

	xx = lxx;
	xy = lxy;
	xz = lxz;
	xw = lxw;
	xt = lxt;
    xs = lxs;
	xo = lxo;

	yx = lyx;
	yy = lyy;
	yz = lyz;
	yw = lyw;
	yt = lyt;
    ys = lys;
	yo = lyo;

	zx = lzx;
	zy = lzy;
	zz = lzz;
	zw = lzw;
	zt = lzt;
    zs = lzs;
	zo = lzo;

	wx = lwx;
	wy = lwy;
	wz = lwz;
	ww = lww;
	wt = lwt;
    ws = lws;
	wo = lwo;
	
	tx = ltx;
	ty = lty;
	tz = ltz;
	tw = ltw;
	tt = ltt;
    ts = lts;
	to = lto;	
		
	sx = lsx;
	sy = lsy;
	sz = lsz;
	sw = lsw;
	st = lst;
    ss = lss;
	so = lso;	
		
		
}

/** rotate theta degrees about the yz plan */
void Matrix6D::yzrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nyx = (double) (yx * ct + zx * st);
	double Nyy = (double) (yy * ct + zy * st);
	double Nyz = (double) (yz * ct + zz * st);
	double Nyw = (double) (yw * ct + zw * st);
	double Nyt = (double) (yt * ct + zt * st);
	double Nys = (double) (ys * ct + zs * st);
	double Nyo = (double) (yo * ct + zo * st);

	double Nzx = (double) (zx * ct - yx * st);
	double Nzy = (double) (zy * ct - yy * st);
	double Nzz = (double) (zz * ct - yz * st);
	double Nzw = (double) (zw * ct - yw * st);
	double Nzt = (double) (zt * ct - yt * st);
	double Nzs = (double) (zs * ct - ys * st);
	double Nzo = (double) (zo * ct - yo * st);
	

	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
	ys = Nys;
	
	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
	zs = Nzs;
}
        
/** rotate theta degrees about the xz plan */
void Matrix6D::xzrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nxx = (double) (xx * ct + zx * st);
	double Nxy = (double) (xy * ct + zy * st);
	double Nxz = (double) (xz * ct + zz * st);
	double Nxw = (double) (xw * ct + zw * st);
	double Nxt = (double) (xt * ct + zt * st);
	double Nxs = (double) (xs * ct + zs * st);
	double Nxo = (double) (xo * ct + zo * st);

	double Nzx = (double) (zx * ct - xx * st);
	double Nzy = (double) (zy * ct - xy * st);
	double Nzz = (double) (zz * ct - xz * st);
	double Nzw = (double) (zw * ct - xw * st);
	double Nzt = (double) (zt * ct - xt * st);
	double Nzs = (double) (zs * ct - xs * st);
	double Nzo = (double) (zo * ct - xo * st);

	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
	xs = Nxs;

	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
    zs = Nzs;
}

/** rotate theta degrees about the  xy plan */
void Matrix6D::xyrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nyx = (double) (yx * ct + xx * st);
	double Nyy = (double) (yy * ct + xy * st);
	double Nyz = (double) (yz * ct + xz * st);
	double Nyw = (double) (yw * ct + xw * st);
	double Nyt = (double) (yt * ct + xt * st);
    double Nys = (double) (ys * ct + xs * st);
	double Nyo = (double) (yo * ct + xo * st);

	double Nxx = (double) (xx * ct - yx * st);
	double Nxy = (double) (xy * ct - yy * st);
	double Nxz = (double) (xz * ct - yz * st);
	double Nxw = (double) (xw * ct - yw * st);
	double Nxt = (double) (xt * ct - yt * st);
	double Nxs = (double) (xs * ct - ys * st);
	double Nxo = (double) (xo * ct - yo * st);

	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
	ys = Nys;
	
	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
    xs = Nxs;
}
    
/** rotate theta degrees about the  xw plan */   
     
void Matrix6D::xwrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nwx = (double) (wx * ct + xx * st);
	double Nwy = (double) (wy * ct + xy * st);
	double Nwz = (double) (wz * ct + xz * st);
	double Nww = (double) (ww * ct + xw * st);
	double Nwt = (double) (wt * ct + xt * st);
	double Nws = (double) (ws * ct + xs * st);
	double Nwo = (double) (wo * ct + xo * st);

	double Nxx = (double) (xx * ct - wx * st);
	double Nxy = (double) (xy * ct - wy * st);
	double Nxz = (double) (xz * ct - wz * st);
	double Nxw = (double) (xw * ct - ww * st);
	double Nxt = (double) (xt * ct - wt * st);
	double Nxs = (double) (xs * ct - ws * st);
	double Nxo = (double) (xo * ct - wo * st);

	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
    ws = Nws;
	
	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
	xs = Nxs;
} 
/** rotate theta degrees about the  xt plan */   
     
void Matrix6D::xtrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Ntx = (double) (tx * ct + xx * st);
	double Nty = (double) (ty * ct + xy * st);
	double Ntz = (double) (tz * ct + xz * st);
	double Ntw = (double) (tw * ct + xw * st);
	double Ntt = (double) (tt * ct + xt * st);
	double Nts = (double) (ts * ct + xs * st);
	double Nto = (double) (to * ct + xo * st);

	double Nxx = (double) (xx * ct - tx * st);
	double Nxy = (double) (xy * ct - ty * st);
	double Nxz = (double) (xz * ct - tz * st);
	double Nxw = (double) (xw * ct - tw * st);
	double Nxt = (double) (xt * ct - tt * st);
	double Nxs = (double) (xs * ct - ts * st);
	double Nxo = (double) (xo * ct - to * st);

	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
    ts = Nts;
	
	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
	xs = Nxs;
}  
    
/** rotate theta degrees about the  yt plan */   
     
void Matrix6D::ytrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Ntx = (double) (tx * ct + yx * st);
	double Nty = (double) (ty * ct + yy * st);
	double Ntz = (double) (tz * ct + yz * st);
	double Ntw = (double) (tw * ct + yw * st);
	double Ntt = (double) (tt * ct + yt * st);
	double Nts = (double) (ts * ct + ys * st);
	double Nto = (double) (to * ct + yo * st);

	double Nyx = (double) (yx * ct - tx * st);
	double Nyy = (double) (yy * ct - ty * st);
	double Nyz = (double) (yz * ct - tz * st);
	double Nyw = (double) (yw * ct - tw * st);
	double Nyt = (double) (yt * ct - tt * st);
	double Nys = (double) (ys * ct - ts * st);
	double Nyo = (double) (yo * ct - to * st);

	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
	ts = Nts;
	
	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
	ys = Nys;
}  
    
/** rotate theta degrees about the  zt plan */   
     
void Matrix6D::ztrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Ntx = (double) (tx * ct + zx * st);
	double Nty = (double) (ty * ct + zy * st);
	double Ntz = (double) (tz * ct + zz * st);
	double Ntw = (double) (tw * ct + zw * st);
	double Ntt = (double) (tt * ct + zt * st);
	double Nts = (double) (ts * ct + zs * st);
	double Nto = (double) (to * ct + zo * st);

	double Nzx = (double) (zx * ct - tx * st);
	double Nzy = (double) (zy * ct - ty * st);
	double Nzz = (double) (zz * ct - tz * st);
	double Nzw = (double) (zw * ct - tw * st);
	double Nzt = (double) (zt * ct - tt * st);
	double Nzs = (double) (zs * ct - ts * st);
	double Nzo = (double) (zo * ct - to * st);

	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
	ts = Nts;
	
	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
	zs = Nzs;
}  
    
        
/** rotate theta degrees about the  wt plan */   
     
void Matrix6D::wtrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Ntx = (double) (tx * ct + wx * st);
	double Nty = (double) (ty * ct + wy * st);
	double Ntz = (double) (tz * ct + wz * st);
	double Ntw = (double) (tw * ct + ww * st);
	double Ntt = (double) (tt * ct + wt * st);
	double Nts = (double) (ts * ct + ws * st);
	double Nto = (double) (to * ct + wo * st);

	double Nwx = (double) (wx * ct - tx * st);
	double Nwy = (double) (wy * ct - ty * st);
	double Nwz = (double) (wz * ct - tz * st);
	double Nww = (double) (ww * ct - tw * st);
	double Nwt = (double) (wt * ct - tt * st);
	double Nws = (double) (ws * ct - ts * st);
	double Nwo = (double) (wo * ct - to * st);

	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
	ts = Nts;
	
	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
	ws = Nws;
}    
/** rotate theta degrees about the  yw plan */   
     
void Matrix6D::ywrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nwx = (double) (wx * ct + yx * st);
	double Nwy = (double) (wy * ct + yy * st);
	double Nwz = (double) (wz * ct + yz * st);
	double Nww = (double) (ww * ct + yw * st);
	double Nwt = (double) (wt * ct + yt * st);
	double Nws = (double) (ws * ct + ys * st);
	double Nwo = (double) (wo * ct + yo * st);

	double Nyx = (double) (yx * ct - wx * st);
	double Nyy = (double) (yy * ct - wy * st);
	double Nyz = (double) (yz * ct - wz * st);
	double Nyw = (double) (yw * ct - ww * st);
	double Nyt = (double) (yt * ct - wt * st);
	double Nys = (double) (ys * ct - ws * st);
	double Nyo = (double) (yo * ct - wo * st);

	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
	ws = Nws;
	
	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
	ys = Nys;
}     

      
/** rotate theta degrees about the  yw plan */   
     
void Matrix6D::zwrot(double theta) {
    
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nwx = (double) (wx * ct + zx * st);
	double Nwy = (double) (wy * ct + zy * st);
	double Nwz = (double) (wz * ct + zz * st);
	double Nww = (double) (ww * ct + zw * st);
	double Nwt = (double) (wt * ct + zt * st);
	double Nws = (double) (ws * ct + zs * st);
	double Nwo = (double) (wo * ct + zo * st);

	double Nzx = (double) (zx * ct - wx * st);
	double Nzy = (double) (zy * ct - wy * st);
	double Nzz = (double) (zz * ct - wz * st);
	double Nzw = (double) (zw * ct - ww * st);
	double Nzt = (double) (zt * ct - wt * st);
	double Nzs = (double) (zs * ct - ws * st);
	double Nzo = (double) (zo * ct - wo * st);

	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
	ws = Nws;
	
	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
	zs = Nzs;
}     







//Fives new Rotational plans: xs, ys, zs, ws, ts





/** rotate theta degrees about the  xs plan */   
     
void Matrix6D::xsrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nsx = (double) (sx * ct + xx * st);
	double Nsy = (double) (sy * ct + xy * st);
	double Nsz = (double) (sz * ct + xz * st);
	double Nsw = (double) (sw * ct + xw * st);
	double Nst = (double) (st * ct + xt * st);
	double Nss = (double) (ss * ct + xs * st);
	double Nso = (double) (so * ct + xo * st);

	double Nxx = (double) (xx * ct - sx * st);
	double Nxy = (double) (xy * ct - sy * st);
	double Nxz = (double) (xz * ct - sz * st);
	double Nxw = (double) (xw * ct - sw * st);
	double Nxt = (double) (xt * ct - st * st);
	double Nxs = (double) (xs * ct - ss * st);
	double Nxo = (double) (xo * ct - so * st);

	so = Nso;
	sx = Nsx;
	sy = Nsy;
	sz = Nsz;
	sw = Nsw;
	st = Nst;
    ss = Nss;
	
	xo = Nxo;
	xx = Nxx;
	xy = Nxy;
	xz = Nxz;
	xw = Nxw;
	xt = Nxt;
	xs = Nxs;
} 

/** rotate theta degrees about the  ys plan */   
     
void Matrix6D::ysrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nsx = (double) (sx * ct + yx * st);
	double Nsy = (double) (sy * ct + yy * st);
	double Nsz = (double) (sz * ct + yz * st);
	double Nsw = (double) (sw * ct + yw * st);
	double Nst = (double) (st * ct + yt * st);
	double Nss = (double) (ss * ct + ys * st);
	double Nso = (double) (so * ct + yo * st);

	double Nyx = (double) (yx * ct - sx * st);
	double Nyy = (double) (yy * ct - sy * st);
	double Nyz = (double) (yz * ct - sz * st);
	double Nyw = (double) (yw * ct - sw * st);
	double Nyt = (double) (yt * ct - st * st);
	double Nys = (double) (ys * ct - ss * st);
	double Nyo = (double) (yo * ct - so * st);

	so = Nso;
	sx = Nsx;
	sy = Nsy;
	sz = Nsz;
	sw = Nsw;
	st = Nst;
    ss = Nss;
	
	yo = Nyo;
	yx = Nyx;
	yy = Nyy;
	yz = Nyz;
	yw = Nyw;
	yt = Nyt;
	ys = Nys;
} 



/** rotate theta degrees about the  zs plan */   
     
void Matrix6D::zsrot(double theta) {
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nsx = (double) (sx * ct + zx * st);
	double Nsy = (double) (sy * ct + zy * st);
	double Nsz = (double) (sz * ct + zz * st);
	double Nsw = (double) (sw * ct + zw * st);
	double Nst = (double) (st * ct + zt * st);
	double Nss = (double) (ss * ct + zs * st);
	double Nso = (double) (so * ct + zo * st);

	double Nzx = (double) (zx * ct - sx * st);
	double Nzy = (double) (zy * ct - sy * st);
	double Nzz = (double) (zz * ct - sz * st);
	double Nzw = (double) (zw * ct - sw * st);
	double Nzt = (double) (zt * ct - st * st);
	double Nzs = (double) (zs * ct - ss * st);
	double Nzo = (double) (zo * ct - so * st);

	so = Nso;
	sx = Nsx;
	sy = Nsy;
	sz = Nsz;
	sw = Nsw;
	st = Nst;
    ss = Nss;
	
	zo = Nzo;
	zx = Nzx;
	zy = Nzy;
	zz = Nzz;
	zw = Nzw;
	zt = Nzt;
	zs = Nzs;
} 



/** rotate theta degrees about the  ws plan */   
     
void Matrix6D::wsrot(double theta)
{
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nsx = (double) (sx * ct + wx * st);
	double Nsy = (double) (sy * ct + wy * st);
	double Nsz = (double) (sz * ct + wz * st);
	double Nsw = (double) (sw * ct + ww * st);
	double Nst = (double) (st * ct + wt * st);
	double Nss = (double) (ss * ct + ws * st);
	double Nso = (double) (so * ct + wo * st);

	double Nwx = (double) (wx * ct - sx * st);
	double Nwy = (double) (wy * ct - sy * st);
	double Nwz = (double) (wz * ct - sz * st);
	double Nww = (double) (ww * ct - sw * st);
	double Nwt = (double) (wt * ct - st * st);
	double Nws = (double) (ws * ct - ss * st);
	double Nwo = (double) (wo * ct - so * st);

	so = Nso;
	sx = Nsx;
	sy = Nsy;
	sz = Nsz;
	sw = Nsw;
	st = Nst;
    ss = Nss;
	
	wo = Nwo;
	wx = Nwx;
	wy = Nwy;
	wz = Nwz;
	ww = Nww;
	wt = Nwt;
	ws = Nws;
} 


/** rotate theta degrees about the  ts plan */   
 
void Matrix6D::tsrot(double theta)
{
	theta *= (pi / 180);
	double ct = Math::Cos(theta);
	double st = Math::Sin(theta);

	double Nsx = (double) (sx * ct + tx * st);
	double Nsy = (double) (sy * ct + ty * st);
	double Nsz = (double) (sz * ct + tz * st);
	double Nsw = (double) (sw * ct + tw * st);
	double Nst = (double) (st * ct + tt * st);
	double Nss = (double) (ss * ct + ts * st);
	double Nso = (double) (so * ct + to * st);

	double Ntx = (double) (tx * ct - sx * st);
	double Nty = (double) (ty * ct - sy * st);
	double Ntz = (double) (tz * ct - sz * st);
	double Ntw = (double) (tw * ct - sw * st);
	double Ntt = (double) (tt * ct - st * st);
	double Nts = (double) (ts * ct - ss * st);
	double Nto = (double) (to * ct - so * st);

	so = Nso;
	sx = Nsx;
	sy = Nsy;
	sz = Nsz;
	sw = Nsw;
	st = Nst;
    ss = Nss;
	
	to = Nto;
	tx = Ntx;
	ty = Nty;
	tz = Ntz;
	tw = Ntw;
	tt = Ntt;
	ts = Nts;
} 

/** Reinitialize to the unit matrix */
void Matrix6D::unit()
{
	xo = 0;
	xx = 1;
	xy = 0;
	xz = 0;
	xw = 0;
	xt = 0;
    xs = 0;
	
	yo = 0;
	yx = 0;
	yy = 1;
	yz = 0;
	yw = 0;
	yt = 0;
    ys = 0;
	
	zo = 0;
	zx = 0;
	zy = 0;
	zz = 1;
	zw = 0;
	zt = 0;
    zs = 0;
	
	wo = 0;
	wx = 0;
	wy = 0;
	wz = 0;
	ww = 1;
	wt = 0;
    ws = 0;
	
	to = 0;
	tx = 0;
	ty = 0;
	tz = 0;
	tw = 0; 	
	tt = 1;
    ts = 0;
	
	so = 0;
	sx = 0;
	sy = 0;
	sz = 0;
	sw = 0; 	
	st = 0;
    ss = 1;

	ko = 1;
	kx = 0;
	ky = 0;
	kz = 0;
	kw = 0;
	kt = 0;
    ks = 0;
}
}
