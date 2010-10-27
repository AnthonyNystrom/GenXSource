<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MiniVideoPage.aspx.cs" Inherits="MiniVideoPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="/style.css?v=8" rel="stylesheet" type="text/css" />
        <link href="/StyleB.css?v=8" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="/lib/swfobject.js" language="javascript"></script>
        <script type="text/javascript" src="/lib/jquery.js" language="javascript"></script>
</head>
<body>
<style type="text/css">
html, body {
    background-color:#FFFFFF;
}
html {
    background:#FFFFFF none repeat scroll 0%;
}
</style>
    <form id="form1" runat="server">
    
    
     
      
      <%if (VideoOnly){ %>
      <div class="profile_box clearfix" style="width:335px">
      <%}  else{ %>
         <div class="profile_box clearfix">
      <%} %>
      <%--               <h3 class="profile_video_heading"><%=MainTitle %></h3>
                <p><%=MainSubTitle %></p>--%>
                <div class="profile_video" id="divDefaultView">
                    <%if (PageType == DefaultPageType.Video)
                      {%>

                			<script type="text/javascript">
	                                var s1 = new SWFObject("/flvplayer.swf","n2fplayer","332","260","7");
	                                s1.addParam("allowfullscreen","true");
	                                s1.addParam('bgcolor','#FFFFFF');
	                                s1.addParam('wmode','opaque');
	                                s1.addVariable("Ad","false");
	                                s1.addVariable("file","http://www.next2friends.com/<%=VideoURL%>");
	                                s1.addVariable("width","332");
	                                s1.addVariable("height","260");
	                                s1.addVariable("autostart","true");
	                                s1.write("divDefaultView");
                            </script>

                    <%}
                      else if (PageType == DefaultPageType.LiveBroadcast)
                      { %>
         
                            <script type="text/javascript">
			                    function loadMovie(){
	                                var s1 = new SWFObject("/flvplayer.swf","n2fplayer","480","400","7");
	                                s1.addParam("allowfullscreen","true");
	                                s1.addParam('bgcolor','#FFFFFF');
	                                s1.addVariable("Ad","false");
	                                s1.addVariable("live","true");
	                                s1.addVariable("videofile","<%=VideoURL%>");
	                                s1.addVariable("width","327");
	                                s1.addVariable("height","260");
	                                s1.addVariable("autostart","true");
	                                s1.write("divDefaultView");
	                            }
                            </script>
                    <%}%>
                </div>
                    
                 <%if (!VideoOnly){ %>
                <div class="profile_video_info" style="width:120px">
                    <p>
                        <div class="vote">
                            <span class="vote_count" id="spanVote">
                                <%=DefaultVoteCount%></span> <a href="<%=DefaultVoteUpLink %>" onmouseover="return true;" id="vUp" class="up">up</a>
                            <a href="<%=DefaultVoteDownLink %>" id="vDown" onmouseover="return true;" class="down">down</a></div>
                        <p>
                            Views: <%=DefaultNumberOfViews%><br />                            
                            <%--Favorited: <a href="#">5</a><br />--%>
                            Comments: <a target="_parent" href="<%=PermaLink %>"><span id="spanNumberOfComments2"> <%=NumberOfComments%></span></a></p>
                            
                           
                           <%if (PageType == DefaultPageType.Video)
                             { %> <a target="_parent" href="<%=ReportAbuseLink %>">Report Abuse</a><br /><%} %>
                             
                            Link<br /><input type="text" onclick="this.select();" style="width:88px" value="<%=PermaLink %>" class="form_txt2"/><br />
                            Embed<br /><input type="text" onclick="this.select();" style="width:88px" value='<%=EmbedLink %>' class="form_txt2"/><br /><br />

                </div>
                <%} %>
               
            </div>
    
    
    
    </form>
    
    
    <script type="text/javascript">
    function vote(WebID, dir){
        var up = $('#vUp');
        var down = $('#vDown');
        
        up.addClass('voted');
        down.addClass('voted');
        up.attr('href','#');
        down.attr('href','#');
        
        MiniVideoPage.Vote(WebID, dir, vote_Callback);
    }

    function vote_Callback(response){

        if(response.value!=null) {
            //update the vote box and disable the links
            var spanVote = $('#spanVote');
            var currentScore = spanVote.html();
            var newScore  = response.value + parseInt(currentScore);
            spanVote.html(newScore);
        }
    }
    
    </script>
</body>
</html>
