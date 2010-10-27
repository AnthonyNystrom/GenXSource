<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Friend.aspx.cs" Inherits="FriendPage" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/Main.master" %>
<%@ Register TagPrefix="N2F"  namespace="Next2Friends.WebControls"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<!-- middle start -->
	<div id="middle" class="clearfix no_subnav">

<%--		<!--subnav start -->
		<ul id="subnav" class="clearfix">
		    <li><a href="/friends">My friends</a></li>
			<li><a href="/friend-requests">Friend requests</a></li>
			<li><a href="/friends/?t=px">Proximity tags</a></li>
			<li><a href="/friends/?t=blocked">Blocked</a></li>
			<li><a href="/invite">Invite friends</a></li>
		</ul>
		
		
		<%if(ShowStats){ %>
		<p class="friends_stats">You have <a href="/FriendRequest.aspx"><%=friendStats.AllRequests%> Friend Requests</a>, <a href="/Friend.aspx?t=px"><%=friendStats.ProximityTags%> Proximity Tags</a></p>
        <%} %>--%>

		<div class="lister_top clearfix">

					<ul class="lister_tabnav">
						<li><a id="contentTab1" <%=CurrentTab1 %> href="/proximity-tags/?to=1">First Name</a></li>
						<li><a id="contentTab2" <%=CurrentTab2 %> href="/proximity-tags/?to=2">Last Name</a></li>
						<li><a id="contentTab3" <%=CurrentTab3 %> href="/proximity-tags/?to=3">Nickname</a></li>
					</ul>
		</div>

		<div class="friends_top clearfix">
			<%=PageHeaderTitle %>
			<div class="right"><%=DefaultHTMLPager %></div>
		</div>




				
<%--			<div class="friend_search">
				    <asp:TextBox runat="server" CssClass="form_txt2" Width="180" id="txtSearch" ></asp:TextBox>
				    <asp:DropDownList runat="server" ID="drpSearchOptions"  Width="170" CssClass="form_txt2">
				        <asp:ListItem Text="My friends" Value="0" ></asp:ListItem>
				        <asp:ListItem Text="Next2Friends network" Value="1" Selected="True"></asp:ListItem>
				    </asp:DropDownList>
				<asp:Button runat="server" CssClass="form_btn2" Text="Search" ID="btnSearch" onclick="btnSearch_Click"  />
				<a href="#" class="btn_search_advance">Options</a>
				<div class="advance_friend_search">
                        Sex 
					    <asp:DropDownList runat="server" ID="drpGender" Width="90" class="form_txt2">
					        <asp:ListItem Text="Any" Value="-1"></asp:ListItem>
					        <asp:ListItem Text="Male" Value="1"></asp:ListItem>
					        <asp:ListItem Text="Female" Value="0"></asp:ListItem>
					    </asp:DropDownList>
				
                        Country 
						<N2F:CountryDropdown runat="server" ID="drpCopuntries" class="form_txt2" CountryDropdownType="Search">
						<asp:ListItem Text="All Countries" Value="-1"></asp:ListItem>
						
						</N2F:CountryDropdown>

				</div>
			</div>--%>


    <span id="divFriends">
       <%=DefaultHTMLLister%>
       </span>

        <span><%=DefaultHTMLPager %></span>
	</div>

	<!-- middle end -->

    <script type="text/javascript" src="/js/Friend.js"></script>
</asp:Content>



