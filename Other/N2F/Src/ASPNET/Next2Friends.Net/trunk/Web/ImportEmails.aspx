<%@ Page language="c#"  MasterPageFile="~/Main.master" validateRequest="false" Inherits="ImportEmails" CodeFile="ImportEmails.aspx.cs" %>
<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript" src="/lib/jquery.localscroll-min.js"></script>
<script type="text/javascript" src="/lib/jquery.scrollTo-min.js"></script>

<style type="text/css">
    
.selectAll{
    background:transparent url(images/select-all-mini.gif) no-repeat scroll center;
    color:#FFFFFF;
    line-height:100%;
    margin-left:0px;
    padding:1px 7px;
}

.selectNone{
    background:transparent url(images/select-none-mini.gif) no-repeat scroll center;
    color:#FFFFFF;
    line-height:100%;
    margin-right:4px;
    padding:1px 5px;
}
</style>
<!-- middle start -->
	<div id="middle" class="clearfix">
		<!-- page start -->
		<div class="page clearfix fullPageBkg">

				<!-- NEW CONTENT STARTS HERE -->
				<%if (IsSignup) { %>
				<div style="float:right;"><input class="form_btn" type="button" value="Skip this step" onclick="location.href='/download';return false;"></div>
				<%} %>
				<h2 class="bottomSpace">Find your friends!</h2>
				
				<div class="contactSelector">
					<p>
						Sign into your webmail and connect with all your friends who are Next2Friends members!<br /><%--<br />
						You can use the invite friends page as much as you like and we will ensure that only one email is sent out per contact.--%>
					</p>
					<p>
						<img src="/images/email-logos.jpg?v=1" width="330" height="70" alt="" />
					</p>
					<div class="profile_box" style="text-align:center;">
					    <%=HelperMessage %>
					    <%if (ImportStage){%>
					    					    <p>
						        <asp:Button runat="server" CssClass="form_btn" Text="Cancel" ID="btnCancel" OnClick="btnCancel_Click"  />
						        <input type="button" class="form_btn" value="Invite!" id="btnImport" onclick="postInvite();" />
				                <%--<asp:Button runat="server" CssClass="form_btn" Text="Invite!" ID="btnImport" OnClick="btnImport_Click" />--%>
						    </p>
                         <%} %>
					</div>
					
					<%if (!ImportStage) { %>
					<div class="profile_box">
						<table cellspacing="6" cellpadding="0">
							<tr>
								<td width="">
									Logon:
								</td>
								<td>
							<asp:TextBox ID="txtUserID" CssClass="form_txt" runat="server" Width="110px" Text=""></asp:TextBox>&nbsp;
							<asp:DropDownList ID="emailCat" CssClass="form_txt" runat="server" Width="111px">
                            <asp:ListItem Value="hotmail.com" Selected="True">Hotmail.com</asp:ListItem>
                            <asp:ListItem Value="yahoo.com">Yahoo!</asp:ListItem>
                            <asp:ListItem Value="live.com">Live</asp:ListItem>
                            <asp:ListItem Value="msn.com">MSN</asp:ListItem>
                            <asp:ListItem Value="gmail.com">Gmail</asp:ListItem>
                            <asp:ListItem Value="aol.com">AOL</asp:ListItem>
                            <asp:ListItem Value="rediff.com">Rediff</asp:ListItem>
                      <%--      <asp:ListItem Value="myspace.com">Myspace</asp:ListItem>
                            <asp:ListItem Value="web.de">web.de</asp:ListItem>
                            <asp:ListItem Value="mail.com">mail.com</asp:ListItem>
                            <asp:ListItem Value="mail.ru">mail.ru</asp:ListItem>
                            <asp:ListItem Value="plaxo.com">plaxo.com</asp:ListItem>--%>
                            <asp:ListItem Value="linkedin.com">LinkedIn</asp:ListItem>
     <%--                       <asp:ListItem Value="163.com">163.com</asp:ListItem>
                            <asp:ListItem Value="sina.com">sina.com</asp:ListItem>
                            <asp:ListItem Value="qq.com">qq.com</asp:ListItem>--%>
                        </asp:DropDownList>
							</td>
							</tr>
							<tr>

								<td>
									Password:
								</td>
								<td>
							<asp:TextBox runat="server" TextMode="Password" ID="txtPassword" CssClass="form_txt" Width="228px"></asp:TextBox>
						</td>
							</tr>
						</table>
						<p style="text-align:right">
							<asp:Button runat="server" CssClass="form_btn" Text="Login" ID="btnWebMailLogin" OnClick="btnWebMailLogin_Click" />
						</p>
					</div>
					<%} %>
					
					<p><strong>Step 1:</strong>  Login to your webmail or social networking account. Your logon name could be your email address or your user name depending on the service.</p>
                    <p><strong>Step 2:</strong>  Select the contacts you would like to invite or send a Friend request to.</p>
                    <p><strong>Step 3:</strong>  Click invite to send your email invites and Friend requests</p>
					
			<%--		<p>
					    Your privacy is our top concern. Your contacts are your private information. Next2Friends will not store your username and password and any information you upload will be securely imported for you own use. Next2Friends will not send your contacts any e-mail. For more information please see the Next2Friends Privacy Policy.
					</p>--%>
				</div>
				
				<div class="contactBlock" id="ContactLister">
					<div class="nameTag">
						<ul>
							<%=HTMLIndexList%>
						</ul>
					</div>
					<div class="contactList">
						<div class="contactHeader">
						    <span style="float:left;left:0px;width:215px">
							    Imported <strong><%=CountactCount%></strong> contacts 
							</span>
							
							<span id="sSelect1"  style="display:none;float:left;background: transparent url(images/arrow-select-down.gif) no-repeat scroll left center; padding-left: 10px; font-size: 11px;">
							    <a href="javascript:selectEmail(this,true);void(0);" class="selectAll">All</a>
                                <a href="javascript:selectEmail(this,false);void(0);" class="selectNone">None</a>
                            </span>
                              <span style="float:left;">&nbsp;</span>
                            <span id="sSelect2" style="display:none;float:left;background: transparent url(images/arrow-select-down.gif) no-repeat scroll left center; padding-left: 10px; font-size: 11px;">
							    <a href="javascript:selectFriend(this,true);void(0);" class="selectAll">All</a>
                                <a href="javascript:selectFriend(this,false);void(0);" class="selectNone">None</a> 
                            </span>
                             
                             
							<%--<span style="font-size:11px;display:none;" id="sSelect">
							Select all 
							<input type="checkbox" id="checkAllEmail" onchange="selectAllEmail(this);"> Email Invite &nbsp;&nbsp;
							<input type="checkbox" id="checkAllFriends" onchange="selectAllFriend(this);"> Friend Request</span>--%>
						</div>
						<div class="subcatHolder" id="mainContactList">
								
							<%if (ImportStage) { %>				
							<div style="text-align: center; width: 200px; position: relative; left: 100px; top: 150px;">
				            <p><img src="http://www.next2friends.com/images/contactloading.gif" alt="loading contacts" /></p>
				            <p>Loading your contacts</p>
				            </div>
						    <%}else{ %>
						    <div style="text-align: center; width: 250px; position: relative; left: 75px; top: 150px;">
				            <p><%=ContactBoxMessage %></p>
				            </div>
						    <%} %>
						</div>
					</div>

				</div>
				
				<!-- NEW CONTENT ENDS HERE -->
				
				
	    </div>
		<!-- page end -->
    </div>

    
    
	<script type="text/javascript">
	<%if(ImportStage){ %>
	 $(document).ready(function() {
        ImportEmails.GetContact(getContacts_Callback);
     });
     
     function getContacts_Callback(response){
         if(response.error==null){
            $("#mainContactList").html(response.value)
            
            $("#sSelect1").css('display','');
            $("#sSelect2").css('display','');
                        
	        $(".fchb").click(function(){
		        checkSibling(this);
	        });

	        $(".echb").click(function(){
		        uncheckSibling(this);
	        });
	        	        
	        $('#ContactLister').localScroll({
               target:'#mainContactList'
            });
         }
     }
     
     <%} %>
     function selectEmail(chb,selectAll){
 
        var emailInvite = $('.echb');
       
        for (var i=0;i<emailInvite.length;i++){
            emailInvite[i].checked = selectAll;
        }
           
        if(!selectAll){
             var friendInvite = $('.fchb');
            for (var i=0;i<friendInvite.length;i++){
                friendInvite[i].checked = false;
            }
        }
     }
     
     function selectFriend(chb,selectAll){

        var friendInvite = $('.fchb');
        
        for (var i=0;i<friendInvite.length;i++){
            friendInvite[i].checked = selectAll;
        }
        
        if(selectAll){
            var emailInvite = $('.echb');
           
            for (var i=0;i<emailInvite.length;i++){
                emailInvite[i].checked = selectAll;
            }
        }
           
     }
     
    function checkSibling(chb){
         if(chb.checked){
                var emailChb = $('#e'+chb.id.substring(1));
                if(emailChb[0]!=null){
                    emailChb[0].checked = true;
                }
          }
     }
     
    function uncheckSibling(chb){
         if(!chb.checked){
                var friendChb = $('#f'+chb.id.substring(1));
                if(friendChb[0]!=null){
                    friendChb[0].checked = false;
                }
          }
     }

    function postInvite() 
    {
    
         var myForm = document.createElement("form");
         myForm.method="post" ;
         myForm.action = "/import" ;
                      
         var emailInvite = $('.echb');
         var friendInvite = $('.fchb');
         
         var friendValues = '';
         var emailValues = '';
     
         for (var i=0;i<friendInvite.length;i++){
            if(friendInvite[i].checked){
                 friendValues += friendInvite[i].id+',';
             }
         }
          
         for (var i=0;i<emailInvite.length;i++){
            if(emailInvite[i].checked){
                emailValues +=emailInvite[i].id+',';
             }
         }
          
         var myInput = document.createElement("input");
         myInput.setAttribute("type", "hidden");
         myInput.setAttribute("name", "friendlist") ;
         myInput.setAttribute("value", friendValues);
         myForm.appendChild(myInput) ;
         
         var myInput1 = document.createElement("input");
         myInput.setAttribute("type", "hidden");
         myInput1.setAttribute("name", "emaillist") ;
         myInput1.setAttribute("value", emailValues);
         myForm.appendChild(myInput1) ;
         
         document.body.appendChild(myForm) ;
         myForm.submit() ;
    }
	 </script>




</asp:Content>
