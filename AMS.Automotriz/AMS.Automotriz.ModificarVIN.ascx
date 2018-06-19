<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.ModificarVIN.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ModificarVIN" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript">

    var MIN_TEXTLENGTH = 1;

    function ValidarCambio()
    {
<%--        var ddlNuevo = document.getElementById('<%=ddlVINNuevo.ClientID%>'); var tbNuevo = document.getElementById('<%=tbVINNuevo.ClientID%>'); if (ddlNuevo.value == '' && tbNuevo.value == '') { alert('Por favor ingrese el valor del VIN nuevo!'); return false; } return confirm('Esta seguro de realizar este proceso de cambio');--%>
    }
    function CambioValorTbNuevo()
    {
<%--        var tbNuevo = document.getElementById('<%=tbVINNuevo.ClientID%>'); var ddlNuevo = document.getElementById('<%=ddlVINNuevo.ClientID%>'); if (tbNuevo.value == '') ddlNuevo.disabled = false; else ddlNuevo.disabled = true;--%>
    }
    function CambioValorDdlNuevo()
    {
<%--        var tbNuevo = document.getElementById('<%=tbVINNuevo.ClientID%>'); var ddlNuevo = document.getElementById('<%=ddlVINNuevo.ClientID%>'); var ddlCatalogoNuevo = document.getElementById('<%=ddlCatalogoNuevo.ClientID%>'); if (ddlNuevo.value == '') { tbNuevo.value = ''; tbNuevo.disabled = ddlCatalogoNuevo.disabled = false; } else { tbNuevo.disabled = ddlCatalogoNuevo.disabled = true; }--%>
    }
    function abrirLupa()
    {
        var vin = document.getElementById('<%=txtVinViejo.ClientID%>');
        ModalDialog(vin, 'SELECT MCAT_VIN FROM MCATALOGOVEHICULO', new Array(), 1);
                         
    }
    function checkPostback(vin)
    {
        if (vin.value.length > MIN_TEXTLENGTH)
        {
            __doPostBack('<%=txtVinViejo.ClientID%>', vin.value);
        }
    }
    function changeUpper(obj)
    {
        var estado = document.getElementById("<%=lbEstado.ClientID%>").innerHTML.split(": ");
        document.getElementById("<%=txtVinNuevo.ClientID%>").value = obj.value.toUpperCase();
        if(estado[1] == "Usado")
        {
            if (obj.value.length > 1)
                document.getElementById("<%=btnConfirmarVin.ClientID%>").disabled = false;
            else
                document.getElementById("<%=btnConfirmarVin.ClientID%>").disabled = true;
        }
        else
        {
            if (obj.value.length != 17)
                document.getElementById("<%=btnConfirmarVin.ClientID%>").disabled = true;
            else
                document.getElementById("<%=btnConfirmarVin.ClientID%>").disabled = false;
        }
        
    }
</script>
<fieldset>
<legend>Información General Sobre la Referencia</legend>
	<table>
		<tr>
			<td><asp:Label Text="Seleccione o escriba el VIN que desea cambiar:" runat="server"></asp:Label>&nbsp;&nbsp;
			<%--<asp:dropdownlist id="ddlVINViejo" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="cargarInfoVin"></asp:dropdownlist>--%>
                <asp:TextBox ID="txtVinViejo" runat="server" Width="300px" onChange="checkPostback(this)"></asp:TextBox>
                
                <%--<asp:TextBox ID="txtVinViejo" runat="server" Width="300px" onblur="checkPostback(this)"></asp:TextBox>--%>
            <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirLupa();"></asp:Image>
			</td>
		</tr>
        <tr>
            <td style="width:605px;"></td>
            <td align="left" >
                <fieldset id="fldInfoVin" runat="server" visible="false" style="width:100%;margin:0;">
                    <legend>Información sobre el Vin</legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label id="lbCatalogo" runat="server"></asp:Label><br /><br />
                            </td>
                            <td>
                                <asp:Label id="lbPlaca" runat="server"></asp:Label><br /><br />
                            </td>
                            <td>
                                <asp:Label id="lbMotor" runat="server"></asp:Label><br /><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label id="lbNit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label id="lbColor" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label id="lbAno" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
	</table>
    <br /><br />
    <table>
        <tr>
            <td style="width:520px;">
                <asp:Label ID="lbEstado" runat="server" Font-Bold="true"></asp:Label><br />
                <asp:Label ID="lbText" runat="server" Text="Escriba acontinuación el VIN Nuevo: " Visible="false"></asp:Label>&nbsp;&nbsp;
                <asp:TextBox ID="txtVinNuevo" runat="server" Width="300px" Visible="false" onKeyUp="changeUpper(this)" ></asp:TextBox>
            </td>
            <td>
                <asp:Button id="btnConfirmarVin" runat="server" OnClick="confirmarVin" Text="Confirmar Existencia!" Visible="false" Enabled="false"/>
            </td>
        </tr>
        <tr>
            <td><br /><br />
                <asp:Label ID="lbConfirmacion" runat="server"></asp:Label><br />
                <%--<asp:CheckBox id="chkPrincipal" runat="server" Text="Cambiar Vin en la tabla principal y adyacentes.(VIN nuevo No existe)" Visible ="true"/><br />
                <asp:CheckBox id="chkRelacion" runat="server" Text="Cambiar sólo en las relaciones(El VIN nuevo ya existe)" Visible ="true"/>--%>
                <asp:Label ID="lbDescripcion" runat="server"></asp:Label>
                <%--<asp:RadioButtonList id="rBtnVin" runat="server" Visible="false" >
                    <asp:ListItem  Selected="True" Value="0">Cambiar Vin en la tabla principal y adyacentes(VIN nuevo No existe).</asp:ListItem>
                    <asp:ListItem Value="1">Cambiar sólo en las relaciones(El VIN nuevo ya existe).</asp:ListItem>
                </asp:RadioButtonList>--%>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button id="btnCambio" runat="server" Text="Realizar Cambio" onclick="btnCambio_Click" Visible="false" cssClass="noEspera"></asp:Button>
</fieldset>
<asp:Label ID="lbScript" runat="server" ></asp:Label>
