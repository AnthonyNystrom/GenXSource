#pragma once

#include "GraphicsObject.h"

class CProteinVistaRenderer;

class CDrawBoundingBox: public CMoleculeRenderObject
{
public:
	CDrawBoundingBox();

	virtual HRESULT InitDeviceObjects();
	virtual HRESULT Render();
	virtual HRESULT DeleteDeviceObjects();

};