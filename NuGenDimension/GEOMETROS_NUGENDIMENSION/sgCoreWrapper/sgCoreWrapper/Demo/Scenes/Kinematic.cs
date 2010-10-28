using System;
using System.Collections.Generic;
using System.Text;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Structs;

namespace Demo.Scenes
{
	public static class Kinematic
	{
		public static void CreateRotationSurface()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(15.0, 14.0, -2.0, 2.0, -3.0);

			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl2 = msgSplineStruct.Create();

			tmpPnt.x = 1.0; tmpPnt.y = -3.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,0);
			tmpPnt.x = 3.0; tmpPnt.y = -2.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,1);
			tmpPnt.x = 2.0; tmpPnt.y = -1.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,2);
			tmpPnt.x = 3.0; tmpPnt.y = 1.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,3);
			tmpPnt.x = 2.0; tmpPnt.y = 4.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,4);
			tmpPnt.x =4.0; tmpPnt.y = 5.0; tmpPnt.z = 0.0;
			spl2.AddKnot(tmpPnt,5);

			msgSpline spl2_obj = msgSpline.Create(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR,12);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgSplineStruct.Delete(spl2);

			msgPointStruct p1 = new msgPointStruct(0,-2,0);
			msgPointStruct p2 = new msgPointStruct(0,0,0);

			msg3DObject rO = (msg3DObject)msgKinematic.Rotation(spl2_obj, p1, p2, 250, false);

			scene.AttachObject(rO);
			rO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 3);

			msgVectorStruct transV1 = new msgVectorStruct(-1, 0, 0);
			rO.InitTempMatrix().Translate(transV1);
			rO.ApplyTempMatrix();
			rO.DestroyTempMatrix();
		}

		public static void CreateRotationBody()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl2 = msgSplineStruct.Create();
			int fl=0;
			for (double i=0.0;i<2.0*Math.PI;i+=0.4)
			{
				tmpPnt.x = ((double)(fl%3+2))*Math.Cos(i);
				tmpPnt.y = ((double)(fl%3+2))*Math.Sin(i);
				tmpPnt.z = 0.0;
				spl2.AddKnot(tmpPnt,fl);
				fl++;
			}
			
			spl2.Close();

			msgSpline spl2_obj = msgSpline.Create(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR,12);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS,2);

			msgSplineStruct.Delete(spl2);

			msgPointStruct p1 = new msgPointStruct(10,10,0);
			msgPointStruct p2 = new msgPointStruct(20,15,0);

			msg3DObject rO = (msg3DObject)msgKinematic.Rotation(spl2_obj,p1,p2,290,true);

			scene.AttachObject(rO);
			rO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 8);

			msgVectorStruct transV1 = new msgVectorStruct( -6, 0, 0 );
			rO.InitTempMatrix().Translate(transV1);
			rO.ApplyTempMatrix();
			rO.DestroyTempMatrix();
		}

		public static void CreateExtrudeSurfaces()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl2 = msgSplineStruct.Create();

			int fl=0;
			for (double i=0.0;i<10.0;i+=1.0)
			{
				tmpPnt.x = i;
				tmpPnt.y = (fl%2 > 0)?-i/2.0:i/2.0;
				tmpPnt.z = 0.0;
				spl2.AddKnot(tmpPnt,fl);
				fl++;
			}
			
			msgSpline spl2_obj = msgSpline.Create(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgSplineStruct.Delete(spl2);

			msgVectorStruct extVec = new msgVectorStruct(1,-2,3);

			msg3DObject exO = (msg3DObject)msgKinematic.Extrude(spl2_obj, null, extVec,false);

			scene.AttachObject(exO);
			exO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 20);

			msgVectorStruct transV1 = new msgVectorStruct(8, 0, 0);
			exO.InitTempMatrix().Translate(transV1);
			exO.ApplyTempMatrix();
			exO.DestroyTempMatrix();
		}

		public static void CreateExtrudeBody()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			Utils.AddFloorInScene(30.0, 25.0, -2.0, 0.0, -2.0);
			
			msgPointStruct tmpPnt = new msgPointStruct();

			msgSplineStruct spl2 = msgSplineStruct.Create();
			int fl=0;
			for (double i=0.0;i<2.0*3.14159265;i+=0.13)
			{
				tmpPnt.x = ((double)(fl%3+2))*Math.Cos(i);
				tmpPnt.y = ((double)(fl%3+2))*Math.Sin(i);
				tmpPnt.z = 0.0;
				spl2.AddKnot(tmpPnt,fl);
				fl++;
			}
			spl2.Close();

			msgSpline spl2_obj = msgSpline.Create(spl2);
			msgSplineStruct.Delete(spl2);
			scene.AttachObject(spl2_obj);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			spl2_obj.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgCircleStruct cirGeo = new msgCircleStruct();
			msgPointStruct cirC = new msgPointStruct(0.0, 0.0, 0.0);
			msgVectorStruct cirNor = new msgVectorStruct(0.0, 0.0, 1.0);
			cirGeo.FromCenterRadiusNormal(cirC,1.6, cirNor);
			msg2DObject cir = msgCircle.Create(cirGeo);
			scene.AttachObject(cir);
			cir.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			cir.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgVectorStruct extVec = new msgVectorStruct(1,-2,5);

			msg3DObject exO = (msg3DObject)msgKinematic.Extrude(spl2_obj, new msg2DObject[] { cir }, extVec,true);

			scene.AttachObject(exO);
			exO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 30);

			msgVectorStruct transV1 = new msgVectorStruct( -7, 0, 0 );
			exO.InitTempMatrix().Translate(transV1);
			exO.ApplyTempMatrix();
			exO.DestroyTempMatrix();
		}

		public static void CreateSpiralSurface()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgArcStruct ArcGeo = new msgArcStruct();
			msgPointStruct ArP1 = new msgPointStruct(-1.0, -3.0, 0.0);
			msgPointStruct ArP2 = new msgPointStruct(-1.0, -2.0, 0.0);
			msgPointStruct ArP3 = new msgPointStruct(0.0, -3.5, 0.0);
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			msg2DObject ar = msgArc.Create(ArcGeo);
			scene.AttachObject(ar);
			ar.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			ar.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgPointStruct axeP1 = new msgPointStruct(2.0, -3.0, 0.0);
			msgPointStruct axeP2 = new msgPointStruct(2.0, 3.0, 0.0);

			msg3DObject spirO = (msg3DObject)msgKinematic.Spiral(ar, null, axeP1, axeP2, 4, 10, 15,false);

			scene.AttachObject(spirO);
			spirO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 85);

			msgVectorStruct transV1 = new msgVectorStruct(0, -11.5, 0);
			spirO.InitTempMatrix().Translate(transV1);
			spirO.ApplyTempMatrix();
			spirO.DestroyTempMatrix();
		}

		public static void CreateSpiralBody()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgCircleStruct cirGeo1 = new msgCircleStruct();
			msgPointStruct cirC1 = new msgPointStruct(2.0, -2.0, 0.0);
			msgVectorStruct cirNor1 = new msgVectorStruct(0.0, 0.0, 1.0);
			cirGeo1.FromCenterRadiusNormal(cirC1,3.0, cirNor1);
			msg2DObject cir1 = msgCircle.Create(cirGeo1);
			scene.AttachObject(cir1);
			cir1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			cir1.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgCircleStruct cirGeo2 = new msgCircleStruct();
			msgPointStruct cirC2 = new msgPointStruct(2.0, -2.3, 0.0);
			msgVectorStruct cirNor2 = new msgVectorStruct(0.0, 0.0, 1.0);
			cirGeo2.FromCenterRadiusNormal(cirC2,1.5, cirNor2);
			msg2DObject cir2 = msgCircle.Create(cirGeo2);
			scene.AttachObject(cir2);
			cir2.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			cir2.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgPointStruct axeP1 = new msgPointStruct(6.0, -3.0, 0.0);
			msgPointStruct axeP2 = new msgPointStruct(6.0, 3.0, 0.0);

			msg3DObject spirO = (msg3DObject)msgKinematic.Spiral(cir1, new msg2DObject[] { cir2 }, axeP1,axeP2,12,30,16,true);

			scene.AttachObject(spirO);
			spirO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 56);

			msgVectorStruct transV1 = new msgVectorStruct( 0, 7, 0 );
			spirO.InitTempMatrix().Translate(transV1);
			spirO.ApplyTempMatrix();
			spirO.DestroyTempMatrix();
		}

		public static void CreatePipeSurface()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgArcStruct ArcGeo = new msgArcStruct();
			msgPointStruct ArP1 = new msgPointStruct(1.0, -4.0, 0.0);
			msgPointStruct ArP2 = new msgPointStruct(1.0, -3.6, 0.0);
			msgPointStruct ArP3 = new msgPointStruct(1.2, -3.5, 0.0);
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			msg2DObject ar = msgArc.Create(ArcGeo);
			scene.AttachObject(ar);
			ar.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			ar.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgObject[] objcts = new msgObject[6];

			objcts[0] = msgLine.Create(0.0, -4.0, 0.0, 0.0, -2.0, 0.0);

			ArP1.x = 0.0; ArP1.y = -2.0; ArP1.z = 0.0;
			ArP2.x = 1.0; ArP2.y = -1.0; ArP2.z = 0.0;
			ArP3.x = 0.4; ArP3.y = -1.2; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[1] = msgArc.Create(ArcGeo);

			ArP1.x = 1.0; ArP1.y = -1.0; ArP1.z = 0.0;
			ArP2.x = 2.0; ArP2.y = 0.0; ArP2.z = 0.0;
			ArP3.x = 1.9; ArP3.y = -0.5; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[2] = msgArc.Create(ArcGeo);

			ArP1.x = 2.0; ArP1.y = 0.0; ArP1.z = 0.0;
			ArP2.x = 1.0; ArP2.y = 1.0; ArP2.z = 0.0;
			ArP3.x = 1.6; ArP3.y = 0.8; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[3] = msgArc.Create(ArcGeo);

			objcts[4] = msgLine.Create(1.0, 1.0, 0.0, -1.0, 1.0, 0.0);

			ArP1.x = -1.0; ArP1.y = 1.0; ArP1.z = 0.0;
			ArP2.x = -1.0; ArP2.y = 0.0; ArP2.z = 1.0;
			ArP3.x = -1.1; ArP3.y = 1.0; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[5] = msgArc.Create(ArcGeo);

			msgContour cnt1 = msgContour.CreateContour(objcts);
			scene.AttachObject(cnt1);
			cnt1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			cnt1.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgPointStruct point_in_plane = new msgPointStruct(1.0, -4.0, 0.0);

			bool close = false;
			msg3DObject pipO = (msg3DObject)msgKinematic.Pipe(ar,null, cnt1, point_in_plane, 0.0, ref close);

			scene.AttachObject(pipO);
			pipO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 25);

			msgVectorStruct transV1 = new msgVectorStruct(2.5, 1, 0);
			pipO.InitTempMatrix().Translate(transV1);
			pipO.ApplyTempMatrix();
			pipO.DestroyTempMatrix();
		}

		public static void CreatePipeBody()
		{
			msgScene scene = msgScene.GetScene();
			scene.Clear();

			msgObject[] objcts = new msgObject[5];

			msgPointStruct ArP1 = new msgPointStruct();
			msgPointStruct ArP2 = new msgPointStruct();
			msgPointStruct ArP3 = new msgPointStruct();
			msgArcStruct ArcGeo = new msgArcStruct();

			ArP1.x = 0.0; ArP1.y = -2.0; ArP1.z = 0.0;
			ArP2.x = 1.0; ArP2.y = -1.0; ArP2.z = 0.0;
			ArP3.x = 0.4; ArP3.y = -1.2; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[0] = msgArc.Create(ArcGeo);

			ArP1.x = 1.0; ArP1.y = -1.0; ArP1.z = 0.0;
			ArP2.x = 2.0; ArP2.y = 0.0; ArP2.z = 0.0;
			ArP3.x = 1.9; ArP3.y = -0.5; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[1] = msgArc.Create(ArcGeo);

			ArP1.x = 2.0; ArP1.y = 0.0; ArP1.z = 0.0;
			ArP2.x = 1.0; ArP2.y = 1.0; ArP2.z = 0.0;
			ArP3.x = 1.6; ArP3.y = 0.8; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[2] = msgArc.Create(ArcGeo);

			objcts[3] = msgLine.Create(1.0, 1.0, 0.0, -1.0, 1.0, 0.0);

			ArP1.x = -1.0; ArP1.y = 1.0; ArP1.z = 0.0;
			ArP2.x = -1.0; ArP2.y = 0.0; ArP2.z = 1.0;
			ArP3.x = -1.1; ArP3.y = 1.0; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[4] = msgArc.Create(ArcGeo);

			msgContour cnt1 = msgContour.CreateContour(objcts);
			scene.AttachObject(cnt1);
			cnt1.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			cnt1.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			ArP1.x = -0.2; ArP1.y = -0.2; ArP1.z = 0.0;
			ArP2.x = -0.1; ArP2.y = 0.2; ArP2.z = 0.0;
			ArP3.x = -0.3; ArP3.y = 0.1; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[0] = msgArc.Create(ArcGeo);


			ArP1.x = -0.1; ArP1.y = 0.2; ArP1.z = 0.0;
			ArP2.x = 0.3; ArP2.y = 0.5; ArP2.z = 0.0;
			ArP3.x = 0.2; ArP3.y = 0.6; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[1] = msgArc.Create(ArcGeo);

			ArP1.x = 0.3; ArP1.y = 0.5; ArP1.z = 0.0;
			ArP2.x = -0.2; ArP2.y = -0.2; ArP2.z = 0.0;
			ArP3.x = 0.6; ArP3.y = -0.4; ArP3.z = 0.0;
			ArcGeo.FromThreePoints(ArP1,ArP2,ArP3,false);
			objcts[2] = msgArc.Create(ArcGeo);

			msgObject[] objcts2 = null;
			Array.Copy(objcts, objcts2, 3);
			msgContour cnt2 = msgContour.CreateContour(objcts2);
			scene.AttachObject(cnt2);
			cnt2.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			cnt2.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgCircleStruct cirGeo = new msgCircleStruct();
			msgPointStruct cirC = new msgPointStruct(0.3, -0.1, 0.0);
			msgVectorStruct cirNor = new msgVectorStruct(0.0, 0.0, 1.0);
			cirGeo.FromCenterRadiusNormal(cirC,0.31, cirNor);
			msg2DObject cir = msgCircle.Create(cirGeo);
			scene.AttachObject(cir);
			cir.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 12);
			cir.SetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS, 2);

			msgPointStruct point_in_plane = new msgPointStruct(0.0,0.0,0.0);

			bool  close = true;
			msg3DObject pipO = (msg3DObject)msgKinematic.Pipe(cnt2, new msg2DObject[] { cir }, cnt1, point_in_plane, 0.0, ref close);

			scene.AttachObject(pipO);
			pipO.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 25);

			msgVectorStruct transV1 = new msgVectorStruct(3.0, 1.0, 0);
			pipO.InitTempMatrix().Translate(transV1);
			pipO.ApplyTempMatrix();
			pipO.DestroyTempMatrix();
		}
	}
}
