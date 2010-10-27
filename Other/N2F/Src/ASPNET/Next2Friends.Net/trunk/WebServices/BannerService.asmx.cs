using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.Services;
using Next2Friends.Data;
using Next2Friends.WebServices.Banner;
using data = Next2Friends.Data;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to retrieve advert data.
    /// </summary>
    [WebService(
        Description = "Provides functionality to retrieve advert data.",
        Name = "BannerService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public sealed class BannerService : WebService
    {
        public const String Identifier = "BannerService";

        /// <summary>
        /// Call this method to get next banner.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty String, or
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Call this method to get next banner.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty String, or<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public BannerResponse GetNextBanner(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            BannerResponse response = null;

            try
            {
                var banner = data.Banner.GetNextBanner(BannerType.MobileBanner, "Next2Friends Mobile");

                response = new BannerResponse()
                {
                    Base64BannerBinary = data.Photo.ImageToBase64String(
                        new Bitmap(@"\\www\live" + banner.FileLocation)),
                    BannerURL = @"http://www.next2friends.com/ad/" + banner.WebBannerID
                };
            }
            catch (Exception e)
            {
                throw e;
            }

            return response;
        }
    }
}