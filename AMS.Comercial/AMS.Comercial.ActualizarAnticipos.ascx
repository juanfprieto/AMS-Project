<%@ Control Language="c#" Debug="true" autoeventwireup="false" codebehind="AMS.Comercial.ActualizarAnticipos.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ActualizarAnticipos" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>

<DIV align="center">
	<table id="Table1" class="filtersIn">

		<tbody>
			<tr>
				<td colSpan="1"><b>Documento</b>
					<asp:textbox id="TextNumeroDocumento" runat="server" Width="78px" Height="24px"></asp:textbox></td>
				<td colSpan="2"><b>Placa:</b>
					<asp:textbox id="TxtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo > 0 AND TESTA_CODIGO <> 3', new Array(),1);TraerBus(this.value);"
						runat="server" Width="48px" Font-Size="XX-Small" MaxLength="6" Height="24px"></asp:textbox>
				<td colSpan="3"><b>Agencia</b>
					<asp:dropdownlist id="ddlAgencia" runat="server"></asp:dropdownlist></td>
				<td colSpan="4"><b>Concepto</b>
					<asp:dropdownlist id="ddlConcepto" runat="server"></asp:dropdownlist></td>
				<td colSpan="5"><b>Responsable</b>
					<asp:textbox id="TxtResponsable" ondblclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
						runat="server" Width="82px" Font-Size="XX-Small" ReadOnly="False" Height="24px"></asp:textbox>
				<td colSpan="6"><b>Fecha Inicial </b>
					<asp:textbox id="TextFechaI" onkeyup="DateMask(this)" runat="server" Width="84px"></asp:textbox></td>
				<td colSpan="6"><b>Fecha Final </b>
					<asp:textbox id="TextFechaF" onkeyup="DateMask(this)" runat="server" Width="79px"></asp:textbox></td>
				<td colSpan="7">
					<b>Planilla</b>
					<asp:textbox id="TextPlanilla" runat="server" Width="62px"></asp:textbox></td>
			</tr>
		</tbody>
	</table>
	<table id="Table2" class="filtersIn">
		<tbody>
			<TR>
				<TD colSpan="1"><asp:button id="btnBuscar" Font-Size="XX-Small" Font-Bold="True" Runat="server"
						Text="Buscar"></asp:button></TD>
			</TR>
			<tr>
				<td align="center" colSpan="1"><B>OTROS INGRESOS-GASTOS-ANTICIPOS 
						Y SERVICIOS:</B></td>
			</tr>
		</tbody>
	</table>
	<asp:panel id="pnlAnticipos" Runat="server" Visible="False" Width="889px" Height="509px">
<asp:datagrid id="dgrAnticipos" runat="server" cssclass="datagrid" OnItemCommand="dgActualizarAnticipo"
			PageSize="20" AllowPaging="True" ShowFooter="False" AutoGenerateColumns="False">
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="items"></ItemStyle>
			<HeaderStyle cssclass="header"></HeaderStyle>
			<Columns>
				<asp:TemplateColumn>
					<ItemTemplate>
						<asp:ImageButton id="btnEditar" ImageUrl="../img/Edit.jpg" AlternateText="Modificar Registro" Runat="server"
							CommandName="Actualizar" Height="18px"></asp:ImageButton>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<ItemTemplate>
						<asp:ImageButton id="ImageBorrar" ImageUrl="../img/Delete.jpg" AlternateText="Borrar Registro" Runat="server"
							CommandName="Borrar" Height="18px"></asp:ImageButton>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<ItemTemplate>
						<asp:ImageButton id="ImageImprimir" ImageUrl="../img/Imprimir.jpg" AlternateText="Imprimir Registro"
							Runat="server" CommandName="Imprimir" Height="18px"></asp:ImageButton>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="DOCUMENTO"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_DOCUMENTO" DataFormatString="{0:yyyy-MM-dd}"
					HeaderText="FECHA"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_AGENCIA" HeaderText="CDGO"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_AGENCIA" HeaderText="AGENCIA"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MCAT_PLACA" HeaderText="PLACA"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="BUS" HeaderText="BUS"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MPLA_CODIGO" HeaderText="PLANILLA"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_CONCEPTO" HeaderText="CDGO"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_CONCEPTO" HeaderText="CONCEPTO"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NIT_RESPONSABLE" HeaderText="NIT RESPNSBLE"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_GASTO"
					HeaderText="VALOR"></asp:BoundColumn>
			</Columns>
			<PagerStyle Mode="NumericPages"></PagerStyle>
		</asp:datagrid></TD></TR></TR></asp:panel></DIV>
