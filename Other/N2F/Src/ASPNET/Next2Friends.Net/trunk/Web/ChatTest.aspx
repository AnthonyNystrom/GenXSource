<%@ Page Language="C#" MasterPageFile="~/Main.master" Title="Untitled Page" %>

<script runat="server">

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <p>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" DataSourceID="ObjectDataSource1">
            <Columns>
                <asp:BoundField DataField="ChatID" HeaderText="ChatID" 
                    SortExpression="ChatID" />
                <asp:BoundField DataField="ChatWebID" HeaderText="ChatWebID" 
                    SortExpression="ChatWebID" />
                <asp:BoundField DataField="MemberIDFrom" HeaderText="MemberIDFrom" 
                    SortExpression="MemberIDFrom" />
                <asp:BoundField DataField="MemberIDTo" HeaderText="MemberIDTo" 
                    SortExpression="MemberIDTo" />
                <asp:BoundField DataField="Message" HeaderText="Message" 
                    SortExpression="Message" />
                <asp:CheckBoxField DataField="Delivered" HeaderText="Delivered" 
                    SortExpression="Delivered" />
                <asp:BoundField DataField="Fakey" HeaderText="Fakey" SortExpression="Fakey" />
                <asp:BoundField DataField="DTCreated" HeaderText="DTCreated" 
                    SortExpression="DTCreated" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetAllChat" TypeName="Next2Friends.Data.Chat">
        </asp:ObjectDataSource>
    </p>
</asp:Content>

