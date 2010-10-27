var defaultCommentFunction;
var commentType;
var ObjectWebId;

function ajaxPostComment(type, WebID,attempt){
    attempt = (attempt==null) ? 1 : attempt;
    var txtNewComment = $('#txtNewComment' + WebID)[0];
    if(txtNewComment.value!=""){
    alert(txtNewComment.value);
        $('#btnPostComment' + WebID).attr('disabled','true');
        $('#btnCancel').attr('disabled','true');     
        alert( type+'\n'+  
            WebID+'\n'+txtNewComment.value+'\n'+attempt);
        Comments.PostComment(type, WebID, safeHTML(txtNewComment.value), attempt, ajaxPostComment_Callback);  
    }
}

function ajaxPostComment_Callback(response,args){
alert('ajaxPostComment_Callbac'); 
    if(response.value!=null){  
        alert(response.value.Text);        
        $('#divNewComment' + args.args.WebID).css('display','none');
        $('#txtNewComment' + args.args.WebID)[0].value = '';
        $('#divNewComment' + args.args.WebID).appendTo('#divNewCommentHolder' + args.args.WebID);
        $('#ulCommentList' + args.args.WebID).html(response.value.HTML + $('#ulCommentList' + args.args.WebID).html());
        
        updateCommentCount(args.args.type,response.value.TotalNumberOfComments,args.args.WebID);
        
        $('#btnPostComment' + args.args.WebID).removeAttr("disabled");
        $('#btnCancel').removeAttr("disabled");
        $('#pBeFirst').remove();
        
          
    }else {        
        //connection error
        if(response.error.Status==0){
          if(args.args.Attempt<3){
            ajaxPostComment(args.args.WebID,args.args.CommentText,args.args.Attempt+1)
          }else{
            $('#btnPostComment' + args.args.WebID).removeAttr("disabled");
            $('#btnCancel').removeAttr("disabled");
            alert('Ooops, there was a problem posting your comment, please try again in a few moments!');
          }
        }
    }
}

function showPostComment(WebID){    
    $('#btnPostComment' + WebID).unbind("click");
    $('#btnPostComment' + WebID).bind("click", function(){        
        ajaxPostComment(commentType,WebID);
        void(0);
    });

    $('#txtNewComment' + WebID).val("");
    $('#divNewComment' + WebID).appendTo('#divNewCommentHolder' + WebID);
    $('#btnPostComment' + WebID).val("post");
    $('#divNewComment' + WebID).fadeIn('fast');
}

function cancelShowPostComment(WebID){
    $('#divNewComment' + WebID).fadeOut('fast');
    $('#txtNewComment' + WebID)[0].value='';
    $('#divNewComment' + WebID).appendTo('#divNewCommentHolder' + WebID);
}

function deleteComment(type,WebID,ObjWebID){   
    $('#divNewComment' + ObjWebID).appendTo('#divNewCommentHolder' + ObjWebID);
    Comments.DeleteComment(type,WebID,deleteComment_Callback);
}

function deleteComment_Callback(response,args){
    
    if(response!=null && response.value!=null){        
        
        var commentDiv = $('#li' + args.args.WebID);
        $('#edit' + args.args.WebID).remove();
        $('#reply' + args.args.WebID).remove();
        $('#delete' + args.args.WebID).remove();
        //commentDiv.remove();
        var commentBody = $('#commentBody' + args.args.WebID);
        commentBody.html('<i style="font-weight:lighter;color:gray">Comment Deleted</i>');
        
        $('#divNewComment' + args.args.WebID).fadeOut('fast');
        
        updateCommentCount(args.args.type,response.value,args.args.WebID);        
    }else{
        alert('Ooops, there was a problem deleting your comment, please try again!');
    }
}

function editComment(type,WebID,ObjWebID){
    var commentDiv = $('#' + WebID);
    var txtNewComment = $('#txtNewComment' + ObjWebID);
    var divNewComment = $('#divNewComment' + ObjWebID);
    
    var commentBodyHidden = $('#commentBodyHidden' + WebID);    
    
    $('#txtNewComment' + ObjWebID).val(commentBodyHidden.html());    
    $('#btnPostComment' + ObjWebID).val("update");
    
    $('#btnPostComment' + ObjWebID).unbind("click");
    $('#btnPostComment' + ObjWebID).bind("click", function(){
        performEdit(type,WebID,ObjWebID);
        void(0);
    });
    
    //$('#btnPostComment').attr("onclick","performEdit('" + type + "','" + WebID + "');void(0);");
    divNewComment.appendTo(commentDiv);
    divNewComment.fadeIn('fast');      
}

function performEdit(type,WebID,ObjWebID)
{
    if($('#txtNewComment' + WebID).val()!="")
    {
        var txtNewComment = $('#txtNewComment' + ObjWebID);
        Comments.UpdateComment(type, WebID, safeHTML(txtNewComment.val()),ObjWebID, performEdit_Callback);
    }
}

