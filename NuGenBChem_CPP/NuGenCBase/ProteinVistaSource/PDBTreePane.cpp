
#include "stdafx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "PDBTreePane.h"
#include "ResiduePane.h"
#include "vector"
//#include "MltiTree.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//////////////////////////////////////////////////////////////////////////////////////////////////
// CPDBTreePane
//

BEGIN_MESSAGE_MAP(CPDBTreePane, CWnd)
	//{{AFX_MSG_MAP(CPDBTreePane)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_NOTIFY(NM_RCLICK, IDC_TREE_CTRL, OnRclickTreeCtrl)
	ON_NOTIFY(NM_CLICK, IDC_TREE_CTRL, OnClickTreeCtrl)
	ON_NOTIFY(TVN_ITEMEXPANDED, IDC_TREE_CTRL, OnItemExpanded)
	ON_NOTIFY(TVN_ITEMEXPANDING, IDC_TREE_CTRL, OnItemExpanding)
	//}}AFX_MSG_MAP
	ON_COMMAND(IDD_SYNC_PDB, OnFullSyncPDB)
	ON_COMMAND(IDD_SAVE_SELECTION, OnSaveSelection)
	ON_COMMAND(ID_SELECTION_OPTION, OnSelectionOption)
	 
	ON_WM_TIMER()
	ON_WM_DESTROY()
END_MESSAGE_MAP()

AFX_INLINE BOOL  CreateImageList(CImageList& il, UINT nID,CDC *m_DC,long sizeImage = 16)
{
	il.Create(sizeImage, sizeImage, ILC_MASK|ILC_COLOR24, 0, 0);
	CBitmap bmp;
	VERIFY(bmp.LoadBitmap(nID));
	il.Add(&bmp,RGB(0,128,128));
	return TRUE;
}

CPDBTreePane::CPDBTreePane() :m_bSelectionChanged(FALSE)
{
	this->m_residueTreeCtrl = new CTreeCtrl;
}
CPDBTreePane::~CPDBTreePane()
{

}
int CPDBTreePane::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if (CWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
 
	if( !m_residueTreeCtrl->Create(WS_VISIBLE | WS_CHILD | WS_HSCROLL | WS_VSCROLL | TVS_HASBUTTONS | TVS_HASLINES | 
		TVS_LINESATROOT | TVS_SHOWSELALWAYS | TVS_CHECKBOXES , CRect(0, 0, 0, 0), this, IDC_TREE_CTRL))
	{
		return -1;
	}
	CDC * m_CDC=this->GetDC();
	if (!CreateImageList(m_residueImageList, IDB_BITMAP_RESIDUE_TREE,m_CDC))
		return -1;
	m_residueTreeCtrl->SetImageList( &m_residueImageList, TVSIL_NORMAL );
 
	// m_residueTreeCtrl->SetMultiSelect(TRUE);

	SetTimer(200, 100/2, NULL);	//	0.1초/2 = 0.05초
	m_residueTreeCtrl->SetScrollTime(0);

	return 0;
}
 
void CPDBTreePane::OnSize(UINT nType, int cx, int cy)
{
	ChangeSize(nType,cx,cy);
}
void CPDBTreePane::ChangeSize(UINT nType, int cx, int cy)
{
	CWnd::OnSize(nType, cx, cy); 
	if (m_residueTreeCtrl->GetSafeHwnd())
	{
		m_residueTreeCtrl->MoveWindow(0, 0, cx, cy);
	}
}
 
HRESULT	CPDBTreePane::OnUpdate()
{
		 
	InitialUpdateTreeCtrl();
	return S_OK;
}
void CPDBTreePane::Init(CProteinVistaRenderer * pProteinVistaRenderer)
{
	this->m_pProteinVistaRenderer=pProteinVistaRenderer;
	InitialUpdateTreeCtrl();
}
 
void CPDBTreePane::Delete() 
{
	KillTimer(200);
	m_residueTreeCtrl->DestroyWindow();
	m_residueImageList.DeleteImageList();
	SAFE_DELETE(m_residueTreeCtrl);
} 

void CPDBTreePane::InitialUpdateTreeCtrl()
{
	if(m_pProteinVistaRenderer == NULL)
		return;

	m_residueTreeCtrl->DeleteAllItems();
	ResetTreeItem();

	InsertChild(NULL);
	m_residueTreeCtrl->EnsureVisible(m_residueTreeCtrl->GetRootItem());
}	

