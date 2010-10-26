// ProteinVista.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "Interface.h"
#include "Utility.h"
#include "PDBTreePane.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "ClipPlane.h"
#include "SelectionListPane.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



#define PIW_HEADER		(_T("Protein Insight Workspace"))
#define PIW_VERSION		121

void CProteinVistaApp::OnSaveWorkspace(void)
{
	if ( m_strPathWorkspace.IsEmpty() == TRUE )
		OnSaveAsWorkspace();
	else
		SaveWorkspaceFile(m_strPathWorkspace);

}

void CProteinVistaApp::OnSaveAsWorkspace(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	//	pdb filename 저장.
	CProteinVistaRenderer * pProteinVistaRenderer = GetProteinVistaRenderer();
	if ( pProteinVistaRenderer == NULL )
		return;

	CString strFilename;
	for ( int i = 0 ; i < pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
	{
		strFilename += pProteinVistaRenderer->m_arrayPDBRenderer[i]->GetPDBInst()->GetPDB()->m_strPDBID;
		strFilename += _T("_");
	}

	CTime t = CTime::GetCurrentTime();   
	strFilename += t.Format(_T("%m%d%y"));

	// pdb파일을 저장할 디렉토리 설정
	CString filename = m_strBaseWorkspacePath+ strFilename + _T(".piw");

	static char BASED_CODE szFilter[] = "Workspace Files (*.piw)|*.piw||";

	CWnd* ownerWnd =CWnd::FromHandle(this->m_CanvsHandle);
	CFileDialog	fileDialog(FALSE, "piw", filename , OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, szFilter , ownerWnd, 0);
	if ( fileDialog.DoModal() == IDCANCEL )
		return;

	m_strPathWorkspace = fileDialog.GetPathName();

	OnSaveWorkspace();
}

void CProteinVistaApp::SaveWorkspaceFile(CString filename)
{
	CProteinVistaRenderer * pProteinVistaRenderer = GetProteinVistaRenderer();
	if ( pProteinVistaRenderer == NULL )
		return;

	CMemFile fileSaveWorkspace(1024*300);	//	about 300K

	fileSaveWorkspace.Write(PIW_HEADER, strlen(PIW_HEADER));

	long version = PIW_VERSION;
	fileSaveWorkspace.Write(&version, sizeof(long));


	int nTexture = 0;
	CMapTextureFilename::iterator it;
	for ( it = pProteinVistaRenderer->m_mapTextureFilename.begin( ); it != pProteinVistaRenderer->m_mapTextureFilename.end( ); it++ )
	{
		nTexture ++;
	}
	fileSaveWorkspace.Write(&nTexture, sizeof(INT));

	for ( it = pProteinVistaRenderer->m_mapTextureFilename.begin( ); it != pProteinVistaRenderer->m_mapTextureFilename.end( ); it++ )
	{
		CTextureInfo* textureInfo = it->second;
		textureInfo->Save(&fileSaveWorkspace, textureInfo->m_strFilename);
	}

	//    pdbs
	long nPDB = pProteinVistaRenderer->m_arrayPDBRenderer.size();
	fileSaveWorkspace.Write(&nPDB, sizeof(long));

	long iProgress =GetMainActiveView()->InitProgress(100);

	//	파일이름 및 transform 저장
	for ( int i = 0 ; i < pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
	{
		GetMainActiveView()->SetProgress( (i+1)*100/pProteinVistaRenderer->m_arrayPDBRenderer.size(), iProgress );

		CPDBRenderer * pPDBRenderer = pProteinVistaRenderer->m_arrayPDBRenderer[i];
		CString strFilename = pPDBRenderer->GetPDBInst()->GetPDB()->m_strFilename;
		CString strPDBID = pPDBRenderer->GetPDBInst()->GetPDB()->m_strPDBID;

		D3DXMATRIX matWorld = pPDBRenderer->m_matWorld;

		WriteString(fileSaveWorkspace, strFilename);
		WriteString(fileSaveWorkspace, strPDBID);

		fileSaveWorkspace.Write(&pPDBRenderer->m_matWorld, sizeof(D3DXMATRIXA16));
		fileSaveWorkspace.Write(&pPDBRenderer->m_matWorldPrevious, sizeof(D3DXMATRIXA16));
		fileSaveWorkspace.Write(&pPDBRenderer->m_matWorldUserInput, sizeof(D3DXMATRIXA16));
		fileSaveWorkspace.Write(&pPDBRenderer->m_matPDBCenter, sizeof(D3DXMATRIXA16));
		fileSaveWorkspace.Write(&pPDBRenderer->m_matPDBCenterInv, sizeof(D3DXMATRIXA16));

		fileSaveWorkspace.Write(&pPDBRenderer->m_selectionCenterTransformed, sizeof(D3DXVECTOR3));

		fileSaveWorkspace.Write(&pPDBRenderer->m_bSelected, sizeof(BOOL));
		fileSaveWorkspace.Write(&pPDBRenderer->m_bSelectionRotCenter, sizeof(BOOL));
		fileSaveWorkspace.Write(&pPDBRenderer->m_iBioUnit, sizeof(long));				
		fileSaveWorkspace.Write(&pPDBRenderer->m_bAttatchBioUnit, sizeof(BOOL));
		fileSaveWorkspace.Write(&pPDBRenderer->m_biounitCenter, sizeof(D3DXVECTOR3));
		fileSaveWorkspace.Write(&pPDBRenderer->m_matTransformBioUnit, sizeof(D3DXMATRIXA16));
	}

	GetMainActiveView()->EndProgress(iProgress);

	iProgress =GetMainActiveView()->InitProgress(100);
	//	biomolecule 저장
	//	biomolecule의 child들.
	for ( int i = 0 ; i < pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
	{
		GetMainActiveView()->SetProgress( (i+1)*100/pProteinVistaRenderer->m_arrayPDBRenderer.size(), iProgress );

		CPDBRenderer * pPDBRenderer = pProteinVistaRenderer->m_arrayPDBRenderer[i];
		long nBioUnit = pPDBRenderer->m_arrayPDBRendererChildBioUnit.size();
		fileSaveWorkspace.Write(&nBioUnit, sizeof(long));

		for ( int j = 0 ; j < pPDBRenderer->m_arrayPDBRendererChildBioUnit.size() ; j++ )
		{
			CPDBRenderer * pPDBRendererChild = pPDBRenderer->m_arrayPDBRendererChildBioUnit[j];

			//	pPDBRendererChild 가 몇번째 인가를 확인.
			long iChild = 0;
			for ( iChild = 0 ; iChild < pProteinVistaRenderer->m_arrayPDBRenderer.size() ; iChild ++ )
			{
				if ( pPDBRendererChild == pProteinVistaRenderer->m_arrayPDBRenderer[iChild] )
				{
					break;
				}
			}
			fileSaveWorkspace.Write(&iChild, sizeof(long));
		}
	}

	//	
	//	save camera position & rotation
	//
	fileSaveWorkspace.Write(&pProteinVistaRenderer->m_FromVec, sizeof(D3DXVECTOR3));
	fileSaveWorkspace.Write(&pProteinVistaRenderer->m_ToVec, sizeof(D3DXVECTOR3));
	fileSaveWorkspace.Write(&pProteinVistaRenderer->m_matCameraRotation, sizeof(D3DXMATRIXA16));
	fileSaveWorkspace.Write(&pProteinVistaRenderer->m_matCameraRotationTemp, sizeof(D3DXMATRIXA16));
	fileSaveWorkspace.Write(&pProteinVistaRenderer->m_radius, sizeof(FLOAT));
	fileSaveWorkspace.Write(&pProteinVistaRenderer->m_cameraZPos, sizeof(FLOAT));

	GetMainActiveView()->EndProgress(iProgress);

	long numSelection = 0;
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = pProteinVistaRenderer->m_arraySelectionDisplay[i];
		if ( pSelectionDisplay )
			numSelection ++;
	}

	fileSaveWorkspace.Write(&numSelection, sizeof(long));

	iProgress =GetMainActiveView()->InitProgress(100);

	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		GetMainActiveView()->SetProgress( (i+1)*100/MAX_DISPLAY_SELECTION_INDEX, iProgress );

		CSelectionDisplay * pSelectionDisplay = pProteinVistaRenderer->m_arraySelectionDisplay[i];
		if ( pSelectionDisplay )
		{
			CString strPDBID = pSelectionDisplay->m_pPDBRenderer->GetPDBInst()->GetPDB()->m_strPDBID;
			//	pAtom->m_serial 를 구분하기 위해 PDB ID를 넣어준다.
			WriteString(fileSaveWorkspace, strPDBID);

			//	index pdbRenderer
			for ( int iFind = 0 ; iFind < pProteinVistaRenderer->m_arrayPDBRenderer.size(); iFind++ )
			{
				if ( pSelectionDisplay->m_pPDBRenderer == pProteinVistaRenderer->m_arrayPDBRenderer[iFind] )
				{
					break;
				}
			}

			//	CPDBRender의 index를 저장.
			fileSaveWorkspace.Write(&iFind, sizeof(long));

			fileSaveWorkspace.Write(&pSelectionDisplay->m_displayStyle, sizeof(long));
			fileSaveWorkspace.Write(&pSelectionDisplay->m_center, sizeof(D3DXVECTOR3));
			fileSaveWorkspace.Write(&pSelectionDisplay->m_radius, sizeof(FLOAT));

			long numAtom = pSelectionDisplay->m_arrayAtomInst.size();
			fileSaveWorkspace.Write(&numAtom , sizeof(long));

			for ( int iAtom = 0 ; iAtom < numAtom ; iAtom ++ )
			{
				CAtom * pAtom = pSelectionDisplay->m_arrayAtomInst[iAtom]->GetAtom();
				LARGE_INTEGER li;
				li.LowPart = pAtom->m_serial; 
				li.HighPart = pAtom->m_iModel;

				fileSaveWorkspace.Write(&(li.QuadPart), sizeof(LONGLONG));
			}

			fileSaveWorkspace.Write(&(pSelectionDisplay->m_bShow), sizeof(BOOL));

			//	
			//	property를 저장한다.
			//
			pSelectionDisplay->GetPropertyCommon()->Save(&fileSaveWorkspace);
			pSelectionDisplay->m_pRenderProperty->Save(&fileSaveWorkspace);

			//	Clip Plane 을 저장한다.
			pSelectionDisplay->m_pClipPlane1->Save(fileSaveWorkspace);
			pSelectionDisplay->m_pClipPlane2->Save(fileSaveWorkspace);
		}
	}

	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = pProteinVistaRenderer->m_arraySelectionDisplay[i];
		if ( pSelectionDisplay )
		{
			fileSaveWorkspace.Write(&pSelectionDisplay->m_iSerial, sizeof(long));
		}
	}

	fileSaveWorkspace.Write(&(CSelectionDisplay::m_maxSelectionIndex), sizeof(long));

	long iSelect = GetMainActiveView()->GetSelectList()->GetSelectedItem();
	fileSaveWorkspace.Write(&iSelect, sizeof(long));

	pProteinVistaRenderer->m_pPropertyScene->Save(&fileSaveWorkspace);
	pProteinVistaRenderer->m_pClipPlane->Save(fileSaveWorkspace);
	for ( int i = 0 ; i < MAX_LIGHTS ; i++ )
	{
		pProteinVistaRenderer->m_LightControl[i].Save(fileSaveWorkspace);
	}

	GetMainActiveView()->EndProgress(iProgress);

	//	write in real file.
	CFile fileSave;
	if ( fileSave.Open(filename , CFile::modeCreate |  CFile::modeWrite ) == FALSE )
	{
		AfxMessageBox("Cannot open File. Check Read-Only option");
		return;
	}

	LONGLONG lenFile = fileSaveWorkspace.GetLength();
	BYTE * pData = fileSaveWorkspace.Detach();

	fileSave.Write(pData, lenFile);

	fileSave.Close();
	fileSaveWorkspace.Close();

	free(pData);
	m_strPathWorkspace =filename;
	////
	//CView * pView =  GetMainActiveView();
	//if ( pView )
	//{
	//	CDocument * pDoc = pView->GetDocument();
	//	pDoc->SetPathName(m_strPathWorkspace, TRUE);
	//}
	::OutputTextMsg("Save successfully.");
}

