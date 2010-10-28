#include "stdafx.h"

#include "scriptClasses.h"
#include "ScriptDefs.h"

int OBJECT_TAG;
int POINT_TAG;
int LINE_TAG;
int CIRCLE_TAG;
int BOX_TAG;
int SPHERE_TAG;
int CYLINDER_TAG;
int CONE_TAG;
int TORUS_TAG;
int ELLIPSOID_TAG;
int SPHERIC_BAND_TAG;

void _isArgObject(lua_State* LS, int arg, char* method) 
{
	if(!_isChild(LS,arg,OBJECT_TAG))
		_argError(LS,arg,"Object",method);
}

int   scrObjectClone(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"Clone");
	sgCObject *obj = (sgCObject*)_getSelfInstance(LS);
	if (obj==NULL)
		_badObject(LS);
	
	sgCObject* clObj = obj->Clone();
	if (clObj==NULL)
		_cantCreateObjectError(LS,"Clone");

	sgGetScene()->AttachObject(clObj);
	_createInstance(LS,OBJECT_TAG,clObj);
	return 1;
}

int scrObjectTranslate(lua_State* LS) 
{
	int nargs = _hasArgs(LS,4,"Translate");
	_isArgObject(LS,1,"Translate"); // self
	_isArgNumber(LS,2,"Translate");   // x
	_isArgNumber(LS,3,"Translate");   // y
	_isArgNumber(LS,4,"Translate");   // z
	sgCObject *obj = (sgCObject*)_getSelfInstance(LS);
	if (obj==NULL)
		_badObject(LS);

	SG_VECTOR transVect;
	transVect.x = _getDouble(LS,nargs,2,0);
	transVect.y = _getDouble(LS,nargs,3,0);
	transVect.z = _getDouble(LS,nargs,4,0);
	if (transVect.x!=0.0 || transVect.y!=0.0 || transVect.z!=0.0)
	{
		obj->InitTempMatrix()->Translate(transVect);
		obj->ApplyTempMatrix();
		obj->DestroyTempMatrix();
	}
	return 0; //  количество аргументов, которые запихиваем в LUA стек
}

int   scrObjectRotate(lua_State* LS)
{
	int nargs = _hasArgs(LS,7,"Rotate");
	_isArgObject(LS,1,"Rotate"); // self
	_isArgNumber(LS,2,"Rotate");   // x
	_isArgNumber(LS,3,"Rotate");   // y
	_isArgNumber(LS,4,"Rotate");   // z
	_isArgNumber(LS,5,"Rotate");   // x
	_isArgNumber(LS,6,"Rotate");   // y
	_isArgNumber(LS,7,"Rotate");   // z
	_isArgNumber(LS,8,"Rotate");   // angl
	sgCObject *obj = (sgCObject*)_getSelfInstance(LS);
	if (obj==NULL)
		_badObject(LS);

	SG_POINT axeP;
	axeP.x = _getDouble(LS,nargs,2,0);
	axeP.y = _getDouble(LS,nargs,3,0);
	axeP.z = _getDouble(LS,nargs,4,0);
	SG_VECTOR axeDir;
	axeDir.x = _getDouble(LS,nargs,5,0)-axeP.x;
	axeDir.y = _getDouble(LS,nargs,6,0)-axeP.y;
	axeDir.z = _getDouble(LS,nargs,7,0)-axeP.z;
	double angl = _getDouble(LS,nargs,8,0);
	if (angl!=0.0)
	{
		obj->InitTempMatrix()->Rotate(axeP,axeDir,angl);
		obj->ApplyTempMatrix();
		obj->DestroyTempMatrix();
	}
	return 0; //  количество аргументов, которые запихиваем в LUA стек
}

int   scrObjectSetColor(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"SetColor");
	_isArgObject(LS,1,"SetColor"); // self
	_isArgNumber(LS,2,"SetColor");   
	sgCObject *obj = (sgCObject*)_getSelfInstance(LS);
	if (obj==NULL)
		_badObject(LS);

	int oCol = _getInt(LS,nargs,2,0);
	if (oCol<0) 
		oCol = 0;
	if (oCol>230) 
		oCol = 230;
	obj->SetAttribute(SG_OA_COLOR,(unsigned short)oCol);
	return 0; //  количество аргументов, которые запихиваем в LUA стек
}

int   scrSetLineThickness(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"SetLineThickness");
	_isArgObject(LS,1,"SetLineThickness"); // self
	_isArgNumber(LS,2,"SetLineThickness");   
	sgCObject *obj = (sgCObject*)_getSelfInstance(LS);
	if (obj==NULL)
		_badObject(LS);

	int lth = _getInt(LS,nargs,2,0);
	if (lth<1) 
		lth = 1;
	
	obj->SetAttribute(SG_OA_LINE_THICKNESS,(unsigned short)lth);
	return 0; //  количество аргументов, которые запихиваем в LUA стек
}

