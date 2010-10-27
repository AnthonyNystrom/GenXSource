#include "Connection.h"
#include "HTTPClient.h"
#include "NetSystem.h"
#include "FileSystem.h"
#include "Application.h"
#include "Utils.h"

NetSystem* HTTPClient::pNetSystem	=	NULL;
uint8 HTTPClient::buffer[BUFFER_SZ];

HTTPClient::HTTPClient(const char8 *gameAddr, /*const char8 *updateAddr, */NetSystem *pNS)
:	gameServerAddr	(NULL)
//,	updateServerAddr(NULL)
{
	int32 len = Utils::StrLen(gameAddr);
	gameServerAddr = new char8 [len + 1];
	Utils::StrCpy(gameServerAddr, gameAddr);

	//len = Utils::StrLen(updateAddr);
	//updateServerAddr = new char8 [len + 1];
	//Utils::StrCpy(updateServerAddr, updateAddr);

	pNetSystem	= pNS;

	isConnected = false;
	needToClose = false;

//	IShell *pIShell = ((AEEApplet*)GETAPPINSTANCE())->m_pIShell;
//
//	if (ISHELL_CreateInstance(pIShell, AEECLSID_WEB, (void **)&pIWeb) != SUCCESS)
//	{ 
//		UTILS_LOG(EDMP_ERROR, "HTTPClient::HTTPClient: ISHELL_CreateInstance(AEECLSID_WEB) failed");
//	}
//	else
//	{
//		// set up the callback function to receive response from server 
//// 		CALLBACK_Init(&structWebCB, IWebCB, this);  // out, in, in 
//		
//		if (ISHELL_CreateInstance(pIShell,AEECLSID_SOURCEUTIL, (void**)&pISourceUtil) != SUCCESS)
//		{
//			UTILS_LOG(EDMP_ERROR, "HTTPClient::HTTPClient: ISHELL_CreateInstance(AEECLSID_SOURCEUTIL) failed");
//		}
//	}
}

