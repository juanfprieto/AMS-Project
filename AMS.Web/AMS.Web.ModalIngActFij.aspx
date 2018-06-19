<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ModalIngActFij.aspx.cs" Inherits="AMS.Web.ModalActivos" %>
<HTML>
	<HEAD>
	</HEAD>
	<body>
		<form runat="server">
			<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
			<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
			<asp:Panel id="pnlDatos" runat="server" Width="451px">
				<TABLE style="WIDTH: 450px; HEIGHT: 194px">
					<TR>
						<TD>Datos del Activo Fijo
						</TD>
					</TR>
					<TR>
					</TR>
					<TR>
						<TD>Código :
						</TD>
						<TD>
							<asp:TextBox id="tbcod" runat="server" MaxLength="10"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv1" runat="server" ControlToValidate="tbcod">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Descripción&nbsp;:
						</TD>
						<TD>
							<asp:TextBox id="tbdesc" runat="server" Width="241px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv2" runat="server" ControlToValidate="tbdesc">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Centro de Costos Asignado :
						</TD>
						<TD>
							<asp:DropDownList id="ddlcencos" runat="server"></asp:DropDownList></TD>
					</TR>
					<TR>
						<TD>Marca :
						</TD>
						<TD>
							<asp:TextBox id="tbmarca" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv3" runat="server" ControlToValidate="tbmarca">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Modelo :
						</TD>
						<TD>
							<asp:TextBox id="tbmodelo" runat="server" Width="250px" MaxLength="10"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv4" runat="server" ControlToValidate="tbmodelo">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Placa :
						</TD>
						<TD>
							<asp:TextBox id="tbplaca" runat="server" Width="250px" MaxLength="10"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv5" runat="server" ControlToValidate="tbplaca">*</asp:RequiredFieldValidator></TD>
					</TR>
				</TABLE>
			</asp:Panel>
			<p>
			</p>
			<asp:Panel id="pnlCompra" runat="server" Visible="False">
				<TABLE>
					<TR>
						<TD>Datos Sobre la Compra</TD>
					</TR>
					<TR>
					</TR>
					<TR>
						<TD>Fecha de la Factura de Compra :
						</TD>
						<TD>
							<asp:TextBox id="tbfecfaccom" onkeyup="DateMask(this)" runat="server"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv6" runat="server" ControlToValidate="tbfecfaccom">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Fecha de Ingreso :
						</TD>
						<TD>
							<asp:TextBox id="tbfecing" onkeyup="DateMask(this)" runat="server"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv7" runat="server" ControlToValidate="tbfecing">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Fecha de Inicio de Depreciación :
						</TD>
						<TD>
							<asp:TextBox id="tbfecinidep" onkeyup="DateMask(this)" runat="server"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv8" runat="server" ControlToValidate="tbfecinidep">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Nit del Proveedor :
						</TD>
						<TD>
							<asp:TextBox id="tbnit" ondblclick="ModalDialog(this,'SELECT mnit_nit,mnit_nombres CONCAT \' \' CONCAT mnit_apellidos FROM dbxschema.mnit WHERE tvig_vigencia=\'V\'',new Array())"
								runat="server" MaxLength="15"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv9" runat="server" ControlToValidate="tbnit">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Prefijo de Factura de Compra al Proveedor :
						</TD>
						<TD>
							<asp:DropDownList id="ddlpref" runat="server" AutoPostBack="True" onSelectedIndexChanged="ddlPref_IndexChanged"></asp:DropDownList></TD>
					</TR>
					<TR>
						<TD>Número de Factura de Compra al Proveedor :
						</TD>
						<TD>
							<asp:DropDownList id="ddlnum" runat="server"></asp:DropDownList></TD>
					</TR>
					<TR>
						<TD>Número del Pedido :
						</TD>
						<TD>
							<asp:TextBox id="tbnumped" runat="server"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv10" runat="server" ControlToValidate="tbnumped">*</asp:RequiredFieldValidator></TD>
					</TR>
				</TABLE>
			</asp:Panel>
			<p>
			</p>
			<asp:Panel id="pnlCostos" runat="server" Visible="False">
				<TABLE>
					<TR>
						<TD>Datos de los Costos y Valores
						</TD>
					</TR>
					<TR>
					</TR>
					<TR>
						<TD>Valor Histórico de Compra :
						</TD>
						<TD>
							<asp:TextBox id="tbvalhis" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha">0</asp:TextBox>
							<asp:RequiredFieldValidator id="rfv11" runat="server" ControlToValidate="tbvalhis">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Valor Histórico de Compra en Dolares :
						</TD>
						<TD>
							<asp:TextBox id="tbvalhisdol" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha">0</asp:TextBox>
							<asp:RequiredFieldValidator id="rfv12" runat="server" ControlToValidate="tbvalhisdol">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Valor de las mejoras :
						</TD>
						<TD>
							<asp:TextBox id="tbvalmej" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha">0</asp:TextBox>
							<asp:RequiredFieldValidator id="rfv13" runat="server" ControlToValidate="tbvalmej">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Valor de la Inflación Acumulada :
						</TD>
						<TD>
							<asp:TextBox id="tbvalinf" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha">0</asp:TextBox>
							<asp:RequiredFieldValidator id="rfv14" runat="server" ControlToValidate="tbvalinf">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Valor de la Depreciación Acumulada :
						</TD>
						<TD>
							<asp:TextBox id="tbvaldep" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha">0</asp:TextBox>
							<asp:RequiredFieldValidator id="rfv15" runat="server" ControlToValidate="tbvaldep">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Valor de la Inflación de la Depreciación Acumulada&nbsp; :
						</TD>
						<TD>
							<asp:TextBox id="tbvalinfdep" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha">0</asp:TextBox>
							<asp:RequiredFieldValidator id="rfv16" runat="server" ControlToValidate="tbvalinfdep">*</asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Número de Cuotas (Meses) a Depreciar :
						</TD>
						<TD>
							<asp:TextBox id="tbnumcuo" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
							<asp:RequiredFieldValidator id="rfv17" runat="server" ControlToValidate="tbnumcuo">*</asp:RequiredFieldValidator>
							<asp:RegularExpressionValidator id="rev1" runat="server" ControlToValidate="tbnumcuo" ValidationExpression="\d+">*</asp:RegularExpressionValidator></TD>
					</TR>
					<TR>
						<TD>Vigencia :
						</TD>
						<TD>
							<asp:DropDownList id="ddlvig" runat="server"></asp:DropDownList></TD>
					</TR>
				</TABLE>
			</asp:Panel>
			<p>
			</p>
			<p>
				<asp:Panel id="pnlCuentas" runat="server" Visible="False">
					<TABLE>
						<TR>
							<TD>Datos Sobre las Cuentas Contables
							</TD>
						</TR>
						<TR>
						</TR>
						<TR>
							<TD>Cuenta Real :
							</TD>
							<TD>
								<asp:TextBox id="tbcuereal" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripción FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
									runat="server" MaxLength="16" ToolTip="Haga doble click para iniciar la busqueda"></asp:TextBox>
								<asp:RequiredFieldValidator id="rfv18" runat="server" ControlToValidate="tbcuereal">*</asp:RequiredFieldValidator></TD>
						</TR>
						<TR>
							<TD>Cuenta de la Depreciación :
							</TD>
							<TD>
								<asp:TextBox id="tbcuedep" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripción FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
									runat="server" MaxLength="16" ToolTip="Haga doble click para iniciar la busqueda"></asp:TextBox>
								<asp:RequiredFieldValidator id="rfv19" runat="server" ControlToValidate="tbcuedep">*</asp:RequiredFieldValidator></TD>
						</TR>
						<TR>
							<TD>Cuenta del Gasto de la Depreciación :
							</TD>
							<TD>
								<asp:TextBox id="tbcuegasdep" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripción FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
									runat="server" MaxLength="16" ToolTip="Haga doble click para iniciar la busqueda"></asp:TextBox>
								<asp:RequiredFieldValidator id="rfv20" runat="server" ControlToValidate="tbcuegasdep">*</asp:RequiredFieldValidator></TD>
						</TR>
						<TR>
							<TD>Cuenta de Inflación :
							</TD>
							<TD>
								<asp:TextBox id="tbcueinf" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripción FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
									runat="server" MaxLength="16" ToolTip="Haga doble click para iniciar la busqueda"></asp:TextBox>
								<asp:RequiredFieldValidator id="rfv21" runat="server" ControlToValidate="tbcueinf">*</asp:RequiredFieldValidator></TD>
						</TR>
						<TR>
							<TD>Cuenta de la Inflación de la Depreciación :
							</TD>
							<TD>
								<asp:TextBox id="tbcueinfdep" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripción FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
									runat="server" MaxLength="16" ToolTip="Haga doble click para iniciar la busqueda"></asp:TextBox>
								<asp:RequiredFieldValidator id="rfv22" runat="server" ControlToValidate="tbcueinfdep">*</asp:RequiredFieldValidator></TD>
						</TR>
						<TR>
							<TD>Cuenta de la Corrección Monetaria de la Inflación :
							</TD>
							<TD>
								<asp:TextBox id="tbcormoninf" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripción FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
									runat="server" MaxLength="16" ToolTip="Haga doble click para iniciar la busqueda"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>Cuenta de la Corrección Monetaria de la Depreciación :
							</TD>
							<TD>
								<asp:TextBox id="tbcormondep" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripción FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
									runat="server" MaxLength="16" ToolTip="Haga doble click para iniciar la busqueda"></asp:TextBox></TD>
						</TR>
					</TABLE>
				</asp:Panel>
			</p>
			<p>
				<asp:LinkButton id="lnbAnt" runat="server" onCommand="lnbAnt_Command" CausesValidation="False" Enabled="False">Anterior</asp:LinkButton>
				&nbsp;&nbsp;
				<asp:LinkButton id="lnbSig" runat="server" onCommand="lnbSig_Command">Siguiente</asp:LinkButton>
			</p>
			<p>
				<asp:Button id="btnGuardar" onclick="btnGuardar_Click" runat="server" Visible="False" CausesValidation="False"
					Text="Guardar"></asp:Button>
				&nbsp;&nbsp;
				<asp:Button id="btnCancelar" onclick="btnCancelar_Click" runat="server" CausesValidation="False"
					Text="Cancelar"></asp:Button>
			</p>
			<p>
				<asp:Label id="lb" runat="server"></asp:Label>
			</p>
		</form>
	</body>
</HTML>
