/* ------------------------------------------------
 * VerificationMessages.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Next2Friends.CrossPoster.Client
{
    static class VerificationMessages
    {
        public static readonly String InvalidBlogAddress = "Please, specify the XML-RPC interface address.";
        public static readonly String InvalidBlogName = "Please, specify the name for the blog so you could recognize different accounts.";
        public static readonly String InvalidUsername = "Please, specify the username that will be used to post to the blog.";
        public static readonly String InvalidPassword = "Please, specify the password that will be used to post to the blog.";
    }
}
