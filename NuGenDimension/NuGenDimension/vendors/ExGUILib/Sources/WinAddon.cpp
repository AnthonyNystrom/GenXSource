#include "StdAfx.h"
#include "WinAddon.h"

LPARAM ListView_GetItemData( HWND hList, int nItem){
	LV_ITEM lvi;
	lvi.iItem = nItem;
	lvi.iSubItem = 0;
	lvi.mask = LVIF_PARAM;
	return ListView_GetItem( hList, &lvi ) ?  lvi.lParam : 0 ;
}

void ListView_SetItemImage( HWND hList, int nItem, int nSubItem, int nImage ) {
		LV_ITEM lvi;
		lvi.iItem = nItem;
		lvi.iSubItem = nSubItem;
		lvi.mask = LVIF_IMAGE; 
		lvi.iImage = nImage;
		ListView_SetItem( hList, &lvi );	
}

HTREEITEM TreeView_EnsureExists( HWND hTree, TCHAR* lpszName, HTREEITEM hRoot, int nImage ){
	// Пытаемся отыскать
	HTREEITEM hItem = TreeView_GetNextItem( hTree, hRoot, TVGN_CHILD );
	if ( hItem ) {
		TCHAR szName[101] = _T("");
		TV_ITEM tvi;
		tvi.mask = TVIF_TEXT;
		tvi.cchTextMax = 100;
		tvi.pszText = szName;
		do{
			tvi.hItem = hItem;			
			if ( TreeView_GetItem( hTree, &tvi ) && 0 == _tcsicmp( szName, lpszName ) )
				return hItem;
		}while( NULL != ( hItem = TreeView_GetNextItem( hTree, hItem, TVGN_NEXT ) ) );
	}

	// Создаем, коль уж не нашли 
	TVINSERTSTRUCT tvis;
	tvis.hInsertAfter = TVI_LAST;
	tvis.hParent = hRoot;
	tvis.item.mask = TVIF_TEXT;
	tvis.item.pszText = lpszName;
	if( -1 != nImage ){
		tvis.item.mask |= TVIF_TEXT;
		tvis.item.iImage = nImage;
	}
	hItem = TreeView_InsertItem( hTree, &tvis );

	// Сортируем по имения
	TreeView_SortChildren( hTree, hRoot, TRUE );

	return hItem;
}

int ListView_AddCol( HWND hWnd, int nItem, int cx, TCHAR* pszName ) {

	LV_COLUMN lvc;
	lvc.iSubItem = nItem;
	lvc.mask = LVCF_FMT | LVCF_WIDTH | LVCF_TEXT | LVCF_SUBITEM;
	lvc.fmt = LVCFMT_LEFT;
	lvc.cx = cx; 
	lvc.pszText = pszName;

	return ListView_InsertColumn( hWnd, lvc.iSubItem, &lvc );
}

int ListView_AppendText( HWND hList, TCHAR* lpszText ) {
	LV_ITEM lvi;
	lvi.iItem = ListView_GetItemCount( hList );
	lvi.iSubItem = 0;
	lvi.mask = LVIF_TEXT ;
	lvi.iImage = 0;
	lvi.pszText = lpszText;
	return ListView_InsertItem( hList, &lvi);
}

void ListView_SelectItem( HWND hList, int nItem ) {
	ListView_SetItemState( hList, nItem, LVIS_SELECTED | LVIS_FOCUSED, LVIS_SELECTED | LVIS_FOCUSED );
}

int ListView_DeleteItemEx( HWND hList, int nItem ) {
	ListView_DeleteItem( hList, nItem );
	int nCount = ListView_GetItemCount( hList );
	ListView_SelectItem( hList, nCount > nItem ? nItem : nItem - 1 );
	return nCount > nItem ? nItem : nItem - 1;
}

HWND EnsureParent( HWND hWndParent ) {
	if ( hWndParent ) {

		return hWndParent;
	
	}else {

		CWnd* pMainWnd = AfxGetMainWnd();
		if ( !pMainWnd || !IsWindowVisible( pMainWnd->m_hWnd ) )
			return NULL;

		hWndParent = AfxGetMainWnd()->m_hWnd;
		::SetActiveWindow( hWndParent );
		::SetForegroundWindow( hWndParent );
		return hWndParent;

	}
}


