<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IMSPlanEdit.aspx.cs" Inherits="IMSPlanEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
<style>
body{
    font-family:Arial;
}
.divC{
height: 150px; 
display: block;
border:#000099 1px solid;
padding:5px;
}

.imgClass{
	width:200px;
	height:50px;
}

.imgClass:hover{
	width:auto;
	height:auto;
	position:static;
	z-index:99999;
}

.Banner1{

	position:relative;
        top: 10px;
        left: 5px;
    }

.Banner2{

	position:relative;
        top: -94px;
        left: 300px;
    }

.Banner3{

	position:relative;
        top: -201px;
        left: 624px;
        width: 228px;
    }
    
.PlanLevel{

position:relative;
        top: 0px;
        left: 120px;
    }
</style>
    <form id="form1" runat="server">
    <script type="text/javascript" src="/lib/jquery.js"></script>
    <%if(LoggedIn){ %>
    <p><asp:Button runat="server" ID="btnLogout" Text="Logout" onclick="btnLogout_Click" /></p>
    <p>Company Name Filter <input type="text" id="txtAutoComplete" style="width:120px" /></p>
     
     
 
    <%for (int i = 0; i < Businesses.Count; i++)
      { 
          %>
      <div id="divC<%=Businesses[i].BusinessID %>" class="divC">    
         
    <div style="position:relative;width:300px;">
         <p><strong>Company:</strong> <%=Businesses[i].CompanyName%></p>
         <p><strong>Sector:</strong> <%=Businesses[i].IndustrySector%></p>
         <p><strong>Contact:</strong> <%=Businesses[i].Member.FirstName %> <%=Businesses[i].Member.LastName %> </p>
         <p><strong>Email:</strong> <%=Businesses[i].Member.Email%></p>
    </div>
    
     <div style="position:relative; top:-149px; left: 305px; height: 146px;">
     <% System.Collections.Generic.List<Next2Friends.Data.IMSPlan> plan = Businesses[i].IMSPlan;
          
        if(plan.Count > 0){ %>
         <div class="PlanLevel"><select>
             <option value="0" <%if(Businesses[i].IMSPlan[0].PlanLevel == 0 ){%>selected<%} %>>Lite</option>
             <option value="1" <%if(Businesses[i].IMSPlan[0].PlanLevel == 1) {%>selected<%} %>>Premium</option>
             <option value="2" <%if(Businesses[i].IMSPlan[0].PlanLevel == 2) {%>selected<%} %>>Enterprise</option>
         </select>
         </div>
         <div style="top:40px;">
         <div class="Banner1">
             <p>Banner 1 - 154px x 350px</p>
             <p><img src="/ad/banner1.jpg" class="imgClass" /></p>
         </div>
         <div class="Banner2">
             <p>Banner 2 - 154px x 350px</p>
             <p><img src="/ad/banner1.jpg" class="imgClass" /></p>
         </div>
         <div class="Banner3">
             <p>Banner 3 - 154px x 350px</p>
             <p><img src="/ad/banner1.jpg" class="imgClass" /></p>
         </div>
         </div>
         <%} %>
    </div>
    
    </div>
    <p></p>
     <script type="text/javascript">
      <%=JSBusinessArray %>
        $('#txtAutoComplete').keyup( function(e) { 
            for(var i=0;i<Companies.length;i++){
                if(Companies[i][0].toLowerCase().indexOf($('#txtAutoComplete').val().toLowerCase()) > -1){
                    $('#divC'+Companies[i][1]).show();
                }else{
                 $('#divC'+Companies[i][1]).hide();
                }
           }
         });
    </script>
        
    <%} %>
    <%}else{ %>
    <asp:Label runat="server" ID="lblLogin"><p>Enter username and password</p></asp:Label>
    <p>Username <asp:TextBox runat="server" ID="txtuserName" Text="rachel"></asp:TextBox></p>
    <p>Password <asp:TextBox runat="server" ID="txtPassword" Text="password"></asp:TextBox>
        <asp:Button runat="server" ID="btnLogin" Text="Login" onclick="btnLogin_Click" /></p>
    
    <%} %>
    </form>
</body>
</html>
