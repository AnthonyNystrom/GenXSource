//-----------------------------------------------------------------------------
// File: DShowTextures.h
//
// Desc: DirectShow sample code - adds support for DirectShow videos playing 
//       on a DirectX 8.0 texture surface. Turns the D3D texture tutorial into 
//       a recreation of the VideoTex sample from previous versions of DirectX.
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// Modified by Toshiyuki Takahei
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include <windows.h>
#include <mmsystem.h>
#include <atlbase.h>
#include <stdio.h>
#include <strsafe.h>

#include <streams.h>

// Callback function type
typedef void (*TexRenderCallback)(void* obj, BYTE* pPixelBuffer, DWORD data);

//-----------------------------------------------------------------------------
// Define GUID for OpenGL Texture Renderer
// {9E5F6B72-23B3-4eeb-8920-447951720900}
//-----------------------------------------------------------------------------
struct __declspec(uuid("{9E5F6B72-23B3-4eeb-8920-447951720900}")) CLSID_GLTextureRenderer;

//-----------------------------------------------------------------------------
// CTextureRenderer Class Declarations
//-----------------------------------------------------------------------------
class CTextureRenderer : public CBaseVideoRenderer
{
public:
    CTextureRenderer(LPUNKNOWN pUnk,HRESULT *phr);
    ~CTextureRenderer();

public:
    HRESULT CheckMediaType(const CMediaType *pmt );     // Format acceptable?
    HRESULT SetMediaType(const CMediaType *pmt );       // Video format notification
    HRESULT DoRenderSample(IMediaSample *pMediaSample); // New video sample
    void    RegisterCallback(TexRenderCallback func, void* obj, DWORD data) // Register a callback function
    { 
        m_pCBFunc = func;
        m_pCBObj = obj;
        m_nCBData = data;
    }

    LONG m_lVidWidth;   // Video width
    LONG m_lVidHeight;  // Video Height

    TexRenderCallback m_pCBFunc; // Texture buffer copy callback function
    void* m_pCBObj;              // 'this' pointer of the OpenGL renderer
    DWORD m_nCBData;             // Which eye?
};

//-----------------------------------------------------------------------------
// CTextureRendererGraphManager Class Declarations
//-----------------------------------------------------------------------------
class CTextureRendererGraphManager
{
public:
    CTextureRendererGraphManager();
    virtual ~CTextureRendererGraphManager();

    void Clean();  // Clear all the internal data.

    HRESULT LoadFile(BSTR& filename, BOOL withAudio=TRUE);  // Load a movie file.
    void RegisterCallback(TexRenderCallback func, void* obj, DWORD data) // Register a callback function.
    {
        m_pCBFunc = func;
        m_pCBObj = obj;
        m_nCBData = data;
        if (m_pRenderer)
            m_pCTR->RegisterCallback(func, obj, data);
    }

    BOOL CheckMovieStatus();  // If the movie has ended, rewind to beginning.

    // Movie information
    HRESULT GetWidth(LONG* width);          // Get Width of the movie.
    HRESULT GetHeight(LONG* height);        // Get Height of the movie.
    HRESULT GetDuration(double* duration);  // Get Duration of the movie in sec.

    // Control the playback
    HRESULT Run();                          // Play the movie.
    HRESULT Pause();                        // Pause the movie (and keep the current image).
    HRESULT Stop(BOOL forcedStop = FALSE);  // Stop the movie.
    HRESULT IsPlaying(BOOL* playing);       // Return TRUE if playing.
    HRESULT SetLoop(BOOL loop);             // Set the loop mode.
    HRESULT GetLoop(BOOL* loop);            // Get the loop mode.

    HRESULT SetPosition(double pos);        // Set the current playback position in sec.
    HRESULT GetPosition(double* pos);       // Get the current playback position in sec.
    HRESULT SetRate(double rate);           // Set the playback speed (1.0 is the original speed).
    HRESULT GetRate(double *rate);          // Get the playback speed (1.0 is the original speed).

    HRESULT SetVolume(LONG volume);         // Set the sound volume (0L is max, -10000L is min).
    HRESULT GetVolume(LONG* volume);        // Get the sound volume (0L is max, -10000L is min).
    HRESULT VolumeSilence();                // Mute the sound volume.
    HRESULT VolumeFull();                   // Maximize the sound volume.

    HRESULT GetFramesDropped(int* frames);  // Get the number of the dropped frames (not worked?).

protected:
    CComPtr<IGraphBuilder>  m_pGB;       // GraphBuilder interface
    CComPtr<IMediaControl>  m_pMC;       // Media Control interface
    CComPtr<IMediaPosition> m_pMP;       // Media Position interface
    CComPtr<IMediaSeeking>  m_pMS;       // Media Seeking interface
    CComPtr<IMediaEvent>    m_pME;       // Media Event interface
    CComPtr<IBasicAudio>    m_pBA;       // Basic Audio interface
    CComPtr<IBaseFilter>    m_pRenderer; // Our custom renderer interface
	CTextureRenderer*       m_pCTR;      // Our custom renderer
    TexRenderCallback       m_pCBFunc;   // Texture buffer transfer callback function
    void*                   m_pCBObj;    // 'this' pointer of the OpenGL renderer
    DWORD                   m_nCBData;   // Which eye?
    BOOL                    m_bPlaying;  // Playing or not
    BOOL                    m_bLoop;     // Loop mode
};
