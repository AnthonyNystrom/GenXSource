        /////////////////////////////////////////////////////////////////////
        ////// VARIABLES
        ////////////////////////////////////////////////////////////////////
        
        var incomingMessages = null;
        var otherMemberWebID = null;      
        var otherMemberNick = null;
        var divChatDisplayHeight = '';
        var divChatDisplayParentHeight = '';
        var minimized = false;
        
        //var chatConsole = '<div id="divChatConsoleHTML"><table cellspacing="0" style="border-top:solid 1px #cccccc;border-right:solid 1px #cccccc;border-left:solid 1px #cccccc;background-color: white; height: 100%; font-family: Verdana;padding: 0px;" width="100%"><tr><td align="center" style="background-color: #73a6ff;height:10px;cursor: pointer;"><table width="100%" height="100%" cellspacing="0" ><tr><td align="left"><img src="img/available.png" /><div style="width: 100%; text-align: left; vertical-align:top; color: White; font-size: 80%; font-weight: bold;display:inline"><div id="titleNick" style="display:inline">Hamid</div></div></td><td align="right"><img src="minimise.gif" onclick="minimiseChatConsole();" /><img src="close.gif" onclick="void(0);" /></td></tr></table></td></tr><tr><td align="right" valign="bottom" width="100%"><!-- This is where the chat will be displayed--><div id="divChatDisplay" style="text-align: left; width: 100%; height: 100%;overflow-x: hidden; overflow:auto; font-size: 80%;"></div></td></tr><tr><td style="height: 70px;" align="center"><!-- The text input box that calls checkEnter() on each keypress. If enter was pressed then the chatSend() ajax method is called. txtInput is then reset to blank.--><textarea id="txtInput" cols="4" style="overflow:auto; border:solid 1px #cccccc;height: 50px; width: 95%; font-family: Verdana;font-size: 90%;" onkeypress="return checkEnter(event)" onfocus="this.style.borderColor="#73a6ff";this.style.borderWidth="2px";" onblur="this.style.borderColor="#cccccc";this.style.borderWidth="1px";"></textarea></td></tr></table></div>';
        var chatConsole = '<div id="divChatConsoleHTML"><table cellspacing="0" style="border-top:solid 1px #cccccc;border-right:solid 1px #cccccc;border-left:solid 1px #cccccc;background-color: white; height: 100%; font-family: Verdana;padding: 0px;" width="100%"><tr><td align="center" style="background-color: #73a6ff;height:10px;cursor: pointer;"><table width="100%" height="100%" cellspacing="0" ><tr><td align="left"><img src="img/available.png" /><div style="width: 100%; text-align: left; vertical-align:top; color: White; font-size: 80%; font-weight: bold;display:inline"><div id="titleNick" style="display:inline">Hamid</div></div></td><td align="right"><img src="minimise.gif" onclick="minimiseChatConsole();" /><img src="close.gif" onclick="void(0);" /></td></tr></table></td></tr><tr><td align="right" valign="bottom" width="100%"><!-- This is where the chat will be displayed--><div id="divChatDisplay" style="text-align: left; width: 100%; height: 100%;overflow-x: hidden; overflow:auto; font-size: 80%;"></div></td></tr><tr><td style="height: 70px;" align="center"><!-- The text input box that calls checkEnter() on each keypress. If enter was pressed then thechatSend() ajax method is called. txtInput is then reset to blank.--><textarea id="txtInput" cols="4" style="overflow:auto; border:solid 1px #cccccc;height: 50px; width: 95%; font-family: Verdana;font-size: 90%;" onkeypress="return checkEnter(event,11)" onfocus="this.style.borderColor="#73a6ff";this.style.borderWidth="2px";" onblur="this.style.borderColor="#cccccc";this.style.borderWidth="1px";"></textarea></td></tr></table></div>';
        
        //////////////////////////////////////////////////////////////////////
        ///// PAGE SPECIFIC GETTER AND SETTER FUNCTIONS
        /////////////////////////////////////////////////////////////////////        
        
        ///
        /// Get HTML for the Chat Window
        /// Not used currently
        ///
        function getChatConsoleHTML()
        {
            return chatConsole ;                   
        }        
        
        ///
        /// Get own Nick
        ///
        function getMyNick()
        {   
            return parent.getMyMemberNick();          
        }        

        ///
        /// Get own WebMemberID
        ///
        function getMyWebID()
        {               
            return parent.getMyMemberID();
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
            otherMemberNick = varOtherMemberNick;
            var titleNick = document.getElementById('titleNick');
            titleNick.innerHTML = otherMemberNick;            
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
            //change the icon according to the status
            if(onlineStatus == 'Online')
            {
                changeStatusIcon('img/available.png');
            }
            else if(onlineStatus == 'Offline' )
            {
                changeStatusIcon('img/offline.png');        
            }
            else if(onlineStatus == 'Busy' )
            {
                changeStatusIcon('img/busy.png');            
            }
            else if(onlineStatus == 'Away' )
            {
            
            }                                 
        }        
                
        ///
        /// Helper function for chaning the Status Icon
        ///
        function changeStatusIcon(imgUrl)
        {
            var statusIcon = document.getElementById('statusIcon');
            statusIcon.src = imgUrl;
        }
        
        
        
        ///////////////////////////////////////////////////////////////////////////////////
        ////// Message Sending Related Functions
        //////////////////////////////////////////////////////////////////////////////////
    
    
        ///
        /// Send Message using the Ajax Call to Server
        ///
        function sendMessage(message)
        {   
            ChatWindow.SendMessage(getMyWebID(),getOtherWebID(), getMyNick(), message ,sendMessage_CallBack)
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
            return document.getElementById('txtInput').value;
        }
        
        ///
        /// Event Handler for keypress event of the input text area
        ///
        function checkEnter(e)
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
                    // send the message
                    sendMessage(message);           
                    //empty the chat window
                    document.getElementById('txtInput').value = '';
                    //return false so no 'enter' key is displayed in the input box
                }
                return false;
            }
        }
        
        ///
        /// Add the incoming or outgoing message to the display window
        ///
        function addMessageToDisplay(who,message){                        
                
                var divChatDisplay = document.getElementById('divChatDisplay');
                divChatDisplay.innerHTML = divChatDisplay.innerHTML + '<strong>'+who+':</strong>' + message + '<br/><div style="height:4px"></div>';               
                
                
                 //increase the height up to a 180px
                 if(divChatDisplay.clientHeight < 170)
                 {
                    divChatDisplay.style.height = (divChatDisplay.clientHeight + 20) +'px';
                    divChatDisplayHeight = divChatDisplay.style.height;
                 }
                 
                  //increase the parent height up to a 300px
                 if(document.documentElement.clientHeight < 300)
                 {
                    var chatParentDiv = parent.document.getElementById('chatParentDiv');
                    var chatParentIframe = parent.document.getElementById('chatParentIframe' + getOtherWebID());
                    
                    chatParentDiv.style.height = (document.documentElement.clientHeight + 20)+'px';                    
                    chatParentIframe.style.height = (document.documentElement.clientHeight + 20)+'px';                  
                    
                    divChatDisplayParentHeight = chatParentIframe.style.height;
                 }                
                
          
                // scroll the div to the bottom to display the newest message
                divChatDisplay.scrollTop = divChatDisplay.scrollHeight; 
        }
        
        
        ////////////////////////////////////////////////////////////////////////////////////
        ////////// CHAT WINDOW MANIPULATION
        ///////////////////////////////////////////////////////////////////////////////////
        
        ///
        /// Close the chat window
        ///
        function closeChatWindow()
        { 
            //this.parentNode.removeChild(this);
            parent.closeFriendChatWindow(otherMemberWebID);
        }
        
        ///
        /// Minimize or Restore the chat window based on its state
        ///
        function minRestoreChatWindow()
        {
            if( minimized )
                restoreChatWindow();
            else
                minimizeChatWindow();   
        }
        
        ///
        /// Minimize chat window
        ///
        function minimizeChatWindow()
        {
            var divChatDisplay = document.getElementById('divChatDisplay');                
            //increase the height up to a 180px
            divChatDisplay.style.height = 50 +'px';                
            
            //var chatParentDiv = parent.document.getElementById('chatParentDiv');
            var chatParentIframe = parent.document.getElementById('chatParentIframe' + getOtherWebID());

            //chatParentDiv.style.height = 50 + 'px';
            chatParentIframe.style.height = 50 + 'px';  
            
            minimized = true;
        }
        
        ///
        /// Restore chat window
        ///
        function restoreChatWindow()
        {
            var divChatDisplay = document.getElementById('divChatDisplay');                
            //increase the height up to a 180px
            divChatDisplay.style.height = 200 +'px';                
            
            var chatParentDiv = parent.document.getElementById('chatParentDiv');
            var chatParentIframe = parent.document.getElementById('chatParentIframe' + getOtherWebID());

            chatParentDiv.style.height = 200 + 'px';
            chatParentIframe.style.height = 200 + 'px';  
            
            minimized = false;
            
        }
       
    