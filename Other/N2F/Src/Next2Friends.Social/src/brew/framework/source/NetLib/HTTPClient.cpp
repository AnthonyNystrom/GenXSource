#include "AEEStdLib.h"
#include "AEEShell.h"
#include "HTTPClient.h"
#include "NetSystem.h"
#include "FileSystem.h"
#include "Application.h"
#include "Utils.h"

NetSystem* HTTPClient::pNetSystem	=	NULL;
uint8 HTTPClient::buffer[BUFFER_SZ];

HTTPClient::HTTPClient(const char8 *gameAddr, /*const char8 *updateAddr, */NetSystem *pNS)
:	pIWeb			(NULL)  
,	pIWebResponse	(NULL)
,	pISource		(NULL)
,	pISourceUtil	(NULL)
,	pIPeek			(NULL)
,	gameServerAddr	(NULL)
//,	updateServerAddr(NULL)
{
	int32 len = Utils::StrLen(gameAddr);
	gameServerAddr = new char8 [len + 1];
	Utils::StrCpy(gameServerAddr, gameAddr);

	//len = Utils::StrLen(updateAddr);
	//updateServerAddr = new char8 [len + 1];
	//Utils::StrCpy(updateServerAddr, updateAddr);

	pNetSystem	= pNS;

	IShell *pIShell = ((AEEApplet*)GETAPPINSTANCE())->m_pIShell;

	if (ISHELL_CreateInstance(pIShell, AEECLSID_WEB, (void **)&pIWeb) != SUCCESS)
	{ 
		UTILS_LOG(EDMP_ERROR, "HTTPClient::HTTPClient: ISHELL_CreateInstance(AEECLSID_WEB) failed");
	}
	else
	{
		// set up the callback function to receive response from server 
// 		CALLBACK_Init(&structWebCB, IWebCB, this);  // out, in, in 
		
		if (ISHELL_CreateInstance(pIShell,AEECLSID_SOURCEUTIL, (void**)&pISourceUtil) != SUCCESS)
		{
			UTILS_LOG(EDMP_ERROR, "HTTPClient::HTTPClient: ISHELL_CreateInstance(AEECLSID_SOURCEUTIL) failed");
		}
	}
}

HTTPClient::~HTTPClient()
{
	SAFE_DELETE_ARRAY(gameServerAddr);
	//SAFE_DELETE_ARRAY(updateServerAddr);

	// cancel callback  
	CALLBACK_Cancel(&structWebCB); 

	if (pIWebResponse)	IWEBRESP_Release(pIWebResponse); 
	if (pIWeb)			IWEB_Release(pIWeb); 
//	if (pISource)		ISOURCE_Release(pISource);
	if (pISourceUtil)	ISOURCEUTIL_Release(pISourceUtil);
	if (pIPeek)			IPEEK_Release(pIPeek);
}

//bool HTTPClient::Init(const char8 *addr)
//{
//	int32 len = Utils::StrLen(addr);
//	serverAddr = new char8 [len + 1];
//
//	Utils::StrCpy(serverAddr, addr);
//
//	return true;
//}

bool HTTPClient::Request(const uint8 *data, uint32 sz/*, bool isUpdateRequest*/)
{
	//char8 *addr = isUpdateRequest ? updateServerAddr : gameServerAddr;
	char8 *addr = gameServerAddr;

#ifdef USE_HTTP_GET
	int32 reqLen = Utils::StrLen(addr);
	int32 fullLength = reqLen + 1 + sz * 2;

	if(fullLength > GET_REQUEST_SZ)
	{
#endif//USE_HTTP_GET
#ifdef USE_HTTP_POST
		POSTRequest(addr, (uint8 *)data, sz);
		return true;
#endif//USE_HTTP_POST
#ifdef USE_HTTP_GET
	}
	else if (fullLength > REQUEST_STRING_SZ)
	{
		return NULL;
	}

	Utils::StrCpy(reqString, addr);

	char8 *req = reqString + reqLen;

	for(uint32 i = 0; i < sz; ++i)
	{
		uint8 c = *data++;
		*req++ = ((c >> 4) & 0xF) + 'a';
		*req++ = (c & 0xF) + 'a';
	}
	*req = 0;

	GETRequest(reqString);

	return true;
#endif//USE_HTTP_GET
}

void HTTPClient::IWebCB(void *p)
{
	UTILS_LOG(EDMP_NOTICE, "HTTPClient::IWebCB:  START! ");

	HTTPClient *pThis = (HTTPClient*)p;

	// get info about the response 
	pThis->pWebRespInfo = IWEBRESP_GetInfo(pThis->pIWebResponse); 

	// check error code 
	if (!WEB_ERROR_SUCCEEDED(pThis->pWebRespInfo->nCode)) 
	{ 
		pNetSystem->OnError();
		UTILS_LOG(EDMP_ERROR, "Response Error: %0xd , %d", pThis->pWebRespInfo->nCode, pThis->pWebRespInfo->nCode);
		return; 
	} 

	// get pointer to Source object 
	pThis->pISource = pThis->pWebRespInfo->pisMessage; 
	if (pThis->pISource == NULL) 
	{ 
		pNetSystem->OnError();
		UTILS_LOG(EDMP_ERROR, "Response Error: pISource == NULL");		
		return; 
	}  

	// register ISource Read callback
	CALLBACK_Init(&pThis->structWebCB, ReadCB, pThis);  // out, in, in 

	pNetSystem->OnStartRead();
	pThis->allreadyReaded = 0;

	// post a read; data is processed by ISource callback 
	ISOURCE_Readable(pThis->pISource,  &pThis->structWebCB); 

	UTILS_LOG(EDMP_NOTICE, "HTTPClient::IWebCB:  FINISH! ");
} 

