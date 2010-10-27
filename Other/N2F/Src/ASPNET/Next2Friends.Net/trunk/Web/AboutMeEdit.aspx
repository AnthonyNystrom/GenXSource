<%@ Page Language="C#" MasterPageFile="~/View.master" AutoEventWireup="true" CodeFile="AboutMeEdit.aspx.cs" Inherits="AboutMeEdit" Title="Untitled Page" ValidateRequest="false" %>
<%@ Register TagPrefix="N2F"  namespace="Next2Friends.WebControls"%>
<%@ Import Namespace="Next2Friends.Data" %>
<%@ Import Namespace="Next2Friends.Misc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="RightColContentHolder" runat="Server">

<!-- profile right start -->
 <%if (member.AccountType == (int)AccountType.Personal)
   { %>
        <div class="profile_rightcol">
        
			<div class="edit_settings">
				<h2>Basic profile</h2>
				<p>

					<label for="name">First Name</label>
					<asp:TextBox id="txtFName" runat="server" CssClass="form_txt longInput"></asp:TextBox>					
				</p>
				<p>

					<label for="name">Last Name</label>
					<asp:TextBox id="txtLName" runat="server" CssClass="form_txt longInput"></asp:TextBox>					
				</p>
<%--				<p>
					<label for="tagline">Tag line</label>
					<asp:TextBox id="txtTagLine" runat="server" CssClass="form_txt longInput"></asp:TextBox>					
				</p>--%>
				<p>

					<label>Date of birth</label>
					<N2F:DayDropdown runat="server" ID="drpDay" CssClass="form_menu"></N2F:DayDropdown>
					<N2F:MonthDropdown runat="server" ID="drpMonth" CssClass="form_menu"></N2F:MonthDropdown>
					<N2F:YearDropdown runat="server" ID="drpYear" CssClass="form_menu"></N2F:YearDropdown>
					<%--<input type="checkbox" /> Hide my age--%>

					
				</p>
				<p>
					<label for="gender">Gender</label>
					<asp:DropDownList ID="cmbGender" runat="server" CssClass="form_txt" style="width:178px">
					    <asp:ListItem Value="1" Text="Male"></asp:ListItem>
					    <asp:ListItem Value="0" Text="Female"></asp:ListItem>
					</asp:DropDownList>

				</p>
				<p>
					<label for="relship">Relationship status</label>					
					<asp:DropDownList ID="cmbRelationShipStat" runat="server" CssClass="form_txt longInput">
					    <asp:ListItem Value="-1" Text="Not Saying"></asp:ListItem>
					    <asp:ListItem Value="1" Text="Single"></asp:ListItem>
					    <asp:ListItem Value="2" Text="Married"></asp:ListItem>
					    <asp:ListItem Value="3" Text="Divorced"></asp:ListItem>
					    <asp:ListItem Value="4" Text="Dating"></asp:ListItem>
					    <asp:ListItem Value="5" Text="Seeing"></asp:ListItem>
					    <asp:ListItem Value="6" Text="Kinda Have A Thing"></asp:ListItem>
					</asp:DropDownList>
				</p>
				<p>

					<label for="otherHalf">My Other Half</label>					
					<asp:DropDownList ID="cmbOtherHalf" runat="server" CssClass="form_txt longInput">
					<asp:ListItem Value="-1" Text="Not Saying"></asp:ListItem>
					</asp:DropDownList>
					
				</p>
				
				<p>
					<label for="country">Country</label>

					<N2F:CountryDropdown runat="server" ID="drpCopuntries" 
                        CssClass="form_txt longInput"></N2F:CountryDropdown>
				</p>
				
				<p>
						<label for="dayJob">By day - I am a</label>								
						<N2F:ProfessionDropdown runat="server" ID="drpDayJob" CssClass="form_txt longInput"></N2F:ProfessionDropdown><asp:Literal runat="server" ID="Literal1">&nbsp;</asp:Literal>
						
						
		        </p>
				<p>
					<label for="nightJob">By night - I am a</label>								
					<N2F:ProfessionDropdown runat="server" ID="drpNightJob" CssClass="form_txt longInput"></N2F:ProfessionDropdown><asp:Literal runat="server" ID="Literal2">&nbsp;</asp:Literal>					
					
				</p>
				<p>
					<label for="favInterest">My favorite interest is</label>								
					<N2F:HobbyDropdown runat="server" ID="drpFavInterest" CssClass="form_txt longInput"></N2F:HobbyDropdown><asp:Literal runat="server" ID="Literal3">&nbsp;</asp:Literal>
				</p>
				
				<p>
					<label for="zipcode">Zip / Postal code</label>
					<asp:TextBox id="txtZip" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">City</label>
					<asp:TextBox id="txtHomeTown" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">MySpace Profile URL</label>
					<asp:TextBox id="txtMySpace" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">FaceBook Profile URL</label>
					<asp:TextBox id="txtFaceBook" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>                
                <p>
					<label for="hometown">Blog URL</label>
					<asp:TextBox id="txtBlog" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">Blog Feed URL</label>
					<asp:TextBox id="txtBlogFeed" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
			</div>			
			<div class="edit_settings">
				<h2>My Life</h2>
				<p>
					<asp:TextBox id="txtMyLife" onkeyup="aboutMeLen(this,'mylifeChar');" onchange="aboutMeLen(this,'mylifeChar');" runat="server" CssClass="form_txt" TextMode="MultiLine"></asp:TextBox>
				</p>
				<p>
					Characters&nbsp;remaining:&nbsp;<span id="mylifeChar"><%=AboutMeLength%></span>
				</p>
			</div>
				
			<div class="edit_settings">

				<h2>Things I like</h2> 
				<h3>
					Music<br />
					<small>eg. Led Zeppelin, Madonna, etc.</small>
				</h3>
				<p>
					<asp:TextBox id="txtMusic" runat="server" CssClass="form_txt" onkeyup="aboutMeLen(this,'musicChar');" onchange="aboutMeLen(this,'musicChar');"
                        TextMode="MultiLine"></asp:TextBox>

				</p>
				<p>
					Characters&nbsp;remaining:&nbsp;<span id="musicChar"><%=MusicLength%></span>
				</p>
				<h3>
					Movies<br />
					<small>eg. The Matrix, Finding Nemo, etc.</small>
				</h3>
				<p>
					<asp:TextBox id="txtMovie" runat="server" CssClass="form_txt" onkeyup="aboutMeLen(this,'moviesChar');" onchange="aboutMeLen(this,'moviesChar');"
                        TextMode="MultiLine"></asp:TextBox>

				</p>
				<p>
					Characters&nbsp;remaining:&nbsp;<span id="moviesChar"><%=MoviesLength%></span>
				</p>
				<h3>
					Books<br />
					<small>eg. The Hobbit, Prey, etc.</small>
				</h3>
				<p>
					<asp:TextBox id="txtBook" runat="server" CssClass="form_txt" TextMode="MultiLine" onkeyup="aboutMeLen(this,'booksChar');" onchange="aboutMeLen(this,'booksChar');"></asp:TextBox>
				</p>
				<p>
					Characters&nbsp;remaining:&nbsp;<span id="booksChar"><%=BooksLength%></span>
				</p>
				<p class="alignRight">
				    <input type="button" value="Cancel" class="form_btn" 
                        onclick="__doPostBack('<%=btnCancel.UniqueID %>', '');return false;" />
				    <input type="button" value="Update" class="form_btn" 
                        onclick="__doPostBack('<%=btnSave.UniqueID %>', '');return false;" />
                    
                    <asp:Button id="btnCancel" Text="Save" runat="server" CssClass="hiddenButton" onclick="btnCancel_Click"/>
                    <asp:Button id="btnSave" Text="Save" runat="server" CssClass="hiddenButton" onclick="btnSave_Click"/>
				</p>
			</div>
        </div>
    <%}else if (member.AccountType == (int)AccountType.Business){ %>
        
        <!-- business profile right start -->
        <div class="profile_rightcol">
        
			<div class="edit_settings">
				<h2>Business profile</h2>
				<p>

					<label for="name">Company Name</label>
					<asp:TextBox id="txtCName" runat="server" CssClass="form_txt longInput"></asp:TextBox>					
				</p>
				<p>
					<label for="tagline">Company Website</label>
					<asp:TextBox id="txtCWebsite" runat="server" CssClass="form_txt longInput"></asp:TextBox>					
				</p>
			
				<p>
					<label for="zipcode">Tag Line</label>
					<asp:TextBox id="txtCTagLine" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">Contact First</label>
					<asp:TextBox id="txtContactFirst" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">Contact Last</label>
					<asp:TextBox id="txtContactLast" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">Industry Sector</label>
					<asp:DropDownList runat="server" CssClass="form_txt" ID="drpIndustrySector" Width="178">
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
                </p>                
                <p>
					<label for="hometown">Year Founded</label>
					<asp:DropDownList runat="server" CssClass="form_txt" ID="drpYearFounded" Width="178"></asp:DropDownList>
                </p>
                
                <p>
					<label for="hometown">Company Size</label>
					<asp:DropDownList runat="server" CssClass="form_txt" ID="drpCompanySize" Width="178">
                        <asp:ListItem Text="1-20" Value="1-20"></asp:ListItem>
                        <asp:ListItem Text="21-100" Value="21-100"></asp:ListItem>
                        <asp:ListItem Text="101-500" Value="101-500"></asp:ListItem>
                        <asp:ListItem Text="+500" Value="+500"></asp:ListItem>
                    </asp:DropDownList>
                </p>
                <p>
					<label for="hometown">Street Address (optional)</label>
					<asp:TextBox id="txtCAddress" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                <p>
					<label for="hometown">State / Province (optional)</label>
					<asp:TextBox id="txtCState" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
				<p>
					<label for="country">Country</label>

					<N2F:CountryDropdown runat="server" ID="drpCCountery" 
                        CssClass="form_txt longInput"></N2F:CountryDropdown>
				</p>
		        <p>
					<label for="hometown">City</label>
					<asp:TextBox id="txtCCity" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>    
				
				<p>
					<label for="zipcode">Zip / Postal code</label>
					<asp:TextBox id="txtCZipcode" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
            <p>
					<label for="hometown">MySpace Profile URL</label>
					<asp:TextBox id="txtCMySpaceURL" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">FaceBook Profile URL</label>
					<asp:TextBox id="txtCFacebookURL" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>                
                <p>
					<label for="hometown">Blog URL</label>
					<asp:TextBox id="txtCBlogURL" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                
                <p>
					<label for="hometown">Blog Feed URL</label>
					<asp:TextBox id="txtCBlogFeedURL" runat="server" CssClass="form_txt longInput"></asp:TextBox>
                </p>
                

			                                                           
			</div>			
			<div class="edit_settings">
				<h2>Our Company</h2>
				<p>
					<asp:TextBox id="txtOurCompany" onkeyup="aboutMeLen(this,'companyChar');" onchange="aboutMeLen(this,'companyChar');" runat="server" CssClass="form_txt" TextMode="MultiLine"></asp:TextBox>
				</p>
				<p>
					Characters&nbsp;remaining:&nbsp;<span id="companyChar"><%=OurCompanyLength%></span>
				</p>
			</div>
				
			<div class="edit_settings">

				<h2>Our Products / Services</h2> 
				<h3>
					What we offer<br />
				</h3>
				<p>
					<asp:TextBox id="txtBusinessDescription1" runat="server" CssClass="form_txt" onkeyup="aboutMeLen(this,'dis1Char');" onchange="aboutMeLen(this,'dis1Char');"
                        TextMode="MultiLine"></asp:TextBox>

				</p>
				<p>
					Characters&nbsp;remaining:&nbsp;<span id="dis1Char"><%=Description1Length%></span>
				</p>
				<h3>
					What sets us apart<br />
				</h3>
				<p>
					<asp:TextBox id="txtBusinessDescription2" runat="server" CssClass="form_txt" onkeyup="aboutMeLen(this,'dis2Char');" onchange="aboutMeLen(this,'dis2Char');"
                        TextMode="MultiLine"></asp:TextBox>

				</p>
				<p>
					Characters&nbsp;remaining:&nbsp;<span id="dis2Char"><%=Description2Length%></span>
				</p>
				<h3>
					Where you can find us<br />
				</h3>
				<p>
					<asp:TextBox id="txtBusinessDescription3" runat="server" CssClass="form_txt" TextMode="MultiLine" onkeyup="aboutMeLen(this,'dis3Char');" onchange="aboutMeLen(this,'dis3Char');"></asp:TextBox>
				</p>
				<p>
					Characters&nbsp;remaining:&nbsp;<span id="dis3Char"><%=Description1Length%></span>
				</p>
				<p class="alignRight">
				    <input type="button" value="Cancel" class="form_btn" 
                        onclick="__doPostBack('<%=btnCancel2.UniqueID %>', '');return false;" />
				    <input type="button" value="Update" class="form_btn" 
                        onclick="__doPostBack('<%=btnSave2.UniqueID %>', '');return false;" />
                    
                    <asp:Button id="btnCancel2" Text="Save" runat="server" CssClass="hiddenButton" onclick="btnCancel2_Click"/>
                    <asp:Button id="btnSave2" Text="Save" runat="server" CssClass="hiddenButton" onclick="btnSave2_Click"/>
				</p>
			</div>
        
        <!-- box end -->
   </div>
    <!-- profile right end -->
 <%} %>
	
	<!-- middle end -->

    <script type="text/javascript" src="/js/AboutMeEdit.js"></script>

</asp:Content>
