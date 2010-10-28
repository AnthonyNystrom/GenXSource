

#if !defined(AFX_HYPERGEOMETRICPROBABILITY_H__38A70C7D_CD14_42AB_BBAF_0D88C413CAD8__INCLUDED_)
#define AFX_HYPERGEOMETRICPROBABILITY_H__38A70C7D_CD14_42AB_BBAF_0D88C413CAD8__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Probability.h"

/*	When to use the HyperGeometricProbability
		This probability is the one used to compute odds in 
	many of the lotterys in the United States.  It is used when
	you are picking from two sets in a mixed population. The 
	basic example is a bag filled with red and black chips. 5 red
	chips and 4 black ones.  You are asked to choose 3 chips
	without looking.  What is the chance you will pick exactly
	two black chips.  If you look below you will see the construction
	parameters.  The population size, N, is 4 + 5 = 9. The sample size,
	n, is 3 since we are picking 3 chips.  The sample set we are interested
	in is black chips which there are 3 of so r = 3.  And finally the
	random variable, y, is 2, the number of black chips we want to choose out of 
	3 pulls.  

		To compute lottery odds lets assume the lottery in question has 
	50 numbers and they choose 6 of them at random live on television. 
	So we know N = 50 and the sample size n=6. You have 6 numbers you paid
	money for that you are interested in, these six numbers represent the 
	selected set r, r = 6.  Now for the grand payoff, getting all six numbers
	you need all picked numbers to be in your selected set.  y = 6.  Often there
	is a lesser payout for 5 numbers which you could compute by changing y = 5. Finally
	you usually win your money back if you match two numbers so here y = 2.  To 
	find the chance of winning anything you would compute P(y<=6) - P(y=1) since 
	usually matching one number has no payout.  Many lotterys use an extra
	number affectionately called a mega number or some sort which is separate from 
	the regular 50 or so numbers. This last number is computed by a regular
	BinomialProbability and multiplied against the previous result for six numbers
	(since the events are independent) to get the odds of winning.
*/
class HyperGeometricProbability : public Probability  
{
public:
	/*	Constructor Parameters
			N - the population size; N must be > 0
			n - the sample size; must be n <= N
			r - the sample set of items we are interested in.  
			y - the number of items from the sample set we are interested in. must be y <= r
	*/
	HyperGeometricProbability(int N=0, int n=0, int y=0, int r=0, Probability::RandomVariableType rvt=EQUAL);
	virtual ~HyperGeometricProbability(){};
	virtual double GetVariance() const;
	virtual double GetExpectedValue() const;

protected:
	int m_selectedset;
	virtual double ComputeResult() throw (ProbabilityException);
	int m_black;
	int m_red;
	int m_sample;
	int m_population;
	virtual void SetRV(int Y);
};

#endif // !defined(AFX_HYPERGEOMETRICPROBABILITY_H__38A70C7D_CD14_42AB_BBAF_0D88C413CAD8__INCLUDED_)