void CPDBTreePane::ResetTreeItem()
{
	for ( int nPDB = 0 ; nPDB < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); nPDB++ )
	{
		CPDBInst * pPDBInst = m_pProteinVistaRenderer->m_arrayPDBRenderer[nPDB]->GetPDBInst();
		pPDBInst->m_hTreeItem = NULL;

		for ( long iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
		{
			CModelInst * pModelInst = pPDBInst->m_arrayModelInst[iModel];
			pModelInst->m_hTreeItem = NULL;

			for(int nChain = 0; nChain < pModelInst->m_arrayChainInst.size(); nChain++ )
			{
				CChainInst * pChainInst = pModelInst->m_arrayChainInst[nChain];
				pChainInst->m_hTreeItem = NULL;

				for( int iResidue = 0 ; iResidue < pChainInst->m_arrayResidueInst.size() ; iResidue++ )
				{
					CResidueInst *	pResidueInst = pChainInst->m_arrayResidueInst[iResidue];
					pResidueInst->m_hTreeItem = NULL;

					for( int nAtom = 0; nAtom < pResidueInst->m_arrayAtomInst.size() ; nAtom++ )
					{
						CAtomInst * pAtomInst = pResidueInst->m_arrayAtomInst[nAtom];
						pAtomInst->m_hTreeItem = NULL;
					}
				}
			}
		}
	}
}

 
void CPDBTreePane::InsertChild(HTREEITEM hItemParent)
{
	//	Root node.
	if (hItemParent == NULL)
	{
		for(int nPDB = 0 ; nPDB < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); nPDB++)
		{
			CPDBInst * pPDBInst = m_pProteinVistaRenderer->m_arrayPDBRenderer[nPDB]->GetPDBInst();

			CString strPDB = pPDBInst->GetPDB()->m_strPDBID;
			if ( pPDBInst->m_pPDBRenderer->m_pPDBRendererParentBioUnit != NULL )
			{
				CString strBioUnit;
				strBioUnit.Format("-Biomolecule %d" , pPDBInst->m_pPDBRenderer->m_iBioUnit);
				strPDB += strBioUnit;
			}
			
			HTREEITEM htPDB = m_residueTreeCtrl->InsertItem(strPDB, BITMAP_INDEX_PDB, BITMAP_INDEX_PDB);
			m_residueTreeCtrl->SetItemData(htPDB, (DWORD_PTR)pPDBInst);
			pPDBInst->m_hTreeItem = htPDB;
			CheckTreeItem(pPDBInst);

			TVITEM Item;
			Item.hItem = htPDB;
			Item.mask = TVIF_CHILDREN;
			Item.cChildren = 1;
			m_residueTreeCtrl->SetItem(&Item);
		}
		return;
	}

	CProteinObjectInst * pObject = (CProteinObjectInst *)m_residueTreeCtrl->GetItemData(hItemParent);
	CPDBInst * pPDBInst = dynamic_cast<CPDBInst*>(pObject);
	if ( pPDBInst )
	{
		long bAddModel= FALSE;
		for ( int iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
		{
			CModelInst * pModelInst = pPDBInst->m_arrayModelInst[iModel];
			if ( pModelInst->GetModel()->m_bValidTreeCtrl == TRUE )
			{
				CString strModel;
				strModel.Format("Model %d", pModelInst->GetModel()->m_iModel);

				//	node 에 추가한다.
				HTREEITEM htModel = m_residueTreeCtrl->InsertItem(strModel, BITMAP_INDEX_MODEL, BITMAP_INDEX_MODEL, hItemParent);
				m_residueTreeCtrl->SetItemData(htModel, (DWORD_PTR)pModelInst);
				pModelInst->m_hTreeItem = htModel;
				CheckTreeItem(pModelInst);

				TVITEM Item;
				Item.hItem = htModel;
				Item.mask = TVIF_CHILDREN;
				Item.cChildren = pModelInst->m_arrayChainInst.size();
				m_residueTreeCtrl->SetItem(&Item);

				bAddModel= TRUE;
			}
		}

		if ( bAddModel == FALSE )
		{	//	model 이 없어 추가된것이 없다. pPDB->m_arrayChain을 사용
			for ( int iChain = 0 ; iChain < pPDBInst->m_arrayarrayChainInst[0].size() ; iChain ++ )
			{
				CChainInst * pChainInst = pPDBInst->m_arrayarrayChainInst[0][iChain];

				CString strChain;
				strChain.Format("Chain %c", pChainInst->GetChain()->m_chainID);

				HTREEITEM htChain = m_residueTreeCtrl->InsertItem(strChain, BITMAP_INDEX_CHAIN, BITMAP_INDEX_CHAIN, hItemParent);
				m_residueTreeCtrl->SetItemData(htChain, (DWORD_PTR)pChainInst);
				pChainInst->m_hTreeItem = htChain;
				CheckTreeItem(pChainInst);

				TVITEM Item;
				Item.hItem = htChain;
				Item.mask = TVIF_CHILDREN;
				Item.cChildren = pChainInst->m_arrayResidueInst.size();
				m_residueTreeCtrl->SetItem(&Item);
			}
		}
		
		return;
	}
	CModelInst * pModelInst = dynamic_cast<CModelInst*>(pObject);
	if ( pModelInst )
	{
		for ( int iChain = 0 ; iChain < pModelInst->m_arrayChainInst.size() ; iChain ++ )
		{
			CChainInst * pChainInst = pModelInst->m_arrayChainInst[iChain];

			CString strChain;
			strChain.Format("Chain %c", pChainInst->GetChain()->m_chainID);

			HTREEITEM htChain = m_residueTreeCtrl->InsertItem(strChain, BITMAP_INDEX_CHAIN, BITMAP_INDEX_CHAIN, hItemParent);
			m_residueTreeCtrl->SetItemData(htChain, (DWORD_PTR)pChainInst);
			pChainInst->m_hTreeItem = htChain;
			CheckTreeItem(pChainInst);

			TVITEM Item;
			Item.hItem = htChain;
			Item.mask = TVIF_CHILDREN;
			Item.cChildren = pChainInst->m_arrayAtomInst.size();
			m_residueTreeCtrl->SetItem(&Item);
		}
		return;
	}

	CChainInst * pChainInst = dynamic_cast<CChainInst*>(pObject);
	if ( pChainInst )
	{
		for( int iResidue = 0 ; iResidue < pChainInst->m_arrayResidueInst.size() ; iResidue++ )
		{
			CResidueInst *	pResidueInst = pChainInst->m_arrayResidueInst[iResidue];

			CString		strResidue;
			CString		strSS;
			if ( pResidueInst->GetResidue()->GetSS() == SS_NONE )
				strSS = "-";
			else if ( pResidueInst->GetResidue()->GetSS() == SS_HELIX )
				strSS = "H";
			else if ( pResidueInst->GetResidue()->GetSS() == SS_SHEET )
				strSS = "S";
			strResidue.Format("[%03d] [%s] %s",pResidueInst->GetResidue()->GetResidueNum(), strSS, pResidueInst->GetResidue()->GetResidueName());

			HTREEITEM htResidue = m_residueTreeCtrl->InsertItem(strResidue, BITMAP_INDEX_RESIDUE, BITMAP_INDEX_RESIDUE, hItemParent);
			m_residueTreeCtrl->SetItemData(htResidue, (DWORD_PTR)pResidueInst);
			pResidueInst->m_hTreeItem= htResidue;
			CheckTreeItem(pResidueInst);

			TVITEM Item;
			Item.hItem = htResidue;
			Item.mask = TVIF_CHILDREN;
			Item.cChildren = pResidueInst->m_arrayAtomInst.size();
			m_residueTreeCtrl->SetItem(&Item);
		}

		return;
	}
	CResidueInst * pResidueInst = dynamic_cast<CResidueInst*>(pObject);
	if ( pResidueInst )
	{
		// Atom 추가
		for( int nAtom = 0; nAtom < pResidueInst->m_arrayAtomInst.size() ; nAtom++ )
		{
			CAtomInst * pAtomInst = pResidueInst->m_arrayAtomInst[nAtom];

			CString strAtom;
			strAtom.Format("[%d] %s (%.2f, %.2f, %.2f)", pAtomInst->GetAtom()->m_serial, pAtomInst->GetAtom()->m_atomName, 
														pAtomInst->GetAtom()->m_posOrig.x, pAtomInst->GetAtom()->m_posOrig.y, pAtomInst->GetAtom()->m_posOrig.z );

			// Atom 속성이 SideChain인 경우 아이콘을 다르게 표시
			if ( pAtomInst->GetAtom()->m_bHETATM == FALSE )
				pAtomInst->m_hTreeItem = m_residueTreeCtrl->InsertItem(strAtom, BITMAP_INDEX_ATOM, BITMAP_INDEX_ATOM, hItemParent);
			else
				pAtomInst->m_hTreeItem = m_residueTreeCtrl->InsertItem(strAtom, BITMAP_INDEX_HETATM, BITMAP_INDEX_HETATM, hItemParent);

			// 추가된 Atom 트리 아이템에 PDB Atom 포인터 연결
			m_residueTreeCtrl->SetItemData(pAtomInst->m_hTreeItem , (DWORD_PTR)pAtomInst);
			CheckTreeItem(pAtomInst);
		}
		return;
	}
}

