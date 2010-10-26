#ifndef __COLOR_SCHEME_H__
#define __COLOR_SCHEME_H__

//	RenderProperty.cpp 참고.

#define			NUM_COLOR_SCHEME		(COLOR_SCHEME_LAST)

enum COLOR_SCHEME {		COLOR_SCHEME_CPK = 0,			//	cpk
						COLOR_SCHEME_AMINO_ACID,		//	아미노산 별로 color 설정. color 는 정해져 있음.
						COLOR_SCHEME_CHAIN,				//	체인별로 color 설정.
						COLOR_SCHEME_OCCUPANCY,
						COLOR_SCHEME_TEMPARATURE,
						COLOR_SCHEME_PROGRESSIVE,
						COLOR_SCHEME_HYDROPATHY,
						COLOR_SCHEME_SINGLECOLOR,
						COLOR_SCHEME_CUSTOM,
						COLOR_SCHEME_LAST
};

enum { AMBIENT, DIFFUSE, SPECULAR };
//    
class CColorRow
{
public:
	CString			m_name;
	D3DXCOLOR		m_color;

	CColorRow(CString & name, D3DXCOLOR color);
	CColorRow() {} 
	virtual ~CColorRow() {} 

	CColorRow ( CColorRow const & colorRow );
	CColorRow&	operator = ( const CColorRow & colorRow );
};

typedef std::vector < COLORREF >  CArrayRGBColor;
typedef std::vector < CColorRow * > CArrayColorRow;

#define min3v(v1, v2, v3)   ((v1)>(v2)? ((v2)>(v3)?(v3):(v2)):((v1)>(v3)?(v3):(v2)))
#define max3v(v1, v2, v3)   ((v1)<(v2)? ((v2)<(v3)?(v3):(v2)):((v1)<(v3)?(v3):(v1)))
typedef struct
{
	BYTE  red ;              // [0,255]
	BYTE  green ;            // [0,255]
	BYTE  blue ;             // [0,255]

}COLOR_RGB;

typedef struct
{
	float hue ;              // [0,360]
	float saturation ;       // [0,100]
	float luminance ;        // [0,100]
}COLOR_HSL;

void RGBtoHSL(/*[in]*/const COLOR_RGB *rgb, /*[out]*/COLOR_HSL *hsl);
void HSLtoRGB(const COLOR_HSL *hsl, COLOR_RGB *rgb);

class CColorSchemeDefault
{
public:
	CColorSchemeDefault();
	~CColorSchemeDefault();

	static D3DXCOLOR GetAmbientColorFromDiffuse(D3DXCOLOR & ambient);
	static D3DXCOLOR GetSpecularColorFromDiffuse(D3DXCOLOR & ambient);

	CArrayColorRow	m_arrayColorRowDefault[NUM_COLOR_SCHEME];

	void CopyArrayColorRow(CArrayColorRow & dest, CArrayColorRow & source);

	static D3DCOLOR	ConvertHSIToRGB(float _H, float _S, float _I);
	static void ConvertRGBToHSI(COLORREF rgb, float *h, float* s, float* l);

private:
	static FLOAT		m_colorAmbientIntensityDelta;
	static FLOAT		m_colorSpecularIntensityDelta;

	D3DXCOLOR	m_colorAmbientDelta;
	D3DXCOLOR	m_colorSpecularDelta;
};

D3DXCOLOR FindGradientColor ( int iStep , int nStep );
D3DXCOLOR FindGradientColor ( D3DXCOLOR color1, D3DXCOLOR color2, int iStep , int nStep );
D3DXCOLOR FindGradientColor ( CArrayColorRow & arrayColorRow, int iStep , int nStep );
void GenerateGradientColor ( CArrayColorRow & arrayColorRow, CArrayRGBColor & arrayRGBColor , int nStep );

#endif
