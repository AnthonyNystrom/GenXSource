using System;


namespace Genetibase.MathX.NeGenProbability
{
    /// <summary>
    /// When to use the NuGenNegativeBinomialProbability
    /// The Negative Binomial NuGenProbability is similar to the 
    /// Geometric NuGenProbability in concept but similar to the
    /// Binomial NuGenProbability in computation.  The Negative
    /// Binomial probability answers the question, "What is 
    /// the chance that the Kth successful trial happens on
    /// the Yth trial.
    /// For example, Die rolling, what is the chance of rolling
    /// a one EXACTLY three times with the last one happening
    /// on the sixth roll. Here K=3, Y=6 and the chance to roll
    /// a one is 1/6
    /// </summary>
    public class NuGenNegativeBinomialProbability : NuGenProbability
    {

        protected int m_trials;
        protected int m_kth_success;
        protected double m_chance_of_success;

        //public NuGenNegativeBinomialProbability(int Y=1, int K=1, double p=0.0, NuGenProbability::RandomVariableType rvt=EQUAL);
        /// <summary>
        /// The Negative Binomial NuGenProbability
        /// The equation for this probability is:
        /// P(Y) = (Y-1)!/(((Y-1)-(K-1))!(K-1)!)*p^K*(1-p)^(Y-K)
        /// </summary>
        /// <param name="Y">number of the trial that the kth success happens; must be 0 less than or equal to Y</param>
        /// <param name="K">number of successful trials; must be 0 is less than or equal to K which is less than or equal to Y</param>
        /// <param name="p">chance of success for each trial; must be 0.0 is less than or equal to p which is less than or equal to 1.0</param>
        /// <param name="rvt">Random variable comparison. Whether this probability is cumulative and which way it is.</param>
        public NuGenNegativeBinomialProbability(int Y, int K, double p, RandomVariableType rvt)
            : base(Y, rvt)
        {
            m_trials = Y;
            m_kth_success = K;
            m_chance_of_success = p;

            if (!(0 <= Y && K >= 0)) throw new NuGenProbabilityException("0<=Y && K>=0");
            if (!(p >= 0.0 && p <= 1.0)) throw new NuGenProbabilityException("p >=0.0 && p<=1.0");
        }

        /// <summary>
        /// Computing the result.
        /// This algorithm is exactly the same as the Binomial NuGenProbability
        /// except that Y-1 is replacing N and K-1 is replacing Y in 
        /// the original algorithm for computing the factorial part.
        /// This directly relates to the fact that we assume K-1 successes
        /// have happened out of Y-1 trials and that the last trial is 
        /// the Kth success. The factorial part figures out how many possible
        /// ways we can have K-1 successes in Y-1 trials then the actual
        /// probabilities of those successes including the last one are 
        /// multiplied along with the Y-K number of failed trial probabilities. 
        /// </summary>
        /// <returns>the computed result</returns>
        protected override double ComputeResult()
        {

            // the base class function GetResult will possibly
            // set our number of trials below K so just return 0.0;
            if (m_kth_success > m_trials || m_trials == 0)
                return 0.0;

            // initialize some variables
            double result = 1.0;
            int K = m_kth_success;
            int Y = m_trials;

            double P = m_chance_of_success;
            double Q = 1.0 - P;
            int range = 0, np = 0, nq = 0, nnumer = 0, ndenom = 0;

            // validate
            if (!(K <= Y && K >= 0)) throw new NuGenProbabilityException("K<=Y && K >=0");
            if (!(P <= 1.0 && P >= 0.0)) throw new NuGenProbabilityException("P <= 1.0 && P >=0.0");

            // check optimizations
            if (K == 1)
            {
                return result = Math.Pow(Q, Y);
            }

            if (K == Y)
            {
                return result = Math.Pow(P, K);
            }

            // reorder the factorials to account for cancellations
            // in numerator and denominator.
            if (K < Y - K)
            {
                range = K - 1;		// Y-K cancels out
            }

            else
            {
                range = (Y - 1) - (K - 1);	// K cancels out
            }
            np = K;
            nq = Y - K;
            ndenom = range;
            nnumer = Y - 1;

            while (np > 0 || nq > 0 || ndenom > 0 || nnumer > (Y - 1 - range))
            {

                // If the result is greater than one we want to divide by 
                // a denominator digit or multiply by percentage p or q.
                // If we are out of numerator digits then finish multiplying
                // with our powers of p or q or dividing by a denom digit.
                if (result >= 1.0 || nnumer == (Y - 1 - range))
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
                else if (result < 1.0 || np == 0 /* || nq ==0 || ndenom ==0 */ )
                {

                    if (nnumer > (Y - 1 - range))
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
                    throw new NuGenProbabilityException("Binomial NuGenProbability computation error- possible value infinity or YaY");
                }
            }
            return result;
        }

        /// <summary>
        /// The Expected Value is defined by 
        /// E(Y) = K / p
        /// </summary>
        /// <returns>the expected result</returns>
        public override double GetExpectedValue()
        {

            if (m_chance_of_success == 0.0)
                return Double.MaxValue;
            return m_kth_success / m_chance_of_success;
        }

        /// <summary>
        /// The variance is defined by 
        /// o^2 = K*(1-p)/p^2
        /// </summary>
        /// <returns>the variance</returns>
        public override double GetVariance()
        {

            // the limit of K*(1-p)/p^2 as p->0 is infinity
            if (m_chance_of_success == 0.0)
                return Double.MaxValue;
            return m_kth_success * (1.0 - m_chance_of_success) / (m_chance_of_success * m_chance_of_success);
        }

        /// <summary>
        /// Random variable
        /// </summary>
        /// <param name="Y"></param>
        protected override void SetRV(int Y)
        {

            if (!(Y >= 0)) throw new NuGenProbabilityException("Y>=0");
            m_trials = Y;
        }
    }
}
