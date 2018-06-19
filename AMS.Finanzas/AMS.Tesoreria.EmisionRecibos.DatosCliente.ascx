<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EmisionRecibos.DatosCliente.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.DatosCliente" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table id="Table" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<strong>DATOS DEL CLIENTE</strong>
			</td>
			<td>
			</td>
			<td>
				<strong>POR CUENTA DE</strong>
			</td>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				CC/Nit :
			</td>
			<td>
				<asp:TextBox id="datCli" onDblClick="ModalDialog(this, 'select M.MNIT_nit, nombre, M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M, VMNIT VN, pciudad P WHERE M.MNIT_NIT = VN.MNIT_NIT AND M.pciu_codigo=P.pciu_codigo ORDER BY M.mnit_nit ',1,new Array())"
					ToolTip="Haga Click" runat="server" class="tpequeno"></asp:TextBox>
				<asp:RequiredFieldValidator id="validatorNit" runat="server" ControlToValidate="datCli" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                <asp:Image id="imgLupaCli" runat="server" ImageUrl="../img/AMS.Search.png" onclick="ModalDialog(this, 'select M.MNIT_nit, nombre, M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M, VMNIT VN, pciudad P WHERE M.MNIT_NIT = VN.MNIT_NIT AND M.pciu_codigo=P.pciu_codigo ORDER BY M.mnit_nit ',1,new Array())"></asp:Image>
			</td>
			<td>
				CC/Nit :
			</td>
			<td>
				<asp:TextBox id="datBen" onDblClick="ModalDialog(this, 'select M.MNIT_nit, nombre, M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M, VMNIT VN, pciudad P WHERE M.MNIT_NIT = VN.MNIT_NIT AND M.pciu_codigo=P.pciu_codigo ORDER BY M.mnit_nit ',1,new Array())"
					ToolTip="Haga Click" runat="server" class="tpequeno"></asp:TextBox>
				<asp:RequiredFieldValidator id="validatorNitBen" runat="server" ControlToValidate="datBen" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>
				Nombre :
			</td>
			<td>
				<asp:TextBox id="datClia" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
			</td>
			<td>
				Nombre :
			</td>
			<td>
				<asp:TextBox id="datBena" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Dirección :
			</td>
			<td>
				<asp:TextBox id="datClib" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
			</td>
			<td>
				Dirección :
			</td>
			<td>
				<asp:TextBox id="datBenb" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Ciudad :
			</td>
			<td>
				<asp:TextBox id="datClic" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
			</td>
			<td>
				Ciudad :
			</td>
			<td>
				<asp:TextBox id="datBenc" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Teléfono :
			</td>
			<td>
				<asp:TextBox id="datClid" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
			</td>
			<td>
				Teléfono :
			</td>
			<td>
				<asp:TextBox id="datBend" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				<center>
					<asp:Button id="aceptar" onclick="aceptar_Click" runat="server" Text="Aceptar" Enabled="True"></asp:Button>
				</center>
			</td>
		</tr>
	</tbody>
</table>
