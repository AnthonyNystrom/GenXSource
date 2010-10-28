#if !defined (__CoolControlsManager_h)
#define __CoolControlsManager_h

#include <afxtempl.h>   // For CMapPtrToPtr

//////////////////////////////////////////////////////////////////////
// CPaintManager class

#include "bitmap32.h"

class CPaintManager {
	public:
/*		enum ButtonStatesEnum {
				bsNormal,
				bsHover,
				bsChecked,
		};
		enum DrawStatesEnum {
				dsNormal, 
				dsHover,
				dsAlternate,
				dsFocus = 0x04,
				dsHoverMask = 0x05,
				dsDisabled = 0x08,
		};
*/
	// Base class for all controls and dialogs
	class CCMCore {
		public:            
			void Subclass( HWND hWnd, WNDPROC wndNewProc );
			void Unsubclass();

			// New window procedure (must be implemented by derived classes)
			virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam ) = 0;

		protected:
			HWND m_hWnd;                          // Handle of the window            
			WNDPROC m_oldWndProc;                 // Address of original window procedure
	};

		// Class that holds important control informations and is
		// responsible for drawing the control
	class CCMControl : public CCMCore {
		protected:
			BOOL m_bFullRepaint;

			UINT m_nLastState;
			HBRUSH m_hbrActiveBorder;
			HBRUSH m_hbrInactiveBorder;
			HBRUSH m_hbrWindow;
			HBRUSH m_hbrDisabledWindow;
			HPEN	m_pnActiveBorder;
			HPEN	m_pnInactiveBorder;
			HPEN	m_pnWindow;
			HPEN	m_pnDisabledWindow;
			
			BOOL m_bHover;
			BOOL m_bEnabled;
			BOOL m_bFocused;
			TCHAR* m_pszText;
			void InitText();
			LRESULT RePaintControl( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL bSilent = TRUE );
			void DrawCtrlBk( HDC hDC, LPRECT lprc );
			void DrawBtnText( HDC hDC, TCHAR* lpszText, LPRECT lprc, DWORD dwBtnStyle, BOOL bGrayed, DWORD dwHDefault, DWORD dwVDefault );
		// Construction/destruction
		public:            
			CCMControl();
			~CCMControl();
	      
		// Operations
		public:
			// Main drawing routine            
			void RePaint();   
			// Returns TRUE if control needs repainting 
//			virtual BOOL NeedRedraw( const POINT& point );

			void ResetHover();
			void SetHover( HWND hwndOld );
			BOOL IsHovered( ) {return m_bHover; }

			// New window procedure
/*			void TraceMsg( const UINT uMsgFilter, UINT uMsg, TCHAR* lpszName ) {
				if( 0 == _tcsicmp( m_pszText, _T("default button") ) && uMsgFilter == uMsg )
					TRACE1("MESSAGE: %s\r\n", lpszName );
			}*/

			virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
	        
			static HWND m_hWndOld;           // Previously 'hovered' or 'focused' window

		// Implementation
		protected:            
			// Paint the scrollbars if a window contains at least one
			void DrawScrollBars( HDC hDC, LPRECT lprc );
			// Paint the scrollbar control or an embedded window scrollbar (horiz or vert)
			void DrawScrollBar( HDC hDC, LPRECT lprc,
															int nType, BOOL bScrollbarCtrl = FALSE );
					// Draw the scrollbar thumb
			void DrawScrollbarThumb( HDC hDC, LPRECT lprc );

			// There are helper functions for drawing 3D frames
			static void Draw3dBorder( HDC hDC, const RECT& rc, 
																		int nColor1, int nColor2,
																		int nColor3, int nColor4 );                               
			static void Draw3dBorder( HDC hDC, const RECT& rc, 
																		int nColor1, int nColor2,
																		int nColor3, int nColor4,
																		int nColor5, int nColor6 );
			static void FillSolidRect( HDC hDC, LPRECT lprc, COLORREF clr );
			static void FillSolidRect( HDC hDC, int x, int y, int cx, int cy, COLORREF clr );
			
			static void Draw3dRect( HDC hDC, int x, int y, int cx, int cy,
																	COLORREF clrTopLeft, COLORREF clrBottomRight );
			static void Draw3dRect( HDC hDC, const RECT& rect,
																	COLORREF clrTopLeft, COLORREF clrBottomRight );

		// Overrides
		protected:                                                            
			// Prepares DC and RECT for further drawings
			virtual HDC BeginDraw( LPRECT lprc );    

					// Drawing code which is specific to the control
			virtual void DrawControl( HDC hDC, LPRECT lprc );            

			// Returns TRUE if control (or one of its children) has a focus
//			virtual BOOL IsFocused();
	        
//					short m_nState;                       // Current control state
//					short m_nOldState;                    // Previous control state
		};

      // Edit windows
      class CCMEdit : public CCMControl {         
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
      };

      // ComboBoxes (all styles are suported)
      class CCMComboBox : public CCMControl {
         public:         
            virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
            //virtual BOOL IsFocused();
      };

      // Date/Time pickers
      class CCMDateTime : public CCMComboBox {
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
      };

      // Pushbuttons
      class CCMPushButton : public CCMControl {
				public:
					CCMPushButton() {
						 m_bFullRepaint = TRUE;
					}
					virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
      };

      // Checkboxes
      class CCMCheckBox : public CCMPushButton {
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );

            // Check boxes and radio buttons are always drawn
            // in the same way regardless of focus   
