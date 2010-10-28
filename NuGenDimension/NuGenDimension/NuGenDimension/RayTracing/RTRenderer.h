#ifndef __RT_RENDERER__
#define __RT_RENDERER__

#include <math.h>
#include <float.h>

#include <vector>
#include "..//NuGenDimension.h"
#include "..//Dialogs//DlgRender.h"

#define MAX_COORD(x,y,z)   ((x > y) ? ((x > z) ? 0 : 2) : ((y > z) ? 1 : 2))

typedef double UV_COORD[2];

#define DEPTH_MIN 1.0e-6
#define DEPTH_MAX 1.0e7
#define EPSILON 0.00001f

#define X_PLUS 1
#define X_MINUS 2
#define Y_PLUS 3
#define Y_MINUS 4
#define Z_PLUS 5
#define Z_MINUS 6

SG_POINT   RT_TO_SG(RT_POINT rtP);
RT_POINT   SG_TO_RT(SG_POINT sgP);

class  MyRTObject : public rtIObject
{
private:
	sgC3DObject*   m_objct;

	bool    MyIntersectTriangle(SG_POINT rayInitial, SG_VECTOR rayDirection, 
		SG_VECTOR P_1, SG_VECTOR P_2, SG_VECTOR P_3, double *Depth)
	{
		SG_VECTOR V1, V2, Temp, trNormal;

		V1 = sgSpaceMath::VectorsSub(P_1, P_2);
		V2 = sgSpaceMath::VectorsSub(P_3, P_2);

		trNormal = sgSpaceMath::VectorsVectorMult(V1, V2);

		if (!sgSpaceMath::NormalVector(trNormal))
			return false;

		int trAxe =  MAX_COORD(fabs(trNormal.x), fabs(trNormal.y), fabs(trNormal.z));
		bool swap = false;

		switch (trAxe)
		{
		case 0:
			if ((P_2.y - P_3.y)*(P_2.z - P_1.z) < (P_2.z - P_3.z)*(P_2.y - P_1.y))
				swap = true;
			break;
		case 1:
			if ((P_2.x - P_3.x)*(P_2.z - P_1.z) < (P_2.z - P_3.z)*(P_2.x - P_1.x))
				swap = true;
			break;
		case 2:
			if ((P_2.x - P_3.x)*(P_2.y - P_1.y) < (P_2.y - P_3.y)*(P_2.x - P_1.x))
				swap = true;
			break;
		}

		if (swap)
		{
			Temp = P_2;
			P_2 = P_1;
			P_1 = Temp;
		}

		double vOr, vDir;
		double s, t;
		double Distance;

		vDir = sgSpaceMath::VectorsScalarMult(trNormal, rayDirection);

		if (fabs(vDir) < EPSILON)
			return false;

		vOr = sgSpaceMath::VectorsScalarMult(trNormal, rayInitial);
		Distance = -sgSpaceMath::VectorsScalarMult(trNormal, P_1);


		*Depth = -(Distance + vOr) / vDir;

		if ((*Depth < DEPTH_MIN) || (*Depth > DEPTH_MAX))
			return false;

		switch (trAxe)
		{
		case 0:
			s = rayInitial.y + *Depth * rayDirection.y;
			t = rayInitial.z + *Depth * rayDirection.z;
			if ((P_2.y - s) * (P_2.z - P_1.z) < (P_2.z - t) * (P_2.y - P_1.y))
				return false;
			if ((P_3.y - s) * (P_3.z - P_2.z) < (P_3.z - t) * (P_3.y - P_2.y))
				return false;
			if ((P_1.y - s) * (P_1.z - P_3.z) < (P_1.z - t) * (P_1.y - P_3.y))
				return false;
			return true;
		case 1:
			s = rayInitial.x + *Depth * rayDirection.x;
			t = rayInitial.z + *Depth * rayDirection.z;
			if ((P_2.x - s) * (P_2.z - P_1.z) < (P_2.z - t) * (P_2.x - P_1.x))
				return false;
			if ((P_3.x - s) * (P_3.z - P_2.z) < (P_3.z - t) * (P_3.x - P_2.x))
				return false;
			if ((P_1.x - s) * (P_1.z - P_3.z) < (P_1.z - t) * (P_1.x - P_3.x))
				return false;
			return true;
		case 2:
			s = rayInitial.x + *Depth * rayDirection.x;
			t = rayInitial.y + *Depth * rayDirection.y;
			if ((P_2.x - s) * (P_2.y - P_1.y) < (P_2.y - t) * (P_2.x - P_1.x))
				return false;
			if ((P_3.x - s) * (P_3.y - P_2.y) < (P_3.y - t) * (P_3.x - P_2.x))
				return false;
			if ((P_1.x - s) * (P_1.y - P_3.y) < (P_1.y - t) * (P_1.x - P_3.x))
				return false;
			return true;
		}
		return false;
	}

