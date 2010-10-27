<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="ajax.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
function yes()
{
    alert("Continue");
}
function no()
{
    alert("Dont continue");
}
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Extenders: Modal Popup</title>
</head>
<body>
    <form id="form1" runat="server">
<%--        <asp:ScriptManager id="ScriptManager1" runat="server" /> 
        
        <div style="margin:20px;">
        
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Log into chat" /> 
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" style="display:none">
                <div style="margin:10px">
                You are about to enter integrated chat mode which means that your location bar in 
                your browser wont show you location while it's running. <br />Is this okay?<br /><br />
                <asp:Button ID="Button1" runat="server" Text="Yes" width="40px"  />
                <asp:Button ID="Button2" runat="server" Text="No"  width="40px" />
                </div>
        </asp:Panel>
        </div>
           
        <act:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                    TargetControlID="LinkButton1"
                    PopupControlID="Panel1"
                    BackgroundCssClass="modalBackground"
                    DropShadow="true"
                    OkControlID="Button1"
                    OnOkScript="yes()"
                    OnCancelScript="no()"
                    CancelControlID="Button2" />
   --%>
    </form>
</body>
</html>