//            virtual BOOL IsFocused() { return FALSE; }
      };

      // Radiobuttons
      class CCMRadioButton : public CCMPushButton {
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
      };

      // Spin Buttons
      class CCMUpDown : public CCMControl {
				protected:
					BOOL m_bLButtonDown;
					POINT m_ptCursor;
					CFont m_fntArrows;
        public:
					CCMUpDown() {
						m_bLButtonDown = FALSE;
						
						HDC hDC = ::GetDC ( 0 );
						int ppi = ::GetDeviceCaps( hDC, LOGPIXELSX );
						::ReleaseDC( 0, hDC );
						int pointsize = MulDiv(60, 96, ppi); // 6 points at 96 ppi
						m_fntArrows.CreatePointFont(pointsize, _T("Marlett"));
						m_bFullRepaint = TRUE;
					}
					~CCMUpDown() {

					}

					virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
         protected:
//            void DrawButton( HDC hDC, LPRECT lprc, UINT nType, UINT nState );
            virtual void DrawControl( HDC hDC, LPRECT lprc );
      };

      // Edit control in simple combobox
      class CCMEditCombo : public CCMEdit {            
				protected:
					virtual HDC BeginDraw( LPRECT lprc );
      };

      // Stand-alone scrollbar controls
      class CCMScrollBar : public CCMControl {
         public:
            virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
      };

      // Header control
      class CCMHeaderCtrl : public CCMControl {
         public:
            virtual BOOL NeedRedraw( const POINT& point );
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
            void DrawButton( HDC hDC, const RECT& rc, int nState );
         private:
            int m_nOldItem;        // Recently selected item
      };

      // Slider control
      class CCMTrackbar : public CCMControl {
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
            void DrawThumb( HDC hDC, LPRECT lprc );
      };

      // Toolbars
      class CCMToolbar : public CCMControl {
         public:            
            virtual BOOL NeedRedraw( const POINT& point );                     
            virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
            void DrawButton( HDC hDC, LPRECT lprc, int nState );            
         private:
            int m_nOldItem;        // Recently selected item
      };

      // IP Address control
      class CCMIPAddress : public CCMControl {
         public:
            virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
         protected:            
            virtual BOOL IsFocused();
      };

       // Tab control
      class CCMTabControl : public CCMControl {
         public:
            enum OrientationsEnum {
               tabTop,
               tabLeft,
               tabRight,
               tabBottom,
            };

            CCMTabControl() 
            {
               m_nOldItem = -1;
            }
            virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
            virtual BOOL NeedRedraw( const POINT& point );
         protected:
            virtual void DrawControl( HDC hDC, LPRECT lprc );
            void DrawTab( HDC hDC, const RECT& rc, int nItem, int nState );            
            virtual BOOL IsFocused() { return FALSE; }
            OrientationsEnum GetOrientation() const;
         private:
            int m_nOldItem;         // Recently selected item
      };

      // Dialog (parent of controls)
      class CCMDialog : public CCMCore {
         public:
            virtual LRESULT WindowProc( UINT uMsg, WPARAM wParam, LPARAM lParam );
      };
      
   public:
      CPaintManager();
      virtual ~CPaintManager();
      
      void Install( HWND hWnd );            // Installs control manager for given window only
      void Uninstall( HWND hWnd );          // Removes all window controls from the map
      
      // Installs WH_CALLWNDPROC hook, which automatically 
      // handles all dialogs in the givent thread. If dwThreadID = -1, then
      // current thread is used
		public:
      void InstallHook( TCHAR* lpszFilename, DWORD dwThreadID = -1, BOOL bDialogOnly = TRUE );
                                                         
      // Installs WH_CALLWNDPROC hook for 
      // all dialogs in system (valid only for a DLL)
      void InstallGlobalHook( HINSTANCE hInstance, BOOL bDialogOnly = TRUE );

      // Uninstals hook
      void UninstallHook( DWORD dwThreadID = -1 );                 
      
      // Adds single control to the map
      BOOL AddControl( HWND hWnd, LPCTSTR lpszClass = NULL  );
      // Removes single control from the map
      BOOL RemoveControl( HWND hWnd );      

      void AddDialog( HWND hWnd );          // Add dialog
      void RemoveDialog( HWND hWnd );       // Removes dialog window from the map

      void Enable( BOOL bEnable = TRUE );   // Temporary enable/disable control manager
      BOOL IsEnabled() const;               // Returns TRUE if control manager is enabled

      void TimerProc();                     // Timer procedure 

      static CMapPtrToPtr m_ctrlMap;        // Main control map      
      static CMapWordToPtr m_threadMap;     // Main thread map      
		
   protected:
      // Other class members      
      static BOOL m_bEnabled;               // TRUE if control manager is enabled
      BOOL m_bDialogOnly;                   // Process only controls in dialogs
      HOOKPROC m_hkWndProc;                 // Old WH_WNDPROC hook procedure
      UINT m_uTimerID;                      // Our timer identifier
      static CMapPtrToPtr m_dlgMap;         // Map for dialog window (in general - for owner of controls)

      // Give access to the protected members for these functions
      friend LRESULT CALLBACK CCM_CallWndProc( int nCode, WPARAM wParam, LPARAM lParam );
      friend static LRESULT CALLBACK CCM_DialogProc( HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam );
      friend static LRESULT CALLBACK CCM_ControlProc( HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam );
      friend static void CALLBACK CCM_TimerProc( HWND hwnd, UINT uMsg, UINT idEvent, DWORD dwTime );

	// Control's skin data
	static void DrawText( HDC hDC, TCHAR* lpszText, LPRECT lprc, UINT nFormat, BOOL bGrayed );


	static CBitmap32 m_bmpButtonSkin;
	static void Button_DrawBkGnd( HDC hDC, LPRECT lprc, UINT nState );

	static CBitmap32 m_bmpCheckSkin;
	static void CheckBox_DrawBkGnd( HDC hDC, LPRECT lprc, UINT nState );

	static CBitmap32 m_bmpRadioSkin;
	static void RadioButton_DrawBkGnd( HDC hDC, LPRECT lprc, UINT nState );

	static CBitmap32 m_bmpSpinDownSkin;
	static CBitmap32 m_bmpSpinUpSkin;
	static CBitmap32 m_bmpSpinLeftSkin;
	static CBitmap32 m_bmpSpinRightSkin;
	static CBitmap32 m_bmpSpinDownGlyphSkin;
	static CBitmap32 m_bmpSpinUpGlyphSkin;
	static CBitmap32 m_bmpSpinLeftGlyphSkin;
	static CBitmap32 m_bmpSpinRightGlyphSkin;
	static void Spin_DrawBkGnd( HDC hDC, UINT nType, LPRECT lprc, UINT nState );
	static void Spin_DrawGlyph( HDC hDC, UINT nType, LPRECT lprc, UINT nState );

};

