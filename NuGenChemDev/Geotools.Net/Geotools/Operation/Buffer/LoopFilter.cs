/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. (translated from Java Topology Suite, 
 *  Copyright 2001 Vivid Solutions)
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */


#region Using
using System;
using System.Collections;
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Buffer
{
	/// <summary>
	/// LineFilter implements a filter that removes small loops from the line created
	/// by BufferLineBuilder
	/// </summary>
	internal class LoopFilter
	{

		private static Coordinates _arrayTypeCoordinates = new Coordinates();

		private int _maxPointsInLoop = 10;           // maximum number of points in a loop
		private double _maxLoopExtent = 10.0;    // the maximum X and Y extents of a loop
		private Coordinates _newPts = new Coordinates();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the LoopFilter class.
		/// </summary>
		public LoopFilter()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		public Coordinates Filter(Coordinates inputPts)
		{
			
			_newPts.Clear();
			int i = 0;
			while (i < inputPts.Count) 
			{
				AddPoint((Coordinate)inputPts[i]);
				int loopSize = CheckForLoop(inputPts, i);
				// skip loop if one was found
				i++;
				if (loopSize > 0) 
				{
					//Assert.isTrue(inputPts[i - 1].equals(inputPts[i - 1 + loopSize]), "non-loop found in LoopFilter");
					if (!(inputPts[i - 1].Equals(inputPts[i - 1 + loopSize])))
					{
						throw new InvalidOperationException("non-loop found in LoopFilter");
					}
					i += loopSize;
				}
			}
			//return (Coordinate[]) newPts.toArray(arrayTypeCoordinate);
			return _newPts;
		}

		private void AddPoint(Coordinate p)
		{
			
			// don't add duplicate points
			if (_newPts.Count >= 1 && _newPts[_newPts.Count - 1].Equals(p))
				return;
			_newPts.Add(p);
			
		}

		/// <summary>
		/// Find a small loop starting at this point, if one exists. If found, return the index of the 
		/// last point of the loop. If none exists, return 0
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		private int CheckForLoop(Coordinates pts, int startIndex)
		{
			
			Coordinate startPt = pts[startIndex];
			Envelope env = new Envelope();
			env.ExpandToInclude(startPt);
			int endIndex = startIndex;
			for (int j = 1; j <= _maxPointsInLoop; j++) 
			{
				endIndex = startIndex + j;
				if (endIndex >= pts.Count) break;

				env.ExpandToInclude(pts[endIndex]);
				if (pts[endIndex].Equals(startPt)) 
				{
					if (env.Height < _maxLoopExtent && env.Width < _maxLoopExtent) 
					{
						return j;
					}
				}
			}
			return 0;
		}

		#endregion

	}
}
