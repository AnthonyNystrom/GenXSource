// ReportCreatorView.h : interface of the CReportCreatorView class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_REPORTCREATORVIEW_H__FD25583C_8991_4117_853F_4264E9678998__INCLUDED_)
#define AFX_REPORTCREATORVIEW_H__FD25583C_8991_4117_853F_4264E9678998__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "ReportEditor/ReportEditor.h"
#include "ReportEditor/HorzRuler.h"
#include "ReportEditor/VertRuler.h"
#include "ReportEditor/CornerBox.h"

#include "ReportCreatorDoc.h"

#include "Dialogs//ReportPagesPreviewDlg.h"
struct MyEditor
{
	CReportEditor* editor;
	CHorzRuler*    Horz_Ruler;
	CVertRuler*    Vert_Ruler;
	CCornerBox*    Corner_Box;
	MyEditor()
	{
		editor = NULL;
		Horz_Ruler = NULL;
		Vert_Ruler = NULL;
		Corner_Box = NULL;
	};
};

//class CReportPagesPreviewDlg;

class CReportCreatorView : public CView
{
	friend class CReportPagesPreviewDlg;
protected: // create from serialization only
	CReportCreatorView();
	DECLARE_DYNCREATE(CReportCreatorView)

// Attributes
public:
	CReportCreatorDoc* GetDocument();

	void AddPage(CDiagramEntityContainer* cont, bool vertiacal, bool repaint=true);
	int  GetCurrentPage();
	void SetCurrentPage(int);

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CReportCreatorView)
	public:
	virtual void OnDraw(CDC* pDC);  // overridden to draw this view
	virtual void OnPrint(CDC* pDC, CPrintInfo*);
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();
	protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CReportCreatorView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CReportCreatorView)
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnButtonSettings();
	afx_msg void OnButtonGrid();
	afx_msg void OnUpdateButtonGrid(CCmdUI* pCmdUI);
	afx_msg void OnButtonMargin();
	afx_msg void OnUpdateButtonMargin(CCmdUI* pCmdUI);
	afx_msg void OnButtonSnap();
	afx_msg void OnUpdateButtonSnap(CCmdUI* pCmdUI);
	afx_msg void OnButtonAddBox();
	afx_msg void OnButtonAddLabel();
	afx_msg void OnButtonAddLine();
	afx_msg void OnEditCopy();
	afx_msg void OnUpdateEditCopy(CCmdUI* pCmdUI);
	afx_msg void OnEditCut();
	afx_msg void OnUpdateEditCut(CCmdUI* pCmdUI);
	afx_msg void OnEditPaste();
	afx_msg void OnUpdateEditPaste(CCmdUI* pCmdUI);
	afx_msg void OnEditUndo();
	afx_msg void OnUpdateEditUndo(CCmdUI* pCmdUI);
	afx_msg void OnZoomIn();
	afx_msg void OnZoomOut();
	afx_msg void OnButtonAlignBottom();
	afx_msg void OnUpdateButtonAlignBottom(CCmdUI* pCmdUI);
	afx_msg void OnButtonAlignLeft();
	afx_msg void OnUpdateButtonAlignLeft(CCmdUI* pCmdUI);
	afx_msg void OnButtonAlignRight();
	afx_msg void OnUpdateButtonAlignRight(CCmdUI* pCmdUI);
	afx_msg void OnButtonAlignTop();
	afx_msg void OnUpdateButtonAlignTop(CCmdUI* pCmdUI);
	afx_msg void OnButtonSameSize();
	afx_msg void OnUpdateButtonSameSize(CCmdUI* pCmdUI);
	afx_msg void OnButtonZoomToFit();
	afx_msg void OnButtonProperties();
	afx_msg void OnButtonAddPicture();
	afx_msg void OnDestroy();
	afx_msg void OnDelete();
	afx_msg void OnInsert();
	//}}AFX_MSG

	LRESULT OnEditorHScroll( WPARAM, LPARAM );
	LRESULT OnEditorVScroll( WPARAM, LPARAM );
	LRESULT OnEditorZoom( WPARAM, LPARAM );
	LRESULT OnEditorMouse( WPARAM z, LPARAM );
	LRESULT OnRulerMeasurements( WPARAM, LPARAM );

	DECLARE_MESSAGE_MAP()

private:
	CReportPagesPreviewDlg* m_pages_preview_dlg;
public:
	void SetPagesPreviewDlg(CReportPagesPreviewDlg* nppd) {m_pages_preview_dlg = nppd;};
private:
	// Private data
	std::vector<MyEditor>	m_editors;
	int                     m_current_editor;

	int				m_screenResolutionX;

	void			SaveSettings();
public:
	int  GetPagesCount() {return (int)m_editors.size();};
	virtual void OnPrepareDC(CDC* pDC, CPrintInfo* pInfo = NULL);

	afx_msg void OnReportAddPage();

	void   InsertPictureToPage(int,HENHMETAFILE,float leftPerc=0.0, 
											float topPerc=0.0,
											float WidthPerc=100.0, 
											float HeightPerc=100.0);
};

#ifndef _DEBUG  // debug version in ReportCreatorView.cpp
inline CReportCreatorDoc* CReportCreatorView::GetDocument()
   { return (CReportCreatorDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_REPORTCREATORVIEW_H__FD25583C_8991_4117_853F_4264E9678998__INCLUDED_)
