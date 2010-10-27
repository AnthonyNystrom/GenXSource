<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="ViewGallery.aspx.cs" Inherits="ViewGallery" Title="Untitled Page" %>
<%@ Register src="Comments.ascx" tagname="Comments" tagprefix="uc1" %>
<%@ Register src="~/UserControls/ForwardToFriend.ascx" tagname="ForwardToFriend" tagprefix="uc2" %>
<%@ Import Namespace="Next2Friends.Misc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftUpperContentHolder" runat="Server">
    <ul id="addFavUl" class="profile_menu" style="display: none;">					
	<li><p><a href="" id="addFav" onmouseover="return true;" class="add_favorites">Share with Friends</a><span id="spanAddFavourites"></span></p></li>	
</ul>
    <p>    <!-- AddThis Button BEGIN -->
        <script type="text/javascript">addthis_pub  = 'Next2Friends';var addThisWebPhotoID = '';</script>
        <a href="http://www.addthis.com/bookmark.php" onmouseover="return addthis_open(this, '', 'http://www.next2friends.com/gallery/?g=<%=DefaultGallery.WebPhotoCollectionID %>&m=<%=ViewingMember.WebMemberID %>'+addThisWebPhotoID, '<%=MainTitle %>')" onmouseout="addthis_close()" onclick="return addthis_sendto()"><img src="http://s9.addthis.com/button1-bm.gif" width="125" height="16" border="0" alt="" /></a><script type="text/javascript" src="http://s7.addthis.com/js/152/addthis_widget.js"></script>
        <!-- AddThis Button END -->
   </p>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftColContentHolder" runat="Server">
    <uc2:ForwardToFriend ID="forwardToFriend" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="RightColContentHolder" runat="Server">
<script type="text/javascript" src="/lib/jquery-ui.packed.js"></script>
<style type="text/css">
    
div.sliderContainer {
width:268px;
height:28px;
background-image:url(/images/slider-bkg.gif);
background-position:0px 0px;
background-repeat:no-repeat;
position:absolute;
top:6px;
right:10px;
}

.ui-slider {
width: 200px;
height: 16px;
margin:6px 0px 0px 26px;
position: relative;
background-image:none;
cursor:pointer;
}

