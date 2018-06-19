<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModProcesosJuridicos.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModProcesosJuridicos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="RemeasLabel" runat="server" Font-Size="Medium" Font-Bold="True">Modificar Procesos Juridicos</asp:label>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
&nbsp;
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
	<TR>
		<td><asp:label id="Label1" runat="server" Font-Bold="True">Proceso a Modificar:</asp:label></TD>
		<td><asp:dropdownlist id="procesos" runat="server"></asp:dropdownlist></TD>
	</TR>
	<tr>
		<td><asp:button id="cargar" runat="server" Text="Cargar" OnClick="Cargar_Click"></asp:button></td>
	</tr>
</TABLE>
<asp:panel id="Panel1" runat="server" Height="350px" Width="720px" Visible="False">
	<TABLE id="Table2" style="WIDTH: 680px; HEIGHT: 708px" cellSpacing="0" cellPadding="0"
		width="680" border="0">
		<TR>
			<td>
				<asp:Label id="Label2" Font-Bold="True" runat="server" ForeColor="Black">Fecha de Apertura:</asp:Label></TD>
			<td>
				<asp:Label id="fechaA" runat="server">Label</asp:Label></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label3" Font-Bold="True" runat="server" ForeColor="Black">Numero de Proceso Interno:</asp:Label></TD>
			<td>
				<asp:Label id="numproI" runat="server">Label</asp:Label></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label4" Font-Bold="True" runat="server" ForeColor="Black">Placa Vehiculo:</asp:Label></TD>
			<td>
				<asp:Label id="placa" runat="server">Label</asp:Label></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label5" Font-Bold="True" runat="server" ForeColor="Black">Demandado:</asp:Label></TD>
			<td>
				<asp:Label id="demandado" runat="server">Label</asp:Label></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label6" Font-Bold="True" runat="server" ForeColor="Black">Demandante:</asp:Label></TD>
			<td>
				<asp:Label id="demandante" runat="server">Label</asp:Label></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label7" Font-Bold="True" runat="server" ForeColor="Black">Documento de Identidad Demandante:</asp:Label></TD>
			<td>
				<asp:Label id="docdemandante" runat="server">Label</asp:Label></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label8" Font-Bold="True" runat="server" ForeColor="Black">Asunto:</asp:Label></TD>
			<td>
				<asp:TextBox id="asunto" runat="server" Width="300px" Height="90px" TextMode="MultiLine"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label9" Font-Bold="True" runat="server" ForeColor="Black">Juzgado:</asp:Label></TD>
			<td>
				<asp:TextBox id="juzgado" runat="server" Width="160px"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label11" Font-Bold="True" runat="server" ForeColor="Black">Numero Proceso en Juzgado:</asp:Label></TD>
			<td>
				<asp:TextBox id="nomprocJ" runat="server" Width="160px"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label12" Font-Bold="True" runat="server" ForeColor="Black">Pretenciones:</asp:Label></TD>
			<td>
				<asp:TextBox id="pretenciones" runat="server" Width="300px" Height="90px" TextMode="MultiLine"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label13" Font-Bold="True" runat="server" ForeColor="Black">Observaciones:</asp:Label></TD>
			<td>
				<asp:TextBox id="observaciones" runat="server" Width="300px" Height="90px" TextMode="MultiLine"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label14" Font-Bold="True" runat="server" ForeColor="Black">Actividades:</asp:Label></TD>
			<td>
				<asp:TextBox id="actividades" runat="server" Width="300px" Height="90px" TextMode="MultiLine"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label15" Font-Bold="True" runat="server" ForeColor="Black">Fecha Ultima Actualizacion:</asp:Label></TD>
			<td>
				<asp:Label id="Label18" Font-Bold="True" runat="server" ForeColor="Black">Año</asp:Label>
				<asp:TextBox id="Año" runat="server" Width="56px"></asp:TextBox>
				<asp:Label id="Label19" Font-Bold="True" runat="server" ForeColor="Black">Mes</asp:Label>
				<asp:TextBox id="Mes" runat="server" Width="56px"></asp:TextBox>
				<asp:Label id="Label20" Font-Bold="True" runat="server" ForeColor="Black">Dia</asp:Label>
				<asp:TextBox id="DiaF" runat="server" Width="40px"></asp:TextBox>
				<asp:RangeValidator id="RangeValidator1" runat="server" MaximumValue="31" MinimumValue="1" ControlToValidate="DiaF"
					Type="Integer" ErrorMessage="Dia Invalido"></asp:RangeValidator>
				<asp:RangeValidator id="RangeValidator2" runat="server" MaximumValue="12" MinimumValue="1" ControlToValidate="Mes"
					Type="Integer" ErrorMessage="Mes Invalido"></asp:RangeValidator></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 22px">
				<asp:Label id="Label21" Font-Bold="True" runat="server" ForeColor="Black">Fecha Proxima Diligencia</asp:Label></TD>
			<TD style="HEIGHT: 22px">
				<asp:TextBox id="fechaprox" Font-Bold="True" runat="server" Width="120px" ForeColor="White" BackColor="Red"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label16" Font-Bold="True" runat="server" ForeColor="Black">Observaciones Ultima Actualizacion:</asp:Label></TD>
			<td>
				<asp:TextBox id="obsult" runat="server" Width="300px" Height="90px" TextMode="MultiLine"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label10" Font-Bold="True" runat="server" ForeColor="Black">Estado:</asp:Label></TD>
			<td>
				<asp:DropDownList id="estado" runat="server"></asp:DropDownList></TD>
		</TR>
		<TR>
			<td>
				<asp:Label id="Label22" Font-Bold="True" runat="server" Visible="False" ForeColor="Black">Ubicacion Fisica:</asp:Label></TD>
			<td>
				<asp:TextBox id="ubicacion" runat="server" Width="300px" Height="90px" TextMode="MultiLine"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:Button id="Actualizar" onclick="Actualizacion_Click" runat="server" Text="Actualizar"></asp:Button></TD>
			<td>
				<asp:Label id="Label17" runat="server">Label</asp:Label></TD>
		</TR>
	</TABLE>
</asp:panel>
