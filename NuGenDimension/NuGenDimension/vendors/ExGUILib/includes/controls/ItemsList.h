#pragma once

#include  "StdAfx.h"
#define CItemsListTmpl CItemsList< classDialog, classItem, classItemList > 

template < class classDialog, class classItem, class classItemList >
class CItemsList: 
	public CEGListCtrl
{
protected:
	classItemList m_lstItems;
	classItem * m_pItem;
public:
	CItemsList();
	~CItemsList();
	
	BOOL InitList( TCHAR* pszFilename, BSTR bstrRoot, CWnd* pWndTask, UINT nImageList =0 , CEGMenu * pMenu = NULL, CWnd * pWndMenuHandler = NULL );
	classItemList * GetData() { return &m_lstItems; }

// overrides
protected:
	virtual void InitCols();
	virtual void InitItem( classItem * pItem, int nItem );
	virtual BOOL IsItemVisible( classItem * pItem );

// operations
public:
	void Refresh();
	void DeleteItem( classItem * pItem );
	int AppendItem( classItem * pItem, int nItem = -1 );
	void RefreshItem( classItem * pItem, int nItem = -1 );
protected:
	void ReloadList();

// Generated message map functions
// #define DECLARE_MESSAGE_MAP()
#ifdef _AFXDLL
private: 
	static const AFX_MSGMAP_ENTRY _messageEntries[];
protected: 
	static const AFX_MSGMAP messageMap;
	static const AFX_MSGMAP* PASCAL GetThisMessageMap();
	virtual const AFX_MSGMAP* GetMessageMap() const;
#else
private:
	static const AFX_MSGMAP_ENTRY _messageEntries[]; 
protected: 
	static const AFX_MSGMAP messageMap; 
	virtual const AFX_MSGMAP* GetMessageMap() const; 
#endif

public:
	afx_msg int OnCreate( LPCREATESTRUCT lpCreateStruct );
	afx_msg void OnItemChanged( NMHDR * pNotifyStruct, LRESULT* pResult );
	afx_msg void OnDblClick( NMHDR * pNotifyStruct, LRESULT* pResult );
	afx_msg void OnKeyDown( UINT nChar, UINT nRepCnt, UINT nFlags );
	afx_msg UINT OnGetDlgCode( );

	// for implicit calls only
	void OnItemNew();
	void OnItemEdit();
	void OnItemDelete();
	void OnUpdateItemNew(CCmdUI *pCmdUI);
	void OnUpdateItemEdit(CCmdUI *pCmdUI);
	void OnUpdateItemDelete(CCmdUI *pCmdUI);
};

template < class classDialog, class classItem, class classItemList >
CItemsListTmpl::CItemsList() {
	m_pItem = NULL;	
}


template < class classDialog, class classItem, class classItemList >
CItemsListTmpl::~CItemsList() {

}

