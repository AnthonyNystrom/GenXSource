#pragma once


// CInfoBar
typedef enum
{
	INFO_TEXT,
	INFO_PROGRESS
}  INFO_TYPE;

class CInfoBar : public CDialogBar
{
	DECLARE_DYNAMIC(CInfoBar)

public:
	CInfoBar();
	virtual ~CInfoBar();

	void     SetMessageString(const char* str);
	void     SetWarningString(const char* str, const char* str2);
	void     SetErrorString(const char* str, const char* str2);

	void     Progress(int perc);
private:
	INFO_TYPE        m_type;
public:
	void     SetInfoStyle(INFO_TYPE newType);
private:
	UINT_PTR                m_timer_ID;

	int                     m_timers_count;

	COLORREF   m_bk_color;
	COLORREF   m_text_color;
	CButton    m_frame;
	CString    m_cur_string;
	CString    m_reserve_string;
	CProgressCtrl   m_progress;

	virtual CSize  CalcDynamicLayout(int nLength, DWORD nMode);

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnPaint();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};


