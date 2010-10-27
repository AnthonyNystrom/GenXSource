( function( $ ) {
        $.dequeue = function( a , b ){
                return $(a).dequeue(b);
        };

 })( jQuery ); 


var winArray = new Array();
var minWinArray = new Array();

// Contains the 4 open windows
var openArray = new Array();
openArray[0] = openArray[1] = openArray[2] = openArray[3] = null;

var fLoad = true;
var draggedWin = null;
var timer1 = null;

var token = 'xyz';

var chatMode = 0;
var autoLoadChatMode = -1;
var myId = '';

var sM = new Array();
var smComplete = true;
var lockSM = false;

var rM = new Array();
var lockRM = false;
var fR = 0;

var lastWinL = 10;
var lastWinT = -300;

var aL = false;

var monNames=new Array("Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec");
var dayNames = new Array("Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday");

$(document).ready(function(){    
    
    $('#cWinSrcFriendList').appendTo($("#cWins"))
    
    if( autoLoadChatMode >-1 && chatMode == -1 )
    {
        chatMode = autoLoadChatMode;
    }
    
    if( chatMode == 0 )
    {
        chatLoad(chatMode,false);
    }
    
 });


function Message(wmi,m) {
    this.wmi = wmi;
    this.m = m;
}

function Coordinates(x,y)
{
    this.x = x;
    this.y = y;
}

//Integerated
function chatLogin()
{
    //chatLoad(0,$('#chkAutoChat').attr('checked'));
    chatLoad(0,true);
}

function chatLoginWin()
{
    chatLoad(1,false);
}


function openChatWin()
{  
   window.open ('/chat/ChatWindowed.aspx','chatWindow','menubar=0,resizable=1,width=360,height=450');
}

//function toggleAutoLogin()
//{
//    $('#chkAutoChat').attr('checked',!$('#chkAutoChat').attr('checked')); 
//    
//    if( $('#chkAutoChat').attr('checked') )
//    {
//        chatLogin();
//    }
//}

function chatLogoutEx()
{
    chatUnload();
}

function chatLoad(x,rem)
{
    login(x,rem);
}

function chatUnload()
{
    logout();	
}

function closeDropDown()
{
    $('#chatDropDown').hide();
}

function showChatBar()
{
    $('#chatBar').show();
}

function hideChatBar()
{
    $('#chatBar').hide();
}

function showChatLogOut()
{
    $('#chatLogOut').show();
    $('#chatWin').hide();
    $('#chatInt').hide();
    $('#chatAuto').hide();
    
}

function hideChatLogOut()
{
    $('#chatLogOut').hide();
    $('#chatWin').show();
    $('#chatInt').show();
    $('#chatAuto').show();
}

function reOpenWindows()
{
    var c = $.cookie('winArray');    
    var d = $.cookie('minWinArray');
    var i = 0; 
   
    if( c == null )
        return;
    
    var minWA = new Array();
    
    if( d != null )
        minWA = d.split(',');   
        
    var wA = c.split(','); 
    
    for(i = 0 ; i < wA.length; i++ )
    {        
        c = $.cookie(wA[i]);
        pos = null;
        pleft = 0;
        ptop = 0;
       
        if( c!= null )
        {
            pos = c.split(',');        
            pleft = pos[0];
            ptop = pos[1];
        }

        if( wA[i] != '' && minWA[i] != 'FriendList')
        {
           openWin( wA[i],!isFriendOnline(wA[i]),pleft,ptop );            
        }
    }
    
    var showFriendList = true;
    
    for(i = 0 ; i < minWA.length; i++ )
    {   
       if( minWA[i] != '' )
        {
            if( minWA[i] == 'FriendList' )
                showFriendList = false;
                
            minWin( minWA[i],true);
            //removeFromOpenWinArray(minWA[i]);        
        }
    }

    
    LayoutOpenWins();
    
    if( showFriendList )
    {
        $('#cWinSrcFriendList').show();
    }
    else
    {
    }                  

}


//Events
function chatWindowOpenClose(winId)
{   
    if( winId == null )
        return;
        
    try{
        $.cookie('winArray',winArray.toString(),{path: '/'});
        $.cookie(winId,$('#cWinSrc' + winId).position().left + ',' + $('#cWinSrc' + winId).position().top,{path: '/'});
    }catch(ex){}
    
    
}

