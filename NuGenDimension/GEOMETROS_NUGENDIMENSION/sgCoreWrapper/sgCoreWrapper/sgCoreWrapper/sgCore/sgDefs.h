#ifndef  __sgDefs__
#define  __sgDefs__


#ifndef sgCore_API
#define  sgCore_API __declspec(dllimport)
#else
#undef   sgCore_API
#define  sgCore_API __declspec(dllexport)
#endif

typedef struct
{
  double x;
  double y;
  double z;
} SG_POINT;

#define  SG_VECTOR   SG_POINT

typedef struct
{
  SG_POINT  p1;
  SG_POINT  p2;
} SG_LINE;

typedef  void*       SG_OBJ_HANDLE;
typedef  void(*SG_DRAW_LINE_FUNC)(SG_POINT*,SG_POINT*);

struct  SG_USER_DYNAMIC_DATA
{
  virtual void Finalize()   =0;
};



void*    sgPrivateAccess(int, void*, void*);
#define  PRIVATE_ACCESS   friend void* sgPrivateAccess(int, void*, void*);



#endif