var imgSkipDisabled = 'images/btn-skip-off.gif';
var imgSkipEnabled = 'images/btn-skip.gif';
var AskAFriendWebId = '';
//document.getElementsByTagName('body').onuload='document.close();';    

function ajaxSkipQuestion(){

    var iSkip = document.getElementById('iSkip');
    iSkip.src = imgSkipDisabled
    
   // alert('test1 ');
    AskAFriendPage.SkipQuestion(GetSkipQuestionHTML_Callback);		
  
}
   
    
function SubmitResponse(SelectedValue){

    disableVoteOptions(true);

    AskAFriendPage.ResponseQuestion(SelectedValue, GetNextQuestionHTML_Callback);
}

function updatePage(response){

/*
        var ulSelection = document.getElementById('ulSelection');
        var h3Question = document.getElementById('h3Question');
        var spanLastAAF = document.getElementById('spanLastAAF');
        var pBookmarks = document.getElementById('pBookmarks');
        var divNewComment = document.getElementById('divNewComment');
        
        
        ulSelection.innerHTML = response.value.HTML;
        h3Question.innerHTML = response.value.Question;
        pBookmarks.innerHTML = response.value.Bookmarks;
        //divNewComment.innerHTML = response.value.CommentPost;
  */  
  //alert(response.value.HTML);
  $('#ulSelection').html(response.value.HTML);
  $('#h3Question').html(response.value.Question);
  $('#pBookmarks').html(response.value.Bookmarks);
        if(response.value.LastAAF!=""){
    //        spanLastAAF.innerHTML = response.value.LastAAF;
            $('#spanLastAAF') .html(response.value.LastAAF);
        }
        
        getComments("AskAFriend",response.value.WebAskAFriendID);
}


function disableVoteOptions(disable){

    var checkboxes = document.getElementsByName('rbResponse');
    
    for(var i=0;i<checkboxes.length;i++){
        checkboxes[i].disabled = disable;
        
        if(!disable){
            checkboxes[i].checked = false;
        }
    }
}

var error = null;
function GetNextQuestionHTML_Callback(response){

    if(response.value != null){
        //updatePage(response);
        //window.location='askafriend.aspx';
     //   disableVoteOptions(true);
   //   updatePage(response);
        
        //window.location.reload();
        if(returnToDash){
            location.href = '/dashboard';
        }else{
            location.href = '/ask';
        }
        
        
  //      disableVoteOptions(false);
     }else {
        error = response.error
        alert('Ooops, there was a problem sending your vote, please try again!');
        disableVoteOptions(false);
    }
}

function GetSkipQuestionHTML_Callback(response){

    if(response.value != null){
        updatePage(response);
     //   alert('test2 ');
     }else {

       // alert('Ooops, there was a problem skipping your Question, please try again!');
        disableVoteOptions(false);
    }
    
    var iSkip = document.getElementById('iSkip');
    iSkip.src = imgSkipEnabled;
}

function getDomain() {
    if ( this.domainUrl == null ) {
        this.domainUrl = String( document.location );
        while ( this.domainUrl.lastIndexOf( '/' ) > 8 ) {
            this.domainUrl = this.domainUrl.substring( 0, this.domainUrl.lastIndexOf( '/' ) );
        }
    }
    return this.domainUrl;
}


//function showPostComment(){
//    var divNewComment = document.getElementById('divNewComment');
//    divNewComment.style.display = 'block';
//}

//function cancelShowPostComment(){
//    var divNewComment = document.getElementById('divNewComment');
//    divNewComment.style.display = 'none';
//    
//    var txtNewComment = document.getElementById('txtNewComment');
//    txtNewComment.value = '';
//    
//}


//function ajaxPostComment(WebQuestionID){
//    var txtNewComment = document.getElementById('txtNewComment');
//    
//    // make sure the comment isnt blank
//    if(txtNewComment.value!=""){
//        
//        AskAFriendPage.PostComment(WebQuestionID, txtNewComment.value, ajaxPostComment_Callback);
//    }
//}

//function ajaxPostComment_Callback(response){
//    
//        if(response.value!=null){
//        
//            var divNewComment = document.getElementById('divNewComment');
//            var txtNewComment = document.getElementById('txtNewComment');
//            var olComments = document.getElementById('olComments');
//            
//            txtNewComment.value = '';
//            divNewComment.style.display = 'none';
//            
//            olComments.innerHTML = response.value.HTML + olComments.innerHTML;  
//        }else {

//            alert('Ooops, there was a problem posting your comment, please try again!');
//        
//        }
//   
//}
