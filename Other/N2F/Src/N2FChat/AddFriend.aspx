<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddFriend.aspx.cs" Inherits="AddFriend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Chat Invitations</title>
    <link rel="Stylesheet" href="css/StyleSheet.css" type="text/css"/>
    <script language="javascript" type="text/javascript">
    
    function refreshFriends()
    {
        if(opener.refreshFriends)
        {
            opener.refreshFriends();
        }
    }
    
    </script>
</head>
<body style="margin:10px" onunload="refreshFriends();">
    <form id="form1" runat="server">    
    <div style="solid;font-family: Verdana;font-size: 90%;background-color:#C3D9FF">
    <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4"></b></b>
        <div style="margin:10px">
        Chat Invitations
        <div style="solid;font-family: Verdana;font-size: 80%;background-color:#E0ECFF;margin:10px">
            <asp:Label ID="Label1" runat="server" Text="To invite people to Chat, type their email addresses here:"></asp:Label>
            <br />
            <asp:TextBox ID="txtEmailAddresses" runat="server" Rows="10" TextMode="MultiLine" Width="376px"></asp:TextBox></div>        
            <asp:Button ID="btnOk" runat="server" Text="Send Invites" OnClick="btnOk_Click"/>
       </div>
    <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1"></b></b>
     </div>  
        
   </form>
</body>
</html>
