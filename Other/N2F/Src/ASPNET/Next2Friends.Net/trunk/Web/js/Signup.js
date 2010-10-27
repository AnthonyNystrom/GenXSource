var ahref = $get('aAvailability');
var txtNickName = $get('ctl00_ContentPlaceHolder1_txtNickName');
var spanErrNickName = $get('spanErrNickName');
var enableCheckAvailability = false;

function setAvailabilityLink(){
	if(txtNickName.value==''){
	    enableCheckAvailability = false;
	}else{
    
	    if(spanErrNickName!=null){
	        spanErrNickName.style.display = 'none';
	    }
	    
	    ahref.innerHTML ='Check availability';
	    ahref.setAttribute('class', 'checkAvailability'); 
	    enableCheckAvailability = true;
	    ahref.style.display = 'inline';
	}
}

function checkAvailability(){
    if(enableCheckAvailability){
        ahref.innerHTML ='Checking availability';
        ahref.setAttribute('class', 'checkAvailability checkingAvailability'); 
        Signup.CheckAvailability(txtNickName.value, checkAvailability_Callback);
        txtNickName.focus();
    }					    
}

function checkAvailability_Callback(response){
    
    // if the name is available
    if(response.value==true){
        ahref.innerHTML ='Available';
        ahref.setAttribute('class', 'checkAvailability checkingAvailability yesAvailability'); 
	    enableCheckAvailability = false;
        
    }else{
        ahref.innerHTML ='Not Available';
        ahref.setAttribute('class', 'checkAvailability checkingAvailability notAvailalble'); 
	    enableCheckAvailability = false;
    }
}

function hidePasswordError(){
    var spanErrPassword = $get('spanErrPassword');
    if(spanErrPassword!=null){
        spanErrPassword.style.display = 'none';
    }

}