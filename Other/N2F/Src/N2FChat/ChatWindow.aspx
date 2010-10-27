<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChatWindow.aspx.cs" Inherits="ChatWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link rel="Stylesheet" href="css/StyleSheet.css" type="text/css"/>
    <title>Chat Window</title>
    <script language="javascript" src="javascript/chatWindow.js"></script>
    <script language="javascript" >
        function setIds()
        {
            setOtherWebID("<%=otherMemberWebID %>");        
            setOtherMemberNick("<%=otherMemberNick %>");            
        }
    </script>
</head>
<body onload="setIds();">
    <form id="form1" runat="server">
     <div id="chatParentDiv">
     <b class="rtopdark"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4"></b></b>
        <!-- this DIV is used only as a code source template to fill the IFrame chat instances. The actual instance below is never displayed-->
        <!-- this could reside in a string constant to optimise the amount of time it takes to instanciate a new object. this method is used for easy edit more in debug as you can see the html and JS  -->
        <div id="divChatConsoleHTML">        
            <!-- The follwoing Script can be stored in an external js file and refernced from within this iframe. 
            To reload the script to the browser initially, just put a script link in this page and it will be cached -->

            <table cellspacing="0" style="border-right:solid 1px #cccccc;border-left:solid 1px #cccccc;background-color: white; height: 100%; font-family: Verdana;
                padding: 0px;" width="100%">
                <tr>
                    <td align="center" style="background-color: #73a6ff;height:10px;cursor: pointer;">
                        <table width="100%" height="100%" cellspacing="0" >
                            <tr>
                                <td align="left"><img id="statusIcon" src="img/offline.png" />
                                <div style="width: 100%; text-align: left; vertical-align:top; color: White; font-size: 80%; font-weight: bold;display:inline">
                                <div id="titleNick" style="display:inline">Nick</div></div>
                                </td>
                                <td align="right">
                                    <img src="minimise.gif" onclick="minRestoreChatWindow();" />
                                    <img src="close.gif" onclick="closeChatWindow();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="bottom" width="100%">
                        <!-- This is where the chat will be displayed-->
                        <div id="divChatDisplay" style="text-align: left; width: 100%; height: 100%;
                            overflow-x: hidden; overflow:auto; font-size: 80%;">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 70px;" align="center">
                        <!-- The text input box that calls checkEnter() on each keypress. If enter was pressed then the
                        chatSend() ajax method is called. txtInput is then reset to blank.
                    -->
                        <textarea id="txtInput" cols="4" style="overflow:auto; border:solid 1px #cccccc;height: 50px; width: 95%; font-family: Verdana;
                            font-size: 80%;" onkeypress="return checkEnter(event);" 
                            onfocus="this.style.borderColor='#73a6ff';this.style.borderWidth='2px';" 
                            onblur="this.style.borderColor='#cccccc';this.style.borderWidth='1px';"></textarea>
                    </td>
                </tr>
            </table>
        </div>
   <b class="rbottomonlyborder"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1"></b></b>   
   </div>
   </form>
   </body>
   </html>
