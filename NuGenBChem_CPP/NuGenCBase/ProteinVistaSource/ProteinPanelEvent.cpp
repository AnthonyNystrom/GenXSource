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
#include <vcclr.h>

#include "FileDialogExt.h"
#include "OpenPDBIDDialog.h"
//#include "ProteinViewPanel.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif
///////////////////////////////////////////////////////////////////////////
void CProteinVistaView::OnSurfaceGenAlgorithmMQ()
{
	if ( m_pProteinVistaRenderer )
	{
		m_pProteinVistaRenderer->m_surfaceGenAlgoritm = 0;
	    this->OnPaint();
	}
}
 
void CProteinVistaView::OnSurfaceGenAlgorithmMSMS()
{
	if ( m_pProteinVistaRenderer )
	{
		m_pProteinVistaRenderer->m_surfaceGenAlgoritm = 1;
	    this->OnPaint();
	}
}
void CProteinVistaView::OnSurfaceBiounitGenAlgorithmMQ()
{
	if ( m_pProteinVistaRenderer )
	{
		m_pProteinVistaRenderer->m_surfaceBiounitGenAlgoritm = 0;
	    this->OnPaint();
	}
}
void CProteinVistaView::OnSurfaceBiounitGenAlgorithmMSMS()
{
	if ( m_pProteinVistaRenderer )
	{
		m_pProteinVistaRenderer->m_surfaceBiounitGenAlgoritm = 1;
	    this->OnPaint();
	}
}
 
///////////////////////////////////////////////////////////////////////////
void CProteinVistaView::OnDisplayBioUnit()
{
	OnDisplayBioUnitSurface(ID_BIOUNIT_SURFACE_POLYGON_0);
}

void CProteinVistaView::OnDisplayBioUnitStyle(UINT mode)
{
	if (!m_pProteinVistaRenderer)
	{
		return;
	}
	if ( m_pProteinVistaRenderer->m_pActivePDBRenderer == NULL )
		return;

	long displayMode = (mode-ID_BUTTON_BIOUNIT_WIREFRAME);

	CPDBInst * pPDBInst = m_pProteinVistaRenderer->m_pActivePDBRenderer->GetPDBInst();
	if ( pPDBInst == NULL )
		return;

	if ( pPDBInst->m_pPDBRenderer->m_arrayPDBRendererChildBioUnit.size() != 0 )
	{
		OutputTextMsg("You already made Biomolecule");
		return;
	}

	if ( pPDBInst->m_pPDBRenderer->m_pPDBRendererParentBioUnit != NULL )
	{
		OutputTextMsg("This protein is sub-protein");
		return;
	}

	long iProgress = GetMainActiveView()->InitProgress(100);

	CSTLArrayPDBRenderer arrayPDBRenderer;
	arrayPDBRenderer.reserve(pPDBInst->GetPDB()->m_bioUnitMatrix.size());

	for ( int i = 0 ; i < pPDBInst->GetPDB()->m_bioUnitMatrix.size() ; i++ )
	{
		HRESULT hr = S_OK;
		GetMainActiveView()->SetProgress( (i+1)*100/pPDBInst->GetPDB()->m_bioUnitMatrix.size(), iProgress);

		//	현재 PDB 에 대해서 다른 Instance를 만든다.
		CPDBRenderer * pPDBRenderer = m_pProteinVistaRenderer->AddPDBRenderer(pPDBInst->GetPDB());

		CString strChain = pPDBInst->GetPDB()->m_strArrayBioUnitChain[i];
		pPDBRenderer->GetPDBInst()->DeleteUnUsedChainInBioUnit(strChain);

		//	현재것에 child로 넣는다.
		m_pProteinVistaRenderer->m_pActivePDBRenderer->AddChildPDBRendererBioUnit(pPDBRenderer);

		//	현재것에 child 로 transform matrix를 넣는다.
		pPDBRenderer->SetBioUnitTransform(pPDBInst->m_pPDBRenderer, i+1, pPDBInst->GetPDB()->m_bioUnitMatrix[i]);

		arrayPDBRenderer.push_back(pPDBRenderer);
	}

	OnPDBChanged();

	for ( int i = 0 ; i < arrayPDBRenderer.size(); i++ )
	{
		GetMainActiveView()->SetProgress( (i+1)*100/arrayPDBRenderer.size(), iProgress);
		m_pProteinVistaRenderer->SetInitialDefaultSelection(arrayPDBRenderer[i], displayMode );
	}

	GetMainActiveView()->EndProgress(iProgress);

	//	모든것을 deselect 한다.
	m_pProteinVistaRenderer->DeSelectAllAtoms();

	//	현재것을 active 시킨다
	m_pProteinVistaRenderer->SetActivePDBRenderer(pPDBInst->m_pPDBRenderer);

	OnAttatchBiounit();

	OnCenterMolecule();
	OnViewAll();
	this->OnPaint();
}

