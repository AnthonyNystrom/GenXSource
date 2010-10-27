using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ComponentModel;
using Next2Friends.Data;
using System.Globalization;
using System.Diagnostics;
using Next2Friends.Misc;
using Next2Friends.WebServices.Fix;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Adjusts fixes to Next2Friends database or file structure.
    /// </summary>
    [WebService(
        Description = "Adjusts fixes to Next2Friends database or file structure.",
        Name = "FixService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class FixService : WebService
    {
        public const String Identifier = "FixService";
        private const Int32 _defaultPhotoResourceFileId = 1;

        [WebMethod]
        public void FixProfilePhotos()
        {
            using (var dr = Member.GetAllMemberReader())
            {
                while (dr.Read())
                {
                    var memberId = Convert.ToInt32(dr["MemberID"], CultureInfo.InvariantCulture);
                    var profilePhotoId = Convert.ToInt32(dr["ProfilePhotoResourceFileID"], CultureInfo.InvariantCulture);

                    if (profilePhotoId == _defaultPhotoResourceFileId)
                        continue;

                    var resourceFile = new ResourceFile(profilePhotoId);
                    FixProfilePhoto(memberId, resourceFile.Path, resourceFile.FileName);
                }

                dr.Close();
            }
        }

        [WebMethod]
        public void FixMemberProfile()
        {
            var ids = new[] { 
                4895, 4896, 4897, 4899,
                4900, 4901,
                4911, 4916, 4919,
                4921, 4924, 4925, 4926, 4928,
                4933, 4934, 4935, 4939 };

            foreach (var id in ids)
            {
                var profile = new MemberProfile()
                {
                    MemberID = id
                };
                profile.Save();

                Log.Logger(String.Format("MemberProfile added for Member = {0}", id), Identifier);
            }
        }

        private void FixProfilePhoto(Int32 memberId, String path, String fileName)
        {
            Debug.Assert(!String.IsNullOrEmpty(fileName), "!String.IsNullOrEmpty(fileName)");

            using (var thumbnail = ResourceService.GetThumbnailImage(path, fileName))
            {
                if (thumbnail.Width != 102 || thumbnail.Height != 102)
                {
                    using (var mediumImage = ResourceService.GetMediumImage(path, fileName))
                    {
                        Log.Logger(String.Format("Processing Member = {0}", memberId), Identifier);

                        Data.Photo.ProcessProfilePhoto(
                            memberId
                            , ResourceService.GetNickname(path)
                            , mediumImage);
                    }
                }
            }
        }
    }
}
