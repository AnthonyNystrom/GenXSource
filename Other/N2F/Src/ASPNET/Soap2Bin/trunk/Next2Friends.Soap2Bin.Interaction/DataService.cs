using System;
using System.Runtime.InteropServices;
using Next2Friends.Soap2Bin.Core;
using Next2Friends.Soap2Bin.Interaction.AskService;
using Next2Friends.Soap2Bin.Interaction.DashboardService;
using Next2Friends.Soap2Bin.Interaction.TagService;
using Next2Friends.Soap2Bin.Interaction.BlogService;
using Next2Friends.Soap2Bin.Interaction.CommentService;
using Next2Friends.Soap2Bin.Interaction.PhotoService;

namespace Next2Friends.Soap2Bin.Interaction
{
    static class DataService
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct Byte2SByteMap
        {
            [FieldOffset(0)]
            public Byte[] ByteArray;
            [FieldOffset(0)]
            public SByte[] SByteArray;
        }

        private static Byte2SByteMap _byte2SByteMap = new Byte2SByteMap();

        public static WebComment ReadWebComment(this DataInputStream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            return new WebComment()
            {
                ID = input.ReadInt32(),
                ObjectID = input.ReadInt32(),
                Nickname = input.ReadString(),
                Text = input.ReadString(),
                DTCreated = input.ReadDateTime().Ticks.ToString(),
                InReplyToCommentID = input.ReadInt32(),
                ParentCommentID = input.ReadInt32(),
                CommentType = input.ReadInt32()
            };
        }

        public static TagUpdate ReadTagUpdate(this DataInputStream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            return new TagUpdate()
            {
                DeviceTagID = input.ReadStringArray(),
                TagValidationString = input.ReadStringArray()
            };
        }

        public static String[] ReadStringArray(this DataInputStream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            var length = input.ReadInt32();
            String[] result = new String[length];
            for (var i = 0; i < length; i++)
                result[i] = input.ReadString();
            return result;
        }

        public static Byte[] ReadByteArray(this DataInputStream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            var length = input.ReadInt32();
            SByte[] result = new SByte[length];
            for (var i = 0; i < length; i++)
                result[i] = (SByte)input.ReadByte();
            _byte2SByteMap.SByteArray = result;
            return _byte2SByteMap.ByteArray;
        }

        public static void WriteInt32Array(this DataOutputStream output, Int32[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
                output.WriteInt32(item);
        }

        public static void WriteStringArray(this DataOutputStream output, String[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
                output.WriteString(item);
        }

        public static void WriteByteArray(this DataOutputStream output, Byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            _byte2SByteMap.ByteArray = value;
            foreach (var item in _byte2SByteMap.SByteArray)
                output.WriteByte(item);
        }

        public static void WriteGetEntryResult(this DataOutputStream output, BlogEntry value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.ID);
            output.WriteString(value.Title);
            output.WriteString(value.Body);
            output.WriteDateTime(value.DTCreated.ToDateTime());
        }

        public static void WriteGetItemsResult(this DataOutputStream output, DashboardItem[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
            {
                output.WriteString(item.AskQuestion);
                output.WriteDateTime(item.DateTime.ToDateTime());
                output.WriteInt32(item.FeedItemType);
                output.WriteString(item.Friend1FullName);
                output.WriteString(item.Friend2FullName);
                output.WriteString(item.FriendNickname1);
                output.WriteString(item.FriendNickname2);
                output.WriteString(item.FriendWebMemberID1);
                output.WriteString(item.FriendWebMemberID2);
                output.WriteString(item.Text);
                output.WriteString(item.ThumbnailUrl);
                output.WriteString(item.Title);
                output.WriteInt32(item.ObjectID);
                output.WriteString(item.WebPhotoCollectionID);
                output.WriteString(item.WebPhotoID);
            }
        }

        public static void WriteGetCommentResult(this DataOutputStream output, WebComment value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.ID);
            output.WriteInt32(value.ObjectID);
            output.WriteString(value.Nickname);
            output.WriteString(value.Text);
            output.WriteDateTime(value.DTCreated.ToDateTime());
            output.WriteInt32(value.InReplyToCommentID);
            output.WriteInt32(value.ParentCommentID);
            output.WriteInt32(value.CommentType);
        }

        public static void WriteGetQuestionResult(this DataOutputStream output, AskQuestion value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.ID);
            output.WriteString(value.Question);
            output.WriteDateTime(value.DTCreated.ToDateTime());
        }

        public static void WriteGetResponseResult(this DataOutputStream output, AskResponse value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.AskQuestionID);
            output.WriteString(value.Question);
            output.WriteByteArray(Convert.FromBase64String(value.PhotoBase64Binary));
            output.WriteInt32Array(value.ResponseValues);
            output.WriteFloat(Convert.ToSingle(value.Average));
            output.WriteInt32(value.ResponseType);
            output.WriteStringArray(value.CustomResponses);
        }

        public static void WriteSubmitQuestionResult(this DataOutputStream output, AskQuestionConfirm value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteString(value.AdvertURL);
            output.WriteString(value.AdvertImage);
            output.WriteString(value.AskQuestionID);
        }

        public static void WriteGetNewFriendsResult(this DataOutputStream output, DashboardNewFriend[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
            {
                output.WriteDateTime(item.DateTime.ToDateTime());
                output.WriteString(item.Nickname1);
                output.WriteString(item.Nickname2);
            }
        }

        public static void WriteGetPhotosResult(this DataOutputStream output, DashboardPhoto[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
            {
                output.WriteDateTime(item.DateTime.ToDateTime());
                output.WriteString(item.Nickname);
                output.WriteString(item.Text);
                output.WriteString(item.Title);
                output.WriteString(item.ThumbnailUrl);
            }
        }

        public static void WriteGetVideosResult(this DataOutputStream output, DashboardVideo[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
            {
                output.WriteDateTime(item.DateTime.ToDateTime());
                output.WriteString(item.Nickname);
                output.WriteString(item.Text);
                output.WriteString(item.Title);
                output.WriteString(item.ThumbnailUrl);
            }
        }

        public static void WriteGetWallCommentsResult(this DataOutputStream output, DashboardWallComment[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
            {
                output.WriteDateTime(item.DateTime.ToDateTime());
                output.WriteString(item.Nickname1);
                output.WriteString(item.Nickname2);
                output.WriteString(item.Text);
            }
        }

        public static void WriteUploadTagsResult(this DataOutputStream output, TagConfirmation[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
            {
                output.WriteString(item.DeviceID);
                output.WriteBoolean(item.ConfirmedByServer);
            }
        }

        public static void WriteGetCommentsResult(this DataOutputStream output, WebComment[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
                output.WriteGetCommentResult(item);
        }

        public static void WriteGetPhotosForCollectionResult(this DataOutputStream output, WebPhoto[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
            {
                output.WriteInt32(item.ID);
                output.WriteDateTime(item.CreatedDT.ToDateTime());
                output.WriteString(item.Title);
                output.WriteString(item.Description);
            }
        }

        public static void WriteGetPhotoThumbnailResult(this DataOutputStream output, String thumbnailBase64String)
        {
            if (thumbnailBase64String == null)
                throw new ArgumentNullException("thumbnailBase64String");
            output.WriteByteArray(Convert.FromBase64String(thumbnailBase64String));
        }

        public static void WriteGetQuestionsResult(this DataOutputStream output, AskQuestion[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            output.WriteInt32(value.Length);
            foreach (var item in value)
                output.WriteGetQuestionResult(item);
        }
    }
}
