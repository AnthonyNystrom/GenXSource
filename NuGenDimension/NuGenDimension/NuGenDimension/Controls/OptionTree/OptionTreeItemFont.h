
#ifndef OT_ITEMFONT
#define OT_ITEMFONT

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// OptionTreeItemFile.h : header file
//

// Added Headers
//#include "CommonRes.h"
#include "OptionTreeDef.h"
#include "OptionTreeItem.h"

class COptionTreeItemFont : public COptionTreeItem
{
public:
	COptionTreeItemFont();
	virtual ~COptionTreeItemFont();
	virtual void OnMove();
	virtual void OnRefresh();
	virtual void OnCommit();
	virtual void OnActivate();
	virtual void CleanDestroyWindow();
	virtual void OnDeSelect();
	virtual void OnSelect();
	virtual void DrawAttribute(CDC *pDC, const RECT &rcRect);

	BOOL CreateFontItem(const LOGFONT* lf, double coef);

	COLORREF  GetColor() {return m_color;};
	void      SetColor(COLORREF nc) {m_color = nc;};

public:
	LOGFONT		m_lf;
private:
	double m_coef;
	COLORREF m_color;
};

#endif // !OT_ITEMFILE
