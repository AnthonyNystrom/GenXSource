

#if !defined(AFX_NEGATIVEBINOMIALPROBABILITY_H__608496F7_54D8_4D74_9F8E_008D948E042A__INCLUDED_)
#define AFX_NEGATIVEBINOMIALPROBABILITY_H__608496F7_54D8_4D74_9F8E_008D948E042A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Probability.h"

/*	When to use the NegativeBinomialProbability
	The Negative Binomial Probability is similar to the 
	Geometric Probability in concept but similar to the
	Binomial Probability in computation.  The Negative
	Binomial probability answers the question, "What is 
	the chance that the Kth successful trial happens on
	the Yth trial.
	For example, Die rolling, what is the chance of rolling
	a one EXACTLY three times with the last one happening
	on the sixth roll. Here K=3, Y=6 and the chance to roll
	a one is 1/6
*/
class NegativeBinomialProbability : public Probability  
{
public:
	/*	Constructor Parameters:
			Y - number of the trial that the kth success happens; must be 0 <= Y
			K - number of successful trials; must be 0 <= K <= Y
			p - chance of success for each trial; must be 0.0 <= p <= 1.0
			rvt - Random variable comparison. Whether this probability is cumulative and which way it is. 
	*/
	NegativeBinomialProbability(int Y=1, int K=1, double p=0.0, Probability::RandomVariableType rvt=EQUAL);
	virtual ~NegativeBinomialProbability(){};
	virtual double GetVariance() const;
	virtual double GetExpectedValue() const;

protected:
	virtual void SetRV(int Y);
	virtual double ComputeResult();
	int m_trials;
	int m_kth_success;
	double m_chance_of_success;
};

#endif // !defined(AFX_NEGATIVEBINOMIALPROBABILITY_H__608496F7_54D8_4D74_9F8E_008D948E042A__INCLUDED_)
