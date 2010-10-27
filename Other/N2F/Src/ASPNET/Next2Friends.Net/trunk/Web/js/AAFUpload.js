var lastOption = -1;

function enableOptions(option){
    
    if(lastOption==option){
        return;
    }
    
    if(option==1){
        $('#divMulti').fadeOut(500);
        $('#divCustom').fadeOut(500);
        $('#spanPhotoNo').text('');
    }else if(option==2){
        $('#divMulti').fadeIn(500);
        $('#divCustom').fadeOut(500);
        $('#spanPhotoNo').text('1');
    }else if(option==3){
        $('#divMulti').fadeOut(500);
        $('#divCustom').fadeOut(500);
        $('#spanPhotoNo').text('');
    }else if(option=4){
        $('#divMulti').fadeOut(500);
        $('#divCustom').fadeIn(500);
        $('#spanPhotoNo').text('');
    }
    
    lastOption = option;
}