	bool MyIntersect_Box(SG_POINT RayInitial, SG_VECTOR RayDir, SG_POINT Corner1, SG_POINT Corner2)
	{
		int smin = 0, smax = 0;   
		double t, tmin, tmax;
		SG_VECTOR P, D;

		P = RayInitial;
		D = RayDir;

		tmin = 0.0;
		tmax = FLT_MAX;

		if (D.x < -EPSILON)
		{
			t = (Corner1.x - P.x) / D.x;
			if (t < tmin) 
				return false;
			if (t <= tmax)
			{
				smax = X_PLUS;
				tmax = t;
			}
			t = (Corner2.x - P.x) / D.x;
			if (t >= tmin)
			{
				if (t > tmax) 
					return false;
				smin = X_MINUS;
				tmin = t;
			}
		}
		else
		{
			if (D.x > EPSILON)
			{
				t = (Corner2.x - P.x) / D.x;
				if (t < tmin) 
					return false;
				if (t <= tmax)
				{
					smax = X_MINUS;
					tmax = t;
				}
				t = (Corner1.x - P.x) / D.x;
				if (t >= tmin)
				{
					if (t > tmax)   
						return false;
					smin = X_PLUS;
					tmin = t;
				}
			}
			else
			{
				if ((P.x < Corner1.x) || (P.x > Corner2.x))
				{
					return false;
				}
			}
		}

		if (D.y < -EPSILON)
		{
			t = (Corner1.y - P.y) / D.y;
			if (t < tmin) 
				return false;
			if (t <= tmax - DEPTH_MIN)
			{
				smax = Y_PLUS;
				tmax = t;
			}
			else
			{
				if (t <= tmax + DEPTH_MIN)
				{
					if (-D.y > fabs(D.x)) smax = Y_PLUS;
				}
			}
			t = (Corner2.y - P.y) / D.y;
			if (t >= tmin + DEPTH_MIN)
			{
				if (t > tmax) return false;
				smin = Y_MINUS;
				tmin = t;
			}
			else
			{
				if (t >= tmin - DEPTH_MIN)
				{
					if (-D.y > fabs(D.x)) smin = Y_MINUS;
				}
			}
		}
		else
		{
			if (D.y > EPSILON)
			{
				t = (Corner2.y - P.y) / D.y;
				if (t < tmin) return false;
				if (t <= tmax - DEPTH_MIN)
				{
					smax = Y_MINUS;
					tmax = t;
				}
				else
				{
					if (t <= tmax + DEPTH_MIN)
					{
						if (D.y > fabs(D.x)) smax = Y_MINUS;
					}
				}
				t = (Corner1.y - P.y) / D.y;
				if (t >= tmin + DEPTH_MIN)
				{
					if (t > tmax) return false;
					smin = Y_PLUS;
					tmin = t;
				}
				else
				{
					if (t >= tmin - DEPTH_MIN)
					{
						if (D.y > fabs(D.x)) smin = Y_PLUS;
					}
				}
			}
			else
			{
				if ((P.y < Corner1.y) || (P.y > Corner2.y))
				{
					return false;
				}
			}
		}

		if (D.z < -EPSILON)
		{
			t = (Corner1.z - P.z) / D.z;
			if (t < tmin) return false;
			if (t <= tmax - DEPTH_MIN)
			{
				smax = Z_PLUS;
				tmax = t;
			}
			else
			{
				if (t <= tmax + DEPTH_MIN)
				{
					switch (smax)
					{
					case X_PLUS :
					case X_MINUS : if (-D.z > fabs(D.x)) smax = Z_PLUS; 
						break;
					case Y_PLUS :
					case Y_MINUS : if (-D.z > fabs(D.y)) smax = Z_PLUS; 
						break;
					}
				}
			}
			t = (Corner2.z - P.z) / D.z;
			if (t >= tmin + DEPTH_MIN)
			{
				if (t > tmax) return false;
				smin = Z_MINUS;
				tmin = t;
			}
			else
			{
				if (t >= tmin - DEPTH_MIN)
				{
					switch (smin)
					{
					case X_PLUS :
					case X_MINUS : if (-D.z > fabs(D.x)) smin = Z_MINUS; 
						break;
					case Y_PLUS :
					case Y_MINUS : if (-D.z > fabs(D.y)) smin = Z_MINUS; 
						break;
					}
				}
			}
		}
		else
		{
			if (D.z > EPSILON)
			{
				t = (Corner2.z - P.z) / D.z;
				if (t < tmin) return false;
				if (t <= tmax - DEPTH_MIN)
				{
					smax = Z_MINUS;
					tmax = t;
				}
				else
				{
					if (t <= tmax + DEPTH_MIN)
					{
						switch (smax)
						{
						case X_PLUS :
						case X_MINUS : if (D.z > fabs(D.x)) smax = Z_MINUS; 
							break;
						case Y_PLUS :
						case Y_MINUS : if (D.z > fabs(D.y)) smax = Z_MINUS; 
							break;
						}
					}
				}
				t = (Corner1.z - P.z) / D.z;
				if (t >= tmin + DEPTH_MIN)
				{
					if (t > tmax) return false;
					smin = Z_PLUS;
					tmin = t;
				}
				else
				{
					if (t >= tmin - DEPTH_MIN)
					{
						switch (smin)
						{
						case X_PLUS :
						case X_MINUS : if (D.z > fabs(D.x)) smin = Z_PLUS; 
							break;
						case Y_PLUS :
						case Y_MINUS : if (D.z > fabs(D.y)) smin = Z_PLUS; 
							break;
						}
					}
				}
			}
			else
			{
				if ((P.z < Corner1.z) || (P.z > Corner2.z))
				{
					return false;
				}
			}
		}
		if (tmax < DEPTH_MIN)
		{
			return  false;
		}

		return(true);
	}

