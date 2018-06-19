<%@ Control Language="c#" autoeventwireup="True" codebehind="AMS.Reportes.ReportsUI.ascx.cs" Inherits="AMS.Reportes.CrystalReportUI" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="Email" Src="AMS.Tools.Email.ascx" %>

<asp:ScriptManager ID="ScriptManager" runat="server" />
<script type ="text/javascript" src="../js/AMS.Web.masks.js"></script>
<script type ="text/javascript" src="../js/generales.js"></script>
<script>
    $(function () {
        //mirar documentacion JQueryUI sobre datepicker
        if ($(".calendario").val() == "") {
            $(".calendario").datepicker();
            $(".calendario").datepicker("option", "dateFormat", "yy-mm-dd");
            $(".calendario").datepicker("option", "showAnim", "slideDown");
        }
    });

</script>


<fieldset>
    <legend>Filtros Asociados</legend>
    <asp:placeholder id="phForm1"  runat="server" visible="true" ></asp:placeholder>

    <asp:placeholder id="plcParamAUX" runat="server" Visible = "false">
        <FIELDSET>
            <LEGEND>Parámetros asociados al Reporte</LEGEND>
            <asp:placeholder id="phForm2" runat="server"></asp:placeholder>
        </FIELDSET>
    </asp:placeholder>
</fieldset>
<br>
<fieldset>
	<legend>Reporte</legend>
	<table class="filtersIn">
		<tr>
			<td colspan="2">
				<fieldset  class="infield" >
					<legend>Impresión</legend>
					<div class="">
						Tipo de Reporte:<br>
						<div class="tabular">
                            <asp:RadioButtonList ID="rdbImpresion" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="pdf" Selected="True"><img src="../img/pdf_icon.png" width="15">Adobe Acrobat</asp:ListItem>
                                <asp:ListItem Value="doc"><img src="../img/doc_icon.png" width="18">MS Word</asp:ListItem>
                                <asp:ListItem Value="xls"><img src="../img/xls_icon.png" width="17">Ms Excel</asp:ListItem>
                                <asp:ListItem Value="rtf"><img src="../img/rtf_icon.png" width="17">Texto Enriquecido</asp:ListItem>
                                <asp:ListItem Value="lpt"><img src="../img/lpt_icon.png" width="18">Lista en Impresora</asp:ListItem>
                            </asp:RadioButtonList>
						</div>
					</div>
                    <br />
					<div class="">
						Impresoras:
						<asp:dropdownlist id="Impresoras" class="dmediano" runat="server"></asp:dropdownlist>
					</div>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td>
				<fieldset class="infield">
					<legend>Datos del reporte</legend>
					Titulo del Reporte:
					<asp:textbox id="Titulo" runat="server" ></asp:textbox>
					Subtitulo del Reporte:
					<asp:textbox id="Subtitulo" runat="server"></asp:textbox>
					Codigo del Reporte:
					<asp:textbox id="CodigoRep" runat="server" ></asp:textbox>
				</fieldset>
			</td>
			<td>			
				<fieldset class="infield">
					<legend>Datos de la empresa</legend>
					Nombre de La Empresa:
					<asp:textbox id="Empresa" runat="server"></asp:textbox>
					Nit de la Empresa:
					<asp:textbox id="Nit" runat="server" ></asp:textbox>
				</fieldset>
			</td>
		</tr>
	</table>
</fieldset>
<br>
<fieldset>
	<table class="filtersInAuto">
		<tr>
			<td width="35%">
				<asp:button id="btnImprimir" runat="server" CausesValidation="False" Text="Imprimir" onclick="btnImprimir_Click" UseSubmitBehavior="false" 
                onClientClick="espera();" class="noEspera" ></asp:button>
				<br><br>
				<asp:hyperlink id="Ver" runat="server" Visible="False" Target="_blank">Click aquí para ver el Reporte...</asp:hyperlink>
			</td>
			<td>
                 <uc1:Email runat="server" id="opcEnviarMail"></uc1:Email>
			</td>
		</tr>
	</table>
</fieldset>
<p><asp:label id="lb" runat="server"></asp:label></p>
