function dmp(webMemberID){
     var fullName = '&nbsp;';

    if((typeof(window['memberArray']) != "undefined")){
         for(var i=0;i<memberArray.length;i++){
            if(memberArray[i][0]==webMemberID){
                fullName = memberArray[i][1];
            }
         }
     }

     npopup(fullName,"<div id='divProfileHTML' style='height: 125px;'>Loading profile</div>",535,115);
     ajaxcall("/ajax/popup.aspx?funcname=GetMiniProfile","webMemberID="+webMemberID,displayMiniProfile_callback);
}

function displayMiniProfile_callback(response){
        $('#divProfileHTML').html(response)
}


function sendmessageshow(nickname){
     npopup('Send message',"<div id='divProfileHTML' style='height: 125px;'>Loading Gallery</div>",680,380);
     ajaxcall("/ajax/popup.aspx?funcname=sendmessageshow","nickname="+nickname,displaymessagesend_callback);
}

function displaymessagesend_callback(response){
     $('#divProfileHTML').html(response)
}


function npopup(title,content,width,height){

    var left = width / 2;
    var top = height / 2;
    
    
    if($.browser.msie && $.browser.version.substr(0,1) == '6'){
        var ScrollTop = (document.body.parentElement) ? document.body.parentElement.scrollTop : 0;
        top = ScrollTop;
        top = top - 100;
    }else{
        top = "-"+top;
    }
    
    if($('#divPopup').length==0){
        $(document.body).append('<div id="divPopup"></div>');
    }

    var html = '<div class="innerPopup" style="width:'+width+'px; margin-left:-'+left+'px;margin-top:'+top+'px;"><div class="innerPopupTop"><div class="innerPopupTopCorner"></div></div>';
    html += '<div class="innerPopupContent"><div class="innerPopupTitle"><a class="popupCloseLink" href="javascript:closePopup();">	<img src="/images/close.gif" width="16" height="16" alt="" />';
    html += '</a><h3>'+title+'</h3></div><div class="innerPopupBlock" style="height:'+height+'px;" id="divpuContent">'+content+'</div></div><div class="innerPopupBottom">';
    html += '<div class="innerPopupBottomCorner"></div></div></div>';
    
    $('#divPopup').html(html);
        
}


function closePopup(){
    $('#divPopup').html('');
}

function escListener(add){
    if(add){
        $(document).keypress(function(e) {
		              if (e.which == 27) {
        		       	closePopup();
        		       	escListener(false);
		              } 
        });
    }else{
        $(document).unbind("keypress");
    }
}

