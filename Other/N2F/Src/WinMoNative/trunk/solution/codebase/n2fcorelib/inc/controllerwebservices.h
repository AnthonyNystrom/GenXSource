#pragma once

#include "webservicebase.h"


// forward declarations to eliminate not needed includes 


namespace WS_PhotoOrganise {
	class PhotoOrganiseSoap;
}

struct TWSCustomEndPointDescriptor {
	TWebServiceType wsType;
	CString			wsEndPoint;
};

typedef CSimpleArray<TWSCustomEndPointDescriptor> TWSCutsomEPList;

//! ControlerWebServices class

/*!
	ControlerWebServices class wraps all communication with gSOAP-related API
*/

class ControllerWebServices
{
public:

	//! ControllerWebServices constructor
	N2FCORE_API ControllerWebServices();

	//! ControllerWebServices destructor
	N2FCORE_API virtual ~ControllerWebServices();

	//! Initialization method
	/*! 
		Initializes all supported web-services objects
		\param list list of custom end-points for web-services
		\see GenerateCustomEndPointsList
		\return bool as initialization result
	*/
	N2FCORE_API bool InitializeController(TWSCutsomEPList& list);

	//! generates list of custom end-points for each supported web-service
	/*! 
		generates list of custom end-points for each supported web-service
		\param list [out] list of custom end-points for web-services
		\see GenerateCustomEndPointsList
	*/
	N2FCORE_API void GenerateCustomEndPointsList(TWSCutsomEPList& list);

	//! resolves web-service wrapper object by web-service type id
	/*! 
		resolves web-service wrapper object by web-service type id
		\param wsType web-service type id
		\return WebServiceBase pointer to web-service wrapper object instance
		\see TWebServiceType
	*/
	N2FCORE_API WebServiceBase *GetWebService(TWebServiceType wsType);

	N2FCORE_API bool Execute_N2F_MemberService_CheckUserExists( 
		WebServiceN2FMemberServiceLoginMethodDataProvider *dataProvider, bool& userExists );

	N2FCORE_API bool Execute_N2F_SnapUpService_DeviceUploadPhoto(
		WebServiceN2FSnapUpServiceDeviceUploadMethodDataProvider *dataProvider );

protected:
	
	CSimpleMap<TWebServiceType, WebServiceBase*>	iSupportedServices;


private:

	
	WS_PhotoOrganise::PhotoOrganiseSoap		*iPhotoOrganise;


};