void Err(TCHAR* lpszMessage, HWND hWndParent){
	::MessageBox( EnsureParent( hWndParent ), lpszMessage, _T("Ошибка"), MB_ICONERROR | MB_OK | MB_SETFOREGROUND | MB_TOPMOST);
}

void Info(TCHAR* lpszMessage, HWND hWndParent){
	::MessageBox( EnsureParent( hWndParent ), lpszMessage, _T("Информация"), MB_ICONINFORMATION | MB_OK | MB_SETFOREGROUND | MB_TOPMOST);
}

BOOL Ask(TCHAR* lpszQuestion, HWND hWndParent){
	return ( IDYES == ::MessageBox( EnsureParent( hWndParent ), lpszQuestion, _T("Вопрос"), MB_ICONQUESTION | MB_YESNO | MB_SETFOREGROUND | MB_TOPMOST) );
}


BOOL GetFileData(TCHAR* lpszFilename, LPVOID* lppvData, LPDWORD lpdwSize){

	CFileMap fm;
	if ( !fm.ReOpen( lpszFilename,  _T("File1") ) )
		return FALSE;
	
	*lpdwSize = fm.m_dwFileSizeLow;
	if ( NULL != lppvData) { // not a test
		*lppvData = malloc(*lpdwSize);
		memcpy( *lppvData, fm.m_pvData, *lpdwSize );
	}

	return TRUE;
}

BOOL GetFileSizeByFileName(TCHAR* lpszFileName, LPDWORD lpdwFileSize){
	
	*lpdwFileSize = 0;
	
	HANDLE hFile = CreateFile( lpszFileName, FILE_READ_DATA, 0, NULL, OPEN_EXISTING, 0, NULL );
	if (INVALID_HANDLE_VALUE == hFile)
		return FALSE;

	DWORD dwSizeHigh;
	*lpdwFileSize = GetFileSize(hFile, &dwSizeHigh);
	
	CloseHandle( hFile );

	return TRUE;
}


BOOL SetFileData(TCHAR* lpszFileName, LPVOID lpvData, DWORD dwSize, LPVOID lpvPrefix, DWORD dwPrefixSize){

	HANDLE hFile = CreateFile( lpszFileName, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_ALWAYS , 0, NULL );
	if (INVALID_HANDLE_VALUE == hFile)
		return FALSE;

	BOOL bResult = TRUE;
	DWORD dwWritten;
	
	if (dwPrefixSize > 0)
		bResult = ( WriteFile( hFile, lpvPrefix, dwPrefixSize, &dwWritten, NULL) &&  dwPrefixSize == dwWritten );
	bResult = bResult && ( WriteFile( hFile, lpvData, dwSize, &dwWritten, NULL) &&  dwSize == dwWritten );
	SetEndOfFile( hFile ); 
	CloseHandle( hFile );

	return bResult;
}

BOOL SafeCopyFile(TCHAR* lpszSourceFilename, TCHAR* lpszDestFilename, HWND hWndParent){
	LPVOID lpMsgBuf;
	TCHAR szQuestion [ 1024 ];
	while( !CopyFile( lpszSourceFilename, lpszDestFilename, FALSE ) ) {
		DWORD dwError = GetLastError();
		if( !FormatMessage(  FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
			NULL, dwError, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR) &lpMsgBuf, 0, NULL ) ) {
			_stprintf( szQuestion, _T("при копировании файла '%s' в '%s' произошла ошибка:\r\nКод ошибки - %d\r\n\r\nПовторить попытку?"), 
				lpszSourceFilename, lpszDestFilename, dwError );
		}else{
			_stprintf( szQuestion, _T("при копировании файла '%s' в '%s' произошла ошибка:\r\n%s\r\n\r\nПовторить попытку?"),
				lpszSourceFilename, lpszDestFilename, lpMsgBuf );
			LocalFree( lpMsgBuf );
		}
		if( !Ask( szQuestion, hWndParent ) )
			return FALSE;
	}
	return TRUE;
}

