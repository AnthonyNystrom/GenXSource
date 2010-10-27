var inboxPage = 0;
var trashPage = 0;
var sentPage = 0;
var xmlDoc = null;

var totalInboxPages = 0;
var totalTrashPages = 0;
var totalSentPages = 0;

var showingTab = -1;
var URLRedirect = null;

var monthNames = new Array(7)
monthNames[0] = "Jan";
monthNames[1] = "Feb";
monthNames[2] = "Mar";
monthNames[3] = "Apr";
monthNames[4] = "May";
monthNames[5] = "Jun";
monthNames[6] = "Jul";
monthNames[7] = "Aug";
monthNames[8] = "Sep";
monthNames[9] = "Oct";
monthNames[10] = "Nov";
monthNames[11] = "Dec";

$(document).ready(function() 
{   
    if( PassKey != '' )
    {
        ajaxOpenMessage(WMsgID,PassKey);
    }
    else
    {
        loadContacts();    
        refreshInbox();
        setInterval("checkNewMessages()",10000)
        //refreshTrash(); 
        //refreshSent();
        
        
        createNewMessage(true);
        var retCode = handleSpecialRequests();
        
        if( retCode == false )
        {
            createNewMessage(true);
            displayInbox();    
        }
    }
});


function handleSpecialRequests()
{
    if (InitialSendValue == "True")
    {
        createNewMessage();
        var txtSendTo = document.getElementById('txtSendTo');        
        txtSendTo.value = InitialSendNickName;
        
        return true;
    }   
    else if (InitialForwardValue == "True")
    {
        var txtMessageBody = document.getElementById('txtMessageBody');
        txtMessageBody.value = InitialForwardText.replace("&#39;","'");        
        return true;
    }
    
    return false;
}

function getInboxMessages(pageNo)
{
    InboxPage.GetInboxMessages(pageNo,getInboxMessages_Callback);
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
            appendMessage(messages[i],"divInbox",1,true);
        }
        
        try
        {
            $.timer(100, function (timer) {
                $("#divInbox").fadeIn('fast');
                timer.stop();
            });
        }catch(ex)
        {
            $("#divInbox").fadeIn('fast');
        }
    
        setTopPager(inboxPage, totalInboxPages,"divTopInboxPager");
        setBottomPager(inboxPage, totalInboxPages,"divBottomInboxPager","getInboxMessages");
    }
}

function checkNewMessages(){
    InboxPage.GetNewMessages(checkNewMessages_Callback);
}

function checkNewMessages_Callback(response){

    if(response!=null && response.value!=null){
    
        var messages = response.value;
        
        for(var i=messages.length - 1; i>=0 ;i--)
        {   
             appendMessage(messages[i],"divInbox",1,false);
        }
       /* 
        var spanInboxCount = document.getElementById('spanInboxCount');
        var currentnumber = parseInt(spanInboxCount.innerHTML);
        currentnumber += response.value.length;
       //currentnumber= response.value.length;
        spanInboxCount.innerHTML = currentnumber;
        //spanInboxCount.innerHTML=spanInboxCount.innerHTML+spanInboxCount.innerHTML;
        */
        $('#spanInboxCount').text(parseInt($('#spanInboxCount').text())+response.value.length);
        
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
    InboxPage.GetTrash(pageNo,getTrashMessages_Callback);
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
         appendMessage(messages[i],"divTrash",3,true);
    }
    
    try
    {
        $.timer(100, function (timer) {
            $("#divTrash").fadeIn('fast');
            timer.stop();
        });
    }
    catch(ex)
    {
        $("#divTrash").fadeIn('fast');
    }
    
    setTopPager(trashPage, totalTrashPages,"divTopTrashPager");
    setBottomPager(trashPage, totalTrashPages,"divBottomTrashPager","getTrashMessages");
    }
}

function getSentMessages(pageNo)
{
    InboxPage.GetSent(pageNo,getSentMessages_Callback);
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
             appendMessage(messages[i],"divSent",2,true);
        }
        
        try
        {
            $.timer(100, function (timer) {
                $("#divSent").fadeIn('fast');
                timer.stop();
            });
        }
        catch(ex)
        {
            $("#divSent").fadeIn('fast');
        }
        
        setTopPager(sentPage, totalSentPages,"divTopSentPager");
        setBottomPager(sentPage, totalSentPages,"divBottomSentPager","getSentMessages");
    }
}

function test()
{
    alert('test');
}