	bool MyInside_Box(SG_POINT pnt, SG_POINT Corn1, SG_POINT Corn2)
	{
		if ((pnt.x < Corn1.x) || (pnt.x > Corn2.x))
			return false;
		if ((pnt.y < Corn1.y) || (pnt.y > Corn2.y))
			return false;
		if ((pnt.z < Corn1.z) || (pnt.z > Corn2.z))
			return false;

		return true;
	}

	void MyGetPhongNormal(SG_VECTOR P1, SG_VECTOR P2, SG_VECTOR P3,
		SG_VECTOR p1Norm, SG_VECTOR p2Norm, SG_VECTOR p3Norm, 
		SG_VECTOR IPoint,SG_VECTOR& Result)
	{
		SG_VECTOR trPerp;

		SG_VECTOR P3_P2, VTemp1, VTemp2;
		double x, y, z, uDenominator, Proj;

		P3_P2 = sgSpaceMath::VectorsSub(P3, P2);

		x = fabs(P3_P2.x);
		y = fabs(P3_P2.y);
		z = fabs(P3_P2.z);

		VTemp1 = sgSpaceMath::VectorsSub(P2, P3);

		sgSpaceMath::NormalVector(VTemp1);

		VTemp2 = sgSpaceMath::VectorsSub(P1, P3);

		Proj = sgSpaceMath::VectorsScalarMult( VTemp2, VTemp1);

		VTemp1.x*=Proj;
		VTemp1.y*=Proj;
		VTemp1.z*=Proj;

		trPerp = sgSpaceMath::VectorsSub(VTemp1, VTemp2);

		sgSpaceMath::NormalVector(trPerp);

		uDenominator = sgSpaceMath::VectorsScalarMult( VTemp2, trPerp);

		trPerp.x/=-uDenominator;
		trPerp.y/=-uDenominator;
		trPerp.z/=-uDenominator;

		int Axis;
		double u, v;
		SG_VECTOR PI_P1;

		PI_P1 = sgSpaceMath::VectorsSub(IPoint, P1);

		u = sgSpaceMath::VectorsScalarMult( PI_P1, trPerp);

		if (u < EPSILON)
		{
			Result = p1Norm;
			return;
		}

		Axis = MAX_COORD(x, y, z);

		switch(Axis) {
case 0:
	v = (PI_P1.x / u + P1.x - P2.x) / (P3.x - P2.x);
	break;
case 1:
	v = (PI_P1.y / u + P1.y - P2.y) / (P3.y - P2.y);
	break;
case 2:
	v = (PI_P1.z / u + P1.z - P2.z) / (P3.z - P2.z);
	break;
		}

		Result.x = p1Norm.x + u * (p2Norm.x - p1Norm.x + v * (p3Norm.x - p2Norm.x));
		Result.y = p1Norm.y + u * (p2Norm.y - p1Norm.y + v * (p3Norm.y - p2Norm.y));
		Result.z = p1Norm.z + u * (p2Norm.z - p1Norm.z + v * (p3Norm.z - p2Norm.z));

		sgSpaceMath::NormalVector(Result);
	}

