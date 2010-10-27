<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Comments.ascx.cs" Inherits="Comments" %>
<script language="javascript" type="text/javascript" src="/js/comments.js?v2"></script>
<script language="javascript">
commentType = "<%=CommentType.ToString() %>";
ObjectWebId = "<%=ObjectWebId %>";
</script>
<a name="comments"></a>
<h4 class="box_title collapsible">
    Comments <span class="spanNumberOfComments3" id="spanNumberOfComments3<%=ObjectWebId %>">(<%=NumberOfComments%>)</span></h4>
<p class="add_profile_comments">
                    <%if (IsLoggedIn)
                      { %>
                    <a class="showPostComment" href="javascript:showPostComment('<%=ObjectWebId %>');void(0);">
                        <%}
                      else
                      {%>
                        </a><a href="<%=LoginUrl %>">
                            <%} %>Post comment</a></p>
<br />


<div class="divNewCommentHolder" id="divNewCommentHolder<%=ObjectWebId %>">
<div class="divNewComment" id="divNewComment<%=ObjectWebId %>" style="display: none; width: 100%;">
    <p class="align_right">
        <p><textarea class="txtNewComment" id="txtNewComment<%=ObjectWebId %>" cols="20" name="S1" rows="5" style="width: 100%;"></textarea></p>
        <p class="align_right"><input type="button" id="btnCancel" class="form_btn2 btnCancel" value="cancel" onclick="cancelShowPostComment('<%=ObjectWebId %>');"></input>
        <input type="button" id="btnPostComment<%=ObjectWebId %>" class="form_btn2 btnPostComment"  value="post"></input></p>
    </p>
</div>
</div> 


<% if (Collapsed)
   {%>
<div class="collapsible_div" style="display: none;">
<%}
   else
   {%>
<div class="collapsible_div">
<%} %>

    <ul id="ulCommentList<%=ObjectWebId %>" class="ulCommentList profile_commentlist">
                               <%=PageComments%>
                    </ul>
</div>
