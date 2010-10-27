<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Feed.aspx.cs" Inherits="Feed" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript" src="/js/feed.js"></script>
<script type="text/javascript" src="/lib/popup.js?c=1"></script>
<script type="text/javascript" src="/lib/swfobject.js"></script>
<script>
function mp3(file,div,title){

    var so = new SWFObject("/ep_player.swf", "ep_player", "301", "16", "9", "#FFFFFF");
    so.addVariable("skin", "/mp3/skins/micro_player/skin.xml");
    so.addVariable("autoplay", "false");
    so.addVariable("shuffle", "false");
    so.addVariable("key", "FM37NL9CEFR0HFRWDSRR");
    so.addVariable("repeat", "false");
    so.addVariable("buffertime", "1");
    so.addParam("wmode","opaque");
    so.addParam("allowscriptaccess", "always");
    file = 'http://www.next2friends.com' + file;
    so.addVariable("file", "<location>"+ file +"</location><creator></creator><title>"+title+"</title>");
    
    so.write(div);
}
</script>
	<div id="middle" class="clearfix no_subnav">	
	<div class="profile_leftcol">
	
		 <div class="profile_box">
		    <p id="pStatus"><strong>I am:</strong> <%=MemberStatus %></p>
	        <a href="javascript:displayEditStatus()">Change status</a>
<%--	        <p></p>
	        <p id="plocation"><strong>My Location</strong> <%=MemberLocation %></p>
	        <a href="javascript:displayLocation()">Change my location</a>--%>
	     </div>
	     
	     
	     <div>
	        <a href="/invite">
	        <img src="/images/invite-now.jpg" style="border:1px;border-color:#000000" />
	       </a>
	     </div>
	     <p></p>
	     	     
	     
	          <div class="profile_box">
                <h3 class="profile_name"> Friends Requests</h3>
                <div class="collapsible_div" id="divfr">
                        <%=FriendRequestLister%>
                </div>
                    <p class="view_all">
						<a href="/friend-requests">See all requests</a>
					</p>
            </div>
            
           
            	     <div class="profile_box">
	      <h3 class="profile_name"> Profile Visitors</h3>
	    
	     <div class="collapsible_div">
	     
	     <%if (ViewCount == 0)
         { %>
         You have no recent visitors
         <%} %>
	     
	     <%if (ViewCount > 0)
         { %>
        <ul class="friends_list">
            <div><span style="color:#666666;"></span>
            </div>
            <%=ViewLister%>                        
        </ul>
        <%} %>        
                </div>
	     </div>
            
            <div class="profile_box">
                <h3 class="profile_name">
                    Proximity Tags</h3>
                <div class="collapsible_div">
                    <%if (NumberOfProximityTags == 0)
                      { %>
                    You have no recent visitors
                    <%} %>
                    <%if (NumberOfProximityTags > 0)
                      { %>
                    <ul class="friends_list">
<%--                        <div>
                            <span style="color: #666666;"></span>
                        </div>--%>
                        <%=ProximityTagsLister%>
                    </ul>
                    <%} %>
                    <p class="view_all">
						<a href="/proximity-tags">See all proximity tags</a>
					</p>
                </div>
            </div>
	     
	     
	</div>
	  
	<div class="profile_rightcol">
	<!-- box start -->
				<div class="profile_box">
				     <a class="subscribeRSSLink" href="/rss.aspx?feed=dashboard&nickname=<%=member.NickName %>&token=<%=RSSToken %>">subscribe</a>
					<h2>Friend Feed</h2>
		

                    <%=FeedHTML%>
				
	
				</div>
				<!-- box end -->
		</div>	
		
		</div>		

<script type="text/javascript">
    
var statusText = '<%=MemberStatus %>';
var locationText = '<%=MemberLocation %>';

function displayEditStatus(){


    var html='<input type="text" value="'+statusText+'" id="txtStatusText" maxlength="60"  style="padding:3px;width:336px;" /> ';
	//html+='<input type="reset" onclick="cancelStatus()" class="form_btn" value="Cancel" />';
	html+='<input type="button" onclick="saveStatus();" class="form_btn" value="Done" />';
    npopup('Change your status!',html,470,30);
}

function saveStatus() {
    var newStatus = $('#txtStatusText').val();
    Feed.SaveProfileStatus(newStatus,saveStatus_callback);
}


function cancelStatus() {
    $('#divEditStatus').hide();
    $('#txtStatus').get(0).value = statusText;
}

function saveStatus_callback(response) {

    if(response.error==null){
        statusText = $('#txtStatusText').val();
        $('#pStatus').html('<strong>I am:</strong> '+statusText);
        closePopup();
    }else{
        alert("there was a problem with your request, please try again.");
    }
}

