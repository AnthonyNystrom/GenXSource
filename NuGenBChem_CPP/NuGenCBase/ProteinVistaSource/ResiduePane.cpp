
#include "StdAfx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "ProteinVistaRenderer.h"
#include "ResiduePane.h"
#include "pdb.h"
#include "pdbInst.h"
#include "PDBRenderer.h"
#include "Interface.h"
#include "PDBTreePane.h" 
#include "SelectionListPane.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define		IDC_RESIDUE_LIST_CTRL	(WM_USER+155)
/////////////////////////////////////////////////////////////////////////////
// CResiduePane
BEGIN_MESSAGE_MAP(CResiduePane, CWnd)
	//{{AFX_MSG_MAP(CResiduePane)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_NOTIFY(GVN_SELCHANGED, IDC_RESIDUE_LIST_CTRL, OnSelectionChanged)
	//}}AFX_MSG_MAP
	ON_WM_SETFOCUS()
	ON_WM_DESTROY()
	ON_WM_TIMER()
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CResiduePane message handlers
CResiduePane::CResiduePane()
{
	m_bSelectionChanged = FALSE; 
	m_displayStyle = ONE_RESIDUE; 
	m_residueGridCtrl = new CGridCtrl();
}
CResiduePane::~CResiduePane()
{
   SAFE_DELETE(m_residueGridCtrl);
}
int CResiduePane::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
	if (m_residueGridCtrl->GetSafeHwnd() == 0)
	{
		m_residueGridCtrl->Create(CRect(0, 0, 0, 0), this, IDC_RESIDUE_LIST_CTRL);
	}
	SetTimer(120, 100/2, NULL);
	return 0;
}


void CResiduePane::OnSize(UINT nType, int cx, int cy)
{
	ChangeSize(nType,cx,cy);
}
void CResiduePane::ChangeSize(UINT nType, int cx, int cy)
{
	CWnd::OnSize(nType, cx, cy);
	if (m_residueGridCtrl->GetSafeHwnd())
	{
		m_residueGridCtrl->MoveWindow(0, 0, cx, cy);
	}
}

void CResiduePane::OnSetFocus(CWnd* /*pOldWnd*/)
{
	m_residueGridCtrl->SetFocus();
}
 