BOOL SafeMoveFile(TCHAR* lpszSourceFilename, TCHAR* lpszDestFilename, HWND hWndParent){
	LPVOID lpMsgBuf;
	TCHAR szQuestion [ 1024 ];
	while( !MoveFileEx( lpszSourceFilename, lpszDestFilename, MOVEFILE_REPLACE_EXISTING | MOVEFILE_WRITE_THROUGH ) ) {
		DWORD dwError = GetLastError();
		if( !FormatMessage(  FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
			NULL, dwError, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR) &lpMsgBuf, 0, NULL ) ) {
			_stprintf( szQuestion, _T("при перемещении файла '%s' в '%s' произошла ошибка:\r\nКод ошибки - %d\r\n\r\nПовторить попытку?"), 
				lpszSourceFilename, lpszDestFilename, dwError );
		}else{
			_stprintf( szQuestion, _T("при перемещении файла '%s' в '%s' произошла ошибка:\r\n%s\r\n\r\nПовторить попытку?"),
				lpszSourceFilename, lpszDestFilename, lpMsgBuf );
			LocalFree( lpMsgBuf );
		}
		if( !Ask( szQuestion, hWndParent ) )
			return FALSE;
	}
	return TRUE;
}

BOOL SafeSetFileData(TCHAR* lpszFileName, LPVOID lpvData, DWORD dwSize, LPVOID lpvPrefix, DWORD dwPrefixSize, HWND hWndParent){
	LPVOID lpMsgBuf;
	TCHAR szQuestion [ 1024 ];
	while( !SetFileData( lpszFileName, lpvData, dwSize, lpvPrefix, dwPrefixSize ) ) {
		DWORD dwError = GetLastError();
		if( !FormatMessage(  FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
			NULL, dwError, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR) &lpMsgBuf, 0, NULL ) ) {
			_stprintf( szQuestion, _T("при перезаписи файла '%s' произошла ошибка:\r\nКод ошибки - %d\r\n\r\nПовторить попытку?"), 
				lpszFileName, dwError );
		}else{
			_stprintf( szQuestion, _T("при перезаписи файла '%s' произошла ошибка:\r\n%s\r\n\r\nПовторить попытку?"),
				lpszFileName, lpMsgBuf );
			LocalFree( lpMsgBuf );
		}
		if( !Ask( szQuestion, hWndParent ) )
			return FALSE;
	}
	return TRUE;
}

BOOL DoEvents( LONG nMSecs )
{
	COleDateTime dtBegin = COleDateTime::GetCurrentTime();
	COleDateTimeSpan sp;

	do {
/*    static MSG msg;
		if ( ::PeekMessage( &msg, NULL, 0, 0, PM_NOREMOVE ) ) {
      if ( !AfxGetApp() ->PumpMessage() )
      {
          ::PostQuitMessage( 0 );
					return FALSE;
			}
		}
		*/
			MSG msg;
			while( PeekMessage( &msg, NULL, NULL, NULL, PM_REMOVE ) ) {
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}
		sp = COleDateTime::GetCurrentTime() - dtBegin;
	} while( LONG( (double)sp * 24 * 60 * 60 * 1000 ) < nMSecs );
	return TRUE;
}


void BrowseFolder( HWND hWnd, UINT nItemID, TCHAR* lpszPrompt ) {
	HWND hWndItem = ::GetDlgItem( hWnd, nItemID );

	CString strVal;
	int nLen = ::GetWindowTextLength( hWndItem );
	if ( nLen > 0 ) {
		::GetWindowText( hWndItem, strVal.GetBuffer( nLen + 1), nLen+1 );
		strVal.ReleaseBuffer();
	}

	if ( lpszPrompt ) 
		strVal = GetPath( hWnd, strVal, lpszPrompt );
	else
		strVal = GetPath( hWnd, strVal, _T("Укажите папку") );
	if( !strVal.IsEmpty() ) 
		::SetWindowText( hWndItem, (LPCTSTR)strVal );
}

