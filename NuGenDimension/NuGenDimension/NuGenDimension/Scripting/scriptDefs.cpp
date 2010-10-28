#include "stdafx.h"

#include "scriptDefs.h"
#include "ScriptDlg.h"

int sc_printf(const char *message,...)
{
	int ret;
	va_list argptr;

	va_start(argptr,message);
	ret=vprintf(message,argptr);
	va_end(argptr);

	if (global_loger)
		global_loger->SetMessage(message);

	return ret;
}

int _hasArgs(lua_State* LS, int minArgs, char* method) 
{
  int nargs = lua_gettop(LS);
  if(nargs < minArgs) 
  {
    char msg[MAX_CHARS_IN_ERROR];
	//sprintf(msg,"'%s' needs %d argument(s)",method,minArgs);#OBSOLETE RISK
	sprintf_s(msg,"'%s' needs %d argument(s)",method,1024,minArgs);
    lua_error(LS,msg);
  }
  return nargs;
}

void _argError(lua_State* LS, int arg, char* type, char* method) 
{
  char msg[MAX_CHARS_IN_ERROR];
  //sprintf(msg,"'%s' needs one '%s' as argument %d",method,type,arg);#OBSOLETE RISK
  sprintf_s(msg,"'%s' needs one '%s' as argument %d",method,1024,type,1024,arg);
  lua_error(LS,msg);
}

void _argMustBeMoreThanError(lua_State* LS, int arg, double minVal, char* method)
{
	char msg[MAX_CHARS_IN_ERROR];
	//sprintf(msg,"'%s' needs as argument %d more than %.2f",method,arg,minVal);#OBSOLETE RISK
	sprintf_s(msg,"'%s' needs as argument %d more than %.2f",method,1024,arg,minVal);
	lua_error(LS,msg);
}

void _argMustBeLessThanError(lua_State* LS, int arg, double maxVal, char* method)
{
	char msg[MAX_CHARS_IN_ERROR];
	//sprintf(msg,"'%s' needs as argument %d less than %.2f",method,arg,maxVal);#OBSOLETE RISK
	sprintf_s(msg,"'%s' needs as argument %d less than %.2f",method,1024,arg,maxVal);
	lua_error(LS,msg);
}

void _cantCreateObjectError(lua_State* LS, char* method)
{
	char msg[MAX_CHARS_IN_ERROR];
	//sprintf(msg,"Can not create object - '%s' function",method);#OBSOLETE RISK
	sprintf_s(msg,"Can not create object - '%s' function",method,1024);
	lua_error(LS,msg);
}

void _badObject(lua_State* LS)
{
	char msg[20];
	//sprintf(msg,"Bad object");#OBSOLETE
	sprintf_s(msg,"Bad object");
	lua_error(LS,msg);
}

void _isArgNumber(lua_State* LS, int arg, char* method) 
{
  if(!lua_isnumber(LS,arg))
    _argError(LS,arg,"number",method);
}

void _isArgString(lua_State* LS, int arg, char* method) 
{
  if(!lua_isstring(LS,arg))
    _argError(LS,arg,"string",method);
}

void _isArgPointer(lua_State* LS, int arg, char* method) 
{
  if(!lua_isuserdata(LS,arg))
    _argError(LS,arg,"pointer",method);
}

void _isArgTable(lua_State* LS, int arg, char* method) {
	if(!lua_istable(LS,arg))
		_argError(LS,arg,"table",method);
}

void _isArgFunction(lua_State* LS, int arg, char* method) {
	if(!lua_isfunction(LS,arg))
		_argError(LS,arg,"function",method);
}

void _getTableElement(lua_State* LS, int table, 
									int idx, int tag, char* type, char* method) 
{
	lua_rawgeti(LS,table,idx);
	if(lua_tag(LS,-1) != tag) 
	{
		char msg[MAX_CHARS_IN_ERROR];
		//sprintf(msg,"'%s' needs a table with '%s' elements",method,type);#OBSOLETE RISK
		sprintf_s(msg,"'%s' needs a table with '%s' elements",method,1024,type,1024);
		lua_error(LS,msg);
	}
}

