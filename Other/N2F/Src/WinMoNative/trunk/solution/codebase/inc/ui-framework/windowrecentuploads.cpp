#include "stdafx.h"
#include <windowrecentuploads.h>

CWindowRecentUploads::CWindowRecentUploads()
{
	this->SetWindowID( EWndRecentUploads );

	iLBImageMaxWidth = iLBImageMaxHeight = 60;
}

CWindowRecentUploads::~CWindowRecentUploads()
{
	this->UninitializeListboxItemsArray();
}

LRESULT CWindowRecentUploads::OnCreate( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	LOGFONT lf = {0};
	lf.lfHeight = 12;
	lf.lfWeight = FW_BOLD;
	lf.lfStrikeOut = FALSE;
	lf.lfItalic = FALSE;
	lf.lfUnderline = FALSE;
	_tcsncpy( lf.lfFaceName, _T("Tahoma"), LF_FACESIZE );
	iFontBold.CreateFontIndirect(&lf);
	lf.lfWeight = FW_NORMAL;
	iFontSmall.CreateFontIndirect(&lf);

	iImages.Create(iLBImageMaxWidth+10, iLBImageMaxHeight+10, ILC_COLOR24, 0, 1);


	iImgListbox.Create( m_hWnd, NULL, NULL, WS_CHILD|WS_VISIBLE, 0, ID_IMGLISTBOX_RECENT_UPLOADS );
	iImgListbox.SetFont( iFontBold );
	iImgListbox.SetSmallFont( iFontSmall );
	iImgListbox.SetImageList( iImages, LVSIL_SMALL );

	ILBSETTINGS cfg;
	iImgListbox.GetPreferences(&cfg);
	cfg.clrHighlite = RGB(247,247,247);
	cfg.clrHighliteText = RGB(0,0,0);
	cfg.clrHighliteBorder = RGB(148,150,148);
	cfg.clrText = RGB(0,0,0);
	cfg.clrBackground = RGB(255,255,255);
	cfg.sizeMargin.cx = cfg.sizeMargin.cy = 4;
	cfg.sizeSubIndent.cx = 10;
	cfg.sizeSubIndent.cy = 4;
	iImgListbox.SetPreferences(cfg);
	
	this->UpdateListboxContent();


	return TRUE;
}

LRESULT CWindowRecentUploads::OnSize( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	RECT rcClient = {0};
	this->GetClientRect( &rcClient );

	iImgListbox.MoveWindow( &rcClient );

	return 0;
}

LRESULT CWindowRecentUploads::OnPaint( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	CPaintDC dc(m_hWnd);

	//::SetBkMode( dc, TRANSPARENT );

	return 0;
}

void CWindowRecentUploads::GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info )
{
	TMBBInfoStruct btnInfo;

	if ( EBITLeftButton == requestType )
	{
		btnInfo.biButtonID = EMBBRecentUploadsBack;
		btnInfo.biButtonName = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUploadsBackCaption );
	}
	else if ( EBITRightButton == requestType )
	{
		btnInfo.biButtonID = EMBBRecentUploadsMenu;
		btnInfo.biButtonName = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUploadsMenuCaption );
	}

	info = btnInfo;
}

void CWindowRecentUploads::GetPopupMenuItems( TPopupMenuItemsCollection& items )
{
	items.RemoveAll();

	TPopupMenuItem tempItem;

	tempItem.caption = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUplaodsHelpItem );
	tempItem.isSeparator = false;
	tempItem.id = EMIRUploadsHelp;
	items.Add( tempItem );

	tempItem.isSeparator = true;
	tempItem.id = EMIRUploadsSeparator1;
	items.Add( tempItem );

	tempItem.caption = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUploadsUploadItem );
	tempItem.isSeparator = false;
	tempItem.id = EMIRUploadsUpload;
	items.Add( tempItem );

	tempItem.id = EMIRUploadsExitApplication;
	tempItem.isSeparator = false;
	tempItem.caption = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUploadsExitItem );
	items.Add( tempItem );
}

LRESULT CWindowRecentUploads::OnCommand( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	UINT commandID = LOWORD(wParam);

	if ( this->IsLeftButton(commandID) )
	{
		this->DoBackToPrevView();
	}
	else if ( EMIRUploadsHelp == commandID )
	{
		this->DoHelp();
	}
	else if ( EMIRUploadsUpload == commandID )
	{
		this->DoReUpload();
	}
	else if ( EMIRUploadsExitApplication == commandID )
	{
		this->DoExitApplication();
	}

	return 0;
}

LRESULT CWindowRecentUploads::OnSetFocus( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	//this->UpdateListboxContent();

	bHandled = FALSE;
	return 0;
}

LRESULT CWindowRecentUploads::OnRefreshList( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	this->UpdateListboxContent();

	return 0;
}

