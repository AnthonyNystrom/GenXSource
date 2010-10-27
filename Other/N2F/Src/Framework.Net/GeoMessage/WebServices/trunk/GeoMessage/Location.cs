/* ------------------------------------------------
 * LocationDescriptor.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.GeoMessage
{
    /// <summary>
    /// Represents the standard set of basic location information. This includes the timestamped coordinates, accuracy, speed, course,
    /// and information about the positioning method used for the location, plus an optional textual address.
    /// </summary>
    public sealed class Location
    {
        /// <summary>
        /// Creates a new instance of the <c>Location</c> class.
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        /// Gets or sets the current address associated data.
        /// </summary>
        public AddressInfo AddressInfo { get; set; }

        /// <summary>
        /// Gets or sets the course made good in degrees relative to true north. The value is always in the range [0.0,360.0) degrees.
        /// </summary>
        public Single Course { get; set; }

        /// <summary>
        /// Gets or sets extra information about the location.
        /// </summary>
        public String ExtraInfo { get; set; }

        /// <summary>
        /// Gets or sets information about the location method used. The returned value is a bitwise combination (OR) of the method technology,
        /// method type and assistance information. The method technology values are defined as <c>MTE_*</c> in the <c>LocationMethod</c>
        /// enumeration, the method type values are named <c>MTY_*</c> and assistance information values are named <c>MTA_*</c>.
        /// For example, if the location method used is terminal based, network assisted E-OTD, the value 0x00050002
        /// <c>( = MTY_TERMINALBASED | MTA_ASSISTED | MTE_TIMEDIFFERENCE)</c> would be returned.
        /// If the location is determined by combining several location technologies, the returned value may have several
        /// <c>MTE_*</c> bits set.
        /// If the used location method is unknown, the returned value must have all the bits set to zero.
        /// </summary>
        public Int32 LocationMethod { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of this location and their accuracy.
        /// </summary>
        public QualifiedCoordinates QualifiedCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the current ground speed in meters per second (m/s) at the time of measurement.
        /// The speed is always a non-negative value. Note that unlike the coordinates, speed does not have an
        /// associated accuracy because the methods used to determine the speed typically are not able
        /// to indicate the accuracy.
        /// </summary>
        public Single Speed { get; set; }

        /// <summary>
        /// Gets or sets the time stamp at which the data was collected. This timestamp should represent the point
        /// in time when the measurements were made and should be in ticks that are standard in the
        /// Microsoft .NET Framework.
        /// </summary>
        public Int64 TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether this Location instance represents a valid location with coordinates
        /// or an invalid one where all the data, especially the latitude and longitude coordinates, may not be present.
        /// A valid Location object contains valid coordinates whereas an invalid Location object may not contain
        /// valid coordinates but may contain <c>ExtraInfo</c> to provide information on why it was not possible
        /// to provide a valid Location object.
        /// </summary>
        public Boolean IsValid { get; set; }
    }
}
