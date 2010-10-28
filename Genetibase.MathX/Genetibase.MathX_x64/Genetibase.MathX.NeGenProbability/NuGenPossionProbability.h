

#if !defined(AFX_POISSONPROBABILITY_H__4EA2BD8D_DBA2_4B5B_9AB7_4FE1C9155992__INCLUDED_)
#define AFX_POISSONPROBABILITY_H__4EA2BD8D_DBA2_4B5B_9AB7_4FE1C9155992__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Probability.h"

// When to use the Poisson probability
/*	Use when you have events that can happen instantaneously
	and at any time. The chance of two or more events happening
	at the same time is effectively zero for a time span small 
	enough.

	For example: A certain street corner has an average of 7
	traffic accidents per year.  What is the probability that 
	there will be 8 accidents the following year.  Here our 
	lambda is 7, the average from which we base the likelihood
	of traffic accidents happening in a year.  Our random variable
	is 8.  The probability will be less than if our random variable 
	was 7 and more than if our random variable was 9.  This probability
	distribution has a bell shaped curve to it.
*/
class PoissonProbability : public Probability  
{
public:
	/*	Constructor Parameters
			Y - The number of events we want to know the probability of happening 
				given a known average in a given time period.
			Lambda - The average number of events in a given time period.
			rvt -	The random variable type. Whether it is cumulative or not and which way.
	*/
	PoissonProbability(int Y=0,double Lambda=0,Probability::RandomVariableType rvt=EQUAL);
	virtual ~PoissonProbability(){};
	virtual double GetVariance() const;
	virtual double GetExpectedValue() const;

protected:
	double m_poisson_average;
	int m_changes;
	virtual void SetRV(int Y);
	virtual double ComputeResult() throw (ProbabilityException);
};

#endif // !defined(AFX_POISSONPROBABILITY_H__4EA2BD8D_DBA2_4B5B_9AB7_4FE1C9155992__INCLUDED_)
