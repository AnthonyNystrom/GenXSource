#ifndef __CURSORER__
#define __CURSORER__
#define MAX_CURSOR_SIZE  15

typedef struct 
{
	BOOL   isRound;
	unsigned short    size;
	unsigned short    insideColor;
	unsigned short    outsideColor;
} CURSOR_STRUCTURE;

class CCursorer
{
	CURSOR_STRUCTURE   m_cursor_structure;
	HCURSOR            m_hCursor;
public:
	CCursorer();
	~CCursorer();

	CURSOR_STRUCTURE*   GetCursorStructure() ;
	bool                      SetCursorStructure(CURSOR_STRUCTURE&);

	const HCURSOR   GetCursor() const {return m_hCursor;};

private:
	HBITMAP		GetCursorBitmap();
	HCURSOR		CreateCursorFromBitmap(HBITMAP hSourceBitmap,
										COLORREF clrTransparent,
										DWORD   xHotspot,
										DWORD   yHotspot);
	void		GetMaskBitmaps(HBITMAP hSourceBitmap, 
									COLORREF clrTransparent, 
									HBITMAP &hAndMaskBitmap, 
									HBITMAP &hXorMaskBitmap);


};

#endif