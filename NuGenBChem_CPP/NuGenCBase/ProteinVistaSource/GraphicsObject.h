#ifndef __GRAPHICSOBJECT_H__
#define __GRAPHICSOBJECT_H__

class CProteinVistaRenderer;

class CMoleculeRenderObject
{
public:
    virtual HRESULT OneTimeSceneInit()            { return S_OK; }
    virtual HRESULT InitDeviceObjects()           { return S_OK; }
    virtual HRESULT RestoreDeviceObjects()        { return S_OK; }
    virtual HRESULT FrameMove()                   { return S_OK; }
    virtual HRESULT Render()                      { return S_OK; }
    virtual HRESULT InvalidateDeviceObjects()     { return S_OK; }
    virtual HRESULT DeleteDeviceObjects()         { return S_OK; }
    virtual HRESULT FinalCleanup()                { return S_OK; }

	CMoleculeRenderObject();
	virtual ~CMoleculeRenderObject(){}

public:
	CProteinVistaRenderer		* m_pProteinVistaRenderer;

	LPDIRECT3DDEVICE9	GetD3DDevice();

public:
	virtual void Init(CProteinVistaRenderer * pRenderer) { m_pProteinVistaRenderer = pRenderer; } 
};


typedef std::vector <CMoleculeRenderObject *> CSTLArrayRenderObject;

#endif