	void MyGetPhongUV(SG_VECTOR P1, SG_VECTOR P2, SG_VECTOR P3,
		UV_COORD p1UV, UV_COORD p2UV, UV_COORD p3UV, 
		SG_VECTOR InterPoint,UV_COORD Result)
	{
		double w1, w2, w3, t1, t2;
		SG_VECTOR vecA, vecB;
		SG_VECTOR Sd_1, Sd_2;

		SG_VECTOR P;

		P=InterPoint;

		Sd_1 = sgSpaceMath::VectorsSub(P3, P2);
		Sd_2 = sgSpaceMath::VectorsSub(P3, P1);

		vecA = sgSpaceMath::VectorsSub(P, P1);

		t1 = sgSpaceMath::VectorsScalarMult(Sd_2, Sd_1);
		t2 = sgSpaceMath::VectorsScalarMult(Sd_1, Sd_1);
		vecB.x = Sd_1.x*t1/t2; vecB.y = Sd_1.y*t1/t2; vecB.z = Sd_1.z*t1/t2;
		vecB.x -= Sd_2.x; vecB.y -= Sd_2.y; vecB.z -= Sd_2.z;

		t1 = sgSpaceMath::VectorsScalarMult(vecA, vecB);
		t2 = sgSpaceMath::VectorsScalarMult(vecB, vecB);
		w1 = 1+t1/t2;

		Sd_1 = sgSpaceMath::VectorsSub(P3, P1);
		Sd_2 = sgSpaceMath::VectorsSub(P3, P2);

		vecA = sgSpaceMath::VectorsSub(P, P2);


		t1 = sgSpaceMath::VectorsScalarMult(Sd_2, Sd_1);
		t2 = sgSpaceMath::VectorsScalarMult(Sd_1, Sd_1);
		vecB.x = Sd_1.x*t1/t2; vecB.y = Sd_1.y*t1/t2; vecB.z = Sd_1.z*t1/t2;
		vecB.x -= Sd_2.x; vecB.y -= Sd_2.y; vecB.z -= Sd_2.z;


		t1 = sgSpaceMath::VectorsScalarMult(vecA, vecB);
		t2 = sgSpaceMath::VectorsScalarMult(vecB, vecB);
		w2 = 1+t1/t2;

		Sd_1 = sgSpaceMath::VectorsSub(P2, P1);
		Sd_2 = sgSpaceMath::VectorsSub(P2, P3);

		vecA = sgSpaceMath::VectorsSub(P, P3);

		t1 = sgSpaceMath::VectorsScalarMult(Sd_2, Sd_1);
		t2 = sgSpaceMath::VectorsScalarMult(Sd_1, Sd_1);
		vecB.x = Sd_1.x*t1/t2; vecB.y = Sd_1.y*t1/t2; vecB.z = Sd_1.z*t1/t2;
		vecB.x -= Sd_2.x; vecB.y -= Sd_2.y; vecB.z -= Sd_2.z;

		t1 = sgSpaceMath::VectorsScalarMult(vecA, vecB);
		t2 = sgSpaceMath::VectorsScalarMult(vecB, vecB);
		w3 = 1+t1/t2;

		Result[0] =  w1 * p1UV[0] + w2 * p2UV[0] +  w3 * p3UV[0];
		Result[1] =  w1 * p1UV[1] + w2 * p2UV[1] +  w3 * p3UV[1];
	}

public:
	MyRTObject(sgC3DObject* ob)
	{
		m_objct = ob;
	}
	virtual  void         GetGabarits(RT_POINT& gab_min, RT_POINT& gab_max) const
	{
		SG_POINT gMin,gMax;
		m_objct->GetGabarits(gMin, gMax);
		gab_min.x = gMin.x;  gab_min.y = gMin.y;  gab_min.y = gMin.y;
		gab_max.x = gMax.x;  gab_max.y = gMax.y;  gab_max.y = gMax.y;
	}

