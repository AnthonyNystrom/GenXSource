// ParamsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "EGParamsDlg.h"
#include "WinAddon.h"

/*
** CEGParamsPage dialog
*/

CEGParamsPage::CEGParamsPage ( UINT nIDResource, TCHAR* lpszName, TCHAR* lpszKey, CEGParamsDlg* pParamsDlg )
	: CDialog( nIDResource, pParamsDlg )
{
	m_nResourceID = nIDResource;

	m_pszName = m_pszKey = NULL;
	if ( lpszName )
		m_pszName = _tcsdup( lpszName );
	if ( lpszKey )
		m_pszKey = _tcsdup( lpszKey );

	m_pParamsDlg = pParamsDlg;
	pParamsDlg->AddPage( this );
}

CEGParamsPage::~CEGParamsPage (){ 
	if( m_pszName ) free( m_pszName );
	if( m_pszKey ) free( m_pszKey );
}


void CEGParamsPage::OnSetDirty()
{
	SetDirty( TRUE );
}

void CEGParamsPage::SetDirty( BOOL bIsDirty ){
	ASSERT( m_pParamsDlg );
	m_pParamsDlg->SetPageDirty( this, bIsDirty );
}

// CParamsDlg dialog
#define COLOR_MASK	RGB(0x00,0x80,0x80)

IMPLEMENT_DYNAMIC(CEGParamsDlg, CDialog)
CEGParamsDlg::CEGParamsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CEGParamsDlg::IDD, pParent)
{
	m_hCurWnd = NULL;
	m_hTree = NULL;
	m_hCursor = NULL;
}

CEGParamsDlg::~CEGParamsDlg()
{

}

void CEGParamsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TREE, m_tree);
}


BEGIN_MESSAGE_MAP(CEGParamsDlg, CDialog)
	ON_NOTIFY(TVN_SELCHANGED, IDC_TREE, OnSelChanged)
	ON_NOTIFY(TVN_ITEMEXPANDED, IDC_TREE, OnItemExpanded)
	ON_COMMAND( IDC_APPLY, OnApply )
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()


// CParamsDlg message handlers
BOOL CEGParamsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	CEGCtrlFntCracker FntCracker( m_hWnd );
	if ( FntCracker.CaptureCtrl( IDC_PARAMS_TITLE ) ) {
		FntCracker.lf.lfWeight = FW_BOLD;
		FntCracker.ReleaseCtrl();
	}

	m_hTree = ::GetDlgItem( m_hWnd, IDC_TREE );
	HIMAGELIST himl = ImageList_Create( 16, 16, ILC_COLORDDB | ILC_MASK, 3, 1);
	ImageList_AddMasked( himl, LoadBitmap( AfxGetResourceHandle() , MAKEINTRESOURCE( IDB_PARAMS ) ), COLOR_MASK );
	TreeView_SetImageList( m_hTree, himl, TVSIL_NORMAL );


	CRect rcHeader, rcTree;
	GetDlgItem(IDC_HEADER)->GetWindowRect(&rcHeader);
	ScreenToClient(&rcHeader);

	::GetWindowRect( m_hTree, &rcTree );
	ScreenToClient(&rcTree);

	m_rcParam.top = rcHeader.bottom+5;
	m_rcParam.bottom = rcTree.bottom-5;
	m_rcParam.left = rcHeader.left;
	m_rcParam.right = rcHeader.right;

	// Создаем ключи 
	HTREEITEM hItem;
	TCHAR szKey[1025]=_T("");
	
	vector<CEGParamsPage*>::iterator itPage;
	for( itPage = m_pages.begin(); itPage != m_pages.end(); ++itPage ){
		_tcsncpy(szKey, (*itPage)->GetKey(), 1024 );
		TCHAR* pszSubKey = _tcstok( szKey, _T("\\") );
		hItem = TVI_ROOT;
		while( pszSubKey != NULL ){
			hItem = TreeView_EnsureExists( m_hTree, pszSubKey, hItem, 0 );
			if ( !hItem ) 
				return FALSE;
			pszSubKey = _tcstok( NULL, _T("\\") );
			if ( !pszSubKey ) {
				TV_ITEM tvi;
				tvi.hItem = hItem;
				tvi.mask = TVIF_PARAM | TVIF_IMAGE;
				tvi.iImage = -1;
				tvi.lParam = (LPARAM) (*itPage);
				TreeView_SetItem( m_hTree, &tvi );
				// Создаем страницу свойств
				(*itPage)->Create( (*itPage)->GetResourceID(), this );
				// Ресайзим
				(*itPage)->MoveWindow( &m_rcParam );
			}
		}
	}

	::SetFocus( m_hTree );

	return FALSE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CEGParamsDlg::OnSelChanged(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMTREEVIEW pNMTreeView = reinterpret_cast<LPNMTREEVIEW>(pNMHDR);

	if (m_tree.ItemHasChildren(pNMTreeView->itemNew.hItem)){
		// активируется родительский узел
		
		// если фокус был не у одного из дочерних элементов, то активируем первый из них
		if (m_tree.GetParentItem(pNMTreeView->itemOld.hItem) != pNMTreeView->itemNew.hItem){
			HTREEITEM hItem = m_tree.GetNextItem(pNMTreeView->itemNew.hItem,TVGN_CHILD);
			m_tree.EnsureVisible(hItem);
			m_tree.SelectItem(hItem);
		}
		// ну и надо закрыть всяких там открытых соседей
		// 1. Снизу
		HTREEITEM hItem = m_tree.GetNextItem(pNMTreeView->itemNew.hItem,TVGN_NEXT);
		while(hItem){
			if(m_tree.Expand(hItem,TVE_COLLAPSE))
				m_tree.SetItemImage(hItem,0,0);
			hItem = m_tree.GetNextItem(hItem,TVGN_NEXT);
		}
		// 2. Сверху
		hItem = m_tree.GetNextItem(pNMTreeView->itemNew.hItem,TVGN_PREVIOUS);
		while(hItem){
			if(m_tree.Expand(hItem,TVE_COLLAPSE))
				m_tree.SetItemImage(hItem,0,0);
			hItem = m_tree.GetNextItem(hItem,TVGN_PREVIOUS);
		}
		
	}else{
		// активируется дочерний узел

		// убираем стрелочку с предыдущего дочернего узла
		m_tree.SetItemImage(m_hCursor,-1,-1);
		// и ставим стрелочку на этот
		m_tree.SetItemImage(pNMTreeView->itemNew.hItem,2,2);
		// Запоминаем позицию стрелки
		m_hCursor = pNMTreeView->itemNew.hItem;
		
		// показываем соответствующую страницу настройки
		CEGParamsPage* pPage = (CEGParamsPage*)pNMTreeView->itemNew.lParam;
		if ( pPage->m_hWnd != m_hCurWnd){
			// прячем (если было) предыдущее окно настройки
			if ( m_hCurWnd ) 
				::ShowWindow( m_hCurWnd, SW_HIDE );
			// показываем окно настройки для текущего элемента
			m_hCurWnd = pPage->m_hWnd;
			::ShowWindow( m_hCurWnd, SW_SHOW );
			// Устанавливаем заголовок раздела
			::SetDlgItemText( m_hWnd, IDC_PARAMS_TITLE, pPage->GetName() );
		}

		// ну и надо закрыть всяких там открытых соседей
		// 1. Снизу
		HTREEITEM hItem = m_tree.GetNextItem(pNMTreeView->itemNew.hItem,TVGN_NEXT);
		while(hItem){
			if(m_tree.Expand(hItem,TVE_COLLAPSE))
				m_tree.SetItemImage(hItem,0,0);
			hItem = m_tree.GetNextItem(hItem,TVGN_NEXT);
		}
		// 2. Сверху
		hItem = m_tree.GetNextItem(pNMTreeView->itemNew.hItem,TVGN_PREVIOUS);
		while(hItem){
			if(m_tree.Expand(hItem,TVE_COLLAPSE))
				m_tree.SetItemImage(hItem,0,0);
			hItem = m_tree.GetNextItem(hItem,TVGN_PREVIOUS);
		}
	}
	*pResult = 0;
}

