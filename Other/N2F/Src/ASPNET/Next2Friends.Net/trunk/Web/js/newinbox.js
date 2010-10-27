var inboxPage = 0;
var trashPage = 0;
var sentPage = 0;

var totalInboxPages = 0;
var totalTrashPages = 0;
var totalSentPages = 0;

$(document).ready(function() 
{   
    refreshInbox();
    refreshTrash(); 
    refreshSent();
    createNewMessage();
});

function startInbox() 
{
	setInterval("refreshInbox()",5000)
}

function getInboxMessages(pageNo)
{
    NewInboxPage.GetInboxMessages(pageNo,getInboxMessages_Callback);
}

function getInboxMessages_Callback(response)
{
    if(response!=null && response.value!=null){
        $("#divInbox").children().not("#inboxItemSource").remove();
        var messages = response.value.AjaxMessages;
        inboxPage = response.value.CurrentPage;
        totalInboxPages = response.value.NumberOfPages;
    
        for(var i=0;i<messages.length;i++)
        {
             appendMessage(messages[i],"divInbox",true);
        }
        
        setTopPager(inboxPage, totalInboxPages,"divTopInboxPager");
        setBottomPager(inboxPage, totalInboxPages,"divBottomInboxPager","getInboxMessages");
    }
}

function setBottomPager(pageNo,pageCount,list,func)
{
    var appendHtml = "";
    for( var i = 0 ; i < pageCount; i++ )
    {
        if( i == pageNo )
            appendHtml += "&nbsp;" + (i+1);
        else
            appendHtml += "<a href='javascript:" + func + "(" + i + ")';>" + (i+1) + "</a>";        
    }
    $("#" + list).html("Page: " + appendHtml);
}

function setTopPager(pageNo,pageCount,list)
{   
    $("#" + list).html("Messages: page " + (pageNo + 1) + " of " + pageCount);
}

function getTrashMessages(pageNo)
{
    NewInboxPage.GetTrash(pageNo,getTrashMessages_Callback);
}

function getTrashMessages_Callback(response)
{
    if(response!=null && response.value!=null){
    $("#divTrash").children().not("#inboxItemSource").remove();    
    var messages = response.value.AjaxMessages;
    
    trashPage = response.value.CurrentPage;
    totalTrashPages = response.value.NumberOfPages;
    
    for(var i=0;i<messages.length;i++)
    {
         appendMessage(messages[i],"divTrash",true);
    }    
    
    setTopPager(trashPage, totalTrashPages,"divTopTrashPager");
    setBottomPager(trashPage, totalTrashPages,"divBottomTrashPager","getTrashMessages");
    }
}

function getSentMessages(pageNo)
{
    NewInboxPage.GetSent(pageNo,getSentMessages_Callback);
}

function getSentMessages_Callback(response)
{
    if(response!=null && response.value!=null){
        $("#divSent").children().not("#inboxItemSource").remove();    
        var messages = response.value.AjaxMessages;    

        sentPage = response.value.CurrentPage;
        totalSentPages = response.value.NumberOfPages;
    
        for(var i=0;i<messages.length;i++)
        {
             appendMessage(messages[i],"divSent",false);
        }
        
        setTopPager(sentPage, totalSentPages,"divTopSentPager");
        setBottomPager(sentPage, totalSentPages,"divBottomSentPager","getSentMessages");
    }
}