	virtual  rtCMaterial* GetMaterial();

	virtual  bool         Intersect(const RT_POINT& rayPnt, const RT_VECTOR& rayDir, rtIIntersectionsStack* intStack)
	{
		SG_POINT pMin,pMax;

		m_objct->GetGabarits(pMin,pMax);

		RT_VECTOR vMin,vMax;
		vMin = SG_TO_RT(pMin);
		vMax = SG_TO_RT(pMax);

		if (!MyIntersect_Box(RT_TO_SG(rayPnt), RT_TO_SG(rayDir), pMin,pMax))
			return 0;

		const SG_ALL_TRIANGLES* trngls = m_objct->GetTriangles();

		if (trngls==NULL)
			return 0;

		SG_VECTOR  P,D;

		sgCMatrix  mmm(m_objct->GetWorldMatrixData());

		if (m_objct->GetWorldMatrixData() != NULL)
		{
			mmm.Inverse();
			mmm.Transparent();

			SG_POINT rI, rD;

			rI = RT_TO_SG(rayPnt);
			rD = RT_TO_SG(rayDir);

			mmm.ApplyMatrixToVector(rI,rD);

			P = rI;
			D = rD;
		}
		else
		{
			P = RT_TO_SG(rayPnt);
			D = RT_TO_SG(rayDir);
		}

		SG_VECTOR trP1, trP2, trP3;
		double IntersDepth;

		register bool Intersection_Found;
		Intersection_Found = false;

		SG_VECTOR IPoint;

		SG_POINT sgP1, sgP2,sgP3;
		SG_VECTOR sgN1, sgN2, sgN3;

		const sgCBRep* br= m_objct->GetBRep();

		for (unsigned int i=0;i<br->GetPiecesCount();i++)
		{
			const sgCBRepPiece* brPiece= br->GetPiece(i); 

			brPiece->GetLocalGabarits(pMin,pMax);

			vMin.x = pMin.x;  vMin.y = pMin.y;  vMin.z = pMin.z;
			vMax.x = pMax.x;  vMax.y = pMax.y;  vMax.z = pMax.z;

			if (!MyIntersect_Box(P, D, pMin,pMax))
				continue;

			int stTr, endTr;
			brPiece->GetTrianglesRange(stTr,endTr);

			for (int trInd=stTr; trInd<=endTr; trInd++)
			{
				trP1.x = trngls->allVertex[3*trInd].x;  
				trP1.y = trngls->allVertex[3*trInd].y;
				trP1.z = trngls->allVertex[3*trInd].z;

				trP2.x = trngls->allVertex[3*trInd+1].x;  
				trP2.y = trngls->allVertex[3*trInd+1].y;
				trP2.z = trngls->allVertex[3*trInd+1].z;

				trP3.x = trngls->allVertex[3*trInd+2].x;  
				trP3.y = trngls->allVertex[3*trInd+2].y;
				trP3.z = trngls->allVertex[3*trInd+2].z;

				if (MyIntersectTriangle(P, D ,trP1, trP2, trP3,&IntersDepth))
				{
					if ((IntersDepth > DEPTH_MIN) && (IntersDepth < DEPTH_MAX))
					{
						IPoint.x = rayPnt.x+IntersDepth*rayDir.x; 
						IPoint.y = rayPnt.y+IntersDepth*rayDir.y;
						IPoint.z = rayPnt.z+IntersDepth*rayDir.z;

						SG_VECTOR phNorm;
						SG_VECTOR trN1, trN2, trN3;

						sgP1.x = trngls->allVertex[3*trInd].x;  
						sgP1.y = trngls->allVertex[3*trInd].y;
						sgP1.z = trngls->allVertex[3*trInd].z;

						sgP2.x = trngls->allVertex[3*trInd+1].x;  
						sgP2.y = trngls->allVertex[3*trInd+1].y;
						sgP2.z = trngls->allVertex[3*trInd+1].z;

						sgP3.x = trngls->allVertex[3*trInd+2].x;  
						sgP3.y = trngls->allVertex[3*trInd+2].y;
						sgP3.z = trngls->allVertex[3*trInd+2].z;

						trN1.x = sgN1.x = trngls->allNormals[3*trInd].x;  
						trN1.y = sgN1.y = trngls->allNormals[3*trInd].y;
						trN1.z = sgN1.z = trngls->allNormals[3*trInd].z;

						trN2.x = sgN2.x = trngls->allNormals[3*trInd+1].x;  
						trN2.y = sgN2.y = trngls->allNormals[3*trInd+1].y;
						trN2.z = sgN2.z = trngls->allNormals[3*trInd+1].z;

						trN3.x = sgN3.x = trngls->allNormals[3*trInd+2].x;  
						trN3.y = sgN3.y = trngls->allNormals[3*trInd+2].y;
						trN3.z = sgN3.z = trngls->allNormals[3*trInd+2].z;

						sgCMatrix  m2(m_objct->GetWorldMatrixData());
						m2.Transparent();

						m2.ApplyMatrixToVector(sgP1, sgN1);
						m2.ApplyMatrixToVector(sgP2, sgN2);
						m2.ApplyMatrixToVector(sgP3, sgN3);

						trP1.x = sgP1.x;    trP1.y = sgP1.y;  trP1.z = sgP1.z;
						trP2.x = sgP2.x;    trP2.y = sgP2.y;  trP2.z = sgP2.z;
						trP3.x = sgP3.x;    trP3.y = sgP3.y;  trP3.z = sgP3.z;

						trN1.x = sgN1.x;    trN1.y = sgN1.y;  trN1.z = sgN1.z;
						trN2.x = sgN2.x;    trN2.y = sgN2.y;  trN2.z = sgN2.z;
						trN3.x = sgN3.x;    trN3.y = sgN3.y;  trN3.z = sgN3.z;

						MyGetPhongNormal(trP1, trP2, trP3, trN1, trN2, trN3, IPoint, phNorm);

						UV_COORD resUV;
						if (trngls->allUV)
						{ 
							UV_COORD p1UV, p2UV, p3UV;
							p1UV[0] = trngls->allUV[6*trInd];
							p1UV[1] = trngls->allUV[6*trInd+1];
							p2UV[0] = trngls->allUV[6*trInd+2];
							p2UV[1] = trngls->allUV[6*trInd+3];
							p3UV[0] = trngls->allUV[6*trInd+4];
							p3UV[1] = trngls->allUV[6*trInd+5];
							MyGetPhongUV(trP1, trP2, trP3, p1UV, p2UV, p3UV, IPoint,resUV);
						}
						else
						{
							resUV[0] = resUV[1] = 0.0;
						}

						{
							RT_POINT tpp;
							memcpy(&tpp,&IPoint,sizeof(RT_POINT));
							RT_VECTOR tpv;
							memcpy(&tpv,&phNorm,sizeof(RT_VECTOR));
							intStack->AddIntersection(tpp,IntersDepth,tpv,resUV[0],resUV[1]);

							Intersection_Found = true;
						}
					}
				} 
			}
		}
		return(Intersection_Found);
	};

};