function appendMessage(msg,dynId,messageTab,append)
{
    var clonedItem = null;
    if( append )
        clonedItem = $("#inboxItemSource").clone(true).appendTo($("#" + dynId)); 
    else
        clonedItem = $("#inboxItemSource").clone(true).prependTo($("#" + dynId)); 
    

    
    clonedItem.attr("id","inboxItemSource" + msg.WebMessageID);
    clonedItem.attr("trashtype",msg.TrashType);
    clonedItem.css("visibility","visible");
    clonedItem.css("display","block");
    
    var msgItemSource = clonedItem.children("#msgItemSource");
    msgItemSource.attr("id","msgItemSource" + msg.WebMessageID);
    
    
    if( msg.TrashType == 2 && dynId != 'divInbox' )
    {        
        msgItemSource.click(function () {
            ajaxOpenSentMessage($(this).attr("id").replace("msgItemSource",""));
        });        
    }
    else if(msg.TrashType == 1 || dynId == 'divInbox')
    {
        msgItemSource.click(function () {
            ajaxOpenMessage($(this).attr("id").replace("msgItemSource",""));
        });
    }    
    
    var fromItemSource = clonedItem.children().children("#fromItemSource");
    fromItemSource.attr("id","fromItemSource" + msg.WebMessageID);
    
    fromItemSource.click(function (e) {        
        dmp(msg.FromNickName,msg.WebMemberIDFrom);
        e.stopPropagation();
        e.preventDefault();
    });
    
    
    var statusItemSource = clonedItem.children().children("#statusItemSource");
    statusItemSource.attr("id","statusItemSource" + msg.WebMessageID);
    
    var excerptItemSource = clonedItem.children().children("#excerptItemSource");
    excerptItemSource.attr("id","excerptItemSource" + msg.WebMessageID);
    
    var timeItemSource = clonedItem.children().children("#timeItemSource");
    timeItemSource.attr("id","timeItemSource" + msg.WebMessageID);   
    
    //fromItemSource.html("<a class=\"clearfix\" href=\"view.aspx?m=" + msg.WebMemberIDFrom + "\" >" + msg.FromNickName + "</a>");
    fromItemSource.html(msg.FromNickName);
    
    if( msg.IsRead || messageTab == 2 || messageTab == 3 )
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
    
    if( messageTab == 2 || messageTab == 3  )
    {
        statusItemSource.remove();
    }    
    
    excerptItemSource.html(msg.Body);
    timeItemSource.html( timeToDisplay(msg.DTSent) );
}

function timeToDisplay(dtSent)
{
    var browserTime = new Date();
    var msgTime = new Date();
    msgTime.setTime(dtSent);
    
    var trMsgTime = new Date(Date.UTC(msgTime.getFullYear(),msgTime.getMonth(),msgTime.getDate(),msgTime.getHours(),msgTime.getMinutes(),msgTime.getSeconds()) + browserTime.getTimezoneOffset() * 60 * 1000);
    
    var hrs = trMsgTime.getHours();
    var mins = trMsgTime.getMinutes() + '';
    var period = 'AM';
    
    if( hrs >= 12 )
    {
        hrs = hrs % 12;
        period = 'PM';
    }
    
    if( hrs == 0 )
        hrs = 12;
            
    hrs = hrs + '';              
        
    if( mins.length == 1 )
        mins = '0' + mins;        

    if(trMsgTime.getFullYear() == browserTime.getFullYear() && 
        trMsgTime.getMonth() == browserTime.getMonth() && 
        trMsgTime.getDay() == browserTime.getDay() )
    {   
        return hrs + ':' + mins + ' ' + period;
    }
    else
    {
        return monthNames[trMsgTime.getMonth()] + " "  + (trMsgTime.getDate()) + ", "+ hrs + ':' + mins + ' ' + period;
    }
    
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
        InboxPage.DeleteMessages(deleteList, false, deleteMessages_Callback);
    }
}

function deleteMessages_Callback(response){
    
    if(response!=null && response.value!=null){
            refreshInbox();      
    }else{
        alert('Ooops, there was a problem deleting your messages, please try again!');
    }
}

function deleteSent(){   
    
    var selectedMsg = $("#divSent").children().children("#chkBox:checked");
    
    var deleteList = new Array();
    var pointer=0;
    
    jQuery.each(selectedMsg, function(i, val) {    
      deleteList[pointer++] = $( this ).parent().attr("id").replace("inboxItemSource","");
    });    
    
    
    if(deleteList.length>0){
        InboxPage.DeleteSentMessages(deleteList, false, deleteSent_Callback);
    }
}