// BEGIN_MESSAGE_MAP(CMainFrame, CNewFrameWnd)
#ifdef _AFXDLL
	template < class classDialog, class classItem, class classItemList >
	const AFX_MSGMAP* PASCAL CItemsListTmpl::GetThisMessageMap() 
	{ return &CItemsListTmpl::messageMap; } 

	template < class classDialog, class classItem, class classItemList >
	const AFX_MSGMAP* CItemsListTmpl::GetMessageMap() const 
	{ return &CItemsListTmpl::messageMap; } 

	template < class classDialog, class classItem, class classItemList >
	AFX_COMDAT const AFX_MSGMAP CItemsListTmpl::messageMap = 
	{ &CEGListCtrl::GetThisMessageMap, &CItemsListTmpl::_messageEntries[0] }; 

	template < class classDialog, class classItem, class classItemList >
	AFX_COMDAT const AFX_MSGMAP_ENTRY CItemsListTmpl::_messageEntries[] = 
	{
#else
	template < class classDialog, class classItem, class classItemList >
	const AFX_MSGMAP* CItemsListTmpl::GetMessageMap() const 
		{ return &CItemsListTmpl::messageMap; } 

	template < class classDialog, class classItem, class classItemList >
	AFX_COMDAT const AFX_MSGMAP CItemsListTmpl::messageMap = 
	{ &CEGListCtrl::messageMap, &CItemsListTmpl::_messageEntries[0] }; 

	template < class classDialog, class classItem, class classItemList >
	AFX_COMDAT const AFX_MSGMAP_ENTRY CItemsListTmpl::_messageEntries[] = 
	{ 
#endif

	ON_WM_CREATE()
	ON_NOTIFY_REFLECT( LVN_ITEMCHANGED, OnItemChanged )
	ON_NOTIFY_REFLECT( NM_DBLCLK, OnDblClick )
	ON_WM_KEYDOWN( )
	ON_WM_GETDLGCODE( )
// END_MESSAGE_MAP()
	{0, 0, 0, 0, AfxSig_end, (AFX_PMSG) 0 }
};

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::InitCols() {

}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::InitItem( classItem * pItem, int nItem  ) {

}

template < class classDialog, class classItem, class classItemList >
BOOL CItemsListTmpl::IsItemVisible( classItem * pItem ) {
	return TRUE;
}

template < class classDialog, class classItem, class classItemList >
int  CItemsListTmpl::OnCreate( LPCREATESTRUCT lpCreateStruct ) {
	
	if (CEGListCtrl::OnCreate(lpCreateStruct) == -1)
		return -1;

	InitCols();

	return 0L;
}

template < class classDialog, class classItem, class classItemList >
UINT CItemsListTmpl::OnGetDlgCode( ) {

	UINT nResult = CEGListCtrl::OnGetDlgCode( );

	nResult |= DLGC_WANTALLKEYS;

	return nResult;
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnKeyDown( UINT nChar, UINT nRepCnt, UINT nFlags ) {

	CEGListCtrl::OnKeyDown( nChar, nRepCnt, nFlags );

	switch( nChar ) {
		case VK_INSERT:
			OnItemNew();
			break;
		case VK_RETURN:
			OnItemEdit();
			break;
		case VK_DELETE:
			OnItemDelete();
			break;
	}
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnDblClick( NMHDR * pNotifyStruct, LRESULT* pResult ) {

	OnItemEdit();

}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnItemChanged( NMHDR * pNotifyStruct, LRESULT* pResult ) {
	
	LPNMLISTVIEW lpnmhdr = (LPNMLISTVIEW)pNotifyStruct;

	if ( 0 == lpnmhdr->uNewState ) {
		m_pItem = NULL;
	} else if ( (lpnmhdr->uNewState & LVIS_SELECTED) == LVIS_SELECTED) {
		m_pItem = reinterpret_cast< classItem * >( lpnmhdr->lParam );
	}

	*pResult = 1;
}

template < class classDialog, class classItem, class classItemList >
BOOL CItemsListTmpl::InitList( TCHAR* pszFilename, BSTR bstrRoot, CWnd* pWndTask, UINT nImageList, CEGMenu * pMenu, CWnd * pWndMenuHandler ) {
	
	// creation
	if( !CEGListCtrl::Create( WS_CHILD | LVS_SHOWSELALWAYS | LVS_REPORT | LVS_SINGLESEL, CRect(0,0,0,0), pWndTask, 1 ) )
		return FALSE;

	ListView_SetExtendedListViewStyle( m_hWnd, LVS_EX_HEADERDRAGDROP | LVS_EX_FULLROWSELECT );

	// assign image list
	if ( nImageList > 0 ) {
		CImageList iml;
		iml.Create(16, 16, ILC_COLOR24 | ILC_MASK, 0, 4 );
		CBitmap bmp;
		bmp.LoadBitmap( nImageList );
		iml.Add(&bmp, RGB(255,0,255) );
		ListView_SetImageList( m_hWnd, iml.Detach(), LVSIL_SMALL );
		ListView_DeleteAllItems( m_hWnd );
	}

	// assign menu	
	SetMenu( pMenu, pWndMenuHandler ? pWndMenuHandler : this );
	
	// initialization
	m_lstItems.LoadFromFile( pszFilename, bstrRoot);

	// InitItems
	ReloadList();

	return TRUE;
}


template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::ReloadList()
{
	ListView_DeleteAllItems( m_hWnd );

	classItemList::iterator itItem = m_lstItems.begin(),  itItemLast = m_lstItems.end();

	int nItem = 0;
	for ( ; itItem != itItemLast; ++itItem ) 
		if ( IsItemVisible( (*itItem) ) )
			AppendItem( (*itItem), nItem++ );

	if ( ListView_GetItemCount( m_hWnd ) > 0 ) {
		ListView_SelectItem( m_hWnd, 0 );
		m_pItem = reinterpret_cast< classItem* >( ListView_GetItemData( m_hWnd, 0 ) );
	}
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::DeleteItem( classItem * pItem ) {

	int nCount = ListView_GetItemCount( m_hWnd );
	for ( int nItem = 0; nItem < nCount; ++nItem ) {
		if ( pItem == reinterpret_cast< classItem * >( ListView_GetItemData( m_hWnd, nItem ) ) ) {
			ListView_DeleteItem( m_hWnd, nItem);
			break;
		}
	}	
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::Refresh()
{
	classItem * savedItem = m_pItem;
	
	// Refresh info
	LV_ITEM lvi;
	lvi.iSubItem = 0;
	int nCount = ListView_GetItemCount( m_hWnd );
	for (lvi.iItem = 0; lvi.iItem < nCount; ++lvi.iItem ) {
		lvi.mask = LVIF_PARAM; 
		if ( ListView_GetItem( m_hWnd, &lvi ) )
			RefreshItem( reinterpret_cast< classItem* >( lvi.lParam ), lvi.iItem );
	}

	m_pItem = savedItem;
}

template < class classDialog, class classItem, class classItemList >
int CItemsListTmpl::AppendItem( classItem * pItem, int nItem ) {

	// Добавление указанного объекта
	if( -1 == nItem ) 
		nItem = ListView_GetItemCount( m_hWnd );

	LV_ITEM lvi;
	lvi.iItem = nItem;
	lvi.iSubItem = 0;
	lvi.mask = LVIF_TEXT | LVIF_PARAM;
	lvi.iImage = 0;
	lvi.pszText = _T("");
	lvi.lParam = reinterpret_cast< LPARAM > ( pItem );
	int nRes = ListView_InsertItem( m_hWnd, &lvi);
	
	RefreshItem( pItem, lvi.iItem );

	return nRes;
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::RefreshItem( classItem * pItem, int nItem ) {

	if ( -1 != nItem ) {
		InitItem( pItem, nItem );
	} else {
		// Обновление реквизитов указанного объекта
		int nCount = ListView_GetItemCount( m_hWnd );
		for ( int nItem = 0; nItem < nCount; ++nItem ) {
			if( pItem == reinterpret_cast< classItem* >( ListView_GetItemData( m_hWnd, nItem ) ) ) {
				classItem* pSavedActiveItem = m_pItem;
				InitItem( pItem, nItem );
				m_pItem = pSavedActiveItem;
				break;
			}
		}
	}
}

// handlers
template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnItemNew(){
	classDialog dlg;
	classItem * pItem = new classItem;
	pItem->InitNew();

	if ( !dlg.Edit( pItem ) ) {
		delete pItem;
	} else {
		m_lstItems.push_back( pItem );
		m_lstItems.SaveChanges( );
		int nItem = AppendItem( pItem );
		if ( -1 != nItem )
			ListView_SelectItem( m_hWnd, nItem );
	}
	::SetFocus( m_hWnd );
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnItemEdit(){
	if ( m_pItem ) {
		classDialog dlg;
		if ( dlg.Edit( m_pItem ) ) {
			m_lstItems.SaveChanges( );
			RefreshItem( m_pItem );
		}
	}
	::SetFocus( m_hWnd );
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnItemDelete(){
	if ( m_pItem ) {
		int nItem = ListView_GetNextItem( m_hWnd, -1, LVIS_SELECTED );
		if ( -1 != nItem ) {
			m_lstItems.RemoveItem( m_pItem );
			m_lstItems.SaveChanges( );
			nItem = ListView_DeleteItemEx( m_hWnd, nItem );
			m_pItem = reinterpret_cast< classItem* > ( ListView_GetItemData( m_hWnd, nItem ) );
		}
	}	
	::SetFocus( m_hWnd );
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnUpdateItemNew(CCmdUI *pCmdUI){

}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnUpdateItemEdit(CCmdUI *pCmdUI){
	pCmdUI->Enable( NULL != m_pItem );
}

template < class classDialog, class classItem, class classItemList >
void CItemsListTmpl::OnUpdateItemDelete(CCmdUI *pCmdUI){
	pCmdUI->Enable( NULL != m_pItem );
}
