<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MyLinks.aspx.cs" Inherits="MyLinks" Title="My Links" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- middle start -->
    <div id="middle" class="no_subnav clearfix">
    
    
        <!-- subnav end -->
        <div class="lister_topbar clearfix">
            <h2>
                Subscriptions & Favorites</h2>
        </div>
        <!--/lister topbar -->
        <ul class="feat_videos" id="featuredVideos">
            <li>
                <div class='vid_thumb'>
                    <a href='view.aspx?v=ODA2ZTIxYTE3ODQzNDdlNT'>
                        <img src='user/lawrence/vthmb/MDJhZGE1N2EyYjQ5NGZj1.jpg' alt='thumb' width='124'
                            height='91' /> 1 days ago
                </div>
                <div class='vid_info'>
                    <h3>
                        <a href='view.aspx?v=ODA2ZTIxYTE3ODQzNDdlNT'>short title</a></h3>
                    <div class='vote vote_condensed'>
                        <span class='vote_count'>2</span></div>
                    short description</p>
                    <p class='metadata'>
                        Views: 0 Comments: 1<br />
                        From: <a href='view.aspx?m=MzY3MTE5ODJjY2VlNDA0OD'>lawrence</a></p>
                </div>
            </li>
        </ul>
    </div>
    <!-- middle end -->
</asp:Content>
