// BirchCtrl.cpp : implementation file
//

#include "stdafx.h"
#include "BirchCtrl.h"
#include ".\birchctrl.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CBirch::CBirch()
{
	m_pTopNode				= new CTreeNode();	// The tree top
	m_pSelected				= NULL;	
	m_wnd   =  NULL;
}

CBirch::~CBirch()
{
	DeleteNode( m_pTopNode );	// Delete all childs if there are any
	delete m_pTopNode;			// Delete top node
	m_pTopNode = NULL;
}

HTREENODE CBirch::InsertSibling(	HTREENODE pInsertAfter, const CString& csLabel,
									COLORREF crText /* = 0 */, BOOL bUseDefaultTextColor /* = TRUE */)
{
	ASSERT( pInsertAfter != NULL );	// Make sure the node exists

	HTREENODE pNewNode = new CTreeNode();

	pNewNode->csLabel	= csLabel;					// New node's label

	if( bUseDefaultTextColor )
		pNewNode->bUseDefaultTextColor = TRUE;		// Use the default text color
	else
		pNewNode->crText = crText;					// New node's text color

	pNewNode->pParent	= pInsertAfter->pParent;	// Nas the same parent

	pNewNode->iNodeLevel = pInsertAfter->pParent->iNodeLevel+1;

	ASSERT(pNewNode->iNodeLevel==pInsertAfter->iNodeLevel);

	// Insert the new node between pInsertAfter and its next sibling
	pNewNode->pNextSibling		= pInsertAfter->pNextSibling;
	if (pNewNode->pNextSibling==NULL)
		pNewNode->pParent->pLastChild = pNewNode;
	else
		pNewNode->pNextSibling->pPrevSibling = pNewNode;

	pInsertAfter->pNextSibling	= pNewNode;
	pNewNode->pPrevSibling = pInsertAfter;

	return pNewNode;
}

HTREENODE CBirch::InsertChild( HTREENODE pParent, const CString& csLabel,
								  COLORREF crText /* = 0 */, BOOL bUseDefaultTextColor /* = TRUE */)
{
	ASSERT( pParent != NULL );	// Make sure the node exists

	if( pParent == HTOPNODE )	// Check for top node
		pParent = m_pTopNode;


	HTREENODE pNewNode = new CTreeNode();

	// Basic node information
	pNewNode->csLabel	= csLabel;	// New node's label

	if( bUseDefaultTextColor )
		pNewNode->bUseDefaultTextColor = TRUE;		// Use the default text color
	else
		pNewNode->crText = crText;					// New node's text color

	pNewNode->pParent	= pParent;	// New node's parent

	pNewNode->iNodeLevel = pParent->iNodeLevel+1;

	if (pParent->pFirstChild==NULL)
	{
		ASSERT(pParent->pLastChild==NULL);
		pParent->pFirstChild=pParent->pLastChild=pNewNode;
		pNewNode->pPrevSibling = pNewNode->pNextSibling = NULL;
	}
	else
	{
		pNewNode->pNextSibling	= NULL;
		pNewNode->pPrevSibling = pNewNode->pParent->pLastChild;
		pNewNode->pParent->pLastChild->pNextSibling = pNewNode;
		pNewNode->pParent->pLastChild = pNewNode;
	}

	return pNewNode;
}

void CBirch::DeleteNode( HTREENODE pNode)
{
	ASSERT( pNode != NULL );	// Make sure the node exists

	// Don't delete the top node
	if( pNode == HTOPNODE )
		DeleteNode( m_pTopNode);

	// Delete childs
	if( pNode->pFirstChild != NULL )
		DeleteSubTree( pNode );

	// If this node is not the top node, fix pointers in sibling list
	if( pNode != m_pTopNode )
	{
		HTREENODE pNodePar = pNode->pParent;

		// If first child, set the parent pointer to the next sibling
		// Otherwise, find sibling before and set its sibling pointer to the node's sibling
		if( pNodePar->pFirstChild == pNode )
			pNodePar->pFirstChild = pNode->pNextSibling;
		if( pNodePar->pLastChild == pNode )
			pNodePar->pLastChild = pNode->pPrevSibling;
		{
			HTREENODE  prevNd = pNode->pPrevSibling;
			HTREENODE  nextNd = pNode->pNextSibling;

			if (prevNd)
				prevNd->pNextSibling = nextNd;
			if (nextNd)
				nextNd->pPrevSibling = prevNd;
		}

		delete pNode;

		pNode = NULL;
	}
}

