<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DisplayBlog.ascx.cs" Inherits="DisplayBlog" %>
<%@ Register src="Comments.ascx" tagname="Comments" tagprefix="uc1" %>
<%@ Register src="~/UserControls/ForwardToFriend.ascx" tagname="ForwardToFriend" tagprefix="uc2" %>
<script language="javascript" src="/js/blog.js"></script>
<!-- box start -->
		<div class="profile_box">
		    <div id="divEditBlog<%=WebBlogID %>" style="width:623px;height:355px;display:none;">
		        <p>Title<textarea id="txtBlogTitle<%=WebBlogID %>" rows="1" class="form_txt2"  cols="8" style="height:20px;width: 100%;"></textarea><textarea id="txtBlogTitleSource<%=WebBlogID %>" rows="1" class="form_txt2"  cols="8" style="visibility:hidden;display:none;height:20px;width: 100%;"><%=BlogTitle %></textarea></p>
                <p>Entry<br /><textarea id="txtBlogBody<%=WebBlogID %>" rows="15" class="form_txt2"  cols="8" style="width: 100%;"></textarea><textarea id="txtBlogBodySource<%=WebBlogID %>" rows="15" class="form_txt2"  cols="8" style="visibility:hidden;display:none;width: 100%;"><%=BlogEditableBody%></textarea></p>

                <p class="align_right">
                    <input type="button" id="btnCancel<%=WebBlogID %>" class="form_btn2" value="Cancel" onclick="cancelEditBlog('<%=WebBlogID %>');" />
                    <input type="button" id="btnEdit<%=WebBlogID %>" class="form_btn2" value="Update" onclick="ajaxEditBlog('<%=WebBlogID %>');" />
                    <input type="button" id="btnPost<%=WebBlogID %>" class="form_btn2" value="Post" onclick="ajaxPostBlog();" />  
                </p>                      
            </div>
            <div id="divBlog">
                
                <%if (IsMyPage)
                  { %>             
                <p class="right">                                 
                    <a href="javascript:showEditBlog('<%=WebBlogID %>');">Edit</a>
                    <a href="javascript:crossPostOptions('<%=WebBlogID %>');">Cross post</a>
                <%} %>
                <h2 class="left" id="blogTitle<%=WebBlogID %>"><%=BlogTitle%></h2>
			    
			    <p class="clear"><small><%=BlogCreationDt%></small></p>				
			    <p id="blogBody<%=WebBlogID %>"><%=BlogBody%></p>			    
		    </div>
        </div>				
		<!-- box end -->
		<!-- box start -->
		<div class="profile_box" runat="server" id="divComments">
            <uc1:Comments ID="Comments1" runat="server" />
        </div>
		<!-- box end -->
		<uc2:ForwardToFriend ID="forwardToFriend" runat="server" />	
		
		<script type="text/javascript">
		var webBlogID = '';
		function crossPostOptions(WebBlogID){
		    webBlogID = WebBlogID;
            npopup('Transfer to your blog ',"<div id='divOptionsHTML'>Loading...</div>",535,115);
            DisplayBlog.GetCrossPostOptions(crossPostOptions_callback);
        }
        
        function crossPostOptions_callback(response){
            if(response.error==null){ 
                $('#divOptionsHTML').html(response.value)
            }
        }

        function CrossPost(){
        
           $('#btnCp').attr('disabled','true');
            var blog = $('#drpSelect').val();
            var username = $('#txtUserName').val();
            var password = $('#txtPassword').val();
            var wpaddress = $('#txtAddress').val();

            DisplayBlog.CrossPost(webBlogID, blog,username,password,wpaddress,crossPost_callback);
        }
        
        function crossPost_callback(response){
            if(response.error==null){ 
                $('#divMsg').html(response.value); 
                if(response.value==''){
                    closePopup();
                }
            }
            
            $('#btnCp').removeAttr('disabled');
        }		
		</script>