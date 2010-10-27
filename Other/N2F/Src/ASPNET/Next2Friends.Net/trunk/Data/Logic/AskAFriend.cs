using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public enum AskResponseType { YesNo, AB, RateTo10, MultipleSelect, None }

    public partial class AskAFriend
    {
        private List<AskAFriendPhoto> _Photo;

        public List<AskAFriendPhoto> Photo
        {
            get
            {
                //if (_Photo == null)
                //{
                _Photo = Next2Friends.Data.AskAFriendPhoto.GetAskAFriendPhotoByAskAFriendIDWithJoin(this.AskAFriendID);
                //}


                return _Photo;

            }
        }

        public void SaveImages()
        {
            for (int i = 0; i < Photo.Count; i++)
            {
                Photo[i].AskAFriendID = this.AskAFriendID;
                Photo[i].Save();
            }
        }

        public static bool IsVoteValueAllowed(AskAFriend AAF, int QuestionResponseValue)
        {
            bool CanVote = false;

            // make sure the at the right value was passed to the server. the back button may have been used and the user
            // may send the a value to the next question that isnt allowed. for example '10' when the question can only be 1 or 2
            if (AAF.ResponseType == (int)AskResponseType.AB || AAF.ResponseType == (int)AskResponseType.AB)
            {
                if (QuestionResponseValue == 1 || QuestionResponseValue == 2)
                {
                    CanVote = true;
                }
            }
            else if (AAF.ResponseType == (int)AskResponseType.MultipleSelect)
            {
                if (QuestionResponseValue > 0 && (QuestionResponseValue < AAF.NumberOfPhotos + 1))
                {
                    CanVote = true;
                }
            }
            else if (AAF.ResponseType == (int)AskResponseType.RateTo10)
            {
                if (QuestionResponseValue > 0 && QuestionResponseValue < 11)
                {
                    CanVote = true;
                }
            }

            return CanVote;
        }


    }
}
