#include "stdafx.h"

#include <n2fcore.h>

#include <logger.h>

#include <stdio.h>

#define		LOG_FILE_NAME_BASE	_T("\\temp\\n2flog")

BaseLogger* BaseLogger::iLoggerInstance = NULL;

N2FCORE_API	BaseLogger* BaseLogger::CreateLogger()
{
	if ( iLoggerInstance )
		BaseLogger::FreeLogger();

	iLoggerInstance = new BaseLogger();

	return iLoggerInstance;
}

N2FCORE_API	BaseLogger* BaseLogger::GetLogger()
{
	if ( NULL == iLoggerInstance )
		BaseLogger::CreateLogger();

	return iLoggerInstance;
}

N2FCORE_API	void BaseLogger::FreeLogger()
{
	if ( iLoggerInstance )
	{
		delete iLoggerInstance;
		iLoggerInstance = NULL;
	}
}

N2FCORE_API	BaseLogger::BaseLogger()
{

}

N2FCORE_API	BaseLogger::~BaseLogger()
{

}

N2FCORE_API	void BaseLogger::Output(LPCSTR time, LPCSTR file, ULONG lineno, 
								LPCSTR function, LPCSTR format, ... )
{
	// we should be here ever
	ASSERT(FALSE);
}

N2FCORE_API  void BaseLogger::TraceOutput( LPCTSTR str )
{
	ASSERT( str );


	if ( str )
		OutputDebugString(str);

}


/**************************
***		FileLogger class implementation
***************************/

N2FCORE_API	FileLogger::FileLogger()
{
	iFileName.Format(_T("%s-%s.txt"), LOG_FILE_NAME_BASE, CString(__TIMESTAMP__));
	iFileName.Replace(_T(' '), _T('_'));
	iFileName.Replace(_T(':'), _T('-'));

	FILE *f = _tfopen(iFileName, _T("wt"));
	if ( NULL != f )
	{
		_ftprintf(f, _T(" N2F FileLogger @ %s\n\n"), CString(__TIMESTAMP__));
		fclose(f);
	}
	
	

	const size_t szDescr = 512;
	const size_t szBody = 2048;

	iMsgDescriptionBuf = new TCHAR[szDescr];
	if ( iMsgDescriptionBuf )
	{
		iMsgDescriptionBufSize = sizeof(TCHAR)*szDescr;
		ZeroMemory(iMsgDescriptionBuf, iMsgDescriptionBufSize);
	}

	iMsgBodyBuf = new TCHAR[szBody];
	if ( iMsgBodyBuf )
	{
		iMsgBodyBufSize = sizeof(TCHAR)*szBody;
		ZeroMemory(iMsgBodyBuf, iMsgBodyBufSize);
	}
}

N2FCORE_API	FileLogger::~FileLogger()
{
	if ( iMsgBodyBuf )
	{
		delete [] iMsgBodyBuf;
		iMsgBodyBuf = NULL;
	}

	if ( iMsgDescriptionBuf )
	{
		delete [] iMsgDescriptionBuf;
		iMsgDescriptionBuf = NULL;
	}
}

N2FCORE_API	void FileLogger::Output( LPCSTR time, LPCSTR file, ULONG lineno, LPCSTR function, LPCSTR format, ... )
{
	CString strResult;

	FILE *f = _tfopen(iFileName, _T("at"));
	if ( f )
	{
		ASSERT( NULL != iMsgBodyBuf);
		ASSERT( NULL != iMsgDescriptionBuf);

		ZeroMemory(iMsgDescriptionBuf, iMsgDescriptionBufSize);
		ZeroMemory(iMsgBodyBuf, iMsgBodyBufSize);

		_stprintf(iMsgDescriptionBuf, _T("[%s][%s:%d][%s]:: "), CString(time), CString(file), lineno, CString(function));

		//_ftprintf(f, _T("[%s][%s:%d][%s]:: "), CString(time), CString(file), lineno, CString(function));

		va_list arg_list;
		va_start( arg_list, format );
		//_vftprintf(f, format, arg_list);
		_vstprintf(iMsgBodyBuf, CString(format), arg_list);
		va_end( arg_list );

		CString strDescr(iMsgDescriptionBuf), strBody(iMsgBodyBuf);
		strResult += strDescr;
		strResult += strBody;
		strResult += _T("\n");

		_ftprintf(f, _T("%s"), strResult);

		fclose(f);
	}

#if USE_RUNTIME_TRACING_IN_PARALLEL
	if ( strResult.GetLength() > 0 )
		this->TraceOutput( strResult );
#endif	//#if USE_RUNTIME_TRACING_IN_PARALLEL
}

N2FCORE_API	 BaseLogger* FileLogger::CreateLogger()
{
	if ( iLoggerInstance )
		FileLogger::FreeLogger();

	iLoggerInstance = new FileLogger();

	return iLoggerInstance;
}

N2FCORE_API	 BaseLogger* FileLogger::GetLogger()
{
	if ( NULL == iLoggerInstance )
		FileLogger::CreateLogger();

	return iLoggerInstance;
}