void CBirch::ToggleNode( HTREENODE pNode)
{
	ASSERT( pNode != NULL );

	pNode->bOpen = !( pNode->bOpen );

	if (m_wnd && m_wnd->IsWindowVisible())
		m_wnd->Invalidate();

}

void CBirch::SetNodeColor( HTREENODE pNode, COLORREF crText)
{
	ASSERT( pNode != NULL );

	pNode->bUseDefaultTextColor	= FALSE;
	pNode->crText				= crText;
}

void CBirch::DeleteSubTree( HTREENODE pNode )
{
	if( pNode->pFirstChild == NULL )
		return;

	HTREENODE curNd = pNode->pFirstChild;
	while(curNd)
	{
		DeleteSubTree(curNd);
		HTREENODE tmpNd = curNd->pNextSibling;
		DeleteNode(curNd);
		curNd = tmpNd;
	}
}

/////////////////////////////////////////////////////////////////////////////
// CBirchCtrl

CBirchCtrl::CBirchCtrl()
{
	m_bShowLines			= TRUE;				// Show lines by default
	m_bScrollBarMessage		= FALSE;			// Only relevant when calculating the scrollbar

	m_iIndent				= 16;				// Indentation for tree branches
	m_iPadding				= 4;				// Padding between tree and the control border

	m_iLineHeight			= 0;
	m_iLineHeightByFont		= 0;

	m_iDocHeight			= 0;				// A polite yet meaningless default

	m_crDefaultTextColor	= RGB(58,58,58);	// Some default
	m_crConnectingLines		= RGB(128,128,128);	// Some default
	// Safeguards

	m_birch                 = NULL;

	m_edit                  = NULL;
	m_editable_item         = NULL;

}

CBirchCtrl::~CBirchCtrl()
{
	m_Font.DeleteObject();
	if (m_edit)
	{
		m_edit->DestroyWindow();
		delete m_edit;
		m_edit = NULL;
	}
}


BEGIN_MESSAGE_MAP(CBirchCtrl, CWnd)
	//{{AFX_MSG_MAP(CBirchCtrl)
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_WM_VSCROLL()
	ON_WM_LBUTTONDOWN()
	ON_WM_MOUSEWHEEL()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
//	PUBLIC METHODS
/////////////////////////////////////////////////////////////////////////////

CBirchCtrl& CBirchCtrl::SetTextFont( LONG nHeight, BOOL bBold, BOOL bItalic, const CString& csFaceName )
{
	m_lgFont.lfHeight			= -MulDiv( nHeight, GetDeviceCaps( GetDC()->m_hDC, LOGPIXELSY ), 72 );
	m_lgFont.lfWidth			= 0;
	m_lgFont.lfEscapement		= 0;
	m_lgFont.lfOrientation		= 0;
	m_lgFont.lfWeight			= ( bBold )? FW_BOLD:FW_DONTCARE;
	m_lgFont.lfItalic			= (BYTE)( ( bItalic )? TRUE:FALSE );
	m_lgFont.lfUnderline		= FALSE;
	m_lgFont.lfStrikeOut		= FALSE;
	m_lgFont.lfCharSet			= DEFAULT_CHARSET;
	m_lgFont.lfOutPrecision		= OUT_DEFAULT_PRECIS;
	m_lgFont.lfClipPrecision	= CLIP_DEFAULT_PRECIS;
	m_lgFont.lfQuality			= DEFAULT_QUALITY;
	m_lgFont.lfPitchAndFamily	= DEFAULT_PITCH | FF_DONTCARE;

	//STRCPY( m_lgFont.lfFaceName, csFaceName );#OBSOLETE
	strcpy_s( m_lgFont.lfFaceName, sizeof(m_lgFont.lfFaceName)/sizeof(m_lgFont.lfFaceName[0]), csFaceName );


	if( m_Font.GetSafeHandle() != NULL )
		m_Font.DeleteObject();
	
	m_Font.CreateFontIndirect( &m_lgFont );

	// Calculate node height for this font
	CDC		*pDC		= GetDC();
	int		iSaved		= pDC->SaveDC();
	CFont*	pOldFont	= pDC->SelectObject( &m_Font );

	// Calculate the height of this font with a character likely to be 'big'
	// and don't forget to add a little padding
	m_iLineHeightByFont = m_iLineHeight	= pDC->GetTextExtent( "X" ).cy + 4;

	pDC->SelectObject( pOldFont );
	pDC->RestoreDC( iSaved );
	ReleaseDC( pDC );

	return *this;
}

