#include "StdAfx.h"

#include "PaintManager.h"
#include "NewMenu.h"

// If you don't want to see extra TRACE diagnostics,
// modify the line below to: #define CCM_TRACE
#define CCM_TRACE TRACE

#define CCM_TIMER_VAL 100        // 100 ms timer period seems to be good enough...
#define MAX_CLASSNAME 64         // Length of buffer for retrieving the class name

////////////////////////////////////////////////////////////////////////
// CCMControl static members initialization

// skins { 

CBitmap32 CPaintManager::m_bmpButtonSkin;

CBitmap32 CPaintManager::m_bmpCheckSkin;

CBitmap32 CPaintManager::m_bmpRadioSkin;

CBitmap32 CPaintManager::m_bmpSpinDownSkin;
CBitmap32 CPaintManager::m_bmpSpinUpSkin;
CBitmap32 CPaintManager::m_bmpSpinLeftSkin;
CBitmap32 CPaintManager::m_bmpSpinRightSkin;
CBitmap32 CPaintManager::m_bmpSpinDownGlyphSkin;
CBitmap32 CPaintManager::m_bmpSpinUpGlyphSkin;
CBitmap32 CPaintManager::m_bmpSpinLeftGlyphSkin;
CBitmap32 CPaintManager::m_bmpSpinRightGlyphSkin;

// } skins


HWND CPaintManager::CCMControl::m_hWndOld = NULL;
CMapPtrToPtr CPaintManager::m_ctrlMap = 10;
CMapPtrToPtr CPaintManager::m_dlgMap = 10;
BOOL CPaintManager::m_bEnabled = TRUE;

// Changed 02.03.1999 Mike Walter
CMapWordToPtr CPaintManager::m_threadMap = 10;     


///////////////////////////////////////////////////////////////////////
// Here is the one and only CPaintManager object
static CPaintManager g_ctrlManager;

CPaintManager& GetPaintManager()
{
   return g_ctrlManager;
}

////////////////////////////////////////////////////////////////////////
// WH_CALLWNDPROC hook procedure

LRESULT CALLBACK CCM_CallWndProc( int nCode, WPARAM wParam, LPARAM lParam )
{   
   HOOKPROC hHookProc;
   if ( g_ctrlManager.m_threadMap.Lookup( (WORD)GetCurrentThreadId(), (void*&)hHookProc ) == FALSE )
   {
      TRACE( "CPaintManager: No hook for this thread installed!\n" );
      return 0;
   }

   if ( nCode == HC_ACTION )
   {      
      CWPSTRUCT* pwp = (CWPSTRUCT*)lParam;
      if ( g_ctrlManager.IsEnabled() )
      {
         if ( g_ctrlManager.m_bDialogOnly == TRUE )        
         {
            if ( pwp->message == WM_INITDIALOG )
               g_ctrlManager.Install( pwp->hwnd );
         }
         else if ( pwp->message == WM_CREATE && g_ctrlManager.IsEnabled() )
         {
            TCHAR szBuf[MAX_CLASSNAME];
            if ( GetWindowLong( pwp->hwnd, GWL_STYLE ) & WS_CHILD )
            {
               GetClassName( pwp->hwnd, szBuf, MAX_CLASSNAME );
               if ( lstrcmp( szBuf, _T( "ScrollBar" ) ) ) // Don't add scrollbars
                  g_ctrlManager.AddControl( pwp->hwnd );
            }
         }
      }
   }   
   // Changed 02.03.1999 Mike Walter
   return CallNextHookEx( (HHOOK)hHookProc, nCode, wParam, lParam );
}

// Install a hook for the current thread only
void CPaintManager::InstallHook( TCHAR* lpszFilename, DWORD dwThreadID, BOOL bDialogOnly )
{
	//_tcsncpy( m_szSkinLibrary, lpszFilename, MAX_PATH );
	// Здесь надо проверить формат библиотеки и загрузить скины 
	HMODULE hSkinLib = LoadLibrary( lpszFilename );
	if ( NULL == hSkinLib )
		return;
	
	m_bmpButtonSkin.LoadBitmap( _T("BUTTON_BMP"), hSkinLib );
	m_bmpCheckSkin.LoadBitmap( _T("CHECKBOX16_BMP"), hSkinLib );
	m_bmpRadioSkin.LoadBitmap( _T("RADIOBUTTON16_BMP"), hSkinLib );

	m_bmpSpinDownSkin.LoadBitmap( _T("SPINBUTTONBACKGROUNDDOWN_BMP"), hSkinLib );
	m_bmpSpinUpSkin.LoadBitmap( _T("SPINBUTTONBACKGROUNDUP_BMP"), hSkinLib );
	m_bmpSpinLeftSkin.LoadBitmap( _T("SPINBUTTONBACKGROUNDLEFT_BMP"), hSkinLib );
	m_bmpSpinRightSkin.LoadBitmap( _T("SPINBUTTONBACKGROUNDRIGHT_BMP"), hSkinLib );
	m_bmpSpinDownGlyphSkin.LoadBitmap( _T("SPINDOWNGLYPH_BMP"), hSkinLib );
	m_bmpSpinUpGlyphSkin.LoadBitmap( _T("SPINUPGLYPH_BMP"), hSkinLib );
	m_bmpSpinLeftGlyphSkin.LoadBitmap( _T("SPINLEFTGLYPH_BMP"), hSkinLib );
	m_bmpSpinRightGlyphSkin.LoadBitmap( _T("SPINRIGHTGLYPH_BMP"), hSkinLib );

	::FreeLibrary( hSkinLib );


   m_bDialogOnly = bDialogOnly;

   // Changes 02.03.1999 Mike Walter
   HOOKPROC hNewHook;

   if ( m_threadMap.Lookup( (WORD)( dwThreadID == -1 ? GetCurrentThreadId() : dwThreadID ), (void*&)hNewHook ) == FALSE )
   {
      hNewHook = (HOOKPROC)SetWindowsHookEx( WH_CALLWNDPROC,
                                             (HOOKPROC)CCM_CallWndProc,
                                             NULL,
                                             ( dwThreadID == -1 ? GetCurrentThreadId() : dwThreadID ) );

      m_threadMap.SetAt( (WORD)( dwThreadID == -1 ? GetCurrentThreadId() : dwThreadID ), hNewHook );

      CCM_TRACE( "CPaintManager: WH_CALLWNDPROC hook installed for thread: %d\n", ( dwThreadID == -1 ? GetCurrentThreadId() : dwThreadID ) );
   }
   else
      CCM_TRACE( "CPaintManager: WH_CALLWNDPROC hook already installed for thread: %d!\n", ( dwThreadID == -1 ? GetCurrentThreadId() : dwThreadID ) );
}

// Install a global hook for all windows in the system.
// This function may be called only when is put in a DLL.
void CPaintManager::InstallGlobalHook( HINSTANCE hInstance, BOOL bDialogOnly )
{
   ASSERT( hInstance );      // hInstance must not be NULL!
   ASSERT( m_hkWndProc == NULL );
   
   m_bDialogOnly = bDialogOnly;

   HOOKPROC hkProc = (HOOKPROC)GetProcAddress( hInstance, "CCM_CallWndProc" );

   m_hkWndProc = (HOOKPROC)SetWindowsHookEx( WH_CALLWNDPROC,
                                         (HOOKPROC)hkProc,
                                         hInstance,
                                         0 );   

   CCM_TRACE( _T( "CPaintManager: WH_CALLWNDPROC global hook installed\n" ) );
}

void CPaintManager::UninstallHook( DWORD dwThreadID )
{
   // ASSERT( m_hkWndProc != NULL );

   // Changes 02.03.1999 Mike Walter
   HOOKPROC hHookProc;

   if ( dwThreadID == -1 )
   {
      if ( g_ctrlManager.m_threadMap.Lookup( (WORD)GetCurrentThreadId(), (void*&)hHookProc ) == FALSE )
      {
         CCM_TRACE( "CPaintManager: No hook installed for thread: %d!\n", GetCurrentThreadId() );
         return;
      }

      UnhookWindowsHookEx( (HHOOK)hHookProc );
      m_threadMap.RemoveKey( (WORD)GetCurrentThreadId() );

      CCM_TRACE( "CPaintManager: Hook uninstalled for thread: %d\n", GetCurrentThreadId() );
      CCM_TRACE( "CPaintManager: Thread map has %d items\n",g_ctrlManager.m_threadMap.GetCount() );
   }
   else
   {
      if ( g_ctrlManager.m_threadMap.Lookup( (WORD)dwThreadID, (void*&)hHookProc) == FALSE )
      {
         CCM_TRACE( "CPaintManager: No hook installed for thread: %d!\n", dwThreadID );
         return;
      }

      UnhookWindowsHookEx( (HHOOK)hHookProc );
      m_threadMap.RemoveKey( (WORD)dwThreadID );

      CCM_TRACE( "CPaintManager: Hook uninstalled for thread: %d\n", dwThreadID );
      CCM_TRACE( "CPaintManager: Thread map has %d items\n", g_ctrlManager.m_threadMap.GetCount() );
   }

   if ( m_uTimerID && g_ctrlManager.m_threadMap.IsEmpty() == TRUE )
      KillTimer( NULL, m_uTimerID );
}

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CPaintManager::CPaintManager()
{  
   m_hkWndProc = NULL;
   m_uTimerID = 0;
   CCM_TRACE( _T( "CPaintManager::CPaintManager()\n" ) );
}

CPaintManager::~CPaintManager()
{  
   // Changed 02.03.1999 Mike Walter
   POSITION pos = m_threadMap.GetStartPosition();

   while ( pos )
   {
      HOOKPROC hHook;
      DWORD dwThreadID = 0;

      m_threadMap.GetNextAssoc( pos, (WORD&)dwThreadID, (void*&)hHook );
      UninstallHook( dwThreadID );
   }

   // If we have any elements in the map (normally impossible), unsubclass they and remove
   pos = m_ctrlMap.GetStartPosition();

   while ( pos )
   {
      HWND hWnd;

      CCMControl* pCtl;
      m_ctrlMap.GetNextAssoc( pos, (void*&)hWnd, (void*&)pCtl );
      pCtl->Unsubclass();
      m_ctrlMap.RemoveKey( hWnd );
      delete pCtl;
   }

   // Now do the same things for dialog map
   pos = m_dlgMap.GetStartPosition();

   while ( pos )
   {
      HWND hWnd;
      CCMDialog* pCtl;
      m_dlgMap.GetNextAssoc( pos, (void*&)hWnd, (void*&)pCtl );

      pCtl->Unsubclass();
      m_dlgMap.RemoveKey( hWnd );
      delete pCtl;
   }

   CCM_TRACE( "CPaintManager::~CPaintManager()\n" ); 
}

void CPaintManager::Install( HWND hWnd )
{
   CCMControl* pCtl;
   if ( m_dlgMap.Lookup( hWnd, (void*&)pCtl ) ) // Already in the dialog map
      return;

   // Iterate through all child windows
   HWND hCtrl = GetTopWindow( hWnd );

   while ( hCtrl )
   {
      if ( GetWindowLong( hCtrl, GWL_STYLE ) & WS_CHILD )
      {
         TCHAR szBuf[MAX_CLASSNAME];
         GetClassName( hCtrl, szBuf, MAX_CLASSNAME );
         if ( lstrcmpi( szBuf, _T( "#32770" ) ) ) // Never add child dialogs!
            AddControl( hCtrl );
      }
      hCtrl = GetNextWindow( hCtrl, GW_HWNDNEXT );
   }    

   AddDialog( hWnd ); // Add parent window as well

   // Now redraw all recently inserted controls   
   hCtrl = GetTopWindow( hWnd );
   while ( hCtrl ) 
   {
      if ( m_ctrlMap.Lookup( hCtrl, (void*&)pCtl ) )
         pCtl->RePaint();
      hCtrl = GetNextWindow( hCtrl, GW_HWNDNEXT );
   }    
}

void CPaintManager::Uninstall( HWND hWnd )
{
   // Remove all window controls from the map
   // when the window is about to destroy
   CCM_TRACE( _T( "CPaintManager: Uninstall, handle: %X\n" ), hWnd );
   
   HWND hCtrl = GetTopWindow( hWnd );
   while ( hCtrl )
   {
      if ( GetWindowLong( hCtrl, GWL_STYLE ) & WS_CHILD )
         RemoveControl( hCtrl );
      hCtrl = GetNextWindow( hCtrl, GW_HWNDNEXT );
   }
}

