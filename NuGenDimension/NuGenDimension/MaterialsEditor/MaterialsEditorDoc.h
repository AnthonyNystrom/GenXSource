// MaterialsEditorDoc.h : interface of the CMaterialsEditorDoc class
//


#pragma once


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

	int	    m_iSolidMaterial;
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
	int			  imageType;
	unsigned int  imageSize;
	BYTE*		  imageBits;
	CxImage*	  image;     // в файл не записывается.
}TEXTURE_ITEM;



class CMaterialsEditorDoc : public CDocument
{
protected: // create from serialization only
	CMaterialsEditorDoc();
	DECLARE_DYNCREATE(CMaterialsEditorDoc)

// Attributes
public:

// Operations
public:

// Overrides
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);

// Implementation
public:
	virtual ~CMaterialsEditorDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif


private:
	MAT_MAIN*				  m_mat_main_header;
	std::vector<MAT_ITEM>	  m_materials;
	std::vector<TEXTURE_ITEM> m_textures;
	void                      ClearAll();
public:
	const MAT_MAIN*  GetMainHeader() {return m_mat_main_header;};
	bool             SetComment(const CString& comment);
	MAT_ITEM*        GetMaterialByIndex(int ind);
	MAT_ITEM*        AddNewMaterial();
	bool             DeleteMaterial(int ind);
	TEXTURE_ITEM*    AddNewTexture(CString& filePath, int imType);
	bool             DeleteTexture(int ind);
	TEXTURE_ITEM*    GetTextureByIndex(int ind);
protected:

// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnOpenDocument(LPCTSTR lpszPathName);
	virtual BOOL OnSaveDocument(LPCTSTR lpszPathName);
	virtual void OnCloseDocument();
};


