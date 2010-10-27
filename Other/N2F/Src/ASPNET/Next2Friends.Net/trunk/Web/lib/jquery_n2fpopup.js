 

 
// jQuery.fn.n2fpopup = function(title,callback) {
// 
//    if(this.html().indexOf('innerPopup') == -1){
//        
//        var starthtml = '<div class="innerPopup" id="divPopup"><div class="innerPopupTop"/><div class="innerPopupContent"><div class="innerPopupTitle"><a style="float:right;" href="javascript:n2fpopupClose();' + callback + '">close</a><h3>'+title+'</h3></div><div class="innerPopupBlock">';		
//        var endhtml = '</div></div><div class="innerPopupBottom"/></div>';
//        var boxedhtml = starthtml+this.html()+endhtml;
//        
//        this.html(boxedhtml);
//        this.show();
//    }
//    
//    $('#divPopup').show();
//    
//};



function n2fpopupClose() {
    $('#divPopup').hide();
}
