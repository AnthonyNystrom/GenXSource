<%@ Page Language="C#" MasterPageFile="main.master"  AutoEventWireup="true" CodeFile="Signup.aspx.cs" Inherits="Signup" %>
<%@ Register TagPrefix="N2F"  namespace="Next2Friends.WebControls"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Lanap.BotDetect" Namespace="Lanap.BotDetect" TagPrefix="BotDetect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:TextBox ID="TextBox1" runat="server" Style="display:none; visibility:hidden;"></asp:TextBox>
<asp:TextBox ID="TextBox2" runat="server" Style="display:none; visibility:hidden;"></asp:TextBox>
<ajaxToolkit:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
	<!-- middle start -->
	<div id="middle" class="no_subnav clearfix">

		<!-- page start -->
		<div class="page clearfix fullPageBkg">
		
			<!-- signup col start -->
			<div class="signup_col">
				<h2>Create Your Next2Friends Account</h2>
				<p>It's fun and easy. Just fill out the account info below.</p>
				<!-- signup form div start -->
				<div class="signup_form">
					<div class="top"></div>
					
						<div id="signup_form">
						    <p>
								<label for="email">Account Type</label>
								
								<asp:RadioButton id="rbPersonal" runat="server"
                                TextAlign="right" 
                                GroupName="gpAccount" onclick="TogglePersonal();"
                                Checked="True"/>&nbsp;Personal&nbsp;
                                <asp:RadioButton id="rbBusiness" runat="server"
                                TextAlign="right"
                                GroupName="gpAccount" onclick="ToggleBusiness();"
                                Checked="False"/>&nbsp;Corporate&nbsp;
                               
							</p>
							
							<script type="text/javascript">
							function ToggleBusiness(){
							    $('#lblFirstName').html('Contact first name');
							    $('#lblLastName').html('Contact last name');
							    $('#pCompanyName').show();
							    $('#pInductrySector').show();
							    $('#pYearFounded').show();
							    $('#pNumberOfEmployees').show();
							    $('#lblNick').html('NickName');
							    $('#pGender').hide();
							    $('#pDOB').hide();
							}
							
							function TogglePersonal(){
							    $('#lblFirstName').html('First name');
							    $('#lblLastName').html('Last name');
							    $('#pCompanyName').hide();
							    $('#pInductrySector').hide();
							    $('#pYearFounded').hide();
							    $('#pNumberOfEmployees').hide();
							    $('#lblNick').html('Display name');
							    $('#pGender').show();
							    $('#pDOB').show(); 
							}
							
							
							</script>
							
							
							<p>
								<label for="email">Email</label>
								<asp:TextBox runat="server" CssClass="form_txt" ID="txtEmail" ></asp:TextBox><asp:Literal runat="server" ID="errTxtEmail">&nbsp;</asp:Literal>
							</p>
							<p>
								<label for="email" id="lblFirstName">First name</label>
								<asp:TextBox runat="server" CssClass="form_txt" ID="txtFirstName" ></asp:TextBox><asp:Literal runat="server" ID="errTxtFirstName">&nbsp;</asp:Literal>
							</p>
							<p>
								<label for="email" id="lblLastName">Last name</label>
								<asp:TextBox runat="server" CssClass="form_txt" ID="txtLastName" ></asp:TextBox><asp:Literal runat="server" ID="errTxtLastName">&nbsp;</asp:Literal>
							</p>
							<p id="pCompanyName" style="display:none;">
								<label for="username">Company name</label>
								<asp:TextBox runat="server"  CssClass="form_txt" MaxLength="30" ID="txtCompanyName" ></asp:TextBox><asp:Literal runat="server" ID="errTxtCompanyName"></asp:Literal> 
							</p>
							<p id="pInductrySector" style="display:none;">
								        <label for="industry">Industry sector</label>
        								
							        <asp:DropDownList runat="server" CssClass="form_txt" ID="drpIndustrySector" Width="178">
								        <asp:ListItem Value="--" Text="----"></asp:ListItem>
								        <asp:ListItem Value="Agriculture" Text="Agriculture"></asp:ListItem>
                                        <asp:ListItem Value="Apparel" Text="Apparel" ></asp:ListItem>
                                        <asp:ListItem Value="Banking"  Text="Banking"></asp:ListItem>
                                        <asp:ListItem Value="Biotechnology"  Text="Biotechnology"></asp:ListItem>
                                        <asp:ListItem Value="Chemicals"  Text="Chemicals"></asp:ListItem>
                                        <asp:ListItem Value="Communications"  Text="Communications"></asp:ListItem>
                                        <asp:ListItem Value="Construction"  Text="Construction"></asp:ListItem>
                                        <asp:ListItem Value="Consulting"  Text="Consulting"></asp:ListItem>
                                        <asp:ListItem Value="Education"  Text="Education"></asp:ListItem>
                                        <asp:ListItem Value="Electronics"  Text="Electronics"></asp:ListItem>
                                        <asp:ListItem Value="Energy"  Text="Energy"></asp:ListItem>
                                        <asp:ListItem Value="Engineering"  Text="Engineering"></asp:ListItem>
                                        <asp:ListItem Value="Entertainment"  Text="Entertainment"></asp:ListItem>
                                        <asp:ListItem Value="Environmental"  Text="Environmental"></asp:ListItem>
                                        <asp:ListItem Value="Finance"  Text="Finance"></asp:ListItem>
                                        <asp:ListItem Value="Food & Beverage"  Text="Food & Beverage"></asp:ListItem>
                                        <asp:ListItem Value="Government"  Text="Government"></asp:ListItem>
                                        <asp:ListItem Value="Healthcare"  Text="Healthcare"></asp:ListItem>
                                        <asp:ListItem Value="Hospitality"  Text="Hospitality"></asp:ListItem>
                                        <asp:ListItem Value="Insurance"  Text="Insurance"></asp:ListItem>
                                        <asp:ListItem Value="Internet"  Text="Internet"></asp:ListItem>
                                        <asp:ListItem Value="Machinery"  Text="Machinery"></asp:ListItem>
                                        <asp:ListItem Value="Manufacturing"  Text="Manufacturing"></asp:ListItem>
                                        <asp:ListItem Value="Media"  Text="Media"></asp:ListItem>
                                        <asp:ListItem Value="Not For Profit"  Text="Not For Profit"></asp:ListItem>
                                        <asp:ListItem Value="Recreation"  Text="Recreation"></asp:ListItem>
                                        <asp:ListItem Value="Retail"  Text="Retail"></asp:ListItem>
                                        <asp:ListItem Value="Shipping"  Text="Shipping"></asp:ListItem>
                                        <asp:ListItem Value="Technology"  Text="Technology"></asp:ListItem>
                                        <asp:ListItem Value="Telecommunications"  Text="Telecommunications"></asp:ListItem>
                                        <asp:ListItem Value="Transportation"  Text="Transportation"></asp:ListItem>
                                        <asp:ListItem Value="Utilities"  Text="Utilities"></asp:ListItem>
                                        <asp:ListItem Value="Other"  Text="Other"></asp:ListItem>
                                </asp:DropDownList>
								
								
								<asp:Literal runat="server" ID="errTxtIndustrySector"></asp:Literal> 
							
							
							</p>
							<p id="pYearFounded" style="display:none;">
								<label for="industry">Year founded</label>
        								
							    <asp:DropDownList runat="server" CssClass="form_txt" ID="drpYearFounded" Width="178">

                                </asp:DropDownList>
								<asp:Literal runat="server" ID="errTxtYearFounded"></asp:Literal> 
							
							</p>
							<p id="pNumberOfEmployees" style="display:none;">
								<label for="industry">Company size</label>
        								
							    <asp:DropDownList runat="server" CssClass="form_txt" ID="drpCompanySize" Width="178">
							        <asp:ListItem Value="--" Text="----"></asp:ListItem>
                                    <asp:ListItem Text="1-20" Value="1-20"></asp:ListItem>
                                    <asp:ListItem Text="21-100" Value="21-100"></asp:ListItem>
                                    <asp:ListItem Text="101-500" Value="101-500"></asp:ListItem>
                                    <asp:ListItem Text="+500" Value="+500"></asp:ListItem>
                                </asp:DropDownList>
								<asp:Literal runat="server" ID="errTxtNumberOfEmployees"></asp:Literal> 
							
							</p>

							<p>
								<label for="username" id="Label1">Nickname</label>
								<asp:TextBox runat="server"  CssClass="form_txt" MaxLength="10" ID="txtNickName" onkeyup="setAvailabilityLink();" ></asp:TextBox><asp:Literal runat="server" ID="errTxtNickName"></asp:Literal> <a href="javascript:checkAvailability();" id="aAvailability" style="display:none;"  class="checkAvailability">Check availability</a>
								
								<small class="indent">Your nickname can only contain letters A- Z or numbers 0 - 9</small>
							</p>
							<p>
								<label for="password">Password</label>
								<asp:TextBox runat="server" id="txtPassword1" onkeydown="hidePasswordError()" TextMode="Password" CssClass="form_txt"></asp:TextBox><asp:Literal runat="server" ID="errTxtPassword1"></asp:Literal>
	
							</p>
							<p>
								<label for="confirm">Confirm password</label>
								<asp:TextBox runat="server" id="txtPassword2" TextMode="Password" CssClass="form_txt"></asp:TextBox><br />
								
								<small class="indent">Your password must be at least 7 characters long</small>
								<span class="indent">
																
                                    <ajaxToolkit:PasswordStrength ID="PS" runat="server"
                                        TargetControlID="txtPassword1"
                                        DisplayPosition="RightSide"
                                        StrengthIndicatorType="Text"
                                        PreferredPasswordLength="10"
                                        PrefixText=""
                                        TextCssClass="PasswordStrength"
                                        MinimumNumericCharacters="0"
                                        MinimumSymbolCharacters="0"
                                        RequiresUpperAndLowerCaseCharacters="false"
                                        TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
                                        CalculationWeightings="50;15;15;20" />
                                        
								<style type="text/css">
								.PasswordStrength {
									background-color:Transparent;
								    color: #FF6600;
	                                font-size: 95%;
	                                background: url(images/error-msg.gif) no-repeat left center;
	                                padding-left: 6px;
								}
								</style>
								
								</span>
							</p>
							<p>
								<label for="country">Country</label>
								<N2F:CountryDropdown runat="server" ID="drpCopuntries"></N2F:CountryDropdown><asp:Literal runat="server" ID="errDrpCountries">&nbsp;</asp:Literal>
							</p>
							<p>
								<label for="city">City</label>
								<asp:TextBox runat="server" CssClass="form_txt" ID="txtCity" ></asp:TextBox>
							</p>
							<p>
								<label for="postal_code">Postal code</label>
								<asp:TextBox runat="server" CssClass="form_txt" ID="txtZipPostcode" MaxLength="8" ></asp:TextBox><asp:Literal runat="server" ID="errTxtZipPostcode">&nbsp;</asp:Literal><br />
								<small class="indent">Optional field for US, UK, and Canada only</small>
							</p>
							<p id="pGender">
								<label>Gender</label>
								<asp:RadioButton id="rbMale" runat="server"
                                TextAlign="right"
                                GroupName="gpGender"
                                Checked="False"/>&nbsp;Male&nbsp;
                                <asp:RadioButton id="rbFemale" runat="server"
                                TextAlign="right"
                                GroupName="gpGender"
                                Checked="False"/>&nbsp;Female&nbsp;
                                <asp:Literal runat="server" ID="errRblGender">&nbsp;</asp:Literal>
							</p>
							<p id="pDOB">
								<label>Date of birth</label>
								<N2F:DayDropdown runat="server" ID="drpDay" CssClass="form_menu"></N2F:DayDropdown>
								<N2F:MonthDropdown runat="server" ID="drpMonth" CssClass="form_menu"></N2F:MonthDropdown>
								<N2F:YearDropdown runat="server" ID="drpYear" CssClass="form_menu"></N2F:YearDropdown><asp:Literal runat="server" ID="errDOB">&nbsp;</asp:Literal>
							</p>
