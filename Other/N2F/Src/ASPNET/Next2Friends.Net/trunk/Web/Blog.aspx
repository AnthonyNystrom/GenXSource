<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="Blog.aspx.cs" Inherits="Blog" %>
<%@ MasterType VirtualPath="~/View.master" %>
<%@ Register src="Comments.ascx" tagname="Comments" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftColContentHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftUpperContentHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="RightColContentHolder" runat="Server">
<script language="javascript" src="/js/blog.js"></script>
<!-- box start -->
				<!-- box start -->
				<div class="profile_box">
				    <div id="divEditBlog" style="width:623px;height:355px;display:none;">
				        <input type="hidden" id="txtWebBlogId" value="<%=strBlogID %>" />
				        
				        <p>Title<textarea id="txtBlogTitle" rows="1" class="form_txt2"  cols="8" style="height:20px;width: 100%;"></textarea><textarea id="txtBlogTitleSource" rows="1" class="form_txt2"  cols="8" style="visibility:hidden;display:none;height:20px;width: 100%;"><%=BlogTitle %></textarea></p>
                        <p>Entry<br /><textarea id="txtBlogBody" rows="15" class="form_txt2"  cols="8" style="width: 100%;"></textarea><textarea id="txtBlogBodySource" rows="15" class="form_txt2"  cols="8" style="visibility:hidden;display:none;width: 100%;"><%=BlogEditableBody%></textarea></p>

                        <p class="align_right">
                            <input type="button" id="btnCancel" class="form_btn2" value="Cancel" onclick="cancelEditBlog();" />
                            <input type="button" id="btnEdit" class="form_btn2" value="Update" onclick="ajaxEditBlog();" />
                            <input type="button" id="btnPost" class="form_btn2" value="Post" onclick="ajaxPostBlog();" />  
                        </p>                      
                    </div>
                    <div id="divBlog">
                        <%if (IsMyPage)
                          { %>
                        
                        <p class="right">
                         <% if (HasContent)
                            {%>
                            <a href="javascript:showEditBlog();">Edit</a>  |  
                            <a href="javascript:crossPostOptions();">Cross post</a>  |  
                        <%} %>
                            <a href="javascript:showPostBlog();">+ Post New</a></p>
                            
                        <%} %>
                        
                        <% if (HasContent)
                           {%>
                        
					    <h2 class="left" id="blogTitle"><%=BlogTitle %></h2>
					
					    <p class="clear"><small><%=BlogCreationDt %></small></p>

    					
					    <p id="blogBody"><%=BlogBody %></p>
					    <%}
                           else
                           {%>
                            <p >Member currently has no blog entries</p>   
                           <%} %>
				    </div>
                </div>		
                
                <script type="text/javascript">
		            function crossPostOptions(){
                        npopup('Auto submit your blog ',"<div id='divOptionsHTML'>Loading...</div>",460,170);
                        Blog.GetCrossPostOptions(crossPostOptions_callback);
                    }
                    
                    function crossPostOptions_callback(response){
                        if(response.error==null){ 
                            $('#divOptionsHTML').html(response.value)
                        }
                    }

                    function saveLogin(){
                       $('#btnCp').attr('disabled','true');
                        var blog = $('#drpSelect').val();
                        var username = $('#txtUserName').val();
                        var password = $('#txtPassword').val();
                        var wpaddress = $('#txtAddress').val();
                        var autosub = $('#chbAuto').get(0).checked;

                        Blog.CrossPost(blog,username,password,wpaddress,autosub,saveLogin_callback);
                    }
                    
                    
                    function saveLogin_callback(response){
                        if(response.error==null){ 
                            if(response.valueOf==0){
                                $('#divMsg').html('Saved'); 
                                setMessagecss(1);
                            }else{
                                $('#divMsg').html('There was an error'); 
                                setMessagecss(2);
                            }
                        }else{
                            alert('oops, something went wrong, please try again');
                        }
                        $('#btnCp').removeAttr('disabled');
                    }		
                    
                    function blogselect(){
                        var selected = $('#drpSelect').val();
                        hold(true,selected);

                        Blog.GetCrossPostValues(selected,blogselect_Callback);
                    }
                    
                    function blogselect_Callback(response,args){
                        if(response.error==null){ 
                            $('#txtUserName').val(response.value[0]);
                            $('#txtPassword').val(response.value[1]);
                            $('#txtAddress').val(response.value[2]);
                            $('#chbAuto').get(0).checked = response.value[3];
                        }
                        $('#divMsg').html('Enter your blog login details');
                        setMessagecss(0);
                        hold(false,args.args.BlogService);
                    }
                    
                    function hold(disable,selected){
                        if(disable){
                            $('#btnCp').attr('disabled',true);
                            $('#txtUserName').attr('disabled',true);
                            $('#txtPassword').attr('disabled',true);
                            $('#txtAddress').attr('disabled',true);
                            $('#chbAuto').attr('disabled',true);
                            if(selected=='wordpress'){
                                $('#lblWp').css('color','#000000');
                            }
                        }else{
                            $('#btnCp').removeAttr('disabled');
                            $('#txtUserName').removeAttr('disabled');
                            $('#txtPassword').removeAttr('disabled');
                            if(selected=='wordpress'){
                                $('#txtAddress').removeAttr('disabled');
                                $('#lblWp').css('color','#BBBBBB');
                            }
                            $('#chbAuto').removeAttr('disabled');
                        }
                    
                    }
                    
                    function setMessagecss(type){
                        if(type==0){
                            $('#frmMsg').toggleClass('formLoginMessage');
                        }else if(type=1){
                            $('#frmMsg').toggleClass('formSuccessMessage');
                        }else if(type=2){
                            $('#frmMsg').toggleClass('formErrorMessage');
                        }
                     
                    }
		            </script>
				
				<!-- box end -->
				<!-- box start -->
				 <% if (HasContent)
                 {%>
				<div class="profile_box" runat="server" id="divComments">
                    <uc1:Comments ID="Comments1" runat="server" />
                </div>
                <%} %>
				<!-- box end -->	
				<%=DefaultHTMLPager %>			
</asp:Content>