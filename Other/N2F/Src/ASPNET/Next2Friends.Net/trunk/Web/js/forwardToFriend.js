function setForwardToFriend(type,WebID)
{
    $("#btnForward").attr("onclick","javascript:forwardToFriend('" + type +  "','" + WebID + "');void(0);");
    $('#txtMessage').val("Check out this Next2Friends " + type);
}


function forwardToFriend(type,WebID)
{  
    
        var emails = new Array();
        
        var txt1 = $('#txtEmail1');
        var txt2 = $('#txtEmail2');
        var txt3 = $('#txtEmail3');

        emails[0] = (txt1.val()!=txt1[0].defaultValue) ?  txt1.val() : null;
        emails[1] = (txt2.val()!=txt2[0].defaultValue) ?  txt2.val() : null;
        emails[2] = (txt3.val()!=txt3[0].defaultValue) ?  txt3.val() : null;
        
        if( emails[0] == null && emails[1] == null && emails[2] == null )
        {
            return;
        }
 
        ForwardToFriendCtrl.ForwardToFriend(type, WebID, emails,safeHTML($('#txtMessage').val()), forwardToFriend_Callback);  
}

function forwardToFriend_Callback(response,args)
{
    if(response.value!=null)
    {
        if( response.value == -1 )
        {
            $('#lblMessage').html("<span class='error_alert'>One or more invalid email addresses</span>");
        } 
        else if( response.value == -2 )
        {
            alert('Ooops, there was a problem sending your invite, please try again in a few moments!');
        }
        else
        {
            $('#lblMessage').html("<span style='color:#0257AE;font-size:smaller;'>Your invite has been sent</span>");
            $('#txtEmail1')[0].value = $('#txtEmail1')[0].defaultValue;
            $('#txtEmail2')[0].value = $('#txtEmail2')[0].defaultValue;
            $('#txtEmail3')[0].value = $('#txtEmail3')[0].defaultValue;
        }         
    }
    else 
    {
        alert('Ooops, there was a problem sending your invite, please try again in a few moments!');
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

function clearEmail(txt){
     if (txt.value == txt.defaultValue){
         txt.value = ""
     }
}

function outClear(txt){
    if(txt.value==''){
        txt.value=txt.defaultValue;
    }
}
