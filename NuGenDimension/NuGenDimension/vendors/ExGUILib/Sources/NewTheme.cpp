#include "StdAfx.h"
#include "newtheme.h"
#include "EGMenu.h"

CNewTheme::CNewTheme(void)
{
}

CNewTheme::~CNewTheme(void)
{
}

void SetupTheme( int nThemeID, int nSkinUsed, CString strSkin ) {
//	GetPaintManager().UninstallHook();
	switch( nThemeID ) {
		case 0:
			CEGMenu::SetMenuDrawMode( CEGMenu::STYLE_ORIGINAL_NOBORDER );
			nSkinUsed = 0;
			break;
		case 1:
			CEGMenu::SetMenuDrawMode( CEGMenu::STYLE_XP );
			break;
		case 3:
			CEGMenu::SetMenuDrawMode( CEGMenu::STYLE_ICY );
			break;
		default:
			CEGMenu::SetMenuDrawMode( CEGMenu::STYLE_XP_2003 );
			break;
	}

	/*
	if ( 1 == nSkinUsed && !strSkin.IsEmpty() ) {
		TCHAR szFilename[ MAX_PATH + 1 ] = _T("");
		if ( 0 != GetModuleFileName( NULL, szFilename, MAX_PATH ) ) {
			
			// Составляем имя файла
			PathRemoveFileSpec( szFilename );
			PathAppend( szFilename, _T("Skins") );
			PathAddBackslash( szFilename );
			_tcscat( szFilename, (TCHAR*)(LPCTSTR)strSkin );
			_tcscat( szFilename, _T(".skin") );

			// Ищем указанный файл		
			WIN32_FIND_DATA ffd;
			HANDLE hFind = FindFirstFile( szFilename, &ffd );
			if ( INVALID_HANDLE_VALUE != hFind ) {
				GetPaintManager().InstallHook( szFilename );
				FindClose( hFind );
			}
		}
	}
	*/
}

void InstallThemeSupport(){
	int nThemeID = AfxGetApp()->GetProfileInt( _T("GUI"), _T("THEME_ID"), 2);
	int nSkinUsed = AfxGetApp()->GetProfileInt( _T("GUI"), _T("SKIN_USED"), 2);
	CString strSkin = AfxGetApp()->GetProfileString( _T("GUI"), _T("SKIN"), _T(""));

	SetupTheme( nThemeID, nSkinUsed, strSkin );
	CEGMenu::SetXpBlending(0);
  CEGMenu::SetAcceleratorsDraw(1);
}


