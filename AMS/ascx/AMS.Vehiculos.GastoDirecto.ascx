<%@ Control Language="c#" codebehind="AMS.Vehiculos.GastoDirecto.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.GastoDirecto" %>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
				<img height="60" src="../img/AMS.Flyers.News.png" border="0">
			</th>
            	 
            <td>
            
            <table id="Table2" class="filtersIn">
                <p>
                    Escoja el tipo de Inclusión en Gastos Directos a Realizar :
                    <asp:DropDownList id="tipoInclusion" class="dmediano" runat="server">
                    <asp:ListItem Value="V" Selected="True">Veh&#237;culos</asp:ListItem>
                    <asp:ListItem Value="E">Embarques</asp:ListItem>
                    </asp:DropDownList>
                </p>
                <p>
                     <asp:CheckBox id="chkConsecutivo" runat="server" Text="Generar Consecutivo Automático" Checked="True"
                     TextAlign="Left"></asp:CheckBox>                    
                </p>
                <p>
                 <asp:Button id="btnRealizar" onclick="Realizar_Inclusion" runat="server" Text="Realizar"></asp:Button>
                </p>
                </table>
            
			</td>
		</tr>
	</tbody>
</table>
<asp:Label id="lb" runat="server"></asp:Label>
</fieldset>