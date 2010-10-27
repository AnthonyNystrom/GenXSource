<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="View.aspx.cs" Inherits="MemberView" %>
<%@ MasterType VirtualPath="~/View.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftUpperContentHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftColContentHolder" runat="Server">

<link rel="stylesheet" type="text/css" href="/jcarousel-css/jquery.jcarousel.css" />
<link rel="stylesheet" type="text/css" href="/jcarousel-css/skin.css" />

<script type="text/javascript" src="/js/memberview.js"></script>
<script type="text/javascript" src="/lib/jcarousel.js"></script>
<script type="text/javascript" src="/lib/interface.js"></script>
<script type="text/javascript" src="/lib/popup.js"></script>
<script type="text/javascript" src="/lib/swfobject.js" language="javascript"></script>
              <div>
	            <a href="/invite">
	                 <img src="/images/invite-now.jpg" alt="invite friends" style="border:1px;border-color:#000000" />
	            </a>
	          </div>
	            
	          <p></p>
	     
			  <div class="profile_box" id="inviteDiv" runat="server">
				<h4 class="box_title collapsible">Invite Friends</h4>
				<div class="collapsible_div">								
				<ul class="friends_list">
				<li>
                    <asp:Label ID="litInvite" runat="server" Text="" EnableViewState="false"></asp:Label>
                </li>	
                <li>
						<label for="email">Email</label>
						<br />						
						<asp:TextBox runat="server" ID="txtFriend1" CssClass="form_txt_small"></asp:TextBox>
				</li>	
				<li>	
				    <label for="message">Message</label>
				    <br /><asp:HiddenField runat="server" ID="InviteWebmemberID" Value="" />
					<asp:TextBox runat="server" TextMode="MultiLine" ID="txtInviteMessage" CssClass="form_txt_small" style="height:50px;" Text='Check out this Next2Friends profile!'></asp:TextBox>
				</li>
			
				<li>
				    <div class="inviteFriendsButton">
							<input type="button" value="Invite Friend" class="form_btn" onclick="__doPostBack('<%=btnInvite.UniqueID %>', '');return false;" />
					        <asp:Button runat="server" ID="btnInvite" OnClick="btnInvite_Click" CssClass="hiddenButton" Text="Invite Friend" />
				    </div>
				</li>
				
				</ul>
				
				</div>
			</div>
    <!-- box start -->
            <div class="profile_box">
            

                <h4 class="box_title collapsible">
                    Friends <span>(<%=NumberOfFriends %>)</span></h4>
                <div class="collapsible_div">
                    <ul class="friends_list">
                        <%=FriendLister %>
                    </ul>
                    <p class="view_all">
                       <a href="/users/<%=ViewingMember.NickName %>/friends">See all friends</a></p>
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
                       <%-- <a href="#">See all subscribers</a>--%></p>
                </div>
            </div>
            <!-- box end -->

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="RightColContentHolder" runat="Server">
    <!-- box start -->
    <div class="profile_box" style="display:none;">
     
        <div id="divVideoPlayer"></div>
     
