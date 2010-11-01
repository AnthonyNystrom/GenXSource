<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OCIDML Server</title>
	<link href="./css/styles.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            OCIDML Server</h1>
            <p>
        <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label><br />
        <asp:BulletedList ID="lstDiagrams" runat="server" DisplayMode="HyperLink">
        </asp:BulletedList>    
        </p>
    </div>
    	<div class="footer">
		Copyright &copy; <a href="http://www.genetibase.com">Genetibase, Inc.</a> All Rights Reserved.
	</div>
    </form>
</body>
</html>