void CPDBTreePane::OnItemExpanding(NMHDR* pNMHDR, LRESULT* pResult)
{
	NMTREEVIEW * nmTreeView = (NMTREEVIEW *)pNMHDR;
	if ( nmTreeView->action == TVE_EXPAND )
	{
		HTREEITEM hItem = nmTreeView->itemNew.hItem;
		if ( m_residueTreeCtrl->GetChildItem(hItem) == NULL )
		{
			InsertChild(hItem);
		}
	}
	*pResult = 0;
}
 
//////////////////////////////////////////////////////////////////////////
 
void CPDBTreePane::UpdateTreeCtrlFromPDBSelection()
{
	OnSyncPDB();

	if ( m_pProteinVistaRenderer->m_pLastPickObjectInst != NULL )
	{
		//m_residueTreeCtrl->SelectAll(FALSE);
		long state = 0;
		if ( m_pProteinVistaRenderer->m_pLastPickObjectInst->GetSelect() == TRUE )
			state = TVIS_SELECTED;

		//	화면에 보이게 함.
		if ( m_pProteinVistaRenderer->m_pLastPickObjectInst->m_hTreeItem == NULL )
		{
			//	tree 가 펼쳐지지 않아서 NULL 이다. Tree를 펼쳐야 한다.
			CSTLArrayProteinnObjectInst objectParent;
			
			CProteinObjectInst * pObjectParentInst = m_pProteinVistaRenderer->m_pLastPickObjectInst;
			while(1)
			{
				pObjectParentInst = pObjectParentInst->m_pParent;
				if ( pObjectParentInst == NULL )
					break;
				objectParent.push_back(pObjectParentInst);
			}

			for ( int i = objectParent.size()-1 ; i >= 0 ; i-- )
			{
				m_residueTreeCtrl->Expand(objectParent[i]->m_hTreeItem, TVE_EXPAND);
			}
		}

		if ( m_pProteinVistaRenderer->m_pLastPickObjectInst->m_hTreeItem )
		{	//	tree를 펼쳐도 여전히 m_hTreeItem가 0인 경우가 있을때를 방지.
			m_residueTreeCtrl->SetItemState( m_pProteinVistaRenderer->m_pLastPickObjectInst->m_hTreeItem, LIS_FOCUSED|state, LIS_FOCUSED|TVIS_SELECTED );	
		}
	}
}