function closeWin(winId)
{    
    var chatWin = '#cWinSrc' + winId;    
    
    //$("#cWinSrc" + winId).draggable("destroy");
        
    var idx  = jQuery.inArray( winId, winArray );
    if( idx >=0 )
        winArray[idx] = '';
        
    jQuery.unique( winArray );
    
    closeChatBarButton(winId);
    //fire    
    chatWindowOpenClose(winId);  
    
    $(chatWin).remove();
    
    removeFromOpenWinArray(winId);
    $.cookie(winId,null,{ path: '/'});
    LayoutOpenWins();
}

function minWin(winId,noEffect)
{
    if( winId == null )
        return;
        
    noEffect = noEffect == null ? false : noEffect;
    
    if( !noEffect )
    {
        try
        {
	        $('#cWinSrc' + winId).TransferTo(
	            {
	                to:'chatBarLi' + winId,
		            className:'minimize',
		            duration: 400
	            }
	        ).hide();
	    }catch(ex){}
	}
	else
	{
        $('#cWinSrc' + winId).hide();
        $('#cWinSrc' + winId).TransferTo(
            {
                to:'chatBarLi' + winId,
	            className:'',
	            duration: 400
            }
	     );
    }
    
    var idx  = jQuery.inArray( winId, minWinArray );
     
    if( idx < 0 )
    {
        minWinArray.push(winId);
    }
        
    jQuery.unique( minWinArray );
    $.cookie('minWinArray',minWinArray.toString(),{path: '/'});
    
    minChatBarButton(winId);
}

function scrollAllDown()
{
    var i = 0;
    for( i = 0 ; i < openArray.length; i++ )
    {
        if( openArray[i] != null )
        {
            $('#wPanel' + openArray[i]).each(function(){this.scrollTop = this.scrollHeight});           
        }
    }    
}

function restoreWin(winId)
{
    if ( $('#cWinSrc' + winId).is(':visible') )
        return;
    
    var idx  = jQuery.inArray( winId, minWinArray );
    
    if( idx >=0 )
        minWinArray[idx] = '';
        
    jQuery.unique( minWinArray );
    
    //putInOpenWinArray(winId);
    //LayoutOpenWins();
    
    try
    {
        $('#chatBarLi' + winId).TransferTo(
		    {
			    to:'cWinSrc' + winId,
			    className:'minimize', 
			    duration: 400,
			    complete: function()
			    {
				    $('#cWinSrc' + winId).show();
			    }
		    }
	    );
	}catch(ex){}
        
    restoreChatBarButton(winId);
    noMessageWin(winId);
    $('#wPanel' + winId).each(function(){this.scrollTop = this.scrollHeight});
    $.cookie('minWinArray',minWinArray.toString(),{path: '/'});
    
    $.timer(500, function (timer) {
        $('#wPanel' + winId).each(function(){this.scrollTop = this.scrollHeight});
        scrollAllDown();
        timer.stop();
    });
}

function minRestoreWin(winId)
{    
    if ( $('#cWinSrc' + winId).is(':visible') )
    {
        minWin(winId,false);        
    }
    else
    {
        restoreWin(winId);
    }
}

function isWinOpen(winId)
{
    if ( $('#cWinSrc' + winId).length > 0)
    {
        return true;
    }
    
    return false;
}

function isFriendOnline(friendId)
{
    var idx = jQuery.inArray( friendId, onf );
    return (idx >= 0);
}

function putInwinArray(winId)
{
    var idx  = jQuery.inArray( winId, winArray );
    if( idx < 0 )
    {
        winArray.push(winId);
    }
}

function removeFromOpenWinArray(winId)
{
    var idx  = jQuery.inArray( winId, openArray );    
    if( idx < 0 )
        return;
    
    openArray[idx] = null;
//        
//    var i = 0;
//    for( i = idx; i < openArray.length - 1 ; i++ )
//    {
//        openArray[i] = openArray[i + 1];
//    }
//    
//    openArray[openArray.length - 1 ] = null;
}

function putInOpenWinArray(winId)
{
    var idx  = jQuery.inArray( winId, openArray );    
    if( idx >= 0 )
        return;
    
    var i = 0;
    
//    for( i = 0 ; i < openArray.length; i++ )
//    {
//        if( openArray[i] == null )
//        {
//            openArray[i] = winId;
//            return;
//        }
//    }    
    
    openArray.push(winId);    
}

