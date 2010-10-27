<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MD.aspx.cs" Inherits="MD" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download Next2friends</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <%=UserAgent%><br />
    <%=MyMobilePhone.Manufacturer%><br />
    <%=MyMobilePhone.Model%><br />
    </div>
    
    <select id='manu' onchange='getModels(this.value);'>
        <option value=''>Model</option>
        <option value='LG'>LG</option>
        <option value='Motorola'>Motorola</option>
        <option value='Nokia'>Nokia</option>
        <option value='Samsung'>Samsung</option>
        <option value='Sony Ericcson'>Sony Ericcson</option>
    </select>
<br>
<select id='model' onchange=''></select>

</form>
<script type="text/javascript">

function getModels(manu){

    var index = 0;
    var models = document.getElementById('model');

    if(manu==''){
	    return;
    }

    models.options.length = 0;
    models.style.visibility = 'visible';

    for(var i=0;i<Devices.length;i++){
	    if(Devices[i][0]==manu){
    	
		    models.options[index++] = new Option(Devices[i][1], Devices[i][1], true, false);
	    }
    }
}

<%=MyMobilePhone.ToJavaScriptArray() %>

function autoSet(ma,mo){

    var manufacturer = document.getElementById('manu');
    var models = document.getElementById('model');
    
    for(var i=0;i<manufacturer.length;i++){
        if(manufacturer.options[i].value==ma){
            manufacturer.selectedIndex=i;
        }
    }
    
    getModels(ma);
    
    for(var i=0;i<models.length;i++){
        if(models.options[i].value==mo){
            models.selectedIndex=i;
        }
    }
}

autoSet('<%=MyMobilePhone.Manufacturer%>','<%=MyMobilePhone.Model%>');
</script>
    </form>
</body>
</html>
