

  $(document).ready(function() {
      $("a")( 
          function() {
              var href = $(this).attr('href');
              var a1 = href.split("DURL=");
              if (a1 != href) {
                  window.status = a1[1];
              } 
          }
      );
  });


function attachJQueryListeners(){


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
	

	//toggle delete_this class
	$(".delete_this .checkbox").click(function(){
		$(this).parents("p").toggleClass("checked");
	});
}
