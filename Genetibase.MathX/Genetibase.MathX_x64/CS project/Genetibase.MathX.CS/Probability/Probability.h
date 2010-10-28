
#pragma warning(disable: 4290)

#if !defined(AFX_PROBABILITY_H__34941F48_695B_4409_9785_75DD232B50C0__INCLUDED_)
#define AFX_PROBABILITY_H__34941F48_695B_4409_9785_75DD232B50C0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
#define UNDEFINED -1.0

class Probability  
{
public:
	enum RandomVariableType { EQUAL, LESSTHAN, GREATERTHAN, LESSTHANOREQUAL, GREATERTHANOREQUAL, NOTEQUAL };

	/* Constructor Parameters:
		rv - The Random Variable value.
		rvt - Random Variable type, whether it is cumulative and which way it is cumulative.
		prob - The probability of the event.  This can be set before hand if it is known
				and the GetResult function will return it. 
	*/
	Probability( int rv=0, RandomVariableType rvt=EQUAL, double prob=UNDEFINED)
		:m_RV(rv), m_RVT(rvt), m_probability(prob){};
	virtual ~Probability(){};

	class ProbabilityException  
	{
	public:
		ProbabilityException(const char* whatString):What(whatString){}
		inline const char* what() const { return What; }
	protected:
		const char* What;
	};

	virtual double GetResult() throw (ProbabilityException) ;
	virtual double GetExpectedValue() const = 0;
	virtual double GetVariance() const = 0;
protected:
	virtual double ComputeResult() = 0;
	virtual void   SetRV(int Y)    = 0;
	RandomVariableType m_RVT;
	double m_probability;
	int m_RV;
};

#endif // !defined(AFX_PROBABILITY_H__34941F48_695B_4409_9785_75DD232B50C0__INCLUDED_)
