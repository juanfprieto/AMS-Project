<%@ Page Language="c#" codebehind="AMS.Finanzas.Tesoreria.ImpresionRC.aspx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ImpresionRC" %>
<HTML>
	<HEAD>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body onload="window.print();">
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
		<table style="WIDTH: 650px; BACKGROUND-COLOR: white">
			<tbody>
				<tr>
					<td align="center" colspan="4"><asp:label id="lbEmpresa" runat="server"></asp:label></td>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
					<td align="center" colspan="4"><asp:label id="lbNit" runat="server"></asp:label></td>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
					<td align="center" colspan="4"><asp:label id="lbDireccion" runat="server"></asp:label></td>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
					<td align="right"><asp:label id="lbRecibo" runat="server"></asp:label>
					&nbsp;
					<td align="left">&nbsp;<asp:label id="lbNumero" runat="server"></asp:label></td>
					<td align="center"><asp:label id="lbTipo" runat="server"></asp:label></td>
					<td align="center"><asp:label id="lbFecha" runat="server"></asp:label></td>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
					<td align="center" colspan="2"><asp:label id="lbBeneficiario" runat="server"></asp:label></td>
					<td align="center" colspan="2"><asp:label id="lbNitBen" runat="server"></asp:label></td>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
					<td align="center" colspan="2"><asp:label id="lbConcepto" runat="server"></asp:label></td>
					<td align="center" colspan="2"><asp:label id="lbSede" runat="server"></asp:label></td>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
					<td colspan="4" bgcolor="#000000"></td>
				</tr>
				<tr>
					<td align="center" colspan="4">
						<p><asp:label id="lbDocs" runat="server"></asp:label></p>
						<center>
							<p><asp:datagrid id="dgDocs" runat="server" Font-Names="Verdana" CellPadding="3" Font-Name="Verdana"
									Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" AutoGenerateColumns="False" Width="650">
									<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
									<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
									<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
									<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
									<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
									<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
									<Columns>
										<asp:BoundColumn DataField="PREFIJO" HeaderText="Prefijo Factura"></asp:BoundColumn>
										<asp:BoundColumn DataField="NUMERODOCUMENTO" HeaderText="N&#250;mero Factura"></asp:BoundColumn>
										<asp:BoundColumn DataField="VALORABONAR" HeaderText="Valor Abonado" DataFormatString="{0:C}"></asp:BoundColumn>
									</Columns>
								</asp:datagrid></p>
						</center>
						<p><asp:label id="lbNoCaus" runat="server"></asp:label></p>
						<center>
							<p><asp:datagrid id="dgNoCaus" runat="server" Font-Names="Verdana" CellPadding="3" Font-Name="Verdana"
									Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" AutoGenerateColumns="False" Width="650">
									<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
									<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
									<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
									<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
									<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
									<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
									<Columns>
										<asp:BoundColumn DataField="DESCRIPCION" HeaderText="Descripci&#243;n"></asp:BoundColumn>
										<asp:BoundColumn DataField="CUENTA" HeaderText="Cuenta"></asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="SEDE" HeaderText="Sede"></asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="CENTROCOSTO" HeaderText="Centro de Costo"></asp:BoundColumn>
										<asp:BoundColumn DataField="PREFIJO" HeaderText="Prefijo Documento"></asp:BoundColumn>
										<asp:BoundColumn DataField="NUMERO" HeaderText="Numero Documento"></asp:BoundColumn>
										<asp:BoundColumn DataField="NIT" HeaderText="Nit Beneficiario"></asp:BoundColumn>
										<asp:BoundColumn DataField="VALORDEBITO" HeaderText="Valor Debito" DataFormatString="{0:C}"></asp:BoundColumn>
										<asp:BoundColumn DataField="VALORCREDITO" HeaderText="Valor Credito" DataFormatString="{0:C}"></asp:BoundColumn>
										<asp:BoundColumn DataField="VALORBASE" HeaderText="Valor Base" DataFormatString="{0:C}"></asp:BoundColumn>
									</Columns>
								</asp:datagrid></p>
						</center>
						<p><asp:label id="lbVarios" runat="server"></asp:label></p>
						<center><asp:datagrid id="dgAbonos" runat="server" Font-Names="Verdana" CellPadding="3" Font-Name="Verdana"
								Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" AutoGenerateColumns="False" Width="650">
								<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
								<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
								<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
								<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
								<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
								<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
								<Columns>
									<asp:BoundColumn DataField="PREFIJO" ReadOnly="True" HeaderText="Prefijo del Pedido"></asp:BoundColumn>
									<asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="N&#250;mero del Pedido"></asp:BoundColumn>
									<asp:BoundColumn DataField="VALORPEDIDO" ReadOnly="True" HeaderText="Valor Total Pedido" DataFormatString="{0:C}"></asp:BoundColumn>
									<asp:BoundColumn DataField="ABONO" HeaderText="Valor a Abonar" DataFormatString="{0:C}"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></center>
					</td>
				</tr>
				<tr>
					<td colspan="4" bgcolor="#000000"></td>
				</tr>
				<tr>
					<td align="center" colspan="4">
						<p><asp:label id="lbPagos" runat="server"></asp:label></p>
						<p><asp:datagrid id="dgPagos" runat="server" Font-Names="Verdana" CellPadding="3" Font-Name="Verdana"
								Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" AutoGenerateColumns="False" Width="650">
								<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
								<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
								<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
								<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
								<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
								<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
								<Columns>
									<asp:BoundColumn DataField="TIPO" HeaderText="Tipo de Pago"></asp:BoundColumn>
									<asp:BoundColumn DataField="CODIGOBANCO" HeaderText="Banco"></asp:BoundColumn>
									<asp:BoundColumn DataField="NUMERODOC" HeaderText="N&#250;mero del Documento"></asp:BoundColumn>
									<asp:BoundColumn DataField="TIPOMONEDA" HeaderText="Tipo de Moneda"></asp:BoundColumn>
									<asp:BoundColumn DataField="VALOR" HeaderText="Valor" DataFormatString="{0:C}"></asp:BoundColumn>
									<asp:BoundColumn DataField="VALORTC" HeaderText="Tasa Cambio" DataFormatString="{0:C}"></asp:BoundColumn>
									<asp:BoundColumn DataField="VALORBASE" HeaderText="Valor Base" DataFormatString="{0:C}"></asp:BoundColumn>
									<asp:BoundColumn DataField="FECHA" HeaderText="Fecha"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></p>
						<p><asp:label id="lbRets" runat="server"></asp:label></p>
						<p align="center"><asp:datagrid id="dgRtns" runat="server" Font-Names="Verdana" CellPadding="3" Font-Name="Verdana"
								Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" AutoGenerateColumns="False" Visible="true" Width="650">
								<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
								<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
								<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
								<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
								<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
								<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
								<Columns>
									<asp:BoundColumn DataField="CODRET" HeaderText="C&#243;digo de Retenci&#243;n"></asp:BoundColumn>
									<asp:BoundColumn DataField="VALOR" HeaderText="Valor" DataFormatString="{0:C}"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td colspan="4" bgcolor="#000000"></td>
				</tr>
				<tr>
					<td align="center">
						Elaborado por :&nbsp;&nbsp; 
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
					<td align="center">Revisado por :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
					</td>
					<td align="center">Aprobado por :&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; 
						&nbsp; &nbsp; &nbsp;</td>
					<td align="center">Firma y Sello de Recibido&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						&nbsp;&nbsp;&nbsp;
					</td>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
				<tr>
					<td></td>
					<td></td>
					<td></td>
					<td>CC o N.I.T</td>
				</tr>
			</tbody>
		</table>
	</body>
</HTML>
