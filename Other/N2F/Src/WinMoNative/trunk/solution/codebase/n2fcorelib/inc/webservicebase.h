#pragma once

#include "webservices-supported.h"

//! WebServiceBase class

/*!
	WebServiceBase class provides base functionality for webservice-wrapper class
*/

class WebServiceBase
{
public:

	//! WebServiceBase constructor

	/*!
		\param wsType type of WS supported
		\see TWebServiceType
	*/
	N2FCORE_API WebServiceBase(TWebServiceType wsType);

	//! WebServiceBase destructor
	N2FCORE_API virtual ~WebServiceBase();

	//! GetType returns type of web-service wrapped

	/*!
		GetType returns type of web-service wrapped
		\return TWebServiceType value, which is type of wrapped web service
		\see TWebServiceType
	*/
	N2FCORE_API TWebServiceType	GetType();

	//! Web-service initialization method

	/*!
		Wrapped web-service initialization method
		\param customEndPoint a custom end-point to use for web-service, if empty string is passed - default end-point is used
		\return bool as initialization result
	*/
	N2FCORE_API virtual bool Initialize(CString& customEndPoint);

	N2FCORE_API bool	IsInitialized();

	//! Resolve result code for the last SOAP operation completed
	/*! 
	Resolve result code for the last SOAP operation completed
	\return int result code ( 0 == success )
	*/
	N2FCORE_API int				GetLastSoapResult();

protected:

	//! Checks if custom end-point was set

	/*!
		Checks if custom end-point was set
		\return bool if custom end-point was set, otherwise - false
	*/
	N2FCORE_API bool			IsCustomeEndPointSet();

	//! Set custom end-point

	/*!
		Set custom end-point for wrapped web-service
		\param customEndPoint end-point value, can be empty string if endpoint should be reseted
	*/
	N2FCORE_API void			SetCustomEndPoint(CString& customEndPoint);

	//! Get custom end-point

	/*!
		Get custom end-point for wrapped web-service
		\param customEndPoint [out] end-point value
	*/
	N2FCORE_API void			GetCustomEndPoint(CString& customEndPoint);


	//! Set last soap operation result
	/*! 
		Set last soap operation result
		\param int result code ( 0 == success )
	*/
	N2FCORE_API void			SetLastSoapResult(int resultCode);

	

private:

	TWebServiceType	iType;

	bool			iIsInitialized;

	int				iLastSoapResult;

	bool			iCustomEndPointWasSet;
	CString			iCustomEndPoint;

};
