<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="MobileSignup.aspx.cs" Inherits="MobileSignup" %>
<%@ Register TagPrefix="N2F"  namespace="Next2Friends.WebControls"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Next2Friends Signup</title>
    <style type="text/css">
    .formerror_msg{
   	color: #FF6600;
    }
   .form_btn {
	    padding: 1px 10px;
		border:solid 1px #9aaabb;
		font-size:11px;
		cursor:pointer;
    }
	
	.mobileInputTxt {
	width:160px;
	border:solid 1px #9aaabb;
	padding:1px;
	font-size:11px;
	}
	label,
	p {
	font-family:Verdana, Arial, Helvetica, sans-serif;
	font-size:11px;
	}

	p {
	margin:0 0 10px 0;
	}
	
	select.form_menu {
	width:160px;
	border:solid 1px #9aaabb;
	padding:1px;
	font-size:11px;
	}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:164px;border:solid 1px #9aaabb;padding:5px;">
    <img src="/images/logo-mobile.gif" />
    
<%--    Is Mobile : <%=MobileBrowser.IsMobileDevice.ToString() %><br />
    Platform : <%=MobileBrowser.Platform.ToString() %>--%>
    <%if (CurrentStage == MobileSignupStage.Stage1)
      { %>
        <p>
            <label>Email</label> <br />  
            <asp:Literal runat="server" id="errTxtEmail"></asp:Literal>
            <asp:TextBox runat="server" ID="txtEmail" CssClass="mobileInputTxt" MaxLength="50"></asp:TextBox>
        </p>
        <p>
            <label>Nickname</label><br />  
            <asp:Literal runat="server" id="errTxtNickName"></asp:Literal>
            <asp:TextBox runat="server" ID="txtNickName" CssClass="mobileInputTxt" MaxLength="12"></asp:TextBox>
            
        </p>
        
        <p>
            <label>Country</label><br /> 
            <asp:Literal runat="server" ID="errDrpCountries"></asp:Literal>
            <N2F:CountryDropdown runat="server" ID="drpCountries" style="width:160px;" class="form_menu"></N2F:CountryDropdown>
        </p>
        <p>
            <label>Birthday</label><br />
            <asp:Literal runat="server" ID="errDOB"></asp:Literal>
            <N2F:DayDropdown runat="server" ID="drpDay" CssClass="form_menu" style="width: 45px;"></N2F:DayDropdown>
		    <N2F:MonthDropdown runat="server" ID="drpMonth" CssClass="form_menu" style="width: 58px;"></N2F:MonthDropdown>
		    <N2F:YearDropdown runat="server" ID="drpYear" CssClass="form_menu" style="width: 50px;"></N2F:YearDropdown>
		    
		</p>				
        <p>
            <label>Gender</label><br /> 
            <asp:Literal runat="server" id="errDrpGender"></asp:Literal>
            <asp:DropDownList runat="server" CssClass="form_menu" ID="drpGender">
                <asp:ListItem Text="---" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                <asp:ListItem Text="Female" Value="0"></asp:ListItem>
            </asp:DropDownList>
            
        </p>
        
        
        <p style="text-align:right">
        <asp:Button runat="server" ID="btnSignup1" Text="Next" CssClass="form_btn" onclick="btnSignup1_Click" />
        </p>
    <%}else if (CurrentStage == MobileSignupStage.Stage2){ %>
        <p>
            <label>First Name</label><br /> 
            <asp:Literal runat="server" id="errTxtFirstName"></asp:Literal>
            <asp:TextBox runat="server" ID="txtFirstName" CssClass="mobileInputTxt" MaxLength="50"></asp:TextBox>
            </p>
        <p>
            <label>Last Name</label><br /> 
            <asp:Literal runat="server" id="errTxtLastName"></asp:Literal>
            <asp:TextBox runat="server" ID="txtLastName" CssClass="mobileInputTxt" MaxLength="50"></asp:TextBox>
            
        </p>
        <p>
            <label>Password</label><br /> 
            <asp:Literal runat="server" id="errTxtPassword"></asp:Literal>
            <asp:TextBox runat="server" CssClass="mobileInputTxt" ID="txtPassword"></asp:TextBox>
            
        </p>
        <p>
            <label>Confirm</label><br /> 
            <asp:TextBox runat="server" CssClass="mobileInputTxt" ID="txtConfirm"></asp:TextBox>
        </p>
        <p>
            <label>Accept terms</label><br /> 
            <asp:Literal runat="server" id="errChbTOS"></asp:Literal>
            <asp:CheckBox runat="server" ID="cbTOS" />
        </p>
        <p style="text-align:right">
            <asp:Button runat="server" ID="btnSignup2" Text="Next" CssClass="form_btn"  onclick="btnSignup2_Click" />
        </p>
        
    
    <%}
      else if (CurrentStage == MobileSignupStage.Complete)
      { %>
        
            <style type="text/css">
            .downloadlink {
	            font-family:Verdana, Arial, Helvetica, sans-serif;
	            font-size:11px;
            }
            .downloadlink small {
	            font-size: 100%;
	            display: block;
	            padding: 3px 0 0 0px;
	            color: #999;
            }
            </style>
    
            <p>Thank you!</p>
            <p>Please select your download.</p>
           
            <span class='downloadlink'>Live (Symbian only)<br /><a style='font-size:smaller' href='/3'>.Sisx </a><small>Live v1.0 (154k)</small></span><br />
            <span class='downloadlink'>Ask <br /><a style='font-size:smaller' href='/4'>.Jar </a> | <a style='font-size:smaller' href='/5'>.Jad </a><small>Ask v1.0 (245k)</small></span><br />
            <span class='downloadlink'>Tag <br /><a style='font-size:smaller' href='/6'>.Jar </a> | <a style='font-size:smaller' href='/7'>.Jad </a><small>Tag v1.0  (125k)</small></span><br />
            <span class='downloadlink'>Snap-up <br /><a style='font-size:smaller' href='/8'>.Jar </a> | <a style='font-size:smaller' href='/9'>.Jad </a> <small>Snap-up v1.0  (100k)</small></span><br />
            
              

    <%} %>

    
    </div>
    </form>
</body>
</html>
