// NuGenDimension.h : interface of the CNuGenDimensionDoc class
//


#pragma once

//#ifndef __PRECISION__
	#define PREC_COUNT   6
	static float precisions[PREC_COUNT] = {0.001f, 0.01f, 0.1f, 1.0f, 10.0f, 100.0f};
	#define __PRECISION__
//#endif

	typedef struct 
	{
		unsigned char		CurrentLayer;
		unsigned char		CurrentColor;
		unsigned char		CurrentLineThickness;
		unsigned char		CurrentLineType;
		unsigned char       CurrentPrecision;
	} SCENE_SETUPS;

class CNuGenDimensionDoc : public CDocument
{
protected: // create from serialization only
	CNuGenDimensionDoc();
	DECLARE_DYNCREATE(CNuGenDimensionDoc)

private:
	SCENE_SETUPS   m_scene_setups;

// Attributes
public:
	virtual  BOOL IsModified();

// Operations
public:
	void SetSceneSetups(const SCENE_SETUPS&);
	void GetSceneSetups(SCENE_SETUPS&);

// Overrides
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);

// Implementation
public:
	virtual ~CNuGenDimensionDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnOpenDocument(LPCTSTR lpszPathName);
	virtual BOOL OnSaveDocument(LPCTSTR lpszPathName);
	virtual void OnCloseDocument();
	afx_msg void OnDxfExport();
	afx_msg void OnDxfImport();
	afx_msg void OnSTLExport();
	afx_msg void OnSTLImport();
};


extern CNuGenDimensionDoc*  global_3D_document;


