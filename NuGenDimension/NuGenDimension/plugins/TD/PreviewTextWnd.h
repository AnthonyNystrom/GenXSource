#pragma once


// CPreviewTextWnd

class CPreviewTextWnd : public CWnd
{
	DECLARE_DYNAMIC(CPreviewTextWnd)

public:
	CPreviewTextWnd();
	virtual ~CPreviewTextWnd();
private:
	CString      m_text;
	SG_TEXT_STYLE* m_cur_style_pntr;
	unsigned  int  m_cur_fnt;
public:
	void         SetCurFont(unsigned int cf)
	{
		m_cur_fnt = cf;
	};
    void         SetTextStylePointer(SG_TEXT_STYLE* tsp)
	{
		ASSERT(tsp);
		m_cur_style_pntr = tsp;
	}
public:

	void   SetText(const char*);
	void   ChangeStyle()
	{
		SetText(m_text);
	}
	//void   SetStyle()
protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnPaint();
};


