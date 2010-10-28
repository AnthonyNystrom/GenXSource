using System;
using System.Collections.Generic;
using System.Text;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Structs;

namespace Demo.Scenes
{
	public static class Bool
	{
		public static void CreateIntersection()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgTorus tor1 = msgTorus.Create(2,1 ,24,24);
			msgTorus tor2 = msgTorus.Create(2,0.3 ,24,24);
			msgVectorStruct transV1 = new msgVectorStruct(1, 1, 0);
			tor2.InitTempMatrix().Translate(transV1);
			tor2.ApplyTempMatrix();
			tor2.DestroyTempMatrix();
			scene.AttachObject(tor1);
			tor1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 5);
			scene.AttachObject(tor2);
			tor2.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 45);

			msgVectorStruct transV2 = new msgVectorStruct(0, 0, 1.5);

			msgGroup bool1 = msgBoolean.Intersection(tor1, tor2);

			int ChCnt = bool1.GetChildrenList().GetCount();

			msgObject[] allChilds = null;
			bool1.BreakGroup(ref allChilds);
			
			msgObject.DeleteObject(bool1);
			for (ushort i=0; i < ChCnt;i++)
			{
				allChilds[i].InitTempMatrix().Translate(transV2);
				allChilds[i].ApplyTempMatrix();
				allChilds[i].DestroyTempMatrix();
				scene.AttachObject(allChilds[i]);
				allChilds[i].SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, (ushort)(10+i));
			}

			msgBox bx1 = msgBox.Create(2, 2, 1);
			msgSphere sp1 = msgSphere.Create(1, 24, 24);
			msgVectorStruct transV4 = new msgVectorStruct(3, 3, 0);
			bx1.InitTempMatrix().Translate(transV4);
			bx1.ApplyTempMatrix();
			bx1.DestroyTempMatrix();
			scene.AttachObject(bx1);
			bx1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 55);
			msgVectorStruct transV5 = new msgVectorStruct(3, 4, 0);
			sp1.InitTempMatrix().Translate(transV5);
			sp1.ApplyTempMatrix();
			sp1.DestroyTempMatrix();
			scene.AttachObject(sp1);
			sp1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 75);

			msgGroup bool2 = msgBoolean.Intersection(sp1, bx1);

			ChCnt = bool2.GetChildrenList().GetCount();
			bool2.BreakGroup(ref allChilds);
						
			msgObject.DeleteObject(bool2);
			for (ushort i = 0; i < ChCnt; i++)
			{
				allChilds[i].InitTempMatrix().Translate(transV2);
				allChilds[i].ApplyTempMatrix();
				allChilds[i].DestroyTempMatrix();
				scene.AttachObject(allChilds[i]);
				allChilds[i].SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, (ushort)(10 + i));
			}
		}

		public static void CreateUnion()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgTorus tor1 = msgTorus.Create(2,1 ,24,24);
			msgTorus tor2 = msgTorus.Create(2,0.5 ,24,24);

			msgVectorStruct transV1 = new msgVectorStruct(1, 3.5, 0);
			tor2.InitTempMatrix().Translate(transV1);
			tor2.ApplyTempMatrix();
			tor2.DestroyTempMatrix();
			scene.AttachObject(tor1);
			tor1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 5);
			scene.AttachObject(tor2);
			tor2.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 45);

			msgVectorStruct transV2 = new msgVectorStruct(-6.5,0,0);

			msgGroup bool1 = msgBoolean.Union(tor1, tor2);

			bool1.InitTempMatrix().Translate(transV2);
			bool1.ApplyTempMatrix();
			bool1.DestroyTempMatrix();
			scene.AttachObject(bool1);
			bool1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 3);
			bool1.SetAttribute(msgObjectAttrEnum.SG_OA_DRAW_STATE, (ushort)SG_OA_DRAW_STATEValuesEnum.SGDS_FRAME);
		}

		public static void CreateSub()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(15.0, 14.0, -2.0, 2.0, -1.0);

			msgTorus tor1 = msgTorus.Create(2,1 ,24,24);
			msgTorus tor2 = msgTorus.Create(2,0.8 ,24,24);
			msgVectorStruct transV1 = new msgVectorStruct(1,1,0);
			tor2.InitTempMatrix().Translate(transV1);
			tor2.ApplyTempMatrix();
			tor2.DestroyTempMatrix();
			scene.AttachObject(tor1);
			tor1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR,5);
			scene.AttachObject(tor2);
			tor2.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR,25);

			msgVectorStruct transV2 = new msgVectorStruct(-6.0,0,0);

			msgGroup bool1 = msgBoolean.Sub(tor1, tor2);

			bool1.InitTempMatrix().Translate(transV2);
			bool1.ApplyTempMatrix();
			bool1.DestroyTempMatrix();
			scene.AttachObject(bool1);
			bool1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 3);
		}

		public static void CreateIntersectionContours()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();
 
			msgTorus tor1 = msgTorus.Create(2,1 ,36,36);
			msgTorus tor2 = msgTorus.Create(2,0.6 ,36,36);
			msgVectorStruct transV1 = new msgVectorStruct(1,1,0);
			tor2.InitTempMatrix().Translate(transV1);
			tor2.ApplyTempMatrix();
			tor2.DestroyTempMatrix();
			scene.AttachObject(tor1);
			tor1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR,65);
			scene.AttachObject(tor2);
			tor2.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR,105);

			msgVectorStruct transV2 = new msgVectorStruct(5,0,0.0);
			
			msgGroup bool1 = msgBoolean.IntersectionContour(tor1, tor2);

			bool1.InitTempMatrix().Translate(transV2);
			bool1.ApplyTempMatrix();
			bool1.DestroyTempMatrix();
			scene.AttachObject(bool1);
			bool1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 0);
			bool1.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);
		}

		public static void CreateSections()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgTorus tor1 = msgTorus.Create(2,1 ,36,36);
			scene.AttachObject(tor1);

			msgVectorStruct transV2 = new msgVectorStruct(-6,0,0);

			for (int i=-30;i<30;i+=4)
			{
				msgVectorStruct plN = new msgVectorStruct();
				plN.x =0.0; plN.y = 1.0; plN.z = 1.0;

				msgGroup bool1 = msgBoolean.Section(tor1, plN, 0.1*i );

				if (bool1 != null)
				{
					bool1.InitTempMatrix().Translate(transV2);
					bool1.ApplyTempMatrix();
					bool1.DestroyTempMatrix();
					scene.AttachObject(bool1);
					bool1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, (ushort)(i + 50));
					bool1.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);
				}
			}
		}
	}
}
