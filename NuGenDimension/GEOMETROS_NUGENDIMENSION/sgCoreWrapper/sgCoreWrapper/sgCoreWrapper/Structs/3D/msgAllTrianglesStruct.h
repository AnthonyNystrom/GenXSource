#pragma once
#include "sgCore/sg3D.h"
#include "Structs/msgPointStruct.h"
#include "Structs/msgDoubleStruct.h"
#include "Helpers/ArrayHelper.h"

using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgAllTrianglesStruct
		{
		public:
			property array<msgPointStruct^>^ allVertex
			{
				array<msgPointStruct^>^ get() { return _allVertex; }
			}
			property array<msgVectorStruct^>^ allNormals
			{
				array<msgVectorStruct^>^ get() { return _allNormals; }
			}
			property array<msgDoubleStruct^>^ allUV
			{
				array<msgDoubleStruct^>^ get() { return _allUV; }
			}
		internal:
			msgAllTrianglesStruct(SG_ALL_TRIANGLES* allTriangles)
			{
				_sgAllTriangles = allTriangles;
				int count = _sgAllTriangles->nTr * 3;
				_allVertex = ArrayHelper::GetManagedArray<msgPointStruct, SG_POINT>(_sgAllTriangles->allVertex, 
					count);
				_allNormals = ArrayHelper::GetManagedArray<msgVectorStruct, SG_POINT>(_sgAllTriangles->allNormals, 
					count);
				_allUV = ArrayHelper::GetManagedArray<msgDoubleStruct, double>(_sgAllTriangles->allUV, 
					count * 2);
			}
		private:
			array<msgPointStruct^>^ _allVertex;
			array<msgVectorStruct^>^ _allNormals;
			array<msgDoubleStruct^>^ _allUV;
			SG_ALL_TRIANGLES* _sgAllTriangles;
		};

		public ref struct msgIndexTriangle
		{
		public:
			msgIndexTriangle()
			{
				_ind_triangle = new SG_INDEX_TRIANGLE();
				_needDelete = true;
			}

			~msgIndexTriangle()
			{
				this->!msgIndexTriangle();
			}

			!msgIndexTriangle()
			{
				if (_needDelete)
				{
					delete _ind_triangle;
				}
			}

			property long first_index
			{
				long get() { return _ind_triangle->ver_indexes[0]; }
				void set(long value) { _ind_triangle->ver_indexes[0] = value; }
			}

			property long second_index
			{
				long get() { return _ind_triangle->ver_indexes[1]; }
				void set(long value) { _ind_triangle->ver_indexes[1] = value; }
			}

			property long third_index
			{
				long get() { return _ind_triangle->ver_indexes[2]; }
				void set(long value) { _ind_triangle->ver_indexes[2] = value; }
			}

		private:
			bool _needDelete;

		internal:
			msgIndexTriangle(SG_INDEX_TRIANGLE* ind_tr)
			{
				_ind_triangle = ind_tr;
				_needDelete = false;
			}

			SG_INDEX_TRIANGLE* _ind_triangle;
		};
	}
}