void CWindowRecentUploads::DoBackToPrevView()
{
	LOGME();

	this->ShowWindow( SW_HIDE );
	WindowsManager::GetInstnace()->ShowWindow( EWndOperations );
	WindowsManager::GetInstnace()->BringWindowToTop( EWndOperations );
	WindowsManager::GetInstnace()->GetWindow( EWndOperations )->SetFocus();

	LOGMSG("done");
}

void CWindowRecentUploads::DoReUpload()
{
	LOGME();

	int idx = iImgListbox.GetSelectedIndex();
	if ( -1 == idx )
	{
		LOGMSG("no item selected in listbox");
		return;
	}

	TRecentUploadItem item;
	ControllersHost::GetInstance()->SettingsController()->GetRecentUploadItemByIndex( idx, item );

	this->ShowWindow( SW_HIDE );
	WindowsManager::GetInstnace()->ShowWindow( EWndOperations );
	WindowsManager::GetInstnace()->BringWindowToTop( EWndOperations );
	WindowsManager::GetInstnace()->GetWindow( EWndOperations )->SetFocus();
	WindowsManager::GetInstnace()->GetWindow( EWndOperations )->SendMessage( WM_OPERATIONS_UPLOADFILE, 0, (LPARAM)&item );
}

void CWindowRecentUploads::UpdateListboxContent()
{
	iImgListbox.DeleteAllItems();

	this->UpdateListboxItemsArray();

	for ( int i = 0; i < iListboxItems.GetSize(); ++i )
	{
		iImgListbox.InsertItem( &(iListboxItems[i]) );
	}

	iImgListbox.SelectItem(0);

}

void CWindowRecentUploads::UpdateListboxItemsArray()
{
	this->UninitializeListboxItemsArray();

	int ruItemsCnt = ControllersHost::GetInstance()->SettingsController()->GetStoredRecentUploadsCount();
	for ( int i = 0; i < ruItemsCnt; ++i )
	{
		TRecentUploadItem item;
		ControllersHost::GetInstance()->SettingsController()->GetRecentUploadItemByIndex( i, item );

		ILBITEM lbItem = {0};
		lbItem.mask = ILBIF_IMAGE|ILBIF_SELIMAGE|ILBIF_TEXT|ILBIF_SUBTEXT|ILBIF_STYLE|ILBIF_FORMAT;
		lbItem.style = ILBS_IMGLEFT|ILBS_SELROUND;
		lbItem.format = DT_LEFT;

		lbItem.iItem = i;

		LPTSTR tmpStrPtr = new TCHAR[item.fileTitle.GetLength()+1];
		ZeroMemory( tmpStrPtr, sizeof(TCHAR)*(item.fileTitle.GetLength() + 1));
		_tcsncpy( tmpStrPtr, item.fileTitle, item.fileTitle.GetLength() );

		lbItem.pszText = tmpStrPtr;

		CString stFormat = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUploadsSubTextFormat );
		CString strUploaded = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUploadsUploadedText );
		CString strNotUploaded = ControllersHost::GetInstance()->ConfigController()->String( ESRencetUploadsNUpldText );
		
		CString subText;
		subText.Format( stFormat, item.dateTime.wYear, item.dateTime.wMonth, item.dateTime.wDay );
		subText += _T('\n');
		subText += (item.isFinished? strUploaded: strNotUploaded);

		tmpStrPtr = new TCHAR[subText.GetLength()+1];
		ZeroMemory( tmpStrPtr, sizeof(TCHAR)*(subText.GetLength() + 1));
		_tcsncpy( tmpStrPtr, subText, subText.GetLength() );

		lbItem.pszSubText = tmpStrPtr;
		//item.cchMaxSubText = _tcslen(st1);

		IMAGE_KEY_TYPE loadedImage = INVALID_IMAGE_KEY_VALUE, resizedImage = INVALID_IMAGE_KEY_VALUE;
		UINT maxWidth = iLBImageMaxWidth, maxHeight = iLBImageMaxHeight;

		ImageHelper::GetInstance()->GetImageFromFile( item.filePath, loadedImage );
		ImageHelper::GetInstance()->GetThumbnailImageForImage( loadedImage, maxWidth, maxHeight, resizedImage );

		lbItem.iImage = lbItem.iSelImage = resizedImage;

		iListboxItems.Add( lbItem );

		ImageHelper::GetInstance()->FreeImage( loadedImage );
	}
}

void CWindowRecentUploads::UninitializeListboxItemsArray()
{
	for ( int i = 0; i < iListboxItems.GetSize(); ++i )
	{
		if ( iListboxItems[i].pszText )
			delete [] iListboxItems[i].pszText;

		if ( iListboxItems[i].pszSubText )
			delete [] iListboxItems[i].pszSubText;
	}

	iListboxItems.RemoveAll();
}

void CWindowRecentUploads::GetHelpCaption( CString& caption )
{
	caption = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUploadsHelpCaption );
}

void CWindowRecentUploads::GetHelpText( CString& text )
{
	text = ControllersHost::GetInstance()->ConfigController()->String( ESRecentUploadsHelpText );
}