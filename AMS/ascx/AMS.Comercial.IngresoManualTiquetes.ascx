<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.IngresoManualTiquetes.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_IngresoManualTiquetes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<P>
	<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
	<asp:label id="GirosLabel" Font-Size="XX-Small" Font-Bold="True" runat="server">Ingreso Manual de Tiquetes</asp:label>
	<HR style="WIDTH: 148.66%" width="148.66%" color="#000099" SIZE="3">
<P></P>
<FIELDSET style="WIDTH: 894px; HEIGHT: 406px"><LEGEND>Venta Manual Tiquetes</LEGEND>
	<TABLE id="Table1" style="WIDTH: 870px; HEIGHT: 366px" cellSpacing="0" cellPadding="0"
		width="870" bgColor="white" border="0">
		<tr>
			<td></td>
			<td></td>
		</tr>
		<tr>
			<td><asp:label id="Label5" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Fecha</asp:label></td>
			<td><asp:textbox id="fecha" Font-Size="XX-Small" runat="server"></asp:textbox></td>
		</tr>
		<tr>
			<td><asp:label id="Label6" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Hora</asp:label></td>
			<td><asp:textbox id="hora" Font-Size="XX-Small" runat="server" Width="70px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="HEIGHT: 13px"><asp:label id="Label7" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Bus</asp:label></td>
			<td style="HEIGHT: 13px"><asp:dropdownlist id="bus" Font-Size="XX-Small" runat="server" Width="100px"></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD style="HEIGHT: 10px"><asp:label id="Label1" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Agencia</asp:label></TD>
			<TD style="HEIGHT: 10px"><asp:dropdownlist id="agencia" Font-Size="XX-Small" runat="server" Width="250px"></asp:dropdownlist></TD>
			<TD style="HEIGHT: 10px"></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 15px"><asp:label id="Label2" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Despachador</asp:label></TD>
			<TD style="HEIGHT: 15px"><asp:dropdownlist id="despachador" Font-Size="XX-Small" runat="server" Width="250px"></asp:dropdownlist></TD>
			<TD style="HEIGHT: 15px"></TD>
		</TR>
		<TR>
			<td><asp:label id="Label3" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Planilla</asp:label></TD>
			<td><asp:textbox id="planilla" Font-Size="XX-Small" runat="server" Width="40px"></asp:textbox></TD>
			<td></TD>
		</TR>
		<TR>
			<td><asp:label id="Label4" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Tiquetes</asp:label></TD>
			<td></TD>
			<td></TD>
		</TR>
		<tr>
			<td></td>
			<td><asp:datagrid id="Grid" runat="server" Width="790px" HorizontalAlign="Center" AutoGenerateColumns="False">
					<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="NUMERO TIQUETE">
							<ItemTemplate>
								<asp:TextBox ID="NUMERO" Runat="server" Width="40"></asp:TextBox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="ESTADO">
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:DropDownList ID="DDLESTADO" Runat="server" Width="40"></asp:DropDownList>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="DESTINO">
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:DropDownList ID="DDLDESTINO" Runat="server" Width="60"></asp:DropDownList>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="VALOR">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:TextBox ID="VALOR" Runat="server"></asp:TextBox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="CANTIDAD">
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:TextBox ID="CANTIDAD" Runat="server" Width="30"></asp:TextBox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="TIPO TIQUETE">
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:DropDownList ID="TTIPOTIQUETE" Runat="server" Width="60"></asp:DropDownList>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="AGREGAR">
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:Button CommandName="AddDatasRow" ID="btnAdd" Runat="server" Text="AGREGAR" Font-Bold="True"
									Font-Size="XX-SMALL"></asp:Button>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
				</asp:datagrid></td>
		</tr>
		<tr>
			<td></td>
			<td></td>
		</tr>
		<tr>
			<td></td>
			<td><asp:button id="Guardar" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black"
					Text="Guardar"></asp:button></td>
		</tr>
	</TABLE>
</FIELDSET>
