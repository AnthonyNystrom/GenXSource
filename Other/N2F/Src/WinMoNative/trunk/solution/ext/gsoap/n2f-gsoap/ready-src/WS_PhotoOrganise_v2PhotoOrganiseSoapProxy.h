/* WS_PhotoOrganise_v2PhotoOrganiseSoapProxy.h
   Generated by gSOAP 2.7.10 from .\wsdl-h\PhotoOrganise.v2.h
   Copyright(C) 2000-2008, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#ifndef WS_PhotoOrganise_v2PhotoOrganiseSoapProxy_H
#define WS_PhotoOrganise_v2PhotoOrganiseSoapProxy_H
#include "WS_PhotoOrganise_v2H.h"
extern SOAP_NMAC struct Namespace WS_PhotoOrganise_v2_namespaces[];

namespace WS_PhotoOrganise_v2 {
class PhotoOrganiseSoap
{   public:
	/// Runtime engine context allocated in constructor
	struct soap *soap;
	/// Endpoint URL of service 'PhotoOrganiseSoap' (change as needed)
	const char *endpoint;
	/// Constructor allocates soap engine context, sets default endpoint URL, and sets namespace mapping table
	PhotoOrganiseSoap() { soap = soap_new(); if (soap) soap->namespaces = WS_PhotoOrganise_v2_namespaces; endpoint = "http://services.next2friends.com/n2fwebservices/photoorganise.asmx"; };
	/// Destructor frees deserialized data and soap engine context
	virtual ~PhotoOrganiseSoap() { if (soap) { soap_destroy(soap); soap_end(soap); soap_free(soap); } };
	/// Invoke 'Login' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__Login(_PhotoOrganiseV2__Login *PhotoOrganiseV2__Login, _PhotoOrganiseV2__LoginResponse *PhotoOrganiseV2__LoginResponse) { return soap ? soap_call___PhotoOrganiseV23__Login(soap, endpoint, NULL, PhotoOrganiseV2__Login, PhotoOrganiseV2__LoginResponse) : SOAP_EOM; };
	/// Invoke 'CreateNewCollection' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__CreateNewCollection(_PhotoOrganiseV2__CreateNewCollection *PhotoOrganiseV2__CreateNewCollection, _PhotoOrganiseV2__CreateNewCollectionResponse *PhotoOrganiseV2__CreateNewCollectionResponse) { return soap ? soap_call___PhotoOrganiseV23__CreateNewCollection(soap, endpoint, NULL, PhotoOrganiseV2__CreateNewCollection, PhotoOrganiseV2__CreateNewCollectionResponse) : SOAP_EOM; };
	/// Invoke 'GetCollections' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__GetCollections(_PhotoOrganiseV2__GetCollections *PhotoOrganiseV2__GetCollections, _PhotoOrganiseV2__GetCollectionsResponse *PhotoOrganiseV2__GetCollectionsResponse) { return soap ? soap_call___PhotoOrganiseV23__GetCollections(soap, endpoint, NULL, PhotoOrganiseV2__GetCollections, PhotoOrganiseV2__GetCollectionsResponse) : SOAP_EOM; };
	/// Invoke 'GetPhotosByCollection' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__GetPhotosByCollection(_PhotoOrganiseV2__GetPhotosByCollection *PhotoOrganiseV2__GetPhotosByCollection, _PhotoOrganiseV2__GetPhotosByCollectionResponse *PhotoOrganiseV2__GetPhotosByCollectionResponse) { return soap ? soap_call___PhotoOrganiseV23__GetPhotosByCollection(soap, endpoint, NULL, PhotoOrganiseV2__GetPhotosByCollection, PhotoOrganiseV2__GetPhotosByCollectionResponse) : SOAP_EOM; };
	/// Invoke 'UploadPhoto' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__UploadPhoto(_PhotoOrganiseV2__UploadPhoto *PhotoOrganiseV2__UploadPhoto, _PhotoOrganiseV2__UploadPhotoResponse *PhotoOrganiseV2__UploadPhotoResponse) { return soap ? soap_call___PhotoOrganiseV23__UploadPhoto(soap, endpoint, NULL, PhotoOrganiseV2__UploadPhoto, PhotoOrganiseV2__UploadPhotoResponse) : SOAP_EOM; };
	/// Invoke 'DeviceUploadPhoto' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__DeviceUploadPhoto(_PhotoOrganiseV2__DeviceUploadPhoto *PhotoOrganiseV2__DeviceUploadPhoto, _PhotoOrganiseV2__DeviceUploadPhotoResponse *PhotoOrganiseV2__DeviceUploadPhotoResponse) { return soap ? soap_call___PhotoOrganiseV23__DeviceUploadPhoto(soap, endpoint, NULL, PhotoOrganiseV2__DeviceUploadPhoto, PhotoOrganiseV2__DeviceUploadPhotoResponse) : SOAP_EOM; };
	/// Invoke 'JavaUploadPhoto' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__JavaUploadPhoto(_PhotoOrganiseV2__JavaUploadPhoto *PhotoOrganiseV2__JavaUploadPhoto, _PhotoOrganiseV2__JavaUploadPhotoResponse *PhotoOrganiseV2__JavaUploadPhotoResponse) { return soap ? soap_call___PhotoOrganiseV23__JavaUploadPhoto(soap, endpoint, NULL, PhotoOrganiseV2__JavaUploadPhoto, PhotoOrganiseV2__JavaUploadPhotoResponse) : SOAP_EOM; };
	/// Invoke 'RenameCollection' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__RenameCollection(_PhotoOrganiseV2__RenameCollection *PhotoOrganiseV2__RenameCollection, _PhotoOrganiseV2__RenameCollectionResponse *PhotoOrganiseV2__RenameCollectionResponse) { return soap ? soap_call___PhotoOrganiseV23__RenameCollection(soap, endpoint, NULL, PhotoOrganiseV2__RenameCollection, PhotoOrganiseV2__RenameCollectionResponse) : SOAP_EOM; };
	/// Invoke 'DeletePhoto' of service 'PhotoOrganiseSoap' and return error code (or SOAP_OK)
	virtual int __PhotoOrganiseV23__DeletePhoto(_PhotoOrganiseV2__DeletePhoto *PhotoOrganiseV2__DeletePhoto, _PhotoOrganiseV2__DeletePhotoResponse *PhotoOrganiseV2__DeletePhotoResponse) { return soap ? soap_call___PhotoOrganiseV23__DeletePhoto(soap, endpoint, NULL, PhotoOrganiseV2__DeletePhoto, PhotoOrganiseV2__DeletePhotoResponse) : SOAP_EOM; };
};

} // namespace WS_PhotoOrganise_v2

#endif
