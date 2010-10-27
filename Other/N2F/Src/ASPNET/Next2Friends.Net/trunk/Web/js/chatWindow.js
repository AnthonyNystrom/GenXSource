        var focused = false;
        var lastMessageDt = null;
        var lastMessageShowing = false;
        var lastMessageTimer = -1;
        var lastMessageInterval = 120;
        
        var dayNames = new Array(7)
        dayNames[0] = "Sunday";
        dayNames[1] = "Monday";
        dayNames[2] = "Tuesday";
        dayNames[3] = "Wednesday";
        dayNames[4] = "Thursday";
        dayNames[5] = "Friday";
        dayNames[6] = "Saturday";
        
        
        function isFocused()
        {
            return focused;
        }
        
        function getLastMessageNotification()
        {
            var hrs = lastMessageDt.getHours();
            var mins = lastMessageDt.getMinutes() + '';
            var period = 'AM';
            
            if( hrs >= 12 )
            {
                hrs = hrs % 12;
                
                if( hrs == 0 )
                    hrs = 12;
                
                period = 'PM';
            }
            
            hrs = hrs + '';
            if( hrs.length == 1 )
                hrs = '0' + hrs;                
                
            if( mins.length == 1 )
                mins = '0' + mins;
                
            
            return 'Sent at ' + hrs + ':' + mins + ' ' + period + ' on '  + dayNames[lastMessageDt.getDay()];
        }
        
        function showLastMessageDate()
        {
            var currentDt = new Date();
            var diffSec =  (currentDt - lastMessageDt) / 100;            
            
            if( diffSec >= lastMessageInterval && lastMessageShowing == false)
            {
                showLastMessageDiv();
                divChatDisplay.scrollTop = divChatDisplay.scrollHeight;         
                setTimeout("document.getElementById('divChatDisplay').scrollTop =  document.getElementById('divChatDisplay').scrollHeight",0);
            }
            else
            {
                clearTimeout(lastMessageTimer);
                lastMessageTimer = setTimeout('showLastMessageDate()',lastMessageInterval * 1000);
            }
        }
        
        function showLastMessageDiv()
        {   
            var divLastMessage = document.getElementById('divLastMessage');
            var divChatDisplay = document.getElementById('divChatDisplay');
            
            divLastMessage.innerHTML = getLastMessageNotification();
            
            divChatDisplay.style.height = extractVal(divChatDisplay.style.height) - extractVal(divLastMessage.style.height) + 'px';
            divLastMessage.style.display = 'block';
            divLastMessage.style.visibility = 'visible'; 
            lastMessageShowing = true;
        }
        
        function hideLastMessageDiv()
        {
            if( lastMessageShowing == false )
                return;
                
            var divLastMessage = document.getElementById('divLastMessage');
            var divChatDisplay = document.getElementById('divChatDisplay');
            divChatDisplay.style.height = extractVal(divChatDisplay.style.height) + extractVal(divLastMessage.style.height) + 'px';
            divLastMessage.style.display = 'none';
            divLastMessage.style.visibility = 'hidden';
            lastMessageShowing = false;
        }
        
        function extractVal(styleHeight)
        { 
            if ( styleHeight == null )
            {
                return 0;
            }
                
            try
            {
                var px = styleHeight.indexOf("px",0);
                var h = 0;
                if ( px >= 0 )
                {
                    h = styleHeight.substr(0,px );
                }
                             
                return h * 1;
            }
            catch(ex)
            {
                return 0;
            }
        }
        
        
        function focusInput()
        {
            try
            {            
                var txtInput = document.getElementById('txtInput');                
                txtInput.focus();
                return true;
            }
            catch(ex){return false;}
        }       
        
        /////////////////////////////////////////////////////////////////////
        ////// VARIABLES
        ////////////////////////////////////////////////////////////////////
        
        var incomingMessages = null;
        var otherMemberWebID = null;      
        var otherMemberNick = null;
        var divChatDisplayHeight = '';
        var divChatDisplayParentHeight = '';
        var minimized = false;
        var chatWindowHeight = 0;
        var chatWindowTop = 0;
        
        //var chatConsole = '<div id="divChatConsoleHTML"><table cellspacing="0" style="border-top:solid 1px #cccccc;border-right:solid 1px #cccccc;border-left:solid 1px #cccccc;background-color: white; height: 100%; font-family: Verdana;padding: 0px;" width="100%"><tr><td align="center" style="background-color: #509DEB;height:10px;cursor: pointer;"><table width="100%" height="100%" cellspacing="0" ><tr><td align="left"><img src="../images/online.gif" /><div style="width: 100%; text-align: left; vertical-align:top; color: White; font-size: 80%; font-weight: bold;display:inline"><div id="titleNick" style="display:inline">Hamid</div></div></td><td align="right"><img src="minimise.gif" onclick="minimiseChatConsole();" /><img src="close.gif" onclick="void(0);" /></td></tr></table></td></tr><tr><td align="right" valign="bottom" width="100%"><!-- This is where the chat will be displayed--><div id="divChatDisplay" style="text-align: left; width: 100%; height: 100%;overflow-x: hidden; overflow:auto; font-size: 80%;"></div></td></tr><tr><td style="height: 70px;" align="center"><!-- The text input box that calls checkEnter() on each keypress. If enter was pressed then the chatSend() ajax method is called. txtInput is then reset to blank.--><textarea id="txtInput" cols="4" style="overflow:auto; border:solid 1px #cccccc;height: 50px; width: 95%; font-family: Verdana;font-size: 90%;" onkeypress="return checkEnter(event)" onfocus="this.style.borderColor="#509DEB";this.style.borderWidth="2px";" onblur="this.style.borderColor="#cccccc";this.style.borderWidth="1px";"></textarea></td></tr></table></div>';
        var chatConsole = '<div id="divChatConsoleHTML"><table cellspacing="0" style="border-top:solid 1px #cccccc;border-right:solid 1px #cccccc;border-left:solid 1px #cccccc;background-color: white; height: 100%; font-family: Verdana;padding: 0px;" width="100%"><tr><td align="center" style="background-color: #509DEB;height:10px;cursor: pointer;"><table width="100%" height="100%" cellspacing="0" ><tr><td align="left"><img src="../images/online.gif" /><div style="width: 100%; text-align: left; vertical-align:top; color: White; font-size: 80%; font-weight: bold;display:inline"><div id="titleNick" style="display:inline">Hamid</div></div></td><td align="right"><img src="minimise.gif" onclick="minimiseChatConsole();" /><img src="close.gif" onclick="void(0);" /></td></tr></table></td></tr><tr><td align="right" valign="bottom" width="100%"><!-- This is where the chat will be displayed--><div id="divChatDisplay" style="text-align: left; width: 100%; height: 100%;overflow-x: hidden; overflow:auto; font-size: 80%;"></div></td></tr><tr><td style="height: 70px;" align="center"><!-- The text input box that calls checkEnter() on each keypress. If enter was pressed then thechatSend() ajax method is called. txtInput is then reset to blank.--><textarea id="txtInput" cols="4" style="overflow:auto; border:solid 1px #cccccc;height: 50px; width: 95%; font-family: Verdana;font-size: 90%;" onkeypress="return checkEnter(event,11)" onfocus="this.style.borderColor="#509DEB";this.style.borderWidth="2px";" onblur="this.style.borderColor="#cccccc";this.style.borderWidth="1px";"></textarea></td></tr></table></div>';
        
        //////////////////////////////////////////////////////////////////////
        ///// PAGE SPECIFIC GETTER AND SETTER FUNCTIONS
        /////////////////////////////////////////////////////////////////////        
        
        ///
        /// Get HTML for the Chat Window
        /// Not used currently
        ///
        //function getChatConsoleHTML()
        //{
        //    return chatConsole ;                   
        //}        
        
        ///
        /// Get own Nick
        ///
        function getMyNick()
        { 
            try
            {  
                return parent.getMyMemberNick();          
            }
            catch(ex){}
        }        

        ///
        /// Get own WebMemberID
        ///
        function getMyWebID()
        {            
            try
            {   
                return parent.getMyMemberID();
            }
            catch(ex){}
        }        
                
        ///
        /// Get the Nick of the other user
        //
        function getOtherMemberNick()
        {
            return otherMemberNick;            
        }
        
        ///
        /// Set the Nick of the other user
        //
        function setOtherMemberNick(varOtherMemberNick)
        {
            try
            {
                otherMemberNick = varOtherMemberNick;            
                var titleNick = document.getElementById('titleNick');
                titleNick.innerHTML = otherMemberNick;            
            }
            catch(ex){}
        }
        
        
        ///
        /// Get the WebMemberID of the other user
        //
        function getOtherWebID()
        {
            return otherMemberWebID;            
        }
        
        ///
        /// Set the WebMemberID of the other user
        //
        function setOtherWebID(varOtherMemberWebID)
        {
            otherMemberWebID = varOtherMemberWebID;
        }
        
        ///
        /// Set the icon of the current window based on the status of the 
        /// person for whom the chat window is opened
        /// This function is called by its parent page
        ///
        function setStatusIcon(onlineStatus)
        {
            try
            {
                //change the icon according to the status
                if(onlineStatus == 'Online')
                {
                    changeStatusIcon('../images/online.gif');
                }
                else if(onlineStatus == 'Offline' )
                {
                    changeStatusIcon('../images/offline.gif');        
                }
                else if(onlineStatus == 'Busy' )
                {
                    changeStatusIcon('../images/busy.png');            
                }
                else if(onlineStatus == 'Away' )
                {
                
                }            
            }
            catch(ex){}                                 
        }        
                
        ///
        /// Helper function for chaning the Status Icon
        ///
        function changeStatusIcon(imgUrl)
        {
            try
            {
                var statusIcon = document.getElementById('statusIcon');
                statusIcon.src = imgUrl;
            }
            catch(ex){}
        }
        
        
        
        ///////////////////////////////////////////////////////////////////////////////////
        ////// Message Sending Related Functions
        //////////////////////////////////////////////////////////////////////////////////
    
    
        ///
        /// Send Message using the Ajax Call to Server
        ///
        function sendMessage(message)
        {   
            try
            {
                ChatWindow.SendMessage(getMyWebID(),getOtherWebID(), escape(getMyNick()), escape( message ) ,sendMessage_CallBack);
            }
            catch(ex)
            { 
                
            }
        }
        
        function sendMessage_CallBack(response) 
        {        
             //alert(response);
        }
        
        ///
        /// Get the message that has been entered by the user
        ///
        function getMessageToSend()
        {
            try
            {
                return document.getElementById('txtInput').value;
            }
            catch(ex){}   
        }
        
        ///
        /// Event Handler for keypress event of the input text area
        ///
        function checkEnter(e)
        {   
            try
            {
                // catches the return key event to send the message
                var characterCode

                // firefox and IE key event getter
                if(e && e.which){
                    e = e
                    characterCode = e.which 
                }
                else{
                    e = event
                    characterCode = e.keyCode 
                }

                if(characterCode == 13){ //send the message if enter was hit         

                    // get the message to send from the txtInput box
                    var message = getMessageToSend();                
                    //only send the mesage if it is not blank
                    if(message!=''){                
                        //display the message in the display box
                        
                        addMessageToDisplay('me', message);
                        
                        hideLastMessageDiv();
                        // send the message
                        sendMessage(message);           
                        //empty the chat window
                        document.getElementById('txtInput').value = '';
                        //return false so no 'enter' key is displayed in the input box
                    }
                    return false;
                }
            }
            catch(ex){}
        }
        
        var borderTopOrigColor;
        var borderBottomOrigColor;
        var borderCenterOrigColor;
        
        function toggleTitleBarColor(newMsg)
        {
            if( newMsg == true )
            {            
                var borderTop = document.getElementById('borderTop');
                var borderBottom  = document.getElementById('borderBottom');
                var borderCenter  = document.getElementById('borderCenter');
                
                borderTop.style.backgroundColor = 'Orange';
                borderBottom.style.backgroundColor = 'Orange';
                borderCenter.style.backgroundColor = 'Orange';
            }
        }
        
        ///
        /// Add the incoming or outgoing message to the display window
        ///
        function addMessageToDisplay(who,message)
        {
            try
            {
                
                var divChatDisplay = document.getElementById('divChatDisplay');
                divChatDisplay.innerHTML = divChatDisplay.innerHTML + '<strong>'+who+':</strong>' + message + '<br/><div style="height:4px"></div>';
                
                //if( who != 'me' )
                //{
                //    toggleTitleBarColor(true);
                //}
                
                 //increase the height up to a 170px
                 //if(divChatDisplay.clientHeight < 170)
                 //{
                 //   divChatDisplay.style.height = (divChatDisplay.clientHeight + 20) +'px';
                 //   divChatDisplayHeight = divChatDisplay.style.height;
                 //}
                 
                  //increase the parent height up to a 300px
                // if(document.documentElement.clientHeight < 300)
                // {
                //    var chatParentIframe = parent.document.getElementById('chatParentIframe' + getOtherWebID());
                    
                //    chatParentIframe.style.height = (document.documentElement.clientHeight + 20)+'px';                  
                    
                //    divChatDisplayParentHeight = chatParentIframe.style.height;
                // }   
                 
                 //-var chatParentIframe = parent.document.getElementById('chatParentIframe' + getOtherWebID());                    
                 //-var origH = extractVal(chatParentIframe.style.height);
                    
                  //increase the parent height up to a 300px
                 //if(origH < 290)
                 //{
                    //var chatParentDiv = parent.document.getElementById('chatParentDiv');
                    
                    //chatParentDiv.style.height = (document.documentElement.clientHeight + 20) + 'px';                    
                    //chatParentIframe.style.height = (document.documentElement.clientHeight + 20)+'px';
                    //-chatParentIframe.style.height = (origH + 20)  + 'px';
                    
                    //-var origT = extractVal(chatParentIframe.style.top);                    
                    //-chatParentIframe.style.top = (origT - 20) + 'px';
                    
                    //divChatDisplayParentHeight = chatParentIframe.style.height;                    
                    //if(parent.adjustChatDivTop)
                    //{
                    //    parent.adjustChatDivTop();
                    //}
                 //}
                
          
                // scroll the div to the bottom to display the newest message                                
                divChatDisplay.scrollTop = divChatDisplay.scrollHeight;         
                setTimeout("document.getElementById('divChatDisplay').scrollTop =  document.getElementById('divChatDisplay').scrollHeight",0);
                
                hideLastMessageDiv();
                
                lastMessageDt = new Date();
                showLastMessageDate();
                
                return true;
            }
            catch(ex){return false;}
        }
        
        
        ////////////////////////////////////////////////////////////////////////////////////
        ////////// CHAT WINDOW MANIPULATION
        ///////////////////////////////////////////////////////////////////////////////////

        
        ///
        /// Close the chat window
        ///
        function closeChatWindow()
        { 
            try
            {
                //this.parentNode.removeChild(this);
                parent.closeFriendChatWindow(otherMemberWebID);
            }
            catch(ex){}
        }
        
        ///
        /// Minimize or Restore the chat window based on its state
        ///
        function minRestoreChatWindow()
        {
            try
            {
                if( minimized )
                    restoreChatWindow();
                else
                    minimizeChatWindow();
            }
            catch(ex){}
        }
        
        ///
        /// Minimize chat window
        ///
        function minimizeChatWindow()
        {
            try
            {
                //var divChatMain = document.getElementById('divChatMain');
                //divChatMain.style.height = 25 +'px';
                //divChatMain.style.top = -125 + 'px';
                
                //var chatParentDiv = parent.document.getElementById('chatParentDiv');
                var chatParentIframe = parent.document.getElementById('chatParentIframe' + getOtherWebID());

                //chatParentDiv.style.height = 50 + 'px';
                //chatParentIframe.style.height = 25 + 'px';
                chatWindowHeight = extractVal( chatParentIframe.style.height);
                chatWindowTop = extractVal( chatParentIframe.style.top);
                chatParentIframe.style.top = -25  + 'px';
                
                minimized = true;
            }
            catch(ex){}
        }
        
        ///
        /// Restore chat window
        ///
        function restoreChatWindow()
        {
            try
            {
                //var divChatDisplay = document.getElementById('divChatDisplay');                
                //increase the height up to a 180px
                //divChatDisplay.style.height = 200 +'px';                
                
                //var chatParentDiv = parent.document.getElementById('chatParentDiv');
                var chatParentIframe = parent.document.getElementById('chatParentIframe' + getOtherWebID());

                //chatParentDiv.style.height = 150 + 'px';
                //chatParentIframe.style.height = 150 + 'px';  
                chatParentIframe.style.top = chatWindowTop + 'px';
                minimized = false;            
            }
            catch(ex){}            
        }
        
        
       function setFocusVar(foc)
       {
            focused = foc;
       }