function addChatBarButton(winId,off)
{
    if( $('#chatBarLi' + winId).length > 0 )
        return;
        
    var chatBarLi = $('#chatBarLiSrc').clone(true).appendTo($("#chatBar"));
    chatBarLi.attr('id','chatBarLi' + winId);
    
    chatBarLi.find('#chatBarBtnSrc').attr('id','chatBarBtn' + winId);
    chatBarLi.find('#chatBarBtn' + winId).unbind("click");
    chatBarLi.find('#chatBarBtn' + winId).bind("click", function()
    {        
        minRestoreWin(winId);
        void(0);
    });
    
    var info = getInfo( winId,off );
    
    chatBarLi.find('#chatBarBtnTextSrc').attr('id' , 'chatBarBtnText' + winId);
    chatBarLi.find('#chatBarBtnText' + winId).html(info[0]);
    
    chatBarLi.show();
}

function closeChatBarButton(winId)
{   
    $('#chatBarLi' + winId).remove();     
}

function minChatBarButton(winId)
{                   
    $('#chatBarBtn' + winId).removeClass('active');
    $('#chatBarBtn' + winId).addClass('minimized');
}

function restoreChatBarButton(winId)
{   
    $('#chatBarBtn' + winId).removeClass('minimized');
    $('#chatBarBtn' + winId).addClass('active');
}


function LayoutOpenWins()
{
    var width = $(window).width();
    var left = width - 230 - 5; // 5 px buffer
    
    var i = 0;
    for( i = 0 ; i < openArray.length; i++ )
    {
        if( openArray[i] != null )
        {
            $("#cWinSrc" + openArray[i]).css('left',left + 'px');
            left -= 240;
        }
    }
}

function openWin(winId,off,pleft,ptop)
{   
    if( isWinOpen(winId) )
    {
        restoreWin(winId);
        return;
    }       
    
    addChatBarButton(winId,off);
    
    putInwinArray(winId);
    putInOpenWinArray(winId);
    
    var info = getInfo( winId,off );
    
    var chatWin = $('#cWinSrc').clone(true).appendTo($("#cWins"));    
    chatWin.attr('id','cWinSrc' + winId);    

    chatWin.find('#chatWindowHeader').attr('id' , 'chatWindowHeader' + winId);    
    
    chatWin.find('#wAvator').attr('id' , 'wAvator' + winId);
    chatWin.find('#wAvator' + winId).attr("src",info[2]);    
    
    chatWin.find('#wNick').attr('id' , 'wNick' + winId);
    chatWin.find('#wNick' + winId).html(info[0]);
        
    chatWin.find('#wPM').attr('id' , 'wPM' + winId);
    chatWin.find('#wPM' + winId).html(info[1]);
    
    chatWin.find('#wPanel').attr('id' , 'wPanel' + winId);
    chatWin.find('#collapseDiv').attr('id' , 'collapseDiv' + winId);
    
    chatWin.find('#chatScroll').attr('id' , 'chatScroll' + winId);
    chatWin.find('#wInput').attr('id' , 'wInput' + winId);    
    
    chatWin.find('#wMin').attr('id' , 'wMin' + winId);
    chatWin.find('#wMin' + winId).unbind("click");    
    chatWin.find('#wMin' + winId).bind("click", function()
    {        
        minWin(winId,false);
        void(0);
    });
        
    chatWin.find('#wClose').attr('id' , 'wClose' + winId);
    chatWin.find('#wClose' + winId).unbind("click");    
    chatWin.find('#wClose' + winId).bind("click", function()
    {        
        closeWin(winId);
        void(0);
    });
    
    $('#wInput' + winId).keyup(function(e)
    {   
        if(e.keyCode == 13)
        {
            enterPressed(winId);
        }
        
        noMessageWin(winId);        
    });
    
    chatWin.click( function () {
        var zmax = 0;
          $( this ).siblings().each(function() {
                var cur =  $( this ).css( 'zIndex');                
                zmax = cur > zmax ? $( this ).css( 'zIndex') : zmax;
                zmax = zmax - 0;                
        });        
        $( this ).css( 'zIndex', zmax+1 );
        try{
            $('#wInput' + winId).focus();
        }catch(ex){}
        
        noMessageWin(winId);
    });
    
    
    getLast10Msgs(winId);
    LayoutOpenWins();  
    chatWin.show();
    
    try{
        $('#wInput' + winId).focus();
    }catch(ex){}
    
    chatWindowOpenClose(winId);
    
}

