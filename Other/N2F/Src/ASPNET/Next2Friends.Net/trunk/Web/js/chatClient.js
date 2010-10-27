        var chatWindowHTML = '';//<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChatWindow.aspx.cs" Inherits="ChatWindow" %><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml" ><head runat="server"><link rel="Stylesheet" href="../css/chat.css" type="text/css"/><title>Chat Window</title><script language="javascript" src="../js/chatWindow.js" type="text/javascript"></script></head><body style="background-color:Transparent"><form id="form1" runat="server"><b class="rtopdark"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4"></b></b><div id="divChatConsoleHTML"><table cellspacing="0" style="border-right:solid 1px #cccccc;border-left:solid 1px #cccccc;background-color: white; height: 100%; font-family: Verdana;padding: 0px;" width="100%"><tr><td align="center" style="background-color: #509DEB;height:10px;cursor: pointer;"><table width="100%" height="100%" cellspacing="0" ><tr><td align="left"><img id="statusIcon" src="../images/offline.gif" /><div style="width: 100%; text-align: left; vertical-align:top; color: White; font-size: 80%; font-weight: bold;display:inline"><div id="titleNick" style="display:inline">Nick</div></div></td><td align="right"><img src="../images/minimize.gif" onclick="minRestoreChatWindow();" /><img src="../images/close.gif" onclick="closeChatWindow();" /></td></tr></table></td></tr><tr><td align="right" valign="bottom" width="100%"><div id="divChatDisplay" style="text-align: left; width: 100%; height: 194px;overflow-x: hidden; overflow:auto; font-size: 80%;"></div></td></tr><tr><td style="height: 70px;" align="center"><textarea id="txtInput" cols="4" style="overflow:auto; border:solid 1px #cccccc;height: 50px; width: 95%; font-family: Verdana;font-size: 80%;" onkeypress="return checkEnter(event);" onfocus="this.style.borderColor=\'#509DEB\';this.style.borderWidth=\'2px\';" onblur="this.style.borderColor=\'#cccccc\';this.style.borderWidth=\'1px\';"></textarea></td></tr></table></div><b class="rbottomonlyborder"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1"></b></b></form></body></html>';
        
        var timeOutMsgId = null;                
           
        var myMemberID = -1;       // current users memberid
        var EmailAddress = '';       // current users EmailAddress
        var Password = '';         // current users Password
        var myNick = ''; 
        var ajaxMember = null;
        var friends = null;
        var friendsForUpdate = null;
        var tempStatus = '';
        var actualTitle = 'Next2Friends';
        var blinkTitle = 'New Message';   
        var customText = '';
        var friendWindowHeight = 0;
        var minimized = false;
        var focused = true;
        var mainFocused = null;
        
        var getFriendStatusLastCall = new Date();
        var getMessageLastCall = new Date(); 
        
        var openChatWindows = new Array();
        
        function ChatWindow(webMemberID,hasNewMessage,message,windowReference) 
        {
            this.webMemberID = webMemberID;
            this.hasNewMessage = hasNewMessage;
            this.message = message;
            this.windowReference = windowReference;
        }    
        
        function GetChatWindow(webMemberID)
        {
            var i;
            var chatWindow = null;
            
            for( i = 0; i < openChatWindows.length; i++ )
            {
                chatWindow = openChatWindows[i];
                
                if( chatWindow.webMemberID == webMemberID )
                {
                    return chatWindow;
                }                
            }
            
            return null;
        }   
        
        // Encode the string for use in javascript methods
        function encodeString(x)
        {  
            if ( x == null )
            {
                return x;           
            }
                        
            return urlencode(x);            
        }
        
        //Extract the height as an integer from the css style attribute
        function extractVal(styleHeight)
        { 
            if ( styleHeight == null )
            {
                return 0;
            }
                
            //try
            {
                var px = styleHeight.indexOf("px",0);
                var h = 0;
                if ( px >= 0 )
                {
                    h = styleHeight.substr(0,px );
                }
                             
                return h * 1;
            }
            //catch(ex){return 0;}
        }       

        function getHeight()
        {
            try
            {
                var myHeight = 0;
                
                if( typeof( window.innerHeight ) == 'number' ) 
                {
                    myHeight = window.innerHeight;
                } 
                else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) 
                {
                    myHeight = document.documentElement.clientHeight;
                } 
                else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
                {
                   myHeight = document.body.clientHeight;
                }

                return myHeight;
             }
             catch(ex){}
        }
        
        // adjust the top of the invisible chat DIV
        function adjustChatDivTop()
        {            
            try
            {   
                var h = getHeight();                        
                var chatParentDiv = document.getElementById('chatParentDiv');
                var origH = extractVal(chatParentDiv.style.height); 
                chatParentDiv.style.top = (h - origH) + 'px';         
            }
            catch(ex){}
            
        }
        
        function getIframe( frameName )
        {   
            //var frm = $("#" + frameName).get();
            var frm = document.frames ? document.frames[frameName] : document.getElementById(frameName);
            //var frm = document.getElementById(frameName);            
            //alert(frm.contentWindow);
            return frm;
        }
        
        function getIframeDocJq( frameName )
        {   
            var frm = $("#" + frameName).get();
            
            if( frm.document )
            {
                return frm.document;
            }
            
            else if( frm.contentWindow )
            {
                return frm.contentWindow;
            }
        }
        
        function getIframeDoc(objIframe)
        {
		var oIf = getIframe( objIframe);
		if( oIf == null )
		return null;

            if( oIf.document )
            {
                return oIf;
            }
            else if( oIf.contentWindow )
            {
                return oIf.contentWindow;
            }
        }        
        
        // Add the specified message to message list
        function addMessageToDisplay(otherMemberId, message,otherMemberNick)
        {  
            var retVal = false;
            //try
            {
                clearTimeout(timeOutMsgId);            
                
                var iFrameDoc = getIframeDoc("chatParentIframe" + "" + otherMemberId);
                            
                if ( message != null && message != '')
                {
                    retVal = iFrameDoc.addMessageToDisplay( unescape(otherMemberNick),unescape(message));
                }
                
                return retVal;
            }
            //catch(ex){}            
        }
        
        function openChatWindowHelper(otherMemberId,message,otherMemberNick)
        {

            var thisWindow = new ChatWindow(otherMemberId,false,'');
            openChatWindows.push( thisWindow );
                        
            //get any open chat windows
            var iFrameDoc = getIframeDoc("chatParentIframe" + "" + otherMemberId);
		
            var setdone1 = false;
            var setdone2 = false;
            var setdone3 = false;
            //if any chat window is open for the specific member
            //set the icon
            if ( iFrameDoc != null)
            {
		
                if ( iFrameDoc.setOtherWebID)
                {
                    iFrameDoc.setOtherWebID(otherMemberId);
                    setdone1 = true;
                }                
                
                if ( iFrameDoc.setOtherMemberNick)
                {				
                    iFrameDoc.setOtherMemberNick(otherMemberNick);
                    setdone2 = true;
                }               
                
                if( message == '' )
                {                    
                    if ( iFrameDoc.focusInput)
                    {
                       setdone3 = iFrameDoc.focusInput();;
                    }                    
                }
                else
                {
                    setdone3 = true;
                }
                
                //addMessageToDisplay(otherMemberId,message,otherMemberNick);
            }            
            
            if( !setdone1 || !setdone2 || !setdone3)
            {   
                setTimeout("openChatWindowHelper('" + otherMemberId + "','" + message + "','" + otherMemberNick + "')",100 );
            }            
        }
        
        // open new chat window for the otherMemberId if not already open
        function openChatWindow(otherMemberId,message,otherMemberNick)
        {   
			

            //try
            {
                var chatParentDiv = document.getElementById('chatParentDiv');
                var chatParentIframeSource = document.getElementById('chatParentIframe');
                
                var newIframe = null;
                var createdNew = false;
                
                if ( otherMemberId != null )
                {
                    newIframe = getIframe("chatParentIframe" + "" + otherMemberId);
                }

                
                if ( newIframe == null )
                {
                    createdNew = true;
                    newIframe = chatParentIframeSource.cloneNode(true);                   
                    chatParentDiv.insertBefore(newIframe,chatParentDiv.firstChild);
                    
                    newIframe.setAttribute("id","chatParentIframe" + "" + otherMemberId);
                    newIframe.style.visibility = 'visible';
                    newIframe.style.display = 'inline';            
                }

                if ( createdNew )
                {
                    setTimeout("openChatWindowHelper('" + otherMemberId + "','" + message + "','" + otherMemberNick + "')",100 );
                    //timeOutMsgId = setTimeout("addMessageToDisplay('" + otherMemberId + "','" + otherMemberNick + "','" + message + "')",5000 );
                }                
                //else
                //{
                //    addMessageToDisplay(otherMemberId,message,otherMemberNick);
                //}            
            }
            //catch(ex){}
        }    
        
        
        // open new chat window for the otherMemberId if not already open
        function openChatWindowEx(otherMemberId)
        {
            //try
            {
                var friend = getFriendObject( otherMemberId );

                
                if( friend == null )
                    return;                
                
                openChatConsole(otherMemberId,'',friend.NickName);
                // Get the current status of the friend
                var imgUrl = getFriendStatusIcon( otherMemberId );
                
                //setTimeout("focusChatConsole('" + otherMemberId + "')",100);
                // Set the status of the friend in newly opened window
                setTimeout("setFriendWindowStatusIcon('" + otherMemberId + "','" + escape(friend.OnlineStatusString) + "')",500);
                // Set the nick of the friend in newly opened window
                setTimeout("setFriendWindowMemberNick('" + otherMemberId + "','" + escape(friend.NickName) + "')",500);
            }
            //catch(ex){}                    
        }   
           
           
        function focusChatConsole(otherMemberId)
        {
            var chatIframeDoc = getIframeDoc("chatParentIframe" + "" + otherMemberId);
            
            if (chatIframeDoc!=null && chatIframeDoc.focusInput)
            {
                chatIframeDoc.focusInput();
            }            
            else
            {
                setTimeout("focusChatConsole('" + otherMemberId + "')",100);
            }
        }   
        
    //returns true if the member is logged in
    function isLoggedIn()
    {
        return (ajaxMember != null);
    }
    
    // Get the memberID of the logged in member
    function getMyMemberID()
    {   
        return myMemberID;
    }
    
    // set the member ID of the logged in member
    function setMyMemberID(memberID)
    {
        myMemberID = memberID;
    }
    
    // get the nick of the logged in member
    function getMyMemberNick()
    {
        return myNick;
    }
     
    // a new chat message has come in from the server or the user clicked on a name from the friend list.
    // this launches a new chatwindow console for this member   
    function openChatConsole(memberID, message, nickName)
    {   
        //try
        {
            openChatWindow( memberID ,message,nickName);
        }
        //catch(ex){}
    }

    //SEND CHAT MESSAGE TO SERVER
    function sendChat(toMemberID, message)
    {
        //try
        {
            // add a blank string to convert the ints to strings        
            ChatClient.SendMessage(toMemberID+'', myMemberID+'', EmailAddress, message);            
        }
        //catch(ex){}
    }
    
    
    /////////////////////////////////////////////////////
    // Polling the server will be introduced later as an optimisation.
    // instead of calling the ajax methods in intervals of 2 seconds. The web server will loop and check for new server
    // events for a few minutes before returning (where the method is called again). If a new event is fired the method will
    // return immediatly, returning the data to the browser within seconds. This increases the number of concurrent connections
    // but reduces the overall hits which create unnessesary overhead
    //POLL SERVER