.ui-slider-handle {
position: absolute;
z-index: 1;
height: 16px;
width: 16px;
top: 0px;
left: 0px;
background-image:url(/images/slider-handle.gif);
}
</style>

                <script type="text/javascript">
                
                var WebPhotoID = '';
                var CommentsArr = new Array();
                var NumberOfPhotos = <%=NumberOfPhotosHTML %>;
                var pageCount=<%=pageCount%>;
                var photoLength=<%=photoLength%>;
                var PhotoCollectionID = '<%=URLPhotoCollectionID %>';
                
                
    function GetPageIndex()
    {
    
        var querystring={};
        var pairs=location.search.substring(1).split("&");
        for(var i=0;i<pairs.length;i++)
        {
            var s=pairs[i].split('=');
            querystring[s[0]]=s[1];
        }
        
        if(querystring['p']==undefined || querystring['p']=='')
            return 1;
          
          
        querystring['p']=(parseInt(querystring['p']));

        if(isNaN(querystring['p']))
            return 1;
        return querystring['p'];
    }
    
    function GetPictureID()
    {
    
        var querystring={};
        var pairs=location.search.substring(1).split("&");
        for(var i=0;i<pairs.length;i++)
        {
            var s=pairs[i].split('=');
            querystring[s[0]]=s[1];
        }

        return querystring['wp'];
    }
    
    function GetPhotoIndex(WebPhId)
    {   
        var idx = 0;
        for( idx = 0; idx < photos.length; idx++)
        {   
            if( photos[idx][1] == WebPhId )
            return idx;
        }
    }
    
    function GetPageDir()
    {
    
        var querystring={};
        var pairs=location.search.substring(1).split("&");
        for(var i=0;i<pairs.length;i++)
        {
            var s=pairs[i].split('=');
            querystring[s[0]]=s[1];
        }
                
        if(querystring['dir']==undefined || querystring['dir']=='')
            return 'c';
          
          

        return querystring['dir'];
    }
    
    function GetNewLocation(currentIndex,toPageIndex)
    {
        var querystring={};
        var pairs=location.search.substring(1).split("&");
        for(var i=0;i<pairs.length;i++)
        {
            var s=pairs[i].split('=');
            querystring[s[0]]=s[1];
        }
        querystring['p']=toPageIndex;

    //if(currentIndex>toPageIndex && toPageIndex==1)
    if(currentIndex==pageCount && toPageIndex==1)
            querystring['dir']='n';
          //else if(currentIndex<toPageIndex && toPageIndex==pageCount)
        else if(currentIndex==1 && toPageIndex==pageCount)
            querystring['dir']='p';
          else if(currentIndex>toPageIndex)
            querystring['dir']='p';
        else if(currentIndex<toPageIndex  )
            querystring['dir']='n';
        else
            querystring['dir']='c';
                  
        var newLocation='?';
        
        for(key in querystring)
        {
        // document.writeln(querystring[key]);
          newLocation=newLocation.concat(key,'=',querystring[key],'&');
         }
          newLocation=newLocation.substring(0,newLocation.length-1);
          
          return newLocation;
       
    }
                
       
                function toggleDrop(){
                    $('#gallery_drop').slideToggle('fast');
                }
                
                function showPhoto(index){
                
                    $('#imgMain').attr('src',photos[index][0]);
                    $('#divCaption').html(photos[index][2]);
                    WebPhotoID = photos[index][1];
                    addThisWebPhotoID = '&wp='+WebPhotoID;
                    
    				<%if (IsLoggedIn)
                    { %>
    				$('#addFav').attr('href','javascript:addToFavourites("Photo","' + WebPhotoID + '")');
    				setForwardToFriend("Photo",WebPhotoID);
    				<%}
                    else
                    {%>
                    $('#addFav').attr('href','<%=LoginUrl%>');
                    <%}%>
                      
                    $('#spanAddFavourites').html('');
                    $('#addFavUl').fadeIn('fast');
                    var pageIndex=GetPageIndex();

                    var Next;
                    var Prev;
                    
                    if((index+1)==photos.length)
                    {
                       Next=0; 
                    }
                    else
                    {
                        Next=index+1; 
                    }
                    
                    if((index)==0)
                    {
                       Prev= photos.length-1;
                        
                    }else
                    {
                        Prev=index-1;
                    }
                    
               
                    
                    //alert(pageCount);
     
                    //$('#hCurrentIndex').html('Viewing '+Curr+' of ' +NumberOfPhotos);
                    if(pageCount>1)
                    {
                    if(index==0)
                       if(pageIndex>1)
                        $('#aPreviousImage').attr('href',GetNewLocation(pageIndex,pageIndex-1));
                       else
                       $('#aPreviousImage').attr('href',GetNewLocation(pageIndex,pageCount));
                    else                  
                        $('#aPreviousImage').attr('href','javascript:showPhoto('+Prev+')');
                    
                    if(Next==0)
                        if(pageIndex==pageCount)
                            $('#aNextImage').attr('href',GetNewLocation(pageIndex,1));
                        else
                            $('#aNextImage').attr('href',GetNewLocation(pageIndex,pageIndex+1));
                       
                    else
                        $('#aNextImage').attr('href','javascript:showPhoto('+Next+')');
                        }
                        else
                        {
                            $('#aPreviousImage').attr('href','javascript:showPhoto('+Prev+')');
                            $('#aNextImage').attr('href','javascript:showPhoto('+Next+')');
                        }
                   
                    $('#divShowThumbs').hide();
                    $('#divShowPhoto').fadeIn('fast');

                    
                    LoadComments(WebPhotoID);
                    
                    $('#divComments').fadeIn('fast');
                    $('#loading').html('- Loading comments');
                    //$('#hCurrentIndex').fadeIn('fast');  
                    
                    //cache next Images
                    $('<img style="display:none;" src="'+photos[Next][0]+'">').appendTo("body");
                    //$('<img style="display:none;" src="'+photos[Prev][0]+'">').appendTo("body");
                }
                
                $(document).ready(function(){
                
                    
                    var WebPhotoID = GetPictureID();
                    var showPhotoIndex;
                    
                    if( WebPhotoID!=null && WebPhotoID != '' )
                    {                                           
                       showPhotoIndex = GetPhotoIndex(WebPhotoID);                       
                    }
                    
                    if( showPhotoIndex != null)
                    {
                        showPhoto(showPhotoIndex);
                    }
                    else
                    {
                    
                    var dir=GetPageDir();
                    if(dir=='p')
                        showPhoto(photos.length-1);
                    else if(dir=='n')
                        showPhoto(0);
                    else
                        $('#divShowThumbs').show();
                    }
                
                });
                
                function LoadComments(WebPhotoID){
                    getComments("Photo",WebPhotoID);
                }
                
                
                function showThumbs(){
                    $('#divShowPhoto').hide();
                    $('#divShowThumbs').fadeIn('fast');
                    $('#divComments').fadeOut('fast');
                    $('#addFavUl').fadeOut('fast');
                    setForwardToFriend("PhotoCollection",PhotoCollectionID);
                    //$('#hCurrentIndex').html('&nbsp;');  
                    $('#ulCommentList').html('');
					$('#NumberOfComments').html('(0)');
					addThisWebPhotoID = '';
					
                }
                

                
                </script>

				<!-- box start -->
				<div class="profile_box">
					<p class="gallery_breadcrumb"><a href="/users/<%=ViewingMember.NickName %>/photos"><%=ViewingMember.NickName %>'s photos</a> / <a href="javascript:showThumbs();"><%=GalleryNameHTML %></a></p>