void CEGParamsDlg::OnItemExpanded(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMTREEVIEW pNMTreeView = reinterpret_cast<LPNMTREEVIEW>(pNMHDR);
	
	if (pNMTreeView->action == 2)
		m_tree.SetItemImage(pNMTreeView->itemNew.hItem,1,1);
	else
		m_tree.SetItemImage(pNMTreeView->itemNew.hItem,0,0);
	
	*pResult = 0;
}

void CEGParamsDlg::OnApply()
{
	BOOL bFailed = FALSE;
	set<CEGParamsPage*>::iterator itPage, itPageLast = m_dirty_pages.end();
	for ( itPage = m_dirty_pages.begin(); itPage != itPageLast; ++itPage ) {
		if ( !(*itPage)->Save() ) {
			TCHAR szQuestion[201]=_T("");
			_sntprintf( szQuestion, 200, _T("Не удалось сохранить изменения для \"%s\"!\r\n\r\nПродолжить с остальными? "), (*itPage)->GetName() );
			if ( !Ask( szQuestion ) ) 
				return;	
			bFailed = TRUE;
		}
	}
	if ( !bFailed ) {
		// giving them a chance to apply changes
		for ( itPage = m_dirty_pages.begin(); itPage != itPageLast; ++itPage )
			(*itPage)->OnBeforeClose();

		CDialog::OnOK();
	}
}

void CEGParamsDlg::OnOK()
{
	// Избегаем закрытия по ENTER
}

void CEGParamsDlg::AddPage( CEGParamsPage* pPage ) {
	// Добавляем в список
	m_pages.push_back( pPage );
}

void CEGParamsDlg::SetPageDirty( CEGParamsPage* pPage, BOOL bDirty ){
	set<CEGParamsPage*>::iterator itPage = m_dirty_pages.find( pPage );
	if ( itPage != m_dirty_pages.end() )
		m_dirty_pages.erase( itPage );
	if ( bDirty )
		m_dirty_pages.insert( pPage );
	::EnableWindow( ::GetDlgItem( m_hWnd, IDC_APPLY ), !m_dirty_pages.empty() );
}


HBRUSH CEGParamsDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	if ( CTLCOLOR_STATIC == nCtlColor && IDC_PARAMS_TITLE == pWnd->GetDlgCtrlID() ) {
		pDC->SetTextColor( 0xFFFFFF );
		pDC->SetBkMode( TRANSPARENT );
		hbr = (HBRUSH)GetStockObject( GRAY_BRUSH );
	}

	return hbr;
}
