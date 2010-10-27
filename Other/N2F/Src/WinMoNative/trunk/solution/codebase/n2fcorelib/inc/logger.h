#pragma once

#if USE_FILE_LOGGER
#	define	LOGMSG(format, ...)	FileLogger::GetLogger()->Output(__TIME__, __FILE__, __LINE__, __FUNCTION__, format, __VA_ARGS__)
#	define	UNINITIALIZE_LOGGER	FileLogger::FreeLogger()
#	define	LOGME()	LOGMSG("this is me");
#else
#	define	LOGMSG(format, ...)
#	define	LOGME()
#endif	// #else - #if defined USE_FILE_LOGGER

class BaseLogger
{
public:

	N2FCORE_API	static BaseLogger* CreateLogger();
	N2FCORE_API	static BaseLogger* GetLogger();
	N2FCORE_API	static void FreeLogger();

	N2FCORE_API	virtual void Output(LPCSTR time, LPCSTR file, ULONG lineno, 
		LPCSTR function, LPCSTR format, ... );

	N2FCORE_API virtual void TraceOutput(LPCTSTR str);

protected:

	N2FCORE_API	BaseLogger();
	N2FCORE_API	virtual ~BaseLogger();

	static BaseLogger* iLoggerInstance;
};

class FileLogger: public BaseLogger
{
public:

	N2FCORE_API	static BaseLogger* CreateLogger();
	N2FCORE_API	static BaseLogger* GetLogger();

	N2FCORE_API	virtual void Output(LPCSTR time, LPCSTR file, ULONG lineno, 
		LPCSTR function, LPCSTR format, ... );

protected:

	N2FCORE_API	FileLogger();
	N2FCORE_API	virtual ~FileLogger();

	CString iFileName;

	TCHAR	*iMsgDescriptionBuf;
	size_t	iMsgDescriptionBufSize;
	TCHAR	*iMsgBodyBuf;
	size_t	iMsgBodyBufSize;

	friend BaseLogger;
};
