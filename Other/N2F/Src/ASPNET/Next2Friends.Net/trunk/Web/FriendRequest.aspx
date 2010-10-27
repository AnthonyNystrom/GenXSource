<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="FriendRequest.aspx.cs" Inherits="FriendRequestPage" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager runat="server" EnablePageMethods="true"></asp:ScriptManager>



<!-- middle start -->
	<div id="middle" class="clearfix no_subnav">
    
 <%--   	<!--subnav start -->
		<ul id="subnav" class="clearfix">
		    <li><a href="/friends">My friends</a></li>
			<li><a href="/friend-requests">Friend requests</a></li>
			<li><a href="/friends/?t=px">Proximity tags</a></li>
			<li><a href="/friends/?t=blocked">Blocked</a></li>
			<li><a href="/invite">Invite friends</a></li>
		</ul>
		<!-- subnav end -->--%>
							
		<div class="friends_top clearfix">
			<h2>Friend Requests <small><span id="cntFR">(<%=NumberOfFriendRequests%>)</span></small></h2>


		</div>

                <%=DefaultHTMLLister %>
                
	</div>

	<!-- middle end -->

<script type="text/javascript" src="js/FriendRequest.js?v=3"></script>
<script language="javascript" type="text/javascript">
    PageListerType = <%=PageListerType %>;
</script>
</asp:Content>


