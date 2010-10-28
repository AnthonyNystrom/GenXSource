// ReportPagesPreviewDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "ReportPagesPreviewDlg.h"
#include ".\reportpagespreviewdlg.h"

#include "..//ReportCreatorView.h"


// CReportPagesPreviewDlg dialog

IMPLEMENT_DYNAMIC(CReportPagesPreviewDlg, CDialog)
CReportPagesPreviewDlg::CReportPagesPreviewDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CReportPagesPreviewDlg::IDD, pParent)
{
	m_view = NULL;
}

CReportPagesPreviewDlg::~CReportPagesPreviewDlg()
{
}

void CReportPagesPreviewDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CReportPagesPreviewDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_WM_ERASEBKGND()
	ON_WM_LBUTTONDOWN()
END_MESSAGE_MAP()


// CReportPagesPreviewDlg message handlers

void CReportPagesPreviewDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class

	//CDialog::OnCancel();
}

void CReportPagesPreviewDlg::OnOK()
{
	// TODO: Add your specialized code here and/or call the base class

	//CDialog::OnOK();
}

void CReportPagesPreviewDlg::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	
	CRect clR;
	GetClientRect(clR);
	dc.FillSolidRect(0,0,clR.Width(),clR.Height(),::GetSysColor(COLOR_BTNSHADOW));

	if (m_view)
	{
		size_t sz = m_view->m_editors.size();
		int yPos = 5;
		for (size_t i=0;i<sz;i++)
		{
			if (m_view->m_editors[i].editor->m_offscreen_dc)
			{
				double ow = static_cast<double>(m_view->m_editors[i].editor->m_offscreen_W);
				double oh = static_cast<double>(m_view->m_editors[i].editor->m_offscreen_H);
				double prop = ow/oh;
				/*CRect pgRct(clR.left+clR.Width()/2-ow/2,yPos+1,
					clR.left+clR.Width()/2+ow/2,yPos+1+oh);#WARNING*/
				  CRect pgRct(clR.left+clR.Width()/2-(int)(ow/2),yPos+1,
					clR.left+clR.Width()/2+(int)(ow/2),yPos+1+(int)oh);
				if (i==m_view->GetCurrentPage())
				{
					pgRct.InflateRect(2,2);
					dc.FillSolidRect(pgRct,RGB(255,0,0));
					pgRct.InflateRect(-2,-2);
				}	
				/*dc.StretchBlt(pgRct.left,pgRct.top,ow,oh,
						m_view->m_editors[i].editor->m_offscreen_dc,
						0,0,
						m_view->m_editors[i].editor->m_offscreen_W,
						m_view->m_editors[i].editor->m_offscreen_H,SRCCOPY);*#WARNING*/
				dc.StretchBlt(pgRct.left,pgRct.top,(int)ow,(int)oh,
						m_view->m_editors[i].editor->m_offscreen_dc,
						0,0,
						m_view->m_editors[i].editor->m_offscreen_W,
						m_view->m_editors[i].editor->m_offscreen_H,SRCCOPY);
				//yPos+=oh+8;#WARNING
				yPos+=(int)oh+8;
			}
		}
	}
}

void CReportPagesPreviewDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	if (m_view)
	{
		size_t sz = m_view->m_editors.size();
		for (size_t i=0;i<sz;i++)
		{
			m_view->m_editors[i].editor->RepaintFromThumbnail();
		}
	}
}

BOOL CReportPagesPreviewDlg::OnEraseBkgnd(CDC* pDC)
{
	return FALSE;//CDialog::OnEraseBkgnd(pDC);
}

void CReportPagesPreviewDlg::OnLButtonDown(UINT nFlags, CPoint point)
{
	if (m_view)
	{
		size_t sz = m_view->m_editors.size();
		int yPos = 5;
		CRect clR;
		GetClientRect(clR);
		for (size_t i=0;i<sz;i++)
		{
			if (m_view->m_editors[i].editor->m_offscreen_dc)
			{
				double ow = static_cast<double>(m_view->m_editors[i].editor->m_offscreen_W);
				double oh = static_cast<double>(m_view->m_editors[i].editor->m_offscreen_H);
				double prop = ow/oh;
				/*CRect pgRct(clR.left+clR.Width()/2-ow/2,yPos+1,
					clR.left+clR.Width()/2+ow/2,yPos+1+oh);#WARNING*/
				CRect pgRct(clR.left+clR.Width()/2-(int)(ow/2),yPos+1,
					clR.left+clR.Width()/2+(int)(ow/2),yPos+1+(int)oh);

				if (pgRct.PtInRect(point))
				{
					if (m_view->GetCurrentPage()==i)
						goto lll;
					else
					{
						m_view->SetCurrentPage(i);
						Invalidate();
						goto lll;
					}
				}	
				//yPos+=oh+8;#WARNING
				yPos+=(int)oh+8;
			}
		}
	}
lll:
	CDialog::OnLButtonDown(nFlags, point);
}
