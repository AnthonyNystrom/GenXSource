$(document).ready(function(){
						   
	$('#box p').css({ opacity: "0.8" });
	$('#box p').hide();
	
	//fade
	$('#box .ipod').mouseover(function() {
		$('.ipod-specs').show();
	});
	$('#box .ipod').mouseout(function() {
		$('.ipod-specs').hide();
	});

	$('#box .laptop').mouseover(function() {
		$('.laptop-specs').show();
	});
	$('#box .laptop').mouseout(function() {
		$('.laptop-specs').hide();
	});

	//open referbox
	$('#referbox .top').click(function(){
		$('#referbox .middle').slideToggle(500);
	});
	
}); //close doc ready