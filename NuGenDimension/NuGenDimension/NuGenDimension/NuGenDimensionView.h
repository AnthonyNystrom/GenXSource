// NuGenDimensionView.h : interface of the CNuGenDimensionView class
//


#pragma once

#include "NuGenDimensionDoc.h"
#include "OpenGLView//OpenGLView.h"
#include "OpenGLView//ClientCapture.h"

class CNuGenDimensionView : public COpenGLView
{
protected: // create from serialization only
	CNuGenDimensionView();
	DECLARE_DYNCREATE(CNuGenDimensionView)

// Attributes
public:
	CNuGenDimensionDoc* GetDocument() const;

// Operations
private:
	bool    m_isPrinting;
	CClientCapture m_CapturedImage;
public:
	bool    IsPrintRegime() const {return m_isPrinting;};

// Overrides
public:
	virtual void OnDraw(CDC* pDC);  // overridden to draw this view
	void    ResetHandAction() {m_hand_action=HA_ROTATE;};
protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);


protected:
	virtual   void DrawScene(GLenum mode = GL_RENDER, bool selSubObj=false);
	virtual   void DrawFromCommander();


// Implementation
public:
	virtual ~CNuGenDimensionView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
public:
	afx_msg BOOL OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message);
	virtual void OnInitialUpdate();
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnFilePrintPreview();
};

extern CNuGenDimensionView*       global_opengl_view;

#ifndef _DEBUG  // debug version in NuGenDimensionView.cpp
inline CNuGenDimensionDoc* CNuGenDimensionView::GetDocument() const
   { return reinterpret_cast<CNuGenDimensionDoc*>(m_pDocument); }
#endif

