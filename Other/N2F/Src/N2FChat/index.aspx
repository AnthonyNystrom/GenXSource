<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>N2FChat</title>
</head>
<body style="margin: 0px">
    <form id="form1" runat="server">
    <div>
        <!-- this iframe is for displaying the main page.-->
        <iframe src="chatcontrols.aspx" id="iFrameChatControls" style="width: 500px; height: 500px;
            margin: 0px; padding: 0px; border: 0px"></iframe>
        <!-- this DIV is used only as a code source template to fill the IFrame chat instances. The actual instance below is never displayed-->
        <!-- this could reside in a string constant to optimise the amount of time it takes to instanciate a new object. this method is used for easy edit more in debug as you can see the html and JS  -->
        <div id="divChatConsoleHTML" style="display: none; visibility: hidden;">
            <!-- The follwoing Script can be stored in an external js file and refernced from within this iframe. 
            To reload the script to the browser initially, just put a script link in this page and it will be cached -->

            <script>
            
                function checkEnter(e){ // catches the return key event to send the message
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
                        var message = document.getElementById('txtInput').value;   
                        
                        //only send the mesage if it is not blank
                        if(message!=''){
                            //display the message in the display box
                            addMessageToDisplay('me', message);
                            // send the message
                            sendChat(memberID, message);
                            //return false so no 'enter' key is displayed in the input box
                        }
                        return false;
                    }
                }
                
                // Add the incoming or outgoing message to the display window
                function addMessageToDisplay(who, message){
                
                        var mainWnd = parent.document.getElementById('iFrameChatControls');                  
                        
                        //empty the chat window
                        document.getElementById('txtInput').value = null;
                        
                        var divChatDisplay = document.getElementById('divChatDisplay');
                        divChatDisplay.innerHTML = divChatDisplay.innerHTML + '<strong>'+who+':</strong>' + message + '<br/><div style="height:4px"></div>';
                        
                        
                         // increase the height up to a 180px
                         if(divChatDisplay.clientHeight < 170){
                            divChatDisplay.style.height = (divChatDisplay.clientHeight + 20) +'px';
                         }
                         
                         // increase the height up to a 300px
                         if(document.documentElement.clientHeight < 300){
                           parent.document.getElementById('chat'+memberID).style.height = (document.documentElement.clientHeight + 20)+'px';
                         }
                  
                        // scroll the div to the bottom to display the newest message
                        divChatDisplay.scrollTop = divChatDisplay.scrollHeight; 
                }
                
                // sends the chat message to the server by calling the send chat command
                function sendChat(memberID, message){
                    var mainWnd = parent.document.getElementById('iFrameChatControls');   
                    mainWnd.contentWindow.sendChat(memberID, message);
                }
                
                var min = false;
                // for minimising the chat window.. needs improvement to make it slick. This is fired
                // when the user clicks the minimise image icon on the chat window. More needs to be
                // to hide certain elements on the chatwindow so they dont bunch up and cause scrollbars (and looks cleaner)
                // it also needs memory so it can be restored the same height before minimise was called. 
                function minimiseChatConsole(){
                    var iFrame = parent.document.getElementById('chat'+memberID)
                    
                    if(min){
                        //iFrame.style.height = 300+'px'; // the 300 needs to be an global variable
                       // iFrame.overflow = 'scroll';
                       // min = false;
                    }else{
                       // iFrame.style.height = 50+'px';
                       // iFrame.overflow = 'hidden';
                       // min = true;
                    }
                }
                
            </script>

            <table cellspacing="0" style="border-top:solid 1px #cccccc;border-right:solid 1px #cccccc;border-left:solid 1px #cccccc;background-color: white; height: 100%; font-family: Verdana;
                padding: 0px;" width="100%">
                <tr>
                    <td align="center" style="background-color: #73a6ff;height:20px" >
                        <table width="100%" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <div style="width: 100%; text-align: left; color: White; font-size: 80%; font-weight: bold;">
                                        &nbsp;&nbsp;

                                        <script>
                                        /* check if the nickname is defined. this will be when created 
                                        via js but not on the initial page load. this code simply stops an error */
                                        if(typeof( window[ 'nickName' ] ) != "undefined" ){document.writeln(nickName);}</script>

                                    </div>
                                </td>
                                <td>
                                </td>
                                <td align="right">
                                    <img src="minimise.gif" style="cursor: pointer;" onclick="minimiseChatConsole();" />
                                    <img src="close.gif" onclick="void(0);" />
                                &nbsp;
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="bottom" >
                        <!-- This is where the chat will be displayed-->
                        <div id="divChatDisplay" style="text-align: left; width: 210px; height: 100%;
                            overflow-x: hidden; overflow: scroll; overflow: -moz-scrollbars-vertical; font-size: 80%;">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 70px;" align="center">
                        <!-- The text input box that calls checkEnter() on each keypress. If enter was pressed then the
                    chatsend() ajax method is called. txtInput is then reset to blank.
                    -->
                        <textarea id="txtInput" cols="4" style="border:solid 1px #cccccc;height: 50px; width: 90%; font-family: Verdana;
                            font-size: 90%;" onkeypress="return checkEnter(event)" 
                            onfocus="this.style.borderColor='#73a6ff';this.style.borderWidth='2px';" 
                            onblur="this.style.borderColor='#cccccc';this.style.borderWidth='1px';"></textarea>
                    </td>
                </tr>
            </table>
            <!-- give the txtInput box focus on initial load -->

            <script>document.getElementById('txtInput').focus();</script>

        </div>
    </div>

    <script language="javascript">
    var chatConsoles = new Array(); //array of chatwindows
    var right = 20;     // default most right position of the first chat window
    var width = 225;    // width of the iframe chatwindow
    var height = 170;   // height of the iframe chatwindow
    var chatWindowHTML = document.getElementById('divChatConsoleHTML').innerHTML
     
    /* 
        creates a new instance of the chat window and stores it in the chatConsoles array, 
        or if one has already been created it simply appends the incoming chat message
    */
    function createAppendChatConsole(memberID, message, nickName){
    
        var iFrame = null;
        
        // check if there is already a chat console open for this member
        for(var i=0;i<chatConsoles.length;i++) {
            if(chatConsoles[i][0]==memberID){
                iFrame = chatConsoles[i][2];
                nickName = chatConsoles[i][1];
            }
        }
    
        // if the member does not currently have a chat window open for this user then create one and sotr it in the array
        if(iFrame==null){
            //create the new iframe
            iFrame = document.createElement("iframe");
            iFrame.id = 'chat'+memberID;
            iFrame.style.width = width+"px";
            iFrame.style.height = height+"px";
            iFrame.style.bottom = 0+'px';
            iFrame.style.position = 'absolute';
            iFrame.style.margin = 0+'px';
            iFrame.style.padding = 0+'px';
            iFrame.style.border = 0+'px';
            
             // set the postition of the window.
             // this will eventually need a seperate funtion to organise and position the chatwindows for when new window is closed
             // When a window is opened or closed all the current open windows will need to be positioned accordingly
          
            iFrame.style.right = (chatConsoles.length*width)+(chatConsoles.length*30)+20+'px';
            document.body.appendChild(iFrame); //append the iframe to the DOM
                        
            // write the contents of the window including passing in the nickName and memberID as a param
            // This window needs to be prototypes to extend it properties for memberID and nickName. at the moment this method is a bit hacky
            iFrame.contentWindow.document.write('<html><body style="padding:0px;margin:0px;">');           
            iFrame.contentWindow.document.write('<script>var memberID='+memberID+';var nickName="'+nickName+'";<' + "/" + 'script>'+chatWindowHTML);
            iFrame.contentWindow.document.write('</body></html>');
            
            // get the next empty element in the array
            var index = chatConsoles.length;
            
            // store the details and iFrame in the array for later access
            chatConsoles[index] = new Array();
            chatConsoles[index][0] = memberID;
            chatConsoles[index][1] = nickName;
            chatConsoles[index][2] = iFrame;
         }      
         
         //add the incoming message
          if(message!=''){
                iFrame.contentWindow.addMessageToDisplay(nickName, message);
          }
    }
    
    </script>

    </form>
</body>
</html>