HRESULT CResiduePane::OnUpdate() 
{
	m_residueGridCtrl->DeleteAllItems();

	m_residueGridCtrl->SetFixedColumnCount(1);
	m_residueGridCtrl->SetEditable(FALSE);
	m_residueGridCtrl->SetRowResize(FALSE);
	m_residueGridCtrl->SetColumnResize(FALSE);

	long horzMax = 1;
	long vertMax = 0;

	CSTLArrayPDBRenderer & arrayPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer;
	if ( arrayPDBRenderer.size() == 0 )
		return S_OK;

	for ( int iPDB = 0; iPDB < arrayPDBRenderer.size() ; iPDB++ )
	{
		CPDBInst * pPDBInst = arrayPDBRenderer[iPDB]->GetPDBInst();

		for ( int iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
		{
			CModelInst * pModelInst = pPDBInst->m_arrayModelInst[iModel];

			long nChain = pModelInst->m_arrayChainInst.size();
			for ( int iChain = 0 ; iChain < nChain ; iChain ++ )
			{
				CChainInst * pChainInst = pPDBInst->GetChainInst(iModel ,iChain);
				vertMax ++;

				if ( (long)pChainInst->m_arrayResidueInst.size() > horzMax )
				{
					horzMax = pChainInst->m_arrayResidueInst.size();
				}
			}
		}
	}

	m_residueGridCtrl->SetRowCount(max(1,vertMax));
	m_residueGridCtrl->SetColumnCount(horzMax+1);		//	fixed size

	//
	SetGridCtrlText();

	m_residueGridCtrl->AutoSize();
	return S_OK;
}

void CResiduePane::SetGridCtrlText()
{
	CSTLArrayPDBRenderer & arrayPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer;
	if ( arrayPDBRenderer.size() == 0 )
		return;

	long vertIndex = 0;
	for ( int iPDB = 0; iPDB < arrayPDBRenderer.size() ; iPDB++ )
	{
		CPDBInst * pPDBInst = arrayPDBRenderer[iPDB]->GetPDBInst();

		for ( int iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
		{
			CModelInst * pModelInst = pPDBInst->m_arrayModelInst[iModel];

			long nChain = pModelInst->m_arrayChainInst.size();
			for ( int iChain = 0 ; iChain < nChain ; iChain ++ )
			{
				CChainInst * pChainInst = pPDBInst->GetChainInst(iModel ,iChain);

				CGridCellBase * pCell = m_residueGridCtrl->GetCell(vertIndex, 0 );
				if ( pCell != NULL )
				{
					if ( pPDBInst->m_arrayModelInst.size() == 1 )
					{
						pCell->SetText(pChainInst->GetChain()->m_strPDBID);
					}
					else
					{
						CString strChain;
						strChain.Format("[%d]%s", iModel, pChainInst->GetChain()->m_strPDBID);
						pCell->SetText(strChain);
					}
					pCell->SetData((LPARAM)(pChainInst));
				}

				for ( int j = 0 ; j < pChainInst->m_arrayResidueInst.size() ; j++ )
				{
					CGridCellBase * pCell = m_residueGridCtrl->GetCell(vertIndex, j+1);
					if ( pCell != NULL )
					{
						BOOL selectColor = 30;
						 

						long ss = pChainInst->m_arrayResidueInst[j]->GetResidue()->GetSS();
						if ( ss == SS_NONE )
							pCell->SetBackClr(RGB(min(180+selectColor,255), min(180+selectColor,255) , min(180+selectColor,255) ));
						else if ( ss == SS_HELIX )
							pCell->SetBackClr(RGB(min(180+selectColor,255), min(selectColor,255), min(selectColor,255) ));
						else if ( ss == SS_SHEET )
							pCell->SetBackClr(RGB(min(selectColor,255) ,min (180+selectColor, 255) ,min(selectColor,255) ));

						CString strText;
						CResidueInst * pResidueInst = pChainInst->m_arrayResidueInst[j];
						if ( m_displayStyle == 0 )
							strText.Format("%s" , pResidueInst->GetResidue()->m_residueNameOneChar);
						if ( m_displayStyle == 1 )
							strText.Format("%d %s" , pResidueInst->GetResidue()->GetResidueNum(), pResidueInst->GetResidue()->m_residueNameOneChar );
						if ( m_displayStyle == 2 )
							strText.Format("%d(%d) %s" , pResidueInst->GetResidue()->GetResidueNum(), j, pResidueInst->GetResidue()->m_residueNameOneChar );
						if ( m_displayStyle == 3 )
							strText.Format("%s" , pResidueInst->GetResidue()->GetResidueName() );
						if ( m_displayStyle == 4 )
							strText.Format("%d %s" , pResidueInst->GetResidue()->GetResidueNum(), pResidueInst->GetResidue()->GetResidueName() );
						if ( m_displayStyle == 5 )
							strText.Format("%d(%d) %s" , pResidueInst->GetResidue()->GetResidueNum(), j, pResidueInst->GetResidue()->GetResidueName() );
						pCell->SetText(strText);
						pCell->SetData((LPARAM)(pResidueInst));
					}
				}
				vertIndex ++;
			}
		}
	}
}

void CResiduePane::OnDisplaySelected()
{
	//	enum { ONE_RESIDUE, ONE_RESIDUE_INDEX, ONE_RESIDUE_INDEX_NUMBER, THREE_RESIDUE, THREE_RESIDUE_INDEX, THREE_RESIDUE_INDEX_NUMBER };
	m_displayStyle = (++m_displayStyle)%6;
	SetGridCtrlText();
	m_residueGridCtrl->AutoSize();
}

 
void CResiduePane::UpdateResidueFromPDBSelection()
{
	long rowCount = m_residueGridCtrl->GetRowCount();
	long colCount = m_residueGridCtrl->GetColumnCount();
	if( rowCount == 0 || colCount == 0 )
		return;

	long vertIndex = 0;

	long visibleX = -1;
	long visibleY = -1;

	CSTLArrayPDBRenderer & arrayPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer;
	if ( arrayPDBRenderer.size() == 0 )
		return;

	m_residueGridCtrl->EnableSelection();

	for ( int iPDB = 0; iPDB < arrayPDBRenderer.size() ; iPDB++ )
	{
		CPDBInst * pPDBInst = arrayPDBRenderer[iPDB]->GetPDBInst();

		for ( int iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
		{
			CModelInst * pModelInst = pPDBInst->m_arrayModelInst[iModel];

			long nChain = pModelInst->m_arrayChainInst.size();
			for ( int iChain = 0 ; iChain < nChain ; iChain ++ )
			{
				CChainInst * pChainInst = pPDBInst->GetChainInst(iModel ,iChain);

				for ( int j = 0 ; j < pChainInst->m_arrayResidueInst.size() ; j++ )
				{
					long y = vertIndex;
					long x = pChainInst->m_arrayResidueInst[j]->GetResidue()->m_arrayIndex+1;		//	+1: fixed position

					if ( m_pProteinVistaRenderer->m_pLastPickObjectInst == pChainInst->m_arrayResidueInst[j] )
					{
						visibleY = y;
						visibleX = x;
					}

					UINT state = m_residueGridCtrl->GetItemState(y,x);
					if ( pChainInst->m_arrayResidueInst[j]->GetSelect() == TRUE )
						m_residueGridCtrl->SetItemState(y,x, state|GVIS_SELECTED);
					else
						m_residueGridCtrl->SetItemState(y,x, state&(~GVIS_SELECTED));
				}

				vertIndex++;
			}
		}
	}

	if ( visibleX != -1  && visibleY != -1 )
		m_residueGridCtrl->EnsureVisible(visibleY, visibleX);
		
	m_residueGridCtrl->Refresh();		//	Invalidate

	//	pdb 로 부터 데이타를 읽었으므로 다시 읽지 않는다.
	m_bSelectionChanged = FALSE;
}

void CResiduePane::OnSelectionChanged(NMHDR* pNMHDR, LRESULT* pResult)
{;
	NM_GRIDVIEW * pnmgv = (NM_GRIDVIEW *)pNMHDR;
	if (pnmgv!=NULL && m_residueGridCtrl !=NULL && m_residueGridCtrl->GetSelectedCount() > 0 )
	{
		if ( pnmgv->iRow >= 0 && pnmgv->iColumn >= 0 )	
		{
			 CGridCellBase* grid = m_residueGridCtrl->GetCell(pnmgv->iRow,pnmgv->iColumn);
			if(grid==NULL)
				return;
			m_pProteinVistaRenderer->m_pLastPickObjectInst = (CProteinObjectInst*)grid->GetData();
		}else
		{
		
			CCellRange	range = m_residueGridCtrl->GetSelectedCellRange();
			int row = range.GetMinRow();
			int col = range.GetMinCol();

			if ( row >= 0 && col >= 0 )
			{
				m_pProteinVistaRenderer->m_pLastPickObjectInst = (CProteinObjectInst*)m_residueGridCtrl->GetCell(row,col)->GetData();
				//m_pProteinVistaRenderer->m_pLastPickObjectInst->
			}
		}
		m_bSelectionChanged = TRUE;
	}
}

void CResiduePane::UpdatePDBFromGridCtrl()
{
	m_pProteinVistaRenderer->SelectAll(FALSE);

	long yMax = m_residueGridCtrl->GetRowCount();
	long xMax = m_residueGridCtrl->GetColumnCount();

	for ( int y = 0 ; y < yMax ; y++ )
	{
		for ( int x = 1 ; x < xMax ; x++ )		//	fixed position(+1)
		{
			CProteinObjectInst * pObject = (CProteinObjectInst *)m_residueGridCtrl->GetCell(y,x)->GetData();

			if ( pObject != NULL )
			{
				UINT state = m_residueGridCtrl->GetItemState(y,x);
				if ( state & GVIS_SELECTED )
				{
					m_pProteinVistaRenderer->SelectChildren(pObject, TRUE);
				}
			}
		}
	}

 
	 if(GetMainActiveView()->GetCPDBTreePane())
        GetMainActiveView()->GetCPDBTreePane()->UpdateTreeCtrlFromPDBSelection();
 
      m_pProteinVistaRenderer->UpdateAtomSelectionChanged();
		
 
	if ( m_pProteinVistaRenderer->m_pActivePDBRenderer )
		m_pProteinVistaRenderer->m_pActivePDBRenderer->UpdatePDBRendererCenter();

	g_bRequestRender = TRUE;
}


void CResiduePane::OnTimer(UINT_PTR nIDEvent)
{
	if ( m_bSelectionChanged == TRUE )
	{
		UpdatePDBFromGridCtrl();
		m_bSelectionChanged = FALSE;
	}
	CWnd::OnTimer(nIDEvent);
}

void CResiduePane::OnDestroy()
{
	CWnd::OnDestroy();
}

void CResiduePane::Delete()
{
	KillTimer(120);
	m_residueGridCtrl->DestroyWindow();
}
