using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Next2Friends.Data;
using Next2Friends.Misc;

public partial class AskAFriendPage : System.Web.UI.Page
{
    public AjaxAskAFriend InitialAAF;

    public bool IsPermalink = false;
    public string AskAFriendWebId = "none";

    public bool IsLoggedIn
    {
        get
        {
            member = (Member)Session["Member"];

            if (member == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
    public Member member = null;
    public string LoginUrl;
    private string ServerURL = ASP.global_asax.WebServerRoot + "ask/";
    public string ReturnToDash = "false";

    protected void Page_Load(object sender, EventArgs e)
    {
        LoginUrl = @"signup.aspx?u=" + Request.Url.AbsoluteUri;

        AjaxPro.Utility.RegisterTypeForAjax(typeof(AskAFriendPage));
        Comments1.CommentType = CommentType.AskAFriend;

        string Return = Request.Params["return"];

        if (Return != null)
        {
            ReturnToDash = "true";
        }

        //int t=0;

        //if(Session["t"]!=null)
        //{
        //    t=(int)Session["t"];
        //    t++;
        //    Session["t"]=t;
        //}
        //else
        //{
        //    Session["t"]=0;
        //}

        //lblT.Text= t.ToString();

        try
        {

            //string strAAFQuestion = Request.Params["q"];

            AskAFriend askAFriend = null;

            askAFriend = ExtractPageParams.ExtractAskAFriendFromURL(this.Page, this.Context);


            if (askAFriend != null)
            {
                CurrentAskAFriend = askAFriend;

                LastAskAFriend = CurrentAskAFriend;

                IsPermalink = true;

                InitialAAF = GenarateAjaxAskAFriend();
                AskAFriendWebId = InitialAAF.WebAskAFriendID;
                Comments1.ObjectWebId = AskAFriendWebId;
                return;
            }
        }
        catch
        {
            return;
        }

        InitialAAF = NextQuestion();

        AskAFriendWebId = CurrentAskAFriend.WebAskAFriendID;

        Comments1.ObjectId = CurrentAskAFriend.AskAFriendID;
        Comments1.ObjectWebId = CurrentAskAFriend.WebAskAFriendID;

    }


    public AskAFriend CurrentAskAFriend
    {
        get
        {
            return (AskAFriend)Session["AskAFriend"];
        }
        set
        {
            Session["AskAFriend"] = value;
        }
    }

    public AskAFriend LastAskAFriend
    {
        get
        {
            return (AskAFriend)Session["LastAskAFriend"];
        }
        set
        {
            Session["LastAskAFriend"] = value;
        }

    }

    /// <summary>
    /// 
    /// Genarate new question
    /// </summary>
    /// <returns>AjaxAskAFriend should return a new AjaxAskAFriend Object</returns>

    [AjaxPro.AjaxMethod]
    public AjaxAskAFriend SkipQuestion()
    {
        //All situations in this method
        //1. if we destroyed session object
        //2. should we skip this question if anything goes wrong, if yes should
        //I return NextQuestion
        try
        {
            AskAFriend AAF = CurrentAskAFriend;

            if (AAF != null)
            {
                //AAF.SkipAAFQuestion();
                AAF.SkipAskQuestion();
            }

        }
        catch (Exception ex)
        {
            Next2Friends.Data.Trace.Tracer(ex.ToString(), "AAF SkipQuestion");
        }

        return NextQuestion();
    }

    /// <summary>
    /// Respond to the question
    /// </summary>
    /// <param name="SelectedValue"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod]
    public bool ResponseQuestion(int SelectedValue)
    {
        try
        {
            AAFResponse(SelectedValue);

        }
        catch (Exception ex)
        {
            Next2Friends.Data.Trace.Tracer(ex.ToString(), "AAF ResponseQuestion");


        }
        // return NextQuestion();
        //return new AjaxAskAFriend();
        return true;
    }

    public string GenarateResult(AskAFriend askAFriend)
    {

        if (askAFriend == null)
            //  return "Oops";
            return "";

        List<int> resultArr = null;


        try
        {

            resultArr = AskAFriend.GetAskAFriendResult(askAFriend);

        }
        catch (Exception ex)
        {

            Next2Friends.Data.Trace.Tracer("CAskafriend line 284" + ex.Message, "AAF", "ost");

            return "Oops";
        }

        finally { }
        if (resultArr == null)

            return "";



        StringBuilder sbLastAAF = new StringBuilder();

        if (askAFriend.ResponseType == (int)AskResponseType.RateTo10)
        {

            object[] Lastparameters = new object[12];

            Lastparameters[0] = (askAFriend.Question.Length > 28) ? askAFriend.Question.Substring(0, 28) + ".." : askAFriend.Question;
            Lastparameters[1] = askAFriend.Photo[0].PhotoResourceFile.FullyQualifiedURL;
            Lastparameters[2] = resultArr[0];
            Lastparameters[3] = resultArr[1];
            Lastparameters[4] = resultArr[2];
            Lastparameters[5] = resultArr[3];
            Lastparameters[6] = resultArr[4];
            Lastparameters[7] = resultArr[5];
            Lastparameters[8] = resultArr[6];
            Lastparameters[9] = resultArr[7];
            Lastparameters[10] = resultArr[8];
            Lastparameters[11] = resultArr[9];

            sbLastAAF.AppendFormat(@"   <div class='aaf_next'>
					                <p>{0}</p>
					                <img src='/{1}' style='width:100px;' alt='{0}' />
					                <ul class='bar_graph clearfix'>
						                <li>
							                1<div class='bar' style='width: {2}px;'></div>
							                2<div class='bar' style='width: {3}px;'></div>
							                3<div class='bar' style='width: {4}px;'></div>
							                4<div class='bar' style='width: {5}px;'></div>
							                5<div class='bar' style='width: {6}px;'></div>
				                            6<div class='bar' style='width: {7}px;'></div>
							                7<div class='bar' style='width: {8}px;'></div>
							                8<div class='bar' style='width: {9}px;'></div>
							                9<div class='bar' style='width: {10}px;'></div>
                                            10<div class='bar' style='width: {11}px;'></div>
						                </li>
					                </ul></div>", Lastparameters);
        }
        else
        {
            object[] Lastparameters = new object[7];

            Lastparameters[0] = (askAFriend.Question.Length > 28) ? askAFriend.Question.Substring(0, 28) + ".." : askAFriend.Question;
            Lastparameters[1] = askAFriend.Photo[0].PhotoResourceFile.FullyQualifiedURL;

            if (askAFriend.ResponseType == (int)AskResponseType.YesNo)
            {
                Lastparameters[2] = "Yes";
                Lastparameters[3] = "No";
            }
            else if (askAFriend.ResponseType == (int)AskResponseType.AB)
            {
                Lastparameters[2] = askAFriend.ResponseA;
                Lastparameters[3] = askAFriend.ResponseB;
            }
            else if (askAFriend.ResponseType == (int)AskResponseType.MultipleSelect)
            {
                if (askAFriend.NumberOfPhotos == 2)
                {

                }
                // Lastparameters[6] = askAFriend.ResponseB;
            }

            Lastparameters[4] = resultArr[0];
            Lastparameters[5] = resultArr[1];

            if (askAFriend.ResponseType == (int)AskResponseType.MultipleSelect)
            {
                sbLastAAF.AppendFormat(@"<div class='aaf_next'>
					                <p>{0}</p>
					                <img src='/{1}' style='width:100px;' alt='' />
					                <ul class='bar_graph clearfix'>
							                {2}<div class='bar' style='width: {4}px;'></div>
                                            {3}<div class='bar' style='width: {5}px;'></div>
					                </ul></div>", Lastparameters);
            }
            else
            {
                sbLastAAF.AppendFormat(@"<div class='aaf_next'>
					                <p>{0}</p>
					                <img src='/{1}' style='width:100px;' alt='' />
					                <ul class='bar_graph clearfix'>
							                {2}<div class='bar' style='width: {4}px;'></div>
                                            {3}<div class='bar' style='width: {5}px;'></div>
					                </ul></div>", Lastparameters);
            }
        }

        return sbLastAAF.ToString();

    }


    /// <summary>
    /// render the HTML form responses 
    /// </summary>
    public string GenarateAnswers(AskAFriend askAFriend)
    {
        bool HasntYetVoted = false;

        if (member != null) // it means if memeber is null HasntYetVoted=false
        {
            HasntYetVoted = AskAFriendResponse.HasntYetVoted(member, CurrentAskAFriend);
        }

        HasntYetVoted = true;

        StringBuilder sbHTML = new StringBuilder();

        //if (!IsPermalink && HasntYetVoted)
         if(true)
        {



            // base on type of the question we should generate answers
            // base on premaLink or not we give the user chance to vote or not
            // Should we disable voting for anymouse users

            if (askAFriend.ResponseType == (int)AskResponseType.YesNo)
            {

                //why we are using li

                string photo = askAFriend.Photo[0].PhotoResourceFile.FullyQualifiedURL;

                sbHTML.AppendFormat(@"<li>
					    <input type='radio' value='1' name='rbResponse' onclick='SubmitResponse(1);' >&nbsp;Yes&nbsp;&nbsp;    
                        <input type='radio' value='2' name='rbResponse' onclick='SubmitResponse(2);'>&nbsp;No 
						<br />

						<img src='/{0}' alt='' />
					</li>", photo);
            }
            else if (askAFriend.ResponseType == (int)AskResponseType.AB)
            {
                object[] parameters = new object[3];
                ///AAF.Photo[0].PhotoResourceFile.FullyQualifiedURL is empty
                parameters[0] = askAFriend.Photo[0].PhotoResourceFile.FullyQualifiedURL;
                parameters[1] = askAFriend.ResponseA;
                parameters[2] = askAFriend.ResponseB;

                sbHTML.AppendFormat(@"  <li>
                                      <input type='radio' name='rbResponse' value='1' onclick='SubmitResponse(1);'>&nbsp;{1}&nbsp;&nbsp;
                                      <input type='radio' name='rbResponse' value='2' onclick='SubmitResponse(2);'>&nbsp;{2} 
						              <br />
						              <img src='/{0}' alt='' />
					                </li>", parameters);

            }
            else if (askAFriend.ResponseType == (int)AskResponseType.RateTo10)
            {

                string photo = askAFriend.Photo[0].PhotoResourceFile.FullyQualifiedURL;

                sbHTML.AppendFormat(@"<li>
					    <input type='radio' name='rbResponse' value='1' onclick='SubmitResponse(1)'>&nbsp;1 
                        <input type='radio' name='rbResponse' value='2' onclick='SubmitResponse(2)'>&nbsp;2  
					    <input type='radio' name='rbResponse' value='3' onclick='SubmitResponse(3)'>&nbsp;3     
                        <input type='radio' name='rbResponse' value='4' onclick='SubmitResponse(4)'>&nbsp;4  
					    <input type='radio' name='rbResponse' value='5' onclick='SubmitResponse(5)'>&nbsp;5     
                        <input type='radio' name='rbResponse' value='6' onclick='SubmitResponse(6)'>&nbsp;6  
					    <input type='radio' name='rbResponse' value='7' onclick='SubmitResponse(7)'>&nbsp;7     
                        <input type='radio' name='rbResponse' value='8' onclick='SubmitResponse(8)'>&nbsp;8 
					    <input type='radio' name='rbResponse' value='9' onclick='SubmitResponse(9)'>&nbsp;9    
                        <input type='radio' name='rbResponse' value='10' onclick='SubmitResponse(10)'>&nbsp;10 

						<br />

						<img src='/{0}' alt='' />
					</li>", photo);


            }
            else if (askAFriend.ResponseType == (int)AskResponseType.MultipleSelect)
            {

                for (int i = 0; i < askAFriend.Photo.Count; i++)

                    sbHTML.AppendFormat("<li><input name='rbResponse' type='radio' value='{0}' onclick='SubmitResponse({0})'/><br /><img src='/{1}"

                        + "' width='160' alt='' /></li>", i + 1, askAFriend.Photo[i].PhotoResourceFile.FullyQualifiedURL);


            }
        }
        else
        {
            //permalink disables voting

            if (CurrentAskAFriend.ResponseType == (int)AskResponseType.MultipleSelect)
            {
                object[] parameters = new object[3];
                parameters[0] = "";
                parameters[1] = "";
                parameters[2] = "";

                try
                {
                parameters[0] = CurrentAskAFriend.Photo[0].PhotoResourceFile.FullyQualifiedURL;
                }
                catch { }
                try
                {
                parameters[1] = CurrentAskAFriend.Photo[1].PhotoResourceFile.FullyQualifiedURL;
                }
                catch { }
                try
                {

                parameters[2] = CurrentAskAFriend.Photo[2].PhotoResourceFile.FullyQualifiedURL;
                }
                catch { }
                


                sbHTML.AppendFormat(@"<li><img src='/{0}' width='160' alt='' /></li>
					                      <li><img src='/{1}' width='160' alt='' /></li>
					                      <li><img src='/{2}' width='160' alt='' /></li>", parameters);

            }
            else
            {
                object[] parameters = new object[3];

                parameters[0] = CurrentAskAFriend.Photo[0].PhotoResourceFile.FullyQualifiedURL;
                parameters[1] = CurrentAskAFriend.ResponseA;
                parameters[2] = CurrentAskAFriend.ResponseB;

                sbHTML.AppendFormat(@"<li><img src='/{0}' alt='' />
					                      </li>", parameters);
            }
            /////////////
        }


        return sbHTML.ToString();

    }


    /// <summary>
    /// Create the AddThis bookmarks html
    /// </summary>
    /// <param name="askAFriend"></param>
    /// <returns></returns>
    public string GenarateBookmarks(AskAFriend askAFriend)
    {

        StringBuilder sbBookmarks = new StringBuilder();

        object[] bookmarkParameters = new object[2];

        bookmarkParameters[0] = askAFriend.WebAskAFriendID;
        bookmarkParameters[1] = askAFriend.Question;

        sbBookmarks.AppendFormat(@"<script type='text/javascript'>addthis_pub  = 'Next2Friends';var addThisWebPhotoID = '';</script>
        <a href='http://www.addthis.com/bookmark.php' onmouseover=""return addthis_open(this, '', 'http://www.next2friends.com/ask/{0}'+addThisWebPhotoID, '{1}')"" onmouseout='addthis_close()' onclick='return addthis_sendto()'><img src='http://s9.addthis.com/button1-bm.gif' width='125' height='16' border='0' alt='' /></a><script type='text/javascript' src='http://s7.addthis.com/js/152/addthis_widget.js'></script>", bookmarkParameters);

        return sbBookmarks.ToString();
    }

    /// <summary>
    /// render the HTML comments 
    /// </summary>
    public string GenarateComments(AskAFriend askAFriend)
    {

        AjaxAAFComment[] commentList = AjaxAAFComment.GetAAFCommentsByAskAFriendIDWithJoin(askAFriend.AskAFriendID);

        StringBuilder sbComments = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            if (commentList.Length <= i)
                break;

            sbComments.Append(commentList[i].ToHTML());
        }
        return sbComments.ToString();
    }

    /// <summary>
    /// Render the html permalinks
    /// </summary>
    public string GenaratePermaLink(AskAFriend askAFriend)
    {
        return string.Format("<input type='text' onclick='this.select();' style='width:430px' value='{0}'>", ServerURL + askAFriend.WebAskAFriendID);
    }

    /// <summary>
    /// Forward to the next question
    /// </summary>
    public AjaxAskAFriend NextQuestion()
    {
        LastAskAFriend = CurrentAskAFriend;

        CurrentAskAFriend = AskAFriend.GetNextQuestion();

        return GenarateAjaxAskAFriend();

    }

    public AjaxAskAFriend GenarateAjaxAskAFriend()
    {
        AjaxAskAFriend ajaxAskAFriend = new AjaxAskAFriend();

        ajaxAskAFriend.WebAskAFriendID = CurrentAskAFriend.WebAskAFriendID;

        ajaxAskAFriend.Question = CurrentAskAFriend.Question;

        ajaxAskAFriend.LastAAF = GenarateResult(LastAskAFriend);

        ajaxAskAFriend.Comments = GenarateComments(CurrentAskAFriend);

        ajaxAskAFriend.Bookmarks = GenarateBookmarks(CurrentAskAFriend);

        //ajaxAskAFriend.CommentPost = GenarateCommentPost(CurrentAskAFriend);

        ajaxAskAFriend.HTML = GenarateAnswers(CurrentAskAFriend) + GenaratePermaLink(CurrentAskAFriend);

        return ajaxAskAFriend;
    }

    private void AAFResponse(int QuestionResponseValue)
    {
        AskAFriend AAF = CurrentAskAFriend;

        if (AAF != null)
        {
            //bool IsAllowed = AskAFriend.IsVoteValueAllowed(AAF, QuestionResponseValue);
            bool IsAllowed = true;
            // only add the vote if a valid response was sent to the server
            if (IsAllowed)
            {
                AskAFriendResponse AAFResponse = new AskAFriendResponse();

                AAFResponse.AskAFriendID = AAF.AskAFriendID;

                Member me = (Member)Session["Member"];

                if (me != null)
                {
                    AAFResponse.MemberID = me.MemberID;
                }

                AAFResponse.Result = QuestionResponseValue;

                AAFResponse.Save();

                AAF.TotalVotes++;
                AAF.Save();
            }
        }
    }


    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        Master.SkinID = "AskAFriend";
        base.OnPreInit(e);
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        // Master.HTMLTitle = PageTitle.GetAAFTitle(CurrentAskAFriend);
        //     Master.MetaDescription = CurrentAskAFriend.Question;
        //   Master.MetaKeywords = "Questions, Ask a question, Poll, Mobile phone";
    }
}
