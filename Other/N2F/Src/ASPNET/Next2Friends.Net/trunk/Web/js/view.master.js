function changeProfilePhoto(html){
    $('#divChangePhoto').fadeIn('fast');
}

function subscribeToMember(webMemberID, link){
    ViewMaster.SubscribeToMember(webMemberID, subscribeToMember_Callback);
}

function subscribeToMember_Callback(response){

    $('#spanSubscribe').html("&nbsp;<img src='/images/check.gif' />")
}

function addTofriends(webMemberID, link){
    ViewMaster.AddToFriends(webMemberID, addToFriends_Callback);
}

function addToFriends_Callback(response){
    if(response.value!=null){
        $('#spanAddToFriends').html("&nbsp;<img src='/images/check.gif' />");
        $('.add_to_friend').html("Request Sent");
        $('.add_to_friend').attr("href","#");
    }else {
        alert('Ooops, there was a problem with your request, please try again!');
    }
}


function vote(param, WebID, dir){

    var up = $('#vUp');
    var down = $('#vDown');
    
    up.addClass('voted');
    down.addClass('voted');
    up.attr('href','#');
    down.attr('href','#');
    
    ViewMaster.Vote(param, WebID, dir, vote_Callback);
}

function vote_Callback(response){

    if(response.value!=null) {
        //update the vote box and disable the links
        var spanVote = $('#spanVote');
        var currentScore = spanVote.html();
        var newScore  = response.value + parseInt(currentScore);
        spanVote.html(newScore);
    }
}

$(document).click(
function(event){

if(!$(event.target).is('#divSkins') && !$(event.target).is('.btn_change_skin'))
    $(
        "#divSkins").hide();
    }

);

function addToFavourites(ObjectType,WebOjbectID){
    ViewMaster.AddToFavourites(ObjectType,WebOjbectID, addToFavourites_Callback);
}

function addToFavourites_Callback(response){
    if(response.value!=null){
        $('#spanAddFavourites').html("&nbsp;<img src='/images/check.gif' />");
    }else if(response.error!=null){
         alert('Ooops, there was a problem with your request, please try again!');
    }

}