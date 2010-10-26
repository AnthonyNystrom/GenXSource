#pragma once

#include <wmsdk.h>
#include <wmsysprf.h>

#ifndef WMFORMAT_SDK_VERSION
#define WMFORMAT_SDK_VERSION WMT_VER_9_0
#endif

class CwmvFile
{
	IWMProfile			*m_pWMProfile;
	IWMWriter			*m_pWMWriter;
	IWMInputMediaProps	*m_pVideoProps;
	IWMProfileManager	*m_pWMProfileManager;
	HDC					m_hwmvDC;
	TCHAR				m_szErrMsg[MAX_PATH];
	DWORD				m_dwVideoInput;
	DWORD				m_dwCurrentVideoSample;
	QWORD				m_msVideoTime;
	DWORD				m_dwFrameRate;				// Frames Per Second Rate (FPS)

	int					m_nAppendFuncSelector;		//0=Dummy	1=FirstTime	2=Usual

	HRESULT	AppendFrameFirstTime(HBITMAP );
	HRESULT	AppendFrameUsual(HBITMAP);
	HRESULT	AppendDummy(HBITMAP);
	HRESULT	(CwmvFile::*pAppendFrame[3])(HBITMAP hBitmap);

	HRESULT	AppendFrameFirstTime(int, int, LPVOID,int );
	HRESULT	AppendFrameUsual(int, int, LPVOID,int );
	HRESULT	AppendDummy(int, int, LPVOID,int );
	HRESULT	(CwmvFile::*pAppendFrameBits[3])(int, int, LPVOID,int );

	/// Takes care of creating the memory, streams, compression options etc. required for the movie
	HRESULT InitMovieCreation(int nFrameWidth, int nFrameHeight, int nBitsPerPixel);

	/// Takes care of releasing the memory and movie related handles
	void ReleaseMemory();

	/// Sets the Error Message
	void SetErrorMessage(LPCTSTR lpszErrMsg);

public:
	/// <Summary>
	/// Constructor accepts the filename, ProfileGUID and frame rate settings
	/// as parameters.
	/// lpszFileName: Name of the output movie file to create
	/// guidProfileID: GIUD of the Video Profile to be used for compression and other Settings
	/// dwFrameRate: The Frames Per Second (FPS) setting to be used for the movie
	/// </Summary>
	CwmvFile(LPCTSTR lpszFileName = _T("Output.wmv"),
			const GUID& guidProfileID = WMProfile_V80_1400NTSCVideo, //	WMProfile_V80_384Video,
			DWORD dwFrameRate = 1);

	CwmvFile(LPCTSTR lpszFileName,
		IWMProfile			*pWMProfile,
		DWORD dwFrameRate = 1);

	/// <Summary> 
	/// Destructor closes the movie file and flushes all the frames
	/// </Summary>
	~CwmvFile(void);

	/// </Summary>
	/// Inserts the given HBitmap into the movie as a new Frame at the end.
	/// </Summary>
	HRESULT	AppendNewFrame(HBITMAP hBitmap);

	/// </Summary>
	/// Inserts the given bitmap bits into the movie as a new Frame at the end.
	/// The width, height and nBitsPerPixel are the width, height and bits per pixel
	/// of the bitmap pointed to by the input pBits.
	/// </Summary>
	HRESULT	AppendNewFrame(int nWidth, int nHeight, LPVOID pBits,int nBitsPerPixel=32);

	/// <Summary>
	/// Returns the last error message, if any.
	/// </Summary>
	LPCTSTR GetLastErrorMessage() const {	return m_szErrMsg;	}
};
