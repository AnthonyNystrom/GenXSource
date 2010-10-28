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

namespace MathNet.Numerics.Distributions
{
    /// <summary>
    /// Provides generation of laplace distributed random numbers.
    /// </summary>
    /// <remarks>
    /// The implementation of the <see cref="LaplaceDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Laplace_distribution">Wikipedia - Laplace distribution</a>.
    /// </remarks>
    public sealed class LaplaceDistribution : ContinuousDistribution
    {
        private double _location;
        private double _scale;

        #region Construction
        /// <summary>
        /// Initializes a new instance, using a <see cref="SystemRandomSource"/>
        /// as underlying random number generator.
        /// </summary>
        public LaplaceDistribution()
            : base()
        {
            SetDistributionParameters(0.0, 1.0);
        }

        /// <summary>
        /// Initializes a new instance, using the specified <see cref="RandomSource"/>
        /// as underlying random number generator.
        /// </summary>
        /// <param name="random">A <see cref="RandomSource"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="random"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public LaplaceDistribution(RandomSource random)
            : base(random)
        {
            SetDistributionParameters(0.0, 1.0);
        }

        /// <summary>
        /// Initializes a new instance, using a <see cref="SystemRandomSource"/>
        /// as underlying random number generator.
        /// </summary>
        public LaplaceDistribution(double location, double scale)
            : base()
        {
            SetDistributionParameters(location, scale);
        }
        #endregion

        #region Distribution Parameters
        /// <summary>
        /// Gets or sets the location mu parameter.
        /// </summary>
        public double Location
        {
            get { return _location; }
            set { SetDistributionParameters(value, _scale); }
        }

        /// <summary>
        /// Gets or sets the scale b parameter.
        /// </summary>
        public double Scale
        {
            get { return _scale; }
            set { SetDistributionParameters(_location, value); }
        }

        public void SetDistributionParameters(double location, double scale)
        {
            if(!IsValidParameterSet(location, scale))
                throw new ArgumentOutOfRangeException();

            _location = location;
            _scale = scale;
        }

        /// <summary>
        /// Determines whether the specified parameters is valid.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if scale is greater than 0.0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidParameterSet(double location, double scale)
        {
            return scale > 0;
        }
        #endregion

        #region Distribution Properties
        /// <summary>
        /// Gets the minimum possible value of generated random numbers.
        /// </summary>
        public override double Minimum
        {
            get { return double.MinValue; }
        }

        /// <summary>
        /// Gets the maximum possible value of generated random numbers.
        /// </summary>
        public override double Maximum
        {
            get { return double.MaxValue; }
        }

        /// <summary>
        /// Gets the mean value of generated random numbers.
        /// </summary>
        public override double Mean
        {
            get { return _location; }
        }

        /// <summary>
        /// Gets the median of generated random numbers.
        /// </summary>
        public override double Median
        {
            get { return _location; }
        }

        /// <summary>
        /// Gets the variance of generated random numbers.
        /// </summary>
        public override double Variance
        {
            get { return 2.0 * _scale * _scale; }
        }

        /// <summary>
        /// Gets the skewness of generated random numbers.
        /// </summary>
        public override double Skewness
        {
            get { return 0.0; }
        }

        public override double ProbabilityDensity(double x)
        {
            return 1.0 / (2.0 * _scale) * Math.Exp(-Math.Abs(x - _location) / _scale);
        }

        public override double CumulativeDistribution(double x)
        {
            if(x < _location)
                return 0.5 * Math.Exp((x - _location) / _scale);
            else
                return 1.0 - 0.5 * Math.Exp((_location - x) / _scale);
        }
        #endregion

        #region Generator
        /// <summary>
        /// Returns a laplace distributed floating point random number.
        /// </summary>
        /// <returns>A laplace distributed double-precision floating point number.</returns>
        public override double NextDouble()
        {
            double rand = 0.5 - this.RandomSource.NextDouble();
            return _location - _scale * Math.Sign(rand) * Math.Log(2.0 * Math.Abs(rand));
        }
        #endregion
    }
}
