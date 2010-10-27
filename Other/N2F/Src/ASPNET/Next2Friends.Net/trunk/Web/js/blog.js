var showingEdit = false;

function showEditBlog(WebBlogID){

    if(WebBlogID == null )
        WebBlogID = '';
 
    if(!showingEdit){         
        $('#btnPost' + WebBlogID).hide();
        $('#btnEdit' + WebBlogID).show();
        
        $('#txtBlogTitle' + WebBlogID).val( $('#txtBlogTitleSource' + WebBlogID).val() );
        $('#txtBlogBody' + WebBlogID).val( $('#txtBlogBodySource' + WebBlogID).val() );
        
        $('#divEditBlog' + WebBlogID).fadeIn('fast');
        $('#divBlog').hide();
       
        showingEdit = true;
   }
}

function showPostBlog(WebBlogID){
 
    if(WebBlogID == null )
        WebBlogID = '';
 
    var divEditBlog = $('#divEditBlog');
    
    if(!showingEdit){ 
        $('#btnPost').show();
        $('#btnEdit').hide();
        
        $('#txtBlogTitle').val( '' );
        $('#txtBlogBody').val( '' );
        
        $('#divBlog').hide();
        $('#divComments').hide();        
        
        divEditBlog.fadeIn('fast');
               
        showingEdit = true;
   }
}

function cancelEditBlog(WebBlogID){
    
    if(WebBlogID == null )
        WebBlogID = '';
    
    $('#divEditBlog' + WebBlogID).hide();
    $('#divBlog').show();
    $('#divComments').show();
    showingEdit = false;
}

function ajaxEditBlog(WebBlogID){
    var title = $('#txtBlogTitle' + WebBlogID).val();
    var body = $('#txtBlogBody' + WebBlogID).val(); 
       
    $('#btnEdit' + WebBlogID).attr('disabled','true');
    $('#btnCancel' + WebBlogID).attr('disabled','true');
    
    Blog.UpdateBlog(WebBlogID, title , body , ajaxEditBlog_Callback)
}

function ajaxEditBlog_Callback(response,args){

    if(response.value!=null){
        $('#txtBlogTitleSource' + args.args.WebBlogID).val( $('#txtBlogTitle' + args.args.WebBlogID).val() );
        $('#txtBlogBodySource' + args.args.WebBlogID).val( $('#txtBlogBody' + args.args.WebBlogID).val() );
        
        $('#blogTitle' + args.args.WebBlogID).html(args.args.Title);
        $('#blogBody' + args.args.WebBlogID).html(response.value);

        cancelEditBlog(args.args.WebBlogID);
    }else {
        alert('Ooops, there was a problem updating your Blog, please try again!');
    }
    
        $('#btnEdit' + args.args.WebBlogID).removeAttr("disabled");
        $('#btnCancel' + args.args.WebBlogID).removeAttr("disabled");
}

function ajaxPostBlog(){
    var title = $('#txtBlogTitle').val();
    var body = $('#txtBlogBody').val();
    $('#btnPost').attr('disabled','true');
    $('#btnCancel').attr('disabled','true');
    Blog.NewBlog(title , body , ajaxPostBlog_Callback)
}

function ajaxPostBlog_Callback(response,args){

    if(response.value!=null){
        window.location.href = window.location.href;
    }else {
        alert('Ooops, there was a problem posting your Blog, please try again!');
    }
    
    $('#btnPost').removeAttr("disabled");
    $('#btnCancel').removeAttr("disabled");
}