<%--							<p>
								<label>Phone Make</label>
								<N2F:DayDropdown runat="server" ID="DayDropdown1" CssClass="form_menu"></N2F:DayDropdown>
								<N2F:MonthDropdown runat="server" ID="MonthDropdown1" CssClass="form_menu"></N2F:MonthDropdown>
								<N2F:YearDropdown runat="server" ID="YearDropdown1" CssClass="form_menu"></N2F:YearDropdown><asp:Literal runat="server" ID="Literal1">&nbsp;</asp:Literal>
							</p>
							<p>
								<label>Phone Model</label>
								<N2F:DayDropdown runat="server" ID="DayDropdown2" CssClass="form_menu"></N2F:DayDropdown>
								<N2F:MonthDropdown runat="server" ID="MonthDropdown2" CssClass="form_menu"></N2F:MonthDropdown>
								<N2F:YearDropdown runat="server" ID="YearDropdown2" CssClass="form_menu"></N2F:YearDropdown><asp:Literal runat="server" ID="Literal2">&nbsp;</asp:Literal>
							</p>
							<p>
								<label>Phone number</label>
								<N2F:DayDropdown runat="server" ID="DayDropdown3" CssClass="form_menu"></N2F:DayDropdown>
								<N2F:MonthDropdown runat="server" ID="MonthDropdown3" CssClass="form_menu"></N2F:MonthDropdown>
								<N2F:YearDropdown runat="server" ID="YearDropdown3" CssClass="form_menu"></N2F:YearDropdown><asp:Literal runat="server" ID="Literal3">&nbsp;</asp:Literal>
							</p>--%>
							
							<p>
							    <label>Referral Email address</label>
						        <asp:TextBox ID="txtReferralEmail" MaxLength="50" runat="server" CssClass="form_txt"></asp:TextBox>
							</p>
							<p>
							    <label>Are you human?</label>
						        <BotDetect:Captcha ID="SignupCaptcha" runat="server" /><br />
							</p>
							<p>
							    <label>Enter the characters in the image above</label>
							    <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form_txt"></asp:TextBox><asp:Literal runat="server" ID="errCaptcha">&nbsp;</asp:Literal>
							</p>
							<p class="indent">
							    <asp:CheckBox runat="server" ID="chbAgree" /><asp:Literal runat="server" ID="errChbAgree">&nbsp;</asp:Literal>
								I agree to the <a href="/TOU.aspx">Terms of Use</a> and <a href="/PP.aspx">Privacy Policy</a>
							</p>
							
							<p class="indent">
							    <input type="button" value="Signup" class="form_btn" onclick="__doPostBack('<%=btnSignup.UniqueID %>', '');return false;" />
                                <asp:Button id="btnSignup" Text="Signup" runat="server" CssClass="hiddenButton" onclick="btnSignup_Click"/>
							</p>
						<script type="text/javascript">
						if($('#ctl00_ContentPlaceHolder1_rbPersonal')[0].checked){
                            TogglePersonal()
						}else{
						    ToggleBusiness()
						}
						</script>
						</div>
						
					
					<div class="bottom"></div>
				</div>
				<!-- signup form div end -->
			</div>
			<!-- signup col end -->

			<!-- login col start -->
			<div class="login_col">
				<h2>Member Login</h2>
				<p>Already have a Next2Friends account? Login here.</p>
				
				<div class="login_form">
					<div class="top"></div>
					
					<div id="login_form">
					<asp:Literal runat="server" ID="errLogin"></asp:Literal>
						<p>
							<label for="login_email">Email</label>
							<asp:TextBox runat="server" Cssclass="form_txt" ID="txtEmailLogin" ></asp:TextBox>
						</p>
						<p>
							<label for="login_password">Password</label>
							<asp:TextBox TextMode="Password" runat="server" Cssclass="form_txt" ID="txtPasswordLogin" ></asp:TextBox>
						</p>
						<p class="indent">
						    <input type="button" value="Login" class="form_btn" onclick="__doPostBack('<%=btnLogin.UniqueID %>', '');return false;" />
						    <asp:Button CssClass="hiddenButton" runat="server" id="btnLogin" Text="Login"  onclick="btnLogin_Click" /><asp:CheckBox runat="server" ID="chbRememberMe"/> Remember me
					</p>
					<p class="indent"><small><a href="/ForgottenPassword.aspx">Forgot your password?</a> <br /></small></p></div>
			</div>
			<!-- login col end -->
		
		</div>
		<!-- page end -->
    </div>
	</div>
	<!-- middle end -->
	<script type="text/javascript" src="js/Signup.js"></script>
	
	

	
	</asp:Content>