using System;


namespace Genetibase.MathX.NeGenProbability
{
    /// <summary>
    /// When to use the HyperGeometricProbability
    /// This probability is the one used to compute odds in 
    /// many of the lotterys in the United States.  It is used when
    /// you are picking from two sets in a mixed population. The 
    /// basic example is a bag filled with red and black chips. 5 red
    /// chips and 4 black ones.  You are asked to choose 3 chips
    /// without looking.  What is the chance you will pick exactly
    /// two black chips.  If you look below you will see the construction
    /// parameters.  The population size, N, is 4 + 5 = 9. The sample size,
    /// n, is 3 since we are picking 3 chips.  The sample set we are interested
    /// in is black chips which there are 3 of so r = 3.  And 
    /// finally the
    /// random variable, y, is 2, the number of black chips we want to choose out of 
    /// 3 pulls.  
    ///
    /// To compute lottery odds lets assume the lottery in question has 
    /// 50 numbers and they choose 6 of them at random live on television. 
    /// So we know N = 50 and the sample size n=6. You have 6 numbers you paid
    /// money for that you are interested in, these six numbers represent the 
    /// selected set r, r = 6.  Now for the grand payoff, getting all six numbers
    /// you need all picked numbers to be in your selected set.  y = 6.  Often there
    /// is a lesser payout for 5 numbers which you could compute by changing y = 5. Finally
    /// you usually win your money back if you match two numbers so here y = 2.  To 
    /// find the chance of winning anything you would compute P(y &lt; =6) - P(y=1) since 
    /// usually matching one number has no payout.  Many lotterys use an extra
    /// number affectionately called a mega number or some sort which is separate from 
    /// the regular 50 or so numbers. This last number is computed by a regular
    /// BinomialProbability and multiplied against the previous result for six numbers
    /// (since the events are independent) to get the odds of winning.
    /// </summary>
    public class NuGenHyperGeometricProbability : NuGenProbability
    {

        protected int m_selectedset;
        protected int m_black;
        protected int m_red;
        protected int m_sample;
        protected int m_population;
        //public HyperGeometricProbability(int N=0, int n=0, int y=0, int r=0, NuGenProbability::RandomVariableType rvt=EQUAL);

        /// <summary>
        /// The HyperGeometric probability is defined by:
        /// The equation: (rCy)*((N-r)C(n-y))/(NCn)
        /// where (x C y ) represents a combination, "x choose y" 
        /// </summary>
        /// <param name="N">the population size; N must be > 0</param>
        /// <param name="n">the sample size; must be n is less than or equal to N</param>
        /// <param name="y">the sample set of items we are interested in.</param>
        /// <param name="r">the number of items from the sample set we are interested in. must be y is less than or equal to r</param>
        /// <param name="rvt">type of the random variable</param>
        public NuGenHyperGeometricProbability(int N, int n, int y, int r, RandomVariableType rvt)
            : base(y, rvt)
        {
            m_population = N;
            m_sample = n;
            m_red = y;
            m_black = n - y;
            m_selectedset = r;

            if (!(y <= r)) throw new NuGenProbabilityException("y <= r");
            if (!(n - y <= N - r)) throw new NuGenProbabilityException("n-y <= N-r");
            if (!(y >= 0)) throw new NuGenProbabilityException("y>=0");
            if (!(N > 0)) throw new NuGenProbabilityException("N>0");
            if (!(r >= 0)) throw new NuGenProbabilityException("r>=0");
            if (!(n >= 0)) throw new NuGenProbabilityException("n>=0");
        }

        /// <summary>
        /// Random variable
        /// </summary>
        /// <param name="Y"></param>
        protected override void SetRV(int Y)
        {

            if (!(Y >= 0)) throw new NuGenProbabilityException("Y>=0");
            if (!(Y <= m_selectedset)) throw new NuGenProbabilityException("Y <= m_selectedset");
            if (!(m_sample - Y <= m_population - m_selectedset)) throw new NuGenProbabilityException("m_sample - Y <= m_population - m_selectedset");
            m_red = Y;
            m_black = (m_sample - m_red);
        }

        /// <summary>
        /// The expected value or population mean is defined by: E(Y) = n*r/N 
        /// </summary>
        /// <returns>the expected value</returns>
        public override double GetExpectedValue()
        {
            return m_sample * m_selectedset / m_population;
        }

        /// <summary>
        /// The variance is defined by: 
        /// V(Y) = n * (r/N) * (N-r)/N * (N-n)/(N-1)
        /// </summary>
        /// <returns>the variance</returns>
        public override double GetVariance()
        {

            if (m_population == 1)
                return 0.0;
            return m_sample * m_selectedset / m_population * (m_population - m_selectedset) / m_population *
                (m_population - m_sample) / (m_population - 1);
        }

        /// <summary>
        /// The equation: (rCy)*((N-r)C(n-y))/(NCn)
        /// Computing the result
        /// The computation is composed of pure combinations which are 
        /// handled in the same fashion as the Binomial and NegativeBinomial
        /// probabilities.  The equation is broken up into numerators and
        /// denominators and then appropriately multiplied by or divided by 
        /// depending on the running result being above or below 1.0.
        /// </summary>
        /// <returns>the computed result</returns>
        protected override double ComputeResult()
        {

            if (m_population == 0)
                return 0.0;

            double result = 1.0;
            int numer1, numer2, numer3, numer1cmp, numer2cmp, numer3cmp;
            int denom1, denom2, denom3, range1, range2, range3;
            numer1 = numer1cmp = m_selectedset;
            numer2 = numer2cmp = m_population - m_selectedset;
            numer3 = numer3cmp = m_population;

            //optimization on (rCy)
            if (m_selectedset - m_red > m_red)
                range1 = denom1 = m_red;

            else
                range1 = denom1 = m_selectedset - m_red;

            //optimization on ((N-r)C(n-y))
            if ((m_population - m_selectedset) - (m_sample - m_red) > m_sample - m_red)
                range2 = denom2 = m_sample - m_red;

            else
                range2 = denom2 = (m_population - m_selectedset) - (m_sample - m_red);

            //optimization on (NCn)
            if (m_population - m_sample > m_sample)
                range3 = denom3 = m_sample;

            else
                range3 = denom3 = m_population - m_sample;

            while (numer1 > numer1cmp - range1 || numer2 > numer2cmp - range2 || numer3 > numer3cmp - range3
                || denom1 > 0 || denom2 > 0 || denom3 > 0)
            {

                if (result > 1.0 || denom3 == 0)
                {

                    if (denom1 > 0)
                        result = result / denom1--;

                    else if (denom2 > 0)
                        result = result / denom2--;

                    else if (numer3 > numer3cmp - range3)
                        result = result / numer3--;

                    else
                        throw new NuGenProbabilityException("HyperGeometric NuGenProbability error- premature end of divisors");
                }

                else if (result <= 1.0 || numer3 - range3 == 0)
                {

                    if (numer1 > numer1cmp - range1)
                        result = result * numer1--;

                    else if (numer2 > numer2cmp - range2)
                        result = result * numer2--;

                    else if (denom3 > 0)
                        result = result * denom3--;

                    else
                        throw new NuGenProbabilityException("HyperGeometric NuGenProbability error- premature end of multipliers");
                }

                else
                    throw new NuGenProbabilityException("HyperGeometric NuGenProbability error- result is NaN");
            }
            return result;
        }
    }
}
