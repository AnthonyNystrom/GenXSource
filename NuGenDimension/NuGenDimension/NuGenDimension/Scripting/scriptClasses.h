#ifndef __SCRIPT_CLASSES_
#define __SCRIPT_CLASSES_

extern int OBJECT_TAG;
extern int POINT_TAG;
extern int LINE_TAG;
extern int CIRCLE_TAG;
extern int BOX_TAG;
extern int SPHERE_TAG;
extern int CYLINDER_TAG;
extern int CONE_TAG;
extern int TORUS_TAG;
extern int ELLIPSOID_TAG;
extern int SPHERIC_BAND_TAG;

void _isArgObject(lua_State* LS, int arg, char* method);
int   scrObjectTranslate(lua_State* LS);
int   scrObjectRotate(lua_State* LS);
int   scrObjectSetColor(lua_State* LS);
int   scrSetLineThickness(lua_State* LS);
int   scrObjectClone(lua_State* LS);
void _addObjectMethods(lua_State* LS);


void _isArgPoint(lua_State* LS, int arg, char* method);
int scrPoint(lua_State* LS);
int scrPointGetPoint(lua_State* LS);
void _addPointMethods(lua_State* LS);

void _isArgLine(lua_State* LS, int arg, char* method);
int scrLine(lua_State* LS);
int scrLineGetPoint(lua_State* LS);
void _addLineMethods(lua_State* LS);

void _isArgCircle(lua_State* LS, int arg, char* method);
int scrCircle(lua_State* LS);
int scrCircleGetRadius(lua_State* LS);
int scrCircleGetCenter(lua_State* LS);
int scrCircleGetNormal(lua_State* LS);
void _addCircleMethods(lua_State* LS);

void _isArgBox(lua_State* LS, int arg, char* method);
int scrBox(lua_State* LS);
int scrBoxGetXSize(lua_State* LS);
int scrBoxGetYSize(lua_State* LS);
int scrBoxGetZSize(lua_State* LS);
void _addBoxMethods(lua_State* LS);

void _isArgSphere(lua_State* LS, int arg, char* method);
int scrSphere(lua_State* LS);
int scrSphereGetRadius(lua_State* LS);
int scrSphereGetMeridiansCount(lua_State* LS);
int scrSphereGetParallelsCount(lua_State* LS);
void _addSphereMethods(lua_State* LS);

void _isArgCylinder(lua_State* LS, int arg, char* method);
int scrCylinder(lua_State* LS);
int scrCylinderGetRadius(lua_State* LS);
int scrCylinderGetMeridiansCount(lua_State* LS);
int scrCylinderGetHeight(lua_State* LS);
void _addCylinderMethods(lua_State* LS);

void _isArgCone(lua_State* LS, int arg, char* method);
int scrCone(lua_State* LS);
int scrConeGetRadius1(lua_State* LS);
int scrConeGetRadius2(lua_State* LS);
int scrConeGetMeridiansCount(lua_State* LS);
int scrConeGetHeight(lua_State* LS);
void _addConeMethods(lua_State* LS);

void _isArgTorus(lua_State* LS, int arg, char* method);
int scrTorus(lua_State* LS);
int scrTorusGetRadius(lua_State* LS);
int scrTorusGetThickness(lua_State* LS);
void _addTorusMethods(lua_State* LS);

void _isArgEllipsoid(lua_State* LS, int arg, char* method);
int scrEllipsoid(lua_State* LS);
int scrEllipsoidGetXSize(lua_State* LS);
int scrEllipsoidGetYSize(lua_State* LS);
int scrEllipsoidGetZSize(lua_State* LS);
int scrEllipsoidGetMeridiansCount(lua_State* LS);
int scrEllipsoidGetParallelsCount(lua_State* LS);
void _addEllipsoidMethods(lua_State* LS);

void _isArgSphericBand(lua_State* LS, int arg, char* method);
int scrSphericBand(lua_State* LS);
int scrSphericBandGetRadius(lua_State* LS);
int scrSphericBandCoefficient1(lua_State* LS);
int scrSphericBandCoefficient2(lua_State* LS);
void _addSphericBandMethods(lua_State* LS);

const struct luaL_reg sgScriptTable[] = {
	// Transform
	{"Point",scrPoint},
	{"Line",scrLine},
	{"Circle",scrCircle},
	{"Box", scrBox},
	{"Sphere", scrSphere},
	{"Cylinder", scrCylinder},
	{"Cone", scrCone},
	{"Torus", scrTorus},
	{"Ellipsoid", scrEllipsoid},
	{"SphericBand", scrSphericBand},
};

#endif




/*
static int ObjectIncludes(lua_State* LS) {
_hasArgs(LS,4,"ObjectIncludes");
_isArgObject(LS,1,"ObjectIncludes"); // self
_isArgNumber(LS,2,"ObjectIncludes");   // x
_isArgNumber(LS,3,"ObjectIncludes");   // y
_isArgNumber(LS,4,"ObjectIncludes");   // z
GLObject* obj = (GLObject*)_getSelfInstance(LS);
M3Vector pos(
(float)lua_tonumber(LS,2),
(float)lua_tonumber(LS,3),
(float)lua_tonumber(LS,4)
);
if(obj->includes(pos))
lua_pushnumber(LS,1);
else
lua_pushnil(LS);
return 1;
}


static int ObjectsGetCount(lua_State* LS) 
{
_hasArgs(LS,1,"ObjectsGetCount");
_isArgObjects(LS,1,"ObjectsGetCount"); // self
GLObjects* objs = (GLObjects*)_getSelfInstance(LS);
int count = objs->getObjectsCount();
lua_pushnumber(LS,count);
return 1;
}

static int ObjectsGetFirst(lua_State* LS) 
{
_hasArgs(LS,1,"ObjectsGetFirst");
_isArgObjects(LS,1,"ObjectsGetFirst"); // self
GLObjects* objs = (GLObjects*)_getSelfInstance(LS);
GLObject* obj = objs->getFirstObject();
if(obj)
_createInstance(LS,OBJECT_TAG,obj);
else
lua_pushnil(LS);
return 1;
}

static int ObjectsGetNext(lua_State* LS) 
{
_hasArgs(LS,1,"ObjectsGetNext");
_isArgObjects(LS,1,"ObjectsGetNext"); // self
GLObjects* objs = (GLObjects*)_getSelfInstance(LS);
GLObject* obj = objs->getNextObject();
if(obj)
_createInstance(LS,OBJECT_TAG,obj);
else
lua_pushnil(LS);
return 1;
}*/