CBirchCtrl& CBirchCtrl::SetDefaultTextColor( COLORREF crText )
{
	m_crDefaultTextColor = crText;
	
	return *this;
}



int CBirchCtrl::DrawNodesRecursive( CDC* pDC, HTREENODE pNode, int y, 
										CPen* linesPen, CPen* marksPen, CRect rFrame )
{
	if (!m_birch)
		return 0;

	if (!pNode->bVisible)
		return (y-1);

	CRect	rNode;

	// The node's location and dimensions on screen
	rNode.left		= m_iIndent*pNode->iNodeLevel+3;
	rNode.top		= y;
	rNode.right		= rFrame.right - m_iPadding;
	rNode.bottom	= y + m_iLineHeight;

	pNode->rNode.CopyRect( rNode );		// Record the rectangle

	if (pNode->pParent==m_birch->GetTopNode())
	{
		CRect rct(0,rNode.top+1,rFrame.right,rNode.bottom-1);
		themeData.DrawThemedRect(pDC,&rct,TRUE);
		pDC->FillSolidRect(rct.left,rct.top-1,rct.Width(),1,GetSysColor(COLOR_3DFACE));
		pDC->FillSolidRect(rct.left,rct.bottom,rct.Width(),1,GetSysColor(COLOR_3DSHADOW));
	}

	int iJointX	= pNode->rNode.left - m_iIndent - 6;
	int iJointY	= pNode->rNode.top + ( m_iLineHeight / 2 );
	int iDispY = iJointY - pNode->pParent->rNode.top - ( m_iLineHeight / 2 );

	if( rNode.bottom > 0 && rNode.top < rFrame.bottom )
	{
		COLORREF cr			= ( pNode->bUseDefaultTextColor )? m_crDefaultTextColor:pNode->crText;
		COLORREF crOldText	= pDC->SetTextColor( cr );

		size_t bmps_cnt = pNode->item_bitmaps.size();
		int  buf_len = 1;

		if (bmps_cnt>0)
		{
			for (size_t jj=0;jj<bmps_cnt;jj++)
			{
				int bw = pNode->item_bitmaps[jj]->GetWidth();
				int bh = pNode->item_bitmaps[jj]->GetHeight();
				CRect destR(pNode->rNode.left+buf_len,
							pNode->rNode.top+m_iLineHeight/2-bh/2,
							pNode->rNode.left+buf_len+bw,
							pNode->rNode.top+m_iLineHeight/2+bh/2);
				CRect srcR(0, 0, bw,bh);
				//pDC->Rectangle(destR);
				pNode->item_bitmaps[jj]->Draw(pDC,&destR,&srcR);
				buf_len += bw+1;
				//CTreeCtrl::CreateDragImage()
			}
		}

		pDC->DrawText( pNode->csLabel, CRect(rNode.left+buf_len+2,rNode.top,
					rNode.right,rNode.bottom), DT_LEFT | DT_SINGLELINE | DT_VCENTER );
	
		pDC->SetTextColor( crOldText );

		// draw lines START
		// If the parent is not the top node, throw a connecting line to it
			if( pNode->pParent != m_birch->GetTopNode() )
			{
				// How far up from the joint is the parent?
				// Use 1 pixel wide rectangles to draw lines
				pDC->SelectObject(linesPen);
				pDC->MoveTo(iJointX, iJointY);
				pDC->LineTo(iJointX+m_iIndent, iJointY);
				//if (pNode==pNode->pParent->pLastChild)
				{
					pDC->MoveTo(iJointX, iJointY);
					pDC->LineTo(iJointX, iJointY-iDispY+5);
				}
			}
			
			pDC->SelectObject(marksPen);
			// Put a solid dot to mark a node
			if (pNode->pFirstChild != NULL)
			{
				pDC->Rectangle(iJointX + m_iIndent - 4, iJointY - 4, iJointX + m_iIndent +5, iJointY +5);
				pDC->MoveTo(iJointX + m_iIndent - 2, iJointY);
				pDC->LineTo(iJointX + m_iIndent + 3, iJointY);
				if (!pNode->bOpen)
				{
					pDC->MoveTo(iJointX + m_iIndent, iJointY - 2);
					pDC->LineTo(iJointX + m_iIndent, iJointY + 3);
				}
			}
			// Hollow out the dot if the node has no childs
			else
			{
				pDC->Ellipse(iJointX + m_iIndent - 1, iJointY - 1, iJointX + m_iIndent +2, iJointY +2);
			}
		// draw lines END
	}
	else
	{
		pDC->SelectObject(linesPen);
		if( pNode->pParent != m_birch->GetTopNode()/* && pNode==pNode->pParent->pLastChild*/)
		{
			pDC->MoveTo(iJointX, iJointY);
			pDC->LineTo(iJointX, iJointY-iDispY+5);
		}
	}


	// If there are no child or siblings, then this branch is done
	if( pNode->pFirstChild == NULL)
		return pNode->rNode.bottom;//pNode->rNode.Height();

	// If the node is open AND it has childs, then draw those
	int iLastNodePos=pNode->rNode.bottom;
	if( pNode->bOpen)
	{
		HTREENODE curTreeNode = pNode->pFirstChild ;
		while (curTreeNode)
		{
			iLastNodePos = DrawNodesRecursive(	pDC, curTreeNode,
				iLastNodePos+1,
				linesPen, marksPen,
				rFrame );
			curTreeNode = curTreeNode->pNextSibling;
		}
	}

	return iLastNodePos;
}

