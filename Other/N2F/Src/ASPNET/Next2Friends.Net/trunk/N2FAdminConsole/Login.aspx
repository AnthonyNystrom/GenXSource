<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="N2FAdminConsole.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <p><asp:TextBox ID="txtEmail" runat="server">admin@next2friends.com</asp:TextBox></p>
        <p><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></p>
        <p><asp:Button ID="btnLogin" runat="server" Text="Login" onclick="btnLogin_Click" /></p>
    
    </div>
    </form>
</body>
</html>
