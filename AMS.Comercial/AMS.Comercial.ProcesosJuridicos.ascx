<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ProcesosJuridicos.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ProcesosJuridicos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="RemeasLabel" Font-Bold="True" Font-Size="Medium" runat="server">Procesos Juridicos</asp:label>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
&nbsp;
<TABLE id="Table1" style="WIDTH: 740px; HEIGHT: 746px" cellSpacing="0" cellPadding="0"
	width="740" border="0">
	<TR>
		<TD><asp:label id="Label1" Font-Bold="True" runat="server" ForeColor="Black">Numero de Proceso Interno:</asp:label></TD>
		<TD><asp:label id="numproceso" Font-Bold="True" runat="server" ForeColor="Black">Label</asp:label></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD style="HEIGHT: 18px"><asp:label id="Label2" Font-Bold="True" runat="server" ForeColor="Black">Fecha Apertura:</asp:label></TD>
		<TD style="HEIGHT: 18px"><asp:label id="Label14" Font-Bold="True" runat="server" ForeColor="Black">Año</asp:label><asp:dropdownlist id="añoA" runat="server"></asp:dropdownlist><asp:label id="Label15" Font-Bold="True" runat="server" ForeColor="Black">Mes</asp:label><asp:dropdownlist id="mesA" runat="server"></asp:dropdownlist><asp:label id="Label16" Font-Bold="True" runat="server" ForeColor="Black">Dia</asp:label><asp:textbox id="diaA" runat="server" Width="30px"></asp:textbox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Valor Invalido" ControlToValidate="diaA"
				ValidationExpression="\d+">Valor Invalido</asp:RegularExpressionValidator></TD>
		<TD style="HEIGHT: 18px"></TD>
	</TR>
	<TR>
		<TD><asp:label id="Label3" Font-Bold="True" runat="server" ForeColor="Black">Placa:</asp:label></TD>
		<TD><asp:dropdownlist id="placa" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD style="HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" runat="server" ForeColor="Black">Demandado:</asp:label></TD>
		<TD style="HEIGHT: 18px"><asp:dropdownlist id="demandado" runat="server" Width="300px"></asp:dropdownlist></TD>
		<TD style="HEIGHT: 18px"></TD>
	</TR>
	<TR>
		<TD><asp:label id="Label5" Font-Bold="True" runat="server" ForeColor="Black">Demandante:</asp:label></TD>
		<TD><asp:textbox id="demandante" runat="server" Width="300px"></asp:textbox></TD>
		<TD></TD>
	</TR>
	<tr>
		<td><asp:label id="Label21" Font-Bold="True" runat="server" ForeColor="Black">Documento Identidad Demandante</asp:label></td>
		<td><asp:textbox id="docdemandante" runat="server"></asp:textbox></td>
		<td></td>
	</tr>
	<TR>
		<TD><asp:label id="Label6" Font-Bold="True" runat="server" ForeColor="Black">Asunto:</asp:label></TD>
		<TD><asp:textbox id="asunto" runat="server" Width="300px" TextMode="MultiLine" Height="90px"></asp:textbox></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD><asp:label id="Label7" Font-Bold="True" runat="server" ForeColor="Black">Juzgado:</asp:label></TD>
		<TD><asp:textbox id="juzgado" runat="server"></asp:textbox></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD><asp:label id="Label8" Font-Bold="True" runat="server" ForeColor="Black">Numero de proceso Juzgado:</asp:label></TD>
		<TD><asp:textbox id="procesojuz" runat="server" Width="160px"></asp:textbox></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD><asp:label id="Label9" Font-Bold="True" runat="server" ForeColor="Black">Pretenciones:</asp:label></TD>
		<TD><asp:textbox id="pretenciones" runat="server" Width="300px" TextMode="MultiLine" Height="90px"></asp:textbox></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD><asp:label id="Label10" Font-Bold="True" runat="server" ForeColor="Black">Observaciones:</asp:label></TD>
		<TD><asp:textbox id="observaciones" runat="server" Width="300px" TextMode="MultiLine" Height="90px"></asp:textbox></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD><asp:label id="Label11" Font-Bold="True" runat="server" ForeColor="Black">Actividades:</asp:label></TD>
		<TD><asp:textbox id="activiades" runat="server" Width="300px" TextMode="MultiLine" Height="90px"></asp:textbox></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD style="HEIGHT: 17px"><asp:label id="Label12" Font-Bold="True" runat="server" ForeColor="Black">Fecha Ultima Actuacion:</asp:label></TD>
		<TD style="HEIGHT: 17px"><asp:label id="Label20" Font-Bold="True" runat="server" ForeColor="Black">Año</asp:label><asp:dropdownlist id="añoU" runat="server"></asp:dropdownlist><asp:label id="Label19" Font-Bold="True" runat="server" ForeColor="Black">Mes</asp:label><asp:dropdownlist id="mesU" runat="server"></asp:dropdownlist><asp:label id="Label18" Font-Bold="True" runat="server" ForeColor="Black">Dia</asp:label><asp:textbox id="diaU" runat="server" Width="30px"></asp:textbox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ErrorMessage="Valor Invalido" ControlToValidate="diaU"
				ValidationExpression="\d+">Valor Invalido</asp:RegularExpressionValidator></TD>
		<TD style="HEIGHT: 17px"></TD>
	</TR>
	<tr>
		<td>
			<asp:Label id="Label24" runat="server" Font-Bold="True" ForeColor="Black">Fecha Proxima Diligencia</asp:Label></td>
		<td>
			<asp:TextBox id="proxima" runat="server" Font-Bold="True" ForeColor="White" Width="120px" BackColor="Red"></asp:TextBox></td>
	</tr>
	<TR>
		<TD><asp:label id="Label17" Font-Bold="True" runat="server" ForeColor="Black">Observaciones Ultima Actuacion:</asp:label></TD>
		<TD><asp:textbox id="obserult" runat="server" Width="300px" TextMode="MultiLine" Height="90px"></asp:textbox></TD>
		<TD></TD>
	</TR>
	<tr>
		<td><asp:label id="Label13" runat="server" Font-Bold="True" ForeColor="Black">Estado Proceso:</asp:label></td>
		<td><asp:dropdownlist id="estadoproceso" runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>
			<asp:Label id="Label23" runat="server" Font-Bold="True" ForeColor="Black">Ubicacion Fisica:</asp:Label></td>
		<td>
			<asp:TextBox id="ubicacion" runat="server" Width="300px" Height="90px" TextMode="MultiLine"></asp:TextBox></td>
	</tr>
	<tr>
		<td>
			<asp:Button id="Guardar" runat="server" Text="Guardar Proceso" OnClick="Guardar_Click"></asp:Button>
			<asp:Label id="Label22" runat="server">Label</asp:Label></td>
	</tr>
</TABLE>
