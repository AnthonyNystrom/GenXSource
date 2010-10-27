<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ViewGalleryPB.aspx.cs" Inherits="ViewGalleryPB" Title="Untitled Page" %>

<%@ Import Namespace="Next2Friends.Misc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager runat="server" EnablePageMethods="true"></asp:ScriptManager>
	<!-- middle start -->
	<div id="middle" class="clearfix no_subnav">

			<!-- profile left start -->
			<div class="profile_leftcol">

				<!-- box start -->
          <!-- box start -->
            <div class="profile_box">
                <h3 class="profile_name">
                    <a href="view.aspx?m=<%=ViewingMember.WebMemberID %>"><%=ViewingMember.NickName %></a></h3>
                <p class="profile_pic">
                    
                    <a href="view.aspx?m=<%=ViewingMember.WebMemberID %>"><img src="<%=PhotoURL%>" alt="avatar" /></a><%if (IsMyPage)
                                                              { %><br />
          
                    <%} %></p>
                <p>
                    <strong>Age</strong>: <%=ViewingMember.AgeYears %><br />
                    <strong>Country</strong>: <%=ViewingMember.CountryName%><br />
                   <%-- <strong>Last Online</strong>: <%=ViewingMember.CountryName%><br />--%>
                    <strong>Subscribers</strong>: <a href="#"><%=NumberOfMemberSubscribers %></a> <%if(!IsMyPage){ %>| <a href="<%=SubscribeLink %>">Subscribe</a> <%} %><br />
                    <strong>Videos</strong>: <a href="view.aspx?m=<%=ViewingMember.WebMemberID %>&js=v"><%=NumberOfVideos %></a><br />
                    <strong>Photos</strong>: <a href="view.aspx?m=<%=ViewingMember.WebMemberID %>&js=v"><%=NumberOfPhotos %></a><br />
                    <strong>Friends</strong>: <a href="view.aspx?m=<%=ViewingMember.WebMemberID %>"><%=NumberOfFriends %></a>
                </p>
                <%if(!IsMyPage){ %>
                <ul class="profile_menu">
                    <li><a href="<%=SubscribeLink %>" onmouseover="return true;" class="subscribe">Subscribe</a><span id="spanSubscribe"></span></li>
                    <%if (!IsMyPage)
                      { %>
                    <li><a href="<%=AddToFriendsLink %>" onmouseover="return true;" class="add_to_friend">Send Friend request</a><span id="spanAddToFriends"></span></li>
                    <%} %>
                    <li><a href="<%=SendMessageLink %>" onmouseover="return true;" class="send_message">Send Message</a></li>
                   <%-- <li><a href="<%=BlockMemberLink %>" class="block_user">Block Member</a><span id="spanBlockMember"></span></li>--%>
                  <%--  <li><a href="<%=AddFavouritesLink %>" onmouseover="return true;" class="add_favorites">Add to Favorites</a><span id="spanAddFavourites"></span></li>--%>
                </ul>
                <%} %>
            </div>

				<!-- box end -->

			</div>
			<!-- profile left end -->


			<!-- profile right start -->
			<div class="profile_rightcol">

                <script type="text/javascript">
                var WebPhotoID = '';
                
                function toggleDrop(){
                    $('#gallery_drop').slideToggle('fast');
                }
                
                function showPhoto(index){
                
                    $('#imgMain').attr('src',photos[index][0]);
                    $('#divCaption').html(photos[index][2]);
                    
                    var Prev = index-1;
                    var Next = index+1;
                    var Curr = index+1;
                    
                    $('#hCurrentIndex').html('Viewing '+Curr+' of ' +photos.length);
                    
                    $('#aPreviousImage').attr('href','javascript:showPhoto('+Prev+')');
                    $('#aNextImage').attr('href','javascript:showPhoto('+Next+')');

                    if(Prev<0){
                        $('#aPreviousImage').fadeOut('fast');
                    }else{
                        $('#aPreviousImage').fadeIn('fast');
                    }
                    
                    if(Next>=photos.length){
                        $('#aNextImage').fadeOut('fast');
                    }else{
                        $('#aNextImage').fadeIn('fast');
                    }
                    
                    $('#divShowThumbs').hide();
                    $('#divShowPhoto').fadeIn('fast');

                    WebPhotoID = photos[index][1];
                    ViewGallery.GetComments(WebPhotoID, GetComments_Callback);
                    
                    $('#aPostComment').attr('href','javascript:ajaxPostComment("'+WebPhotoID+'")');
                    
                    $('#divComments').fadeIn('fast');
                    $('#CommentList').html('Loading comments');
                    $('#hCurrentIndex').fadeIn('fast');  
                    
                }
                
                function GetComments_Callback(response){
                    
                    if(response.value!=null){

                        var Arr = response.value;
                        var Comments = '';

                        for(var i=0;i<Arr.length;i++){
//                            Comments += '<li class="clearfix">';
//					        Comments += 			'<img src="'+Arr[i].PhotoURL+'" class="commenter_avatar" />';
//					        Comments += 			'<div class="comment_entry">';
//					        Comments += 				'<p class="commenter">';
//					        Comments += 					'<cite><a href="#">'+Arr[i].PhotoURL+'</a></cite><br />';
//					        Comments += 					'<small>Posted 3 days ago</small><br />';
//					        Comments += 					'<span class="comment_edit"><a href="#">Edit</a> <a href="#">X</a></span></p>';
//					        Comments += 				'<p></p>'	;						
//					        Comments += 			'</div>';
//					        Comments += '</li>';

                            Comments += Arr[i].HTML;
					    }
    					
					    $('#ulCommentList').html(Comments);
					    $('#NumberOfComments').html('('+Arr.length+')');
					}
                }
                
                function showThumbs(){
                    $('#divShowPhoto').hide();
                    $('#divShowThumbs').fadeIn('fast');
                    $('#divComments').fadeOut('fast');
                    $('#hCurrentIndex').fadeOut('fast');  
                }
                                
       
                </script>

				<!-- box start -->
				<div class="profile_box">
					<p class="gallery_breadcrumb"><a href="view.aspx?m=<%=ViewingMember.WebMemberID %>"><%=ViewingMember.NickName %></a> / <a href="ViewGallery.aspx?g=<%=DefaultGallery.WebPhotoCollectionID %>"><%=GalleryNameHTML %></a></p>
					<h2><%=GalleryNameHTML %> (<%=NumberOfPhotosHTML %> photos)</h2>
					<h3 id="hCurrentIndex" style="display:<%=DisplayCurrentIndex%>"><%=CurrentlyShowing%></h3>

					<div class="browse_dropdown">
						<a href="javascript:toggleDrop();" class="btn_browse">Browse</a>
						<ul id="gallery_drop" style="display:none;">
							<%=GalleryDropHTML %>
						</ul>
					</div>

					<div class="collapsible_div" id="divShowThumbs" style="display:<%=DisplayGallery%>;">
						<ul class="profile_gallery clearfix">
