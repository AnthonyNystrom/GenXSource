#ifndef __FRAMEWORK_FRAMEWORK_H__
#define __FRAMEWORK_FRAMEWORK_H__

/*!	@mainpage
Tutorial: creation of framework-based application in VC7:
1. Create empty project

Configure project:

2. Project property->C/C++->General->Additional Include directories->path to framework's .h files

3. Project property->Linker->Input->Additional Dependicies->path to framework.lib

4. Create class derived from IAppicationCore

Example: 
@code
class TestApp : public IApplicationCore
@endcode

5. Implement functions in this class:
@code
virtual void Update();
virtual void Render();
virtual void OnResume();
virtual void OnSuspend();
virtual void OnChar();
@endcode
6. Implement global function:
@code
IApplicationCore* CreateApplication(Application * app)
@endcode
This function must create your class (TestApp) and return pointer to it.
Your class should also save Application pointer(Application * app).

Example:
@code
IApplicationCore* CreateApplication(Application * app)
{
return new TestApp(app);
}
@endcode

7. Implement global function:
@code
void ReleaseApplication(IApplicationCore * appCore)
@endcode
Deletes TestApp

Example: 
@code
void ReleaseApplication(IApplicationCore * appCore)
{
delete appCore;
}
@endcode

In addition for BREW emulator:

8. Project property->Debugging->Command->path to emulator

9. Project property->C/C++->Preprocessor->Preprocessor definitions. 

Set: WIN32;_DEBUG;_WINDOWS;_USRDLL;AEE_SIMULATOR

10. Project property->Linker->Input->Force Symbol References->_AEEClsCreateInstance

11. Implement global functions:
@code
uint32 GetClsID()
uint16 GetVersion()
@endcode

Example:
@code
uint32 GetClsID()
{
return AEECLSID_TIKENGINE;
}

uint16 GetVersion()
{
return 0x0010;
}
@endcode
*/

#include "Utils.h"
#include "Config.h"
#include "Application.h"
#include "IApplicationCore.h"
#include "ResourceSystem.h"
#include "BaseTypes.h"

#endif // __FRAMEWORK_FRAMEWORK_H__