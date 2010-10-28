#ifndef __MAT_LOADER__
#define __MAT_LOADER__

// Заголовончая структура файла фиблиотеки
typedef struct
{
	char	szSign[16];		
	int		nVersion;		
	int		nTotalMat;
	int     nTotalTex;
	char	szComment[64];
}MAT_MAIN;

typedef struct 
{
	char	szName[32];

	float	AmbientR;
	float	AmbientG;
	float	AmbientB;

	float	DiffuseR;
	float	DiffuseG;
	float	DiffuseB;

	float	EmissionR;
	float	EmissionG;
	float	EmissionB;

	float	SpecularR;
	float	SpecularG;
	float	SpecularB;

	float	Transparent;	
	float	Shininess;

	int		m_iSolidMaterial;
	float	reserve2;
	float	reserve3;
	float	reserve4;
	float	reserve5;	
	float	reserve6;		
	float	reserve7;	
	float	reserve8;	
	float	reserve9;	
	float 	reserve10;

	LONG	nIdxTexture;	
}MAT_ITEM;

typedef struct 
{
	int		Width;
	int		Height;
	void*	pPicBits;
} OPENGL_TEXTURE;

typedef struct
{
	int			  imageType;
	unsigned int  imageSize;
	BYTE*		  imageBits;
	CxImage*	  image;     // в файл не записывается.

	//  этого нет в файле и в materialEditor. 
	//  Надо для отрисовки
	OPENGL_TEXTURE*   opengl_texture;   
}TEXTURE_ITEM;


typedef struct _CGLRGBTRIPLE { 
	BYTE rgbRed;
	BYTE rgbGreen;
	BYTE rgbBlue;
} CGLRGBTRIPLE;

class CMatLoader
{
public:
	CMatLoader();
	~CMatLoader();
private:
	MAT_MAIN*				  m_mat_main_header;
	std::vector<MAT_ITEM>	  m_materials;
	std::vector<TEXTURE_ITEM> m_textures;

	void             CalcOpenGLTexture(TEXTURE_ITEM*);
public:
	const MAT_MAIN*  GetMainHeader() {return m_mat_main_header;};
	bool			 AttachMatLibrary(LPCTSTR path);
	bool             DetachMatLibrary();
	bool             IsAttached() {return (m_mat_main_header!=NULL);};
	MAT_ITEM*        GetMaterialByIndex(int ind);
	TEXTURE_ITEM*    GetTextureByIndex(int ind);

};
#endif