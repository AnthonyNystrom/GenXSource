#include "stdafx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "ProteinVistaRenderer.h"
#include "Mmsystem.h"
#include "PDBRenderer.h"
#include "pdb.h"
#include "pdbInst.h"
#include "Interface.h"
#include "wmvFile.h"
#include "Utility.h"
#include "Utils.h"
#include "SkyBox.h"

#include "FileDialogExt.h"
#include "OpenPDBIDDialog.h"
//#include "SelectPDBDialog.h"

#include "DotNetInterface.h"
#include "PIProperty.h"

#include "SelectionListPane.h"
#include "ResiduePane.h"
#include "PDBTreePane.h"

using namespace System;
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define	 ID_RENDER_TIMER		100

void CALLBACK mmTimerCallback(UINT uTimerID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dw1, DWORD_PTR dw2);
CProteinVistaView::CProteinVistaView(void):m_pProteinVistaRenderer(NULL)
{
	m_pWMVFile = NULL;
 
	mComboxList = gcnew System::Windows::Forms::ComboBox();
	mProgressBar = gcnew System::Windows::Forms::ToolStripProgressBar();

	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	mResiduePanel = new CResiduePane;
	mSelectPanel = new CSelectionListPane;
	mPDBTreePanel = new CPDBTreePane();
	this->mOutputMethod =NULL;
	this->mGetHeaderInfoMethod =NULL;
	this->mInitProgressMethod =NULL;
	this->mEndProgressMethod =NULL;
	this->mResetProgressMethod =NULL;
	this->mSetProgressMethod =NULL;
	this->mPropertyChanged =NULL;
	this->m_pProteinVistaRenderer =NULL;
	this->m_bCreateRender =FALSE;
	this->m_bShowLog = FALSE;
 
}
 
/////////////////////////////////////////////////////////////////////////////
void CProteinVistaView::OutPutLog(CString msgInfo)
{
	if(mOutputMethod )
	{
		mOutputMethod(msgInfo);
	}
}
void CProteinVistaView::SetStatusBarText(CString strText)
{

}

void CProteinVistaView::SetProgress(int currentNumber,long totalCount)
{
	if(this->mSetProgressMethod)
	{
		this->mSetProgressMethod(currentNumber,totalCount);
	}
}
long CProteinVistaView::InitProgress(long totalCount)
{
	if(this->mInitProgressMethod)
	{
		this->mInitProgressMethod(totalCount);
	}
	return totalCount;
}
void CProteinVistaView::EndProgress(long totalCount)
{
	if(this->mEndProgressMethod)
	{
      this->mEndProgressMethod(totalCount);
	}
}
void CProteinVistaView::ResetProgress()
{
	if(this->mResetProgressMethod)
	{
		this->mResetProgressMethod(0);
	}
}

CPIProperty^ CProteinVistaView::GetProperty()
{
    if(mProperty)
	{
		return  this->mProperty->GetInstance();
	}
	return nullptr;
}
void CProteinVistaView::RefreshProptery(CSelectionDisplay * pSelectionDisplay,long mode)
{
	switch (mode)
	{
	case WIREFRAME:
		mProperty = gcnew CPIPropertyWireframe(pSelectionDisplay);
		break;
	case STICKS:
		mProperty = gcnew CPIPropertyStick(pSelectionDisplay);
		break;
	case SPACEFILL:
		mProperty = gcnew CPIPropertySpaceFill(pSelectionDisplay);
		break;
	case BALLANDSTICK:
		mProperty = gcnew CPIPropertyBallnStick(pSelectionDisplay);
		break;
	case RIBBON:
		mProperty = gcnew CPIPropertyRibbon(pSelectionDisplay);
		break;
	case SURFACE:
		mProperty = gcnew CPIPropertySurface(pSelectionDisplay);
		break;
	case NGRealistic:
		mProperty = gcnew CPIPropertySurface(pSelectionDisplay);
		break;
	default:
		mProperty = gcnew CPIPropertyScene(pSelectionDisplay);
		break; 
	}
	if(this->mPropertyChanged)
	{
		this->mPropertyChanged();
	} 
}
void CProteinVistaView::SetCombIndex(long index)
{
	if(this->mComboxList)
		return ;
	if(this->mComboxList->Items->Count >(int)index)
	{
		this->mComboxList->SelectedIndex =index; 
	}
}