// In lpszClass you can specify class name, which will be used 
// instead of true class name (useful for non-standard controls 
// that are similar to the one of those we have supported)
BOOL CPaintManager::AddControl( HWND hWnd, LPCTSTR lpszClass )
{  
   CCMControl* pCtl = NULL; 

   // Must not be NULL or already in the map
   if ( hWnd == NULL || m_ctrlMap.Lookup( hWnd, (void*&)pCtl ) )
      return FALSE;   
   
   TCHAR szBuf[MAX_CLASSNAME];
   if ( lpszClass == NULL )
      GetClassName( hWnd, szBuf, MAX_CLASSNAME );
   else
      lstrcpy( szBuf, lpszClass );

   DWORD dwStyle = GetWindowLong( hWnd, GWL_STYLE );
   DWORD dwExStyle = GetWindowLong( hWnd, GWL_EXSTYLE );

	if ( !lstrcmpi( szBuf, _T( "Button" ) ) )
	{
		if ( ( dwStyle & BS_OWNERDRAW ) == BS_OWNERDRAW ) {        
			return FALSE;     // Do not subclass ownerdraw buttons
		} else if ( ( dwStyle & BS_GROUPBOX ) == BS_GROUPBOX ||
									( dwStyle & BS_FLAT ) == BS_FLAT ) {
			 return FALSE;     // Skip all group boxes and flat buttons
		} else if ( ( dwStyle & BS_AUTOCHECKBOX ) == BS_AUTOCHECKBOX ||
                ( dwStyle & BS_CHECKBOX ) == BS_CHECKBOX ||                
								( dwStyle & BS_3STATE ) == BS_3STATE ) {
			pCtl = new CCMCheckBox;
		} else if ( ( dwStyle & BS_AUTORADIOBUTTON ) == BS_AUTORADIOBUTTON ||  
									( dwStyle & BS_RADIOBUTTON ) == BS_RADIOBUTTON ) {
			pCtl = new CCMRadioButton;
		}else
         pCtl = new CCMPushButton;     // If none of the above then it must be a pushbutton!
   } /*else if ( !lstrcmpi( szBuf, _T( "ComboBox" ) ) ) {      
      // Special case for simple comboboxes
      if ( ( dwStyle & 0x03 ) == CBS_SIMPLE )
      {
         hWnd = GetTopWindow( hWnd );
         while ( hWnd )
         {
            AddControl( hWnd );
            hWnd = GetNextWindow( hWnd, GW_HWNDNEXT );
         }
         return FALSE;
      }   
      else
         pCtl = new CCMComboBox;
   }*/
   else if ( !lstrcmpi( szBuf, _T( "Edit" ) ) )
   {
      // Edit window in a simple combobox
      GetClassName( GetParent( hWnd ), szBuf, MAX_CLASSNAME );
      if ( !lstrcmpi( szBuf, _T( "ComboBox" ) ) && 
            ( GetWindowLong( GetParent( hWnd ), GWL_STYLE ) & 0x03 ) == CBS_SIMPLE ) 
         pCtl = new CCMEditCombo;
      else
      {
         if ( dwExStyle & WS_EX_CLIENTEDGE )
            pCtl = new CCMEdit;
      }
     #if defined _DEBUG
       lstrcpy( szBuf, _T( "Edit" ) );
     #endif
   }/*
   else if ( !lstrcmpi( szBuf, _T( "ListBox" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
         pCtl = new CCMControl;
   }
   else if ( !lstrcmpi( szBuf, _T( "SysListView32" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
      {
         pCtl = new CCMControl;
         AddControl( GetTopWindow( hWnd ) );  // Don't forget to add the header control
      }
   }
   else if ( !lstrcmpi( szBuf, _T( "SHELLDLL_DefView" ) ) ) // In open/save common dialogs
   {
      AddControl( GetTopWindow( hWnd ) );  // Add child ListView control
      return FALSE;
   }
   else if ( !lstrcmpi( szBuf, _T( "SysTreeView32" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
         pCtl = new CCMControl;
   }
   else if ( !lstrcmpi( szBuf, _T( "SysDateTimePick32" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
      {
         pCtl = new CCMDateTime;
         if ( dwStyle & DTS_UPDOWN )
            AddControl( GetTopWindow( hWnd ) );  // Add up-down control as well
      }
   }
   else if ( !lstrcmpi( szBuf, _T( "SysMonthCal32" ) ) )
      pCtl = new CCMControl;
	 */ 
   else if ( !lstrcmpi( szBuf, _T( "msctls_updown32" ) ) )
      pCtl = new CCMUpDown;
	 /*
   else if ( !lstrcmpi( szBuf, _T( "ComboLBox" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
         pCtl = new CCMControl;
   }
   else if ( !lstrcmpi( szBuf, _T( "ScrollBar" ) ) )
   {
      if ( !( dwStyle & SBS_SIZEBOX ) && !( dwStyle & SBS_SIZEGRIP ) )
         pCtl = new CCMScrollBar;
   }
   else if ( !lstrcmpi( szBuf, _T( "ComboBoxEx32" ) ) )
   {
      AddControl( GetTopWindow( hWnd ) );
      return FALSE;
   }
   else if ( !lstrcmpi( szBuf, _T( "msctls_hotkey32" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
         pCtl = new CCMControl;
   }
   else if ( !lstrcmpi( szBuf, _T( "SysIPAddress32" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
         pCtl = new CCMIPAddress;
   }
   else if ( !lstrcmpi( szBuf, _T( "msctls_trackbar32" ) ) )
      pCtl = new CCMTrackbar;
   else if ( !lstrcmpi( szBuf, _T( "RichEdit" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
         pCtl = new CCMControl;
   }
   else if ( !lstrcmpi( szBuf, _T( "RichEdit20W" ) ) )
   {
      if ( dwExStyle & WS_EX_CLIENTEDGE )
         pCtl = new CCMControl;
   }
   else if ( !lstrcmpi( szBuf, _T( "SysHeader32" ) ) )
   {
      if ( dwStyle & HDS_BUTTONS )         
         pCtl = new CCMHeaderCtrl;
      else
         return FALSE;
   }
   else if ( !lstrcmpi( szBuf, _T( "ToolbarWindow32" ) ) )
   {
      // Skip the flat toolbars
      if ( dwStyle & TBSTYLE_FLAT )
         return FALSE;
      HWND hCtrl = GetTopWindow( hWnd );  // Add additional toolbar controls
      while ( hCtrl )
      {
         AddControl( hCtrl );
         hCtrl = GetNextWindow( hCtrl, GW_HWNDNEXT );
      }      
      pCtl = new CCMToolbar;
   }
   else if ( !lstrcmpi( szBuf, _T( "SysTabControl32" ) ) )
   {
      pCtl = new CCMTabControl;
      HWND hWndTop = GetTopWindow( hWnd );
      if ( hWndTop )
         AddControl( hWndTop );  // Also add the up-down control (horizontal tabs only)
   }*/
   else  // Unknown control, do not process
      return FALSE;

   if ( pCtl )
   {
      CCM_TRACE( _T( "CPaintManager::AddControl, handle: %X, type: %s\n" ), 
                 hWnd, szBuf );

      // Perform window subclassing
      pCtl->Subclass( hWnd, CCM_ControlProc );

      // Add the control to the map         
      m_ctrlMap.SetAt( hWnd, pCtl );
      
      if ( m_uTimerID == 0 ) // If timer is not initialized yet
      {    
         m_uTimerID = (UINT)SetTimer( NULL, 0, CCM_TIMER_VAL, CCM_TimerProc );
         CCM_TRACE( _T( "CControlManager: Timer created\n" ) );
         ASSERT( m_uTimerID );    // Failed to create the timer
      }
      return TRUE;
   }
   return FALSE;
}

BOOL CPaintManager::RemoveControl( HWND hWnd )
{
   BOOL bResult = TRUE;   
   CCMControl* pCtl;
   if ( m_ctrlMap.Lookup( hWnd, (void*&)pCtl ) == FALSE )
      bResult =  FALSE;   

  #if defined _DEBUG
   TCHAR szBuf[MAX_CLASSNAME];
   GetClassName( hWnd, szBuf, MAX_CLASSNAME );         
   CCM_TRACE( _T( "CPaintManager::RemoveControl, handle: %X, class: %s, " ), 
           hWnd, szBuf );
   CCM_TRACE( bResult ? _T( "OK\n" ) : _T( "fail\n" ) );
  #endif

   if ( bResult == TRUE )
   {
      // Unsubclass window and next remove it from the map
      pCtl->Unsubclass();
      m_ctrlMap.RemoveKey( hWnd );
      delete pCtl;      // Destroy the object

      if ( m_ctrlMap.IsEmpty() )
      {
         KillTimer( NULL, m_uTimerID );
         CCM_TRACE( _T( "CPaintManager: Timer killed, map is empty\n" ) );
         m_uTimerID = 0;
      }  
      else
         CCM_TRACE( _T( "CPaintManager: map has %d items\n" ), m_ctrlMap.GetCount() );
   }
   return bResult;
}

void CPaintManager::AddDialog( HWND hWnd )
{   
   if ( hWnd  )
   {
      CCMDialog* pCtl = new CCMDialog;
      pCtl->Subclass( hWnd, CCM_DialogProc );    // Perform window subclassing
      m_dlgMap.SetAt( hWnd, pCtl ); // Add the dialog to the map   
   }
}

void CPaintManager::RemoveDialog( HWND hWnd )
{   
   CCMDialog* pCtl;
   if ( m_dlgMap.Lookup( hWnd, (void*&)pCtl ) == TRUE )   
   {
      // Unsubclass window and next remove it from the map
      pCtl->Unsubclass();
      m_dlgMap.RemoveKey( hWnd );
      delete pCtl;      // Destroy the object
   }
}

static void CALLBACK CCM_TimerProc( HWND /*hwnd*/, UINT /*uMsg*/, 
                                    UINT /*idEvent*/, DWORD /*dwTime*/ )
{ 
   g_ctrlManager.TimerProc();
}

void CPaintManager::TimerProc()
{
   // Do not process when the map is empty or the capture is set
   if ( m_ctrlMap.IsEmpty() || GetCapture() != NULL )
      return;

   POINT point;
   GetCursorPos( &point );
   HWND hWnd = WindowFromPoint( point );
   
   CCMControl* pCtl;

   // Lookup for a window in the map      
   if ( m_ctrlMap.Lookup( hWnd, (void*&)pCtl ) == FALSE ) // Not found
   {
      // If window does not exist in the map, it can be
      // a child of the control (e.g. edit control in ComboBox
      // or header control in ListView). If so, get the parent window and
      // carry on
      hWnd = GetParent( hWnd );            
      // Not our window, so just reset previous control and exit
      if ( hWnd == NULL || m_ctrlMap.Lookup( hWnd, (void*&)pCtl ) == FALSE ) {            
         // Not our window, so just reset previous control and exit
         if ( m_ctrlMap.Lookup( CCMControl::m_hWndOld, (void*&)pCtl ) == TRUE )
	         pCtl->ResetHover();
         return;
      }      
   }

//   if ( pCtl->NeedRedraw( point ) ) // Window has been found and needs to be redrawn!
	 if ( !pCtl->IsHovered() ) // Window has been found and needs to be redrawn!
   {
      // First, reset old control (if any)      
      CCMControl* pCtlOld;
      if ( m_ctrlMap.Lookup( CCMControl::m_hWndOld, (void*&)pCtlOld ) == TRUE)
         pCtlOld->ResetHover();

      // Redraw control under the cursor            
			pCtl->SetHover( hWnd );
   }
}

///////////////////////////////////////////////////////////////////////
// All messages from subclassed dialogs will come here

static LRESULT CALLBACK CCM_DialogProc( HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam )
{  
   // Try to find dialog in the dialog map    
   CPaintManager::CCMDialog* pCtl;
   if ( g_ctrlManager.m_dlgMap.Lookup( hWnd, (void*&)pCtl ) == FALSE )
   {
      // This is not our dialog, so just apply default processing
      return DefWindowProc( hWnd, uMsg, wParam, lParam ); 
   }

   // Otherwise, let the dialog to process this message
   return pCtl->WindowProc( uMsg, wParam, lParam );
}

///////////////////////////////////////////////////////////////////////
// All messages from subclassed controls will come here

static LRESULT CALLBACK CCM_ControlProc( HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam )
{  
   CPaintManager::CCMControl* pCtl;

   // Try to find window in the control map
   if ( g_ctrlManager.m_ctrlMap.Lookup( hWnd, (void*&)pCtl ) == FALSE )
   {
      // This is not our window, so just apply default processing
      return DefWindowProc( hWnd, uMsg, wParam, lParam ); 
   }

   // Otherwise, let the control to process this message
   return pCtl->WindowProc( uMsg, wParam, lParam );
}

//////////////////////////////////////////////////////////////////////////////
// CCMControl and derived classes

CPaintManager::CCMControl::CCMControl()
{
	m_hWnd = NULL;
	m_oldWndProc = NULL;
	//   m_nState = dsNormal;
	//   m_nOldState = dsNormal;
	m_hbrActiveBorder = ::CreateSolidBrush( RGB( 0x7F, 0x9D, 0xB9 ) );
	m_hbrInactiveBorder = ::CreateSolidBrush( ::GetSysColor( COLOR_3DSHADOW ) );
	m_hbrWindow = ::CreateSolidBrush( ::GetSysColor( COLOR_WINDOW ) );
	m_hbrDisabledWindow = ::CreateSolidBrush( ::GetSysColor( COLOR_BTNFACE ) );
	m_pnActiveBorder = CreatePen( PS_SOLID, 1, RGB( 0x7F, 0x9D, 0xB9 ) );
	m_pnInactiveBorder = CreatePen( PS_SOLID, 1, ::GetSysColor( COLOR_3DSHADOW ) );
	m_pnWindow = CreatePen( PS_SOLID, 1, ::GetSysColor( COLOR_WINDOW ) );
	m_pnDisabledWindow = CreatePen( PS_SOLID, 1, ::GetSysColor( COLOR_BTNFACE ) );
	m_nLastState = 100;
	m_pszText = NULL;
	m_bEnabled = TRUE;
	m_bFocused = FALSE;
	m_bHover = FALSE;
	m_bFullRepaint = FALSE;
}

CPaintManager::CCMControl::~CCMControl()
{
	::DeleteObject( m_hbrActiveBorder );
	::DeleteObject( m_hbrInactiveBorder );
	::DeleteObject( m_hbrWindow );
	::DeleteObject( m_hbrDisabledWindow );
	::DeleteObject( m_pnActiveBorder );
	::DeleteObject( m_pnInactiveBorder );
	::DeleteObject( m_pnWindow );
	::DeleteObject( m_pnDisabledWindow );
	if ( NULL != m_pszText ) {
		free( m_pszText );
		m_pszText = NULL;
	}
}

void CPaintManager::CCMControl::InitText() {

	if ( NULL != m_pszText ) {
		free( m_pszText );
		m_pszText = NULL;
	}
	
	int nLength = GetWindowTextLength( m_hWnd );
	m_pszText = (TCHAR*)malloc( sizeof( TCHAR ) * (nLength + 1) );
	memset( (LPVOID)m_pszText, 0, sizeof( TCHAR ) * (nLength + 1) );
	if ( nLength > 0 )
		GetWindowText( m_hWnd, m_pszText, nLength + 1 );
}

