<%@ Control Language="c#" codebehind="AMS.Contabilidad.MovCom.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.MovCom" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
	<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
    function mostratDialogo() {
        var open = document.getElementById('<%=typeDoc.ClientID%>');
        ModalDialog(open, "SELECT pdoc_codigo as PREFIJO, pdoc_nombre  CONCAT ' - ' CONCAT pdoc_codigo AS NOMBRE FROM pdocumento order by pdoc_nombre", new Array());
    }
   
    

</script>
	<br>
	<p>
		<table class="filters">
			<tbody>
				<tr>
					<th class="filterHead">
				<img height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
					<td valign="middle">
						<p>
							Año:<asp:DropDownList id="year" runat="server" AutoPostBack="True" onselectedindexchanged="year_SelectedIndexChanged" class="dpequeno"></asp:DropDownList>
							&nbsp;&nbsp;&nbsp; Mes:<asp:DropDownList id="month" runat="server" AutoPostBack="True" onselectedindexchanged="month_SelectedIndexChanged"  class="dpequeno"></asp:DropDownList>
							<br>
							&nbsp;<asp:RadioButtonList id="RadioOpcion" runat="server" AutoPostBack="True" Onselectedindexchanged="opcion_cambio"> 
								<asp:ListItem Value="TC"  Selected = "false">Todos los comprobantes</asp:ListItem>
								<asp:ListItem Value="UTC" Selected = "false">Un tipo de comprobante</asp:ListItem>
                                
                          
								<asp:ListItem Value="UCE" Selected = "true" >Un comprobante específico</asp:ListItem>
							</asp:RadioButtonList>
							<br>
							&nbsp; &nbsp;<asp:Label id="lblTipoComp"  AutoPostBack="True" runat="server">Tipo de Comprobante: </asp:Label>
							<asp:DropDownList id="typeDoc" class="dmediano" runat="server" AutoPostBack="True" onselectedindexchanged="typeDoc_SelectedIndexChanged"></asp:DropDownList>
                            <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="mostratDialogo()"></asp:Image>
							<asp:Label id="lblNumComp" runat="server"><br />Numero de Comprobante:</asp:Label>&nbsp;
							<asp:DropDownList id="ddlNumComp" class="dmediano" runat="server"></asp:DropDownList>
						</p>
						<p>
							<asp:Button id="Consulta" onclick="Consulta_Click" runat="server" Text="Generar" ></asp:Button>
						</p>
					</td>
                    
				</tr> 
			</tbody>
		</table>
	</p>
	<p>
		<asp:PlaceHolder id="toolsHolder" runat="server" visible="false">
			<TABLE class="tools">
				<TR>
					<th class="filterHead">
				<img height="30" src="../img/AMS.Flyers.Tools.png" border="0">
			</th>
					<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
						</A>
					</TD>
					<TD>&nbsp; &nbsp;Enviar por correo
						<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
					<TD>
						<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 188px; POSITION: absolute; TOP: 271px" runat="server"
							ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
						<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail.jpg"
							alt="Enviar por email" BorderWidth="0px"></asp:ImageButton></TD>					
                      <td style="padding-left: 19px;text-align: center;">
                                <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel" OnClick="ImprimirExcelGrid" runat="server"
                                    alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" cssClass="Excelimg" style="width: 50%; margin-bottom: -6px;">
                                </asp:ImageButton>
                                <br />
                                <font size="1">Descargar Excel</font>
                            </td>
                    <asp:Label ID="lblResult" class="bg-success" runat="server"></asp:Label></p>
				</TR>
			</TABLE>
		</asp:PlaceHolder>
	</p>
	<p>
	</p>
	<br>
	<table class="reports" align="center" bgcolor="gray">
		<tbody>
			<tr>
				<td>
					<asp:Table id="tabPreHeader" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
						Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"
						EnableViewState="False" Width="100%"></asp:Table>
				</td>
			</tr>
			<tr>
				<td align="center">
					<p>
						<asp:DataGrid id="dg" runat="server" cssclass="datagrid" AutoGenerateColumns="False" OnItemDataBound="Report_ItemDataBound">
							<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						</asp:DataGrid>
					</p>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Table id="tabFirmas" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana" Font-Size="8pt"
						Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0" EnableViewState="False"
						Width="100%"></asp:Table>
				</td>
			</tr>
		</tbody>
	</table>
	<asp:Label id="hola" runat="server"></asp:Label>