void HTTPClient::ReadCB(void *p)
{
	HTTPClient *pThis = (HTTPClient*)p;
	
	// read data from stream; get number of bytes read
	int32 byteCount;
	if (pThis->pWebRespInfo->lContentLength - pThis->allreadyReaded > sizeof(buffer))
	{
		byteCount = ISOURCE_Read(pThis->pISource, (char8*)buffer, sizeof(buffer));
	}
	else
	{
		byteCount = ISOURCE_Read(pThis->pISource, (char8*)buffer, pThis->pWebRespInfo->lContentLength - pThis->allreadyReaded);
	}

	switch (byteCount)
	{ 
		case ISOURCE_WAIT:  // buffer empty, but more data expected 
			// post another read
			UTILS_LOG(EDMP_DEBUG, "HTTPClient::ReadCB: ISOURCE_WAIT");

			ISOURCE_Readable(pThis->pISource, &pThis->structWebCB); 
			return; 

		case ISOURCE_ERROR:  // Error occurred 
			UTILS_LOG(EDMP_ERROR, "HTTPClient::ReadCB: ISOURCE_ERROR");
			pNetSystem->OnError();
			return; 

		case ISOURCE_END:   // Buffer empty; all data received 
			UTILS_LOG(EDMP_DEBUG, "HTTPClient::ReadCB: ISOURCE_END");
			pNetSystem->OnFinishRead();
			return; 

		default:      // data read; copy from chunk buffer 
			UTILS_LOG(EDMP_DEBUG, "HTTPClient::ReadCB: default:  size = %d", byteCount);

			if (byteCount + pThis->allreadyReaded <= pThis->pWebRespInfo->lContentLength || pThis->pWebRespInfo->lContentLength <= 0)
			{
				pNetSystem->OnRead(buffer, byteCount);
				pThis->allreadyReaded += byteCount;
				if (pThis->allreadyReaded >= pThis->pWebRespInfo->lContentLength || (pThis->pWebRespInfo->lContentLength < 0 && byteCount > sizeof(buffer)))
				{
					pNetSystem->OnFinishRead();
				}
				else
				{
					ISOURCE_Readable(pThis->pISource,&pThis->structWebCB); 
				}
			}
			else
			{
				pNetSystem->OnRead(buffer, pThis->pWebRespInfo->lContentLength - pThis->allreadyReaded);
				pNetSystem->OnFinishRead();
			}

			//  post  another  read 
			return; 
	}  
}

void HTTPClient::GETRequest(char8 *url)
{
	UTILS_LOG(EDMP_DEBUG, "HTTPClient::GETRequest: %s", url);

	if (pIWebResponse)
	{ 
		IWEBRESP_Release(pIWebResponse); 
		pIWebResponse  =  NULL; 
	}

	//Андрейка - если два запроса шли один за другим, то второй не вычитвывался, приходило сразу ISOURCE_WAIT, за ним ISOURCE_END 
	CALLBACK_Init(&structWebCB, IWebCB, this);  // out, in, in 
	// сейчас вычитывается и второй

	IWEB_GetResponse(pIWeb,
					(	pIWeb, 
						&pIWebResponse, 
						&structWebCB, 
						url,
						WEBOPT_HEADER, "X-Method: GET\r\n",            
						WEBOPT_END)
					); 
}

void HTTPClient::POSTRequest(char *url, uint8 *data, int32 size)
{
	UTILS_LOG(EDMP_DEBUG, "HTTPClient::POSTRequest: url = %s, datasize = %d", url, size);

	if (pIPeek)
	{
		IPEEK_Release(pIPeek);
		pIPeek	=	NULL;
	}

	ISOURCEUTIL_PeekFromMemory(pISourceUtil, (const void *)data, size, 0,0,&pIPeek);

	if(pIWebResponse) 
	{ 
		IWEBRESP_Release(pIWebResponse); 
		pIWebResponse  =  NULL; 
	} 

	//Андрейка - если два запроса шли один за другим, то второй не вычитвывался, приходило сразу ISOURCE_WAIT, за ним ISOURCE_END 
	CALLBACK_Init(&structWebCB, IWebCB, this);  // out, in, in 
	// сейчас вычитывается и второй

	if(pIWeb)
		IWEB_GetResponse(pIWeb, 
						(	pIWeb,
							&pIWebResponse, 
							&structWebCB, 
							url,
							WEBOPT_HEADER, "X-Method: POST\r\n",     
							WEBOPT_METHOD, "POST",
  							WEBOPT_BODY, pIPeek, 
  							WEBOPT_CONTENTLENGTH, size, 
							WEBOPT_END)
						); 
}
