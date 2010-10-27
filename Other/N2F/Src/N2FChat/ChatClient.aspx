<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChatClient.aspx.cs" Inherits="ChatClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>N2F Chat</title>
    
    <style type="text/css">

            #chatParentDiv{
            position:absolute;
            width: 80%;
            left: 0px;
            top: 400px;
            border: 2px solid black;
            background-color: lightyellow;
            padding: 0px;
            z-index: 100;
            visibility:hidden;
            }

</style>
<link rel="Stylesheet" href="css/StyleSheet.css" type="text/css"/>
<script language="javascript" type="text/javascript" src="javascript/chatClient.js"></script>
<script language="javascript">
</script>
   
</head>
<body id="mainBody" onload = "getNewMessages();" onfocus="">
    <form id="form1" runat="server">
    <div style="width:200px" id="mainDiv">
        

        Enter NickName
        <input type="text" id="txtNickName" value="hamidmq@yahoo.com" />
        <input type="text" id="txtPassword" value="password" />
        <input type="button" value="logout" id="btnLoginOut"
            onclick="logout()" />
        <input type="button" value="login" id="btnLogin"
            onclick="login()" />
        <asp:Button ID="btnKillApplicationStatus" runat="server" BackColor="Red"
                OnClick="btnKillApplicationStatus_Click" Text="Kill Application Status" Visible="False" />
                
        
        <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4"></b></b>
        <div id="chatClient" style="background-color:#C3D9FF;font-family: Verdana;cursor: pointer;">Chat         
            <div id="divMySelf" style="border: 1px #C3D9FF solid;font-family: Verdana;font-size: 80%;background-color:#C3D9FF;">
                <img id="myStatusIcon" src="img/offline.png" />
                <div id="myTitleNick" style="display:inline;text-align: left; vertical-align:top;">Nick</div>                
            </div>
            <div style="border: 1px #C3D9FF solid;font-family: Verdana;font-size: 70%;background-color:#C3D9FF">
                <img src="img/offline.png" style="visibility:hidden" />
                <div style="display:inline;text-align: left; vertical-align:top;" onclick="toggleStatusMenu();">Set Status Here</div>
            </div>            
            <div id="divFriendSource" style="border: 1px #C3D9FF solid;border-top:0px; font-family: Verdana;font-size: 80%;background-color:white;visibility:hidden;display:none" onmouseover="divMouseOver('divFriendSource')" onmouseout= "divMouseOut('divFriendSource')" onclick="friendMouseClick('divFriendSource')">
                <img id="statusIcon" src="img/offline.png" />
                <div id="titleNick" style="display:inline;text-align: left; vertical-align:top;">Nick</div>
                <div id = "divCustomMessageParent" style="visibility:hidden;display:none">
                    <img id="hiddenStatusIcon" src="img/offline.png" />&nbsp;<div id="divCustomMessage" style="display:inline; text-align: left; vertical-align:top;font-size: 70%">Custom Message</div>
                </div>
            </div>
         </div>   
         <div style="font-family: Verdana;font-size: 60%;background-color:#C3D9FF;"><a href="#" onclick="window.open('AddFriend.aspx');">Add Contact</a></div>
                <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1"></b></b>
   
        <div id='chatParentDiv' style="z-index:1000;display:block;visibility:visible;width:100%">
            <iframe id='chatParentIframe' src="chatWindow.aspx?a=aaaa&b=NICK" scrolling="no" frameborder ="0" marginheight="0" marginwidth="0" height = "150px" width ="230px" style="display:none;visibility:hidden;"> </iframe>
        </div>
        <br />
        <textarea type="text" id="txtConsole" cols="10" style="width:600px;height:500px;visibility:visible;display:block" ></textarea> 
    </div>
    <div id="divStatusMenu" style="position:relative;left:20px;top:-60px;visibility:visible;display:none;z-index:1000;background:#C3D9FF;width:200px;background-color:#C3D9FF;font-family: Verdana;cursor: pointer;font-size: 80%;">
            <div id="divStatusOnline" onmouseover="divMouseOver('divStatusOnline')" onmouseout="divMouseOut('divStatusOnline','#C3D9FF')" onclick="setStatus('Online',null);toggleStatusMenu();" style="border:1px #CCCCCC solid;">Online</div>
            <div id="divStatusAway" onmouseover="divMouseOver('divStatusAway')" onmouseout="divMouseOut('divStatusAway','#C3D9FF')" onclick="setStatus('Away',null);toggleStatusMenu();" style="border:1px #CCCCCC solid;border-top:0px">Away</div>
            <div id="divStatusBusy" onmouseover="divMouseOver('divStatusBusy')" onmouseout="divMouseOut('divStatusBusy','#C3D9FF')" onclick="setStatus('Busy',null);toggleStatusMenu();" style="border:1px #CCCCCC solid;border-top:0px;">Busy</div>
            <div id="divStatusOffline" onmouseover="divMouseOver('divStatusOffline')" onmouseout="divMouseOut('divStatusOffline','#C3D9FF')" onclick="logout();toggleStatusMenu();" style="border:1px #CCCCCC solid;border-top:0px;">Sign out</div>
    </div>
    
    </form>
    </body>
</html>