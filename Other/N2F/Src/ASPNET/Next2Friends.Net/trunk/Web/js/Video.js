var cachedListerContent = new Array();
var cachedPager = new Array();

function ajaxGetListerContent(tabType){

    updateCurrentTab(tabType);
    
    if(cachedListerContent[tabType]==null){
        VideoPage.GetListerContent(tabType+0, ajaxGetListerContent_Callback)
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
    //document.getElementById('pPageNav').innerHTML = pagerHTML;    
}

function updateCurrentTab(tabType){
    document.getElementById('contentTab1').className  = '';
    document.getElementById('contentTab2').className  = '';
    document.getElementById('contentTab3').className  = '';
    document.getElementById('contentTab4').className  = '';
    
    document.getElementById('contentTab'+tabType).className  = 'current';
        
    //var txtSearch = document.getElementById('ctl00_ContentPlaceHolder1_txtSearch');
    //txtSearch.value = '';
    
}
