// devtestppc.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <commctrl.h>


#include <controllerwebservices.h>
#include <controllerutil.h>

#include <webservice-n2f-memberservice.h>
#include <webservice-n2f-memberservice-v2.h>
#include <webservice-n2f-photoorganise.h>
#include <webservice-n2f-photoorganise-v2.h>

#include <xmlparserwrapper.h>
#include <xmlparsersimpleconfig.h>
#include <configuration-data.h>


void TestTicksCount();
void TestLogger();
void TestReadFileToBuffer();
void TestN2FGetMemberId(CString& resultMID);
void TestN2FDeviceUploadPhoto(CString& memberID);
void TestN2FGetMemberId_v2(CString& memberID);
void TestN2FDeviceUploadPhoto_v2(CString& memberID);
void TestXmlParserWrapper();
void TestSScanf();

ControllerWebServices ws;

int _tmain(int argc, _TCHAR* argv[])
{

	

	//TWSCutsomEPList list;
	//ws.GenerateCustomEndPointsList(list);

	//ws.InitializeController(list);
	//
	//TestTicksCount();

	//TestLogger();

	//
	//TestReadFileToBuffer();

	//CString memberID1;
	//TestN2FGetMemberId(memberID1);
	//TestN2FDeviceUploadPhoto(memberID1);

	//CString memberID2;
	//TestN2FGetMemberId_v2(memberID2);
	//TestN2FDeviceUploadPhoto_v2(memberID2);

	//TestXmlParserWrapper();

	TestSScanf();

	UNINITIALIZE_LOGGER;

	return 0;
}

void TestLogger()
{
	LOGMSG("\n");

	LOGMSG("Testing logger, saying %s", _T("Hello world! from logger"));
}

void TestN2FGetMemberId(CString& resultMID)
{

	LOGMSG("\n");

	

	WebServiceN2FMemberService *wsMS = (WebServiceN2FMemberService*)(ws.GetWebService(EWS_N2F_MemberServices));

	CString nick("asidden");
	CString pass(_T("next2friends"));
	CString memberId;

	//bool res = ws.GetMemberId(nick, pass, memberId);
	//int status = ws.GetLastSoapResult();

	bool res = wsMS->GetMemberId(nick, pass, memberId);
	int status = wsMS->GetLastSoapResult();

	LOGMSG("ws.GetMemberId returned %d, result: %s", res?1:0, memberId);
	LOGMSG("ws.GetLastSoapResult() returned %d", status);

	resultMID = memberId;
}

void TestN2FDeviceUploadPhoto( CString& memberID )
{
	LOGMSG("\n");

	WebServiceN2FPhotoOrganise *wsPO = (WebServiceN2FPhotoOrganise*)(ws.GetWebService(EWS_N2F_PhotoOrganise));

	ASSERT(wsPO != NULL);

	CString pass("next2friends");
	CString photoFile("\\waterfall.jpg");
	SYSTEMTIME st = {0};

	::GetLocalTime(&st);

	bool res = wsPO->DeviceUploadPhoto(memberID, pass, photoFile, st);
	int status = wsPO->GetLastSoapResult();

	LOGMSG("ws.DeviceUploadPhoto returned %d", res?1:0);
	LOGMSG("ws.GetLastSoapResult() returned %d", status);
}


void TestN2FGetMemberId_v2(CString& memberID)
{
	LOGMSG("\n");

	WebServiceN2FMemberService_v2 *wsMS = (WebServiceN2FMemberService_v2*)(ws.GetWebService(EWS_N2F_MemberServices_v2));

	CString nick("asidden");
	CString pass(_T("next2friends"));
	CString memberId;

	//bool res = ws.GetMemberId(nick, pass, memberId);
	//int status = ws.GetLastSoapResult();

	bool res = wsMS->GetMemberId(nick, pass, memberId);
	int status = wsMS->GetLastSoapResult();

	LOGMSG("ws.GetMemberId returned %d, result: %s", res?1:0, memberId);
	LOGMSG("ws.GetLastSoapResult() returned %d", status);

	memberID = memberId;
}

