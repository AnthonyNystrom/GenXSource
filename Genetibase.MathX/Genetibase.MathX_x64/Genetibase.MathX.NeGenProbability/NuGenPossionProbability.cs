using System;

namespace Genetibase.MathX.NeGenProbability
{
    /// <summary>
    /// When to use the Poisson probability
    /// Use when you have events that can happen instantaneously
    /// and at any time. The chance of two or more events happening
    /// at the same time is effectively zero for a time span small 
    /// enough.
    /// 
    /// For example: A certain street corner has an average of 7
    /// traffic accidents per year.  What is the probability that 
    /// there will be 8 accidents the following year.  Here our 
    /// lambda is 7, the average from which we base the likelihood
    /// of traffic accidents happening in a year.  Our random variable
    /// is 8.  The probability will be less than if our random variable 
    /// was 7 and more than if our random variable was 9.  This probability
    /// distribution has a bell shaped curve to it. 
    /// </summary>
    public class NuGenPoissonProbability : NuGenProbability
    {

        protected double m_poisson_average;
        protected int m_changes;
        //public NuGenPoissonProbability(int Y=0,double Lambda=0,NuGenProbability::RandomVariableType rvt=EQUAL);

        /// <summary>
        ///  The Poisson probability.
        /// The equation for this probability is:
        /// P(Y) = lambda^Y * e^lambda / Y!
        /// </summary>
        /// <param name="Y">The number of events we want to know the probability of happening given a known average in a given time period.</param>
        /// <param name="Lambda">The average number of events in a given time period.</param>
        /// <param name="rvt">The random variable type. Whether it is cumulative or not and which way.</param>
        public NuGenPoissonProbability(int Y, double Lambda, RandomVariableType rvt)
            : base(Y, rvt)
        {
            m_changes = Y;
            m_poisson_average = Lambda;

            if (!(Y >= 0)) throw new NuGenProbabilityException("Y >=0");
            if (!(Lambda >= 0)) throw new NuGenProbabilityException("Lambda >=0");
        }

        /// <summary>
        /// Computing the result.
        /// Similar in form to the Binomial and the Negative Binomial functions.
        /// Components are broken up into factors of lambda, e, and digits of
        /// the factorial in the denominator.  Lambda is broken down into its
        /// integral and fractional form with the first calculation being multiplying
        /// the result by a power of e to this fractional form.
        /// Like the Binomial, the running result is made to hover around the value
        /// of one. If the result is over one then we divide by a denominator digit.
        /// If the result is less than one then we multiply by lambda or e.
        /// </summary>
        /// <returns>the computed results</returns>
        protected override double ComputeResult()
        {
            int powers, denom, integral;

            double fractional;//, temp;
            powers = denom = m_changes;
            //fractional	 = modf(m_poisson_average, temp);
            integral = (int)m_poisson_average; // this should take the integral part
            fractional = m_poisson_average - integral;

            double result = 1.0;
            result *= Math.Pow(Math.Exp(1), -1.0 * fractional);

            while (powers > 0 || denom > 0 || integral > 0)
            {

                if (result > 1.0 || powers == 0)
                {

                    if (denom > 0)
                    {
                        result /= denom;
                        --denom;
                    }

                    else if (integral > 0)
                    {
                        result /= Math.Exp(1);
                        --integral;
                    }

                    else
                        throw new NuGenProbabilityException("Poisson error- premature end of denominator counter");

                }

                else if (result <= 1.0 || integral == 0)
                {

                    if (powers > 0)
                    {
                        result *= m_poisson_average;
                        --powers;
                    }

                    else
                        throw new NuGenProbabilityException("Poisson error- premature end of power or integral counter");
                }

                else
                    throw new NuGenProbabilityException("Poisson error- result is NaN");
            }
            return result;
        }

        /// <summary>
        /// Random variable
        /// </summary>
        /// <param name="Y"></param>
        protected override void SetRV(int Y)
        {

            if (!(Y >= 0)) throw new NuGenProbabilityException("Y>=0");
            m_changes = Y;
        }

        /// <summary>
        /// The mean and the variance of a Poisson probability are the same.
        /// </summary>
        /// <returns>the expected value</returns>
        public override double GetExpectedValue()
        {
            return m_poisson_average;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>the variance</returns>
        public override double GetVariance()
        {
            return m_poisson_average;
        }
    }
}