function appendMessage(msg,dynId,showRead)
{
    var clonedItem = $("#inboxItemSource").clone(true).appendTo($("#" + dynId)); 
    clonedItem.attr("id","inboxItemSource" + msg.WebMessageID);
    clonedItem.css("visibility","visible");
    clonedItem.css("display","block");
    
    var msgItemSource = clonedItem.children("#msgItemSource");
    msgItemSource.attr("id","msgItemSource" + msg.WebMessageID);
    
    msgItemSource.click(function () {
        ajaxOpenMessage($(this).attr("id").replace("msgItemSource",""));
    });
    
    
    var fromItemSource = clonedItem.children().children("#fromItemSource");
    fromItemSource.attr("id","fromItemSource" + msg.WebMessageID);
    
    var statusItemSource = clonedItem.children().children("#statusItemSource");
    statusItemSource.attr("id","statusItemSource" + msg.WebMessageID);
    
    var excerptItemSource = clonedItem.children().children("#excerptItemSource");
    excerptItemSource.attr("id","excerptItemSource" + msg.WebMessageID);
    
    var timeItemSource = clonedItem.children().children("#timeItemSource");
    timeItemSource.attr("id","timeItemSource" + msg.WebMessageID);   
    
    fromItemSource.html("<a class=\"clearfix\" href=\"view.aspx?m=" + msg.WebMemberIDFrom + "\" >" + msg.FromNickName + "</a>");
    
    if( msg.IsRead )
    {        
        clonedItem.removeClass("unread");
        clonedItem.addClass("read");
        statusItemSource.html("&raquo; Read");
    }
    else
    {
        clonedItem.removeClass("read");
        clonedItem.addClass("unread");
        statusItemSource.html("&raquo; New");
    }
    
    if( !showRead )
    {
        statusItemSource.remove();
    }    
    excerptItemSource.html(msg.Body);
    timeItemSource.html(msg.TimeAgo);    
}

function refreshInbox()
{
   getInboxMessages(inboxPage);    
}

function refreshTrash()
{
   getTrashMessages();    
}

function refreshSent()
{
   getSentMessages();    
}

function deleteMessages(){   
    
    var selectedMsg = $("#divInbox").children().children("#chkBox:checked");
    
    var deleteList = new Array();
    var pointer=0;
    
    jQuery.each(selectedMsg, function(i, val) {    
      deleteList[pointer++] = $( this ).parent().attr("id").replace("inboxItemSource","");
    });    
    
    
    if(deleteList.length>0){
        NewInboxPage.DeleteMessages(deleteList, false, deleteMessages_Callback);
    }

}

function deleteMessages_Callback(response){
    
    if(response!=null && response.value!=null){
            refreshInbox();      
    }else{
        alert('Ooops, there was a problem deleting your messages, please try again!');
    }
}

function emptyTrash(){

    var selectedMsg = $("#divTrash").children().children();
    
    var deleteList = new Array();
    var pointer=0;
    
    jQuery.each(selectedMsg, function(i, val) {    
      deleteList[pointer++] = $( this ).parent().attr("id").replace("inboxItemSource","");
    });
    
    if(deleteList.length>0){
        NewInboxPage.DeleteMessages(deleteList, true, emptyTrash_Callback);
    }
}

function emptyTrash_Callback(response){
    
    if(response!=null && response.value!=null){
            refreshTrash();      
    }else{
        alert('Ooops, there was a problem deleting your messages, please try again!');
    }
}

function ajaxOpenMessage(WebMessageID){
    NewInboxPage.OpenMessage(WebMessageID, ajaxOpenMessage_Callback);
}

function ajaxOpenMessage_Callback(response){

    if(response.value!=null){
        var messages = response.value.AjaxMessages;
        
        $("#osMessageList").children().not("#msgViewSource").remove();    
        for(var i=0;i<messages.length;i++)
        {
             openMessageView(messages[i],response);
        }
    }else{
        alert('Ooops, there was a problem opening your message, please try again!');
        error = response.error;
    }
}

