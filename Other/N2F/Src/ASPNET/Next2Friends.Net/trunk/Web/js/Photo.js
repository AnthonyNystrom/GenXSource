var cachedListerContent = new Array();
var cachedPager = new Array();

function ajaxGetListerContent(tabType){

    updateCurrentTab(tabType);
    
    if(cachedListerContent[tabType]==null){
        PhotoPage.GetListerContent(tabType+0, ajaxGetListerContent_Callback, ajaxGetListerContent_CallbackFail)
    }else{
        updateListerContent(cachedListerContent[tabType], cachedPager[tabType]);
    }
}

function ajaxGetListerContent_Callback(response, b,c){

    if(response!=null){
        if(response.value!=null){
            updateListerContent(response.value.HTML, response.value.PagerHTML);
            cachedListerContent[response.value.TabType] = response.value.HTML;
            cachedPager[response.value.TabType] = response.value.PagerHTML;
        }
    }  
}


function ajaxGetListerContent_CallbackFail(response, b,c){

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
    
    var hBrowsing = document.getElementById('hBrowsing');

    if (tabType == 1){
        hBrowsing.innerHTML = "You are browsing: Latest Photos";
    }
    else if (tabType == 2){
        hBrowsing.innerHTML = "You are browsing: Most Viewed Photos";
    }
    else if (tabType == 3){
        hBrowsing.innerHTML = "You are browsing: Most Discussed Photos";
    }
    else if (tabType == 4) {
        hBrowsing.innerHTML = "You are browsing: Top Rated Photos";
    }
    
    var txtSearch = document.getElementById('ctl00_ContentPlaceHolder1_txtSearch');
    txtSearch.value = '';
    
}
