/* ------------------------------------------------
 * LocationMethod.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using Next2Friends.WebServices.GeoMessage.Data;
using System.Data;

namespace Next2Friends.WebServices.GeoMessage
{
    /// <summary>
    /// Represents coordinates as latitude-longitude-altitude values that are associated with an accuracy value.
    /// </summary>
    public sealed class QualifiedCoordinates
    {
        /// <summary>
        /// Creates a new instance of the <c>QualifiedCoordinates</c> class.
        /// </summary>
        public QualifiedCoordinates()
        {
        }

        /// <summary>
        /// Gets or sets the altitude component of this coordinate. Altitude is defined to mean height above the WGS84 reference ellipsoid.
        /// <c>0.0</c> means a location at the ellipsoid surface, negative values mean the location is below the ellipsoid surface,
        /// <c>Single.NaN</c> that the altitude is not available.
        /// </summary>
        public Single Altitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude component of this coordinate. Positive values indicate northern latitude and negative values southern latitude.
        /// The latitude is given in WGS84 datum.
        /// </summary>
        public Double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of this coordinate. Positive values indicate eastern longitude and negative values western longitude.
        /// The longitude is given in WGS84 datum.
        /// </summary>
        public Double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the horizontal accuracy of the location in meters (1-sigma standard deviation).
        /// A value of <c>Single.NaN</c> means the horizontal accuracy could not be determined.
        /// </summary>
        public Single HorizontalAccuracy { get; set; }

        /// <summary>
        /// Gets or sets the accuracy of the location in meters in vertical direction (orthogonal to ellipsoid surface, 1-sigma standard deviation).
        /// A value of <c>Single.NaN</c> means the vertical accuracy could not be determined. Must be greater or equal to <c>0</c>.
        /// </summary>
        public Single VerticalAccuracy { get; set; }
    }
}
