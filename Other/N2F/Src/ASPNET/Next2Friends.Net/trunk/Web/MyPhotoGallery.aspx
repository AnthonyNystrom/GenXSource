<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MyPhotoGallery.aspx.cs"
    Inherits="MyPhotoGallery" Title="My Photos" %>

<%--<%@ Register TagPrefix="N2F"  namespace="Next2Friends.WebControls"%>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- <link rel="stylesheet" href="lib/thickbox.css" type="text/css" media="screen" /> -->
    <!-- <link rel="Stylesheet" href="hover-button.css" type="text/css" media="screen" /> -->
    <style type="text/css" media="all">
        @import "/lib/Thickbox.css";
    </style>

    <script type="text/javascript">
        var lbClicked = false;

        function lbLive() {
            lbClicked = true;
            return void (0);
        }
    </script>

    <script type="text/javascript" src="/js/GalleryEdit.js"></script>

    <script type="text/javascript" src="/lib/jquery_thickbox.js"></script>

    <script type="text/javascript" src="/lib/FancyZoom.js"></script>

    <script type="text/javascript" src="/lib/jquery.rotate.1-1.js"></script>

    <!-- middle start -->
    <div id="middle" class="clearfix no_subnav">
        <div class="edit_gallery">
            <div class="top_bar clearfix">
                <h4 class="btn_edit_gallery">
                    Edit/Show Galleries</h4>
                <a href="#" class="btn_add_new">+ Add New</a>
            </div>
            <div class="add_new clearfix">
                <p class="cat_name">
                    <label>
                        Gallery Name</label><br />
                    <asp:TextBox runat="server" CssClass="form_txt" ID="txtGalleryName"></asp:TextBox>
                </p>
                <p class="description">
                    <label>
                        Description</label><br />
                    <asp:TextBox runat="server" TextMode="MultiLine" CssClass="form_txt" ID="txtGalleryDescription"></asp:TextBox>
                </p>
                <p class="actions">
                    <input type="button" value="Create" class="form_btn" onclick="__doPostBack('<%=btnNewGallery.UniqueID %>', '');return false;" />
                    <asp:Button CssClass="hiddenButton" runat="server" ID="btnNewGallery" OnClick="btnNewGallery_Click"
                        Text="Save" />
                </p>
            </div>
            <!--/add new -->
            <div class="category_list" id="divGalleryList">
                <%=MyGalleriesHTML %>
            </div>
        </div>
        <div class="clear" id="divLister">
            <h2>
                <%=GalleryName %></h2>
            <div class="clear" id="div1">
                <p class="right">
                    <a href="javascript:lbLive();" id="atb" title="Upload your photos" class="thickbox">
                        <img src="/images/btn-upload-blue.gif" alt="Upload" /></a></p>
                <%=PagerHTML %>
            </div>
            <div class="clear">
                <asp:Repeater ID="PhotoRepeater" runat="server" OnItemCreated="Repeater1_ItemCreated">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class='edit_item clearfix' id="photoItem<%#Indexer %>" style="height: 220px">
                            <div class='edit_item_left'>
                                <asp:HiddenField runat="server" ID="WebPhotoID" Value='<%#DataBinder.Eval(Container.DataItem, "WebPhotoID")%>' />
                                <asp:HiddenField runat="server" ID="Rotation" Value='0' />
                                <p class='rotate'>
                                    <img src='/images/rotate-left.gif' alt='rotate left' style="cursor: pointer;" onclick="<%#FuncDec %>;$('#img<%#DataBinder.Eval(Container.DataItem, "WebPhotoID")%>').rotateLeft();rotate('<%#Indexer%>', true);" />
                                    Rotate
                                    <img src='/images/rotate-right.gif' alt='rotate right' style="cursor: pointer;" onclick="<%#FuncDec %>;$('#img<%#DataBinder.Eval(Container.DataItem, "WebPhotoID")%>').rotateRight();rotate('<%#Indexer%>', false);" />
                                </p>
                                <p class='edit_thumb'>
                                    &nbsp;<br />
                                    <a href="http://www.next2friends.com/<%#DataBinder.Eval(Container.DataItem, "PhotoResourceFile.FullyQualifiedURL")%>?cache=<%=NoCacheID %>">
                                        <img src='http://www.next2friends.com/<%#DataBinder.Eval(Container.DataItem, "ThumbnailResourceFile.FullyQualifiedURL")%>?cache=<%=NoCacheID %>'
                                            id='img<%#DataBinder.Eval(Container.DataItem, "WebPhotoID")%>' alt='thumb' /></a>
                                </p>
                                <div class="hover-button">
                                    <a href="/PicnikEdit.aspx?WebPhotoId=<%#DataBinder.Eval(Container.DataItem, "WebPhotoID")%>&TB_iframe=true&height=530&width=820&modal=true"
                                        class="thickbox"
                                        title="Edit photo">
                                        <img id="img-edit-picnik" src="images/edit-picnik-normal.png" alt="Edit photo" />
                                    </a>
                                </div>
                            </div>
                            <div class='edit_item_right'>
                                <p>
                                    Caption<br />
                                    <asp:TextBox runat="server" TextMode="MultiLine" onclick="<%#FuncDec %>" Text='<%#DataBinder.Eval(Container.DataItem, "Caption")%>'
                                        ID="txtCaption" CssClass="form_txt caption"></asp:TextBox></textarea>
                                </p>
                                <p>
                                    Move to gallery<br />
                                    <asp:DropDownList runat="server" ID="drpGallery" onchange="<%#FuncDec %>" SelectedValue='<%#DataBinder.Eval(Container.DataItem, "WebPhotoCollectionID")%>'
                                        CssClass="form_menu" DataSource='<%# photoCollections %>' DataTextField="Name"
                                        DataValueField="WebPhotoCollectionID">
                                    </asp:DropDownList>
                                </p>
                                <%--                    <p class=''><N2F:CategoryDropdown runat="server" SelectedValue='<%#DataBinder.Eval(Container.DataItem, "CategoryID")%>' Width="200" onchange="<%#FuncCategorySelect %>" ID="drpCategories"></N2F:CategoryDropdown></p>
                    <p class=''><asp:TextBox runat="server" onclick="<%#FuncDec %>" Text='<%#DataBinder.Eval(Container.DataItem, "Tags")%>' CssClass="form_txt" Width="272" ID="txtTags"></asp:TextBox></p>
                    <p class='' id="pTags<%#Indexer %>"></p>--%>
                                <p class='delete_this'>
                                    <asp:CheckBox runat="server" ID="chbDelete" onclick="<%#FuncDec %>" CssClass="checkbox" />
                                    Delete this photo</p>
                                <p class="quick_save" id="pQSave<%#Indexer %>">
                                    <input disabled="disabled" id="btnQSave<%#Indexer %>" onclick="saveItem('<%#DataBinder.Eval(Container.DataItem, "WebPhotoID")%>','<%#Indexer %>')"
                                        type="button" value="Quick Save" class="form_btn" /></p>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <p class="left">
                <%=PagerHTML %></p>
            <p class="save_all">
                <input type="button" value="Save all" class="form_btn" onclick="__doPostBack('<%=btnSave.UniqueID %>', '');return false;" />
                <asp:Button CssClass="hiddenButton" runat="server" ID="btnSave" OnClick="btnSave_Click"
                    Text="Save all" /></p>
        </div>
        <%--	<script type="text/javascript">
	<%=CategoryTagsArrays %>
	</script>--%>
        <!-- middle end -->

        <script type="text/javascript">
            $(document).ready(function() {
                $('#atb').attr('href', '/uploader.aspx?&DefaultGalleryID=<%=DefaultWebPhotoCollectionID %>&TB_iframe=true&height=530&width=820&modal=true');
                if (lbClicked) {
                    $('#atb').click();
                }

            });

            setupZoom();

        </script>

    </div>
</asp:Content>
