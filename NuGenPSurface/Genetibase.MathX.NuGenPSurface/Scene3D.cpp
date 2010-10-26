#include "Scene3D.h"

using namespace System::Windows;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Media3D;
using namespace Genetibase::NuGenPSurface::AvalonBridge;

Scene3D::Scene3D(void)
{
}

Model3DGroup^ Scene3D::ToAvalonObj(void)
{
	Model3DGroup^ group = gcnew Model3DGroup();
	
	if (Models != nullptr)
	{
		// add models
		for (int model = 0; model < Models->Length; model++)
		{
			group->Children->Add(Models[model]->ToAvalonObj());
		}
	}

	return group;
}

GeometryModel3D^ Genetibase::NuGenPSurface::AvalonBridge::Model3D::ToAvalonObj(void)
{
	// convert geometry
	MeshGeometry3D^ geometry = gcnew MeshGeometry3D();

	// verts
	if (Geometry->Vertices != nullptr)
	{
		geometry->Positions = gcnew Point3DCollection(Geometry->Vertices->Length);
		for (int vertex = 0; vertex < Geometry->Vertices->Length; vertex++)
		{
			Vector3D^ position = Geometry->Vertices[vertex];
			geometry->Positions->Add(Point3D(position->X, position->Y, position->Z));
		}
	}
	// normals
	if (Geometry->Normals != nullptr)
	{
		geometry->Normals = gcnew Vector3DCollection(Geometry->Normals->Length);
		for (int nIdx = 0; nIdx < Geometry->Normals->Length; nIdx++)
		{
			Vector3D^ normal = Geometry->Normals[nIdx];
			geometry->Normals->Add(System::Windows::Media::Media3D::Vector3D(normal->X, normal->Y, normal->Z));
		}
	}
	// tex coords
	if (Geometry->TexCoords != nullptr)
	{
		geometry->TextureCoordinates = gcnew PointCollection(Geometry->TexCoords->Length);
		for (int tcIdx = 0; tcIdx < Geometry->TexCoords->Length; tcIdx++)
		{
			TexCoord2D^ texCoord = Geometry->TexCoords[tcIdx];
			geometry->TextureCoordinates->Add(Point(texCoord->U, texCoord->V));
		}
	}
	// triangle indices
	if (Geometry->PrimIndices != nullptr)
	{
		geometry->TriangleIndices = gcnew Int32Collection(Geometry->PrimIndices->Length);
		for (int tIdx = 0; tIdx < Geometry->PrimIndices->Length; tIdx++)
		{
			geometry->TriangleIndices->Add(Geometry->PrimIndices[tIdx]);
		}
	}

	// material
	System::Windows::Media::Media3D::Material^ material = nullptr;
	if (Material != nullptr)
	{
		material = gcnew DiffuseMaterial(gcnew SolidColorBrush(Color::FromArgb(Material->Diffuse.A, Material->Diffuse.R, Material->Diffuse.G, Material->Diffuse.B)));
	}

	return gcnew GeometryModel3D(geometry, material);
}