void _addObjectMethods(lua_State* LS) 
{
	_addMethod(LS,"Clone",scrObjectClone);
	_addMethod(LS,"Translate",scrObjectTranslate);
	_addMethod(LS,"Rotate",scrObjectRotate);
	_addMethod(LS,"SetColor",scrObjectSetColor);
	_addMethod(LS,"SetLineThickness",scrSetLineThickness);
}



/************************************************************************/
/* POINT                                                                 */
/************************************************************************/
void _isArgPoint(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,POINT_TAG))
		_argError(LS,arg,"Point",method);
}
int scrPoint(lua_State* LS)
{
	int nargs = _hasArgs(LS,3,"Point");
	_isArgNumber(LS,1,"Point");   // x
	_isArgNumber(LS,2,"Point");   // y
	_isArgNumber(LS,3,"Point");   // z
	double px = _getDouble(LS,nargs,1,0);
	double py = _getDouble(LS,nargs,2,0);
	double pz = _getDouble(LS,nargs,3,0);
	
	sgCPoint* pnt = sgCreatePoint(px,py,pz);
	if (pnt==NULL)
		_cantCreateObjectError(LS,"Point");

	sgGetScene()->AttachObject(pnt);
	_createInstance(LS,POINT_TAG,pnt);
	return 1;
}

int scrPointGetPoint(lua_State* LS)
{
	_hasArgs(LS,1,"GetPoint");
	_isArgPoint(LS,1,"GetPoint"); // self
	sgCPoint* pnt = (sgCPoint*)_getSelfInstance(LS);
	if (pnt==NULL)
		_badObject(LS);

	lua_newtable(LS); 
	lua_pushstring(LS, "x"); 
	lua_pushnumber(LS, pnt->GetGeometry()->x); 
	lua_settable(LS, -3); 
	lua_pushstring(LS, "y"); 
	lua_pushnumber(LS, pnt->GetGeometry()->y); 
	lua_settable(LS, -3); 
	lua_pushstring(LS, "z"); 
	lua_pushnumber(LS, pnt->GetGeometry()->z); 
	lua_settable(LS, -3); 
	return 1;
}
void _addPointMethods(lua_State* LS)
{
	_addMethod(LS,"GetPoint",scrPointGetPoint);
}


/************************************************************************/
/* LINE                                                                 */
/************************************************************************/
void _isArgLine(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,LINE_TAG))
		_argError(LS,arg,"Line",method);
}
int scrLine(lua_State* LS)
{
	int nargs = _hasArgs(LS,6,"Line");
	_isArgNumber(LS,1,"Line");   // x
	_isArgNumber(LS,2,"Line");   // y
	_isArgNumber(LS,3,"Line");   // z
	_isArgNumber(LS,4,"Line");   // x
	_isArgNumber(LS,5,"Line");   // y
	_isArgNumber(LS,6,"Line");   // z
	double px1 = _getDouble(LS,nargs,1,0);
	double py1 = _getDouble(LS,nargs,2,0);
	double pz1 = _getDouble(LS,nargs,3,0);
	double px2 = _getDouble(LS,nargs,4,0);
	double py2 = _getDouble(LS,nargs,5,0);
	double pz2 = _getDouble(LS,nargs,6,0);

	sgCLine* ln = sgCreateLine(px1,py1,pz1,px2,py2,pz2);
	if (ln==NULL)
		_cantCreateObjectError(LS,"Line");

	sgGetScene()->AttachObject(ln);
	_createInstance(LS,LINE_TAG,ln);
	return 1;
}
int scrLineGetPoint(lua_State* LS)
{
	int nargs = _hasArgs(LS,2,"GetPoint");
	_isArgLine(LS,1,"GetPoint"); // self
	_isArgNumber(LS,2,"GetPoint"); 
	
	sgCLine* ln = (sgCLine*)_getSelfInstance(LS);
	if (ln==NULL)
		_badObject(LS);

	int pnt_nmbr = _getInt(LS,nargs,2,0);

	if (pnt_nmbr<0) 
		pnt_nmbr = 0;
	if (pnt_nmbr>1) 
		pnt_nmbr = 1;

	SG_POINT lineP; 
	if (pnt_nmbr==0)
		lineP = ln->GetGeometry()->p1;
	else
		lineP = ln->GetGeometry()->p2;

	lua_newtable(LS); 
	lua_pushstring(LS, "x"); 
	lua_pushnumber(LS, lineP.x); 
	lua_settable(LS, -3); 
	lua_pushstring(LS, "y"); 
	lua_pushnumber(LS, lineP.y); 
	lua_settable(LS, -3); 
	lua_pushstring(LS, "z"); 
	lua_pushnumber(LS, lineP.z); 
	lua_settable(LS, -3); 
	return 1;
}

