<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CommunityOLD.aspx.cs" Inherits="CommunityPageOLD" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="lib/jquery.js"></script>
    <script type="text/javascript" src="lib/jquery_global.js"></script>
    <script type="text/javascript" src="lib/jquery.jcarousellite.pack.js"></script>
    <script type="text/javascript" src="js/Community.js"></script>
    
<!-- middle start -->
	<div id="middle" class="clearfix">

		<ul id="subnav" class="clearfix">
			<li><a href="MatchProfile.aspx">My Tagging Profile</a></li>
			<%if (IsLoggedIn) { %><li><a href="MyNspots.aspx">My Nspots</a></li><%} %>
		</ul>

			<h3>Featured NSpots</h3>
			<div class="featured_nspots">
				<div class="prev"></div>
				<div class="next"></div>
				
				<div class="thumbs">
					<ul>
                        <%=DefaultFeaturedNSpotScroller %>
					</ul>
				</div>

				<script type="text/javascript">
				$(".thumbs").jCarouselLite({
					btnNext: ".featured_nspots .next",
					btnPrev: ".featured_nspots .prev",
					visible: 3,
					speed: 500,
					scroll: 1,
					circular: false
				});    
				</script>
			</div>
			
			<div class="nspots_statistics">
				<strong>Statistics</strong><br />
				Members: <%=stats.Members %><br />
				Proximity Matches: <%=stats.ProximityMatches %><br />
				Friend Connections: <%=stats.FriendConnections %><br />
				NSpots <%=stats.NSpots %><br />
			</div>
			
	<!-- lister left start -->
			<div class="lister_leftcol">
			
				<div class="lister_top clearfix">
					<h1 class="lister_title">NSpots</h1>

						<ul class="lister_tabnav">
						    <li><a class="current" id="contentTab1" href="javascript:ajaxGetListerContent(1);">Featured NSpots</a></li>
						    <li><a id="contentTab2" href="javascript:ajaxGetListerContent(2);">Most Viewed</a></li>
						    <li><a id="contentTab3" href="javascript:ajaxGetListerContent(3);">Most Discussed</a></li>
						    <li><a id="contentTab4" href="javascript:ajaxGetListerContent(4);">Top Rated</a></li>
					</ul>
				</div>

				
				<div class="lister_middle clearfix">				

					<ul class="videos_list clearfix" id="ulContentLister">
					    <%=DefaultNSpotLister%>
					</ul>
					
					<p class="pagenav" id="pPageNav"><%=DefaultNSpotPager %></p>

				</div>

			</div>
			<!-- lister left end -->


			<!-- profile right start -->
			<div class="lister_rightcol">
				
			</div>

			<!-- profile right end -->
			
		


	</div>
	<!-- middle end -->

</asp:Content>

