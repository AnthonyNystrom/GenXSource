/*
============================================================================
Name        : SupAppUi.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Main application UI class (controller)
============================================================================
*/

#ifndef __SUPAPPUI_H__
#define __SUPAPPUI_H__

// INCLUDES
#include <aknappui.h>
#include <eikmobs.h>
#include "common.h"

// FORWARD DECLARATIONS
class CSupAppLoginView;
class CSupAppMainView;
class CSupAppFileBrowserView;
class CSupAppImageCartView;
class CSupSplashScreenView;

class CTimer;

class CSplashScreenTimer : public CTimer
{
public:

	static CSplashScreenTimer* NewL( const TInt aPriority, MRequestObserver& aNotify );
	static CSplashScreenTimer* NewLC( const TInt aPriority, MRequestObserver& aNotify );
	void Start( TInt aTime );
	virtual ~CSplashScreenTimer();

protected: // from CTimer

	/**
	* Active object post-request handling.
	*/
	void RunL();
	TInt RunError( TInt aError );

private:

	/**
	* Constructor.
	* @param aPriority Priority of the active object.
	* @param aNotify A handle to the class to be notified of a timeout event.
	*/
	CSplashScreenTimer( const TInt aPriority, MRequestObserver& aNotify);
	void ConstructL();

private:

	// Handle to the class to be notified of a timeout event.
	MRequestObserver& iNotify;
};


// CLASS DECLARATION
/**
* CSupAppUi application UI class.
* Interacts with the user through the UI and request message processing
* from the handler class
*/
class CSupAppUi : public CAknAppUi, public MRequestObserver//, public MEikAutoMenuObserver
{
public: // Constructors and destructor

	/**
	* ConstructL.
	* 2nd phase constructor.
	*/
	void ConstructL();

	/**
	* CSupAppUi.
	* C++ default constructor. This needs to be public due to
	* the way the framework constructs the AppUi
	*/
	CSupAppUi();

	/**
	* ~CSupAppUi.
	* Virtual Destructor.
	*/
	virtual ~CSupAppUi();

public: // New added methods,
	// just return pointers to application views

	/**
	*
	*/
	CSupAppLoginView* GetLoginView() { return iAppLoginView; }

	/**
	*
	*/
	CSupAppMainView* GetMainView() { return iAppMainView; }

	/**
	*
	*/
	CSupAppFileBrowserView* GetFilebrowserView() { return iAppFileBrowserView; }

	/**
	*
	*/
	CSupAppImageCartView* GetImageCartView() { return iAppImageCartView; }


protected: // From MRequestObserver

	void HandleRequestCompletedL(const TInt& aError);

private:  // Functions from base classes

	/**
	* From MEikAutoMenuObserve, DynInitMenuPaneL.
	* Allows to change menu items dynamically.
	* @param aResourceId Menu pane id
	* @param aMenuPane Pointer to the menu pane object
	*/
	void DynInitMenuPaneL(TInt aResourceId, CEikMenuPane* aMenuPane);

public:
	/**
	* From CEikAppUi, HandleCommandL.
	* Takes care of command handling.
	* @param aCommand Command to be handled.
	*/
	void HandleCommandL( TInt aCommand );

private:
	/**
	*  HandleStatusPaneSizeChange.
	*  Called by the framework when the application status pane
	*  size is changed.
	*/
	void HandleStatusPaneSizeChange();

	/**
	*  From CCoeAppUi, HelpContextL.
	*  Provides help context for the application.
	*  size is changed.
	*/
	CArrayFix<TCoeHelpContext>* HelpContextL() const;

	/**
	* Shows privacy statement if needed.
	*/
	void PrivacyStatementL();

	/**
	* Reads unicode file from specified location.
	* @param aFs reference to open file session
	* @param aFileName full file name
	* @return heap descriptor which contains whole file
	*/
	static HBufC * ReadUnicodeFileL(RFs& aFs, const TDesC& aFileName);

private: // Data

	CSplashScreenTimer* iTimer;

	/**
	* The login view
	* Owned by CSupAppUi
	*/
	CSupAppLoginView* iAppLoginView;

	/**
	* The main view
	* Owned by CSupAppUi
	*/
	CSupAppMainView* iAppMainView;


	/**
	* The file browser view
	* Owned by CSupAppUi
	*/
	CSupAppFileBrowserView* iAppFileBrowserView;

	/**
	* The upload image card view
	* Owned by CSupAppUi
	*/
	CSupAppImageCartView* iAppImageCartView;

	/**
	*
	*/
	CSupSplashScreenView* iAppSplashScreenView;

	/**
	* Indicates whether splash screen has been just shown
	*/
	TBool iSplashScreenShown;

};

#endif // __SUPAPPUI_H__

// End of File