void _addLineMethods(lua_State* LS)
{
	_addMethod(LS,"GetPoint",scrLineGetPoint);
}


/************************************************************************/
/* CIRCLE                                                               */
/************************************************************************/
void _isArgCircle(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,CIRCLE_TAG))
		_argError(LS,arg,"Circle",method);
}

int scrCircle(lua_State* LS)
{
	int nargs = _hasArgs(LS,7,"Circle");
	_isArgNumber(LS,1,"Circle");   // x
	_isArgNumber(LS,2,"Circle");   // y
	_isArgNumber(LS,3,"Circle");   // z
	_isArgNumber(LS,4,"Circle");   // x
	_isArgNumber(LS,5,"Circle");   // y
	_isArgNumber(LS,6,"Circle");   // z
	_isArgNumber(LS,7,"Circle");   // rad
	SG_CIRCLE crcl;
	crcl.normal.x = _getDouble(LS,nargs,1,0);
	crcl.normal.y = _getDouble(LS,nargs,2,0);
	crcl.normal.z = _getDouble(LS,nargs,3,1);
	crcl.center.x = _getDouble(LS,nargs,4,0);
	crcl.center.y = _getDouble(LS,nargs,5,0);
	crcl.center.z = _getDouble(LS,nargs,6,0);
	crcl.radius = _getDouble(LS,nargs,7,1);

	sgCCircle* c_obj = sgCreateCircle(crcl);
	if (c_obj==NULL)
		_cantCreateObjectError(LS,"Circle");

	sgGetScene()->AttachObject(c_obj);
	_createInstance(LS,CIRCLE_TAG,c_obj);
	return 1;
}

int scrCircleGetRadius(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"GetRadius");
	_isArgCircle(LS,1,"GetRadius"); // self

	sgCCircle* cir = (sgCCircle*)_getSelfInstance(LS);
	if (cir==NULL)
		_badObject(LS);

	const SG_CIRCLE* cir_geo = cir->GetGeometry();

	lua_pushnumber(LS,cir_geo->radius);
	return 1;

}

int scrCircleGetCenter(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"GetCenter");
	_isArgCircle(LS,1,"GetCenter"); // self

	sgCCircle* cir = (sgCCircle*)_getSelfInstance(LS);
	if (cir==NULL)
		_badObject(LS);

	const SG_CIRCLE* cir_geo = cir->GetGeometry();

	lua_newtable(LS); 
	lua_pushstring(LS, "x"); 
	lua_pushnumber(LS, cir_geo->center.x); 
	lua_settable(LS, -3); 
	lua_pushstring(LS, "y"); 
	lua_pushnumber(LS, cir_geo->center.y); 
	lua_settable(LS, -3); 
	lua_pushstring(LS, "z"); 
	lua_pushnumber(LS, cir_geo->center.z); 
	lua_settable(LS, -3); 
	return 1;
}

int scrCircleGetNormal(lua_State* LS)
{
	int nargs = _hasArgs(LS,1,"GetNormal");
	_isArgCircle(LS,1,"GetNormal"); // self

	sgCCircle* cir = (sgCCircle*)_getSelfInstance(LS);
	if (cir==NULL)
		_badObject(LS);

	const SG_CIRCLE* cir_geo = cir->GetGeometry();

	lua_newtable(LS); 
	lua_pushstring(LS, "x"); 
	lua_pushnumber(LS, cir_geo->normal.x); 
	lua_settable(LS, -3); 
	lua_pushstring(LS, "y"); 
	lua_pushnumber(LS, cir_geo->normal.y); 
	lua_settable(LS, -3); 
	lua_pushstring(LS, "z"); 
	lua_pushnumber(LS, cir_geo->normal.z); 
	lua_settable(LS, -3); 
	return 1;
}

void _addCircleMethods(lua_State* LS)
{
	_addMethod(LS,"GetRadius",scrCircleGetRadius);
	_addMethod(LS,"GetCenter",scrCircleGetCenter);
	_addMethod(LS,"GetNormal",scrCircleGetNormal);
}


