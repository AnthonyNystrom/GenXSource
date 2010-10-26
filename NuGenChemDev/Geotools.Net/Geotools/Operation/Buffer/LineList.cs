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
	/// Summary description for LineList.
	/// </summary>
	internal class LineList
	{

		private static Coordinates _arrayTypeCoordinate = new Coordinates();
		ArrayList _lines = new ArrayList();
		ArrayList _currPtList;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the LineList class.
		/// </summary>
		public LineList()
		{
			AddNewList();
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		private void AddNewList()
		{
			
			_currPtList = new ArrayList();
			_lines.Add(_currPtList);
			
		}

		public ArrayList GetLines()
		{
			
			ArrayList coordLines = new ArrayList();
			foreach(object obj in _lines)
			//for (Iterator i = lines.iterator(); i.hasNext(); ) 
			{
				ArrayList ptList = (ArrayList) obj;
				coordLines.Add(ptList);
			}
			return coordLines;
		}

		/*private ArrayList GetCoordinates(ArrayList ptList)
		{
			
			Coordinate[] coord = (Coordinate[]) ptList.toArray(arrayTypeCoordinate);
			return coord;
		}*/


		public void AddPt(Coordinate pt)
		{
			
			Coordinate bufPt = new Coordinate(pt);
			bufPt.MakePrecise();
			// don't add duplicate points
			Coordinate lastPt = null;
			if (_currPtList.Count >= 1)
				lastPt = (Coordinate) _currPtList[_currPtList.Count- 1];
			if (lastPt != null && bufPt.Equals(lastPt)) return;

			_currPtList.Add(bufPt);
			//System.out.println(bufPt);
			
		}

		public void EndEdge()
		{
			
			if (_currPtList.Count < 1) return;
			Coordinate lastPt =  (Coordinate)_currPtList[_currPtList.Count - 1];

			// start a new list for the next pt
			AddNewList();
			
		}

		#endregion

	}
}
