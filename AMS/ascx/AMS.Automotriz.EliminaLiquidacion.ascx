<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.EliminaLiquidacion.ascx.cs" Inherits="AMS.Automotriz.EliminaLiquidacion" %>


<fieldset id="fldLiquida" runat="server">
    <p>
        <table>
            <tr>
                <td><asp:Label> Tipo Comisión: </asp:Label><br />
                <asp:DropDownList id="ddltipoComi" runat="server" Width="150px" Height="22px">
                    <asp:ListItem Value="0" Text="Seleccione.."> </asp:ListItem>
                    <asp:ListItem Value="M" Text="Técnico" ></asp:ListItem>
                    <asp:ListItem Value="R" Text="Recepcionista" ></asp:ListItem>
                </asp:DropDownList></td>
                <td>
                    
                    <asp:Label> Fecha Liquidación: </asp:Label><br />
                    <asp:TextBox id="txtFecha" runat="server" Width="125px" TextMode="Date" ReadOnly="true"></asp:TextBox>
                    <img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'"
						src="../img/AMS.Icon.Calendar.gif" border="0">
                    <table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
						onmouseout="calendar.style.visibility='hidden'">
                        <tr>
							<td>
								<asp:calendar BackColor="Beige" id="fecha" runat="server" OnSelectionChanged="ChangeDate"></asp:Calendar>
							</td>
                        </tr>
				    </table>
                </td>
                
            </tr>
        </table>
        <br /><br /><br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnMostrar" runat="server" OnClick="mostrarLiquidacion" Text="Mostrar Ordenes" />
    </p>
</fieldset>
<fieldset id="fldOrdenes" runat="server" visible="false">
    <P align="right">
	    <ASP:DataGrid id="dgOrden" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
		    CellPadding="3" CellSpacing="1" >
		    <FooterStyle cssclass="footer"></FooterStyle>
		    <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		    <PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
		    <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="item"></ItemStyle>
	    </ASP:DataGrid>
    </P> <br /><br /><br />
    <table>
        <tr>
            <td>
                <asp:Button id="btnEliminar" runat="server" OnClick="EliminaLiquida" Text="Eliminar"/>
            </td>
            <td>
                <asp:Button id="btnCancelar" runat="server" OnClick="cancelarAccion" Text="Cancelar"/>
            </td>

        </tr>
    </table>
</fieldset>
