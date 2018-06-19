<%@ Control Language="c#" codebehind="AMS.Vehiculos.PostVenta.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.PostVenta" %>

<fieldset>        
	    <TABLE id="Table1" class="filtersIn">
         <legend class="Legends">Post Venta de Vehículos</legend>
		    <TR>
              <td>      
        <p>
	    <asp:PLaceHolder id="controls" runat="server">
		   <P>Proceso Post Venta :<br />
			<asp:DropDownList id="proceso" runat="server" class="dmediano"></asp:DropDownList>
           </P>
		  <P>Catálogo de Vehículo :<br>
			<asp:DropDownList id="catalogo" runat="server" class="dpequeno" OnSelectedIndexChanged="Cambio_Tipo_Documento"
				AutoPostBack="true"></asp:DropDownList>
                <br>
               VIN Vehículo :<br>
			<asp:DropDownList id="vinVehiculo" runat="server" class="dpequeno"></asp:DropDownList>
            </P>
		   <P>Obervaciones :
			<asp:TextBox id="observaciones" runat="server" class="dmediano" TextMode="MultiLine"></asp:TextBox>
            </P>
		  <P>Fecha del Proceso : <IMG onmouseover="calendar1.style.visibility='visible'" onmouseout="calendar1.style.visibility='hidden'"
				src="../img/AMS.Icon.Calendar.gif" border="0">
			<TABLE id="calendar1" onmouseover="calendar1.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
				onmouseout="calendar1.style.visibility='hidden'">
				<TR>
					<TD>
						<asp:calendar BackColor=Beige id="fechaProceso" runat="server"></asp:Calendar>
                    </TD>
				</TR>
			</TABLE>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Estado Proceso :
			<asp:DropDownList id="estado" runat="server" Width="199px"></asp:DropDownList></P>
		<P>
        </td>
        </TR>
        </TABLE>
</fieldset>  
 


			<asp:Button id="btnProceso" onclick="Realizar_Proceso" runat="server" Text="Realizar Proceso"></asp:Button>
        </P>
	</asp:PLaceHolder>
<P>
</P>
<p>
</p>
<p>
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p>
<p>
	<asp:DataGrid id="procesosPostVenta" runat="server" cssclass="datagrid" GridLines="Vertical" AutoGenerateColumns="false">
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="CODIGO PROCESO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGOPROCESO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DESCRIPCIÓN PROCESO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DESCPROCESO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ESTADO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="OBSERVACIÓN">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OBSERVACION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="FECHA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Button id="btnVolver" onclick="Volver" runat="server" Text="Volver" Visible="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
