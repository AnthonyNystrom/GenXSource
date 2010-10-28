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
    /// Provides generation of pareto distributed random numbers.
    /// </summary>
    /// <remarks>
    /// The implementation of the <see cref="ParetoDistribution"/> type bases upon information presented on
    ///   <a href="http://en.wikipedia.org/wiki/Pareto_distribution">Wikipedia - Pareto distribution</a> and
    ///   <a href="http://www.xycoon.com/par_random.htm">Xycoon - Pareto Distribution</a>.
    /// </remarks>
    public sealed class ParetoDistribution : ContinuousDistribution
    {
        private double _location;
        private double _shape;
        private double helper1;

        #region Construction
        /// <summary>
        /// Initializes a new instance, using a <see cref="SystemRandomSource"/>
        /// as underlying random number generator.
        /// </summary>
        public ParetoDistribution()
            : base()
        {
            SetDistributionParameters(1.0, 1.0);
        }

        /// <summary>
        /// Initializes a new instance, using the specified <see cref="RandomSource"/>
        /// as underlying random number generator.
        /// </summary>
        /// <param name="random">A <see cref="RandomSource"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="random"/> is NULL (<see langword="Nothing"/> in Visual Basic).
        /// </exception>
        public ParetoDistribution(RandomSource random)
            : base(random)
        {
            SetDistributionParameters(1.0, 1.0);
        }

        /// <summary>
        /// Initializes a new instance, using a <see cref="SystemRandomSource"/>
        /// as underlying random number generator.
        /// </summary>
        public ParetoDistribution(double location, double shape)
            : base()
        {
            SetDistributionParameters(location, shape);
        }
        #endregion

        #region Distribution Parameters
        /// <summary>
        /// Gets or sets the location xm parameter.
        /// </summary>
        public double Location
        {
            get { return _location; }
            set { SetDistributionParameters(value, _shape); }
        }

        /// <summary>
        /// Gets or sets the shape k parameter.
        /// </summary>
        public double Shape
        {
            get { return _shape; }
            set { SetDistributionParameters(_location, value); }
        }

        public void SetDistributionParameters(double location, double shape)
        {
            if(!IsValidParameterSet(location, shape))
                throw new ArgumentOutOfRangeException();

            _location = location;
            _shape = shape;
            helper1 = 1.0 / shape;
        }

        /// <summary>
        /// Determines whether the specified parameters is valid.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if both location and shape are greater than 0.0; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsValidParameterSet(double location, double shape)
        {
            return location > 0.0 && shape > 0.0; ;
        }
        #endregion

        #region Distribution Properties
        /// <summary>
        /// Gets the minimum possible value of generated random numbers.
        /// </summary>
        public override double Minimum
        {
            get { return _location; }
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
        /// <exception cref="NotSupportedException"/>
        public override double Mean
        {
            get {
                if(_shape > 1.0)
                    return _location * _shape / (_shape - 1.0);
                else
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the median of generated random numbers.
        /// </summary>
        public override double Median
        {
            get { return _location * Math.Pow(2.0, 1.0 / _shape); }
        }

        /// <summary>
        /// Gets the variance of generated random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException"/>
        public override double Variance
        {
            get
            {
                if(_shape > 2.0)
                {
                    double a = _shape - 1.0;
                    return _shape * _location * _location / (a * a * (_shape - 2.0));
                }
                else
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the skewness of generated random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException"/>
        public override double Skewness
        {
            get
            {
                if(_shape > 3.0)
                    return 2.0 * (1.0 + _shape) / (_shape - 3.0) * Math.Sqrt((_shape - 2) / _shape);
                else
                    throw new NotSupportedException();
            }
        }

        public override double ProbabilityDensity(double x)
        {
            return Math.Exp(Math.Log(_shape) + _shape * Math.Log(_location) - (_shape + 1.0) * Math.Log(x));
        }

        public override double CumulativeDistribution(double x)
        {
            return 1.0 - Math.Pow(_location / x, _shape);
        }
        #endregion

        #region Generator
        /// <summary>
        /// Returns a pareto distributed floating point random number.
        /// </summary>
        /// <returns>A pareto distributed double-precision floating point number.</returns>
        public override double NextDouble()
        {
            return _location / Math.Pow(1.0 - this.RandomSource.NextDouble(), this.helper1);
        }
        #endregion
    }
}