function openMessageView(msg,response)
{
    var clonedItem = $("#msgViewSource").clone(true).appendTo($("#osMessageList")); 
    clonedItem.attr("id","msgViewSource" + msg.WebMessageID);    
    
    var msgViewFromSource = $("#msgViewSource" + msg.WebMessageID + " #msgViewFromSource");
    msgViewFromSource.attr("id","msgViewFromSource" + msg.WebMessageID);    
    
    var msgViewTimeSource = $("#msgViewSource" + msg.WebMessageID + " #msgViewTimeSource");
    msgViewTimeSource.attr("id","msgViewTimeSource" + msg.WebMessageID);
    
    var msgViewBodySource = $("#msgViewSource" + msg.WebMessageID + " #msgViewBodySource");
    msgViewBodySource.attr("id","msgViewBodySource" + msg.WebMessageID);
    
    var btnQuickSend = $("#msgViewSource" + msg.WebMessageID + " #btnQuickSend");
    btnQuickSend.attr("id","btnQuickSend" + msg.WebMessageID);
    
    var msgViewReplyVideoSource = $("#msgViewSource" + msg.WebMessageID + " #msgViewReplyVideoSource");
    msgViewReplyVideoSource.attr("id","msgViewReplyVideoSource" + msg.WebMessageID);

    btnQuickSend.click(function () {        
        quickSend(msg.FromNickName,msg.WebMessageID);
    });
    
    msgViewReplyVideoSource.click(function () {
        replyToMessage(msg.FromNickName);
    });

    msgViewFromSource.html(msg.FromNickName);
    msgViewBodySource.html(msg.Body);
    
    var vp = new VMPlayer( "width=420px,height=350px,target=divVMPlayer", response.value.DefaultVideoMessageFile, null, null, null, null );
    vp.writeFlashPlayer();
    
    showElements(2);
    
    scroll(0,0);
}


function newMessageView(response){

    if(response.value != null){
        
        var pCollapseOptions = document.getElementById("pCollapseOptions");
        var NumberOfMessages = response.value.AjaxMessages.length;
        
        if(osMessageList1==null){
            var osMessageList1 = document.getElementById('osMessageList1');
        }
        osMessageList1.innerHTML = '';
        // call the ajax command to retrive the number of the message headers and the first message
        for(var i=0;i<NumberOfMessages;i++){
        
           osMessageList1.innerHTML += response.value.AjaxMessages[i].HTML;
        }

        var vp = new VMPlayer( "width=420px,height=350px,target=divVMPlayer", response.value.DefaultVideoMessageFile, null, null, null, null );
        vp.writeFlashPlayer();
        
        var newHTML = '';
        
        if(NumberOfMessages>10){
            newHTML += '<a href="#" class="collpase_all_message">Collapse all</a>';
            newHTML +=  '<a href="#" class="show_all_message">Show all message ('+NumberOfMessages+')</a>' ;
			newHTML += '<a href="#" class="show_recent_only">Show 10 only</a>';
        }
        
        pCollapseOptions.innerHTML = newHTML;
         
        showElements(2);
        
        scroll(0,0);
        
    }else{
        alert('Ooops, there was a problem opening your message, please try again!');
    }
}

function getNormalizedDigit(x)
{
    if( x <= 9 )
    {
        return '0' + x;
    }
    
    return x;
}

function getBrowserDateTime()
{
    var currBrowserDt = new Date();    
    var dtString = '';
    
    dtString += currBrowserDt.getYear() + '-';
    dtString += getNormalizedDigit(currBrowserDt.getMonth() + 1) + '-';       
    dtString += getNormalizedDigit(currBrowserDt.getDate());  
    
    dtString += getNormalizedDigit(currBrowserDt.getHours()) + ':';
    dtString += getNormalizedDigit(currBrowserDt.getMinutes()) + ':';
    dtString += getNormalizedDigit(currBrowserDt.getSeconds());
    
    return dtString;
}

function quickSend(nickName, webMessageID){
    var txtSendTo = nickName;
    var txtQuickSend = document.getElementById("txtQuickSend");
    var btnQuickSend = document.getElementById("btnQuickSend");
    
    btnQuickSend.disabled = true;
    NewInboxPage.QuickSend(nickName, safeHTML(txtQuickSend.value),getBrowserDateTime(), webMessageID, ajaxquickSend_Callback );
}

