#pragma once
#include "sgCore\sg3D.h"
#include "Objects\msgObject.h"
#include "Objects\3D\msgBRep.h"
#include "Structs\3D\msgAllTrianglesStruct.h"
#include "Structs\3D\msgMaterialStruct.h"
#include "Structs\msgMatrixRepresentationStruct.h"

namespace sgCoreWrapper
{
	namespace Objects
	{
		public enum class msgTriangulationTypeEnum
		{
			SG_VERTEX_TRIANGULATION,
			SG_DELAUNAY_TRIANGULATION
		};

		public enum class msg3DObjectTypeEnum
		{
			SG_UNKNOWN_3D = 0,
			SG_BODY,
			SG_SURFACE,
		};
		
		public ref class msg3DObject : msgObject
		{
		public:
			static void AutoTriangulate(bool isTriang, msgTriangulationTypeEnum triangulationType)
			{
				sgC3DObject::AutoTriangulate(isTriang, (SG_TRIANGULATION_TYPE)triangulationType);
			}

			bool Triangulate(msgTriangulationTypeEnum triangulationType)
			{
				return sg3DObject->Triangulate((SG_TRIANGULATION_TYPE)triangulationType);
			}

			msg3DObjectTypeEnum Get3DObjectType()
			{
				return (msg3DObjectTypeEnum)sg3DObject->Get3DObjectType();
			}

			msgBRep^ GetBRep()
			{
				return gcnew msgBRep(sg3DObject->GetBRep());
			}

			msgAllTrianglesStruct^ GetTriangles()
			{
				SG_ALL_TRIANGLES* triangs = (SG_ALL_TRIANGLES*)sg3DObject->GetTriangles();
				if (triangs)
				{
					return gcnew msgAllTrianglesStruct(triangs);
				}
				else
				{
					return nullptr;
				}
			}

			bool virtual ApplyTempMatrix() override
			{
				return sg3DObject->ApplyTempMatrix();
			}

			msgMatrixRepresentationStruct^ GetWorldMatrixData()
			{
				return gcnew msgMatrixRepresentationStruct((double*)sg3DObject->GetWorldMatrixData());
			}

			void SetMaterial(msgMaterialStruct^ material)
			{
				sg3DObject->SetMaterial(*material->_sgMaterial);
			}

			msgMaterialStruct^ GetMaterial()
			{
				return gcnew msgMaterialStruct((SG_MATERIAL*)sg3DObject->GetMaterial());
			}

			bool CalculateOptimalUV(double% optU, double% optV)
			{
				double optU_val = optU;
				double optV_val = optV;
				bool result = sg3DObject->CalculateOptimalUV(optU_val, optV_val);
				optU = optU_val;
				optV = optV_val;
				return result;
			}

			double GetVolume()
			{
				return sg3DObject->GetVolume();
			}

			double GetSquare()
			{
				return sg3DObject->GetSquare();
			}
		internal:
			msg3DObject(sgC3DObject* object) : msgObject(object)
			{
			}

			property sgC3DObject* sg3DObject
			{
				sgC3DObject* get() { return (sgC3DObject*)_sgCObject; }
			}
		};
	}
}