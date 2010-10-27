<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChatCtrl.ascx.cs" Inherits="ChatCtrl" %>

<script type="text/javascript" src="/js/util.js"></script>
<script type="text/javascript" src="/lib/interface.js"></script>
<script language="javascript" type="text/javascript" src="/js/ChatCtrl.js?v=10"></script>
<script language="javascript" type="text/javascript">
    chatMode = <%=ChatMode %>;
    autoLoadChatMode = <%=AutoLoadChatMode%>;
    myId = '<%= MyId %>';
</script>
<div class="chatFriendList" id="cWinSrcFriendList"  style="display:none">
	<h2 id="chatFriendListTitle">
		<a href="javascript:minWin('FriendList');"><img src="/images/minimize.gif" width="16" height="16" alt="minimize" /></a>
		Online Friends
	</h2>
	<div class="chatOnlineFriends" id="chatFriends">
	    <div id="chatOnlineFriends">
		    <div class="chatFriendEntry" id="fEntrySrc" style="display:none">
			    <img id="fAvator" src="/user/NoPhoto.jpg" width="25" height="25" alt="" />
			    <h3 id="fNick"></h3>
			    <p id="fPM">
			    </p>
		    </div>
		</div>
        <div id="chatOfflineFriends">
	        <div class="chatFriendEntry chatFriendEntryOffline" id="ofEntrySrc" style="display:none;">
			    <img id="ofAvator" src="/user/NoPhoto.jpg" width="25" height="25" alt="" />
			    <h3 id="ofNick"></h3>
			    <p id="ofPM">
			    </p>
		    </div>
	    </div>
	</div>
</div>
<div id="cWins2">
	<div class="chatConversation chatConversationNoMsg" id="cWinSrc" style="display:none;">
		<div class="chatWindowHeader" id="chatWindowHeader">
			<a href="javascript:void(0);" id="wClose"><img src="/images/close.gif" width="16" height="16" alt="close" /></a>
			<a href="javascript:void(0);" id="wMin"><img src="/images/minimize.gif" width="16" height="16" alt="minimize" /></a>
			<img id="wAvator" src="/user/NoPhoto.jpg" width="25" height="25" alt="" class="avatar" />
			<h3 id="wNick"></h3>
			<p id="wPM">
			</p>
		</div>
		<div id="collapseDiv">
		    <div class="conversationPanel" id="wPanel">    		
			    <div class="chatEntry" style="visibility:hidden;display:none">
				    <h3>
					    <span class="entryTime">a</span>
					    <span class="partner">b</span>
				    </h3>
				    <p>
				    </p>
			    </div>
			    <div id="chatScroll"></div>
		    </div>
		    <div class="chatInput">
			    <input type="text" id="wInput" />
		    </div>
		</div>
	</div>
</div>