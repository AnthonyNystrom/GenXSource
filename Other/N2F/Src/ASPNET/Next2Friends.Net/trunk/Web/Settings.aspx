<%@ Page Language="C#" MasterPageFile="main.master"  AutoEventWireup="true"  CodeFile="Settings.aspx.cs" Inherits="SettingsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <script type="text/javascript" src="/lib/jquery_thickbox.js"></script>
       <script type="text/javascript" src="js/settings.js"></script>
       <style type="text/css" media="all">@import "/lib/Thickbox.css";</style>
 
    <!-- middle start -->
	<div id="middle" class="no_subnavclearfix">

		<!-- page start -->
		<div class="page clearfix fullPageBkg">
		
				<h2>Next2Friends Settings</h2>
				<p>Once you are happy just click save.</p>
				
				<div id="settings_tabs">
				    <div class="long-tab">
				        <ul class="tab_nav">
					        <li><a href="/Settings.aspx" class="current" id="settingsTab1">General</a></li>
					        <li><a href="/Accounts.aspx" id="settingsTab2">Accounts</a></li>
				        </ul>
				        <div class="settings_tab_content">
                            <div class="settings_area_large">
					            <div class="top"></div>
					            <div class="firstHalfColumn">
					            <p><span class="bottomSpace" style="font-weight:bold">Notification Settings</span></p>
						                <p>
							                <asp:CheckBox runat="server" ID="chbNewMessage" Checked="true"></asp:CheckBox>
								            <label for="notify">Notify me by email on new Message</label>
							            </p>
							            <p>
							                <asp:CheckBox runat="server" ID="chbNewAAFComment"  Checked="true"></asp:CheckBox>
								            <label for="notify">Notify me by email on new AAF Comment</label>
							            </p>
							            <p>
							                <asp:CheckBox runat="server" ID="chbNewFriendRequest" Checked="true" ></asp:CheckBox>
								            <label for="notify">Notify me by email on new Friend Request</label>
							            </p>
							            <p>
							                <asp:CheckBox runat="server" ID="chbSubscriberEvent" Checked="true" ></asp:CheckBox>
								            <label for="notify">Notify me by email on new Subscriber Event</label>
							            </p>
							            <p>
								            <asp:CheckBox runat="server" ID="chbNewProfileComment" Checked="true" ></asp:CheckBox>
								            <label for="notify">Notify me by email on new profile comment</label>
							            </p>
							            <p>
								            <asp:CheckBox runat="server" ID="chbNotifyNewPhotoComment" Checked="true" ></asp:CheckBox>
								            <label for="notify">Notify me by email on new Photo comment</label>
							            </p>
							            <p>
								            <asp:CheckBox runat="server" ID="chbNotifyNewVideoComment" Checked="true" ></asp:CheckBox>
								            <label for="notify">Notify me by email on new Video comment</label>
							            </p>
							            <p>
								            <asp:CheckBox runat="server" ID="chbNotifyNewVideo" Checked="true" ></asp:CheckBox>
								            <label for="notify">Notify me by email on new Videos</label>
							            </p>
														            <p>
								            <asp:CheckBox runat="server" ID="chbNotifyNewBlog" Checked="true" ></asp:CheckBox>
								            <label for="notify">Notify me by email on new Blogs</label>
							            </p>
							            <p class="indent">
							                <asp:CheckBox runat="server" ID="chbNewsLetter" Checked="true" ></asp:CheckBox>
								            <label for="notify">Receive News Letter</label>
							            </p>
            						
					            </div>
					            <div class="secondHalfColumn">
					                <%if (!IsSignup){ %>
						            <p>
						                <span class="<%=EmailMessageCss %>"><%=EmailErrorMessage%></span>
							            <strong>Update Email Address</strong>
					               </p>
						            <p>
							            <label>Email</label>
							            <asp:TextBox runat="server" ID="txtEmail" class="form_txt" ></asp:TextBox>
						            </p>
						            <p>
            						
							            <span class="<%=PasswordMessageCss %>"><%=PasswordErrorMessage%></span>
							            <strong>Change Password</strong>
            						
						            </p>
						            <p>
							             <label>Old Password</label>
                                         <asp:TextBox TextMode="Password" runat="server" ID="txtOldPassword" class="form_txt"></asp:TextBox>   
						            </p>
						            <p>
							            <label>New Password</label>
							            <asp:TextBox TextMode="Password" runat="server" ID="txtNewPassword" class="form_txt"></asp:TextBox>  
						            </p>
						            <p>
							            <label>Repeat Password</label>
                                        <asp:TextBox TextMode="Password" runat="server" ID="txtConfirmPassword" class="form_txt"></asp:TextBox>  
						            </p>
						            <p class="right">
							            Your password must be at least 7 characters long
						            </p>
						            <%} %>
						            <div class="clear"></div>
						            <p>
							            <strong>Upload Profile photo</strong>
					               </p>
					               <p style="vertical-align:top;">
					                  <p class="profile_pic">
					                    <img style="float:left;width:50px;height:50px" alt="profile photo" src="http://www.next2friends.com/<%=PhotoURL %>"/>
					                  </p> &nbsp
					                  <a href="/cphotopicker.aspx?m=<%=member.WebMemberID %>&keepThis=false&TB_iframe=true&height=480&width=630" style="cursor:pointer;" title="Upload and frame your profile photo" class="thickbox">Click here to Upload your profile photo</a>
            					        
					               </p>
					            </div>
					            <div class="clear"></div>
					            <p class="align_right" style="margin-right:30px;">
            					        
							            <input type="button" value="Save" class="form_btn" onclick="__doPostBack('<%=btnSave.UniqueID %>', '');return false;" />
							            <asp:Button id="btnSave" Text="Save" runat="server" CssClass="hiddenButton" onclick="btnSave_Click" />
					            </p>
					            <div class="bottom"></div>
				            </div>           
				        </div>
			        </div>
			    </div>
	</div>
	</div>
    <script type="text/javascript">
        function showProfilePhoto(){
             npopup('Upload Profile Photo',"<iframe style='height:500px;width:700px' src='/cphotopicker.aspx?m=<%=member.WebMemberID %>&keepThis=false&TB_iframe=true&height=480&width=630'>",720,550);
        }
    </script>

</asp:Content>