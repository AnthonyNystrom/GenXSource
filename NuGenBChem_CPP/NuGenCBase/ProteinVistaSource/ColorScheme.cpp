#include "StdAfx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"

#include "PDBRenderer.h"
#include "ColorScheme.h"
 
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

void RGBtoHSL(/*[in]*/const COLOR_RGB *rgb, /*[out]*/COLOR_HSL *hsl)
{
	float h=0, s=0, l=0;
	// normalizes red-green-blue values
	float r = rgb->red/255.f;
	float g = rgb->green/255.f;
	float b = rgb->blue/255.f;
	float maxVal = max3v(r, g, b);
	float minVal = min3v(r, g, b);

	if(maxVal == minVal)
	{
		h = 0; // undefined
	}

	else if(maxVal==r && g>=b)
	{
		h = 60.0f*(g-b)/(maxVal-minVal);
	}

	else if(maxVal==r && g<b)
	{
		h = 60.0f*(g-b)/(maxVal-minVal) + 360.0f;
	}

	else if(maxVal==g)
	{
		h = 60.0f*(b-r)/(maxVal-minVal) + 120.0f;
	}
	else if(maxVal==b)
	{
		h = 60.0f*(r-g)/(maxVal-minVal) + 240.0f;
	}

	// luminance

	l = (maxVal+minVal)/2.0f;
	// saturation

	if(l == 0 || maxVal == minVal)
	{
		s = 0;
	}

	else if(0<l && l<=0.5f)
	{
		s = (maxVal-minVal)/(maxVal+minVal);
	}
	else if(l>0.5f)
	{
		s = (maxVal-minVal)/(2 - (maxVal+minVal)); //(maxVal-minVal > 0)?
	}
	hsl->hue = (h>360)? 360 : ((h<0)?0:h);
	hsl->saturation = ((s>1)? 1 : ((s<0)?0:s))*100;
	hsl->luminance = ((l>1)? 1 : ((l<0)?0:l))*100;
}

void HSLtoRGB(const COLOR_HSL *hsl, COLOR_RGB *rgb)
{
	float h = hsl->hue;                  // h must be [0, 360]
	float s = hsl->saturation/100.f;     // s must be [0, 1]
	float l = hsl->luminance/100.f;      // l must be [0, 1]
	float R, G, B;

	if(hsl->saturation == 0)
	{
		// achromatic color (gray scale)
		R = G = B = l*255.f;
	}
	else
	{
		float q = (l<0.5f)?(l * (1.0f+s)):(l+s - (l*s));
		float p = (2.0f * l) - q;
		float Hk = h/360.0f;
		float T[3];
		T[0] = Hk + 0.3333333f; // Tr   0.3333333f=1.0/3.0
		T[1] = Hk;              // Tb
		T[2] = Hk - 0.3333333f; // Tg

		for(int i=0; i<3; i++)
		{
			if(T[i] < 0) T[i] += 1.0f;
			if(T[i] > 1) T[i] -= 1.0f;

			if((T[i]*6) < 1)
			{
				T[i] = p + ((q-p)*6.0f*T[i]);
			}
			else if((T[i]*2.0f) < 1) //(1.0/6.0)<=T[i] && T[i]<0.5
			{
				T[i] = q;
			}
			else if((T[i]*3.0f) < 2) // 0.5<=T[i] && T[i]<(2.0/3.0)
			{
				T[i] = p + (q-p) * ((2.0f/3.0f) - T[i]) * 6.0f;
			}
			else T[i] = p;
		}
		R = T[0]*255.0f;
		G = T[1]*255.0f;
		B = T[2]*255.0f;

	}
	rgb->red = (BYTE)((R>255)? 255 : ((R<0)?0 : R));
	rgb->green = (BYTE)((G>255)? 255 : ((G<0)?0 : G));
	rgb->blue = (BYTE)((B>255)? 255 : ((B<0)?0 : B));

}



