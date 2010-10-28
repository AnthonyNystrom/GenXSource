// ParamsGUIPage.cpp : implementation file
//

#include "stdafx.h"
#include "EGAppParamsPage.h"
#include "PaintManager.h"
#include <shlwapi.h>
#include <windowsx.h>
#include "NewTheme.h"

// CEGAppParamsPage dialog

IMPLEMENT_DYNAMIC(CEGAppParamsPage, CDialog)
CEGAppParamsPage::CEGAppParamsPage( CEGParamsDlg* pParamsDlg, CEGMenu* pDefaultNewMenu, TCHAR* lpszKey )
	: CEGParamsPage( CEGAppParamsPage::IDD, _T("Стили оформления интерфейса"), lpszKey, pParamsDlg )
{
	m_pDefaultNewMenu = pDefaultNewMenu; 
}

CEGAppParamsPage::~CEGAppParamsPage()
{
}

void CEGAppParamsPage::DoDataExchange(CDataExchange* pDX)
{
	CEGParamsPage::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CEGAppParamsPage, CEGParamsPage)
	ON_BN_CLICKED( IDC_GUI_CLASSIC, OnSelectTheme )
	ON_BN_CLICKED( IDC_GUI_OFFICEXP, OnSelectTheme )
	ON_BN_CLICKED( IDC_GUI_OFFICE2003, OnSelectTheme )
	ON_BN_CLICKED( IDC_GUI_ICY, OnSelectTheme )
	ON_BN_CLICKED( IDC_GUI_XP_VISUAL_STYLES, OnSelectTheme )
	ON_BN_CLICKED( IDC_ENABLE_SKINS, OnEnableSkins )
	ON_CBN_SELCHANGE( IDC_SKINS, CEGParamsPage::OnSetDirty )

END_MESSAGE_MAP()


// CEGAppParamsPage message handlers


BOOL CEGAppParamsPage::Save(){ 
	
	if( BST_CHECKED == IsDlgButtonChecked( IDC_GUI_CLASSIC ) ) {
		m_nNewThemeID = 0;
	} else if( BST_CHECKED == IsDlgButtonChecked( IDC_GUI_OFFICEXP ) ) {
		m_nNewThemeID = 1;
	} else if( BST_CHECKED == IsDlgButtonChecked( IDC_GUI_ICY ) ) {
		m_nNewThemeID = 3;
	} else { // office 2003
		m_nNewThemeID = 2;
	}	

	m_nNewSkinUsed = BST_CHECKED == IsDlgButtonChecked( IDC_ENABLE_SKINS ) ? 1 : 0;
	GetDlgItemText( IDC_SKINS, m_strNewSkin );


	AfxGetApp()->WriteProfileInt( _T("GUI"),_T("THEME_ID"), m_nNewThemeID );
	AfxGetApp()->WriteProfileInt( _T("GUI"),_T("SKIN_USED"), m_nNewSkinUsed );
	AfxGetApp()->WriteProfileString( _T("GUI"),_T("SKIN"), m_strNewSkin );

	return TRUE;
};

void CEGAppParamsPage::OnBeforeClose(){
	if ( m_nOldThemeID != m_nNewThemeID || m_nOldSkinUsed != m_nNewSkinUsed || m_strOldSkin != m_strNewSkin) {
		SetupTheme( m_nNewThemeID, m_nNewSkinUsed, m_strNewSkin );
		if ( m_pDefaultNewMenu )
			UpdateMenuBarColor( *m_pDefaultNewMenu );
		::InvalidateRect( NULL, NULL, true);
	}
}