void CBirchCtrl::ResetScrollBar()
{
	// Flag to avoid a call from OnSize while resetting the scrollbar
	m_bScrollBarMessage = TRUE;

	CRect rFrame;

	GetClientRect( rFrame );

	// Need for scrollbars?
	if( rFrame.Height() > m_iDocHeight + 8 )
	{
		ShowScrollBar( SB_VERT, FALSE );	// Hide it
		SetScrollPos( SB_VERT, 0 );
	}
	else
	{
		SCROLLINFO	si;
		si.cbSize = sizeof(SCROLLINFO);
		si.fMask = SIF_PAGE | SIF_RANGE;
		si.nPage = rFrame.Height()/*/m_iLineHeight*/;
		si.nMax = (m_iDocHeight + 8)/*/m_iLineHeight*/;
		si.nMin = 0 ;

		SetScrollInfo( SB_VERT, &si );
		EnableScrollBarCtrl( SB_VERT, TRUE );
	}

	m_bScrollBarMessage = FALSE;
}

HTREENODE CBirchCtrl::FindNodeByPoint( const CPoint& point, HTREENODE pNode )
{
	HTREENODE curTreeNode = pNode->pFirstChild ;
	HTREENODE tmpNd = NULL;
	while (curTreeNode)
	{
		if (curTreeNode->bVisible)
		{
			// Found it?
			if( (point.y>curTreeNode->rNode.top)&&
				(point.y<curTreeNode->rNode.bottom))
					return curTreeNode;

			if( curTreeNode->bOpen && curTreeNode->pFirstChild != NULL )
				if ((tmpNd=FindNodeByPoint( point, curTreeNode ))!=NULL)
					return tmpNd;
		}

		curTreeNode = curTreeNode->pNextSibling;
	}

	return NULL;
}

/////////////////////////////////////////////////////////////////////////////
// CBirchCtrl message handlers