/*
//
//	http://www.umass.edu/microbio/rasmol/distrib/rasman.htm#cpkcolours
//	CPK Color model
//

					ELEMENT                   COLOR NAME       RGB DECIMAL    RGB HEXADECIMAL
C					Carbon                    light grey       [200,200,200]  C8 C8 C8
O					Oxygen                    red              [240,0,0]      F0 00 00
H					Hydrogen                  white            [255,255,255]  FF FF FF

N					Nitrogen                  light blue       [143,143,255]  8F 8F FF
S					Sulphur                   sulphur yellow   [255,200,50]   FF C8 32
Cl B				Chlorine, Boron           green            [0,255,0]      00 FF 00

P Fe Ba				Phosphorus, Iron, Barium  orange           [255,165,0]    FF A5 00
Na					Sodium                    blue             [0,0,255]      00 00 FF
Mg					Magnesium                 forest green     [34,139,34]    22 8B 22
Zn Cu Ni Br			Zn, Cu, Ni, Br            brown            [165,42,42]    A5 2A 2A

Ca, Mn, Al, Ti, Cr, Ag    Ca, Mn, Al, Ti, Cr, Ag    dark grey        [128,128,144]  80 80 90
F, Si, Au		    F, Si, Au                 goldenrod        [218,165,32]   DA A5 20
I					Iodine                    purple           [160,32.240]   A0 20 F0

Li					Lithium                   firebrick        [178,34,34]    B2 22 22
He					Helium                    pink             [255,192,203]  FF C0 CB
					Unknown                   deep pink        [255,20,147]   FF 14 93
 */

/*
 Alpha helices are coloured magenta, [255,0,128], 
 beta sheets are coloured yellow, [255,200,0], 
 turns are coloured pale blue, [96,128,255] 
 and all other residues are coloured white
*/

/*
 [verified against 2.6 source code 99/03/13]
   ASP,GLU   bright red [230,10,10]     E6 0A 0A
   LYS,ARG   blue       [20,90,255]     14 5A FF
   PHE,TYR   mid blue   [50,50,170]     32 32 AA
   GLY       light grey [235,235,235]   EB EB EB
   ALA       dark grey  [200,200,200]   C8 C8 C8
   HIS       pale blue  [130,130,210]   82 82 D2
   CYS,MET       yellow [230,230,0]     E6 E6 00
   SER,THR       orange [250,150,0]     FA 96 00
   ASN,GLN       cyan   [0,220,220]     00 DC DC
   LEU,VAL,ILE   green  [15,130,15]     0F 82 0F
   TRP           pink   [180,90,180]    B4 5A B4
   PRO, PCA, HYP flesh  [220,150,130]   DC 96 82
 */	

#define SELECT_COLOR		60

FLOAT		CColorSchemeDefault::m_colorAmbientIntensityDelta = 0.3f;
FLOAT		CColorSchemeDefault::m_colorSpecularIntensityDelta = 0.4f;


//D3DCOLOR atomColorDiffuse[MAX_ATOM] = 
//				{
//					D3DCOLOR_ARGB(0,180,180,180),		//	c
//					D3DCOLOR_ARGB(0,50,50,255),			//	n
//					D3DCOLOR_ARGB(0,255,50,50),			//	o
//					D3DCOLOR_ARGB(0,255,200,50),		//	s
//					D3DCOLOR_ARGB(0,235,235,235),		//	h
//					D3DCOLOR_ARGB(0,255,165,0),			//	p
//					D3DCOLOR_ARGB(0,0,255,0),
//					D3DCOLOR_ARGB(0,165,42,42),
//					D3DCOLOR_ARGB(0,0,0,255),
//					D3DCOLOR_ARGB(0,255,165,0),
//					D3DCOLOR_ARGB(0,34,139,34),
//					D3DCOLOR_ARGB(0,255,20,147),
//					D3DCOLOR_ARGB(0,128,128,144),
//					D3DCOLOR_ARGB(0,160,32,240),
//					D3DCOLOR_ARGB(0,218,165,32),
//					D3DCOLOR_ARGB(0,0,255,0),
//					D3DCOLOR_ARGB(0,255,20,147)
//				};