CProteinVistaView::~CProteinVistaView(void)
{
	SAFE_DELETE(m_pWMVFile);
	DeleteDefaultPane();
	if ( m_pProteinVistaRenderer )
	{
		m_pProteinVistaRenderer->Cleanup3DEnvironment();
		m_pProteinVistaRenderer->FinalCleanup();

		SAFE_RELEASE( m_pProteinVistaRenderer->m_pD3D );
		SAFE_DELETE ( m_pProteinVistaRenderer );
	}
}
void CProteinVistaView::UpdateAllPanes()
{
	if ( this->mSelectPanel && mSelectPanel->GetSafeHwnd() )
	{
		mSelectPanel->OnUpdate();
		mSelectPanel->Invalidate(true);
	}
	if ( this->mResiduePanel && mResiduePanel->GetSafeHwnd() )	
	{
		mResiduePanel->OnUpdate();
		mResiduePanel->Invalidate(true);
	}
	if ( this->mPDBTreePanel && mPDBTreePanel->GetSafeHwnd() )	
	{
		mPDBTreePanel->OnUpdate();
		mPDBTreePanel->Invalidate(true);
	}

}
void CProteinVistaView::DeleteDefaultPane()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if ( mPDBTreePanel && mPDBTreePanel->GetSafeHwnd() )
	{
		mPDBTreePanel->Delete();
		mPDBTreePanel->DestroyWindow();
		SAFE_DELETE(mPDBTreePanel);
	}
	if ( mSelectPanel && mSelectPanel->GetSafeHwnd() )
	{
		mSelectPanel->Delete();
		mSelectPanel->DestroyWindow();
		SAFE_DELETE(mSelectPanel);
	}
	if ( mResiduePanel && mResiduePanel->GetSafeHwnd() )
	{
		mResiduePanel->Delete();
		mResiduePanel->DestroyWindow();
		SAFE_DELETE(mResiduePanel);
	}
}
void CProteinVistaView::OnPaint( )
{
	UNREFERENCED_PARAMETER(m_pProteinVistaRenderer);
	if(m_pProteinVistaRenderer !=NULL)
	{
	   m_pProteinVistaRenderer->Render3DEnvironment();
	   m_pProteinVistaRenderer->HandlePossibleSizeChange();
	}
}
void CProteinVistaView::OnSizeChange()
{
	/*UNREFERENCED_PARAMETER(m_pProteinVistaRenderer);
	if(m_pProteinVistaRenderer !=NULL)
	{
		m_pProteinVistaRenderer->HandlePossibleSizeChange();
	}*/
}
CHTMLListCtrl * CProteinVistaView::GetSelectList()
{
    return this->mSelectPanel->m_htmlListCtrl;
}
CSelectionListPane * CProteinVistaView::GetSelectPanel()
{
	return this->mSelectPanel;
}

void CProteinVistaView::CreateResiduePanel(HWND hWnd)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CWnd* owner=CWnd::FromHandle(hWnd);
 
	CRect rect;
	owner->GetClientRect(rect);
	this->mResiduePanel->Create(_T("STATIC"), NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,rect.Width(), rect.Height()), owner, 0);
}
void CProteinVistaView::ResiduesSizeChange(int cx, int cy)
{
	this->mResiduePanel->MoveWindow(0, 0, cx, cy);
	this->mResiduePanel->ChangeSize(2,cx,cy);
}
void CProteinVistaView::CreateSelectPanel(HWND hWnd)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CWnd* owner=CWnd::FromHandle(hWnd);

	CRect rect;
	owner->GetClientRect(rect);
	this->mSelectPanel->Create(_T("STATIC"), NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,rect.Width(), rect.Height()), owner, 0);
}
void CProteinVistaView::SelectPanelSizeChange(int cx, int cy)
{
	this->mSelectPanel->MoveWindow(0, 0, cx, cy);
	this->mSelectPanel->ChangeSize(2,cx,cy);
}
void CProteinVistaView::CreatePDBTreePanel(HWND hWnd)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CWnd* owner=CWnd::FromHandle(hWnd);

	CRect rect;
	owner->GetClientRect(rect);
	this->mPDBTreePanel->Create(_T("STATIC"), NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,rect.Width(), rect.Height()), owner, 0);     
}
void CProteinVistaView::PDBTreePanelSizeChange(int cx, int cy)
{
	this->mPDBTreePanel->MoveWindow(0, 0, cx, cy);
	this->mPDBTreePanel->ChangeSize(2,cx,cy);
}