BOOL CPDBTreePane::CheckTreeItem(CProteinObjectInst * pObjectInst)
{
	return	m_residueTreeCtrl->SetCheck(pObjectInst->m_hTreeItem, pObjectInst->GetSelect());
}

 
void CPDBTreePane::OnSyncPDB()
{	
	if(m_pProteinVistaRenderer == NULL)
		return;

	//	TVN_ITEMEXPANDED 를 이용해서, 화면에 보이는것만 Selection marking을 한다.
	HTREEITEM hItem = m_residueTreeCtrl->GetRootItem();
	while (hItem)
	{
		//	arrayTreeItem.push_back(hItem);
		CProteinObjectInst* pObjectInst = (CProteinObjectInst *)m_residueTreeCtrl->GetItemData(hItem);
		m_residueTreeCtrl->SetCheck(hItem, pObjectInst->GetSelect());

		hItem = m_residueTreeCtrl->GetNextVisibleItem(hItem);
	}
}

 
void CPDBTreePane::OnFullSyncPDB()
{
	long	iProgress = GetMainActiveView()->InitProgress(100);

	for ( int nPDB = 0 ; nPDB < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); nPDB++ )
	{
		GetMainActiveView()->SetProgress( (nPDB+1)*100/m_pProteinVistaRenderer->m_arrayPDBRenderer.size() , iProgress);
		CPDBInst * pPDBInst = m_pProteinVistaRenderer->m_arrayPDBRenderer[nPDB]->GetPDBInst();

		long nModelSelected = 0;
		for ( long iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
		{
			CModelInst * pModelInst = pPDBInst->m_arrayModelInst[iModel];

			long nChainSelected = 0;
			for(int nChain = 0; nChain < pModelInst->m_arrayChainInst.size(); nChain++ )
			{
				CChainInst * pChainInst = pModelInst->m_arrayChainInst[nChain];

				long nResidueSelected = 0;
				for( int iResidue = 0 ; iResidue < pChainInst->m_arrayResidueInst.size() ; iResidue++ )
				{
					CResidueInst *	pResidueInst = pChainInst->m_arrayResidueInst[iResidue];

					long nAtomSelected = 0;
					for( int nAtom = 0; nAtom < pResidueInst->m_arrayAtomInst.size() ; nAtom++ )
					{
						CAtomInst * pAtomInst = pResidueInst->m_arrayAtomInst[nAtom];
						CheckTreeItem(pAtomInst);

						if ( pAtomInst->GetSelect() == TRUE )
							nAtomSelected++;
					}

					if ( nAtomSelected == pResidueInst->m_arrayAtomInst.size() )
						pResidueInst->SetSelect(TRUE);

					CheckTreeItem(pResidueInst);

					if ( pResidueInst->GetSelect() == TRUE )
						nResidueSelected++;
				}

				if( (nResidueSelected == pChainInst->m_arrayResidueInst.size()) && (pChainInst->m_arrayResidueInst.size() > 0))
					pChainInst->SetSelect(TRUE);

				CheckTreeItem(pChainInst);

				if ( pChainInst->GetSelect() == TRUE )
					nChainSelected ++;
			}
			if ( nChainSelected == pModelInst->m_arrayChainInst.size() )
				pModelInst->SetSelect(TRUE);

			if ( pModelInst->GetModel()->m_bValidTreeCtrl == TRUE )
				CheckTreeItem(pModelInst);

			if ( pModelInst->GetSelect() == TRUE )
				nModelSelected ++;
		}

		if ( nModelSelected == pPDBInst->m_arrayModelInst.size() )
			pPDBInst->SetSelect(TRUE);

		CheckTreeItem(pPDBInst);
	}
	GetMainActiveView()-> EndProgress(iProgress);
}
 
