using System;


namespace Genetibase.MathX.NeGenProbability
{
    /// <summary>
    /// When to use a Geometric probability :
    /// The geometric probability asks the question, "Given the chance
    /// that each trial being successful is the same, what is the probability
    /// that the first success happens on a the Yth trial."
    /// This can be referred to as a russian roulette type of question.  If a 
    /// revolver holds one bullet out of six chambers the chance for each 
    /// trial is the same.  The probability answers the percentage that bullet
    /// will go off in each successive round given all the previous rounds
    /// were unsuccesful. This can be seen as the simple multiplication rule of
    /// probability, for example, P(Y=3) = chance of failure * chance of failure *
    /// chance of success = 5/6 * 5/6 * 1/6 = 25/216 or about 2%
    /// </summary>
    public class NuGenGeometricProbability : NuGenProbability
    {

        protected int m_trial_of_success;
        protected double m_chance_of_success;

        //public GeometricProbability(int Y=0, double p=1.0, NuGenProbability::RandomVariableType rvt=EQUAL);

        /*	The discrete geometric probability
            The equation for this probability is:
            P(Y) = (1.0-p)^(y-1) * p
        */

        /// <summary>
        /// Constructs a geometric probability
        /// </summary>
        /// <param name="Y">The number of the trial that the success occurs on. Must be 0 is less than or equal to Y</param>
        /// <param name="p">Chance of success for each trial.	Must be 0.0 is less than or equal to p which is less than or equal to 1.0</param>
        /// <param name="rvt">Random variable comparison. Whether this probability is cumulative and which way it is. </param>
        public NuGenGeometricProbability(int Y, double p, RandomVariableType rvt)
            : base(Y, rvt)
        {
         
            m_trial_of_success = Y;
            m_chance_of_success = p;

            if (!(Y >= 0)) throw new NuGenProbabilityException("Y>=0");
            if (!(p >= 0.0 && p <= 1.0)) throw new NuGenProbabilityException("p >= 0.0  && p<=1.0");
        }

        /// <summary>
        /// Computing the Result.	
        /// </summary>
        /// <returns></returns>
        protected override double ComputeResult()
        {

            if (m_trial_of_success == 0) return 0.0;
            return Math.Pow((1.0 - m_chance_of_success), m_trial_of_success - 1.0) * m_chance_of_success;
        }

        /// <summary>
        /// Expected value or population mean is defined by E(Y) = 1/p 
        /// </summary>
        /// <returns>expected value</returns>
        public override double GetExpectedValue()
        {

            if (m_chance_of_success == 0.0)
                return Double.MaxValue;
            return 1.0 / m_chance_of_success;
        }

        /// <summary>
        /// Variance is defined by V(Y)= (1-p)/p^2 
        /// </summary>
        /// <returns>variance</returns>
        public override double GetVariance()
        {

            if (m_chance_of_success == 0.0)
                return Double.MaxValue;
            return (1.0 - m_chance_of_success) / (m_chance_of_success * m_chance_of_success);
        }

        /// <summary>
        /// Random variable
        /// </summary>
        /// <param name="Y"></param>
        protected override void SetRV(int Y)
        {

            if (!(Y >= 0)) throw new NuGenProbabilityException("Y>=0");
            m_trial_of_success = Y;
        }
    }
}