function ajaxquickSend_Callback(response){
    
    var btnQuickSend = document.getElementById("btnQuickSend");
    
    if(response.value!=null){
            
        showElements(3);     
    
    }else{
        alert('Ooops, there was a problem sending your message, please try again!');
    }
    
    //enable the send button
    btnQuickSend.disabled = false;
}

function replyToMessage(nickname){
   
    $("#txtSendTo").val(nickname);
    createNewMessage();
}

function createNewMessage(){
   
    var divNewMessage = document.getElementById("divNewMessage");
    var divVMPlayer = document.getElementById("divVMPlayer");
    var txtSendTo = document.getElementById("txtSendTo");
    var txtMessageBody = document.getElementById("txtMessageBody");
  
    if(divNewMessage.style.display != 'none'){
        if(txtSendTo.value!='' || txtMessageBody.value!=''){
            if(confirm('Your message has not been sent.\r\nDiscard message?')){
                txtSendTo.value = '';
                txtMessageBody.value = '';
                return;
            }
        }
    }

    setupVMPlayer();
    
    showElements(1);
}

function setupVMPlayer(){
    NewInboxPage.GetVMToken(getVMToken_Callaback);
}

function getVMToken_Callaback(response){
    
    var token = response.value;
    
    vm = new VMRecorder( "width=420px,height=350px,target=divVMRecorder,bgcolor=#eae9e9", token, null, null, null, null );
    vm.writeFlashPlayer();
}

function ajaxSendMessage(){
    var txtSendTo = document.getElementById("txtSendTo");
    var txtMessageBody = document.getElementById("txtMessageBody");
    var btnSend = document.getElementById("btnSend");
    
    btnSend.disabled = true;
    var VMToken = (vm==null) ? '' : vm.token;    
    NewInboxPage.SendMessage( $("#txtSendTo").val() , safeHTML( $("#txtMessageBody").val() ), VMToken, ajaxSendMessage_Callback );
}

function ajaxSendMessage2(){        
    NewInboxPage.SendMessage("hash", "HELLO", "", ajaxSendMessage_Callback );
}

function ajaxSendMessage_Callback(response){
    
    var txtSendTo = document.getElementById("txtSendTo");
    var txtMessageBody = document.getElementById("txtMessageBody");
    var divNewMessage = document.getElementById("divNewMessage");
    var divVMPlayer = document.getElementById("divVMPlayer");
    var btnSend = document.getElementById("btnSend");

    if(response.value!=null){
        if(response.value==0){
        
            //if(URLRedirect!=null){
            //       window.location = URLRedirect;
            //}else{
                txtSendTo.value = ''
                txtMessageBody.value = ''
                
                showElements(3);
               
                btnSend.disabled = false;
            //}
            
        }else if(response.value==1){
            alert('the Next2Friends member or email address was invalid');
        }else if(response.value==2){
            alert('Unexpected error sending message');
        }
    }else{
        alert('Ooops, there was a problem sending your message, please try again!');
        btnSend.disabled = false;
    }
    

    
}

function ajaxSendMessage2_Callback(response){    
    var btnSend = document.getElementById("btnSend");
    alert(response.value);
    if(response.value!=null){
        if(response.value==0){
        
            if(URLRedirect!=null){
                   window.location = URLRedirect;
            }else{
                $("#txtSendTo").val("");
                $("#txtMessageBody").val("");
                
                showElements(3);
               
                btnSend.disabled = false;
            }
            
        }else if(response.value==1){
            alert('the Next2Friends member or email address was invalid');
        }else if(response.value==2){
            alert('Unexpected error sending message');
        }
    }else{
        alert('Ooops, there was a problem sending your message, please try again!');
        btnSend.disabled = false;
    }
    

    
}