bool _isChild(lua_State* LS, int arg, int tag) {
	int childTag = lua_tag(LS,arg);
	if(childTag != tag) {
		lua_getglobal(LS,"_CLASS");
		do {
			lua_rawgeti(LS,-1,childTag);
			lua_pushstring(LS,"_PARENT");
			lua_gettable(LS,-2);
			if(lua_isnil(LS,-1)) {
				lua_pop(LS,2);
				return false;
			}
			childTag = (int)lua_tonumber(LS,-1);
			lua_pop(LS,2);
		} while(childTag != tag);
		lua_pop(LS,1);
	}
	return true;
}

void _createInstance(lua_State* LS, int tag, void* pointer) {
	lua_newtable(LS);
	lua_settag(LS,tag);
	lua_pushstring(LS,"ptr");
	lua_pushuserdata(LS,pointer);
	lua_settable(LS,-3);
}

void* _getInstance(lua_State* LS, int tableIndex) 
{
	lua_pushstring(LS,"ptr");
	lua_gettable(LS,tableIndex);
	void* ptr = lua_touserdata(LS,-1);
	lua_pop(LS,1);
	return ptr;
}

void* _getSelfInstance(lua_State* LS) {
	return _getInstance(LS,1);
}

void _deleteSelfInstance(lua_State* LS) {
	lua_pushstring(LS,"ptr");
	lua_pushnil(LS);
	lua_settable(LS,1);
}

bool _getBool(lua_State* LS, int nargs, int arg, bool def) 
{
  return nargs < arg? def: (!lua_isnil(LS,arg));
}

int _getInt(lua_State* LS, int nargs, int arg, int def) 
{
  return nargs < arg? def:
    (lua_isnumber(LS,arg)? (int)lua_tonumber(LS,arg): def);
}

double _getDouble(lua_State* LS, int nargs, int arg, float def) 
{
  return nargs < arg? def:
    (lua_isnumber(LS,arg)? (double)lua_tonumber(LS,arg): def);
}

char* _getString(lua_State* LS, int nargs, int arg) 
{
  return nargs < arg? NULL:
    (lua_isstring(LS,arg)? (char*)lua_tostring(LS,arg): NULL);
}

void* _getPointer(lua_State* LS, int nargs, int arg) 
{
  if(nargs >= arg && lua_isuserdata(LS,arg)) 
  {
    void* ptr = lua_touserdata(LS,arg);
    return ptr;
  } else
    return NULL;
}

void _addMethod(lua_State* LS, char* name, LLCFunction f) {
	lua_pushstring(LS,name);
	lua_pushcfunction(LS,f);
	lua_settable(LS,-3);
}


int _methodFinder(lua_State* LS) {
	lua_getglobal(LS,"_CLASS");
	lua_rawgeti(LS,-1,lua_tag(LS,1));
	lua_pushvalue(LS,2);
	lua_gettable(LS,-2);
	while(lua_isnil(LS,-1)) {
		lua_pop(LS,1);
		lua_pushstring(LS,"_PARENT");
		lua_gettable(LS,-2);
		if(lua_isnil(LS,-1)) {
			char msg[MAX_CHARS_IN_ERROR];
			//sprintf(msg,"method '%s' not found in table",lua_tostring(LS,2));#OBSOLETE RISK
			const char *ppp = lua_tostring(LS,2);
			sprintf_s(msg,"method '%s' not found in table",ppp,1024);
			lua_error(LS,msg);
		} else {
			int parentTag = (int)lua_tonumber(LS,-1);
			lua_pop(LS,2);
			lua_rawgeti(LS,-1,parentTag);
			lua_pushvalue(LS,2);
			lua_gettable(LS,-2);
		}
	}
	return 1;
}

void _setupClass(lua_State* LS, int& tag, void (*addMethods)(lua_State* LS),
						int parentTag) 
{
		tag = lua_newtag(LS);
		lua_pushcfunction(LS,_methodFinder);
		lua_settagmethod(LS,tag,"index");
		lua_getglobal(LS,"_CLASS");
		lua_newtable(LS);
		if(parentTag != LUA_NOTAG) 
		{
			lua_pushstring(LS,"_PARENT");
			lua_pushnumber(LS,parentTag);
			lua_settable(LS,-3);
		}
		addMethods(LS);
		lua_rawseti(LS,-2,tag);
		lua_pop(LS,1);
}

void _setDestroyer(lua_State* LS, int tag, LLCFunction destroyer	) 
{
	lua_pushcfunction(LS,destroyer);
	lua_settagmethod(LS,tag,"gc");
}