void CBirchCtrl::OnPaint() 
{
	CPaintDC dc(this); // Device context for painting
	
	if (!m_birch)
		return;

	// Double-buffering
	CDC*		pDCMem		= new CDC;
	CBitmap*	pOldBitmap	= NULL;
	CBitmap		bmpCanvas;
	CRect		rFrame;

	GetClientRect( rFrame );

	pDCMem->CreateCompatibleDC( &dc );

	bmpCanvas.CreateCompatibleBitmap( &dc, rFrame.Width(), rFrame.Height() );

	pOldBitmap = pDCMem->SelectObject( &bmpCanvas );

	// START DRAW -------------------------------------------------

	// If there is a bitmap loaded, use it
	// Otherwise, paint the background white
    pDCMem->FillSolidRect( rFrame, GetSysColor(COLOR_WINDOW) );

	UINT	nMode		= pDCMem->SetBkMode( TRANSPARENT );
	CFont*	pOldFont	= pDCMem->SelectObject( &m_Font );

	CPen *pOldPen = pDCMem->GetCurrentPen();

	CPen linesPen(PS_SOLID, 1,  m_crConnectingLines),
		marksPen(PS_SOLID,  1, m_crConnectingLines);

	int iLastNodePos = m_iPadding - GetScrollPos( SB_VERT );
	
	HTREENODE curTreeNode = m_birch->GetTopNode()->pFirstChild ;
	while (curTreeNode)
	{
		iLastNodePos = DrawNodesRecursive(	pDCMem, curTreeNode,
			iLastNodePos+1,
			&linesPen,
			&marksPen,
			rFrame );
		curTreeNode = curTreeNode->pNextSibling;
	}

	pDCMem->SelectObject(pOldPen);
	
	pDCMem->SelectObject( pOldFont );
	pDCMem->SetBkMode( nMode );
	
	// END DRAW   -------------------------------------------------

	dc.BitBlt( 0, 0, rFrame.Width(), rFrame.Height(), pDCMem, 0, 0, SRCCOPY );

	pDCMem->SelectObject( pOldBitmap );

	delete pDCMem;

	iLastNodePos+=GetScrollPos( SB_VERT );

	// Has the total document height changed?
	if( iLastNodePos != m_iDocHeight )
	{
		BOOL bInvalidate = ( ( m_iDocHeight < rFrame.Height() ) != ( iLastNodePos < rFrame.Height() ) );
		
		m_iDocHeight = iLastNodePos;

		ResetScrollBar();

		// If the scrollbar has just been hidden/shown, repaint
		if( bInvalidate )
			Invalidate();
	}
}

void CBirchCtrl::OnSize(UINT nType, int cx, int cy) 
{
	// Setting the scroll sends its own size message. Prevent it thus avoiding an ugly loop.
	// Other than that, resizing the control means that the tree height may change (word-wrap).
	if( !m_bScrollBarMessage )
		ResetScrollBar();

	if (m_edit)
	{
		CRect rrr;
		m_edit->GetWindowRect(rrr);
		ScreenToClient(rrr);
		rrr.right = cx;
		m_edit->MoveWindow(rrr);
	}
	
	CWnd::OnSize(nType, cx, cy);
}

void CBirchCtrl::OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar) 
{
	int iScrollBarPos = GetScrollPos( SB_VERT );

	CRect rFrame;

	GetClientRect( rFrame );

	switch( nSBCode )
	{
		case SB_LINEUP:
			iScrollBarPos = max( iScrollBarPos - m_iLineHeight, 0 );
		break;

		case SB_LINEDOWN:
			iScrollBarPos = min( iScrollBarPos + m_iLineHeight, GetScrollLimit( SB_VERT ) );
		break;

		case SB_PAGEUP:
			iScrollBarPos = max( iScrollBarPos - rFrame.Height(), 0 );
		break;

		case SB_PAGEDOWN:
			iScrollBarPos = min( iScrollBarPos + rFrame.Height(), GetScrollLimit( SB_VERT ) );
		break;

		case SB_THUMBTRACK:
		case SB_THUMBPOSITION:
			iScrollBarPos = nPos;
			break;
	}		

	SetScrollPos( SB_VERT, iScrollBarPos );

	if (m_edit)
	{
		m_edit->DestroyWindow();
		delete m_edit;
		m_edit = NULL;
	}
	
	Invalidate();
	
	CWnd::OnVScroll(nSBCode, nPos, pScrollBar);
}