void CPDBTreePane::OnSaveSelection()
{
	CSTLArraySelectionInst selection;
	for ( int nPDB = 0 ; nPDB < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); nPDB++ )
	{
		CPDBInst * pPDBInst = m_pProteinVistaRenderer->m_arrayPDBRenderer[nPDB]->GetPDBInst();

		pPDBInst->GetSelectNodeChild(selection);
	}
	if ( selection.size() > 0 )
		SaveSelection(selection);
}
 
void CPDBTreePane::OnSelectionOption()
{
	 
}
 

void CPDBTreePane::OnTimer(UINT_PTR nIDEvent)
{
	if ( m_bSelectionChanged == TRUE )
	{
		OnSyncPDB();
		GetMainActiveView()->GetResiduePanel()->UpdateResidueFromPDBSelection();

		m_pProteinVistaRenderer->UpdateAtomSelectionChanged();

		if ( m_pProteinVistaRenderer->m_pActivePDBRenderer )
			m_pProteinVistaRenderer->m_pActivePDBRenderer->UpdatePDBRendererCenter();

		m_bSelectionChanged = FALSE;
		g_bRequestRender = TRUE;
	}

	CWnd::OnTimer(nIDEvent);
}

void CPDBTreePane::OnDestroy()
{
	CWnd::OnDestroy();
}
void CPDBTreePane::OnItemExpanded(NMHDR* pNMHDR, LRESULT* pResult)
{
	NMTREEVIEW * nmTreeView = (NMTREEVIEW *)pNMHDR;
	if ( nmTreeView->action == TVE_EXPAND )
	{
		OnSyncPDB();
	}
	*pResult = 0;
}

