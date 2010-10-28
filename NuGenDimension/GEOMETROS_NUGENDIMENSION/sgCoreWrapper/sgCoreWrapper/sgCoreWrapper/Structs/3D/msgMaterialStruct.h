#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public enum msgMixColorTypeEnum
		{
			SG_MODULATE_MIX_TYPE=1,
			SG_BLEND_MIX_TYPE   =2,
			SG_REPLACE_MIX_TYPE =3
		};

		public enum msgUVTypeEnum
		{
			SG_CUBE_UV_TYPE      =1,
			SG_SPHERIC_UV_TYPE   =2,
			SG_CYLINDER_UV_TYPE  =3
		};

		public ref struct msgMaterialStruct
		{
		public:
			msgMaterialStruct()
			{
				_sgMaterial = new SG_MATERIAL();
				_needDelete = true;
			}

			~msgMaterialStruct()
			{
				this->!msgMaterialStruct();
			}

			!msgMaterialStruct()
			{
				if (_needDelete)
				{
					delete _sgMaterial;
				}
			}

			property int MaterialIndex
			{
				int get() { return _sgMaterial->MaterialIndex; }
				void set(int value) { _sgMaterial->MaterialIndex = value;}
			}

			property double TextureScaleU
			{
				double get() { return _sgMaterial->TextureScaleU; }
				void set(double value) { _sgMaterial->TextureScaleU = value; }
			}
			
			property double TextureScaleV
			{
				double get() { return _sgMaterial->TextureScaleV; }
				void set(double value) { _sgMaterial->TextureScaleV = value; }
			}

			property double TextureShiftU
			{
				double get() { return _sgMaterial->TextureShiftU; }
				void set(double value) { _sgMaterial->TextureShiftU = value; }
			}

			property double TextureShiftV
			{
				double get() { return _sgMaterial->TextureShiftV; }
				void set(double value) { _sgMaterial->TextureShiftV = value; }
			}

			property double TextureAngle
			{
				double get() { return _sgMaterial->TextureAngle; }
				void set(double value) { _sgMaterial->TextureAngle = value; }
			}

			property bool TextureSmooth
			{
				bool get() { return _sgMaterial->TextureSmooth; }
				void set(bool value) { _sgMaterial->TextureSmooth = value; }
			}

			property bool TextureMult
			{
				bool get() { return _sgMaterial->TextureMult; }
				void set(bool value) { _sgMaterial->TextureMult = value; }
			}
		private:
			bool _needDelete;
		internal:
			msgMaterialStruct(SG_MATERIAL* material)
			{
				_sgMaterial = material;
				_needDelete = false;
			}

			SG_MATERIAL* _sgMaterial;
		};
	}
}