CColorSchemeDefault::CColorSchemeDefault()
{	
	m_colorAmbientDelta = D3DXCOLOR(0.2,0.2,0.2,0);
	m_colorSpecularDelta = D3DXCOLOR(0.35,0.35,0.35,0);

	{	//	CPK
		static D3DCOLOR atomColorCPKDiffuse[MAX_ATOM] = 
		{
			D3DCOLOR_ARGB(0,200,200,200),
			D3DCOLOR_ARGB(0,50,50,255),		
			D3DCOLOR_ARGB(0,255,50,50),
			D3DCOLOR_ARGB(0,255,200,50),
			D3DCOLOR_ARGB(0,235,235,235),
			D3DCOLOR_ARGB(0,255,165,0),
			D3DCOLOR_ARGB(0,0,255,0),
			D3DCOLOR_ARGB(0,165,42,42),
			D3DCOLOR_ARGB(0,0,0,255),
			D3DCOLOR_ARGB(0,255,165,0),
			D3DCOLOR_ARGB(0,34,139,34),
			D3DCOLOR_ARGB(0,255,20,147),
			D3DCOLOR_ARGB(0,128,128,144),
			D3DCOLOR_ARGB(0,160,32,240),
			D3DCOLOR_ARGB(0,218,165,32),
			D3DCOLOR_ARGB(0,0,255,0),
			D3DCOLOR_ARGB(0,255,20,147)
		};

		static CString arrayAtomName[MAX_ATOM] = { "C","N","O","S","H","P","CL","ZN","NA","FE","MG","K","CA","I","F","B","UNKNOWN" };

		m_arrayColorRowDefault[COLOR_SCHEME_CPK].reserve(MAX_ATOM);

		for ( int i = 0 ; i < MAX_ATOM ; i ++ )
		{
			D3DXCOLOR diffuse = atomColorCPKDiffuse[i];
			D3DXCOLOR ambient = GetAmbientColorFromDiffuse(diffuse);
			D3DXCOLOR specular= GetSpecularColorFromDiffuse(diffuse);

			CColorRow colorRow(arrayAtomName[i], diffuse );

			m_arrayColorRowDefault[COLOR_SCHEME_CPK].push_back(new CColorRow(colorRow));
		}
	}

	{	//	residue.
		static D3DCOLOR residueColor[] = 
		{
			D3DCOLOR_ARGB(0, 200,200,200 ),
			D3DCOLOR_ARGB(0, 20,90,255 ),
			D3DCOLOR_ARGB(0, 0,220,220 ),
			D3DCOLOR_ARGB(0, 230,10,10 ),
			D3DCOLOR_ARGB(0, 230,230,0 ),
			D3DCOLOR_ARGB(0, 230,10,10 ),
			D3DCOLOR_ARGB(0, 0,220,220 ),
			D3DCOLOR_ARGB(0, 235,235,235 ),
			D3DCOLOR_ARGB(0, 130,130,210 ),
			D3DCOLOR_ARGB(0, 15,130,15 ),
			D3DCOLOR_ARGB(0, 15,130,15 ),
			D3DCOLOR_ARGB(0, 20,90,255 ),
			D3DCOLOR_ARGB(0, 230,230,0 ),
			D3DCOLOR_ARGB(0, 50,50,170 ),
			D3DCOLOR_ARGB(0, 220,150,130 ),
			D3DCOLOR_ARGB(0, 250,150,0 ),
			D3DCOLOR_ARGB(0, 250,150,0 ),
			D3DCOLOR_ARGB(0, 180,90,180 ),
			D3DCOLOR_ARGB(0, 50,50,170 ),
			D3DCOLOR_ARGB(0, 15,130,15 )
		};

		static CString arrayResidueName[RSD_VAL+1] = { "ALA","ARG","ASN","ASP","CYS","GLU","GLN","GLY","HIS","ILE","LEU","LYS","MET","PHE","PRO","SER","THR","TRP","TYR","VAL" };

		m_arrayColorRowDefault[COLOR_SCHEME_AMINO_ACID].reserve(RSD_VAL+1);

		for ( int i = 0 ; i < RSD_VAL+1 ; i ++ )
		{
			D3DXCOLOR diffuse = residueColor[i];
			D3DXCOLOR ambient = GetAmbientColorFromDiffuse(diffuse);
			D3DXCOLOR specular= GetSpecularColorFromDiffuse(diffuse);

			CColorRow colorRow(arrayResidueName[i], diffuse );

			m_arrayColorRowDefault[COLOR_SCHEME_AMINO_ACID].push_back(new CColorRow(colorRow));
		}
	}

	{
		
		D3DXCOLOR diffuse1(CColorSchemeDefault::ConvertHSIToRGB(0.0f, 1.0f, 0.5f));
		D3DXCOLOR ambient1 = GetAmbientColorFromDiffuse(diffuse1);
		D3DXCOLOR specular1 = GetSpecularColorFromDiffuse(diffuse1);
		CColorRow colorRow1( CString("Color 1"), diffuse1 );
		
		D3DXCOLOR diffuse2(CColorSchemeDefault::ConvertHSIToRGB(212/255.0f, 255/255.0f, 127/255.0f));
		D3DXCOLOR ambient2 = GetAmbientColorFromDiffuse(diffuse2);
		D3DXCOLOR specular2 = GetSpecularColorFromDiffuse(diffuse2);
		CColorRow colorRow2( CString("Color 2"), diffuse2 );

		m_arrayColorRowDefault[COLOR_SCHEME_OCCUPANCY].push_back(new CColorRow(colorRow1));
		m_arrayColorRowDefault[COLOR_SCHEME_OCCUPANCY].push_back(new CColorRow(colorRow2));

		m_arrayColorRowDefault[COLOR_SCHEME_TEMPARATURE].push_back(new CColorRow(colorRow1));
		m_arrayColorRowDefault[COLOR_SCHEME_TEMPARATURE].push_back(new CColorRow(colorRow2));

		m_arrayColorRowDefault[COLOR_SCHEME_PROGRESSIVE].push_back(new CColorRow(colorRow1));
		m_arrayColorRowDefault[COLOR_SCHEME_PROGRESSIVE].push_back(new CColorRow(colorRow2));

		m_arrayColorRowDefault[COLOR_SCHEME_HYDROPATHY].push_back(new CColorRow(colorRow1));
		m_arrayColorRowDefault[COLOR_SCHEME_HYDROPATHY].push_back(new CColorRow(colorRow2));

		m_arrayColorRowDefault[COLOR_SCHEME_SINGLECOLOR].push_back(new CColorRow(colorRow1));
	}
	
	//    COLOR_SCHEME_CHAIN 은 chain갯수에 따라 정해지므로, 여기서 구하지 않는다.
	//    m_arrayColorRowDefault[COLOR_SCHEME_CHAIN]  //  갯수가 정해진다.
}

