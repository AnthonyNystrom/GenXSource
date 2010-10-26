#include "stdafx.h"
#include "GraphicsObject.h"
#include "ProteinVistaRenderer.h"


CMoleculeRenderObject::CMoleculeRenderObject() 
{ 
	m_pProteinVistaRenderer = NULL;
}


LPDIRECT3DDEVICE9	CMoleculeRenderObject::GetD3DDevice() 
{ 
	if ( m_pProteinVistaRenderer == NULL )	return NULL;	
	return m_pProteinVistaRenderer->GetD3DDevice();
}