HDC CPaintManager::CCMControl::BeginDraw( LPRECT lprc )
{
	 // First time initialization
   if ( NULL == m_pszText ) {
	   InitText();
	   m_bEnabled = IsWindowEnabled( m_hWnd );
	   m_bFocused = ( ::GetFocus() == m_hWnd );
   }

   GetWindowRect( m_hWnd, lprc );         
   OffsetRect( lprc, -lprc->left, -lprc->top );
   return GetWindowDC( m_hWnd );
}

void CPaintManager::CCMControl::RePaint()
{  
	RECT rect;
	HDC hDC = BeginDraw( &rect );

	if ( GetWindowLong( m_hWnd, GWL_EXSTYLE ) & WS_EX_CLIENTEDGE ) {
		RECT rc;
		GetWindowRect( m_hWnd, &rc );
		POINT point = { 0, 0 };
		ClientToScreen( m_hWnd, &point );
		if ( point.x == rc.left + 3 )
				InflateRect( &rect, -1, -1 );
	}
	
//	if( 0 == _tcsicmp( _T("default button"), m_pszText ) )
//		TRACE3(" RECT( %d, %d, %d) \r\n", rect.left, rect.top, rect.bottom );

	// flickering free!
//	HDC hMemDC = ::CreateCompatibleDC( hDC );
//	HBITMAP hbmpMemDC = ::CreateCompatibleBitmap( hDC, rect.right-rect.left, rect.bottom - rect.top );
//	HGDIOBJ hgdiOld = ::SelectObject( hMemDC, hbmpMemDC );

	DrawControl( hDC, &rect );
	
//	::BitBlt( hDC, 0, 0, rect.right-rect.left, rect.bottom-rect.top, hMemDC, 0, 0, SRCCOPY );
//	::SelectObject( hMemDC, hgdiOld );
//	::DeleteDC( hMemDC );

	::ReleaseDC( WindowFromDC( hDC ), hDC );
}

/*BOOL CPaintManager::CCMControl::NeedRedraw( const POINT& point )
{
   return m_hWnd != m_hWndOld ? TRUE : FALSE;
}
*/
void CPaintManager::CCMCore::Subclass( HWND hWnd, WNDPROC wndNewProc )
{
   ASSERT( hWnd );      // Do you really want to subclass a window that has a NULL handle?
   m_hWnd = hWnd;   

   // Store address of the original window procedure
   m_oldWndProc = (WNDPROC)(LONG_PTR)(LONG)GetWindowLongPtr( m_hWnd, GWL_WNDPROC );

   // And set the new one
   SetWindowLongPtr( m_hWnd, GWL_WNDPROC, (LONG)(LONG_PTR)wndNewProc );
}

void CPaintManager::CCMCore::Unsubclass()
{
   SetWindowLongPtr( m_hWnd, GWL_WNDPROC, (LONG)(LONG_PTR)m_oldWndProc );
}

LRESULT CPaintManager::CCMControl::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
	switch ( uMsg )
	{     
		case WM_SETTEXT:
//			TraceMsg( uMsg, WM_SETTEXT, _T("WM_SETTEXT") );
			m_nLastState = 1000;
			return RePaintControl( uMsg, wParam, lParam );

	  case WM_SETFOCUS:
//			TraceMsg( uMsg, WM_SETFOCUS, _T("WM_SETFOCUS") );
			m_bFocused = TRUE;
			m_nLastState = 1000;
			return RePaintControl( uMsg, wParam, lParam );

		case WM_KILLFOCUS:
//			TraceMsg( uMsg, WM_KILLFOCUS, _T("WM_KILLFOCUS") );
			m_bFocused = FALSE;
			m_nLastState = 1000;
			return RePaintControl( uMsg, wParam, lParam );

		case WM_ENABLE:
//			TraceMsg( uMsg, WM_ENABLE, _T("WM_ENABLE") );
			m_bEnabled = (wParam != 0);
#if(_WIN32_WINNT >= 0x0500)
	  case WM_UPDATEUISTATE:
//			TraceMsg( uMsg, WM_UPDATEUISTATE, _T("WM_UPDATEUISTATE") );
			m_nLastState = 1000;
#endif
		case WM_ERASEBKGND: // SHOULDN'T receive, see WM_PAINT
			m_nLastState = 1000;
//			TraceMsg( uMsg, WM_ERASEBKGND, _T("WM_ERASEBKGND") );
	  case WM_NCPAINT: // SHOULDN'T receive, see WM_PAINT
//			TraceMsg( uMsg, WM_NCPAINT, _T("WM_NCPAINT") );
	  case WM_PAINT:
//			TraceMsg( uMsg, WM_PAINT, _T("WM_PAINT") );
			if( m_bFullRepaint ) {
				ValidateRect( m_hWnd, NULL );
				return RePaintControl( WM_NULL/*uMsg*/, wParam, lParam );
			} else {
				return RePaintControl( uMsg, wParam, lParam );
			}

		case WM_NCDESTROY:
			// Unsubclass window and remove it from the map
      CallWindowProc( m_oldWndProc, m_hWnd, uMsg, wParam, lParam );
      g_ctrlManager.RemoveControl( m_hWnd );
      return 0;
   }

   return CallWindowProc( m_oldWndProc, m_hWnd, uMsg, wParam, lParam );
}

void CPaintManager::CCMControl::DrawBtnText( HDC hDC, TCHAR* lpszText, LPRECT lprc, DWORD dwBtnStyle, BOOL bGrayed, DWORD dwHDefault, DWORD dwVDefault ) {
		
	UINT nFormat = 0;//DT_END_ELLIPSIS;
	if( BS_MULTILINE & dwBtnStyle )
		nFormat |= DT_WORDBREAK;
	else
		nFormat |= DT_VCENTER | DT_SINGLELINE;
	if ( BS_RIGHT & dwBtnStyle ) {
		if( BS_LEFTTEXT & dwBtnStyle )
			nFormat |= DT_LEFT;
		else
			nFormat |= DT_RIGHT;
	} else if ( BS_CENTER & dwBtnStyle ) {
		nFormat |= DT_CENTER;
	} else if ( BS_LEFT & dwBtnStyle ) {
		if( BS_LEFTTEXT & dwBtnStyle )
			nFormat |= DT_RIGHT;
		else
			nFormat |= DT_LEFT;
	} else {
		nFormat |= dwHDefault;
	}		

	if ( BS_TOP & dwBtnStyle ) {
		nFormat |= DT_TOP;
	} else if ( BS_CENTER & dwBtnStyle ) {
		nFormat |= DT_VCENTER;
	} else if ( BS_BOTTOM & dwBtnStyle ) {
		nFormat |= DT_BOTTOM;
	} else {
		nFormat |= dwVDefault;
	}
	
	DrawText( hDC, lpszText, lprc, nFormat, bGrayed );
}

void CPaintManager::CCMControl::DrawCtrlBk( HDC hDC, LPRECT lprc ) {

	BOOL bTransparent = WS_EX_TRANSPARENT == ( GetWindowLong( m_hWnd, GWL_EXSTYLE ) & WS_EX_TRANSPARENT );
	if ( !bTransparent ) {
		// Opaque bkgnd
		FillSolidRect( hDC, lprc, GetSysColor( COLOR_BTNFACE ) );
	} else {
		// transparent bkgnd
		HWND hWndParent = ::GetParent( m_hWnd );
		if ( hWndParent ) {
			::IntersectClipRect( hDC, lprc->left, lprc->top, lprc->right, lprc->bottom );
			CPoint pt(0, 0);
			MapWindowPoints( m_hWnd, hWndParent, &pt, 1);
			pt = CDC::FromHandle(hDC)->OffsetWindowOrg( pt.x, pt.y );
			::SendMessage( hWndParent, WM_ERASEBKGND, (WPARAM)hDC, 0L);
			CDC::FromHandle(hDC)->SetWindowOrg(pt.x, pt.y);
		}
	}
}

LRESULT CPaintManager::CCMControl::RePaintControl( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL bSilent ) {
	

	if ( !bSilent && m_bFullRepaint ) 
		SendMessage( m_hWnd, WM_SETREDRAW, FALSE, 0);

	LRESULT lRes = CallWindowProc( m_oldWndProc, m_hWnd, uMsg, wParam, lParam );

	if ( WM_SETTEXT == uMsg )
		InitText();		  

	if ( !bSilent && m_bFullRepaint ) 
		SendMessage( m_hWnd, WM_SETREDRAW, TRUE, 0);
	
	RePaint();

	return lRes;
}

void CPaintManager::CCMControl::DrawControl( HDC hDC, LPRECT lprc )
{ 
	RECT rcBorder = *lprc;
	::InflateRect( &rcBorder, -1, -1);
	if ( !m_bEnabled ) {
		::FrameRect( hDC, lprc, m_hbrInactiveBorder );
		::FrameRect( hDC, &rcBorder, m_hbrDisabledWindow );
	} else {
		::FrameRect( hDC, lprc, m_hbrActiveBorder );
		::FrameRect( hDC, &rcBorder, m_hbrWindow );
	}

   DrawScrollBars( hDC, lprc );
}

void CPaintManager::CCMControl::DrawScrollbarThumb( HDC hDC, LPRECT lprc )
{
/*   if ( m_nState & dsHoverMask )
      Draw3dBorder( hDC, rc, COLOR_3DFACE, COLOR_3DDKSHADOW, 
                             COLOR_3DHIGHLIGHT, COLOR_3DSHADOW );
   else
      Draw3dBorder( hDC, rc, COLOR_3DHIGHLIGHT, COLOR_3DSHADOW,
                             COLOR_3DFACE, COLOR_3DFACE );
														 */
}

void CPaintManager::CCMControl::DrawScrollBars( HDC hDC, LPRECT lprc )
{  
   const int nFrameSize  = GetSystemMetrics( SM_CXEDGE ); 
   const int nScrollSize = GetSystemMetrics( SM_CXHSCROLL );   

   RECT rc; 
   DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );  
   if ( dwStyle & WS_HSCROLL &&  dwStyle & WS_VSCROLL ) {
      rc.left = lprc->left + nFrameSize; 
			rc.top = lprc->bottom - nFrameSize - nScrollSize;
      rc.right = lprc->right - nFrameSize - nScrollSize; 
			rc.bottom = lprc->bottom - nFrameSize;
      
			DrawScrollBar( hDC, &rc, SB_HORZ );

      rc.left = lprc->right - nFrameSize - nScrollSize; 
			rc.top = lprc->top + nFrameSize;
      rc.right = lprc->right - nFrameSize; 
			rc.bottom = lprc->bottom - nFrameSize - nScrollSize;
      
			DrawScrollBar( hDC, &rc, SB_VERT );

   } else if ( dwStyle & WS_VSCROLL ) {
      rc.left = lprc->right - nFrameSize - nScrollSize; 
			rc.top = lprc->top + nFrameSize;
      rc.right = lprc->right - nFrameSize; 
			rc.bottom = lprc->bottom - nFrameSize;
      
			DrawScrollBar( hDC, &rc, SB_VERT );

   } else if ( dwStyle & WS_HSCROLL ) {
      rc.left = lprc->left + nFrameSize; 
			rc.top = lprc->bottom - nFrameSize - nScrollSize;
      rc.right = lprc->right - nFrameSize; 
			rc.bottom = lprc->bottom - nFrameSize;
      
			DrawScrollBar( hDC, &rc, SB_HORZ );
   }
}

void CPaintManager::CCMControl::DrawScrollBar( HDC hDC, LPRECT lprc, 
                                                     int nType, BOOL bScrollbarCtrl )
{   
   int nScrollSize = GetSystemMetrics( SM_CXHSCROLL );

   // The minimal thumb size depends on the system version
   // For Windows 98 minimal thumb size is a half of scrollbar size 
   // and for Windows NT is always 8 pixels regardless of system metrics. 
   // I really don't know why.
   int nMinThumbSize;
   if ( GetVersion() & 0x80000000 ) // Windows 98 code
      nMinThumbSize = nScrollSize / 2;
   else                    
      nMinThumbSize = 8;
   
   // Calculate the arrow rectangles
   RECT rc1 = *lprc, rc2 = *lprc;   
   if ( nType == SB_HORZ ) {
      if ( ( lprc->right - lprc->left ) < 2 * nScrollSize )
         nScrollSize = ( lprc->right - lprc->left ) / 2;

      rc1.right = lprc->left + nScrollSize;
      rc2.left = lprc->right - nScrollSize;
   } else { // SB_VERT
 
      if ( ( lprc->bottom - lprc->top ) < 2 * nScrollSize )
         nScrollSize = ( lprc->bottom - lprc->top ) / 2;

      rc1.bottom = lprc->top + nScrollSize;      
      rc2.top = lprc->bottom - nScrollSize;
   }   

   // Draw the scrollbar arrows
   DrawScrollbarThumb( hDC, &rc1 );
   DrawScrollbarThumb( hDC, &rc2 );

   // Disabled scrollbar never have a thumb
   if ( bScrollbarCtrl == TRUE && !m_bEnabled )
      return;      
 
   SCROLLINFO si;
   si.cbSize = sizeof( SCROLLINFO );
   si.fMask = SIF_ALL;     
   GetScrollInfo( m_hWnd, bScrollbarCtrl ? SB_CTL : nType, &si );

   // Calculate the size and position of the thumb   
   int nRange = si.nMax - si.nMin + 1;      
   if ( nRange ) {
      int nScrollArea = ( nType == SB_VERT ? ( lprc->bottom - lprc->top ) : ( lprc->right - lprc->left ) ) - 2 * nScrollSize;

      int nThumbSize; 
      if ( si.nPage == 0 ) // If nPage is not set then thumb has default size
         nThumbSize = GetSystemMetrics( SM_CXHTHUMB );
      else
         nThumbSize = max( MulDiv( si.nPage ,nScrollArea, nRange ), nMinThumbSize );

      if ( nThumbSize >= nScrollArea ) {
         nThumbSize = nScrollArea;
         if ( bScrollbarCtrl == FALSE )
            return;
      }

      int nThumbPos;
      if ( UINT( nRange ) == si.nPage ) {
         nThumbPos = 0;
         nThumbSize--;
      } else
         nThumbPos = MulDiv( si.nPos - si.nMin, nScrollArea - nThumbSize, nRange - si.nPage );

      if ( nType == SB_VERT ) {
         rc1.top += nScrollSize + nThumbPos;
         rc1.bottom = rc1.top + nThumbSize;
			} else { // SB_HORZ
         rc1.left += nScrollSize + nThumbPos;
         rc1.right = rc1.left + nThumbSize;
      }

      if ( nThumbSize <= nScrollArea ) // Don't draw the thumb when it's larger than the scroll area
         DrawScrollbarThumb( hDC, &rc1 );
   }   
}

