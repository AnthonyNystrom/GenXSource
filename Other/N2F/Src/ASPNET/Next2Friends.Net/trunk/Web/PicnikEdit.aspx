<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PicnikEdit.aspx.cs" Inherits="PicnikEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="lib/thickbox.css" type="text/css" media="screen" />
    <script type="text/javascript">
        function closePicnik() {
            if (parent == null)
                alert("parent == null");

            parent.tb_remove();
            parent.location.reload(true);
        }
    </script>
</head>
<body style="font: 75%/140% Arial, Helvetica, sans-serif;">
    <div id="divUploader" style="height:500px;text-align:center;background-color:#FFFFFF;">
        <div id='TB_title' style='width:100%'>
            <div id='TB_ajaxWindowTitle'>Edit Your Photo</div>
            <div id='TB_closeAjaxWindow'><a id="TB_closeWindowButton1" href="javascript:parent.tb_remove();">close</a></div>
        </div>
        <script type="text/javascript">
            document.write("<iframe id=\"picnik-frame\" width=\"830px\" height=\"500px\" src=\"");
            document.write("<%=PicnikUrl %>");
            document.write("\"></iframe>");
        </script>
    </div>
</body>
</html>
