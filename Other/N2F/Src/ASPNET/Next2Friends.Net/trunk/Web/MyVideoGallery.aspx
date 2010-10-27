<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MyVideoGallery.aspx.cs" Inherits="MyVideoGallery" Title="My Videos" %>
<%@ Register TagPrefix="N2F"  namespace="Next2Friends.WebControls"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<link rel="stylesheet" href="/lib/thickbox.css" type="text/css" media="screen" />

<script type="text/javascript">
var lbClicked = false;

function lbLive(){
lbClicked = true;
return void(0);
}
//$(document).click(
//function(event){

//if(!$(event.target).is('.suggested_tags') && !$(event.target).is("input") && !$(event.target).parent().is("suggested_tags") && !$(event.target).is("select"))
//$(
//".suggested_tags").hide();
//)
//}

$(document).click(
function(event){

if(!$(event.target).is('#divSkins') && !$(event.target).is('.btn_change_skin'))
    $(
        "#divSkins").hide();
    }

);


</script>
<script type="text/javascript" src="/lib/swfobject.js"></script>
<script type="text/javascript"src="/lib/jquery_thickbox.js"></script>	
	<!-- middle start -->
	<div id="middle" class="clearfix no_subnav">


	    <p class="right"><a href="javascript:lbLive();" title="Upload your Videos" class="thickbox" id="atb"><img src="/images/btn-upload-blue.gif" alt="Upload" /></a></p>
        <%=PagerHTML%>

    <div class="clear">
    
    <%=NoVideosMessage%>
     
    <asp:Repeater ID="VideoRepeater" runat="server" onitemcreated="Repeater1_ItemCreated" >
    <HeaderTemplate></HeaderTemplate>
    <ItemTemplate>
    <%Indexer = GetNextIndex(); %>
               <div class="edit_item clearfix" id="VideoItem<%#Indexer %>">
                <div class='edit_item_left'>
                    <asp:HiddenField runat="server" id="WebVideoID" value='<%#DataBinder.Eval(Container.DataItem, "WebVideoID")%>'/>
                    <p class='edit_thumb'>
                        
                        <%#PreviewHTML%>

                    </p>
        
                </div>

                <div class="edit_item_right">
                
                    <p>Title <asp:TextBox runat="server" onclick="<%#FuncDec %>" Text='<%#DataBinder.Eval(Container.DataItem, "Title")%>' ID="txtTitle" CssClass="form_txt title"></asp:TextBox></p>
                    <p>
                        Caption<br />
                        <asp:TextBox runat="server" onclick="<%#FuncDec %>" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "Description")%>' ID="txtCaption" CssClass="form_txt caption"></asp:TextBox></textarea>
                    </p>
                    
                    <p> Category
                        <N2F:CategoryDropdown runat="server" SelectedValue='<%#DataBinder.Eval(Container.DataItem, "Category")%>' Width="200" onchange="<%#FuncCategorySelect %>" ID="drpCategories"></N2F:CategoryDropdown>
                    </p>
                    <p class="tags_input"><asp:TextBox runat="server" onfocus="<%#FuncShowTags %>" onclick="<%#FuncDec %>" Text='<%#DataBinder.Eval(Container.DataItem, "Tags")%>' CssClass="form_txt" Width="265" ID="txtTags"></asp:TextBox></p>
                    <p class='suggested_tags' style="display:none;" id="pTags<%#Indexer %>"> Choose a category for suggested Tags <a href="javascript:hideTags('<%#Indexer %>')" title="close"><img src="/images/close-tags.gif" alt="close" class="close_tags" /></a></p>                   
                    <p class='delete_this'><asp:CheckBox runat="server" ID="chbDelete" onchange="<%#FuncDec %>" onclick="toggleColor(this)" CssClass="checkbox"  /> Delete this video &nbsp;<asp:CheckBox runat="server" ID="chbPrivacy" onchange="<%#FuncDec %>" onclick="toggleColor(this)" CssClass="checkbox"  /> Friends only</p>                    
                    <p class="quick_save" id="pQSave<%#Indexer %>"><input disabled="disabled" id="btnQSave<%#Indexer %>" onclick="saveItem('<%#DataBinder.Eval(Container.DataItem, "WebVideoID")%>','<%#Indexer %>')" type="button" value="Quick Save" class="form_btn" /></p>
                    <div class="clear"></div>
                </div>
            </div>
                        
                    
    </ItemTemplate>
    <FooterTemplate></FooterTemplate>
    </asp:Repeater>  
    <div class="clear"></div>  
    <%=PagerHTML%>
    <%if (!NoVideos)
      {%>
		<p class="save_all"><input type="button" value="Save all" class="form_btn" onclick="__doPostBack('<%=btnSave.UniqueID %>', '');return false;" />
		<asp:Button CssClass="hiddenButton" runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save" /></p>
		<%} %>
		
	</div>
	</div>
	<!-- middle end -->
	
	<script type="text/javascript">
	<%=CategoryTagsArrays %>
	</script>

<script type="text/javascript" src="/js/VideoGalleryEdit.js"></script>

<script type="text/javascript">
$(document).ready(function(){
    $('#atb').attr('href','/UploadVideo2.aspx?TB_iframe=true&height=425&width=500&modal=true');
    if(lbClicked){
        $('#atb').click();
    }
});
function closeme(){
    tb_remove();
}
</script>


</asp:Content>

