<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Nomina.AdministrarNovedades.ascx.cs" Inherits="AMS.Nomina.AMS_Nomina_AdministrarNovedades" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<legend>Administrador de Novedades
	</legend>
	<p></p>
	
			<td>Año :
			</td>
			<td><asp:dropdownlist id="ddlano" class="dpequeno" Runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>Mes :
			</td>
			<td><asp:dropdownlist id="ddlmes" class="dpequeno" Runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>Quincena :
			</td>
			<td><asp:dropdownlist id="ddlquincena" class="dpequeno" Runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>Empleado :
			</td>
			<td>
				<asp:TextBox ReadOnly="True" id="txtEmpleado" onclick="ModalDialog(this,'Select me.memp_codiempl AS NIT, mn.MNIT_APELLIDOS CONCAT \' \'CONCAT COALESCE(mn.MNIT_APELLIDO2,\'\') CONCAT \' \' CONCAT mn.MNIT_NOMBRES CONCAT \' \' CONCAT COALESCE(MN.mnit_NOMBRE2,\'\') AS NOMBRE from DBXSCHEMA.MEMPLEADO me, DBXSCHEMA.MNIT mn where me.mnit_nit=mn.mnit_nit and me.test_estado <> \'4\' ORDER BY MEMP_CODIEMPL', new Array(),1)"
					runat="server" class="tmediano" Font-Size="XX-Small"></asp:TextBox>
				<asp:textbox id="txtEmpleadoa" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox>
				<br>
				<asp:CheckBox id="Todos" runat="server" Text="Todos los Empleados"></asp:CheckBox>
			</td>
		</tr>
		<tr>
			<td>Concepto :
			</td>
			<td><asp:dropdownlist id="ddlConceptos" class="dmediano" Runat="server"></asp:dropdownlist>
				<br>
				<asp:CheckBox id="Conceptos" runat="server" Text="Todos los Conceptos"></asp:CheckBox>
			</td>
		</tr>
		<tr>
			<td><asp:button id="btnAceptar" Runat="server" Text="Aceptar" CausesValidation="False"></asp:button></td>
		</tr>
        </td>
      
        </table>
