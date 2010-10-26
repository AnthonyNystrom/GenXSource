#include "stdafx.h"
#include "pdb.h"
#include "pdbInst.h"
#include "SelectionListPane.h"
#include "ProteinVistaRenderer.h"
#include "PDBRenderer.h"
#include "ProteinVistaView.h"
#include "Interface.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


IMPLEMENT_DYNCREATE(CSelectionListPane, CWnd)

/////////////////////////////////////////////////////////////////////////////
// CSelectionListPane
BEGIN_MESSAGE_MAP(CSelectionListPane, CWnd)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_SETFOCUS()
	ON_COMMAND(ID_TOOLBAR_SHOWALL, OnShowAll)
 	ON_COMMAND(ID_TOOLBAR_HIDEALL, OnHideAll)	 
	ON_COMMAND(ID_TOOLBAR_DELETE, OnDelete)
 	ON_COMMAND(ID_TOOLBAR_DESELECT_ALL, OnDeselect)
 	ON_COMMAND(ID_TOOLBAR_UNION, OnUnion)
	ON_COMMAND(ID_TOOLBAR_INTERSECT, OnIntersect)
 	ON_COMMAND(ID_TOOLBAR_SUBTRACT, OnSubtract)
 	ON_COMMAND(ID_TOOLBAR_RESULT, OnResult)
 	ON_COMMAND(ID_TOOLBAR_UP, OnCurrentUp )
	ON_COMMAND(ID_TOOLBAR_DOWN, OnCurrentDown)
	ON_COMMAND(ID_TOOLBAR_CENTER_CURRENT_SELECTION, OnCenterCurrentVP )
 
	ON_NOTIFY(HTMLLIST_DESELECTED, ID_HTML_LIST_CTRL, OnListDeSelected)
	ON_NOTIFY(HTMLLIST_SELECTED, ID_HTML_LIST_CTRL, OnListSelected)
	ON_NOTIFY(HTMLLIST_ITEMCHECKED, ID_HTML_LIST_CTRL, OnListItemChecked)
	ON_NOTIFY(HTMLLIST_LBUTTONDBLCLICK, ID_HTML_LIST_CTRL, OnListItemDBLClick)
END_MESSAGE_MAP()
AFX_INLINE BOOL  CreateImageListA(CImageList& il, UINT nID,CDC* m_DC, long sizeImage = 16)
{
	il.Create(sizeImage, sizeImage, ILC_MASK|ILC_COLOR24, 0, 0);//4，1
	CBitmap bmp;
	VERIFY(bmp.LoadBitmap(nID));
	il.Add(&bmp,RGB(255,0,255)); 
 
	return TRUE;
}
CSelectionListPane::CSelectionListPane()
{
	m_pBooleanSelectionDisplay = NULL;
	m_selectOperation = 0;
	m_htmlListCtrl = new CHTMLListCtrl();
}

void CSelectionListPane::UpdateSelectFromResidue(CString pdbName)
{
}
HRESULT CSelectionListPane::OnUpdate()
{
	m_htmlListCtrl->DeleteAllItems();
	if ( m_pProteinVistaRenderer == NULL )
		return E_FAIL;
	CSTLArraySelectionDisplay& selctionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay;
	for ( int iSerial = 0 ; iSerial < CSelectionDisplay::m_maxSelectionIndex ; iSerial ++ )
	{
		for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
		{
			CSelectionDisplay * pSelectionDisplay = selctionDisplay[i];
			if ( pSelectionDisplay != NULL )
			{
				  
				if ( pSelectionDisplay->m_iSerial == iSerial )
				{
					CString strHTML;
					MakeSelectionPaneText(iSerial, pSelectionDisplay, strHTML);
					
					long pos = m_htmlListCtrl->InsertItem(strHTML ,selctionDisplay[i]->m_displayStyle ,HTML_TEXT);
					m_htmlListCtrl->SetItemCheck(pos, selctionDisplay[i]->m_bShow);
					m_htmlListCtrl->SetItemData ( pos, (LPARAM)selctionDisplay[i] );

					selctionDisplay[i]->m_iDisplaySelectionList = pos;
               
					break;
				}
			}
		}
	}
	m_htmlListCtrl->Invalidate();
	return S_OK;
}

