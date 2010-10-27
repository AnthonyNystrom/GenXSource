/* ------------------------------------------------
 * Soap2BinHandler.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;
using Next2Friends.Soap2Bin.Interaction.AskService;
using Next2Friends.Soap2Bin.Interaction.BlogService;
using Next2Friends.Soap2Bin.Interaction.CommentService;
using Next2Friends.Soap2Bin.Interaction.DashboardService;
using Next2Friends.Soap2Bin.Interaction.MemberService;
using Next2Friends.Soap2Bin.Interaction.Properties;
using Next2Friends.Soap2Bin.Interaction.SnapUpService;
using Next2Friends.Soap2Bin.Interaction.TagService;
using Next2Friends.Soap2Bin.Interaction.PhotoService;

namespace Next2Friends.Soap2Bin.Interaction
{
    /// <summary>
    /// Processes requests from mobile clients and converts SOAP to binary format and vice versa.
    /// </summary>
    public class Soap2BinHandler : IHttpHandler
    {
        private static Dictionary<Int32, IGateway> _methods = new Dictionary<Int32, IGateway>();

        static Soap2BinHandler()
        {
            _methods.Add(1, new CheckUserExistsGateway());
            _methods.Add(2, new GetEncryptionKeyGateway());
            _methods.Add(3, new GetMemberIDGateway());
            _methods.Add(4, new GetTagIDGateway());
            _methods.Add(5, new RemindPasswordGateway());
            /* 6 is empty - Method removed. */
            _methods.Add(7, new AttachPhotoGateway());
            _methods.Add(8, new CompleteQuestionGateway());
            /* 9 is empty - Method removed. */
            /* 10 is empty - Method removed. */
            _methods.Add(11, new GetQuestionGateway());
            _methods.Add(12, new GetQuestionIDsGateway());
            _methods.Add(13, new GetResponseGateway());
            /* 14 is empty - Method removed. */
            _methods.Add(15, new SkipQuestionGateway());
            _methods.Add(16, new SubmitQuestionGateway());
            _methods.Add(17, new VoteForQuestionGateway());
            _methods.Add(18, new GetNewFriendsGateway());
            _methods.Add(19, new GetPhotosGateway());
            _methods.Add(20, new GetVideosGateway());
            _methods.Add(21, new GetWallCommentsGateway());
            _methods.Add(22, new DeviceUploadPhotoGateway());
            _methods.Add(23, new UploadTagsGateway());
            _methods.Add(24, new SetMemberStatusTextGateway());
            _methods.Add(25, new GetMemberStatusTextGateway());
            _methods.Add(26, new GetThumbnailGateway());
            _methods.Add(27, new GetItemsGateway());
            /* 28 is empty - Method removed. */
            /* 29 is empty - Method removed. */
            _methods.Add(30, new AddCommentGateway());
            _methods.Add(31, new GetCommentGateway());
            _methods.Add(32, new GetCommentIDsGateway());
            _methods.Add(33, new HasNewCommentsGateway());
            _methods.Add(34, new RemoveCommentGateway());
            _methods.Add(35, new EditCommentGateway());
            _methods.Add(36, new GetPhotosForCollectionGateway());
            _methods.Add(37, new GetEntryGateway());
            /* 38 is empty - Method removed. */
            _methods.Add(39, new GetQuestionsGateway());
            _methods.Add(40, new GetCommentsGateway());
            _methods.Add(41, new GetPhotoThumbnailGateway());
            _methods.Add(42, new CreateUserGateway());
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the System.Web.IHttpHandler instance.
        /// </summary>
        /// <returns>
        /// true if the System.Web.IHttpHandler instance is reusable; otherwise, false.
        /// </returns>
        public Boolean IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements
        /// the System.Web.IHttpHandler interface.
        /// </summary>
        /// <param name="context">
        /// An System.Web.HttpContext object that provides references to the intrinsic
        /// server objects (for example, Request, Response, Session, and Server) used
        /// to service HTTP requests.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;
            var request = context.Request;
            var requestType = request.RequestType;

            if (requestType.IsGet())
                ProcessGetRequest(request, response);
            else if (requestType.IsPost())
                ProcessPostRequest(request, response, context.Session);
        }

        private static void ProcessGetRequest(HttpRequest request, HttpResponse response)
        {
            response.ContentType = "text/html; charset=iso-8859-1";
            response.Write(Resources.Message_ServerIsActive);
        }

        private static void ProcessPostRequest(HttpRequest request, HttpResponse response, HttpSessionState session)
        {
            response.ContentType = "application/octet-stream";

            var memoryStream = new MemoryStream();
            var output = new DataOutputStream(memoryStream);

            try
            {
                var input = new DataInputStream(request.InputStream);
                var version = request.Headers["version"];
                if (version != null)
                {
                    if (!HttpProcessor.PROTOCOL_VERSION.Equals(version, StringComparison.Ordinal))
                        throw new IOException(String.Format(
                            CultureInfo.CurrentCulture,
                            Resources.IO_InvalidProtocolVersion,
                            version,
                            HttpProcessor.PROTOCOL_VERSION));
                }

                if (input.ReadInt16() == HttpProcessor.INVOCATION_CODE)
                    InvokeMethod(session, input.ReadInt32(), input, output);
                input.Close();
            }
            catch (Exception e)
            {
                if (output == null)
                    output = new DataOutputStream(response.OutputStream);

                output.WriteInt16(HttpProcessor.RESULT_EXCEPTION);
                Debug.WriteLine(e.StackTrace);
                output.WriteString(e.ToString());
            }

            response.SetContentLength(memoryStream.Length);

            try
            {  
                var data = new Byte[memoryStream.Length];
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.Read(data, 0, data.Length);
                response.OutputStream.Write(data, 0, data.Length);
            }
            finally
            {
                if (output != null)
                    output.Close();
                response.OutputStream.Close();
            }
        }

        private static void InvokeMethod(HttpSessionState session, Int32 requestID, DataInputStream input, DataOutputStream output)
        {
            _methods[requestID].Invoke(session, input);
            output.WriteInt16(HttpProcessor.RESULT_SUCCESSFUL);
            _methods[requestID].Return(output);
        }
    }
}
