<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="UploadPhoto.aspx.cs" Inherits="UploadPhotoPage" Title="Upload Photos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" EnablePageMethods="true"></asp:ScriptManager>
 <script src="js/Upload.js"></script>

<!-- middle start -->
	<div id="middle" class="no_subnav clearfix">

			<!-- lister left start -->
			<div class="lister_leftcol">
			
				<div class="lister_top clearfix">
					<h2>Upload photos</h2>
				</div>
				
				<div class="lister_middle clearfix">				
					<div class="lister_topbar clearfix"></div>

					<ul class="videos_list clearfix" id="ulContentLister">
						<p>Next2Friends welcomes your uploads </p>
						<p>We currently support JPG, PNG and GIF images.</p>
                        <p>Click Add Files, make your selection (You can add up to 15 files at a time), then click Upload.</p>
						
						<div>

                        <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"
                         codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0"
                         width="100%" height="375" id="fileUpload" align="middle">
                            <param name="allowScriptAccess" value="sameDomain" />
                            <param name="movie" value="swf/FlashFileUpload.swf" />
                            <param name="quality" value="high" />
                            <param name="wmode" value="transparent">
                            <PARAM NAME=FlashVars VALUE='uploadPage=Upload.axd<%=GetFlashVars()%>&completeFunction=UploadComplete()'>
                            <embed src="swf/FlashFileUpload.swf"
                             FlashVars='uploadPage=Upload.axd<%=GetFlashVars()%>&completeFunction=UploadComplete()'
                             quality="high" wmode="transparent" width="100%" height="375" 
                             name="fileUpload" align="middle" allowScriptAccess="sameDomain" 
                             type="application/x-shockwave-flash" 
                             pluginspage="http://www.macromedia.com/go/getflashplayer" />
                        </object>

                        
                    </div>
						<asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
						
					</ul>
					


				</div>

			</div>
			<!-- lister left end -->


			<!-- profile right start -->
			<div class="lister_rightcol">
		<%--		<h4>Video Category</h4>
				<ul class="sidebar_nav"><li><a href="#">Arts &amp; Animation</a></li>

					<li><a href="#"> Auto &amp; Vehicles </a></li>
					<li><a href="#">Comedy</a></li>
					<li><a href="#">Entertainment</a></li>
					<li><a href="#">Extreme</a></li>
					<li><a href="#">Music</a></li>

					<li><a href="#">News</a></li>
					<li><a href="#">People</a></li>
					<li><a href="#">Pets &amp; Animals</a></li>
					<li><a href="#">Science &amp; Technology</a></li>
					<li><a href="#">Sports</a></li>

					<li><a href="#">Travel &amp; Places</a></li>
					<li><a href="#">Video Blogs</a></li>
					<li><a href="#">Video Comments</a></li>
					<li><a href="#">Video Games</a></li>
				</ul>
				<h4>Extra Menu?</h4>

				<ul class="sidebar_nav"><li><a href="#">Arts &amp; Animation</a></li>
					<li><a href="#">News</a></li>
					<li><a href="#">Pets &amp; Animals</a></li>
					<li><a href="#">Science &amp; Technology</a></li>

					<li><a href="#">Sports</a></li>
					<li><a href="#">Video Blogs</a></li>
					<li><a href="#">Video Comments</a></li>
				</ul>--%>
			</div>
			<!-- profile right end -->
			
		


	</div>

	<!-- middle end -->


</asp:Content>



