
function unfriendMember(WebMemberID){
    if(confirm("Are you sure you want to perminantly remove this friend from your network?")){
        FriendPage.UnfriendMember(WebMemberID, unfriendMember_Callback);
    }
}

function unfriendMember_Callback(response){
    
    if(response.value!=null){
        var divFriends = document.getElementById('divFriends');
        var divFriend = document.getElementById('divFriend'+response.value);
        
        divFriends.removeChild(divFriend);
       
    }
}



function unblockMember(WebMemberID){

       FriendPage.UnblockMember(WebMemberID, unblockMember_Callback);

}

function unblockMember_Callback(response){
    
    if(response.value!=null){
        var divFriends = document.getElementById('divFriends');
        var divFriend = document.getElementById('divFriend'+response.value);
        
        divFriends.removeChild(divFriend);
       
    }
}


function addTofriends(webMemberID){
    FriendPage.AddToFriends(webMemberID, addToFriends_Callback);
}

function addToFriends_Callback(response){
    if(response.value!=null){
    
        var spanAddToFriends = document.getElementById('spanAddToFriends'+response.value.WebMemberID);
        spanAddToFriends.innerHTML = "&nbsp;<img src='images/check.gif' />";
    }else {
        alert('Ooops, there was a problem with your request, please try again!');
    }
}
