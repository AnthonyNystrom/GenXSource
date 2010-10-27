<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="VideoView.aspx.cs" Inherits="VideoView" %>
<%@ MasterType VirtualPath="~/View.master" %>

<%@ Register src="Comments.ascx" tagname="Comments" tagprefix="uc1" %>
<%@ Register src="~/UserControls/ForwardToFriend.ascx" tagname="ForwardToFriend" tagprefix="uc2" %>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftUpperContentHolder" runat="Server">
<%--<ul id="addFavUl" class="profile_menu">					
	<li><p><a href="<%=AddFavouritesLink %>" onmouseover="return true;" class="add_favorites">Add to Favorites</a><span id="spanAddFavourites"></span></p></li>
	<li></li>
</ul>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftColContentHolder" runat="Server">
    <uc2:ForwardToFriend ID="forwardToFriend" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="RightColContentHolder" runat="Server">

<!-- box start -->
            <div class="profile_box clearfix">
               <h3 class="profile_video_heading"><%=MainTitle %></h3>
                <p><%=MainSubTitle %></p>
                <div class="profile_video" id="divDefaultView">
                    <%if (PageType == DefaultPageType.Video)
                      {%>

                			<script type="text/javascript">
			                    function loadMovie(){
	                                var s1 = new SWFObject("/flvplayer.swf","n2fplayer","480","400","7");
	                                s1.addParam("allowfullscreen","true");
	                                s1.addParam('bgcolor','#FFFFFF');
	                                s1.addParam('wmode','opaque');
	                                s1.addVariable("Ad","false");
	                                s1.addVariable("file","http://www.next2friends.com/<%=VideoURL%>");
	                                s1.addVariable("width","480");
	                                s1.addVariable("height","400");
	                                s1.addVariable("autostart","true");
	                                s1.write("divDefaultView");
	                            }
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
	                                s1.addVariable("width","480");
	                                s1.addVariable("height","400");
	                                s1.addVariable("autostart","true");
	                                s1.write("divDefaultView");
	                            }
                            </script>
                    <%}%>
                </div>
                
                  <%if (PageType != DefaultPageType.LiveBroadcast)
                    {%>                             
                 
                <div class="profile_video_info">
                    <p>
                        <div class="vote">
                            <span class="vote_count" id="spanVote">
                                <%=DefaultVoteCount%></span> <a href="<%=DefaultVoteUpLink %>" onmouseover="return true;" id="vUp"class="up">up</a>
                            <a href="<%=DefaultVoteDownLink %>" id="vDown" onmouseover="return true;" class="down">down</a></div>
                        <p>
                            Views: <%=DefaultNumberOfViews%><br />                            
                            <%--Favorited: <a href="#">5</a><br />--%>
                            Comments: <a href="#comments"><span id="spanNumberOfComments2"> <%=NumberOfComments%></span></a></p>
                            
                           
                           <%if (PageType == DefaultPageType.Video)
                             { %> <a href="<%=ReportAbuseLink %>">Report Abuse</a><br /><%} %>
                             
                            Link <input type="text" onclick="this.select();" style="width:120px" value="<%=PermaLink %>" class="form_txt"/><br />
                            Embed <input type="text" onclick="this.select();" style="width:120px" value='<%=EmbedLink %>' class="form_txt"/><br /><br />
                            
                            <ul id="addFavUl" class="profile_menu">					
	                            <li><p><a href="<%=AddFavouritesLink %>" onmouseover="return true;" class="add_favorites">Share with Friends</a><span id="spanAddFavourites"></span></p></li>
	                            <li></li>
                            </ul>
                            
                            <!-- AddThis Button BEGIN -->
                            <script type="text/javascript">addthis_pub  = 'Next2Friends';</script>
                            <a href="http://www.addthis.com/bookmark.php" onmouseover="return addthis_open(this, '', 'http://www.next2friends.com/<%=ThisURL %>', '<%=MainTitle %>')" onmouseout="addthis_close()" onclick="return addthis_sendto()"><img src="http://s9.addthis.com/button1-bm.gif" width="125" height="16" border="0" alt="" /></a><script type="text/javascript" src="http://s7.addthis.com/js/152/addthis_widget.js"></script>
                            <!-- AddThis Button END -->
                </div>
                <%} %>
               
            </div>
            <!-- box end -->
  <%--          <div class="profile_box">
            <script type="text/javascript">

                function initialize()
                {
                    setTimeout( function()
                    {
                        var plugin = document.getElementById( "Main" );
                        if ( !plugin ) plugin = document.all[ "Main" ];

                        videoSlider.init( plugin, function( item )
                        {
                            if(item.uniqueId.indexOf('_')>-1){
                                player(item.url,"",true,false,null);
                            }else{
                                player(item.uniqueId,"",true,true,item.uniqueId);
                            }
                            
                        } );
                        videoSlider.insert(0, null, 'http://www.next2friends.com/user//Shelton/video/NWEwNDEzYTQzYjE5NDgzOG.flv', 'http://www.next2friends.com/user//Shelton/vthmb/NWEwNDEzYTQzYjE5NDgzOG.jpg', 'shelton', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Doyley/video/MjBjNmQzYWI1MjU5NDM2Yj.flv', 'http://www.next2friends.com/user//Doyley/vthmb/MjBjNmQzYWI1MjU5NDM2Yj.jpg', 'Doyley', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Lawrence/video/Nzc2NTBiMjFkZDAxNDg5Nz.flv', 'http://www.next2friends.com/user//Lawrence/vthmb/Nzc2NTBiMjFkZDAxNDg5Nz.jpg', 'Lawrence', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Lawrence/video/Y2U4MThhODBkYmQ0NDY3M2.flv', 'http://www.next2friends.com/user//Lawrence/vthmb/Y2U4MThhODBkYmQ0NDY3M2.jpg', 'Lawrence', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Doyley/video/MDNkZGZkNGFhMTg3NGE1NG.flv', 'http://www.next2friends.com/user//Doyley/vthmb/MDNkZGZkNGFhMTg3NGE1NG.jpg', 'Doyley', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Doyley/video/OTVjZDkwNTA2MmE5NDViNT.flv', 'http://www.next2friends.com/user//Doyley/vthmb/OTVjZDkwNTA2MmE5NDViNT.jpg', 'Doyley', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/Mzk1ZjNiMjdiMTJjNDA0OD.flv', 'http://www.next2friends.com/user//Genius/vthmb/Mzk1ZjNiMjdiMTJjNDA0OD.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Anthony/video/NTY4ZTY1NjdkOTk4NGQ1YW.flv', 'http://www.next2friends.com/user//Anthony/vthmb/NTY4ZTY1NjdkOTk4NGQ1YW.jpg', 'Anthony', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Lawrence/video/NDkzZjNmNmNmODI3NDA5ZD.flv', 'http://www.next2friends.com/user//Lawrence/vthmb/NDkzZjNmNmNmODI3NDA5ZD.jpg', 'Lawrence', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/ODg3OTE5ZTg1OTc5NDNhN2.flv', 'http://www.next2friends.com/user//Genius/vthmb/ODg3OTE5ZTg1OTc5NDNhN2.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/NmY5NjhmZjYyZjNhNDhmNT.flv', 'http://www.next2friends.com/user//Genius/vthmb/NmY5NjhmZjYyZjNhNDhmNT.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/NzUzNDBhMDQ1NjY0NDU4Nm.flv', 'http://www.next2friends.com/user//Genius/vthmb/NzUzNDBhMDQ1NjY0NDU4Nm.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/YmZmZDliZTVhYjFmNGI0N2.flv', 'http://www.next2friends.com/user//Genius/vthmb/YmZmZDliZTVhYjFmNGI0N2.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/ZTk1MzUxMjhmN2I0NDdmMz.flv', 'http://www.next2friends.com/user//Genius/vthmb/ZTk1MzUxMjhmN2I0NDdmMz.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/YjM2NWFhYmM4N2I3NDQ2ZW.flv', 'http://www.next2friends.com/user//Genius/vthmb/YjM2NWFhYmM4N2I3NDQ2ZW.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/MWRkOWRmYTgxZmFjNGJmNT.flv', 'http://www.next2friends.com/user//Genius/vthmb/MWRkOWRmYTgxZmFjNGJmNT.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/OWE0ZjFlMDQxMjk0NDJjNm.flv', 'http://www.next2friends.com/user//Genius/vthmb/OWE0ZjFlMDQxMjk0NDJjNm.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Hanserik/video/OTA1ZDQzYTdjZTU4NDA4Y2.flv', 'http://www.next2friends.com/user//Hanserik/vthmb/OTA1ZDQzYTdjZTU4NDA4Y2.jpg', 'HansErik', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Hanserik/video/NmVlZGU1ZGUxZWNhNDlmOW.flv', 'http://www.next2friends.com/user//Hanserik/vthmb/NmVlZGU1ZGUxZWNhNDlmOW.jpg', 'HansErik', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Hanserik/video/NjA4ZGM2ZDYyOWNkNGQ2Nz.flv', 'http://www.next2friends.com/user//Hanserik/vthmb/NjA4ZGM2ZDYyOWNkNGQ2Nz.jpg', 'HansErik', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//olibs/video/NGZhNDlhMDBlYWYwNDU2Mj.flv', 'http://www.next2friends.com/user//olibs/vthmb/NGZhNDlhMDBlYWYwNDU2Mj.jpg', 'olibs', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//olibs/video/MGUxZDU2ZDU0MDY5NDVlNj.flv', 'http://www.next2friends.com/user//olibs/vthmb/MGUxZDU2ZDU0MDY5NDVlNj.jpg', 'olibs', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Hanserik/video/NDkzZTZjNzkxZDUwNDFkZT.flv', 'http://www.next2friends.com/user//Hanserik/vthmb/NDkzZTZjNzkxZDUwNDFkZT.jpg', 'HansErik', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Hanserik/video/ZTA0Yzc0M2ExZGUzNGVlY2.flv', 'http://www.next2friends.com/user//Hanserik/vthmb/ZTA0Yzc0M2ExZGUzNGVlY2.jpg', 'HansErik', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Hanserik/video/MjcwMWQxNDc5NmMyNDE0Y2.flv', 'http://www.next2friends.com/user//Hanserik/vthmb/MjcwMWQxNDc5NmMyNDE0Y2.jpg', 'HansErik', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Hanserik/video/YzUyYThjMjI4N2IwNDQ5Nm.flv', 'http://www.next2friends.com/user//Hanserik/vthmb/YzUyYThjMjI4N2IwNDQ5Nm.jpg', 'HansErik', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Rwilde/video/YmQzNzRjOTVkOTViNGJjZm.flv', 'http://www.next2friends.com/user//Rwilde/vthmb/YmQzNzRjOTVkOTViNGJjZm.jpg', 'rwilde', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Rwilde/video/OTI4NWVmODVkZWYzNGQxNz.flv', 'http://www.next2friends.com/user//Rwilde/vthmb/OTI4NWVmODVkZWYzNGQxNz.jpg', 'rwilde', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Rwilde/video/ZjVmMWZmNGY5ZGI5NDJlYz.flv', 'http://www.next2friends.com/user//Rwilde/vthmb/ZjVmMWZmNGY5ZGI5NDJlYz.jpg', 'rwilde', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Rwilde/video/YzQ3N2Y0Y2I5ZjZmNDhhZG.flv', 'http://www.next2friends.com/user//Rwilde/vthmb/YzQ3N2Y0Y2I5ZjZmNDhhZG.jpg', 'rwilde', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Shelton/video/MmQxMTBlNTQxMTBkNDVjMD.flv', 'http://www.next2friends.com/user//Shelton/vthmb/MmQxMTBlNTQxMTBkNDVjMD.jpg', 'shelton', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Lawrence/video/MmY1OTdjMTMzZWI2NDExNj.flv', 'http://www.next2friends.com/user//Lawrence/vthmb/MmY1OTdjMTMzZWI2NDExNj.jpg', 'Lawrence', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Lawrence/video/ZDE4ZWU2NzA3ZWYyNGM3NT.flv', 'http://www.next2friends.com/user//Lawrence/vthmb/ZDE4ZWU2NzA3ZWYyNGM3NT.jpg', 'Lawrence', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/MGQzYzM1NTZhNzEyNDdmNG.flv', 'http://www.next2friends.com/user//Genius/vthmb/MGQzYzM1NTZhNzEyNDdmNG.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/YzQyMDllMTA3NmI1NGI0YW.flv', 'http://www.next2friends.com/user//Genius/vthmb/YzQyMDllMTA3NmI1NGI0YW.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/MjlhY2QyOTgzNjU4NGYzMm.flv', 'http://www.next2friends.com/user//Genius/vthmb/MjlhY2QyOTgzNjU4NGYzMm.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/ZGYzMjY5YTBkY2EyNDU4OT.flv', 'http://www.next2friends.com/user//Genius/vthmb/ZGYzMjY5YTBkY2EyNDU4OT.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/Y2VlOWRiZDcwOGJjNGU1Zj.flv', 'http://www.next2friends.com/user//Genius/vthmb/Y2VlOWRiZDcwOGJjNGU1Zj.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Lawrence/video/MmJjYzNjM2FjM2FlNGY2OD.flv', 'http://www.next2friends.com/user//Lawrence/vthmb/MmJjYzNjM2FjM2FlNGY2OD.jpg', 'Lawrence', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//olibs/video/MDc4YmUzMTI0OTZhNDg0ZW.flv', 'http://www.next2friends.com/user//olibs/vthmb/MDc4YmUzMTI0OTZhNDg0ZW.jpg', 'olibs', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/MWFiZTI5ODY0MGI5NGFhNG.flv', 'http://www.next2friends.com/user//Genius/vthmb/MWFiZTI5ODY0MGI5NGFhNG.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/YjBiNzJlMDlkMzJlNDhhOT.flv', 'http://www.next2friends.com/user//Genius/vthmb/YjBiNzJlMDlkMzJlNDhhOT.jpg', 'Genius', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Lawrence/video/ZmQzYmQxYzlkZDcwNGZjN2.flv', 'http://www.next2friends.com/user//Lawrence/vthmb/ZmQzYmQxYzlkZDcwNGZjN2.jpg', 'Lawrence', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Shelton/video/OTllMDVjYjk1YjNmNDQ5YW.flv', 'http://www.next2friends.com/user//Shelton/vthmb/OTllMDVjYjk1YjNmNDQ5YW.jpg', 'shelton', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//olibs/video/NGMxNTNkZTFjNWZkNGE0Yz.flv', 'http://www.next2friends.com/user//olibs/vthmb/NGMxNTNkZTFjNWZkNGE0Yz.jpg', 'olibs', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Shelton/video/NWY3MTA4MmM3MDQ3NDEwZG.flv', 'http://www.next2friends.com/user//Shelton/vthmb/NWY3MTA4MmM3MDQ3NDEwZG.jpg', 'shelton', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Anthony/video/YWNmZDA5NzA5MmY5NDNhOG.flv', 'http://www.next2friends.com/user//Anthony/vthmb/YWNmZDA5NzA5MmY5NDNhOG.jpg', 'Anthony', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//olibs/video/MjNlYjg3ZjM2NzJjNGI5NG.flv', 'http://www.next2friends.com/user//olibs/vthmb/MjNlYjg3ZjM2NzJjNGI5NG.jpg', 'olibs', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Doyley/video/MDI0MGZiNGFjMzFmNGZiY2.flv', 'http://www.next2friends.com/user//Doyley/vthmb/MDI0MGZiNGFjMzFmNGZiY2.jpg', 'Doyley', false, false );videoSlider.insert(0, null, 'http://www.next2friends.com/user//Genius/video/NzIyMzA3ZDZhMmE3NDExM2.flv', 'http://www.next2friends.com/user//Genius/vthmb/NzIyMzA3ZDZhMmE3NDExM2.jpg', 'Genius', false, false );livePush('ODc2OTA2NGE0NjRkNDI2OW', '', 'http://www.next2friends.com//user/Doyley/vthmb/ODc2OTA2NGE0NjRkNDI2OW.jpg', 'Doyley', true, false);
                       
                    }, 0 );

                    return [];
                }
                </script>
            <script type="text/javascript" src="/lib/swfobject.js" language="javascript"></script>
            <script type="text/javascript" src="/N2F.VideoSlider.js?c=2" language="javascript"></script>
            Other videos from <%=ViewingMember.NickName %>
                <div id="flashcontent" style="text-align:center;">
               
                <script type="text/javascript">
	                    var so = new SWFObject( "/N2F.VideoSlider.swf?c=<%=DateTime.Now.Ticks %>", "Main", "600", "150", "9.0.0", "#FFFFFF", true );
	                    so.addParam( "wmode", "transparent");
	                    so.addParam( "scale", "showall");
	                    so.addParam( "quality", "high");
	                    so.addParam( "allowScriptAccess", "always");
	                    so.addVariable( "initMethod", "initialize" );
	                    so.addVariable( "selectMethod", "selectThumbnail" );
	                    so.write( "flashcontent" );
                    </script>
                    <noscript>
	                    <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0" width="500" height="150" id="Main" align="middle">
		                    <param name="allowScriptAccess" value="always" />
		                    <param name="allowFullScreen" value="false" />
		                    <param name="wmode" value="transparent" />
		                    <param name="flashvars" value="initMethod=initialize&selectMethod=selectThumbnail" />
		                    <param name="movie" value="/N2F.VideoSlider.swf?c=<%=DateTime.Now.Ticks %>" /><param name="quality" value="high" /><param name="bgcolor" value="#ffffff" />
		                    <embed src="/N2F.VideoSlider.swf?c=<%=DateTime.Now.Ticks %>" quality="high" bgcolor="#ffffff" width="600" height="150" name="Main" align="middle" allowScriptAccess="always" allowFullScreen="false" wmode="transparent" flashvars="initMethod=initialize&selectMethod=selectThumbnail" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
	                    </object>
                    </noscript>
			    </div>
			</div>
			--%>
            <!-- box start -->
            <div class="profile_box" runat="server" id="divComments">
                <uc1:Comments ID="Comments1" runat="server" />
            </div>
            <!-- box end -->
<script type="text/javascript" src="/lib/swfobject.js"></script>
<script type="text/javascript">
loadMovie(); 
	
function initialize()
{

    setTimeout( function()
    {
        var plugin = document.getElementById( "Main" );
        if ( !plugin ) plugin = document.all[ "Main" ];

        videoSlider.init( plugin, function( item )
        {
            if(item.uniqueId.indexOf('_')>-1){
                player(item.url,"",true,false,null);
            }else{
                player(item.uniqueId,"",true,true,item.uniqueId);
            }
            
        } );
        <%=SliderJS %>
       
    }, 0 );

    return [];
}
</script>

</asp:Content>