/*
BOOL CPaintManager::CCMControl::IsFocused()
{ 
   return m_hWnd == GetFocus() ? TRUE : FALSE;
}
*/

//////////////////////////////////////////////////////////////////////////////
// CCMEdit class

void CPaintManager::CCMEdit::DrawControl( HDC hDC, LPRECT lprc )
{

  // Check if edit window has an associated up-down control.
  // If so draw a border around both controls
	HWND hWnd = GetNextWindow( m_hWnd, GW_HWNDNEXT );
	HWND hUpDown = NULL;
  if ( hWnd ) {
		TCHAR szBuf[MAX_CLASSNAME];
    // Retrieve window class name
    GetClassName( hWnd, szBuf, MAX_CLASSNAME );
		if ( lstrcmpi( szBuf, _T( "msctls_updown32" ) ) == 0 ) {   // Up-down is found
			DWORD dwStyle = GetWindowLong( hWnd, GWL_STYLE );
      if ( ( dwStyle & UDS_ALIGNRIGHT || dwStyle & UDS_ALIGNLEFT ) &&
						SendMessage( hWnd, UDM_GETBUDDY, 0, 0L ) == (LRESULT)m_hWnd )
				hUpDown = hWnd;
    }
  }
	
//   if ( GetWindowLong( m_hWnd, GWL_STYLE ) & ES_READONLY )
//      m_nState |= dsDisabled;
//   else
//      m_nState &= ~dsDisabled;

   CCMControl::DrawControl( hDC, lprc );
   if ( hUpDown ) {
	   ::InvalidateRect( hUpDown, NULL, TRUE );
	   ::UpdateWindow( hUpDown );
   }
}

//////////////////////////////////////////////////////////////////////////////
// CCMComboBox class

void CPaintManager::CCMComboBox::DrawControl( HDC hDC, LPRECT lprc )
{
   // First, draw borders around the control   
   CCMControl::DrawControl( hDC, lprc );

   // Now, we have to draw borders around the drop-down arrow
   RECT rc = *lprc;
   InflateRect( &rc, -2, -2 );
   rc.left = rc.right - GetSystemMetrics( SM_CXHSCROLL );
   
   if ( IsWindowEnabled( m_hWnd ) == TRUE ) {
      /*if ( m_nState & dsHoverMask )
         Draw3dBorder( hDC, rc, COLOR_3DFACE, COLOR_3DDKSHADOW,
                                COLOR_3DHIGHLIGHT, COLOR_3DSHADOW );
      else*/
         Draw3dBorder( hDC, rc, COLOR_3DHIGHLIGHT, COLOR_3DSHADOW,
                                COLOR_3DFACE, COLOR_3DFACE );
   } else
      Draw3dBorder( hDC, rc, COLOR_3DFACE, COLOR_3DFACE,
                             COLOR_3DFACE, COLOR_3DFACE );
}
//
//BOOL CPaintManager::CCMComboBox::IsFocused()
//{ 
//   // Special case for drop-down and simple ComboBoxes 
//   // which contain child edit control and focus always 
//   // goes to that edit window   
//   if ( ( GetWindowLong( m_hWnd, GWL_STYLE ) & 0x03 ) == CBS_DROPDOWN )
//   {
//      HWND hWnd = GetTopWindow( m_hWnd );
//      if ( hWnd && hWnd == GetFocus() )
//         return TRUE;
//   }
//
//   return CCMControl::IsFocused();
//}

LRESULT CPaintManager::CCMComboBox::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch ( uMsg )
   {      
      // Drop-down ComboBox receives neither WM_SETFOCUS nor WM_KILLFOCUS
      // Instead, it receives these next two messages
      case WM_LBUTTONUP:      // For kill focus
         if ( lParam == -1 )
					 return RePaintControl( uMsg, wParam, lParam );

      case WM_COMMAND:
         if ( HIWORD( wParam ) == EN_SETFOCUS )
					 return RePaintControl( uMsg, wParam, lParam );

	 }

	 return CCMControl::WindowProc( uMsg, wParam, lParam );
}

//////////////////////////////////////////////////////////////////////////////
// CCMDateTime class

void CPaintManager::CCMDateTime::DrawControl( HDC hDC, LPRECT lprc )
{
   if ( GetWindowLong( m_hWnd, GWL_STYLE ) & DTS_UPDOWN )
      CCMControl::DrawControl( hDC, lprc );
   else
      CCMComboBox::DrawControl( hDC, lprc );
}

//////////////////////////////////////////////////////////////////////////////
// CCMPushButton class
LRESULT CPaintManager::CCMPushButton::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
	switch ( uMsg )
	{ 
		case BM_SETSTYLE:
//			TraceMsg( uMsg, BM_SETSTYLE, _T("BM_SETSTYLE") );
		case BM_SETCHECK: 
//			TraceMsg( uMsg, BM_SETCHECK, _T("BM_SETCHECK") );
		case BM_SETSTATE:
//			TraceMsg( uMsg, BM_SETSTATE, _T("BM_SETSTATE") );
			//m_nLastState = 1000;
			return RePaintControl( uMsg, wParam, lParam, TRUE );
	}
	return CCMControl::WindowProc( uMsg, wParam, lParam);
}

void CPaintManager::CCMPushButton::DrawControl( HDC hDC, LPRECT lprc )
{
	// 0 - NORMAL
	// 1 - HOVER
	// 2 - PUSHED
	// 3 - DISABLED
	// 4 - SELECTED

	DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );
	DWORD dwState = (DWORD)SendMessage( m_hWnd, BM_GETSTATE, 0, 0 );
	UINT nState = 0;
	if ( !m_bEnabled ) {
		nState = 3; // DISABLED
	} else {
		if (  dwState & BST_PUSHED ) {
			nState = 2; // PUSHED
		} else {
			if ( m_bFocused || dwState & BST_FOCUS || dwStyle & BS_DEFPUSHBUTTON )
				nState = 4; // SELECTED
			if ( m_bHover )
				nState = 1; // HOVER
		}
	}

//	if ( 0 == _tcsicmp( m_pszText, _T("default button") ) )
//		TRACE2(" > state %d, %d\r\n", nState, m_nLastState );

	if ( m_nLastState == nState && ( 2 == m_nLastState || 1 == m_nLastState ) )
		;
	else
	{
		DrawCtrlBk( hDC, lprc );

		CRect rect( *lprc );

		// 1. Draw BK
		Button_DrawBkGnd( hDC, lprc, nState );
		
		// 2. Draw Focus
		InflateRect( &rect, -3, -3 );
		if ( m_bFocused )
			::DrawFocusRect( hDC, &rect );
		
		// 3. Draw text
		if ( 2 == nState )
			::OffsetRect( &rect, 1, 1 );
		
		DrawBtnText( hDC, m_pszText, &rect, dwStyle, !m_bEnabled, DT_CENTER, DT_SINGLELINE | DT_VCENTER );
	}
	m_nLastState = nState;
}


//////////////////////////////////////////////////////////////////////////////
// CCMCheckBox class

void CPaintManager::CCMCheckBox::DrawControl( HDC hDC, LPRECT lprc )
{
	DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );
	if ( dwStyle & BS_PUSHLIKE ) {

		// 0 - NORMAL
		// 1 - HOVER
		// 2 - PUSHED
		// 3 - DISABLED

		DWORD dwStyle = ::GetWindowLong( m_hWnd, GWL_STYLE );

		UINT nState = 0;
		if ( !m_bEnabled ) {
			nState = 3;
		} else {
			if ( SendMessage( m_hWnd, BM_GETSTATE, 0, 0 ) & ( BST_CHECKED | BST_PUSHED )) {
				nState = 2;
			} else if ( m_bHover ) {
				nState = 1;
			}
		}

		if ( m_nLastState == nState && ( 1 == m_nLastState || 2 == m_nLastState ) )
			;
		else {
			// 1. Draw BK
			Button_DrawBkGnd( hDC, lprc, nState );
			
			// 2. Draw Focus
			::InflateRect( lprc, -3, -3 );
			if ( m_bFocused )
				::DrawFocusRect( hDC, lprc );
			
			// 3. Draw text
			if ( 2 == nState )
				::OffsetRect( lprc, 1, 1 );
			
			DrawBtnText( hDC, m_pszText, lprc, dwStyle, !m_bEnabled, DT_CENTER, DT_SINGLELINE | DT_VCENTER  );
		}
		m_nLastState = nState;
   
   } else { // standard		
		// Control State
		// 0 - BST_UNCHEKED | NORMAL
		// 1 - BST_UNCHEKED | HOVER
		// 2 - BST_UNCHEKED | PUSHED
		// 3 - BST_UNCHEKED | DISABLED
		// 4 - BST_CHEKED | ENABLED
		// 5 - BST_CHEKED | HOVER
		// 6 - BST_CHEKED | PUSHED
		// 7 - BST_CHEKED | DISABLED
		// 8 - BST_INDETERMINATE | ENABLED
		// 9 - BST_INDETERMINATE | HOVER
		// 10 - BST_INDETERMINATE | PUSHED
		// 11 - BST_INDETERMINATE | DISABLED

		DWORD dwCtrlState = 0;
		if ( ::SendMessage( m_hWnd, BM_GETSTATE, 0, 0) & BST_PUSHED ) {
			dwCtrlState = 2;
		} else if ( m_bHover ) {
			dwCtrlState = 1;
		} else if ( !IsWindowEnabled( m_hWnd ) ) {
			dwCtrlState = 3;
		}

		DWORD dwState = dwCtrlState + DWORD( 4 * ::SendMessage( m_hWnd, BM_GETCHECK, 0, 0) );


		if ( ( m_nLastState == dwState ) && 
			( 1 == m_nLastState || 2 == m_nLastState ||
			  5 == m_nLastState || 6 == m_nLastState ||
			  9 == m_nLastState || 10 == m_nLastState ) )
			;
		else {
			DrawCtrlBk( hDC, lprc );

			CRect rc( *lprc );
			if( BS_LEFTTEXT & dwStyle ) {
				rc.left = lprc->right - 16;
			} else {
				rc.right = lprc->left + 16;
			}
			const int nCheckHeight = 16;
			rc.top = rc.top + ( rc.Height() - nCheckHeight ) / 2;
			rc.bottom = rc.top + nCheckHeight;

			CheckBox_DrawBkGnd( hDC, &rc, dwState );

			CRect rcText( *lprc );
			if( BS_LEFTTEXT & dwStyle ) {
				rcText.right = lprc->right - 18;
			} else {
				rcText.left = lprc->left + 18;
			}

			// focus rect
			if ( m_bFocused )
				::DrawFocusRect( hDC, &rcText );
			rcText.DeflateRect( 2, 2 );
			DrawBtnText( hDC, m_pszText, &rcText, dwStyle, !m_bEnabled, DT_LEFT, DT_SINGLELINE | DT_VCENTER  );
		}
		m_nLastState = dwState ;
   }
}

//////////////////////////////////////////////////////////////////////////////
// CCMRadioButton class

/*
void CPaintManager::CCMRadioButton::DrawFrame( POINT* ptArr, int nColor, 
                                                     HDC hDC, int xOff, int yOff )
{
   for ( int i = 0; ; i++ )
   {
      if ( !ptArr[i].x && !ptArr[i].y )
         return;

      SetPixel( hDC, ptArr[i].x + xOff, ptArr[i].y + yOff, GetSysColor( nColor ) );
   }
}
*/

