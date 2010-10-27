/* ------------------------------------------------
 * AddressInfo.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using Next2Friends.WebServices.GeoMessage.Data;
using System.Data;

namespace Next2Friends.WebServices.GeoMessage
{
    /// <summary>
    /// Holds textual address information about a location. Typically the information is e.g. street address.
    /// If the value of a field is not available, it is set to <code>null</code>.
    /// The names of the fields use terms and definitions that are commonly used e.g. in the United States.
    /// http://www.forum.nokia.com/document/Java_ME_Developers_Library_v2/GUID-4AEC8DAF-DDCC-4A30-B820-23F2BA60EA52/javax/microedition/location/AddressInfo.html
    /// </summary>
    public sealed class AddressInfo
    {
        /// <summary>
        /// Creates a new instance of the <c>AddressInfo</c> class.
        /// </summary>
        public AddressInfo()
        {
        }

        /// <summary>
        /// Gets or sets the address extension, e.g. flat number.
        /// <example>
        /// <code>American Example: Flat 5</code>
        /// -or-
        /// <code>British Example: The Oaks</code>
        /// </example>
        /// </summary>
        public String Extension { get; set; }

        /// <summary>
        /// Gets or sets the street name and number.
        /// <example>
        /// <code>American Example: 10 Washington Street</code>
        /// -or-
        /// <code>British Example: 20 Greenford Court</code>
        /// </example>
        /// </summary>
        public String Street { get; set; }

        /// <summary>
        /// Gets or sets the zip or postal code.
        /// <example>
        /// <code>American Example: 12345</code>
        /// -or-
        /// <code>British Example: AB1 9YZ</code>
        /// </example>
        /// </summary>
        public String PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the town or city name.
        /// <example>
        /// <code>American Example: Palo Alto</code>
        /// -or-
        /// <code>British Example: Cambridge</code>
        /// </example>
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// Gets or sets a county, which is an entity between a state and a city.
        /// <example>
        /// <code>American Example: Santa Clara County</code>
        /// -or-
        /// <code>British Example: Cambridgeshire</code>
        /// </example>
        /// </summary>
        public String County { get; set; }

        /// <summary>
        /// Gets or sets the state or province.
        /// <example>
        /// <code>American Example: California</code>
        /// -or-
        /// <code>British Example: England</code>
        /// </example>
        /// </summary>
        public String State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// <example>
        /// <code>American Example: United States of America</code>
        /// -or-
        /// <code>British Example: United Kingdom</code>
        /// </example>
        /// </summary>
        public String Country { get; set; }

        /// <summary>
        /// Gets or sets the country as a two-letter ISO 3166-1 code.
        /// <example>
        /// <code>American Example: US</code>
        /// -or-
        /// <code>British Example: GB</code>
        /// </example>
        /// </summary>
        public String CountryCode { get; set; }

        /// <summary>
        /// Gets or sets a municipal district.
        /// </summary>
        public String District { get; set; }

        /// <summary>
        /// Gets or sets a building name.
        /// </summary>
        public String BuildingName { get; set; }

        /// <summary>
        /// Gets or sets a building floor.
        /// </summary>
        public String BuildingFloor { get; set; }

        /// <summary>
        /// Gets or sets a building room.
        /// </summary>
        public String BuildingRoom { get; set; }

        /// <summary>
        /// Gets or sets a building zone.
        /// </summary>
        public String BuildingZone { get; set; }

        /// <summary>
        /// Gets or sets a street in a crossing.
        /// </summary>
        public String Crossing1 { get; set; }

        /// <summary>
        /// Gets or sets a street in a crossing.
        /// </summary>
        public String Crossing2 { get; set; }

        /// <summary>
        /// Gets or sets a URL for this place.
        /// <example>
        /// <code>American Example: http://www.americanurl.com </code>
        /// -or-
        /// <code>British Example: http://britishurl.co.uk </code>
        /// </example>
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// Gets or sets a phone number for this place.
        /// </summary>
        public String PhoneNumber { get; set; }
    }
}
