// MaterialsEditorDoc.cpp : implementation of the CMaterialsEditorDoc class
//

#include "stdafx.h"
#include "MaterialsEditor.h"

#include "MaterialsEditorDoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMaterialsEditorDoc

IMPLEMENT_DYNCREATE(CMaterialsEditorDoc, CDocument)

BEGIN_MESSAGE_MAP(CMaterialsEditorDoc, CDocument)
END_MESSAGE_MAP()


// CMaterialsEditorDoc construction/destruction

CMaterialsEditorDoc::CMaterialsEditorDoc()
			:m_mat_main_header(NULL)
{
	// TODO: add one-time construction code here

}

CMaterialsEditorDoc::~CMaterialsEditorDoc()
{
	if (m_mat_main_header)
	{
		ASSERT(0);
		ClearAll();
	}
}


void  CMaterialsEditorDoc::ClearAll()
{
	if (m_mat_main_header)
	{
		size_t texSz = m_textures.size();
		ASSERT(m_materials.size()==m_mat_main_header->nTotalMat);
		ASSERT(texSz==m_mat_main_header->nTotalTex);

		for (size_t i=0;i<texSz;i++)
		{
			delete m_textures[i].image;
			delete[] m_textures[i].imageBits;
		}
		m_textures.clear();
		m_materials.clear();
		free(m_mat_main_header);
		m_mat_main_header = NULL;
	}
}


bool  CMaterialsEditorDoc::SetComment(const CString& comment)
{
	if(comment.GetLength()>63)
	{
		ASSERT(0);
		return false;
	}

	//strcpy(m_mat_main_header->szComment,comment);#OBSOLETE
	strcpy_s(m_mat_main_header->szComment,sizeof(m_mat_main_header->szComment),comment);

	SetModifiedFlag();
	return true;
}

MAT_ITEM*  CMaterialsEditorDoc::GetMaterialByIndex(int ind)
{
	int mtSz = (int)m_materials.size();

	if (ind<0 || ind>=mtSz)
	{
		ASSERT(0);
		return NULL;
	}

	return &m_materials[ind];
}

TEXTURE_ITEM*  CMaterialsEditorDoc::GetTextureByIndex(int ind)
{
	int tSz = (int)m_textures.size();

	if (ind<0 || ind>=tSz)
	{
		ASSERT(0);
		return NULL;
	}

	return &m_textures[ind];
}

BOOL CMaterialsEditorDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	ClearAll();

	if (m_mat_main_header)
		ASSERT(0);

	m_mat_main_header = (MAT_MAIN*)malloc(sizeof(MAT_MAIN));
	memset(m_mat_main_header,0,sizeof(MAT_MAIN));

	//strcpy(m_mat_main_header->szSign,_T("AR_MAT_V1.0"));#OBSOLETE
	strcpy_s(m_mat_main_header->szSign,sizeof(m_mat_main_header->szSign),"AR_MAT_V1.0");

	m_mat_main_header->nVersion	= 1;
	m_mat_main_header->nTotalMat = 0;
	m_mat_main_header->nTotalTex =0;
	//strcpy(m_mat_main_header->szComment,_T(" "));#OBSOLETE
	strcpy_s(m_mat_main_header->szComment,sizeof(m_mat_main_header->szComment)," ");

	m_materials.clear();
	m_textures.clear();

	AddNewMaterial();

	SetModifiedFlag(FALSE);

	return TRUE;
}




// CMaterialsEditorDoc serialization

void CMaterialsEditorDoc::Serialize(CArchive& ar)
{
	if (ar.IsStoring())
	{
		// TODO: add storing code here
	}
	else
	{
		// TODO: add loading code here
	}
}


// CMaterialsEditorDoc diagnostics

#ifdef _DEBUG
void CMaterialsEditorDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CMaterialsEditorDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG


// CMaterialsEditorDoc commands

