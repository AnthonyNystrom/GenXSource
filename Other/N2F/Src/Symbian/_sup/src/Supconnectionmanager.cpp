#include <s32file.h>
#include <aknlists.h> 
#include <commdb.h>
#include <aknpopup.h> 
#include <CommDbConnPref.h>

#include "Supconnectionmanager.h"

CSupConnectionManager* CSupConnectionManager::iSelfInstance = NULL;


CSupConnectionManager::CSupConnectionManager()
{
	iSocketServ = NULL;
	iConnection = NULL;
	iIAPId = 0;
	iSettingsFile = TFileName(KNullDesC);
}

CSupConnectionManager::~CSupConnectionManager()
{
	if (iSocketServ)
	{
		delete iSocketServ;
		iSocketServ = NULL;
	}

	if (iConnection)
	{
		delete iConnection;
		iConnection = NULL;
	}
}

void CSupConnectionManager::ConstructL()
{
	// Set connection manager settings file path
	RFs &fs = CCoeEnv::Static()->FsSession();

	TFileName privatePath;
	fs.PrivatePath(privatePath);
	TParse parser;
	TFileName processFileName(RProcess().FileName());
	User::LeaveIfError(parser.Set(KConnManagerSettingsFilename, &privatePath, &processFileName));
	iSettingsFile = parser.FullName();

	// Read IAP description form file
	ReadSavedIAPL();
}

CSupConnectionManager* CSupConnectionManager::GetInstanceL()
{
	if (!iSelfInstance)
	{
		iSelfInstance = new (ELeave)CSupConnectionManager;

		iSelfInstance->ConstructL();
	}

	return iSelfInstance;
}

void CSupConnectionManager::SelectIAPL()
{
	__LOGSTR_TOFILE("CSupConnectionManager::SelectIAPL() begins");

	CArrayFixFlat<TIapData>* iEApList = new (ELeave) CArrayFixFlat<TIapData>(2);
	TInt stack = 0;

	// Make listitems. and PUSH it
	CAknSinglePopupMenuStyleListBox* list = new(ELeave) CAknSinglePopupMenuStyleListBox;
	CleanupStack::PushL(list);
	stack++; 

	// Create popup list and PUSH it.
	CAknPopupList* popupList = CAknPopupList::NewL(list, R_AVKON_SOFTKEYS_OK_CANCEL, AknPopupLayouts::EMenuWindow);
	CleanupStack::PushL(popupList);
	stack++; 

	CDesCArrayFlat* items = new (ELeave) CDesCArrayFlat(5);
	CleanupStack::PushL(items);
	stack++; 

	// initialize listbox.
	list->ConstructL(popupList, CEikListBox::ELeftDownInViewRect);
	list->CreateScrollBarFrameL(ETrue);
	list->ScrollBarFrame()->SetScrollBarVisibilityL(CEikScrollBarFrame::EOff,
		CEikScrollBarFrame::EAuto);


	TBuf<52> iapfromtable;
	TInt err = KErrNone;

	CCommsDatabase* iCommsDB = CCommsDatabase::NewL(EDatabaseTypeIAP);
	CleanupStack::PushL(iCommsDB);
	stack++;

#ifdef __SERIES60_3X__
	CCommsDbTableView* gprsTable = iCommsDB->OpenIAPTableViewMatchingBearerSetLC(
		ECommDbBearerGPRS|ECommDbBearerWLAN|ECommDbBearerVirtual,
		ECommDbConnectionDirectionOutgoing); 
#else
	CCommsDbTableView* gprsTable = iCommsDB->OpenIAPTableViewMatchingBearerSetLC(
		ECommDbBearerGPRS|ECommDbBearerVirtual,
		ECommDbConnectionDirectionOutgoing); 
#endif
	User::LeaveIfError(gprsTable->GotoFirstRecord());
	TInt i = 0;
	TUint32 id;
	TIapData eap;	

	TInt cur = 0; // Target IAP id

	do
	{
		gprsTable->ReadTextL(TPtrC(COMMDB_NAME), iapfromtable);
		gprsTable->ReadUintL(TPtrC(COMMDB_ID), id);
		items->AppendL(iapfromtable);
		eap.iIap = id;
		eap.iName.Copy(iapfromtable);
		iEApList->AppendL(eap);

		err = gprsTable->GotoNextRecord();
		i++;
	}
	while (err == KErrNone);

	CleanupStack::PopAndDestroy(2);
	stack--;

	// Set listitems.
	CTextListBoxModel* model = list->Model();
	model->SetItemTextArray(items);
	model->SetOwnershipType(ELbmOwnsItemArray);
	CleanupStack::Pop();    

	popupList->SetTitleL(_L("Select access point"));
	list->SetListBoxObserver(popupList);
	TInt popupOk = popupList->ExecuteLD();
	CleanupStack::Pop();  

	TInt iap = 0;

	if (popupOk)
	{	
		TInt index = list->CurrentItemIndex();
		iap = (*iEApList)[index].iIap;

		if (iIAPId != iap)
		{
			iIAPId = iap;
		}
	}

	CleanupStack::PopAndDestroy();  
	iEApList->Reset();

	delete iEApList;
	

	__LOGSTR_TOFILE("CSupConnectionManager::SelectIAPL() ends");
}

