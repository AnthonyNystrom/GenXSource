<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="NSpot.aspx.cs" Inherits="NSpotPage" Title="Untitled Page" %>
<%@ Import Namespace="Next2Friends.Misc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="js/ufo.js"></script>
    <script type="text/javascript" src="js/NSpot.js"></script>
    <script type="text/javascript" src="js/VideoPlayer.js"></script>
    <script type="text/javascript" src="lib/jquery.js"></script>
    <script type="text/javascript" src="lib/jquery_global.js"></script>

    <!-- middle start -->
    <div id="middle" class="clearfix">
        <!--subnav start -->
        <ul id="subnav" class="clearfix">
        </ul>
        <!-- subnav end -->
        <!-- profile left start -->
        <div class="profile_leftcol">
            <!-- box start -->
            <div class="profile_box">
                <p class="profile_pic"><a href="/Nspot.aspx?n=<%=ViewingNSpot.WebNSpotID %>&np=1">
                    <img src="<%=PhotoURL%>" alt="avatar" /></a></p>
                <p>
                    <strong>Nspot owner</strong>: <a href="/view.aspx?m=<%=ViewingNSpot.Member.WebMemberID%>">
                        <%=ViewingNSpot.Member.NickName%></a><br />
                    <strong>Country</strong>:
                    <%=ViewingNSpot.Member.ISOCountry%><br />
                    <strong>Comments</strong>: <a href="#comments"><span id="spanNumberOfComments1">
                        <%=NumberOfComments %></span></a><br />
                    <%-- <strong>Videos</strong>: <a href="#"><%=NumberOfVideos %></a><br />--%>
                    <%--<strong>Photos</strong>: <a href="#">
                        <%=NumberOfPhotos %></a><br />--%>
                    <strong>Members</strong>: <a href="#">
                        <%=NumberOfMembers %></a>
                </p>
            </div>
            <!-- box end -->
            <!-- box start -->
            <div class="profile_box">
                blank for now
            </div>
            <!-- box end -->
        </div>
        <!-- profile left end -->
        <!-- profile right start -->
        <div class="profile_rightcol">
        
        
           <%if (LoadLargeNSpotPhoto){ %>
             
             <div class="profile_box">
               
                <img src="<%=LargePhotoURL %>" />
               
            </div>
            
            <%} %>
        
        
        
            <div class="profile_box">
                <h3 class="profile_video_heading">
                    <%=ViewingNSpot.Name%></h3>
                <p>
                    <%=ViewingNSpot.Description%></p>
                    
                    
                  <div id="profilePhotoGallery">
                    		<noscript>
                    			<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0" width="628" height="500" id="gallery" align="middle">
									<param name="allowScriptAccess" value="sameDomain" />
									<param name="movie" value="gallery.swf" />
									<param name="quality" value="high" />
									<param name="bgcolor" value="#ffffff" />
									<param name="FlashVars" value="" />
									<embed src="gallery.swf" flashvars="xmlfile=MyImagesXML.aspx?n=<%=ViewingNSpot.WebNSpotID%>" quality="high" bgcolor="#ffffff" width="628" height="500" name="gallery" align="middle" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
								</object>
							</noscript>
                    	</div>
   
                    
	                    <script type="text/javascript">
							var F1 = { movie:"gallery.swf", width:"628", height:"500", majorversion:"8",quality:"high", build:"40", allowscriptaccess:"sameDomain",flashvars:"xmlfile=MyImagesXML.aspx?n=<%=ViewingNSpot.WebNSpotID%>>" };
							UFO.create(F1, "profilePhotoGallery");
						</script>
						
           
                </object>
            </div>
            <!-- box start -->
            <div class="profile_box clearfix">
                <h4 class="box_title"> NSpot Members</h4>
                <ul class="friends_list">
                    <%=DefaultMemberLister %>
                </ul>
                <p class="view_all">
                    <%--<a href="#" class="previous">Previous</a> <a href="#">See all friends (38)</a> <a
                        href="#" class="next">Next</a>--%></p>
            </div>
            <!-- box end -->
            <!-- box start -->
            <div class="profile_box">
                <a name="comments"></a>
                <h4 class="box_title collapsible">
                    Comments <span id="spanNumberOfComments3">(<%=NumberOfComments %>)</span></h4>
                <p class="add_profile_comments">
                    <%if (IsLoggedIn)
                      { %>
                    <a href="javascript:showPostComment();void(0);">
                        <%}
                      else
                      {%>
                        <a href="<%=LoginUrl %>">
                            <%} %>Post comment</a></p>
                <div id="divNewComment" style="display: none; width: 100%;">
                    <p class="align_right">
                        <textarea rows="5" id="txtNewComment" style="width: 100%;"></textarea><br />
                        <a href="javascript:cancelShowPostComment();void(0);">cancel</a> &nbsp;&nbsp;|&nbsp;&nbsp;
                        <a href="javascript:ajaxPostComment(<%=DefaultNewCommentParams %>);void(0);">post</a></p>
                </div>
                <div class="collapsible_div">
                    <ol class="profile_commentlist">
                        <li class="clearfix" id="liCommentList">
                            <%=PageComments %>
                        </li>
                    </ol>
                </div>
            </div>
        </div>
        <!-- box end -->
    </div>
    <!-- profile right end -->
</asp:Content>
