<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AsignarAgencias.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_AsignarAgencias" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="fieltersIn">
		<tr>
			<td colSpan="3"><b>Información del Usuario:</b></td>
		</tr>
		<tr>
			<td><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Usuario :</asp:label></td>
			<td><asp:dropdownlist id="ddlUsuario" runat="server" Font-Size="XX-Small" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
		</tr>
		<TR>
			<td>&nbsp;</TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlAgencia" Visible="False" Runat="server">
		<TABLEid="Table2" class="fieltersIn">
			<TR>
				<TD colSpan="3"><B>Agencias Asignadas:</B></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<td>
					<asp:datagrid id="dgrAgencias" runat="server" cssclass="datagrid" DataKeyField="CODIGO" AutoGenerateColumns="False">
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Seleccione" ItemStyle-HorizontalAlign="Left">
								<ItemTemplate>
									<asp:CheckBox id="chkAgencia" Font-Size="XX-Small" Runat=server Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "EXISTE")) %>'>
									</asp:CheckBox>&nbsp;&nbsp;<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid>
					<asp:CheckBoxList id="chkAgencias" Runat="server"></asp:CheckBoxList></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<TD align="left">
					<asp:button id="btnSeleccionar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Asignar Agencias"></asp:button></TD>
			</TR>
			<TR>
				<TD colSpan="3">
					<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label>&nbsp;</TD>
			</TR>
			<TR>
				<TD colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel></DIV>
