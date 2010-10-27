<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MyNspots.aspx.cs" Inherits="MyNspots" Title="My NSpots" %>
<%@ Register TagPrefix="N2F" Namespace="Next2Friends.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- middle start -->
    <div id="middle" class="clearfix">
        <!--subnav start -->
        <ul id="subnav" class="clearfix">
            <li><a href="/MatchProfile.aspx">My Tagging Profile</a></li>
            <%if (IsLoggedIn)
              { %><li><a href="/MyNspots.aspx">New NSpot</a></li><%} %>
        </ul>
        <!-- subnav end -->
        <!-- page start -->
        <div class="page clearfix fullPageBkg">
            <h2>
                NSpots</h2>
            <p>
                Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Nunc euismod adipiscing
                magna. Quisque sollicitudin nisi a nisi feugiat suscipit. Praesent molestie. Curabitur
                laoreet, augue in pharetra adipiscing, ipsum lectus blandit leo, quis viverra odio
                purus eget lectus. Cras vitae libero. Duis sed pede id erat laoreet varius. Ut felis
                est, aliquet ut, dictum at, consectetuer sed, ante.
            </p>
            <%if (ShowWizard)
              { %>
            <div class="nspots_wizard">
                <p>
                    <label>
                        NSpot Name</label><asp:TextBox runat="server" ID="txtNspotName" CssClass="form_txt longInput"></asp:TextBox>
                        <asp:Literal runat="server" ID="litNspotName">&nbsp;</asp:Literal>
                </p>
                <p>
                    <label>
                        Open time</label>
                    <N2F:DayDropdown runat="server" ID="drpDay" CssClass="form_menu smallInput">
                    </N2F:DayDropdown>
                    <N2F:MonthDropdown runat="server" ID="drpMonth" CssClass="form_menu smallInput">
                    </N2F:MonthDropdown>
                    <N2F:CurrentYearDropdown runat="server" ID="drpYear" CssClass="form_menu smallInput">
                    </N2F:CurrentYearDropdown>
                    -
                    <N2F:HourDropdown runat="server" ID="drpHour" CssClass="form_menu smallInput">
                    </N2F:HourDropdown>
                    <N2F:MinuteDropdown runat="server" ID="drpMinute" CssClass="form_menu smallInput">
                    </N2F:MinuteDropdown>
                    <asp:Literal runat="server" ID="litBeginTime">&nbsp;</asp:Literal>
                </p>
                <p>
                    <label>
                        Duration</label>
                    <N2F:HourDurationDropdown runat="server" ID="drpHourLast" CssClass="form_txt longInput">
                    </N2F:HourDurationDropdown> Hours
                    <asp:Literal runat="server" ID="litDuration">&nbsp;</asp:Literal>
                </p>
                <p>
                    <label>
                        Description</label>
                    <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" CssClass="form_txt longInput" />
                    <asp:Literal runat="server" ID="litDescription">&nbsp;</asp:Literal>
                </p>
                <p>
                    <label>
                        Main Photo</label>
                    <asp:FileUpload runat="server" ID="FileUpload" CssClass="form_txt longFileUpload" />
                    <asp:Literal runat="server" ID="litBrowsePhoto">&nbsp;</asp:Literal>
                </p>
                <p class="indent">
                    <asp:CheckBox runat="server" ID="ChbMakePrivate" />
                    Automatically accept new members (you can remove members later)</p>
                <p class="indent">
                    <asp:Button runat="server" ID="btnSubmit" Text="Create NSpot" CssClass="form_btn"
                        OnClick="btnSubmit_Click" /></p>
            </div>
            <%}
              else
              { %>
            Your hotspot has been created. <a href="">Create another</a>
            <%} %>
        </div>
        
        		<div class="friend_list clearfix" id="divFriends">
                    <%=DefaultNSpotLister %>
			</div>
			

	
        <!-- page end -->
    </div>
    <!-- middle end -->
</asp:Content>
