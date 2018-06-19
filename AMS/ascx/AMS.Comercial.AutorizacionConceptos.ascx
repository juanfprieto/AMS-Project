<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AutorizacionConceptos.ascx.cs" Inherits="AMS.Comercial.AMS_Autorizacion_Conceptos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Tools.js"></script>
<DIV align="center">
	<table id="Table1" class="fieltersIn">
		<tbody>
			<TR>
				<TD colSpan="3"><B>Autorizacion Conceptos:</B></TD>
			</TR>
			<tr>
				<td ><b>OPCIONES:</b></td>
			</tr>
			<TR>
				<TD align="left"><asp:button style="Z-INDEX: 0" id="btnAdicionar" Width="80px" Height="25px" Font-Size="XX-Small"
						Font-Bold="False" Runat="server" Text="Adicionar" EnableViewState="False"></asp:button></TD>
				<TD align="center"><asp:button style="Z-INDEX: 0" id="btnBuscar" Width="80px" Height="24px" Font-Size="XX-Small"
						Font-Bold="False" Runat="server" Text="Buscar" EnableViewState="False"></asp:button></TD>
				<TD align="right"><asp:button id="btnSalir" Width="80px" Height="24px" Font-Size="XX-Small" Font-Bold="False"
						Runat="server" Text="Salir"></asp:button></TD>
			</TR>
		</tbody>
	</table>
	<BR>
	<asp:panel id="pnlBuscar" Width="889px" Height="509px" Runat="server" Visible="False">
		<TABLE id="Table2" class="fieltersIn">
			<TR>
				<TD colSpan="3"><B>Agencia</B>
					<asp:dropdownlist id="ddlAgencia1" runat="server"></asp:dropdownlist></TD>
				<TD colSpan="4"><B>Concepto</B>
					<asp:dropdownlist id="ddlCargo1" runat="server"></asp:dropdownlist></TD>
				<TD colSpan="4"><B>Concepto</B>
					<asp:dropdownlist id="ddlConcepto1" runat="server"></asp:dropdownlist></TD>
				<TD align="right">
					<asp:button style="Z-INDEX: 0" id="btnSeleccionar" Text="Seleccionar" Runat="server" Font-Bold="False"
						Font-Size="XX-Small" Height="24px" Width="80px"></asp:button></TD>
			</TR>
			</TR></TABLE>
		<asp:panel id="pnlAutorizaciones" Runat="server" Height="509px" Width="889px" Visible="False">
<asp:datagrid id="dgrAutorizaciones" runat="server" cssclass="datagrid" AllowPaging="True" PageSize="20"
				OnItemCommand="dgActualizarAutorizacion" AutoGenerateColumns="False" ShowFooter="False">
				<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
				<ItemStyle cssclass="item"></ItemStyle>
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
					<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_AGENCIA" HeaderText="CDGO"></asp:BoundColumn>
					<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_AGENCIA" HeaderText="AGENCIA"></asp:BoundColumn>
					<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_CONCEPTO" HeaderText="CDGO"></asp:BoundColumn>
					<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_CONCEPTO" HeaderText="CONCEPTO"></asp:BoundColumn>
					<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_MAXIMO_AUTORIZACION"
						HeaderText="VALOR"></asp:BoundColumn>
					<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_REPORTE" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"
						HeaderText="FECHA REPORTE"></asp:BoundColumn>
				</Columns>
				<PagerStyle Mode="NumericPages"></PagerStyle>
			</asp:datagrid></TD></TR></TR></asp:panel>
		<asp:panel id="pnlAdicion" Runat="server" Visible="False">
			<TABLE id="Table2" class="fieltersIn">
				<TR>
					<TD colSpan="3"><B>Adicionar Autorizaciones Conceptos Agencia:</B></TD>
				</TR>
				<TR>
					<TD>
						<asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label>
						<asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" Width="247px" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD align="left">
						<asp:datagrid style="Z-INDEX: 0" id="dgrAutorizacion" runat="server" cssclass="datagrid" AutoGenerateColumns="False"
							ShowFooter="True">
							<AlternatingItemStyle class="alternate"></AlternatingItemStyle>
							<ItemStyle cssclass="item"></ItemStyle>
							<HeaderStyle cssclass="header"></HeaderStyle>
							<FooterStyle cssclass="footer"></FooterStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Cargo">
									<ItemTemplate>
										<asp:dropdownlist id="ddlCargo" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
										</asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Concepto ">
									<ItemTemplate>
										<asp:dropdownlist id="ddlConcepto" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Vr. Autorizado">
									<ItemTemplate>
										<asp:textbox id="txtValorAutorizado" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
				</TR>
			</TABLE>
			<BR>
		</asp:panel>
	</asp:panel><asp:panel id="pnlActualizar" Runat="server" Visible="False">
		<TABLE id="Table3" class="fieltersIn">
			<TR>
				<TD colSpan="2"><B>Información Autorizacion:</B></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia:</asp:label></TD>
				<TD>
					<asp:label id="lblAgencia" Font-Bold="True" Font-Size="XX-Small" Width="20px" runat="server"></asp:label>&nbsp;&nbsp;</TD>
				<asp:label id="lblNombreAgencia" Font-Bold="True" Font-Size="XX-Small" Width="150px" runat="server"></asp:label></TD>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Cargo:</asp:label></TD>
				<TD>
					<asp:label id="lblCargo" Font-Bold="True" Font-Size="XX-Small" Width="20px" runat="server"></asp:label>&nbsp;&nbsp;</TD>
				<asp:label id="Label6" Font-Bold="True" Font-Size="XX-Small" Width="150px" runat="server"></asp:label></TD>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Concepto :</asp:label></TD>
				<TD>
					<asp:label id="lblConcepto" Font-Bold="True" Font-Size="XX-Small" Width="20px" runat="server"></asp:label>&nbsp;&nbsp;</TD>
				<asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" Width="150px" runat="server"></asp:label></TD>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label16" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Total :</asp:label></TD>
				<TD vAlign="top">
					<asp:textbox id="txtValorAutorizacion" onkeyup="NumericMask(this)" Font-Size="XX-Small" Width="100px"
						runat="server" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD colSpan="2" align="center">&nbsp;</TD>
			</TR>
		</TABLE>
		<TABLE id="Table4" class="fieltersIn">
			<TR>
				<TD vAlign="top">
				<TD vAlign="top">
					<asp:button id="BtnModificar" Text="Modificar" Runat="server" Font-Bold="True" Font-Size="XX-Small"></asp:button>&nbsp;&nbsp;&nbsp;</TD>
				<TD vAlign="top">
					<asp:button id="BtnBorrar" Text="Borrar" Runat="server" Font-Bold="True" Font-Size="XX-Small"
						Width="65px"></asp:button></TD>
				<TD vAlign="top">
					<asp:button id="BtnRegresar" Text="Regresar" Runat="server" Font-Bold="True" Font-Size="XX-Small"
						Width="65px"></asp:button></TD>
			</TR>
		</TABLE>
	</asp:panel><asp:label id="lblError" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label></DIV>
