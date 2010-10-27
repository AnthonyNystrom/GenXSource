<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="VideoMessage.aspx.cs" Inherits="VideoMessagePage" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<link href="css/inbox.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="js/VMPlayer.js"></script>

	<!-- middle start -->
	<div class="clearfix" id="middle">

		<!--subnav start -->
		<ul class="clearfix" id="subnav">
		</ul>
		<!-- subnav end -->

		
<!-- page start -->

		<div class="page gradient clearfix">
		
		    <!-- START Mew Message -->
			<div class="recorder_wrap clearfix" style="display: none;" id="divNewMessage">
			
				<!-- recorder start -->
				<div class="vm_player" id="divVMRecorder">
					
				</div>
				<!-- recorder end -->
	
				<!-- vm_message start -->
				<div class="vm_rightcol">

					
						<p>
							<label for="send_to">To</label>
							<input size="40" id="txtSendTo" onkeyup="searchIndex(event);" style="width: 330px;" class="form_txt" name="send_to" type="text">
						    </p><div id="Autocomplete" class="form_txt" style="position: absolute; display: none; width: 330px;"></div>
						
						<p>
							<textarea class="form_txt" style="width: 375px; height: 260px;" id="txtMessageBody" onfocus="hideAutocomplete();" name="message"></textarea>
						</p>
						<p class="indent" style="text-align: right;">

							<input class="form_btn" value="Send Video Message" name="send_vm" id="btnSend" onclick="ajaxSendMessage();" type="button">
						</p>
					
				</div>
				<!-- vm_message end -->
				
			</div>
			<!-- END Mew Message -->
			
			
			<!-- player start -->
			<span id="spanViewMessage" style="display: block;">
			
			<% if (MessageLoaded)
      {%>
			<div class="vm_player" id="divVMPlayer"> </div>

            <script type="text/javascript">
                var vp = new VMPlayer('width=420px,height=350px,target=divVMPlayer','<%=emailMessage.VideoMessage.WebResourceFileID %>',null,null,null,null);
                vp.writeFlashPlayer();
            </script>
			<!-- player end -->
	
				<!-- vm_message start -->
				<div class="vm_rightcol">
					<ol class="message_list" id="osMessageList"><li><p class="message_head"><cite><%=emailMessage.Member.FirstName%> <%=emailMessage.Member.LastName%>:</cite> <span class="timestamp"><%=emailMessage.DTCreated%></span></p>

                            <div style="display: block;" class="message_body">
                                <p><br><%=emailMessage.Text%></p>
                            </div>
                        </li></ol>
					
					

				</div>
				<!-- vm_message end -->
			</span>

			<%}else{ %>
			    Sorry, Your message is no longer availble.
			<%} %>
		
		</div>

		<!-- page end -->




	</div>
	</div>

</asp:Content>
