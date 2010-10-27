var cachedListerContent = new Array();
var cachedPager = new Array();

function unfriendMember(WebMemberID){
    if(confirm("Are you sure you want to perminantly remove this friend from your network?")){
        CommunityPage.UnfriendMember(WebMemberID, unfriendMember_Callback);
    }
}

function unfriendMember_Callback(response){
    
    if(response.value!=null){
        var divFriends = document.getElementById('divFriends');
        var divFriend = document.getElementById('divFriend'+response.value);
        
        divFriends.removeChild(divFriend);
       
    }
}



function addTofriends(webMemberID){    
    CommunityPage.AddToFriends(webMemberID, addToFriends_Callback);
}

function addToFriends_Callback(response){
    if(response.value!=null){
        $('#spanAddToFriends' + response.value.WebMemberID).html("&nbsp;<img src='/images/check.gif' />");
    }else {
        alert('Ooops, there was a problem with your request, please try again!');
    }
}

function ajaxGetListerContent(tabType){

    updateCurrentTab(tabType);
    
    if(cachedListerContent[tabType]==null){
        CommunityPage.GetListerContent(tabType+0, ajaxGetListerContent_Callback)
    }else{
        updateListerContent(cachedListerContent[tabType], cachedPager[tabType]);
    }
}

function ajaxGetListerContent_Callback(response){

    if(response!=null){
        if(response.value!=null){
            updateListerContent(response.value.HTML, response.value.PagerHTML);
            cachedListerContent[response.value.TabType] = response.value.HTML;
            cachedPager[response.value.TabType] = response.value.PagerHTML;
        }
    }  
}

function updateListerContent(html, pagerHTML){
    document.getElementById('ulContentLister').innerHTML = html;
    document.getElementById('pPageNav').innerHTML = pagerHTML;    
}

function updateCurrentTab(tabType){
    document.getElementById('contentTab1').className  = '';
    document.getElementById('contentTab2').className  = '';
    document.getElementById('contentTab3').className  = '';
    document.getElementById('contentTab4').className  = '';
    
    document.getElementById('contentTab'+tabType).className  = 'current';
        
}