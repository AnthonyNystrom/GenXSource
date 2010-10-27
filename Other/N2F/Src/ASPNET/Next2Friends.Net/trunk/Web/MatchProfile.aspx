<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MatchProfile.aspx.cs"
    Inherits="MatchProfilePage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="/styleB.css" rel="stylesheet" type="text/css" />
    <!-- middle start -->
    <div id="middle" class="clearfix">
        <!--subnav start -->
        <ul id="subnav" class="clearfix">
        </ul>
        <div class="page clearfix fullPageBkg">
            <h2>My Tagging Profile</h2>
			<p></p>
            <div class="leftFloater quarter">
                <h3>
                    Age range</h3>
                <ul>
                           
                        <li><asp:RadioButton id="rbAll" runat="server" TextAlign="right" GroupName="rbAge" Checked="False"/> All</li>   
                        <li><asp:RadioButton id="rb1830" runat="server" TextAlign="right" GroupName="rbAge" Checked="False"/> 18 - 24</li>
                        <li><asp:RadioButton id="rb2530" runat="server" TextAlign="right" GroupName="rbAge" Checked="False"/> 25 - 30</li>
                        <li><asp:RadioButton id="rb3134" runat="server" TextAlign="right" GroupName="rbAge" Checked="False"/> 31 - 34</li>
                        <li><asp:RadioButton id="rb35" runat="server" TextAlign="right" GroupName="rbAge" Checked="False"/> 35+</li>
                
                </ul>
            </div>
            <div class="leftFloater quarter">
                <h3>
                    Gender</h3>
                <ul>
                        <li><asp:RadioButton id="rbGenderBoth" runat="server" TextAlign="right" GroupName="rbGender" Checked="False"/> Both</li>
                        <li><asp:RadioButton id="rbGenderMale" runat="server" TextAlign="right" GroupName="rbGender" Checked="False"/> Male</li>
                        <li><asp:RadioButton id="rbGenderFemale" runat="server" TextAlign="right" GroupName="rbGender" Checked="False"/> Female</li>
                </ul>
            </div>
<%--            <div class="leftFloater quarter">
                <h3>
                    Looking for</h3>
                <ul>
                    <li> <asp:CheckBox runat="server" ID="chbLookingForRelationship" Text="" /> Relationship</li>
                    <li> <asp:CheckBox runat="server" ID="chbLookingForFriedship" Text="" /> Friendship</li>
                    <li> <asp:CheckBox runat="server" ID="chbLookingForBusiness" Text="" /> Business</li>
                </ul>
            </div>--%>
            <div class="leftFloater quarter">
                <h3>
                    Sexuality</h3>
                <ul>
                        <li><asp:RadioButton id="rbUndisclosed" runat="server" TextAlign="right" GroupName="rbSexuality" Checked="False"/> Undisclosed</li>
                        <li><asp:RadioButton id="rbGay" runat="server" TextAlign="right" GroupName="rbSexuality" Checked="False"/> Gay</li>
                        <li><asp:RadioButton id="rbStraight" runat="server" TextAlign="right" GroupName="rbSexuality" Checked="False"/> Straight</li>
                        <li><asp:RadioButton id="rbBisexual" runat="server" TextAlign="right" GroupName="rbSexuality" Checked="False"/> Bisexual</li>
       
                </ul>
            </div>
            <div class="clearfix">
            </div>
            <div class="leftFloater quarter">
                <h3>
                    Music</h3>
                <ul>
                    <li>
                        <asp:DropDownList runat="server" ID="dropMusic"></asp:DropDownList>
                    </li>
                </ul>
            </div>
            <div class="leftFloater quarter">
                <h3>
                    Interests</h3>
                <ul>
                    <li>
                        <asp:DropDownList runat="server" ID="drpinterests">
                            <asp:ListItem Value="-1" Text="select"></asp:ListItem>
                            <asp:ListItem Value="0" Text="Arts & Animation"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Auto & Vehicles"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Comedy"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Entertainment"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Extreme"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Music"></asp:ListItem>
                            <asp:ListItem Value="6" Text="News"></asp:ListItem>
                            <asp:ListItem Value="7" Text="People"></asp:ListItem>
                            <asp:ListItem Value="8" Text="Pets & Animals"></asp:ListItem>
                            <asp:ListItem Value="9" Text="Science & Technology"></asp:ListItem>
                            <asp:ListItem Value="10" Text="Sports"></asp:ListItem>
                            <asp:ListItem Value="11" Text="Travel & Places"></asp:ListItem>
                            <asp:ListItem Value="12" Text="Video Blogs"></asp:ListItem>
                            <asp:ListItem Value="13" Text="Video Comments"></asp:ListItem>
                            <asp:ListItem Value="14" Text="Video Games"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                </ul>
            </div>
            <div class="clearfix"></div>
            <p style="text-align:right;">
                <asp:Button runat="server" ID="btnSave" CssClass="form_btn" Text="Save" OnClick="btnSave_Click" />
            <p>
        </div>
    </div>
</asp:Content>
