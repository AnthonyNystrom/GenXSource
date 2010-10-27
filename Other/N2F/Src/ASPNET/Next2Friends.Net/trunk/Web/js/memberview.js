var showingEdit = false;
var hideCarousel = '';

function showEditAboutMe(){
 
    var divEditAboutMe = $('#divEditAboutMe');
    
    if(!showingEdit){
       
        var iframeAboutMe = $('#iframeAboutMe');
        var txtAboutMe = $('#txtAboutMe');
        
        var height = (iframeAboutMe.height() < 150) ? 150 : iframeAboutMe.height();
        
        txtAboutMe.height(height);
        
        txtAboutMe.value = 'loading..';
        
        iframeAboutMe.hide();
        divEditAboutMe.fadeIn('fast');
        
        ajaxGetAboutMe();
        
        showingEdit = true;
   }
}

function fixAboutMeHeight(newh)
{
    $('#iframeAboutMe').attr("height",newh);
}

function cancelEditAboutMe(){
    $('#divEditAboutMe').hide();
    $('#divAboutMe').fadeIn('fast');
    $('#iframeAboutMe').fadeIn('fast');
    showingEdit = false;
}

function ajaxGetAboutMe(){

    MemberView.GetAboutMeText(ajaxGetAboutMe_Callback)
}


function ajaxGetAboutMe_Callback(response){

    $('#txtAboutMe')[0].value = response.value;
}

function ajaxEditAboutMe(){
    var txtAboutMe = $('#txtAboutMe')[0];
    MemberView.UpdateAboutMe(txtAboutMe.value, ajaxEditAboutMe_Callback)
}


function ajaxEditAboutMe_Callback(response){    
    if(response.value!=null){
        var iframeAboutMe = $('#iframeAboutMe');
        var divEditAboutMe = $('#divEditAboutMe');
        
        iframeAboutMe.css('display','block');
        divEditAboutMe.css('display','none');
        
        showingEdit = false;        
        iframeAboutMe.attr('src',response.value);

    }else {
        alert('Ooops, there was a problem updating your Profile, please try again!');
    }
}



var cachedListerContent = new Array();

function updateCurrentTab(tabType){
    $('#contentTab1').removeClass('current');
    $('#contentTab2').removeClass('current');
    $('#contentTab'+tabType).addClass('current');
}

function showGalleryD(webMemberID){
             
        var s1 = new SWFObject("gallery.swf","gallery","628","500","7");
        s1.addParam("allowfullscreen","true");
        s1.addParam("autostart","true");
        s1.addParam('bgcolor','#FFFFFF');
        s1.addVariable("xmlfile","MyImagesXML.aspx?m="+webMemberID);
        s1.addVariable("width","525");
        s1.addVariable("height","396");
        s1.write("ulContentLister");     
        
        $('#pPager').html('');
}

function ajaxGetListerContent(webMemberID, tabType, Page){

    updateCurrentTab(tabType);

    // dont need to reload the gallery
    if(tabType!=2){

        if(cachedListerContent[tabType]==null){
            MemberView.GetListerContent(webMemberID, tabType+0, Page, ajaxGetListerContent_Callback)
        }else{
            updateListerContent(cachedListerContent[tabType]);
        }
    }else{

        updateListerContent('');
        showGalleryD(webMemberID);
    }
}

function ajaxGetListerContent_Callback(response){

    if(response.value!=null){
        
        updateListerContent(response.value.HTML);
        $('#pPager').html(response.value.PagerHTML);
        //cachedListerContent[response.value.TabType] = response.value.HTML;
		$(".profile_vid_list li:even").addClass("clear");
    }else {
        alert('Ooops, there was a problem with your request, please try again!');
    }
    
function updateListerContent(html){
    $('#ulContentLister').html(html);
}
}