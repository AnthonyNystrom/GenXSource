#pragma once

//! ControlerUtil class

/*!
	ControlerUtil class provides helfull functionality
*/
class ControllerUtil
{

public:
	//! ControllerUtil constructor
	N2FCORE_API ControllerUtil();

	//! ControllerUtil destructor
	N2FCORE_API virtual ~ControllerUtil();

	//! Convert desired wide-char string to multi-byte
	/*! 
		Convert wide-char string to multi-byte one
		\param strUnicode source wide-char string
		\param nUnicodeCharsCnt length of source string in wide-chars, if -1 - wcslen is counted
		\param pStrMultibyte [out] pointer to pointer, where to store address of created buffer with multi-byte string
		\param pnMultibyteBufCharCnt [out] pointer to int, where to store number of chars in created multi-byte buffer, this buffer is allocated with new [] by method, 
		and should be released by client code wiht delete []
		\return bool true - if succeeded, false - if failed
	*/
	N2FCORE_API static bool ConvertWideCharToMultiByte(LPCTSTR strUnicode, int nUnicodeCharsCnt, char **pStrMultibyte, int *pnMultibyteBufCharCnt);

	//! Convert binary buffer with Base64 encoding

	/*!
		Convert binary buffer with Base64 encoding
		\param buffer pointer to binary buffer
		\param bufferSize buffer size in bytes
		\param pStrB64 [out] pointer to resulting multi-byte string with encoded data, this string buffer is allocated by method with new[] operator, 
		and should be released by client code with delete [] operator
		\return bool true - if succeeded, false - if failed
	*/
	N2FCORE_API static bool EncodeDataWithBase64(const char* buffer, int bufferSize, char** pStrB64);

	//! Convert binary buffer with Base64 encoding

	///*!
	//	Convert binary buffer with Base64 encoding
	//	\param buffer pointer to binary buffer
	//	\param bufferSize buffer size in bytes
	//	\param pStrB64 [out] pointer to resulting multi-byte string with encoded data, this string buffer is allocated by method with new[] operator, 
	//	and should be released by client code with delete [] operator
	//	\return bool true - if succeeded, false - if failed
	//*/
	//N2FCORE_API static bool DecodeDataWithBase64(const char* buffer, int bufferSize, char** pStrB64);

	N2FCORE_API static bool CurrentLocalTimeInTicks(INT64& ticks, CString& strResult);

	N2FCORE_API static bool SystemTimeInTicks(SYSTEMTIME &st, INT64& ticks, CString& strResult);

	N2FCORE_API static bool ReadBinaryFileToBuffer(CString& fileName, char** pBuffer, size_t *pBufferLength);

	N2FCORE_API static void GetModuleFolder(CString& folderPath);
};

