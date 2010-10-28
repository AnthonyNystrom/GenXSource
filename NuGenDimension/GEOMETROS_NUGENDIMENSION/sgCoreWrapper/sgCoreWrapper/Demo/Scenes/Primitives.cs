using System;
using System.Collections.Generic;
using System.Text;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Structs;

namespace Demo.Scenes
{
	public static class Primitives
	{
		public static void CreateBoxes()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(7.0, 7.0, 1.0, 1.0, -0.3);

			msgBox bx1 = msgBox.Create(1, 2, 2.1);

			scene.AttachObject(bx1);
			bx1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 8);

			msgBox bx2 = msgBox.Create(1, 1, 1);

			msgVectorStruct transV = new msgVectorStruct();
			transV.x = 1.5;
			transV.y = 0.0;
			transV.z = 0.0;
			bx2.InitTempMatrix().Translate(transV);
			bx2.ApplyTempMatrix();
			bx2.DestroyTempMatrix();

			bx2.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 0);
			bx2.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 1);
			bx2.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_TYPE, 1);

			scene.AttachObject(bx2);
		}

		public static void CreateSpheres()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(20.0, 10.0, 0.0, 1.0, -2.0);

			for (ushort i = 3; i < 5; i++)
			{
				msgSphere sp1 = msgSphere.Create(i, 24, 24);

				msgVectorStruct transV1 = new msgVectorStruct(5 * (i - 3), 2, 0);
				sp1.InitTempMatrix().Translate(transV1);
				sp1.ApplyTempMatrix();
				sp1.DestroyTempMatrix();

				scene.AttachObject(sp1);
				sp1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, i);
				if (i % 2 > 0)
				{
					sp1.SetAttribute(msgObjectAttrEnum.SG_OA_DRAW_STATE, (ushort)SG_OA_DRAW_STATEValuesEnum.SGDS_FRAME);
				}
			}
		}

		public static void CreateCylinders()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(80.0, 50.0, 20.0, 10.0, 5.0);

			for (ushort i = 2; i < 8; i++)
			{
				msgCylinder cy1 = msgCylinder.Create(i, 2 * i, 24);
				msgVectorStruct transV1 = new msgVectorStruct(5 * i, 10, 10);
				cy1.InitTempMatrix().Translate(transV1);
				cy1.ApplyTempMatrix();
				cy1.DestroyTempMatrix();

				scene.AttachObject(cy1);
				cy1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, i);

				if (i % 2 > 0)
				{
					cy1.SetAttribute(msgObjectAttrEnum.SG_OA_DRAW_STATE, (ushort)SG_OA_DRAW_STATEValuesEnum.SGDS_FRAME);
				}
			}
		}

		public static void CreateCones()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(200.0, 100.0, 50.0, 10.0, 5.0);

			for (ushort i = 2; i < 10; i++)
			{
				msgCone co1 = msgCone.Create(i, i / 3, 10 * i, 36);
				msgVectorStruct transV1 = new msgVectorStruct(10 * i, 10, 10);
				co1.InitTempMatrix().Translate(transV1);
				co1.ApplyTempMatrix();
				co1.DestroyTempMatrix();

				scene.AttachObject(co1);
				co1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, i);
			}
		}

		public static void CreateTors()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(100.0, 50.0, 50.0, 10.0, 2.0);
			Random rand = new Random();
			for (ushort i = 2; i < 10; i++)
			{
				msgTorus tor1 = msgTorus.Create(i, i / 6 + 2, 24, 24);
				msgPointStruct rotCen = new msgPointStruct(0.0, 0.0, 0.0);
				msgVectorStruct rotDir = new msgVectorStruct(rand.Next(), rand.Next(), rand.Next());
				tor1.InitTempMatrix().Rotate(rotCen, rotDir, rand.Next() * 360);
				msgVectorStruct transV1 = new msgVectorStruct(10 * i, 10, 10);
				tor1.GetTempMatrix().Translate(transV1);
				tor1.ApplyTempMatrix();
				tor1.DestroyTempMatrix();

				scene.AttachObject(tor1);
				tor1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, i);
			}
		}

		public static void CreateEllipsoids()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(100.0, 50.0, 50.0, 10.0, 2.0);
			Random rand = new Random();
			for (ushort i = 3; i < 10; i++)
			{
				msgEllipsoid ell1 = msgEllipsoid.Create(i, i / 6 + 2, 2 * i, 24, 24);
				msgPointStruct rotCen = new msgPointStruct(0.0, 0.0, 0.0);
				msgVectorStruct rotDir = new msgVectorStruct(rand.Next(), rand.Next(), rand.Next());
				ell1.InitTempMatrix().Rotate(rotCen, rotDir, rand.Next() * 360);
				msgVectorStruct transV1 = new msgVectorStruct(10 * i, 10, 10);
				ell1.GetTempMatrix().Translate(transV1);
				ell1.ApplyTempMatrix();
				ell1.DestroyTempMatrix();

				scene.AttachObject(ell1);
				ell1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, i);
			}
		}

		public static void CreateBands()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(40.0, 40.0, 15.0, 10.0, 0.0);

			for (ushort i = 2; i < 5; i++)
			{
				msgSphericBand sb1 = msgSphericBand.Create(i, -0.4, 0.5, 24);
				msgVectorStruct transV1 = new msgVectorStruct(5 * i, 10, 10);
				sb1.InitTempMatrix().Translate(transV1);
				sb1.ApplyTempMatrix();
				sb1.DestroyTempMatrix();

				scene.AttachObject(sb1);
				sb1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, i);
			}
		}
	}
}
