#ifndef   __sgRayTracer__
#define  __sgRayTracer__

struct  SG_VIEW_PORT
{
  virtual int       GetWidth() = 0;
  virtual int       GetHeight() = 0;

  virtual void  __cdecl RenderPixel(int x, int y,
    double pixRed, double pixGreen, double pixBlue, double pixAlpha) = 0;

  virtual void      EndRender() = 0;
};

namespace  sgRayTracingTexture
{

typedef enum
{
  RT_SOLID,
  RT_CHECKER,
  RT_BRICK,
  RT_IMAGE,
  RT_PARAMETRIC,
} RT_TEXTURE_TYPE;

typedef struct
{
  float  m_red;
  float  m_green;
  float  m_blue;
  float  m_alpha;
} RT_COLOR;

class  sgCTexture
{
public:
  virtual  RT_TEXTURE_TYPE             GetType() const  =0;
  virtual  SG_VECTOR                   GetTranslateVector() const
                        {   SG_VECTOR resVec = {0.0, 0.0, 0.0};
                          return resVec;};
  virtual  SG_VECTOR                   GetRotateVector() const
                        {   SG_VECTOR resVec = {0.0, 0.0, 0.0};
                        return resVec;};
  virtual  SG_VECTOR                   GetScaleVector() const
                        {   SG_VECTOR resVec = {1.0, 1.0, 1.0};
                        return resVec;};

  virtual ~sgCTexture() {};
};

class  sgCSolidTexture : public sgCTexture
{
public:
  RT_TEXTURE_TYPE   GetType() const  {return RT_SOLID;};

  virtual  void     GetColor(RT_COLOR& col) const = 0;

  virtual ~sgCSolidTexture() {};
};

class  sgCCheckerTexture : public sgCTexture
{
public:
  RT_TEXTURE_TYPE   GetType() const  {return RT_CHECKER;};

  virtual  void     GetColors(RT_COLOR& col_1, RT_COLOR& col_2) const = 0;

  virtual ~sgCCheckerTexture() {};
};

class  sgCBrickTexture : public sgCTexture
{
public:
  RT_TEXTURE_TYPE   GetType() const  {return RT_BRICK;};

  virtual  void     GetColors(RT_COLOR& col_1, RT_COLOR& col_2) const = 0;
  virtual  void     GetSizes(SG_VECTOR& bricksSizes, float& intervalSize) const = 0;

  virtual ~sgCBrickTexture() {};
};

class  sgCImageTexture : public sgCTexture
{
public:
  RT_TEXTURE_TYPE   GetType() const  {return RT_IMAGE;};

  virtual  void     GetColor(double uCoord, double vCoord, RT_COLOR& col) const = 0;

  virtual ~sgCImageTexture() {};
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


class  sgCParametricTexture : public sgCTexture
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
  virtual  void                          GetColorsMapTurbulenceParams(SG_VECTOR& turbVec,
                                     int&   stepsCountParam,
                                     double& randomShiftParam,
                                     double& stepCoeffParam) const
                        {
                          turbVec.x = turbVec.y = turbVec.z = 0.0;
                          stepsCountParam = 5;
                          randomShiftParam = 2.0;
                          stepCoeffParam = 0.5;
                        };

  virtual  void     GetGradientVector(SG_VECTOR&  gradVec) const
          {gradVec.x = gradVec.y = 0.0; gradVec.z = 1.0;};

  virtual ~sgCParametricTexture() {};
};

};


class  sgCMaterial
{
public:
  virtual   void     GetAmbient(sgRayTracingTexture::RT_COLOR& ambCol) const
                  {ambCol.m_red = ambCol.m_green = ambCol.m_blue = 0.1f;};
  virtual   void     GetReflection(sgRayTracingTexture::RT_COLOR& reflCol)  const
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

  virtual   const    sgRayTracingTexture::sgCTexture*   GetTexture() const  = 0;

  virtual ~sgCMaterial() {};
};

class  sgCObject;
class  sgCMaterialsLibrary
{
public:
  virtual sgCMaterial*  GetMaterial(const sgCObject*) = 0;
};

namespace  sgRayTracingLightSource
{
  typedef enum
  {
    RT_POINT,
    RT_SPOT,
    RT_CYLINDER
  } RT_LIGHT_SOURCE_TYPE;

  class  sgCLightSource
  {
  public:
    virtual  RT_LIGHT_SOURCE_TYPE           GetType() const  =0;
    virtual  SG_POINT                       GetLocation() const = 0;
    virtual  sgRayTracingTexture::RT_COLOR  GetColor()   const = 0;
    virtual  bool                           GetAtmosphericAttenuation() const
                          {return false;};
    virtual  bool                           GetAtmosphericInteraction() const
                          {return true;};
    virtual  double                         GetAttenuationDistance() const
                          {return 0.0;};
    virtual  double                         GetAttenuationPower() const
                          {return 0.0;};

    virtual ~sgCLightSource() {};
  };

  class sgCPointLightSource : public sgCLightSource
  {
  public:
    RT_LIGHT_SOURCE_TYPE           GetType() const {return RT_POINT;};

    virtual ~sgCPointLightSource() {};
  };

  class sgCSpotLightSource : public sgCLightSource
  {
  public:
    RT_LIGHT_SOURCE_TYPE           GetType() const {return RT_SPOT;};
    virtual        SG_POINT        GetLightPoint() const = 0;
    virtual        double          GetLightRadius() const = 0;
    virtual        double          GetRadius()      const = 0;

    virtual ~sgCSpotLightSource() {};
  };

  class sgCCylinderLightSource : public sgCLightSource
  {
  public:
    RT_LIGHT_SOURCE_TYPE           GetType() const {return RT_CYLINDER;};
    virtual        SG_POINT        GetLightPoint() const = 0;
    virtual        double          GetLightRadius() const = 0;
    virtual        double          GetRadius()      const = 0;

    virtual ~sgCCylinderLightSource() {};
  };
};

class  sgCLightSourcesContainer
{
public:
  virtual unsigned int                                    GetLightSourcesCount() = 0;
  virtual const sgRayTracingLightSource::sgCLightSource*  GetLightSource(unsigned int ind)= 0;
};

namespace sgRayTracer
{
   typedef enum
  {
    CT_PERSPECTIVE,
    CT_ORTHOGRAPHIC
  } CAMERA_TYPE;

  sgCore_API    bool   sgSetCamera(CAMERA_TYPE cam_type,
                    SG_VECTOR cam_location,
                    SG_VECTOR cam_look_point,
                    SG_VECTOR cam_y,
                    SG_VECTOR cam_x,
                    const double* cam_angle);

  sgCore_API    void   sgSetBackgroundColor(const sgRayTracingTexture::RT_COLOR& col);

  sgCore_API    void   sgSetAntialiasFlag(bool isAntialias);

  sgCore_API    void   sgSetAtmosphereParams(double density, double brightness, const sgRayTracingTexture::RT_COLOR& col);

  sgCore_API    bool   sgSetMaterialsLibrary(sgCMaterialsLibrary*);
  sgCore_API    bool   sgSetLightSourcesContainer(sgCLightSourcesContainer*);

  sgCore_API    bool   sgStart(SG_VIEW_PORT* nVP);
  sgCore_API    bool   sgStop();
};


#endif    // __sgRayTracer__
