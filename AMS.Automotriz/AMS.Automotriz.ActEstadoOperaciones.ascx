<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.ActEstadoOperaciones.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ActEstadoOperaciones" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>



<fieldset>
<table id="Table" class="filtersIn">
	<tr>
		<td>Técnico:
		</td>
		<td><asp:dropdownlist id="ddlMecanicos" class="dmediano" Runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Clave:
		</td>
		<td><asp:textbox id="txtClave" Runat="server" class="tpequeno" TextMode="Password"></asp:textbox></td>
	</tr>
	<tr>
		<td>
			<asp:Button Runat="server" ID="btnValidarPass" Text="Validar" OnClick="ValidarPass"></asp:Button>
		</td>
	</tr>
</table>
<P>Si el técnico buscado no aparece, es debido a que no está debidamente enlazado a los 
	empleados, especialidades y-o sedes
	<meta content="True" name="vs_showGrid">
</P>
</fieldset>
<br />
 <p></p>
<asp:datagrid id="dgOperaciones" cssclass="datagrid" runat="server" OnItemDataBound="DataBound_Operaciones" AutoGenerateColumns="False">
	<Columns>
		<asp:BoundColumn DataField="PREFIJO" HeaderText="PREFIJO"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMERO" HeaderText="NUMERO"></asp:BoundColumn>
		<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO"></asp:BoundColumn>
		<asp:BoundColumn DataField="OPERACION" HeaderText="OPERACION"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="ESTADO OPERACION">
			<ItemTemplate>
				<asp:DropDownList id="ddlOperaciones" runat="server"></asp:DropDownList>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="OBSERVACIONES">
			<ItemTemplate>
				<asp:TextBox id="txtObservaciones" TextMode="MultiLine" runat="server"></asp:TextBox>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="PLACA" HeaderText="PLACA"></asp:BoundColumn>
	 
	</Columns>
</asp:datagrid>
<p></p>
<br />
	<left style="margin-left:4em;">
        <fieldset id="fldMail" runat="server" visible="false">
            <table>
                <tr>
                    <td>
                        <asp:TextBox id="txtMail" runat="server" placeholder="*Correo Jefe de Repuestos (Opcional)" Style="border-radius:3px; height:25px; width:270px"  Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label id="lbMail" runat="server" Text="Ingrese el correo del jefe de Repuestos para notificar los repuestos faltantes. (Opcional)" Font-Italic="true" ForeColor="MidnightBlue" Font-Size="13px" Visible="false"></asp:Label><br /><br />
                    </td>
                </tr>
            </table>
        </fieldset>
		<P></P>
	</left>
<p>
    <asp:button id="btnActualizar" onclick="Actualizar" runat="server" Text="Actualizar " Visible="False" style="position:relative; left:100px; padding:10px; font-size: large; margin:5px;"></asp:Button>
</p>
    
<br />
<br />
<br />
<placeholder id="plhEnvioMail" runat="server" Visible="true">
    <asp:datagrid id="dgMail" cssClass="datagrid" runat="server" OnItemDataBound="DataBound_Operaciones" AutoGenerateColumns="False">
	<Columns>
		<asp:BoundColumn DataField="PREFIJO" HeaderText="PREFIJO"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMERO" HeaderText="NUMERO"></asp:BoundColumn>
		<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO"></asp:BoundColumn>
		<asp:BoundColumn DataField="OPERACION" HeaderText="OPERACION"></asp:BoundColumn>
        <asp:BoundColumn DataField="ESTADO_OPERACION" HeaderText="OPERACION"></asp:BoundColumn>
        <asp:BoundColumn DataField="OBSERVACIONES" HeaderText="OPERACION"></asp:BoundColumn>
		<asp:BoundColumn DataField="PLACA" HeaderText="PLACA"></asp:BoundColumn>
	 
	</Columns>
</asp:datagrid>
</placeholder>
<P>
	<asp:Label id="error" runat="server"></asp:Label></P>



