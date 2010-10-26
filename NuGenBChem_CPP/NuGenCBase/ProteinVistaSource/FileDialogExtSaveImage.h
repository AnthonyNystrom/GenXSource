#pragma once


// CFileDialogExtSaveImage 대화 상자입니다.

class CFileDialogExtSaveImage : public CFileDialog
{
	DECLARE_DYNAMIC(CFileDialogExtSaveImage)

public:
	CFileDialogExtSaveImage(BOOL bOpenFileDialog, 
		LPCTSTR lpszDefExt = NULL,
		LPCTSTR lpszFileName = NULL,
		DWORD dwFlags = OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,
		LPCTSTR lpszFilter = NULL,
		CWnd* pParentWnd = NULL);

	virtual ~CFileDialogExtSaveImage();

	virtual BOOL OnInitDialog();
	virtual void OnOK();

	// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOG_FILE_EXTENSION_SAVE_IMAGE };

	int		m_imageWidth;
	int		m_imageHeight;

	int		m_imageWidthDefault;
	int		m_imageHeightDefault;

	int		m_comboBoxImageFormat;
	int		m_checkCurrentImageSize;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.
	void	OnCurrentImageSize();

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnClose();
	afx_msg void OnDestroy();
};
