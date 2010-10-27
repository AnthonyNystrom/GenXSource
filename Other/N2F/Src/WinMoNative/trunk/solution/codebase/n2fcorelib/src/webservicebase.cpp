#include "stdafx.h"
#include "webservicebase.h"


N2FCORE_API WebServiceBase::WebServiceBase( TWebServiceType wsType )
	:iType(wsType),
	iCustomEndPointWasSet(false),
	iLastSoapResult(0),
	iIsInitialized(false)
{

}

N2FCORE_API WebServiceBase::~WebServiceBase()
{

}

N2FCORE_API TWebServiceType WebServiceBase::GetType()
{
	return iType;
}

N2FCORE_API bool WebServiceBase::IsCustomeEndPointSet()
{
	return iCustomEndPointWasSet;
}

N2FCORE_API void WebServiceBase::SetCustomEndPoint( CString& customEndPoint )
{
	iCustomEndPoint = customEndPoint;
	if ( iCustomEndPoint.GetLength() > 0 )
		iCustomEndPointWasSet = true;
	else
		iCustomEndPointWasSet = false;
}

N2FCORE_API void WebServiceBase::GetCustomEndPoint( CString& customEndPoint )
{
	customEndPoint = iCustomEndPoint;
}

N2FCORE_API bool WebServiceBase::Initialize( CString& customEndPoint )
{
	// empty initialization in base class

	this->SetCustomEndPoint(customEndPoint);

	this->iIsInitialized = true;

	return true;
}

N2FCORE_API int WebServiceBase::GetLastSoapResult()
{
	return this->iLastSoapResult;
}

N2FCORE_API void WebServiceBase::SetLastSoapResult( int resultCode )
{
	this->iLastSoapResult = resultCode;
}

N2FCORE_API bool WebServiceBase::IsInitialized()
{
	return iIsInitialized;
}
