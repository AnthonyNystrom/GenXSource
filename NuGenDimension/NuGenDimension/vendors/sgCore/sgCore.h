#ifndef   __sgCore__
#define  __sgCore__

#pragma pack(1)

#include "sgDefs.h"
#include "sgErrors.h"
#include "sgMatrix.h"
#include "sgSpaceMath.h"
#include "sgObject.h"
#include "sg2D.h"
#include "sgGroup.h"
#include "sg3D.h"

#include "sgScene.h"
#include "sgAlgs.h"


#include "sgTD.h"
#include "sgIApp.h"
#include "sgFileManager.h"

sgCore_API    bool   sgInitKernel();
sgCore_API    void   sgFreeKernel();
sgCore_API    void   sgGetVersion(int& major, int& minor, int& release, int& build);
/****************************************************************/
/*************************************************************************************/

#endif    // __sgCore__