void TestN2FDeviceUploadPhoto_v2( CString& memberID )
{
	LOGMSG("\n");

	WebServiceN2FPhotoOrganise_v2 *wsPO = (WebServiceN2FPhotoOrganise_v2*)(ws.GetWebService(EWS_N2F_PhotoOrganise_v2));

	ASSERT(wsPO != NULL);

	CString pass("next2friends");
	CString photoFile("\\waterfall.jpg");
	SYSTEMTIME st = {0};

	::GetLocalTime(&st);

	bool res = wsPO->DeviceUploadPhoto(memberID, pass, photoFile, st);
	int status = wsPO->GetLastSoapResult();

	LOGMSG("ws.DeviceUploadPhoto returned %d", res?1:0);
	LOGMSG("ws.GetLastSoapResult() returned %d", status);
}

void TestTicksCount()
{
	char *ptr = new char[100];
	ZeroMemory(ptr, sizeof(char)*100);

	SYSTEMTIME st = {0};

	st.wYear = 2008;
	st.wMonth = 2;
	st.wDay = 21;
	st.wHour = 19;
	st.wMinute = 44;
	st.wSecond = 16;

	INT64 res = 0;
	CString str;
	ControllerUtil::SystemTimeInTicks(st, res, str);

	_ui64toa_s(res, ptr, 99, 10);

	LOGMSG("Ticks count returned: %s", CString(ptr));

	delete [] ptr;
}

void TestReadFileToBuffer()
{
	CString fileName("\\waterfall.jpg");

	char *buf = NULL;
	size_t size = 0;

	ControllerUtil::ReadBinaryFileToBuffer(fileName, &buf, &size);
	if ( NULL != buf )
	{
		LOGMSG("Created buffer size: %d", size);

		FILE *f = fopen("\\1.jpg", "wb");
		if ( NULL != f )
		{
			size_t bytesWritten = fwrite(buf, sizeof(char), size, f);
			LOGMSG("Bytes written: %d", bytesWritten);
			fclose(f);
		}
	}
}

void TestXmlParserWrapper()
{
	XmlParserWrapper *xmlParser = new XmlParserWrapper;
	ASSERT(NULL != xmlParser);

	xmlParser->Initialize();

	XmlParserSimpleConfig *graphicsParser = NULL;
	graphicsParser = XmlParserSimpleConfig::CreateGraphicsConfigParser();
	ASSERT( NULL != graphicsParser );

	CString filePath("\\n2f-graphics.xml");

	if ( xmlParser->ParseXmlFile(filePath, graphicsParser) )
	{
		SimpleConfigItemsList list(graphicsParser->GetResultsList());

		LOGMSG("Results received: %d", list.GetSize());

		for ( int i = 0; i < list.GetSize(); ++i )
		{
			LOGMSG("#%d: name: %s, type: %d, id: %d, value: %s",
				i, list[i]->name, list[i]->type, list[i]->id, 
				list[i]->value);
		}

	}
	else
	{
		LOGMSG("Parsing failed");
	}

}

void TestSScanf()
{
	char buf[] = "[ff][cc][ee]";
	TCHAR buf2[] = _T("[ff][cc][ee]");

	CString buf3(L"[00][ff][00]");

	BYTE a = 0, b = 0, c = 0;

	sscanf(buf, "[%x][%x][%x]", &a, &b, &c);
	LOGMSG("Result: %x %x %x", a, b, c);

	a = b = c = 0;

	_stscanf(buf2, _T("[%x][%x][%x]"), &a, &b, &c);
	LOGMSG("Result: %x %x %x", a, b, c);

	a = b = c = 0;

	_stscanf(buf3, _T("[%x][%x][%x]"), &a, &b, &c);
	LOGMSG("Result: %x %x %x", a, b, c);
}

