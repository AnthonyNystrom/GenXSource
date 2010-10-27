<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MyAskAFriend.aspx.cs" Inherits="AskAFriendMain" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<%--	<ul id="subnav" class="clearfix">
			<li><a href="AskAFriend.aspx">Answer questions!</a></li>
			<li><a href="MyAskAFriend.aspx?t=1">My Questions</a></li>
			<li><a href="MyAskAFriend.aspx?t=2">My Answers</a></li>
		</ul>--%>				
	<!-- middle start -->
	<div id="middle" class="clearfix">

<%--		<ul id="subnav" class="clearfix">
			<li><a href="AskAFriend.aspx">Play!</a></li>
			<%if (IsLoggedIn){%>
			<li><a href="MyAskAFriend.aspx"><img src="images/BtnMyQuestions.gif" /></a></li>
			<li><a href="AAFUpload.aspx"><img src="images/btnNewQuestion.gif" /></a></li>
			<%}else{%>
			<li><a href="Signup.aspx"><img src="images/btnNewQuestion.gif" /></a></li>
			<%} %>
		</ul>
--%>



			<!-- lister left start -->
			<div class="aaf_lister_left">
			
				<div class="lister_top clearfix">
					<h1 class="lister_title" style="margin-bottom:8px">My Questions</h1>
<%--					<ul class="lister_tabnav">
						<li><a href="MyAskAFriend.aspx?t=recent" <%if(OrderBy==1){ %>class="current"<%} %>>Most Recent</a></li>
						<li><a href="MyAskAFriend.aspx?t=voted" <%if(OrderBy==2){ %>class="current"<%} %>>Most Voted</a></li>

					</ul>--%>
				</div>
				
				<div class="lister_middle clearfix">
				
					<ul class="aaf_list clearfix">
					
					<%if (IHaveNoQuestions)
       { %>
					
					You have not submitted any questions. Please download the Ask-A-Friend software to you mobile device from <a href="download.aspx">here</a>.
					
					<%}
       else
       {%>
						<%=DefaultHTMLLister%>
						<%} %>
					</ul>

					
					<%--<p class="pagenav plain"><a href="#" class="previous">Previous</a> <a href="#">1</a> <a href="#">2</a> <a href="#">3</a> <a href="#" class="next">Next</a></p>--%>
				</div>

		</div>

			<!-- lister left end -->


			<!-- profile right start -->
			<div class="aaf_lister_right">
				<h4>Next2Friends Top Questions</h4>
				<div class="top10_box">
					<ol>
                        <%=DefaultTop10Lister %>
					</ol>

					<div class="bottom"></div>
				</div>
			</div>
			<!-- profile right end -->
			
		


	</div>
	<!-- middle end -->
</asp:Content>


