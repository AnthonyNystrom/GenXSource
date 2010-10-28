#pragma once
#include "sgCore/sgAlgs.h"
#include "Objects/msgGroup.h"
#include "Objects/2D/msg2DObject.h"
#include "Structs/msgPointStruct.h"
#include "Helpers/ObjectCreateHelper.h"

using namespace sgCoreWrapper::Structs;
using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgKinematic abstract sealed
		{
		public:
			static msgObject^ Rotation(msg2DObject^ rotObj, msgPointStruct^ axePnt1,
				msgPointStruct^ axePnt2, double angl_degree, bool isClose)
			{
				return ObjectCreateHelper::CreateObject(sgKinematic::Rotation(*rotObj->sg2DObject,
					*axePnt1->_point, *axePnt2->_point, angl_degree, isClose));
			}

			static msgObject^ Extrude(msg2DObject^ outContour, array<msg2DObject^>^ holes,
				msgPointStruct^ extrDir, bool isClose)
			{
				if (holes == nullptr)
				{
					return ObjectCreateHelper::CreateObject(sgKinematic::Extrude(
					*outContour->sg2DObject, NULL, 0, *extrDir->_point, isClose));
				}

				const sgC2DObject** uholes = GetUnHoles(holes);
				msgObject^ obj = ObjectCreateHelper::CreateObject(sgKinematic::Extrude(
					*outContour->sg2DObject, uholes, holes->Length, *extrDir->_point, isClose));
				delete[] uholes;
				return obj;
			}

			static msgObject^ Spiral(msg2DObject^ outContour, array<msg2DObject^>^ holes,
				msgPointStruct^ axePnt1, msgPointStruct^ axePnt2, double screw_step,
				double screw_height, short meridians_count, bool isClose)
			{
				if (holes == nullptr)
				{
					return ObjectCreateHelper::CreateObject(sgKinematic::Spiral(
						*outContour->sg2DObject, NULL, 0, *axePnt1->_point, *axePnt2->_point,
						screw_step, screw_height, meridians_count, isClose));
				}

				const sgC2DObject** uholes = GetUnHoles(holes);
				msgObject^ obj = ObjectCreateHelper::CreateObject(sgKinematic::Spiral(
					*outContour->sg2DObject, uholes, holes->Length, *axePnt1->_point, *axePnt2->_point,
					screw_step, screw_height, meridians_count, isClose));
				delete[] uholes;
				return obj;
			}

			static msgObject^ Pipe(msg2DObject^ outContour, array<msg2DObject^>^ holes,
				msg2DObject^ guideContour, msgPointStruct^ point_in_outContour_plane,
				double angle_around_point_in_outContour_plane, bool% isClose)
			{
				bool isClose_val = isClose;
				if (holes == nullptr)
				{
					msgObject^ obj = ObjectCreateHelper::CreateObject(sgKinematic::Pipe(
						*outContour->sg2DObject, NULL, 0, *guideContour->sg2DObject,
						*point_in_outContour_plane->_point, angle_around_point_in_outContour_plane,
						isClose_val));
					isClose = isClose_val;
					return obj;
				}

				const sgC2DObject** uholes = GetUnHoles(holes);
				msgObject^ obj = ObjectCreateHelper::CreateObject(sgKinematic::Pipe(
					*outContour->sg2DObject, uholes, holes->Length, *guideContour->sg2DObject,
					*point_in_outContour_plane->_point, angle_around_point_in_outContour_plane,
					isClose_val));
				isClose = isClose_val;
				delete[] uholes;
				return obj;
			}

		private:
			static const sgC2DObject** GetUnHoles(array<msg2DObject^>^ holes)
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