/************************************************************************/
/* BOX                                                                  */
/************************************************************************/
void _isArgBox(lua_State* LS, int arg, char* method) {
	if(!_isChild(LS,arg,BOX_TAG))
		_argError(LS,arg,"Box",method);
}
int scrBox(lua_State* LS) 
{
	int nargs = _hasArgs(LS,3,"Box");
	_isArgNumber(LS,1,"Box");   // x
	_isArgNumber(LS,2,"Box");   // y
	_isArgNumber(LS,3,"Box");   // z
	double sd1 = _getDouble(LS,nargs,1,1);
	if (sd1<=0.0)
		_argMustBeMoreThanError(LS,1,0.0,"Box");

	double sd2 = _getDouble(LS,nargs,2,1);
	if (sd2<=0.0)
		_argMustBeMoreThanError(LS,2,0.0,"Box");

	double sd3 = _getDouble(LS,nargs,3,1);
	if (sd3<=0.0)
		_argMustBeMoreThanError(LS,3,0.0,"Box");

	sgCBox* box = sgCreateBox(sd1,sd2,sd3);
	if (box==NULL)
		_cantCreateObjectError(LS,"Box");

	sgGetScene()->AttachObject(box);
	_createInstance(LS,BOX_TAG,box);
	return 1;
}

int scrBoxGetXSize(lua_State* LS) {
	_hasArgs(LS,1,"GetXSize");
	_isArgBox(LS,1,"GetXSize"); // self
	sgCBox* box = (sgCBox*)_getSelfInstance(LS);
	if (box==NULL)
		_badObject(LS);
	
	SG_BOX bx_geo;
	box->GetGeometry(bx_geo);
	lua_pushnumber(LS,bx_geo.SizeX);
	return 1;
}
int scrBoxGetYSize(lua_State* LS) {
	_hasArgs(LS,1,"GetYSize");
	_isArgBox(LS,1,"GetYSize"); // self
	sgCBox* box = (sgCBox*)_getSelfInstance(LS);
	if (box==NULL)
		_badObject(LS);

	SG_BOX bx_geo;
	box->GetGeometry(bx_geo);
	lua_pushnumber(LS,bx_geo.SizeY);
	return 1;
}
int scrBoxGetZSize(lua_State* LS) {
	_hasArgs(LS,1,"GetZSize");
	_isArgBox(LS,1,"GetZSize"); // self
	sgCBox* box = (sgCBox*)_getSelfInstance(LS);
	if (box==NULL)
		_badObject(LS);

	SG_BOX bx_geo;
	box->GetGeometry(bx_geo);
	lua_pushnumber(LS,bx_geo.SizeZ);
	return 1;
}
void _addBoxMethods(lua_State* LS) {
	_addMethod(LS,"GetXSize",scrBoxGetXSize);
	_addMethod(LS,"GetYSize",scrBoxGetYSize);
	_addMethod(LS,"GetZSize",scrBoxGetZSize);
}




/************************************************************************/
/* SPHERE                                                               */
/************************************************************************/
void _isArgSphere(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,SPHERE_TAG))
		_argError(LS,arg,"Sphere",method);
}
int scrSphere(lua_State* LS)
{
	int nargs = _hasArgs(LS,3,"Sphere");
	_isArgNumber(LS,1,"Sphere");   
	_isArgNumber(LS,2,"Sphere");   
	_isArgNumber(LS,3,"Sphere");   
	double rad = _getDouble(LS,nargs,1,1);
	if (rad<=0.0)
		_argMustBeMoreThanError(LS,1,0.0,"Sphere");

	int mer_cnt = _getInt(LS,nargs,2,24);
	if (mer_cnt<=3)
		_argMustBeMoreThanError(LS,2,3,"Sphere");

	int par_cnt = _getInt(LS,nargs,3,24);
	if (par_cnt<=3)
		_argMustBeMoreThanError(LS,3,3,"Sphere");

	sgCSphere* sph = sgCreateSphere(rad,mer_cnt,par_cnt);
	if (sph==NULL)
		_cantCreateObjectError(LS,"Sphere");

	sgGetScene()->AttachObject(sph);
	_createInstance(LS,SPHERE_TAG,sph);
	return 1;
}
int scrSphereGetRadius(lua_State* LS)
{
	_hasArgs(LS,1,"GetRadius");
	_isArgSphere(LS,1,"GetRadius"); // self
	sgCSphere* sph = (sgCSphere*)_getSelfInstance(LS);
	if (sph==NULL)
		_badObject(LS);

	SG_SPHERE sp_geo;
	sph->GetGeometry(sp_geo);
	lua_pushnumber(LS,sp_geo.Radius);
	return 1;
}
int scrSphereGetMeridiansCount(lua_State* LS)
{
	_hasArgs(LS,1,"GetMeridiansCount");
	_isArgSphere(LS,1,"GetMeridiansCount"); // self
	sgCSphere* sph = (sgCSphere*)_getSelfInstance(LS);
	if (sph==NULL)
		_badObject(LS);

	SG_SPHERE sp_geo;
	sph->GetGeometry(sp_geo);
	lua_pushnumber(LS,sp_geo.MeridiansCount);
	return 1;
}
int scrSphereGetParallelsCount(lua_State* LS)
{
	_hasArgs(LS,1,"GetParallelsCount");
	_isArgSphere(LS,1,"GetParallelsCount"); // self
	sgCSphere* sph = (sgCSphere*)_getSelfInstance(LS);
	if (sph==NULL)
		_badObject(LS);

	SG_SPHERE sp_geo;
	sph->GetGeometry(sp_geo);
	lua_pushnumber(LS,sp_geo.ParallelsCount);
	return 1;
}
void _addSphereMethods(lua_State* LS)
{
	_addMethod(LS,"GetRadius",scrSphereGetRadius);
	_addMethod(LS,"GetMeridiansCount",scrSphereGetMeridiansCount);
	_addMethod(LS,"GetParallelsCount",scrSphereGetParallelsCount);
}


