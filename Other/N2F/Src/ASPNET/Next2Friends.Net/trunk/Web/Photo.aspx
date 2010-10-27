<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Photo.aspx.cs" Inherits="PhotoPage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="middle" class="topbar clearfix fullPageBkg">
	<div class="fullPageBkg">
			
				<div class="lister_top clearfix">
				</div>

				
				<div class="clearfix">				
					<div class="lister_topbar clearfix">
																    
							<div class="right">					    
								<%=DefaultHTMLPager %>

								</div>
								
					</div><!--/lister topbar -->

					<ul class="videos_list clearfix" id="ulContentLister">
						
						<%=DefaultHTMLLister %>

					</ul>
					
					<div class="right">		
						<%=DefaultHTMLPager %>
					</div>

				</div>

			</div>
			
			</div>

<script src="/js/Photo.js"></script>
</asp:Content>

