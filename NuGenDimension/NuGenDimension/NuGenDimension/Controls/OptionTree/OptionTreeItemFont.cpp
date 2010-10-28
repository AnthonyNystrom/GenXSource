
#include "stdafx.h"
#include "OptionTreeItemFont.h"

// Added Headers
#include "OptionTree.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

COptionTreeItemFont::COptionTreeItemFont()
{
	// Initialize variables
	// Set item type
	SetItemType(OT_ITEM_FONT);
	m_coef = 0.0;
}

COptionTreeItemFont::~COptionTreeItemFont()
{

}

void COptionTreeItemFont::DrawAttribute(CDC *pDC, const RECT &rcRect)
{
	// Declare variables
	COLORREF crOld;
	int nOldBack;
	CRect rcText, rcClient;
	CString strText = _T("");
	HGDIOBJ hOld;
	COLORREF crOldBack;

	// Make sure options aren't NULL
	if (m_otOption == NULL)
	{
		return;
	}

	// Get window rect
	rcClient = rcRect;

	// Select font
	hOld = pDC->SelectObject(m_otOption->GetNormalFont());
	
	// Set text color
	if (IsReadOnly() == TRUE || m_otOption->IsWindowEnabled() == FALSE)
	{
		crOld = pDC->SetTextColor(GetSysColor(COLOR_GRAYTEXT));
	}
	else
	{
		crOld = pDC->SetTextColor(GetTextColor());
	}

	// Set background mode
	nOldBack = pDC->SetBkMode(TRANSPARENT);

	// Set background color
	crOldBack = pDC->SetBkColor(GetBackgroundColor());	


	strText = CString(m_lf.lfFaceName)+CString("...");
	// Get text rectangle
	rcText.left  = rcRect.left + 1;
	rcText.right = rcRect.right;
	rcText.top = rcRect.top + 1;
	rcText.bottom = rcRect.bottom;

	// Draw text
	pDC->DrawText(strText, rcText, DT_VCENTER);
	pDC->DrawText(strText, rcText, DT_VCENTER | DT_CALCRECT);
	
	// Restore GDI ojects
	pDC->SetTextColor(crOld);
	pDC->SetBkMode(nOldBack);
	pDC->SelectObject(hOld);
	pDC->SetBkColor(crOldBack);
}

void COptionTreeItemFont::OnCommit()
{
}

void COptionTreeItemFont::OnRefresh()
{
}

void COptionTreeItemFont::OnMove()
{
}


#define round(a) ( int ) ( a + .5 )

void COptionTreeItemFont::OnActivate()
{
	// Declare variables
//	CFont *pFont;
//	LOGFONT lf;
	BOOL bMultiline = FALSE;
	int m_nNumItems = 0;
//	POSITION psPos;
	
	CFontDialog	dlg( &m_lf );
	dlg.m_cf.rgbColors = m_color;
	dlg.m_cf.Flags |= CF_EFFECTS;
	int height = m_lf.lfHeight / 10;
	height = round( static_cast< double >( height ) * m_coef );
	m_lf.lfHeight = -( height );
	if (dlg.DoModal() == IDOK)
	{
		dlg.GetCurrentFont( &m_lf );
		m_lf.lfHeight = dlg.GetSize();
		m_color = dlg.GetColor();
	}
	
	// Change height
	/*if (bMultiline == TRUE)
	{
		// -- Get font
		pFont = m_otOption->GetNormalFont();
		pFont->GetLogFont(&lf);

		if ((abs(lf.lfHeight) + 3) * m_nNumItems > m_lDefaultHeight)
		{
			SetItemHeight((abs(lf.lfHeight) + 3) * m_nNumItems + OT_SPACE);
			SetDrawMultiline(TRUE);
		}
		else
		{
			SetItemHeight(m_lDefaultHeight);
			SetDrawMultiline(FALSE);
		}
	}*/

	// Update items
	if (m_otOption != NULL)
	{
		m_otOption->UpdatedItems();
	}
}

void COptionTreeItemFont::CleanDestroyWindow()
{

}

void COptionTreeItemFont::OnDeSelect()
{
}

void COptionTreeItemFont::OnSelect()
{
}


BOOL COptionTreeItemFont::CreateFontItem(const LOGFONT* lf, double coef)
{
	// Make sure options is not NULL
	if (m_otOption == NULL)
	{
		return FALSE;
	}

	memcpy(&m_lf,lf,sizeof(LOGFONT));
	m_coef = coef;

	return TRUE;
}
