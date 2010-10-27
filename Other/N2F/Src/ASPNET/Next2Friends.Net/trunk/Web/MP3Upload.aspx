<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MP3Upload.aspx.cs" Inherits="MP3Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Extenders: Modal Popup</title>
    
    <link href="/style.css" rel="stylesheet" type="text/css" />
    <link href="/styleb.css?v=2a" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/lib/jquery.js"></script>
    

<style type="text/css">
html, body {
    background-color:#FFFFFF;
    background-image:none;
}

.mp3row{
	width:450px;
	height:20px;
	padding:2px;
	background-color:#dddddd;
	border:1px solid #999999
}
.noMp3
{
	position:absolute;
	left:150px;
	
}
</style>
</head>

<body style="background-color:#FFFFFF">

<form runat="server">


<div class="iframePlaylist">
    <div class='noMp3' id="divNoMp3" style="display:<%=DisplayNoMP3Message%>;">Your MP3 playlist is empty</div>
    <%=Mp3Lister %>
    
</div>
<div class="uploadIndicator" id="divuploadIndicator" style="display:none;">
		<strong>Uploading...</strong>&nbsp;&nbsp;
		<img src="/images/ajax-loader.gif" width="43" height="11" alt="" />
	</div>

<asp:FileUpload CssClass="form_btn2" Width="235" runat="server" ID="mp3Upload" />
<asp:Button CssClass="form_btn2" runat="server" Text="Upload" ID="btnUpload" OnClientClick="return isMP3();" OnClick="btnUpload_Click" />
<script type="text/javascript">
var restartPlayer = <%=RestartPlayer %>
function deleter(WebMP3ID){
    MP3Upload.DeleteMp3(WebMP3ID, deleteMp3_Callback);
}

function deleteMp3_Callback(response){
    if(response.error==null){
    $('#mp3Lister')
        $('#mp3'+response.value).remove();

        if($('#mp3Lister').children().length==0){
            $('#divNoMp3').show();
        }
        
    }else{
        alert("There was an problem with your request, please try again.");
    }
}
function isMP3(){
    var filename = $('#mp3Upload').val();
    var go = false;
    if(filename.length>5){
        if(filename.substring(filename.length-4,filename.length).toLowerCase()=='.mp3'){
            go = true;
        }else{
            alert('Only mp3 music files are allowed');
            go = false;
        }
    }else{
        go=false;
    }
    
    if(go){
        $('#divuploadIndicator').show();
        return true;
        
    }else{
        alert('Only mp3 music files are allowed');
        return false;
    }
}


</script>
</form>
</body>
</html>
