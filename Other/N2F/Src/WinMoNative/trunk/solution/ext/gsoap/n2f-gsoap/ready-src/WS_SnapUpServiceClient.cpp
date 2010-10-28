/* WS_SnapUpServiceClient.cpp
   Generated by gSOAP 2.7.10 from .\wsdl-h\SnapUpService.h
   Copyright(C) 2000-2008, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/
#include "WS_SnapUpServiceH.h"

namespace WS_SnapUpService {

SOAP_SOURCE_STAMP("@(#) WS_SnapUpServiceClient.cpp ver 2.7.10 2008-05-17 16:12:16 GMT")


SOAP_FMAC5 int SOAP_FMAC6 soap_call___SnapUpService2__DeviceUploadPhoto(struct soap *soap, const char *soap_endpoint, const char *soap_action, _SnapUpService__DeviceUploadPhoto *SnapUpService__DeviceUploadPhoto, _SnapUpService__DeviceUploadPhotoResponse *SnapUpService__DeviceUploadPhotoResponse)
{	struct __SnapUpService2__DeviceUploadPhoto soap_tmp___SnapUpService2__DeviceUploadPhoto;
	if (!soap_endpoint)
		soap_endpoint = "http://next2friends.com:90/SnapUpService.asmx";
	if (!soap_action)
		soap_action = "http://www.next2friends.com/DeviceUploadPhoto";
	soap->encodingStyle = NULL;
	soap_tmp___SnapUpService2__DeviceUploadPhoto.SnapUpService__DeviceUploadPhoto = SnapUpService__DeviceUploadPhoto;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___SnapUpService2__DeviceUploadPhoto(soap, &soap_tmp___SnapUpService2__DeviceUploadPhoto);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___SnapUpService2__DeviceUploadPhoto(soap, &soap_tmp___SnapUpService2__DeviceUploadPhoto, "-SnapUpService2:DeviceUploadPhoto", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___SnapUpService2__DeviceUploadPhoto(soap, &soap_tmp___SnapUpService2__DeviceUploadPhoto, "-SnapUpService2:DeviceUploadPhoto", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!SnapUpService__DeviceUploadPhotoResponse)
		return soap_closesock(soap);
	SnapUpService__DeviceUploadPhotoResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	SnapUpService__DeviceUploadPhotoResponse->soap_get(soap, "SnapUpService:DeviceUploadPhotoResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

SOAP_FMAC5 int SOAP_FMAC6 soap_call___SnapUpService2__JavaUploadPhoto(struct soap *soap, const char *soap_endpoint, const char *soap_action, _SnapUpService__JavaUploadPhoto *SnapUpService__JavaUploadPhoto, _SnapUpService__JavaUploadPhotoResponse *SnapUpService__JavaUploadPhotoResponse)
{	struct __SnapUpService2__JavaUploadPhoto soap_tmp___SnapUpService2__JavaUploadPhoto;
	if (!soap_endpoint)
		soap_endpoint = "http://next2friends.com:90/SnapUpService.asmx";
	if (!soap_action)
		soap_action = "http://www.next2friends.com/JavaUploadPhoto";
	soap->encodingStyle = NULL;
	soap_tmp___SnapUpService2__JavaUploadPhoto.SnapUpService__JavaUploadPhoto = SnapUpService__JavaUploadPhoto;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___SnapUpService2__JavaUploadPhoto(soap, &soap_tmp___SnapUpService2__JavaUploadPhoto);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___SnapUpService2__JavaUploadPhoto(soap, &soap_tmp___SnapUpService2__JavaUploadPhoto, "-SnapUpService2:JavaUploadPhoto", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___SnapUpService2__JavaUploadPhoto(soap, &soap_tmp___SnapUpService2__JavaUploadPhoto, "-SnapUpService2:JavaUploadPhoto", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!SnapUpService__JavaUploadPhotoResponse)
		return soap_closesock(soap);
	SnapUpService__JavaUploadPhotoResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	SnapUpService__JavaUploadPhotoResponse->soap_get(soap, "SnapUpService:JavaUploadPhotoResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

SOAP_FMAC5 int SOAP_FMAC6 soap_call___SnapUpService3__DeviceUploadPhoto(struct soap *soap, const char *soap_endpoint, const char *soap_action, _SnapUpService__DeviceUploadPhoto *SnapUpService__DeviceUploadPhoto, _SnapUpService__DeviceUploadPhotoResponse *SnapUpService__DeviceUploadPhotoResponse)
{	struct __SnapUpService3__DeviceUploadPhoto soap_tmp___SnapUpService3__DeviceUploadPhoto;
	if (!soap_endpoint)
		soap_endpoint = "http://next2friends.com:90/SnapUpService.asmx";
	if (!soap_action)
		soap_action = "http://www.next2friends.com/DeviceUploadPhoto";
	soap->encodingStyle = NULL;
	soap_tmp___SnapUpService3__DeviceUploadPhoto.SnapUpService__DeviceUploadPhoto = SnapUpService__DeviceUploadPhoto;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___SnapUpService3__DeviceUploadPhoto(soap, &soap_tmp___SnapUpService3__DeviceUploadPhoto);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___SnapUpService3__DeviceUploadPhoto(soap, &soap_tmp___SnapUpService3__DeviceUploadPhoto, "-SnapUpService3:DeviceUploadPhoto", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___SnapUpService3__DeviceUploadPhoto(soap, &soap_tmp___SnapUpService3__DeviceUploadPhoto, "-SnapUpService3:DeviceUploadPhoto", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!SnapUpService__DeviceUploadPhotoResponse)
		return soap_closesock(soap);
	SnapUpService__DeviceUploadPhotoResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	SnapUpService__DeviceUploadPhotoResponse->soap_get(soap, "SnapUpService:DeviceUploadPhotoResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

SOAP_FMAC5 int SOAP_FMAC6 soap_call___SnapUpService3__JavaUploadPhoto(struct soap *soap, const char *soap_endpoint, const char *soap_action, _SnapUpService__JavaUploadPhoto *SnapUpService__JavaUploadPhoto, _SnapUpService__JavaUploadPhotoResponse *SnapUpService__JavaUploadPhotoResponse)
{	struct __SnapUpService3__JavaUploadPhoto soap_tmp___SnapUpService3__JavaUploadPhoto;
	if (!soap_endpoint)
		soap_endpoint = "http://next2friends.com:90/SnapUpService.asmx";
	if (!soap_action)
		soap_action = "http://www.next2friends.com/JavaUploadPhoto";
	soap->encodingStyle = NULL;
	soap_tmp___SnapUpService3__JavaUploadPhoto.SnapUpService__JavaUploadPhoto = SnapUpService__JavaUploadPhoto;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___SnapUpService3__JavaUploadPhoto(soap, &soap_tmp___SnapUpService3__JavaUploadPhoto);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___SnapUpService3__JavaUploadPhoto(soap, &soap_tmp___SnapUpService3__JavaUploadPhoto, "-SnapUpService3:JavaUploadPhoto", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___SnapUpService3__JavaUploadPhoto(soap, &soap_tmp___SnapUpService3__JavaUploadPhoto, "-SnapUpService3:JavaUploadPhoto", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!SnapUpService__JavaUploadPhotoResponse)
		return soap_closesock(soap);
	SnapUpService__JavaUploadPhotoResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	SnapUpService__JavaUploadPhotoResponse->soap_get(soap, "SnapUpService:JavaUploadPhotoResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

} // namespace WS_SnapUpService


/* End of WS_SnapUpServiceClient.cpp */