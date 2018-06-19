<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ModalDialogIns.aspx.cs" Inherits="AMS.Web.ModalDialogIns" %>
<%@ outputcache duration="10" varybyparam="params" %>
<HTML>
	<HEAD>
        <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />

        <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
        <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
		<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
		<script language="javascript" src="../js/AMS.Web.WizardDirection.js" type="text/javascript"></script>
		<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
		<script language="javascript">

        <%
            if(Request.QueryString["Vals"]!=null)
            {
                String parametros = Request.QueryString["Vals"];
                //Response.Write("alert(" + parametros.Replace("*", ",") + ");");
                Response.Write("parent.terminarDialogo( '" + parametros.Replace("*", ",") + "','');");
            }
        %>

    function digitoVerificacion(nit,dv){
    
        var nitStr = nit.value;
        
        if(/^(\d{9})$/.test(nitStr)){

            var aux;
            var residuo = 0;
            var primos = new Array(15);
            primos[0] = 3;
            primos[1] = 7;
            primos[2] = 13;
            primos[3] = 17;
            primos[4] = 19;
            primos[5] = 23;
            primos[6] = 29;
            primos[7] = 37;
            primos[8] = 41;
            primos[9] = 43;
            primos[10] = 47;
            primos[11] = 53;
            primos[12] = 59;
            primos[13] = 67;
            primos[14] = 71;
            
            var i;
            for(i=0;i<nitStr.length;i++){
                
                aux = nitStr.charAt(nitStr.length-1-i);
                residuo += parseInt(aux) * primos[i]; 
            }
            residuo %= 11;
            
            if(residuo > 1)
                residuo = 11 - residuo;
                
            dv.value = residuo;
        }
        else if(/\D/.test(nitStr)){
            alert('El nit sólo debe contener números!');
        }
    }
</script>
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="mainApp">
		<script language="javascript">
        <% if(Request.QueryString["reg"]=="1")Response.Write("alert('Registro actualizado.');"); %>
        <% if(Request.QueryString["reg"]=="0")Response.Write("alert('No se pudo actualizar el registro.');"); %>
		</script>
		<form runat="server">

            <div id="dialog" title="Basic dialog" style="visibility:hidden; box-shadow : 2px 2px 5px #222;">
            </div>
			<p>
				<asp:Label id="lbInfo" runat="server"></asp:Label>
			</p>
			<p>
				<asp:PlaceHolder id="phForm" runat="server"></asp:PlaceHolder>
			</p>
			<p>
				<asp:Button id="btInsert" onclick="InsertRecords" runat="server" Text="Añadir nuevo registro"></asp:Button>
			</p>
			<p>
			</p>
			<p>
			</p>
		</form>
	</body>
</HTML>

<script type="text/javascript">

    function Cargar_Nombre(obj) {
        ModalDialogIns.Cargar_Nombre(obj.value, Cargar_Nombre_CallBack);
    }

    function Cargar_Nombre_CallBack(response) {
        var respuesta = response.value;

        if (respuesta != "") {
            alert("El nit ingresado ya existe por favor revise !!!");

        }

    }
</script>