void CPDBTreePane::OnClickTreeCtrl(NMHDR* pNMHDR, LRESULT* pResult)
{
	HTREEITEM         hTreeItemClick;
	CPoint            ptCur;
	TVHITTESTINFO     tvht;  
	CString           strText;

	GetCursorPos( &ptCur );
	m_residueTreeCtrl->ScreenToClient((LPPOINT)&ptCur );

	tvht.pt.x = ptCur.x; 
	tvht.pt.y = ptCur.y; 

	UINT uFlags=0;
	hTreeItemClick = m_residueTreeCtrl->HitTest( ptCur, &uFlags );

	//	click check box
	if( uFlags & TVHT_ONITEMSTATEICON )
	{
		ClickTreeCtrl(hTreeItemClick);
	}

	*pResult = 0;
}

void CPDBTreePane::ClickTreeCtrl(HTREEITEM hTreeItemClick)
{
	UINT state = m_residueTreeCtrl->GetItemState(hTreeItemClick, TVIS_SELECTED );
	if ( (state & TVIS_SELECTED) == 0 )
	{	//	현재꺼가 새로운것의 check를 한것임. -> 기존것을 전부 지우고 하나만 select
		//m_residueTreeCtrl->SelectAll(FALSE);
		m_residueTreeCtrl->SelectItem(hTreeItemClick);
	}
 
	m_pProteinVistaRenderer->m_pLastPickObjectInst = (CProteinObjectInst *)m_residueTreeCtrl->GetItemData(hTreeItemClick);
	BOOL bCheckClick = m_residueTreeCtrl->GetCheck(hTreeItemClick);

	HTREEITEM hItem =m_residueTreeCtrl->GetSelectedItem();//GetFirstSelectedItem();
	while (hItem)
	{
		CProteinObjectInst* pObject = (CProteinObjectInst *)m_residueTreeCtrl->GetItemData(hItem);

		if ( bCheckClick == TRUE )
		{	//	TRUE->FALSE 일 경우에 parent의 check를 전부 없애준다.
			CProteinObjectInst* pObjectClick = (CProteinObjectInst *)m_residueTreeCtrl->GetItemData(hItem);
			pObjectClick->SetSelect(FALSE);

			while(1)
			{
				CProteinObjectInst* pObjectParent = pObjectClick->m_pParent;
				if ( pObjectParent == NULL )
					break;

				pObjectParent->SetSelect(FALSE);
				pObjectClick = pObjectParent;
			}
		}

		pObject->SetSelectChild(!bCheckClick);
		break;
		//hItem = m_residueTreeCtrl->GetNextSelectedItem(hItem);
	}

	m_bSelectionChanged = TRUE;
}

void CPDBTreePane::SetParentUnselect (CProteinObjectInst * pObjectClick)
{
	while(1)
	{
		CProteinObjectInst * pObjectParent = pObjectClick->m_pParent;
		if ( pObjectParent == NULL )
			break;

		pObjectParent->SetSelect(FALSE);
		pObjectClick = pObjectParent;
	}
}

void CPDBTreePane::OnRclickTreeCtrl(NMHDR* pNMHDR, LRESULT* pResult)
{
	CPoint ptMenu ;
	::GetCursorPos(&ptMenu);

	ShowSelectionOptionMenu(ptMenu);

	*pResult = 0;
}

void CPDBTreePane::ShowSelectionOptionMenu(POINT pt)
{
	CPoint ptMenu(pt);

	CMenu menu;
	menu.LoadMenu(IDR_MENU_TREE_SELECTION_POPUP);

	//// track menu
	//int nMenuResult = CXTPCommandBars::TrackPopupMenu(menu.GetSubMenu(0), TPM_NONOTIFY | TPM_RETURNCMD | TPM_LEFTALIGN |TPM_RIGHTBUTTON, ptMenu.x, ptMenu.y, this, NULL);

	//if ( nMenuResult == ID_SELECT_SPLIT_NEW_PDB )
	//{
	//	OnSaveSelection();
	//}
	//else
	//if ( nMenuResult == ID_CENTER_SELECTED_MOLECULE )
	//{
	//	m_pProteinVistaRenderer->SetCameraAnimation();
	//}
	//else
	//if ( nMenuResult != 0 )
	//{
	//	m_bSelectionChanged = FALSE;
	//	m_pProteinVistaRenderer->SelectSpecificAtoms(nMenuResult);
	//}
}

