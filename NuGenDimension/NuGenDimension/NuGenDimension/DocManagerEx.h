#ifndef __DOC_MANAGER_EX__
#define __DOC_MANAGER_EX__

class CDocManagerEx : public CDocManager
{
    DECLARE_DYNAMIC(CDocManagerEx)
public:
	CDocManagerEx() {};
	virtual    ~CDocManagerEx() {};
    //CDocument*    CreateNewDocument(int doc_index, CString filename = "");

	void          ActivateFrame(CDocTemplate*);

	virtual BOOL DoPromptFileName(CString& fileName, UINT nIDSTitle,
		DWORD lFlags, BOOL bOpenFileDialog, CDocTemplate* pTemplate);

	void    StartApplication(CCommandLineInfo& cmdInfo);
public:
	//{{AFX_VIRTUAL(CDocManagerEx)
		virtual void OnFileNew();
		virtual void OnFileOpen();
	//}}AFX_VIRTUAL
};

#endif