BOOL CMaterialsEditorDoc::OnOpenDocument(LPCTSTR lpszPathName)
{
	if (!CDocument::OnOpenDocument(lpszPathName))
		return FALSE;

	ClearAll();

	CFile  file;
	CFileException	fe;

	if (!file.Open(lpszPathName, CFile::modeRead, &fe))	
	{
		ASSERT(0);
		return FALSE;
	}

	if (m_mat_main_header)
		ASSERT(0);

	m_mat_main_header = (MAT_MAIN*)malloc(sizeof(MAT_MAIN));
	memset(m_mat_main_header,0,sizeof(MAT_MAIN));

	if (file.Read(m_mat_main_header,sizeof(MAT_MAIN))!=sizeof(MAT_MAIN)) return FALSE;

	if(strcmp(m_mat_main_header->szSign,"AR_MAT_V1.0"))
	{
		ASSERT(0);
		file.Close();
		free(m_mat_main_header);
		m_mat_main_header = NULL;
		return FALSE;
	}

	m_materials.reserve(m_mat_main_header->nTotalMat);

	int i;

	m_materials.reserve(m_mat_main_header->nTotalMat);
	MAT_ITEM tmpItem;
	for (i=0;i<m_mat_main_header->nTotalMat;i++)
	{
		if (file.Read(&tmpItem,sizeof(MAT_ITEM))!=sizeof(MAT_ITEM)) 
		{
			m_materials.clear();
			free(m_mat_main_header);
			m_mat_main_header = NULL;
			return FALSE;
		}
		m_materials.push_back(tmpItem);
	}

	m_textures.reserve(m_mat_main_header->nTotalTex);

	for (i=0;i<m_mat_main_header->nTotalTex;i++)
	{
		TEXTURE_ITEM  tmp_t_item;
		memset(&tmp_t_item,0,sizeof(TEXTURE_ITEM));

		if (file.Read(&tmp_t_item.imageType,sizeof(int))!=sizeof(int)) 
		{
			ASSERT(0);
			return FALSE;
		}
		if (file.Read(&tmp_t_item.imageSize,sizeof(unsigned int))!=sizeof(unsigned int)) 
		{
			ASSERT(0);
			return FALSE;
		}
		try
		{
			tmp_t_item.imageBits = new BYTE[tmp_t_item.imageSize];
		}
		catch (std::bad_alloc) 
		{
			AfxMessageBox("bad_alloc exception in OnOpenDocument function");
			ASSERT(0);
			return NULL;
		}

		if (file.Read(tmp_t_item.imageBits,tmp_t_item.imageSize*sizeof(BYTE))!=tmp_t_item.imageSize*sizeof(BYTE))
		{
			delete[] tmp_t_item.imageBits;
			return FALSE;
		}

		tmp_t_item.image = new CxImage(tmp_t_item.imageBits, tmp_t_item.imageSize, tmp_t_item.imageType);

		if (!tmp_t_item.image->IsValid())
		{
			AfxMessageBox(tmp_t_item.image->GetLastError());
			delete tmp_t_item.image;
			tmp_t_item.image = NULL;
			delete[] tmp_t_item.imageBits;
			ASSERT(0);
			return FALSE;
		}

		m_textures.push_back(tmp_t_item);
	}

	file.Close();

	return TRUE;
}

BOOL CMaterialsEditorDoc::OnSaveDocument(LPCTSTR lpszPathName)
{
	// TODO: Add your specialized code here and/or call the base class
	CFile			file;
	CFileException	fe;

	if (!file.Open(lpszPathName, CFile::modeWrite | CFile::shareDenyWrite | CFile::modeCreate, &fe))
	{
		ASSERT(0);
		return FALSE;
	}

	file.Write(m_mat_main_header,sizeof(MAT_MAIN));

	int i;

	ASSERT(m_materials.size()==m_mat_main_header->nTotalMat);
	ASSERT(m_textures.size()==m_mat_main_header->nTotalTex);

	for (i=0;i<m_mat_main_header->nTotalMat;i++)
		file.Write(&m_materials[i],sizeof(MAT_ITEM));

	for (i=0;i<m_mat_main_header->nTotalTex;i++)
	{
		file.Write(&m_textures[i].imageType,sizeof(int));
		file.Write(&m_textures[i].imageSize,sizeof(unsigned int));
		file.Write(m_textures[i].imageBits,m_textures[i].imageSize*sizeof(BYTE));
	}

	file.Close();

	SetModifiedFlag(FALSE);

	return TRUE;
}

void CMaterialsEditorDoc::OnCloseDocument()
{
	ClearAll();
	CDocument::OnCloseDocument();
}


