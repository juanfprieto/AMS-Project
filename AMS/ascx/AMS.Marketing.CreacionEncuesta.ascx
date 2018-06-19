<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Marketing.CreacionEncuesta.ascx.cs" Inherits="AMS.Marketing.CreacionEncuesta" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<P><asp:panel id="Pnlaplicacion" runat="server">
		<asp:label id="LbAplicacion" runat="server" Font-Bold="True">Datos de la Encuesta</asp:label>
		<P></P>
		<TABLE id="Table1" class="filtersIn">
			<TR>
				<TD>
					<asp:label id="LbEncuesta" runat="server">Codigo Encuesta.</asp:label></TD>
				<TD>
					<asp:textbox id="TbnEncuesta" runat="server" MaxLength="10" Width="112px"></asp:textbox>
					<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Digite el Código" ControlToValidate="TbnEncuesta"></asp:requiredfieldvalidator></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="LbFecha" runat="server">Fecha</asp:label></TD>
				<TD>
					<asp:textbox id="TbFecha" onkeyup="DateMask(this)" runat="server" Width="112px"></asp:textbox>
					<asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" Width="180px" ErrorMessage="Digite Fecha(aaaa-mm-dd)"
						ControlToValidate="TbFecha" ValidationExpression="\d{4}-\d{2}-\d{2}" ForeColor="Navy"></asp:regularexpressionvalidator>
					<asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" Font-Bold="True" ErrorMessage="Debe Digitar Fecha"
						ControlToValidate="TbFecha" ForeColor="Navy"></asp:requiredfieldvalidator></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="LbNombre" runat="server" Font-Bold="True" Width="176px">Nombre de la Encuesta</asp:label></TD>
				<TD>
					<asp:textbox id="TbNombre" runat="server" Width="337px"></asp:textbox>&nbsp;
					<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" Font-Bold="True" Width="184px" ErrorMessage="Digite Nombre Encuesta"
						ControlToValidate="TbNombre" ForeColor="Navy"></asp:requiredfieldvalidator></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="LbRespon" runat="server">Responsable Encuesta</asp:label></TD>
				<TD>
					<asp:textbox id="TbRespon" runat="server" Width="336px"></asp:textbox>&nbsp;
					<asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" Width="240px" ErrorMessage="Digite Nombre Responsable"
						ControlToValidate="TbRespon" ForeColor="Navy"></asp:requiredfieldvalidator></TD>
			</TR>
			<TR>
				<TD>Objetivo de la Encuesta</TD>
				<TD>
					<asp:TextBox id="tbobj" runat="server" Width="480px"></asp:TextBox>
					<asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ErrorMessage="Digite el objetivo" ControlToValidate="tbobj"></asp:RequiredFieldValidator></TD>
			</TR>
		</TABLE>
	</asp:panel>
<P></P>
<asp:panel id="PnlPregunta" runat="server" Width="576px" Visible="False">
	<TABLE>
		<TR>
			<TD align="left">
				<asp:label id="LbPreguntas" runat="server" Font-Bold="True" Visible="False">Preguntas de la Encuesta</asp:label>
				<asp:datagrid id="Datagrid1" runat="server" cssclass="datagrid" AutoGenerateColumns="False"	CellPadding="3" showfooter="True">
					<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
					<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
					<ItemStyle cssclass="item"></ItemStyle>
					<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
					<FooterStyle cssclass="footer"></FooterStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="PREGUNTAS">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PREGUNTAS") %>
							</ItemTemplate>
							<FooterTemplate>
								<center>
									<asp:TextBox id="Textbox2" runat="server" ReadOnly="true" OnClick="ModalDialog(this,'select ppre_codpreg,ppre_descripcion from PPREGUNTAENCUESTA',new Array(),1)"
										Width="70" ToolTip="Haga Click"></asp:TextBox>
								</center>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="COMENTARIOS">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "COMENTARIOS") %>
							</ItemTemplate>
							<FooterTemplate>
								<center>
									<asp:TextBox id="Textbox2a" runat="server" ReadOnly="true" Width="170" ToolTip="Haga Click"></asp:TextBox>
								</center>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="AGREGAR/ELIMINAR">
							<ItemTemplate>
								<asp:Button id="btnbor" runat="server" Text="Eliminar" commandname="eliminar" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:Button id="btnagr" runat="server" Text="Agregar" commandname="agregar" />
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
				<P></P>
			</TD>
		</TR>
	</TABLE>
</asp:panel>
<asp:linkbutton id="lnbAnt" runat="server">Anterior</asp:linkbutton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:linkbutton id="lnbSig" runat="server" CommandName="lnbSig" CommandArgument="lnbSig">Siguiente</asp:linkbutton>
<P><asp:button id="BtnGuardar" runat="server" Text="Guardar " Visible="False" onclick="btnGuardar_Click"></asp:button>&nbsp;
	<asp:button id="BtnCancelar" runat="server" Text="Cancelar" onclick="BtnCancelar_Click"></asp:button></P>
<P>
	<asp:Label id="lb" runat="server"></asp:Label></P>
