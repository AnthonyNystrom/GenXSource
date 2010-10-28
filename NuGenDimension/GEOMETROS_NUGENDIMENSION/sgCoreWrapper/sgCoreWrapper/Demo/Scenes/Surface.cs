using System;
using System.Collections.Generic;
using System.Text;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Structs;

namespace Demo.Scenes
{
	public static class Surface
	{
		public static void CreateMesh()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			short x_sz = 20;
			short y_sz = 20;
			msgPointStruct[] pnts = new msgPointStruct[x_sz*y_sz];

			for (short i = 0; i < x_sz; i++)
			{
				for (short j = 0; j < y_sz; j++)
				{
					pnts[i * y_sz + j] = new msgPointStruct();
					pnts[i*y_sz+j].x = i-x_sz/2;
					pnts[i*y_sz+j].y = j-y_sz/2;
					pnts[i*y_sz+j].z = 0.1*(pnts[i*y_sz+j].x*pnts[i*y_sz+j].x +
						pnts[i*y_sz+j].y*pnts[i*y_sz+j].y);
				}
			}

			msgObject msh = msgSurfaces.Mesh(x_sz,y_sz,pnts);

			scene.AttachObject(msh);
			msh.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 30);
			msh.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 1);
		}

		public static void CreateFace()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl2 = msgSplineStruct.Create();

			tmpPnt.x = -1.0; tmpPnt.y = -3.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,0);
			tmpPnt.x = -3.0; tmpPnt.y = 0.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,1);
			tmpPnt.x = -1.0; tmpPnt.y = -1.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,2);
			tmpPnt.x = 0.0; tmpPnt.y = 1.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,3);
			tmpPnt.x = -1.0; tmpPnt.y = 4.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,4);
			tmpPnt.x =3.0; tmpPnt.y = 1.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,5);
			tmpPnt.x =2.0; tmpPnt.y = -3.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,6);
			tmpPnt.x =1.0; tmpPnt.y = -1.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,7);
			tmpPnt.x =1.0; tmpPnt.y = -4.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,8);
			spl2.Close();

			msgSpline spl2_obj = msgSpline.Create(spl2);
			msgSplineStruct.Delete(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 0);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 1);

			msg2DObject[] holes = new msg2DObject[3];

			msgCircleStruct cirGeo = new msgCircleStruct();
			msgPointStruct cirC = new msgPointStruct(0.8, 1.0, 0.0);
			msgVectorStruct cirNor = new msgVectorStruct(0.0, 0.0, 1.0);
			cirGeo.FromCenterRadiusNormal(cirC,0.8, cirNor);
			holes[0] = msgCircle.Create(cirGeo);
			scene.AttachObject(holes[0]);
			holes[0].SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 0);
			holes[0].SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 1);

			cirC.x = 1.6; cirC.y = -1.0;
			cirGeo.FromCenterRadiusNormal(cirC,0.2, cirNor);
			holes[1] = msgCircle.Create(cirGeo);
			scene.AttachObject(holes[1]);
			holes[1].SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 0);
			holes[1].SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 1);


			cirC.x = 0.0; cirC.y = -1.0;
			cirGeo.FromCenterRadiusNormal(cirC,0.4, cirNor);
			holes[2] = msgCircle.Create(cirGeo);
			scene.AttachObject(holes[2]);
			holes[2].SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 0);
			holes[2].SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 1);

			msg3DObject fcO = (msg3DObject)msgSurfaces.Face(spl2_obj,holes);

			scene.AttachObject(fcO);
			fcO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 50);

			msgVectorStruct transV1 = new msgVectorStruct(-5,0,0);
			fcO.InitTempMatrix().Translate(transV1);
			fcO.ApplyTempMatrix();
			fcO.DestroyTempMatrix();
		}

		public static void CreateCoonsFrom3Curves()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl1 = msgSplineStruct.Create();

			tmpPnt.x = -6.0; tmpPnt.y = 0.0; tmpPnt.z = -1.0;
			spl1.AddKnot(tmpPnt,0);
			tmpPnt.x = -5.0; tmpPnt.y = 0.0; tmpPnt.z = -2.0;
			spl1.AddKnot(tmpPnt,1);
			tmpPnt.x = -3.0; tmpPnt.y = 0.0; tmpPnt.z = -1.0;
			spl1.AddKnot(tmpPnt,2);
			tmpPnt.x = -2.0; tmpPnt.y = 0.0; tmpPnt.z = -2.0;
			spl1.AddKnot(tmpPnt,3);
			tmpPnt.x = -1.0; tmpPnt.y = 0.0; tmpPnt.z = 1.0;
			spl1.AddKnot(tmpPnt,4);
			tmpPnt.x =2.0; tmpPnt.y = 0.0; tmpPnt.z = 1.0;
			spl1.AddKnot(tmpPnt,5);
			tmpPnt.x =2.0; tmpPnt.y = 0.0; tmpPnt.z = 0.0;
			spl1.AddKnot(tmpPnt,6);
			tmpPnt.x =4.0; tmpPnt.y = 0.0; tmpPnt.z = 1.0;
			spl1.AddKnot(tmpPnt,7);

			msgSpline spl1_obj = msgSpline.Create(spl1);
			msgSplineStruct.Delete(spl1);
			scene.AttachObject(spl1_obj);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 0);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 1);

			msgSplineStruct spl2 = msgSplineStruct.Create();

			tmpPnt.x = -6.0; tmpPnt.y = 0.0; tmpPnt.z = -1.0;
			spl2.AddKnot(tmpPnt,0);
			tmpPnt.x = -6.0; tmpPnt.y = 0.5; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,1);
			tmpPnt.x = -6.0; tmpPnt.y = 2.0; tmpPnt.z = -1.0;
			spl2.AddKnot(tmpPnt,2);
			tmpPnt.x = -6.0; tmpPnt.y = 3.0; tmpPnt.z = 1.0;
			spl2.AddKnot(tmpPnt,3);
			tmpPnt.x =-4.0; tmpPnt.y = 4.0; tmpPnt.z = 1.0;
			spl2.AddKnot(tmpPnt,4);

			msgSpline spl2_obj = msgSpline.Create(spl2);
			msgSplineStruct.Delete(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 50);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 3);

			msgPointStruct ArP1 = new msgPointStruct();
			msgPointStruct ArP2 = new msgPointStruct();
			msgPointStruct ArP3 = new msgPointStruct();
			msgArcStruct ArcGeo = new msgArcStruct();
			ArP1.x = -4.0; ArP1.y = 4.0; ArP1.z = 1.0;
			ArP2.x = 4.0; ArP2.y = 0.0; ArP2.z = 1.0;
			ArP3.x = 0.0; ArP3.y = 5.0; ArP3.z = -4.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			msgArc arcObj = msgArc.Create(ArcGeo);

			scene.AttachObject(arcObj);
			arcObj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 20);
			arcObj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 3);

			msg3DObject coons = (msg3DObject)msgSurfaces.Coons(spl1_obj, spl2_obj, arcObj, null, 36, 36, 4, 4);

			scene.AttachObject(coons);
			coons.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 4);

			msgVectorStruct transV1 = new msgVectorStruct(0,0,-0.4);
			coons.InitTempMatrix().Translate(transV1);
			coons.ApplyTempMatrix();
			coons.DestroyTempMatrix();
		}

		public static void CreateCoonsFrom4Curves()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl1 = msgSplineStruct.Create();
			tmpPnt.x = -6.0; tmpPnt.y = 0.0; tmpPnt.z = -1.0;
			spl1.AddKnot(tmpPnt,0);
			tmpPnt.x = -5.0; tmpPnt.y = 0.0; tmpPnt.z = 2.0;
			spl1.AddKnot(tmpPnt,1);
			tmpPnt.x = -3.0; tmpPnt.y = 0.0; tmpPnt.z = -2.0;
			spl1.AddKnot(tmpPnt,2);
			tmpPnt.x = -2.0; tmpPnt.y = 0.0; tmpPnt.z = -2.0;
			spl1.AddKnot(tmpPnt,3);
			tmpPnt.x = -1.0; tmpPnt.y = 0.0; tmpPnt.z = 1.0;
			spl1.AddKnot(tmpPnt,4);
			tmpPnt.x =2.0; tmpPnt.y = 0.0; tmpPnt.z = 1.0;
			spl1.AddKnot(tmpPnt,5);
			tmpPnt.x =2.0; tmpPnt.y = 0.0; tmpPnt.z = 0.0;
			spl1.AddKnot(tmpPnt,6);
			tmpPnt.x =4.0; tmpPnt.y = 0.0; tmpPnt.z = 1.0;
			spl1.AddKnot(tmpPnt,7);

			msgSpline spl1_obj = msgSpline.Create(spl1);
			msgSplineStruct.Delete(spl1);
			scene.AttachObject(spl1_obj);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 0);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 1);

			msgSplineStruct spl2 = msgSplineStruct.Create();

			tmpPnt.x = -5.0; tmpPnt.y = 4.0; tmpPnt.z = 2.0;
			spl2.AddKnot(tmpPnt,0);
			tmpPnt.x = -3.0; tmpPnt.y = 4.0; tmpPnt.z = 1.0;
			spl2.AddKnot(tmpPnt,1);
			tmpPnt.x = -2.0; tmpPnt.y = 4.0; tmpPnt.z = 2.0;
			spl2.AddKnot(tmpPnt,2);
			tmpPnt.x = -1.0; tmpPnt.y = 4.0; tmpPnt.z = -1.0;
			spl2.AddKnot(tmpPnt,3);
			tmpPnt.x =4.0; tmpPnt.y = 4.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,4);

			msgSpline spl2_obj = msgSpline.Create(spl2);
			msgSplineStruct.Delete(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 10);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgSplineStruct spl3 = msgSplineStruct.Create();

			tmpPnt.x = -6.0; tmpPnt.y = 0.0; tmpPnt.z = -1.0;
			spl3.AddKnot(tmpPnt,0);
			tmpPnt.x = -7.0; tmpPnt.y = 0.5; tmpPnt.z = 3.0;
			spl3.AddKnot(tmpPnt,1);
			tmpPnt.x = -4.0; tmpPnt.y = 2.0; tmpPnt.z = -1.0;
			spl3.AddKnot(tmpPnt,2);
			tmpPnt.x = -5.0; tmpPnt.y = 3.0; tmpPnt.z = 1.0;
			spl3.AddKnot(tmpPnt,3);
			tmpPnt.x =-5.0; tmpPnt.y = 4.0; tmpPnt.z = 2.0;
			spl3.AddKnot(tmpPnt,4);

			msgSpline spl3_obj = msgSpline.Create(spl3);
			msgSplineStruct.Delete(spl3);
			scene.AttachObject(spl3_obj);
			spl3_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 50);
			spl3_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 3);

			msgSplineStruct spl4 = msgSplineStruct.Create();

			tmpPnt.x = 4.0; tmpPnt.y = 0.0; tmpPnt.z = 1.0;
			spl4.AddKnot(tmpPnt,0);
			tmpPnt.x = 3.0; tmpPnt.y = 1.0; tmpPnt.z = 2.0;
			spl4.AddKnot(tmpPnt,1);
			tmpPnt.x = 5.0; tmpPnt.y = 2.0; tmpPnt.z = -1.0;
			spl4.AddKnot(tmpPnt,2);
			tmpPnt.x = 3.0; tmpPnt.y = 3.0; tmpPnt.z = 0.5;
			spl4.AddKnot(tmpPnt,3);
			tmpPnt.x =4.0; tmpPnt.y = 4.0; tmpPnt.z = 0.0;
			spl4.AddKnot(tmpPnt,4);

			msgSpline spl4_obj = msgSpline.Create(spl4);
			msgSplineStruct.Delete(spl4);
			scene.AttachObject(spl4_obj);
			spl4_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 100);
			spl4_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 4);

			msg3DObject coons = (msg3DObject)msgSurfaces.Coons(spl1_obj,
																spl2_obj,
																spl3_obj,
																spl4_obj, 36, 36, 4, 4);

			scene.AttachObject(coons);
			coons.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 4);

			msgVectorStruct transV1 = new msgVectorStruct(0,0,-0.3);
			coons.InitTempMatrix().Translate(transV1);
			coons.ApplyTempMatrix();
			coons.DestroyTempMatrix();
		}

		public static void CreateLinear()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(30.0, 30.0, 2.0, 5.0, -2.0);
			
			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl1 = msgSplineStruct.Create();
			int fl=0;
			for (double i=0.0;i<2.0*3.14159265;i+=0.4)
			{
				tmpPnt.x = ((double)(fl%3+2))*Math.Cos(i);
				tmpPnt.y = ((double)(fl%3+2))*Math.Sin(i);
				tmpPnt.z = 0.0;
				spl1.AddKnot(tmpPnt,fl);
				fl++;
			}
			spl1.Close();

			msgSpline spl1_obj = msgSpline.Create(spl1);
			scene.AttachObject(spl1_obj);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgSplineStruct.Delete(spl1);

			msgCircleStruct cirGeo = new msgCircleStruct();
			cirGeo.center.x = 2.0; cirGeo.center.y = -2.0; cirGeo.center.z = 8.0;
			cirGeo.normal.x = 0.0; cirGeo.normal.y = 0.0; cirGeo.normal.z = 1.0;
			cirGeo.radius = 1.5;
			msgCircle cir = msgCircle.Create(cirGeo);
			scene.AttachObject(cir);
			cir.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			cir.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msg3DObject linO1 = (msg3DObject)msgSurfaces.LinearSurfaceFromSections(spl1_obj,
				cir, 0.5f ,false);

			scene.AttachObject(linO1);
			linO1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 90);

			msgVectorStruct transV1 = new msgVectorStruct(0,7,0);
			linO1.InitTempMatrix().Translate(transV1);
			linO1.ApplyTempMatrix();
			linO1.DestroyTempMatrix();


			msg3DObject linO2 = (msg3DObject)msgSurfaces.LinearSurfaceFromSections(spl1_obj,
				cir, 0.7f, true);

			scene.AttachObject(linO2);
			linO2.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 150);

			transV1.x = 8.0; transV1.y = 0.0;
			linO2.InitTempMatrix().Translate(transV1);
			linO2.ApplyTempMatrix();
			linO2.DestroyTempMatrix();


			msg3DObject linO3 = (msg3DObject)msgSurfaces.LinearSurfaceFromSections(spl1_obj,
				cir, 0.3f, false);

			scene.AttachObject(linO3);
			linO3.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 30);

			transV1.x = -8.0; transV1.y = 0.0;
			linO3.InitTempMatrix().Translate(transV1);
			linO3.ApplyTempMatrix();
			linO3.DestroyTempMatrix();
		}

		public static void CreateFromClips()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl1 = msgSplineStruct.Create();

			tmpPnt.x = 98.0; tmpPnt.y = 0.0; tmpPnt.z = -13.0;
			spl1.AddKnot(tmpPnt,0);
			tmpPnt.x = 85.0; tmpPnt.y = 0.0; tmpPnt.z = 19.0;
			spl1.AddKnot(tmpPnt,1);
			tmpPnt.x = 43.0; tmpPnt.y = 0.0; tmpPnt.z = -31.0;
			spl1.AddKnot(tmpPnt,2);
			tmpPnt.x = 5.0; tmpPnt.y = 0.0; tmpPnt.z = -3.0;
			spl1.AddKnot(tmpPnt,3);
			tmpPnt.x = -11.0; tmpPnt.y = 0.0; tmpPnt.z = -39.0;
			spl1.AddKnot(tmpPnt,4);
			tmpPnt.x =-48.0; tmpPnt.y = 0.0; tmpPnt.z = 23.0;
			spl1.AddKnot(tmpPnt,5);
			tmpPnt.x =-125.0; tmpPnt.y = 0.0; tmpPnt.z = 23.0;
			spl1.AddKnot(tmpPnt,6);

			msgSpline spl1_obj = msgSpline.Create(spl1);
			msgSplineStruct.Delete(spl1);
			scene.AttachObject(spl1_obj);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgSplineStruct spl2 = msgSplineStruct.Create();

			tmpPnt.x = 96.0; tmpPnt.y = 150.0; tmpPnt.z = 8.0;
			spl2.AddKnot(tmpPnt,0);
			tmpPnt.x = 66.0; tmpPnt.y = 150.0; tmpPnt.z = -20.0;
			spl2.AddKnot(tmpPnt,1);
			tmpPnt.x = 12.0; tmpPnt.y = 150.0; tmpPnt.z = 37.0;
			spl2.AddKnot(tmpPnt,2);
			tmpPnt.x = -128.0; tmpPnt.y = 150.0; tmpPnt.z = -23.0;
			spl2.AddKnot(tmpPnt,3);

			msgSpline spl2_obj = msgSpline.Create(spl2);
			msgSplineStruct.Delete(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgLine ln_obj = msgLine.Create(100.0,100.0,50.0, -121.0,100.0,-50.0);
			scene.AttachObject(ln_obj);
			ln_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			ln_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgArcStruct arcG = new msgArcStruct();
			msgPointStruct arcBeg = new msgPointStruct(98.0, 50.0 , -80.0);
			msgPointStruct arcEnd = new msgPointStruct(-117.0, 50.0 , -80.0);
			msgPointStruct arcMid = new msgPointStruct(-55.0, 50.0 , -50.0);
			arcG.FromThreePoints(arcBeg,arcEnd,arcMid,false);
			msgArc arc_obj = msgArc.Create(arcG);
			scene.AttachObject(arc_obj);
			arc_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			arc_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msg2DObject[] objcts = new msg2DObject[4];
			objcts[0] = spl1_obj;
			objcts[1] = arc_obj;
			objcts[2] = ln_obj;
			objcts[3] = spl2_obj;

			double[] param = new double[4];
			param[0] = param[1] = param[2] = param[3] = 0.0;

			msgObject surf = msgSurfaces.SplineSurfaceFromSections(objcts, param, false);

			scene.AttachObject(surf);
			surf.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 24);

			msgVectorStruct transV1 = new msgVectorStruct(0,0,-5);
			surf.InitTempMatrix().Translate(transV1);
			surf.ApplyTempMatrix();
			surf.DestroyTempMatrix();
		}

		public static void CreateBodyFromClips()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(20.0, 20.0, 5.0, 2.0, -4.0);

			msgPointStruct tmpPnt = new msgPointStruct();
			msgSplineStruct spl1 = msgSplineStruct.Create();
			int fl=0;
			for (double i=0.0;i<2.0*3.14159265;i+=0.4)
			{
				tmpPnt.x = ((double)(fl%3+2))*Math.Cos(i);
				tmpPnt.y = ((double)(fl%3+2))*Math.Sin(i);
				tmpPnt.z = 0.0;
				spl1.AddKnot(tmpPnt,fl);
				fl++;
			}
			spl1.Close();

			msgSpline spl1_obj = msgSpline.Create(spl1);
			msgSplineStruct.Delete(spl1);
			scene.AttachObject(spl1_obj);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			spl1_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgSplineStruct spl2 = msgSplineStruct.Create();

			tmpPnt.x = 0.0; tmpPnt.y = -0.9; tmpPnt.z = 2.0;
			spl2.AddKnot(tmpPnt,0);
			tmpPnt.x = 1.4; tmpPnt.y = 0.9; tmpPnt.z = 2.0;
			spl2.AddKnot(tmpPnt,1);
			tmpPnt.x = -1.6; tmpPnt.y = 0.6; tmpPnt.z = 2.0;
			spl2.AddKnot(tmpPnt,2);
			tmpPnt.x = -1.2; tmpPnt.y = -1.6; tmpPnt.z = 2.0;
			spl2.AddKnot(tmpPnt,3);
			spl2.Close();

			msgSpline spl2_obj = msgSpline.Create(spl2);
			msgSplineStruct.Delete(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			scene.AttachObject(spl1_obj);
			scene.AttachObject(spl2_obj);

			msg2DObject[] ooo = new msg2DObject[3];
			ooo[1] = spl1_obj;
			ooo[0] = spl2_obj;
			ooo[2] = (msg2DObject)spl2_obj.Clone();

			scene.AttachObject(ooo[2]);

			msgPointStruct axeP = new msgPointStruct(0.0, 0.0, 0.0);
			msgVectorStruct axeD = new msgVectorStruct(0.0, 0.0, 1.0);
			axeD.z = 0.0; axeD.x = 1.0;
			ooo[2].InitTempMatrix().Rotate(axeP,axeD, Math.PI/4.0);
			msgVectorStruct trV = new msgVectorStruct(-1.0, 1.0, -3.0);
			ooo[2].GetTempMatrix().Translate(trV);
			ooo[2].ApplyTempMatrix();
			ooo[2].DestroyTempMatrix();

			double[] ppp = new double[3];
			ppp[0] = 0.1;
			ppp[1] = 0.0;
			ppp[2] = 0.2;


			msgObject lsf = msgSurfaces.SplineSurfaceFromSections(ooo, ppp, true);

			scene.AttachObject(lsf);
			lsf.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 23);

			msgVectorStruct transV1 = new msgVectorStruct(8,0,0);
			lsf.InitTempMatrix().Translate(transV1);
			lsf.ApplyTempMatrix();
			lsf.DestroyTempMatrix();
		}
	}
}
