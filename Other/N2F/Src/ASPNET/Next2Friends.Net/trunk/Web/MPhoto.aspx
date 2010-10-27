<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="MPhoto.aspx.cs" Inherits="MPhotos" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/View.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftColContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftUpperContentHolder" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="RightColContentHolder" Runat="Server">
<link rel="alternate" type="application/rss+xml" title="<%=ViewingMember.NickName %>'s Videos RSS" href="/rss.aspx?feed=photo&nickname=<%=ViewingMember.NickName %>">
				<!-- box start -->
				<div class="profile_box">
				    <a class="subscribeRSSLink" href="/rss.aspx?feed=photo&nickname=<%=ViewingMember.NickName %>">subscribe</a>
					<h4 class="box_title collapsible">Photo Galleries <%=GalleryDetailsHTML %></h4>
					<div class="collapsible_div">
					<%if (ShowCarousel){ %>	
						<ul class="photogalleryul">
                        	<%=GalleryListerHTML %>
                    	</ul>
                    <%}else{ %>
                            <%=GalleryListerHTML %>
                    <% }%>
				    </div>
				</div>
				
				<!-- box end -->
				
				
</asp:Content>

