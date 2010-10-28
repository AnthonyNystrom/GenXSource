#pragma once

DWORD WINAPI AnimateProc(LPVOID lpParameter);

class CEGTrayIcon
{
	TCHAR* m_szTitle;
	TCHAR* m_szSecondTitle;

	friend DWORD WINAPI AnimateProc(LPVOID lpParameter);

	HANDLE hStopEvent;
	HICON* m_hIcons;

	int m_nCount;
	int m_nInterval;
	
	HICON m_hMain;
	HICON m_hSecond;
	BOOL m_bMainView;
	HMENU m_hMenu;
	HMENU m_hSourceMenu;
	BOOL m_bIconExists;

	void _SetIcon( HICON hIcon, TCHAR* pszTitle );
public:

	CEGTrayIcon();
	~CEGTrayIcon();

	HWND m_hWnd;
	UINT m_nNotify;

	void SetIcon( HICON hIcon, TCHAR* szTitle );
	void SetSecondIcon( HICON hIcon, TCHAR* szTitle );
	void SetView( BOOL bMain = TRUE );
	
	BOOL StartAnimate(HICON* hIcons,int nCount, int nInterval);
	void StopAnimate();

	void SetMenu( UINT nMenu,  UINT nSubMenu = 0, HINSTANCE hInst = AfxGetResourceHandle() );
	void SetMenu( LPCSTR pszMenu,  UINT nSubMenu = 0, HINSTANCE hInst = AfxGetResourceHandle()  );
	void SetMenu( HMENU hMenu );
	HMENU GetMenu() { return m_hMenu; };

	BOOL IsUsed() { return m_bIconExists; }
protected:
	void Animate();
};