HTTPClient::~HTTPClient()
{
	SAFE_DELETE_ARRAY(gameServerAddr);
	ClearHandles();
	//SAFE_DELETE(pConnection);
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
	if (!isConnected)
	{
		if (!ConnectToServer())
		{
			return false;
		}
	}
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


bool HTTPClient::GETRequest(char8 *url)
{
	//UTILS_LOG(EDMP_DEBUG, "HTTPClient::GETRequest: %s", url);

	//if (pIWebResponse)
	//{ 
	//	IWEBRESP_Release(pIWebResponse); 
	//	pIWebResponse  =  NULL; 
	//}

	////Андрейка - если два запроса шли один за другим, то второй не вычитвывался, приходило сразу ISOURCE_WAIT, за ним ISOURCE_END 
	//CALLBACK_Init(&structWebCB, IWebCB, this);  // out, in, in 
	//// сейчас вычитывается и второй

	//IWEB_GetResponse(pIWeb,
	//				(	pIWeb, 
	//					&pIWebResponse, 
	//					&structWebCB, 
	//					url,
	//					WEBOPT_HEADER, "X-Method: GET\r\n",            
	//					WEBOPT_END)
	//				); 
	return false;
}

bool HTTPClient::POSTRequest(char *url, uint8 *data, int32 size)
{
	UTILS_LOG(EDMP_DEBUG, "HTTPClient::POSTRequest: url = %s, datasize = %d", url, size);

	//if (pIPeek)
	//{
	//	IPEEK_Release(pIPeek);
	//	pIPeek	=	NULL;
	//}

	//ISOURCEUTIL_PeekFromMemory(pISourceUtil, (const void *)data, size, 0,0,&pIPeek);

	//if(pIWebResponse) 
	//{ 
	//	IWEBRESP_Release(pIWebResponse); 
	//	pIWebResponse  =  NULL; 
	//} 

	////Андрейка - если два запроса шли один за другим, то второй не вычитвывался, приходило сразу ISOURCE_WAIT, за ним ISOURCE_END 
	//CALLBACK_Init(&structWebCB, IWebCB, this);  // out, in, in 
	//// сейчас вычитывается и второй

	//if(pIWeb)
	//	IWEB_GetResponse(pIWeb, 
	//					(	pIWeb,
	//						&pIWebResponse, 
	//						&structWebCB, 
	//						url,
	//						WEBOPT_HEADER, "X-Method: POST\r\n",     
	//						WEBOPT_METHOD, "POST",
 // 							WEBOPT_BODY, pIPeek, 
 // 							WEBOPT_CONTENTLENGTH, size, 
	//						WEBOPT_END)
	//					); 

	//LPVOID		requestBody = NULL;
	//DWORD		requestBodyLen = 0;

	LPCWSTR AcceptTypes[2] = {TEXT("*/*"), NULL}; 
	hRequest = HttpOpenRequest(hSession, TEXT("POST"),	(LPCWSTR)objectNameW
		, HTTP_VERSION		// HTTP Version
		, NULL			// Referrer
		, AcceptTypes	// AcceptTypes
		, INTERNET_FLAG_RELOAD | INTERNET_FLAG_NO_CACHE_WRITE 	// Flags
		, 0);		// Context
	if (!hRequest) 
	{
		UTILS_LOG(EDMP_DEBUG, "HTTPClient::ConnectToServer: Can't perform http request: %d", GetLastError());
		ClearHandles();
		return false;
	}


	int result = 0;
	int reqCnt = 0;
	while (!result)
	{
		int result =  HttpSendRequest( hRequest
			, NULL				// lpszHeaders
			, 0					// dwHeadersLength
			, data		// lpOptional
			, size);	// dwOptionalLength
		if (result)
		{
			isInRequest = true;
			return true;							
		}
		if (reqCnt > 30)
		{
			UTILS_LOG(EDMP_ERROR, "HTTPClient::POSTRequest: Can't send request: %d", GetLastError());
			int err = GetLastError();
			pNetSystem->OnError();
			return false;
		}
		reqCnt++;
	}

	UTILS_LOG(EDMP_DEBUG, "HTTPClient::POSTRequest: Can't send request: %d", GetLastError());
	return false;

}

bool HTTPClient::ConnectToServer()
{
	//if ( m_statusHandler != NULL ) {
	//	(*m_statusHandler)(m_statusHandlerData, WEBS_CONNECT, (void *)NULL);
	//}

	pConnection = new CConnection();

	char16 tempAddr[255];
	Utils::StrToWstr(gameServerAddr, tempAddr, 255*2);
	if (pConnection->IsAvailable((LPCTSTR)tempAddr) == S_OK)
	{
		UTILS_LOG(EDMP_DEBUG, "HTTPClient::ConnectToServer: Connection is available");
	}
	else
	{
		UTILS_LOG(EDMP_ERROR, "HTTPClient::ConnectToServer: Problems with connection manager");
	}

	pConnection->AttemptConnect((LPCTSTR)tempAddr);

	while (pConnection->GetStatus() != CONNMGR_STATUS_CONNECTED)
	{
		Sleep(10);
	}

	hInternet = InternetOpen(TEXT("Internet"), INTERNET_OPEN_TYPE_PRECONFIG, NULL, 0, 0);
	if (!hInternet) 
	{
		UTILS_LOG(EDMP_DEBUG, "HTTPClient::ConnectToServer: Can't open internet: %d", GetLastError());
		return false;
	}
//"http://next2friends.com:100/soap2bin.handler"
	char8 servName[100];
	char8 objectName[100];
	objectName[0] = 0;
	char8 portName[100];
	portName[0] = 0;
	char8 *ptr = gameServerAddr;
	enum eParseUrlState
	{
		EPST_START = 0,
		EPST_SERVER,
		EPST_PORT,
		EPST_OBJECT,
	};
	eParseUrlState state = EPST_START;
	ptr += 7;
	state = EPST_SERVER;
	char8* text = servName;
	int32 index = 0;
	while (*ptr)
	{
		switch(state)
		{
		case EPST_SERVER:
			{
				if (*ptr == ':')
				{
					text[index] = 0;
					state = EPST_PORT;
					text = portName;
					index = 0;
					ptr++;
					continue;
				}
				else if (*ptr == '/')
				{
					text[index] = 0;
					state = EPST_OBJECT;
					text = objectName;
					index = 0;
					ptr++;
					continue;
				}
			}
			break;
		case EPST_PORT:
			{
				if (*ptr == '/')
				{
					text[index] = 0;
					state = EPST_OBJECT;
					text = objectName;
					index = 0;
					ptr++;
					continue;
				}
			}
			break;
		}
		text[index++] = *ptr;
		ptr++;
	}
	text[index] = 0;
	
	char16 servNameW[100];
	Utils::StrToWstr(servName, servNameW, 100);


	INTERNET_PORT port = (INTERNET_PORT)Utils::Atoi(portName);
	UTILS_LOG(EDMP_DEBUG, "Server name = %S,  port  = %d", servNameW, port);
	hSession = InternetConnect(hInternet
		, (LPCWSTR)servNameW
		, port
		, NULL
		, NULL
		, INTERNET_SERVICE_HTTP
		, 0		// Flags
		, 0);	// Context

	if (!hSession) 
	{			
		UTILS_LOG(EDMP_DEBUG, "HTTPClient::ConnectToServer: Can't connect to internet: %d", GetLastError());
		ClearHandles();
		return false;
	}

	Utils::StrToWstr(objectName, objectNameW, 100);
	UTILS_LOG(EDMP_DEBUG, "Object name = %S", objectNameW);


	isConnected = true;


	HANDLE readThread = CreateThread(
		NULL, /*security*/
		0, /*stack ignored*/
		HTTPClient::Update, /*thread function*/
		(LPVOID) this, /*thread data*/
		0, /* create thread in running state*/
		NULL /*don't need the thread ID*/
		);
	CloseHandle(readThread);

	return true;
}

void HTTPClient::ClearHandles()
{
	BOOL result = TRUE;
	DWORD lastErr = 0;
	if (isConnected)
	{
		needToClose = true;
		while (needToClose)
		{
			Sleep(15);
		}
	}

	if (hRequest) 
	{
		result = InternetCloseHandle(hRequest);
		if ( ! result ) 
		{
			UTILS_LOG(EDMP_DEBUG, "HTTPClient::ClearHandles: Can't close hRequest: %d", GetLastError());
			return;
		}
	}

	if (hSession) 
	{
		result = InternetCloseHandle(hSession);
		if ( ! result ) 
		{
			UTILS_LOG(EDMP_DEBUG, "HTTPClient::ClearHandles: Can't close hSession: %d", GetLastError());
			return;
		}
	}

	if (hInternet) 
	{
		result = InternetCloseHandle(hInternet);
		if ( ! result ) 
		{
			UTILS_LOG(EDMP_DEBUG, "HTTPClient::ClearHandles: Can't close hInternet: %d", GetLastError());
			return;
		}
	}

	UTILS_LOG(EDMP_DEBUG, "HTTPClient::ClearHandles: DONE");
}

DWORD WINAPI HTTPClient::Update(LPVOID who)
{
	HTTPClient *pThis = (HTTPClient*)who;
	while (!pThis->needToClose)
	{
		if (!pThis->isInRequest)
		{
			Sleep(100);
			continue;
		}
		Sleep(1000);
		//DWORD dwStatusCode = 200;
		//DWORD lenStatusCode = sizeof(DWORD);
		//if (HttpQueryInfo(hRequest, HTTP_QUERY_STATUS_CODE | HTTP_QUERY_FLAG_NUMBER
		//	, &dwStatusCode, &lenStatusCode, NULL))
		//{
		//	if (!(((dwStatusCode) >= 200) && ((dwStatusCode) < 300)))
		//	{
		//		UTILS_LOG(EDMP_DEBUG, "HTTPClient::Update: QueryInfo: %d", dwStatusCode);
		//		return;
		//	}
		//}
		//else
		//{
		//	return;
		//}

		DWORD dwSize = 0;
		BOOL res = InternetQueryDataAvailable(pThis->hRequest, &dwSize, 0, 0);
		if (!dwSize)
		{
			continue;
		}
		if (!res)
		{
			UTILS_LOG(EDMP_ERROR, "HTTPClient::Update: InternetQueryDataAvailable: %d", GetLastError());
			return 0;
		}
		if (pThis->needToClose)
		{
			pThis->needToClose = false;
			return 0;
		}

		pThis->pNetSystem->OnStartRead();

		INTERNET_BUFFERS ib = { sizeof(INTERNET_BUFFERS) };

		ib.lpvBuffer = buffer;

		ib.dwBufferLength = sizeof(buffer);


		// Get then wait for the response

		while (true)
		{
			int32 readedBytes = 0;
			if(InternetReadFileExA(pThis->hRequest, (LPINTERNET_BUFFERSA)&ib, IRF_SYNC, 0) == FALSE)
			{
				UTILS_LOG(EDMP_ERROR, "HTTPClient::Update: InternetReadFileExA: %d", GetLastError());
				return 0;
			}
			//if(InternetReadFile(pThis->hRequest, pThis->buffer, 1024, (LPDWORD)&readedBytes) == FALSE)
			//{
			//	UTILS_LOG(EDMP_DEBUG, "HTTPClient::Update: InternetReadFile failed");
			//	return 0;
			//}
			//if (!readedBytes)
			//{
			//	break;
			//}
			if (!ib.dwBufferLength)
			{
				UTILS_LOG(EDMP_NOTICE, "HTTPClient::Update: !ib.dwBufferLength");
				break;
			}
			if (pThis->needToClose)
			{
				pThis->needToClose = false;
				UTILS_LOG(EDMP_NOTICE, "HTTPClient::Update: pThis->needToClose = false;");
				return 0;
			}
			

			UTILS_LOG(EDMP_NOTICE, "HTTPClient::Update: read len = %d", ib.dwBufferLength);
			pThis->pNetSystem->OnRead((uint8*)buffer, ib.dwBufferLength);
			//pThis->pNetSystem->OnRead(pThis->buffer, readedBytes);
		}
		if (pThis->hRequest) 
		{
			InternetCloseHandle(pThis->hRequest);
			pThis->hRequest = 0;
		}
		pThis->isInRequest = false;
		pThis->pNetSystem->OnFinishRead();
	}
	pThis->needToClose = false;
	return 0;

}