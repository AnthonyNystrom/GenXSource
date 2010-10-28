#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "RTMaterials.h"
#include "RTWoods.h"
#include "RTGranites.h"
#include "RTImage.h"


//#include "..//resource.h"

#include <vector>


class CSolidTexture : public rtTexture::rtCSolidTexture
{
	rtTexture::RT_COLOR m_color;
public:
	CSolidTexture() 
	{
		m_color.m_red = m_color.m_green = m_color.m_blue = m_color.m_alpha = 0.0;
	}
	void        SetColor(float rC, float gC, float bC, float aC)
	{
		m_color.m_red = rC;  m_color.m_green = gC; m_color.m_blue = bC; m_color.m_alpha = aC;
	}
	void        SetColor(const rtTexture::RT_COLOR& col)
	{
		memcpy(&m_color, &col, sizeof(rtTexture::RT_COLOR));
	}

	virtual  void     GetColor(rtTexture::RT_COLOR& col) const
	{
		memcpy(&col, &m_color, sizeof(rtTexture::RT_COLOR));
	}
};


class CMaterialWithSolidTexture : public rtCMaterial
{
	rtTexture::RT_COLOR m_ambient_Col;
	rtTexture::RT_COLOR m_reflection_Col;
public:
	CSolidTexture   m_texture;

	typedef  enum
	{
		DIFFUSE          = 0,
		PHONG            = 1,
		PHONG_RAD        = 2,
		SPECULAR         = 3,
		REFRACTION       = 4, 
		REFR_DEGREE      = 5,
		REFR_SHAD_LIGHT  = 6,
		GRANULAR         = 7,
		BRILLIANCE       = 8,
		METALLIC         = 9,
		LAST_PARAM       = 10
	}  MATERIAL_PARAM;

protected:
	float    m_params[LAST_PARAM];
public:
	CMaterialWithSolidTexture()
	{
		m_ambient_Col.m_red = m_ambient_Col.m_green = 
			m_ambient_Col.m_blue = m_ambient_Col.m_alpha = 0.0;
		m_reflection_Col.m_red = m_reflection_Col.m_green = 
			m_reflection_Col.m_blue = m_reflection_Col.m_alpha = 0.0;
		m_params[DIFFUSE]          = 0.5f;
		m_params[PHONG]            = 0.0f;
		m_params[PHONG_RAD]        = 20.0f;
		m_params[SPECULAR]         = 0.0f;
		m_params[REFRACTION]       = 0.0f; 
		m_params[REFR_DEGREE]      = 1.0f;
		m_params[REFR_SHAD_LIGHT]  = 0.0f;
		m_params[GRANULAR]         = 20.0f;
		m_params[BRILLIANCE]       = 1.0f;
		m_params[METALLIC]         = 0.0f;
	}

	void        SetAmbientColor(float rC, float gC, float bC, float aC)
	{
		m_ambient_Col.m_red = rC;  m_ambient_Col.m_green = gC; 
		m_ambient_Col.m_blue = bC; m_ambient_Col.m_alpha = aC;
	}
	void        SetAmbientColor(const rtTexture::RT_COLOR& col)
	{
		memcpy(&m_ambient_Col, &col, sizeof(rtTexture::RT_COLOR));
	}

	virtual   void     GetAmbient(rtTexture::RT_COLOR& ambCol) const
	{
		memcpy(&ambCol, &m_ambient_Col, sizeof(rtTexture::RT_COLOR));
	}

	void        SetReflectionColor(float rC, float gC, float bC, float aC)
	{
		m_reflection_Col.m_red = rC;  m_reflection_Col.m_green = gC; 
		m_reflection_Col.m_blue = bC; m_reflection_Col.m_alpha = aC;
	}
	void        SetReflectionColor(const rtTexture::RT_COLOR& col)
	{
		memcpy(&m_reflection_Col, &col, sizeof(rtTexture::RT_COLOR));
	}

	virtual   void     GetReflection(rtTexture::RT_COLOR& reflCol)  const
	{
		memcpy(&reflCol, &m_reflection_Col, sizeof(rtTexture::RT_COLOR));
	};

	void      SetParam(MATERIAL_PARAM param_ind, float param_val)
	{
		if (param_ind<LAST_PARAM)
			m_params[param_ind] = param_val;
	}