function chatAppendMessage(winId,nick,message,sentDt,extMsg)
{
    var wPanel = $('#wPanel' + winId);    
    wPanel.html(wPanel.html() + '<div class="chatEntry"><h3><span class="entryTime">' + getTime(sentDt) + '</span><span class="partner">' + nick + '</span></h3><p>' + message + '</p></div>');   
    
    $('#wPanel' + winId).each(function(){this.scrollTop = this.scrollHeight});
    
    if( extMsg )
    {   
        newMessageWin(winId);
    }
}

function chatPrependMessage(winId,nick,message,sentDt)
{
    var wPanel = $('#wPanel' + winId);    
    wPanel.html('<div class="chatEntry"><h3><span class="entryTime">' + getTime(sentDt) + '</span><span class="partner">' + nick + '</span></h3><p>' + message + '</p></div>' + wPanel.html());   
    
    $('#wPanel' + winId).each(function(){this.scrollTop = this.scrollHeight});
}

function newMessageWin(winId)
{
    $('#cWinSrc' + winId).removeClass('chatConversationNoMsg');
    $('#cWinSrc' + winId).addClass('chatConversationNewMsg');    
    $('#chatBarBtn' + winId).addClass('newMsg');
}

function noMessageWin(winId)
{
    $('#cWinSrc' + winId).removeClass('chatConversationNewMsg');
    $('#cWinSrc' + winId).addClass('chatConversationNoMsg');
    $('#chatBarBtn' + winId).removeClass('newMsg');
}

function enterPressed(winId)
{
    var textEntered = $('#wInput' + winId).val().trim();
    
    if( textEntered == '')
        return;
        
    chatAppendMessage(winId,'me',textEntered,new Date(),false);    
    sendMessage(winId,textEntered);
    $('#wInput' + winId).val('');    
}



function populateOnlineFriendList(friends)
{
    var i = 0;
    for( i = 0; i < friends.length; i++ )
    {
        if( i == 0 )
        {            
            addFriendToOnlineList(friends[i],null);
        }
        else
            addFriendToOnlineList(friends[i],friends[i-1]);
    }
}

function populateOfflineFriendList(friends)
{
    var i = 0;
    for( i = 0; i < friends.length; i++ )
    {
         if( i == 0 )
            addFriendToOfflineList(friends[i],null);
        else
            addFriendToOfflineList(friends[i],friends[i-1]);
    }
}

function removeFriendFromOnlineList(friend)
{
    
    var friendId = friend.WebMemberID;
    $('#fEntrySrc' + friendId ).remove();
    
}

function removeFriendFromOfflineList(friend)
{
    var friendId = friend.WebMemberID;    
    $('#ofEntrySrc' + friendId ).remove();
}

function addFriendToOnlineList(friend,previousFriend)
{
    var friendId = friend.WebMemberID;
    
    if( $('#fEntrySrc' + friendId ).length > 0 )
        return;
    
    if( $('#ofEntrySrc' + friendId ).length > 0 )
        removeFriendFromOfflineList(friend);    
        
    var fEntry = null;
    
    if( previousFriend == null )
    {
        fEntry = $("#fEntrySrc").clone(true).prependTo($("#chatOnlineFriends"));
    }
    else
    {
        fEntry = $("#fEntrySrc").clone(true).insertAfter($("#fEntrySrc" + previousFriend.WebMemberID));
    }
    
    fEntry.attr("id","fEntrySrc" + friendId);
    
    fEntry.click( function () {
        openWin(friendId,false);        
    }); 
    
    var fNick = fEntry.children("#fNick");
    fNick.attr("id","fNick" + friendId);
    fNick.html(friend.NickName);
    
    var fPM = fEntry.children("#fPM");
    fPM.attr("id","fPM" + friendId);
    fPM.html(friend.CustomMessage);
    
    var fAvator = fEntry.children("#fAvator");
    fAvator.attr("id","fAvator" + friendId);
    fAvator.attr("src",friend.AvatorUrl);
    
    fEntry.show();
}

