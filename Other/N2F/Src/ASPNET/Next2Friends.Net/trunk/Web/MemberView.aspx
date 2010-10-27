<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="MemberView.aspx.cs" Inherits="MemberView" %>
<%@ Register src="Comments.ascx" tagname="Comments" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftColContentHolder" runat="Server">
    <link href="style.css" rel="stylesheet" type="text/css" />

<link rel="stylesheet" type="text/css" href="jcarousel-css/jquery.jcarousel.css" />
<link rel="stylesheet" type="text/css" href="jcarousel-css/skin.css" />

<script type="text/javascript" src="js/memberview.js"></script>
<script type="text/javascript" src="lib/FancyZoom.js"></script>
<script type="text/javascript" src="lib/jcarousel.js"></script>
<script type="text/javascript" src="lib/interface.js"></script>
    <!-- box start -->
            <div class="profile_box">
            

                <h4 class="box_title collapsible">
                    Friends <span>(<%=NumberOfFriends %>)</span></h4>
                <div class="collapsible_div">
                    <ul class="friends_list">
                        <%=FriendLister %>
                    </ul>
                    <p class="view_all">
                       <%-- <a href="#">See all friends</a></p>--%>
                </div>
                <a name="subscribers"></a>
                 </div>
            <!-- box end -->
				
				
				<!-- box start -->
            <div class="profile_box">            
                <h4 class="box_title collapsible">                
                    Subscribers <span>(<%=NumberOfMemberSubscribers%>)</span></h4>
                <div class="collapsible_div">
                    <ul class="friends_list">
                        <%=MemberSubscribers %>
                    </ul>
                    <p class="view_all">
                       <%-- <a href="#">See all subscribers</a></p>--%>
                </div>
            </div>
            <!-- box end -->
            <!-- box start -->
            <div class="profile_box" id="inviteDiv" runat="server">
				<h4 class="box_title collapsible">Invite Friends</h4>
				<div class="collapsible_div">								
				<UL class="friends_list">
				<li>                        
                    <asp:Literal runat="server" ID="litInvite">Please feel free to invite your friends!</asp:Literal> 
                </li>
				<li>	
				    <label for="email1">Message</label>
				    <br />
					<asp:TextBox runat="server" TextMode="MultiLine" ID="txtInviteMessage" CssClass="form_txt_small" style="height:50px;" Text="Your personal message here!"></asp:TextBox>
				</li>
			    <li>
						<label for="email1">Email 1</label>
						<br />						
						<asp:TextBox runat="server" ID="txtFriend1" CssClass="form_txt_small"></asp:TextBox>
				</li>				
				<li>
					<label for="email2">Email 2</label>
					<br />
					<asp:TextBox runat="server" ID="txtFriend2" CssClass="form_txt_small"></asp:TextBox>
				</li>

				<li>
					<label for="email3">Email 3</label>
					<br />
					<asp:TextBox runat="server" ID="txtFriend3" CssClass="form_txt_small"></asp:TextBox>
				</li>
				<li>
					<label for="email4">Email 4</label>
					<br />
					<asp:TextBox runat="server" ID="txtFriend4" CssClass="form_txt_small"></asp:TextBox>
				</li>

				<li>
					<label for="email5">Email 5</label>
					<br />
					<asp:TextBox runat="server" ID="txtFriend5" CssClass="form_txt_small"></asp:TextBox>
				</li>
				<li>
				<p>
							<input type="button" value="Invite Friends" class="form_btn" onclick="__doPostBack('<%=btnInvite.UniqueID %>', '');return false;" />
					        <asp:Button runat="server" ID="btnInvite" OnClick="btnInvite_Click" CssClass="hiddenButton" Text="Invite Friends" />
				</p>
				</li>					
				</ul>
				</div>
			</div>
			<!-- box end -->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="RightColContentHolder" runat="Server">
    <!-- box start -->
            <div class="profile_box">
                <h4 class="box_title collapsible">
                    About me</h4>
                <p class="add_profile_comments">
                    <%if (IsMyPage)
                      { %><a href="AboutMeEdit.aspx">Edit</a><%} %></p>
                <div class="collapsible_div">
                <div class="aboutCol1">
						
						<%if (MyLife != "")
                        { %>
						<h5>My Life</h5>
						<p>
						    <%=MyLife %>							
						</p>
						<%} %>
						<h5>Gender</h5>
						<p><%=Gender %></p>
						<h5>Last Active</h5>
						<p><%=LastActive %></p>
						<h5>Profile Views</h5>
						<p><%=ProfileViews %></p>
						<h5>Relationship Status</h5>
						<p><%=RelationshipStatus %>
						
						<%if (OtherHalf != "Not Saying")
                        { %>
							<%=OtherHalf %>
						<%} %>
						</p>
						<%if (Hometown != "")
                        { %>
						<h5>Hometown</h5>
						<p><%=Hometown %></p>
						<%} %>
						
						<%if (Country != "")
                        { %>						
						<h5>Country</h5>
						<p><%=Country %></p>
						<%} %>						
						
						<h5>Member since</h5>
						<p><%=MemberSince %></p>
					</div>
					<div class="aboutCol2">
					    
					    <%if (Music != "")
                        { %>
						<h5>Music</h5>
						<p>
							<%=Music %>
						</p>
						<%} %>
						<%if (Books != "")
                        { %>
						<h5>Books</h5>
						<p>
							<%=Books %>
						</p>
						<%} %>
						<%if (Movies != "")
                        { %>						
						<h5>Movies</h5>
						<p>
							<%=Movies %>
						</p>
						<%} %>	
						<h5><%=Nick %>&#39;s direct URL</h5>
						<p>
							<a href="<%=DirectUrl %>"><%=DirectUrlText %></a>
						</p>
						
						<%if (MySpaceURL != "")
                        { %>						
						<h5>My MySpace Profile URL</h5>
						<p>
							<a href="<%=MySpaceURL%>"><%=MySpaceURLText%></a>
						</p>
						<%} %>	
						
						
						<%if (FaceBookURL != "")
                        { %>						
						<h5>My FaceBook Profile URL</h5>
						<p>
							<a href="<%=FaceBookURL%>"><%=FaceBookURLText%></a>
						</p>
						<%} %>	
						
						
						<%if (BlogURL != "")
                        { %>						
						<h5>My Blog URL</h5>
						<p>
							<a href="<%=BlogURL %>"><%=BlogURLText %></a>
						</p>
						<%} %>	
						
						
						<%if (BlogFeedURL != "")
                        { %>						
						<h5>My Blogs Feed URL</h5>
						<p>
							<a href="<%=BlogFeedURL%>"><%=BlogFeedURLText%></a>
						</p>
						<%} %>	
											
					</div>
					<div class="clear"></div>                                       
                    
            </div>
            </div>  
            
            <!-- box start -->
            <div class="profile_box">
                <h4 class="box_title collapsible">
                    My Embedded Content</h4>
                <p class="add_profile_comments">
                    <%if (IsMyPage)
                      { %><a href="javascript:showEditAboutMe();void(0);">Edit</a><%} %></p>
                <div class="collapsible_div" style="overflow:hidden">
                
                <iframe id="iframeAboutMe" style="overflow:hidden" width="615" src="AboutMe.aspx?m=<%=ViewingMember.WebMemberID%>" id="iFrameAboutMe" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" ></iframe>               
                        
                    <div id="divEditAboutMe" style="display: none;width:623px;text-align:right;">
                        <textarea id="txtAboutMe" cols="8" style="width: 100%;"></textarea><br />
                        <a href="javascript:cancelEditAboutMe();">Cancel</a>  |  <a href="javascript:ajaxEditAboutMe();void(0);">
                        Update</a></div>
                </div>
            </div>          	
				<!-- box start -->
				<div class="profile_box">
					<h4>Photos <%=GalleryDetailsHTML %></h4>
					<div class="collapsible_div carousel">
						<ul class="jcarousel-skin">
                        	<%=GalleryListerHTML %>
                    	</ul>
				    </div>
				</div>
				<!-- box end -->
				
<script type="text/javascript">
    jQuery('.carousel ul').jcarousel({
		scroll: 1
    });
</script>
                        
            
            <!-- box end -->
            <!-- box start -->
            <div class="profile_box tabs_box">
            <h4>Photos (25 photos in 3 galleries)</h4>  
                <div class="collapsible_div profile_tabs_content">
                    <ul class="profile_vid_list" id="ulContentLister">
                        <%=DefaultLister %>
                    </ul>
                    <p class="view_all" id="pPager"><%=DefaultPager%></p>                    
                </div>
                <!-- profile_tabs_content -->
            </div>
            <!-- box end -->
            <!-- box start -->
            <div class="profile_box" runat="server" id="divComments">
                <uc1:Comments ID="Comments1" runat="server" />
            </div>
            <!-- box end -->
</asp:Content>