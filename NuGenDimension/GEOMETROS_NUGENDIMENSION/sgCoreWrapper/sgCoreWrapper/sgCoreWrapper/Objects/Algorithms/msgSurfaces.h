#pragma once
#include "sgCore/sgAlgs.h"
#include "Objects/msgGroup.h"
#include "Objects/msgObject.h"
#include "Objects/2D/msg2DObject.h"
#include "Objects/3D/msg3DObject.h"
#include "Structs/msgPointStruct.h"
#include "Helpers/ObjectCreateHelper.h"

using namespace sgCoreWrapper::Structs;
using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgSurfaces abstract sealed
		{
		public:
			static msgObject^ Face(msg2DObject^ outContour, array<msg2DObject^>^ holes)
			{
				if (holes == nullptr)
				{
					return ObjectCreateHelper::CreateObject(sgSurfaces::Face(*outContour->sg2DObject, NULL, 0));
				}

				const sgC2DObject** uholes = GetC2DObject(holes);
				msgObject^ obj = ObjectCreateHelper::CreateObject(sgSurfaces::Face(
					*outContour->sg2DObject, uholes, holes->Length));
				delete[] uholes;
				return obj;
			}

			static msgObject^ Coons(msg2DObject^ firstSide, msg2DObject^ secondSide,
				msg2DObject^ thirdSide, msg2DObject^ fourthSide, short uSegments,
				short vSegments, short uVisEdges, short vVisEdges)
			{
				return ObjectCreateHelper::CreateObject(sgSurfaces::Coons(
					*firstSide->sg2DObject, *secondSide->sg2DObject, *thirdSide->sg2DObject,
					fourthSide->sg2DObject, uSegments, vSegments, uVisEdges, vVisEdges));
			}

			static msgObject^ Mesh(short dimens_1, short dimens_2, array<msgPointStruct^>^ pnts)
			{
				SG_POINT* points = new SG_POINT[dimens_1*dimens_2];
				for (int i = 0; i < dimens_1*dimens_2; i++)
				{
					points[i] = *pnts[i]->_point;
				}
				msgObject^ result = ObjectCreateHelper::CreateObject(sgSurfaces::Mesh(dimens_1, dimens_2, points));
				delete[] points;
				return result;
			}

			static msgObject^ SewSurfaces(array<msg3DObject^>^ surfaces)
			{
				int count = surfaces->Length;
				const sgC3DObject** usurfaces = new const sgC3DObject*[count];
				for (int i = 0; i < count; i++)
				{
					usurfaces[i] = surfaces[i]->sg3DObject;
				}
				msgObject^ obj = ObjectCreateHelper::CreateObject(sgSurfaces::SewSurfaces(usurfaces, count));
				delete[] usurfaces;
				return obj;
			}

			static msgObject^ LinearSurfaceFromSections(msg2DObject^ firstSide,
				msg2DObject^ secondSide, double firstParam, bool isClose)
			{
				return ObjectCreateHelper::CreateObject(sgSurfaces::LinearSurfaceFromSections(
					*firstSide->sg2DObject,	*secondSide->sg2DObject, firstParam, isClose));
			}

			static msgObject^ SplineSurfaceFromSections(array<msg2DObject^>^ sections,
				array<double>^ params, bool isClose)
			{
				const sgC2DObject** usections = GetC2DObject(sections);
				double* uparams = new double[params->Length];
				for (int i = 0; i < params->Length; i++)
				{
					uparams[i] = params[i];
				}
				msgObject^ result =  ObjectCreateHelper::CreateObject(
					sgSurfaces::SplineSurfaceFromSections(
					usections, (const double*)uparams, sections->Length, isClose));
				delete[] uparams;
				return result;
			}
		private:
			static const sgC2DObject** GetC2DObject(array<msg2DObject^>^ holes)
			{
				int count = holes->Length;
				const sgC2DObject** uholes = new const sgC2DObject*[count];
				for (int i = 0; i < count; i++)
				{
					uholes[i] = holes[i]->sg2DObject;
				}
				return uholes;
			}
		};
	}
}