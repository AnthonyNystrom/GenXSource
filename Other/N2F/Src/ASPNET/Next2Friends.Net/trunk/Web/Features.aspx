<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Features.aspx.cs" Inherits="Features" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<link rel="stylesheet" type="text/css" href="/jcarousel-css/jquery.jcarousel.css" />
<link rel="stylesheet" type="text/css" href="/jcarousel-css/skin.css" />
<script type="text/javascript" src="/lib/jcarousel.js"></script>
<script type="text/javascript" src="/lib/popup.js"></script>


<script type="text/javascript">

$(document).ready(function(){
						   
    $('.featuresblock').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback,
        // This tells jCarousel NOT to autobuild prev/next buttons
        buttonNextHTML: null,
        buttonPrevHTML: null
    });
    
    
}); //close doc ready

function mycarousel_initCallback(carousel) {

    $('.jcarousel-control a').click(function(){
        carousel.scroll($.jcarousel.intval($(this).text()));
        return false;
    });
    
    $('.nextprev').css('visibility','visible');

    $('.slide1').click(function(){
        carousel.scroll(1);
        return false;
    });
    $('.slide2').click(function(){
        carousel.scroll(2);
        return false;
    });
    $('.slide3').click(function(){
        carousel.scroll(3);
        return false;
    });
    $('.slide4').click(function(){
        carousel.scroll(4);
        return false;
    });
    $('.slide5').click(function(){
        carousel.scroll(5);
        return false;
    });

    $('.feature-next').click(function(){
        carousel.next();
        return false;
    });

    $('.feature-prev').click(function(){
        carousel.prev();
        return false;
    });
};

function livepopup(){
    var html = '<iframe src="/MiniVideoPage.aspx?v=MDg5ZGY3&videoonly=1" style="border:0px" frameborder="0" scrolling="no" width="380" height="285"></iframe>';   
    npopup('Live',html,415,285);
}
</script>


<!-- middle start -->
	<div id="middle" class="clearfix">
		<!-- page start -->
		<div class="clearfix">
                	<!-- features carousel -->
		<div class="features">
	
		
			<div id="featuresul" style="width: 830px;overflow: hidden;">
				<ul class="featuresblock">
					<li>
						<img src="/images/next2friends-live-mobile-video-broadcasting.jpg" alt="Next2Friends Live Mobile Video Broadcasting" />
						<h2>Live Mobile Broadcasting</h2>
						<p>Next2Friends Live allows you to broadcast live events from any capable mobile device or webcam for your family, 
						friends or the entire Next2Friends community to watch in real-time!</p>
						 <p>Videos are streamed and saved to your Next2Friends profile where they can then be embedded into your blog or website.  
						 Next2Friends Live is the first to offer truly live-streamed video from a mobile device making it possible to enjoy a level 
						 of real-time interaction unavailable anywhere else!  
						 </p>
						 <p><a href="javascript:livepopup()">View video</a></p>
						<p class="nextprev"><a href="#" class="feature-next">Next</a></p>
					</li>
					<li>
						<img src="/images/next2friends-snapup.jpg" alt="Next2Friends Snapup" />
						<h2>Snapup</h2>
						<p>Easily snap and upload a photo to the web in just two clicks!  Never transfer photos from your phone to your computer again! 
						Next2Friends automatically sends a copy of your Snap Up photo to your email. Photos are auto-organized by date in your profile and appear in your friends’ Dashboard.
						<br /><br />
						 The Dashboard is a great new feature of the Next2Friends Social Suite.  Get all the latest news and updates from your friends delivered right to your phone!   </p>
						<p class="nextprev"><a href="#" class="feature-prev">Previous</a> <a href="#" class="feature-next">Next</a></p>
					</li>
					<li>
						<img src="/images/next2friends-ask.jpg" alt="Next2Friends ask" />
						<h2>Ask</h2>
						<p>Snap a photo with your phone, type in a question and beam it to all your friends to receive and respond to from either their 
						mobile phones or their computers.  Get real time answers when it matters the most!</p>
						<p>Ask is another part of the dynamic new Social Suite.  With the Next2Friends Social Suite you can also comment on your friends’ latest photos and videos right from your phone!    </p>
						<p class="nextprev"><a href="#" class="feature-prev">Previous</a> <a href="#" class="feature-next">Next</a></p>
					</li>
					<li>
						<img src="/images/next2friends-tag.jpg" alt="Next2Friends Tag" />
						<h2>Proximity Tagging</h2>
						<p>Build a network of friends out of the people you pass everyday!  Next2Friends Tag extends your profile into physical space by sensing (within 30 ft) and automatically suggesting others with whom you have something in common.</p>
                        <p>Now do even more with the Next2Friends Social Suite!  Read and respond to emails, update your status, and adjust your settings, all from your phone!    </p>
						
						<p class="nextprev"><a href="#" class="feature-prev">Previous</a> </p>
					</li>
				</ul>	
			</div>		
			
			<div class="controlbar">
				<ul>
					<li><a href="#" class="slide1">Live Mobile Broadcasting</a></li>
					<li><a href="#" class="slide2">Snapup</a></li>
					<li><a href="#" class="slide3">Ask</a></li>
					<li><a href="#" class="slide4">Proximity Tagging</a></li>
				</ul>
				<a href="/download"><img src="/images/features-download.gif" alt="Download" /></a>
			</div>
		</div>
		<!-- /features carousel -->		
		</div>
		<!-- page end -->

	</div>
	
	
</asp:Content>

