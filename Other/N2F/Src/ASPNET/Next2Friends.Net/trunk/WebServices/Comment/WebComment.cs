/* ------------------------------------------------
 * WebComment.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Next2Friends.WebServices.Comment
{
    public sealed class WebComment
    {
        public Int32 ID { get; set; }
        public Int32 ObjectID { get; set; }
        public String Nickname { get; set; }
        public String Text { get; set; }
        public String DTCreated { get; set; }
        public Int32 InReplyToCommentID { get; set; }
        public Int32 ParentCommentID { get; set; }
        public Int32 CommentType { get; set; }
    }
}