function safeHTML(value){
    value = value.replace(/&/g,'&amp;')  
                .replace(/</g,'&lt;')  
                .replace(/>/g,'&gt;')  
                .replace(/\'/g,'&#39;')  
                .replace(/"/g,'&quot;');
       
    var re_nlchar = '';         
    value = escape(value);

    if(value.indexOf('%0D%0A') > -1){
        re_nlchar = /%0D%0A/g ;
    }else if(value.indexOf('%0A') > -1){
        re_nlchar = /%0A/g ;
    }else if(value.indexOf('%0D') > -1){
        re_nlchar = /%0D/g ;
    }

    return unescape( value.replace(re_nlchar,'<br />') );
}

// selects all the delete checkboxes in the inbox
function selectAllMessages(){

    checkBoxSelection(true, 'divInbox');
}

// unselects all the delete checkboxes in the inbox
function selectNoneMessages(){

    checkBoxSelection(false, 'divInbox');
}

// selects all the delete checkboxes in the inbox
function selectAllTrash(){

    checkBoxSelection(true, 'divTrash');
}

// unselects all the delete checkboxes in the inbox
function selectNoneTrash(){

    checkBoxSelection(false, 'divTrash');
}

function checkBoxSelection(value, list){

    $("#" + list).children().children("#chkBox").attr("checked",value);
}

function hide(list)
{
    $("#" + list).removeClass("current");
}

function show(list)
{
    $("#" + list).addClass("current");    
}

function showElements(show){
    // 1 show new message
    // 2 show message
    // 3 show inbox 
    
    var divNewMessage = document.getElementById("divNewMessage");
    var spanViewMessage = document.getElementById("spanViewMessage");
    var spanInbox = document.getElementById("spanInbox");
    var spanTrash = document.getElementById("spanTrash");
    var spanSent = document.getElementById("spanSent");
    var tabInbox = document.getElementById("tabInbox");  
    var tabNewMessage = document.getElementById("tabNewMessage");  
    var tabTrash = document.getElementById("tabTrash");  
    var tabSent = document.getElementById("tabSent");
    
    if(show==1){
    
        divNewMessage.style.display = 'block';
        spanViewMessage.style.display = 'none';
        spanInbox.style.display = 'none';
        spanTrash.style.display = 'none';
        spanSent.style.display = 'none';
        
        tabInbox.setAttribute('class', 'watch');
        tabNewMessage.setAttribute('class', 'watch current');
        tabTrash.setAttribute('class', 'watch');
        tabSent.setAttribute('class', 'watch');
    
    }else if(show==2){
    
        divNewMessage.style.display = 'none';
        spanViewMessage.style.display = 'block';
        spanInbox.style.display = 'none';
        spanTrash.style.display = 'none';
        spanSent.style.display = 'none';
        
    }else if(show==3){
    
        divNewMessage.style.display = 'none';
        spanViewMessage.style.display = 'none';
        spanInbox.style.display = 'block';
        spanTrash.style.display = 'none';
        spanSent.style.display = 'none';
        
        tabInbox.setAttribute('class', 'watch current');
        tabNewMessage.setAttribute('class', 'watch');
        tabTrash.setAttribute('class', 'watch');
        tabSent.setAttribute('class', 'watch');
    } else if(show==4){
    
        divNewMessage.style.display = 'none';
        spanViewMessage.style.display = 'none';
        spanInbox.style.display = 'none';
        spanTrash.style.display = 'block';
        spanSent.style.display = 'none';
        
        tabInbox.setAttribute('class', 'watch');
        tabNewMessage.setAttribute('class', 'watch');
        tabTrash.setAttribute('class', 'watch current');
        tabSent.setAttribute('class', 'watch');        
    }  
    else if(show==5){
    
        divNewMessage.style.display = 'none';
        spanViewMessage.style.display = 'none';
        spanInbox.style.display = 'none';
        spanTrash.style.display = 'none';
        spanSent.style.display = 'block';
        
        tabInbox.setAttribute('class', 'watch');
        tabNewMessage.setAttribute('class', 'watch');
        tabTrash.setAttribute('class', 'watch');
        tabSent.setAttribute('class', 'watch current');
    }  
}