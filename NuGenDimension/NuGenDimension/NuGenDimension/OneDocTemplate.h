#if !defined(AFX_ONEDOCTEMPLATE_H__C7F06469_3F87_49E8_887E_6CB13F8C8B5C__INCLUDED_)
#define AFX_ONEDOCTEMPLATE_H__C7F06469_3F87_49E8_887E_6CB13F8C8B5C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class COneDocTemplate : public CEGMultiDocTemplate  
{
	DECLARE_DYNAMIC(COneDocTemplate)

// Constructors
public:
	COneDocTemplate(UINT nIDResource, CRuntimeClass* pDocClass,
		CRuntimeClass* pFrameClass, CRuntimeClass* pViewClass);

// Implementation
public:
	virtual ~COneDocTemplate();
	virtual void AddDocument(CDocument* pDoc);
	virtual CDocument* OpenDocumentFile(
		LPCTSTR lpszPathName, BOOL bMakeVisible = TRUE);
	virtual void SetDefaultTitle(CDocument* pDocument);

#ifdef _DEBUG
	virtual void AssertValid() const;
#endif //_DEBUG
};

#endif // !defined(AFX_ONEDOCTEMPLATE_H__C7F06469_3F87_49E8_887E_6CB13F8C8B5C__INCLUDED_)
