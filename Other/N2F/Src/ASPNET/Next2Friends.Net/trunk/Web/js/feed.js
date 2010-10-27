function deleteFavourite(favouriteID){
    Feed.DeleteFavourite(favouriteID, deleteFavourite_Callback);
}
function deleteFavourite(objectID,objectType){
    Feed.DeleteFavourite(objectID,objectType, deleteFavourite_Callback);
}

function deleteFavourite_Callback(response){
    if(response.value!=null){
    
      $('#'+response.value).remove();
       
    }else {
        alert('Ooops, there was a problem with your request, please try again!');
    }
}