void CProteinVistaView::OnDisplayBioUnitSurface(UINT quality)
{
	if (!m_pProteinVistaRenderer)
	{
		return;
	}
	int resolution = quality - ID_BIOUNIT_SURFACE_POLYGON_0;			//	0..10

	int resolutionOld = m_pProteinVistaRenderer->m_renderQualityPreset.GetSurfaceQuality();
	m_pProteinVistaRenderer->m_renderQualityPreset.SetSurfaceQuality(resolution);

	int surfaceGenAlgorithm = m_pProteinVistaRenderer->m_surfaceGenAlgoritm;
	m_pProteinVistaRenderer->m_surfaceGenAlgoritm = m_pProteinVistaRenderer->m_surfaceBiounitGenAlgoritm;

	OnDisplayBioUnitStyle( ID_BUTTON_BIOUNIT_SURFACE );

	m_pProteinVistaRenderer->m_renderQualityPreset.SetSurfaceQuality(resolutionOld);
	m_pProteinVistaRenderer->m_surfaceGenAlgoritm = surfaceGenAlgorithm;
	this->OnPaint();
}

 
void CProteinVistaView::OnButtonPrevFrame() 
{

}
 
void CProteinVistaView::OnAttatchBiounit()
{
	if (m_pProteinVistaRenderer)
	{
	   m_pProteinVistaRenderer->AttatchBioUnit();
	   this->OnPaint();
	}
}

//////////////////////////////////////////////////////////////////////////
//
 
void CProteinVistaView::OnCenterMolecule()
{
	if (m_pProteinVistaRenderer && m_pProteinVistaRenderer->m_pActivePDBRenderer )
	{
		m_pProteinVistaRenderer->m_pActivePDBRenderer->CenterPDBRenderer();
		this->OnPaint();
	}
}
 
void CProteinVistaView::OnViewAll()
{
	if (!m_pProteinVistaRenderer)
	{
		return;
	}
	//	view all code here.
	m_pProteinVistaRenderer->CameraViewAll();
	this->OnPaint();
}

void CProteinVistaView::OnViewAllDisplayParams()
{
	if (!m_pProteinVistaRenderer)
	{
		return;
	}
	//	view all code here.
	m_pProteinVistaRenderer->CameraViewAll();
	this->OnPaint();
}
 
void CProteinVistaView::OnFlagMoleculeSelectionCenter()
{
	if (!m_pProteinVistaRenderer)
	{
		return;
	}
	if ( m_pProteinVistaRenderer->m_pActivePDBRenderer != NULL )
	{
		m_pProteinVistaRenderer->m_pActivePDBRenderer->m_bSelectionRotCenter = !(m_pProteinVistaRenderer->m_pActivePDBRenderer->m_bSelectionRotCenter);

		m_pProteinVistaRenderer->m_pActivePDBRenderer->GetCenterRadiusBB();
		m_pProteinVistaRenderer->m_pActivePDBRenderer->SetTransformCenter();
		this->OnPaint();
	}
}