void CPDBTreePane::SaveSelection(CSTLArraySelectionInst &selection)
{
	CString filename = GetMainApp()->m_strBaseSavePDBSelection + "*.ent";

	if ( selection.size() == 1 )
	{
		CChain * pChain = dynamic_cast<CChain*>(selection[0]);
		if ( pChain )
		{
			filename = GetMainApp()->m_strBaseSavePDBSelection + pChain->m_strPDBID + ".ent";
		}
	}
     AFX_MANAGE_STATE(AfxGetStaticModuleState());
	// pdb파일을 저장할 디렉토리 설정
	static char BASED_CODE szFilter[] = "PDB Files (*.ent)|*.ent||";
	CFileDialog	fileDialog(FALSE, "ent", filename , OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, szFilter , this, 0);
	if ( fileDialog.DoModal() == IDCANCEL )
		return;

	CStdioFile file (fileDialog.GetPathName(), CFile::modeCreate | CFile::modeWrite | CFile::typeText );

	TCHAR buff[1024];
	for ( int iSelection = 0 ; iSelection < selection.size() ; iSelection ++ )
	{ 
		CSTLArrayAtomInst selectAtom;
		selection[iSelection]->GetChildAtoms(selectAtom);
		for ( int i = 0 ; i < selectAtom.size() ; i++ )
		{
			CAtomInst * pAtomInst = selectAtom[i];

			sprintf(buff, "%-6s%5d %4s%c%3s %c%4d    %8.3f%8.3f%8.3f%6.2f%6.2f              \n", 
									"ATOM",
									pAtomInst->GetAtom()->m_serial,
									pAtomInst->GetAtom()->m_atomName,
									pAtomInst->GetAtom()->m_altLoc,
									pAtomInst->GetAtom()->m_residueName,
									pAtomInst->GetAtom()->m_chainID,
									pAtomInst->GetAtom()->m_residueNum,
									pAtomInst->GetAtom()->m_pos.x,
									pAtomInst->GetAtom()->m_pos.y,
									pAtomInst->GetAtom()->m_pos.z,
									pAtomInst->GetAtom()->m_occupancy,
									pAtomInst->GetAtom()->m_temperature );

			file.WriteString(buff);
		}

	}

	file.WriteString("END\r\n");

	file.Close();

	sprintf(buff, "저장된 %s를 현재 화면에 추가하시겠습니까?" , fileDialog.GetFileName());
	if ( AfxMessageBox(buff, MB_YESNOCANCEL) == IDYES )
		GetMainActiveView()->AddPDB(fileDialog.GetPathName());
}

void CPDBTreePane::ItemExpandToChain(HTREEITEM hItem)
{
	HTREEITEM itemRoot = hItem;
	if ( itemRoot == NULL )
	{
		HTREEITEM hNextItem;
		itemRoot = m_residueTreeCtrl->GetRootItem();
		while (itemRoot != NULL)
		{
			ItemExpandToChain(itemRoot);
			hNextItem = m_residueTreeCtrl->GetNextItem(itemRoot, TVGN_NEXT);
			itemRoot = hNextItem;
		}

		return;
	}

	CProteinObjectInst* pObjectInstClick = (CProteinObjectInst*)(m_residueTreeCtrl->GetItemData(itemRoot));
	if ( pObjectInstClick == NULL || pObjectInstClick->GetName().Find(_T("CHAIN")) != -1 )	return;

	m_residueTreeCtrl->Expand(itemRoot, TVE_EXPAND);

	if (m_residueTreeCtrl->ItemHasChildren(itemRoot))
	{
		HTREEITEM hNextItem;
		HTREEITEM hChildItem = m_residueTreeCtrl->GetChildItem(itemRoot);

		while (hChildItem != NULL)
		{
			ItemExpandToChain(hChildItem);
			hNextItem = m_residueTreeCtrl->GetNextItem(hChildItem, TVGN_NEXT);
			hChildItem = hNextItem;
		}
	}
}

