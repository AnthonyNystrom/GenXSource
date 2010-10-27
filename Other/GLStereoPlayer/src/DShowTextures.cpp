//-----------------------------------------------------------------------------
// File: DShowTextures.cpp
//
// Desc: Based on DirectShow sample code - adds support for DirectShow videos playing 
//       on a DirectX 9.0 texture surface. Turns the D3D texture tutorial into 
//       a recreation of the VideoTex sample from previous versions of DirectX.
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// Modified by Toshiyuki Takahei
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"

#include "DShowTextures.h"

#include "strsafe.h"

// Define this if you want to render only the video component with no audio
//
//   #define NO_AUDIO_RENDERER

// An application can advertise the existence of its filter graph
// by registering the graph with a global Running Object Table (ROT).
// The GraphEdit application can detect and remotely view the running
// filter graph, allowing you to 'spy' on the graph with GraphEdit.
//
// To enable registration in this sample, define REGISTER_FILTERGRAPH.
//
#define REGISTER_FILTERGRAPH

void Msg(TCHAR *szFormat, ...);

HRESULT AddToROT(IUnknown *pUnkGraph); 
void RemoveFromROT(void);

//-----------------------------------------------------------------------------
// CTextureRendererGraphManager constructor
//-----------------------------------------------------------------------------
CTextureRendererGraphManager::CTextureRendererGraphManager()
{
    m_pGB = NULL;
    m_pMC = NULL;
    m_pMP = NULL;
    m_pMS = NULL;
    m_pME = NULL;
    m_pBA = NULL;
    m_pRenderer = NULL;
    m_pCBFunc = NULL;
    m_nCBData = 0;
    m_bPlaying = FALSE;
    m_bLoop = TRUE;
}

//-----------------------------------------------------------------------------
// CTextureRendererGraphManager destructor
//-----------------------------------------------------------------------------
CTextureRendererGraphManager::~CTextureRendererGraphManager()
{
    Clean();
}

//-----------------------------------------------------------------------------
// Clean: Clear all the internal data.
//-----------------------------------------------------------------------------
void CTextureRendererGraphManager::Clean()
{
#ifdef REGISTER_FILTERGRAPH
    // Pull graph from Running Object Table (Debug)
    RemoveFromROT();
#endif

    // Shut down the graph
    if (!(!m_pMC)) m_pMC->Stop();

    if (!(!m_pMC)) m_pMC.Release();
    if (!(!m_pME)) m_pME.Release();
    if (!(!m_pMP)) m_pMP.Release();
    if (!(!m_pMS)) m_pMS.Release();
    if (!(!m_pBA)) m_pBA.Release();
    if (!(!m_pGB)) m_pGB.Release();
    if (!(!m_pRenderer)) m_pRenderer.Release();

    m_pGB = NULL;
    m_pMC = NULL;
    m_pMP = NULL;
    m_pMS = NULL;
    m_pME = NULL;
    m_pRenderer = NULL;

    m_pCBFunc = NULL;

    m_bPlaying = FALSE;
    m_bLoop = FALSE;
}

//-----------------------------------------------------------------------------
// LoadFile: Load a movie file.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::LoadFile(BSTR& filename, BOOL withAudio)
{
    HRESULT hr;
    CComPtr<IBaseFilter>    pFSrc;          // Source Filter
    CComPtr<IPin>           pFSrcPinOut;    // Source Filter Output Pin   

    Clean();

    // Create the filter graph
    if (FAILED(m_pGB.CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC)))
        return E_FAIL;

#ifdef REGISTER_FILTERGRAPH
    // Register the graph in the Running Object Table (for debug purposes)
    AddToROT(m_pGB);