class MyLightSource : public rtLightSource::rtCPointLightSource
{
public:
  virtual  RT_POINT                       GetLocation() const
  {
    RT_POINT resP = {150.0, -150.0, 150.0};
    return resP;
  }
  virtual  rtTexture::RT_COLOR  GetColor()   const
  {
    rtTexture::RT_COLOR col = {1.0, 1.0, 1.0, 0.0};
    return col;
  }
  virtual  bool                           GetAtmosphericInteraction() const
  {return false;};
};

class MyLightSource2 : public rtLightSource::rtCSpotLightSource
{
public:
  virtual  RT_POINT                       GetLocation() const
  {
    RT_POINT resP = {1.0, -1.0, 50.0};
    return resP;
  }
  virtual  RT_POINT                         GetLightPoint()const
  {
    RT_POINT resP = {1.0, -1.0, 0.0};
    return resP;
  }
  virtual  double                         GetLightRadius()const
  {
    return 3.0;
  }
  virtual  double                         GetRadius()const
  {
    return 10.0;
  }
  virtual  rtTexture::RT_COLOR  GetColor()   const
  {
    rtTexture::RT_COLOR col = {1.0, 1.0, 1.0, 0.0};
    return col;
  }
  virtual  bool                           GetAtmosphericAttenuation() const
  {return true;};
  virtual  bool                           GetAtmosphericInteraction() const
  {return true;};
};

