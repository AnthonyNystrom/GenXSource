//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
//
// Use of this sample source code is subject to the terms of the Microsoft
// license agreement under which you licensed this sample source code. If
// you did not accept the terms of the license agreement, you are not
// authorized to use this sample source code. For the terms of the license,
// please see the license agreement between you and Microsoft or, if applicable,
// see the LICENSE.RTF on your install media or the root of your tools installation.
// THE SAMPLE SOURCE CODE IS PROVIDED "AS IS", WITH NO WARRANTIES OR INDEMNITIES.
//
// Connection.h: interface for the CConnection class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CONNECTION_H__33D8B1CD_31D7_409D_8565_D53240608E57__INCLUDED_)
#define AFX_CONNECTION_H__33D8B1CD_31D7_409D_8565_D53240608E57__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
//#include <ShellAPI.h>
#include <WinBase.h>
#include <connmgr.h>
#include <connmgr_proxy.h>
#pragma comment( lib, "cellcore.lib" )

extern const GUID IID_ConnPrv_IProxyExtension;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Connection manager helper class
//
class CConnection
{
public:
    CConnection()
    {
        m_hThreadStop = CreateEvent(NULL, FALSE, FALSE, NULL);
        m_hConnectionThread = NULL;
        m_pszPath = NULL;
    }

    virtual ~CConnection()
    {
        HangupConnection();

        if( m_hThreadStop )
        {
            CloseHandle( m_hThreadStop );
            m_hThreadStop=NULL;
        }

        delete m_pszPath;
    }

    //
    // Can we get to this resource 'quickly'
    //
    HRESULT IsAvailable( LPCTSTR pszPath=NULL, BOOL bProxy=FALSE )
    {
        HRESULT hr=GetNetworkFromPath(pszPath);
        SetProxy(bProxy);

        if(SUCCEEDED(hr))
        {
            BOOL bAvailable = FALSE;
            HANDLE hConn = NULL;

            CONNMGR_CONNECTIONINFO ci = { 0 };
            ci.cbSize           = sizeof(ci);
            ci.dwParams         = CONNMGR_PARAM_GUIDDESTNET
                                        | CONNMGR_PARAM_MAXCONNLATENCY;
            ci.dwFlags          = bProxy?CONNMGR_FLAG_PROXY_HTTP:0;
            ci.ulMaxConnLatency = 4000;         // 4 second
            ci.bDisabled        = TRUE;
            ci.dwPriority       = CONNMGR_PRIORITY_USERINTERACTIVE;
            ci.guidDestNet      = GetNetworkGuid();

            if (SUCCEEDED(hr=ConnMgrEstablishConnection(&ci, &hConn)))
            {
                DWORD dwResult = WaitForSingleObject(hConn, 400);

                switch (dwResult)
                {
                    case WAIT_OBJECT_0:
                    {
                        DWORD dwStatus;
                        if( SUCCEEDED(ConnMgrConnectionStatus(hConn, &dwStatus)) &&
                            ( (dwStatus == CONNMGR_STATUS_CONNECTED) || (dwStatus == CONNMGR_STATUS_CONNECTIONDISABLED) ))
                        {
                            hr=S_OK;
                            CheckForRequiredProxy(hConn);
                        }
                        else
                        {
                            hr=S_FALSE;
                        }
                        break;
                    }

                    case WAIT_TIMEOUT:
                        hr=E_FAIL;
                        break;
                }
                ConnMgrReleaseConnection(hConn, FALSE);
            }
        }
        return hr;
    }