function addFriendToOfflineList(friend,previousFriend)
{
    var friendId = friend.WebMemberID;
    
    if( $('#ofEntrySrc' + friendId ).length > 0 )
        return;
    
    if( $('#fEntrySrc' + friendId ).length > 0 )
        removeFriendFromOnlineList(friend); 
    
    var fEntry = null;
    
    if( previousFriend == null )
    {
        fEntry = $("#ofEntrySrc").clone(true).prependTo($("#chatOfflineFriends"));
    }
    else
    {
        fEntry = $("#ofEntrySrc").clone(true).insertAfter($("#ofEntrySrc" + previousFriend.WebMemberID));
    }
    
    fEntry.attr("id","ofEntrySrc" + friendId);
    
    fEntry.click( function () {
        openWin(friendId,true);        
    }); 
    
    var fNick = fEntry.children("#ofNick");
    fNick.attr("id","ofNick" + friendId);
    fNick.html(friend.NickName);
    
    var fPM = fEntry.children("#ofPM");
    fPM.attr("id","ofPM" + friendId);
    fPM.html(friend.CustomMessage);
    
    var fAvator = fEntry.children("#ofAvator");
    fAvator.attr("id","ofAvator" + friendId);
    fAvator.attr("src",friend.AvatorUrl);
    
    fEntry.show();
}

function getInfo(friendId,off)
{
    var info = new Array();
    var ap = '';
    
    if ( $('#fNick' + friendId).length <= 0 )
        ap = 'o';
        
    if( friendId == 'FriendList' )
    {        
        info[0] = "Friends";
        info[1] = "";
        info[2] = "";
    }
    else
    {
        info[0] = $('#' + ap + 'fNick' + friendId).html();
        info[1] = $('#' + ap + 'fPM' + friendId).html();
        info[2] = $('#' + ap + 'fAvator' + friendId).attr("src");
    }
            
    return info;
}

function sortFriend(objA,objB)
{
        if( objA.NickName.toUpperCase() > objB.NickName.toUpperCase() )
        {
            return 1;
        }
        else if( objA.NickName.toUpperCase() < objB.NickName.toUpperCase() )
        {
            return -1;
        }
        else
        {
            return 0;
        }
}

function getTime(msgDate)
{
    var currentDt = new Date();
    
    var diff =  ((currentDt - msgDate) / 1000) / 24 / 60 / 60 ;
    var diffDay = currentDt.getDay() - msgDate.getDay();

    var hrs = msgDate.getHours();
    var mins = msgDate.getMinutes() + '';
    var period = 'am';
    
    if( hrs >= 12 )
    {
        hrs = hrs % 12;        
        period = 'pm';
    }
    
    if( hrs == 0 )
        hrs = 12;
    
    hrs = getDoubleDigit(hrs);
    
    mins = getDoubleDigit(mins);
    
    var retString = hrs + ':' + mins + '' + period;
    
    if( diff >= 1 || diffDay >= 1 )
    {
        return hrs + ':' + mins + '' + period + ' on '  + getDoubleDigit(msgDate.getDate()) + " " + monNames[msgDate.getMonth() ] + " " + msgDate.getFullYear();
    }
    
    return retString;
}

function getDoubleDigit(x)
{
    if( x.length == 1 )
        x = '0' + x;
        
    return x;
}



///////////////////////////////////////
/// Ajax Methods
///////////////////////////////////////
var off;
var onf;

////////////////////////////////////////////////////
//Login
function login(x,rem)
{
   ChatCtrl.Login(x,rem,login_CallBack); 
}
   
function login_CallBack(response,args)
{
   if( response!= null )
    {
        if( response.value == 0 )
        {
            putInOpenWinArray('FriendList');
            putInwinArray('FriendList');
            
            LayoutOpenWins();
            addChatBarButton('FriendList');
 
            getUpdate();
            
            timer1 = $.timer(2000, getUpdate);
            
            showChatBar();
            showChatLogOut();
        }
    }
}

////////////////////////////////////////////////////
//Logout
function logout()
{
   ChatCtrl.Logout(logout_CallBack); 
}

   
function logout_CallBack(response,args)
{
   if( response!= null )
    {
        if( response.value == 0 )
        {
            var i = 0;
            for(i = 0;i < winArray.length; i++ )
            {   
                if( winArray[i] != 'FriendList' )
                    closeWin(winArray[i]);
            }
            
            timer1.stop();
            
            $('#cWinSrcFriendList').hide();
            hideChatBar();
            hideChatLogOut();
            
            fLoad = true;
            minWinArray = new Array();
            winArray = new Array();
            openArray = new Array();
            
            $.cookie('winArray',winArray.toString(),{path: '/'});
            $.cookie('minWinArray',minWinArray.toString(),{path: '/'});
        }
    }
}


