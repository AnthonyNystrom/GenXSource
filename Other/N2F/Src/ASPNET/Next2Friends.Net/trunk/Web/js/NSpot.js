
function showPostComment(){
    var divNewComment = document.getElementById('divNewComment');
    divNewComment.style.display = 'block';
}

function cancelShowPostComment(){
    var divNewComment = document.getElementById('divNewComment');
    divNewComment.style.display = 'none';
    
    var txtNewComment = document.getElementById('txtNewComment');
    txtNewComment.value = '';
    
}


function showEditAboutMe(){
 
    var divEditAboutMe = document.getElementById('divEditAboutMe');
    
    if(divEditAboutMe.style.display == 'none'){
       
        var divAboutMe = document.getElementById('divAboutMe');
        var txtAboutMe = document.getElementById('txtAboutMe');
        
        var height = (divAboutMe.clientHeight < 150) ? 150 : divAboutMe.clientHeight;
        
        txtAboutMe.style.height = height+'px';
        
        txtAboutMe.value = 'loading..';
        
        divAboutMe.style.display = 'none';
        divEditAboutMe.style.display = 'block';
        
        ajaxGetAboutMe();
    }
}

function cancelEditAboutMe(){
 
    var divEditAboutMe = document.getElementById('divEditAboutMe');
    var divAboutMe = document.getElementById('divAboutMe');
    
    divAboutMe.style.display = 'block';
    divEditAboutMe.style.display = 'none';
        
}


function ajaxGetAboutMe(){

    MemberProfilePage.GetAboutMeText(ajaxGetAboutMe_Callback)
}


function ajaxGetAboutMe_Callback(response){

    var txtAboutMe = document.getElementById('txtAboutMe');
    txtAboutMe.value = response.value;
}


function ajaxEditAboutMe(){

    var txtAboutMe = document.getElementById('txtAboutMe');
    MemberProfilePage.UpdateAboutMe(txtAboutMe.value, ajaxEditAboutMe_Callback)
}


function ajaxEditAboutMe_Callback(response){

    if(response.value!=null){
        var divAboutMe = document.getElementById('divAboutMe');
        var divEditAboutMe = document.getElementById('divEditAboutMe');
        
        divAboutMe.style.display = 'block';
        divEditAboutMe.style.display = 'none';
        
        divAboutMe.innerHTML = response.value;

    }else {
        alert('Ooops, there was a problem updating your Profile, please try again!');
    }
}

function ajaxPostComment(type, WebID){
    var txtNewComment = document.getElementById('txtNewComment');
    
    // make sure the comment isnt blank
    if(txtNewComment.value!=""){
        
        NSpotPage.PostComment(type, WebID, txtNewComment.value, ajaxPostComment_Callback);
    }
}

function ajaxPostComment_Callback(response){

    if(response.value!=null){
    
        var divNewComment = document.getElementById('divNewComment');
        var txtNewComment = document.getElementById('txtNewComment');
        var liCommentList = document.getElementById('liCommentList');
        var spanNumberOfComments1 = document.getElementById('spanNumberOfComments1');
        var spanNumberOfComments2 = document.getElementById('spanNumberOfComments2');
        var spanNumberOfComments3 = document.getElementById('spanNumberOfComments3');    
           
            
        txtNewComment.value = '';
        divNewComment.style.display = 'none';
        
        spanNumberOfComments1.innerHTML = response.value.TotalNumberOfComments; 
        
        if(spanNumberOfComments2!=null){
            spanNumberOfComments2.innerHTML = response.value.TotalNumberOfComments; 
        }
        spanNumberOfComments3.innerHTML = '('+response.value.TotalNumberOfComments+')';  
        
        liCommentList.innerHTML = response.value.HTML + liCommentList.innerHTML;   
    }else {
        alert('Ooops, there was a problem posting your comment, please try again!');
    }
}

function updateCurrentTab(tabType){
    document.getElementById('contentTab1').className  = '';
    document.getElementById('contentTab2').className  = '';
    document.getElementById('contentTab3').className  = '';
    
    document.getElementById('contentTab'+tabType).className  = 'current';
}

var cachedListerContent = new Array();

function ajaxGetListerContent(webMemberID, tabType){

    updateCurrentTab(tabType);
    
    if(cachedListerContent[tabType]==null){
        MemberProfilePage.GetListerContent(webMemberID, tabType+0, ajaxGetListerContent_Callback)
    }else{
        updateListerContent(cachedListerContent[tabType]);
    }
}

function ajaxGetListerContent_Callback(response){
    updateListerContent(response.value.HTML);
    cachedListerContent[response.value.TabType] = response.value.HTML;

}

function updateListerContent(html){
    document.getElementById('ulContentLister').innerHTML = html;
}

function changeProfilePhoto(html){

    var divChangePhoto = document.getElementById('divChangePhoto');
    divChangePhoto.style.display = 'block';
    
}

function subscribeToMember(webMemberID, link){
    MemberProfilePage.SubscribeToMember(webMemberID, subscribeToMember_Callback);
    //link.href = 'javascript:void();';
}

function subscribeToMember_Callback(response){

    var spanSubscribe = document.getElementById('spanSubscribe');
    spanSubscribe.innerHTML = "&nbsp;<img src='images/check.gif' />";
}

function addTofriends(webMemberID, link){
    MemberProfilePage.AddToFriends(webMemberID, addToFriends_Callback);
    //link.href = 'javascript:void();';
}

function addToFriends_Callback(response){

    
    var spanAddToFriends = document.getElementById('spanAddToFriends');
    spanAddToFriends.innerHTML = "&nbsp;<img src='images/check.gif' />";

}

function blockMember(webMemberID, link){
    MemberProfilePage.BlockMember(webMemberID, blockMember_Callback);
    //link.href = 'javascript:void();';
}

function blockMember_Callback(response){

    
    var spanBlockMember = document.getElementById('spanBlockMember');
    spanBlockMember.innerHTML = "&nbsp;<img src='images/check.gif' />";

}

function addToFavourites(webMemberID, link){
    MemberProfilePage.AddToFavourites(webMemberID, addToFavourites_Callback);
}

function addToFavourites_Callback(response){

    if(response.value!=null){
        var spanAddFavourites = document.getElementById('spanAddFavourites');
        spanAddFavourites.innerHTML = "&nbsp;<img src='images/check.gif' />";
    }else if(response.error!=null){
        alert(response.error);
    }

}