<%--							<li class="gallery_prev"><a href="#"><img src="images/nspots-prev.gif" alt="previous" /></a></li>
							<li class="gallery_next"><a href="#"><img src="images/nspots-next.gif" alt="next" /></a></li>--%>
                        	<%=GalleryListerHTML %>
                    	</ul>
					</div>
					<!-- open photo -->
					<div class="collapsible_div" id="divShowPhoto" style="display:<%=DisplayPhoto%>;">
						<ul class="profile_gallery clearfix">
							
							<%=PrevPageHTML %>
							<%=NextPageHTML %>

                        	<li class="zoom">
                            	<img src="<%=DefaultPhotoURL %>"  alt="thumb" id="imgMain" />
                                <p class="cat_details" id="divCaption"><%=DefaultPhotoCaption %></p>
                            </li>
                    	</ul>
					</div>
					
				</div>
				<!-- box end -->


            <!-- box start -->
            <div class="profile_box" id="divComments"  style="display:<%=DisplayComments%>">
                <a name="comments"></a>
                <h4 class="box_title collapsible">
                    Comments <span id="NumberOfComments">(<%=NumberOfComments %>)</span></h4>
                <p class="add_profile_comments">
                    <%if (IsLoggedIn)
                      { %>
                    <a href="javascript:showPostComment();void(0);">
                        <%}
                      else
                      {%>
                        <a href="<%=LoginUrl %>">
                            <%} %>Post comment</a></p><br />
                <div id="divNewComment" style="display: none; width: 100%;">
                    <p class="align_right">
                        <textarea rows="5" id="txtNewComment" style="width: 100%;"></textarea><br />
                        <a href="javascript:cancelShowPostComment();void(0);">cancel</a> &nbsp;&nbsp;|&nbsp;&nbsp;
                        <a href="javascript:ajaxPostComment('<%=DefaultWebPhotoID %>');void(0);">post</a></p>
                </div>
                <div class="collapsible_div">
                
                    <ul class="profile_commentlist" id="ulCommentList">
                           <%=DefaultPhotoComments %>
                    </ul>
                </div>
            </div>


			</div>
			<!-- profile right end -->
			
			


	</div>
	<!-- middle end -->
    <script type="text/javascript" src="lib/jQuery.js"></script>
    
    <script type="text/javascript">   
    
    function showPostComment(){
        $('#divNewComment').fadeIn('fast');
    }

    function cancelShowPostComment(){
        $('#divNewComment').fadeOut('fast');
        $('#txtNewComment')[0].value='';

    }
    
    function ajaxPostComment(WebID){
        var txtNewComment = $('#txtNewComment')[0];
        
        // make sure the comment isnt blank
        if(txtNewComment.value!=""){
            
            ViewGallery.PostComment(WebID, txtNewComment.value, ajaxPostComment_Callback);
        }
    }

    function ajaxPostComment_Callback(response){

    if(response.value!=null){
    
        var divNewComment = $('#divNewComment');
        var txtNewComment = $('#txtNewComment')[0];
        var ulCommentList = $('#ulCommentList');
        var NumberOfComments = $('#NumberOfComments');

        ulCommentList.html(response.value.HTML + ulCommentList.html());

        txtNewComment.value = '';
        divNewComment.css('display','none');
        
        NumberOfComments.html('('+response.value.TotalNumberOfComments+')'); 

    }else {
        alert('Ooops, there was a problem posting your comment, please try again!');
    }
}
    </script>

    
    <%=JSPhotoArrayHTML %>
    
    
</asp:Content>
