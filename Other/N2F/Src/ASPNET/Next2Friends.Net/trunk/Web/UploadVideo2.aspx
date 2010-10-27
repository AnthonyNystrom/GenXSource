<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadVideo2.aspx.cs" Inherits="UploadVideo2" Title="" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.QuickStart" Assembly="Telerik.QuickStart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.WebControls" Assembly="RadUpload.NET2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link rel="stylesheet" href="lib/thickbox.css" type="text/css" media="screen" />
    <script type="text/javascript" src="/lib/jquery.js"></script>
    <script type="text/javascript" src="lib/jquery.validate.pack.js"></script>
    <script type="text/javascript">
$(document).ready(function() {
	var validator = $("#form1").validate({
		rules: {txtTitle: {
				required: true,
				minlength: 10
			},
			txtCaption: {
				required: true,
				minlength: 15
				
			}},
		messages: {
				txtTitle: {
				required: "Provide a title",
				rangelength: jQuery.format("Title must be at least {0} characters")
			},
			password_confirm: {
				required: "Provide a discription",
				minlength: jQuery.format("Description must be at least {0} characters")				
			}},
		// the errorPlacement has to take the table layout into account
		errorPlacement: function(error, element) {
			if ( element.is(":radio") )
				error.appendTo( element.parent().next().next() );
			else if ( element.is(":checkbox") )
				error.appendTo ( element.next() );
			else
				error.appendTo( element.parent().next() );
		}
				
	});	
});
</script>

</head>
<style>
body {
	font: 75%/140% Arial, Helvetica, sans-serif;
}
.form_btn2 {
	padding: 1px 10px;
	background: #fff url(images/form-btn.gif) repeat-x;
	border: solid 1px #9aaabb;
}
.error
{
    color:Red;
    /*float:left;*/
}
</style>
<body>

    <form id="form1" runat="server">
    <div id="divUploader" style="background-color:#FFF;height:480;width:100%;text-align:center;border:15px">
        <div id='TB_title' style='width:100%'><div id='TB_ajaxWindowTitle'>Upload your videos</div><div id='TB_closeAjaxWindow'><a id='TB_closeWindowButton' href='javascript:self.parent.tb_remove();'>close</a></div></div>
         <br /><br />
         <table style="width:100%;height:250px;">
         
            <tr>
            <td width="50px"></td>
                <td style="text-align:left;vertical-align:top;">
<%--                <h3>
					Next2Friends support the following formats:
				</h3>--%>
				  <div class="edit_item_right">
                
                    <p>Title <asp:TextBox runat="server"  MaxLength="30" ID="txtTitle" style="width:269px;" CssClass="form_txt title"></asp:TextBox><div class="error"></div></p>
                    <p>
                        Caption<br />
                        <asp:TextBox runat="server" TextMode="MultiLine" style="width:295px;"  ID="txtCaption" CssClass="form_txt caption"></asp:TextBox><div class="error"></div>
                    </p>
                    <div class="clear"></div>
                </div>
                <p></p>
                <p></p>
                <div class="edit_item_right">
                <br />
                <ul type="square">
				    <li>Windows Media (AVI, WMV, ASF)</li>
				    <li>QuickTime (MOV, QT, DV)</li>
				    <li>MPEG (MPEG-1, MPEG-2, MPEG-4)</li>
				    <li>Mobile phone formats (3GP, 3G2).</li>
                </ul>
                <br />
                    <input type="file" runat="server" id="inputFile" class="form_btn2" />
                   <%-- <rad:RadUpload runat="server" ID="radUpload" Skin="Web20" />--%>
                    <asp:button id="button1" runat="server" OnClientClick="document.getElementById('divMessage').innerHTML='';" OnClick="buttonSubmit_Click" text="Upload" cssclass="form_btn2" />
                </div>
                </td>
                
                </tr>
                <tr>
                <td width="50px"></td>
                <td>
                    <rad:radprogressmanager id="Radprogressmanager1" Skin="Default2006"  SuppressMissingHttpModuleError="false" runat="server" />
                    <rad:radprogressarea   Visible="true" id="RadProgressArea1" DisplayCancelButton="True" Skin="Default2006" runat="server"></rad:radprogressarea>
                    <div id="div1"><asp:Literal runat="server" ID="litCompleted"></asp:Literal></div>
                </td>

            </tr>
            </table>
    </div>
    </form>
</body>
</html>

