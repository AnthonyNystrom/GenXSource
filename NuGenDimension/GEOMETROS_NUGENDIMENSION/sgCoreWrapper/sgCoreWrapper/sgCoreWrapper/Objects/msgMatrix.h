#pragma once
#include "sgCore/sgMatrix.h"
#include "Structs/msgMatrixRepresentationStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{

		/*sgCMatrix();
		sgCMatrix(const double*);
		sgCMatrix& operator = (const sgCMatrix& );*/
		public ref class msgMatrix
		{
		public:
			msgMatrix()
			{
				_sgCMatrix = new sgCMatrix();
				_needDelete = true;
			}

			msgMatrix(double dim)
			{
				_sgCMatrix = new sgCMatrix(&dim);
				_needDelete = true;
			}

			~msgMatrix()
			{
				this->!msgMatrix();
			}

			!msgMatrix()
			{
				if (_needDelete)
				{
					delete _sgCMatrix;
				}
			}

			bool SetMatrix(msgMatrix matrix)
			{
				return _sgCMatrix->SetMatrix(matrix._sgCMatrix);
			}

			msgMatrixRepresentationStruct^ GetData()
			{
				return gcnew msgMatrixRepresentationStruct((double*)_sgCMatrix->GetData());
			}

			msgMatrixRepresentationStruct^ GetTransparentData()
			{
				return gcnew msgMatrixRepresentationStruct((double*)_sgCMatrix->GetTransparentData());
			}

			void Identity()
			{
				_sgCMatrix->Identity();
			}

			void Transparent()
			{
				_sgCMatrix->Transparent();
			}

			bool Inverse()
			{
				return _sgCMatrix->Inverse();
			}

			void Multiply(msgMatrix^ matrB)
			{
				_sgCMatrix->Multiply(*matrB->_sgCMatrix);
			}

			void Translate(msgVectorStruct^ transVector)
			{
				_sgCMatrix->Translate(*transVector->_point);
			}

			void Rotate(msgPointStruct^ axePoint, msgVectorStruct^ axeDir, double alpha_radians)
			{
				_sgCMatrix->Rotate(*axePoint->_point, *axeDir->_point, alpha_radians);
			}

			void VectorToZAxe(msgVectorStruct^ vect)
			{
				_sgCMatrix->VectorToZAxe(*vect->_point);
			}

			void ApplyMatrixToVector(msgPointStruct^ vectBegin, msgVectorStruct^ vectDir)
			{
				_sgCMatrix->ApplyMatrixToVector(*vectBegin->_point, *vectDir->_point);
			}

			void ApplyMatrixToPoint(msgPointStruct^ pnt)
			{
				_sgCMatrix->ApplyMatrixToPoint(*pnt->_point);
			}
		private:
			bool _needDelete;
	
		internal:
			msgMatrix(sgCMatrix* matrix)
			{
				_sgCMatrix = matrix;
				_needDelete = false;
			}
			
			sgCMatrix* _sgCMatrix;
		};
	}
}