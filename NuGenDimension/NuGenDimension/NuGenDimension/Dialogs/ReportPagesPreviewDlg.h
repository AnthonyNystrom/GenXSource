#pragma once

#include "..//ReportCreatorView.h"
// CReportPagesPreviewDlg dialog
class CReportCreatorView;

class CReportPagesPreviewDlg : public CDialog, public IThumbnailer
{
	DECLARE_DYNAMIC(CReportPagesPreviewDlg)

public:
	CReportPagesPreviewDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CReportPagesPreviewDlg();

// Dialog Data
	enum { IDD = IDD_REPORT_PAGES_PREVIEW };

	virtual  void InvalidateThumbnailer()
	{
		Invalidate();
	};

	virtual  int  GetThumbnailerSize()
	{
		CRect rrr;
		GetWindowRect(rrr);
		return 3*rrr.Width()/4;
	};

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
	virtual void OnCancel();
	virtual void OnOK();
public:
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
private:
	CReportCreatorView*   m_view;
public:
	void SetView(CReportCreatorView* nv) {m_view = nv;};
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
};