/************************************************************************/
/* CYLINDER                                                             */
/************************************************************************/
void _isArgCylinder(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,CYLINDER_TAG))
		_argError(LS,arg,"Cylinder",method);
}
int scrCylinder(lua_State* LS)
{
	int nargs = _hasArgs(LS,3,"Cylinder");
	_isArgNumber(LS,1,"Cylinder");   
	_isArgNumber(LS,2,"Cylinder");   
	_isArgNumber(LS,3,"Cylinder");   
	double rad = _getDouble(LS,nargs,1,1);
	if (rad<=0.0)
		_argMustBeMoreThanError(LS,1,0.0,"Cylinder");

	double heig = _getDouble(LS,nargs,2,1);
	if (heig<=0.0)
		_argMustBeMoreThanError(LS,2,0.0,"Cylinder");

	int mer_cnt = _getInt(LS,nargs,3,24);
	if (mer_cnt<=3)
		_argMustBeMoreThanError(LS,3,3,"Cylinder");

	sgCCylinder* cyl = sgCreateCylinder(rad,heig,mer_cnt);
	if (cyl==NULL)
		_cantCreateObjectError(LS,"Cylinder");

	sgGetScene()->AttachObject(cyl);
	_createInstance(LS,CYLINDER_TAG,cyl);
	return 1;
}
int scrCylinderGetRadius(lua_State* LS)
{
	_hasArgs(LS,1,"GetRadius");
	_isArgCylinder(LS,1,"GetRadius"); // self
	sgCCylinder* cyl = (sgCCylinder*)_getSelfInstance(LS);
	if (cyl==NULL)
		_badObject(LS);

	SG_CYLINDER cyl_geo;
	cyl->GetGeometry(cyl_geo);
	lua_pushnumber(LS,cyl_geo.Radius);
	return 1;
}
int scrCylinderGetMeridiansCount(lua_State* LS)
{
	_hasArgs(LS,1,"GetMeridiansCount");
	_isArgCylinder(LS,1,"GetMeridiansCount"); // self
	sgCCylinder* cyl = (sgCCylinder*)_getSelfInstance(LS);
	if (cyl==NULL)
		_badObject(LS);

	SG_CYLINDER cyl_geo;
	cyl->GetGeometry(cyl_geo);
	lua_pushnumber(LS,cyl_geo.MeridiansCount);
	return 1;
}
int scrCylinderGetHeight(lua_State* LS)
{
	_hasArgs(LS,1,"GetHeight");
	_isArgCylinder(LS,1,"GetHeight"); // self
	sgCCylinder* cyl = (sgCCylinder*)_getSelfInstance(LS);
	if (cyl==NULL)
		_badObject(LS);

	SG_CYLINDER cyl_geo;
	cyl->GetGeometry(cyl_geo);
	lua_pushnumber(LS,cyl_geo.Height);
	return 1;
}
void _addCylinderMethods(lua_State* LS)
{
	_addMethod(LS,"GetRadius",scrCylinderGetRadius);
	_addMethod(LS,"GetMeridiansCount",scrCylinderGetMeridiansCount);
	_addMethod(LS,"GetHeight",scrCylinderGetHeight);
}


