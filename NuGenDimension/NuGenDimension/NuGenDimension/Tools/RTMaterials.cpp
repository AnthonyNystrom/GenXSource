#include "stdafx.h"
#include "RTMaterials.h"

static sgRayTracingTexture::RT_COLORS_MAP_ELEMENT   gran[] = 
{
	{0.0f, {0.439216f, 0.439216f, 0.439216f, 0.0f}},
	{0.0f, {0.439216f, 0.439216f, 0.439216f, 0.0f}}
};

class CMatLibrary
{
	std::vector<sgCMaterial*>  m_materials;
public:
	CMatLibrary()
	{
	}
	~CMatLibrary()
	{
	}
};

static  CMatLibrary   mat_library;



class CDefaultTexture : public sgRayTracingTexture::sgCSolidTexture
{
public:
	float  m_red;
	float  m_green;
	float  m_blue;
	float  m_alpha;
	virtual  void     GetColor(sgRayTracingTexture::RT_COLOR& col) const
	{
		col.m_red = m_red;
		col.m_green = m_green;
		col.m_blue = m_blue;
		col.m_alpha = m_alpha;
	}
};

class CDefaultMaterial : public sgCMaterial
{
public:
	CDefaultTexture  m_def_texture;

	/*virtual   float    GetDiffuse()     const 
	{return 0.1;};
	virtual   float    GetPhong()     const 
	{return 0.5;};
	virtual   float    GetPhongRadius()     const 
	{return 40.0;};
	virtual   void     GetAmbient(sgRayTracingTexture::RT_COLOR& ambCol) const
	{
		ambCol.m_red = 0.1;
		ambCol.m_green = 0.1;
		ambCol.m_blue = 0.1;
		ambCol.m_alpha = 0.0;
	};
	virtual   void     GetReflection(sgRayTracingTexture::RT_COLOR& reflCol)  const
	{
		reflCol.m_red = 0.25;
		reflCol.m_green = 0.25;
		reflCol.m_blue = 0.25;
		reflCol.m_alpha = 0.0;
	};
	virtual   float    GetSpecular()    const {return 1.0f;};
	virtual   bool     IsRefraction()  const {return true;};
	virtual   float    GetRefractionDegree()  const {return 1.5f;};
	virtual   float    GetGranular()    const {return 100.0f;};*/
	//virtual   float    GetBrilliance()  const {return 5.0f;};
	//virtual   float    GetMetallic()    const {return 1.0f;};

	virtual   const sgRayTracingTexture::sgCTexture*   GetTexture() const {return &m_def_texture;};

};

static  CDefaultMaterial   default_material;

sgCMaterial*   GetMaterial(int ind, 
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
}