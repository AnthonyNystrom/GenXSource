#pragma once

#include <vector>

double CalcMatchResidueRmsd(CSTLVectorValueArray & m_atomPosArrayCa1, CSTLLONGArray & matchResidue1, CSTLVectorValueArray & m_atomPosArrayCa2, CSTLLONGArray & matchResidue2 , long nMatch , D3DXMATRIX &transform );
double CalcMatchResidueRmsd(CSTLVectorValueArray & arrayPos1, CSTLVectorValueArray & arrayPos2, D3DXMATRIX &transform );

double GetLocalSuperImposeTransform( IN CSTLVectorValueArray & arrayPos1, IN CSTLLONGArray & matchIndex1, IN CSTLVectorValueArray & arrayPos2, IN CSTLLONGArray & matchIndex2, long nMatch, OUT D3DXMATRIX &transform , BOOL bCalcRMSD = FALSE );
double GetLocalSuperImposeTransform( IN CVectorArray & arrayPos1, IN long iBegin1 , IN CVectorArray	& arrayPos2, IN long iBegin2, IN long nAtom , OUT D3DXMATRIX &transform );
double GetLocalSuperImposeTransform( IN CSTLArrayVector & arrayPos1, IN long iBegin1 , IN CSTLArrayVector	& arrayPos2, IN long iBegin2, IN long nAtom , OUT D3DXMATRIX &transform );

