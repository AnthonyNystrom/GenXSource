<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="MLive.aspx.cs" Inherits="MLive" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="LeftUpperContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftColContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightColContentHolder" Runat="Server">


				<!-- box start -->
				<div class="profile_box">
				 <%-- <a class="subscribeRSSLink" href="/rss.aspx?feed=live&nickname=<%=ViewingMember.NickName %>">subscribe</a>--%>
					<h4 class="box_title collapsible">Live Video</h4>
					<div class="collapsible_div">
					    <div style="text-align:center;">
	                          <p></p>  
	                        <script type="text/javascript" src="/SWFObject.js" language="javascript"></script>

                            <div id="flashcontent"></div>
                            <script type="text/javascript">	
                            var so = new SWFObject( "http://services.next2friends.com/livewidget/n2flw1.swf?c=1", "Main", "420", "320", "9.0.0", "#000000", true );
                            so.addParam( "scale", "showall" );
                            so.addParam( "allowScriptAccess", "always" );
                            so.addParam( "allowFullScreen", "true" );
                            so.addParam("wmode","opaque");
                            so.addVariable( "nickname", "<%=ViewingMember.NickName %>" );
                            <%=IsFriendKey %>
                            so.addVariable( "photo", "<%=PhotoURL %>" );
                            so.write( "flashcontent" );
                            </script> 
                            Embed code! <input type="text" onclick="this.select();" style="width:300px" value='<%=EmbedLink %>' class="form_txt"/><br /><br />
				        </div>
				        
				        <p></p>
				    </div>
				</div>
				
				<!-- box end -->
</asp:Content>

