<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="MFriends.aspx.cs" Inherits="MFriends" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/View.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftUpperContentHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="RightColContentHolder" Runat="Server">
<script language="javascript" type="text/javascript">
    function unfriendMember(WebMemberID){
        if(confirm("Are you sure you want to permanently remove this friend from your network?")){
            MFriends.UnfriendMember(WebMemberID, unfriendMember_Callback);
        }
    }

    function unfriendMember_Callback(response){
        
        if(response.value!=null){
            var divFriends = document.getElementById('divFriends');
            var divFriend = document.getElementById('divFriend'+response.value);
            
            divFriends.removeChild(divFriend);
           
        }
    }
</script>

            <div class="profile_box" id="profileFriendsList">
                <h4 class="box_title collapsible">Friends</h4>
                 <div class="collapsible_div">

                     <div class="right">
                        <%=DefaultHTMLPager %>
                     </div>
                     <div style="height:50px"></div>
                       <span id="divFriends">
                           <%=DefaultHTMLLister%>
                       </span>

                      <div class="right">
                       <%=DefaultHTMLPager %>
                       </div>
                       
                   </div>
            </div> 


</asp:Content>

