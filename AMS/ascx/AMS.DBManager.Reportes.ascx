<%@ Control Language="c#" autoeventwireup="True" codebehind="AMS.DBManager.Reportes.ascx.cs" Inherits="AMS.DBManager.Reportes" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Web.masks.js"></script>

<p>
<FIELDSET>
    <LEGEND >Filtros Asociados a este Reporte</LEGEND>
    <asp:placeholder id="phForm1"  runat="server"></asp:placeholder>
</FIELDSET>
</p>

<P>
<asp:placeholder id="plcParamAUX" runat="server" Visible = "true">
<FIELDSET>
    <LEGEND>Parámetros asociados al Reporte</LEGEND>
    <asp:placeholder id="phForm2" runat="server"></asp:placeholder>
</FIELDSET>
</asp:placeholder>
</P>

<P>
<FIELDSET>
	<TABLE class="filtersIn" border="1">
		<TR>
			<TD>Tipo de Reporte</TD>
			<TD><asp:dropdownlist id="TipoReporte" runat="server">
					<asp:ListItem Value="pdf">Adobe Acrobat</asp:ListItem>
					<asp:ListItem Value="doc">MS Word</asp:ListItem>
					<asp:ListItem Value="xls">MS Excel</asp:ListItem>
					<asp:ListItem Value="rtf">Texto Enriquecido</asp:ListItem>
					<asp:ListItem Value="lpt">Listar en Impresora</asp:ListItem>
				</asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD>Impresoras</TD>
			<TD><asp:dropdownlist id="Impresoras" runat="server"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD>Titulo del Reporte</TD>
			<TD>
				<asp:textbox id="Titulo" runat="server" ></asp:textbox>
			</TD>
		</TR>
		<TR>
			<TD>Subtitulo del Reporte</TD>
			<TD><asp:textbox id="Subtitulo" runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Código del Reporte
			</TD>
			<TD>
				<asp:textbox id="CodigoRep" runat="server" ></asp:textbox>
			</TD>
		</TR>
		<TR>
			<TD>Nombre de la Empresa
			</TD>
			<TD>
				<asp:textbox id="Empresa" runat="server"></asp:textbox>
			</TD>
		</TR>
		<TR>
			<TD>Nit de la Empresa</TD>
			<TD><asp:textbox id="Nit" runat="server" ></asp:textbox></TD>
		</TR>
	</TABLE>
</FIELDSET>
</P>

<P>
<FIELDSET>
	<TABLE class="filtersIn" border="1">
		<TR>
			<TD>
				<asp:button id="btnImprimir" runat="server" CausesValidation="False" Text="Imprimir" onclick="btnImprimir_Click" UseSubmitBehavior="false" ></asp:button></TD>
			<TD>
				<asp:hyperlink id="Ver" runat="server" Visible="False" Target="_blank">De Click Aqui para ver el Reporte</asp:hyperlink>
			</TD>
		</TR>
		<tr>
			<td colspan="2" align="center">
				<P>
					<asp:label id="lbInfo" Text="Si desea enviar este reporte por correo, por favor digite la dirección.<br> Si desea enviarlo a varias direcciones, por favor separelas por comas(,) :"
						Visible="False" Runat="server"></asp:label></P>
				<P>
					<asp:textbox id="tbMail" runat="server" Width="274px" Visible="False"></asp:textbox><asp:requiredfieldvalidator id="rfv1" runat="server" Visible="False" ControlToValidate="tbMail" Display="Dynamic"
						ErrorMessage="Campo Obligatorio"></asp:requiredfieldvalidator><asp:imagebutton id="imgBtnMail" runat="server" Visible="False" ImageUrl="../img/AMS.Icon.Mail.jpg"
						ToolTip="Enviar Correo" onclick="ImageButton1_Click"></asp:imagebutton></P>
			</td>
		</tr>
	</TABLE>
</FIELDSET>
</P>
<p><asp:label id="lb" runat="server"></asp:label></p>