BOOL CEGAppParamsPage::OnInitDialog()
{
	CEGParamsPage::OnInitDialog();
	
	int nThemeID;
	switch( CEGMenu::GetMenuDrawMode() ) {
		case CEGMenu::STYLE_ORIGINAL_NOBORDER:
			CheckDlgButton( IDC_GUI_CLASSIC, BST_CHECKED );
			nThemeID = 0;
			break;
		case CEGMenu::STYLE_XP:
			CheckDlgButton( IDC_GUI_OFFICEXP, BST_CHECKED );
			nThemeID = 1;
			break;
		case CEGMenu::STYLE_ICY:
			CheckDlgButton( IDC_GUI_ICY, BST_CHECKED );
			nThemeID = 3;
			break;
		default: //case CEGMenu::STYLE_XP_2003:
			CheckDlgButton( IDC_GUI_OFFICE2003, BST_CHECKED );
			nThemeID = 2;
			break;
	}
	m_nNewThemeID = m_nOldThemeID = nThemeID;
	m_hSkins = ::GetDlgItem( m_hWnd, IDC_SKINS );

	CString strSkin = AfxGetApp()->GetProfileString( _T("GUI"), _T("SKIN"), _T("") );
	m_strOldSkin = strSkin;
	int nIsSkinUsed = AfxGetApp()->GetProfileInt( _T("GUI"), _T("SKIN_USED"), 0 );
	m_nOldSkinUsed = nIsSkinUsed;
	CheckDlgButton( IDC_ENABLE_SKINS, 1 == nIsSkinUsed ? BST_CHECKED : BST_UNCHECKED );
	::EnableWindow( m_hSkins, 1 == nIsSkinUsed ? TRUE : FALSE );
	

	// поиск тем
	TCHAR szFilename[ MAX_PATH + 1 ] = _T("");
	if ( 0 != GetModuleFileName( NULL, szFilename, MAX_PATH ) ) {
		PathRemoveFileSpec( szFilename );
		PathAppend( szFilename, _T("Skins") );
		PathAddBackslash( szFilename );
		_tcscat( szFilename, _T("*.skin") );
		WIN32_FIND_DATA ffd;
		HANDLE hFind = FindFirstFile( szFilename, &ffd );
		if ( INVALID_HANDLE_VALUE != hFind ) {
			do {
				if ( ( ffd.dwFileAttributes &  FILE_ATTRIBUTE_DIRECTORY ) == 0 ) {
					_tcscpy( szFilename, ffd.cFileName );
					PathRemoveExtension( szFilename );
					int nItem = ComboBox_AddString( m_hSkins, szFilename );
					if ( szFilename == strSkin )
						ComboBox_SetCurSel( m_hSkins, nItem );
				}
			}while( FindNextFile( hFind, &ffd) );
		}
		FindClose( hFind );
	}

	if ( ComboBox_GetCount( m_hSkins ) == 0 ) 
		::EnableWindow( m_hSkins, FALSE );

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CEGAppParamsPage::OnEnableSkins() {
	CEGParamsPage::OnSetDirty();

	int nItemCount = ComboBox_GetCount( m_hSkins );

	::EnableWindow( m_hSkins, ( nItemCount > 0 ) && ( BST_CHECKED == IsDlgButtonChecked( IDC_ENABLE_SKINS ) ) ? TRUE : FALSE );
}

void CEGAppParamsPage::OnSelectTheme() {
	CEGParamsPage::OnSetDirty();

	if( BST_CHECKED == IsDlgButtonChecked( IDC_GUI_CLASSIC ) ||
		BST_CHECKED == IsDlgButtonChecked( IDC_GUI_XP_VISUAL_STYLES )
		) {
			::EnableWindow( ::GetDlgItem( m_hWnd, IDC_ENABLE_SKINS ), FALSE );
			::EnableWindow( m_hSkins, FALSE );
	} else {
			::EnableWindow( ::GetDlgItem( m_hWnd, IDC_ENABLE_SKINS ), TRUE );

			int nItemCount = ComboBox_GetCount( m_hSkins );
			::EnableWindow( m_hSkins, ( nItemCount > 0 ) && ( BST_CHECKED == IsDlgButtonChecked( IDC_ENABLE_SKINS ) ) ? TRUE : FALSE );
	}
}