<%--            <script type="text/javascript">
            function player(id,start){
                var p = new SWFObject("/flvplayer.swf","n2fplayer","327","260","7");
                p.addParam("allowfullscreen","true");
                p.addParam("autostart","true");
                p.addParam('bgcolor','#FFFFFF');
                p.addVariable("Ad",false);       
                p.addVariable("videofile",id);
                p.addVariable("live","true");
                p.addVariable("width","327");
                p.addVariable("autostart",start);
                p.addVariable("height","260");
                p.write("divVideoPlayer");
            }
            
            player('',false);
            var isPlaying = false;
            
            function getLive(){
                if(isPlaying==false){
                    MemberView.GetLiveBroadcast('<%=ViewingMember.WebMemberID %>',getLive_callback);
                }
            }
            
            function getLive_callback(response,args){
                if(response.error==null){
                    if(response.value!=null){
                        player(response.value,true);
                        isPlaying = true;
                        $('#profile_box').css('display','block')
                    }
                }
            }
            
            setInterval('getLive()',3000);
            </script>--%>
     
     </div>
    
    <div class="profile_box">

				 
					<h4 class="box_title">Live Video</h4>
					<div class="collapsible_div">
					    <div style="text-align: center;">
	                       <object height="234" width="306"><param value="nickname=<%=ViewingMember.NickName %>" name="flashvars"><param value="http://services.next2friends.com/livewidget/n2flw1.swf" name="movie"><param value="true" name="allowFullScreen"><embed wmode="opaque" allowfullscreen="true" type="application/x-shockwave-flash" flashvars="nickname=<%=ViewingMember.NickName %>" src="http://services.next2friends.com/livewidget/n2flw1.swf" height="234" width="306"></object><br />
				           <br />Embed code! <input type="text" onclick="this.select();" style="width:220px" value='<%=EmbedLink %>' class="form_txt"/><br /><br />
				        </div>
				        
				      </div>
				</div>
				
				
    		<!-- box start -->
				<div class="profile_box">
					<h4 class="box_title collapsible">Photo galleries <%=GalleryDetailsHTML %></h4>
					 <%if (IsMyPage){%><p class="add_profile_comments"><a href="/myphotos">Edit Photo Galleries</a></p><% }%>
					<div class="collapsible_div carousel">
					<%if (ShowCarousel){ %>	
						<ul class="jcarousel-skin">
                        	<%=GalleryListerHTML %>
                    	</ul>
                    <%}else{ %>
                            <%=GalleryListerHTML %>
                    <% }%>
                     <p class="view_all">
                       <a href="/users/<%=ViewingMember.NickName %>/photos">See all Photos</a></p>
				    </div>
				</div>
				<!-- box end -->
			
                <%if (ShowCarousel){ %>						
                <script type="text/javascript">
                    jQuery('.carousel ul').jcarousel({
		                scroll: 3
                    });
                </script>
                <%} %>

            <!-- box end -->
            <!-- box start -->
            <div class="profile_box">
            <h4 class="box_title collapsible">Videos</h4>
            <%if (IsMyPage){%><p class="add_profile_comments"><a href="/myvideos">Edit Videos</a></p><% }%>
                <div class="collapsible_div">
                    
                        <%=DefaultLister %>
                    <%=DefaultPager%>
                    <p></p>
                     <p class="view_all">
                       <a href="/users/<%=ViewingMember.NickName %>/videos">See all Videos</a></p>
				    </div>
                </div>
                
                
            <div class="profile_box">
                <h4 class="box_title collapsible">
                    <%=AboutTitle %></h4>
                <p class="add_profile_comments">
                    <%if (IsMyPage)
                      { %><a href="/users/<%=ViewingMember.NickName %>/editaboutme">Edit</a><%} %></p>
                <div class="collapsible_div">
                <div class="aboutCol1">
						
						<%if (ShowAge)
                        { %> <h5>Age</h5>
                        
                        <p> <%=ViewingMember.AgeYears%></p><%} %> 
						<%if (MyLife != "")
                        { %>
						
						
						
						<h5><%=Field1Title %></h5>
						<p>
						    <%=MyLife %>							
						</p>
						<%} %>
						<%if (ViewingMember.AccountType==0){
                        %>
						<h5>Gender</h5>
						<p><%=Gender %></p>
						<h5>Last Active</h5>
						<p><%=LastActive%></p>
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
						<p><%=Country %> <img src="/images/flags/<%=ViewingMember.ISOCountry%>.gif" title="<%=ViewingMember.CountryName%>"  alt="<%=ViewingMember.CountryName%>" /></p>
						<%} %>						
						
						<h5>Member since</h5>
						<p><%=MemberSince %></p>
						<%}else{ %>	
						
						<%if (BasicInfo != string.Empty) { %><h5>Basic Information</h5>
						<p><%=BasicInfo%></p>
						<%} %>
						<h5>Company Name</h5>
						<p><%=CompanyName%></p>
						<h5>Company website</h5>
						<p><%=CompanyWebsite %></p>
						<h5></h5>
						
						<h5>Industry Sector</h5>
						<p><%=IndustrySector%></p>
						<h5>Year Founded</h5>
						<p><%=YearFounded %></p>
						<h5>Company Size</h5>
						<p><%=CompanySize %></p>
						
						<h5>Home country</h5>
						<p><%=CompanyCountry %></p>
					
						
						<%} %>
						<h5><%=Nick %>'s direct URL</h5>
						<p>
							<a href="<%=DirectUrl %>"><%=DirectUrlText %></a>
						</p>
					</div>
					<div class="aboutCol2">
					     <%if (DayJob != "")
                        { %>
						<h5>Day Job</h5>
						<p>
							<a href="/community/?t=full&pId=<%=DayJobID %>&to=6"><%=DayJob %></a>
						</p>
						<%} %>
					    
					    <%if (NightJob != "")
                        { %>
						<h5>Night Job</h5>
						<p>
							<a href="/community/?t=full&pId=<%=NightJobID %>&to=6"><%=NightJob %></a>
						</p>
						<%} %>
						
						 <%if (Hobby != "")
                        { %>
						<h5>Favourite Interest</h5>
						<p>
							<a href="/community/?t=full&hId=<%=HobbyID %>&to=7"><%=Hobby %></a>
						</p>
						<%} %>
					    <%if (Music != "")
                        { %>
						<h5><%=Field2Title %></h5>
						<p>
							<%=Music %>
						</p>
						<%} %>
						<%if (Books != "")
                        { %>
						<h5><%=Field3Title %></h5>
						<p>
							<%=Books %>
						</p>
						<%} %>
						<%if (Movies != "")
                        { %>						
						<h5><%=Field4Title %></h5>
						<p>
							<%=Movies %>
						</p>
						<%} %>	
						
						
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
						
						<p><div id="flashcontent"><a href="http://www.adobe.com/products/flashplayer/">Flash Player upgrade required</a></div></p>
						<%if (IsMyPage){ %>
                        <p><a href="javascript:displayMP3Upload();">Upload MP3's</a></p>
                        <%} %>
                        <form></form>
                        <div id="load"></div>
                        <script type="text/javascript">
                        
                        function restartMP3(){
                        
	                        var so = new SWFObject("/ep_player.swf", "ep_player", "301", "16", "9", "#FFFFFF");
	                        so.addVariable("skin", "/mp3/skins/micro_player/skin.xml");
	                        var nocacheid = Math.round(Math.random()*10000);
	                        //so.addVariable("playlist", "http://www.next2friends.com/user/<%=ViewingMember.NickName %>/mp3/playlist.xml");
	                        so.addVariable("playlist", "/mp3/GetMp3XML.aspx?n=<%=ViewingMember.NickName %>&c="+nocacheid);
	                        so.addVariable("autoplay", "true");
	                        so.addVariable("shuffle", "false");
	                        so.addVariable("key", "FM37NL9CEFR0HFRWDSRR");
	                        so.addVariable("repeat", "false");
	                        so.addVariable("buffertime", "1");
	                        so.addParam("wmode","opaque");
	                        so.addParam("allowscriptaccess", "always");
	                        
	                        so.write("flashcontent");
                        }
                        restartMP3();
                        </script>
											
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
                
                <iframe id="iframeAboutMe" name="iframeAboutMe" style="overflow:hidden" width="615" src="/AboutMe.aspx?m=<%=ViewingMember.WebMemberID%>" id="iFrameAboutMe" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" ></iframe>               
                        
                    <div id="divEditAboutMe" style="display: none;width:623px;text-align:right;">
                        <textarea id="txtAboutMe" cols="8" style="width: 100%;"></textarea><br />
                        <a href="javascript:cancelEditAboutMe();">Cancel</a>  |  <a href="javascript:ajaxEditAboutMe();void(0);">Update</a></div>
                </div>
            </div>          	
		
                <!-- profile_tabs_content -->
           <%-- </div>--%>
            <!-- box end -->
            
