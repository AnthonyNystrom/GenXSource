<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ForgottenPassword.aspx.cs" Inherits="ForgottenPasswordPage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<!-- middle start -->
	<div id="middle" class="clearfix no_subnav">

		<!-- page start -->
		<div class="page clearfix">
		
				<h2>Forgot your password?</h2>
				
				<%if (PasswordSent)
                  { %>
            				    
			                    Thank you, Your password will be emailed to you in a few moments.
            				
			                <%}
                  else
                  { %>
				<div class="login_form">
					<div class="top"></div>
					
					<div id="login_form">
						
						<asp:Literal runat="server" ID="libMessage"><p style="width:280px">
						    Please enter your email address and we will send you your password</p>
						</asp:Literal>

						<p>
							<label for="login_email">Email</label>
							<asp:TextBox runat="server" MaxLength="60" ID="txtEmail" CssClass="form_txt"></asp:TextBox>
						
						</p>
						<p class="indent">
							<input type="button" value="Send" class="form_btn" onclick="__doPostBack('<%=btnSend.UniqueID %>', '');return false;" />
					        <asp:Button runat="server" ID="btnSend" OnClick="btnSend_Click" CssClass="hiddenButton" Text="Login" />
						</p>
					</div>

				</div>
				
				<%} %>
		
		</div>
		<!-- page end -->

	</div>
	<!-- middle end -->
	
</asp:Content>