void CProteinVistaView::CreatePanel() 
{
	this->m_pProteinVistaRenderer = new CProteinVistaRenderer (this);
	this->m_pProteinVistaRenderer->m_hWnd =::GetMainApp()->m_CanvsHandle;
	this->m_pProteinVistaRenderer->m_hWndFocus =::GetMainApp()->m_CanvsHandle;
	this->m_pProteinVistaRenderer->m_bCreateMultithreadDevice = true;

	HINSTANCE hInstance =AfxGetInstanceHandle();//::GetMainApp()->m_HInstance;
	HRESULT hr = m_pProteinVistaRenderer->Create(hInstance);
	
	if ( FAILED(hr) )
		return;
	this->m_bCreateRender=TRUE;
		 
	CPDB * pPDB = (CPDB*) this->m_arrayPDB[0];
	CPDBRenderer * pPDBRenderer = this->m_pProteinVistaRenderer->AddPDBRenderer(pPDB);
	
	mSelectPanel->Init(m_pProteinVistaRenderer);
	mResiduePanel->Init(m_pProteinVistaRenderer);
    mPDBTreePanel->Init(m_pProteinVistaRenderer);

	m_pProteinVistaRenderer->SetInitialDefaultSelection(pPDBRenderer, CSelectionDisplay::RIBBON );

	OnPDBChanged();

	CString headInfo = GetPDBHeaderInfo();
    if(this->mGetHeaderInfoMethod)
	{
		this->mGetHeaderInfoMethod(headInfo);
	}
	this->m_pProteinVistaRenderer->m_pPropertyScene->m_iAntialiasing =3;
	this->m_pProteinVistaRenderer->SetAntialiasing();
	this->RenderProteinVistaRenderer();
    
}
 
LRESULT CProteinVistaView::OnChangeDisplayMode(WPARAM wParam, LPARAM lParam)
{
	int mode = wParam;
	CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)lParam;

	pSelectionDisplay->ChangeDisplayMode(mode);
	return 0;
}
/////////////////////////////////////////////////////////////
void CProteinVistaView::DeleteContents() 
{
	for ( int i = 0 ; i < m_arrayPDB.size() ; i++ )
	{
		delete m_arrayPDB[i];
	}
	m_arrayPDB.clear();
}
LRESULT CProteinVistaView::DefWindowProc(UINT message, WPARAM wParam, LPARAM lParam) 
{
	if ( m_pProteinVistaRenderer )
	{
		m_pProteinVistaRenderer->MsgProc(GetMainApp()->m_CanvsHandle, message, wParam, lParam);
	}
	return TRUE;
}
void CProteinVistaView::RenderProteinVistaRenderer()
{
	static FLOAT fAppTimeOld = 0.0f;
	static FLOAT fAppTimeInfo = 0.0f;

	FLOAT fAppTime = DXUtil_Timer( TIMER_GETAPPTIME );

	if ( g_bDisplayFPS == TRUE )	
		g_bRequestRender = TRUE;

	if ( g_bRequestRender == TRUE )		 
	{
		g_bRequestRender = FALSE;		 

		if ( m_pProteinVistaRenderer && m_pProteinVistaRenderer->GetD3DDevice() )
		{
			m_pProteinVistaRenderer->Render3DEnvironment();
		}

		fAppTimeOld = fAppTime;
	}
 
	if ( g_bDisplayFPS == TRUE )	
	{
		if ( fAppTime - fAppTimeOld > (1.0f/1.0f) )
		{
			if ( m_pProteinVistaRenderer && m_pProteinVistaRenderer->GetD3DDevice() )
			{
				m_pProteinVistaRenderer->Render3DEnvironment();
			}

			fAppTimeOld = fAppTime;
		}
	}

	if ( fAppTime - fAppTimeInfo > 0.5f )
	{
		fAppTimeInfo = fAppTime;
	}
}