void CPaintManager::CCMRadioButton::DrawControl( HDC hDC, LPRECT lprc )
{  
	DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );

	if ( dwStyle & BS_PUSHLIKE ) {
		// 0 - NORMAL
		// 1 - HOVER
		// 2 - PUSHED
		// 3 - DISABLED

		UINT nState = 0;
		if ( !m_bEnabled ) {
			nState = 3;
		} else {
			if ( SendMessage( m_hWnd, BM_GETSTATE, 0, 0 ) & ( BST_CHECKED | BST_PUSHED )) {
				nState = 2;
			} else if ( m_bHover ) {
				nState = 1;
			}
		}

		if ( m_nLastState == nState && ( 1 == m_nLastState || 2 == m_nLastState ) )
			;
		else {

			DrawCtrlBk( hDC, lprc );

			// 1. Draw BK
			Button_DrawBkGnd( hDC, lprc, nState );
			
			// 2. Draw Focus
			InflateRect( lprc, -3, -3 );
			if ( m_bFocused )
				::DrawFocusRect( hDC, lprc );
			
			// 3. Draw text
			if ( 2 == nState )
				::OffsetRect( lprc, 1, 1 );
			DrawBtnText( hDC, m_pszText, lprc, dwStyle, !m_bEnabled, DT_CENTER, DT_SINGLELINE | DT_VCENTER  );
		}
		m_nLastState = nState;
	} else {

		// Control State
		// 0 - BST_UNCHEKED | NORMAL
		// 1 - BST_UNCHEKED | HOVER
		// 2 - BST_UNCHEKED | PUSHED
		// 3 - BST_UNCHEKED | DISABLED
		// 4 - BST_CHEKED | ENABLED
		// 5 - BST_CHEKED | HOVER
		// 6 - BST_CHEKED | PUSHED
		// 7 - BST_CHEKED | DISABLED


		DWORD dwCtrlState = 0;
		if ( ::SendMessage( m_hWnd, BM_GETSTATE, 0, 0) & BST_PUSHED ) {
			dwCtrlState = 2;
		} else if ( m_bHover ) {
			dwCtrlState = 1;
		} else if ( !m_bEnabled ) {
			dwCtrlState = 3;
		}

		DWORD dwState = dwCtrlState + DWORD( 4 * ::SendMessage( m_hWnd, BM_GETCHECK, 0, 0) );
		
		if ( ( m_nLastState == dwState ) && 
			( 1 == m_nLastState || 2 == m_nLastState ||
			  5 == m_nLastState || 6 == m_nLastState ) ) {
			;
		} else  {
			DrawCtrlBk( hDC, lprc );

			CRect rc( *lprc );
			if( BS_LEFTTEXT & dwStyle ) {
				rc.left = lprc->right - 16;
			} else {
				rc.right = lprc->left + 16;
			}
			const int nCheckHeight = 16;
			rc.top = rc.top + ( rc.Height() - nCheckHeight ) / 2;
			rc.bottom = rc.top + nCheckHeight;

			RadioButton_DrawBkGnd( hDC, &rc, dwState );

			CRect rcText( *lprc );
			if( BS_LEFTTEXT & dwStyle ) {
				rcText.right = lprc->right - 18;
			} else {
				rcText.left = lprc->left + 18;
			}

			// focus rect
			if ( m_bFocused )
				::DrawFocusRect( hDC, &rcText );
			rcText.DeflateRect( 2, 2 );

			// Text
			DrawBtnText( hDC, m_pszText, &rcText, dwStyle, !m_bEnabled, DT_LEFT, DT_SINGLELINE | DT_VCENTER  );
		}
		m_nLastState = dwState ;
	}
}

//////////////////////////////////////////////////////////////////////////////
// CCMUpDown class

LRESULT CPaintManager::CCMUpDown::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
	switch ( uMsg )
	{      
		case WM_LBUTTONDOWN:
			m_bLButtonDown = TRUE;
			m_ptCursor.x = LOWORD( lParam );
			m_ptCursor.y = HIWORD( lParam );
			break;

		case WM_LBUTTONUP:
			m_bLButtonDown = FALSE;
			break;

   }

	return CCMControl::WindowProc( uMsg, wParam, lParam );
}

/*
void CPaintManager::CCMUpDown::DrawButton( HDC hDC, LPRECT lprc, UINT nType, UINT nState )
{
	BOOL bPushed = m_bLButtonDown && CRect(*lprc).PtInRect( m_ptCursor );
	FillSolidRect( hDC,  lprc, ::GetSysColor( COLOR_BTNFACE ) );
	if ( !bDisabled ) {
		if( bPushed ) {
			Spin_DrawBkGnd( hDC, 
//			themeData.DrawUpDown( CDC::FromHandle( hDC ), *lprc, 0, FALSE, FALSE );
			lprc->top+=2;
			lprc->left+=2;
		} else {
			themeData.DrawUpDown( CDC::FromHandle( hDC ), *lprc, 0, FALSE, FALSE );
			lprc->top++;
			lprc->left++;
		}
	} else {
		Draw3dBorder( hDC, *lprc, COLOR_3DHIGHLIGHT, COLOR_3DSHADOW, COLOR_3DFACE, COLOR_3DFACE );
		lprc->top++;
		lprc->left++;
	}
	::DrawText( hDC, lpszText, 1, lprc,  DT_CENTER | DT_VCENTER | DT_SINGLELINE );
}
*/

void CPaintManager::CCMUpDown::DrawControl( HDC hDC, LPRECT lprc )
{  
   RECT rc = *lprc;
//   CCMControl* pCtl = NULL;

   DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );
   HWND hWnd = (HWND)SendMessage( m_hWnd, UDM_GETBUDDY, 0, 0L );
   
   BOOL bBodyActive = hWnd ? ::IsWindowEnabled( hWnd ) : TRUE;
   if ( hWnd && ( dwStyle & UDS_ALIGNRIGHT || dwStyle & UDS_ALIGNLEFT ) )
   {
		// border
		HPEN hOldPen = (HPEN)::SelectObject( hDC, bBodyActive ? m_pnActiveBorder : m_pnInactiveBorder);
		
		if ( dwStyle & UDS_ALIGNLEFT ) {
			MoveToEx(hDC, rc.right, 0, NULL);
			LineTo( hDC, 0, 0 );
			LineTo( hDC, 0, rc.bottom-1 );
			LineTo( hDC, rc.right, rc.bottom-1 );
/*			::SelectObject( hDC, bBodyActive ? m_pnWindow : m_pnDisabledWindow);
			MoveToEx(hDC, rc.right-1, 1, NULL);
			LineTo( hDC, 1, 1 );
			LineTo( hDC, 1, rc.bottom-2 );
			LineTo( hDC, rc.right, rc.bottom-2 );

			rc.left += 2; */
			rc.left++;
		} else { // UDS_ALIGNRIGHT
			MoveToEx(hDC, rc.left, 0, NULL);
			LineTo( hDC, rc.right - 1, 0 );
			LineTo( hDC, rc.right - 1, rc.bottom -1 );
			LineTo( hDC, rc.left - 1, rc.bottom - 1 );
/*			::SelectObject( hDC, bBodyActive ? m_pnWindow : m_pnDisabledWindow);
			MoveToEx(hDC, rc.left, 1, NULL);
			LineTo( hDC, rc.right - 2, 1 );
			LineTo( hDC, rc.right - 2, rc.bottom -2 );
			LineTo( hDC, rc.left - 2, rc.bottom - 2 );
			rc.right -= 2; */
			rc.right--;
		}
//		rc.top += 2;
//		rc.bottom -= 2;
		rc.top++;
		rc.bottom--;
		
		FillSolidRect( hDC, &rc, GetSysColor( bBodyActive ? COLOR_WINDOW : COLOR_BTNFACE ) );
/*		// Разделитель кнопок
		RECT r = rc;
		if ( dwStyle & UDS_HORZ ) {
			r.right = r.left + ( rc.right - rc.left ) / 2;
			MoveToEx(hDC, r.right, r.top, NULL);
			LineTo( hDC, r.right, r.bottom );
		} else {
			r.bottom = r.top + ( rc.bottom - rc.top ) / 2;
			MoveToEx(hDC, r.left, r.bottom, NULL);
			LineTo( hDC, r.right, r.bottom );
		}
*/
		::SelectObject( hDC, hOldPen );
	 } 

 
   // Draw buttons
	RECT r = rc;
	BOOL bDisabled = !bBodyActive || !m_bEnabled;
	if ( bDisabled ) 
		::SetTextColor( hDC, ::GetSysColor( COLOR_3DSHADOW ) );


	if ( dwStyle & UDS_HORZ ) {
		
		int nSize = ( rc.right - rc.left ) / 2;
		
		r.right = r.left + nSize;
		//Spin_DrawBkGnd( hDC, 2, &r, 0 );
		FillSolidRect( hDC, &r, GetSysColor( COLOR_BTNFACE ) );

		r.left = rc.right - nSize;
		r.right = rc.right;
		// Spin_DrawBkGnd( hDC, 3, &r, 0 );
		FillSolidRect( hDC, &r, GetSysColor( COLOR_BTNFACE ) );

	} else {
		
		int nSize = ( rc.bottom - rc.top ) / 2;
		
		r.bottom = r.top + nSize;
		Spin_DrawBkGnd( hDC, 0, &r, 0 );
		//FillSolidRect( hDC, &r, GetSysColor( COLOR_BTNFACE ) );
	
		r.top = rc.bottom - nSize;
		r.bottom = rc.bottom;
		// Spin_DrawBkGnd( hDC, 1, &r, 0 );
		FillSolidRect( hDC, &r, GetSysColor( COLOR_BTNFACE ) );

	}


//   if ( pCtl == NULL )  // Get parent (e.g. for datetime with up-down controls)
//   {
//      hWnd = GetParent( m_hWnd );
//      g_ctrlManager.m_ctrlMap.Lookup( hWnd, (void*&)pCtl );
//   }

 /*
   if ( pCtl && IsWindowEnabled( hWnd ) )   // Redraw parent or buddy if neccesary
   {
      if ( m_nState & dsHoverMask )
         pCtl->SetState( 0, dsHover );
      else
         pCtl->SetState( dsHover, 0 );
   }
	 */
}

//////////////////////////////////////////////////////////////////////////////
// CCMEditCombo class

HDC CPaintManager::CCMEditCombo::BeginDraw( LPRECT lprc )
{
	 // First time initialization
   if ( NULL == m_pszText ) {
	   InitText();
	   m_bEnabled = IsWindowEnabled( m_hWnd );
	   m_bFocused = ( ::GetFocus() == m_hWnd );
   }
   GetWindowRect( m_hWnd, lprc );
   InflateRect( lprc, 3, 3 );
   OffsetRect( lprc, -lprc->left, -lprc->top );
   // Draw onto that DC that is most suitable for given class   
   return GetWindowDC( GetParent( m_hWnd ) );
}

//////////////////////////////////////////////////////////////////////////////
// CCMScrollBar class

void CPaintManager::CCMScrollBar::DrawControl( HDC hDC, LPRECT lprc )
{  
   DrawScrollBar( hDC, lprc, 
             ( GetWindowLong( m_hWnd, GWL_STYLE ) & SBS_VERT ) ? SB_VERT : SB_HORZ,
             TRUE );
}

LRESULT CPaintManager::CCMScrollBar::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch ( uMsg )
   {      
      // Scrollbar messages
      case SBM_SETPOS:
         if ( !lParam )  // redraw flag
            break;

      case SBM_SETSCROLLINFO:      
         if ( !wParam )  // redraw flag
            break;

      case SBM_SETRANGEREDRAW:
				 RePaintControl( uMsg, wParam, lParam );
         return 0;

      default:
         return CCMControl::WindowProc( uMsg, wParam, lParam );
   }

   return CallWindowProc( m_oldWndProc, m_hWnd, uMsg, wParam, lParam );
}

//////////////////////////////////////////////////////////////////////////////
// CCMHeaderCtrl class

void CPaintManager::CCMHeaderCtrl::DrawButton( HDC hDC, const RECT& rc, int nState )
{
/*   if ( nState & dsHoverMask )
      Draw3dBorder( hDC, rc, COLOR_3DHIGHLIGHT, COLOR_3DDKSHADOW,
                             COLOR_3DLIGHT, COLOR_3DSHADOW );
   else
      Draw3dBorder( hDC, rc, COLOR_3DHIGHLIGHT, COLOR_3DSHADOW,
                             COLOR_3DFACE, COLOR_3DFACE );      
														 */
}

void CPaintManager::CCMHeaderCtrl::DrawControl( HDC hDC, LPRECT lprc )
{
   int nOldItem = m_nOldItem;
   m_nOldItem = -1;

   RECT rc;
   POINT point;
   GetCursorPos( &point );

   // This code fails if we will have standalone header control but such cases are rare...
   HWND hWnd = GetParent( m_hWnd ); 
   GetClientRect( GetParent( m_hWnd ), &rc );
   ScreenToClient( GetParent( m_hWnd ), &point );
   // Test if mouse pointer is within the client area of the list control
   BOOL bInView = PtInRect( &rc, point );    

   GetClientRect( m_hWnd, &rc );
   rc.right = 0;
   GetCursorPos( &point );
   ScreenToClient( m_hWnd, &point );
   hDC = GetDC( m_hWnd );
   
   int nState;
   int nCount = (int)SendMessage( m_hWnd, HDM_GETITEMCOUNT, 0, 0L );   

   for ( int i = 0; i < nCount; i++ )
   {
    #if (_WIN32_IE >= 0x0300)
      HDITEM hi;
      hi.mask = HDI_ORDER;
      SendMessage( m_hWnd, HDM_GETITEM, i, (LPARAM)&hi );
      SendMessage( m_hWnd, HDM_GETITEMRECT, hi.iOrder, (LPARAM)&rc );
    #else
      SendMessage( m_hWnd, HDM_GETITEMRECT, i, (LPARAM)&rc );
    #endif
      nState = 0;
//      if ( bInView & PtInRect( &rc, point ) )
//      {
//         nState = dsHover;
//       #if (_WIN32_IE >= 0x0300)
       //  m_nOldItem = hi.iOrder;
       //#else
       //  m_nOldItem = i;
       //#endif
      //}
      DrawButton( hDC, rc, nState );
   }

   int l = rc.right;
   GetClientRect( m_hWnd, &rc );
   rc.left = l;
   DrawButton( hDC, rc, 0 );

	 /*
   // If header is a child of ListView, redraw the list so 
   // it will indicate proper state      
   CCMControl* pCtl;   
   if ( g_ctrlManager.m_ctrlMap.Lookup( hWnd, (void*&)pCtl ) )
   {
      if ( m_nOldItem >= 0 )
         pCtl->SetState( 0, dsHover );
      else if ( nOldItem >= 0 )
         pCtl->SetState( dsHover, 0 );
   }
	*/

   ReleaseDC( m_hWnd, hDC );
}