void CSelectionListPane::Delete()
{
	m_ImageList.DeleteImageList();
	m_htmlListCtrl->DestroyWindow();
}

void CSelectionListPane::MakeSelectionPaneText(long index, CSelectionDisplay * pSelectionDisplay, CString &strHTML)
{
	CPDBInst * pPDBInst = pSelectionDisplay->m_pPDBRenderer->GetPDBInst();
	CString strName;
	if ( pSelectionDisplay->m_selectionInst.size() == 1 )
	{	//	selection이 1개이면, model, chain 등 이름을 붙일수 있다.
		CProteinObjectInst * pObject = pSelectionDisplay->m_selectionInst[0];
		strName = pObject->GetName();
	}

	CString str1;
	str1.Format("<font color=#ff0000>%s<p>", pSelectionDisplay->GetPropertyCommon()->m_strSelectionName );

	CString str2;
	if ( strName.IsEmpty() != TRUE )
		str2.Format("[%s]</font><p>", strName );
	else
		str2.Format("</font><p>");

	CString str3;
	str3.Format("Filename: %s<p>", pPDBInst->GetPDB()->m_strFilename);

	CString str4;
	str4.Format("Number of Selected Atoms: %d", pSelectionDisplay->m_arrayAtomInst.size() );

	strHTML = str1 + str2 + str3 + str4;
}

/////////////////////////////////////////////////////////////////////////////
// CSelectionListPane message handlers

int CSelectionListPane::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
	if (m_htmlListCtrl->GetSafeHwnd() == 0)
	{
		CDC* m_DC =this->GetDC();
		CreateImageListA(m_ImageList, IDR_TOOLBAR_MOLECULE_HIRES,m_DC, 40);

		m_htmlListCtrl->Create(this,CRect(0,0,0,0),ID_HTML_LIST_CTRL);
		m_htmlListCtrl->SetImageList(&m_ImageList);
		m_htmlListCtrl->SetExtendedStyle(HTMLLIST_STYLE_GRIDLINES|HTMLLIST_STYLE_CHECKBOX|HTMLLIST_STYLE_IMAGES);
	}
	return 0;
}

void CSelectionListPane::OnSize(UINT nType, int cx, int cy)
{
	ChangeSize(nType, cx, cy);	 
}
void CSelectionListPane::ChangeSize(UINT nType, int cx, int cy)
{
	CWnd::OnSize(nType, cx, cy);
	if (m_htmlListCtrl->GetSafeHwnd())
	{
		m_htmlListCtrl->MoveWindow(0, 0, cx, cy);
	}
}
void CSelectionListPane::OnSetFocus(CWnd* /*pOldWnd*/)
{
	m_htmlListCtrl->SetFocus();
}
 
void CSelectionListPane::OnShowAll()
{
	for ( int i = 0 ; i < m_htmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)m_htmlListCtrl->GetItemData(i);
		pSelectionDisplay->m_bShow = TRUE;
		m_htmlListCtrl->SetItemCheck(i, TRUE);
		//pSelectionDisplay->GetPropertyCommon()->m_pItempShow->SetBool(pSelectionDisplay->m_bShow);
	}

	m_htmlListCtrl->Invalidate();

	g_bRequestRender = TRUE;
}

void CSelectionListPane::OnHideAll()
{
	for ( int i = 0 ; i < m_htmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)m_htmlListCtrl->GetItemData(i);
		pSelectionDisplay->m_bShow = FALSE;
		m_htmlListCtrl->SetItemCheck(i, FALSE);
		//pSelectionDisplay->GetPropertyCommon()->m_pItempShow->SetBool(pSelectionDisplay->m_bShow);
	}

	m_htmlListCtrl->Invalidate();
	g_bRequestRender = TRUE;
}