void CProteinVistaView::OnPDBChanged()
{
	UpdateActivePDBComboBox();
	UpdateAllPanes();
	
}
void CProteinVistaView::SetTimelinePos()
{
	g_bRequestRender = TRUE;
}
void CProteinVistaView::OnAddPdb(array<String^>^ pdbFiles ) 
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (pdbFiles == nullptr || pdbFiles->Length ==0)
		return;
	CStringArray strFileArray;
	for(int i=0;i<pdbFiles->Length;i++)
	{
		strFileArray.Add(MStrToCString(pdbFiles[i]));
	}

	if(strFileArray.GetSize() == 0) return;
	if ( strFileArray.GetSize() > 0 )
	{
		//	
		CStringArray	strFileArrayNew;
		for ( int i = 0 ; i < strFileArray.GetSize() ; i++ )
		{
			CString strNewFilename;
			if ( ::GetMainApp()->CheckExistPDBFile(strFileArray[i], strNewFilename) == TRUE )
				strFileArrayNew.Add(strNewFilename);
		}
		if ( strFileArrayNew.GetSize() > 0 )
		{
			for(int i = 0; i < strFileArrayNew.GetSize(); i++)
			{
				AddPDB(strFileArrayNew[i]);
			}
			this->OnPaint();
		}
	}
}
void CProteinVistaView::OnAddPdb() 
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	// 파일 필터 설정
	static char szFilter[] = "PDB Files (*.ent;*.pdb)|*.ent; *.pdb|All Files (*.*)|*.*||";
	// OpenFile 다이얼로그를 띄움
	CFileDialogExt	fileDialog(TRUE, "ent" , "*.ent" , OFN_ALLOWMULTISELECT , szFilter , NULL );

	long ret = fileDialog.DoModal();
	CStringArray strFileArray;	// 파일 이름을 저장하기 위한 Array

	if (ret == IDOK)
	{
		// 선택된 파일이름을 Array에 저장
		POSITION pos = fileDialog.GetStartPosition();
		while(pos != NULL)
		{
			CString name = fileDialog.GetNextPathName(pos);
			POSITION pos = fileDialog.GetStartPosition();
			strFileArray.Add(name);
		}
	}	
	if ( ret == IDCANCEL && fileDialog.m_bDownloadPDB == TRUE )
	{	 
		for ( int i = 0 ; i < fileDialog.m_strArrayPDBID.GetSize() ; i++ )
		{
			strFileArray.Add(fileDialog.m_strArrayPDBID[i]);
		}
	}
	
	if(strFileArray.GetSize() == 0) return;
	if ( strFileArray.GetSize() > 0 )
	{
		//	
		CStringArray	strFileArrayNew;
		for ( int i = 0 ; i < strFileArray.GetSize() ; i++ )
		{
			CString strNewFilename;
			if ( ::GetMainApp()->CheckExistPDBFile(strFileArray[i], strNewFilename) == TRUE )
				strFileArrayNew.Add(strNewFilename);
		}
		if ( strFileArrayNew.GetSize() > 0 )
		{
			for(int i = 0; i < strFileArrayNew.GetSize(); i++)
			{
				AddPDB(strFileArrayNew[i]);
			}
			this->OnPaint();
		}
	}
}

void CProteinVistaView::AddPDB(CString & strFilename)
{
	CSTLArrayPDB & arrayPDB =this->m_arrayPDB;

	CPDB * pPDB = NULL;
	for ( int i = 0 ; i < arrayPDB.size() ; i++ )
	{
		if ( strFilename.CompareNoCase(arrayPDB[i]->m_strFilename) == 0 )
		{
			pPDB = arrayPDB[i];
			break;
		}
	}
	if ( pPDB == NULL )
	{
		pPDB = new CPDB;
		HRESULT hr = pPDB->Load(strFilename);
		if ( SUCCEEDED(hr) )
		{
			//	구조 분석을 한다.
			pPDB->AnalizeStructure();
			this->m_arrayPDB.push_back((CPDB*)pPDB);
		}
		else
		{
			delete pPDB;
			return;
		}
	}
	
	CPDBRenderer * pPDBRenderer = m_pProteinVistaRenderer->AddPDBRenderer((CPDB*)pPDB);
	OnPDBChanged();
	m_pProteinVistaRenderer->SetInitialDefaultSelection(pPDBRenderer, CSelectionDisplay::WIREFRAME );

}