BOOL CPaintManager::CCMHeaderCtrl::NeedRedraw( const POINT& point )
{
   RECT rc;
   GetClientRect( m_hWnd, &rc );
   rc.right = 0;

   POINT pt = point;
   ScreenToClient( m_hWnd, &pt );

   int nItem = -1;
   int nCount = (int)SendMessage( m_hWnd, HDM_GETITEMCOUNT, 0, 0L );

   for ( int i = 0; i < nCount; i++ )
   {
      HDITEM hi;
      hi.mask = HDI_WIDTH;      
      SendMessage( m_hWnd, HDM_GETITEM, i, (LPARAM)&hi );
      rc.left = rc.right;
      rc.right = rc.left + hi.cxy;
      if ( PtInRect( &rc, pt ) )
      {
         nItem = i;
         break;
      }
   }

   if ( m_hWnd != m_hWndOld || ( m_hWnd == m_hWndOld && m_nOldItem != nItem ) )
      return TRUE;
   return FALSE;
}

//////////////////////////////////////////////////////////////////////////////
// CCMTrackbar class

void CPaintManager::CCMTrackbar::DrawThumb( HDC hDC, LPRECT lprc )
{
/*   DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );

   if ( dwStyle & TBS_BOTH )
   {
      FillSolidRect( hDC, lprc, GetSysColor( COLOR_3DFACE ) );
      if ( m_bHover )
         Draw3dBorder( hDC, lprc, COLOR_3DHIGHLIGHT, COLOR_3DDKSHADOW,
                                COLOR_3DLIGHT, COLOR_3DSHADOW );
      else
         Draw3dBorder( hDC, lprc, COLOR_3DHIGHLIGHT, COLOR_3DSHADOW,
                                COLOR_3DFACE, COLOR_3DFACE );      
      return;
   }
   
   HPEN penHighlight = CreatePen( PS_SOLID, 1, GetSysColor( COLOR_3DHIGHLIGHT ) );
   HPEN penLight = CreatePen( PS_SOLID, 1, GetSysColor( m_bHover ? COLOR_3DLIGHT : COLOR_3DFACE ) );
   HPEN penDkShadow = CreatePen( PS_SOLID, 1, GetSysColor( m_bHover ? COLOR_3DDKSHADOW : COLOR_3DSHADOW ) );
   HPEN penShadow = CreatePen( PS_SOLID, 1, GetSysColor( m_bHover ? COLOR_3DSHADOW : COLOR_3DFACE ) );

   int n;
   if ( dwStyle & TBS_VERT )
   {
      if ( dwStyle & TBS_LEFT )
      {    
         n = ( rc.bottom - rc.top ) / 2 + 1;

         FillSolidRect( hDC, &rc, GetSysColor( COLOR_3DFACE ) );
      
         HPEN hOldPen = (HPEN)SelectObject( hDC, penHighlight );
         MoveToEx( hDC, rc.right - 2, rc.top, NULL );
         LineTo( hDC, rc.left + n - 1, rc.top );
         LineTo( hDC, rc.left, rc.top + n - 1 );         
         
         SelectObject( hDC, penDkShadow );
         LineTo( hDC, rc.left + n - 1, rc.bottom - 1 );
         LineTo( hDC, rc.right - 1, rc.bottom - 1 );
         LineTo( hDC, rc.right - 1, rc.top - 1 );

         SelectObject( hDC, penLight );
         MoveToEx( hDC, rc.right - 3, rc.top + 1, NULL );
         LineTo( hDC, rc.left + n - 1, rc.top + 1 );
         LineTo( hDC, rc.left + 1, rc.top + n - 1 );

         SelectObject( hDC, penShadow );         
         LineTo( hDC, rc.left + n - 1, rc.bottom - 2 );
         LineTo( hDC, rc.right - 2, rc.bottom - 2 );
         LineTo( hDC, rc.right - 2, rc.top );

         SelectObject( hDC, hOldPen );         
      }
      else // TBS_RIGHT
      {
         n = ( rc.bottom - rc.top ) / 2 + 1;

         FillSolidRect( hDC, &rc, GetSysColor( COLOR_3DFACE ) );
      
         HPEN hOldPen = (HPEN)SelectObject( hDC, penHighlight );
         MoveToEx( hDC, rc.left, rc.bottom - 2, NULL );
         LineTo( hDC, rc.left, rc.top );
         LineTo( hDC, rc.right - n, rc.top );
         LineTo( hDC, rc.right - 1, rc.top + n - 1 );

         SelectObject( hDC, penDkShadow );          
         MoveToEx( hDC, rc.left, rc.bottom - 1, NULL );
         LineTo( hDC, rc.right - n, rc.bottom - 1 );
         LineTo( hDC, rc.right, rc.top + n - 2 );         

         SelectObject( hDC, penLight );
         MoveToEx( hDC, rc.left + 1, rc.bottom - 3, NULL );
         LineTo( hDC, rc.left + 1, rc.top + 1 );
         LineTo( hDC, rc.right - n, rc.top + 1 );
         LineTo( hDC, rc.right - 2, rc.top + n - 1 );

         SelectObject( hDC, penShadow );
         MoveToEx( hDC, rc.left + 1, rc.bottom - 2, NULL );
         LineTo( hDC, rc.right - n, rc.bottom - 2 );
         LineTo( hDC, rc.right - 1, rc.top + n - 2 );         

         SelectObject( hDC, hOldPen );
      }      
   }
   else
   {
      if ( dwStyle & TBS_TOP )
      {      
         n = ( rc.right - rc.left ) / 2 + 1;

         FillSolidRect( hDC, &rc, GetSysColor( COLOR_3DFACE ) );
      
         HPEN hOldPen = (HPEN)SelectObject( hDC, penHighlight );
         MoveToEx( hDC, rc.left + n - 2, rc.top + 1, NULL );
         LineTo( hDC, rc.left, rc.top + n - 1 );
         LineTo( hDC, rc.left, rc.bottom - 1 );

         SelectObject( hDC, penDkShadow );          
         LineTo( hDC, rc.right - 1, rc.bottom - 1 );
         LineTo( hDC, rc.right - 1, rc.top + n - 1 );
         LineTo( hDC, rc.left + n - 2, rc.top - 1 );

         SelectObject( hDC, penLight );
         MoveToEx( hDC, rc.left + n - 2, rc.top + 2, NULL );
         LineTo( hDC, rc.left + 1, rc.top + n - 1 );
         LineTo( hDC, rc.left + 1, rc.bottom - 2 );
     
         SelectObject( hDC, penShadow );
         LineTo( hDC, rc.right - 2, rc.bottom - 2 );
         LineTo( hDC, rc.right - 2, rc.top + n - 1 );
         LineTo( hDC, rc.left + n - 2, rc.top );

         SelectObject( hDC, hOldPen );
      }
      else // TBS_BOTTOM
      {
         n = ( rc.right - rc.left ) / 2 + 1;

         FillSolidRect( hDC, &rc, GetSysColor( COLOR_3DFACE ) );
               
         HPEN hOldPen = (HPEN)SelectObject( hDC, penHighlight );
         MoveToEx( hDC, rc.left + n - 2, rc.bottom - 2, NULL );
         LineTo( hDC, rc.left, rc.bottom - n );
         LineTo( hDC, rc.left, rc.top );
         LineTo( hDC, rc.right - 1, rc.top );

         SelectObject( hDC, penDkShadow );          
         LineTo( hDC, rc.right - 1, rc.bottom - n );
         LineTo( hDC, rc.left + n - 2, rc.bottom );

         SelectObject( hDC, penLight );
         MoveToEx( hDC, rc.left + n - 2, rc.bottom - 3, NULL );
         LineTo( hDC, rc.left + 1, rc.bottom - n );
         LineTo( hDC, rc.left + 1, rc.top + 1 );
         LineTo( hDC, rc.right - 2, rc.top + 1 );
     
         SelectObject( hDC, penShadow );
         LineTo( hDC, rc.right - 2, rc.bottom - n );
         LineTo( hDC, rc.left + n - 2, rc.bottom - 1 );

         SelectObject( hDC, hOldPen );
      }
   }

   DeleteObject( penHighlight );
   DeleteObject( penLight );
   DeleteObject( penDkShadow );
   DeleteObject( penShadow );
	 */
}

void CPaintManager::CCMTrackbar::DrawControl( HDC hDC, LPRECT lprc )
{  
   hDC = GetDC( m_hWnd );
   DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );
   
   RECT rc;   
   SendMessage( m_hWnd, TBM_GETCHANNELRECT, 0, (LPARAM)&rc );   

   // BUG!: Windows incorrectly calculates the channel rectangle for
   // sliders with TBS_VERT style, so we have to calculate the rectangle
   // in different manner. This bug appears on all Windows platforms!
   if ( dwStyle & TBS_VERT )
   {  
      int w = ( rc.right - rc.left );
      int h = ( rc.bottom - rc.top );
      rc.top = rc.left;
      rc.bottom = rc.left + w;

      RECT r;
      SendMessage( m_hWnd, TBM_GETTHUMBRECT, 0, (LPARAM)&r );         

      rc.left = r.left + ( ( r.right - r.left ) / 2 + 1 ) - h / 2;
      if ( dwStyle & TBS_LEFT )
         ;
      else if ( dwStyle & TBS_BOTH )
         rc.left -= 1;
      else
         rc.left -= 2;

      rc.right = rc.left + h;
   }

   // Draw the channel rect
   if ( m_bHover )
      Draw3dBorder( hDC, rc, COLOR_3DSHADOW, COLOR_3DHIGHLIGHT,                         
                             COLOR_3DDKSHADOW, COLOR_3DLIGHT );
   else
      Draw3dBorder( hDC, rc, COLOR_3DSHADOW, COLOR_3DHIGHLIGHT,                         
                             COLOR_3DFACE, COLOR_3DFACE );
   
   // Draw the slider thumb
   if ( !( dwStyle & TBS_NOTHUMB ) )
   {
      SetRectEmpty( &rc );
      SendMessage( m_hWnd, TBM_GETTHUMBRECT, 0, (LPARAM)&rc );
      DrawThumb( hDC, &rc );
   }

   ReleaseDC( m_hWnd, hDC );
}

//////////////////////////////////////////////////////////////////////////////
// CCMToolbar class

void CPaintManager::CCMToolbar::DrawButton( HDC hDC, LPRECT lprc, int nState )
{
/*   if ( nState & bsChecked )
   {
      if ( m_bHover )
         Draw3dBorder( hDC, rc,
                       COLOR_3DDKSHADOW, COLOR_3DHIGHLIGHT,
                       COLOR_3DSHADOW, COLOR_3DSHADOW );
      else
         Draw3dBorder( hDC, rc,
                       COLOR_3DSHADOW, COLOR_3DHIGHLIGHT,
                       COLOR_3DFACE, COLOR_3DFACE );
   }
   else
   {
      if ( nState & bsHover )
         Draw3dBorder( hDC, rc,
                       COLOR_3DHIGHLIGHT, COLOR_3DDKSHADOW,
                       COLOR_3DLIGHT, COLOR_3DSHADOW );
      else
         Draw3dBorder( hDC, rc,
                       COLOR_3DHIGHLIGHT, COLOR_3DSHADOW,
                       COLOR_3DFACE, COLOR_3DFACE );
   }
*/
}

void CPaintManager::CCMToolbar::DrawControl( HDC hDC, LPRECT lprc )
{
/*   if ( GetWindowLong( m_hWnd, GWL_STYLE ) & TBSTYLE_FLAT ) // Skip flat toolbars
      return;
      
   int nCount = SendMessage( m_hWnd, TB_BUTTONCOUNT, 0, 0L );

   hDC = GetDC( m_hWnd );  // We will draw toolbar buttons on the client DC

   POINT point;
   GetCursorPos( &point );
   ScreenToClient( m_hWnd, &point );
   
   m_nOldItem = -1;
   int nState = 0;
           
   for ( int i = 0; i < nCount; i++ )
   {     
      RECT rc;       
      TBBUTTON ti;
      SendMessage( m_hWnd, TB_GETBUTTON, i, (LPARAM)&ti );
      SendMessage( m_hWnd, TB_GETITEMRECT, i, (LPARAM)&rc );

      if ( !( ti.fsStyle & TBSTYLE_SEP ) )
      {                  
         nState = ( ti.fsState & TBSTATE_CHECKED ) ? bsChecked : bsNormal;
         if ( PtInRect( &rc, point ) == TRUE )
         {
            if ( ti.fsState & TBSTATE_ENABLED )
               nState |= bsHover;
            m_nOldItem = i;
         }         
         DrawButton( hDC, rc, nState );
      }
   }
      
   ReleaseDC( m_hWnd, hDC );
	 */
}

BOOL CPaintManager::CCMToolbar::NeedRedraw( const POINT& point )
{
   int nCount = (int) SendMessage( m_hWnd, TB_BUTTONCOUNT, 0, 0L );

   POINT pt = point;
   ScreenToClient( m_hWnd, &pt );
   int nItem = -1;                
   for ( int i = 0; i < nCount; i++ )
   {            
      TBBUTTON ti;      
      SendMessage( m_hWnd, TB_GETBUTTON, i, (LPARAM)&ti );

      if ( !( ti.fsStyle & TBSTYLE_SEP ) )
      {
         RECT rc;         
         SendMessage( m_hWnd, TB_GETITEMRECT, i, (LPARAM)&rc );
         if ( PtInRect( &rc, pt ) )
         {
            nItem = i;
            break;
         }
      }
   }

   if ( m_hWnd != m_hWndOld || ( m_hWnd == m_hWndOld && m_nOldItem != nItem ) )
      return TRUE;
   return FALSE;
}

LRESULT CPaintManager::CCMToolbar::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch ( uMsg )
   {  
      case WM_PAINT:
      case WM_NCPAINT:
         m_nOldItem = -1;   // Redraw the whole toolbar unconditionally

      default:
         return CCMControl::WindowProc( uMsg, wParam, lParam );
   }
}

