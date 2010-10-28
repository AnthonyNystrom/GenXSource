
using System;

namespace Genetibase.MathX
{

	public enum RandomVariableType 
	{ EQUAL, LESSTHAN, GREATERTHAN, LESSTHANOREQUAL, GREATERTHANOREQUAL, NOTEQUAL };

	public abstract class NuGenProbability
	{

		public const double UNDEFINED = -1.0;

		protected RandomVariableType m_RVT;
		protected double m_probability;
		protected int m_RV;

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rv">The Random Variable value.</param>
		/// <param name="rvt">Random Variable type, whether it is cumulative and which way it is cumulative.</param>
		/// <param name="prob">The probability of the event.  This can be set before hand if it is known and the GetResult function will return it.</param>
		public NuGenProbability( int rv, RandomVariableType rvt, double prob)
		{
			m_RV=rv; m_RVT=rvt; m_probability=prob;
		}

		public NuGenProbability(int rv, RandomVariableType rvt) : this(rv, rvt, UNDEFINED)
		{
						
		}

		public NuGenProbability(int rv) : this(rv, RandomVariableType.EQUAL, UNDEFINED)
		{
						
		}

		public NuGenProbability() : this(0, RandomVariableType.EQUAL, UNDEFINED)
		{
						
		}

		#endregion

		/// <summary>
		/// This is the public function that is callable to compute a probability
		/// or a cumulative probability. For cumulative probabilities it is assumed
		/// that the Random Variables range is at least computable from zero to Y.
		/// Optimizations could be made to compute cumulative probabilities faster
		/// for events that have a very large number of FINITE trials by taking the
		/// shorter of the computable ranges and possibly subtracting from one if 
		/// necessary.
		/// 
		/// If the probability is set in the constructor then the value won't 
		/// be undefined and will be immediately returned. 
		/// </summary>
		/// <returns>the computed result</returns>
		public double GetResult()
		{
						
			if(m_probability != UNDEFINED )
				return m_probability;

			try
			{
				m_probability = 0.0;
				int i = 0;

				switch(m_RVT)
				{

					case RandomVariableType.EQUAL:
						m_probability = ComputeResult();
						break;

					case RandomVariableType.LESSTHAN:

						for(SetRV(i); i < m_RV; SetRV(++i))
							m_probability += ComputeResult();
						break;

					case RandomVariableType.GREATERTHANOREQUAL:

						for(SetRV(i),m_probability = 1.0; i < m_RV; SetRV(++i))
							m_probability -= ComputeResult();
						break;

					case RandomVariableType.GREATERTHAN:

						for(SetRV(i),m_probability = 1.0; i <= m_RV; SetRV(++i))
							m_probability -= ComputeResult();
						break;

					case RandomVariableType.LESSTHANOREQUAL:

						for(SetRV(i); i <= m_RV; SetRV(++i))
							m_probability += ComputeResult();
						break;

					case RandomVariableType.NOTEQUAL:
						m_probability = 1.0 - ComputeResult();
						return m_probability;
					default:
						throw new NuGenProbabilityException("NuGenProbability error- Random Variable type is unset");
				}
			}

			catch(NuGenProbabilityException pe)
			{
				m_probability = UNDEFINED;
				SetRV(m_RV);
				throw pe;
			}
			SetRV(m_RV);
			return m_probability;
		}

		public abstract double GetExpectedValue();
		public abstract double GetVariance();

		protected abstract double ComputeResult();
		protected abstract void   SetRV(int Y);
	}

	class NuGenProbabilityException : Exception
	{

		public NuGenProbabilityException(string whatString):base(whatString)
		{
						
		}
	}
}