void CProteinVistaView::OnClosePdb(BOOL bRefresh) 
{
	if(m_pProteinVistaRenderer==NULL)
		return;
	if(GetMainApp()->m_bOpenWorkSpace==TRUE)
	{
		AfxMessageBox("Workspace is loading,please wait.");
		return;
	}
	//AFX_MANAGE_STATE(AfxGetStaticModuleState());
	/*CSelectPDBDialog* selectPDBDialog = new CSelectPDBDialog(m_pProteinVistaRenderer);
	selectPDBDialog->InitOption(FALSE, FALSE, -1, CSelectPDBDialog::DISPLAY_PDB);
	if ( selectPDBDialog->DoModal() == IDCANCEL )
		return;

	CSTLArraySelectionInst selectionInst;
	selectPDBDialog->GetSelection (selectionInst);*/
	CSTLArraySelectionInst selectionInst;
	for(int nPDB = 0 ; nPDB < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); nPDB++)
	{
		CPDBRenderer * pPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[nPDB];
		CPDBInst * pPDBInst = pPDBRenderer->GetPDBInst();
		selectionInst.push_back(pPDBInst);
	}

	m_pProteinVistaRenderer->DeSelectAllAtoms();
	for ( int i = 0 ; i < selectionInst.size() ; i++ )
	{
		CPDBInst * pPDBInst = dynamic_cast<CPDBInst *>(selectionInst[i]);
		ClosePDBnChild(pPDBInst->m_pPDBRenderer);
	}
	for ( long iPDB = 0 ; iPDB < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); iPDB++ )
	{
		CPDBRenderer * pPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[iPDB];
		if ( pPDBRenderer == NULL )
			continue;

		if ( pPDBRenderer->GetPDBInst()->GetPDB()->m_bioUnitMatrix.size() > 0 &&
			pPDBRenderer->m_arrayPDBRendererChildBioUnit.size() == 0 &&
			pPDBRenderer->m_pPDBRendererParentBioUnit == NULL )
		{	 
			pPDBRenderer->m_bAttatchBioUnit = FALSE;
			pPDBRenderer->m_biounitCenter = D3DXVECTOR3(0,0,0);
			pPDBRenderer->m_bioUnitMinMaxBB[0] = D3DXVECTOR3(0,0,0);
			pPDBRenderer->m_bioUnitMinMaxBB[1] = D3DXVECTOR3(0,0,0);
			pPDBRenderer->m_biounitRadius = 0.0f;
			D3DXMatrixIdentity(&(pPDBRenderer->m_matTransformBioUnit));
		}
	}
	g_bRequestRender = TRUE;
	selectionInst.clear();
	this->DeleteContents();
     
	if(bRefresh==TRUE)
	{
		this->OnPaint();
	}
	mProperty = nullptr;
	if(this->mPropertyChanged)
	{
		this->mPropertyChanged();
	} 
	m_pProteinVistaRenderer->Cleanup3DEnvironment();
	SAFE_DELETE(m_pProteinVistaRenderer);
}
void CProteinVistaView::ClosePDBnChild(CPDBRenderer * pPDBRenderer)
{
	m_pProteinVistaRenderer->DeSelectAllAtoms();
	if ( pPDBRenderer )
	{
		
		for ( int iChild = 0 ; iChild < pPDBRenderer->m_arrayPDBRendererChildBioUnit.size(); iChild ++ )
		{
			CPDBRenderer * pPDBRendererChild = pPDBRenderer->m_arrayPDBRendererChildBioUnit[iChild];
			ClosePdb(pPDBRendererChild);
			OnPDBChanged();
		}		
		if ( pPDBRenderer->m_pPDBRendererParentBioUnit != NULL )
		{
			CPDBRenderer * pPDBRendererParent = pPDBRenderer->m_pPDBRendererParentBioUnit;
			for ( int iChild = 0 ; iChild < pPDBRendererParent->m_arrayPDBRendererChildBioUnit.size(); iChild ++ )
			{
				if ( pPDBRendererParent->m_arrayPDBRendererChildBioUnit[iChild] == pPDBRenderer )
				{
					pPDBRendererParent->m_arrayPDBRendererChildBioUnit.erase(pPDBRendererParent->m_arrayPDBRendererChildBioUnit.begin()+iChild);
					break;
				}
			}
		}
		ClosePdb(pPDBRenderer);
		OnPDBChanged();
	}
}

