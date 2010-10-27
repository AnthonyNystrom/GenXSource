	var maxLen = 1000;  
	
	function aboutMeLen(field,counter){
	    var txtField = $('#'+field.id)[0];
        if(txtField.textLength>=maxLen){
            var caretposition = caret(txtField);
            txtField.value = txtField.value.substring(0,maxLen);
            txtField.caretPos = caretposition;
        }
                         
        var remain = String(maxLen-txtField.value.length);
        $('#'+counter).html(remain);
        
//        if(remain==0){
//            txtField.css('font-color','#FF0000');
//        }
    }
    
    function caret(node) {
     //node.focus(); 
     /* without node.focus() IE will returns -1 when focus is not on node */
     if(node.selectionStart) return node.selectionStart;
     else if(!document.selection) return 0;
     var c		= "\001";
     var sel	= document.selection.createRange();
     var dul	= sel.duplicate();
     var len	= 0;
     dul.moveToElementText(node);
     sel.text	= c;
     len		= (dul.text.indexOf(c));
     sel.moveStart('character',-1);
     sel.text	= "";
     return len;
    }
    
   var drp="ctl00_ContentPlaceHolder1_drpURL";
   var txt="ctl00_ContentPlaceHolder1_txtCURL";
   
   $(drp).change(function()
   {
       // $(this    
   });