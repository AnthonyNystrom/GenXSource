#ifndef __FRAMEWORK_PLATFORM_SYMBIAN_H__
#define __FRAMEWORK_PLATFORM_SYMBIAN_H__

#include <BITDEV.H>
#include <w32std.h>
#include "coemain.h"
#include "e32std.h"


#include <eikapp.h>
#include <eikdoc.h>
#include <aknappui.h>
#include <coecntrl.h>
#include "Application.h"
#include "IApplicationCore.h"


//void* operator new(unsigned int size) throw();
//void* operator new[](unsigned int size) throw();
//
//void operator delete(void* p) throw ();
//void operator delete[](void* p) throw ();



#define LOG_NAME_MAX_LEN 255

class CFrameworkAppView;
class CFbsBitmap;
class CFbsBitmapDevice;
class CFbsBitGc;

typedef unsigned long       DWORD;

class CAknAppUi;

/**
* CXPlorerApplication application class.
* Provides factory to create concrete document object.
* An instance of CXPlorerApplication is the application part of the
* AVKON application framework for the XPlorer example application.
*/
class BaseApplication : public CEikApplication
{
public: // Functions from base classes

	/**
	* From CApaApplication, AppDllUid.
	* @return Application's UID (KUidXPlorerApp).
	*/
	TUid AppDllUid() const;

protected: // Functions from base classes

	/**
	* From CApaApplication, CreateDocumentL.
	* Creates CXPlorerDocument document object. The returned
	* pointer in not owned by the CXPlorerApplication object.
	* @return A pointer to the created document object.
	*/
	CApaDocument* CreateDocumentL();
};


/**
* CXPlorerDocument application class.
* An instance of class CXPlorerDocument is the Document part of the
* AVKON application framework for the XPlorer example application.
*/
class BaseDocument : public CEikDocument
{
public: // Constructors and destructor

	/**
	* NewL.
	* Two-phased constructor.
	* Construct a CXPlorerDocument for the AVKON application aApp
	* using two phase construction, and return a pointer
	* to the created object.
	* @param aApp Application creating this document.
	* @return A pointer to the created instance of CXPlorerDocument.
	*/
	static BaseDocument* NewL( CEikApplication& aApp );

	/**
	* NewLC.
	* Two-phased constructor.
	* Construct a CXPlorerDocument for the AVKON application aApp
	* using two phase construction, and return a pointer
	* to the created object.
	* @param aApp Application creating this document.
	* @return A pointer to the created instance of CXPlorerDocument.
	*/
	static BaseDocument* NewLC( CEikApplication& aApp );

	/**
	* ~CXPlorerDocument
	* Virtual Destructor.
	*/
	virtual ~BaseDocument();

public: // Functions from base classes

	/**
	* CreateAppUiL
	* From CEikDocument, CreateAppUiL.
	* Create a CXPlorerAppUi object and return a pointer to it.
	* The object returned is owned by the Uikon framework.
	* @return Pointer to created instance of AppUi.
	*/
	CEikAppUi* CreateAppUiL();

private: // Constructors

	/**
	* ConstructL
	* 2nd phase constructor.
	*/
	void ConstructL();

	/**
	* BaseDocument.
	* C++ default constructor.
	* @param aApp Application creating this document.
	*/
	BaseDocument( CEikApplication& aApp );

};


class BaseView;
// CLASS DECLARATION
/**
* BaseAppUi application UI class.
* Interacts with the user through the UI and request message processing
* from the handler class
*/
class BaseAppUi : public CAknAppUi
{
public: // Constructors and destructor

	/**
	* ConstructL.
	* 2nd phase constructor.
	*/
	void ConstructL();

	/**
	* CXPlorerAppUi.
	* C++ default constructor. This needs to be public due to
	* the way the framework constructs the AppUi
	*/
	BaseAppUi();

	/**
	* ~CXPlorerAppUi.
	* Virtual Destructor.
	*/
	virtual ~BaseAppUi();

private:  // Functions from base classes

	/**
	* From CEikAppUi, HandleCommandL.
	* Takes care of command handling.
	* @param aCommand Command to be handled.
	*/
	void HandleCommandL( TInt aCommand );

	/**
	*  HandleStatusPaneSizeChange.
	*  Called by the framework when the application status pane
	*  size is changed.
	*/

	void HandleStatusPaneSizeChange();

public:

	inline BaseView* GetView() { return iAppView; }

	//void SetSockServ(RSocketServ* pServ) { iSockServer = pServ; }
	//RSocketServ* SockServ() { return iSockServer; }

	//void SetSockConnection(RConnection* pConnection) { iConnection = pConnection; }
	//RConnection* SockConnection() { return iConnection; }

	void NeedToExit();

private: // Data

	/**
	* The application view
	* Owned by BaseAppUi
	*/
	BaseView* iAppView;
	//RSocketServ* iSockServer;
	//RConnection* iConnection;
protected:
	void HandleForegroundEventL(TBool aForeground);

};

class SymbianApplication;
class BaseView :  public CCoeControl
{
public:
	static BaseView* NewL( const TRect& aRect );

	static BaseView* NewLC( const TRect& aRect );


	BaseView(void);
	~BaseView(void);

	virtual void Draw( const TRect& aRect ) const;
	virtual TKeyResponse OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType);
	void ConstructL(const TRect& aRect);
	CAknAppUi* AppUi();
	void SetAppUi(BaseAppUi* pAppUI);

	inline RWindow& Window() const
	{
		return CCoeControl::Window();
	}

	void Exit();
	inline SymbianApplication* GetApplication() 
	{
		return m_SymbApp;
	}

	virtual void SizeChanged();


	void StartAppLoop();



protected:
	SymbianApplication* m_SymbApp;
	IApplicationCore* m_pCore;
	BaseAppUi* m_pAppUI;
	TInt 	   iTimerCounter;
};




class SymbianApplication : 
	public Application,
	public CTimer
{
public:
	SymbianApplication();
	virtual ~SymbianApplication();

	bool				InitInstance(BaseView* pView);
	void				StartAppLoop(IApplicationCore* pCore);
	uint16				KeyDecode(uint16 wParam);


	static	void		CompleteWithAppPath(TDes& aPath);

	static char8		logName[LOG_NAME_MAX_LEN];
	BaseView*			m_pView;

	bool ProcessKeyEvent(const TKeyEvent& aKeyEvent, TEventCode aType);

	void Suspend();
	void Resume();

private:
	bool			OnStart();
	bool			OnStop();

	virtual void		OnSuspend();
	virtual void		OnResume();

	virtual uint32	 GetDisplayCount();
	virtual Application::Display* GetDisplayArray();
	virtual void	 CloseApplication();
	virtual void		ExceptionHandling();

	void			OnUpdate();


//From CTimer
	void RunL();
	void DoCancel();


	static const int32 MIN_SLEEP_TIME	=	15;
	static const int32 ELAPSED_TIME		=	80;

	Display*		pDisplay;

public:
	CFbsBitmap*		m_pBitmap;
	TUint32* m_pBitmapBuffer;
	TSize screenSize; 

	friend class CFrameworkAppView;
};

#endif // __FRAMEWORK_PLATFORM_WINCE_H__