    //
    // Attempts to connect to the network
    // overried the Do* methods to see what's happening.
    //
    HRESULT AttemptConnect( LPCTSTR pszPath=NULL, BOOL bProxy=FALSE )
    {
		UTILS_LOG(EDMP_NOTICE, "ConnManager: Starting to attempt connection");
        HRESULT hr=GetNetworkFromPath(pszPath);

		UTILS_LOG(EDMP_NOTICE, "ConnManager: Selected network");
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data1);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data2);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data3);
		UTILS_LOG(EDMP_NOTICE, "------------------");
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[0]);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[1]);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[2]);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[3]);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[4]);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[5]);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[6]);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[7]);
		UTILS_LOG(EDMP_NOTICE, "ConnManager: 0x%X", m_gNetwork.Data4[8]);
		UTILS_LOG(EDMP_NOTICE, "------------------");

        SetProxy(bProxy);


        if( m_hThreadStop == NULL )
            return E_INVALIDARG;

        if( SUCCEEDED(hr) )
        {
			UTILS_LOG(EDMP_NOTICE, "ConnManager: Getting network from path done");
            if( SUCCEEDED ( hr=HangupConnection() ))
            {
                // kick off new thread,
                DWORD dwDummy;

                m_hConnectionThread = CreateThread(NULL, 0, s_ConnectionThread, (LPVOID)this, 0, &dwDummy);

                if ( m_hConnectionThread == NULL )
				{
                    hr=E_FAIL;
					UTILS_LOG(EDMP_NOTICE, "ConnManager: Creating connection thread FAILED");
				}
				else
				{
					UTILS_LOG(EDMP_NOTICE, "ConnManager: Creating connection thread done");
				}
            }
        }
        return hr;
    }

    HRESULT HangupConnection()
    {
        if( m_hConnectionThread )
        {
            SetEvent(m_hThreadStop);
			//wait a few seconds at most for the thread to die
			WaitForSingleObject(m_hConnectionThread, 3000); 
            m_hConnectionThread=NULL;
        }
        return S_OK;
    }

    BOOL    IsProxyRequired() { return m_bProxyRequired; }

    //
    // Properties on this object
    //
    HANDLE  GetConnection() { return m_hConnection; }
    DWORD   GetStatus()     { return m_dwStatus; }
    void    SetCache(BOOL b){ m_bCache=b; }
    BOOL    GetCache() { return m_bCache; }
    GUID    GetNetworkGuid() { return m_gNetwork; }
    BOOL    GetProxy() { return m_bProxy; }
    void    SetProxy(BOOL bProxy) { m_bProxy = bProxy; }
    LPTSTR  GetNetworkPath() { return m_pszPath; }
    LPTSTR  GetProxyServer() { if(m_bProxyRequired) return m_ProxyInfo.szProxyServer; else return NULL; }

    static LPTSTR GetInternetPath() { return CConnection::s_szInternetPath; }
    static LPTSTR GetCorpNetPath() { return CConnection::s_szCorpNetPath; }

protected:      // Methods to override.
    //
    // return S_OK to carry on.
    // return E_FAIL to hangup the connection
    //
    virtual HRESULT DoStatusUpdate(DWORD dwStatus)
    {
		UTILS_LOG(EDMP_NOTICE, "ConnManager: DoStatusUpdate");
        HRESULT hRes=S_OK;
        if( dwStatus & CONNMGR_STATUS_DISCONNECTED )
        {
            if( dwStatus != CONNMGR_STATUS_DISCONNECTED )
                hRes = DoConnectingError();
            else
                hRes= DoDisconnected();
            SetCache( dwStatus == CONNMGR_STATUS_DISCONNECTED );
        }
        else if ( dwStatus == CONNMGR_STATUS_CONNECTED )
        {
            hRes = DoConnected();
        }
        else if ( dwStatus & CONNMGR_STATUS_WAITINGCONNECTION )
        {   
            hRes = DoWaitingForConnection();
        }
        return hRes;
    }

    //
    // Override these methods
    //

    //
    // Called when we 
    //
    virtual HRESULT DoEstablishingConnection()
    {
		UTILS_LOG(EDMP_NOTICE, "ConnManager: DoEstablishingConnection");
        return S_OK;
    }

    //
    // Called when there was an error while connecting
    // generally due to network connection not being available (no modem, no nic etc).
    //
    virtual HRESULT DoConnectingError()
    {
		UTILS_LOG(EDMP_NOTICE, "ConnManager: DoConnectingError");
        // we received an error to do with connecting.
        //SHELLEXECUTEINFO sei = {0};
        //TCHAR szExec[MAX_PATH];
        //wsprintf( szExec, TEXT("-CMERROR 0x%x -report"), GetStatus() );
        //sei.cbSize = sizeof(sei);
        //sei.hwnd = NULL;
        //sei.lpFile = TEXT(":MSREMNET");
        //sei.lpParameters = szExec;
        //sei.nShow = SW_SHOWNORMAL;
        //ShellExecuteEx( &sei );
        return E_FAIL;
    }

    //
    // Called when a connection is now available.
    //
    virtual HRESULT DoConnected()
    {
		UTILS_LOG(EDMP_NOTICE, "ConnManager: DoConnected");
        CheckForRequiredProxy( GetConnection() );
        return S_OK;
    }

    //
    // Called when the existing connection has been disconnected
    // by another network request. we return E_FAIL to hangup here
    //
    virtual HRESULT DoDisconnected()
    {
		UTILS_LOG(EDMP_NOTICE, "ConnManager: DoDisconnected");
        return E_FAIL;
    }

    //
    // Called when we are waiting for the network to become available.
    //
    virtual HRESULT DoWaitingForConnection()
    {
		UTILS_LOG(EDMP_NOTICE, "ConnManager: DoWaitingForConnection");
        return S_OK;
    }

    //
    // Called when we have released the connection
    //
    virtual HRESULT DoReleaseConnection()
    {
		UTILS_LOG(EDMP_NOTICE, "ConnManager: DoReleaseConnection");
        return S_OK;
    }

    //
    // Sets the network GUID from a path.
    //
    HRESULT GetNetworkFromPath(LPCTSTR pszPath)
    {
        if( pszPath )
        {
            if( m_pszPath )
                delete m_pszPath;
            m_pszPath = new TCHAR[lstrlen(pszPath)+1];
            if( m_pszPath == NULL ) 
                return E_OUTOFMEMORY;
            lstrcpy(m_pszPath, pszPath);
        }
        return ConnMgrMapURL(m_pszPath, &m_gNetwork, 0);
    }

    //
    // Thread stub cast and calls.
    //
    static DWORD s_ConnectionThread(LPVOID pData)
    {
        CConnection * pConnection=(CConnection*)pData;
        if( pData )
            return pConnection->ConnectionThread();
        return -1;
    }

    //
    // Thread proc
    // Starts a connection to the network
    //
    DWORD ConnectionThread()
    {
        HANDLE hThisThread=m_hConnectionThread;
        CONNMGR_CONNECTIONINFO ConnInfo={0};
        ConnInfo.cbSize=sizeof(ConnInfo);
        ConnInfo.dwParams=CONNMGR_PARAM_GUIDDESTNET;
        ConnInfo.dwFlags=GetProxy() ? CONNMGR_FLAG_PROXY_HTTP: 0;
        ConnInfo.dwPriority=CONNMGR_PRIORITY_USERINTERACTIVE ;
        ConnInfo.guidDestNet = GetNetworkGuid();

        HRESULT hr = ConnMgrEstablishConnection(&ConnInfo, &m_hConnection);
        if( FAILED( hr ) )
        {
			UTILS_LOG(EDMP_NOTICE, "ConnManager: Establishing connection failed");
            DoConnectingError();
            SetCache(FALSE);
        }
        else
        {
			UTILS_LOG(EDMP_NOTICE, "ConnManager: Establishing connection done");
            DoEstablishingConnection();

            HANDLE hObjects[2];
            hObjects[0]=m_hConnection;
            hObjects[1]=m_hThreadStop;
            BOOL    bStop=FALSE;
            
            ResetEvent(m_hThreadStop);

            while( bStop == FALSE )
            {
                DWORD dwResult = WaitForMultipleObjects( 2, hObjects, FALSE, INFINITE); 
                
                if (dwResult == (WAIT_OBJECT_0))
                { 
                    HRESULT hr;
                    DWORD   dwStatus;
                    hr=ConnMgrConnectionStatus(m_hConnection,&dwStatus);
                    m_dwStatus = dwStatus;
                    if( SUCCEEDED(hr))
                    {
                        if( DoStatusUpdate(m_dwStatus) != S_OK )
                            bStop=TRUE;
                    }
                    else
                    {
                        m_dwStatus=hr;
                        bStop=TRUE;
                    }
                }
                else // failures, or signalled to stop.
                {
                    bStop = TRUE;
                    ResetEvent(m_hThreadStop);
                }
            }
        }

        DoReleaseConnection();

        // Release the connection, caching if we should.
        if( m_hConnection )
        {
            ConnMgrReleaseConnection(m_hConnection, GetCache() );
        }

        CloseHandle(hThisThread);

        return GetStatus();
    }


