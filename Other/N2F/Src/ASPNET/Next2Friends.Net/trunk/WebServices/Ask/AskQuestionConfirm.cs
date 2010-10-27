using System;

namespace Next2Friends.WebServices.Ask
{
    public sealed class AskQuestionConfirm
    {
        /// <summary>
        /// The submission URL if the user clicks on the image
        /// </summary>
        public String AdvertURL { get; set; }

        /// <summary>
        /// The advert image array as a JPEG byte stream
        /// </summary>
        public String AdvertImage { get; set; }

        /// <summary>
        /// The 128 bit ID of the Question
        /// </summary>
        public String AskQuestionID { get; set; }
    }
}
