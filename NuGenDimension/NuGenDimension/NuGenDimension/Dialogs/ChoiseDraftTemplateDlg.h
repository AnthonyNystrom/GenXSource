#pragma once
#include "afxwin.h"

class CDraftTemplatesListBox : public CListBox
{
	DECLARE_DYNAMIC(CDraftTemplatesListBox)

public:
	CDraftTemplatesListBox();
	virtual ~CDraftTemplatesListBox();
private:
	std::vector<CxImage*>  *m_images;
protected:
	DECLARE_MESSAGE_MAP()
public:
	void  SetImagesVectorPointer(std::vector<CxImage*>* nP) {m_images = nP;};

	virtual int CompareItem(LPCOMPAREITEMSTRUCT lpCI) {return 0;};
	virtual void MeasureItem(LPMEASUREITEMSTRUCT /*lpMeasureItemStruct*/);
	virtual void DrawItem(LPDRAWITEMSTRUCT /*lpDrawItemStruct*/);
};

// CChoiseDraftTemplateDlg dialog

class CChoiseDraftTemplateDlg : public CDialog
{
	DECLARE_DYNAMIC(CChoiseDraftTemplateDlg)

public:
	CChoiseDraftTemplateDlg(const CString& appPath, CWnd* pParent = NULL);   // standard constructor
	virtual ~CChoiseDraftTemplateDlg();

// Dialog Data
	enum { IDD = IDD_CHOISE_TEMPLATE_DLG };
private:
	std::vector<CString>   m_TemplatesPathsArray; 
	CString				   m_application_Path; 
	void				   GetTemplatesFiles(CString sPath);

	std::vector<CxImage*>  m_thumbnails;
	std::vector<CString>   m_names;
	void                   LoadAllTemplatesThumbnails();


protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CDraftTemplatesListBox m_templates_list;
	int m_selected_type;
	int m_select;
public:
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	virtual BOOL OnInitDialog();

	int       GetSelectTemplateType() 
	{
		return m_selected_type;
	};
	CString   GetSelectedTemplatePath();
protected:
	virtual void OnOK();
public:
	afx_msg void OnPaint();
	afx_msg void OnBnClickedRepTemplType();
	afx_msg void OnBnClickedRepTemplType2();
	afx_msg void OnBnClickedRepTemplType3();
private:
	BOOL m_regime;
public:
	afx_msg void OnLbnSelchangeDraftTemplatesList();
};
