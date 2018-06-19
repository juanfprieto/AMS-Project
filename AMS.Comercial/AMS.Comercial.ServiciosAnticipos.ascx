<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ServiciosAnticipos.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ServiciosAnticipos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<OBJECT id="prtTiquete" height="1" width="1" classid="CLSID:7AF0226B-7A49-4253-BA78-326BA0FA4497"
	VIEWASTEXT>
</OBJECT>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Información del Anticipo/Servicio:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 214px; HEIGHT: 18px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small" Width="150px" AutoPostBack="True"></asp:dropdownlist></td>
			<TD>&nbsp;</TD>
		</tr>
		<asp:panel id="pnlPlanilla" Visible="False" Runat="server">
			<TR>
				<TD style="WIDTH: 214px; HEIGHT: 18px">
					<asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Número de Planilla</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:dropdownlist id="ddlPlanilla" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></TD>
				<TD>&nbsp;</TD>
			</TR>
			<asp:panel id="pnlConcepto" Runat="server" Visible="False">
				<TR>
					<TD>
						<asp:label id="Label10" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa del Bus :</asp:label></TD>
					<TD>
						<asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0;', new Array(),1)"
							Font-Size="XX-Small" runat="server" Width="80px" MaxLength="6"></asp:textbox>&nbsp;
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 214px; HEIGHT: 18px">
						<asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Concepto:</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px">
						<asp:dropdownlist id="ddlConcepto" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="231px"></asp:dropdownlist></TD>
					<TD>&nbsp;</TD>
				</TR>
			</asp:panel>
		</asp:panel>
		<TR>
			<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlDatos" Visible="False" Runat="server">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Receptor:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label23" Font-Bold="True" Font-Size="XX-Small" runat="server">Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITReceptor" onclick="ModalDialog(this,'SELECT NIT AS NIT,NOMBRES AS NOMBRE,APELLIDOS AS APELLIDOS,RELACION AS RELACION from DBXSCHEMA.VTRANSPORTE_NITSRELACION order by NOMBRES', new Array(),1)"
						Font-Size="XX-Small" runat="server" Width="80px" MaxLength="60" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label24" Font-Bold="True" Font-Size="XX-Small" runat="server">Nombre :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITReceptora" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60"
						readonly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label25" Font-Bold="True" Font-Size="XX-Small" runat="server">Apellido :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITReceptorb" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60"
						readonly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Anticipo/Servicio:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 167px" vAlign="top">
					<asp:label id="Label18" Font-Bold="True" Font-Size="XX-Small" runat="server" Width="177px">DocumentoReferencia/CLAVE :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNumDocReferencia" Font-Size="XX-Small" runat="server" Width="200px" MaxLength="20"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 167px" vAlign="top">
					<asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></TD>
				<TD style="WIDTH: 386px" vAlign="top">
					<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="60px" Runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 167px; HEIGHT: 64px" vAlign="top">
					<asp:label id="Label13" Font-Bold="True" Font-Size="XX-Small" runat="server">Descripcion :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 64px" vAlign="top">
					<asp:textbox id="txtDescripcion" Font-Size="XX-Small" runat="server" Width="458px" ReadOnly="False"
						Height="58px" TextMode="MultiLine"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 167px" vAlign="top">
					<asp:label id="Label14" Font-Bold="True" Font-Size="XX-Small" runat="server">Cantidad Consumo :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtCantidad" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
						Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 167px" vAlign="top">
					<asp:label id="Label15" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Unidad :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtValorUnidad" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
						Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 167px" vAlign="top">
					<asp:label id="Label16" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Total :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtValorTotal" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
						Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 167px" vAlign="top">
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Continuar"></asp:button></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel>
	<asp:Panel ID="pnlConfirma" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Anticipo/Servicio:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Numero Documento :</asp:label></TD>
				<TD style="WIDTH: 386px" vAlign="top">
					<asp:label id="lblNumDocumento" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label19" Font-Bold="True" Font-Size="XX-Small" runat="server">Documento Referencia :</asp:label></TD>
				<TD style="WIDTH: 386px" vAlign="top">
					<asp:label id="lblNumDocumentoRef" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa :</asp:label></TD>
				<TD style="WIDTH: 386px" vAlign="top">
					<asp:label id="lblPlaca" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Receptor :</asp:label></TD>
				<TD style="WIDTH: 386px" vAlign="top">
					<asp:label id="lblNIT" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label17" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></TD>
				<TD style="WIDTH: 386px" vAlign="top">
					<asp:label id="lblFecha" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Descripcion :</asp:label></TD>
				<TD style="WIDTH: 386px" vAlign="top">
					<asp:label id="lblDescripcion" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Cantidad Consumo :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblCantidad" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Unidad :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblValorUnidad" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
					<asp:label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Total :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblValorTotal" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 137px" vAlign="top">
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:button id="btnRegistrar" Font-Bold="True" Font-Size="XX-Small" Width="80px" Runat="server"
						Text="Registrar"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:button id="btnAtras" Font-Bold="True" Font-Size="XX-Small" Width="80px" Runat="server"
						Text="Atras"></asp:button></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:Panel>
	<br>
</DIV>
<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label>
<script language="javascript">
    var txtDescripcion = document.getElementById("<%=txtDescripcion.ClientID%>");
    var txtCantidad = document.getElementById("<%=txtCantidad.ClientID%>");
    var txtValorUnidad = document.getElementById("<%=txtValorUnidad.ClientID%>");
    var txtValorTotal = document.getElementById("<%=txtValorTotal.ClientID%>");
    var txtNITReceptor = document.getElementById("<%=txtNITReceptor.ClientID%>");

    function validarGasto() {
        if (txtNITReceptor.value.length == 0) {
            alert('Debe ingresar el receptor.');
            return (false);
        }
        if (txtDescripcion.value.length == 0 || txtCantidad.value.length == 0 || txtValorUnidad.value.length == 0 || txtValorTotal.value.length == 0) {
            alert('Debe ingresar todos los datos del anticipo o servicio.');
            return (false);
        }
        return (true);
    }
    function Totales() {
        try {
            var totV = parseInt(txtCantidad.value.replace(/\,/g, '')) * parseInt(txtValorUnidad.value.replace(/\,/g, ''));
            parseValor(totV, txtValorTotal);
        }
        catch (err) {
            txtValorTotal.value = '';
        }
    }
    function parseValor(valor, objTot) {
        objTot.value = formatoValor(valor);
    }
    var prtTiquete = document.getElementById("prtTiquete");
    var strImprime = '<%=strImprime%>';
    if (strImprime.length > 0)
        prtTiquete.Imprimir(strImprime.replace(/\\n/g, '\n'));
</script>
