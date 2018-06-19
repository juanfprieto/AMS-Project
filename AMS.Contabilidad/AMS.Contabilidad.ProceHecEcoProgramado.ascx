<%@ Register TagPrefix="uc1" TagName="Seleccionar" Src="AMS.Tools.Seleccionar.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Contabilidad.ProceHecEcoProgramado.ascx.cs" Inherits="AMS.Contabilidad.ProceHecEcoProgramado" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript">
	function Lista()
	{
		w=window.open('AMS.DBManager.Reporte.aspx');
	}
	
	function Validate_Comps(sender, args)
	{
		var controlValidateComps = document.getElementById("<%=hdSvrErrores.ClientID%>");
		if(controlValidateComps.value != '')
		{
			args.IsValid = false;
			alert(controlValidateComps.value);
			return;
		}
		else
			args.IsValid = true;
	}
</script>
<TABLE style="WIDTH: 868px; HEIGHT: 125px">
	<TBODY>
		<tr>
			<td>Año :
			</td>
			<td><asp:dropdownlist id="año" runat="server"></asp:dropdownlist></td>
			<td>Mes :
			</td>
			<td><asp:dropdownlist id="Mes" runat="server"></asp:dropdownlist></td>
			<td>Dia Inicial :
			</td>
			<td><asp:dropdownlist id="DiaInicial" runat="server"></asp:dropdownlist></td>
			<td>Dia Final :
			</td>
			<td><asp:dropdownlist id="DiaFinal" runat="server"></asp:dropdownlist></td>
			<td>Sede (Almacen) :
			</td>
			<td><asp:dropdownlist id="sede" runat="server"></asp:dropdownlist>
				<p></p>
				<asp:checkbox id="Todas" runat="server" Text="Todas las Sedes"></asp:checkbox>
				<p></p>
			</td>
		</tr>
	</TBODY>
</TABLE>
<P></P>
<uc1:seleccionar id="Seleccion" runat="server"></uc1:seleccionar><uc1:seleccionar id="Seleccion1" runat="server"></uc1:seleccionar>
<P align="justify">
    Programar :</P>
<P align="justify">
    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
</P>
<P align="justify">Hora:&nbsp;&nbsp;
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
</P>
<P align="justify"><asp:placeholder id="toolsHolder" runat="server">
		<TABLE class="tools" width="780">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" alt="Enviar por email"
						ImageUrl="../img/AMS.Icon.Mail.jpg"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:placeholder></P>
<P>RESUMEN DEL PROCESO:<asp:Timer ID="Timer1" runat="server">
    </asp:Timer>
</P>
<P><asp:label id="lbInfoCab" runat="server" ForeColor="Red" Font-Bold="True"></asp:label></P>
<P><asp:placeholder id="phGrilla" runat="server">
		<asp:datagrid id="dgMovs" runat="server" Width="800" AutoGenerateColumns="True"></asp:datagrid>
	</asp:placeholder></P>
<asp:button id="CmdContabilizar" runat="server" Text="Contabilizar" 
    onclick="CmdContabilizar_Click" Visible="False"></asp:button>
<asp:button id="btnCancelar" Text="Cancelar o Reiniciar Proceso" Width="184px" CausesValidation="False"
	Runat="server" onclick="btnCancelar_Click" Visible="False"></asp:button>
<asp:linkbutton id="lnkExportarExcel" runat="server" 
    onclick="lnkExportarExcel_Click" Visible="False">Exportar Excel</asp:linkbutton><INPUT id="hdSvrErrores" type="hidden" runat="server">
<asp:CustomValidator id="cvComprobantes" runat="server" Display="None" ClientValidationFunction="Validate_Comps"></asp:CustomValidator>
<P><asp:label id="lbInfo" runat="server"></asp:label></P>
<input id="hdnCont" runat="server" type="hidden">
