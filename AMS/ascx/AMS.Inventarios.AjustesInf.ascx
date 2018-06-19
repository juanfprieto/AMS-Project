<%@ Control Language="c#" codebehind="AMS.Inventarios.AjustesInf.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.AjustesInf" %>
<fieldset >
	<legend>Ajuste:</legend>
	<table id="Table1" class="filtersIn">
		<tbody>
        <p>
			<tr>
				<td>
					Cod. Documento:<br>
					<asp:DropDownList id="ddlCodDoc" class="dpequeno" runat="server" OnSelectedIndexChanged="ChangeDocument" AutoPostBack="True"></asp:DropDownList>
                    </td>
				<td>
					Num. Documento:<br />
					<asp:Label id="lbDoc" class="lpequeno" runat="server">(Aquí va el documento)</asp:Label>
                </td>
				
                  </tr>
            </p>
            <p>
			<tr>
            <td>
					Año:&nbsp; <br />
                    <asp:Label id="lbYear" class="lpequeno" runat="server">(Aquí va el año vigente tomado de
                    cinventar)</asp:Label>
                   </td>
             <td>
					Mes:&nbsp; <br />
                    <asp:Label id="lbMonth" class="lpequeno" runat="server">(Aquí va el mes vigente tomado de
                    cinventar)</asp:Label>
                    </td>
				
			</tr>
            </p>
            <p>
            <tr>
            <td>
					Almacen:<br/>
					<asp:DropDownList id="ddlAlmacen" class="dpequeno" runat="server"></asp:DropDownList>
				</td>
                <td>
					Centro de Costo:<br/>
					<asp:DropDownList id="ddlCentro" class="dmediano" runat="server"></asp:DropDownList>
				</td>
            </tr>
            </p>
            <p>
			<tr>
				<td>
					Vendedor:<br>
					<asp:DropDownList id="ddlVendedor" class="dmediano" runat="server"></asp:DropDownList>
				</td>

                <td>	
                	PAAG&nbsp;vigente para este periodo:
                    <br /><asp:Label id="lbPorcentaje" class="lpequeno" runat="server">(Aquí
                    va el procentaje tomado del PAAG)</asp:Label>
				</td>
			</tr>
            </p>
			<tr>
				<td>
					Fecha:&nbsp;&nbsp;
                    <br /><asp:TextBox id="tbDate" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
					<img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0">
					<table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendar.style.visibility='hidden'">
						<tbody>
							<tr>
								<td>
									<asp:calendar BackColor=Beige id="calDate" runat="server" enableViewState="true" OnSelectionChanged="ChangeDate"></asp:Calendar>
								</td>
							</tr>
						</tbody>
					</table>
				</td>
				<td align="left">
					<asp:Button id="btnEjecuta" onclick="Ajustar" runat="server" Text="Ejecutar"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>
			
<p>
	<asp:DataGrid id="dgPAAG" runat="server"></asp:DataGrid>
</p>
<p>
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p>
