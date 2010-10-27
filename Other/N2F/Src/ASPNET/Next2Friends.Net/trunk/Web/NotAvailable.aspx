<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="NotAvailable.aspx.cs" Inherits="NotAvailable" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<!-- middle start -->
	<div id="middle" class="clearfix">
		<!--subnav start -->
		<ul id="subnav" class="clearfix">
		</ul>
		<!-- subnav end -->

		<!-- page start -->
		<div class="page clearfix fullPageBkg">
			<!-- signup col start -->
			<div class="signup_col">
				<h2>Not available</h2>
				<p>
					<%=Message %> <br />
				    <a href="<%=Referer %>">Go back</a>
				</p>
			</div>
		<!-- page end -->
		</div>
	</div>
	
	
</asp:Content>




