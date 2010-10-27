<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Inbox.aspx.cs" Inherits="InboxPage" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <script language="javascript" type="text/javascript" src="/js/util.js"></script>    
    <!-- middle start -->
	<div class="clearfix" id="middle">

		<ul class="vm_tab">
			<li class="watch current" id="tabInbox"><a href="javascript:displayInbox();void(0);">
                Inbox</a></li>
			<li class="watch" id="tabNewMessage"><a href="javascript:showElements(1);void(0);">
                New</a></li>
			<li class="watch" id="tabSent"><a href="javascript:displaySent();void(0);">
                Sent</a></li>
			<li class="watch" id="tabTrash"><a href="javascript:displayTrash();void(0);">
                Trash</a></li>			
		</ul>
		
		<!-- page start -->
		<div class="page gradient clearfix">
		
		
		
		    <!-- START Mew Message -->
			<div class="recorder_wrap clearfix" style="display:none;" id="divNewMessage">
			
				<!-- recorder start -->
				<div class="vm_player" id="divVMRecorder">
					
				</div>
				<!-- recorder end -->
	
				<!-- vm_message start -->
				<div class="vm_rightcol">
						<p>
							<label for="send_to">To</label>						
							<input type="text" size="40" id="txtSendTo" onkeyup="searchIndex(event);" style="width:330px" class="form_txt" name="send_to"/>
						    <div id="Autocomplete" class="form_txt" style="position:absolute;display:none;width:330px"></div>
						</p>
						<p>
							<textarea class="form_txt" style="width:375px;height:260px;" id="txtMessageBody" onfocus="hideAutocomplete();" name="message"></textarea>
						</p>
						<p class="indent" style="text-align:right;">
							<input type="button" class="form_btn2" value="Send Message" name="send_vm" id="btnSend" onclick="ajaxSendMessage();" />
						</p>
				</div>
				<!-- vm_message end -->
				
			</div>
			<!-- END Mew Message -->
			
			
			<!-- player start -->
			<span id="spanViewMessage" style="display:none;">
			
			<div class="vm_player" id="divVMPlayer"></div>
			<!-- player end -->
				<!-- vm_message start -->
				<div class="vm_rightcol">
					<ol class="message_list" id="osMessageList" >					    
					        <li id="msgViewSource" style="visibility:hidden;display:none">
					            <p class='message_head'>
					                <span class='timestamp' id="msgViewTimeSource">n Days ago</span>
					                <span class="mail_message_expand mail_message_toggle"></span>
					                <SPAN class="from" style="font-weight:bolder" id="msgViewFromSource">
					                        <a id="msgViewNickSource" class="profileview" href="javascript:void(0);"></a>
					                </SPAN>
					            </p>
                                <div class='message_body'>
                                    <p><SPAN class="from" id="msgViewBodySource">MESSAGE BODY</SPAN></p>
                                    <p class='quick_reply_buttons'><a href='#' class='quick_reply'>Quick Reply</a> <a href='#' id="msgViewReplyVideoSource" class='reply_with_video'>
                                        Reply with video attachment</a></p>
			                        <div class='quickreply_wrap' id="quickreply_wrap">
    		                            <p><textarea name='' cols='' rows='' id='txtQuickSend' class='form_txt'></textarea></p>
			                            <p class='align_right'><input name='Send' type='button' id='btnQuickSend' value='Quick Send' class='form_txt' /></p>
                                    </div>
                                </div>                     
                            </li>                        
					</ol>				
					
					<ol class="message_list"  id="osMessageList1">
					    
					</ol>
					
					
			        
					<p class="collapse_buttons" id="pCollapseOptions"></p>

				</div>
				<!-- vm_message end -->
			</span>

			
			<!-- inbox start -->
			<span id="spanInbox" style="display:block;">
			    <div class="message_inbox clearfix">

				    <h3 class="message_page" id="divTopInboxPager"></h3> 
				    	 <p class="message_select">Select: <a href="javascript:selectAllMessages();void(0);" class="select_all"> 
                             All</a>
				    	 <a href="javascript:selectNoneMessages();void(0);" class="select_none"> None</a> 
					     <input name="" type="button" value="Trash" onclick="deleteMessages();" class="form_btn2" />
				    </p>
			    </div>
			    <div class="inbox_list" id="divInbox">			        
			        <P class="message_item clearfix read" style="cursor: pointer;visibility:hidden;display:none" id="inboxItemSource">
                        <INPUT type="checkbox" id="chkBox" name="" value="" class="message_checkbox"/>
                        <SPAN id="msgItemSource">
                            <SPAN class="from" id="fromItemSource">FROM</SPAN>
                            <SPAN name="status" id="statusItemSource" class="status">» Read    </SPAN>
                            <SPAN class="excerpt" id="excerptItemSource">MESSAGE TEXT</SPAN>
                            <SPAN class="time" id="timeItemSource">n days ago</SPAN>
                        </SPAN>
                   </P>			        
			    </div><!-- /inbox list -->

			    <p class="message_pagenav" id="divBottomInboxPager"></p>
            </span>
            <span id="spanSent" style="display:none;">
			    <div class="message_inbox clearfix">

				    <h3 class="message_page" id="divTopSentPager"></h3> 
				    	<p class="message_select">Select: <a href="javascript:selectAllSent();void(0);" class="select_all">
                            All</a> 
				    	<a href="javascript:selectNoneSent();void(0);" class="select_none">None</a> 
					    <input name="" type="button" value="Trash" onclick="deleteSent();" class="form_btn" />

				    </p>
			    </div>
			    <div class="inbox_list" id="divSent">			        
			    </div>

			    <p class="message_pagenav" id="divBottomSentPager"></p>
            </span>
            
            			<!-- inbox start -->
			<span id="spanTrash" style="display:none;">
			    <div class="message_inbox clearfix">

				    <h3 class="message_page" id="divTopTrashPager"></h3> 
				    	<p class="message_select">Select: <a href="javascript:selectAllTrash();void(0);" class="select_all">
                            All</a> 
				    	<a href="javascript:selectNoneTrash();void(0);" class="select_none">None</a> 
					    <input name="" type="button" value="Empty Trash" onclick="emptyTrash();" class="form_btn" />

				    </p>
			    </div>
			    <div class="inbox_list" id="divTrash">			        
			    </div>

			    <p class="message_pagenav" id="divBottomTrashPager"></p>
            </span>
            
            
		
		</div>
		<!-- page end -->


	</div>

<script language="javascript" type="text/javascript">


var InitialSendValue = false;
var InitialForwardValue = false;
var InitialSendNickName = null;
var InitialForwardText = null;
var PassKey = null;
var WMsgID = null;

InitialSendValue = '<%= InitialSendValue %>';
InitialForwardValue = '<%= InitialForwardValue %>';
InitialSendNickName = '<%= InitialSendNickName %>';
InitialForwardText = '<%= InitialForwardText%>';
PassKey = '<%=PassKey %>';
WMsgID = '<%=WebMessageID %>';
</script>

<script type="text/javascript" src="/lib/jquery-1.js"></script>
<script type="text/javascript" src="/js/inbox.js?v=1"></script>
<script type="text/javascript" src="/js/VMRecorder.js"></script>
<script type="text/javascript" src="/js/VMPlayer.js"></script>


</asp:Content>