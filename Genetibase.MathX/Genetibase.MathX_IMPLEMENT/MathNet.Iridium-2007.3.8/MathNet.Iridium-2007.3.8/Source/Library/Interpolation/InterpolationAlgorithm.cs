#region MathNet Numerics, Copyright �2004 Christoph Ruegg 

// MathNet Numerics, part of MathNet
//
// Copyright (c) 2004,	Christoph Ruegg, http://www.cdrnet.net
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

using System;
using MathNet.Numerics;

namespace MathNet.Numerics.Interpolation
{
	public interface IInterpolationAlgorithm
	{
		void Prepare(SampleList samples);
		double Interpolate(double t);
		double Extrapolate(double t);

		bool SupportErrorEstimation {get;}
		double Interpolate(double t, out double error);
	}
}
