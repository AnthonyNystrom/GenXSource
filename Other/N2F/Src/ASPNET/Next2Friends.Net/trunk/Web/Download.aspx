<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Download.aspx.cs" Inherits="Download" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" src="/lib/popup.js?c=1"></script>
	<div id="middle" class="no_subnav clearfix">
		<div class="getn2f">
	
			<div class="selection">
				<h3>What is Your Phone?</h3>
				<p>
					<label for="make">Make</label>
                        <asp:DropDownList runat="server" AutoPostBack="true" OnSelectedIndexChanged="SetManu"  ID="drpManu">
                            <asp:ListItem Text="select" Value="select"></asp:ListItem>
                            <asp:ListItem Text="LG" Value="LG"></asp:ListItem>
                            <asp:ListItem Text="Motorola" Value="Motorola"></asp:ListItem>
                            <asp:ListItem Text="Nokia" Value="Nokia"></asp:ListItem>
                            <asp:ListItem Text="Samsung" Value="Samsung"></asp:ListItem>
                            <asp:ListItem Text="Sony Ericcson" Value="Sony Ericcson"></asp:ListItem>
                            <asp:ListItem Text="BlackBerry" Value="BlackBerry"></asp:ListItem>
                        </asp:DropDownList>
				</p>
				<p>
					<label for="model">Model</label>
					<asp:DropDownList runat="server" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="SetModel" ID="drpModel"></asp:DropDownList>
				</p>

			</div>
			
			<%if (!IsDownload){ %>
			
			<div class="instruction">
				<h1>Download Next2Friends Mobile</h1><br/>
				<p>You can download Next2Friends in a few simple steps</p>
				<p>1. Select your phone make <%--(<a href="#">can’t find your phone</a>?)--%><br />
				   2. Select your phone model<br />
				   3. Your download options will now appear.</p>
				<p>(<a href="/features">What can I do with Next2Friends mobile</a>?)</p>
			</div>
			
			<%}else { %>
			
			<div class="download">
				<div class="box left">
					<h3>Download by Browser</h3>
					<p>Choose your downloads or discover Next2Friends <a href="/features">features here</a></p>
					<%=DownloadList %></div>

				<img src="images/or.gif" alt="OR" class="or" />
				<div class="box right">
					<h3>Download from your phone</h3>
<%--				<p>	
					
		<select id="optCountryCode" name="country_code" style="width:120px">
		<option value="1">United States (+1)</option>
        <option value="93">Afghanistan (+93)</option>
        <option value="355">Albania (+355)</option>
        <option value="213">Algeria (+213)</option>
        <option value="1684">American Samoa (+1684)</option>
        <option value="376">Andorra (+376)</option>
        <option value="244">Angola (+244)</option>
        <option value="1264">Anguilla (+1264)</option>
        <option value="672">Antartica (+672)</option>
        <option value="1268">Antigua and Barbuda (+1268)</option>
        <option value="54">Argentina (+54)</option>
        <option value="374">Armenia (+374)</option>
        <option value="297">Aruba (+297)</option>
        <option value="61">Australia (+61)</option>
        <option value="43">Austria (+43)</option>
        <option value="994">Azerbajjan (+994)</option>
        <option value="1242">Bahamas (+1242)</option>
        <option value="973">Bahrain (+973)</option>
        <option value="880">Bangladesh (+880)</option>
        <option value="1264">Barbados (+1264)</option>
        <option value="375">Belarus (+375)</option>
        <option value="32">Belgium (+32)</option>
        <option value="501">Belize (+501)</option>
        <option value="229">Benin (+229)</option>
        <option value="1441">Bermuda (+1441)</option>
        <option value="975">Bhutan (+975)</option>
        <option value="591">Bolivia (+591)</option>
        <option value="387">Bosnia and Herzegovina (+387)</option>
        <option value="267">Botswana (+267)</option>
        <option value="47">Bouvet Island (+47)</option>
        <option value="55">Brazil (+55)</option>
        <option value="673">Brunei (+673)</option>
        <option value="359">Bulgaria (+359)</option>
        <option value="226">Burkina Faso (+226)</option>
        <option value="257">Burundi (+257)</option>
        <option value="855">Cambodia (+855)</option>
        <option value="237">Cameroon (+237)</option>
        <option value="1">Canada (+1)</option>
        <option value="238">Cape Verde (+238)</option>
        <option value="1345">Cayman Islands (+1345)</option>
        <option value="236">Central African Republic (+236)</option>
        <option value="235">Chad (+235)</option>
        <option value="56">Chile (+56)</option>
        <option value="86">China (+86)</option>
        <option value="57">Colombia (+57)</option>
        <option value="269">Comoros (+269)</option>
        <option value="242">Congo (+242)</option>
        <option value="243">Congo, The Democratic Republic of the (+243)</option>
        <option value="682">Cook Islands (+682)</option>
        <option value="506">Costa Rica (+506)</option>
        <option value="255">Cote D'Ivoire (+255)</option>
        <option value="385">Croatia (+385)</option>
        <option value="53">Cuba (+53)</option>
        <option value="357">Cyprus (+357)</option>
        <option value="420">Czech Republic (+420)</option>
        <option value="45">Denmark (+45)</option>
        <option value="253">Djibouti (+253)</option>
        <option value="1767">Dominica (+1767)</option>
        <option value="1809">Dominican Republic (+1809)</option>
        <option value="593">Ecuador (+593)</option>
        <option value="20">Egypt (+20)</option>
        <option value="503">El Salvador (+503)</option>
        <option value="240">Equatorial Guinea (+240)</option>
        <option value="291">Eritrea (+291)</option>
        <option value="372">Estonia (+372)</option>
        <option value="251">Ethiopia (+251)</option>
        <option value="500">Falkland Islands (Malvinas) (+500)</option>
        <option value="298">Faroe Islands (+298)</option>
        <option value="679">Fiji (+679)</option>
        <option value="358">Finland (+358)</option>
        <option value="33">France (+33)</option>
        <option value="594">French Guiana (+594)</option>
        <option value="689">French Polynesia (+689)</option>
        <option value="241">Gabon (+241)</option>
        <option value="220">Gambia (+220)</option>
        <option value="995">Georgia (+995)</option>
        <option value="49">Germany (+49)</option>
        <option value="233">Ghana (+233)</option>
        <option value="350">Gibraltar (+350)</option>
        <option value="30">Greece (+30)</option>
        <option value="299">Greenland (+299)</option>
        <option value="1473">Grenada (+1473)</option>
        <option value="590">Guadeloupe (+590)</option>
        <option value="1671">Guam (+1671)</option>
        <option value="502">Guatemala (+502)</option>
        <option value="224">Guinea (+224)</option>
        <option value="245">Guinea-Bissau (+245)</option>
        <option value="592">Guyana (+592)</option>
        <option value="509">Haiti (+509)</option>
        <option value="672">Heard Island and McDonal Islands (+672)</option>
        <option value="504">Honduras (+504)</option>
        <option value="852">Hong Kong (+852)</option>
        <option value="36">Hungary (+36)</option>
        <option value="354">Iceland (+354)</option>
        <option value="91">India (+91)</option>
        <option value="62">Indonesia (+62)</option>
        <option value="98">Iran (+98)</option>
        <option value="964">Iraq (+964)</option>
        <option value="353">Ireland (+353)</option>
        <option value="972">Israel (+972)</option>
        <option value="39">Italy (+39)</option>
        <option value="1876">Jamaica (+1876)</option>
        <option value="81">Japan (+81)</option>
        <option value="962">Jordan (+962)</option>
        <option value="7">Kazakhstan (+7)</option>
        <option value="254">Keyna (+254)</option>
        <option value="686">Kiribati (+686)</option>
        <option value="850">Korea, North (+850)</option>
        <option value="82">Korea, South (+82)</option>
        <option value="965">Kuwait (+965)</option>
        <option value="996">Kyrgyzstan (+996)</option>
        <option value="856">Laos (+856)</option>
        <option value="371">Latvia (+371)</option>
        <option value="961">Lebanon (+961)</option>
        <option value="266">Lesotho (+266)</option>
        <option value="231">Liberia (+231)</option>
        <option value="218">Libya (+218)</option>
        <option value="423">Liechtenstein (+423)</option>
        <option value="370">Lithuania (+370)</option>
        <option value="352">Luxembourg (+352)</option>
        <option value="853">Macao (+853)</option>
        <option value="389">Macedonia (+389)</option>
        <option value="261">Mgadagascar (+261)</option>
        <option value="265">Malawi (+265)</option>
        <option value="60">Malaysia (+60)</option>
        <option value="960">Maldives (+960)</option>
        <option value="223">Mali (+223)</option>
        <option value="356">Malta (+356)</option>
        <option value="692">Marshall Islands (+692)</option>
        <option value="596">Martinique (+596)</option>
        <option value="222">Mauritania (+222)</option>
        <option value="230">Mauritius (+230)</option>
        <option value="269">Mayotte (+269)</option>
        <option value="52">Mexico (+52)</option>
        <option value="691">Micronesia, Federated States of (+691)</option>
        <option value="373">Moldova (+373)</option>
        <option value="377">Monaco (+377)</option>
        <option value="976">Mongolia (+976)</option>
        <option value="382">Montenegro (+382)</option>
        <option value="1664">Montserrat (+1664)</option>
        <option value="212">Morocco (+212)</option>
        <option value="258">Mozambique (+258)</option>
        <option value="95">Myanmar (+95)</option>
        <option value="264">Namibia (+264)</option>
        <option value="674">Nauru (+674)</option>
        <option value="977">Nepal (+977)</option>
        <option value="31">Netherlands (+31)</option>
        <option value="599">Netherlands Antilles (+599)</option>
        <option value="687">New Caledonia (+687)</option>
        <option value="64">New Zealand (+64)</option>
        <option value="505">Nicaragua (+505)</option>
        <option value="227">Niger (+227)</option>
        <option value="234">Nigeria (+234)</option>
        <option value="683">Niue (+683)</option>
        <option value="6723">Norfolk Island (+6723)</option>
        <option value="1670">Northern Mariana Islands (+1670)</option>
        <option value="47">Norway (+47)</option>
        <option value="968">Oman (+968)</option>
        <option value="92">Pakistan (+92)</option>
        <option value="680">Palau (+680)</option>
        <option value="970">Palestinian Territory (+970)</option>
        <option value="507">Panama (+507)</option>
        <option value="675">Papua New Guniea (+675)</option>
        <option value="595">Paraguay (+595)</option>
        <option value="51">Peru (+51)</option>
        <option value="63">Phillipines (+63)</option>
        <option value="48">Poland (+48)</option>
        <option value="351">Portugal (+351)</option>
        <option value="1787">Puerto Rico (+1787)</option>
        <option value="974">Qatar (+974)</option>
        <option value="272">Reunion (+272)</option>
        <option value="40">Romania (+40)</option>
        <option value="7">Russia (+7)</option>
        <option value="250">Rwanda (+250)</option>
        <option value="290">Saint Helena (+290)</option>
        <option value="1869">Saint Kitts and Nevis (+1869)</option>
        <option value="1758">Saint Lucia (+1758)</option>
        <option value="508">Saint Pierre and Miquelon (+508)</option>
        <option value="1784">Saint Vincent and the Grenadines (+1784)</option>
        <option value="685">Samoa (+685)</option>
        <option value="378">San Marino (+378)</option>
        <option value="239">Sao Tome and Principe (+239)</option>
        <option value="966">Saudi Arabia (+966)</option>
        <option value="221">Senegal (+221)</option>
        <option value="381">Serbia (+381)</option>
        <option value="248">Seychelles (+248)</option>
        <option value="232">Sierra Leone (+232)</option>
        <option value="65">Singapore (+65)</option>
        <option value="421">Slovakia (+421)</option>
        <option value="386">Slovenia (+386)</option>
        <option value="67">Solomon Islands (+67)</option>
        <option value="252">Somalia (+252)</option>
        <option value="27">South Africa (+27)</option>
        <option value="34">Spain (+34)</option>
        <option value="94">Sri Lanka (+94)</option>
        <option value="249">Sudan (+249)</option>
        <option value="597">Suriname (+597)</option>
        <option value="47">Svalbard and Jan Mayen (+47)</option>
        <option value="268">Swaziland (+268)</option>
        <option value="46">Sweden (+46)</option>
        <option value="41">Switzerland (+41)</option>
        <option value="963">Syria (+963)</option>
        <option value="886">Taiwan (+886)</option>
        <option value="992">Tajikistan (+992)</option>
        <option value="255">Tanzania (+255)</option>
        <option value="66">Thailand (+66)</option>
        <option value="670">Timor-Leste (+670)</option>
        <option value="228">Togo (+228)</option>
        <option value="690">Tokelau (+690)</option>
        <option value="676">Tonga (+676)</option>
        <option value="1868">Trinidad and Tobago (+1868)</option>
        <option value="216">Tunisia (+216)</option>
        <option value="90">Turkey (+90)</option>
        <option value="993">Turkmenistan (+993)</option>
        <option value="1649">Turks and Caicos Islands (+1649)</option>
        <option value="688">Tuvalu (+688)</option>
        <option value="256">Uganda (+256)</option>
        <option value="380">Ukraine (+380)</option>
        <option value="971">United Arab Emirates (+971)</option>
        <option value="44">United Kingdom (+44)</option>
        <option value="1">United States (+1)</option>
        <option value="598">Uruguay (+598)</option>
        <option value="998">Uzbekistan (+998)</option>
        <option value="678">Vanuatu (+678)</option>
        <option value="379">Vatican City State (Holy See) (+379)</option>
        <option value="58">Venezuela (+58)</option>
        <option value="84">Vietnam (+84)</option>
        <option value="1284">Virgin Islands, British (+1284)</option>
        <option value="1340">Virgin Islands, US (+1340)</option>
        <option value="681">Wallis and Futuna (+681)</option>
        <option value="967">Yemen (+967)</option>
        <option value="381">Yugoslavia (+381)</option>
        <option value="260">Zambia (+260)</option>
        <option value="263">Zimbabwe (+263)</option></select>
		<input class="form_txt2" style="width:100px" id="txtPhoneNumber" name="phone_number" value="123-456-7890" type="text">
		
		<input class="button" class="form_btn2" name="submit" value="Send SMS" type="submit">
	</p>--%>
					
					
					<p><a href="javascript:alert('To download, browse to this address on your mobile phone web browser');" class='downloadlink'><%=MobileDownloadPage %></a></p>
					<p>or scan the QR code:</p>
					<p class="qr-code"><img src="<%=QRImageURL %>" alt="qr code" width="115" height="115" /></p>
					<p class="whatisqr"><%--<a href="#">What is QR Code?</a>--%></p>
				</div>

			</div>
			
			<%} %>
		
		</div>

		<!-- /download end -->
		</div>
		
<script type="text/javascript">
function liveHelpPopup(){
    npopup('Important Instructions',"<div id='divProfileHTML' style='height: 500px;overflow:auto'><%=LiveText %></div>",525,500);
}


function socialHelpPopup(){
    npopup('Important Instructions',"<div id='divProfileHTML' style='height: 325px;overflow:auto'><%=SocialText %></div>",525,325);
}


</script>
</asp:Content>