void CSelectionListPane::OnDelete()
{
	long pos = m_htmlListCtrl->GetSelectedItem();

	CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->GetCurrentSelectionDisplay();

	//	연산을 위해 저장된 pointer 지워질때 반드시 확인한다.
	if ( m_pBooleanSelectionDisplay == pSelectionDisplay )
		m_pBooleanSelectionDisplay = NULL;

	DeleteSelectionDisplay(pSelectionDisplay);

	//	
	m_htmlListCtrl->DeleteItem(pos);

	//	전부 DESELECT 한다.
	OnDeselect();

	//	다음 pos을 구한후 이것을 선택.
	if ( m_htmlListCtrl->GetItemCount() != 0 )
	{
		pos = min(pos, m_htmlListCtrl->GetItemCount()-1);
		SelectListItem(pos);
		m_pProteinVistaRenderer->UpdateAtomSelectionChanged();
	}

	g_bRequestRender = TRUE;
}

void CSelectionListPane::DeleteSelectionDisplay(CSelectionDisplay * pSelectionDisplay)
{
	if ( pSelectionDisplay == NULL )
		return;
	if ( m_pProteinVistaRenderer == NULL )
		return;

	//	연산을 위해 저장된 pointer 지워질때 반드시 확인한다.
	if ( m_pBooleanSelectionDisplay == pSelectionDisplay )
		m_pBooleanSelectionDisplay = NULL;

	CSTLArraySelectionDisplay& arraySelctionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay;

	long index = pSelectionDisplay->m_iDisplayStylePDB;
	arraySelctionDisplay[index] = NULL;

	//	
	SetRenderProperty(NULL);

	//	CPDB의 index의 모든값을 0으로 만든다.
	for ( int i = 0 ; i < m_pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
	{
		m_pProteinVistaRenderer->m_arrayPDBRenderer[i]->GetPDBInst()->SetDisplayStyleChild(index, FALSE);
	}

	delete pSelectionDisplay;
}


void CSelectionListPane::OnListItemChecked(NMHDR * pNotifyStruct, LRESULT * result )
{
	NM_HTMLLISTCTRL * nmListCtrl = (NM_HTMLLISTCTRL *)pNotifyStruct;

	//	TRACE("Check:%d\n", nmListCtrl->nItemNo);
	CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)m_htmlListCtrl->GetItemData(nmListCtrl->nItemNo);
	pSelectionDisplay->m_bShow = nmListCtrl->bChecked;
	//pSelectionDisplay->GetPropertyCommon()->m_pItempShow->SetBool(pSelectionDisplay->m_bShow);
	g_bRequestRender = TRUE;
	if(pSelectionDisplay->m_bShow==TRUE)
	{
		 OnListSelected(pNotifyStruct,result);
	}
	else
	{
		OnListDeSelected(pNotifyStruct,result);
	}

	
}

void CSelectionListPane::OnListSelected( NMHDR * pNotifyStruct, LRESULT * result )
{
	NM_HTMLLISTCTRL * nmListCtrl = (NM_HTMLLISTCTRL *)pNotifyStruct;
	//	TRACE("Select:%d\n", nmListCtrl->nItemNo);

	SelectListItem(nmListCtrl->nItemNo);

	m_pProteinVistaRenderer->UpdateAtomSelectionChanged();

	g_bRequestRender = TRUE;
}

void CSelectionListPane::OnListDeSelected( NMHDR * pNotifyStruct, LRESULT * result )
{
	NM_HTMLLISTCTRL * nmListCtrl = (NM_HTMLLISTCTRL *)pNotifyStruct;

	OnDeselect();

	g_bRequestRender = TRUE;
}

  
void CSelectionListPane::DeselectPaneItem()
{
	CSTLArraySelectionDisplay& selctionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay;
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = selctionDisplay[i];
		if ( pSelectionDisplay != NULL )
		{
			pSelectionDisplay->m_bSelect = FALSE;
		}
	}

	SetRenderProperty(NULL);
	 
	m_htmlListCtrl->SetSelectedItem(-1);
	m_htmlListCtrl->Invalidate(TRUE);

	if ( m_pProteinVistaRenderer->m_pActivePDBRenderer )
	{
		m_pProteinVistaRenderer->m_pActivePDBRenderer->UpdatePDBRendererCenter();
	}
}

void CSelectionListPane::OnDeselect()
{
	//    여기 안에서 DeselectPaneItem 가 불린다.
	m_pProteinVistaRenderer->DeSelectAllAtoms();
}


