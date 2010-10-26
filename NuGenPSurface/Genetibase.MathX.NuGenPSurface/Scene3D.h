#pragma once

namespace Genetibase
{
	namespace NuGenPSurface
	{
		namespace AvalonBridge
		{
			ref struct TexCoord2D
			{
			public:
				float U, V;
			};

			ref struct Vector3D
			{
			public:
				float X, Y, Z;

				inline Vector3D(float x, float y, float z) { X = x; Y = y; Z = z; }
			};

			ref struct Vector4D
			{
			public:
				float X, Y, Z, W;
			};

			ref struct Geometry3D
			{
			public:
				array<Vector3D^>^ Vertices;
				array<Vector3D^>^ Normals;
				array<TexCoord2D^>^ TexCoords;
				array<int>^ PrimIndices;
			};

			ref struct ColorARGB
			{
			public:
				unsigned char A, R, G, B;
			};

			ref struct Material
			{
				ColorARGB^ Ambient, Diffuse;
			};

			ref class Model3D
			{
			public:
				Geometry3D^ Geometry;
				Material^ Material;
				Vector3D^ Transform;
				Vector4D^ Rotation;
				Vector3D^ Scaling;

				System::Windows::Media::Media3D::GeometryModel3D^ ToAvalonObj(void);
			};

			ref class Scene3D
			{
			public:
				array<Model3D^>^ Models;

				Scene3D(void);
				System::Windows::Media::Media3D::Model3DGroup^ ToAvalonObj(void);
			};

		}
	}
}