function deleteSent_Callback(response){
    
    if(response!=null && response.value!=null){
            refreshSent();      
    }else{
        alert('Ooops, there was a problem deleting your messages, please try again!');
    }
}

function emptyTrash(){

    var selectedMsg = $("#divTrash").children().children("#chkBox:checked");
    
    var deleteList = new Array();
    var sentDeleteList = new Array();
    var pointer=0;
    var sentPointer=0;
    
    jQuery.each(selectedMsg, function(i, val) {    
      var trashItem = $( this ).parent();
      var trashType = trashItem.attr("trashtype");
      
      if( trashType == "2")
        sentDeleteList[sentPointer++] = trashItem.attr("id").replace("inboxItemSource","");        
      else
        deleteList[pointer++] = trashItem.attr("id").replace("inboxItemSource","");
        
    });
    
    if(deleteList.length>0){
        InboxPage.DeleteMessages(deleteList, true, emptyTrash_Callback);
    }
    
    if( sentDeleteList.length > 0){
        InboxPage.DeleteSentMessages(sentDeleteList, true, emptyTrash_Callback);
    }
}

function emptyTrash_Callback(response){
    
    if(response!=null && response.value!=null){
            refreshTrash();      
    }else{
        alert('Ooops, there was a problem deleting your messages, please try again!');
    }
}

function ajaxOpenSentMessage(WebMessageID){
    InboxPage.OpenSentMessage(WebMessageID, ajaxOpenSentMessage_Callback);
}

function ajaxOpenSentMessage_Callback(response){

    if(response.value!=null){
        var messages = response.value.AjaxMessages;
        
        $("#osMessageList1").children().not("#msgViewSource").remove();    
        for(var i=0;i<messages.length;i++)
        {
            openMessageView(messages[i],response);
        }
        
    }else{
        alert('Ooops, there was a problem opening your message, please try again!');
        error = response.error;
    }
}

function ajaxOpenMessage(WebMessageID){
    InboxPage.OpenMessage(WebMessageID,PassKey, ajaxOpenMessage_Callback);
}

function ajaxOpenMessage_Callback(response){

    if(response.value!=null){
        var messages = response.value.AjaxMessages;
        
        $("#osMessageList1").children().not("#msgViewSource").remove();    
        for(var i=0;i<messages.length;i++)
        {
            openMessageView(messages[i],response);
        }
        
        //var spanInboxCount = document.getElementById('spanInboxCount');
        //spanInboxCount.innerHTML = response.value.NumberOfNewMessages;
        $('#spanInboxCount').text(response.value.NumberOfNewMessages);
        
        
    }else{
        alert('Ooops, there was a problem opening your message, please try again!');
        error = response.error;
    }
}

function openMessageView(msg,response)
{
    var defaultMessageId = response.value.DefaultWebMessageID;
    var clonedItem = $("#msgViewSource").clone(true).appendTo($("#osMessageList1")); 
    clonedItem.attr("id","msgViewSource" + msg.WebMessageID);    
    clonedItem.css("visibility","visible");
    clonedItem.css("display","block");
    
    if( defaultMessageId != msg.WebMessageID )
    {
        $("#msgViewSource" + msg.WebMessageID + " .message_body").hide();      
    }
    
    var msgViewFromSource = $("#msgViewSource" + msg.WebMessageID + " #msgViewFromSource");
    msgViewFromSource.attr("id","msgViewFromSource" + msg.WebMessageID);
    
    var msgViewNickSource = $("#msgViewSource" + msg.WebMessageID + " #msgViewNickSource");
    msgViewNickSource.attr("id","msgViewNickSource" + msg.WebMessageID);
    
    var msgViewTimeSource = $("#msgViewSource" + msg.WebMessageID + " #msgViewTimeSource");
    msgViewTimeSource.attr("id","msgViewTimeSource" + msg.WebMessageID);
    
    var msgViewBodySource = $("#msgViewSource" + msg.WebMessageID + " #msgViewBodySource");
    msgViewBodySource.attr("id","msgViewBodySource" + msg.WebMessageID);
    
    var btnQuickSend = $("#msgViewSource" + msg.WebMessageID + " #btnQuickSend");
    btnQuickSend.attr("id","btnQuickSend" + msg.WebMessageID);
    
    var txtQuickSend = $("#msgViewSource" + msg.WebMessageID + " #txtQuickSend");
    txtQuickSend.attr("id","txtQuickSend" + msg.WebMessageID);
    
    var msgViewReplyVideoSource = $("#msgViewSource" + msg.WebMessageID + " #msgViewReplyVideoSource");
    msgViewReplyVideoSource.attr("id","msgViewReplyVideoSource" + msg.WebMessageID);

    btnQuickSend.click(function () {        
        quickSend(msg.FromNickName,msg.WebMessageID);
    });
    
    msgViewReplyVideoSource.click(function () {
        replyToMessage(msg.FromNickName);
    });

    //msgViewFromSource.html('<a class="profileview" href="javascript:dmp(\''+msg.FromNickName+'\',\''+msg.WebMemberIDFrom+'\');">'+msg.FromNickName+'</a>');
    msgViewNickSource.html(msg.FromNickName);
    
    msgViewNickSource.click(function (e) {  
        dmp(msg.FromNickName,msg.WebMemberIDFrom);
        e.stopPropagation();
        e.preventDefault();
    });
    
    msgViewBodySource.html(msg.Body);
    msgViewTimeSource.html( timeToDisplay(msg.DTSent) );
    
    var vp = new VMPlayer( "width=420px,height=350px,target=divVMPlayer", response.value.DefaultVideoMessageFile, null, null, null, null );
    vp.writeFlashPlayer();
    
    showElements(2);
    
    scroll(0,0);
    
    showMessageAsRead(msg.WebMessageID);
}

