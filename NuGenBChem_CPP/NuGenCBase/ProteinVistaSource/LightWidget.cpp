#include "StdAfx.h"
#include "LightWidget.h"


HRESULT CLightWidget::Save(CFile &file)
{
	file.Write(&m_vCurrentDir, sizeof(D3DXVECTOR3));
	file.Write(&m_vDefaultDir, sizeof(D3DXVECTOR3));
	file.Write(&m_fRadius, sizeof(FLOAT));
	file.Write(&m_matWorld, sizeof(D3DXMATRIXA16));
	file.Write(&m_mView, sizeof(D3DXMATRIXA16));
	file.Write(&m_mRot, sizeof(D3DXMATRIXA16));
	file.Write(&m_mRotSnapshot, sizeof(D3DXMATRIXA16));

	file.Write(&m_ArcBall.m_mRotation, sizeof(D3DXMATRIXA16));
	file.Write(&m_ArcBall.m_mTranslation , sizeof(D3DXMATRIXA16));
	file.Write(&m_ArcBall.m_mTranslationDelta, sizeof(D3DXMATRIXA16));

	file.Write(&m_ArcBall.m_Offset, sizeof(POINT));   
	file.Write(&m_ArcBall.m_nWidth, sizeof(INT));
	file.Write(&m_ArcBall.m_nHeight, sizeof(INT));
	file.Write(&m_ArcBall.m_vCenter, sizeof(D3DXVECTOR2));
	file.Write(&m_ArcBall.m_fRadius, sizeof(FLOAT));
	file.Write(&m_ArcBall.m_fRadiusTranslation, sizeof(FLOAT));

	file.Write(&m_ArcBall.m_qDown, sizeof(D3DXQUATERNION ));
	file.Write(&m_ArcBall.m_qNow, sizeof(D3DXQUATERNION ));

	return S_OK;
}

HRESULT CLightWidget::Load(CFile &file)
{
	UpdateLightDir();

	file.Read(&m_vCurrentDir, sizeof(D3DXVECTOR3));
	file.Read(&m_vDefaultDir, sizeof(D3DXVECTOR3));
	file.Read(&m_fRadius, sizeof(FLOAT));
	file.Read(&m_matWorld, sizeof(D3DXMATRIXA16));
	file.Read(&m_mView, sizeof(D3DXMATRIXA16));
	file.Read(&m_mRot, sizeof(D3DXMATRIXA16));
	file.Read(&m_mRotSnapshot, sizeof(D3DXMATRIXA16));

	file.Read(&m_ArcBall.m_mRotation, sizeof(D3DXMATRIXA16));
	file.Read(&m_ArcBall.m_mTranslation , sizeof(D3DXMATRIXA16));
	file.Read(&m_ArcBall.m_mTranslationDelta, sizeof(D3DXMATRIXA16));

	file.Read(&m_ArcBall.m_Offset, sizeof(POINT));   
	file.Read(&m_ArcBall.m_nWidth, sizeof(INT));
	file.Read(&m_ArcBall.m_nHeight, sizeof(INT));
	file.Read(&m_ArcBall.m_vCenter, sizeof(D3DXVECTOR2));
	file.Read(&m_ArcBall.m_fRadius, sizeof(FLOAT));
	file.Read(&m_ArcBall.m_fRadiusTranslation, sizeof(FLOAT));

	file.Read(&m_ArcBall.m_qDown, sizeof(D3DXQUATERNION ));
	file.Read(&m_ArcBall.m_qNow, sizeof(D3DXQUATERNION ));

	return S_OK;
}