	virtual   float    GetDiffuse()          const {return m_params[DIFFUSE];};
	virtual   float    GetPhong()            const {return m_params[PHONG];};
	virtual   float    GetPhongRadius()      const {return m_params[PHONG_RAD];};
	virtual   float    GetSpecular()         const {return m_params[SPECULAR];};
	virtual   bool     IsRefraction()       const {return (m_params[REFRACTION]>0.5f);};
	virtual   void     GetRefractionParams(float& ref_degre, float& shadow_light) const 
	{ 
		ref_degre = m_params[REFR_DEGREE]; 
		shadow_light = m_params[REFR_SHAD_LIGHT];
	};
	virtual   float    GetGranular()         const {return m_params[GRANULAR];};
	virtual   float    GetBrilliance()       const {return m_params[BRILLIANCE];};
	virtual   bool     IsMetallic()          const {return (m_params[METALLIC]>0.5f);};

	virtual   const    rtTexture::rtCTexture*   GetTexture() const 
													{ return &m_texture;};
};

void CMatLibrary::AddMaterial (void *bitBitMap, int iSizeX, int iSizeY)
{
	CImageMaterial* tmpImT;
	tmpImT = new CImageMaterial();
	tmpImT->m_texture.SetPictureBits(bitBitMap,iSizeX,iSizeY);
	m_materials.push_back(tmpImT);
}
void CMatLibrary::Create(int iMaterial)
{
	CMaterialWithSolidTexture* tmpMat = NULL;
	CGraniteMaterial* tmpMatGran = NULL;
	CWoodMaterial* tmpMatWood = NULL;

// GLASSES START
	// MATERIAL_GLASS_1
	switch (iMaterial) 
	{
	case 1:
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.0f, 0.0f, 0.0f, 0.0f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 1.0f, 1.0f, 0.7f);
		m_materials.push_back(tmpMat);break;
	case 2:
		// MATERIAL_GLASS_2
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 333.330f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.45f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::PHONG, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::PHONG_RAD, 400.0f);
		tmpMat->m_texture.SetColor(0.98f, 0.98f, 0.98f, 0.9f);
		m_materials.push_back(tmpMat);break;
	case 3:

		// MATERIAL_GLASS_3
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 333.330f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.45f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::PHONG, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::PHONG_RAD, 400.0f);
		tmpMat->m_texture.SetColor(0.8f, 1.0f, 0.95f, 0.9f);
		m_materials.push_back(tmpMat);break;
	case 4:
		// MATERIAL_GLASS_4
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(0.98f, 1.0f, 0.99f, 0.75f);
		m_materials.push_back(tmpMat);break;
	case 5:
		// MATERIAL_GLASS_5
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(0.8f, 0.9f, 0.85f, 0.85f);
		m_materials.push_back(tmpMat);break;
	case 6:
		// MATERIAL_GLASS_6
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(0.4f, 0.72f, 0.4f, 0.6f);
		m_materials.push_back(tmpMat);break;
	case 7:
		// MATERIAL_GLASS_7
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(0.7f, 0.5f, 0.1f, 0.6f);
		m_materials.push_back(tmpMat);break;
	case 8:
		// MATERIAL_GLASS_8
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(0.9f, 0.1f, 0.2f, 0.8f);
		m_materials.push_back(tmpMat);break;
	case 9:
		// MATERIAL_GLASS_9
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(0.1f, 0.7f, 0.8f, 0.8f);
		m_materials.push_back(tmpMat);break;
	case 10:
		// MATERIAL_GLASS_10
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(0.8f, 0.8f, 0.2f, 0.8f);
		m_materials.push_back(tmpMat);break;
	case 11:
		// MATERIAL_GLASS_11
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.5f, 0.0f, 0.8f);
		m_materials.push_back(tmpMat);break;
	case 12:
		// MATERIAL_GLASS_12
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 1000.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFRACTION, 1.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_DEGREE, 1.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::REFR_SHAD_LIGHT, 1.0f);
		tmpMat->m_texture.SetColor(0.1f, 0.15f, 0.5f, 0.9f);
		m_materials.push_back(tmpMat);break;
// GLASSES END
	case 13:
// METALLS START
		// MATERIAL_METALL_BRONZE_1
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.3f, 0.2f, 0.1f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 14:
		// MATERIAL_METALL_BRONZE_2
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.3f, 0.2f, 0.1f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 15:
		// MATERIAL_METALL_BRONZE_3
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.3f, 0.2f, 0.1f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 16:
		// MATERIAL_METALL_BRONZE_4
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.3f, 0.2f, 0.1f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 17:
		// MATERIAL_METALL_BRONZE_5
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.3f, 0.2f, 0.1f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 18:
		// MATERIAL_METALL_BRONZE_6
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.35f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 19:
		// MATERIAL_METALL_BRONZE_7
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.35f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 20:
		// MATERIAL_METALL_BRONZE_8
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.35f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 21:
		// MATERIAL_METALL_BRONZE_9
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.35f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 22:
		// MATERIAL_METALL_BRONZE_10
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.35f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 23:
		// MATERIAL_METALL_BRONZE_11
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.58f, 0.42f, 0.20f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 24:
		// MATERIAL_METALL_BRONZE_12
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.58f, 0.42f, 0.20f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 25:
		// MATERIAL_METALL_BRONZE_13
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.58f, 0.42f, 0.20f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 26:
		// MATERIAL_METALL_BRONZE_14
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.58f, 0.42f, 0.20f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 27:
		// MATERIAL_METALL_BRONZE_15
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.58f, 0.42f, 0.20f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 28:
		// MATERIAL_METALL_BRONZE_16
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.65f, 0.50f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 29:
		// MATERIAL_METALL_BRONZE_17
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.65f, 0.50f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 30:
		// MATERIAL_METALL_BRONZE_18
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.65f, 0.50f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 31:
		// MATERIAL_METALL_BRONZE_19
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.65f, 0.50f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 32:
		// MATERIAL_METALL_BRONZE_20
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.65f, 0.50f, 0.25f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 33:
		// MATERIAL_METALL_BRONZE_21
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.70f, 0.55f, 0.40f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 34:
		// MATERIAL_METALL_BRONZE_22
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.70f, 0.55f, 0.40f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 35:
		// MATERIAL_METALL_BRONZE_23
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.70f, 0.55f, 0.40f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 36:
		// MATERIAL_METALL_BRONZE_24
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.70f, 0.55f, 0.40f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 37:
		// MATERIAL_METALL_BRONZE_25
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.70f, 0.55f, 0.40f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 38:

		// MATERIAL_METALL_CHROME_1
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.2f, 0.2f, 0.2f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 39:
		// MATERIAL_METALL_CHROME_2
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.2f, 0.2f, 0.2f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 40:
		// MATERIAL_METALL_CHROME_3
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.2f, 0.2f, 0.2f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 41:
		// MATERIAL_METALL_CHROME_4
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.2f, 0.2f, 0.2f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 42:
		// MATERIAL_METALL_CHROME_5
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.2f, 0.2f, 0.2f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 43:
		// MATERIAL_METALL_CHROME_6
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.39f, 0.41f, 0.43f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 44:
		// MATERIAL_METALL_CHROME_7
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.39f, 0.41f, 0.43f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 45:
		// MATERIAL_METALL_CHROME_8
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.39f, 0.41f, 0.43f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 46:
		// MATERIAL_METALL_CHROME_9
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.39f, 0.41f, 0.43f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 47:
		// MATERIAL_METALL_CHROME_10
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.39f, 0.41f, 0.43f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 48:
		// MATERIAL_METALL_CHROME_11
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.5f, 0.5f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 49:
		// MATERIAL_METALL_CHROME_12
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.5f, 0.5f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 50:
		// MATERIAL_METALL_CHROME_13
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.5f, 0.5f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 51:
		// MATERIAL_METALL_CHROME_14
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.5f, 0.5f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 52:
		// MATERIAL_METALL_CHROME_15
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.5f, 0.5f, 0.5f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 53:
		// MATERIAL_METALL_CHROME_16
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.75f, 0.75f, 0.75f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 54:
		// MATERIAL_METALL_CHROME_17
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.75f, 0.75f, 0.75f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 55:
		// MATERIAL_METALL_CHROME_18
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.75f, 0.75f, 0.75f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 56:
		// MATERIAL_METALL_CHROME_19
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.75f, 0.75f, 0.75f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 57:
		// MATERIAL_METALL_CHROME_20
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.75f, 0.75f, 0.75f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 58:
		// MATERIAL_METALL_CHROME_21
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.95f, 0.95f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 59:
		// MATERIAL_METALL_CHROME_22
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.95f, 0.95f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 60:
		// MATERIAL_METALL_CHROME_23
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.95f, 0.95f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 61:
		// MATERIAL_METALL_CHROME_24
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.95f, 0.95f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 62:
		// MATERIAL_METALL_CHROME_25
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.95f, 0.95f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 63:

		// MATERIAL_METALL_SILVER_1
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.80f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 64:
		// MATERIAL_METALL_SILVER_2
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.80f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 65:
		// MATERIAL_METALL_SILVER_3
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.80f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 66:
		// MATERIAL_METALL_SILVER_4
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.80f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 67:
		// MATERIAL_METALL_SILVER_5
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.80f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 68:
		// MATERIAL_METALL_SILVER_6
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.85f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 69:
		// MATERIAL_METALL_SILVER_7
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.85f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 70:
		// MATERIAL_METALL_SILVER_8
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.85f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 71:
		// MATERIAL_METALL_SILVER_9
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.85f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 72:
		// MATERIAL_METALL_SILVER_10
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.85f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 73:
		// MATERIAL_METALL_SILVER_11
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.90f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 74:
		// MATERIAL_METALL_SILVER_12
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.90f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 75:
		// MATERIAL_METALL_SILVER_13
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.90f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 76:
		// MATERIAL_METALL_SILVER_14
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.90f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 77:
		// MATERIAL_METALL_SILVER_15
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.94f, 0.93f, 0.90f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 78:
		// MATERIAL_METALL_SILVER_16
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.91f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 79:
		// MATERIAL_METALL_SILVER_17
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.91f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 80:
		// MATERIAL_METALL_SILVER_18
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.91f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 81:
		// MATERIAL_METALL_SILVER_19
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.91f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 82:
		// MATERIAL_METALL_SILVER_20
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.95f, 0.91f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 83:
		// MATERIAL_METALL_SILVER_21
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.35f, 0.35f, 0.35f, 0.35f);
		tmpMat->SetReflectionColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.91f, 0.95f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 84:
		// MATERIAL_METALL_SILVER_22
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.3f, 0.3f, 0.3f);
		tmpMat->SetReflectionColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.4f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.91f, 0.95f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 85:
		// MATERIAL_METALL_SILVER_23
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.25f, 0.25f, 0.25f, 0.25f);
		tmpMat->SetReflectionColor(0.5f, 0.5f, 0.5f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.91f, 0.95f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 86:
		// MATERIAL_METALL_SILVER_24
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.15f, 0.15f, 0.15f, 0.15f);
		tmpMat->SetReflectionColor(0.65f, 0.65f, 0.65f, 0.65f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.91f, 0.95f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 87:
		// MATERIAL_METALL_SILVER_25
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.1f, 0.1f, 0.1f, 0.1f);
		tmpMat->SetReflectionColor(0.8f, 0.8f, 0.8f, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(0.91f, 0.95f, 0.91f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 88:

		// MATERIAL_METALL_GOLD_1
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.22f, 0.181f, 0.121f, 0.0f);
		tmpMat->SetReflectionColor(0.55f, 0.4525f, 0.3025f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.2f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.391f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.675f, 0.175f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 89:
		// MATERIAL_METALL_GOLD_2
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.2f, 0.1725f, 0.1275f, 0.0f);
		tmpMat->SetReflectionColor(0.6f, 0.50375f, 0.34625f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.35f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.675f, 0.175f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 90:
		// MATERIAL_METALL_GOLD_3
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.18f, 0.162f, 0.13f, 0.0f);
		tmpMat->SetReflectionColor(0.65f, 0.56f, 0.4f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.306f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.675f, 0.175f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 91:
		// MATERIAL_METALL_GOLD_4
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.6f, 0.5125f, 0.3375f, 0.0f);
		tmpMat->SetReflectionColor(0.70f, 0.62125f, 0.46375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.675f, 0.175f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 92:
		// MATERIAL_METALL_GOLD_5
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.275f, 0.215f, 0.0f);
		tmpMat->SetReflectionColor(0.75f, 0.6875f, 0.5375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.07833f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.675f, 0.175f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 93:
		// MATERIAL_METALL_GOLD_6
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.22f, 0.181f, 0.121f, 0.0f);
		tmpMat->SetReflectionColor(0.55f, 0.4525f, 0.3025f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.2f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.391f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.725f, 0.275f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 94:
		// MATERIAL_METALL_GOLD_7
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.2f, 0.1725f, 0.1275f, 0.0f);
		tmpMat->SetReflectionColor(0.6f, 0.50375f, 0.34625f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.35f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.725f, 0.275f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 95:
		// MATERIAL_METALL_GOLD_8
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.18f, 0.162f, 0.13f, 0.0f);
		tmpMat->SetReflectionColor(0.65f, 0.56f, 0.4f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.306f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.725f, 0.275f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 96:
		// MATERIAL_METALL_GOLD_9
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.6f, 0.5125f, 0.3375f, 0.0f);
		tmpMat->SetReflectionColor(0.70f, 0.62125f, 0.46375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.725f, 0.275f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 97:
		// MATERIAL_METALL_GOLD_10
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.275f, 0.215f, 0.0f);
		//tmpMat->SetReflectionColor(0.75f, 0.6875f, 0.5375f, 0.0f);
		tmpMat->SetReflectionColor(0.1f, 0.0f, 0.1f, 0.5f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.07833f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.725f, 0.275f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 98:
		// MATERIAL_METALL_GOLD_11
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.22f, 0.181f, 0.121f, 0.0f);
		tmpMat->SetReflectionColor(0.55f, 0.4525f, 0.3025f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.2f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.391f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.775f, 0.375f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 99:
		// MATERIAL_METALL_GOLD_12
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.2f, 0.1725f, 0.1275f, 0.0f);
		tmpMat->SetReflectionColor(0.6f, 0.50375f, 0.34625f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.35f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.775f, 0.375f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 100:
		// MATERIAL_METALL_GOLD_13
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.18f, 0.162f, 0.13f, 0.0f);
		tmpMat->SetReflectionColor(0.65f, 0.56f, 0.4f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.306f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.775f, 0.375f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 101:
		// MATERIAL_METALL_GOLD_14
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.6f, 0.5125f, 0.3375f, 0.0f);
		tmpMat->SetReflectionColor(0.70f, 0.62125f, 0.46375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.775f, 0.375f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 102:
		// MATERIAL_METALL_GOLD_15
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.275f, 0.215f, 0.0f);
		tmpMat->SetReflectionColor(0.75f, 0.6875f, 0.5375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.07833f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.775f, 0.375f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 103:
		// MATERIAL_METALL_GOLD_16
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.22f, 0.181f, 0.121f, 0.0f);
		tmpMat->SetReflectionColor(0.55f, 0.4525f, 0.3025f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.2f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.391f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.825f, 0.475f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 104:
		// MATERIAL_METALL_GOLD_17
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.2f, 0.1725f, 0.1275f, 0.0f);
		tmpMat->SetReflectionColor(0.6f, 0.50375f, 0.34625f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.35f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.825f, 0.475f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 105:
		// MATERIAL_METALL_GOLD_18
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.18f, 0.162f, 0.13f, 0.0f);
		tmpMat->SetReflectionColor(0.65f, 0.56f, 0.4f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.306f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.825f, 0.475f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 106:
		// MATERIAL_METALL_GOLD_19
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.6f, 0.5125f, 0.3375f, 0.0f);
		tmpMat->SetReflectionColor(0.70f, 0.62125f, 0.46375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.825f, 0.475f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 107:
		// MATERIAL_METALL_GOLD_20
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.275f, 0.215f, 0.0f);
		tmpMat->SetReflectionColor(0.75f, 0.6875f, 0.5375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.07833f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.825f, 0.475f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 108:
		// MATERIAL_METALL_GOLD_21
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.22f, 0.181f, 0.121f, 0.0f);
		tmpMat->SetReflectionColor(0.55f, 0.4525f, 0.3025f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.2f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 20.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.391f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 2.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.875f, 0.575f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 109:
		// MATERIAL_METALL_GOLD_22
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.2f, 0.1725f, 0.1275f, 0.0f);
		tmpMat->SetReflectionColor(0.6f, 0.50375f, 0.34625f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.3f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 60.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.35f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 3.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.875f, 0.575f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 110:
		// MATERIAL_METALL_GOLD_23
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.18f, 0.162f, 0.13f, 0.0f);
		tmpMat->SetReflectionColor(0.65f, 0.56f, 0.4f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.6f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 80.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.306f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 4.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.875f, 0.575f, 0.0f);
		m_materials.push_back(tmpMat);break;
	case 111:
		// MATERIAL_METALL_GOLD_24
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.6f, 0.5125f, 0.3375f, 0.0f);
		tmpMat->SetReflectionColor(0.70f, 0.62125f, 0.46375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.7f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 100.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 5.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.875f, 0.575f, 0.0f);
		m_materials.push_back(tmpMat);
	case 112:
		// MATERIAL_METALL_GOLD_25
		tmpMat = new CMaterialWithSolidTexture;
		tmpMat->SetAmbientColor(0.3f, 0.275f, 0.215f, 0.0f);
		tmpMat->SetReflectionColor(0.75f, 0.6875f, 0.5375f, 0.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::SPECULAR, 0.8f);
		tmpMat->SetParam(CMaterialWithSolidTexture::GRANULAR, 120.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::DIFFUSE, 0.07833f);
		tmpMat->SetParam(CMaterialWithSolidTexture::BRILLIANCE, 6.0f);
		tmpMat->SetParam(CMaterialWithSolidTexture::METALLIC, 1.0f);
		tmpMat->m_texture.SetColor(1.0f, 0.875f, 0.575f, 0.0f);
		m_materials.push_back(tmpMat);break;
	
