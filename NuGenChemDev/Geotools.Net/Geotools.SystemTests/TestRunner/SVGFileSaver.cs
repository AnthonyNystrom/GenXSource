/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
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
#region using Statements

using System;
using System.IO;
using Geotools.Geometries;
using Geotools.IO;

#endregion using Statements

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// Writes geometries to an SVG file.
	/// </summary>
	public class SVGFileSaver
	{

		#region Members
		#endregion Members

		#region Constructors

		public SVGFileSaver()
		{
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Converts the geometry to svg and writes the svg to a file.
		/// </summary>
		/// <param name="filename">The name of the svg file.</param>
		/// <param name="pm">The precision model for the geometries</param>
		/// <param name="a">The geometry to be written to svg.</param>
		public void CreateSVG(string filename, Geotools.Geometries.PrecisionModel pm, Geometry a)
		{
			GeometrySVGWriter svgWriter = new GeometrySVGWriter(pm);
			StreamWriter sw = new StreamWriter(filename);
			double minx, miny, maxx, maxy;
			a.Extent2D(out minx, out miny, out maxx, out maxy);
			sw.WriteLine(String.Format("<svg viewBox=\"{0} {1} {2} {3}\">",minx,miny,maxx,maxy*1.2));
			svgWriter.Write(a,sw,"fill-rule:evenodd;","fill:ltgray;stroke:blue;stroke-width:1;fill-opacity:0.2");
			sw.WriteLine("</svg>");
			sw.Close();
		}
		/// <summary>
		/// Writes three geometries to an svg file
		/// </summary>
		/// <param name="filename">The path of the svg file.</param>
		/// <param name="a">The A geometry</param>
		/// <param name="b">The B geometry</param>
		/// <param name="c">The C geometry</param>
		public void DisplayTest(string filename, Geometry a, Geometry b, Geometry c)
		{
			Geotools.Geometries.PrecisionModel pm = new Geotools.Geometries.PrecisionModel(1, 0, 0);
			GeometryFactory fact = new GeometryFactory(pm, 0);
			GeometrySVGWriter svgWriter = new GeometrySVGWriter(fact.PrecisionModel);
			StreamWriter sw = new StreamWriter(filename);
			GeometryCollection geomCollection= fact.CreateGeometryCollection(new Geometry[]{a,b,c});
			double minx, miny, maxx, maxy;
			geomCollection.Extent2D(out minx, out miny, out maxx, out maxy);
			sw.WriteLine(String.Format("<svg viewBox=\"{0} {1} {2} {3}\">",minx,miny,maxx,maxy*1.2));
			svgWriter.Write(a,sw,"fill-rule:evenodd;","fill:none;stroke:blue;stroke-width:1;fill-opacity:0.2");
			svgWriter.Write(b,sw,"fill-rule:evenodd;","fill:none;stroke:red;stroke-width:1;fill-opacity:0.2");
			svgWriter.Write(c,sw,"fill-rule:evenodd;","fill:yellow;stroke:green;stroke-width:2;fill-opacity:0.5;stroke-dasharray:2,2");
			sw.WriteLine("</svg>");
			sw.Close();
		}

		/// <summary>
		/// Writes two geometries to an svg file
		/// </summary>
		/// <param name="filename">The path of the svg file.</param>
		/// <param name="a">The A geometry</param>
		/// <param name="b">The B geometry</param>
		public void DisplayTest(string filename, Geometry a, Geometry b)
		{
			Geotools.Geometries.PrecisionModel pm = new Geotools.Geometries.PrecisionModel(1, 0, 0);
			GeometryFactory fact = new GeometryFactory(pm, 0);
			GeometrySVGWriter svgWriter = new GeometrySVGWriter(fact.PrecisionModel);
			StreamWriter sw = new StreamWriter(filename);
			GeometryCollection geomCollection= fact.CreateGeometryCollection(new Geometry[]{a,b});
			double minx, miny, maxx, maxy;
			geomCollection.Extent2D(out minx, out miny, out maxx, out maxy);
			sw.WriteLine(String.Format("<svg viewBox=\"{0} {1} {2} {3}\">",minx,miny,maxx,maxy*1.2));
			svgWriter.Write(a,sw,"fill-rule:evenodd;","fill:blue;stroke:blue;stroke-width:1;fill-opacity:0.2");
			svgWriter.Write(b,sw,"fill-rule:evenodd;","fill:red;stroke:red;stroke-width:1;fill-opacity:0.2");
			sw.WriteLine("</svg>");
			sw.Close();
		}

		#endregion Methods
	}

}
