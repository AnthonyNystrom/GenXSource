#if !defined(AFX_HORZRULER_H__16E3618B_0C1C_441E_8340_CF64CA089A4E__INCLUDED_)
#define AFX_HORZRULER_H__16E3618B_0C1C_441E_8340_CF64CA089A4E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// HorzRuler.h : header file
//
#ifndef MEASURE_PIXELS
	#define MEASURE_PIXELS		0
	#define MEASURE_INCHES		1
	#define MEASURE_CENTIMETERS	2
#endif

/////////////////////////////////////////////////////////////////////////////
// CHorzRuler window

#ifndef round
#define round(a) ( int ) ( a + .5 )
#endif

class CHorzRuler : public CWnd
{
// Construction
public:
	CHorzRuler();

// Attributes
public:

// Operations
public:

	void SetStartPos( int startPos );
	int GetStartPos() const;
	void SetMeasurements( int measurements );
	int GetMeasurements() const;
	void SetZoom( double zoom );
	double GetZoom() const;
	void SetPointerPos( int pointerPos );
	int GetPointerPos() const;

	void DrawPixelScale( CDC* dc, CRect rect );
	void DrawInchScale( CDC* dc, CRect rect );
	void DrawCentimeterScale( CDC* dc, CRect rect );

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CHorzRuler)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CHorzRuler();

	// Generated message map functions
protected:
	//{{AFX_MSG(CHorzRuler)
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnPaint();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	void DrawScale( CDC* dc, CRect rect, double seg, double stepno );

private:
	int		m_startPos;
	int		m_measurements;
	int		m_pointerPos;
	double	m_zoom;


};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_HORZRULER_H__16E3618B_0C1C_441E_8340_CF64CA089A4E__INCLUDED_)