function setfr(WebFriendRequestID, value){
    var go = true;
    if(!value){
        if(!confirm("Are you sure you want to ignore this person?")){
            go = false;
        }
    }
    
    if(go){
        Feed.SetFriendStatus(WebFriendRequestID, value, setfr_Callback);
    }
}

function setfr_Callback(response,args){
    if(response.error==null){
            $('#liFR'+args.args.WebFriendRequestID).remove();
            
            if($('#ulFriendRequests').children().length==0){
               $('#divfr').html('No friend requests');
            }
        
    }else{
         alert('Ooops, there was a problem with your request, please try again!');
    }
}

function displayMiniVideo(webVideoID,title){
    var html = '<iframe src="/MiniVideoPage.aspx?v='+webVideoID+'" style="border:0px" frameborder="0" scrolling="no" width="480" height="285"></iframe>';   
    npopup(title,html,525,285);
}


function displayLocation(){
    var html = '<input type="text" value="" id="txtLocationText" maxlength="60"  style="padding:3px;width:300px;" /> ';
	html+='<input type="button" onclick="saveLocation();" id="btnLoc" class="form_btn" value="Set my location" />';
    npopup('Change your location!',html,470,30);
}

function saveLocation() {

    var txt = $('#txtLocationText')
    if(txt.length>0){
    
        var newLocation = $('#txtLocationText').val();
        Feed.SaveLocation(newLocation,false,SaveLocation_callback);  
        
    }else{
    
        var drp = $('#drpLocationText')
        var newLocation = drp.val();
        Feed.SaveLocation(newLocation,true,SaveLocation_callback); 
        
    }

}
function SaveLocation_callback(response) {
        if(response.error==null){
    
        if(response.value.ResponseType==0){
            alert('Location not found');
        }else if(response.value.ResponseType==1){
            closePopup();
            $('#plocation').html('<strong>My Location</strong> '+response.value.LocationText+'</p>');
        }else if(response.value.ResponseType==2){
            
            var html = '<select id="drpLocationText" style="padding:3px;width:300px;" > <option>Multiple locations found</option>';
            var locList = response.value.LocationList;
            for(var i=0;i<locList.length;i++){
                html += '<option value="'+locList[i].Lcid+'">'+locList[i].Text+'</option>';
            }
	        html+='</select> <input type="button" onclick="saveLocation();" id="btnLoc" class="form_btn" value="Set this location" />';
            $('#txtLocationText').remove();
            $('#divpuContent').html(html);
        }else if(response.value.ResponseType==1){
            closePopup();
        }
      }else{
            alert("there was a problem with your request, please try again.");
        
    }
}


function saveStatus_callback(response) {

    if(response.error==null){
        statusText = $('#txtStatusText').val();
        $('#pStatus').html('<strong>I am:</strong> '+statusText);
        closePopup();
    }else{
        alert("there was a problem with your request, please try again.");
    }
}




function dmp(webMemberID){
     var fullName = '';

     for(var i=0;i<memberArray.length;i++){
        if(memberArray[i][0]==webMemberID){
            fullName = memberArray[i][1];
        }
     }

     npopup(fullName,"<div id='divProfileHTML' style='height: 125px;'>Loading profile</div>",535,115);
     Feed.GetMiniProfile(webMemberID,displayMiniProfile_callback);
}

function displayMiniProfile_callback(response){
    if(response.error==null){ 
        $('#divProfileHTML').html(response.value)
    }
}

<%=JsNameString%>
</script>


<div class="innerPopup" id="divEditStatus" style="display:none;"><div class="innerPopupTop"></div><div class="innerPopupContent">
<div class="innerPopupTitle"><a style="float:right;" href="javascript:cancelStatus();">close</a><h3>Edit Status</h3></div><div class="innerPopupBlock">
    <div id="div2">
    		<input type="text" id="txtStatus" maxlength="60"  style="padding:3px;width:336px;" />
			<input type="reset" onclick="cancelStatus()" class="form_btn" value="Cancel" />
			<input type="button" onclick="saveStatus();" class="form_btn" value="Done" />
    </div>
</div></div><div class="innerPopupBottom"></div></div>	

<div class="innerPopup" id="divMiniVideoPlayer" style="display:none;width:520px"><div class="innerPopupTop"></div><div class="innerPopupContent">
<div class="innerPopupTitle"><a style="float:right;" href="javascript:closeMiniVideo();">close</a><h3 id="h2VideoTitle"></h3></div><div class="innerPopupBlock">
    <div id="divMiniVideo">
    		
    </div>
</div></div><div class="innerPopupBottom"></div></div>	

</asp:Content>