function showMessageAsRead(WebMessageID)
{
    var inboxRow = $("#inboxItemSource" + WebMessageID);     
    inboxRow.removeClass("unread");
    inboxRow.addClass("read");
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
    var btnQuickSend = document.getElementById("btnQuickSend" + webMessageID);    
    btnQuickSend.disabled = true;
    InboxPage.QuickSend(nickName, safeHTML($("#txtQuickSend" + webMessageID).val() ), webMessageID, ajaxquickSend_Callback );
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

function createNewMessage(noShow){
   
    noShow = noShow == null ? false : noShow ;
    
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
    
    if( !noShow )
        showElements(1);
}

function setupVMPlayer(){
    InboxPage.GetVMToken(getVMToken_Callaback);
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
    InboxPage.SendMessage( $("#txtSendTo").val() , safeHTML( $("#txtMessageBody").val() ), VMToken, ajaxSendMessage_Callback );
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
            btnSend.disabled = false;
        }else if(response.value==2){
            alert('Unexpected error sending message');
            btnSend.disabled = false;
        }
        else if(response.value==3){
            alert('Messages to external email addresses must contain a video attachment');
            btnSend.disabled = false;
        }
    }else{
        alert(response.error.Message);
        alert('Ooops, there was a problem sending your message, please try again!');
        btnSend.disabled = false;
    }
    

    
}

