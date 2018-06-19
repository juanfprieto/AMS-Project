<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.VerHojasVida.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_VerHojasVida" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="RemeasLabel" Font-Size="Medium" Font-Bold="True" runat="server">Consultar Hojas de Vida</asp:label>
<P></P>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
	<TR>
		<TD style="HEIGHT: 12px">
			<asp:Label id="Label1" Font-Bold="True" runat="server" ForeColor="Black">Empleado:</asp:Label>
		</TD>
		<TD style="HEIGHT: 12px">
			<asp:DropDownList id="empleado" runat="server"></asp:DropDownList></TD>
	</TR>
	<TR>
		<TD>
			<asp:Button id="Consultar" runat="server" Text="Consultar" OnClick="Generar_Click"></asp:Button></TD>
		<TD></TD>
	</TR>
</TABLE>
<asp:Panel id="Panel1" runat="server" Height="352px" Width="648px" Visible="False">
	<P>
		<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
	<P></P>
	<TABLE id="Table2" style="WIDTH: 774px; HEIGHT: 852px" cellSpacing="0" cellPadding="0"
		width="774" border="0">
		<TR>
			<TD>
				<asp:Image id="foto" runat="server" Width="120px" Height="120px" ImageAlign="Left"></asp:Image></TD>
			<TD style="WIDTH: 116px"></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 6px">
				<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
				&nbsp;</TD>
			<TD style="WIDTH: 116px; HEIGHT: 6px"></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 12px">
				<asp:Label id="Label12" runat="server" Font-Bold="True" ForeColor="Black">Nit:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 12px">
				<asp:Label id="nitLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label54" runat="server" Font-Bold="True" ForeColor="Black">E.P.S</asp:Label></TD>
			<TD>
				<asp:Label id="epsLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 17px">
				<asp:Label id="Label2" runat="server" Font-Bold="True" ForeColor="Black">Nombre:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 17px">
				<asp:Label id="nombre" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label56" runat="server" Font-Bold="True" ForeColor="Black">Numero Afiliacion EPS:</asp:Label></TD>
			<TD>
				<asp:Label id="NumeroAepsLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 5px">
				<asp:Label id="Label19" runat="server" Font-Bold="True" ForeColor="Black">Sexo:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 5px">
				<asp:Label id="sexoLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label57" runat="server" Font-Bold="True" ForeColor="Black">Fondo de Pension:</asp:Label></TD>
			<TD>
				<asp:Label id="FondoPLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 22px">
				<asp:Label id="Label30" runat="server" Font-Bold="True" ForeColor="Black" DESIGNTIMEDRAGDROP="497">Tipo de Sangre:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 22px">
				<asp:Label id="SangreLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label60" runat="server" Font-Bold="True" ForeColor="Black">#Contrato Fonde de Pension:</asp:Label></TD>
			<TD>
				<asp:Label id="NumCFPLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 19px">
				<asp:Label id="Label8" runat="server" Font-Bold="True" ForeColor="Black">Estado Civil:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 19px">
				<asp:Label id="estadoCivil" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label62" runat="server" Font-Bold="True" ForeColor="Black">Fondo de Censantias:</asp:Label></TD>
			<TD>
				<asp:Label id="FcesantiasLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label21" runat="server" Font-Bold="True" ForeColor="Black"># Hijos</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="HijosLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label64" runat="server" Font-Bold="True" ForeColor="Black">#Contrato Fondo de Cesantias:</asp:Label></TD>
			<TD>
				<asp:Label id="NumCFondoCLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 10px">
				<asp:Label id="Label3" runat="server" Font-Bold="True" ForeColor="Black">Estado:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 10px">
				<asp:Label id="estado" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD style="HEIGHT: 10px">
				<asp:Label id="Label66" runat="server" Font-Bold="True" ForeColor="Black">A.R.P:</asp:Label></TD>
			<TD style="HEIGHT: 10px">
				<asp:Label id="arpLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label10" runat="server" Font-Bold="True" ForeColor="Black">Fecha de Nacimiento:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="fechaNacimiento" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label68" runat="server" Font-Bold="True" ForeColor="Black">#Contrato A.R.P:</asp:Label></TD>
			<TD>
				<asp:Label id="NumCarpLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label13" runat="server" Font-Bold="True" ForeColor="Black">Lugar de Nacimiento:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="naciemientoLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label70" runat="server" Font-Bold="True" ForeColor="Black">#Licencia de Conduccion:</asp:Label></TD>
			<TD>
				<asp:Label id="LicenciaLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label4" runat="server" Font-Bold="True" ForeColor="Black">Fecha de Ingreso:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="fechaIngreso" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD>
				<asp:Label id="Label72" runat="server" Font-Bold="True" ForeColor="Black">Categoria Licencia de Conduccion:</asp:Label></TD>
			<TD>
				<asp:Label id="CatLicenciaLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label15" runat="server" Font-Bold="True" ForeColor="Black">Fecha Fin de Contrato:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="FechaFinContrato" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label16" runat="server" Font-Bold="True" ForeColor="Black">Tipo de Contrato:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="TipoContrato" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label17" runat="server" Font-Bold="True" ForeColor="Black">Profesion:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="ProfesionLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label18" runat="server" Font-Bold="True" ForeColor="Black">#Libreta Profesional:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="LibretaProLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD></TD>
			<TD style="WIDTH: 116px"></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD></TD>
			<TD style="WIDTH: 116px"></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 13px">
				<asp:Label id="Label28" runat="server" Font-Bold="True" ForeColor="Black">Forma de Pago:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 13px">
				<asp:Label id="FPagoLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD style="HEIGHT: 13px"></TD>
			<TD style="HEIGHT: 13px"></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 5px">
				<asp:Label id="Label29" runat="server" Font-Bold="True" ForeColor="Black">Banco a Consignar:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 5px">
				<asp:Label id="BancoLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD style="HEIGHT: 5px"></TD>
			<TD style="HEIGHT: 5px"></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label22" runat="server" Font-Bold="True" ForeColor="Black">Sueldo Actual:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="SueldoALabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label23" runat="server" Font-Bold="True" ForeColor="Black">Fecha Sueldo Actual:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="FechaSALabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 23px">
				<asp:Label id="Label24" runat="server" Font-Bold="True" ForeColor="Black">Sueldo Anterior:</asp:Label></TD>
			<TD style="WIDTH: 116px; HEIGHT: 23px">
				<asp:Label id="SueldoAnLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD style="HEIGHT: 23px"></TD>
			<TD style="HEIGHT: 23px"></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label25" runat="server" Font-Bold="True" ForeColor="Black">Fecha Sueldo Anterior:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="FechaSAnLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label26" runat="server" Font-Bold="True" ForeColor="Black">Salario Promedio:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="SalarioPLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label27" runat="server" Font-Bold="True" ForeColor="Black">Subsidio de Trasnporte:</asp:Label></TD>
			<TD style="WIDTH: 116px">
				<asp:Label id="SubTransLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD></TD>
			<TD style="WIDTH: 116px"></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD></TD>
			<TD style="WIDTH: 116px"></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD></TD>
			<TD style="WIDTH: 116px"></TD>
			<TD></TD>
			<TD></TD>
		</TR>
	</TABLE>
</asp:Panel>