void CProteinVistaApp::OnOpenWorkspace(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CString filename = m_strBaseWorkspacePath+ "*.piw";
	static char BASED_CODE szFilter[] = "Workspace Files (*.piw)|*.piw||";


	CWnd* ownerWnd =CWnd::FromHandle(this->m_CanvsHandle);
	CFileDialog	fileDialog ( TRUE, "piw", filename , OFN_HIDEREADONLY, szFilter , ownerWnd, 0 );
	if ( fileDialog.DoModal() == IDCANCEL )
		return;

	CString strFilename = fileDialog.GetPathName();

	OpenWorkspaceFile(strFilename);
}
#pragma managed(push,off)
BOOL CProteinVistaApp::OpenWorkspaceFile(CString &strFilename )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CFile fileLoad;
	if ( fileLoad.Open( strFilename, CFile::modeRead ) == FALSE )
		return FALSE;
	LONGLONG lenFile = fileLoad.GetLength();

	BYTE * pData = new BYTE [lenFile];
	if ( pData == NULL )
		return FALSE;

	fileLoad.Read(pData, lenFile);
	fileLoad.Close();

	CMemFile fileLoadWorkspace(0);
	fileLoadWorkspace.Attach(pData, lenFile, 0);

	TCHAR piwHeader[100] = {0,};
	fileLoadWorkspace.Read(piwHeader, strlen(PIW_HEADER));
	if ( CString(piwHeader) != PIW_HEADER )
	{
		AfxMessageBox("This is not a PIW");
		delete[] pData;
		return FALSE;
	}

	long version;
	fileLoadWorkspace.Read(&version, sizeof(long));
	if (version != PIW_VERSION )
	{
		CString strVersion;
		strVersion.Format("%d",version);
		AfxMessageBox("Version is not correct:" +strVersion );
		delete[] pData;

		return FALSE;
	}
	
    this->m_bOpenWorkSpace =TRUE;
	//	전체 doc을 닫는다.
	this->GetActiveProteinVistaView()->OnClosePdb(FALSE);
	m_strPathWorkspace.Empty();

	long iProgress =GetMainActiveView()->InitProgress(100);
	//
	int nTexture;
	fileLoadWorkspace.Read( &nTexture, sizeof(int));
	//    파일을 texture path에 저장한다.
	for ( int i = 0 ; i < nTexture ; i++ )
	{
		CTextureInfo::Load(&fileLoadWorkspace);
	}

	long nPDB;
	fileLoadWorkspace.Read( &nPDB, sizeof(long));

	for ( int i = 0 ; i < nPDB ; i++ )
	{
		GetMainActiveView()->SetProgress( (i+1)*100/nPDB, iProgress );

		CString strFilename;
		ReadString(fileLoadWorkspace, strFilename);

		CString strPDBID;
		ReadString(fileLoadWorkspace, strPDBID);

		D3DXMATRIX			matWorld;
		D3DXMATRIX			matWorldPrevious;
		D3DXMATRIX			matWorldUserInput;
		D3DXMATRIX			matPDBCenter;
		D3DXMATRIX			matPDBCenterInv;

		D3DXVECTOR3         vObjectCenter;

		BOOL				bSelected;
		BOOL				bSelectionRotCenter;
		long				iBioUnit;
		BOOL				bAttatchBioUnit;
		D3DXVECTOR3			vecCenterBioUnit;
		D3DXMATRIXA16		matTransformBioUnit;

		fileLoadWorkspace.Read(&matWorld, sizeof(D3DXMATRIX));
		fileLoadWorkspace.Read(&matWorldPrevious, sizeof(D3DXMATRIX));
		fileLoadWorkspace.Read(&matWorldUserInput, sizeof(D3DXMATRIX));
		fileLoadWorkspace.Read(&matPDBCenter, sizeof(D3DXMATRIX));
		fileLoadWorkspace.Read(&matPDBCenterInv, sizeof(D3DXMATRIX));
		fileLoadWorkspace.Read(&vObjectCenter, sizeof(D3DXVECTOR3));

		fileLoadWorkspace.Read(&bSelected, sizeof(BOOL));
		fileLoadWorkspace.Read(&bSelectionRotCenter, sizeof(BOOL));
		fileLoadWorkspace.Read(&iBioUnit, sizeof(long));				
		fileLoadWorkspace.Read(&bAttatchBioUnit, sizeof(BOOL));
		fileLoadWorkspace.Read(&vecCenterBioUnit, sizeof(D3DXVECTOR3));
		fileLoadWorkspace.Read(&matTransformBioUnit, sizeof(D3DXMATRIXA16));
		//
		CString strFilenameNew;
		if ( CheckExistPDBFile(strFilename, strFilenameNew) == TRUE )
		{
			strFilename = strFilenameNew;
		}

		if ( i==0 )
		{
			this->OpenDocumentFile(strFilename);
		}
		else
		{
			this->GetActiveProteinVistaView() ->AddPDB(strFilename); 
		} 


		CProteinVistaRenderer * pProteinVistaRenderer = GetProteinVistaRenderer();
		if ( pProteinVistaRenderer == NULL )
		{
			this->m_bOpenWorkSpace = FALSE;
			return FALSE;
		}

		CPDBRenderer * pPDBRenderer = pProteinVistaRenderer->m_arrayPDBRenderer[pProteinVistaRenderer->m_arrayPDBRenderer.size()-1];
		pPDBRenderer->m_matWorld = matWorld;
		pPDBRenderer->m_matWorldPrevious = matWorldPrevious;
		pPDBRenderer->m_matWorldUserInput = matWorldUserInput;
		pPDBRenderer->m_matPDBCenter = matPDBCenter;
		pPDBRenderer->m_matPDBCenterInv = matPDBCenterInv;
		pPDBRenderer->m_selectionCenterTransformed = vObjectCenter;

		pPDBRenderer->m_bSelected = bSelected;
		pPDBRenderer->m_bSelectionRotCenter = bSelectionRotCenter;
		pPDBRenderer->m_iBioUnit = iBioUnit;
		pPDBRenderer->m_bAttatchBioUnit = bAttatchBioUnit;
		pPDBRenderer->m_biounitCenter = vecCenterBioUnit;
		pPDBRenderer->m_matTransformBioUnit = matTransformBioUnit;
	}

	GetMainActiveView()->EndProgress(iProgress);


	CProteinVistaRenderer * pProteinVistaRenderer = GetProteinVistaRenderer();
	if ( pProteinVistaRenderer == NULL )
	{
		delete[] pData;
		this->m_bOpenWorkSpace = FALSE;
		return FALSE;
	}
	else
	{

	}

	iProgress =GetMainActiveView()->InitProgress(100);

	for ( int iParent = 0 ; iParent < nPDB ; iParent++ )
	{
		GetMainActiveView()->SetProgress((iParent+1)*100/nPDB, iProgress );

		long nChild;
		fileLoadWorkspace.Read(&nChild, sizeof(long));

		CPDBRenderer * pPDBRendererParent = pProteinVistaRenderer->m_arrayPDBRenderer[iParent];
		for ( int iChild = 0; iChild < nChild; iChild ++ )
		{
			long iPDBRendererChild;
			fileLoadWorkspace.Read(&iPDBRendererChild, sizeof(long));
			pPDBRendererParent->m_arrayPDBRendererChildBioUnit.push_back(pProteinVistaRenderer->m_arrayPDBRenderer[iPDBRendererChild]);
			pProteinVistaRenderer->m_arrayPDBRenderer[iPDBRendererChild]->m_pPDBRendererParentBioUnit = pPDBRendererParent;
		}
	}

	GetMainActiveView()->EndProgress(iProgress);

	fileLoadWorkspace.Read(&pProteinVistaRenderer->m_FromVec, sizeof(D3DXVECTOR3));
	fileLoadWorkspace.Read(&pProteinVistaRenderer->m_ToVec, sizeof(D3DXVECTOR3));
	fileLoadWorkspace.Read(&pProteinVistaRenderer->m_matCameraRotation, sizeof(D3DXMATRIXA16));
	fileLoadWorkspace.Read(&pProteinVistaRenderer->m_matCameraRotationTemp, sizeof(D3DXMATRIXA16));
	fileLoadWorkspace.Read(&pProteinVistaRenderer->m_radius, sizeof(FLOAT));
	fileLoadWorkspace.Read(&pProteinVistaRenderer->m_cameraZPos, sizeof(FLOAT));


	//	selection list.
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = pProteinVistaRenderer->m_arraySelectionDisplay[i];
		if ( pSelectionDisplay )
		{
			if ( GetMainActiveView()->GetSelectPanel() )
				GetMainActiveView()->GetSelectPanel()->DeleteSelectionDisplay(pSelectionDisplay);
		}
	}
	if ( GetMainActiveView()->GetSelectPanel() )
		GetMainActiveView()->GetSelectPanel()->m_htmlListCtrl->DeleteAllItems(); 

	//
	pProteinVistaRenderer->SelectAll(FALSE);


	iProgress =GetMainActiveView()-> InitProgress(100);

	//
	long numSelection;
	fileLoadWorkspace.Read(&numSelection, sizeof(long));

	for ( int i = 0 ; i < numSelection ; i++ )
	{
		GetMainActiveView()->SetProgress((i+1)*100/numSelection, iProgress );

		long		displayStyle;
		D3DXVECTOR3 center;
		FLOAT		radius;
		long		iSerial;

		CString strPDBID;
		ReadString(fileLoadWorkspace, strPDBID);

		//	CPDBRender의 index를 저장.
		long iFind = 0;
		fileLoadWorkspace.Read(&iFind, sizeof(long));

		fileLoadWorkspace.Read(&displayStyle, sizeof(long));
		fileLoadWorkspace.Read(&center, sizeof(D3DXVECTOR3));
		fileLoadWorkspace.Read(&radius, sizeof(FLOAT));

		long nAtom;
		fileLoadWorkspace.Read(&nAtom, sizeof(long));
		CSTLLONGLONGArray stlArrayLongLong;
		stlArrayLongLong.reserve(nAtom);
		for ( int iAtom = 0 ; iAtom < nAtom ; iAtom ++)
		{
			LONGLONG IndexAtom;
			fileLoadWorkspace.Read(&IndexAtom, sizeof(LONGLONG));
			stlArrayLongLong.push_back(IndexAtom);
		}

		//	현재 선택된것을 reset
		pProteinVistaRenderer->SelectAll(FALSE);

		CPDBInst * pPDBInst = pProteinVistaRenderer->m_arrayPDBRenderer[iFind]->GetPDBInst();
		if ( pPDBInst )
		{

			BOOL bIsSelected = FALSE;
			for ( int iAtom = 0 ; iAtom < stlArrayLongLong.size() ; iAtom ++ )
			{
				std::map < LONGLONG , CAtomInst * > :: const_iterator findResult;

				findResult = pPDBInst->m_hashMapAtomInst.find(stlArrayLongLong[iAtom]);
				if ( findResult != pPDBInst->m_hashMapAtomInst.end() )
				{
					((CAtomInst*)(findResult->second))->m_bSelect = TRUE;
					bIsSelected = TRUE;
				}
			}

			//	속도를 위해서 한번만 셋팅하는 방법을 사용.
			if ( bIsSelected == TRUE )
				pPDBInst->m_pPDBRenderer->m_bIsSelectionExist = TRUE;

			if (this->GetActiveProteinVistaView()->GetCPDBTreePane())
				this->GetActiveProteinVistaView()->GetCPDBTreePane()->OnFullSyncPDB(); 

			CSelectionDisplay * pSelectionDisplay = pProteinVistaRenderer->AddCurrentSelection(displayStyle);

			if ( pSelectionDisplay == NULL )
			{
				OutputTextMsg("Error loading piw\n");
			}

			pSelectionDisplay->m_center = center;
			pSelectionDisplay->m_radius = radius;

			BOOL	bShowHide;
			fileLoadWorkspace.Read(&bShowHide, sizeof(BOOL));
			pSelectionDisplay->m_bShow = bShowHide;

			//	property를 read 한다.
			//	
			CPropertyCommon	* pPropertyCommon = pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
				pPropertyCommon->Load(&fileLoadWorkspace);

			pSelectionDisplay->m_pRenderProperty->Load(&fileLoadWorkspace);

			//	Clip Plane 을 저장한다.
			pSelectionDisplay->m_pClipPlane1->Load(fileLoadWorkspace);
			pSelectionDisplay->m_pClipPlane2->Load(fileLoadWorkspace);
		}
		else
		{
			OutputTextMsg("Error loading piw\n");
		}
	}

	//	m_iSerial
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = pProteinVistaRenderer->m_arraySelectionDisplay[i];
		if ( pSelectionDisplay )
		{
			fileLoadWorkspace.Read(&(pSelectionDisplay->m_iSerial), sizeof(long));
		}
	}
	fileLoadWorkspace.Read(&(CSelectionDisplay::m_maxSelectionIndex), sizeof(long));

	//	
	GetMainActiveView()->EndProgress(iProgress);

	iProgress =GetMainActiveView()->InitProgress(100);


	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		GetMainActiveView()->SetProgress((i+1)*100/MAX_DISPLAY_SELECTION_INDEX, iProgress );
		CSelectionDisplay * pSelectionDisplay = pProteinVistaRenderer->m_arraySelectionDisplay[i];
		if ( pSelectionDisplay )
		{
			pSelectionDisplay->DeleteDeviceObjects();
			pSelectionDisplay->InitRenderSceneSelection();
			pSelectionDisplay->InitDeviceObjects();
		}
	} 

	GetMainActiveView()->EndProgress(iProgress);

	//
	long	iSelect;
	fileLoadWorkspace.Read(&iSelect, sizeof(LONG));

	GetMainActiveView()->GetSelectPanel()->OnUpdate();

	pProteinVistaRenderer->m_pPropertyScene->Load(&fileLoadWorkspace);
	pProteinVistaRenderer->m_pClipPlane->Load(fileLoadWorkspace);	
	for ( int i = 0 ; i < MAX_LIGHTS ; i++ )
	{
		pProteinVistaRenderer->m_LightControl[i].Load(fileLoadWorkspace);
	}
	delete[] pData;
	this->GetActiveProteinVistaView()->UpdateActivePDBComboBox(); 
	//	Load 하고 나서 selection 된것을 표시
	pProteinVistaRenderer->DeSelectAllAtoms();

	if ( iSelect != -1 )
	{
		this->GetActiveProteinVistaView()->GetSelectPanel()->SelectListItem(iSelect); 
		pProteinVistaRenderer->UpdateAtomSelectionChanged();
	}

	m_strPathWorkspace = strFilename;
	g_bRequestRender = TRUE;

	this->m_bOpenWorkSpace = FALSE;
	OutputTextMsg("Load workspace finished.");
	return TRUE;
	 
}
#pragma managed(pop)


