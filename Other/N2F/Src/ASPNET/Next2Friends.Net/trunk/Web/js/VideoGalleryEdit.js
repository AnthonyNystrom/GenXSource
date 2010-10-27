    var prefix = 'ctl00_ContentPlaceHolder1_VideoRepeater_ctl';

function ajaxEditGallery(webVideoCollectionID, txtName, txtDescription){
    MyVideoGallery.EditVideoGallery(webVideoCollectionID, txtName.value, txtDescription.value, ajaxEditGallery_Callback);
}
var error= null;
function ajaxEditGallery_Callback(response){

    if(response.value!=null){
        renameGalleryDropdowns(response.value.WebVideoCollectionID,response.value.Name,response.value.Description)
        
        var itemEdit  = $('#edit'+response.value.WebVideoCollectionID);
        itemEdit.slideUp();
        
    }else{
     alert('Please try again');
     error = response.error;
    }
}

function renameGalleryDropdowns(WebVideoCollectionID, newName, newDescription){

    var drps = document.getElementsByTagName('select');
    var lblName = $('#lblName'+WebVideoCollectionID);
    var lblDescription = $('#lblDescription'+WebVideoCollectionID);
    lblName.innerHTML = '<strong>'+newName+'</strong>';
    lblDescription.innerHTML = newDescription;
    
    for(var i=0;i<drps.length;i++){
    
        if(drps[i].id.indexOf('drpGallery')!=-1){
            
            // loop through the options
            for(var j=0;j<drps[i].options.length;j++){
            
                if(drps[i][j].value==WebVideoCollectionID){
                    drps[i][j].text = newName;
                }
            }
        }
    }
}


function createNewGallery(){

    var newGalleryDescription = $('#newGalleryDescription');
    var newGalleryName = $('#newGalleryName');
    
    MyVideoGallery.CreateNewGallery(newGalleryName.value, newGalleryDescription.value, createNewGallery_Callback);
    
    newGalleryDescription.value = '';
    newGalleryName.value = '';
}


function createNewGallery_Callback(response){

    if(response.value!=null){
        // gallery is created
        addGalleryToDrps(response.value.WebVideoCollectionID,response.value.Name,response.value.Description);
        var newGalleryName = $('#newGalleryName');
        var newGalleryDescription = $('#newGalleryDescription');

        window.location.href = 'MyVideoGallery.aspx';
        //newGalleryName.value = '';
        //newGalleryDescription.value = '';
    }else{
         alert('Please try again');
         error = response.error;
    }
}

function addGalleryToDrps(WebVideoCollectionID, newName, newDescription){
    
    var drps = $('select');
    
    for(var i=0;i<drps.length;i++){
    
        if(drps[i].id.indexOf('drpGallery')!=-1){
            
            drps[i].options[drps[i].options.length] = new Option(newName, WebVideoCollectionID, false, false)

        }
    }

}

function deleteGallery(WebVideoCollectionID){
    window.location.href='MyVideoGallery.aspx?delete='+WebVideoCollectionID;
}

function cancelNew(){
   $('#newGalleryDescription').value='';
   $('#newGalleryName').value='';
}

function rotate(id, isLeft){
    var r = $('#ctl00_ContentPlaceHolder1_VideoRepeater_ctl'+id+'_Rotation');
    var val = parseInt(r.value);

    if(isLeft){
        r.value = (r.value!=3) ? ++val : 0;
    }else{
        r.value = (r.value!=0) ? --val : 3;
    }
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

function saveItem(webVideoID, index){

    var txtTitle = $('#'+prefix+index+'_txtTitle')[0];
    var txtCaption = $('#'+prefix+index+'_txtCaption')[0];
    var txtTags = $('#'+prefix+index+'_txtTags')[0];
    var chbDelete = $('#'+prefix+index+'_chbDelete')[0];
    var chbPrivacy = $('#'+prefix+index+'_chbPrivacy')[0];
    var drpCategories = $('#'+prefix+index+'_drpCategories')[0];
    
    var CatgoryID = drpCategories.options[drpCategories.selectedIndex].value;
    CatgoryID = parseInt(CatgoryID);

    MyVideoGallery.SaveSingle(webVideoID,CatgoryID,txtTags.value, txtTitle.value, txtCaption.value,  chbDelete.checked, index,chbPrivacy.checked, saveItem_Callback);
   
}

function saveItem_Callback(response, args){
    if(response.value!=null){
            var p = $('#pQSave'+response.value.Index).removeClass('item_active');
            var btn = $('#btnQSave'+response.value.Index).disabled = true;
       
        
            //resave the formatted tags back to the textbox
            var txtTags = $('#'+prefix+response.value.Index+'_txtTags')[0];
            txtTags.value = response.value.Tags;
        
        if(response.value.IsRemoved){
            var VideoItem = $('#VideoItem'+response.value.Index);
            VideoItem.fadeOut();
        }
        
    }else {
    
        alert('error occured');
    }
}


function populateTags(index){

    var drpCat = $('#ctl00_ContentPlaceHolder1_VideoRepeater_ctl'+index+'_drpCategories')[0];
    var categoryID = drpCat.value;
    var html='';
    var tags = getCategoryTags(categoryID)
    var close = '<a href="javascript:hideTags(\''+index+'\')" title="close"><img src="images/close-tags.gif" alt="close" class="close_tags" /></a>';
    for(var i=0;i<tags.length;i++){
        html += "<a style='cursor:pointer;' onclick='appendTag(\""+index+"\",\""+tags[i]+"\");$(this).addClass(\"tagged\");'>"+tags[i]+"</a> ";
    }
    
    if(tags.length==0){
        html = ' Choose a category for suggested Tags ';
    }
    
    html += close;
    
    $('#pTags'+index).html(html);
    displayTags(index);
}

function hideTags(index){
    $('#pTags'+index).fadeOut();
}

function displayTags(index){
    $('#pTags'+index).show();
}

function appendTag(index, tag){
    var txtTags = $('#ctl00_ContentPlaceHolder1_VideoRepeater_ctl'+index+'_txtTags')[0];
    
    var currentTags = txtTags.value.split(',');
    var allowed = true;
    
    for(var i=0;i<currentTags.length;i++){
        if(currentTags[i].trim()==tag){
            allowed = false;
        }
    }
    
    if(allowed){
        if(txtTags.value!=""){
            tag += ", ";
        }
        
        txtTags.value = tag + txtTags.value;
    }
}

function getCategoryTags(categoryID){

    for(var i = 0;i<tagArray.length;i++){
        if(tagArray[i][0]==categoryID){
            return tagArray[i][1];
        }
    }
}


