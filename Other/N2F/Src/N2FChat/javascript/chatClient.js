
        //Utility functions
        function sleep(millis)
        {
            var date = new Date();
            var curDate = null;

            do { curDate = new Date(); }
            while(curDate-date < millis);
        } 

   
        // Friend List related javascript
           
            
        
        
        var timeOutMsgId = null;   
        
        function test()
        {   
            getNewMessages();
        }
        
        function getIframe( frameName )
        {   
            var frm = document.frames ? document.frames[frameName] : document.getElementById(frameName);
            //var frm = document.getElementById(frameName);            
            //alert(frm.contentWindow);
            return frm;
        }
        
        
        function openChatWindow(otherMemberId,message,otherMemberNick)
        {   
            var chatParentDiv = document.getElementById('chatParentDiv');
            var chatParentIframeSource = document.getElementById('chatParentIframe');
            var newIframe = null;
            var createdNew = false;
            
            if( otherMemberId != null )
            {
                newIframe = getIframe("chatParentIframe" + "" + otherMemberId);
            }
            
            if( newIframe == null )
            {
                createdNew = true;
                //newIframe = document.createElement("iframe");
                newIframe = chatParentIframeSource.cloneNode(true);
                newIframe.src = "chatWindow.aspx?a=" + otherMemberId + "&b=" + otherMemberNick;                             
                newIframe.setAttribute("id","chatParentIframe" + "" + otherMemberId);
                newIframe.style.visibility = 'visible';
                newIframe.style.display = 'inline';          
                chatParentDiv.appendChild(newIframe);                
            }
            

            if( createdNew )
            {
                timeOutMsgId = setTimeout("temp('" + otherMemberId + "','" + otherMemberNick + "','" + message + "')",1000);
            }
            else
            {
                temp(otherMemberId,otherMemberNick,message);
            }            
        }
        
        function temp(otherMemberId,otherMemberNick, message)
        {
            log(message);   
            clearTimeout(timeOutMsgId);            
            
            var iFrame = getIframe("chatParentIframe" + "" + otherMemberId);
                        
            if( message != null && message != '')
            {
                if( iFrame.contentWindow && iFrame.contentWindow.addMessageToDisplay)               
                    iFrame.contentWindow.addMessageToDisplay(otherMemberNick,message);
                else if(iFrame.addMessageToDisplay)
                    iFrame.addMessageToDisplay(otherMemberNick,message);
                    
               //messageDisplayStatus = true;
            }            
        }        
        
       
    var myMemberID = -1;       // current users memberid
    var EmailAddress = '';       // current users EmailAddress
    var Password = '';         // current users Password
    var myNick = ''; 
    var ajaxMember = null;
    var friends = null;
    var tempStatus = '';
    var actualTitle = 'N2F Chat';
    var blankTitle = 'New Message';
    var flashTitleIntervalId = -1;    
    var customText = '';
    //var messageDisplayStatus = false;
    
    /* comma delimited list of friends online. This is not currently implemented but instead of just storing
    friends as values in the listbox, they should be javascript objects and hold properties such as.
           1 memberID
           2 NickName
           3 AwayStatus
           4 email address
    
    This makes the application better organised and allows us to add in extra functionality with ease
    */
    
    function getMyMemberID()
    {   
        return myMemberID;
    }
    
    function setMyMemberID(memberID)
    {
        myMemberID = memberID;
    }
    
    function getMyMemberNick()
    {
        return myNick;
    }
     
    // a new chat message has come in from the server or the user clicked on a name from the friend list.
    // this launches a new chatwindow console for this member   
    function openChatConsole(memberID, message, nickName)
    {   
        openChatWindow( memberID ,message,nickName);
        //parent.createAppendChatConsole(memberID, message,nickName);
    }

    //SEND CHAT MESSAGE TO SERVER
    function sendChat(toMemberID, message){     
    
        // add a blank string to convert the ints to strings        
        ChatClient.SendMessage(toMemberID+'', myMemberID+'', EmailAddress, message);
        //log('Message sent');
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
//        if(response != null){
//            if(response.value != ""){
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
    
    function divMouseOver(divId)
    {
        var div = document.getElementById(divId);
        div.style.backgroundColor = '#E0ECFF';
    }

    function divMouseOut(divId,revertColor)
    {
        var div = document.getElementById(divId);
        if( revertColor != null )
            div.style.backgroundColor = revertColor;
        else
            div.style.backgroundColor = '#FFFFFF';
    }

    function friendMouseClick(divId)
    {    
        openChatConsole(divId,'','');
        var imgUrl = getFriendStatusIcon( divId );
        var friend = getFriendObject( divId );

        setTimeout("setFriendWindowStatusIcon('" + divId + "','" + friend.OnlineStatusString + "')",500);
        setTimeout("setFriendWindowMemberNick('" + divId + "','" + friend.NickName + "')",500);        
    }

    function setMyNick(varMyNick)
    {    
        myNick = varMyNick;                
        var myTitleNick = document.getElementById('myTitleNick');
        
        if( myTitleNick == null )
            return;

       myTitleNick.innerHTML = myNick;               
    }

    function setMyStatus(onlineStatus)
    {    
       //change the icon according to the status
        if(onlineStatus == 'Online')
        {
            changeMyStatusIcon('img/available.png');
        }
        else if(onlineStatus == 'Offline' )
        {
            changeMyStatusIcon('img/offline.png');        
        }
        else if(onlineStatus == 'Busy' )
        {
            changeMyStatusIcon('img/busy.png');  
        }
        else if(onlineStatus == 'Away' )
        {
        
        }
    }

    function changeMyStatusIcon(imgUrl)
    {   
        var myStatusIcon = document.getElementById('myStatusIcon');
        
        if( myStatusIcon == null )
            return;

        myStatusIcon.src = imgUrl;            
    }

    function toggleStatusMenu()
    {
        var divStatusMenu = document.getElementById('divStatusMenu');
        var offsetHeight = document.getElementById('mainDiv').offsetHeight;                
        divStatusMenu.style.top = -(offsetHeight - 135); 
        
        if(divStatusMenu)
        {
            if(divStatusMenu.style.display != 'none')
            {
                divStatusMenu.style.display='none';
            }
            else
            {
                divStatusMenu.style.display='block';
            }                        
        }
    }
            
            
    function setFriendStatus(otherMemberWebID,onlineStatus)
    {
        //change the icon according to the status
        if(onlineStatus == 'Online')
        {
            changeFriendStatusIcon(otherMemberWebID,'img/available.png');
        }
        else if(onlineStatus == 'Offline' )
        {
            changeFriendStatusIcon(otherMemberWebID,'img/offline.png');        
        }
        else if(onlineStatus == 'Busy' )
        {
            changeFriendStatusIcon(otherMemberWebID,'img/busy.png');
        }
        else if(onlineStatus == 'Away' )
        {
        
        }
    }
    
    ///
    /// Returns the current status icon url of the friend
    ///
    function getFriendStatusIcon(otherMemberWebID)
    {
       var statusIcon = document.getElementById('statusIcon' + otherMemberWebID);
        
        if( statusIcon == null )
            return;

        return statusIcon.src;
    }             
     
    ///
    /// Helper function for chaning the status icon of a friend
    ///  
    function changeFriendStatusIcon(otherMemberWebID,imgUrl)
    {        
        var statusIcon = document.getElementById('statusIcon' + otherMemberWebID);
        
        if( statusIcon == null )
            return;

        statusIcon.src = imgUrl;            
    }
	
	///
	/// Show the specified friend in the Friends List
	///
	function showFriendInList(otherMemberWebID,otherMemberNick,onlineStatus)
    {
        var chatClient = document.getElementById('chatClient');
        var divFriendSource = document.getElementById('divFriendSource');
        
        var newDiv = divFriendSource.cloneNode(true);
        
        
        newDiv.setAttribute("id",otherMemberWebID);            
        newDiv.onmouseover = function() { divMouseOver(otherMemberWebID);}
        newDiv.onmouseout = function() { divMouseOut(otherMemberWebID);}
        newDiv.onclick = function() { friendMouseClick(otherMemberWebID);}
        newDiv.style.visibility = 'visible';
        newDiv.style.display = 'block';
        
        for(i=0; i < newDiv.childNodes.length; i++)
        {
            if(newDiv.childNodes[i].id == 'statusIcon'
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
   }
       
   ///
   /// Set the nick of the specified friend
   ///
   function setFriendNick(otherMemberWebID,otherMemberNick)
   {
        var titleNick = document.getElementById('titleNick' + otherMemberWebID);
        if( titleNick == null )
            return;

       titleNick.innerHTML = otherMemberNick;               
   }
	   
	
	///
	/// NOT USED
	///  
    function getChatWindow(otherMemberId)
    {
        var newDiv = document.createElement(otherMemberId);
        return newDiv;
    }
    
    ///
    ///Close the chat window of the specified friend
    ///
    function closeFriendChatWindow(otherMemberId)
    {
        var chatParentDiv = document.getElementById('chatParentDiv');
        //FIX ME
        // getIframe custom function should work here as it does every where else
        var iFrame = document.getElementById("chatParentIframe" + "" + otherMemberId);            
        chatParentDiv.removeChild( iFrame );
    }
    
    function minimizeChatWindow(otherMemberId)
    {   
        var chatParentDiv = document.getElementById('chatParentDiv');
        var iFrame = getIframe("chatParentIframe" + "" + otherMemberId);            
        chatParentDiv.removeChild( iFrame );
    }
	
	///
	/// Set the icon of the window of the specified friend
	///
	function setFriendWindowStatusIcon(otherMemberId,onlineStatus)
    {   
        //get any open chat windows
        var iFrame = getIframe("chatParentIframe" + "" + otherMemberId);          
        //if any chat window is open for the specific member
        //set the icon
        if( iFrame != null)
        {
            if( iFrame.contentWindow && iFrame.contentWindow.setStatusIcon)
                iFrame.contentWindow.setStatusIcon(onlineStatus);
            else if(iFrame.setStatusIcon)
                iFrame.setStatusIcon(onlineStatus);
        }
    }
    
    ///
	/// Set the nick displayed on top of the window of the specified friend
	///	
    function setFriendWindowMemberNick(otherMemberId,NickName)
    {           
        //get any open chat windows
        var iFrame = getIframe("chatParentIframe" + "" + otherMemberId);                   
        //if any chat window is open for the specific member
        //set the nick
        if( iFrame != null)
        {      
            if( iFrame.contentWindow && iFrame.contentWindow.setOtherMemberNick)
                iFrame.contentWindow.setOtherMemberNick(NickName);
            else if(iFrame.setOtherMemberNick)
                iFrame.setOtherMemberNick(NickName);
        }
    }
    
    ///
    /// Clear the friends list
    /// 
    function clearFriends()
    {   
        var chatClient = document.getElementById('chatClient');
        var divFriendSource = document.getElementById('divFriendSource');
        var divMySelf = document.getElementById('divMySelf');
        for(i = 0; i< friends.length; i++)
        {   
            var friendInList = document.getElementById(friends[i].WebMemberID);
            try
            {            
                chatClient.removeChild( friendInList );
            }
            catch(ex){}
        }
    }
    
    ///
    /// Helper function used for merging the complete friend's list
    /// with the new friend list with status
    ///
    function mergeFriendStatus(friendsStatus)
    {
        setAllFriendsOffline();
        for(i=0; i < friendsStatus.length; i++ )
        {   
            var friend = getFriendObject( friendsStatus[i].WebMemberID );
            if( friend != null )
            {
                try
                {
                    friend.OnlineStatusString = friendsStatus[i].OnlineStatusString;
                    friend.NickName = friendsStatus[i].NickName;
                }
                catch(ex){}
            }
        }
    }
    
    ///
    /// Set the status of all friends to offline.
    /// Called before doing a mergeFriendStatus
    /// Doesn't effect the actual status of any friend nor the displayed status of any friend
    ///
    function setAllFriendsOffline()
    {
        //if( friends == null || !friends.length)
        //    return;
            
        for(i=0; i < friends.length; i++ )
        {   
            friends[i].OnlineStatusString = 'Offline';
        }
    }
    
    ///
    /// Returns a single friend object
    ///
    function getFriendObject(WebMemberID)
    {
        for(i=0; i < friends.length; i++ )
        {
            if( WebMemberID == friends[i].WebMemberID )
                return friends[i];            
        }
        return null;
    } 
	
	///
	/// Populates the Friend list displayed to the user
	///
	function populateFriendList() 
    {
      for(var i =0;i<friends.length;i++) 
        {
            showFriendInList(friends[i].WebMemberID,friends[i].NickName,friends[i].OnlineStatus);
        }
    }
    
    ///
    /// Refresh the friend list
    ///
    function refreshFriends()
    {   
        clearFriends();
        getFriends();
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
        if(ajaxMember != null )
        {        
            // when calling an AJAX method, add the callback as an extra parameter as the last argument
            ChatClient.GetFriendsStatus(ajaxMember.WebMemberID, getFriendsStatus_CallBack);
            
        }
    }
   
   /////////////////////////////////////////////////////
   //GET FRIENDS STATUS CALLBACK 
    function getFriendsStatus_CallBack(response) 
    {      
        var friendsStatus = null; 
        if(response != null )
            friendsStatus = response.value;        
        
        if( friendsStatus != null )
            mergeFriendStatus( friendsStatus )
                    
        for(i=0; i < friends.length; i++ )
        {
            setFriendStatus(friends[i].WebMemberID,friends[i].OnlineStatusString);                        
            setFriendWindowStatusIcon(friends[i].WebMemberID,friends[i].OnlineStatusString);  
            setFriendNick(friends[i].WebMemberID,friends[i].NickName);
            setFriendWindowMemberNick(friends[i].WebMemberID,friends[i].NickName);
        }     
        
        setTimeout("getFriendsStatus()",5000); 
    }
    
    
   /////////////////////////////////////////////////////
   //GET FRIENDS
   function getFriends() 
   {   
        if(ajaxMember != null )
        {
           // when calling an AJAX method, add the callback as an extra parameter as the last argument
            ChatClient.GetFriends(ajaxMember.WebMemberID, getFriends_CallBack);
            
        }
    }    
    
    //GET FRIENDS CALLBACK
    function getFriends_CallBack(response) 
    {          
        if(response != null )
            friends = response.value;        
        
        //if( friends == null || !friends.length)
        //    return;
            
        //for(i=0; i < friends.length; i++ )
        //{
            //log("Friend #" + i + " = " + friends[i].NickName);
        //}
        
        populateFriendList();
    }   
    
   
    
    /////////////////////////////////////////////////////
    //GET NEW MESSAGES & CALLBACK
    function getNewMessages() 
    {   
        ChatClient.GetNewMessages(myMemberID, getNewMessages_CallBack);        
    }

    function getNewMessages_CallBack(response){
        //log('Sucessfully returned new messages');                
        
        if(response.value != "" && response.value != null)
        {                       
            var messages = response.value;
            //log(messages.length);         
               
            for(var i=0;i<messages.length;i++)
            {                    
                //messageDisplayStatus = false;
                openChatConsole(messages[i].OtherMemberWebID,messages[i].Message,messages[i].OtherMemberNick);
                
//                if(messageDisplayStatus == false)
//                {
//                    log('breaking');   
//                    i = i -1;
//                    continue;                    
//                }
                
                var imgUrl = getFriendStatusIcon( messages[i].OtherMemberWebID );
                var friend = getFriendObject( messages[i].OtherMemberWebID );

                setTimeout("setFriendWindowStatusIcon('" + messages[i].OtherMemberWebID + "','" + friend.OnlineStatusString + "')",500);
                setTimeout("setFriendWindowMemberNick('" + messages[i].OtherMemberWebID + "','" + friend.NickName + "')",500);
               // setTimeout("flashTitle()",500);
            }
            
            //log('message string: '+response.value);
        }
        
        
        // set the interval for the new message getter
        setTimeout("getNewMessages()",2000);
    }
    
    /////////////////////////////////////////////////////
    // Set Status & CALLBACK
    function setStatus(onlineStatus,customMessage)
    {  
        if(ajaxMember!=null && ajaxMember.WebMemberID != ''){
            // log into the chat server
            tempStatus = onlineStatus;           
            ChatClient.SetOnlineStatus(ajaxMember.WebMemberID, onlineStatus,'', setStatus_CallBack);
        }
    }
    
    function setStatus_CallBack(response) 
    {
        var res = response.value;
        if( res == true)
        {
            setMyStatus(tempStatus);
            tempStatus = '';
        }                
    }
        
    /////////////////////////////////////////////////////
    // LOGIN & CALLBACK
    function login(){  
    
        myMemberID = Math.round(10000*Math.random()); // create a random ID (ChatClient only this will come from the server initially)
        EmailAddress = document.getElementById('txtNickName').value;
        Password = document.getElementById('txtPassword').value;
        
        // log into the chat server
        ChatClient.LoginToChatServer(EmailAddress, Password, login_CallBack);        
    }
    
    function login_CallBack(response) 
    {        
        if( response != null )
        {
            ajaxMember = response.value;
        }
        
        //if the user logged in successfully
        if(ajaxMember!=null && ajaxMember.WebMemberID != ''){                
                setMyMemberID(ajaxMember.WebMemberID);                   
                setMyNick(ajaxMember.NickName);
                setMyStatus(ajaxMember.OnlineStatusString);
                getFriends();                
                getFriendsStatus();                
                //getNewMessages();
                //pollServer();
               //log('Sucessfully Logged in');            

        }else{
            //try loggin in again
            //login();
        }  
    }
    
    /////////////////////////////////////////////////////
    // LOGOUT & CALLBACK
   function logout(){
        // log into the chat server
        if(ajaxMember != null )
        {
            ChatClient.LogoutOfChatServer(ajaxMember.WebMemberID, logout_CallBack);     
        }
    }
    
   function logout_CallBack(response) {
        //if the user logged in successfully
        if(response!=null && response.value == true)
        {            
            clearFriends();  
            setStatus('Offline','');
        }
    }

    /////////////////////////////////////////////////////////
    //Log events for debuggin only (you must uncomment text area txtConsole to display log)
    function log(event){    
        var txtConsole = document.getElementById('txtConsole');
        txtConsole.value = event + "\r" + txtConsole.value;
    }
    
    
    
    function flashTitle()
    {
        if (document.title == actualTitle)
        {
            document.title = blankTitle;
        }
        else
        {
            document.title = actualTitle;
        }
        
        flashTitleIntervalId = setTimeout("flashTitle()",500);
    }
    
    function cancelTitleFlash()
    {
        clearTimeout(flashTitleIntervalId);
        document.title = actualTitle;
    }
    
    function clearCustomMessageBox()
    {
        var txtCustomMessage = document.getElementById('txtCustomMessage');
        txtCustomMessage.style.background = 'white';
        if(txtCustomMessage.value == 'Set status here')
            txtCustomMessage.value = '';  
    }
    
    function customMessageBoxChange()
    {
        var txtCustomMessage = document.getElementById('txtCustomMessage');
        if(txtCustomMessage != customText )
        {
            
        }
        txtCustomMessage.style.background = '#C3D9FF';              
    }
    