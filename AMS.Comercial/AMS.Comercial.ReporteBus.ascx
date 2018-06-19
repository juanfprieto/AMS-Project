<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ReporteBus.ascx.cs" Inherits="AMS.Comercial.AMS_ReporteBus" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<LINK href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<br>
<p>
	<table class="filters" id="Table2" width="780">
		<tbody>
			<tr>
				<td vAlign="middle" borderColor="gray" width="16" colSpan="0"><IMG height="60" src="../img/AMS.Flyers.Filters.png" border="0">
				</td>
				<td vAlign="middle">
					<p>&nbsp;</p>
					<p>
						<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="392" border="1" style="WIDTH: 392px; HEIGHT: 61px">
							<TR>
								<TD style="WIDTH: 98px"><asp:label id="PlacaLabel" runat="server">Placa</asp:label>&nbsp;:</TD>
								<TD><asp:dropdownlist id="placa" runat="server" Onchange="cargarRuta(this);" Width="136px"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 98px">Fecha :</TD>
								<TD>
									<asp:TextBox id="Fecha" onKeyup="DateMask(this);" runat="server" ReadOnly="True"></asp:TextBox><STRONG>(yyyy-mm-dd)</STRONG></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 98px">En Ruta :</TD>
								<TD>
									De:&nbsp;
									<asp:TextBox id="TextBox1" runat="server" Width="104px" ReadOnly="True"></asp:TextBox>&nbsp; 
									A&nbsp;&nbsp;
									<asp:TextBox id="TextBox2" runat="server" Width="108px" ReadOnly="True"></asp:TextBox></TD>
							</TR>
							<TR>
							</TR>
						</TABLE>
					</p>
					<P>&nbsp;</P>
					<P><asp:button id="Button1" onclick="generar" runat="server" Text="Generar"></asp:button>
					</P>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p></p>
<p><asp:placeholder id="toolsHolder" runat="server" visible="false">&lt; 
<TABLE class=tools width=780>
  <TR>
    <TD width=16><IMG height=30 src="../img/AMS.Flyers.Tools.png" border=0></TD>
    <TD>Imprimir <A href="javascript: Lista()"><IMG height=18 alt=Imprimir 
      src="../img/AMS.Icon.Printer.png" width=20 border=0> </A></TD>
    <TD>&nbsp; &nbsp;Enviar por correo 
<asp:TextBox id=tbEmail runat="server"></asp:TextBox></TD>
    <TD>
<asp:RegularExpressionValidator id=FromValidator2 style="LEFT: 188px; POSITION: absolute; TOP: 271px" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
<asp:ImageButton id=ibMail onclick=SendMail runat="server" BorderWidth="0px" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"></asp:ImageButton></TD>
    <TD width=380></TD></TR></TABLE></asp:placeholder></p>
<p></p>
<br>
<table class="reports" id="Table3" width="780" align="center" bgColor="gray">
	<tbody>
		<TR>
			<td><asp:Table id="tabPreHeader" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
					Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"
					Width="100%"></asp:Table></td>
		</TR>
		<tr>
			<td align="center">
				<p><asp:datagrid id="Grid" runat="server" Width="780px" HorizontalAlign="Center" AutoGenerateColumns="False">
						<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="NUMERO DEL BUS" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO BUS") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PLACA DEL BUS" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CIUDAD ORIGEN" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "ORIGEN") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CIUDAD DESTINO" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DESTINO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="HORA SALIDA" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "HORA SALIDA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FECHA" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CODIGO RUTA" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODIGO RUTA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO PLANILLA" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO PLANILLA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO DE PASAJEROS" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO DE PASAJEROS") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="TOTAL" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TOTAL") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></p>
			</td>
		</tr>
		<tr>
			<td><asp:table id="tabFirmas" BorderWidth="0px" EnableViewState="False" CellSpacing="0" CellPadding="1"
					BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"></asp:table></td>
		</tr>
	</tbody>
</table>
<BLOCKQUOTE dir="ltr" style="MARGIN-RIGHT: 0px">
	<P><asp:label id="lb" runat="server"></asp:label></P>
</BLOCKQUOTE>
<script language:javascript>



function cargarRuta(Obj)
{
	
	var ddlplaca= document.getElementById("<%=placa.ClientID%>");	
	AMS_ReporteBus.cargarRuta_Externas(Obj.value,cargarRuta_Externas_Callback);
}



function cargarRuta_Externas_Callback(response)
{
	var textbox=document.getElementById("<%=TextBox1.ClientID%>");
	var textbox1=document.getElementById("<%=TextBox2.ClientID%>");
	var Fecha=document.getElementById("<%=Fecha.ClientID%>");
	var respuesta=response.value;
		if(respuesta.Tables[0].Rows.length>0)
		{
			var someD = new Date();
			someD = respuesta.Tables[0].Rows[0].FECHA;
			
			var un = someD.getMonth()+1;
			if (un<9)
			{
			un= '0'+un;
			}
			
			un = un.toString();
			
			
			var dosF = someD.getDate();
			if (dosF<9)
			{
			dosF= '0'+dosF;
			}
			
			dosF = dosF.toString();
			
			
			var tresF = someD.getFullYear();
			tresF=tresF.toString();
			
			Fecha.value=tresF+'-'+un+'-'+dosF;
			textbox.value=respuesta.Tables[0].Rows[0].ORIGEN;
			textbox1.value=respuesta.Tables[0].Rows[0].DESTINO;
			
		}
		else
		{
			alert('No hay RUTAS disponibles');
			return;
		}	
		
}



</script>