function performEdit_Callback(response,args){
    if(response!=null && response.value!=null){
        
        $('#divNewComment' + args.args.ObjWebID).fadeOut('fast');
        
        $('#commentBodyHidden' + args.args.WebID).html($('#txtNewComment' + args.args.ObjWebID).val());        
        $('#commentBody' + args.args.WebID).html(response.value);
        //$('#commentBody' + args.args.WebID).html($('#txtNewComment').val());
        $('#txtNewComment' + args.args.ObjWebID).val("");
        $('#divNewComment' + args.args.ObjWebID).appendTo('#divNewCommentHolder' + args.args.WebID);
    }else{
        alert('Ooops, there was a problem editing your comment, please try again!');
    }
}

function replyToComment(type,WebID,ObjWebID){    
    var commentDiv = $('#' + WebID);    
    var txtNewComment = $('#txtNewComment' + ObjWebID);
    var divNewComment = $('#divNewComment' + ObjWebID);
    
    txtNewComment.val("");
    $('#btnPostComment' + ObjWebID).val("post");
    
    $('#btnPostComment' + ObjWebID).unbind("click");
    $('#btnPostComment' + ObjWebID).bind("click", function(){
        performReply(type,WebID,ObjWebID);
        void(0);
    });
    
    //$('#btnPostComment');.attr("onclick","performReply('" + type + "','" + WebID + "');void(0);");    
    divNewComment.appendTo(commentDiv);
    divNewComment.fadeIn('fast');    
}

function performReply(type,WebID,ObjWebID)
{
    if($('#txtNewComment' + ObjWebID).val()!="")
    {
        var txtNewComment = $('#txtNewComment' + ObjWebID);
        Comments.PostReply(type,WebID,safeHTML(txtNewComment.val()),ObjWebID,performReply_Callback);
    }
}


function performReply_Callback(response,args){    
    if(response!=null && response.value!=null){
    
        $('#divNewComment' + args.args.ObjWebID).appendTo('#divNewCommentHolder' + args.args.ObjWebID);
        
        var liComment = $('#li' + args.args.WebID);
        var ol = liComment.parents("#jqCmtOl");
        var appendOLHTML = '';
        
        if( ol.html() == null )
        {
            ol = liComment.children("#jqCmtOl");
        }
        
        if( ol.html() == null )
        {
            appendOLHTML = '<ol class="jqCmtOl" id="jqCmtOl"></ol>';
            liComment.html(liComment.html() + appendOLHTML);
            ol = liComment.children("#jqCmtOl");
        }
        
        
        $('#divNewComment' + args.args.ObjWebID).fadeOut('fast');
        $('#commentBodyHidden' + args.args.WebID).html($('#txtNewComment' + args.args.ObjWebID).val());        
        ol.html(ol.html() + response.value.HTML);
        $('#txtNewComment' + args.args.ObjWebID).val("");
        $('#divNewComment' + args.args.ObjWebID).fadeOut('fast');
        
        updateCommentCount(args.args.type,response.value.TotalNumberOfComments,args.args.WebID);        
    }else{
        alert('Ooops, there was a problem posting your comment, please try again!');
    }
}


function getComments(type,WebID)
{   
    Comments.GetComments(WebID,type,getComments_Callback);    
}


function getComments_Callback(response,args){
    if(response!=null && response.value!=null){
    //alert(args.args.WebID);
        $('#ulCommentList' + args.args.WebID).html('');        
        $('#ulCommentList' + args.args.WebID).html(response.value.CommentHTML);
        updateCommentCount(args.args.type,response.value.TotalNumberOfComments);

//        $('#ulCommentList').html('');        
//        $('#ulCommentList').html(response.value.CommentHTML);
//        updateCommentCount(args.args.type,response.value.TotalNumberOfComments);

    alert(response.value.CommentHTML);       
    }else{
        alert('Ooops, there was a problem retrieving comments, please try again!');
    }
}

function safeHTML(value){
    value = value.replace(/&/g,'&amp;')  
                .replace(/</g,'&lt;')  
                .replace(/>/g,'&gt;')  
                .replace(/\'/g,'&#39;')  
                .replace(/"/g,'&quot;');
       
    var re_nlchar = '';         
    value = escape(value);
    
    if(value.indexOf('%0D%0A') > -1){
        re_nlchar = /%0D%0A/g ;
    }else if(value.indexOf('%0A') > -1){
        re_nlchar = /%0A/g ;
    }else if(value.indexOf('%0D') > -1){
        re_nlchar = /%0D/g ;
    }
    
    if( re_nlchar != '')
    {
        value = value.replace(re_nlchar,'<br />')
    }
    
    return unescape( value );
}

function updateCommentCount(type,cnt,WebID)
{  
    if( type == 'Member')
    { 
        $('#spanNumberOfComments1').html(cnt);
    }
    
    $('#spanNumberOfComments2').html(cnt);
   // $('#spanNumberOfComments3' + WebID).html('(' + cnt + ')');
    $('#spanNumberOfComments3').html('(' + cnt + ')');
}