#endif
    
    // Create the Texture Renderer object
    m_pCTR = new CTextureRenderer(NULL, &hr);
    if (FAILED(hr) || !m_pCTR)
    {
        delete m_pCTR;
        Msg(TEXT("Could not create texture renderer object!  hr=0x%x"), hr);
        Clean();
        return E_FAIL;
    }
    
    // Get a pointer to the IBaseFilter on the TextureRenderer, add it to graph
    m_pRenderer = m_pCTR;
    m_pCTR->RegisterCallback(m_pCBFunc, m_pCBObj, m_nCBData);

    if (FAILED(hr = m_pGB->AddFilter(m_pRenderer, L"GLTextureRenderer")))
    {
        Msg(TEXT("Could not add renderer filter to graph!  hr=0x%x"), hr);
        Clean();
        return E_FAIL;
    }

    // Add the source filter to the graph.
    hr = m_pGB->AddSourceFilter (filename, L"SOURCE", &pFSrc);
    
    // If the media file was not found, inform the user.
    if (hr == VFW_E_NOT_FOUND)
    {
        Msg(TEXT("Could not add source filter to graph!  (hr==VFW_E_NOT_FOUND)\r\n\r\n"));
        Clean();
        return E_FAIL;
    }
    else if(FAILED(hr))
    {
        Msg(TEXT("Could not add source filter to graph!  hr=0x%x"), hr);
        Clean();
        return E_FAIL;
    }

    if (FAILED(hr = pFSrc->FindPin(L"Output", &pFSrcPinOut)))
    {
        Msg(TEXT("Could not find output pin!  hr=0x%x"), hr);
        Clean();
        return E_FAIL;
    }

    if (!withAudio)
    {
        // If no audio component is desired, directly connect the two video pins
        // instead of allowing the Filter Graph Manager to render all pins.

        CComPtr<IPin> pFTRPinIn;      // Texture Renderer Input Pin

        // Find the source's output pin and the renderer's input pin
        if (FAILED(hr = m_pRenderer->FindPin(L"In", &pFTRPinIn)))
        {
            Msg(TEXT("Could not find input pin!  hr=0x%x"), hr);
            Clean();
            return E_FAIL;
        }

        // Connect these two filters
        if (FAILED(hr = m_pGB->Connect(pFSrcPinOut, pFTRPinIn)))
        {
            Msg(TEXT("Could not connect pins!  hr=0x%x"), hr);
            Clean();
            return E_FAIL;
        }
    }
    else
    {
        // Render the source filter's output pin.  The Filter Graph Manager
        // will connect the video stream to the loaded CTextureRenderer
        // and will load and connect an audio renderer (if needed).

        if (FAILED(hr = m_pGB->Render(pFSrcPinOut)))
        {
            Msg(TEXT("Could not render source output pin!  hr=0x%x"), hr);
            Clean();
            return E_FAIL;
        }
    }
   
    // Get the graph's media control, event & position interfaces
    m_pGB.QueryInterface(&m_pMC);
    m_pGB.QueryInterface(&m_pMP);
    m_pGB.QueryInterface(&m_pMS);
    m_pGB.QueryInterface(&m_pME);
    m_pGB.QueryInterface(&m_pBA);

    return S_OK;
}

//-----------------------------------------------------------------------------
// CheckMovieStatus: If the movie has ended, rewind to beginning
//-----------------------------------------------------------------------------
BOOL CTextureRendererGraphManager::CheckMovieStatus()
{
    long lEventCode;
    LONG_PTR lParam1, lParam2;
    HRESULT hr;

    if (!m_pME)
        return FALSE;
        
    // Check for completion events
    hr = m_pME->GetEvent(&lEventCode, &lParam1, &lParam2, 0);
    if (SUCCEEDED(hr))
    {
        // If we have reached the end of the media file, reset to beginning
        if (EC_COMPLETE == lEventCode) 
        {
            if (m_bLoop)
                hr = m_pMP->put_CurrentPosition(0);
            else
                m_bPlaying = FALSE;
        }

        // Free any memory associated with this event
        hr = m_pME->FreeEventParams(lEventCode, lParam1, lParam2);
    }

    return m_bPlaying;
}

//-----------------------------------------------------------------------------
// GetWidth: Get Width of the movie.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::GetWidth(LONG* width)
{
    if (m_pRenderer) *width = m_pCTR->m_lVidWidth;
    return (S_OK);
}

//-----------------------------------------------------------------------------
// GetHeight: Get Height of the movie.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::GetHeight(LONG* height)
{
    if (m_pRenderer) *height = m_pCTR->m_lVidHeight;
    return (S_OK);
}

