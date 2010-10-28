#if !defined(AFX_VERTRULER_H__E5302FEE_F2AB_4887_B517_450F51159895__INCLUDED_)
#define AFX_VERTRULER_H__E5302FEE_F2AB_4887_B517_450F51159895__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// VertRuler.h : header file
//

#ifndef MEASURE_PIXELS
	#define MEASURE_PIXELS		0
	#define MEASURE_INCHES		1
	#define MEASURE_CENTIMETERS	2
#endif

/////////////////////////////////////////////////////////////////////////////
// CVertRuler window

#ifndef round
	#define round(a) ( int ) ( a + .5 )
#endif

class CVertRuler : public CWnd
{
// Construction
public:
	CVertRuler();
	virtual ~CVertRuler();

// Operations

	void SetStartPos( int startPos );
	int GetStartPos() const;
	void SetMeasurements( int measurements );
	int GetMeasurements() const;
	void SetZoom( double zoom );
	double GetZoom() const;
	void SetPointerPos( int pointerPos );
	int GetPointerPos() const;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CVertRuler)
	//}}AFX_VIRTUAL

// Implementation

	// Generated message map functions
protected:
	//{{AFX_MSG(CVertRuler)
	afx_msg void OnPaint();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	void DrawPixelScale( CDC* dc, CRect rect );
	void DrawInchScale( CDC* dc, CRect rect );
	void DrawCentimeterScale( CDC* dc, CRect rect );

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

#endif // !defined(AFX_VERTRULER_H__E5302FEE_F2AB_4887_B517_450F51159895__INCLUDED_)