void CSelectionListPane::OnListItemDBLClick( NMHDR * pNotifyStruct, LRESULT * result )
{
	long pos = m_htmlListCtrl->GetSelectedItem();
	if ( pos == NONE_SELECTED )
		return;

	CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)m_htmlListCtrl->GetItemData(pos);
	
	//	Display property dialogbox


}

void CSelectionListPane::SelectListItem(CSelectionDisplay * pSelectionDisplay)
{
	if(pSelectionDisplay==NULL)
		return;
	pSelectionDisplay->m_bSelect = TRUE;

	long nSelect = GetListCtrlIndex(pSelectionDisplay);

	//	PDB를 선택하게 한다.
	for ( int i = 0 ; i < pSelectionDisplay->m_selectionInst.size() ; i++ )
	{
		CProteinObjectInst * pObject = pSelectionDisplay->m_selectionInst[i];
		m_pProteinVistaRenderer->SelectChildren(pObject, TRUE);
	}

	//	3d Rendering 정보를 설정.
	m_pProteinVistaRenderer->SetActivePDBRenderer(pSelectionDisplay->m_pPDBRenderer);
	pSelectionDisplay->m_pPDBRenderer->UpdatePDBRendererCenter();

	m_htmlListCtrl->SetSelectedItem(nSelect);
	m_htmlListCtrl->Invalidate(TRUE);
 
	m_pProteinVistaRenderer->UpdateSelectionInfoPane();
 
	SetRenderProperty(pSelectionDisplay);

	g_bRequestRender = TRUE;
}

void CSelectionListPane::SelectListItem(long n)
{
	m_pProteinVistaRenderer->SelectAll(FALSE);
	DeselectPaneItem();

	CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)m_htmlListCtrl->GetItemData(n);
	SelectListItem(pSelectionDisplay);

	g_bRequestRender = TRUE;
}

int	CSelectionListPane::GetListCtrlIndex(CSelectionDisplay * pSelectionDisplay)
{
	long iList = -1;
	for ( int i = 0 ; i < m_htmlListCtrl->GetItemCount() ; i++ )
	{
		if ( pSelectionDisplay == (CSelectionDisplay *)(m_htmlListCtrl->GetItemData(i)) )
		{
			iList = i;
			break;
		}
	}

	return iList;
}

void CSelectionListPane::SetRenderProperty(CSelectionDisplay * pSelectionDisplay)
{
	if(pSelectionDisplay )
	{
	   GetMainActiveView()->RefreshProptery(pSelectionDisplay,pSelectionDisplay->m_displayStyle);
	}
	else
	{
		GetMainActiveView()->RefreshProptery(pSelectionDisplay,-1);
	}

}

void CSelectionListPane::OnUnion()
{
	m_pBooleanSelectionDisplay = m_pProteinVistaRenderer->GetCurrentSelectionDisplay();
	m_selectOperation = UNION;
}

void CSelectionListPane::OnIntersect()
{
	m_pBooleanSelectionDisplay = m_pProteinVistaRenderer->GetCurrentSelectionDisplay();
	m_selectOperation = INTERSECT;
}

void CSelectionListPane::OnSubtract()
{
	m_pBooleanSelectionDisplay = m_pProteinVistaRenderer->GetCurrentSelectionDisplay();
	m_selectOperation = SUBTRACT;
}

void CSelectionListPane::OnResult()
{
	CSelectionDisplay * pCurrentSelection = m_pProteinVistaRenderer->GetCurrentSelectionDisplay();

	SelectionOperation(m_pBooleanSelectionDisplay, pCurrentSelection, m_selectOperation );

	m_pBooleanSelectionDisplay = NULL;
	m_selectOperation = 0;
}

