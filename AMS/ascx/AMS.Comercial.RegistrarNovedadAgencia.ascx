<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RegistrarNovedadAgencia.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RegistrarNovedadAgencia" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P>
	<asp:Label id="Label1" runat="server" Font-Bold="True" ForeColor="Black">Registrar Novedades Agencias</asp:Label>
	<HR width="100%" color="blue" SIZE="2">
<P>
	<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="488" border="0" style="WIDTH: 488px; HEIGHT: 57px">
		<TR>
			<TD style="WIDTH: 70px">
				<asp:Label id="Label2" runat="server" Font-Bold="True" ForeColor="Black">Agencia:</asp:Label></TD>
			<TD>
				<asp:DropDownList id="Agencias" runat="server" Width="250px" Font-Bold="True" AutoPostBack="True"></asp:DropDownList></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 70px">
				<asp:Label id="Label3" runat="server" Font-Bold="True" ForeColor="Black">Fecha:</asp:Label></TD>
			<TD>
				<asp:TextBox id="fecha" runat="server" Font-Bold="True"></asp:TextBox></TD>
		</TR>
		<tr>
			<td></td>
		</tr>
	</TABLE>
	<HR width="100%" color="blue" SIZE="2">
	<asp:Panel id="Panel1" runat="server" Height="336px">
		<FIELDSET style="WIDTH: 688px; HEIGHT: 557px"><LEGEND>Novedades</LEGEND>
			<TABLE id="Table2" style="WIDTH: 664px; HEIGHT: 517px" cellSpacing="0" cellPadding="0"
				width="664" border="0">
				<TR>
					<TD style="WIDTH: 154px">
						<asp:Label id="Label8" ForeColor="Black" Font-Bold="True" runat="server">Registrô:</asp:Label></TD>
					<TD>
						<asp:TextBox id="registro" ForeColor="Black" Font-Bold="True" runat="server" Width="200px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="registro" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px">
						<asp:Label id="Label4" ForeColor="Black" Font-Bold="True" runat="server" Width="160px">Responsable Novedad:</asp:Label></TD>
					<TD>
						<asp:TextBox id="responsable" ForeColor="Black" Font-Bold="True" runat="server" Width="200px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ControlToValidate="responsable" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 18px">
						<asp:Label id="Label5" ForeColor="Black" Font-Bold="True" runat="server">Asunto:</asp:Label></TD>
					<TD style="HEIGHT: 18px">
						<asp:DropDownList id="asunto" ForeColor="Black" Font-Bold="True" runat="server" Width="200px"></asp:DropDownList></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px">
						<asp:Label id="Label6" ForeColor="Black" Font-Bold="True" runat="server">Observaciones:</asp:Label></TD>
					<TD>
						<asp:TextBox id="observaciones" ForeColor="Black" Font-Bold="True" runat="server" Width="280px"
							Height="200px" TextMode="MultiLine"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px">
						<asp:Label id="Label7" ForeColor="Black" Font-Bold="True" runat="server">Solucion dada:</asp:Label></TD>
					<TD>
						<asp:TextBox id="solucion" ForeColor="Black" Font-Bold="True" runat="server" Width="280px" Height="200px"
							TextMode="MultiLine"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px">
						<asp:Button id="Registrar" onclick="Registrar_Click" runat="server" Text="Registrar"></asp:Button></TD>
					<TD>
						<asp:Label id="Label9" runat="server">Label</asp:Label></TD>
				</TR>
			</TABLE>
		</FIELDSET>
	</asp:Panel>
<P></P>
