function setFriendStatus(WebFriendRequestID, value){
    var go = true;
    //if(!value){
    //    if(!confirm("Are you sure you want to i?")){
    //        go = false;
    //    }
    //}
    
    if(go){
        FriendRequestPage.SetFriendStatus(WebFriendRequestID, value, setFriendStatus_Callback);
    }
}

function setFriendStatus_Callback(response,args){
    if(response.value!=null){
        var id = 'divFriendRequest' + args.args.WebFriendRequestID;
        var item = document.getElementById(id);
        var lister = document.getElementById('middle');
        
        lister.removeChild(item);
        
        $('#cntFR').html("(" + response.value + ")");
        
    }else{
         alert('Ooops, there was a problem with your request, please try again!');
    }
}

function unfriendMember(WebMemberID){
    if(confirm("Are you sure you want to perminantly remove this friend from your network?")){
        FriendRequestPage.UnfriendMember(WebMemberID, unfriendMember_Callback);
    }
}

function unfriendMember_Callback(response){
    
    if(response.value!=null){
        var divFriends = document.getElementById('divFriends');
        var divFriend = document.getElementById('divFriend'+response.value);
        
        divFriends.removeChild(divFriend);
       
    }
}
