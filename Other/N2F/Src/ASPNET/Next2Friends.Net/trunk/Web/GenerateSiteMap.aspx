﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenerateSiteMap.aspx.cs" Inherits="GenerateSiteMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button runat="server" ID="btnGenerateSiteMap" Text="Generate" onclick="btnGenerateSiteMap_Click" /><br />
    <a href="/gsitemap.xml">View Site Map</a>
    </div>
    </form>
</body>
</html>