/************************************************************************/
/* CONE                                                                 */
/************************************************************************/
void _isArgCone(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,CONE_TAG))
		_argError(LS,arg,"Cone",method);
}
int scrCone(lua_State* LS)
{
	int nargs = _hasArgs(LS,4,"Cone");
	_isArgNumber(LS,1,"Cone");
	_isArgNumber(LS,2,"Cone");   
	_isArgNumber(LS,3,"Cone");   
	_isArgNumber(LS,4,"Cone");   
	double rad_1 = _getDouble(LS,nargs,1,1.0);
	if (rad_1<=0.0)
		_argMustBeMoreThanError(LS,1,0.0,"Cone");

	double rad_2 = _getDouble(LS,nargs,2,1.0);
	if (rad_2<=0.0)
		_argMustBeMoreThanError(LS,2,0.0,"Cone");

	double heig = _getDouble(LS,nargs,3,1.0);
	if (heig<=0.0)
		_argMustBeMoreThanError(LS,3,0.0,"Cone");

	int mer_cnt = _getInt(LS,nargs,4,24);
	if (mer_cnt<=3)
		_argMustBeMoreThanError(LS,4,3,"Cone");

	sgCCone* con = sgCreateCone(rad_1,rad_2,heig,mer_cnt);
	if (con==NULL)
		_cantCreateObjectError(LS,"Cone");

	sgGetScene()->AttachObject(con);
	_createInstance(LS,CONE_TAG,con);
	return 1;
}
int scrConeGetRadius1(lua_State* LS)
{
	_hasArgs(LS,1,"GetRadius1");
	_isArgCone(LS,1,"GetRadius1"); // self
	sgCCone* con = (sgCCone*)_getSelfInstance(LS);
	if (con==NULL)
		_badObject(LS);

	SG_CONE con_geo;
	con->GetGeometry(con_geo);
	lua_pushnumber(LS,con_geo.Radius1);
	return 1;
}
int scrConeGetRadius2(lua_State* LS)
{
	_hasArgs(LS,1,"GetRadius2");
	_isArgCone(LS,1,"GetRadius2"); // self
	sgCCone* con = (sgCCone*)_getSelfInstance(LS);
	if (con==NULL)
		_badObject(LS);

	SG_CONE con_geo;
	con->GetGeometry(con_geo);
	lua_pushnumber(LS,con_geo.Radius2);
	return 1;
}
int scrConeGetMeridiansCount(lua_State* LS)
{
	_hasArgs(LS,1,"GetMeridiansCount");
	_isArgCone(LS,1,"GetMeridiansCount"); // self
	sgCCone* con = (sgCCone*)_getSelfInstance(LS);
	if (con==NULL)
		_badObject(LS);

	SG_CONE con_geo;
	con->GetGeometry(con_geo);
	lua_pushnumber(LS,con_geo.MeridiansCount);
	return 1;
}
int scrConeGetHeight(lua_State* LS)
{
	_hasArgs(LS,1,"GetHeight");
	_isArgCone(LS,1,"GetHeight"); // self
	sgCCone* con = (sgCCone*)_getSelfInstance(LS);
	if (con==NULL)
		_badObject(LS);

	SG_CONE con_geo;
	con->GetGeometry(con_geo);
	lua_pushnumber(LS,con_geo.Height);
	return 1;
}
void _addConeMethods(lua_State* LS)
{
	_addMethod(LS,"GetRadius1",scrConeGetRadius1);
	_addMethod(LS,"GetRadius2",scrConeGetRadius2);
	_addMethod(LS,"GetMeridiansCount",scrConeGetMeridiansCount);
	_addMethod(LS,"GetHeight",scrConeGetHeight);
}