void CProteinVistaView::ClosePdb(CPDBRenderer * pPDBRenderer)
{
    DeleteSelectionList(pPDBRenderer->GetPDBInst());
	m_pProteinVistaRenderer->DeletePDBRenderer(pPDBRenderer);
	/*for ( int iPDB = 0 ; iPDB < this->m_arrayPDB.size(); iPDB ++ )
	{
		if ( m_arrayPDB[iPDB] == pPDBRenderer )
		{
			m_arrayPDB.RemoveAt(iPDB);
			delete pPDBRenderer;
			break;
		}
	}*/
}
void CProteinVistaView::DeleteSelectionList(CPDBInst * pPDBInst)
{
	CHTMLListCtrl* htmlListCtrl =this->mSelectPanel->m_htmlListCtrl;
	for ( int i = 0 ; i < htmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *) htmlListCtrl->GetItemData(i);
		if ( pSelectionDisplay )
		{
			for ( int iAtom = 0 ; iAtom < pSelectionDisplay->m_arrayAtomInst.size() ; iAtom ++ )
			{
				if ( pSelectionDisplay->m_arrayAtomInst[iAtom]->m_pPDBInst == pPDBInst )
				{	//	bingo.
					mSelectPanel->DeleteSelectionDisplay(pSelectionDisplay);
					break;
				}
			}
		}
	}
}


HRESULT CProteinVistaView::GetSelectedAtoms(CSTLArrayAtomInst &atomPtrArray)
{
	atomPtrArray.clear();
	atomPtrArray.reserve(1000);

	for ( long iPDB = 0 ; iPDB < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); iPDB++ )
	{
		CPDBRenderer * pPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[iPDB];
		if ( pPDBRenderer == NULL )
			return E_FAIL;

		CPDBInst * pPDBInst = pPDBRenderer->GetPDBInst();
		if ( pPDBInst == NULL )
			return E_FAIL;

		for ( int iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
		{
			long nChain = pPDBInst->GetPDB()->GetNumChain(iModel);
			for ( int i = 0 ; i < nChain ; i++ )
			{
				CChainInst * pChainInst = pPDBInst->GetChainInst(iModel,i);
				for ( int j = 0 ; j < pChainInst->m_arrayAtomInst.size() ; j++ )
				{
					CAtomInst * pAtomInst = pChainInst->m_arrayAtomInst[j];
					if ( pAtomInst->GetSelect() == TRUE )
						atomPtrArray.push_back(pAtomInst);
				}
			}
		}
	}

	return S_OK;
}

HRESULT	CProteinVistaView::SetSelectedAtoms ( CString &strPDBID, CSTLArrayAtomInst &atomPtrArray )
{
	CPDBRenderer * pPDBRenderer = NULL;
	if ( strPDBID.IsEmpty() == TRUE)
	{
		pPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[m_pProteinVistaRenderer->m_arrayPDBRenderer.size()-1];
	}
	else
	{
		for ( int i =0 ; i < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); i++ )
		{
			if ( m_pProteinVistaRenderer->m_arrayPDBRenderer[i]->GetPDBInst()->GetPDB()->m_strFilename == strPDBID )
			{
				pPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[i];
				break;
			}
		}
	}

	if ( pPDBRenderer == NULL )
		return E_FAIL;

	CPDBInst * pPDBInst = pPDBRenderer->GetPDBInst();
	if ( pPDBInst == NULL )
		return E_FAIL;

	for ( int iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
	{
		long nChain = pPDBInst->GetPDB()->GetNumChain(iModel);
		for ( int i = 0 ; i < nChain ; i++ )
		{
			CChainInst * pChainInst = pPDBInst->GetChainInst(iModel,i);
			for ( int j = 0 ; j < pChainInst->m_arrayAtomInst.size() ; j++ )
			{
				CAtomInst * pAtom = pChainInst->m_arrayAtomInst[j];

				for ( int k = 0 ; k < atomPtrArray.size(); k++ )
				{
					if ( atomPtrArray[k] == pAtom )
					{
						pAtom->SetSelect(TRUE);
						break;
					}
				}
			}
		}
	}

	return S_OK;
}

