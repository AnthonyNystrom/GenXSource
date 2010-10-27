<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Video.aspx.cs" Inherits="VideoPage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<style>
.subscribeRSSLink1 {
background:transparent url(/images/rss.png) no-repeat scroll 0 2px;
padding:0 0 0 20px;
width:60px;
float:left;
}
</style>
<script type="text/javascript" src="/js/video.js"></script>
<script type="text/javascript" src="/lib/popup.js?c=1"></script>

<!-- middle start -->
	<div id="middle" class="no_subnav clearfix">

			<!-- lister left start -->
			<div class="lister_leftcol">
			
				<div class="lister_top clearfix">

					<ul class="lister_tabnav">
						<li><a id="contentTab1" <%=CurrentTab1 %> href="/video/?<%=pageUrl %>&to=1">Latest</a></li>
						<li><a id="contentTab2" <%=CurrentTab2 %> href="/video/?<%=pageUrl %>&to=2">Most Viewed</a></li>
						<li><a id="contentTab3" <%=CurrentTab3 %> href="/video/?<%=pageUrl %>&to=3">Most Discussed</a></li>
						<li><a id="contentTab4" <%=CurrentTab4 %> href="/video/?<%=pageUrl %>&to=4">Top Rated</a></li>
					</ul>
				</div>
				
				<div class="lister_middle clearfix">				
					<div class="lister_topbar clearfix">
					
						<span id="hBrowsing2">
						
							<div class="search_videos">
							
								</div>
						    
						    <a class="subscribeRSSLink1" style="float:left" href="/rss.aspx?feed=video">subscribe</a>    
						    
							<div class="right">							    
								<%=DefaultHTMLPager %>
								</div>

						</span>							
						<div class="clear"></div>

					</div>

					<ul class="videos_list clearfix" id="ulContentLister">
						<%=DefaultHTMLLister%>
					</ul>
					
					<span style="text-align:right;"><%=DefaultHTMLPager %></span>

				</div>

			</div>
			<!-- lister left end -->

			<!-- profile right start -->
			<div class="lister_rightcol">
				

				<h4>&nbsp</h4>
				<ul class="sidebar_nav">
				    <%=CategoryHTML %>
				</ul>


				<p class="tag_cloud">

						<%=TagCloudHTML %>
				</p>
			</div>

	</div>
	
	<script type="text/javascript">
	function displayMiniVideo(webVideoID,title){
        var html = '<iframe src="/MiniVideoPage.aspx?v='+webVideoID+'" style="border:0px" frameborder="0" scrolling="no" width="480" height="285"></iframe>';   
        npopup(title,html,525,285);
    }
	</script>
	
</asp:Content>

