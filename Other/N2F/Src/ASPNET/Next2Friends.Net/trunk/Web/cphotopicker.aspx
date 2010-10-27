<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cphotopicker.aspx.cs" Inherits="AjaxTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Ajax Photo picker </title>
    
        <style type="text/css">
        .box
        {
            float:left;
            margin: 4px;
            padding: 4px;
            border-style: solid;
            border-width: 1px;
            text-align:center;
            height:400px;
        }
        .box a
        {
            text-decoration:none;
            color:Blue;
           
            font-family:Arial;
        }
        .box a:hover
        {
            text-decoration:underline;
            color:Blue;            
        }
        .previewWrap
        {
            width: 101px;
            height: 101px;
            overflow: hidden;
            position: relative;
            top: 0px;
            left: 0px;
        }
        .previewWrap img
        {
            position: absolute;
        }
        html, body
        {
            margin: 0;
        }
        #testWrap
        {
            margin: 20px 0 0 50px;
            border: 2px;
        }
        panel
        {
        
        }
        .panel img
        {
            border:0px;
            margin:4px;
        }
        #profileerror
        {
            color:Red;
            margin:3px;
        }
        body {
	        font: 75%/120% Arial, Helvetica, sans-serif;
	        margin:0px;
        }
        a {
	        color: #333;
	        text-decoration: underline;
        }
        a:hover {
	        text-decoration: none;
	        color: #000;
        }
        #wrapper {
	        width: 637px;
	        height: 464px;
	        margin: 0 auto;
	        background: url(images/editor-bg.gif) no-repeat;
	        position: relative;
        }
        .browse {
	        position: absolute;
	        left: 26px;
	        top: 15px;
        }
        .cropimg {
	        width: 457px;
	        height: 400px;
	        position: absolute;
	        left: 10px;
	        top: 60px;
	        text-align: center;
        }
        .previewimg {
	        position: absolute;
	        top: 18px;
	        right: 22px;
        }

        .cpane {
	        position: absolute;
	        top: 145px;
	        right: 7px;
	        width: 129px;
        }
        .rotate {
	        width: 125px;
	        height: 20px;
	        margin: 0 0 0 9px;
	        padding: 0 0 15px;
	        clear: both;
	        font-size: 95%;
	        color: #999;
        }
        .rotate img {
	        float: left;
        }
        .rotate span {
	        width: 55px;
	        display: block;
	        float: left;
	        padding-top: 3px;
	        text-align: center;
        }


        .adjust {
	        position: relative;
	        padding: 3px 0 0 7px;
	        width: 122px;
	        height: 48px;
	        font-size: 95%;
	        line-height: 100%;
	        color: #777;
	        margin-bottom: 10px;
        }
        .adjust .handle {
	        width: 9px;
	        height: 15px;
	        background: url(images/handle.gif) no-repeat;
	        cursor: pointer;
        }
        .adjust span {
	        color: #333;
        }
        .adjust .slider {
	        width: 96px;
	        height: 15px;
	        position: absolute;
	        left: 28px;
	        top: 27px;
        }

        .contrast {
	        background: url(images/contrast-bg.gif) no-repeat;
	        margin-top: 15px;
        }
        .brightness {
	        background: url(images/brightness-bg.gif) no-repeat;
        }


        /* save */
        .save {
	        font-size: 95%;
        }
        .save input {
	        font-size: 110%;
	        padding-left: 10px;
	        padding-right: 10px;
        }

        /* form elements */
        input {
	        font: 100% Arial, Helvetica, sans-serif;
	        padding: 3px 5px;
        }
        .form-txt {
	        background: #fff;
	        border: solid 1px #9aaabb;
        }
        .form-btn {
	        background: #fff url(images/formbtn-bg.gif) repeat-x left 1px;
	        border: solid 1px #9aaabb;
	        padding: 3px 8px;
        }
    </style>
    
    <link type="text/css" href=stylepp.css rel=Stylesheet />
    <link href="images/flora.slider.css" rel="stylesheet" type="text/css" />

    <script src="lib/jquery.js" type="text/javascript"></script>
    <script src="lib/jquery-1.2.3.min.js" type="text/javascript"></script>
    <script src="lib/jquery.imgareaselect-0.2.js" type="text/javascript"></script>
    <script src="lib/jquery.dimensions.js" type="text/javascript"></script>
    <script type="text/javascript" src="lib/ui.mouse.js"></script>
    <script type="text/javascript" src="lib/ui.slider.js"></script>

 
    <script type="text/javascript"> 
     
      var imageAreaOption;
      
      var minWidth=100,minHeight=100;

      var sel=[0,0,0,0];
      
      var rotateFlipOpration;
                  

        $(document).ready(function()
   //     function init()
        {
                   // quit if this function has already been called
//       if (arguments.callee.done) return;

//       // flag this function so we don't do the same thing twice
//       arguments.callee.done = true;
                  imageAreaOption={aspectRatio: '1:1',
                        minHeight:100, minWidth:100,
                        top:50,
                        left:50,
                        width:100,
                        height:100,
                        onSelectChange: preview };
            
            $('#slider1').slider( { 
                'range': false, 
                'stepping': 1, 
                'minValue': 0, 
                'maxValue': 255, 
                'startValue': 0, 
                'stop': function(){ 
                       $("#brightness").text(parseInt($(this).slider('value')));}
                    
       } );  
            $('#slider2').slider( { 
                'range': false, 
                'stepping': 1, 
                'minValue': 0, 
                'maxValue': 255, 
                'startValue': 0, 
                'stop': function(){ 
                     $("#contrast").text(parseInt($(this).slider('value')));}
               
                
        } );  
            AjaxTest.GetPhoto(GetPhoto_Callback);
            
         });
 //}


         function RefreshImage(bmp,selection)
         {
         
               var option;
                
            $("#overlay").remove();
            $("#area").remove();
            $("#border1").remove();
            $("#border2").remove();
            
            $('#previewImage').each(function(){ this.src=bmp.src;});
            $('#display').empty();
            $('#display').append(bmp.getImage()).find('img').each(function()
            {
            
             var imageWidth=this.width;
             var imageHeight=this.height;
            
            //if(imageHeight>imageWidth)            {
//            temp=sel.x1;
//            
//            sel.x1=sel.width-sel.y1;
//            
//            sel.y1=temp-imageWidth;
//            
//            
//            temp=sel.x2;
//            
//            sel.x2=sel.y2+sel.width;
//            
//            sel.y2=temp-imageWidth;
//            
//            
//            
//            temp=sel.width;
//            
//            sel.width=sel.height;
//            
//            sel.height=temp;

                     
            if(selection==false)
                option=imageAreaOption;
            else
                option={aspectRatio: '1:1',
                        minHeight:100, minWidth:100,
                        top:sel.y1,
                        left:sel.x1,
                        width:sel.width,
                        height:sel.height,
                        onSelectChange: preview };
                        
          
          //  option=imageAreaOption;
           
            
            }).load(function(){ $(this).imgAreaSelect(option);
            }); 
            
          SetErrorMessage("");
            
         }
         function GetPhoto_Callback(response)
         { 

            var bmp = response.value; 
                 
           RefreshImage(bmp,false);
           
          }
        
        function ReloadPhoto()
        {

            AjaxTest.ReloadPhoto(RotateFlipPhoto_Callback);
            
            return false;
            
        }
        
        function ReloadPhoto_Callback(response)
        { 

           var bmp = response.value; 
                 
           RefreshImage(bmp,true);
   
         }
        
        function RotateFlipPhoto(operation)
        {
            
            rototateFlipOperation=operation;
            
            AjaxTest.RotateFlipPhoto(operation,RotateFlipPhoto_Callback);
            
            return false;
            
        }

        function RotateFlipPhoto_Callback(response)
        {
            var bmp = response.value; 
            
//            switch (rototateFlipOperation)
//            {
//                case rototateFlipOperation.Rotate90FlipNone:
//                    
//                break;
//            }
            
       RefreshImage(bmp,false);
    
        }
        
        function CropPhoto()
        {
        
           var selection = [sel.x1,sel.y1,sel.width,sel.height];
            
           $('#btnSave').attr('disabled','true');
            
           AjaxTest.Complete(selection,CropPhoto_Callback);
           
           return false;
            
        }
        
        function CropPhoto_Callback(response)
        {
        
            if(response.error==null){
                self.parent.tb_remove();
                parent.location.reload();
            
            }else{
                $('#btnSave').attr('disabled','true');
            }
            
            //var bmp = response.value; 
            
            //RefreshImage(bmp);
 
        }

        function AdjustPhoto()
        {
            var brightness=parseFloat($("#brightness").text());
            var contrast=parseFloat($("#contrast").text());
            
            var values=[brightness,contrast];
            
            AjaxTest.AdjustPhoto(values,AdjustPhoto_Callback);
            
            return false;
            
        }
        
        function AdjustPhoto_Callback(response) 
        {
            
            var bmp = response.value; 
            
            if(bmp==null)
            {
            
                SetErrorMessage("Please reload your picture");
                
                return false;
                
            }
            
            RefreshImage(bmp,true);
        
        }
        
        function TransferCoords(newWidth,newHieght,selection)
        {
            
            
        }
        
        function SetErrorMessage(msg)
        {
        
            $("#profileerror").html(msg); 
                       
        }
        
 function preview(img, selection)
 {
    sel=selection;
     
    imgW = jQuery(img).width();
    imgH = jQuery(img).height();
    // get the ratio of the select area to the src image
	var calcWidth = selection.width;
	var calcHeight = selection.height;
	// ratios for the dimensions of the preview image
	var dimRatio = { 
		x: imgW / calcWidth, 
		y: imgH / calcHeight 
	}; 
	//ratios for the positions within the preview
	var posRatio = { 
		x: calcWidth / minWidth, 
		y: calcHeight / minHeight 
	};
	
	// setting the positions in an obj before apply styles for rendering speed increase
	var calcPos	= {
		w: Math.ceil( minWidth * dimRatio.x ) + 'px',
		h: Math.ceil( minHeight * dimRatio.y ) + 'px',
		x: '-' + Math.ceil( selection.x1 / posRatio.x )  + 'px',
		y: '-' + Math.ceil( selection.y1 / posRatio.y ) + 'px'
	}
	
	$("#previewImage").css({ width: calcPos.w, height:calcPos.h,left:calcPos.x,top:calcPos.y});

 }
 </script> 

