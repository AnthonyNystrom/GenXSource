<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Message.aspx.cs" validateRequest="false" Inherits="N2FAdminConsole.Message" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Send Message to All Users</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>
            
            <asp:TextBox ID="txtNick" runat="server"></asp:TextBox>
            
        </p>
        <p>
            
            <asp:TextBox ID="txtMessage" runat="server" Height="293px" TextMode="MultiLine" 
                Width="100%"></asp:TextBox>
            
        </p>
        <p>
            
            <asp:Button ID="btnTestSend" runat="server" Text="Test Send" 
                onclick="btnTestSend_Click" />
            
        </p>
        <p>
            
            &nbsp;</p>
        <p>
            
            &nbsp;</p>
    </div>
    <p>
            
            <asp:Button ID="btnSend" runat="server" Text="Send to All Users" 
                onclick="btnSend_Click" Enabled="False" BackColor="#CC3300" 
            Font-Bold="True" />
            
        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" 
            oncheckedchanged="CheckBox1_CheckedChanged" Text="Enable Send All" />
    </p>
    <p>
        &nbsp;</p>
    </form>
</body>
</html>
