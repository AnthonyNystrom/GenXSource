<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ReportAbuse.aspx.cs" Inherits="ReportAbusePage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<!-- middle start -->
	<div id="middle" class="clearfix no_subnav">
        <div class="page clearfix fullPageBkg">
		<!-- subnav end -->

			
			<h2>Report Abuse and Copyright infringements</h2>
			

			
			<%if (AbuseCompleted){ %>
                    <p>Thank you<br /><br />
                    We have received your report and we will review it immediately.</p>
            <%}else{ %>
            
                <p>It is important that Next2Friends provides the highest quality of Videos and 
                photos to our members. However, Occationally some members upload copyrighted 
                content or material that some viewers may find offensive.</p>
                
                <p>If you feel that the content you were viewing is abusive or copyrighted then 
                please give us a quick note as to as the nature of the video or photo and we 
                will review it immediatly.</p>
                
                <p>Thanks for your help, we couldnt keep Next2Friends clean and fun without you.</p>
                <div style="width:819px; text-align:right;">
                    <p><asp:TextBox runat="server" CssClass="form_txt" TextMode="MultiLine" Width="100%" Height="150" id="TxtAbuseNotes"></asp:TextBox></p>
                    <p><asp:Button runat="server" CssClass="form_btn" Text="Report" ID="btnReportAbuse" /></p>
                </div>
           
            <%}%>
			
        </div>
	</div>

	<!-- middle end -->
</asp:Content>

