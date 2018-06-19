<%@ Control Language="c#" codebehind="AMS.Automotriz.PreliquidacionOT.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.PreliquidacionOT" %>
<script type ='text/javascript'>
    function cargarDescMob(ob)
    {
            var valor = document.getElementById("<%=txtValMob.ClientID%>").value;
            var porDescuento = document.getElementById("<%=txtDes.ClientID%>").value;
            var ValorDesc = "";

            if (valor != 0)
            {
                ValorDesc = valor * porDescuento / 100;
                document.getElementById("<%=txtValDesc.ClientID%>").value = ValorDesc;
            }
            else
            {
                alert('No se puede aplicar descuento a valores en 0 Revice Por Favor!..')
                return;
            }
    }

    function cargarDescRep(ob)
      {
            var valor = document.getElementById("<%=txtValRep.ClientID%>").value;
            var porDescuento = document.getElementById("<%=txtDesRep.ClientID%>").value;
            var ValorDesc = "";

            if (valor != 0)
            {
                ValorDesc = valor * porDescuento / 100;
                document.getElementById("<%=txtValDescRep.ClientID%>").value = ValorDesc;
            }
            else
            {
                alert('No se puede aplicar descuento a valores en 0 Revice Por Favor!..')
                return;
            }
      }

</script>
<fieldset>
	<legend class="Legends">Orden de Trabajo</legend>
	<table class="filtersIn" cellspacing="5">
		<tbody>
			<tr>
				<td>
					Prefijo Orden de Trabajo :&nbsp;<asp:Label id="lbPrefOT" runat="server"></asp:Label>&nbsp;</td>
				<td>
					Número de Orden de Trabajo :&nbsp;<asp:Label id="lbNumOT" runat="server"></asp:Label>
				</td>
				<td align="right">
					<asp:Button id="Button1" onclick="PreliquidarOT" runat="server" Text="Preliquidar" CssClass="noEspera"></asp:Button>
				</td>
			</tr>
			<tr>
				<td>pr
					Nit Cliente&nbsp;:
					<asp:Label id="lbNitCliente" runat="server"></asp:Label></td>
				<td colspan="2">
					Nombre Cliente :
					<asp:Label id="lbNomCliente" runat="server"></asp:Label></td>
			</tr>
			<tr>
			    <td></td>
			    <td colspan="2">
			        Nombre Contacto :
					<asp:Label id="lbNomContacto" runat="server"></asp:Label>
			    </td>
			</tr>
			<tr>
				<td colspan="3">
					Cargo Principal Orden de Trabajo :
					<asp:Label id="lbCrgPrnOT" runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td colspan="2">
					Otros Cargos Relacionados Orden de Trabajo :
					<asp:DropDownList id="ddlCrgsRels" runat="server"></asp:DropDownList>
				</td>
				<td align="right">
					<asp:Button id="btnInfoCrgs" onclick="MostrarInfoOTCrg" runat="server" Text="Mostrar Información"></asp:Button>
				</td>
			</tr>
            <tr>
				<td>Valor Mano De Obra
					<asp:Textbox id="txtValMob" Enabled="false" runat="server"></asp:Textbox></td>
				<td colspan="2">
					% De Descuento :
					<asp:Textbox id="txtDes" onblur="cargarDescMob(this);" runat="server"></asp:Textbox></td>
                <td colspan="2">
					Valor Descuento :
					<asp:Textbox id="txtValDesc" runat="server"></asp:Textbox></td>
			</tr>
             <tr>
				<td>Valor Repuestos
					<asp:Textbox id="txtValRep" Enabled ="false" runat="server"></asp:Textbox></td>
				<td colspan="2">
					% De Descuento :
					<asp:Textbox id="txtDesRep" onblur="cargarDescRep(this);" runat="server"></asp:Textbox></td>
                <td colspan="2">
					Valor Descuento :
					<asp:Textbox id="txtValDescRep" runat="server"></asp:Textbox></td>
			</tr>
			<tbody></tbody>
		</tbody>
	</table>
</fieldset>
<p>
</p>
<p>
	<asp:PlaceHolder id="plInfoCrg" runat="server">
</p>
<FIELDSET>
	<LEGEND class="Legends">Información Elementos OT</LEGEND>
	<TABLE class="filtersIn">
		<TR>
			<TD align="center">
				<asp:Label id="lbInfoOper" runat="server" forecolor="RoyalBlue">Información Operaciones</asp:Label></TD>
			<TD>
				<asp:DataGrid id="dgInfoOper" runat="server" CssClass="main" BorderWidth="1px" HeaderStyle-BackColor="#ccccdd"
					Font-Size="8pt" Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White"
					BorderStyle="None" GridLines="Vertical" Font-Names="Verdana" AutoGenerateColumns="False">
					<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
					<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO OPERACI&#211;N"></asp:BoundColumn>
						<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCI&#211;N OPERACI&#211;N"></asp:BoundColumn>
						<asp:BoundColumn DataField="MECANICO" HeaderText="MECANICO"></asp:BoundColumn>
						<asp:BoundColumn DataField="VALOROPERACION" HeaderText="VALOR OPERACI&#211;N" DataFormatString="{0:c}"></asp:BoundColumn>
					</Columns>
				</asp:DataGrid></TD>
		</TR>
		<TR>
			<TD align="center">
				<asp:Label id="lbInfoItems" runat="server" forecolor="RoyalBlue">Información Items</asp:Label></TD>
			<TD>
				<asp:DataGrid id="dgInfoItems" runat="server" CssClass="main" BorderWidth="1px"
					HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3" BorderColor="#999999"
					BackColor="White" BorderStyle="None" GridLines="Vertical" Font-Names="Verdana" AutoGenerateColumns="False">
					<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
					<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO ITEM"></asp:BoundColumn>
						<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCI&#211;N ITEM"></asp:BoundColumn>
						<asp:BoundColumn DataField="VALOR" HeaderText="VALOR ITEM" DataFormatString="{0:c}"></asp:BoundColumn>
						<asp:BoundColumn DataField="IVA" HeaderText="IVA" DataFormatString="{0:n}%"></asp:BoundColumn>
						<asp:BoundColumn DataField="CANTIDAD" HeaderText="CANTIDAD" DataFormatString="{0:n}"></asp:BoundColumn>
						<asp:BoundColumn DataField="TOTAL" HeaderText="TOTAL" DataFormatString="{0:c}"></asp:BoundColumn>
						<asp:BoundColumn DataField="TRANSFERENCIA" HeaderText="TRANSFERENCIA RELACIONADA"></asp:BoundColumn>
					</Columns>
				</asp:DataGrid></TD>
		</TR>
	</TABLE>
</FIELDSET>
</asp:PlaceHolder>
<P></P>
<p>
	<asp:LinkButton id="lnkVolver" onclick="Volver" runat="server">Volver</asp:LinkButton>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
