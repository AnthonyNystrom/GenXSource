#pragma once


// CArrowTypeCombo

class CArrowTypeCombo : public CComboBox
{
	DECLARE_DYNAMIC(CArrowTypeCombo)

public:
	CArrowTypeCombo();
	virtual ~CArrowTypeCombo();
private:
	bool    m_left_to_right;
	CBitmap m_bitmaps[16];
public:
	void    SetType(bool l_To_r);

protected:

	// @cmember
	// Called by MFC when visual aspect of combo box changes 
	virtual void DrawItem(LPDRAWITEMSTRUCT lpDIS);
	virtual void MeasureItem(LPMEASUREITEMSTRUCT lpMIS);

	//virtual int AddString(LPCTSTR lpszString) { return -1; }
	virtual int InsertString(int nIndex, LPCTSTR lpszString) { return -1; }
	virtual int DeleteString(int nIndex) { return -1; }

#ifdef _DEBUG

	virtual void PreSubclassWindow();
#endif

	// @access Private Member Functions and Variables
private:

public:
	DECLARE_MESSAGE_MAP()
};


