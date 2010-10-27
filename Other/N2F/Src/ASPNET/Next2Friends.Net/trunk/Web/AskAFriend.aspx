<%@ Page AutoEventWireup="true" Debug="true" CodeFile="AskAFriend.aspx.cs" Inherits="AskAFriendPage"  Language="C#" MasterPageFile="Main.master" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/Main.master" %>
<%@ Register src="Comments.ascx" tagname="Comments" tagprefix="uc1" %>
<%@ Import Namespace="Next2Friends.Data" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
<script type="text/javascript" >

var returnToDash = <%=ReturnToDash %>;

</script>
<script type="text/javascript" src="/js/AskAFriend.js?v=4"></script>

<!-- middle start -->
	<div id="middle" class="clearfix">

		<!--subnav start -->
		<ul id="subnav" class="clearfix">
			<li><a href="/ask">Play!</a></li>
			<%if (IsLoggedIn){%>
			<li><a href="/MyAskAFriend.aspx">My Questions</a></li>
			<li><a href="/AAFUpload.aspx">New Question</a></li>
			<%} %>
		</ul>
		<!-- subnav end -->
		
		<!-- page start -->
		<div class="page clearfix fullPageBkg">
		
			<!-- aaf start -->
	
				<h3 class="aaf_question_large" style="padding:15px 135px 20px 100px" id="h3Question"><%=InitialAAF.Question %></h3>
				
				<ul class="aaf_answers clearfix" id="ulSelection">
                    <%=InitialAAF.HTML %>
				</ul>
				
				<p class="bookmark_this" id="pBookmarks">
					<%=InitialAAF.Bookmarks %>
				</p>
				
				<p>&nbsp;</p>
				<p>&nbsp;</p>
				<div id="divComments" style="position: relative">
                    <uc1:Comments ID="Comments1" runat="server" />
                </div>
                <script language="javascript" type="text/javascript">

                    AskAFriendWebId = "<%=AskAFriendWebId %>";                    
                    getComments('AskAFriend',AskAFriendWebId);
                </script>
		

<%--			<!-- aaf end -->

			<!-- right start -->
			<div class="aaf_rightcol">
			
				<div class="dont_like_this">
					<strong>Don't like this question?</strong>
					Skipping questions helps N2F display only fun and interesting stuff!
				</div><!--/aaf dont like this -->
				
				    <a href="/ask">
				    <img src="/images/btn-skip.gif" id="iSkip" alt="skip" class="bottomSpace" />
                </a>
				
				<div class="dont_like_this what-is-aaf">
					<strong>What is this page?</strong>
					<a href="/features">Find out how to use ask from your mobile in our features page &raquo;</a>
				</div>
              
				<span id="spanLastAAF"><%=InitialAAF.LastAAF%></span>
				
				<!--/aaf next -->
				

			</div>
			<!-- right end -->--%>
			
		
		</div>
		<!-- page end -->



	</div>

	<!-- middle end -->


</asp:Content>
