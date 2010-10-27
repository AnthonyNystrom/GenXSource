using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    [Serializable]
    public class MobileAskAFriendResponse
    {
        public string WebAskAFriendID { get; set; }
        //public string Question { get; set; }
        //public string PhotoBase64Binary { get; set; }
        //public int[] ResponseValues { get; set; }
        //public double Average { get; set; }
        //public int ResponseType { get; set; }
        //public string[] CustomResponses { get; set; }


        public MobileAskAFriendResponse(AskAFriend AAF)
        {
            // empty for Serializable]#
            WebAskAFriendID = "";
        }

        public MobileAskAFriendResponse()
        {
            WebAskAFriendID = "";
        }

        //public MobileAskAFriendResponse(AskAFriend AAF)
        //{

        //    this.WebAskAFriendID = AAF.WebAskAFriendID;
        //    this.Question = AAF.Question;

        //    List<AskAFriendPhoto> photos = AskAFriendPhoto.GetAskAFriendPhotoByAskAFriendIDWithJoin(AAF.AskAFriendID);

        //    string path = @"c:\n2fweb\photo\" + photos[0].PhotoResourceFile.FileName;

        //    //Image img = new Bitmap(path);

        //   // this.PhotoBase64Binary = Photo.ImageToBase64String(img);

        //    this.ResponseValues = Next2Friends.Data.AskAFriend.GetAskAFriendResult(AAF).ToArray();
        //    //this.ResponseType = (int)AAF.ResponseType;

        //    // set the average value (only applicable if the resposnetype is Rate1To10)
        //    this.Average = 0;
            
        //    //for (int i = 0; i < ResponseValues.Length; i++)
        //    //{
        //    //    this.Average += ResponseValues[i];
        //    //}

        //    // get the average value
        //    //Average = Average / Convert.ToDouble(ResponseValues.Length);

        //    // Custom responses will be blank strings if not applicable
        //    //CustomResponses = new string[2];
        //    //CustomResponses[0] = AAF.ResponseA;
        //    //CustomResponses[1] = AAF.ResponseB;

            
        //}
    }
}
