#pragma once


// CSpecSymbolsListBox

class CSpecSymbolsListBox : public CListBox
{
	DECLARE_DYNAMIC(CSpecSymbolsListBox)

public:
	CSpecSymbolsListBox();
	virtual ~CSpecSymbolsListBox();
private:
	unsigned  int  m_cur_fnt;
protected:
	DECLARE_MESSAGE_MAP()
public:
	void         SetCurFont(unsigned int cf);
	virtual int CompareItem(LPCOMPAREITEMSTRUCT lpCI) {return 0;};
	virtual void MeasureItem(LPMEASUREITEMSTRUCT /*lpMeasureItemStruct*/);
	virtual void DrawItem(LPDRAWITEMSTRUCT /*lpDrawItemStruct*/);
};


