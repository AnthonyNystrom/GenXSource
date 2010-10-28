#ifndef   __rtEngine__
#define  __rtEngine__

#ifndef rtEngine_API
#define  rtEngine_API __declspec(dllimport)
#else
#undef   rtEngine_API
#define  rtEngine_API __declspec(dllexport)
#endif

typedef struct
{
	double x;
	double y;
	double z;
} RT_POINT;

#define  RT_VECTOR   RT_POINT


struct  RT_VIEW_PORT
{
  virtual int       GetWidth() = 0;
  virtual int       GetHeight() = 0;

  virtual void  __cdecl RenderPixel(int x, int y,
    double pixRed, double pixGreen, double pixBlue, double pixAlpha) = 0;

  virtual void      EndRender() = 0;
};

namespace  rtTexture
{

	typedef enum
	{
		RT_SOLID,
		RT_CHECKER,
		RT_BRICK,
		RT_IMAGE,
		RT_PARAMETRIC
	} RT_TEXTURE_TYPE;

	typedef struct
	{
		float  m_red;
		float  m_green;
		float  m_blue;
		float  m_alpha;
	} RT_COLOR;

	class  rtCTexture
	{
	public:
		virtual  RT_TEXTURE_TYPE             GetType() const  =0;
		virtual  RT_VECTOR                   GetTranslateVector() const
												{   RT_VECTOR resVec = {0.0, 0.0, 0.0};
												return resVec;};
		virtual  RT_VECTOR                   GetRotateVector() const
												{   RT_VECTOR resVec = {0.0, 0.0, 0.0};
												return resVec;};
		virtual  RT_VECTOR                   GetScaleVector() const
												{   RT_VECTOR resVec = {1.0, 1.0, 1.0};
												return resVec;};

		virtual ~rtCTexture() {};
	};

	class  rtCSolidTexture : public rtCTexture
	{
	public:
		RT_TEXTURE_TYPE   GetType() const  {return RT_SOLID;};

		virtual  void     GetColor(RT_COLOR& col) const = 0;

		virtual ~rtCSolidTexture() {};
	};

	class  rtCCheckerTexture : public rtCTexture
	{
	public:
		RT_TEXTURE_TYPE   GetType() const  {return RT_CHECKER;};

		virtual  void     GetColors(RT_COLOR& col_1, RT_COLOR& col_2) const = 0;

		virtual ~rtCCheckerTexture() {};
	};

	class  rtCBrickTexture : public rtCTexture
	{
	public:
		RT_TEXTURE_TYPE   GetType() const  {return RT_BRICK;};

		virtual  void     GetColors(RT_COLOR& col_1, RT_COLOR& col_2) const = 0;
		virtual  void     GetSizes(RT_VECTOR& bricksSizes, float& intervalSize) const = 0;

		virtual ~rtCBrickTexture() {};
	};

	class  rtCImageTexture : public rtCTexture
	{
	public:
		RT_TEXTURE_TYPE   GetType() const  {return RT_IMAGE;};

		virtual  void     GetColor(double uCoord, double vCoord, RT_COLOR& col) const = 0;

		virtual ~rtCImageTexture() {};
	};

	typedef enum
	{
		RTPT_GRADIENT,
		RTPT_BUMP,
		RTPT_GRANITE,
		RTPT_WOOD
	} RT_PARAMETRIC_TEXTURE_TYPE;

	typedef struct
	{
		float     m_value;
		RT_COLOR  m_color;
	} RT_COLORS_MAP_ELEMENT;


	class  rtCParametricTexture : public rtCTexture
	{
	public:
		RT_TEXTURE_TYPE                        GetType() const   {return RT_PARAMETRIC;};

		virtual  RT_PARAMETRIC_TEXTURE_TYPE    GetParametricType() const = 0;

		virtual  int                           GetColorsMapSize() const = 0;
		virtual  const RT_COLORS_MAP_ELEMENT*  GetColorsMap() const = 0;
		virtual  void                          GetColorsMapParams(double& repeatParam,
															double& shiftParam) const
													{
														repeatParam = 1.0;
														shiftParam = 0.0;
													};
		virtual  void                          GetColorsMapTurbulenceParams(RT_VECTOR& turbVec,
															int&   stepsCountParam,
															double& randomShiftParam,
															double& stepCoeffParam) const
													{
														turbVec.x = turbVec.y = turbVec.z = 0.0;
														stepsCountParam = 5;
														randomShiftParam = 2.0;
														stepCoeffParam = 0.5;
													};

		virtual  void							GetGradientVector(RT_VECTOR&  gradVec) const
													{gradVec.x = gradVec.y = 0.0; gradVec.z = 1.0;};

		virtual ~rtCParametricTexture() {};
	};

};


