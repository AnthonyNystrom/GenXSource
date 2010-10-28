#if !defined(AFX_CORNERBOX_H__B14AEFE5_348C_4365_AE70_C54A1FA241D5__INCLUDED_)
#define AFX_CORNERBOX_H__B14AEFE5_348C_4365_AE70_C54A1FA241D5__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// CornerBox.h : header file
//

#ifndef MEASURE_PIXELS
	#define MEASURE_PIXELS		0
	#define MEASURE_INCHES		1
	#define MEASURE_CENTIMETERS	2
#endif

/////////////////////////////////////////////////////////////////////////////
// CCornerBox window

class CCornerBox : public CWnd
{
// Construction
public:
	CCornerBox();

// Attributes
public:

// Operations
public:

	void SetMeasurements( int measurements );
	int GetMeasurements() const;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CCornerBox)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CCornerBox();

	// Generated message map functions
protected:
	//{{AFX_MSG(CCornerBox)
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnPaint();
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	int m_measurements;

};

extern UINT UWM_MEASUREMENTS;

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CORNERBOX_H__B14AEFE5_348C_4365_AE70_C54A1FA241D5__INCLUDED_)
