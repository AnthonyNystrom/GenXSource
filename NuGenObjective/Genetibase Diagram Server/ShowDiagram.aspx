<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowDiagram.aspx.vb" Inherits="ShowDiagram" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OCIDML Server</title>
	<link href="./css/styles.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="pageWrapper">
	<div class="header">
		<h1 class="homeLink"><a href="Default.aspx" title="Genetibase Diagram Server" onfocus="if(this.blur)this.blur()">Genetibase Diagram Server</a></h1>
	</div>
	<div class="content">
        <asp:Image ID="imgPage" runat="server" />
    </div>
    <div id="pageTabs" class="pagination" runat="server">
    </div>      
	<div class="footer">
		Copyright &copy; <a href="http://www.genetibase.com">Genetibase, Inc.</a> All Rights Reserved.
	</div>
    </div>  
    </form>
</body>
</html>
