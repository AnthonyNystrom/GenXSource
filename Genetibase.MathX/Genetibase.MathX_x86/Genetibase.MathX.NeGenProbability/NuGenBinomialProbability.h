#if !defined(AFX_BINOMIALPROBABILITY_H__A9190C4E_D484_47C3_B141_FFEF2A10AB0A__INCLUDED_)
#define AFX_BINOMIALPROBABILITY_H__A9190C4E_D484_47C3_B141_FFEF2A10AB0A__INCLUDED_

#if _MSC_VER > 1000
#pragma once

#endif // _MSC_VER > 1000
#include "Probability.h"

// When to use a BinomialProbability:
/*
	Use when you need to ask the question, 
	"If the chance of something happening is P percent,
	 what is the probability it will happen Y times out of
	 N attempts."
	Example: Die rolling. If the chance to roll a one is 
	1/6 or 16.7%, what is the probability it will happen 3 times 
	out of 5 rolls.
	Notes: The event must be a discrete event like rolling a die,
	flipping a coin, or pulling a card.  Events that consider an
	average percentage that it will happen or if each trial in turn
	affects the percentage that the event will happen are not 
	modeled by the binomial probability distribution but can give 
	meaningful estimates. ex: A basketball player has a free throw 
	percentage of 70%. What is the probability he makes his next two
	free throw shots. His first shot would then change his free throw
	percentage by a small amount but if this fact is omitted the answer is 
	relatively the same.
*/
class BinomialProbability : public Probability
{
public:
	/*	Constructor Parameters:
			N - number of trials of this experiment; must be 0 <= N
			Y - number of successful trials; must be 0 <= Y <= N
			p - chance of success for each trial; must be 0.0 <= p <= 1.0
			rvt - Random variable comparison. Whether this probability is cumulative and which way it is. 
	*/
	BinomialProbability(int N=0, int Y=0, double p=0.0, Probability::RandomVariableType rvt=EQUAL);
	virtual ~BinomialProbability(){};
	virtual double GetExpectedValue() const;
	virtual double GetVariance() const;

protected:
	virtual double ComputeResult() throw(ProbabilityException);
	virtual void   SetRV(int Y);
	double m_chance_of_success;
	int m_trials;
	int m_successes;
};

#endif // !defined(AFX_BINOMIALPROBABILITY_H__A9190C4E_D484_47C3_B141_FFEF2A10AB0A__INCLUDED_)
