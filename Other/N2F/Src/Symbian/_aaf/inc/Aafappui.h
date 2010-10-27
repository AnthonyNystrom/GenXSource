/*
============================================================================
Name        : AafAppUi.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Main application UI class (controller)
============================================================================
*/

#ifndef __AAFAPPUI_H__
#define __AAFAPPUI_H__

// INCLUDES
#include <aknappui.h>
#include <eikmobs.h>
#include <aknwaitdialog.h>
#include "common.h"

// FORWARD DECLARATIONS
class CAafAppLoginView;
class CAafAppMainView;
class CAafAppFileBrowserView;
class CAafAppImageCartView;
class CAafSplashScreenView;

#if USE_CAMERA
class CAafCameraLandscapeContainer;
class CAafCameraController;
class CAafCameraLandscapeView;
#endif

class CAafAppCameraView;
class CAafAppPQuestionsView;
class CAafAppMyQuestionsView;
class CAafAppCommentsView;

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
* CAafAppUi application UI class.
* Interacts with the user through the UI and request message processing
* from the handler class
*/
class CAafAppUi : public CAknAppUi, public MRequestObserver, public MProgressDialogCallback
{
public: // Constructors and destructor

	/**
	* ConstructL.
	* 2nd phase constructor.
	*/
	void ConstructL();

	/**
	* CAafAppUi.
	* C++ default constructor. This needs to be public due to
	* the way the framework constructs the AppUi
	*/
	CAafAppUi();

	/**
	* ~CAafAppUi.
	* Virtual Destructor.
	*/
	virtual ~CAafAppUi();

public: // New added methods,
	// just return pointers to application views

	CAafAppLoginView* GetLoginView() { return iAppLoginView; }

	CAafAppMainView* GetMainView() { return iAppMainView; }

	CAafAppFileBrowserView* GetFilebrowserView() { return iAppFileBrowserView; }

	CAafAppImageCartView* GetImageCartView() { return iAppImageCartView; }

	CAafAppPQuestionsView* GetPrivateQuestionsView() { return iAppPQuestionsView; }

	CAafAppMyQuestionsView* GetUserQuestionsView() { return iAppMyQuestionsView; }
	
	CAafAppCommentsView* GetQuestionCommentsView() { return iAppQuestionCommentsView; }

protected: // From MRequestObserver

	void HandleRequestCompletedL(const TInt& aError);

	/*
	* New functions
	* Starts wait dialog
	*/
	void StartWaitDialog(TInt aDialogId);

	/**
	* Stops the wait dialog
	*/
	void StopWaitDialog();

	/**
	* Get's called when a dialog is dismissed
	* From MProgressDialogCallback
	*/
	void DialogDismissedL(TInt aButtonId);

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

private:
	/**
	* Opens contacts selection dialog and sms editor
	*/
	void InviteFriendDialog();

	/**
	*
	*/
	TInt ShowQueryDialogL(CDesCArrayFlat* aOptionsArrray, TInt &aSelectedOption);

private: // Data

	// For internal use only
	enum EOperation
	{
		KNoneOperation = -1,
		KLogging,
		KSubmittingQuestion		
	};

	CSplashScreenTimer* iTimer;

	/**
	* The login view
	* Owned by CAafAppUi
	*/
	CAafAppLoginView* iAppLoginView;

	/**
	* The main view
	* Owned by CAafAppUi
	*/
	CAafAppMainView* iAppMainView;


	/**
	* The file browser view
	* Owned by CAafAppUi
	*/
	CAafAppFileBrowserView* iAppFileBrowserView;

	/**
	* The upload image card view
	* Owned by CAafAppUi
	*/
	CAafAppImageCartView* iAppImageCartView;

	/**
	*
	*/
	CAafSplashScreenView* iAppSplashScreenView;

	/**
	*
	*/
	CAafAppPQuestionsView* iAppPQuestionsView;

	/**
	*
	*/
	CAafAppMyQuestionsView* iAppMyQuestionsView;

	/**
	*
	*/
	CAafAppCommentsView* iAppQuestionCommentsView;
	

#if USE_CAMERA
	/**
	*
	*/
	CAafAppCameraView* iAppCameraView;
#endif

	EOperation iCurrentOperation;

	TInt iPhotosCount; // Currently submitted question photos count

	TInt iCurrentPhoto; // Currently processed image order index

	CAknWaitDialog* iWaitDialog; // Wait dialog
};

#endif // __AAFAPPUI_H__

// End of File
