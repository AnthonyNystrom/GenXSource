#pragma once

// controls operations
LPARAM ListView_GetItemData( HWND hList, int nItem);
HTREEITEM TreeView_EnsureExists( HWND hTree, TCHAR* lpszName, HTREEITEM hRoot = TVI_ROOT, int nImage = -1 );
int ListView_AddCol( HWND hWnd, int nItem, int cx, TCHAR* pszName );
int ListView_AppendText( HWND hList, TCHAR* lpszText );
int ListView_DeleteItemEx( HWND hList, int nItem );
void ListView_SelectItem( HWND hList, int nItem );
void ListView_SetItemImage( HWND hList, int nItem, int nSubItem, int nImage );

// dialogs
void Err(TCHAR* lpszMessage, HWND hWndParent = NULL);
void Info(TCHAR* lpszMessage, HWND hWndParent = NULL);
BOOL Ask(TCHAR* lpszQuestion, HWND hWndParent = NULL);

// file operations
BOOL GetFileData(TCHAR* lpszFilename, LPVOID* lppvData, LPDWORD lpdwSize);
BOOL SetFileData(TCHAR* lpszFileName, LPVOID lpvData, DWORD dwSize, LPVOID lpvPrefix = NULL, DWORD dwPrefixSize = 0);
BOOL GetFileSizeByFileName(TCHAR* lpszFileName, LPDWORD lpdwFileSize);

BOOL SafeCopyFile(TCHAR* lpszSourceFilename, TCHAR* lpszDestFilename, HWND hWndParent = NULL);
BOOL SafeMoveFile(TCHAR* lpszSourceFilename, TCHAR* lpszDestFilename, HWND hWndParent = NULL);
BOOL SafeSetFileData(TCHAR* lpszFileName, LPVOID lpvData, DWORD dwSize, LPVOID lpvPrefix = NULL, DWORD dwPrefixSize = 0, HWND hWndParent = NULL);
BOOL DoEvents( LONG nMSecs );

CString GetPath(HWND hWndParent, CString strPath, CString strTitle);
void BrowseFolder( HWND hWnd, UINT nItemID, TCHAR* lpszPrompt = NULL);
void BrowseFile( HWND hWnd, UINT nItemID, TCHAR* lpszFilters = NULL );

/* macros */

#define PAGE_REQUIRED_FIELD( strValue, nCtrlID, name ) \
	if ( strValue.Trim().IsEmpty() ) { \
	Err(_T("Укажите <" name ">"), m_hWnd);	\
		::SetFocus( ::GetDlgItem( m_hWnd, nCtrlID ) );	\
		return FALSE;	\
	}	\

#define DLG_REQUIRED_FIELD( strValue, nCtrlID, name ) \
	if ( strValue.Trim().IsEmpty() ) { \
	Err(_T("Укажите <" name ">"), m_hWnd);	\
		::SetFocus( ::GetDlgItem( m_hWnd, nCtrlID ) );	\
		return;	\
	}	\



class CEGCtrlFntCracker
{
	HWND m_hWnd;
	UINT m_nCtrlID;
public:
	CEGCtrlFntCracker ( HWND hWnd ) {
		m_hWnd = hWnd;
	}

	LOGFONT lf;
	BOOL CaptureCtrl( UINT nCtrlID ){
		m_nCtrlID = nCtrlID;
		HFONT hFont;
		hFont = (HFONT)::SendDlgItemMessage( m_hWnd, m_nCtrlID, WM_GETFONT, 0, 0);
		if (!hFont)
			hFont = (HFONT)::GetStockObject(SYSTEM_FONT);
		return ::GetObject( hFont, sizeof(LOGFONT), (LPVOID)&lf );
	}

	void ReleaseCtrl( ) {
		HFONT hFont = ::CreateFontIndirect(&lf);
		::SendDlgItemMessage( m_hWnd, m_nCtrlID, WM_SETFONT, (WPARAM)hFont, 0L);
	}

};

class CFileMap
{
	HANDLE m_hFile;
	HANDLE m_hMap;
public:
	CFileMap();
	~CFileMap();

	LPVOID m_pvData;
	DWORD m_dwFileSizeHigh;
	DWORD m_dwFileSizeLow;

	BOOL ReOpen(TCHAR* lpszFilename, TCHAR* lpszName); 
	void Close();

};

BOOL operator==(CFileMap& fm1, CFileMap& fm2 );