CColorSchemeDefault::~CColorSchemeDefault()
{
	for ( int i = 0 ; i < NUM_COLOR_SCHEME ; i++ )
	{
		for ( int j = 0 ; j < m_arrayColorRowDefault[i].size(); j++ )
		{
			SAFE_DELETE( m_arrayColorRowDefault[i][j] );
		}
		m_arrayColorRowDefault[i].clear();
	}
}

//
//
//
D3DXCOLOR CColorSchemeDefault::GetAmbientColorFromDiffuse(D3DXCOLOR & ambient)
{
	FLOAT h,s,i;
	CColorSchemeDefault::ConvertRGBToHSI(RGB(ambient.r*255, ambient.g*255, ambient.b*255 ), &h, &s, &i );
	i = max(0.0f, i-m_colorAmbientIntensityDelta);
	return D3DXCOLOR ( CColorSchemeDefault::ConvertHSIToRGB(h,s,i) );
}

D3DXCOLOR CColorSchemeDefault::GetSpecularColorFromDiffuse(D3DXCOLOR & ambient)
{
	FLOAT h,s,i;
	CColorSchemeDefault::ConvertRGBToHSI(RGB(ambient.r*255, ambient.g*255, ambient.b*255 ), &h, &s, &i );
	i = min(1.0f, i+m_colorSpecularIntensityDelta);
	return D3DXCOLOR ( CColorSchemeDefault::ConvertHSIToRGB(h,s,i) );
}

void CColorSchemeDefault::CopyArrayColorRow(CArrayColorRow & dest, CArrayColorRow & source)
{
	for ( int i = 0 ; i < source.size() ; i++ )
	{
		dest.push_back(new CColorRow( source[i]->m_name, source[i]->m_color ) );
	}
}

CColorRow::CColorRow(CString & name, D3DXCOLOR color1 ) 
{
	m_name = name;
	m_color = D3DXCOLOR ( min(max(0.0f,color1.r),1.0f),min(max(0.0f,color1.g),1.0f),min(max(0.0f,color1.b),1.0f), 0.0f );
} 

CColorRow::CColorRow ( CColorRow const & colorRow )
{
	m_name = colorRow.m_name;
	m_color = D3DXCOLOR(colorRow.m_color);
}

CColorRow&	CColorRow::operator = ( const CColorRow & colorRow )
{
	m_name = colorRow.m_name;
	m_color = colorRow.m_color;
	return *this;
}

//
//	HSI is [0..1]
//	return : D3DCOLOR
//
//	static.
D3DCOLOR  CColorSchemeDefault::ConvertHSIToRGB(float _H, float _S, float _I)
{
	COLOR_HSL hslObj ={_H,_S,_I};
	COLOR_RGB rgbObj;
	HSLtoRGB(&hslObj,&rgbObj);
	//COLORREF rgb = CXTColorBase::HLStoRGB (_H,_I,_S);
	return D3DCOLOR_ARGB(0, rgbObj.red, rgbObj.green, rgbObj.blue );
}

