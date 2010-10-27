/*!
@file	Utils.h
@brief	Class Utils
*/
#ifndef __FRAMEWORK_UTILS_H__
#define __FRAMEWORK_UTILS_H__

#include "BaseTypes.h"
#if defined _WIN32 && ! defined __SYMBIAN32__
	#ifndef RELEASE_BUILD
		#define FASSERT(x) \
		if (!(x))\
		{\
			UTILS_TRACE("*** FASSERT: files:%s line:%d expression:%s", __FILE__, __LINE__, #x);\
			__debugbreak();\
		}
	#else
		#define FASSERT(x)
	#endif // RELEASE_BUILD
#else
	#ifndef RELEASE_BUILD
		#define FASSERT(x) \
		if (!(x))\
		{\
			UTILS_TRACE("*** FASSERT: files:%s line:%d expression:%s", __FILE__, __LINE__, #x);\
		}
	#else
		#define FASSERT(x)
	#endif // RELEASE_BUILD
#endif

#define SAFE_DELETE(p)			{if(p) delete(p); p = 0;}		 //!< Безопасное удаление
#define SAFE_DELETE_ARRAY(p)	{if(p) delete[](p); p = 0;}		 //!< Безопасное удаление массива
#define SAFE_RELEASE(p)			{if(p) ((p)->Release()); p = 0;} //!< Безопасный вызов Release обьекта

//! Massage priority in log file
enum eDebugMessagePriority
{
	EDMP_NOTICE		=	0x1,
	EDMP_DEBUG		=	0x2,
	EDMP_WARNING	=	0x4,
	EDMP_ERROR		=	0x8,

	NUM_DEBUG_MESSAGE_PRIORITYS = 4
};

//#define LOG_FILE_NAME	"/log.txt"


//! Class for logging and it contains wrappers for system functions
class Utils
{
public:

	//! @brief This structure contains date information
	struct Date
	{
		uint16   year;		//!< 4-digit year
		uint16   month;		//!< Month 1-12(January=1, December=12)
		uint16   day;		//!< Day 1-31
		uint16   hour;		//!< Hour 0-23
		uint16   minute;	//!< Minute 0-59
		uint16   second;	//!< Seconds 0-59
		uint16   weekDay;	//!< Day of the week 0-6 (0=Monday, 6=Sunday)
	};

	//! @brief Pointer align
	//! @param[in] p - Pointer to alignment
	//! @param[in] align - Size of alignment in bytes (must be equal to 1, 2, 4, 8... ets.)
	static uint8 * AlignPointer(uint8 * p, uint8 align);

	//! \brief Function for coping from src to dest, size in bytes
	//! \param[in] dest - destination buffer 
	//! \param[in] src - source buffer
	//! \param[in] size - the number of bytes to be copied
	static void	Memcpy(void * dest, const void * src, int32 size);

	//! \brief The function copies size bytes of characters from src to dest.
	//!	If some regions of the source area and the destination overlap,
	//!	memmove ensures that the original source bytes in the overlapping region
	//!	are copied before being overwritten.
	//! \param[in] dest - destination buffer
	//! \param[in] src - source buffer
	//! \param[in] size - the number of bytes to be copied
	static void	Memmove(void * dest, const void * src, int32 size);

	//! \brief Function for filling buffer
	//! \param[in] dest - source buffer
	//! \param[in] s - value being written
	//! \param[in] size - the number of bytes to be written
	static void	Memset(void * dest, uint8 s, int32 size);

	//! \brief Compare characters in two buffers.
	//! \param[in] buf1 - First buffer
	//! \param[in] buf2 - Second buffer
	//! \param[in] size - Number of characters
	//! \return value indicates the relationship between the buffers.
	//!	(< 0)	-	buf1 less than buf2 
	//!	(0)		-	buf1 identical to buf2 
	//!	(> 0)	-	buf1 greater than buf2 
	static int32 Memcmp(const void * buf1, const void * buf2, int32 size);

	//! @brief Convert ASCII string to int
	//! @param[in] str - source string 
	//! @return value of converted string
	static int32 Atoi(const char8* str);

	//! @brief Convert Unicode string to int
	//! @param[in] str - source string
	//! @return value of converted string
	static int32 Wtoi(const char16* str);

	//! @brief Convert from ASCII to Unicode 
	//! @param[in] str - source string
	//! @param[in] wStr - destination string
	//! @param[in] size -  the number of bytes to be converted
	static void StrToWstr(const char8 * str, char16 * wStr, int32 size);

	//! @brief Convert from Unicode to ASCII
	//! @param[in] wStr - source string
	//! @param[in] str - destination string
	//! @param[in] size -  the number of bytes to be converted
	static void WstrToStr(const char16 * wStr, char8 * str, int32 size);

	//! @brief Add ASCII src string to ASCII dest string
	//! @param[in] dest - string to which the other string is added
	//! @param[in] src - string being added
	//! @return result of addition
	static char8* StrCat(char8 * dest, const char8 * src);

	//! @brief Copy ASCII string from src to dest
	//! @param[in] dest - destination string
	//! @param[in] src - source string 
	static void StrCpy(char8 * dest, const char8 * src);

	//! @brief Copy ASCII string from src to dest
	//! @param[in] dest - destination string 
	//! @param[in] src - source string
	//! @param[in] size -  the number of bytes to be copied
	static void StrNCpy(char8 * dest, const char8 * src, int32 size);

	//! @brief Compare ASCII strings
	//! @param[in] str1, str2 - source strings
	//! @return 0, if equal
	static int32 StrCmp(const char8 * str1, const char8 * str2);

	//! @brief ASCII Compare strings
	//! @param[in] str1, str2 - source strings
	//! @param[in] size -  the number of bytes to be compared
	//! @return 0, if equal
	static int32 StrNCmp(char8 * str1, char8 * str2, uint32 size);

	//! @brief return length of ASCII string
	//! @param[in] str - source string
	//! @return length of string
	static int32 StrLen(const char8 * str);

	//! @brief standard printf function with ASCII parameters
	//! @param[in] dest - destination string
	//! @param[in] str - source string
	//! @return the number of characters written
	static int32 SPrintf(char8 * dest, const char8 * str, ...);

	//! @brief function for uppercase conversion of ASCII string
	//! @param[in] str - source string
	//! @return converted string
	static char8* StrUpper(char8* str);

	//! @brief function for lowercase conversion of ASCII string
	//! @param[in] str - source string
	//! @return converted string
	static char8* StrLower(char8* str);
	
	//! @brief Add Unicode src string to Unicode dest string
	//! @param[in] dest - source string
	//! @param[in] src - string being added
	//! @return result of addition
	static void WStrCat(char16 * dest, const char16 * src);

	//! @brief Copy Unicode string from src to dest
	//! @param[in] dest - destination string 
	//! @param[in] src - source string
	static void WStrCpy(char16 * dest, const char16 * src);

	//! @brief Copy Unicode string from src to dest
	//! @param[in] dest - destination string
	//! @param[in] size -  the number of bytes to be copied
	//! @param[in] src - source string
	//! @param[in] len -  length of destination string
	static void WStrNCpy(char16 * dest, int32 size, const char16 * src, int32 len);

	//! @brief Compare Unicode strings
	//! @param[in] str1, str2 - source strings
	//! @return 0, if equal
	static int32 WStrCmp(const char16 * str1, const char16 * str2);

	//! @brief Compare Unicode strings
	//! @param[in] str1, str2 - source strings
	//! @param[in] size -  the number of bytes to be compared
	//! @return 0, if equal
	static int32 WStrNCmp(const char16 * str1, const char16 * str2, uint32 size);

	//! @brief return length of Unicode string
	//! @param[in] str - source string
	//! @return length of string
	static int32 WStrLen(const char16 * str);

	//! @brief standard printf function with Unicode parameters
	//! @param[in] dest - destination string
	//! @param[in] size - the number of bytes
	//! @param[in] str - source string
	//! @return the number of characters written
	static void WSPrintf(char16 * dest, int32 size, const char16 * str, ...);

	//! @brief function for uppercase conversion of Unicode string
	//! @param[in] str - source string
	//! @return converted string
	static char16* WStrUpper(char16 *str);

	//! @brief function for lowercase conversion of Unicode string
	//! @param[in] str - source string
	//! @return converted string
	static char16* WStrLower(char16 *str);

	//! @brief function for getting uptime in milliseconds
	//! @return current uptime in milliseconds
	static uint32 GetTime();

	//! @brief Function for setting filter
	//! @param[in] filter - 
	static void SetLogFilter(int32 filter);

	//! @brief Function to write to log file
	//! @param[in] prior - priority @ref eDebugMessagePriority
	//! @param[in] str - string of parameters same as printf
#ifdef UTILS_USE_LOG
#define	UTILS_LOG		Utils::Log
	static void Log(eDebugMessagePriority prior, const char8 *str, ...);
#else
#define	UTILS_LOG	
#endif

	//! @brief Function to write to console
	//! @param[in] str - string of parameters same as printf
#ifdef UTILS_USE_TRACE
#define	UTILS_TRACE		Utils::Trace
	static void Trace(const char8 *str, ...);
#else
#define	UTILS_TRACE	
#endif

	//! @brief Output string to screen for debugging
	//! @param[in] str - string to output 
	//! @param[in] x - x coordinate on screen 
	//! @param[in] y - x coordinate on screen 
	//! @param[in] r - red component
	//! @param[in] g - green component
	//! @param[in] b - blue component
	static void DrawText(const char16 *str, int32 x, int32 y, int32 r, int32 g, int32 b);

	//! @brief Function to saving data to storage
	//! @param[in] ID - class ID
	//! @param[in] version - version of application
	//! @param[in] src - source data
	//! @param[in] size - the number of bytes to save
	//! @return true if success
	static bool StorageSetData(uint64 ID, uint16 version, void *src, uint16 size);

	//! @brief Function to getting data from storage
	//! @param[in] ID - class ID
	//! @param[in] version - version
	//! @param[in] dst - destination data
	//! @param[in] size - the number of bytes to get
	//! @return true - операция выполнена успешно, иначе false
	static bool StorageGetData(uint64 ID, uint16 version, void *dst, uint16 size);

	//! @brief Getting current date
	//! @param[out] src - destination data
	static void GetDate(Date &date);

	static void OpenBrowser(char8 *url);

};
#endif // __FRAMEWORK_UTILS_H__