//////////////////////////////////////////////////////////////////////////////
// CCMTabControl class

void CPaintManager::CCMTabControl::DrawTab( HDC hDC, const RECT& rect, 
                                                   int nItem, int nState )
{
/*   RECT rc = rect;
   int nCurSel = SendMessage( m_hWnd, TCM_GETCURSEL, 0, 0L );
   if ( nCurSel == -1 )
      nCurSel = -2;

   switch ( GetOrientation() )
   {
      case tabLeft:
         if ( nState & bsChecked )
         {
            rc.top -= 2;
            rc.bottom += 2;
            rc.left -= 2;
            rc.right += 1;
         }

         if ( nState & bsHover )
         {
            if ( nCurSel != nItem - 1 )
            {
               FillSolidRect( hDC, rc.left+2, rc.top, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DHIGHLIGHT ) );
               FillSolidRect( hDC, rc.left+2, rc.top+1, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DLIGHT ) );
               SetPixel( hDC, rc.left+1, rc.top+1, GetSysColor( COLOR_3DHIGHLIGHT ) );
            }

            FillSolidRect( hDC, rc.left, rc.top+2, 1, rc.bottom-rc.top-4, GetSysColor( COLOR_3DHIGHLIGHT ) );
            FillSolidRect( hDC, rc.left+1, rc.top+2, 1, rc.bottom-rc.top-4, GetSysColor( COLOR_3DLIGHT ) );

            if ( nCurSel != nItem + 1 )
            {
               FillSolidRect( hDC, rc.left+2, rc.bottom-1, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DDKSHADOW ) );
               FillSolidRect( hDC, rc.left+2, rc.bottom-2, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DSHADOW ) );                        
               SetPixel( hDC, rc.left+1, rc.bottom-2, GetSysColor( COLOR_3DDKSHADOW ) );
            }
         }
         else
         {
            if ( nCurSel != nItem - 1 )
            {
               FillSolidRect( hDC, rc.left+2, rc.top, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DHIGHLIGHT ) );
               FillSolidRect( hDC, rc.left+2, rc.top+1, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DFACE ) );
               SetPixel( hDC, rc.left+1, rc.top+1, GetSysColor( COLOR_3DHIGHLIGHT ) );
            }

            FillSolidRect( hDC, rc.left, rc.top+2, 1, rc.bottom-rc.top-4, GetSysColor( COLOR_3DHIGHLIGHT ) );
            FillSolidRect( hDC, rc.left+1, rc.top+2, 1, rc.bottom-rc.top-4, GetSysColor( COLOR_3DFACE ) );

            if ( nCurSel != nItem + 1 )
            {
               FillSolidRect( hDC, rc.left+2, rc.bottom-1, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DSHADOW ) );
               FillSolidRect( hDC, rc.left+2, rc.bottom-2, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DFACE ) );                        
               SetPixel( hDC, rc.left+1, rc.bottom-2, GetSysColor( COLOR_3DSHADOW ) );
            }
         }       
         break;

      case tabTop:
         if ( nState & bsChecked )
         {
            rc.top -= 2;
            rc.bottom += 1;
            rc.left -= 2;
            rc.right += 2;
         }

         if ( nState & bsHover )
         {
            if ( nCurSel != nItem - 1 )
            {
               FillSolidRect( hDC, rc.left, rc.top+2, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DHIGHLIGHT ) );
               FillSolidRect( hDC, rc.left+1, rc.top+2, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DLIGHT ) );
               SetPixel( hDC, rc.left+1, rc.top+1, GetSysColor( COLOR_3DHIGHLIGHT ) );
            }

            FillSolidRect( hDC, rc.left+2, rc.top, rc.right-rc.left-4, 1, GetSysColor( COLOR_3DHIGHLIGHT ) );
            FillSolidRect( hDC, rc.left+2, rc.top+1, rc.right-rc.left-4, 1, GetSysColor( COLOR_3DLIGHT ) );

            if ( nCurSel != nItem + 1 )
            {
               FillSolidRect( hDC, rc.right-1, rc.top+2, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DDKSHADOW ) );
               FillSolidRect( hDC, rc.right-2, rc.top+2, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DSHADOW ) );            
               SetPixel( hDC, rc.right-2, rc.top+1, GetSysColor( COLOR_3DDKSHADOW ) );
            }                       
         }
         else
         {
            if ( nCurSel != nItem - 1 )
            {
               FillSolidRect( hDC, rc.left, rc.top+2, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DHIGHLIGHT ) );
               FillSolidRect( hDC, rc.left+1, rc.top+2, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DFACE ) );
               SetPixel( hDC, rc.left+1, rc.top+1, GetSysColor( COLOR_3DHIGHLIGHT ) );
            }

            FillSolidRect( hDC, rc.left+2, rc.top, rc.right-rc.left-4, 1, GetSysColor( COLOR_3DHIGHLIGHT ) );
            FillSolidRect( hDC, rc.left+2, rc.top+1, rc.right-rc.left-4, 1, GetSysColor( COLOR_3DFACE ) );

            if ( nCurSel != nItem + 1 )
            {
               FillSolidRect( hDC, rc.right-1, rc.top+2, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DSHADOW ) );
               FillSolidRect( hDC, rc.right-2, rc.top+2, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DFACE ) );                        
               SetPixel( hDC, rc.right-2, rc.top+1, GetSysColor( COLOR_3DSHADOW ) );
            }
         }       
         break;
      
      case tabRight:
         if ( nState & bsChecked )
         {
            rc.top -= 2;
            rc.bottom += 2;
            rc.right += 2;
            rc.left -= 1;
         }

         if ( nState & bsHover )
         {
            if ( nCurSel != nItem - 1 )
            {
               FillSolidRect( hDC, rc.left, rc.top, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DHIGHLIGHT ) );
               FillSolidRect( hDC, rc.left, rc.top+1, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DLIGHT ) );
               SetPixel( hDC, rc.right-2, rc.top+1, GetSysColor( COLOR_3DHIGHLIGHT ) );
            }

            FillSolidRect( hDC, rc.right-1, rc.top+2, 1, rc.bottom-rc.top-4, GetSysColor( COLOR_3DDKSHADOW ) );
            FillSolidRect( hDC, rc.right-2, rc.top+2, 1, rc.bottom-rc.top-4, GetSysColor( COLOR_3DSHADOW ) );

            if ( nCurSel != nItem + 1 )
            {
               FillSolidRect( hDC, rc.left, rc.bottom-2, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DSHADOW ) );
               FillSolidRect( hDC, rc.left, rc.bottom-1, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DDKSHADOW ) );
               SetPixel( hDC, rc.right-2, rc.bottom-2, GetSysColor( COLOR_3DDKSHADOW ) );
            }
         }
         else
         {
            if ( nCurSel != nItem - 1 )
            {
               FillSolidRect( hDC, rc.left, rc.top, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DHIGHLIGHT ) );
               FillSolidRect( hDC, rc.left, rc.top+1, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DFACE ) );
               SetPixel( hDC, rc.right-2, rc.top+1, GetSysColor( COLOR_3DHIGHLIGHT ) );
            }

            FillSolidRect( hDC, rc.right-1, rc.top+2, 1, rc.bottom-rc.top-4, GetSysColor( COLOR_3DSHADOW ) );
            FillSolidRect( hDC, rc.right-2, rc.top+2, 1, rc.bottom-rc.top-4, GetSysColor( COLOR_3DFACE ) );            

            if ( nCurSel != nItem + 1 )
            {
               FillSolidRect( hDC, rc.left, rc.bottom-1, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DSHADOW ) );            
               FillSolidRect( hDC, rc.left, rc.bottom-2, rc.right-rc.left-2, 1, GetSysColor( COLOR_3DFACE ) );
               SetPixel( hDC, rc.right-2, rc.bottom-2, GetSysColor( COLOR_3DSHADOW ) );
            }                        
         }       
         break;

      case tabBottom:
         if ( nState & bsChecked )
         {
            rc.bottom += 2;
            rc.left -= 2;
            rc.right += 2;
            rc.top -=1;
         }

         if ( nState & bsHover )
         {
            if ( nCurSel != nItem - 1 )
            {
               FillSolidRect( hDC, rc.left, rc.top, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DHIGHLIGHT ) );
               FillSolidRect( hDC, rc.left+1, rc.top, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DLIGHT ) );
               SetPixel( hDC, rc.left+1, rc.bottom-2, GetSysColor( COLOR_3DHIGHLIGHT ) );
            }

            FillSolidRect( hDC, rc.left+2, rc.bottom-1, rc.right-rc.left-4, 1, GetSysColor( COLOR_3DDKSHADOW ) );
            FillSolidRect( hDC, rc.left+2, rc.bottom-2, rc.right-rc.left-4, 1, GetSysColor( COLOR_3DSHADOW ) );

            if ( nCurSel != nItem + 1 )
            {
               FillSolidRect( hDC, rc.right-1, rc.top, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DDKSHADOW ) );
               FillSolidRect( hDC, rc.right-2, rc.top, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DSHADOW ) );                        
               SetPixel( hDC, rc.right-2, rc.bottom-2, GetSysColor( COLOR_3DDKSHADOW ) );
            }
         }
         else
         {
            if ( nCurSel != nItem - 1 )
            {
               FillSolidRect( hDC, rc.left, rc.top, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DHIGHLIGHT ) );
               FillSolidRect( hDC, rc.left+1, rc.top, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DFACE ) );
               SetPixel( hDC, rc.left+1, rc.bottom-2, GetSysColor( COLOR_3DHIGHLIGHT ) );
            }

            FillSolidRect( hDC, rc.left+2, rc.bottom-1, rc.right-rc.left-4, 1, GetSysColor( COLOR_3DSHADOW ) );
            FillSolidRect( hDC, rc.left+2, rc.bottom-2, rc.right-rc.left-4, 1, GetSysColor( COLOR_3DFACE ) );

            if ( nCurSel != nItem + 1 )
            {
               FillSolidRect( hDC, rc.right-1, rc.top, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DSHADOW ) );
               FillSolidRect( hDC, rc.right-2, rc.top, 1, rc.bottom-rc.top-2, GetSysColor( COLOR_3DFACE ) );
               SetPixel( hDC, rc.right-2, rc.bottom-2, GetSysColor( COLOR_3DSHADOW ) );
            }
         }       
         break;
   }
	 */
}

CPaintManager::CCMTabControl::OrientationsEnum CPaintManager::CCMTabControl::GetOrientation() const
{
   DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );
   if ( dwStyle & TCS_BOTTOM )
   {
      if ( dwStyle & TCS_VERTICAL )
         return tabRight;
      else
         return tabBottom;               
   }
   else  
   { 
      if ( dwStyle & TCS_VERTICAL )
         return tabLeft;
      else
         return tabTop;
   }
}

void CPaintManager::CCMTabControl::DrawControl( HDC hDC, LPRECT lprc )
{
/*   DWORD dwStyle = GetWindowLong( m_hWnd, GWL_STYLE );
   if ( dwStyle & TCS_BUTTONS ) // Skip button-like tab controls
      return;
      
   hDC = GetDC( m_hWnd );  // We will draw on the client DC

   RECT rc = rect;
   SendMessage( m_hWnd, TCM_ADJUSTRECT, FALSE, (LPARAM)&rc );   
   InflateRect( &rc, 4, 4 );

   RECT rcSel;
   int nCurSel = SendMessage( m_hWnd, TCM_GETCURSEL, 0, 0L );
   SendMessage( m_hWnd, TCM_GETITEMRECT, nCurSel, (LPARAM)&rcSel );

   switch ( GetOrientation() )
   {
      case tabLeft:
         rc.left += 2;
         FillSolidRect( hDC, rc.left, rc.bottom, rc.right-rc.left, -1, GetSysColor( COLOR_3DSHADOW ) );
         FillSolidRect( hDC, rc.right, rc.top, -1, rc.bottom-rc.top, GetSysColor( COLOR_3DSHADOW ) );         
         break;

      case tabTop:
         rc.top += 2;                  
         FillSolidRect( hDC, rc.left, rc.bottom, rc.right-rc.left, -1, GetSysColor( COLOR_3DSHADOW ) );
         FillSolidRect( hDC, rc.right, rc.top, -1, rc.bottom-rc.top, GetSysColor( COLOR_3DSHADOW ) );
         break;

      case tabRight:
         rc.right -= 2;         
         FillSolidRect( hDC, rc.left, rc.bottom, rc.right-rc.left, -1, GetSysColor( COLOR_3DSHADOW ) );
         FillSolidRect( hDC, rc.right, rc.top, -1, rcSel.top-rc.top, GetSysColor( COLOR_3DSHADOW ) );
         FillSolidRect( hDC, rc.right, rcSel.bottom, -1, rc.bottom-rcSel.bottom, GetSysColor( COLOR_3DSHADOW ) );
         break;

      case tabBottom:
         rc.bottom -= 2;         
         FillSolidRect( hDC, rc.left, rc.bottom, rcSel.left-rc.left, -1, GetSysColor( COLOR_3DSHADOW ) );
         FillSolidRect( hDC, rcSel.right, rc.bottom, rc.right-rcSel.right, -1, GetSysColor( COLOR_3DSHADOW ) );
         FillSolidRect( hDC, rc.right, rc.top, -1, rc.bottom-rc.top, GetSysColor( COLOR_3DSHADOW ) );
         break;
   }

   Draw3dRect( hDC, rc.left+1, rc.top+1, rc.right-rc.left-2, rc.bottom-rc.top-2, 
               GetSysColor( COLOR_3DFACE ), GetSysColor( COLOR_3DFACE ) );

   m_nOldItem = -1;
   int nState = 0;   
   POINT point;
   GetCursorPos( &point );
   ScreenToClient( m_hWnd, &point );
   int nCount = SendMessage( m_hWnd, TCM_GETITEMCOUNT, 0, 0L );
      
   for ( int i = 0; i < nCount; i++ )
   {     
      SendMessage( m_hWnd, TCM_GETITEMRECT, i, (LPARAM)&rc );
   
      nState = bsNormal;
      if ( nCurSel != i )
      {
         if ( PtInRect( &rc, point ) == TRUE )
         {
            nState |= bsHover;
            m_nOldItem = i;
         }         
         DrawTab( hDC, rc, i, nState );
      }
   }
      
   nState = bsChecked;
   if ( PtInRect( &rcSel, point ) == TRUE )
   {
      nState |= bsHover;
      m_nOldItem = nCurSel;
   }         
   DrawTab( hDC, rcSel, nCurSel, nState );
   
   if ( nCurSel != 0 )
      switch ( GetOrientation() )
      {
         case tabTop:
            FillSolidRect( hDC, rect.left, rect.top, 2, rcSel.bottom-rcSel.top+2, GetSysColor( COLOR_3DFACE ) );
            break;
         case tabBottom:
            FillSolidRect( hDC, rect.left, rect.bottom, 2, -rcSel.bottom+rcSel.top-2, GetSysColor( COLOR_3DFACE ) );
            break;
      }

   ReleaseDC( m_hWnd, hDC );
	 */
}