//-----------------------------------------------------------------------------
// GetDuration: Get Duration of the movie in sec.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::GetDuration(double* duration)
{
    HRESULT hr;
    if (!m_pMP) return (E_FAIL);
    if (FAILED(hr = m_pMP->get_Duration(duration))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// Run: Play the movie.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::Run()
{
    HRESULT hr;
    m_bPlaying = TRUE;
    if (!m_pMC) return (E_FAIL);
    if (FAILED(hr = m_pMC->Run())) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// Pause : Pause the movie (and keep the current image).
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::Pause()
{
    HRESULT hr;
    m_bPlaying = FALSE;
    if (!m_pMC) return (E_FAIL);
    if (FAILED(hr = m_pMC->Pause())) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// Stop : Stop the movie.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::Stop(BOOL forcedStop)
{
    HRESULT hr;
    m_bPlaying = FALSE;
    if (!m_pMC) return (E_FAIL);
    if (FAILED(hr = (forcedStop ? m_pMC->Stop() : m_pMC->StopWhenReady()))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// IsPlaying: Return TRUE if playing.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::IsPlaying(BOOL* playing)
{
    *playing = m_bPlaying;
    return(S_OK);
}

//-----------------------------------------------------------------------------
// SetLoop: Set the loop mode.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::SetLoop(BOOL loop)
{
    m_bLoop = loop;
    return(S_OK);
}

//-----------------------------------------------------------------------------
// GetLoop: Get the loop mode.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::GetLoop(BOOL* loop)
{
    *loop = m_bLoop;
    return(S_OK);
}

//-----------------------------------------------------------------------------
// SetPosition: Set the current playback position in sec.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::SetPosition(double pos)
{
    HRESULT hr;
    if (!m_pMP) return (E_FAIL);
    if (FAILED(hr = m_pMP->put_CurrentPosition(pos))) return (hr);
    return(S_OK);
}

//-----------------------------------------------------------------------------
// GetPosition: Get the current playback position in sec.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::GetPosition(double* pos)
{
    HRESULT hr;
    if (!m_pMP) return (E_FAIL);
    if (FAILED(hr = m_pMP->get_CurrentPosition(pos))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// SetRate: Set the playback speed (1.0 is the original speed).
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::SetRate(double rate)
{
    HRESULT hr;
    if (!m_pMS) return (E_FAIL);
    if (FAILED(hr = m_pMS->SetRate(rate))) return(hr);
    return(S_OK);
}

//-----------------------------------------------------------------------------
// GetRate: Get the playback speed (1.0 is the original speed).
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::GetRate(double *rate)
{
    HRESULT hr;
    if (!m_pMS) return (E_FAIL);
    if (FAILED(hr = m_pMS->GetRate(rate))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// SetVolume: Set the sound volume (0L is max, -10000L is min).
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::SetVolume(LONG volume)
{
    HRESULT hr;
    if (!m_pBA) return (E_FAIL);
    if (FAILED(hr = m_pBA->put_Volume(volume))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// GetVolume: Get the sound volume (0L is max, -10000L is min).
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::GetVolume(LONG* volume)
{
    HRESULT hr;
    if (!m_pBA) return (E_FAIL);
    if (FAILED(hr = m_pBA->get_Volume(volume))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// VolumeSilence: Mute the sound volume.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::VolumeSilence()
{
    HRESULT hr;
    if (!m_pBA) return (E_FAIL);
    if (FAILED(hr = m_pBA->put_Volume(-10000L))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// VolumeFull: Maximize the sound volume.
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::VolumeFull()
{
    HRESULT hr;
    if (!m_pBA) return (E_FAIL);
    if (FAILED(hr = m_pBA->put_Volume(0L))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// GetFramesDropped: Get the number of the dropped frames (not worked?).
//-----------------------------------------------------------------------------
HRESULT CTextureRendererGraphManager::GetFramesDropped(int* frames)
{
    HRESULT hr;
    if (!m_pRenderer) return (E_FAIL);
    if (FAILED(hr = m_pCTR->get_FramesDroppedInRenderer(frames))) return(hr);
    else return(S_OK);
}

//-----------------------------------------------------------------------------
// CTextureRenderer constructor
//-----------------------------------------------------------------------------
CTextureRenderer::CTextureRenderer( LPUNKNOWN pUnk, HRESULT *phr )
                                  : CBaseVideoRenderer(__uuidof(CLSID_GLTextureRenderer), 
                                    NAME("OpenGL Texture Renderer"), pUnk, phr),
                                    m_pCBFunc(NULL)
{
    // Store and AddRef the texture for our use.
    ASSERT(phr);
    if (phr)
        *phr = S_OK;
}


//-----------------------------------------------------------------------------
// CTextureRenderer destructor
//-----------------------------------------------------------------------------
CTextureRenderer::~CTextureRenderer()
{
}


//-----------------------------------------------------------------------------
// CheckMediaType: This method forces the graph to give us an R8G8B8 video
// type, making our copy to texture memory trivial.
//-----------------------------------------------------------------------------
HRESULT CTextureRenderer::CheckMediaType(const CMediaType *pmt)
{
    HRESULT   hr = E_FAIL;
    VIDEOINFO *pvi=0;
    
    CheckPointer(pmt,E_POINTER);

    // Reject the connection if this is not a video type
    if( *pmt->FormatType() != FORMAT_VideoInfo ) {
        return E_INVALIDARG;
    }
    
    // Only accept RGB24 video
    pvi = (VIDEOINFO *)pmt->Format();

    if(IsEqualGUID( *pmt->Type(),    MEDIATYPE_Video)  &&
       IsEqualGUID( *pmt->Subtype(), MEDIASUBTYPE_RGB24))
    {
        hr = S_OK;
    }
    
    return hr;
}

//-----------------------------------------------------------------------------
// SetMediaType: Graph connection has been made. 
//-----------------------------------------------------------------------------
HRESULT CTextureRenderer::SetMediaType(const CMediaType *pmt)
{
    UINT uintWidth = 2;
    UINT uintHeight = 2;

    // Retrive the size of this media type
    VIDEOINFO *pviBmp;                      // Bitmap info header
    pviBmp = (VIDEOINFO *)pmt->Format();

    m_lVidWidth  = pviBmp->bmiHeader.biWidth;
    m_lVidHeight = abs(pviBmp->bmiHeader.biHeight);

    return S_OK;
}


//-----------------------------------------------------------------------------
// DoRenderSample: A sample has been delivered. Copy it to the texture.
//-----------------------------------------------------------------------------
HRESULT CTextureRenderer::DoRenderSample(IMediaSample* pSample)
{
    BYTE  *pBmpBuffer; // Bitmap buffer, texture buffer

    CheckPointer(pSample,E_POINTER);

    // Get the video bitmap buffer
    pSample->GetPointer(&pBmpBuffer);

    if (m_pCBFunc) m_pCBFunc(m_pCBObj, pBmpBuffer, m_nCBData);

    return S_OK;
}


#ifdef REGISTER_FILTERGRAPH

//-----------------------------------------------------------------------------
// Running Object Table functions: Used to debug. By registering the graph
// in the running object table, GraphEdit is able to connect to the running
// graph. This code should be removed before the application is shipped in
// order to avoid third parties from spying on your graph.
//-----------------------------------------------------------------------------
DWORD dwROTReg = 0xfedcba98;

//-----------------------------------------------------------------------------
// AddToROT: Add to the ROT.
//-----------------------------------------------------------------------------
HRESULT AddToROT(IUnknown* pUnkGraph) 
{
    IMoniker* pmk;
    IRunningObjectTable *pROT;
    if (FAILED(GetRunningObjectTable(0, &pROT))) {
        return E_FAIL;
    }

    WCHAR wsz[256];
    StringCchPrintfW(wsz, NUMELMS(wsz),L"FilterGraph %08x  pid %08x\0", (DWORD_PTR) 0, GetCurrentProcessId());

    HRESULT hr = CreateItemMoniker(L"!", wsz, &pmk);
    if (SUCCEEDED(hr)) 
    {
        // Use the ROTFLAGS_REGISTRATzIONKEEPSALIVE to ensure a strong reference
        // to the object.  Using this flag will cause the object to remain
        // registered until it is explicitly revoked with the Revoke() method.
        //
        // Not using this flag means that if GraphEdit remotely connects
        // to this graph and then GraphEdit exits, this object registration
        // will be deleted, causing future attempts by GraphEdit to fail until
        // this application is restarted or until the graph is registered again.
        hr = pROT->Register(ROTFLAGS_REGISTRATIONKEEPSALIVE, pUnkGraph, 
                            pmk, &dwROTReg);
        pmk->Release();
    }

    pROT->Release();
    return hr;
}

//-----------------------------------------------------------------------------
// RemoveFromROT: Removes from the ROT.
//-----------------------------------------------------------------------------
void RemoveFromROT(void)
{
    IRunningObjectTable *pirot=0;

    if (SUCCEEDED(GetRunningObjectTable(0, &pirot))) 
    {
        pirot->Revoke(dwROTReg);
        pirot->Release();
    }
}

#endif


//-----------------------------------------------------------------------------
// Msg: Display an error message box if needed
//-----------------------------------------------------------------------------
void Msg(TCHAR *szFormat, ...)
{
#ifdef _DEBUG
    TCHAR szBuffer[1024];  // Large buffer for long filenames or URLs
    const size_t NUMCHARS = sizeof(szBuffer) / sizeof(szBuffer[0]);
    const int LASTCHAR = NUMCHARS - 1;

    // Format the input string
    va_list pArgs;
    va_start(pArgs, szFormat);

    // Use a bounded buffer size to prevent buffer overruns.  Limit count to
    // character size minus one to allow for a NULL terminating character.
    StringCchVPrintf(szBuffer, NUMCHARS - 1, szFormat, pArgs);
    va_end(pArgs);

    // Ensure that the formatted string is NULL-terminated
    szBuffer[LASTCHAR] = TEXT('\0');

    MessageBox(NULL, szBuffer, TEXT("Critical Error"), 
               MB_OK | MB_ICONERROR);
#endif //_DEBUG
}


