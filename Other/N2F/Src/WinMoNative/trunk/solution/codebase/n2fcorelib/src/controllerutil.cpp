#include "stdafx.h"

#include "controllerutil.h"

#include <CBase64.h>
#include "datetimeconverter.h"

N2FCORE_API ControllerUtil::ControllerUtil()
{

}

N2FCORE_API ControllerUtil::~ControllerUtil()
{

}



N2FCORE_API bool ControllerUtil::ConvertWideCharToMultiByte( LPCTSTR strUnicode, int nUnicodeCharsCnt, char **pStrMultibyte, int *pnMultibyteBufCharCnt )
{
	bool result = false;

	if ( NULL == strUnicode || 0 == nUnicodeCharsCnt )
		return result;

	if ( NULL == pStrMultibyte )
		return result;

	*pStrMultibyte = NULL;
	if ( pnMultibyteBufCharCnt )
		*pnMultibyteBufCharCnt = 0;

	if ( -1 == nUnicodeCharsCnt )
		nUnicodeCharsCnt = _tcslen(strUnicode);

#ifdef UNICODE

	int resutlCharCnt = nUnicodeCharsCnt + 10;
	char* resultCharBuf = new char[resutlCharCnt];
	ASSERT(NULL != resultCharBuf );
	if ( NULL == resultCharBuf )
		return result;

	ZeroMemory(resultCharBuf, sizeof(char)*resutlCharCnt);

	int charsWritten = ::WideCharToMultiByte(CP_ACP, 0, strUnicode, nUnicodeCharsCnt+1, resultCharBuf, resutlCharCnt, NULL, NULL);
	if ( 0 == charsWritten )
	{
		delete [] resultCharBuf;
		return result;
	}

	*pStrMultibyte = resultCharBuf;

	if ( pnMultibyteBufCharCnt )
		*pnMultibyteBufCharCnt = charsWritten;

#else	//#ifdef UNICODE

	int resultCharCnt = nUnicodeCharsCnt + 1;
	char *resultCharBuf = new char[resultCharCnt];
	ASSERT(NULL != resultCharBuf );
	if ( NULL == resultCharBuf )
		return result;

	ZeroMemory(resultCharBuf, sizeof(char)*resutlCharCnt);

	memcpy(resultCharBuf, strUnicode, resultCharCnt);

	*pStrMultibyte = resultCharBuf;

	if ( pnMultibyteBufCharCnt )
		*pnMultibyteBufCharCnt = resultCharCnt;

#endif	//#else - #ifdef UNICODE

	result = true;

	return result;
}

N2FCORE_API  bool ControllerUtil::EncodeDataWithBase64( const char* buffer, int bufferSize, char** pStrB64 )
{
	bool result = false;

	if ( NULL == pStrB64 )
		return result;

	*pStrB64 = NULL;

	if ( NULL == buffer || 0 == bufferSize )
	{
		return result;
	}

	CBase64 b64;

	char *pB64Output = NULL;
	unsigned int outputBufferSize = b64.CreateMatchingEncodingBuffer(bufferSize, &pB64Output);
	if ( NULL == pB64Output || outputBufferSize == 0 )
		return result;

	char* bufferPtr = const_cast<char*>(buffer);

	b64.EncodeBuffer(bufferPtr, bufferSize, pB64Output);

	unsigned int resultBufferSize = outputBufferSize + 10;
	char *resultBuffer = new char[resultBufferSize];
	if ( NULL == resultBufferSize )
	{
		ASSERT(FALSE);
		free(pB64Output);
		return result;
	}

	ZeroMemory(resultBuffer, sizeof(char)*resultBufferSize);
	memcpy(resultBuffer, pB64Output, outputBufferSize);
	
	*pStrB64 = resultBuffer;
	result = true;

	return result;	
}

N2FCORE_API  bool ControllerUtil::CurrentLocalTimeInTicks( INT64& ticks, CString& strResult )
{
	ticks = 0;

	SYSTEMTIME st = {0};
	::GetLocalTime(&st);

	return ControllerUtil::SystemTimeInTicks( st, ticks, strResult );
}

N2FCORE_API  bool ControllerUtil::SystemTimeInTicks( SYSTEMTIME &st, INT64& ticks, CString& strResult )
{
	ticks = 0;
	strResult.Empty();

	DateTimeConverter dc;
	ticks = dc.GetTicks(st);

	size_t defaultStrBufSize = 100;
	char *strBuf = new char[defaultStrBufSize];
	if ( NULL != strBuf )
	{
		ZeroMemory(strBuf, sizeof(char)*defaultStrBufSize);
		_i64toa_s(ticks, strBuf, defaultStrBufSize-1, 10);

		strResult = CString(strBuf);

		delete [] strBuf;
		strBuf = NULL;
	}

	return true;
}

N2FCORE_API  bool ControllerUtil::ReadBinaryFileToBuffer( CString& fileName, char** pBuffer, size_t *pBufferLength )
{
	bool result = false;
	char *resultBuffer = NULL;

	if ( NULL == pBuffer )
		return result;

	*pBuffer = NULL;

	if ( NULL == pBufferLength )
		return result;

	*pBufferLength = 0;

	FILE *f = _tfopen(fileName, _T("rb"));
	if ( NULL != f )
	{
		fseek(f, 0, SEEK_END);
		long fileSize = ftell(f);
		fseek(f, 0, SEEK_SET);

		resultBuffer = new char[fileSize];
		if ( NULL != resultBuffer )
		{
			ZeroMemory(resultBuffer, sizeof(char)*(fileSize));
			size_t bytesRead = fread(resultBuffer, sizeof(char), fileSize, f);

			if ( bytesRead == fileSize )
			{
				result = true;
				*pBuffer = resultBuffer;
				*pBufferLength = fileSize;
			}
		}

		fclose(f);
	}

	return result;
}

N2FCORE_API  void ControllerUtil::GetModuleFolder( CString& folderPath )
{
	folderPath.Empty();

	TCHAR *strBuffer = new TCHAR[MAX_PATH+1];
	ASSERT( NULL != strBuffer );
	if ( NULL == strBuffer )
		return;

	ZeroMemory( strBuffer, sizeof(TCHAR)*(MAX_PATH+1));

	::GetModuleFileName(NULL, strBuffer, MAX_PATH);

	CString temp(strBuffer);
	int idx = temp.ReverseFind(_T('\\'));
	if ( -1 != idx )
	{
		folderPath = temp.Left( idx + 1 );
	}
	else
	{
		ASSERT(FALSE);
	}

	delete [] strBuffer;
}
