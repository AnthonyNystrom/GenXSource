//dropdown header menu
$(document).ready(function(){
			$("#header_nav li").hover(
				function(){ $("ul", this).fadeIn("fast"); }, 
				function() { } 
			);
	  	if (document.all) {
				$("#header_nav li").hoverClass ("sfHover");
			}
	  });
	  
		$.fn.hoverClass = function(c) {
			return this.each(function(){
				$(this).hover( 
					function() { $(this).addClass(c);  },
					function() { $(this).removeClass(c); }
				);
			});
};	  

$(document).ready(function(){
	
	// Commented by Hamid
	// Moved inline into Main.Master					   
	//toggle login box
	//$(".display_login").click(function(){
	//	$("#header_login").toggle();
	//});

	//display upload hover text
	$("#upload a").hover(function() {
		$(this).find("span").show();
	}, function() {
		$(this).find("span").hide();
	});
	
	//friend request buttons
	$(".approve, .decline").click(function(){
		$(this).parents(".friend_list").slideUp();
	});


	//toggle .profile box
	$(".collapsible").click(function(){
		$(this).siblings(".collapsible_div").slideToggle(800);
		$(this).toggleClass("collapsed");
	});


	//hide message after first
	$(".message_list .message_body:gt(0)").hide();
	
	//hide message after 9
	$(".message_list li:gt(9)").hide();

	
	//toggle message_body
	$(".message_head").click(function(){
		$(this).next(".message_body").slideToggle(500);
	});

	$(".rotate_left").click(function(){
		$(".edit_thumb img").hide();
	});

	//collapse all message
	$(".collpase_all_message").click(function(){
		$(".message_body").slideUp(500);
	});

	//show all message
	$(".show_all_message").click(function(){
		$(this).hide();
		$(".show_recent_only").show();
		$(".message_list li:gt(9)").slideDown();
	});

	//show recent only
	$(".show_recent_only").click(function(){
		$(this).hide();
		$(".show_all_message").show();
		$(".message_list li:gt(9)").slideUp();
	});

	//toggle quick reply
	$(".quick_reply").click(function(){
		$(".quickreply_wrap").slideToggle(500);
		$(this).toggleClass("active");
	});
	
	//rotate left
	$(".rotate_left").click(function(){
		$(this).parents(".edit_item_left").find(".edit_thumb img").rotateLeft(); return false;
	});

	//rotate right
	$(".rotate_right").click(function(){
		$(this).parents(".edit_item_left").find(".edit_thumb img").rotateRight(); return false;
	});

	//toggle delete_this class
	$(".delete_this .checkbox").click(function(){
		$(this).parents("p").toggleClass("checked");
	});

	//toggle gallery category
	$(".btn_edit_gallery").click(function(){
		$(this).toggleClass("active");
		$(".category_list").slideToggle();
	});
	//toggle add new
	$(".btn_add_new").click(function(){
		$(".add_new").slideToggle();
	});
	
	//toggle edit
	$(".actions .edit").click(function(){
		$(this).parents(".actions").next(".edit_cat_item").slideToggle();
	});

	//toggle advance friend search	
	$(".btn_search_advance").click(function(){
		$(this).toggleClass("active");
		$(this).next(".advance_friend_search").slideToggle();
	});

	$(".profile_vid_list li:even").addClass("clear");

}); //close doc ready