BOOL CPaintManager::CCMTabControl::NeedRedraw( const POINT& point )
{   
   int nCount = (int)SendMessage( m_hWnd, TCM_GETITEMCOUNT, 0, 0L );                  

   TCHITTESTINFO thti;
   thti.pt = point;
   ScreenToClient( m_hWnd, &thti.pt );
   int nItem = (int)SendMessage( m_hWnd, TCM_HITTEST, 0, (LPARAM)&thti );

   if ( m_hWnd != m_hWndOld || ( m_hWnd == m_hWndOld && m_nOldItem != nItem ) )
      return TRUE;
   return FALSE;
}

LRESULT CPaintManager::CCMTabControl::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch ( uMsg )
   {  
      case WM_PAINT:
      case WM_NCPAINT:
         m_nOldItem = -1;   // Redraw the whole control unconditionally
         break;
   }
   return CCMControl::WindowProc( uMsg, wParam, lParam );
}

//////////////////////////////////////////////////////////////////////////////
// CCMIPAddress class

BOOL CPaintManager::CCMIPAddress::IsFocused()
{
   HWND hWnd = GetTopWindow( m_hWnd );
   while ( hWnd )
   {
      if ( hWnd == GetFocus() )
         return TRUE;
      hWnd = GetNextWindow( hWnd, GW_HWNDNEXT );
   }

   return FALSE;
}

LRESULT CPaintManager::CCMIPAddress::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch ( uMsg )
   {      
      case WM_COMMAND:
         if ( HIWORD( wParam ) == EN_SETFOCUS || HIWORD( wParam ) == EN_KILLFOCUS )
            RePaintControl( uMsg, wParam, lParam );
         break;

   }

	return CCMControl::WindowProc( uMsg, wParam, lParam );
}

//////////////////////////////////////////////////////////////////////////////
// CCMDialog class

LRESULT CPaintManager::CCMDialog::WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch ( uMsg )
   {      
      case WM_PARENTNOTIFY:         
         if ( LOWORD( wParam ) == WM_CREATE )   // Add dynamically created controls to the map
            g_ctrlManager.AddControl( (HWND)lParam );
         break;

      case WM_NCDESTROY:
         // Unsubclass window and remove it from the map
         CallWindowProc( m_oldWndProc, m_hWnd, uMsg, wParam, lParam );
         g_ctrlManager.RemoveDialog( m_hWnd );
         return 0;
   }

   return CallWindowProc( m_oldWndProc, m_hWnd, uMsg, wParam, lParam );
}

///////////////////////////////////////////////////////////////////////////////////
// Helper functions for drawing 3D frames (borrowed from CDC class)

void CPaintManager::CCMControl::FillSolidRect( HDC hDC, int x, int y, int cx, int cy, COLORREF clr )
{
   SetBkColor( hDC, clr );
   RECT rect;
   SetRect( &rect, x, y, x + cx, y + cy );
   ExtTextOut( hDC, 0, 0, ETO_OPAQUE, &rect, NULL, 0, NULL );
}

void CPaintManager::CCMControl::FillSolidRect( HDC hDC, LPRECT lprc, COLORREF clr )
{
   SetBkColor( hDC, clr );
   ExtTextOut( hDC, 0, 0, ETO_OPAQUE, lprc, NULL, 0, NULL );
}

void CPaintManager::CCMControl::Draw3dRect( HDC hDC, int x, int y, int cx, int cy,
                                                  COLORREF clrTopLeft, COLORREF clrBottomRight )
{
   FillSolidRect( hDC, x, y, cx - 1, 1, clrTopLeft );
   FillSolidRect( hDC, x, y, 1, cy - 1, clrTopLeft );
   FillSolidRect( hDC, x + cx, y, -1, cy, clrBottomRight );
   FillSolidRect( hDC, x, y + cy, cx, -1, clrBottomRight );
}

void CPaintManager::CCMControl::Draw3dRect( HDC hDC, const RECT& rect,
                                                  COLORREF clrTopLeft, COLORREF clrBottomRight )
{
   Draw3dRect( hDC, rect.left, rect.top, rect.right - rect.left,
                    rect.bottom - rect.top, clrTopLeft, clrBottomRight );
}

void CPaintManager::CCMControl::Draw3dBorder( HDC hDC, const RECT& rc, 
                                                    int nColor1, int nColor2,
                                                    int nColor3, int nColor4 )
{
   Draw3dRect( hDC, rc, GetSysColor( nColor1 ), GetSysColor( nColor2 ) );

   Draw3dRect( hDC, rc.left + 1, rc.top + 1, rc.right - rc.left - 2, rc.bottom - rc.top - 2, 
                        GetSysColor( nColor3 ), GetSysColor( nColor4 ) );
}

void CPaintManager::CCMControl::Draw3dBorder( HDC hDC, const RECT& rc, 
                                                    int nColor1, int nColor2,
                                                    int nColor3, int nColor4,
                                                    int nColor5, int nColor6 )
{
   Draw3dRect( hDC, rc, GetSysColor( nColor1 ), GetSysColor( nColor2 ) );

   Draw3dRect( hDC, rc.left + 1, rc.top + 1, rc.right - rc.left - 2, rc.bottom - rc.top - 2, 
                        GetSysColor( nColor3 ), GetSysColor( nColor4 ) );
   Draw3dRect( hDC, rc.left + 2, rc.top + 2, rc.right - rc.left - 4, rc.bottom - rc.top - 4, 
                  GetSysColor( nColor5 ), GetSysColor( nColor6 ) );
}


void CPaintManager::Button_DrawBkGnd( HDC hDC, LPRECT lprc, UINT nState ) {

	// ( 20 x 23 ) x 5y = 20 x 115

	UINT nYOffset = 23 * nState;
	CDC *pDC = CDC::FromHandle( hDC );

	CDC ThemeDC;
	ThemeDC.CreateCompatibleDC(pDC);
	ThemeDC.SelectObject( m_bmpButtonSkin.GetSafeHandle() );
	HDC hThemeDC = ThemeDC.GetSafeHdc();

	int nWidth = 20, nHeight = 23, nBorder = 4;
	int nBOffset = 1; // 0
	nBorder -= nBOffset;

	// Компонуем фон
	COLORREF clrTransparent = RGB( 255, 0, 255 );

	// 1. TopLeft
	::TransparentBlt( hDC, lprc->left, lprc->top, nBorder, nBorder, hThemeDC,  0 + nBOffset, nYOffset+nBOffset, nBorder, nBorder, clrTransparent );
	// 2. TopMiddle
	::StretchBlt( hDC, lprc->left + nBorder, lprc->top, lprc->right - lprc->left - nBorder*2, nBorder, hThemeDC, nBOffset+nBorder, nYOffset+nBOffset, nWidth - (nBorder+nBOffset)*2, nBorder, SRCCOPY );
	// 3. TopRight
	::TransparentBlt( hDC, lprc->right - nBorder, lprc->top, nBorder, nBorder, hThemeDC,  nWidth - nBorder - nBOffset, nYOffset+nBOffset, nBorder, nBorder, clrTransparent );
	
	// 4. MiddleLeft
	::StretchBlt( hDC, lprc->left, lprc->top + nBorder, nBorder, lprc->bottom - lprc->top - 2 * nBorder, hThemeDC, 0+nBOffset, nYOffset+nBOffset+ nBorder, nBorder, nHeight-(nBorder+nBOffset)*2, SRCCOPY );
	// 5. Center
	::StretchBlt( hDC, lprc->left + nBorder, lprc->top + nBorder, lprc->right - lprc->left - 2*nBorder, lprc->bottom - lprc->top - 2*nBorder, hThemeDC, nBorder+nBOffset, nYOffset +nBOffset+ nBorder, (nBorder+nBOffset)*2, nHeight-(nBorder+nBOffset)*2, SRCCOPY );
	// 6. MiddleRight
	::StretchBlt( hDC, lprc->right - nBorder, lprc->top + nBorder, nBorder, lprc->bottom - lprc->top - 2*nBorder, hThemeDC, nWidth - nBorder-nBOffset, nYOffset +nBOffset+ nBorder, nBorder, nHeight-(nBorder+nBOffset)*2, SRCCOPY );
	
	// 7. BottomLeft
	::TransparentBlt( hDC, lprc->left, lprc->bottom - nBorder, nBorder, nBorder, hThemeDC, 0+nBOffset, nYOffset + nHeight - nBorder-nBOffset, nBorder, nBorder, clrTransparent );
	// 8. BottomMiddle
	::StretchBlt( hDC, lprc->left + nBorder, lprc->bottom - nBorder, lprc->right - lprc->left - nBorder*2, nBorder, hThemeDC, nBorder+nBOffset, nYOffset + nHeight - nBorder-nBOffset, nWidth - (nBorder+nBOffset)*2, nBorder, SRCCOPY );
	// 9. BottomRight
	::TransparentBlt( hDC, lprc->right - nBorder, lprc->bottom - nBorder, nBorder, nBorder, hThemeDC, nWidth - nBorder-nBOffset, nYOffset + nHeight - nBorder - nBOffset, nBorder, nBorder, clrTransparent );
}

void CPaintManager::CheckBox_DrawBkGnd( HDC hDC, LPRECT lprc, UINT nState ) {
	const int nCheckHeight = 16;

	//m_bmpCheckSkin.Draw( pDC, &rc, &rcDest );

	HDC hMemDC = ::CreateCompatibleDC( hDC );
	::SelectObject( hMemDC, m_bmpCheckSkin.GetSafeHandle() );
	
	::BitBlt( hDC, lprc->left, lprc->top, lprc->right - lprc->left, lprc->bottom - lprc->top, hMemDC, 0, nState * nCheckHeight, SRCCOPY );

	::DeleteDC( hMemDC );
}

void CPaintManager::RadioButton_DrawBkGnd( HDC hDC, LPRECT lprc, UINT nState ) {
	const int nRadioHeight = 16;

//	int nOldMode = pDC->SetBkMode( OPAQUE );
	CRect rcSrc ( 0, nRadioHeight * nState, lprc->right - lprc->left, nRadioHeight * ( nState + 1 ) );
//	CRect rcSrc ( 0, 32, 16, 48 );
	CRect rcDest( *lprc );
	m_bmpRadioSkin.Draw( CDC::FromHandle( hDC ), &rcDest, &rcSrc );
//	pDC->SetBkMode( nOldMode );

	/*CDC MemDC;
	MemDC.CreateCompatibleDC(pDC);
	MemDC.SelectObject( m_bmpRadioSkin.GetSafeHandle() );
	//pDC->TransparentBlt( rc.left, rc.top, rc.Width(), rc.Height(), &MemDC, 0, nState * nRadioHeight, nRadioHeight, nRadioHeight, GetSysColor( COLOR_WINDOW ) );
	pDC->BitBlt( rc.left, rc.top, rc.Width(), rc.Height(), &MemDC, 0, nState * nRadioHeight, SRCCOPY );
*/
}

void CPaintManager::DrawText( HDC hDC, TCHAR* lpszText, LPRECT lprc, UINT nFormat, BOOL bGrayed ) {

	::SetBkMode( hDC, TRANSPARENT );
	::SelectObject( hDC, ::GetStockObject( ANSI_VAR_FONT ) );
	
	COLORREF clrOldTextColor;
	if ( bGrayed ) {
		clrOldTextColor = ::SetTextColor( hDC, GetSysColor( COLOR_GRAYTEXT ) );
	} else {
		clrOldTextColor = ::SetTextColor( hDC, GetSysColor( COLOR_BTNTEXT ) );
	}
	
	::DrawText( hDC, lpszText, (int)_tcslen( lpszText ), lprc, nFormat );

	::SetTextColor( hDC, clrOldTextColor );
}


void CPaintManager::Spin_DrawBkGnd( HDC hDC, UINT nType, LPRECT lprc, UINT nState ) {
	
	CBitmap32 *pSkin = NULL;

	switch( nType ) {
		case 0: // top
			pSkin = &m_bmpSpinUpSkin;
			break;
		case 1: // bottom
			pSkin = &m_bmpSpinDownSkin;
			break;
		case 2: // left
			pSkin = &m_bmpSpinLeftSkin;
			break;
		default: // right
			pSkin = &m_bmpSpinRightSkin;
	}
	
	CRect rcDest( *lprc ); 
	CRect rcSrc( 0, 0, 15, 16 ); 

	pSkin->Draw( CDC::FromHandle( hDC ), &rcDest, &rcSrc );
}

void CPaintManager::Spin_DrawGlyph( HDC hDC, UINT nType, LPRECT lprc, UINT nState ) {
	
}