<%--            <%if (IsMyPage){ %>
            <div id="uploadMusic" style="display:none;">
                <iframe src="/MP3Upload.aspx" style="border:0px" frameborder="0" width="478" height="185"></iframe>
            </div>        
            <%} %>--%>
        
      
 <%if (IsMyPage){ %>     
<script type="text/javascript">
function displayMP3Upload(){
    var html = '<iframe src="/MP3Upload.aspx" style="border:0px" frameborder="0" width="478" height="185"></iframe>';
    npopup('Upload your MP3s',html,520,180);
}

function mp3popupClose() {
    restartMP3();
}

function displayMiniVideo(webVideoID,title){
    var html = '<iframe src="/MiniVideoPage.aspx?v='+webVideoID+'" style="border:0px" frameborder="0" scrolling="no" width="480" height="285"></iframe>';   
    npopup(title,html,525,285);
}
</script>

<div class="innerPopup" id="divMp3Upload" style="display:none;"><div class="innerPopupTop"></div><div class="innerPopupContent">
<div class="innerPopupTitle"><a style="float:right;" href="javascript:mp3popupClose();">close</a><h3>Upload your MP3s</h3></div><div class="innerPopupBlock">
    <div id="divMp3UploadFrame"></div>
</div></div><div class="innerPopupBottom"></div></div>


 <%} %>
</asp:Content>