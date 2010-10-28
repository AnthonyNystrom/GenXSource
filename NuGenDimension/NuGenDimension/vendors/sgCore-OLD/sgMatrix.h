#ifndef  __sgMatrix__
#define  __sgMatrix__

class sgCore_API sgCMatrix
{
private:
	double     m_matrix[16];
	double*    m_temp_buffer;
public:
	sgCMatrix();
	sgCMatrix(const double*);
	sgCMatrix(sgCMatrix&);
	~sgCMatrix();
	sgCMatrix& operator = (const sgCMatrix& );

	bool  SetMatrix(const sgCMatrix*);

	const double*   GetData();
	const double*   GetTransparentData();

	void  Identity();

	void  Transparent();
	bool  Inverse();
	void  Multiply(const sgCMatrix& MatrB);
	void  Translate(const SG_VECTOR& transVector);
	void  Rotate(const SG_POINT& axePoint, const SG_VECTOR& axeDir, double alpha_radians);
	void  VectorToZAxe(const SG_VECTOR& vect);

	void  ApplyMatrixToVector(SG_POINT& vectBegin, SG_VECTOR& vectDir) const;
	void  ApplyMatrixToPoint(SG_POINT& pnt) const;
};

#endif