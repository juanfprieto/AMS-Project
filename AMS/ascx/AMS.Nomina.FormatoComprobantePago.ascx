<%@ Control Language="c#" codebehind="AMS.Nomina.FormatoComprobantePago.cs" autoeventwireup="false" Inherits="AMS.Nomina.FormatoComprobantePago" %>
<fieldset>

<table id="Table" class="filtersIn">
	<tbody>
		<tr>
			<td colspan="5">
				Comprobante de Pago Sistemas ECAS 
                <%--<div id ="myDiv" runat="server" Width="300px" Height="300px">
                    
                </div>--%>
                <img id="imgLogo" runat="server" Visible="false" />
                <asp:image id="imgLogo1" runat="server" Visible="false"/>
                
            </td>
		</tr>
		<tr>
			<td>
				Codigo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			</td>
			<td>
				Nombre del Empleado
			</td>
			<td>
				Cedula
			</td>
			<td>
				Fecha
			</td>
			<td>
				Sueldo
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label id="LBCODIGO" class="lpequeno" runat="server"></asp:Label></td>
			<td>
				<asp:Label id="LBNOMBRE" class="lpequeno" runat="server"></asp:Label></td>
			<td>
				<asp:Label id="LBCEDULA" class="lpequeno" runat="server"></asp:Label></td>
			<td>
				<asp:Label id="LBFECHA" class="lpequeno" runat="server"></asp:Label></td>
			<td>
				<asp:Label id="LBSUELDO" class="lpequeno" runat="server"></asp:Label></td>
		</tr>
		<tr>
            
			<td colspan="5">
				<asp:DataGrid id="DATAGRIDPAGO" runat="server" cssclass="datagrid"></asp:DataGrid>
			</td>
		</tr>
		<tr>
			<td colspan="2">
			</td>
			<td>
				Subtotal
			</td>
			<td colspan="1">
				&nbsp;<asp:Label id="LBSUBTP" runat="server"></asp:Label></td>
			<td colspan="1">
				&nbsp;<asp:Label id="LBSUBTD" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td colspan="2">
			</td>
			<td>
				Neto&nbsp;</td>
			<td colspan="2">
				&nbsp;<asp:Label id="LBNETO" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td colspan="2">
				Recibido</td>
			<td colspan="3">
				Firma Autorizada</td>
		</tr>
		<tr>
			<td colspan="2">
				--------------------------</td>
			<td colspan="3">
				-----------------------</td>
		</tr>
		<tr>
			<td colspan="2">
				c.c. No.</td>
			<td colspan="3">
			</td>
		</tr>
        <tr>
            <td>
                <asp:label id="lbErrores" runat="server"></asp:label>
            </td>
        </tr>
	</tbody>
</table>
</fieldset>
<p>
</p>
<p>
</p>
