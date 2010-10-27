#include "stdafx.h"
#include <controllerwebservices.h>

#include <controllerutil.h>

#include <webservice-n2f-memberservice.h>
#include <webservice-n2f-memberservice-v2.h>
#include <webservice-n2f-photoorganise.h>
#include <webservice-n2f-photoorganise-v2.h>
#include <webservice-n2f-memberservice-v3.h>
#include <webservice-n2f-snapupservice.h>



// gsoap ws headers
#include <stdsoap2.h>

#include <WS_PhotoOrganisePhotoOrganiseSoapProxy.h>

N2FCORE_API ControllerWebServices::ControllerWebServices():
	iPhotoOrganise(NULL)
{
	LOGMSG("costruction");
}

N2FCORE_API ControllerWebServices::~ControllerWebServices()
{
	LOGMSG("destructing");

	for ( int i = 0; i < iSupportedServices.GetSize(); ++i )
	{
		delete iSupportedServices.GetValueAt(i);
	}

	if ( iPhotoOrganise )
		delete iPhotoOrganise;

	LOGMSG("destructed");
}

//N2FCORE_API bool ControllerWebServices::InitializeController()
//{
//	LOGMSG("intialization");
//	bool result = false;
//
//
//
//	iPhotoOrganise = new WS_PhotoOrganise::PhotoOrganiseSoap();
//	ASSERT(iPhotoOrganise);
//	if ( NULL == iPhotoOrganise )
//	{
//		return result;
//	}
//
//	result = true;
//	return result;
//}

N2FCORE_API bool ControllerWebServices::InitializeController(TWSCutsomEPList& list)
{
	LOGMSG("Initializing");

	bool result = false;

	CSimpleArray<WebServiceBase*> arrayServices;

	arrayServices.Add(new WebServiceN2FMemberService);
	arrayServices.Add(new WebServiceN2FMemberService_v2);
	arrayServices.Add(new WebServiceN2FPhotoOrganise);
	arrayServices.Add(new WebServiceN2FPhotoOrganise_v2);
	arrayServices.Add(new WebServiceN2FMemberService_v3);
	arrayServices.Add(new WebServiceN2FSnapUpService);
	// add all supported web-services here

	for ( int i = 0; i < arrayServices.GetSize(); ++i )
	{
		iSupportedServices.Add(arrayServices[i]->GetType(), arrayServices[i]);
	}

	// custom end-points initialization
	for ( int i = 0; i < list.GetSize(); ++i )
	{
		int idxFound = iSupportedServices.FindKey(list[i].wsType);
		if ( -1 != idxFound )
		{
			iSupportedServices.GetValueAt(idxFound)->Initialize(list[i].wsEndPoint);
		}
	}

	CString csEmpty;
	for ( int i = 0; i < iSupportedServices.GetSize(); ++i )
	{
		WebServiceBase *wsb = iSupportedServices.GetValueAt(i);
		if ( false == wsb->IsInitialized() )
			wsb->Initialize(csEmpty);
	}


	result = true;
	return result;

	LOGMSG("Initialized");
}


N2FCORE_API void ControllerWebServices::GenerateCustomEndPointsList( TWSCutsomEPList& list)
{
	list.RemoveAll();

	TWSCutsomEPList list1, list2;

	TWSCustomEndPointDescriptor customEP;

	//// 1st list
	//customEP.wsType = EWS_N2F_MemberServices;
	//customEP.wsEndPoint = CString("http://next2friends.com:90/MemberServices.asmx");
	//list1.Add(customEP);

	// end of 1st list

	// 2nd list 
	//customEP.wsType = EWS_N2F_MemberServices;
	//customEP.wsEndPoint = CString("http://services.next2friends.com:80/n2fwebservices/memberservices.asmx");
	//list2.Add(customEP);

	// end of 2nd list

	//list = list1;
	list = list2;
}

N2FCORE_API WebServiceBase * ControllerWebServices::GetWebService( TWebServiceType wsType )
{
	WebServiceBase *wsBase = NULL;

	int idxFound = iSupportedServices.FindKey(wsType);
	if ( -1 != idxFound )
	{
		wsBase = iSupportedServices.GetValueAt(idxFound);
	}

	return wsBase;
}

N2FCORE_API bool ControllerWebServices::Execute_N2F_MemberService_CheckUserExists( WebServiceN2FMemberServiceLoginMethodDataProvider *dataProvider, bool& userExists )
{
	LOGME();
	userExists = false;

	if ( NULL == dataProvider )
		return false;

#if OFFLINE_MODE
	userExists = true;
	return true;
#endif	//#if OFFLINE_MODE


	

	bool executionResult = false;

	TWebServiceType loginService = ControllersHost::GetInstance()->SettingsController()->GetLoginServiceID();

	if ( EWS_N2F_MemberService_v3 == loginService )
	{
		CString nick, password;
		dataProvider->GetUsername( nick );
		dataProvider->GetPassword( password );

		WebServiceN2FMemberService_v3 *wsMS = (WebServiceN2FMemberService_v3 *)this->GetWebService( loginService );
		if ( NULL != wsMS )
			executionResult = wsMS->CheckUserExists( nick, password, userExists );
		else
			ASSERT( false );

		return executionResult;
	}

	LOGMSG("Unsupported web-service type: %d", loginService );
	ASSERT(false);
	return false;
}

N2FCORE_API bool ControllerWebServices::Execute_N2F_SnapUpService_DeviceUploadPhoto( WebServiceN2FSnapUpServiceDeviceUploadMethodDataProvider *dataProvider )
{
	LOGME();
	if ( NULL == dataProvider )
		return false;

#if OFFLINE_MODE
	return true;
#endif	//#if OFFLINE_MODE


	bool executionResult = false;

	TWebServiceType uploadService = ControllersHost::GetInstance()->SettingsController()->GetUploadServiceID();

	if ( EWS_N2F_SnapUpService == uploadService )
	{
		CString nick, password, filePath;
		SYSTEMTIME st;
		dataProvider->GetUsername( nick );
		dataProvider->GetPassword( password );
		dataProvider->GetFilePathToUpload( filePath );
		dataProvider->GetTimeForUpload( st );

		WebServiceN2FSnapUpService *wsSU = (WebServiceN2FSnapUpService *)this->GetWebService( uploadService );
		if ( NULL != wsSU )
			executionResult = wsSU->DeviceUploadPhoto( nick, password, filePath, st );
		else
			ASSERT( false );

		return executionResult;
	}

	LOGMSG("Unsupported web-service type: %d", uploadService );
	ASSERT(false);
	return false;

	
}