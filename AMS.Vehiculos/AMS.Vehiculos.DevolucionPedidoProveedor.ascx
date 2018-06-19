<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.DevolucionPedidoProveedor.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_DevolucionPedidoProveedor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%--<script  type="text/javascript">
    function WriteText() {
        document.getElementById('datepicker').removeAttribute('readonly');
    }
    </script>--%>
   <fieldset>        
    <TABLE id="Table1" class="filtersIn">
        <TR>
            <td align="left">
                Prefijo Nota Devoluci�n a Proveedor:                            
            </td>
            <td>
                <asp:DropDownList id="ddlNotDevProv" class="dmediano" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left">
                Cat�logo Veh�culo:              
            </td>
            <td>				
                <asp:DropDownList id="ddlCatVehDev" runat="server" class="dmediano" AutoPostBack="true" OnSelectedIndexChanged="CambioCatalogoDevolucion"></asp:DropDownList><asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image>
            </td>
        </tr>
        <tr>
            <td align="left">
                VIN Veh�culo:
            </td>                
            <td>           
                <asp:DropDownList id="ddlVINDev" class="dmediano" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
		<td>Observaci�n
		</td>
		<td><asp:textbox id="txtobsv" class="tmediano" Runat="server"></asp:textbox></td>
	</tr>       
        <tr>
            <td align="left">
                Fecha:
                <td> <asp:textbox id="tbDate" runat="server" class="tpequeno" ></asp:textbox><IMG onmouseover="calendar.style.visibility='visible'" src="../img/AMS.Icon.Calendar.gif" border="0">
                    <table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
					    onmouseout="calendar.style.visibility='hidden'">
					    <tbody>
						    <tr>
							    <td><asp:calendar BackColor=Beige id="calDate" runat="server" OnSelectionChanged="ChangeDate" enableViewState="true"></asp:calendar></td>
						    </tr>
					    </tbody>
				    </table>
                </td>
       </tr>

			<td> <asp:Button id="btnDevolucion" onclick="RealizarDevolucion" runat="server" Text="Realizar Devoluci�n"></asp:Button></td>        
            <td>
            </td>
        </TR>
    </TABLE>
</fieldset>      
<P>
    <asp:Label id="lb" runat="server" Visible="False">Label</asp:Label>
</P>