TInt CSupConnectionManager::InstallConnectionL()
{
	__LOGSTR_TOFILE("CSupConnectionManager::InstallConnectionL() begins");

	TInt retValue = 0;

	// Try to open selected connection
	if (iSocketServ)
	{
		delete iSocketServ;
		iSocketServ = NULL;
	}

	iSocketServ = new (ELeave)RSocketServ;

	if (iConnection)
	{
		iConnection->Stop();

		delete iConnection;
		iConnection = NULL;
	}

	iConnection = new (ELeave)RConnection;

	User::LeaveIfError(iSocketServ->Connect());
	User::LeaveIfError(iConnection->Open(*iSocketServ));
	TCommDbConnPref pref;
	pref.SetIapId(iIAPId); // IAP ID for connection to be used
	pref.SetDialogPreference( ECommDbDialogPrefDoNotPrompt );
	pref.SetDirection( ECommDbConnectionDirectionOutgoing );
	retValue = iConnection->Start(pref);

	if (retValue == KErrNone)
	{
		// Save IAP description
		SaveIAPL();
	}
	else
	{
		iIAPId = 0;
	}
	
	__LOGSTR_TOFILE("CSupConnectionManager::InstallConnectionL() ends");

	return retValue;
}

TBool CSupConnectionManager::ReadSavedIAPL()
{
	__LOGSTR_TOFILE("CSupConnectionManager::ReadSavedIAPL() begins");

	TBool retValue = ETrue;

	RFs fsSession;
	RFileReadStream readStream; // Read stream from file

	// Install read file session
	User::LeaveIfError(fsSession.Connect());
	CleanupClosePushL(fsSession);

	TInt err = readStream.Open(fsSession, iSettingsFile, EFileStream | EFileRead | EFileShareExclusive);
	CleanupClosePushL(readStream);

	// If file does not exist - return EFalse
	if (err != KErrNone)
	{
		retValue = EFalse;

		__LOGSTR_TOFILE("CSupConnectionManager::ReadSavedIAPL() failed to open");
	}

	if (retValue)
	{
		// iMemberID
		iIAPId = readStream.ReadInt32L();
	}

	// Free resource handlers
	CleanupStack::PopAndDestroy(&readStream);
	CleanupStack::PopAndDestroy(&fsSession);	

	__LOGSTR_TOFILE("CSupConnectionManager::ReadSavedIAPL() ends");

	return retValue;
}

TBool CSupConnectionManager::SaveIAPL()
{
	__LOGSTR_TOFILE("CSupConnectionManager::SaveIAPL() begins");

	// Return value
	TBool retValue = EFalse;

	// If credentials data is not empty
	if (iIAPId != 0)
	{
		retValue = ETrue;

		RFs fsSession;
		RFileWriteStream writeStream; // Write file stream

		// Install write file session
		User::LeaveIfError(fsSession.Connect());
		CleanupClosePushL(fsSession);

		// Open file stream
		// if already exists - replace with newer version
		TInt err = writeStream.Replace(fsSession, iSettingsFile, EFileStream | EFileWrite | EFileShareExclusive);
		CleanupClosePushL(writeStream);

		// Return EFalse if failed to open stream
		if (err != KErrNone)
		{
			retValue = EFalse;

			__LOGSTR_TOFILE("CSupConnectionManager::SaveIAPL() failed to open file");
		}

		if (retValue)
		{
			__LOGSTR_TOFILE("CSupConnectionManager::SaveIAPL() succeed to open the file");

			// Write data
			// iIapId
			writeStream.WriteInt32L(iIAPId);
		
			// Just to ensure that any buffered data is written to the stream
			writeStream.CommitL();
		}	

		// Free resource handlers
		CleanupStack::PopAndDestroy(&writeStream);
		CleanupStack::PopAndDestroy(&fsSession);		
	}	

	__LOGSTR_TOFILE("CSupConnectionManager::SaveIAPL() ends");

	return retValue;
}