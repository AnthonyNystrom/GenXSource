<%@ Page Language="C#" AutoEventWireup="true" CodeFile="J2meDownload.aspx.cs" Inherits="J2MEDownload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download Next2Friends</title>
    <style type="text/css">
    .downloadlink {
	    font-size: 140%;
	    background: url(images/link-arrow.gif) left 3px no-repeat;
	    padding-left: 20px;
    }
    .downloadlink:hover {
	    text-decoration: none;
    }
    .downloadlink small {
	    font-size: 65%;
	    display: block;
	    padding: 3px 0 0 20px;
	    color: #999;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <span class='downloadlink'>Download Ask <a style='font-size:smaller' href='/4'>.Jar </a> | <a style='font-size:smaller' href='/5'>.Jad </a><small>Ask v1.0 (245k)</small></span><br />
    <span class='downloadlink'>Download Tag <a style='font-size:smaller' href='/6'>.Jar </a> | <a style='font-size:smaller' href='/7'>.Jad </a><small>Tag v1.0  (125k)</small></span><br />
    <span class='downloadlink'>Download Snap-up <a style='font-size:smaller' href='/8'>.Jar </a> | <a style='font-size:smaller' href='/9'>.Jad </a> <small>Snap-up v1.0  (100k)</small></span><br />
    
    </div>
    </form>
</body>
</html>