//////////////////////////////////////////////////////////////////////////////////////////
// CPaintManager inlines

inline
void CPaintManager::Enable( BOOL bEnable )
{
   m_bEnabled = bEnable;
}

inline
BOOL CPaintManager::IsEnabled() const
{
   return m_bEnabled;
}

/////////////////////////////////////////////////////////////////////////////
// CCMControl inlines

inline
void CPaintManager::CCMControl::ResetHover() {
	//TRACE1(" reset hover %X\r\n", m_hWnd);
	if( m_bHover ) {
//		TraceMsg( 0, 0, _T("WM_RESET_HOVER") );

		//TRACE0(" done!!! \r\n");
		m_bHover = FALSE;
		m_hWndOld = NULL;
		RePaint();
	}
}

inline
void CPaintManager::CCMControl::SetHover( HWND hwndOld ) {
	//TRACE1(" set hover %X\r\n", hwndOld);
	if ( !m_bHover ) {
//		TraceMsg( 0, 0, _T("WM_SET_HOVER") );
		//TRACE0(" done!!! \r\n");
		m_bHover = TRUE;
		m_hWndOld = hwndOld;
		RePaint();
	}
}

/*
inline
void CPaintManager::CCMControl::SetState( int nFlagRemove, int nFlagAdd, BOOL bRedraw )
{
   m_nState &= ~nFlagRemove;
   m_nState |= nFlagAdd;
   if ( bRedraw )
      DrawBorder();
}

inline
int CPaintManager::CCMControl::GetState() const
{
   return m_nState;
}
*/

// Gives access to the one and only CPaintManager object
CPaintManager& GetPaintManager();

#endif // __CoolControlsManager_h
