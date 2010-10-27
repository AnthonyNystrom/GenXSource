using System;

namespace Next2Friends.WebServices.Video
{
    public sealed class PopupAd
    {
        /// <summary>
        /// The point in milliseconds where the popup will occur
        /// </summary>
        public int Timecode { get; set; }

        /// <summary>
        /// The duration in miliseconds of the popup
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// The URL to fetch the advert image from.
        /// </summary>
        public String ImageURL { get; set; }

        /// <summary>
        /// Link to clickthrough if the user clicks on the link.
        /// </summary>
        public String ClickThroughURL { get; set; }

        /// <summary>
        /// Should the popup be displayed (if false the rest of the parameters will be blank and the advert will not show).
        /// </summary>
        public Boolean Show { get; set; }

    }
}