<%--					<h2 class="left"><%=GalleryNameHTML %></h2> 
					<p class="viewing" id="hCurrentIndex" style="display:none;"></p>--%>
					
					<p class="left"><h2><%=GalleryNameHTML %></h2><%=PagerHTML %></p>
					<%--<p class="viewing" id="hCurrentIndex">&nbsp;</p>--%>
					
					<div class="sliderContainer">
				        <div id='slider' class='ui-slider'>
					        <div class='ui-slider-handle'></div>	
				        </div>
			        </div>
			        

					<div class="browse_dropdown">
						<a href="javascript:toggleDrop();" class="btn_browse">Browse</a>
						<ul id="gallery_drop" style="display:none;">
							<%=GalleryDropHTML %>
						</ul>
					</div>

					<div class="collapsible_div" id="divShowThumbs" style="display:none;">
						<ul class="profile_gallery clearfix">
                        	<%=GalleryListerHTML %>
                    	</ul>
					</div>
					
					<!-- open photo -->
					<div class="collapsible_div" id="divShowPhoto" style="display:none;">
					
						
						
						<p class="gallery_next_prev right">
						    <a href="#" id="aPreviousImage"><img src="/images/photo-prev.gif" alt="previous" /> Previous</a> 
						    <a href="#" id="aNextImage">Next <img src="/images/photo-next.gif" alt="next" /></a>
						</p>
						
												
					

                        	<div class="zoom">
                            	<img src=""  alt="thumb" id="imgMain" />
                                <p class="cat_details" id="divCaption"></p>
                            </div>
                    	
					</div>
					
				</div>
				<!-- box end -->
				
				<div class="profile_box" id="divComments" style="display:none">
                    <uc1:Comments ID="Comments1" runat="server" />
                </div>

				

    <script type="text/javascript">   
    <%=DefaultTab %>
    $(document).ready(function(){

	
	$("#slider").slider({
			slide: function(e,ui) {
				//$('.lister .icon').css('width',ui.value);
				//$('.lister .icon').css('height',ui.value);
				//$('.lister p').css('font-size',(ui.value+40)+'%');
			},
			min: 32,
			max: 96,
			startValue: 64
	});	
	
	});

    </script>

    <%=JSPhotoArrayHTML %>
</asp:Content>