void CProteinVistaView::OnFileAddPDBID()
{
	COpenPDBIDDialog	openPDBIDDialog;
	if ( IDOK == openPDBIDDialog.DoModal() )
	{
		//	
		if ( openPDBIDDialog.m_strArrayPDBID.GetSize()> 0 )
		{
			CStringArray	strFileArrayNew;
			for ( int i = 0 ; i < openPDBIDDialog.m_strArrayPDBID.GetSize() ; i++ )
			{
				CString strNewFilename;
				if ( GetMainApp()->CheckExistPDBFile(openPDBIDDialog.m_strArrayPDBID[i], strNewFilename) == TRUE )
					strFileArrayNew.Add(strNewFilename);
			}

			if ( strFileArrayNew.GetSize() > 0 )
			{
				for(int i = 0; i < strFileArrayNew.GetSize(); i++)
				{
					AddPDB(strFileArrayNew[i]);
				}
			}
		}
	}
}



CString CProteinVistaView::GetPDBHeaderInfo()
{
	CSTLArrayPDBRenderer &arrayPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer;
	long nPDB = arrayPDBRenderer.size();

	CString strHeader;

	if ( nPDB == 0 )
	{
		return "";
	}

	int i;
	strHeader.GetBuffer(1024*100);

	for ( int iHeader = 0 ; iHeader < arrayPDBRenderer.size(); iHeader ++ )
	{
		CPDBInst * pPDBInst = arrayPDBRenderer[iHeader]->GetPDBInst();

		if ( iHeader != 0 )
			strHeader += "\r\n\r\n\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\r\n\r\n";
		strHeader += "<<<< ";
		strHeader += pPDBInst->GetPDB()->m_strPDBID;
		strHeader += " >>>>\r\n\r\n";

		strHeader += "[Header]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArrayHeader.GetSize() ; i++ )		{	strHeader += pPDBInst->GetPDB()->m_strArrayHeader[i]; strHeader += "\r\n"; }

		strHeader += "\r\n[Title]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArrayTitle.GetSize() ; i++ )		{	strHeader += pPDBInst->GetPDB()->m_strArrayTitle[i]; strHeader += "\r\n"; }

		strHeader += "\r\n[Source:the biological and/or chemical source of each biological molecule in the entry]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArraySource.GetSize() ; i++ )		{	strHeader += pPDBInst->GetPDB()->m_strArraySource[i]; strHeader += "\r\n"; }

		strHeader += "\r\n[COMPND:the macromolecular contents of an entry]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArrayKeywords.GetSize() ; i++ )		{	strHeader += pPDBInst->GetPDB()->m_strArrayKeywords[i]; strHeader += "\r\n"; }

		strHeader += "\r\n[KEYWDS: a set of terms relevant to the entry]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArrayEXPDTA.GetSize() ; i++ )		{	strHeader += pPDBInst->GetPDB()->m_strArrayEXPDTA[i]; strHeader += "\r\n"; }

		strHeader += "\r\n[Author:the names of the people responsible for the contents of the entry]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArrayAuthor.GetSize() ; i++ )		{	strHeader += pPDBInst->GetPDB()->m_strArrayAuthor[i]; strHeader += "\r\n"; }

		strHeader += "\r\n[RevisionDate:a history of the modifications made to an entry since its release]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArrayRevisionDate.GetSize() ; i++ ){	strHeader += pPDBInst->GetPDB()->m_strArrayRevisionDate[i]; strHeader += "\r\n"; }

		strHeader += "\r\n[JournalCitation:the primary literature citation ]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArrayJournalCitation.GetSize() ; i++ ){	strHeader += pPDBInst->GetPDB()->m_strArrayJournalCitation[i]; strHeader += "\r\n"; }

		strHeader += "\r\n[Remark:the macromolecular contents of an entry]\r\n";
		for ( i = 0 ; i < pPDBInst->GetPDB()->m_strArrayRemark.GetSize() ; i++ )		{	strHeader += pPDBInst->GetPDB()->m_strArrayRemark[i]; strHeader += "\r\n"; }

	}
	return strHeader;
}

/////////////////////////////////////////////////////////////////////////////////

void CProteinVistaView::OnButtonPlay() 
{
	
} 
void CProteinVistaView::OnButtonStop() 
{

}
void CProteinVistaView::OnButtonNextFrame() 
{
}

void CProteinVistaView::OnButtonGoLast() 
{

}

void CProteinVistaView::OnButtonPlayFast() 
{

}

void CProteinVistaView::OnButtonPlaySlow() 
{
}

void CProteinVistaView::OnButtonGoFirst() 
{
	
}
//////////////////////////////////////////////////////////////////////////////////

