<%@ Register TagPrefix="web" Namespace="WebChart" Assembly="WebChart" %>
<%@ Control Language="c#" codebehind="AMS.Reportes.FormatoReporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Formato" EnableViewState="true"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script type ="text/javascript">

    function Lista() {
        w = window.open('AMS.DBManager.Reporte.aspx');
    }

    function clickOnce(btn, msg) {
        // Comprobamos si se está haciendo una validación
        if (typeof (Page_ClientValidate) == 'function') {
            // Si se está haciendo una validación, volver si ésta da resultado false
            if (Page_ClientValidate() == false) { return false; }
        }

        // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
        if (btn.getAttribute('type') == 'button') {
            // El atributo msg es totalmente opcional. 
            // Será el texto que muestre el botón mientras esté deshabilitado
            if (!msg || (msg = 'undefined')) { msg = 'Procesando..'; }

            btn.value = msg;

            // La magia verdadera :D
            btn.disabled = true;
        }

        return true;
    }

</script>
<asp:placeholder id="filterHolders" runat="server" visible="true">
	<table class="filters">
		<tbody>
			<tr>
				<th class="filterHead">
                <IMG height="70" src="../img/AMS.Flyers.News.png" border="0">
            </th>
            <td>
				<table class="filtersIn">
					<tbody>
				<tr>

				    <td>
					<p><asp:placeholder id="filterHolder" runat="server"></asp:placeholder></p>
				    </td>
                </tr>
                    </tbody>
                </table>
                </td>
			</tr>
            </tr>
		</tbody>
    </table>
</asp:placeholder>
<p><asp:placeholder id="toolsHolder" runat="server" visible="false">
		<TABLE class="tools" width="780">
			<TR>
				<th class="filterHead">
                <IMG height="30" src="../img/AMS.Flyers.News.png" border="0">
            </th>
				<td>Imprimir</td>
				<td><A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0"></A></td>
				<td>&nbsp; &nbsp;Enviar por correo</td><td><asp:TextBox id="tbEmail" runat="server"></asp:TextBox></td>
				<td><asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"
						BorderWidth="0px"></asp:ImageButton>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
					ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
				</TD>
				<TD>&nbsp; &nbsp;Exportar Excel</td>
				<td><asp:ImageButton id="ibExcel" onclick="Excel" runat="server" alt="Exportar Excel" ImageUrl="../img/AMS.Icon.Excel.png" BorderWidth="0px"></asp:ImageButton></TD>
				<TD>&nbsp; &nbsp;Graficar</td>
				<td><asp:DropDownList ID="ddlTipoGrafica" runat="server">
					<asp:ListItem Selected="True" Value="L">Linea</asp:ListItem>
					<asp:ListItem Value="B">Barra</asp:ListItem>
					<asp:ListItem Value="P">Pastel</asp:ListItem>
				</asp:DropDownList></td>
				<td><asp:ImageButton id="ibGrafica" onclick="Graph" runat="server" alt="Graficar" ImageUrl="../img/AMS.Icon.Graph.png"
						BorderWidth="0px"></asp:ImageButton></TD>
				<TD width="40"></TD>
			</tr>
		</TABLE>
	</asp:placeholder>
</p>
<asp:placeholder id="plcOpciones" runat="server" visible="false">
	<P align=center>
		<asp:TextBox id="txtAncho" runat="server" Text="800" Columns=3 MaxLength=4></asp:TextBox>X<asp:TextBox id="txtAlto" runat="server" Text="600" Columns=3 MaxLength=4></asp:TextBox>&nbsp;<asp:Button ID="btnEscala" runat="server" Text="Escalar" OnClick="Graph"/><br><br>
		<Web:ChartControl id="chtGrafica" runat="server" BorderWidth="5px" BorderStyle="Outset" Width="8000px" Height="400px" Visible=false>
			<XTitle StringFormat="Center,Near,Character,LineLimit"></XTitle>
			<YAxisFont StringFormat="Far,Near,Character,LineLimit"></YAxisFont>
			<ChartTitle StringFormat="Center,Near,Character,LineLimit"></ChartTitle>
			<XAxisFont StringFormat="Center,Near,Character,LineLimit"></XAxisFont>
			<Background Color="LightSteelBlue"></Background>
			<YTitle StringFormat="Center,Near,Character,LineLimit"></YTitle>
		</Web:ChartControl>
		<br>
		<asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="Volver"/>
	</P>
	
</asp:placeholder>
<asp:placeholder id="plcReport" runat="server" visible="false">
	<p>
		<table class="reports" width="780" bgColor="gray">
			<tbody>
				<tr>
					<td><asp:table id="tabHeader" BorderWidth="0px" Width="100%" CellSpacing="0" CellPadding="1" BackColor="White"
							GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"></asp:table></td>
				</tr>
				<tr>
					<td><asp:datagrid id="report" runat="server" BorderWidth="2px" CellSpacing="1" CellPadding="3" BackColor="White"
							GridLines="None" BorderColor="White" BorderStyle="Ridge" width="100%" EnableViewState="True"
							AutoGenerateColumns="False">
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#9471DE"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F0F0F0"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#DEDFDE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#E7E7FF" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="Black" BackColor="#C6C3C6"></FooterStyle>
							<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6"></PagerStyle>
						</asp:datagrid></td>
					<td><asp:datagrid id="reportA" runat="server" BorderWidth="2px" CellSpacing="1" CellPadding="3" BackColor="White"
							GridLines="None" BorderColor="White" BorderStyle="Ridge" width="100%" EnableViewState="False"
							AutoGenerateColumns="False">
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#9471DE"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F0F0F0"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#DEDFDE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#E7E7FF" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="Black" BackColor="#C6C3C6"></FooterStyle>
							<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6"></PagerStyle>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td><asp:table id="tabFooter" BorderWidth="0px" Width="100%" CellSpacing="0" CellPadding="1" BackColor="White"
							GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"></asp:table></td>
				</tr>
			</tbody>
		</table>
	</p>
</asp:placeholder>
<P><asp:label id="lbInfo" runat="server"></asp:label></P>