HRESULT CProteinVistaView::UnSelectedAtoms()
{
	m_pProteinVistaRenderer->SelectAll(FALSE);
	return S_OK;
}
int CProteinVistaView::GetNumPDBID(CString PDBID)
{
	for(int i = 0; i < this->m_arrayPDB.size(); i++)
	{
		CPDB * pPDB = this->m_arrayPDB[i];

		if(PDBID == pPDB->m_strPDBID)
			return i;
	}

	return 0;
}
void CProteinVistaView::ChanegPropertyValue(long id,CString value,CSelectionDisplay * m_pSelectionDisplay=NULL)
{
	if(!m_pSelectionDisplay && this->m_pProteinVistaRenderer !=NULL)
	{
		switch( id )
		{
		case PROPERTY_SCENE_BACKGROUND_COLOR:
			if ( this->m_pProteinVistaRenderer )
			{
				this->m_pProteinVistaRenderer->m_pSkyManager->ChangeSphereColor();
			}
			break;
		case PROPERTY_SCENE_BACKGROUND:
			//	변수와 연결. 처리됨.
			break;

		case PROPERTY_SCENE_BACKGROUND_FILENAME:
			{
				this->m_pProteinVistaRenderer->m_pSkyManager->DeleteDeviceObjects();
				this->m_pProteinVistaRenderer->m_pSkyManager->InitDeviceObjects();
			}
			break;

		case PROPERTY_SCENE_MODEL_QUALITY:
			this->m_pProteinVistaRenderer->SetModelQuality();
			break;
		case PROPERTY_SCENE_SHADER_QUALITY:
			this->m_pProteinVistaRenderer->SetShaderQuality();
			break;
		case PROPERTY_SCENE_SELECTION_SHOW:
			this->m_pProteinVistaRenderer->SetShowSelectionMark();
			break;

		case PROPERTY_SCENE_SELECTION_COLOR:
			this->m_pProteinVistaRenderer->SetSelectionColor();
			break;

		case PROPERTY_SCENE_SSAO_ENABLE:
			{
				this->m_pProteinVistaRenderer->Reset3DEnvironment();
			}
			break;
		case PROPERTY_SCENE_SSAO_HALF_SIZE_BLUR:
			{
				this->m_pProteinVistaRenderer->Reset3DEnvironment();
			}
			break;

		case PROPERTY_SCENE_DEPTH_OF_FIELD:
		case PROPERTY_SCENE_FOG_COLOR:
		case PROPERTY_SCENE_FOG_START:
		case PROPERTY_SCENE_FOG_END:
			this->m_pProteinVistaRenderer->SetFog();
			break;

		case PROPERTY_SCENE_NEAR_CLIP_PLANE:
		case PROPERTY_SCENE_FAR_CLIP_PLANE:

			//	sky box를 far clip에 맞춰서 그린다.
			this->m_pProteinVistaRenderer->m_pSkyManager->DeleteDeviceObjects();
			this->m_pProteinVistaRenderer->m_pSkyManager->InitDeviceObjects();

			this->m_pProteinVistaRenderer->SetViewVolume();
			break;

		case PROPERTY_SCENE_CAMERA_SIZE_VIEW_VOL:
		case PROPERTY_SCENE_FOV:
		case PROPERTY_SCENE_CAMERA_TYPE:
			if ( PROPERTY_SCENE_CAMERA_TYPE == id )
			{
				//	change name of property.
				CPropertyScene * propertyScene = dynamic_cast<CPropertyScene*>(this->m_pProteinVistaRenderer);
				if ( propertyScene )
				{
					if ( propertyScene->m_cameraType == CPropertyScene::CAMERA_PERSPECTIVE )
					{
						//propertyScene->m_pItemFOV->SetHidden(FALSE);
						//propertyScene->m_pItemOthoCameraViewVol->SetHidden(TRUE);
					}
					else
					{
						//propertyScene->m_pItemFOV->SetHidden(TRUE);
						//propertyScene->m_pItemOthoCameraViewVol->SetHidden(FALSE);
					}
				}
			}
			this->m_pProteinVistaRenderer->SetViewVolume();
			break;

		case PROPERTY_SCENE_DISPLAY_AXIS:
			//	변수와 연결. 자동 처리됨.
			break;

		case PROPERTY_SCENE_DISPLAY_ANTIALIASING:
			this->m_pProteinVistaRenderer->SetAntialiasing();
			break;

		case PROPERTY_SCENE_LIGHT1_ENABLE:
		case PROPERTY_SCENE_LIGHT2_ENABLE:
		case PROPERTY_SCENE_LIGHT1_COLOR:
		case PROPERTY_SCENE_LIGHT2_COLOR:
		case PROPERTY_SCENE_LIGHT1_INTENSITY:
		case PROPERTY_SCENE_LIGHT2_INTENSITY:
			{		
				this->m_pProteinVistaRenderer->SetShaderLight();
			}
			break;

		case PROPERTY_SCENE_LIGHT1_RADIUS:
			{
				//CXTPPropertyGridItemDouble* pItem = (CXTPPropertyGridItemDouble*)lParam;
				//this->m_pProteinVistaRenderer->m_LightControl[0].SetRadius((float)(pItem->GetDouble()));
			}
			break;

		case PROPERTY_SCENE_LIGHT2_RADIUS:
			{
				//CXTPPropertyGridItemDouble* pItem = (CXTPPropertyGridItemDouble*)lParam;
				//this->m_pProteinVistaRenderer->m_LightControl[1].SetRadius((float)(pItem->GetDouble()));
			}
			break;

		case PROPERTY_SCENE_ENABLE_CLIPPING0:
		case PROPERTY_SCENE_CLIPPING0_SHOW:
		case PROPERTY_SCENE_CLIPPING0_COLOR:
		case PROPERTY_SCENE_CLIPPING0_TRANSPARENCY:
		case PROPERTY_SCENE_CLIPPING0_DIRECTION:
			this->m_pProteinVistaRenderer->SetGlobalClipPlane();
			break;
		case PROPERTY_SCENE_PLANE0_EQUATION:
			{
				CProteinVistaRenderer * pProteinVistaRenderer = GetProteinVistaRenderer();
				CPropertyScene	* pPropertyScene = NULL;
				if ( pProteinVistaRenderer )
				{
					pPropertyScene = pProteinVistaRenderer->m_pPropertyScene;
				}

				if ( pPropertyScene )
				{
					CString strPlaneEqu =value;// pItem->GetValue();

					CString resToken;
					int curPos = 0;

					CSTLFLOATArray	PlaneEqu;

					resToken= strPlaneEqu.Tokenize(_T(","),curPos);
					while (resToken != _T(""))
					{
						PlaneEqu.push_back((float)(atof(resToken)));
						resToken = strPlaneEqu.Tokenize(_T(","), curPos);
					};   

					//    m_clipPlaneEquation1
					if ( PlaneEqu.size() == 4 )
					{
						pPropertyScene->m_clipPlaneEquation0 = D3DXPLANE(PlaneEqu[0],PlaneEqu[1],PlaneEqu[2],PlaneEqu[3] );
						//pProteinVistaRenderer->m_pClipPlane->SetPlaneEquation(pPropertyScene->m_clipPlaneEquation0);
						pProteinVistaRenderer->SetGlobalClipPlane();
					}
				}
			}

			break;

		case PROPERTY_SCENE_DISPLAY_HETATM:
			this->m_pProteinVistaRenderer->SetDisplayHETATM();
			break;
		}
	}
	else
	{
	}
}
 