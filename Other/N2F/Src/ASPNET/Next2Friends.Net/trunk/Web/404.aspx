<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="404.aspx.cs" Inherits="_404" Title="Page not found" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<!-- middle start -->
	<div id="middle" class="clearfix">
		<!--subnav start -->
		<ul id="subnav" class="clearfix">
		</ul>
		<!-- subnav end -->

		<!-- page start -->
		<div class="page clearfix fullPageBkg">
			<!-- signup col start -->
			<div class="signup_col">
				<h2>&quot;Hey, where&#39;s my page?&quot;</h2>
				<p>
					It looks like something is missing here and you ended up on the dreaded 404 
                    page. This could be because of a dead link, or a file was moved or because of 
                    some other reason that neither you nor us have any control over. 
				</p>
				<h3>&quot;Oh, No! What shoud I do now?&quot;</h3>
				<p>
					Your best bet, at the moment, would be to use our search engine (on the top 
                    right side of the page). However, if by any freak occurence, you end up on the 
                    same 404 page, don&#39;t search again or you&#39;ll be caught in an endless loop.
				</p>
				<p>
					You could also start with our <a href="index.aspx">homepage</a>. There you&#39;ll 
                    find all the latest and hottest stuff from our website. If you want more 
                    specific content, you can check the main areas:
					<a href="FriendRequest.aspx" >Friends</a>, <a href="Community.aspx" >Community</a>, 
					<a href="/videos" >Videos</a>, , or even 
					<a href="/ask" >Ask-a-Friend</a>.
				</p>
				<h3>Report a wrong link</h3>
				<p>
					If you truly believe a page should be here, for example, you followed a link by 
                    one of the staff members who must have made a typo, please report the problem as 
                    soon as possible. Please <a href="mailto:contact@next2friends.com">contact</a> 
                    us ASAP. We appreciate the help our users are providing.
				</p>
			</div>
		<!-- page end -->
		</div>
	</div>
	
	
</asp:Content>

