<%@ Page Language="C#" MasterPageFile="~/Main.master" Title="Untitled Page" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        int t = 0;
        if (Session["t"] != null)
        {
            t = (int)Session["t"];
            t++;
            Session["t"] = t;
        }
        else
        {
            Session["t"] = 0;
        }

        lblT.Text = t.ToString();
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label runat=server ID="lblT" />
</asp:Content>

