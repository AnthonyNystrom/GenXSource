using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Next2Friends.Data;
using Next2Friends.Misc;

// temp
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using data = Next2Friends.Data;

namespace TestLaw
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceUploadPhoto("Lawrence", "saphire1", "", DateTime.Now.ToString());
        }

        public static void DeviceUploadPhoto(String nickname, String password, String base64StringPhoto, String dateTime)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(base64StringPhoto))
                throw new ArgumentNullException("base64StringPhoto");
            if (String.IsNullOrEmpty(dateTime))
                throw new ArgumentNullException("dateTime");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);

            PhotoCollection photoCollection = PhotoCollection.GetOrCreatePhotoGalleryForToday(member.MemberID);

            UploadPhoto(member, photoCollection.WebPhotoCollectionID, base64StringPhoto, dateTime);
        }

        private static void UploadPhoto(Member member, String photoCollectionID, String base64StringPhoto, String dateTime)
        {
            var byteImage = Convert.FromBase64String(base64StringPhoto);
            var longDateTime = Int64.Parse(dateTime);
            var takenDT = new DateTime(longDateTime);

            try
            {
                var img = data.Photo.ByteArrayToImage(byteImage);
            }
            catch
            {
                //throw new ArgumentException(Resources.Argument_InvalidBase64PhotoString);
            }


            var photoCollection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(photoCollectionID);

            if (photoCollection == null)
            {
                photoCollection = new PhotoCollection()
                {
                    DTCreated = DateTime.Now,
                    //Description = Resources.SnapUp_DefaultGalleryDescrpiption,
                    MemberID = member.MemberID,
                    //Name = Resources.SnapUp_DefaultGalleryName
                };
                photoCollection.Save();
            }

            data.Photo.ProcessMemberPhoto(member, photoCollection.PhotoCollectionID, byteImage, takenDT);
        }


    }
}
