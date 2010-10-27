<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="Wall.aspx.cs" Inherits="WallPage" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/View.master" %>
<%@ Register src="Comments.ascx" tagname="Comments" tagprefix="uc1" %>


<asp:Content ID="Content3" ContentPlaceHolderID="LeftUpperContentHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftColContentHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="RightColContentHolder" Runat="Server">
    <!-- box start -->
    <div class="profile_box" runat="server" id="divComments">
        <uc1:Comments ID="Comments1" runat="server" />
    </div>
    <!-- box end -->            
</asp:Content>

