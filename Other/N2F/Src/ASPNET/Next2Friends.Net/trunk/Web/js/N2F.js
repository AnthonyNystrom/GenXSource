var chatWinReference = null;

function displayLogin(){

    var header_login = document.getElementById('header_login');
    var txtLogin = document.getElementById('ctl00_txtEmailLogin');
    
    if(header_login.style.display=='block'){
        header_login.style.display = 'none';
    }else{
        header_login.style.display = 'block';
        txtLogin.focus();
    }
}

function displayNavMenu(show){

    var profile_shortcuts = document.getElementById('profile_shortcuts');
    
    if(show){
        profile_shortcuts.style.display = 'block';
    }else{
        profile_shortcuts.style.display = 'none';
    }
}

function w(){
    window.status='';
    return true;
}

function w(str){
    window.status=str;
    return true;
}


function openChatWindow(){

    if(parent.window.location.href.indexOf('chat')>-1){
        parent.window.location.href = '/index.aspx';
    }
    
    window.open ('/chat/ChatClientPopup.aspx','chatWindow','menubar=0,resizable=1,width=360,height=450');

}

function closeChatWindow()
{
	try
	{
		if( chatWinReference )
		{
			chatWinReference.close();
		}
	}
	catch(ex){}
}

function chatLogout(){

	try
	{
		if( chatWinReference )
		{
			chatWinReference.logout();
		}
	}
	catch(ex){}
	
    closeChatWindow();

     if(parent.window.location.href.indexOf('chat')>-1){
        parent.window.location.href = '/index.aspx';
    }
    
}



function setChatWinReference(varchatWinReference)
{
    chatWinReference = varchatWinReference;
}

var searchLine="/video";
var searchsetup=false;

$(document).ready(function() {

   
   function searchGo(){
    var categoryValue = $("#category option:selected").text(); 
    if( categoryValue == "people")
    { 
        var avatar="";
        if($("#avatar").is(':checked'))
            avatar="&avatar="+$("#avatar").val();
        searchLine="/community/?&t=full&search="+$("#search").val()+"&sex="+$("#sex").val()+"&email="+$("#email").val()+"&profile="+$("#profile").val()+"&type="+$("#category").val()
        +"&country="+$("#country").val()+"&city="+$("#city").val()+avatar;
        
    }else
    {
        searchLine="/video/?search="+$("#search").val()+"&type="+$("#category").val();
    }
    document.location=searchLine;
   }
   

   $("#category").change(function(){
   
        var categoryValue = $("#category option:selected").text(); 
       
        if( categoryValue == "people"){ 
           $("#advancedSearch").show(); 
        }else{ 
           $("#advancedSearch").hide(); 
           $("#friendoptions").hide(); 
        } 
   
   });
   
   $("#advancedSearch").click(function(){
            $("#friendoptions").show('fast');
            if(!searchsetup){
                popCountry(); 
               $("#searchButton").click(function(){searchGo();});
               $("#search").keypress(function(e){if(e.which==13)searchGo();});
               $("#email").keypress(function(e){if(e.which==13)searchGo();});
               $("#sex").keypress(function(e){if(e.which==13)searchGo();});
               $("#country").keypress(function(e){if(e.which==13)searchGo();});
               $("#city").keypress(function(e){if(e.which==13)searchGo();});
               $("#avatar").keypress(function(e){if(e.which==13)searchGo();});
   
                searchsetup=true;
            }
       
            
   });


    //$(document).click(function(event){

    //if($(event.target).parent()[0].id!='sCountry' && $(event.target).parent()[0].id!='country' && $(event.target).parent()[0].id!='pSearch' && $(event.target).parent().parent()[0].id!='pSearch' && $(event.target).parent()[0].id!='friendoptions'&& $(event.target).parent().parent()[0].id!='friendoptions'&& $(event.target).parent().parent().parent()[0].id!='friendoptions')
     //   $("#friendoptions").hide(); 

    //});

});
