#include "stdafx.h"

lua_State* lua_state = NULL;


#include "scriptDefs.h"
#include "scriptClasses.h"

static int scriptText(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"scriptText");
	_isArgString(LS,1,"scriptText");
	char* tS = _getString(LS,nargs,1);
//	lua_pushuserdata(LS,sgCore_interface->create_2D_interface.arText(tS));
	return 1;
}

static int scriptSave(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"scriptSave");
	_isArgString(LS,1,"scriptSave");
	const char* fn = _getString(LS,nargs,1);
	sgFileManager::Save(sgGetScene(),fn,NULL,0);
	return 1;
}

/*static int scriptFace(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"scriptFace");
	 
	double rad = _getDouble(LS,nargs,1,1);
	if (rad<=0.0)
		_argMustBeMoreThanError(LS,1,0.0,"SphericBand");

	double beg_coef = _getDouble(LS,nargs,2,1);
	if (beg_coef<-1.0)
		_argMustBeMoreThanError(LS,2,-1.0,"SphericBand");
	if (beg_coef>1.0)
		_argMustBeLessThanError(LS,2,1.0,"SphericBand");

	double end_coef = _getDouble(LS,nargs,3,1);
	if (end_coef<=0.0)
		_argMustBeMoreThanError(LS,3,0.0,"SphericBand");
	if (end_coef>1.0)
		_argMustBeLessThanError(LS,3,1.0,"SphericBand");

	int mer_cnt = _getInt(LS,nargs,4,24);
	if (mer_cnt<=3)
		_argMustBeMoreThanError(LS,4,3,"SphericBand");

	sgCSphericBand* spb = sgCreateSphericBand(rad,beg_coef,end_coef,mer_cnt);
	if (spb==NULL)
		_cantCreateObjectError(LS,"SphericBand");

	sgGetScene()->AttachObject(spb);
	_createInstance(LS,SPHERIC_BAND_TAG,spb);
	return 1;
}*/

//
// registering

void  LuaRegister()
{
	lua_state = lua_open(0);

	lua_baselibopen(lua_state);
	lua_iolibopen(lua_state);
	lua_strlibopen(lua_state);
	lua_mathlibopen(lua_state);

	luaL_openl(lua_state, sgScriptTable);

	lua_newtable(lua_state);
	lua_setglobal(lua_state,"_CLASS");

	_setupClass(lua_state, OBJECT_TAG, _addObjectMethods);
	_setupClass(lua_state,POINT_TAG, _addPointMethods, OBJECT_TAG);
	_setupClass(lua_state,LINE_TAG, _addLineMethods, OBJECT_TAG);
	_setupClass(lua_state,CIRCLE_TAG, _addCircleMethods, OBJECT_TAG);
	_setupClass(lua_state,BOX_TAG, _addBoxMethods, OBJECT_TAG);
	_setupClass(lua_state,SPHERE_TAG, _addSphereMethods, OBJECT_TAG);
	_setupClass(lua_state,CYLINDER_TAG, _addCylinderMethods, OBJECT_TAG);
	_setupClass(lua_state,CONE_TAG, _addConeMethods, OBJECT_TAG);
	_setupClass(lua_state,TORUS_TAG, _addTorusMethods, OBJECT_TAG);
	_setupClass(lua_state,ELLIPSOID_TAG, _addEllipsoidMethods, OBJECT_TAG);
	_setupClass(lua_state,SPHERIC_BAND_TAG, _addSphericBandMethods, OBJECT_TAG);

	/*lua_register(lua_state, "Point",scriptPoint);
	lua_register(lua_state, "Line",scriptLine);
	lua_register(lua_state, "Circle",scriptCircle);
	lua_register(lua_state, "Box",scriptBox);
	lua_register(lua_state, "Sphere",scriptSphere);
	lua_register(lua_state, "Cylinder",scriptCylinder);
	lua_register(lua_state, "Cone",scriptCone);
	lua_register(lua_state, "Torus",scriptTorus);
	lua_register(lua_state, "Ellipsoid",scriptEllipsoid);
	lua_register(lua_state, "SphericBand",scriptSphericBand);
	lua_register(lua_state, "Translate",scriptTranslateObject);
	lua_register(lua_state, "Text",scriptText);
	lua_register(lua_state, "Scale",scriptScaleObject);
	lua_register(lua_state, "Save",scriptSave);*/
	
}

void  LuaUnregister()
{
	lua_close(lua_state);
}

void  LuaRunScript(const char* file_name)
{
	lua_dofile(lua_state, file_name);
}

