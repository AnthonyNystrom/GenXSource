using System;


namespace Genetibase.MathX.NeGenProbability
{
    /// <summary>
    /// When to use a BinomialProbability:
    /// 		Use when you need to ask the question, 
    /// "If the chance of something happening is P percent,
    /// what is the probability it will happen Y times out of
    /// N attempts."
    /// Example: Die rolling. If the chance to roll a one is 
    /// 1/6 or 16.7%, what is the probability it will happen 3 times 
    /// out of 5 rolls.
    /// Notes: The event must be a discrete event like rolling a die,
    /// flipping a coin, or pulling a card.  Events that consider an
    /// average percentage that it will happen or if each trial in turn
    /// affects the percentage that the event will happen are not 
    /// modeled by the binomial probability distribution but can give 
    /// meaningful estimates. ex: A basketball player has a free throw 
    /// percentage of 70%. What is the probability he makes his next two
    /// free throw shots. His first shot would then change his free throw
    /// percentage by a small amount but if this fact is omitted the answer is 
    /// relatively the same. 
    /// </summary>
    public class NuGenBinomialProbability : NuGenProbability
    {

        protected double m_chance_of_success;
        protected int m_trials;
        protected int m_successes;
        /*  The discrete binomial probability.
            The equation for this probability is:
            P(Y) = N!/((N-Y)!Y!)*p^Y*(1-p)^(N-Y)
        */

        #region Constructors

        /// <summary>
        /// Constructs a binomial probability
        /// </summary>
        /// <param name="N">number of trials of this experiment; must be 0 is less than or equal to N</param>
        /// <param name="Y">number of successful trials; must be 0 is less than or equal to Y which is less than or equal to N</param>
        /// <param name="p">chance of success for each trial; must be 0.0 is less than or equal to p which is less than or equal to 1.0</param>
        /// <param name="rvt">Random variable comparison. Whether this probability is cumulative and which way it is.</param>

        NuGenBinomialProbability(int N, int Y, double p, RandomVariableType rvt)
            : base(Y, rvt)
        {
            m_trials = N;
            m_successes = Y;
            m_chance_of_success = p;

            if (!(Y <= N && Y >= 0)) throw new NuGenProbabilityException("Y<=N && Y >=0");
            if (!(p >= 0.0 && p <= 1.0)) throw new NuGenProbabilityException("p >=0.0 && p<=1.0");
        }

        #endregion

        /// <summary>
        ///	Computing the Result.	
        ///	At first the distinction is made that the factorial part
        ///	of the equation is going to be a large number, possibly
        ///	outside of the range of an int type and causing overflow. 
        ///	The second part of the equation involves taking the power
        ///	of two numbers below 1.0 which will yield a very small number
        ///	and here significant digits may be truncated. We know a probability
        ///	MUST be between zero and one so the general algorithm is to 
        ///	interleave the computing of the factorial side and the 
        ///	fractional power side of the equation so that the running 
        ///	result never strays far from being between zero and one.
        ///
        ///	There are a couple optimizations for when the number of 
        ///	successes is zero or equal to the number of trials. Also
        ///	of note is an optimization on the division of the factorial
        ///	by factorial.  All of the digits in either N-Y or Y are 
        ///	reproduced in the numerator. The greatest number of cancellations
        ///	is made depending on which is bigger, N-Y or Y, thus reducing 
        ///	significantly the number of multiplications.
        /// </summary>
        /// <returns>the computed result</returns>
        protected override double ComputeResult()
        {

            if (m_trials == 0) return 0.0;

            // initialize some variables
            double result = 1.0;
            int Y = m_successes;
            int N = m_trials;

            double P = m_chance_of_success;
            double Q = 1.0 - P;
            int range = 0, np = 0, nq = 0, nnumer = 0, ndenom = 0;

            // validate
            if (!(Y <= N && Y >= 0)) throw new NuGenProbabilityException("Y<=N && Y >=0");
            if (!(P <= 1.0 && P >= 0.0)) throw new NuGenProbabilityException("P <= 1.0 && P >=0.0");

            // check optimizations
            if (Y == 0)
            {
                return result = Math.Pow(Q, N);
            }

            if (Y == N)
            {
                return result = Math.Pow(P, Y);
            }

            // reorder the factorials to account for cancellations
            // in numerator and denominator.
            if (Y < N - Y)
            {
                range = Y;		// N-Y cancels out
            }

            else
            {
                range = N - Y;	// Y cancels out
            }
            np = Y;
            nq = N - Y;
            ndenom = range;
            nnumer = N;

            while (np > 0 || nq > 0 || ndenom > 0 || nnumer > (N - range))
            {

                // If the result is greater than one we want to divide by 
                // a denominator digit or multiply by percentage p or q.
                // If we are out of numerator digits then finish multiplying
                // with our powers of p or q or dividing by a denom digit.
                if (result >= 1.0 || nnumer == (N - range))
                {

                    if (ndenom > 0)
                    {
                        //m_resut *= (1.0/ndenom);
                        result /= ndenom;
                        --ndenom;
                    }

                    else if (nq > 0)
                    {
                        result *= Q;
                        --nq;
                    }

                    else if (np > 0)
                    {
                        result *= P;
                        --np;
                    }

                    else
                    {
                        throw new NuGenProbabilityException("Binomial NuGenProbability computation error- check success percentage between 0 and 1");
                    }
                }

                    // If the result is less than one then we want to multiply
                // by a numerator digit. If we are out of denominator digits,
                // powers of p or powers of q then multiply rest of result 
                // by numerator digits.
                else if (result < 1.0 || np == 0)
                {

                    if (nnumer > (N - range))
                    {
                        result *= nnumer;
                        --nnumer;
                    }

                    else
                    {
                        throw new NuGenProbabilityException("Binomial NuGenProbability computation error- unknown error");
                    }
                }

                else
                {
                    throw new NuGenProbabilityException("Binomial NuGenProbability computation error- possible value infinity or NaN");
                }
            }
            return result;
        }

        /// <summary>
        /// When the base class function, GetResult(), is called, if the probability
        /// is cumulative then GetResult will call to set the Random variable then 
        /// compute the probability and add it to the cumulative total.
        /// </summary>
        /// <param name="Y"></param>
        protected override void SetRV(int Y)
        {

            if (!(Y >= 0)) throw new NuGenProbabilityException("Y >=0");
            if (!(Y <= m_trials)) throw new NuGenProbabilityException("Y <= m_trials");
            m_successes = Y;
        }

        /// <summary>
        /// The Expected Value or population mean is defined by E(V) = N*p
        /// </summary>
        /// <returns></returns>
        public override double GetExpectedValue()
        {
            return m_trials * m_chance_of_success;
        }

        /// <summary>
        /// the Variance is defined by o^2 = N*p*q 
        /// </summary>
        /// <returns></returns>
        public override double GetVariance()
        {
            return m_trials * m_chance_of_success * (1.0 - m_chance_of_success);
        }
    }
}
