<%@ Page Language="C#" MasterPageFile="main.master"  AutoEventWireup="true" CodeFile="Accounts.aspx.cs" Inherits="Accounts" %>

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
					        <li><a href="/Settings.aspx" id="settingsTab1">General</a></li>
					        <li><a href="/Accounts.aspx" class="current" id="settingsTab2">Accounts</a></li>
				        </ul>
				        <div class="settings_tab_content">
                            <div class="settings_area_large">
					            <div class="top"></div>
					            <div class="twitterSettings">
					                 <p>
						                <span></span>
							            <strong>Twitter</strong>
					               </p>
						            <p>
							            <label>Username</label>
							            <asp:TextBox runat="server" ID="txtTwitterUserName" class="form_txt"></asp:TextBox>
						            </p>
						            <p>
							             <label>Password</label>
                                         <asp:TextBox TextMode="Password" runat="server" ID="txtTwitterPassword" class="form_txt"></asp:TextBox>   
						            </p>
					            </div>
					            <p class="align_right" style="margin-right:446px;">
							            <input type="button" value="Validate" class="form_btn" />
							            <input type="button" value="Save" class="form_btn" onclick="__doPostBack('<%=btnSave.UniqueID %>', '');return false;" />
							            <asp:Button id="btnSave" OnClick="btnSave_click" Text="Save" runat="server" CssClass="hiddenButton" />
					            </p>
					            <div class="bottom"></div>
				            </div>           
				        </div>
			        </div>
			    </div>
	    </div>
	</div>
</asp:Content>