void BrowseFile( HWND hWnd, UINT nItemID, TCHAR* lpszFilters  ) {
	HWND hWndItem = ::GetDlgItem( hWnd, nItemID );

	CString strVal;
	int nLen = ::GetWindowTextLength( hWndItem );
	if ( nLen > 0 ) {
		::GetWindowText( hWndItem, strVal.GetBuffer( nLen + 1), nLen+1 );
		strVal.ReleaseBuffer();
	}

   CFileDialog fileDlg ( TRUE, NULL, strVal, 0, lpszFilters );
   if( IDOK == fileDlg.DoModal () )
		::SetWindowText( hWndItem, (LPCTSTR)fileDlg.GetPathName() );
}

int CALLBACK lpBrowseCallbackProc( HWND hwnd, UINT uMsg, LPARAM /* lParam */, LPARAM lpData ) {
	if (uMsg == BFFM_INITIALIZED )
			SendMessage(hwnd, BFFM_SETSELECTION, TRUE, (LPARAM) lpData);
  return 0;
}

CString GetPath(HWND hWndParent, CString strPath, CString strTitle)
{
	char szDisplayName[MAX_PATH];
	char szRes[MAX_PATH];
	CString strRes;
	BROWSEINFO  bri;		
	
	bri.hwndOwner = hWndParent;
	bri.pidlRoot  = NULL;
	bri.pszDisplayName = szDisplayName;
	bri.lpszTitle = (LPCTSTR)strTitle;
	bri.ulFlags   = 0;
	bri.lpfn      = lpBrowseCallbackProc;
	bri.lParam    = (LPARAM)(LPCTSTR)strPath;
	bri.iImage    = 0;

	LPITEMIDLIST  lpidl = SHBrowseForFolder(&bri);
	if (lpidl==NULL) // cancel button was pressed
		return "";
	
	// get path
	BOOL bOK = SHGetPathFromIDList(lpidl, szRes);
	LPMALLOC      ppMalloc;
	if (SHGetMalloc(&ppMalloc) ) 
		ppMalloc->Free(lpidl);
	if (!bOK)
		return "";
	return szRes;
}

CFileMap::CFileMap(){
	m_pvData = NULL;
	m_hMap = NULL;
	m_hFile = NULL;
	m_dwFileSizeLow = 0;
	m_dwFileSizeHigh = 0;
}

CFileMap::~CFileMap(){
	Close();
}

void CFileMap::Close(){
	if(	NULL != m_pvData )
		UnmapViewOfFile( m_pvData );
	m_pvData = NULL;

	if ( NULL != m_hMap )
		CloseHandle( m_hMap );
	m_hMap = NULL;

	if ( NULL != m_hFile )
		CloseHandle( m_hFile );
	m_hFile = NULL;

	m_dwFileSizeLow = 0;
	m_dwFileSizeHigh = 0;
}

BOOL CFileMap::ReOpen(TCHAR* lpszFilename, TCHAR* lpszName){
	
	Close();

	m_hFile = CreateFile( lpszFilename, FILE_READ_DATA, 0, NULL, OPEN_EXISTING, 0, NULL );
	if (INVALID_HANDLE_VALUE == m_hFile)
		return FALSE;

	m_dwFileSizeLow = GetFileSize(m_hFile, &m_dwFileSizeHigh);

	m_hMap = CreateFileMapping(m_hFile, NULL, PAGE_READONLY, 0, 0, lpszName );
	if (NULL == m_hMap){
		Close( );
		return FALSE;
	}

	m_pvData = MapViewOfFile( m_hMap, FILE_MAP_READ, 0, 0, 0 );
	if (NULL == m_pvData){
		Close( );
		return FALSE;
	}

	return TRUE;
}

BOOL operator==(CFileMap& fm1, CFileMap& fm2 ){
	return  (fm1.m_dwFileSizeHigh == fm2.m_dwFileSizeHigh ) &&
			( fm1.m_dwFileSizeLow == fm2.m_dwFileSizeLow ) &&
			(	memcmp(fm1.m_pvData, fm2.m_pvData, fm1.m_dwFileSizeLow ) == 0 );
}
