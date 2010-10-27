<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForwardToFriend.ascx.cs" Inherits="ForwardToFriendCtrl" %>
<script language="javascript" type="text/javascript" src="/js/forwardToFriend.js?v=1"></script>
<div class="profile_box">            
    <h4 class="box_title collapsible collapsed">Forward To A Friend</h4>
    <div class="collapsible_div" style="display:none;">
                    <ul class="friends_list">
                        
				<li>
                    <label for="responseMessage" id="lblMessage">         
            
                    </label>
                </li>	
                <li><input type="text" id="txtEmail1" value="Email 1" onfocus="clearEmail(this);" onblur="outClear(this);"  class="form_txt_small" /></li>	
                <li><input type="text" id="txtEmail2" value="Email 2" onfocus="clearEmail(this);" onblur="outClear(this);" class="form_txt_small" /></li>	
                <li><input type="text" id="txtEmail3" value="Email 3" onfocus="clearEmail(this);" onblur="outClear(this);" class="form_txt_small" /></li>	
				<li>	
				    <label for="message">Message</label>				    
					<textarea id="txtMessage" class="form_txt_small" style="height:50px;"></textarea>
				</li>
			
				<li>
				<p class="inviteFriendsButton">
                    <input id="btnForward" type="button" value="Forward To Friend" class="form_btn" onclick="javascript:forwardToFriend('<%=ContentType.ToString() %>','<%=ObjectWebID %>');void(0);" />
				</p>
				</li></ul>                    
                </div>
            </div>