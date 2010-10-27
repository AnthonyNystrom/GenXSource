<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="VUpload.aspx.cs" Inherits="VUpload" Title="Untitled Page" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.QuickStart" Assembly="Telerik.QuickStart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.WebControls" Assembly="RadUpload.NET2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
 <script src="js/Upload.js"></script>

<!-- middle start -->
	<div id="middle" class="no_subnav clearfix">

			<div class="page clearfix fullPageBkg">
			
				
					<h2>Upload Videos</h2>
						<p>
							Next2Friends support the following formats:
						</p>
	                    <ul type="square">
						    <li>Windows Media (AVI, WMV, ASF)</li>
						    <li>QuickTime (MOV, QT, DV)</li>
						    <li>MPEG (MPEG-1, MPEG-2, MPEG-4)</li>
						    <li>Mobile phone formats (3GP, 3G2).</li>
                        </ul>
                        <p>Click Add Files, make your selection then click Upload.</p>
						
                    <input type="file" runat="server" id="inputFile" />
                    
                    <rad:radprogressmanager id="Radprogressmanager1" SkinID="Gold" SuppressMissingHttpModuleError="false" runat="server" />
                    <rad:radprogressarea id="RadProgressArea1" runat="server"></rad:radprogressarea>
                    <asp:button id="buttonSubmit" OnClick="buttonSubmit_Click" runat="server" text="Submit" cssclass="RadUploadButton" />
			      
			</div>
			
		


	</div>

	<!-- middle end -->


</asp:Content>