void CColorSchemeDefault::ConvertRGBToHSI(COLORREF rgb, float *h, float* s, float* l)
{
	double _h = *h;
	double _s = *s;
	double _l = *l;
	COLOR_RGB  rgb2={GetRValue(rgb), GetGValue(rgb), GetBValue(rgb)};
	COLOR_HSL hsl ;
	RGBtoHSL (&rgb2,&hsl);

	*h = hsl.hue;
	*s = hsl.saturation;
	*l = hsl.luminance;
}

D3DXCOLOR FindGradientColor ( int iStep , int nStep )
{
	return FindGradientColor ( D3DXCOLOR(1,0,0,0), D3DXCOLOR(1,0,1,0), iStep , nStep );
}

D3DXCOLOR FindGradientColor ( D3DXCOLOR color1, D3DXCOLOR color2, int iStep , int nStep )
{
	CArrayColorRow arrayColorRow;
	CColorRow colorRow1( CString(""), color1 );
	arrayColorRow.push_back(&colorRow1);

	CColorRow colorRow2( CString(""), color2 );
	arrayColorRow.push_back(&colorRow2);
	
	return FindGradientColor ( arrayColorRow, iStep, nStep );
}

D3DXCOLOR FindGradientColor ( CArrayColorRow & arrayColorRow, int iStep , int nStep )
{
	if ( nStep == 0 )
		return D3DXCOLOR(1.0f, 1.0f, 1.0f, 0.0f);

	if ( iStep < 0 ) iStep = 0;
	if ( iStep >= nStep ) iStep = nStep-1;

	int i = (arrayColorRow.size()-1) * iStep / nStep;

	long nStepThisColor = (nStep+(arrayColorRow.size()-1))/(arrayColorRow.size()-1);

	D3DXCOLOR color1 = arrayColorRow[i]->m_color;
	D3DXCOLOR color2 = arrayColorRow[i+1]->m_color;

	FLOAT h1,s1,i1, h2,s2,i2;
	CColorSchemeDefault::ConvertRGBToHSI(RGB(color1.r*255, color1.g*255, color1.b*255), &h1, &s1, &i1);
	CColorSchemeDefault::ConvertRGBToHSI(RGB(color2.r*255, color2.g*255, color2.b*255), &h2, &s2, &i2);

	D3DXVECTOR3 v1(h1,s1,i1);
	D3DXVECTOR3 v2(h2,s2,i2);
	D3DXVECTOR3 dirStep = (v2-v1)/nStepThisColor;

	//    for ( int j = 0 ; j < nStepThisColor ; j++ )
	long j = iStep - i*nStepThisColor;
	
	D3DXVECTOR3 vj = v1+((v2-v1)/nStepThisColor)*j;
	return D3DXCOLOR(CColorSchemeDefault::ConvertHSIToRGB(vj.x, vj.y, vj.z));
}

void GenerateGradientColor ( CArrayColorRow & arrayColorRow, CArrayRGBColor & arrayRGBColor , int nStep )
{
	arrayRGBColor.clear();
	arrayRGBColor.reserve(nStep+10);

	for ( int i = 0 ; i < arrayColorRow.size()-1 ; i++ )
	{
		long nStepThisColor = (nStep+(arrayColorRow.size()-1))/(arrayColorRow.size()-1);

		D3DXCOLOR color1 = arrayColorRow[i]->m_color;
		D3DXCOLOR color2 = arrayColorRow[i+1]->m_color;

		FLOAT h1,s1,i1, h2,s2,i2;
		CColorSchemeDefault::ConvertRGBToHSI(RGB(color1.r*255, color1.g*255, color1.b*255), &h1, &s1, &i1);
		CColorSchemeDefault::ConvertRGBToHSI(RGB(color2.r*255, color2.g*255, color2.b*255), &h2, &s2, &i2);

		D3DXVECTOR3 v1(h1,s1,i1);
		D3DXVECTOR3 v2(h2,s2,i2);
		D3DXVECTOR3 dirStep = (v2-v1)/nStepThisColor;

		for ( int j = 0 ; j < nStepThisColor ; j++ )
		{
			D3DXVECTOR3 vj = v1+((v2-v1)/nStepThisColor)*j;

			D3DXCOLOR colorRGB(CColorSchemeDefault::ConvertHSIToRGB(vj.x, vj.y, vj.z));
			arrayRGBColor.push_back( RGB ( colorRGB.r * 255, colorRGB.g * 255, colorRGB.b * 255 ) );
		}
	}
}

