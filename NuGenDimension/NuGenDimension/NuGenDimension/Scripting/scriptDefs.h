#ifndef __SCRIPT_DEFS_
#define __SCRIPT_DEFS_

#define MAX_CHARS_IN_ERROR 280

int  sc_printf(const char *message,...);
int  _hasArgs(lua_State* LS, int minArgs, char* method);
void _argError(lua_State* LS, int arg, char* type, char* method);
void _argMustBeMoreThanError(lua_State* LS, int arg, double minVal, char* method);
void _argMustBeLessThanError(lua_State* LS, int arg, double maxVal, char* method);
void _cantCreateObjectError(lua_State* LS, char* method);
void _badObject(lua_State* LS);
void _isArgNumber(lua_State* LS, int arg, char* method);
void _isArgString(lua_State* LS, int arg, char* method);
void _isArgPointer(lua_State* LS, int arg, char* method);
void _isArgTable(lua_State* LS, int arg, char* method);
void _isArgFunction(lua_State* LS, int arg, char* method);
void _getTableElement(lua_State* LS, int table, int idx, int tag, char* type, char* method);

bool _isChild(lua_State* LS, int arg, int tag);
void _createInstance(lua_State* LS, int tag, void* pointer);
void* _getInstance(lua_State* LS, int tableIndex = -2);
void* _getSelfInstance(lua_State* LS);
void _deleteSelfInstance(lua_State* LS);

bool     _getBool(lua_State* LS, int nargs, int arg, bool def = false);
int      _getInt(lua_State* LS, int nargs, int arg, int def);
double   _getDouble(lua_State* LS, int nargs, int arg, float def);
char*    _getString(lua_State* LS, int nargs, int arg);
void*    _getPointer(lua_State* LS, int nargs, int arg);

typedef lua_CFunction LLCFunction;

void _addMethod(lua_State* LS, char* name, LLCFunction f);
int  _methodFinder(lua_State* LS);
void _setupClass(lua_State* LS, int& tag, void (*addMethods)(lua_State* LS),
						int parentTag = LUA_NOTAG) ;
void _setDestroyer(lua_State* LS, int tag, LLCFunction destroyer) ;

#endif