</fieldset>
	<p><asp:datagrid id="dgNov" AllowPaging="True" CellPadding="3" AutoGenerateColumns="False"
			showfooter="True" runat="server" cssclass="datagrid">
			<FooterStyle CssClass="footer"></FooterStyle>
			<HeaderStyle CssClass="header"></HeaderStyle>
			<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Empleado">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "EMPLEADO") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="tbempf" ondblclick="ModalDialog(this,'SELECT MEMP.memp_codiempl,MNIT.mnit_apellidos CONCAT \' \' CONCAT COALESCE(MNIT.mnit_apellido2,\'\') CONCAT \' \' CONCAT MNIT.mnit_nombres CONCAT \' \' CONCAT COALESCE(MNIT.mnit_nombre2,\'\') AS EMPLEADO FROM dbxschema.mempleado MEMP,dbxschema.mnit MNIT WHERE MEMP.mnit_nit=MNIT.mnit_nit AND MEMP.test_estado <> \'4\' ORDER BY MEMP_CODIEMPL',new Array())"
							Runat="server" ToolTip="Haga Doble Click para iniciar la busqueda" MaxLength="15" class="tmediano"></asp:TextBox>
					</FooterTemplate>
					<EditItemTemplate>
						<asp:TextBox id="tbemp" ondblclick="ModalDialog(this,'SELECT MEMP.memp_codiempl,MNIT.mnit_apellidos CONCAT \' \' CONCAT COALESCE(MNIT.mnit_apellido2,\'\') CONCAT \' \' CONCAT MNIT.mnit_nombres CONCAT \' \' CONCAT COALESCE(MNIT.mnit_nombre2,\'\') AS EMPLEADO FROM dbxschema.mempleado MEMP,dbxschema.mnit MNIT WHERE MEMP.mnit_nit=MNIT.mnit_nit AND MEMP.test_estado <> \'4\' ORDER BY MEMP_CODIEMPL',new Array())" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EMPLEADO") %>' ToolTip="Haga Doble Click para iniciar la busqueda" MaxLength="15" class="tpequeno">
						</asp:TextBox>
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Concepto">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CONCEPTO") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox ID="tbconf" class="tmediano" MaxLength="5" Runat="server" onDblClick="ModalDialog(this,'SELECT pcon_concepto,pcon_nombconc FROM dbxschema.pconceptonomina WHERE pcon_claseconc=\'N\' ORDER BY 1',new Array())"
							ToolTip="Haga doble click para iniciar la busqueda"></asp:TextBox>
					</FooterTemplate>
					<EditItemTemplate>
						<asp:TextBox ID="tbcon" class="tmediano" MaxLength="5" Runat="server" onDblClick="ModalDialog(this,'SELECT pcon_concepto,pcon_nombconc FROM dbxschema.pconceptonomina WHERE pcon_claseconc=\'N\' ORDER BY 1',new Array())" ToolTip="Haga doble click para iniciar la busqueda" Text='<%# DataBinder.Eval(Container.DataItem, "CONCEPTO") %>'>
						</asp:TextBox>
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Documento <br> de Referencia">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "DOCUMENTO") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="tbnovf" Runat="server" MaxLength="8" class="tmediano"></asp:TextBox>
					</FooterTemplate>
					<EditItemTemplate>
						<asp:TextBox id="tbnov" Runat="server" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "DOCUMENTO") %>' class="tpequeno">
						</asp:TextBox>
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor Total">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "VALOR","{0:C}") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="tbvaltotf" onkeyup="NumericMask(this)" Runat="server" Text="0" MaxLength="15"
							class="tmediano"></asp:TextBox>
					</FooterTemplate>
					<EditItemTemplate>
						<asp:TextBox id="tbvaltot" onkeyup="NumericMask(this)" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "VALOR","{0:N}") %>' MaxLength="15" class="tmediano">
						</asp:TextBox>
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Cantidad">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox ID="tbcantf" Runat="server" MaxLength="5" class="tmediano"
							onkeyup="NumericMask(this)"></asp:TextBox>
					</FooterTemplate>
					<EditItemTemplate>
						<asp:TextBox ID="tbcant" Runat="server" MaxLength="5" Text='<%# DataBinder.Eval(Container.DataItem, "CANTIDAD","{0:N}") %>' class="tmediano">
						</asp:TextBox>
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Fecha">
					<ItemTemplate>
						<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FECHA")).ToString("yyyy-MM-dd") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox ID="tbfecf" Runat="server" MaxLength="10" onkeyup="DateMask(this)" class="tmediano"></asp:TextBox>
					</FooterTemplate>
					<EditItemTemplate>
						<asp:TextBox ID="tbfec" Runat="server" MaxLength="10" onkeyup="DateMask(this)" class="tmediano" Text='<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FECHA")).ToString("yyyy-MM-dd") %>'>
						</asp:TextBox>
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Editar / <br> Actualizar">
					<ItemTemplate>
						<asp:Button ID="btnedit" Runat="server" Text="Editar" CommandName="Edit" CausesValidation="False"></asp:Button>
					</ItemTemplate>
					<EditItemTemplate>
						<asp:Button runat="server" Text="Actualizar" CommandName="Update" ID="btnupd"></asp:Button>&nbsp;
						<asp:Button runat="server" Text="Cancelar" CommandName="Cancel" CausesValidation="false" ID="btncan"></asp:Button>
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Insertar / <br> Borrar">
					<ItemTemplate>
						<asp:Button id="btnbor" runat="server" Text="Eliminar" commandname="eliminar" />
					</ItemTemplate>
					<FooterTemplate>
						<asp:Button id="btnagr" runat="server" Text="Agregar" commandname="agregar" />
					</FooterTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid></p>
<p><asp:Label ID="lb" Runat="server" Text=""></asp:Label></p>
