<%@ Register TagPrefix="uc1" TagName="Seleccionar" Src="AMS.Tools.Seleccionar.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Contabilidad.ProceHecEco.ascx.cs" Inherits="AMS.Contabilidad.ProceHecEco" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<fieldset>
<TABLE id="Table" class="filtersIn">
	<TBODY>
		<tr>
			<td>Año :
			<br /><asp:dropdownlist id="año" class="dpequeno" runat="server"></asp:dropdownlist></td>
            
			<td>Mes :
			<br /><asp:dropdownlist id="Mes" class="dpequeno" runat="server"></asp:dropdownlist></td>
            </tr>
            <tr>
			<td>Dia Inicial :
			<br /><asp:dropdownlist id="DiaInicial" class="dpequeno" runat="server"></asp:dropdownlist></td>
            
			<td>Dia Final :
			<br /><asp:dropdownlist id="DiaFinal" class="dpequeno" runat="server"></asp:dropdownlist></td>
            </tr>
            <tr>
			<td>Sede (Almacén) :
			<br /><asp:dropdownlist id="sede" class="dpequeno" runat="server"></asp:dropdownlist>
				<p></p>
				<asp:checkbox id="Todas" runat="server" Text="Todas las Sedes"></asp:checkbox>
				<p></p>
			</td>
		</tr>
	</TBODY>
</TABLE>
<P></P>
<uc1:seleccionar id="Seleccion" runat="server"></uc1:seleccionar><uc1:seleccionar id="Seleccion1" runat="server"></uc1:seleccionar>
<P align="justify"><asp:placeholder id="toolsHolder" runat="server">
		<TABLE id="Table" class="filtersIn">
			<TR>
				<th class="filterHead"">
                <IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0">
                </th>
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
<P>RESUMEN DEL PROCESO:</P>
<P><asp:label id="lbInfoCab" runat="server" ForeColor="Red" Font-Bold="True"></asp:label></P>
<P><asp:placeholder id="phGrilla" runat="server">
		<asp:datagrid id="dgMovs" runat="server" Width="800" AutoGenerateColumns="True"></asp:datagrid>
	</asp:placeholder></P>
<asp:button id="CmdContabilizar" runat="server" Text="Contabilizar" onclick="CmdContabilizar_Click" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Contabilizando...')"></asp:button><asp:button id="btnCancelar" Text="Cancelar o Reiniciar Proceso" Width="210px" CausesValidation="False"
	Runat="server" onclick="btnCancelar_Click"></asp:button><asp:linkbutton id="lnkExportarExcel" runat="server" onclick="lnkExportarExcel_Click">Exportar Excel</asp:linkbutton><INPUT id="hdSvrErrores" type="hidden" runat="server">
<asp:CustomValidator id="cvComprobantes" runat="server" Display="None" ClientValidationFunction="Validate_Comps"></asp:CustomValidator>
<P><asp:label id="lbInfo" runat="server"></asp:label></P>
<input id="hdnCont" runat="server" type="hidden">
</fieldset>

<script language:javascript>
 function clickOnce(btn, msg)
        {
            // Comprobamos si se está haciendo una validación
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Procesando...'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }	
</script>		                        