int CProteinVistaView::GetCurrentComboxSelectIndex()
{ 
    return  GetMainActiveView()->mComboxList->SelectedIndex;
}
void CProteinVistaView::UpdateActivePDBComboBox()
{
	 GetMainActiveView()->mComboxList->Items->Clear();

	long iFindOldActivePDBRenderer = -1;
	for ( int i = 0; i < m_pProteinVistaRenderer->m_arrayPDBRenderer.size(); i++ )
	{
		CString strText;
		CPDBRenderer * pPDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[i];

		if ( m_pProteinVistaRenderer->m_pActivePDBRenderer == pPDBRenderer )
			iFindOldActivePDBRenderer = i;

		CPDBInst * pPDBInst = pPDBRenderer->GetPDBInst();
		if ( pPDBRenderer->m_pPDBRendererParentBioUnit != NULL )
		{
			strText.Format("%s-Biomolecule %d (%s)",pPDBInst->GetPDB()->m_strPDBID, pPDBRenderer->m_iBioUnit, pPDBInst->GetPDB()->m_strFilename);
		}
		else
		{
			strText.Format("%s (%s)",pPDBInst->GetPDB()->m_strPDBID, pPDBInst->GetPDB()->m_strFilename);
		}
		GetMainActiveView()->mComboxList->Items->Add(::CStringToMStr(strText));
	}
 
	if ( iFindOldActivePDBRenderer >= 0 )
	{
		GetMainActiveView()->mComboxList->SelectedIndex =iFindOldActivePDBRenderer; 
 
	}
	else if ( iFindOldActivePDBRenderer == -1 &&
		       m_pProteinVistaRenderer->m_arrayPDBRenderer.size() > 0 )
	{
		GetMainActiveView()->mComboxList->SelectedIndex  =0;
		m_pProteinVistaRenderer->m_pActivePDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[0];
		if(m_pProteinVistaRenderer->m_pActivePDBRenderer!=NULL)
		{
           m_pProteinVistaRenderer->m_pActivePDBRenderer->UpdatePDBRendererCenter();
		}
 
	}
	else if ( m_pProteinVistaRenderer->m_arrayPDBRenderer.size() == 0 )
	{
		m_pProteinVistaRenderer->m_pActivePDBRenderer = NULL;
	}
 
}

void CProteinVistaView::OnNextActivePDB()
{
	if ( m_pProteinVistaRenderer == NULL )
		return;
	if ( m_pProteinVistaRenderer->m_arrayPDBRenderer.size() == 0 )
		return;

	long nSel=0;
	long nCount =this->mComboxList->Items->Count;
	long next =this->GetCurrentComboxSelectIndex()+1;
	nSel = next%nCount;
	this->mComboxList->SelectedIndex =nSel;

	m_pProteinVistaRenderer->m_pActivePDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[nSel];
	m_pProteinVistaRenderer->m_pActivePDBRenderer->UpdatePDBRendererCenter();
}
 
void CProteinVistaView::OnPrevActivePDB()
{
	if ( m_pProteinVistaRenderer == NULL )
		return;
	if ( m_pProteinVistaRenderer->m_arrayPDBRenderer.size() == 0 )
		return;
 
	long nSel=0;
	long nCount =this->mComboxList->Items->Count;
	long prev =this->GetCurrentComboxSelectIndex()-1;
	if ( prev < 0 )
		nSel = nCount-1;
	else
		nSel = prev;
	 
	this->mComboxList->SelectedIndex =nSel;
	 
	m_pProteinVistaRenderer->m_pActivePDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[nSel];
	m_pProteinVistaRenderer->m_pActivePDBRenderer->UpdatePDBRendererCenter();
}

void CProteinVistaView::OnActivePDB()
{
	if (!m_pProteinVistaRenderer)
	{
		return;
	}
	int nSel =GetCurrentComboxSelectIndex();
	m_pProteinVistaRenderer->m_pActivePDBRenderer = m_pProteinVistaRenderer->m_arrayPDBRenderer[nSel];
	m_pProteinVistaRenderer->m_pActivePDBRenderer->UpdatePDBRendererCenter();
 
}

 