//    function pollServer() {
//        ChatClient.PollServer(myMemberID, pollServer_CallBack);

//    }
//    
//   function pollServer_CallBack(response) {
//        if (response != null){
//            if (response.value != ""){
//                alert(response.value);
//            }
//        }
//        
//        pollServer();
//        
//        
//    }
   
   
   /////////////////////////////////////////////////////////////////////////////////////////
   /// FUNCTIONS RELATING TO FRIEND LIST & FRIEND CHAT WINDOW
   /////////////////////////////////////////////////////////////////////////////////////////
   
    ///
    /// Updates status of the friend specified by the otherMemberWebID
    ///
    
    // Called when the mouse moves over a friends name
    function divMouseOver(divId)
    {
        //try
        {
            var div = document.getElementById(divId);
            div.style.backgroundColor = '#E0ECFF';
        }//catch(ex){}
    }

    // Called when the mouse moves out from a friends name
    function divMouseOut(divId,revertColor)
    {
        //try
        {
            var div = document.getElementById(divId);
            if ( revertColor != null )
            {
                div.style.backgroundColor = revertColor;
            }
            else
            {
                div.style.backgroundColor = '#FFFFFF';
            }
        }//catch(ex){}
    }

    // Called when a friends name is clicked
    function friendMouseClick(divId)
    {    
	
        //try
        {
            openChatWindowEx(divId);
        }
        //catch(ex){}
    }

    // Set the nick of the logged in member
    function setMyNick(varMyNick)
    {    
        //try
        {
            myNick = varMyNick;                
            var myTitleNick = document.getElementById('myTitleNick');
            
            if ( myTitleNick == null )
                return;
           
           myTitleNick.innerHTML = myNick;
       }
       //catch(ex){}
    }

    // Set the status of the logged in member
    function setMyStatus(onlineStatus)
    {    
        //try
        {
            //change the icon according to the status
            if (onlineStatus == 'Online')
            {
                changeMyStatusIcon('../images/online.gif');
            }
            else if (onlineStatus == 'Offline' )
            {
                changeMyStatusIcon('../images/offline.gif');        
            }
            else if (onlineStatus == 'Busy' )
            {
                changeMyStatusIcon('../images/busy.png');  
            }
            else if (onlineStatus == 'Away' )
            {
            
            }
        }//catch(ex){}
    }

    // Chate the status icon of the logged in member
    function changeMyStatusIcon(imgUrl)
    {   
        //try
        {
            var myStatusIcon = document.getElementById('myStatusIcon');
            
            if ( myStatusIcon == null )
                return;

            myStatusIcon.src = imgUrl;            
        }//catch(ex){}
    }

    // Hide Unhide the set status menu
    function toggleStatusMenu()
    {
        //try
        {
            var divStatusMenu = document.getElementById('divStatusMenu');
            var offsetHeight = document.getElementById('mainDiv').offsetHeight;        
            
            if (divStatusMenu)
            {
                divStatusMenu.style.top = -(offsetHeight - 135); 
                
                if (divStatusMenu.style.display != 'none')
                {
                    divStatusMenu.style.display='none';
                    divStatusMenu.style.visibility = 'visible';
                }
                else
                {
                    divStatusMenu.style.display='block';
                    divStatusMenu.style.visibility = 'hidden';
                }                        
            }
        }
        //catch(ex){}
    }
            
        
    // Set status of the friend specified by the member Id    
    function setFriendStatus(otherMemberWebID,onlineStatus)
    {
        //try
        {
            //change the icon according to the status
            if (onlineStatus == 'Online')
            {
                changeFriendStatusIcon(otherMemberWebID,'../images/online.gif');
            }
            else if (onlineStatus == 'Offline' )
            {
                changeFriendStatusIcon(otherMemberWebID,'../images/offline.gif');        
            }
            else if (onlineStatus == 'Busy' )
            {
                changeFriendStatusIcon(otherMemberWebID,'../images/busy.png');
            }
            else if (onlineStatus == 'Away' )
            {
            
            }
        }
        //catch(ex){}
    }
    
    ///
    /// Returns the current status icon url of the friend
    ///
    function getFriendStatusIcon(otherMemberWebID)
    {
        //try
        {
           var statusIcon = document.getElementById('statusIcon' + otherMemberWebID);
            
            if ( statusIcon == null )
                return;

            return statusIcon.src;
        }
        //catch(ex){}
    }             
     
    ///
    /// Helper function for chaning the status icon of a friend
    ///  
    function changeFriendStatusIcon(otherMemberWebID,imgUrl)
    {        
        //try
            {
            var statusIcon = document.getElementById('statusIcon' + otherMemberWebID);
            
            if ( statusIcon == null )
                return;

            statusIcon.src = imgUrl;
        }
        //catch(ex){}
    }	
	
	function sortFriend(objA,objB)
    {
        if( objA.OnlineStatusString == objB.OnlineStatusString )
        {
            if( objA.NickName > objB.NickName )
            {
                return 1;
            }
            else if( objA.NickName < objB.NickName )
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }            
        
        if( objA.OnlineStatusString == "Online" && 
                    (objB.OnlineStatusString == "Away" || objB.OnlineStatusString == "Offline" ) )
        {
            return -1;
        }
        else if( objA.OnlineStatusString == "Away" && objB.OnlineStatusString == "Offline" )
        {        
            return -1;
        }
        else
        {
            return 1;
        }
    }
	
	///
	/// Show the specified friend in the Friends List
	///
	function showFriendInList(otherMemberWebID,otherMemberNick,onlineStatus,position)
    {
	
        //try
        {
            var chatClient = document.getElementById('friendListDiv');
            var divFriendSource = document.getElementById('divFriendSource');            
            
            var newDiv = divFriendSource.cloneNode(true);   


            
            newDiv.setAttribute("id",otherMemberWebID);            
            newDiv.onmouseover = function() { divMouseOver(otherMemberWebID);};
            newDiv.onmouseout = function() { divMouseOut(otherMemberWebID);};
            newDiv.onclick = function onclick(event) { friendMouseClick(otherMemberWebID);};


            newDiv.style.visibility = 'visible';
            newDiv.style.display = 'block';
            
            for(i=0; i < newDiv.childNodes.length; i++)
            {
                if (newDiv.childNodes[i].id == 'statusIcon'
                    || newDiv.childNodes[i].id == 'titleNick'
                    || newDiv.childNodes[i].id == 'divCustomMessageParent'
                    || newDiv.childNodes[i].id == 'divCustomMessage'                    
                )
                {   
                    newDiv.childNodes[i].id = newDiv.childNodes[i].id + otherMemberWebID;
                }                    
            }
            
            chatClient.appendChild(newDiv);
            setFriendNick(otherMemberWebID,otherMemberNick); 
            setFriendStatus(otherMemberWebID,onlineStatus);
            
            if(chatClient.clientHeight < 160)
            {
               chatClient.style.height = (chatClient.clientHeight + 21) +'px';               
            }
        }//catch(ex){}
   }
   
   
    ///
	/// Show the specified friend in the Friends List
	///
	function adjustFriendInList(otherMemberWebID,position)
    {
        //try
        {
            var chatClient = document.getElementById('friendListDiv');
            var thisFriend = document.getElementById(otherMemberWebID);           
            
            
            if( chatClient.childNodes.item(position) != thisFriend )
            {
                chatClient.removeChild( thisFriend );
                chatClient.insertBefore( thisFriend, chatClient.childNodes.item(position) );                
            }
        }//catch(ex){}
   }

   
   
   ///
	/// Remove the specified friend from the Friends List
	///
	function removeFriendFromList(otherMemberWebID)
    {
        //try
        {
            var chatClient = document.getElementById('friendListDiv');
            var newDiv = document.getElementById(otherMemberWebID);
            
            if( newDiv != null )
            {            
                chatClient.removeChild(newDiv);            
                
                if( friendsForUpdate !=null && friendsForUpdate.length < 8 )
                {
                   chatClient.style.height = (friendsForUpdate.length * 21) +'px';               
                }
            }
        }//catch(ex){}
   }       
   
   ///
   /// Set the display position of the specified friend
   ///
   function setFriendPosition(otherMemberWebID,position)
   {
        //try
        {
            var friendPosition = document.getElementById('friendPosition' + otherMemberWebID);
            if ( friendPosition == null )
                return;

            friendPosition.innerHTML = position;
        }
       //catch(ex){}
   }
   
   ///
   /// Get the display position of the specified friend
   ///
   function getFriendPosition(otherMemberWebID)
   {
        //try
        {
            var friendPosition = document.getElementById('friendPosition' + otherMemberWebID);
            if ( friendPosition == null )
                return -1;

            return friendPosition.innerHTML - 0;
        }
       //catch(ex){}
   }
   
   ///
   /// Set the nick of the specified friend
   ///
   function setFriendNick(otherMemberWebID,otherMemberNick)
   {
        //try
            {
            var titleNick = document.getElementById('titleNick' + otherMemberWebID);
            if ( titleNick == null )
                return;

           titleNick.innerHTML = otherMemberNick;
       }
       //catch(ex){}
   }
    
    ///
    ///Close the chat window of the specified friend
    ///
    function closeFriendChatWindow(otherMemberId)
    {
        //try
        {
            var chatParentDiv = document.getElementById('chatParentDiv');
            var iFrame = getIframe("chatParentIframe" + "" + otherMemberId);
            chatParentDiv.removeChild( iFrame );
        }
        //catch(ex){}
    }
    
    function minRestoreChatClient()
    {
        try
        {
            if( minimized )
                restoreChatClient();
            else
                minimizeChatClient();
        }
        catch(ex){}
    }
    
    ///
        /// Minimize chat window
        ///
        function minimizeChatClient()
        {
            try
            {
                var friendListDiv = parent.document.getElementById('friendListDiv');                
                friendWindowHeight = extractVal( friendListDiv.style.height);
                friendListDiv.style.height = '0px';
                
                minimized = true;
            }
            catch(ex){}
        }
        
        ///
        /// Restore chat window
        ///
        function restoreChatClient()
        {
            try
            {
                var friendListDiv = parent.document.getElementById('friendListDiv');
                friendListDiv.style.height = friendWindowHeight + 'px';
                
                minimized = false;          
            }
            catch(ex){}            
        }
	
	///
	/// Set the icon of the window of the specified friend
	///
	function setFriendWindowStatusIcon(otherMemberId,onlineStatus)
    {   
        //try
        {
            //get any open chat windows
            var iFrameDoc = getIframeDoc("chatParentIframe" + "" + otherMemberId);          
            //if any chat window is open for the specific member
            //set the icon
            if ( iFrameDoc != null)
            {
                if ( iFrameDoc.setStatusIcon)
                {
                    iFrameDoc.setStatusIcon(onlineStatus);
                }                
            }
        }//catch(ex){}
    }
    
    ///
	/// Set the nick displayed on top of the window of the specified friend
	///	
    function setFriendWindowMemberNick(otherMemberId,NickName)
    {      
        //try
        {
            //get any open chat windows
            var iFrameDoc = getIframeDoc("chatParentIframe" + "" + otherMemberId);                   
            //if any chat window is open for the specific member
            //set the nick
            if ( iFrameDoc != null)
            {      
                if ( iFrameDoc.setOtherMemberNick)
                {
                    iFrameDoc.setOtherMemberNick(NickName);
                }
            }
        }
        //catch(ex){}
    }
    
    ///
    /// Clear the friends list
    /// 
    function clearFriends()
    {   
        //try
        {
            var chatClient = document.getElementById('friendListDiv');
            var divFriendSource = document.getElementById('divFriendSource');
            var divMySelf = document.getElementById('divMySelf');
            
            var i = 0;
            for(i = 0; i< friends.length; i++)
            {   
                var friendInList = document.getElementById(friends[i].WebMemberID);
                //try
                {            
                    chatClient.removeChild( friendInList );
                }
                //catch(ex){}
            }
        }
        //catch(ex1){}
    }
    
    ///
    /// Helper function used for merging the complete friend's list
    /// with the new friend list with status
    ///
    function mergeFriendStatus(varFriendsStatus)
    {
        //try
        {       
            setAllFriendsOffline();

            var i = 0;
            for(i=0; i < varFriendsStatus.length ; i++ )
            {   
                var friend = getFriendObject( varFriendsStatus[i].WebMemberID );
                if ( friend != null )
                {
                    //try
                //    {
                        friend.OnlineStatusString = varFriendsStatus[i].OnlineStatusString;
                        friend.NickName = varFriendsStatus[i].NickName;
                //    }
                    //catch(ex){}
                }
            }
        }
        //catch(ex2){}
    }
    
    ///
    /// Set the status of all friends to offline.
    /// Called before doing a mergeFriendStatus
    /// Doesn't effect the actual status of any friend nor the displayed status of any friend
    ///
    function setAllFriendsOffline()
    {
        //try
        {
            var i = 0;
            for(i=0; i < friends.length; i++ )
            {   
                friends[i].OnlineStatusString = 'Offline';
            }        
        }//catch(ex){}
    }
    
    ///
    /// Returns a single friend object
    ///
    function getFriendObject(WebMemberID)
    {
        //try
        {
            var i = 0;
            for(i=0; i < friends.length; i++ )
            {
                if ( WebMemberID == friends[i].WebMemberID )
                    return friends[i];            
            }
            return null;
        }
        //catch(ex){return null;}
    } 
    
    
    ///
    /// Returns a single friend object
    ///
    function getFriendsForUpdateObject(WebMemberID)
    {
        try
        {
            if( friendsForUpdate == null )
            {
                return null;
            }
            
            var i = 0;
            for(i=0; i < friendsForUpdate.length; i++ )
            {
                if ( WebMemberID == friendsForUpdate[i].WebMemberID )
                    return friendsForUpdate[i];            
            }
            return null;
        }
        catch(ex){return null;}
    } 
	
	///
	/// Populates the Friend list displayed to the user
	///
	function populateFriendList() 
    {
        //try
        {   
            var i = 0;
            for(i =0;i<friends.length;i++) 
            {
                showFriendInList(friends[i].WebMemberID,friends[i].NickName,friends[i].OnlineStatus,i);
            }
        }
        //catch(ex){}
    }
    
    
    ///
	/// Update the Friend list displayed to the user
	///
    function updateFriendList() 
    {
        try
        {   
            if( friendsForUpdate == null )
            {
                return;
            }
            
            var i = 0;
            var f = null;
            
            // Add any new friends to the list
            for(i =0;i<friendsForUpdate.length;i++) 
            {
                f = getFriendObject( friendsForUpdate[i].WebMemberID );
                
                // if the friend doesn't already exist
                if( f == null )
                {                
                    showFriendInList(friendsForUpdate[i].WebMemberID,friendsForUpdate[i].NickName,friendsForUpdate[i].OnlineStatus);
                }
            }
            
            // Remove any deleted friends from the list
            for(i =0;i<friends.length;i++) 
            {
                f = getFriendsForUpdateObject ( friends[i].WebMemberID );
                
                // if the friend doesn't exist in the updated friend list 
                // then he/she has been unfriended
                // So remove from the list
                if( f == null )
                {                
                    removeFriendFromList(friends[i].WebMemberID);
                }
            }
            
            friends = friendsForUpdate;
        }
        catch(ex){}
    }
    
    ///
    /// Refresh the friend list
    ///
    function refreshFriends()
    {   
        //try
        {
            clearFriends();
            getFriends(true);
            setTimeout("refreshFriends()",60000); 
        }
        //catch(ex){setTimeout("refreshFriends()",60000);}
    }
   
   /////////////////////////////////////////////////////////////////////////////////////////
   //////// FUNCTIONS THAT INTERACT WITH SERVER
   /////////////////////////////////////////////////////////////////////////////////////////
   
    
   /////////////////////////////////////////////////////
   //GET FRIENDS STATUS
   //Get information about added friends
   //Returns an array of AjaxMember objects which contain the following information
   //Status of the the friend i.e Online,Away,Busy
   //Nick of the friend
   //Custom Message of the friend
   function getFriendsStatus() 
   {
        //try
        {
            if (ajaxMember != null )
            {        
                // when calling an AJAX method, add the callback as an extra parameter as the last argument
                ChatClient.GetFriendsStatus(ajaxMember.WebMemberID, getFriendsStatus_CallBack);
                
            }
        }
        //catch(ex){setTimeout("getFriendsStatus()",5000);}
    }
   
   /////////////////////////////////////////////////////
   //GET FRIENDS STATUS CALLBACK 
    function getFriendsStatus_CallBack(response) 
    {      
        //try
        {            
            var friendsStatus = null; 
            if (response != null )
                friendsStatus = response.value;        
            
            if ( friendsStatus != null )
                mergeFriendStatus( friendsStatus );
                  
            try
            {          
                friends.sort(sortFriend);
            }
            catch(ex){}
            
            var i = 0;            
            
            for(i=0; i < friends.length; i++ )
            {
                setFriendStatus(friends[i].WebMemberID,friends[i].OnlineStatusString);                        
                setFriendWindowStatusIcon(friends[i].WebMemberID,friends[i].OnlineStatusString);  
                setFriendNick(friends[i].WebMemberID,friends[i].NickName);
                setFriendWindowMemberNick(friends[i].WebMemberID,friends[i].NickName);                
                
                try
                {
                    adjustFriendInList( friends[i].WebMemberID, i );
                }
                catch(ex2){}
            }
           
            getFriendStatusLastCall = new Date();
            
            setTimeout("getFriendsStatus()",5000); 
        }
        //catch(ex){setTimeout("getFriendsStatus()",5000);}
    }
    
    
   /////////////////////////////////////////////////////
   //GET FRIENDS
   function getFriends() 
   {   
        //try
        {
            if (ajaxMember != null )
            {
               // when calling an AJAX method, add the callback as an extra parameter as the last argument
                ChatClient.GetFriends(ajaxMember.WebMemberID, getFriends_CallBack);
                
            }
        }
        //catch(ex){setTimeout("getFriends()",5000);}
    }    
    
    //GET FRIENDS CALLBACK
    function getFriends_CallBack(response) 
    {   
        //try
        {
            if (response != null )
                friends = response.value;
            
            if( friends != null )
                friends.sort(sortFriend);
                
            populateFriendList();
            getFriendsStatus();
        }
        //catch(ex){}
    }
    
    
    /////////////////////////////////////////////////////
   //GET FRIENDS
   function getFriendsForUpdate() 
   {   
        //try
        {
            if (ajaxMember != null )
            {
               // when calling an AJAX method, add the callback as an extra parameter as the last argument
                ChatClient.GetFriends(ajaxMember.WebMemberID, getFriendsForUpdate_CallBack);
                
            }
        }
        //catch(ex){setTimeout("getFriends()",5000);}
    }    
    
    //GET FRIENDS CALLBACK
    function getFriendsForUpdate_CallBack(response) 
    {   
        try
        {
            if (response != null )
                friendsForUpdate = response.value;
                
            updateFriendList();  
            
            setTimeout('getFriendsForUpdate()',60000);
        }
        catch(ex){setTimeout('getFriendsForUpdate()',60000);}
    }
    
    
    /////////////////////////////////////////////////////
    //GET NEW MESSAGES & CALLBACK
    function getNewMessages() 
    {   
        //try
        {
            ChatClient.GetNewMessages(myMemberID, getNewMessages_CallBack);        
        }
        //catch(ex){}
    }

    var messages = null;
    function getNewMessages_CallBack(response)
    {
        //try
        {
            if (response.value != "" && response.value != null)
            {   
                messages = response.value;
                  
                var i = 0; 
                for(i=0;i<messages.length;i++)
                {                 
                    openChatConsole(messages[i].OtherMemberWebID,messages[i].Message,messages[i].OtherMemberNick);                    
                    //var imgUrl = getFriendStatusIcon( messages[i].OtherMemberWebID );
                    //var friend = getFriendObject( messages[i].OtherMemberWebID );

                    //setTimeout("setFriendWindowStatusIcon('" + messages[i].OtherMemberWebID + "','" + friend.OnlineStatusString + "')",500);
                    //setTimeout("setFriendWindowMemberNick('" + messages[i].OtherMemberWebID + "','" + friend.NickName + "')",500);
                }
            }            
            
            setTimeout('showMessageHelper()',1000);
       }
       //catch(ex){setTimeout("getNewMessages()",2000);}
    }
    
    function showMessageHelper()
    {
        if( messages != null )
        {
            var i = 0;
            var addStatus = false;
            var chatWindow = false;
            
            for(i=0;i<messages.length;i++)
            {                    
                addStatus = addMessageToDisplay(messages[i].OtherMemberWebID,messages[i].Message,messages[i].OtherMemberNick); 
                
                if( addStatus == false )
                {
                    i--;
                }
                else
                {
                    //chatWindow = GetChatWindow( messages[i].OtherMemberWebID );
                    //chatWindow.hasNewMessages = true;
                    //chatWindow.message = messages[i].OtherMemberNick + " Says ...";
                                                                                
                    //blinkTitle = messages[i].OtherMemberNick + " Says ...";
                    //flashTitle();
                }
            }
        }
                
        messages = null;
        getMessageLastCall = new Date();
        // set the interval for the new message getter
        setTimeout("getNewMessages()",2000);
    }

    /////////////////////////////////////////////////////
    // Set Status & CALLBACK
    function setStatus(onlineStatus,customMessage)
    {  
        //try
        {
            if (ajaxMember!=null && ajaxMember.WebMemberID != ''){
                // log into the chat server
                tempStatus = onlineStatus;           
                ChatClient.SetOnlineStatus(ajaxMember.WebMemberID, onlineStatus,'', setStatus_CallBack);
            }
        }
        //catch(ex){}
    }
    
    function setStatus_CallBack(response) 
    {
        //try
        {
            var res = response.value;
            if ( res == true)
            {
                setMyStatus(tempStatus);
                tempStatus = '';
            }                
        }
        //catch(ex){}
    }
        
    /////////////////////////////////////////////////////
    // LOGIN & CALLBACK
    function login()
    {
       //try
        {
            // log into the chat server
            ChatClient.LoginToChatServer(login_CallBack);        
        }
        //catch(ex){}
    }
    
    function login_CallBack(response) 
    {        
        //try
        {
            if ( response != null )
            {
                ajaxMember = response.value;
            }        
            
            //if the user logged in successfully
            if (ajaxMember!=null && ajaxMember.WebMemberID != ''){
                    setMyMemberID(ajaxMember.WebMemberID);                   
                    setMyNick(ajaxMember.NickName);
                    setMyStatus(ajaxMember.OnlineStatusString);                    
                    getFriends();
                    getNewMessages();
                    setTimeout('getFriendsForUpdate()',60000);
            }else
            {
                ////try loggin in again
                //setTimeout('login()',5000);
            }
        }
        //catch(ex){}  
    }
    
    /////////////////////////////////////////////////////
    // LOGOUT & CALLBACK
   function logout()
   {
       //try
       {
            // log out of the chat server
            if (ajaxMember != null )
            {                
                ChatClient.LogoutOfChatServer(ajaxMember.WebMemberID);//, logout_CallBack);                
                
                try
                {
                    clearFriends();  
                    setStatus('Offline','');
                }catch(ex1){}
            }
       }
       //catch(ex){}
    }
    
   function logout_CallBack(response) 
   {
        //try
        {
            //if the user logged out successfully
            if (response!=null && response.value == true)
            {   
                clearFriends();  
                setStatus('Offline','');
            }
        }
        //catch(ex){}
    }
    
    ///
    /// Check Sanity
    ///
    function checkSanity() 
    {   
        try
        {
            var currentTime = new Date();            
            var diffSecLastMsg = (currentTime - getMessageLastCall) / 1000;
            var diffSecFriendStatus =  (currentTime - getFriendStatusLastCall) / 1000;
            
            if( ajaxMember != null )
            {
                if( diffSecLastMsg >= 30 )
                {
                    getNewMessages();
                }
                    
                if( diffSecFriendStatus >= 30 )
                {
                   getFriendsStatus();
                }
                 
            }
            setTimeout('checkSanity()',15000);
        }
        catch(ex){setTimeout('checkSanity()',15000);}
    }

    var event_id = 1;
    /////////////////////////////////////////////////////////
    //Log events for debuggin only (you must uncomment text area txtConsole to display log)
    function log(event){    
        //var txtConsole = document.getElementById('txtConsole');
        //txtConsole.value = event + "\r" + txtConsole.value;
    }
    
    ///
    /// Flash the title on receipt of new message
    ///
    function flashTitle()
    {
	return;
            
        //try
        {            
            var i = 0;
            
            for( i = 0; i < openChatWindows.length; i++ )
            {
                //alert(openChatWindows[i].message);
            }
            
            //log('focused' + focused );
            if( focused == true || mainFocused == true )
            {   
                cancelTitleFlash();
                return;
            }
            
            if (document.title == actualTitle)
            {   
                //log('same');   
                document.title = blinkTitle;
            }
            else
            {
                //log('different');
                document.title = actualTitle;
            }
            
            setTimeout("flashTitle()",2000);
        }
        //catch(ex){}
    }
    
    ///
    /// Cancel the title flash
    ///
    function cancelTitleFlash()
    {
        //try
        {   
            document.title = actualTitle;
        }
        //catch(ex){}
    }
    
    function setFocusVar(foc)
    {
        //log('foc ' + foc);
        focused = foc;  
        //log('focusedfoc ' + focused);      
    }
    
    function setMainFocusVar(foc)
    {
        //log('mainfoc ' + foc);        
        mainFocused = foc;          
        //log('mainFocused ' + mainFocused);        
    }
    
    function setReference()
    {
        try
        {
            if( window.opener != null && !window.opener.closed && window.opener.setChatWinReference)
            {
                window.opener.setChatWinReference(window);
            }

            setTimeout('setReference()',5000);
        }
        catch(ex)
        {
            setTimeout('setReference()',5000);
        }
    }
    
    
    function sleep(millis)
        {
            var date = new Date();
            var curDate = null;

            do { curDate = new Date(); }
            while(curDate-date < millis);
        } 