/************************************************************************/
/* TORUS                                                               */
/************************************************************************/
void _isArgTorus(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,TORUS_TAG))
		_argError(LS,arg,"Torus",method);
}
int scrTorus(lua_State* LS)
{
	int nargs = _hasArgs(LS,4,"Torus");
	_isArgNumber(LS,1,"Torus");
	_isArgNumber(LS,2,"Torus");   
	_isArgNumber(LS,3,"Torus");   
	_isArgNumber(LS,4,"Torus");   
	double rad_1 = _getDouble(LS,nargs,1,1);
	if (rad_1<=0.0)
		_argMustBeMoreThanError(LS,1,0.0,"Torus");

	double rad_2 = _getDouble(LS,nargs,2,1);
	if (rad_2<=0.0)
		_argMustBeMoreThanError(LS,2,0.0,"Torus");

	int mer_cnt_1 = _getInt(LS,nargs,3,24);
	if (mer_cnt_1<=3)
		_argMustBeMoreThanError(LS,3,3,"Torus");

	int mer_cnt_2 = _getInt(LS,nargs,4,24);
	if (mer_cnt_2<=3)
		_argMustBeMoreThanError(LS,4,3,"Torus");

	sgCTorus* tr = sgCreateTorus(rad_1,rad_2,mer_cnt_1,mer_cnt_2);
	if (tr==NULL)
		_cantCreateObjectError(LS,"Torus");

	sgGetScene()->AttachObject(tr);
	_createInstance(LS,TORUS_TAG,tr);
	return 1;
}
int scrTorusGetRadius(lua_State* LS)
{
	_hasArgs(LS,1,"GetRadius");
	_isArgTorus(LS,1,"GetRadius"); // self
	sgCTorus* tr = (sgCTorus*)_getSelfInstance(LS);
	if (tr==NULL)
		_badObject(LS);

	SG_TORUS tr_geo;
	tr->GetGeometry(tr_geo);
	lua_pushnumber(LS,tr_geo.Radius1);
	return 1;
}
int scrTorusGetThickness(lua_State* LS)
{
	_hasArgs(LS,1,"GetThickness");
	_isArgTorus(LS,1,"GetThickness"); // self
	sgCTorus* tr = (sgCTorus*)_getSelfInstance(LS);
	if (tr==NULL)
		_badObject(LS);

	SG_TORUS tr_geo;
	tr->GetGeometry(tr_geo);
	lua_pushnumber(LS,tr_geo.Radius2);
	return 1;
}
void _addTorusMethods(lua_State* LS)
{
	_addMethod(LS,"GetRadius",scrTorusGetRadius);
	_addMethod(LS,"GetThickness",scrTorusGetThickness);
}


/************************************************************************/
/* ELLIPSOID                                                            */
/************************************************************************/
void _isArgEllipsoid(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,ELLIPSOID_TAG))
		_argError(LS,arg,"Ellipsoid",method);
}
int scrEllipsoid(lua_State* LS)
{
	int nargs = _hasArgs(LS,5,"Ellipsoid");
	_isArgNumber(LS,1,"Ellipsoid");
	_isArgNumber(LS,2,"Ellipsoid");   
	_isArgNumber(LS,3,"Ellipsoid");   
	_isArgNumber(LS,4,"Ellipsoid");  
	_isArgNumber(LS,5,"Ellipsoid");  
	double rad_1 = _getDouble(LS,nargs,1,1);
	if (rad_1<=0.0)
		_argMustBeMoreThanError(LS,1,0.0,"Ellipsoid");

	double rad_2 = _getDouble(LS,nargs,2,1);
	if (rad_2<=0.0)
		_argMustBeMoreThanError(LS,2,0.0,"Ellipsoid");

	double rad_3 = _getDouble(LS,nargs,3,1);
	if (rad_3<=0.0)
		_argMustBeMoreThanError(LS,3,0.0,"Ellipsoid");

	int mer_cnt = _getInt(LS,nargs,4,24);
	if (mer_cnt<=3)
		_argMustBeMoreThanError(LS,4,3,"Ellipsoid");

	int par_cnt = _getInt(LS,nargs,5,24);
	if (par_cnt<=3)
		_argMustBeMoreThanError(LS,5,3,"Ellipsoid");

	sgCEllipsoid* ell = sgCreateEllipsoid(rad_1,rad_2,rad_3,mer_cnt,par_cnt);
	if (ell==NULL)
		_cantCreateObjectError(LS,"Ellipsoid");

	sgGetScene()->AttachObject(ell);
	_createInstance(LS,ELLIPSOID_TAG,ell);
	return 1;
}
int scrEllipsoidGetXSize(lua_State* LS)
{
	_hasArgs(LS,1,"GetXSize");
	_isArgEllipsoid(LS,1,"GetXSize"); // self
	sgCEllipsoid* ell = (sgCEllipsoid*)_getSelfInstance(LS);
	if (ell==NULL)
		_badObject(LS);

	SG_ELLIPSOID ell_geo;
	ell->GetGeometry(ell_geo);
	lua_pushnumber(LS,ell_geo.Radius1);
	return 1;
}
int scrEllipsoidGetYSize(lua_State* LS)
{
	_hasArgs(LS,1,"GetYSize");
	_isArgEllipsoid(LS,1,"GetYSize"); // self
	sgCEllipsoid* ell = (sgCEllipsoid*)_getSelfInstance(LS);
	if (ell==NULL)
		_badObject(LS);

	SG_ELLIPSOID ell_geo;
	ell->GetGeometry(ell_geo);
	lua_pushnumber(LS,ell_geo.Radius2);
	return 1;
}
int scrEllipsoidGetZSize(lua_State* LS)
{
	_hasArgs(LS,1,"GetZSize");
	_isArgEllipsoid(LS,1,"GetZSize"); // self
	sgCEllipsoid* ell = (sgCEllipsoid*)_getSelfInstance(LS);
	if (ell==NULL)
		_badObject(LS);

	SG_ELLIPSOID ell_geo;
	ell->GetGeometry(ell_geo);
	lua_pushnumber(LS,ell_geo.Radius3);
	return 1;
}
int scrEllipsoidGetMeridiansCount(lua_State* LS)
{
	_hasArgs(LS,1,"GetMeridiansCount");
	_isArgEllipsoid(LS,1,"GetMeridiansCount"); // self
	sgCEllipsoid* ell = (sgCEllipsoid*)_getSelfInstance(LS);
	if (ell==NULL)
		_badObject(LS);

	SG_ELLIPSOID ell_geo;
	ell->GetGeometry(ell_geo);
	lua_pushnumber(LS,ell_geo.MeridiansCount);
	return 1;
}
int scrEllipsoidGetParallelsCount(lua_State* LS)
{
	_hasArgs(LS,1,"GetParallelsCount");
	_isArgEllipsoid(LS,1,"GetParallelsCount"); // self
	sgCEllipsoid* ell = (sgCEllipsoid*)_getSelfInstance(LS);
	if (ell==NULL)
		_badObject(LS);

	SG_ELLIPSOID ell_geo;
	ell->GetGeometry(ell_geo);
	lua_pushnumber(LS,ell_geo.ParallelsCount);
	return 1;
}
void _addEllipsoidMethods(lua_State* LS)
{
	_addMethod(LS,"GetXSize",scrEllipsoidGetXSize);
	_addMethod(LS,"GetYSize",scrEllipsoidGetYSize);
	_addMethod(LS,"GetZSize",scrEllipsoidGetZSize);
	_addMethod(LS,"GetMeridiansCount",scrEllipsoidGetMeridiansCount);
	_addMethod(LS,"GetParallelsCount",scrEllipsoidGetParallelsCount);
}