private:
    LPTSTR  m_pszPath;
    BOOL    m_bProxy;
    HANDLE  m_hConnection;  // Connection Manager Handle
    BOOL    m_bCache;       // should we cache this connection when we 'hangup'
    GUID    m_gNetwork;     // the GUID for the network we are connecting to.
    DWORD   m_dwStatus;      // last connection status
    HANDLE  m_hThreadStop;  // Event
    HANDLE  m_hConnectionThread;    // Thread
    BOOL    m_bProxyRequired;

    BOOL    CheckForRequiredProxy(HANDLE hConn)
    {
        m_bProxyRequired=FALSE;
        ZeroMemory(&m_ProxyInfo, sizeof(m_ProxyInfo));
        m_ProxyInfo.dwType = CONNMGR_FLAG_PROXY_HTTP;
        if (SUCCEEDED(ConnMgrProviderMessage(   hConn,
                                                &IID_ConnPrv_IProxyExtension,
                                                NULL,
                                                0,
                                                0,
                                                (PBYTE)&m_ProxyInfo,
                                                sizeof(m_ProxyInfo))))
        {
            if (m_ProxyInfo.dwType == CONNMGR_FLAG_PROXY_HTTP)
            {
                m_bProxyRequired=TRUE;
               // SECURITY: Zero out the username/password from memory.
               ZeroMemory(&(m_ProxyInfo.szUsername), sizeof(m_ProxyInfo.szUsername));
               ZeroMemory(&(m_ProxyInfo.szPassword), sizeof(m_ProxyInfo.szPassword));
            }
        }
        return m_bProxyRequired;
    }


    // Rigged paths that will map  to the correct GUID.
    static LPTSTR s_szInternetPath;
    static LPTSTR s_szCorpNetPath;

    PROXY_CONFIG    m_ProxyInfo;
};

#endif // !defined(AFX_CONNECTION_H__33D8B1CD_31D7_409D_8565_D53240608E57__INCLUDED_)