// METALLS END

// GRANITE BEGIN
	case 113:
		// MATERIAL_GRANITE_1
		tmpMatGran = new CGraniteMaterial(granite_1_color_map,
													6, 0.6, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 114:
		// MATERIAL_GRANITE_2
		tmpMatGran = new CGraniteMaterial(granite_2_color_map,
			6, 0.5, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 115:
		// MATERIAL_GRANITE_3
		tmpMatGran = new CGraniteMaterial(granite_3_color_map,
			6, 0.5, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 116:
		// MATERIAL_GRANITE_4
		tmpMatGran = new CGraniteMaterial(granite_4_color_map,
			6, 0.5, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 117:
		// MATERIAL_GRANITE_5
		tmpMatGran = new CGraniteMaterial(granite_5_color_map,
			7, 0.6, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 118:
		// MATERIAL_GRANITE_6
		tmpMatGran = new CGraniteMaterial(granite_6_color_map,
			10, 0.4, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 119:
		// MATERIAL_GRANITE_7
		tmpMatGran = new CGraniteMaterial(granite_7_color_map,
			6, 0.6, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 120:
		// MATERIAL_GRANITE_8
		tmpMatGran = new CGraniteMaterial(granite_8_color_map,
			6, 0.6, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 121:
		// MATERIAL_GRANITE_9
		tmpMatGran = new CGraniteMaterial(granite_9_color_map,
			6, 0.6, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 122:
		// MATERIAL_GRANITE_10
		tmpMatGran = new CGraniteMaterial(granite_10_color_map,
			6, 0.5, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 123:
		// MATERIAL_GRANITE_11
		tmpMatGran = new CGraniteMaterial(granite_11_color_map,
			7, 0.6, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 124:
		// MATERIAL_GRANITE_12
		tmpMatGran = new CGraniteMaterial(granite_12_color_map,
			5, 0.6, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 125:
		// MATERIAL_GRANITE_13
		tmpMatGran = new CGraniteMaterial(granite_13_color_map,
			7, 0.6, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 126:
		// MATERIAL_GRANITE_14
		tmpMatGran = new CGraniteMaterial(granite_14_color_map,
			12, 0.5, 1.0);
		m_materials.push_back(tmpMatGran);break;
	case 127:
		// MATERIAL_GRANITE_15
		tmpMatGran = new CGraniteMaterial(granite_15_color_map,
			255, 1.0, 2.0, 0.35f, 70.0f);
		m_materials.push_back(tmpMatGran);break;
	case 128:
		// MATERIAL_GRANITE_16
		tmpMatGran = new CGraniteMaterial(granite_16_color_map,
			255, 1.0, 0.4, 0.75f, 120.0f);
		m_materials.push_back(tmpMatGran);break;
// GRANITE END


// WOODS BEGIN
	case 129:
		// MATERIAL_WOOD_1
		tmpMatWood = new CWoodMaterial(wood_1_color_map,19,
											0.045, 0.045, 0.045,
											0.125, 0.125, 0.125);
		m_materials.push_back(tmpMatWood);break;
	case 130:
		// MATERIAL_WOOD_2
		tmpMatWood = new CWoodMaterial(wood_2_color_map,11,
			0.04, 0.04, 0.04,
			0.15, 0.15, 0.15);
		m_materials.push_back(tmpMatWood);break;
	case 131:
		// MATERIAL_WOOD_3
		tmpMatWood = new CWoodMaterial(wood_3_color_map,11,
			0.0425, 0.0425, 0.0425,
			0.2, 0.2, 0.2);
		m_materials.push_back(tmpMatWood);break;
	case 132:
		// MATERIAL_WOOD_4
		tmpMatWood = new CWoodMaterial(wood_4_color_map,7,
			0.03725, 0.03725, 0.03725,
			0.25, 0.225, 1.0,
			5, 2.425, 0.65725);
		m_materials.push_back(tmpMatWood);break;
	case 133:
		// MATERIAL_WOOD_5
		tmpMatWood = new CWoodMaterial(wood_5_color_map,255,
			0.04, 0.04, 0.04,
			0.05, 0.05, 1.0, 3);
		m_materials.push_back(tmpMatWood);break;
	case 134:
		// MATERIAL_WOOD_6
		tmpMatWood = new CWoodMaterial(wood_6_color_map,255,
			0.04, 0.04, 0.04,
			0.05, 0.05, 1.0, 3);
		m_materials.push_back(tmpMatWood);break;
	case 135:
		// MATERIAL_WOOD_7
		tmpMatWood = new CWoodMaterial(wood_7_color_map,255,
			0.05, 0.08, 1.00,
			0.15, 0.15, 1.0, 4);
		m_materials.push_back(tmpMatWood);break;
	case 136:
		// MATERIAL_WOOD_8
		tmpMatWood = new CWoodMaterial(wood_8_color_map,255,
			0.04, 0.04, 0.04,
			0.05, 0.05, 1.0, 3);
		m_materials.push_back(tmpMatWood);break;
	case 137:
		// MATERIAL_WOOD_9
		tmpMatWood = new CWoodMaterial(wood_9_color_map,255,
			0.04, 0.04, 0.04,
			0.05, 0.05, 1.0, 3);
		m_materials.push_back(tmpMatWood);break;
	case 138:
		// MATERIAL_WOOD_10
		tmpMatWood = new CWoodMaterial(wood_10_color_map,255,
			0.04, 0.04, 0.04,
			0.05, 0.05, 1.0, 3);
		m_materials.push_back(tmpMatWood);break;
	// WOODS END
	}
}
CMatLibrary::~CMatLibrary()
{
	Delete();
}
void CMatLibrary::Delete()
{
	/*for (size_t i=0;i<m_materials.size();i++)
		delete m_materials[i];*/
	m_materials.clear();
}

rtCMaterial* CMatLibrary::GetMaterial(size_t mat_ind)
{
	if (mat_ind<m_materials.size())
		return m_materials[mat_ind];
	return NULL;
}

CMatLibrary  mat_library;



class CDefaultTexture : public rtTexture::rtCSolidTexture
{
public:
	float  m_red;
	float  m_green;
	float  m_blue;
	float  m_alpha;
	virtual  void     GetColor(rtTexture::RT_COLOR& col) const
	{
		col.m_red = m_red;
		col.m_green = m_green;
		col.m_blue = m_blue;
		col.m_alpha = m_alpha;
	}
};

class CDefaultMaterial : public rtCMaterial
{
public:
	CDefaultTexture  m_def_texture;
	virtual   const rtTexture::rtCTexture*   GetTexture() const {return &m_def_texture;};

};

class CCheckerTexture : public rtTexture::rtCCheckerTexture
{
public:
	virtual  void     GetColors(rtTexture::RT_COLOR& col_1, 
									rtTexture::RT_COLOR& col_2) const
	{
		col_1.m_red = 1.0;
		col_1.m_green = col_1.m_blue = col_1.m_alpha = 0.0;

		col_2.m_green = 1.0;
		col_2.m_red = col_2.m_blue = col_2.m_alpha = 0.0;
	}
};

class CBrickTexture : public rtTexture::rtCBrickTexture
{
public:
	virtual  void     GetColors(rtTexture::RT_COLOR& col_1, 
		rtTexture::RT_COLOR& col_2) const
	{
		col_1.m_red = col_1.m_green = col_1.m_blue = 1.0;
		col_1.m_alpha = 0.0;

		col_2.m_blue = 1.0;
		col_2.m_red = col_2.m_green = col_2.m_alpha = 0.0;
	}
	virtual  void     GetSizes(RT_VECTOR& bricksSizes, float& intervalSize) const
	{
		bricksSizes.x = 0.5;
		bricksSizes.y = 0.5;
		bricksSizes.z = 0.3;
		intervalSize = 0.02f;
	}
};

static rtTexture::RT_COLORS_MAP_ELEMENT   gradient_map[] = 
{
	{0.2f, {1.0f, 0.0f, 0.0f, 0.0f}},
	{0.4f, {1.0f, 1.0f, 0.0f, 0.0f}},
	{0.5f, {0.0f, 1.0f, 0.0f, 0.0f}},
	{0.7f, {0.0f, 1.0f, 1.0f, 0.0f}},
	{0.9f, {0.0f, 0.0f, 1.0f, 0.0f}}
};

class CGradientTexture : public rtTexture::rtCParametricTexture
{
public:
	virtual  rtTexture::RT_PARAMETRIC_TEXTURE_TYPE    GetParametricType() const
	{
		return rtTexture::RTPT_GRADIENT;
	}

	virtual  int                           GetColorsMapSize() const 
	{
		return 5;
	}

	virtual  const rtTexture::RT_COLORS_MAP_ELEMENT*  GetColorsMap() const
	{
		return gradient_map;
	}

	/*virtual  void                          GetColorsMapParams(double& repeatParam,
		double& shiftParam) const
	{
		repeatParam = 1.0;
		shiftParam = 0.0;
	};

	virtual  void                          GetColorsMapTurbulenceParams(SG_VECTOR& turbVec,
		int&   stepsCountParam,
		double& randomShiftParam,
		double& stepCoeffParam) const
	{
		turbVec.x = 1.0;turbVec.y = 1.0;turbVec.z = 0.0;
		stepsCountParam = 10;
		randomShiftParam = 3.0;
		stepCoeffParam = 0.9;
	};*/

	virtual  void     GetGradientVector(SG_VECTOR&  gradVec) const
					{gradVec.x = 0.0; gradVec.y = 5.9; gradVec.z = -0.8;};
	virtual  RT_VECTOR                   GetScaleVector() const
							{   RT_VECTOR resVec = {3.0, 3.0, 3.0};
							return resVec;};
};


class CBumpTexture : public rtTexture::rtCParametricTexture
{
public:
	virtual  rtTexture::RT_PARAMETRIC_TEXTURE_TYPE    GetParametricType() const
	{
		return rtTexture::RTPT_BUMP;
	}

	virtual  int                           GetColorsMapSize() const 
	{
		return 5;
	}

	virtual  const rtTexture::RT_COLORS_MAP_ELEMENT*  GetColorsMap() const
	{
		return gradient_map;
	}
};

class CCheckerMaterial : public rtCMaterial
{
public:
	CCheckerTexture  m_ch_texture;
	virtual   const rtTexture::rtCTexture*   GetTexture() const {return &m_ch_texture;};

};


class CBrickMaterial : public rtCMaterial
{
public:
	CBrickTexture  m_br_texture;
	virtual   const rtTexture::rtCTexture*   GetTexture() const {return &m_br_texture;};

};


class CGradientMaterial : public rtCMaterial
{
public:
	CGradientTexture  m_grad_texture;
	virtual   const rtTexture::rtCTexture*   GetTexture() const {return &m_grad_texture;};

};


class CBumpMaterial : public rtCMaterial
{
public:
	CBumpTexture  m_bump_texture;
	virtual   const rtTexture::rtCTexture*   GetTexture() const {return &m_bump_texture;};

};



static  CDefaultMaterial   default_material;
static  CCheckerMaterial   checker_material;
static  CBrickMaterial     brick_material;
static  CGradientMaterial  gradient_material;
static  CBumpMaterial      bump_material;

rtCMaterial*   GetMaterial(int ind, 
						   float def_red ,
						   float def_green , 
						   float def_blue, 
						   float def_alpha)
{
	if (ind<0)
	{
		default_material.m_def_texture.m_red   = def_red;
		default_material.m_def_texture.m_green = def_green;
		default_material.m_def_texture.m_blue  = def_blue;
		default_material.m_def_texture.m_alpha = def_alpha;
		return &default_material;
	}
	/*if (ind==0)
	{
		//return &bump_material;  // bump floor
		return &gradient_material;  // gradient floor
		//return &brick_material;  // brick floor
		//return &checker_material;  // checker floor
	}*/
	return mat_library.GetMaterial(ind);
}


void CreateMaterials(int iMaterial)
{
	mat_library.Create(iMaterial);
}


bool           IsImageMaterial(int ind)
{
	if (ind>=MATERIAL_IMAGE_1 && ind<=MATERIAL_IMAGE_3)
		return true;
	return false;
}

void  SetMaterialToObject(sgCObject* ob, int mat_ind)
{
	if (ob->GetType()!=SG_OT_3D)
		return;

	sgC3DObject* obj3D = reinterpret_cast<sgC3DObject*>(ob);

	SG_MATERIAL mat;
	memset(&mat,0,sizeof(SG_MATERIAL));
	mat.MaterialIndex = mat_ind;
	obj3D->SetMaterial(mat);
}