void CBirchCtrl::OnLButtonDown(UINT nFlags, CPoint point) 
{
	if (!m_birch)
		return;

	HTREENODE pClickedOn = NULL;		// Assume no node was clicked on

	if( m_birch->GetTopNode()->pFirstChild != NULL)		// If the tree is populated, search it
		pClickedOn = FindNodeByPoint( point, m_birch->GetTopNode() );

	if (m_edit)
	{
		m_edit->DestroyWindow();
		delete m_edit;
		m_edit = NULL;
	}

	if( pClickedOn != NULL )			// If a node was clicked on
	{
		if (point.x<pClickedOn->rNode.left)
			m_birch->ToggleNode( pClickedOn);
		else
		{
			size_t sss = pClickedOn->item_bitmaps.size();
			int buf_l = 1;
			for (size_t i=0;i<sss;i++)
			{	
				if ((point.x>(pClickedOn->rNode.left+buf_l))&&
					(point.x<(pClickedOn->rNode.left+buf_l+pClickedOn->item_bitmaps[i]->GetWidth())))
				{
					ClickOnIcon(pClickedOn,i);
					return;
				}
				buf_l+=pClickedOn->item_bitmaps[i]->GetWidth()+1;
			}
			if (point.x>(pClickedOn->rNode.left+buf_l) && pClickedOn->bEditable)
			{
				m_edit=new CTreeItemEdit;
				CRect rrr(pClickedOn->rNode.left+buf_l+2,pClickedOn->rNode.top-2,
					pClickedOn->rNode.right,pClickedOn->rNode.bottom+2);
				m_edit->Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | 
					WS_BORDER| ES_AUTOHSCROLL | ES_WANTRETURN,
					rrr, this, 1);
				m_edit->CreateFont(15,"MS Shell Dlg");
				m_edit->SetBirch(this);
				m_edit->SetWindowPos(&wndTop,rrr.left,rrr.top,rrr.Width(),rrr.Height(),SWP_SHOWWINDOW);
				m_edit->SetFocus();
				CString labb = pClickedOn->csLabel;
				StartEditLabel(pClickedOn,labb);
				m_edit->SetWindowText(labb);
				m_edit->SetSel(0,-1);
				m_editable_item = pClickedOn;
			}
		}
	}
	else
		CWnd::OnLButtonDown(nFlags, point);
}

BOOL CBirchCtrl::OnMouseWheel(UINT nFlags, short zDelta, CPoint pt) 
{
	// zDelta greater than 0, means rotating away from the user, that is, scrolling up
	OnVScroll( ( zDelta > 0 )? SB_LINEUP:SB_LINEDOWN, 0, NULL );

	return CWnd::OnMouseWheel(nFlags, zDelta, pt);
}

void CBirchCtrl::FireMessageFromEdit()
{
	if (m_editable_item)
	{
		ASSERT(m_edit);
		CString newLabel;
		m_edit->GetWindowText(newLabel);
		FinishEditLabel(m_editable_item, newLabel);
	}

	if (m_edit)
	{
		m_edit->DestroyWindow();
		delete m_edit;
		m_edit = NULL;
	}
	m_editable_item = NULL;
}








// CTreeItemEdit

IMPLEMENT_DYNAMIC(CTreeItemEdit, CEdit)
CTreeItemEdit::CTreeItemEdit()
{
	m_birch_ctrl = NULL;
}

CTreeItemEdit::~CTreeItemEdit()
{
}


BEGIN_MESSAGE_MAP(CTreeItemEdit, CEdit)
	ON_WM_KEYUP()
	ON_WM_KILLFOCUS()
END_MESSAGE_MAP()

BOOL CTreeItemEdit::CreateFont(LONG lfHeight, LPCTSTR lpszFaceName)
{
	//  Create a font for the combobox
	LOGFONT logFont;
	memset(&logFont, 0, sizeof(logFont));

	if (!::GetSystemMetrics(SM_DBCSENABLED))
	{
		// Since design guide says toolbars are fixed height so is the font.
		logFont.lfHeight = lfHeight;
		//logFont.lfWeight = 0;
		CString strDefaultFont = lpszFaceName;
		lstrcpy(logFont.lfFaceName, strDefaultFont);
		if (!m_font.CreateFontIndirect(&logFont))
		{
			TRACE("Could Not create font for combo\n");
			return FALSE;
		}		
	}
	else
	{
		m_font.Attach(::GetStockObject(SYSTEM_FONT));
	}
	SetFont(&m_font);
	ModifyStyleEx(0, WS_EX_CLIENTEDGE|WS_EX_STATICEDGE, SWP_FRAMECHANGED);
	return TRUE;
}


// CTreeItemEdit message handlers


void CTreeItemEdit::OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	if (nChar==13)
	{
		if (m_birch_ctrl)
			m_birch_ctrl->FireMessageFromEdit();
	}
	else
		CEdit::OnKeyUp(nChar, nRepCnt, nFlags);
}

void CTreeItemEdit::OnKillFocus(CWnd* pNewWnd)
{
	CEdit::OnKillFocus(pNewWnd);
	if (m_birch_ctrl)
		m_birch_ctrl->FireMessageFromEdit();
}
