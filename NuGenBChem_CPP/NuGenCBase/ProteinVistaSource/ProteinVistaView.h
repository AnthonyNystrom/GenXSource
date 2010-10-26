#pragma once
#if !defined(AFX_ProteinVistaVIEW_H__5CCC63AA_7818_4647_B917_01BF3EBC90A8__INCLUDED_)
#define AFX_ProteinVistaVIEW_H__5CCC63AA_7818_4647_B917_01BF3EBC90A8__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "MMSystem.h"
#include "pdb.h"
#include "pdbInst.h"
 
 
#include <vcclr.h>
#include <msclr/auto_gcroot.h>
using namespace System;
#define		MOVIE_TIMER_ID		1001

class	CProteinVistaRenderer;
class	CwmvFile;
class	CAviFile;
class   CProteinVistaApp;
class   CSelectionListPane;
class   CSelectionItem;
class   CSelectionDisplay;
 
class CResiduePane;
class CHTMLListCtrl;
class CPDBTreePane;
ref class CPIProperty;
class CProteinVistaView  
{
private:
	enum{ WIREFRAME, STICKS, SPACEFILL, BALLANDSTICK, RIBBON, SURFACE,NGRealistic };
	BOOL m_bCreateRender;
	
protected:  
	CProteinVistaRenderer *		m_pProteinVistaRenderer;
public:
	CProteinVistaRenderer* GetRender(){ return this->m_pProteinVistaRenderer; }
	CProteinVistaView(void);
	~CProteinVistaView(void);
public:
	BOOL m_bShowLog;
	void RenderProteinVistaRenderer();
	void SetTimelinePos();
	void SetThumbPosition(long pos);

	void OnAddPdb(array<String^>^ pdbFiles );
	void AddPDB(CString & strFilename);
	
	void ClosePdb(CPDBRenderer * pPDBRenderer);
	void ClosePDBnChild(CPDBRenderer * pPDBRenderer);

	void OutPutLog(CString msgInfo);
	void SetProgress(int currentNumber,long totalCount);
	long InitProgress(long totalCount);
	void EndProgress(long totalCount);
	void ResetProgress();
	void SetStatusBarText(CString strText);

	void UpdateAllPanes();
	void DeleteDefaultPane();
public:
	CwmvFile *	m_pWMVFile;
	CAviFile *	m_pAVIFile;
	CSTLArrayPDB		m_arrayPDB;
	msclr::auto_gcroot<System::Windows::Forms::ComboBox^>  mComboxList;
	void SetCombIndex(long index);

	msclr::auto_gcroot<System::Windows::Forms::ToolStripProgressBar^>  mProgressBar;
	msclr::auto_gcroot<CPIProperty^> mProperty;
 
	void (*mOutputMethod)(CString);
	void (*mGetHeaderInfoMethod)(CString);
	void (*mInitProgressMethod)(int);
	void (*mEndProgressMethod)(int);
	void (*mResetProgressMethod)(int);
	void (*mSetProgressMethod)(int,int);
	void (*mPropertyChanged)();
		 
	int     GetCurrentComboxSelectIndex();
	void	OnPDBChanged();
	void	DeleteSelectionList(CPDBInst * pPDBInst);


	CPIProperty^ GetProperty();
	void    RefreshProptery(CSelectionDisplay * pSelectionDisplay);
	HRESULT UnSelectedAtoms();
	HRESULT	SetSelectedAtoms ( CString &strPDBID, CSTLArrayAtomInst &atomPtrArray);
	HRESULT GetSelectedAtoms ( CSTLArrayAtomInst &atomPtrArray);
	int		GetNumPDBID(CString PDBID);

	void	UpdateActivePDBComboBox();
	CString  GetPDBHeaderInfo();
	LRESULT DefWindowProc(UINT message, WPARAM wParam, LPARAM lParam);

	void	OnButtonDisplay(long mode);
	void	OnUpdateButtonDisplay(long mode, CCmdUI* pCmdUI);
	void    DeleteContents() ;
	void    CreatePanel() ;
	void    RefreshProptery(CSelectionDisplay * pSelectionDisplay,long mod);
	void    ChanegPropertyValue(long id,CString value,CSelectionDisplay * m_pSelectionDisplay);
public:
	void OnPaint();
	void OnSizeChange();
public:
	void OnButtonBall();
	void OnButtonBallStick();	
	void OnButtonDotsurface();	
	void OnButtonDotsurfaceWithResolution(UINT id);
	void OnButtonRibbon();
	void OnButtonStick();
	void OnButtonWireframe();
	void OnButtonNextFrame();
	void OnButtonGoFirst();
	void OnButtonGoLast();
	void OnButtonPlay();
	void OnButtonPrevFrame();
	void OnButtonStop();
	void OnButtonPlayFast();
	void OnButtonPlaySlow();
	void OnAddPdb();
	void OnClosePdb(BOOL bRefresh=TRUE);
	void OnCenterMolecule();
	void OnViewAll();
	void OnViewAllDisplayParams();
	void OnNextActivePDB();
	void OnPrevActivePDB();
	void OnDisplayBioUnit();
	void OnDisplayBioUnitStyle(UINT mode);
	void OnDisplayBioUnitSurface(UINT quality);
	void OnAttatchBiounit();
	void OnSurfaceGenAlgorithmMQ();
	void OnSurfaceGenAlgorithmMSMS();
	void OnSurfaceBiounitGenAlgorithmMQ();
	void OnSurfaceBiounitGenAlgorithmMSMS();

	void OnActivePDB();
	void OnFlagMoleculeSelectionCenter();
	LRESULT OnChangeDisplayMode(WPARAM wParam, LPARAM lParam);
	/////////////////////////////////////////////////////////////////////////
	void OnFileScreenshot();
	void OnFileStartMakeMovie(); 
	void OnFileStopMakeMovie();
	void OnFileAddPDBID();
protected:
	CResiduePane* mResiduePanel;
	CSelectionListPane* mSelectPanel;
	CPDBTreePane* mPDBTreePanel;
public :
	void CreateResiduePanel(HWND hWnd);
	void ResiduesSizeChange(int cx, int cy);

    void CreateSelectPanel(HWND hWnd);
	void SelectPanelSizeChange(int cx, int cy);

	void CreatePDBTreePanel(HWND hWnd);
	void PDBTreePanelSizeChange(int cx, int cy);
    
	CHTMLListCtrl * GetSelectList();
	CSelectionListPane * GetSelectPanel();

	CResiduePane* GetResiduePanel()
	{
		return this->mResiduePanel;
	}
	CPDBTreePane* GetCPDBTreePane()
	{
		return this->mPDBTreePanel;
	}
};
#endif 