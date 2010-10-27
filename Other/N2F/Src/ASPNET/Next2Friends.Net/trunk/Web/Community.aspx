<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Community.aspx.cs" Inherits="CommunityPage" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/Main.master" %>
<%@ Register TagPrefix="N2F"  namespace="Next2Friends.WebControls"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<%--    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>--%>

<script type="text/javascript">
    
</script>
<!-- middle start -->
	<div id="middle" class="clearfix">
	
			<!--subnav start -->
		   <div class="lister_top clearfix">

					<ul class="lister_tabnav">
					    <li><a id="contentTab1" <%=CurrentTab1 %> href="/community/?<%=pageUrl %>&to=1">Most Videos</a></li>
						<li><a id="contentTab2" <%=CurrentTab2 %> href="/community/?<%=pageUrl %>&to=2">First Name</a></li>
						<li><a id="contentTab3" <%=CurrentTab3 %> href="/community/?<%=pageUrl %>&to=3">Last Name</a></li>
						<li><a id="contentTab4" <%=CurrentTab4 %> href="/community/?<%=pageUrl %>&to=4">Newest Members</a></li>
						<li><a id="contentTab8" <%=CurrentTab8 %> href="/community/?<%=pageUrl %>&to=8">Country</a></li>
						<li><a id="contentTab6" <%=CurrentTab6 %> href="/community/?<%=pageUrl %>&to=6">Trade</a></li>
						<li><a id="contentTab7" <%=CurrentTab7 %> href="/community/?<%=pageUrl %>&to=7">Hobby</a></li>
	<%--					<li><a id="contentTab4" <%=CurrentTab4 %> href="community.aspx?<%=pageUrl %>&to=4">Last online</a></li>--%>
						
					</ul>					
		</div>

        <div class="clearfix friends_top">
            <div style="text-align:left" class="clearfix left">
                <div style="margin:10px 0;height:18px;">
                    <span style="<%=HideCountry%>"><label for="country">Select Country :&nbsp;</label><N2F:CountryDropdown runat="server"  AutoPostBack="true" ID="drpCountry" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"></N2F:CountryDropdown></span>
		            <span style="<%=HideProfession%>"><label for="trade">Select Trade :&nbsp;</label><N2F:ProfessionDropdown AutoPostBack="true" runat="server" ID="drpProfession" OnSelectedIndexChanged="drpProfession_SelectedIndexChanged"></N2F:ProfessionDropdown></span>
		            <span style="<%=HideHobby%>"><label for="hobby">Select Hobby :&nbsp;</label><N2F:HobbyDropdown runat="server"  AutoPostBack="true" ID="drpHobby" OnSelectedIndexChanged="drpHobby_SelectedIndexChanged"></N2F:HobbyDropdown></span>
		        </div>
		    </div>
			<div style="text-align:right" class="clearfix right"><%=DefaultHTMLPager %></div>
        </div>  
        
    <span id="divFriends">
       <%=DefaultHTMLLister%>
       </span>
       

    <%--<p class="pagenav" id="pPageNav"><%=DefaultHTMLPager %></p>--%>
	<div style="text-align:right" class="clear"><%=DefaultHTMLPager %></div>
	
	</div>

	<!-- middle end -->

   <script src="/js/community.js" type="text/javascript"></script>
</asp:Content>


