#pragma once


// CSaveMovieDialog dialog

class CSaveMovieDialog : public CDialog
{
	DECLARE_DYNAMIC(CSaveMovieDialog)

public:
	CSaveMovieDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSaveMovieDialog();

	int		m_imageWidth;
	int		m_imageHeight;

	int		m_imageWidthDefault;
	int		m_imageHeightDefault;

	int		m_fps;
	int		m_checkCurrentImageSize;

	CString		m_strMovieFilename;
	CString		m_strBitmapFilename;

	CStringArray	m_strArrayFilename;

// Dialog Data
	enum { IDD = IDD_DIALOG_MAKE_MOVIE };

	virtual void OnOK();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()

	void OnCurrentImageSize();
	void OnButtonSaveFilenameBrowse();
	void OnButtonInsert();
	void OnButtonRemove();
	void OnButtonUp();
	void OnButtonDown();
	void OnButtonCheckMovieSize();

};