function ajaxSendMessage2_Callback(response){    
    var btnSend = document.getElementById("btnSend");
    //alert(response.value);
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

// selects all the delete checkboxes in the inbox
function selectAllSent(){

    checkBoxSelection(true, 'divSent');
}

// unselects all the delete checkboxes in the inbox
function selectNoneSent(){

    checkBoxSelection(false, 'divSent');
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

function displayInbox()
{
    $("#divInbox").hide();
            
    if( showingTab != 3 )
        refreshInbox();
        
    showElements(3);
}

function displayTrash()
{  
    $("#divTrash").hide();
     
    if( showingTab != 4 )
        refreshTrash();
        
    showElements(4);
}

function displaySent()
{
    $("#divSent").hide();
    
    if( showingTab != 5 )
        refreshSent();
        
    showElements(5);
}


function showElements(show){
    // 1 show new message
    // 2 show message
    // 3 show inbox 
    
    showingTab = show;
    
    var divNewMessage = $("#divNewMessage");
    var spanViewMessage = $("#spanViewMessage");
    var spanInbox = $("#spanInbox");
    var spanTrash = $("#spanTrash");
    var spanSent = $("#spanSent");
    var tabInbox = $("#tabInbox");  
    var tabNewMessage = $("#tabNewMessage");  
    var tabTrash = $("#tabTrash");  
    var tabSent = $("#tabSent");
    
    if(show==1){
    
        divNewMessage.css("display","block");
        spanViewMessage.css("display","none");
        spanInbox.css("display","none");
        spanTrash.css("display","none");
        spanSent.css("display","none");
        
        tabInbox.removeClass("current");        
        tabNewMessage.addClass("current");
        tabTrash.removeClass("current");
        tabSent.removeClass("current");
    
    }else if(show==2){
    
        divNewMessage.css("display","none");
        spanViewMessage.css("display","block");
        spanInbox.css("display","none");
        spanTrash.css("display","none");
        spanSent.css("display","none");
        
        //tabInbox.removeClass("current");        
        //tabNewMessage.addClass("current");
        //tabTrash.removeClass("current");
        //tabSent.removeClass("current");
        
    }else if(show==3){
    
        divNewMessage.css("display","none");
        spanViewMessage.css("display","none");
        spanInbox.css("display","block");
        spanTrash.css("display","none");
        spanSent.css("display","none");
        
        tabInbox.addClass("current");        
        tabNewMessage.removeClass("current");
        tabTrash.removeClass("current");
        tabSent.removeClass("current");
    } else if(show==4){
    
        divNewMessage.css("display","none");
        spanViewMessage.css("display","none");
        spanInbox.css("display","none");
        spanTrash.css("display","block");
        spanSent.css("display","none");
        
        tabInbox.removeClass("current");        
        tabNewMessage.removeClass("current");
        tabTrash.addClass("current");
        tabSent.removeClass("current");       
    }  
    else if(show==5){
    
        divNewMessage.css("display","none");
        spanViewMessage.css("display","none");
        spanInbox.css("display","none");
        spanTrash.css("display","none");
        spanSent.css("display","block");
        
        tabInbox.removeClass("current");        
        tabNewMessage.removeClass("current");
        tabTrash.removeClass("current");
        tabSent.addClass("current");
    }  
}


function loadContacts(){
    	xmlDoc=InboxPage.GetContacts(loadContacts_Callback);
}

function loadContacts_Callback(response){

    if(response.value!=null){
    
        if (window.ActiveXObject)
         {
              xmlDoc=new ActiveXObject("Microsoft.XMLDOM");
              xmlDoc.async="false";
              xmlDoc.loadXML(response.value);
        }
        // code for Mozilla, Firefox, Opera, etc.
        else
        {
              var parser = new DOMParser();
              xmlDoc = parser.parseFromString(response.value, "text/xml");
        }
	}
}

function searchIndex(e) { 

    var searchTerm = document.getElementById('txtSendTo').value;
    var keynum = 0;
    var keychar
    var numcheck

    if(window.event) // IE
    {
        keynum = e.keyCode
    }
    else if(e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which
    }
    
    if (keynum == 38){
        // up
        
    }
    if(keynum == 40){
        //down
        
    }

	var allitems = xmlDoc.getElementsByTagName("Friend");
	results = new Array;
	var len = searchTerm.length;

    if(searchTerm != ''){
   
		for (var i=0;i<allitems.length;i++) {
		
			if(allitems[i].lastChild!=null){
			
			    var name = allitems[i].lastChild.nodeValue.toLowerCase();
			    
			    if(name.substring(0,len) == searchTerm.toLowerCase()){
		            results.push(allitems[i]);
			    }
			}
		}
		
	}

	showPanel(results);
}

function showPanel(results) {

    var autoComplete = document.getElementById("Autocomplete");
    autoComplete.innerHTML = '';
    
	if (results.length > 0) {
		
		for (var i=0;i<results.length;i++) {

		    autoComplete.innerHTML += '<div class="autoCompleteItem" onclick="setAddress(\'' + results[i].lastChild.nodeValue + '\');">&nbsp;&nbsp;&nbsp;&nbsp;' + results[i].lastChild.nodeValue + '</div>';

		}
		autoComplete.style.display = 'block';
	}else{
	    autoComplete.style.display = 'none';
	}
}

function setAddress(value){

    var autoComplete = document.getElementById("Autocomplete");
    autoComplete.style.display = 'none';
    var txtSendTo = document.getElementById("txtSendTo");
    txtSendTo.value = value;    
}

function hideAutocomplete(){
    
    var autoComplete = document.getElementById("Autocomplete");
    autoComplete.style.display = 'none';
}


function dmp(fullName,webMemberID){
     npopup(fullName,"<div id='divProfileHTML'>Loading profile</div>",535,115);
     InboxPage.GetMiniProfile(webMemberID,displayMiniProfile_callback);
}

function displayMiniProfile_callback(response){
    if(response.error==null){ 
        $('#divProfileHTML').html(response.value)
    }
}