/////////////////////////////////////////////////////
//GETUPDATE
function getUpdate() 
{   
    ChatCtrl.GetUpdate(token,getUpdate_CallBack);
}
   
/////////////////////////////////////////////////////
//GETUPDATE CALLBACK 
function getUpdate_CallBack(response) 
{   
    if( response.value != null )
    {   
        token = response.value.Token;        
            
        off = new Array();
        onf = new Array();
        
        var friends = response.value.Friends;        
        var friend = null;
        
        var i = 0;
        for( i = 0; i < friends.length; i++ )
        {
            friend = friends[i];            
            
            if( friend.OnlineStatus == 0 )
                off.push(friend);
            else
                onf.push(friend);
        }
        
        onf.sort(sortFriend);
        off.sort(sortFriend);
        
        populateOnlineFriendList(onf);
        populateOfflineFriendList(off);
        
        var messages = response.value.Messages;
        
        var message = null;
        
        var i = 0;
        for( i = 0; i < messages.length; i++ )
        {
            message = messages[i];
            
            openWin(message.WebMemberIDFrom);
            chatAppendMessage(message.WebMemberIDFrom,getInfo(message.WebMemberIDFrom,false)[0],message.Message,message.DTCreated,true);
        }
    }
    
    if( fLoad )
    {
        fLoad = false;                
        reOpenWindows();
        //getLast10Msgs('');
    }
}


/////////////////////////////////////////////////////
//GETUPDATE
function getLast10Msgs(winId) 
{
    ChatCtrl.GetLast10Msgs(winId,getLast10Msgs_CallBack);
}
   
/////////////////////////////////////////////////////
//GETUPDATE CALLBACK 
function getLast10Msgs_CallBack(response) 
{   
    if( response.value != null )
    {   
        var messages = response.value.Messages;
        
        var message = null;  
        
        var i = 0;
        
        for( i = messages.length -1; i >= 0 ; i-- )
        {
            message = messages[i];
            
            if( message.WebMemberIDFrom == myId )
            {
                chatPrependMessage(message.WebMemberIDTo,'me',message.Message,message.DTCreated);
            }
            else
            {
                chatPrependMessage(message.WebMemberIDFrom,getInfo(message.WebMemberIDFrom,false)[0],message.Message,message.DTCreated);
            }
        }
    }    
}



/////////////////////////////////////////////////////
//sendUpdate
function sendUpdate() 
{   
    if( !smComplete )
        return;
        
    smComplete = false;
    var s = "";
    
    while( sM.length > 0)
    {
        var message = sM.shift();
        s += message.wmi + "," + message.m + ";";
    }
    
    if( s!= "")
    {
        ChatCtrl.SendMessage(s,sendUpdate_CallBack); 
        smComplete = true; 
    }
    else
    {
         smComplete = true;       
    }
    
//    for(i = 0; i < messages.length; i++)
//    {   
//        ChatCtrl.SendMessage(messages[i].wmi,messages[i].m,sendUpdate_CallBack);
//    }
    
//    var wmis = getRM();

//    for(i = 0; i < wmis.length; i++)
//    {
//        ChatCtrl.MarkDelivered(wmis[i],sendUpdate_CallBack);
//    }
}

/////////////////////////////////////////////////////
//sendUpdate
function sendMessage(wmi,m) 
{     
    ChatCtrl.SendMessage(wmi + ',' + m,sendUpdate_CallBack);
}

   
/////////////////////////////////////////////////////
//sendUpdate CALLBACK 
function sendUpdate_CallBack(response) 
{
    smComplete = true;
}

function getSM()
{
    lockSM = true;
    
    try
    {
        var retArr = sM;
        sM = new Array();
    }catch(ex){}
    
    lockSM = false;
    
    return retArr;
}

function getRM()
{
    lockRM = true;
    
    try
    {
        var retArr = rM;
        rM = new Array();
    }catch(ex){}
    
    lockRM = false;
    
    return retArr;
}