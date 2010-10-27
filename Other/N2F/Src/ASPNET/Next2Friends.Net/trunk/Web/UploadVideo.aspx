<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="UploadVideo.aspx.cs" Inherits="UploadVideo" Title="Untitled Page" %>
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
						
						<div>

                        <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"
                         codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0"
                         width="100%" height="375" id="fileUpload" align="middle">
                            <param name="allowScriptAccess" value="sameDomain" />
                            <param name="movie" value="swf/FlashFileUpload.swf" />
                            <param name="quality" value="high" />
                            <param name="wmode" value="transparent">
                            <param name="FlashVars" value='uploadPage=Upload.axd<%=GetFlashVars()%>&completeFunction=UploadComplete()'>
                            <embed src="swf/FlashFileUpload.swf"
                             FlashVars='uploadPage=Upload.axd<%=GetFlashVars()%>&completeFunction=UploadComplete()'
                             quality="high" wmode="transparent" width="100%" height="375" 
                             name="fileUpload" align="middle" allowScriptAccess="sameDomain" 
                             type="application/x-shockwave-flash" 
                             pluginspage="http://www.macromedia.com/go/getflashplayer" />
                        </object>

                        
                    </div>
						<asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
					

                   <input type="file" runat="server" id="inputFile">
                    
                    <rad:radprogressmanager id="Radprogressmanager1" SuppressMissingHttpModuleError="false" runat="server" />
                    <rad:radprogressarea id="RadProgressArea1" runat="server"></rad:radprogressarea>
                    <asp:button id="buttonSubmit" runat="server" text="Submit" cssclass="RadUploadButton" />
			      
			</div>
			
		


	</div>

	<!-- middle end -->


</asp:Content>

