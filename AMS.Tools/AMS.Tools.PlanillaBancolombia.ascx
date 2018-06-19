<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.PlanillaBancolombia.ascx.cs" Inherits="AMS.Tools.PlanillaBancolombia" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<p>
	<table id="Table1" class="filtersIn">
		<tbody>
			<tr>
				<td>AÑO</td>
				<td><asp:DropDownList id="ddlano" runat="server"></asp:DropDownList></td>
			</tr>
			<tr>
				<td>MES</td>
				<td><asp:DropDownList id="ddlmes" runat="server" ></asp:DropDownList></td>
			</tr>
            <tr>
				<td>QUINCENA</td>
				<td><asp:DropDownList id="ddlquincena" runat="server"></asp:DropDownList></td>
			</tr>
			<tr>
				<td>FECHA</td>
				<td><asp:TextBox id="Fecha" runat="server" onkeyup="DateMask(this)" ></asp:TextBox></td>
			</tr>


		</tbody>
	</table>
</p>
<br>
<P>
<asp:Label id="info" runat="server" Visible="false">Para descargar el archivo haga Click derecho sobre el Link abajo y seleccione "Guardar como..." para determinar la ruta de almacenamiento del archivo de texto.</asp:Label>
<asp:hyperlink download id="hl" runat="server" Visible="False">HyperLink</asp:hyperlink></P>
<asp:button id="btnGenerar1" runat="server" onClick="IniciarProceso" Text="Generar"></asp:button>