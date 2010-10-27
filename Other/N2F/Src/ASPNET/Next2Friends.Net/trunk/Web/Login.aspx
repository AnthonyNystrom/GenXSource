<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!-- middle start -->
	<div id="middle" class="no_subnav clearfix">



		<!-- page start -->
		<div class="page clearfix fullPageBkg">
		
			<!-- login col start -->
			<div class="login_col">
				<h2>Member Login</h2>
				<p>Login to Next2Friends here.</p>
				<p>Arent already a member? <a href="/signup.aspx">signup here</a></p>
				
				<div class="login_form">
					<div class="top"></div>
					
					<div id="login_form">
						<asp:Literal runat="server" ID="errLogin"></asp:Literal>
						<p>
							<label for="login_email">Email</label>
							<asp:TextBox runat="server" Cssclass="form_txt" ID="txtEmailLogin" ></asp:TextBox>
						</p>
						<p>
							<label for="login_password">Password</label>
							<asp:TextBox TextMode="Password" runat="server" Cssclass="form_txt" ID="txtPasswordLogin" ></asp:TextBox>
						</p>
						<p class="indent">
						    <input type="button" value="Login" class="form_btn" onclick="__doPostBack('<%=btnLogin.UniqueID %>', '');return false;" />
						    <asp:Button runat="server" id="btnLogin" Text="Login" CssClass="hiddenButton" onclick="btnLogin_Click" /><asp:CheckBox runat="server" ID="chbRememberMe"/> Remember me
					</p>
					<p class="indent"><small><a href="/ForgottenPassword.aspx">Forgot your password?</a> <br /></small></p></div>
			</div>
			<!-- login col end -->
		
		</div>
		<!-- page end -->

	</div>
	<!-- middle end -->
	</div>		
</asp:Content>

