#ifndef __SKYBOX_H__
#define __SKYBOX_H__

// vertex type definitions

typedef struct {
	D3DXVECTOR3		pos;
	DWORD			col;
	float tu;	float tv;
} CSkyboxVertex,*pCSkyboxVertex;

#define FVF_SKYBOX_VERTEX		D3DFVF_XYZ|D3DFVF_DIFFUSE|D3DFVF_TEX1

class	CSkyManager
{
public:
    virtual HRESULT OneTimeSceneInit()                         { return S_OK; }
    virtual HRESULT InitDeviceObjects();
    virtual HRESULT RestoreDeviceObjects();
    virtual HRESULT FrameMove();
    virtual HRESULT Render();
    virtual HRESULT InvalidateDeviceObjects();
    virtual HRESULT DeleteDeviceObjects();
    virtual HRESULT FinalCleanup();

	CSkyManager(CProteinVistaRenderer * pView = NULL);
	LPD3DXMESH CreateMappedSphere(LPDIRECT3DDEVICE9 pDev,float fRad,UINT slices,UINT stacks,DWORD col);
	void		ChangeSphereColor();

	LPDIRECT3DVERTEXDECLARATION9	m_pDeclSkybox;

	LPD3DXMESH			m_pSkyBox;		
	CString				m_strTextureFilename;
	LPDIRECT3DTEXTURE9	m_pSkyBoxTexture;
	
private:
	CProteinVistaRenderer * m_pProteinVistaRenderer;

	LPDIRECT3DDEVICE9 GetD3DDevice() { return m_pProteinVistaRenderer->GetD3DDevice(); }

};


#endif

