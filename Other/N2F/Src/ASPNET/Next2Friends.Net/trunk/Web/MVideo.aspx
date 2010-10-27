<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="MVideo.aspx.cs" Inherits="MVideo" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/View.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftColContentHolder" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftUpperContentHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="RightColContentHolder" Runat="Server">
<script type="text/javascript" src="/lib/popup.js"></script>

<style>
.subscribeRSSLink1 {
background:transparent url(/images/rss.png) no-repeat scroll 0 2px;
padding:0 0 0 20px;
width:60px;
float:left;
}
</style>


<link rel="alternate" type="application/rss+xml" title="<%=ViewingMember.NickName %>'s Videos RSS" href="/rss.aspx?feed=video&nickname=<%=ViewingMember.NickName %>">
      
            <div class="profile_box">
                <a style="left:0px;" class="subscribeRSSLink1" href="/rss.aspx?feed=video&nickname=<%=ViewingMember.NickName %>">subscribe</a>
            <div class="lister_top clearfix">

					<ul class="lister_tabnav">
						<li><a id="contentTab1" <%=CurrentTab1 %> href="/users/<%=ViewingMember.NickName %>/videos/?<%=TabParams %>&to=1">Latest</a></li>
						<li><a id="contentTab2" <%=CurrentTab2 %> href="/users/<%=ViewingMember.NickName %>/videos/?<%=TabParams %>&to=2">Most Viewed</a></li>
						<li><a id="contentTab3" <%=CurrentTab3 %> href="/users/<%=ViewingMember.NickName %>/videos/?<%=TabParams %>&to=3">Most Discussed</a></li>
						<li><a id="contentTab4" <%=CurrentTab4 %> href="/users/<%=ViewingMember.NickName %>/videos/?<%=TabParams %>&to=4">Top Rated</a></li>
					</ul>
				</div>
				
				<div class="clearfix">				
					<div class="lister_topbar_clear clearfix">
					
						<span id="hBrowsing2">
						
							<div class="search_videos">
							
								</div>
							<div class="right">
								<%=DefaultHTMLPager %>
								</div>

						</span>							
						<div class="clear"></div>

					</div>

					<%=DefaultHTMLLister%>
					
                    <p class="clearfix"></p>
					
					<span style="text-align:right;"><%=DefaultHTMLPager %></span>

				</div>
				</div>
<script type="text/javascript">
	function displayMiniVideo(webVideoID,title){
    var html = '<iframe src="/MiniVideoPage.aspx?v='+webVideoID+'" style="border:0px" frameborder="0" scrolling="no" width="480" height="285"></iframe>';   
    npopup(title,html,525,285);
}
</script>	
</asp:Content>

