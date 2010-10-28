#region Math.NET Iridium (LGPL) by Ruegg + Contributors
// Math.NET Iridium, part of the Math.NET Project
// http://mathnet.opensourcedotnet.info
//
// Copyright (c) 2004-2007, Christoph R�egg,  http://christoph.ruegg.name
//						
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
#endregion
#region Derived From: Copyright 2006 Stefan Trosch�tz
/* 
 * Derived from the Troschuetz.Random Class Library,
 * Copyright � 2006 Stefan Trosch�tz (stefan@troschuetz.de)
 * 
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA 
 */
#endregion

using System;
using MathNet.Numerics.RandomSources;
using MathNet.Numerics.Properties;

namespace MathNet.Numerics.Distributions
{
    /// <summary>
    /// Declares common functionality for all discrete random number
    /// distributions based on a random source.
    /// </summary>
    public abstract class DiscreteDistribution : IDiscreteGenerator, IDiscreteProbabilityDistribution
    {
        #region instance fields
        /// <summary>
        /// Gets or sets a <see cref="RandomSource"/> object that can be used
        /// as underlying random number generator.
        /// </summary>
        public virtual RandomSource RandomSource
        {
            get { return _random; }
            set { _random = value; }
        }

        private RandomSource _random;

        /// <summary>
        /// Gets a value indicating whether the random number distribution can be reset,
        /// so that it produces the same  random number sequence again.
        /// </summary>
        public bool CanReset
        {
            get { return _random.CanReset; }
        }
        #endregion

        #region construction
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreteDistribution"/> class, using a 
        /// <see cref="SystemRandomSource"/> as underlying random number generator.
        /// </summary>
        protected DiscreteDistribution()
            : this(new SystemRandomSource())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreteDistribution"/> class, using the
        /// specified <see cref="RandomSource"/> as underlying random number generator.
        /// </summary>
        /// <param name="random">A <see cref="RandomSource"/> object.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="random"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        protected DiscreteDistribution(RandomSource random)
        {
            if (random == null)
            {
                string message = string.Format(null, Resources.ArgumentNull, "generator");
                throw new ArgumentNullException("generator", message);
            }
            _random = random;
        }
        #endregion

        #region instance methods
        /// <summary>
        /// Resets the random number distribution, so that it produces the same random number sequence again.
        /// </summary>
        public void Reset()
        {
            _random.Reset();
        }
        #endregion

        #region abstract members
        /// <summary>
		/// Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public abstract int Minimum
		{
			get;
		}

        /// <summary>
		/// Gets the maximum possible value of distributed random numbers.
        /// </summary>
        public abstract int Maximum
		{
			get;
		}

        /// <summary>
		/// Gets the mean of distributed random numbers.
        /// </summary>
        public abstract double Mean
		{
			get;
		}
		
		/// <summary>
		/// Gets the median of distributed random numbers.
		/// </summary>
        public abstract int Median
		{
			get;
		}

        /// <summary>
		/// Gets the variance of distributed random numbers.
        /// </summary>
        public abstract double Variance
		{
			get;
		}

        public abstract double Skewness
        {
            get;
        }
		
		/// <summary>
		/// Returns a distributed integer random number.
		/// </summary>
		/// <returns>A distributed 32 bit signed integer number.</returns>
        public abstract int NextInt32();

        public abstract double ProbabilityMass(int x);

        public abstract double CumulativeDistribution(double x);
        #endregion
    }
}
