view<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="AboutMe.aspx.cs" Inherits="AboutMe" Title="Untitled Page" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <style>
            * {
	        margin: 0;
	        padding: 0;
        }
        body {
	        font: 75%/140% Arial, Helvetica, sans-serif;
	        color: #000000;
	        background-color: <%=BGColor%>;
        }

        a {
	        color: #0257ae;
	        text-decoration: none;
	        outline: none;
        }
        a:hover {
	        text-decoration: underline;
        }
        ul {
	        margin: 0;
	        padding: 10px 0;
        }
        ul li {
	        margin: 0;
	        padding: 2px 0 2px 26px;
        }
        p {
	        padding: 0 0 1em;
        }
        img {
	        border: none;
        }
        h2 {
	        font: bold 160%/120% Arial, Helvetica, sans-serif;
	        padding: 0 0 4px;
        }
        h3 {
	        font: bold 120%/120% Arial, Helvetica, sans-serif;
	        padding: 0 0 3px;
        }
        h4 {
	        font: bold 110%/120% Arial, Helvetica, sans-serif;
        }        

    </style>
</head>
<body id="framePage" name="framePage" onload="resizeIframe()"  bgcolor="#ecf0f3">
<script type="text/javascript" src="lib/jQuery.js"></script>
<script type="text/javascript">
function resizeIframe() {
    
    if( parent.fixAboutMeHeight )
        parent.fixAboutMeHeight($(document).height());
    else
        setTimeout('resizeIframe()',1000);
    //var thisHeight = $('#framePage').height();
    
    //$('#iframeAboutMe').style.height = height + 'px';
}

function cSkin(bg,bdr,index)
{
    $('#framePage').css('background-color',bg);                   
}

</script>


    <div>
        <%=AboutMeHTML %>
    </div>

</body>
</html>
