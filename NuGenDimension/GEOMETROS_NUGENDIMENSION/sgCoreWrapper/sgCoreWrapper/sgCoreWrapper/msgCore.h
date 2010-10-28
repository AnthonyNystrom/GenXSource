#pragma once
#include "sgCore\sgCore.h"
#include "Objects\msgFileManager.h"
#include "Objects\msgObject.h"
#include "Objects\msgMatrix.h"
#include "Objects\msgGroup.h"
#include "Objects\msgScene.h"
#include "Objects\msgPoint.h"
#include "Objects\Algorithms\msgSpaceMath.h"
#include "Objects\2d\msgContour.h"
#include "Objects\2D\msgLine.h"
#include "Objects\2D\msgCircle.h"
#include "Objects\2D\msgArc.h"
#include "Objects\2D\msgSpline.h"
#include "Objects\3D\msgBox.h"
#include "Objects\3D\msgCone.h"
#include "Objects\3D\msgCylinder.h"
#include "Objects\3D\msgEllipsoid.h"
#include "Objects\3D\msgSphere.h"
#include "Objects\3D\msgSphericBand.h"
#include "Objects\3D\msgTorus.h"
#include "Objects\Algorithms\msgBoolean.h"
#include "Objects\Algorithms\msgKinematic.h"
#include "Objects\Algorithms\msgSurfaces.h"
#include "Objects\Text\msgDimensions.h"
#include "Objects\Text\msgFont.h"
#include "Objects\Text\msgFontManager.h"
#include "Objects\Text\msgText.h"

namespace sgCoreWrapper
{
  public ref class msgCore abstract sealed
  {
  public:
    static bool InitKernel()
    {
      return sgInitKernel();
    }

    static void FreeKernel(bool show_mem_leak)
    {
      sgFreeKernel(show_mem_leak);
    }

    static void GetVersion(int% major, int% minor, int% release, int% build)
    {
      int major_val = major;
      int minor_val = minor;
      int release_val = release;
      int build_val = build;
      sgGetVersion(major_val, minor_val, release_val, build_val);
      major = major_val;
      minor = minor_val;
      release = release_val;
      build_val = build;
    }
  };
}