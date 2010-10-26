#pragma once

#include "Redirect.h"
#include "PDB.h"
#include "PDBInst.h"
#include "ProteinSurfaceBase.h"

#include <set>

class	CPDBRenderer;

class CProteinSurfaceMSMS: public CProteinSurfaceBase
{
public:
	virtual HRESULT CreateSurface();

	CProteinSurfaceMSMS();
	~CProteinSurfaceMSMS();

	virtual		float	GetSurfaceQuality(int quality);
	virtual		long	GetTypeGenSurface();

private:
	CRedirector m_redirector;
};
