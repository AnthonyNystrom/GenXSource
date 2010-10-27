<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="AAFUpload.aspx.cs" Inherits="AAFUpload" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<!-- middle start -->
	<div id="middle" class="clearfix">

<%--		<!--subnav start -->
		<ul id="subnav" class="clearfix">
			<li><a href="AskAFriend.aspx">Play!</a></li>
			<%if (IsLoggedIn){%>
			<li><a href="MyAskAFriend.aspx"><img src="images/BtnMyQuestions.gif" /></a></li>
			<li><a href="AAFUpload.aspx"><img src="images/btnNewQuestion.gif" /></a></li>
			<%}else{%>
			<li><a href="Signup.aspx"><img src="images/btnNewQuestion.gif" /></a></li>
			<%} %>
		</ul>
		<!-- subnav end -->--%>

		<!--subnav start -->
		<ul id="subnav" class="clearfix">
			<li><a href="/ask">Play!</a></li>
			<%if (IsLoggedIn){%>
			<li><a href="/MyAskAFriend.aspx">My Questions</a></li>
			<li><a href="/AAFUpload.aspx">New Question</a></li>
			<%} %>
		</ul>
		<!-- subnav end -->


		<!-- page start -->
		<div class="page clearfix">
		


			<h2>Ask a Question</h2>
			<p>Ask a Friend allows you to ask any question in response to a photo or photos. You 
                can choose to ask the entire Next2Friends community or make it private and just 
                your friends. You can also use Ask a Friend from your mobile and get results in 
                real time too!</p>

			<asp:Panel runat="server" ID="panelUpload">
			<div class="wizard">
				<p>
					<label>Question</label>
					<asp:TextBox runat="server" ID="txtQuestion" CssClass="form_txt" Width="360"></asp:TextBox><asp:Literal runat="server" ID="litErrQuestion" Text=""></asp:Literal>

				</p>
				<p>
					<label>Response Type</label>
					
					<asp:RadioButton id="rbYesNo" onclick="enableOptions(1);" runat="server" TextAlign="right" GroupName="rbgResponse" Checked="False"/> Yes / No &nbsp;
					<asp:RadioButton id="rbImageSelect" onclick="enableOptions(2);" runat="server" TextAlign="right" GroupName="rbgResponse" Checked="False"/> Image select &nbsp;
					<asp:RadioButton id="rbRate110" onclick="enableOptions(3);" runat="server" TextAlign="right" GroupName="rbgResponse" Checked="False"/> Rate 1-10 &nbsp;
					<asp:RadioButton id="rbCustom" onclick="enableOptions(4);" runat="server" TextAlign="right" GroupName="rbgResponse" Checked="False"/> Custom &nbsp; <asp:Literal runat="server" ID="litErrResponse" Text=""></asp:Literal>
					
				</p>
				<div id="divCustom" <%=divCustomShowHide %>>
				    <p>
					    <label>Custom A</label>
					    <asp:TextBox runat="server" Width="236" ID="txtCustomA" CssClass="form_txt"></asp:TextBox><asp:Literal runat="server" ID="libCustomA" Text=""></asp:Literal>
				    </p>
				    <p>
					    <label>Custom B</label>
					    <asp:TextBox runat="server" Width="236" ID="txtCustomB" CssClass="form_txt"></asp:TextBox><asp:Literal runat="server" ID="libCustomB" Text=""></asp:Literal>
				    </p>
				</div>
				<p>
					<label>Photo <span id="spanPhotoNo"><%=spanPhotoNoValue%></span></label>
					<asp:FileUpload runat="server" ID="FileUpload1" CssClass="form_txt"/><asp:Literal runat="server" ID="litFileUpload1" Text=""></asp:Literal>
				</p>
				<div  id="divMulti" <%=divMultiShowHide %>>
				    <p>
					    <label>Photo 2</label>
					    <asp:FileUpload runat="server" ID="FileUpload2" CssClass="form_txt"/><asp:Literal runat="server" ID="litFileUpload2" Text=""></asp:Literal>
				    </p>
				    <p>
					    <label>Photo 3</label>
					    <asp:FileUpload runat="server" ID="FileUpload3" CssClass="form_txt"/><asp:Literal runat="server" ID="litFileUpload3" Text=""></asp:Literal>
				    </p>
				</div>
				<p>
					<label>Friends Only</label>
					<asp:CheckBox runat="server" id="chbPrivate" Text="" /> Only my friends can view or vote this question
					
				</p>
				<p class="indent">
				<%--<input type="button" value="Preview" class="form_btn" onclick="__doPostBack('<%=btnPreview.UniqueID %>', '');return false;" /> or --%>
				     <input type="button" value="Submit now" class="form_btn" onclick="__doPostBack('<%=btnSubmit.UniqueID %>', '');return false;" /></p>
				<%--<asp:Button runat="server" ID="btnPreview" Text="Submit" CssClass="hiddenButton" onclick="btnPreview_Click" />--%>
				<asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="hiddenButton" onclick="btnSubmit_Click" />
			</div>
            </asp:Panel> 
             <asp:Literal runat="server" ID="litSuccessful" Visible="false">Your question has been submitted and will be live in a few monents</asp:Literal>       



		</div>
		<!-- page end -->



	</div>
	<!-- middle end -->
	<script type="text/javascript" src="lib/jQuery.js"></script>
	<script type="text/javascript" src="lib/interface.js"></script>
	<script type="text/javascript" src="js/AAFUpload.js"></script>
</asp:Content>

