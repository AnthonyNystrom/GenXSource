
function vote(param, WebID, dir){

    var up = $('#VoteUp'+WebID);
    var down = $('#VoteDown'+WebID);
    
    up.addClass('voted');
    down.addClass('voted');
    up.attr('href','#');
    down.attr('href','#');
    
    MemberProfilePage.Vote(param, WebID, dir, vote_Callback);
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

function showPostComment(){
    $('#divNewComment').fadeIn('fast');
}

function cancelShowPostComment(){
    $('#divNewComment').fadeOut('fast');
    $('#txtNewComment')[0].value='';

}

var showingEdit = false;
function showEditAboutMe(){
 
    var divEditAboutMe = $('#divEditAboutMe');
    
    if(!showingEdit){
       
        var iframeAboutMe = $('#iframeAboutMe');
        var txtAboutMe = $('#txtAboutMe');
        
        var height = (iframeAboutMe.height() < 150) ? 150 : iframeAboutMe.height();
        
        txtAboutMe.height(height);
        
        txtAboutMe.value = 'loading..';
        
        iframeAboutMe.hide();
        divEditAboutMe.fadeIn('fast');
        
        ajaxGetAboutMe();
        
        showingEdit = true;
   }
}

function cancelEditAboutMe(){
    $('#divEditAboutMe').hide();
    $('#divAboutMe').fadeIn('fast');
    showingEdit = false;
}


function ajaxGetAboutMe(){

    MemberProfilePage.GetAboutMeText(ajaxGetAboutMe_Callback)
}


function ajaxGetAboutMe_Callback(response){

    $('#txtAboutMe')[0].value = response.value;
}


function ajaxEditAboutMe(){

    var txtAboutMe = $('#txtAboutMe')[0];
    MemberProfilePage.UpdateAboutMe(txtAboutMe.value, ajaxEditAboutMe_Callback)
}


function ajaxEditAboutMe_Callback(response){

    if(response.value!=null){
        var iframeAboutMe = $('#iframeAboutMe');
        var divEditAboutMe = $('#divEditAboutMe');
        
        iframeAboutMe.css('display','block');
        divEditAboutMe.css('display','none');
        
        showingEdit = false;
        
        iframeAboutMe.attr('src',response.value);

    }else {
        alert('Ooops, there was a problem updating your Profile, please try again!');
    }
}

function ajaxPostComment(type, WebID){
    var txtNewComment = $('#txtNewComment')[0];
    
    // make sure the comment isnt blank
    if(txtNewComment.value!=""){
        
        MemberProfilePage.PostComment(type, WebID, txtNewComment.value, ajaxPostComment_Callback);
    }
}

function ajaxPostComment_Callback(response){

    if(response.value!=null){
    
        var divNewComment = $('#divNewComment');
        var txtNewComment = $('#txtNewComment')[0];
        var ulCommentList = $('#ulCommentList');
        var spanNumberOfComments1 = $('#spanNumberOfComments1');
        var spanNumberOfComments2 = $('#spanNumberOfComments2');
        var spanNumberOfComments3 = $('#spanNumberOfComments3');    

        ulCommentList.html(response.value.HTML + ulCommentList.html());

        txtNewComment.value = '';
        divNewComment.css('display','none');
        
        if(spanNumberOfComments1!=null){
            spanNumberOfComments1.html(response.value.TotalNumberOfComments); 
        }
        if(spanNumberOfComments2!=null){
            spanNumberOfComments2.html(response.value.TotalNumberOfComments); 
        }
        if(spanNumberOfComments3!=null){
            spanNumberOfComments3.html('('+response.value.TotalNumberOfComments+')');  
        }
        
          
    }else {
        alert('Ooops, there was a problem posting your comment, please try again!');
    }
}

function updateCurrentTab(tabType){
    $('#contentTab1').removeClass('current');
    $('#contentTab2').removeClass('current');
    $('#contentTab'+tabType).addClass('current');
}

var cachedListerContent = new Array();

function ajaxGetListerContent(webMemberID, tabType, Page){

    updateCurrentTab(tabType);

    // dont need to reload the gallery
    if(tabType!=2){

        if(cachedListerContent[tabType]==null){
            MemberProfilePage.GetListerContent(webMemberID, tabType+0, Page, ajaxGetListerContent_Callback)
        }else{
            updateListerContent(cachedListerContent[tabType]);
        }
    }else{

        updateListerContent('');
        showGalleryD(webMemberID);
    }
}

function showGalleryD(webMemberID){
             
        var s1 = new SWFObject("gallery.swf","gallery","628","500","7");
        s1.addParam("allowfullscreen","true");
        s1.addParam("autostart","true");
        s1.addParam('bgcolor','#FFFFFF');
        s1.addVariable("xmlfile","MyImagesXML.aspx?m="+webMemberID);
        s1.addVariable("width","525");
        s1.addVariable("height","396");
        s1.write("ulContentLister");     
        
        $('#pPager').html('');
}


function ajaxGetListerContent_Callback(response){

    if(response.value!=null){
        
        updateListerContent(response.value.HTML);
        $('#pPager').html(response.value.PagerHTML);
        //cachedListerContent[response.value.TabType] = response.value.HTML;
		$(".profile_vid_list li:even").addClass("clear");
    }else {
        alert('Ooops, there was a problem with your request, please try again!');
    }
    

}

function updateListerContent(html){
    $('#ulContentLister').html(html);
}

function changeProfilePhoto(html){
    $('#divChangePhoto').fadeIn('fast');
}

function subscribeToMember(webMemberID, link){
    MemberProfilePage.SubscribeToMember(webMemberID, subscribeToMember_Callback);
}

function subscribeToMember_Callback(response){

    $('#spanSubscribe').html("&nbsp;<img src='images/check.gif' />")
}

function addTofriends(webMemberID, link){
    MemberProfilePage.AddToFriends(webMemberID, addToFriends_Callback);
}

function addToFriends_Callback(response){
    if(response.value!=null){
        $('#spanAddToFriends').html("&nbsp;<img src='images/check.gif' />");
    }else {
        alert('Ooops, there was a problem with your request, please try again!');
    }
}

function blockMember(webMemberID, link){
    MemberProfilePage.BlockMember(webMemberID, blockMember_Callback);
}

function blockMember_Callback(response){
    $('#spanBlockMember').html("&nbsp;<img src='images/check.gif' />");
}

function addToFavourites(webMemberID, link){
    MemberProfilePage.AddToFavourites(webMemberID, addToFavourites_Callback);
}

function addToFavourites_Callback(response){

    if(response.value!=null){
        var spanAddFavourites = $('#spanAddFavourites').html("&nbsp;<img src='images/check.gif' />");
    }else if(response.error!=null){
        alert(response.error);
    }

}


$(document).ready(function(){

	$("#txtAboutMe").keydown(function(e){
		if(e.keyCode==13){
		    $("#txtAboutMe").height($("#txtAboutMe").height()+16);
		}
	});

}); //close doc ready

function PageGallery(webMemberID, page, numberOfGalleries){
    
    $('#aGallNext').attr('href','javascript:PageGallery("'+webMemberID+'",'+(page+1)+','+numberOfGalleries+')');
    $('#aGallPrev').attr('href','javascript:PageGallery("'+webMemberID+'",'+numberOfGalleries+')');
    
    MemberProfilePage.GetPhotoLister(webMemberID,page, pageGallery_Callback);
     
    if(page==0){
        $('#aGallPrev').fadeOut('fast');
    }else{
        $('#aGallPrev').fadeIn('fast');
    }
    
    if((page+1)<=(numberOfGalleries/3)){
        $('#aGallNext').fadeIn('fast');
    }else{
        $('#aGallNext').fadeOut('fast');
    }
}

function pageGallery_Callback(response){

    if(response.value!=null){
        $('#sGall').html(response.value.HTML);
    }else{
        alert('try again');
    }

}

