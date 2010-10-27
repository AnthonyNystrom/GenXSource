<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label runat=server ID="lblT" />
    </div>
    </form>
</body>
</html>