<script type="text/javascript">
function Loading(st)
{
   if(st)
    $("#loading").show()
   else
    $("#loading").hide()
}

</script>
    
</head>

<body>

    <form id="form1" runat="server">
    <input id="webId" runat="server" type="hidden" />

        <div id="wrapper">

        <div class="browse">
        Upload:

        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-txt form-btn"  />

        <asp:Button ID="Upload" runat="server" Text="Upload" onclick="Upload_Click" CssClass="form-btn"  /> 
        </div>

        <div id="display" runat="server" class="cropimg" >

        <img id="image" alt="" src="" />

        </div>

<div class="previewimg">
        <div id="previewDiv" class="previewWrap">

        <img id="previewImage" src="" class="previewWrap" alt="" />

        </div>
        </div>
        <div class="cpane">
        
        
        <p class="rotate">



        <a href="#" onclick="RotateFlipPhoto(3);return false;"><img src="images/rotate-left.gif" alt="rotate left" style="border-width:0px;"/></a>
        <span>Rotate</span>

        <a href="#" onclick="RotateFlipPhoto(1);return false;"> <img src="images/rotate-right.gif" alt="rotate right" style="border-width:0px;" /></a>
        </p>

        <p class="rotate">



        <a href="#" onclick="RotateFlipPhoto(6);return false;"><img src="images/flip-vertical.gif" alt="flip vertical" style="border-width:0px;" /></a>
        <span>Flip</span>
        <a href="#" onclick="RotateFlipPhoto(4);return false;">  <img src="images/flip-horizontal.gif" alt="flip horizontal" style="border-width:0px;" /></a>
        </p>    

        <%--
        <div id="brightness"></div>    
        <div id='slider1' class='ui-slider-1' style="margin: 5px;">
        <div class='ui-slider-handle' id="indicator1"></div>	
        </div>
        <div id="contrast"></div>
        <div id='slider2'class='ui-slider-1' style="margin: 5px;">
        <div class='ui-slider-handle' id="indicator2"></div>
        </div>

        <a href="#" onclick="AdjustPhoto();return false;">Apply</a>--%>



        <%--     <a href="#" onclick="ReloadPhoto();return false;">Reload</a>&nbsp;&nbsp;--%>
        <p class="save">
        <input name="" id="btnSave" type="button" value="Save" class="form-btn" onclick="CropPhoto();" /> or <a href="javascript:self.parent.tb_remove();">Cancel</a>
        </p>     

        <div id="profileerror"></div>       

        </div>

        </div>


    </form>
</body>
</html>
