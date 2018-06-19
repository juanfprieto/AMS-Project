<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Contabilidad.ExportatrSaldosNIT.ascx.cs" Inherits="AMS.Contabilidad.AMS_Contabilidad_ExportatrSaldosNIT" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
			<td>
            <p>
             <table id="Table1" class="filtersIn">
             <tr>
             <td>
            Año:
            <asp:DropDownList id="ddlAno" class="dpequeno" runat="server"></asp:DropDownList>
            Cuenta Inicial:
            <asp:TextBox ReadOnly="True" id="txtInicial" onclick="ModalDialog(this,'SELECT MCUE_CODIPUC AS CUENTA, MCUE_NOMBRE AS NOMBRE from DBXSCHEMA.MCUENTA WHERE TIMP_CODIGO=\'A\' ORDER BY MCUE_CODIPUC;', new Array(),1)"
					runat="server" class="tpequeno" Font-Size="XX-Small"></asp:TextBox>
                    Cuenta Final:
                    <asp:TextBox ReadOnly="True" id="txtFinal" onclick="ModalDialog(this,'SELECT MCUE_CODIPUC AS CUENTA, MCUE_NOMBRE AS NOMBRE from DBXSCHEMA.MCUENTA WHERE TIMP_CODIGO=\'A\' ORDER BY MCUE_CODIPUC;', new Array(),1)"
					runat="server" class="tpequeno" Font-Size="XX-Small"></asp:TextBox>
		<br />
			Opcion:
			<asp:RadioButtonList ID="rdbOpcion" Runat="server">
					<asp:ListItem Value="0" Selected="True">Saldo acumulado por NIT sin saldo inicial</asp:ListItem>
					<asp:ListItem Value="1">Saldo NIT por cuenta con saldo inicial</asp:ListItem>
				</asp:RadioButtonList>
			<asp:Button id="btnGenerar" runat="server" Text="Generar" onclick="btnGenerar_Click" class="noEspera"></asp:Button>
            </td>
            </tr>
            </table>
            </p>
            </td>
		</tr>
	</TBODY>
</table><br>
<table class="filters" width="780">		
		<tr>
			<td>
				<asp:datagrid id="dgrSaldos" cssclass="datagrid" runat="server" AutoGenerateColumns="True">
					<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" cssclass="item"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" cssclass="header"></HeaderStyle>
					<FooterStyle cssclass="footer"></FooterStyle>
					<Columns></Columns>
				</asp:datagrid>
			</td>
		</tr>
	</tbody>
</table>
