<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProfilePhotoPicker.aspx.cs" Inherits="ProfilePhotoPicker" %>
<html>
<head>
    <meta http-equiv="Content-Language" content="en" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="lib/jquery.js" type="text/javascript"></script>
    <script src="lib/interface.js" type="text/javascript"></script>
	
    <style type="text/css" media="all">
        body
        {
	        background: #fff;
	        margin: 0;
	        padding: 0;
	        height: 100%;
        }
        #resizeMe
        {
	        position: absolute;
	        width: 102px;
	        height: 102px;
	        left: 240px;
	        top: 70px;
	        cursor: move;
	        background-position: -190px -20px ;
	        background-repeat: no-repeat;
	        border-top: thin solid #fff;
	        border-bottom: thin solid #fff;
	        border-left: thin solid #fff;
	        border-right: thin solid #fff;
        }
        #resizeSE,
        #resizeE,
        #resizeNE,
        #resizeN,
        #resizeNW,
        #resizeW,
        #resizeSW,
        #resizeS
        {
	        position: absolute;
	        width: 8px;
	        height: 8px;
	        background-color: #333;
	        border: 1px solid #fff;
	        overflow: hidden;	        
        }
        #resizeSE{
	        bottom: -10px;
	        right: -10px;
	        cursor: se-resize;
	    }
        #resizeE
        {
	        top: 50%;
	        right: -10px;
	        margin-top: -5px;
	        cursor: e-resize;
        }
        #resizeNE
        {
	        top: -10px;
	        right: -10px;
	        cursor: ne-resize;
        }
        #resizeN
        {
	        top: -10px;
	        left: 50%;
	        margin-left: -5px;
	        cursor: n-resize;
	        
        }
        #resizeNW{
	        top: -10px;
	        left: -10px;
	        cursor: nw-resize;
        }
        #resizeW
        {
	        top: 50%;
	        left: -10px;
	        margin-top: -5px;
	        cursor: w-resize;
        }
        #resizeSW
        {
	        left: -10px;
	        bottom: -10px;
	        cursor: sw-resize;
        }
        #resizeS
        {
	        bottom: -10px;
	        left: 50%;
	        margin-left: -5px;
	        cursor: s-resize;
        }
        #container
        {
	        position: absolute;
	        top: 50px;
	        left: 50px;
	        width: <%=ImageWidth%>px;
	        height: <%=ImageHeight%>px;
	        background-color: #ccc;
	        background-image: url(http://www.next2friends.com/<%=ImageURL%>);
	        background-repeat: no-repeat;
	    }
        #democontainer
        {
	        position: absolute;
	        top: 50px;
	        left: 750px;
	        width: <%=ImageWidth%>px;
	        height: <%=ImageHeight%>px;
	        background-color: #fff;
	        border: ridge thin #ccc;	        
	    }
        #imgpreview
        {
        	height:102px;
        	width:102px;
        	src:'images/transparent.gif';
        	top:50px;
        	left:750;    
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <%if (ShowDone)
      { %>
            <script>
            self.parent.tb_remove();
            self.parent.location.reload(true);
            </script>

    <%}
      else
      { %>
    <div id="container"></div>
    <div id="resizeMe">
	    <div id="resizeSE"></div>
	    <div id="resizeE"></div>
	    <div id="resizeNE"></div>
	    <div id="resizeN"></div>
	    <div id="resizeNW"></div>
	    <div id="resizeW"></div>
	    <div id="resizeSW"></div>
	    <div id="resizeS"></div>
    </div>
    <div>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="btnUpload" Text="Upload" runat="server" 
            onclick="btnUpload_Click" />
            <asp:Button ID="btnDone" Text="Done" runat="server" onclick="btnDone_Click" />
     </div>
     
   <%--     <div id="democontainer">
     <img id='imgpreview' src='images/transparent.gif' alt="Image Preview" />

        
     </div>--%>
     
             <asp:HiddenField runat="server" ID="x" Value="0" />
        <asp:HiddenField runat="server" ID="y" Value="0" />
        <asp:HiddenField runat="server" ID="width" Value="0" />
        <asp:HiddenField runat="server" ID="height" Value="0" />
    <%} %>
    <script type="text/javascript">
        $(document).ready(
            function()
	        {
	            var ht, wi, tp, lf, tmp1, tmp2;
	            var img1=$('#resizeMe').css('background-image');			        			        			        
				var img2 = img1.substring(4,img1.length-1);			        				        
				var turl='images/transparent.gif';
		        $('#resizeMe').Resizable(
			    {
			        minWidth: 102,
				    minHeight: 102,
				    maxWidth: 400,
				    maxHeight: 400,
				    minTop: 50,
				    minLeft: 50,
				    maxRight: 700,
				    maxBottom: 500,
					ratio: 1.0,
				    dragHandle: true,

				    handlers: 
				    {
					    se: '#resizeSE',
					    e: '#resizeE',
					    ne: '#resizeNE',
					    n: '#resizeN',
					    nw: '#resizeNW',
					    w: '#resizeW',
					    sw: '#resizeSW',
					    s: '#resizeS'
				    },
				    
				    onDrag : function (size, position)
				    {
				        this.style.backgroundPosition = '-' + (size - 50) + 'px -' + (position - 50) + 'px';
				        lf=size-50; tp=position-50;

				        ht=$(this).height(); 
					    wi=$(this).width();
				        //$('#democontainer').html("<img id='imgpreview' src=" + img2 + " alt='ImagePreview'/>");	
				        //$('#imgpreview').css('src',img2);
				        //$('#imgpreview').css('height',102);
				        //$('#imgpreview').css('width',102);
				        //$('#imgpreview').css('top',tp);
				        //$('#imgpreview').css('left',lf);			        	
                        //$('#imgpreview').crop(lf,tp,202,202,turl);	
                        $('#x').val(lf);
                        $('#y').val(tp);
                        $('#width').val(wi);
                        $('#height').val(ht);
				
				    },
				    
				    onResize : function(size, position) 
				    {
					    //this.style.backgroundPosition = '-' + (size - 50) + 'px -' + (position - 50) + 'px';
				        //lf=size-50; tp=position-50;
					},
					

			    
			    })
	        }
        );   


    </script>

    
    
    </form>


</body>
</html>
