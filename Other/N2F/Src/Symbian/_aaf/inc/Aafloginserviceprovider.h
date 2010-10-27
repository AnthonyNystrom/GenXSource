/*
============================================================================
Name        : Aafloginserviceprovider.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Login stuff controller class
============================================================================
*/

#ifndef __AAFLOGINCONTROLLER_H__
#define __AAFLOGINCONTROLLER_H__

#include <coemain.h>
#include <msenserviceconsumer.h>
#include <e32base.h>
// For xml parsing
#include <xml\contenthandler.h> // for MContentHandler
#include <xml\parser.h> // for CParser

// FORWARD DECLARATION
class CSenServiceConnection;
class MRequestObserver;

using namespace Xml;

// Singleton class
class CAafLoginServiceProvider: public CCoeStatic, public MSenServiceConsumer, public MContentHandler
{
	friend class CAafAppLoginView;

public:
	/**
	* Static method returns an instance of the CAafLoginServiceProvider class
	*/
	static CAafLoginServiceProvider* GetInstanceL();

	/**
	* Initiates login process
	*/
	TInt StartLoginL(MRequestObserver* aObserver, const TDesC8 &aUsername, const TDesC8 &aPassword);

	/**
	* Cancel login process
	*/
	void CancelLogin();

	/**
	* Get current connection status
	*/
	TInt GetStatus();

	/**
	* Indicates whether sup app is logged in already
	*/
	TBool IsLoggedIn();

	// Getters
	/**
	*
	*/
	HBufC8* GetMemberID() const;

	/**
	*
	*/
	HBufC8* GetUsername() const;

	/**
	*
	*/
	HBufC8* GetPassword() const;

	/**
	* Set IAP value to be used by WSF
	*/
	void SetIAPL(TUint32 aIap);

	/**
	* Virtual destructor
	*/
	virtual ~CAafLoginServiceProvider();

protected: // from MSenServiceConsumer - for performing asynchronous requests to web service
	/**
	*
	*/
	void HandleMessageL(const TDesC8 &aResponse);

	/**
	*
	*/
	void HandleErrorL(const TInt aErrorCode, const TDesC8 &aMessage);

	/**
	*
	*/
	void SetStatus(const TInt aStatus);

private:
	/**
	* Default constructor
	*/
	CAafLoginServiceProvider();

	/**
	* Two-phase constructor
	*/
	void ConstructL();

	/**
	* Save member id and password to file
	*/
	TBool WriteDataToFileL();

	/**
	* Read member id and password from file
	*/
	TBool ReadDataFromFileL();

	/**
	* Send appropriate soap message (async request)
	*/
	TInt RequestMemberID(const TDesC8 &aRequestUsername, const TDesC8 &aRequestPassword);


protected: // From MContentHandler
	/**
	*
	*/
	void OnStartDocumentL( const RDocumentParameters &aDocParam,
		TInt aErrorCode );

	/**
	*
	*/
	void OnEndDocumentL( TInt aErrorCode );

	/**
	*
	*/
	void OnStartElementL( const RTagInfo &aElement,
		const RAttributeArray &aAttributes, TInt aErrorCode );

	/**
	*
	*/
	void OnEndElementL( const RTagInfo &aElement, TInt aErrorCode );

	/**
	*
	*/
	void OnContentL( const TDesC8 &aBytes, TInt aErrorCode );

	/**
	*
	*/
	void OnStartPrefixMappingL( const RString &aPrefix, const RString &aUri,
		TInt aErrorCode );

	/**
	*
	*/
	void OnEndPrefixMappingL( const RString &aPrefix, TInt aErrorCode );

	/**
	*
	*/
	void OnIgnorableWhiteSpaceL( const TDesC8 &aBytes, TInt aErrorCode );

	/**
	*
	*/
	void OnSkippedEntityL( const RString &aName, TInt aErrorCode );

	/**
	*
	*/
	void OnProcessingInstructionL( const TDesC8 &aTarget, const TDesC8 &aData,
		TInt aErrorCode);

	/**
	*
	*/
	void OnError( TInt aErrorCode );

	/**
	*
	*/
	TAny *GetExtendedInterface( const TInt32 aUid );

private: // Member variables

	CSenServiceConnection* iConnection;

	MRequestObserver* iRequestObserver; // Callback to the initiator of the login process

	HBufC8* iMemberID; // Currently used MemberID value

	HBufC8* iUsername; // Currently used username value

	HBufC8* iPassword; // Currently used password value

	TBool iLogged; // Variable indicates whether sup app is logged in

	TInt iConectionStatus; // Current connection status

	TFileName iSettingsFile; // Path to the setting file
};


#endif // __AAFLOGINCONTROLLER_H__

// End of File