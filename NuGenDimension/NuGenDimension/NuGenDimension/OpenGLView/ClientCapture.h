
#if !defined(AFX_CLIENTCAPTURE_H__F3017F4D_3A97_11D2_9506_F6C490433B31__INCLUDED_)
#define AFX_CLIENTCAPTURE_H__F3017F4D_3A97_11D2_9506_F6C490433B31__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000


class CClientCapture 
{
public: // create from serialization only
	CClientCapture();
	~CClientCapture();
// Operations
public:
	void Release();
	BOOL WriteDIB( CString csFile);
	void OnDraw(HDC hDC, CRect rcRect, CRect rect);
	BOOL Paint(HDC hDC, CPalette *pal, LPRECT lpDCRect, LPRECT lpDIBRect) const;
	void Capture(CDC *dc, CRect rectDIB);

private:
	HANDLE hDIB;
	
	DWORD Height() const;
	BOOL WriteWindowToDIB( HDC hDC, CDC* dc, CRect rect);
	HANDLE DDBToDIB( CBitmap& bitmap, DWORD dwCompression, CPalette* pPal, CDC* dc);

	BOOL WriteWindowToDIB(CDC* dc, CRect rect);

	LPBITMAPINFO m_pBMI;
	LPBYTE 	m_pBits;
	CPalette pal;
	

};

/////////////////////////////////////////////////////////////////////////////
#endif // !defined(AFX_CLIENTCAPTURE_H__F3017F4D_3A97_11D2_9506_F6C490433B31__INCLUDED_)