MAT_ITEM*  CMaterialsEditorDoc::AddNewMaterial()
{
	MAT_ITEM  tmpItem;
	CString   tmpName;
	tmpName.LoadString(IDS_NEW_MATERIAL_NAME);

	memset(&tmpItem,0,sizeof(MAT_ITEM));
	//strcpy(tmpItem.szName,tmpName.GetBuffer());#OBSOLETE
	strcpy_s(tmpItem.szName,sizeof(tmpItem.szName),tmpName.GetBuffer());

	tmpItem.AmbientR = 0.0f;
	tmpItem.AmbientG = 0.0f;
	tmpItem.AmbientB = 0.0f;

	tmpItem.DiffuseR = 0.1f;
	tmpItem.DiffuseG = 0.5f;
	tmpItem.DiffuseB = 0.8f;

	tmpItem.EmissionR = 0.0f;
	tmpItem.EmissionG = 0.0f;
	tmpItem.EmissionB = 0.0f;

	tmpItem.SpecularR = 0.1f;
	tmpItem.SpecularG = 0.8f;
	tmpItem.SpecularB = 0.5f;

	tmpItem.Shininess = 100.0f;
	tmpItem.Transparent = 0.0f;

	tmpItem.m_iSolidMaterial = 0;

	m_materials.push_back(tmpItem);

	m_mat_main_header->nTotalMat++;

	SetModifiedFlag();

	return &m_materials[m_materials.size()-1];
}

bool  CMaterialsEditorDoc::DeleteMaterial(int ind)
{
	int mtSz = (int)m_materials.size();

	if (ind<0 || ind>=mtSz)
	{
		ASSERT(0);
		return false;
	}

	m_materials.erase(m_materials.begin()+ind);

	m_mat_main_header->nTotalMat--;

	SetModifiedFlag();

	return true;
}

TEXTURE_ITEM*   CMaterialsEditorDoc::AddNewTexture(CString& filePath, int imType)
{
	TEXTURE_ITEM  tmp_t_item;
	memset(&tmp_t_item,0,sizeof(TEXTURE_ITEM));

	tmp_t_item.imageType = imType;

	CFile			file;
	CFileException	fe;

	if (!file.Open(filePath, CFile::modeRead, &fe))
	{
		ASSERT(0);
		return NULL;
	}

	tmp_t_item.imageSize = (unsigned int)file.GetLength();

	try
	{
		tmp_t_item.imageBits = new BYTE[tmp_t_item.imageSize];
	}
	catch (std::bad_alloc) 
	{
		AfxMessageBox("bad_alloc exception in AddNewTexture function");
		ASSERT(0);
		return NULL;
	}

	if (file.Read(tmp_t_item.imageBits,tmp_t_item.imageSize*sizeof(BYTE))!=tmp_t_item.imageSize*sizeof(BYTE))
	{
		delete[] tmp_t_item.imageBits;
		return NULL;
	}

	tmp_t_item.image = new CxImage(tmp_t_item.imageBits, tmp_t_item.imageSize, tmp_t_item.imageType);

	if (!tmp_t_item.image->IsValid())
	{
		AfxMessageBox(tmp_t_item.image->GetLastError());
		delete tmp_t_item.image;
		tmp_t_item.image = NULL;
		delete[] tmp_t_item.imageBits;
		ASSERT(0);
		return NULL;
	}


	m_textures.push_back(tmp_t_item);

	m_mat_main_header->nTotalTex++;

	SetModifiedFlag();

	return &m_textures[m_textures.size()-1];
}

bool CMaterialsEditorDoc::DeleteTexture(int ind)
{
	int tSz = (int)m_textures.size();

	if (ind<0 || ind>=tSz)
	{
		ASSERT(0);
		return false;
	}

	int mSz = (int)m_materials.size();
	for (int i=0;i<mSz;i++)
	{
		if (m_materials[i].nIdxTexture>ind)
			m_materials[i].nIdxTexture--;
		if (m_materials[i].nIdxTexture==ind)
			m_materials[i].nIdxTexture=0;
	}

	delete m_textures[ind].image;
	delete[] m_textures[ind].imageBits;

	m_textures.erase(m_textures.begin()+ind);

	m_mat_main_header->nTotalTex--;

	SetModifiedFlag();

	return true;
}

