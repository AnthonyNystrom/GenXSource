#pragma once
#include "StdAfx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "ProteinVistaRenderer.h"
#include "PDBRenderer.h"
#include "pdb.h"
#include "pdbInst.h"
#include "Utility.h"
#include "SelectionDisplay.h"
#include "pdbInst.h"
#include "Interface.h"

#include "SelectionListPane.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


//	wireframe.
void CProteinVistaView::OnButtonWireframe() 
{
	OnButtonDisplay(CSelectionDisplay::WIREFRAME);
}
 

//	spacefill	
void CProteinVistaView::OnButtonBall() 
{
	OnButtonDisplay(CSelectionDisplay::SPACEFILL);
}
 
//	stick
void CProteinVistaView::OnButtonStick() 
{
	OnButtonDisplay(CSelectionDisplay::STICKS);
}
 
//	ball & stick
void CProteinVistaView::OnButtonBallStick() 
{
	OnButtonDisplay(CSelectionDisplay::BALLANDSTICK);
}
 

//	surface
void CProteinVistaView::OnButtonDotsurface() 
{
	OnButtonDisplay(CSelectionDisplay::SURFACE);
}
 
//	2Â÷±¸Á¶.
void CProteinVistaView::OnButtonRibbon() 
{
	OnButtonDisplay(CSelectionDisplay::RIBBON);
}
 
//========================================================================================
 
void CProteinVistaView::OnButtonDisplay(long mode) 
{
	long indexSelection = -1;
	CSelectionDisplay * pSelectionDisplay =this->m_pProteinVistaRenderer->AddCurrentSelection(mode);
	if ( pSelectionDisplay )
		indexSelection = pSelectionDisplay->m_iDisplayStylePDB;
	else
	{
		OutputTextMsg("No Selections");
		OutputTextMsg("For viewing some molecular unit, you have to select atom(s) and/or residue(s) and/or chanins(s) ...");
	}
	if ( indexSelection != -1 )
	{
		GetMainActiveView()->mSelectPanel->SelectListItem(pSelectionDisplay->m_iDisplaySelectionList);
	
	}
	g_bRequestRender = TRUE;
	GetMainActiveView()->OnPaint();
}

void CProteinVistaView::OnButtonDotsurfaceWithResolution(UINT id)
{
	int resolution = id - ID_SURFACE_POLYGON_0;

	int resolutionOld = m_pProteinVistaRenderer->m_renderQualityPreset.GetSurfaceQuality();
	m_pProteinVistaRenderer->m_renderQualityPreset.SetSurfaceQuality(resolution);

	OnButtonDisplay(CSelectionDisplay::SURFACE);

	m_pProteinVistaRenderer->m_renderQualityPreset.SetSurfaceQuality(resolutionOld);

	GetMainActiveView()->OnPaint();
}

 


