using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{     
    /// <summary>
    /// The response Type - YES/NO |  A/B | Rate 1-10 | Multiple select
    /// </summary>
    public enum AAFResponseType { YesNo, AB, RateTo10, MultipleSelect,None }

    /// <summary>
    /// 
    /// </summary>he
    public partial class AskAFriend
    {
        public ResourceFile DefaultImage { get; set; }

        /// <summary>
        /// Delete the AskAFriend question identified by <paramref name="webAskQuestionID"/>
        /// </summary>
        /// <param name="webAskQuestionID">The WebAskAFriendId of the AskAFriend question</param>
        public static void Delete(String webAskQuestionID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_DeleteAskQuestion");
            db.AddInParameter(dbCommand, "@id", DbType.Int32, AskAFriend.GetAskAFriendByWebAskAFriendID(webAskQuestionID).AskAFriendID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Saves the reponse to the AskAFriend
        /// </summary>
        /// <param name="memberID">The MemberID of the Member responding to the AskAFriend Question</param>
        /// <param name="questionID">The AskAFriendID of the AskAFriend Question being responded</param>
        /// <param name="result"></param>
        public static void VoteForAskQuestion(Int32 memberID, Int32 questionID, Int32 result)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_VoteForAskQuestion");
            db.AddInParameter(dbCommand, "memberID", DbType.Int32, memberID);
            db.AddInParameter(dbCommand, "askAFriendID", DbType.Int32, questionID);
            db.AddInParameter(dbCommand, "result", DbType.Int32, result);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get the AskAFriend question by <paramref name="questionID"/>
        /// </summary>
        /// <param name="questionID">The AskAFriendID of the AskAFriend Question</param>
        /// <returns>An AskAFriend object</returns>
        public static AskAFriend GetAskQuestion(Int32 questionID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetAskQuestion");
            db.AddInParameter(dbCommand, "questionID", DbType.Int32, questionID);

            List<AskAFriend> questions = null;

            using (var dr = db.ExecuteReader(dbCommand))
            {
                questions = AskAFriend.PopulateObject(dr);
                dr.Close();
            }

            if (questions.Count > 0)
                return questions[0];
            else
                return null;
        }

        /// <summary>
        /// Get AskAFriend IDs corresponding to AskAFriend questions of a Member
        /// </summary>
        /// <param name="memberID">The MemberID of the Member</param>
        /// <returns>An Array of AskAFriend IDs</returns>
        public static Int32[] GetAskQuestionIDs(Int32 memberID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetAskQuestionIDs");
            db.AddInParameter(dbCommand, "memberID", DbType.Int32, memberID);

            var questionIDs = new List<Int32>();

            /* Execute the stored procedure. */
            using (var dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                    questionIDs.Add(Convert.ToInt32(dr["AskAFriendID"]));
                dr.Close();
            }

            return questionIDs.ToArray();
        }

        
        /// <summary>
        /// Get AskAFriend corresponding to <paramref name="WebAskAFriendID"/>
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="WebAskAFriendID">WebAskAFriendID of the AskAFriend Question</param>
        /// <returns>An AskAFriend Object</returns>
        public static AskAFriend GetAskAFriendByWebAskAFriendID(int MemberID, string WebAskAFriendID)
        {
            return GetAskAFriendByWebAskAFriendID(WebAskAFriendID);
        }

        /// <summary>
        /// Gets the AskAFriend object identified by the WebAskAFriendID
        /// </summary>
        /// <param name="WebAskAFriendID">The WebAskAFriendID against which the AskAFriend is retrieved</param>
        /// <returns>A single AskAFriend object</returns>
        public static AskAFriend GetAskAFriendByWebAskAFriendID(string WebAskAFriendID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAskAFriendByWebAskAFriendID");
            db.AddInParameter(dbCommand, "WebAskAFriendID", DbType.String, WebAskAFriendID);

            // Create the object array from the datareader
            List<AskAFriend> arr = new List<AskAFriend>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                throw new ArgumentException(String.Format(Properties.Resources.Argument_InvalidWebAskID, WebAskAFriendID));
        }

        
        /// <summary>
        /// Gets AskAFriend Questions that were created after the AskAFriend specified by LastWebAskAFriendID
        /// </summary>
        /// <param name="MemberID">The MemberID</param>
        /// <param name="LastWebAskAFriendID">The WebAskAFriendID</param>
        /// <returns>A list of AskAFriend object</returns>
        public static List<AskAFriend> GetAAFQuestionByMemberIDSinceLastIDWithJoin(int MemberID, string LastWebAskAFriendID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAAFQuestionByMemberIDSinceLastIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "WebAskAFriendID", DbType.String, LastWebAskAFriendID);

            List<AskAFriend> arr = new List<AskAFriend>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            return arr;
        }

        /// <summary>
        /// Gets a random AskAFriend Question
        /// </summary>
        /// <returns>A single AskAFriend object</returns>
        public static AskAFriend GetNextQuestion()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNextAAFQuestion");
            db.AddInParameter(dbCommand, "CategoryID", DbType.String, string.Empty);

            // Create the object array from the datareader
            List<AskAFriend> arr = new List<AskAFriend>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }


            if (arr.Count == 0)
                throw new InvalidOperationException(Properties.Resources.InvalidOperation_NoAskQuestions);

            return arr[0];
        }

        
        /// <summary>
        /// Gets the AskAFriend Question posted by a specific member
        /// </summary>
        /// <param name="MemberID">The MemberID</param>
        /// <param name="OrderBy">The order by parmeter. Possible values are "voted" and "recent"</param>
        /// <returns>A list of AskAFriend objects for the specified with the ordering according to OrderBy parameter</returns>
        public static AskAFriend[] GetAAFQuestionsByMemberIDWithJoin(int MemberID, string OrderBy)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAAFQuestionsByMemberIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int16, MemberID);
            db.AddInParameter(dbCommand, "OrderBy", DbType.String, OrderBy);

            List<AskAFriend> arr = new List<AskAFriend>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateAskAFriendWithJoin(dr);
                dr.Close();
            }

            return arr.ToArray();
        }


        /// <summary>
        /// Getsxz Top AskAFriend Questions sorted by number of votes
        /// </summary>
        /// <param name="Page">The page number to fetch</param>
        /// <param name="PageSize">The page size</param>
        /// <returns>An array of AskAFriend objects</returns>
        public static AskAFriend[] GetTopAAFQuestions(int Page, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetTopAAFQuestions");
            db.AddInParameter(dbCommand, "Page", DbType.Int32, Page);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);

            List<AskAFriend> arr = new List<AskAFriend>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateAskAFriendWithJoin(dr);
                dr.Close();
            }

            return arr.ToArray();
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of AskAFriend. This function should be used internally only.
        /// </summary>
        public static List<AskAFriend> PopulateAskAFriendWithJoin(IDataReader dr)
        {
            List<AskAFriend> arr = new List<AskAFriend>();
            ColumnFieldList list = new ColumnFieldList(dr);

            AskAFriend AAF;

            while (dr.Read())
            {
                AAF = new AskAFriend();
                AAF._askAFriendID = (int)dr["AskAFriendID"];
                AAF._memberID = (int)dr["MemberID"];
                AAF._webAskAFriendID = (string)dr["WebAskAFriendID"];
                AAF._numberOfPhotos = (int)dr["NumberOfPhotos"];
                AAF._question = (string)dr["Question"];
                AAF._responseType = (int)dr["ResponseType"];
                AAF._duration = (int)dr["Duration"];
                AAF._responseA = (string)dr["ResponseA"];
                AAF._responseB = (string)dr["ResponseB"];
                AAF._rejectScore = (int)dr["RejectScore"];
                AAF._isPrivate = (bool)dr["IsPrivate"];
                AAF._totalVotes = (int)dr["TotalVotes"];
                AAF._active = (bool)dr["Active"];
                AAF._wentLiveDT = (DateTime)dr["WentLiveDT"];
                AAF._submittedDT = (DateTime)dr["SubmittedDT"];

                AAF.DefaultImage = new ResourceFile();

                AAF.DefaultImage.ResourceFileID = (int)dr["ResourceFileID"];
                AAF.DefaultImage.WebResourceFileID = (string)dr["WebResourceFileID"];
                AAF.DefaultImage.ResourceType = (int)dr["ResourceType"];
                AAF.DefaultImage.StorageLocation = (int)dr["StorageLocation"];
                AAF.DefaultImage.Server = (int)dr["Server"];
                AAF.DefaultImage.Path = (string)dr["Path"];
                AAF.DefaultImage.FileName = (string)dr["FileName"];
                AAF.DefaultImage.CreatedDT = (DateTime)dr["CreatedDT"];

                AAF.Member = new Member();
                if (list.IsColumnPresent("Nickname")) { AAF.Member.NickName = (string)dr["Nickname"]; }

                arr.Add(AAF);
            }

            dr.Close();

            return arr;
        }


        /// <summary>
        /// Gets the results of the specified AskAFriendQuestion. The results are dependant on the type of the question.
        /// </summary>
        /// <param name="AAF">The AskAFriend object for which the results will be fetched</param>
        /// <returns>A varying length integer array that contains responses to different options.</returns>
        public static List<int> GetAskAFriendResult(AskAFriend AAF)
        {
            List<int> Results = new List<int>();

            if (AAF == null)
                Next2Friends.Data.Trace.Tracer("caskafriend.cs 215 AAF=null", "AAF", "OST");

            int AskAFriendID = AAF.AskAFriendID;
            AskResponseType ResponseType = (AskResponseType)AAF.ResponseType;

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAskAFriendResult");
            db.AddInParameter(dbCommand, "AskAFriendID", DbType.Int32, AskAFriendID);


            Next2Friends.Data.Trace.Tracer("new AAF", "AAF", "OST");
            try
            {
                //execute the stored procedure
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    int ArraySize = 0;

                    if (ResponseType == AskResponseType.RateTo10)
                    {
                        ArraySize = 10;
                    }
                    else
                    {
                        ArraySize = 2;
                    }

                    for (int i = 0; i < ArraySize; i++)
                    {
                        Results.Add(0);
                    }

                    while (dr.Read())
                    {
                        ///////////////

                        if (dr.GetInt32(2) == 1)
                        {
                            Results[0] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 2)
                        {
                            Results[1] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 3)
                        {
                            Results[2] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 4)
                        {
                            Results[3] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 5)
                        {
                            Results[4] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 6)
                        {
                            Results[5] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 7)
                        {
                            Results[6] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 8)
                        {
                            Results[7] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 9)
                        {
                            Results[8] = dr.GetInt32(1);
                        }
                        else if (dr.GetInt32(2) == 10)
                        {
                            Results[9] = dr.GetInt32(1);
                        }
                    }

                    dr.Close();
                }
            }
            catch
            {

                Next2Friends.Data.Trace.Tracer("Ask a friend cs new catch 299", "AAF", "OST");
            }

            //  if (Results == null)


            return Results;
        }


        /// <summary>
        /// Skips the current AskAFriend Question and marks it has skipped in the DB
        /// </summary>
        public void SkipAskQuestion()
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_SkipAAFQuestion");
            db.AddInParameter(dbCommand, "AskAFriendID", DbType.Int32, this.AskAFriendID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Skips the AskAFriend Question and marks it has skipped in the DB
        /// </summary>
        /// <param name="AskAFriendID">The AskAFriendID of the Question</param>
        public static void SkipAskQuestion(int AskAFriendID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_SkipAAFQuestion");
            db.AddInParameter(dbCommand, "AskAFriendID", DbType.Int32, AskAFriendID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Gets AskAFriend from the database With a full join with all the manually specified tables in the HG_GetAskAFriendByWebAskAFriendIDWithJoin SP code 
        /// </summary>
        /// <param name="WebAskAFriendID">The WebAskAFriendID of the AskAFriend</param>
        /// <returns>AskAFriend identified by the WebAskAFriendID or null if not found</returns>
        public static AskAFriend GetAskAFriendByAskAFriendIDWithJoin(string WebAskAFriendID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAskAFriendByWebAskAFriendIDWithJoin");
            db.AddInParameter(dbCommand, "WebAskAFriendID", DbType.String, WebAskAFriendID);

            List<AskAFriend> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObjectWithJoin(dr);
                dr.Close();

            }

            if (arr[0] != null)
                return arr[0];
            else
                return null;
        }


    }
}