class MyLightSource3 : public MyLightSource2
{
public:
	virtual  RT_POINT    GetLocation() const
	{
		RT_POINT resP = {1.0, -20.0, 30.0};
		return resP;
	}
	virtual  rtTexture::RT_COLOR  GetColor()   const
	{
		rtTexture::RT_COLOR col = {1.0, 0.0, 0.0, 0.0};
		return col;
	}
};

class MyLightSource4 : public MyLightSource2
{
public:
	virtual  RT_POINT    GetLocation() const
	{
		RT_POINT resP = {30.0, -2.0, 20.0};
		return resP;
	}
	virtual  rtTexture::RT_COLOR  GetColor()   const
	{
		rtTexture::RT_COLOR col = {0.0, 1.0, 1.0, 0.0};
		return col;
	}
};

typedef enum
{
	LC_FIRST,
	LC_SECOND
} LIGHT_CONFIG;

class MyRenderer : public RT_VIEW_PORT,public rtCLightSourcesContainer,
	public  rtIScene
{
  HBITMAP    m_oldBitmap;
  HDC        m_Compat_hdc;
  HDC        m_hDC;
  SIZE       m_draw_sizes;
  SIZE       m_shifts;
  CDlgRender *m_pDlgRender;
  CWnd *m_pWndPreview;
  int m_iWidhtPrev;
  int m_iHeightPrev;

  MyLightSource m_lS;
  MyLightSource2 m_lS2;
  MyLightSource3 m_lS3;
  MyLightSource4 m_lS4;

  LIGHT_CONFIG   m_light_config;
public:
  HBITMAP    m_bitmap;

  void    SetLightConfig(LIGHT_CONFIG lc)
  {
		m_light_config = lc;
  }
  LIGHT_CONFIG    GetLightConfig()
  {
	  return m_light_config;
  }
 
  virtual unsigned int    GetLightSourcesCount()
  {
	  if (m_light_config==LC_FIRST)
		return 1;
	  if (m_light_config==LC_SECOND)
		return 1;
	  return 1;
  }
  virtual const rtLightSource::rtCLightSource*  GetLightSource(unsigned int ind)
  {
	   if (m_light_config==LC_SECOND)
	   {
			if (ind==0)
				return &m_lS2;
			if (ind==1)
				return &m_lS3;
			if (ind==2)
				return &m_lS4;
	   }
	   return &m_lS;
  }


  virtual int       GetWidth();
  virtual int       GetHeight();
  virtual void      EndRender() {};

  virtual void __cdecl  RenderPixel(int x, int y,
    double pixRed, double pixGreen, double pixBlue, double pixAlpha)
  {
    if (!m_bitmap || !m_Compat_hdc)
      return;

    ::SetPixel(m_Compat_hdc,x,y,RGB((int)floor(pixRed*255.0),
                    (int)floor(pixGreen*255.0),
                    (int)floor(pixBlue*255.0)));

    if ((!(y%10) && x==0) || (y==m_draw_sizes.cy-1))
    {
/*      BitBlt(m_hDC, m_shifts.cx, m_shifts.cy, m_draw_sizes.cx, m_draw_sizes.cy,
        m_Compat_hdc, 0, 0, SRCCOPY);
	  m_pDlgRender->UpdatePreview();*/
		// stretch bitmap
	  ::SetStretchBltMode(m_hDC,HALFTONE);
	  ::StretchBlt(m_hDC,0,0,m_iWidhtPrev,m_iHeightPrev,m_Compat_hdc,m_shifts.cx, m_shifts.cy, m_draw_sizes.cx, m_draw_sizes.cy,SRCCOPY);
    }	
  };

  void InitRender(HDC  hDC, unsigned int width, unsigned int height,
                          unsigned int shift_x,  unsigned int shift_y, int iWidhtPrev, int iHeightPrev)
  {
    DeInitRender();

	m_iWidhtPrev   = iWidhtPrev;
	m_iHeightPrev  = iHeightPrev;

	m_Compat_hdc = CreateCompatibleDC(hDC);
    m_hDC  = hDC;

    BITMAPINFO bmi;
    VOID *pvbits;

    ZeroMemory(&bmi.bmiHeader,sizeof(bmi.bmiHeader));
    bmi.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
    bmi.bmiHeader.biHeight = height;
    bmi.bmiHeader.biWidth = width;
    bmi.bmiHeader.biPlanes = 1;
    bmi.bmiHeader.biBitCount = 24;
    bmi.bmiHeader.biCompression = BI_RGB;
    bmi.bmiHeader.biSizeImage = -1 * bmi.bmiHeader.biHeight * bmi.bmiHeader.biWidth * 4;

    m_bitmap = CreateDIBSection(NULL,&bmi,DIB_RGB_COLORS,&pvbits,NULL,0x0);

    m_oldBitmap = (HBITMAP)SelectObject(m_Compat_hdc, (HBITMAP)m_bitmap);

    m_draw_sizes.cx = width;
    m_draw_sizes.cy = height;
    m_shifts.cx = shift_x;
    m_shifts.cy = shift_y;
  };

  void  DrawOnDC(HDC distDC)
  {
    if (distDC && m_Compat_hdc)
      BitBlt(distDC, m_shifts.cx, m_shifts.cy, m_draw_sizes.cx, m_draw_sizes.cy,
      m_Compat_hdc, 0, 0, SRCCOPY);
  }

  void  DeInitRender()
  {
    if (m_oldBitmap && m_Compat_hdc)
    {
      SelectObject(m_Compat_hdc, (HBITMAP)m_oldBitmap);
      if (m_bitmap)
        DeleteObject(m_bitmap);
      DeleteDC(m_Compat_hdc);
    }
    m_bitmap = NULL;
    m_oldBitmap = NULL;
    m_Compat_hdc = NULL;
    m_hDC  = NULL;
    m_draw_sizes.cx = m_draw_sizes.cy = 0;
    m_shifts.cx = m_shifts.cy = 0;
  }



  MyRenderer()
  {
    m_bitmap = NULL;
    m_oldBitmap = NULL;
    m_Compat_hdc = NULL;
    m_hDC  = NULL;
    m_draw_sizes.cx = m_draw_sizes.cy = 0;
    m_shifts.cx = m_shifts.cy = 0;
	m_light_config = LC_FIRST;
  };

  ~MyRenderer()
  {
    DeInitRender();
  }
 private:
	 std::vector<rtIObject*>  m_objcts;

	 void AttachOneObjectToRTScene(sgCObject* curObj)
	 {
		 if (curObj->GetType()==SG_OT_3D)
		 {
			 sgC3DObject* objBREP = reinterpret_cast<sgC3DObject*>(curObj);
			 if (objBREP->GetTriangles()==NULL)
				 return;
			 m_objcts.push_back(new MyRTObject(objBREP));
		 }
		 else
			 if (curObj->GetType()==SG_OT_GROUP)
			 {
				 sgCGroup* grObj = reinterpret_cast<sgCGroup*>(curObj);
				 sgCObject*  chObj = grObj->GetChildrenList()->GetHead();
				 while (chObj) 
				 {
					 AttachOneObjectToRTScene(chObj);
					 chObj = grObj->GetChildrenList()->GetNext(chObj);
				 }
			 }
	 };

public:
	void FillScene()
	{
		ClearScene();
		sgCObject*  curObj = sgGetScene()->GetObjectsList()->GetHead();
		while (curObj) 
		{
			AttachOneObjectToRTScene(curObj);
			curObj = sgGetScene()->GetObjectsList()->GetNext(curObj);
		}
	};
	void ClearScene()
	{
		size_t  objCnt = m_objcts.size();
		for (size_t obInd = 0; obInd<objCnt;obInd++)
		{
			delete  m_objcts[obInd];
		}
		m_objcts.clear();
	};
	virtual size_t       GetObjectsCount()
	{
		return m_objcts.size();
	}
	virtual rtIObject*   GetObject(size_t indx)
	{
		return m_objcts[indx];
	}
};

extern MyRenderer global_rend;
#endif