class  rtCMaterial
{
public:
  virtual   void     GetAmbient(rtTexture::RT_COLOR& ambCol) const
                  {ambCol.m_red = ambCol.m_green = ambCol.m_blue = 0.1f;};
  virtual   void     GetReflection(rtTexture::RT_COLOR& reflCol)  const
                  {reflCol.m_red = reflCol.m_green = reflCol.m_blue = 0.0f;};
  virtual   float    GetDiffuse()          const {return 0.5f;};
  virtual   float    GetPhong()            const {return 0.0f;};
  virtual   float    GetPhongRadius()      const {return 20.0f;};
  virtual   float    GetSpecular()         const {return 0.0f;};
  virtual   bool     IsRefraction()       const {return false;};
  virtual   void     GetRefractionParams(float& ref_degre, float& shadow_light) const
                    { ref_degre = 1.0f; shadow_light = 0.0f;};
  virtual   float    GetGranular()         const {return 20.0f;};
  virtual   float    GetBrilliance()       const {return 1.0f;};
  virtual   bool     IsMetallic()          const {return false;};

  virtual   const    rtTexture::rtCTexture*   GetTexture() const  = 0;

  virtual ~rtCMaterial() {};
};

namespace  rtLightSource
{
  typedef enum
  {
    RT_POINT_LS,
    RT_SPOT_LS,
    RT_CYLINDER_LS
  } RT_LIGHT_SOURCE_TYPE;

  class  rtCLightSource
  {
  public:
    virtual  RT_LIGHT_SOURCE_TYPE           GetType() const  =0;
    virtual  RT_POINT                       GetLocation() const = 0;
    virtual  rtTexture::RT_COLOR  GetColor()   const = 0;
    virtual  bool                           GetAtmosphericAttenuation() const
                          {return false;};
    virtual  bool                           GetAtmosphericInteraction() const
                          {return true;};
    virtual  double                         GetAttenuationDistance() const
                          {return 0.0;};
    virtual  double                         GetAttenuationPower() const
                          {return 0.0;};

    virtual ~rtCLightSource() {};
  };

  class rtCPointLightSource : public rtCLightSource
  {
  public:
    RT_LIGHT_SOURCE_TYPE           GetType() const {return RT_POINT_LS;};

    virtual ~rtCPointLightSource() {};
  };

  class rtCSpotLightSource : public rtCLightSource
  {
  public:
    RT_LIGHT_SOURCE_TYPE           GetType() const {return RT_SPOT_LS;};
    virtual        RT_POINT        GetLightPoint() const = 0;
    virtual        double          GetLightRadius() const = 0;
    virtual        double          GetRadius()      const = 0;

    virtual ~rtCSpotLightSource() {};
  };

  class rtCCylinderLightSource : public rtCLightSource
  {
  public:
    RT_LIGHT_SOURCE_TYPE           GetType() const {return RT_CYLINDER_LS;};
    virtual        RT_POINT        GetLightPoint() const = 0;
    virtual        double          GetLightRadius() const = 0;
    virtual        double          GetRadius()      const = 0;

    virtual ~rtCCylinderLightSource() {};
  };
};

class  rtCLightSourcesContainer
{
public:
  virtual unsigned int                          GetLightSourcesCount() = 0;
  virtual const rtLightSource::rtCLightSource*  GetLightSource(unsigned int ind)= 0;
};

class  rtIIntersectionsStack
{
public:
	virtual   void   AddIntersection(const RT_POINT& intPnt, double intDepth, const RT_VECTOR& norml, 
		double U_coord, double V_coord) = 0;
};

class  rtIObject
{
public:
	virtual  void         GetGabarits(RT_POINT& gab_min, RT_POINT& gab_max) const =0;

	virtual  rtCMaterial* GetMaterial() = 0;

	virtual  bool         Intersect(const RT_POINT& rayPnt, const RT_VECTOR& rayDir, rtIIntersectionsStack* intStack) = 0;

};

class  rtIScene
{
public:
	virtual size_t       GetObjectsCount()    =0;
	virtual rtIObject*   GetObject(size_t indx) =0; 
};


namespace rtRayTracer
{
  rtEngine_API   bool    rtSetScene(rtIScene* scn);  

  typedef enum
  {
    CT_PERSPECTIVE,
    CT_ORTHOGRAPHIC
  } CAMERA_TYPE;

  rtEngine_API    bool   rtSetCamera(CAMERA_TYPE cam_type,
										RT_VECTOR cam_location,
										RT_VECTOR cam_look_point,
										RT_VECTOR cam_y,
										RT_VECTOR cam_x,
										const double* cam_angle);

  rtEngine_API    void   rtSetBackgroundColor(const rtTexture::RT_COLOR& col);

  rtEngine_API    void   rtSetAntialiasFlag(bool isAntialias);

  rtEngine_API    void   rtSetAtmosphereParams(double density, double brightness, const rtTexture::RT_COLOR& col);

  rtEngine_API    bool   rtSetLightSourcesContainer(rtCLightSourcesContainer*);

  rtEngine_API    bool   rtStart(RT_VIEW_PORT* nVP);
  rtEngine_API    bool   rtStop();
};


#endif    // __rtEngine__
