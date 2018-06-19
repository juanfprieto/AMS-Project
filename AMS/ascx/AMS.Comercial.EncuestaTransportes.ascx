<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.EncuestaTransportes.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_EncuestaTransportes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>

<asp:label id="RemeasLabel" Font-Bold="True" Font-Size="Medium" runat="server">Encuesta Transportes</asp:label>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="500" border="0">
	<TR>
		<TD style="WIDTH: 143px; HEIGHT: 18px"><asp:label id="Label1" runat="server">Encuesta Numero:</asp:label></TD>
		<TD style="HEIGHT: 18px"><asp:label id="codencuesta" runat="server">Label</asp:label></TD>
	</TR>
	<TR>
		<TD style="WIDTH: 143px"><asp:label id="Label2" runat="server">Fecha Encuesta</asp:label></TD>
		<td><asp:label id="fechaencuesta" runat="server">Label</asp:label></TD>
	</TR>
	<TR>
		<TD style="WIDTH: 143px"><asp:label id="Label3" runat="server">Nombre Usuario</asp:label></TD>
		<td><asp:textbox id="nombre" runat="server" OnDBlClick="ModalDialog(this,'Select distinct NOMBRE_USUARIO from DBXSCHEMA.TUSUARIOREMESA', new Array(),1)"></asp:textbox></TD>
	</TR>
	<tr>
		<td style="WIDTH: 143px"><asp:label id="Label6" runat="server">Telefono Usuario</asp:label></td>
		<td><asp:textbox id="telefono" runat="server"></asp:textbox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Valor No Valido" ControlToValidate="telefono"
				ValidationExpression="\d+">Valor No Valido</asp:RegularExpressionValidator></td>
	</tr>
	<tr>
		<td style="WIDTH: 143px"><asp:label id="Label7" runat="server">Servicio a Calificar:</asp:label></td>
		<td><asp:dropdownlist id="servicio" runat="server" Width="130px"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td style="WIDTH: 143px"><asp:label id="Label8" runat="server">Calificacion</asp:label></td>
		<td><asp:dropdownlist id="calser" runat="server" Width="60px">
				<asp:ListItem Value="1">1</asp:ListItem>
				<asp:ListItem Value="2">2</asp:ListItem>
				<asp:ListItem Value="3">3</asp:ListItem>
				<asp:ListItem Value="4">4</asp:ListItem>
				<asp:ListItem Value="5">5</asp:ListItem>
				<asp:ListItem Value="6">6</asp:ListItem>
				<asp:ListItem Value="7">7</asp:ListItem>
				<asp:ListItem Value="8">8</asp:ListItem>
				<asp:ListItem Value="9">9</asp:ListItem>
				<asp:ListItem Value="10" Selected="True">10</asp:ListItem>
			</asp:dropdownlist></td>
	</tr>
	<tr>
		<td style="WIDTH: 143px"><asp:label id="Label9" runat="server">Que debemos Mejorar en este Servicio?</asp:label></td>
		<td><asp:textbox id="opservi" runat="server" Height="90px" Width="300px" TextMode="MultiLine"></asp:textbox></td>
	</tr>
	<tr>
		<td style="WIDTH: 143px; HEIGHT: 18px"><asp:label id="Label10" runat="server">Atencion al Cliente</asp:label></td>
		<td style="HEIGHT: 18px"><asp:dropdownlist id="calaten" runat="server" Width="60px">
				<asp:ListItem Value="1">1</asp:ListItem>
				<asp:ListItem Value="2">2</asp:ListItem>
				<asp:ListItem Value="3">3</asp:ListItem>
				<asp:ListItem Value="4">4</asp:ListItem>
				<asp:ListItem Value="5">5</asp:ListItem>
				<asp:ListItem Value="6">6</asp:ListItem>
				<asp:ListItem Value="7">7</asp:ListItem>
				<asp:ListItem Value="8">8</asp:ListItem>
				<asp:ListItem Value="9">9</asp:ListItem>
				<asp:ListItem Value="10" Selected="True">10</asp:ListItem>
			</asp:dropdownlist></td>
	</tr>
	<tr>
		<td style="WIDTH: 143px"><asp:label id="Label11" runat="server">Que Deberia Mejorar En La Atencion Al Cliente?</asp:label></td>
		<td><asp:textbox id="opaten" runat="server" Height="90px" Width="300px" TextMode="MultiLine"></asp:textbox></td>
	</tr>
	<tr>
		<td style="WIDTH: 143px"><asp:label id="Label12" runat="server">Opiniones Generales</asp:label></td>
		<td>
			<asp:TextBox id="opgen" runat="server" Height="90px" Width="300px" TextMode="MultiLine"></asp:TextBox></td>
	</tr>
	<TR>
		<td style="WIDTH: 143px">
			<asp:Button id="Guardar" runat="server" Text="Guardar Encuesta" OnClick="Grabar_Click"></asp:Button></td>
		<td></td>
	</TR>
	<TR>
		<td style="WIDTH: 143px"></td>
		<td></td>
	</TR>
</TABLE>
