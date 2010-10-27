var prefix = 'ctl00_ContentPlaceHolder1_PhotoRepeater_ctl';

function ajaxEditGallery(webPhotoCollectionID, txtName, txtDescription){

    MyPhotoGallery.EditPhotoGallery(webPhotoCollectionID, txtName.value, txtDescription.value, ajaxEditGallery_Callback);
}
var error= null;
function ajaxEditGallery_Callback(response){

    if(response.value!=null){
        renameGalleryDropdowns(response.value.WebPhotoCollectionID,response.value.Name,response.value.Description)
        
        //var Description = (response.value.Description.length>75) ? response.value.Description.substr(75)+'...' : response.value.Description;
        
        $('#lblName'+response.value.WebPhotoCollectionID).html('<strong>'+response.value.Name+'</strong>');
        $('#lblDescription'+response.value.WebPhotoCollectionID).html(response.value.Description);
        var itemEdit  = $('#edit'+response.value.WebPhotoCollectionID);
        itemEdit.slideUp();
        
    }else{
     alert('Please try again');
     error = response.error;
    }
}

function renameGalleryDropdowns(WebPhotoCollectionID, newName, newDescription){

    var drps = document.getElementsByTagName('select');
    var lblName = $('#lblName'+WebPhotoCollectionID);
    var lblDescription = $('#lblDescription'+WebPhotoCollectionID);
    lblName.innerHTML = '<strong>'+newName+'</strong>';
    lblDescription.innerHTML = newDescription;
    
    for(var i=0;i<drps.length;i++){
    
        if(drps[i].id.indexOf('drpGallery')!=-1){
            
            // loop through the options
            for(var j=0;j<drps[i].options.length;j++){
            
                if(drps[i][j].value==WebPhotoCollectionID){
                    drps[i][j].text = newName;
                }
            }
        }
    }
}


function createNewGallery(){

    var newGalleryDescription = $('#newGalleryDescription');
    var newGalleryName = $('#newGalleryName');
    
    MyPhotoGallery.CreateNewGallery(newGalleryName.value, newGalleryDescription.value, createNewGallery_Callback);
    
    newGalleryDescription.value = '';
    newGalleryName.value = '';
}


function createNewGallery_Callback(response){

    if(response.value!=null){
        // gallery is created
        addGalleryToDrps(response.value.WebPhotoCollectionID,response.value.Name,response.value.Description);
        var newGalleryName = $('#newGalleryName');
        var newGalleryDescription = $('#newGalleryDescription');

        window.location.href = 'MyPhotoGallery.aspx';
        //newGalleryName.value = '';
        //newGalleryDescription.value = '';
    }else{
         alert('Please try again');
         error = response.error;
    }
}

function addGalleryToDrps(WebPhotoCollectionID, newName, newDescription){
    
    var drps = $('select');
    
    for(var i=0;i<drps.length;i++){
    
        if(drps[i].id.indexOf('drpGallery')!=-1){
            
            drps[i].options[drps[i].options.length] = new Option(newName, WebPhotoCollectionID, false, false)

        }
    }

}

function deleteGallery(WebPhotoCollectionID){
    window.location.href='MyPhotoGallery.aspx?delete='+WebPhotoCollectionID;
}

function cancelNew(){
   $('#newGalleryDescription').value='';
   $('#newGalleryName').value='';
}

function rotate(index, isLeft){
    //var r = $('#ctl00_ContentPlaceHolder1_PhotoRepeater_ctl'+id+'_Rotation')[0];
    var r = $('#'+prefix+index+'_Rotation')[0];
  //  alert(r);
    var val = parseInt(r.value);

    if(isLeft){
        r.value = (r.value!=0) ? --val : 3;
    }else{
        
        r.value = (r.value!=3) ? ++val : 0;
    }
//    alert(r.value);
}

function toggleColor(c){

    if(c.checked){
        $(c).parent().parent().removeClass('checked');
    }else{
        $(c).parent().parent().addClass('checked');
    }
}

function enableSave(index){

    var p = $('#pQSave'+index);
    p.addClass('item_active');
    document.getElementById('btnQSave'+index).disabled = false;
    

}

function saveItem(webPhotoID, index){

    var txtCaption = $('#'+prefix+index+'_txtCaption')[0];
    //var txtTags = $('#'+prefix+index+'_txtTags')[0];
    var dropGallery = $('#'+prefix+index+'_drpGallery')[0];
    var rValue = $('#'+prefix+index+'_Rotation')[0];
    var chbDelete = $('#'+prefix+index+'_chbDelete')[0];
    //var drpCategories = $('#'+prefix+index+'_drpCategories')[0];
    
    var rotation = parseInt(rValue.value);
 //   alert('save item'+rotation);
    var newGallery = dropGallery.options[dropGallery.selectedIndex].value;
    //var CatgoryID = drpCategories.options[drpCategories.selectedIndex].value;
    //CatgoryID = parseInt(CatgoryID);

    MyPhotoGallery.SaveSingle(webPhotoID,txtCaption.value,  chbDelete.checked, newGallery, rotation,index, saveItem_Callback)
   
}

function saveItem_Callback(response, args){
 //   alert("saveItem_Callback null");
    if(response.value!=null){
        var p = $('#pQSave'+response.value.Index).removeClass('item_active');
        var btn = $('#btnQSave'+response.value.Index).disabled = true;
       
//        var rotate = $('#ctl00_ContentPlaceHolder1_PhotoRepeater_ctl'+response.value.Index+'_Rotation');
//        rotate.value = 0;
        var rotate = $('#ctl00_ContentPlaceHolder1_PhotoRepeater_ctl'+response.value.Index+'_Rotation')[0];
        rotate.value = 0;
        
            //resave the formatted tags back to the textbox
            //var txtTags = $('#'+prefix+response.value.Index+'_txtTags')[0];
            //txtTags.value = response.value.Tags;
  //          alert("saveItem_Callback");
        
        if(response.value.IsRemoved){
            var photoItem = $('#photoItem'+response.value.Index);
            photoItem.fadeOut();
        }
        
    }else {
    
        alert('error occured');
    
    
    }
}


function populateTags(index){

    var drpCat = $('#ctl00_ContentPlaceHolder1_PhotoRepeater_ctl'+index+'_drpCategories')[0];
    var categoryID = drpCat.value;
    var html='';
    var tags = getCategoryTags(categoryID)
    
    for(var i=0;i<tags.length;i++){
        html += "<a style='cursor:pointer;' onclick='appendTag(\""+index+"\",\""+tags[i]+"\");$(this).css(\"font-weight\",\"bold\");'>"+tags[i]+"</a> ";
    }
    
    $('#pTags'+index).html(html);
    
  
}

function appendTag(index, tag){
    var txtTags = $('#ctl00_ContentPlaceHolder1_PhotoRepeater_ctl'+index+'_txtTags')[0];
    
    var currentTags = txtTags.value.split(',');
    var allowed = true;
    
    for(var i=0;i<currentTags.length;i++){
        if(currentTags[i].trim()==tag){
            allowed = false;
        }
    }
    
    if(allowed){
        if(txtTags.value!=""){
            txtTags.value +=", ";
        }
        
        txtTags.value += tag;
    }
}

function getCategoryTags(categoryID){

    for(var i = 0;i<tagArray.length;i++){
        if(tagArray[i][0]==categoryID){
            return tagArray[i][1];
        }
    }
}