/************************************************************************/
/* SPHERIC BAND                                                         */
/************************************************************************/
void _isArgSphericBand(lua_State* LS, int arg, char* method)
{
	if(!_isChild(LS,arg,SPHERIC_BAND_TAG))
		_argError(LS,arg,"SphericBand",method);
}
int scrSphericBand(lua_State* LS)
{
	int nargs = _hasArgs(LS,4,"SphericBand");
	_isArgNumber(LS,1,"SphericBand");
	_isArgNumber(LS,2,"SphericBand");   
	_isArgNumber(LS,3,"SphericBand");   
	_isArgNumber(LS,4,"SphericBand");   
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
}
int scrSphericBandGetRadius(lua_State* LS)
{
	_hasArgs(LS,1,"GetRadius");
	_isArgSphericBand(LS,1,"GetRadius"); // self
	sgCSphericBand* spb = (sgCSphericBand*)_getSelfInstance(LS);
	if (spb==NULL)
		_badObject(LS);

	SG_SPHERIC_BAND spb_geo;
	spb->GetGeometry(spb_geo);
	lua_pushnumber(LS,spb_geo.Radius);
	return 1;
}
int scrSphericBandCoefficient1(lua_State* LS)
{
	_hasArgs(LS,1,"GetCoefficient1");
	_isArgSphericBand(LS,1,"GetCoefficient1"); // self
	sgCSphericBand* spb = (sgCSphericBand*)_getSelfInstance(LS);
	if (spb==NULL)
		_badObject(LS);

	SG_SPHERIC_BAND spb_geo;
	spb->GetGeometry(spb_geo);
	lua_pushnumber(LS,spb_geo.BeginCoef);
	return 1;
}
int scrSphericBandCoefficient2(lua_State* LS)
{
	_hasArgs(LS,1,"GetCoefficient2");
	_isArgSphericBand(LS,1,"GetCoefficient2"); // self
	sgCSphericBand* spb = (sgCSphericBand*)_getSelfInstance(LS);
	if (spb==NULL)
		_badObject(LS);

	SG_SPHERIC_BAND spb_geo;
	spb->GetGeometry(spb_geo);
	lua_pushnumber(LS,spb_geo.EndCoef);
	return 1;
}
void _addSphericBandMethods(lua_State* LS)
{
	_addMethod(LS,"GetRadius",scrSphericBandGetRadius);
	_addMethod(LS,"GetCoefficient1",scrSphericBandCoefficient1);
	_addMethod(LS,"GetCoefficient2",scrSphericBandCoefficient2);
}