void CSelectionListPane::SelectionOperation(CSelectionDisplay * sel1, CSelectionDisplay * sel2, int op)
{
	OnDeselect();

	CSTLArrayAtomInst & arrayAtom1 = sel1->m_arrayAtomInst;
	CSTLArrayAtomInst & arrayAtom2 = sel2->m_arrayAtomInst;

	if ( op == UNION )
	{
		for ( int i = 0; i < arrayAtom1.size() ; i++ )
		{
			arrayAtom1[i]->m_bSelect= TRUE;
		}

		for ( int i = 0; i < arrayAtom2.size() ; i++ )
		{
			arrayAtom2[i]->m_bSelect= TRUE;
		}
	}
	else
	if ( op == INTERSECT )
	{
		CSTLArrayAtomInst stlArrayAtom;
		stlArrayAtom.reserve(1000);

		for ( int i = 0; i < arrayAtom1.size() ; i++ )
		{
			arrayAtom1[i]->m_bSelect= TRUE;
		}

		for ( int i = 0; i < arrayAtom2.size() ; i++ )
		{
			if ( arrayAtom2[i]->m_bSelect == TRUE )
				stlArrayAtom.push_back(arrayAtom2[i]);
		}

		m_pProteinVistaRenderer->SelectAll(FALSE);

		for ( int i = 0; i < stlArrayAtom.size() ; i++ )
		{
			stlArrayAtom[i]->m_bSelect= TRUE;
		}
	}
	else
	if ( op == SUBTRACT )
	{
		for ( int i = 0; i < arrayAtom1.size() ; i++ )
		{
			arrayAtom1[i]->m_bSelect = TRUE;
		}

		for ( int i = 0; i < arrayAtom2.size() ; i++ )
		{
			arrayAtom2[i]->m_bSelect = FALSE;
		}
	}

	m_pProteinVistaRenderer->SelectedAtomApply();
}
 	
void CSelectionListPane::OnCurrentUp()
{
	int indexSelected = m_htmlListCtrl->GetSelectedItem();
	if(  indexSelected == NONE_SELECTED || m_htmlListCtrl->GetItemCount() <= 1 || indexSelected == 0 )
		return;

	CSelectionDisplay * pCurrentSelectionDisplay = m_pProteinVistaRenderer->GetCurrentSelectionDisplay();
	CSelectionDisplay * pPrevSelectionDisplay = (CSelectionDisplay *) m_htmlListCtrl->GetItemData( m_htmlListCtrl->GetSelectedItem()-1 );

	//	m_iSerial을 바꾸면 listctrl의 순서가 바뀐다.
	long temp = pCurrentSelectionDisplay->m_iSerial;
	pCurrentSelectionDisplay->m_iSerial = pPrevSelectionDisplay->m_iSerial;
	pPrevSelectionDisplay->m_iSerial = temp;

	OnUpdate();

	SelectListItem(pCurrentSelectionDisplay);
}

void CSelectionListPane::OnCurrentDown()
{
	int indexSelected = m_htmlListCtrl->GetSelectedItem();
	if( indexSelected == NONE_SELECTED || m_htmlListCtrl->GetItemCount() <= 1 || m_htmlListCtrl->GetItemCount()-1 == indexSelected )
		return;

	CSelectionDisplay * pCurrentSelectionDisplay = m_pProteinVistaRenderer->GetCurrentSelectionDisplay();
	CSelectionDisplay * pNextSelectionDisplay = (CSelectionDisplay *) m_htmlListCtrl->GetItemData ( m_htmlListCtrl->GetSelectedItem()+1 );

	long temp = pCurrentSelectionDisplay->m_iSerial;
	pCurrentSelectionDisplay->m_iSerial = pNextSelectionDisplay->m_iSerial;
	pNextSelectionDisplay->m_iSerial = temp;

	OnUpdate();

	SelectListItem(pCurrentSelectionDisplay);
}

 
void CSelectionListPane::OnCenterCurrentVP()
{
	CSelectionDisplay * pCurrentSelectionDisplay = m_pProteinVistaRenderer->GetCurrentSelectionDisplay();

	FLOAT animationTime = 0.0f;
	if ( m_pProteinVistaRenderer->m_pPropertyScene->m_bDoubleClockToCameraAnimation == TRUE )
		animationTime = m_pProteinVistaRenderer->m_pPropertyScene->m_fAnimationTime;
	else
		animationTime = 0.0f;

	m_pProteinVistaRenderer->SetCameraAnimation(pCurrentSelectionDisplay->m_arrayAtomInst, animationTime);
}

 