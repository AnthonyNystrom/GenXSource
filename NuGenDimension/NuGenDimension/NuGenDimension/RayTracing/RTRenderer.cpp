#include "stdafx.h"
#include "Painter.h"
#include "RTMaterials.h"
#include "RTRenderer.h"

SG_POINT   RT_TO_SG(RT_POINT rtP)
{
	SG_POINT resP;
	memcpy(&resP, &rtP, sizeof(SG_POINT));
	return resP;
}

RT_POINT   SG_TO_RT(SG_POINT sgP)
{
	RT_POINT resP;
	memcpy(&resP, &sgP, sizeof(RT_POINT));
	return resP;
}

int   MyRenderer::GetWidth()
{
  return m_draw_sizes.cx;
}

int   MyRenderer::GetHeight()
{
  return m_draw_sizes.cy;
}

rtCMaterial* MyRTObject::GetMaterial()
{
  if (m_objct->GetMaterial())
  {
    return ::GetMaterial(m_objct->GetMaterial()->MaterialIndex,
      Painter::GetColorByIndex(m_objct->GetAttribute(SG_OA_COLOR))[0],
      Painter::GetColorByIndex(m_objct->GetAttribute(SG_OA_COLOR))[1],
      Painter::GetColorByIndex(m_objct->GetAttribute(SG_OA_COLOR))[2],
      0.0f);
  }
  return ::GetMaterial(-1,
    Painter::GetColorByIndex(m_objct->GetAttribute(SG_OA_COLOR))[0],
    Painter::GetColorByIndex(m_objct->GetAttribute(SG_OA_COLOR))[1],
    Painter::GetColorByIndex(m_objct->GetAttribute(SG_OA_COLOR))[2],
    0.0f);
};
