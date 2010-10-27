<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="FeedbackPage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<!-- middle start -->
	<div id="middle" class="clearfix no_subnav">
        <div class="page clearfix fullPageBkg">
		<!-- subnav end -->

			
			<h2>Help us make Next2Friends better</h2>
			
			<%if (FeedbackCompleted){ %>
                    <p>Thank you,<br /><br />
                    We have received your feedback and we will review it as soon as we can.</p>
            <%}else{ %>
            
                <p>xxxxxxxxxxxxxxxxxx</p>
                
                <p>Thanks for your help, we couldnt keep Next2Friends clean and fun without you.</p>
                <div style="width:819px; text-align:left;">
                    <p>Name: <asp:TextBox runat="server" ID="txtName" Width="150px"></asp:TextBox></p>
                    <p>Email: <asp:TextBox runat="server" ID="txtEmail" Width="150px"></asp:TextBox></p>
                    <p><asp:TextBox runat="server" CssClass="form_txt" TextMode="MultiLine" Width="100%" Height="150" id="txtFeedback"></asp:TextBox></p>
                </div>
                <div style="width:819px; text-align:right;">
                     <p><asp:Button runat="server" CssClass="form_btn" Text="Post" ID="btnFeedback" /></p>
                </div>
           
            <%}%>
			
        </div>
	</div>

	<!-- middle end -->
</asp:Content>

