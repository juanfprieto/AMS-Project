<%@ Control Language="c#" codebehind="AMS.Vehiculos.EntregaVehiculosProceso.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.EntregaVehiculosProceso" %>

 
<link href="../css/bootstrap.css" type="text/css" rel="stylesheet">
<link href="../css/bootstrap-datetimepicker.css" type="text/css" rel="stylesheet"/>
<script type="text/javascript" src="../js/bootstrap.js"></script>
<script src="../js/bootstrap-datetimepicker.js" type="text/javascript"></script>
    


<script type="text/javascript">
    $(function ()
    {
        $("#<%=txtFechaMatricula.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
        var fechaVal = $("#<%=txtFechaMatricula.ClientID%>").val();
        $("#<%=txtFechaMatricula.ClientID%>").datepicker();
        $("#<%=txtFechaMatricula.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFechaMatricula.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFechaMatricula.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Icon.Calendar.png");
        $("#<%=txtFechaMatricula.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFechaMatricula.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFechaMatricula.ClientID%>").val();

        $("#<%=fechaEntrega.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
        var fechaVal = $("#<%=fechaEntrega.ClientID%>").val();
        $("#<%=fechaEntrega.ClientID%>").datepicker();
        $("#<%=fechaEntrega.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=fechaEntrega.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=fechaEntrega.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Icon.Calendar.png");
        $("#<%=fechaEntrega.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=fechaEntrega.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=fechaEntrega.ClientID%>").val();

        $("#<%=txtFechaMatriculaE.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
        var fechaVal = $("#<%=txtFechaMatriculaE.ClientID%>").val();
        $("#<%=txtFechaMatriculaE.ClientID%>").datepicker();
        $("#<%=txtFechaMatriculaE.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFechaMatriculaE.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFechaMatriculaE.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Icon.Calendar.png");
        $("#<%=txtFechaMatriculaE.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFechaMatriculaE.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFechaMatriculaE.ClientID%>").val();

        $("#<%=txtFechaEntrega.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
        var fechaVal = $("#<%=txtFechaEntrega.ClientID%>").val();
        $("#<%=txtFechaEntrega.ClientID%>").datepicker();
        $("#<%=txtFechaEntrega.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFechaEntrega.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFechaEntrega.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Icon.Calendar.png");
        $("#<%=txtFechaEntrega.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFechaEntrega.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFechaEntrega.ClientID%>").val();
    });
    function abrirEmergente(obj)
    {
        var str = '**NITS_NITENTREGAVEH';
        ModalDialog(obj, str, new Array(), 1);
    }
</script>

<fieldset style="width: 25%; margin: 5px; padding: 20px; margin-left : auto; margin-right: auto;" id="fldProgramacionEntrega" runat="server" visible="false"> 
    <legend> Programar Entrega</legend>
    <center>
        V.I.N: <asp:Label ID="lbVehiculo" runat="server"></asp:Label> <br />
        Prefijo: <asp:Label ID="lbPrefijo" runat="server"></asp:Label> <br /><br />
        <%--Número <asp:Label ID="lbNumero" runat="server"></asp:Label> <br />--%>
        <b>Placa </b><asp:TextBox ID="txtPlaca" runat="server" placeholder="Placa" CssClass="tmediano"></asp:TextBox>
        <b>Núm Inventario </b><asp:TextBox ID="txtInventario" runat="server" placeholder="# inventario" CssClass="tmediano" ReadOnly="true"></asp:TextBox>
        <b>Núm. Matrícula </b><asp:TextBox ID="txtMatricula" runat="server" placeholder="# matrícula" CssClass="tmediano" ></asp:TextBox>
        <b>Fecha matrícula </b><br /><asp:TextBox ID="txtFechaMatricula" runat="server" placeholder="Fecha matrícula" Enabled="false" style="display: inline-block; width:237px;"></asp:TextBox><br />
        <b>Ciudad Matricula </b><br /><asp:DropDownList ID="ddlCiudadMatricula" runat="server" style="width:260px;" ></asp:DropDownList>
        <b>Fecha programada de Entrega </b><br /><asp:TextBox ID="txtFechaEntrega" runat="server" placeholder="Fecha programada de Entrega" style="display: inline-block; width:237px;"></asp:TextBox><br />
        <b>Hora entrega </b><asp:TextBox ID="txtHoraEntrega" runat="server" placeholder="HH" Width="50px" style="display: inline-block;" MaxLength="2" onkeypress="return soloNumero(event, this)"></asp:TextBox> : 
        <asp:TextBox ID="txtMinEntrega" runat="server" placeholder="MM" Width="50px" style="display:inline-block;" MaxLength="2" onkeypress="return soloNumero(event, this)"></asp:TextBox> <b>formato 24 Horas</b><br />
        <b>Entrega</b><br /><asp:TextBox ID="txtNitEntrega" runat="server" placeholder="Quién entrega?" CssClass="tmediano" ReadOnly="true" onClick="abrirEmergente(this);"></asp:TextBox>
        <asp:TextBox ID="txtNitEntregaa" runat="server" placeholder="Nombre Quién entrega?" CssClass="tmediano" ReadOnly="true"></asp:TextBox>
        <b>Recibe</b><br /><asp:TextBox ID="txtNitRecibe" runat="server" placeholder="Nit Quién recibe" CssClass="tmediano" onkeypress="return soloNumero(event, this)" style="display:inline-block;"></asp:TextBox> <%--este nit puede o no, existir--%>
        <asp:TextBox ID="txtNombreRecibe" runat="server" placeholder="Nombre Quién recibe" CssClass="tmediano"></asp:TextBox>
        <b>Almacen </b><br /><asp:DropDownList ID="ddlAlmacen" runat="server" placeholder="Almacen" CssClass="tmediano" AutoPostBack="true"></asp:DropDownList> <%--OnSelectedIndexChanged="cargarCiudades"--%>
        <br /><asp:Button id="btnProgramar" runat="server" OnClick="programarEntrega" Text="Programar!"/>
    </center>
</fieldset>

<asp:PlaceHolder ID="plhEntrega" runat="server">
<fieldset style="width: 25%; margin: 5px; padding: 20px; margin-left : auto; margin-right: auto;" ID="fldentrega">
    <center>
    <legend>Entrega vehiculo</legend>
    <p>
	    Por Favor Digite los datos para la entrega del Vehículo<br />
	    <asp:Label id="vehiculo" runat="server" CssClass="tmediano"></asp:Label><br />
        con el formato año-mes-dia(yyyy-mm-dd) :
    </p><br />
     <b> Documento de Entrega : <br />
                <asp:dropdownlist id="prefijoEntrega" runat="server" CssClass="tmediano" 
                AutoPostBack="true"></asp:dropdownlist></td><br />
    <b>Fecha Exacta de Entrega </b><br /><asp:TextBox id="fechaEntrega" runat="server" style="display: inline-block; width:237px;"></asp:TextBox><br />
    <b>Hora entrega </b><asp:TextBox ID="txthoraendefinitiva" runat="server" placeholder="HH" Width="50px" style="display: inline-block;" MaxLength="2" onkeypress="return soloNumero(event, this)"></asp:TextBox> : 
        <asp:TextBox ID="txtmindefinitiva" runat="server" placeholder="MM" Width="50px" style="display:inline-block;" MaxLength="2" onkeypress="return soloNumero(event, this)"></asp:TextBox> <b>formato 24 Horas</b><br />
    <b>Placa </b><br /><asp:TextBox id="txtPlacaE" CssClass="tmediano" runat="server"></asp:TextBox>
    <b>Fecha matrícula </b><br /><asp:TextBox id="txtFechaMatriculaE" runat="server" style="display: inline-block; width:237px;"></asp:TextBox><br />
    <b>Numero matrícula </b><br /><asp:TextBox id="txtMatriculaE" CssClass="tmediano" runat="server"></asp:TextBox>
    <b>Ciudad Matricula </b><br /><asp:DropDownList ID="ddlCiudadMatriculadefini" runat="server" CssClass="tmediano" ></asp:DropDownList>
    <b>Entrega</b><br /><asp:TextBox ID="txtNitEntregadefini" runat="server" placeholder="Quién entrega?" CssClass="tmediano" ReadOnly="true" onClick="abrirEmergente(this);"></asp:TextBox>
    <asp:TextBox ID="txtNitEntregadefinia" runat="server" placeholder="Nombre Quién entrega?" CssClass="tmediano" ReadOnly="true"></asp:TextBox>
    <b>Recibe</b><br /><asp:TextBox ID="txtNitRecibedefini" runat="server" placeholder="Nit Quién recibe" CssClass="tmediano" onkeypress="return soloNumero(event, this)" style="display:inline-block;"></asp:TextBox> <%--este nit puede o no, existir--%>
    <asp:TextBox ID="txtNombreRecibedefini" runat="server" placeholder="Nombre Quién recibe" CssClass="tmediano" ></asp:TextBox> 
    <b>Almacen </b><br /><asp:DropDownList ID="ddlAlmacendefini" runat="server" placeholder="Almacen" CssClass="tmediano" AutoPostBack="true"></asp:DropDownList>
    <br /><asp:Button id="btnEntrega" onclick="Efectuar_Entrega" runat="server" Text="Efectuar Entrega"></asp:Button>
    </center>
</fieldset>
</asp:PlaceHolder>




<%--<div class="container" >
    <div class="row">
        <div class='col-sm-6'>
            <div class="form-group">
                <div class='input-group date' id='datetimepicker1' runat="server">
                    <input type='text' class="form-control" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
    </div>
</div>--%>
<br />
<%--<div class="container">
    <div class="row">
        <div class='col-sm-6'>
            <div class="form-group">
                <div class='input-group date' id='datetimepicker5'>
                    <input type='text' class="form-control" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
    </div>
</div>--%>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>


