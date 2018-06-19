<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PapeleriaAlmacen.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PapeleriaAlmacen" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:label id="Label1" runat="server" Font-Bold="True" ForeColor="Black">Manejo Papeleria Almacen</asp:label>
<HR width="100%" color="blue" SIZE="2">
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<FIELDSET style="WIDTH: 493px; HEIGHT: 112px"><LEGEND>Seleccion</LEGEND>
	<TABLE id="Table1" style="WIDTH: 488px; HEIGHT: 83px" cellSpacing="0" cellPadding="0" width="488"
		border="0">
		<TR>
			<TD style="HEIGHT: 27px"><asp:radiobuttonlist id="RadioButtonList1" runat="server" Font-Bold="True" ForeColor="Black" RepeatDirection="Horizontal"
					Font-Size="XX-Small" AutoPostBack="True">
					<asp:ListItem Value="1">Despacho</asp:ListItem>
					<asp:ListItem Value="2">Recepcion</asp:ListItem>
					<asp:ListItem Value="3">Retorno</asp:ListItem>
				</asp:radiobuttonlist></TD>
		</TR>
		<TR>
			<td></TD>
		</TR>
	</TABLE>
</FIELDSET>
<asp:panel id="Panel1" runat="server" Height="104px" Width="552px" Visible="False">
	<FIELDSET style="WIDTH: 552px; HEIGHT: 88px"><LEGEND>Despacho</LEGEND>
		<TABLE id="Table2" style="WIDTH: 544px; HEIGHT: 57px" cellSpacing="0" cellPadding="0" width="544"
			border="0">
			<TR>
				<td>
					<asp:Label id="Label17" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Fecha Despacho</asp:Label></TD>
				<td>
					<asp:TextBox id="fechaDes" runat="server" Font-Size="XX-Small" Width="95px"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 158px; HEIGHT: 11px">
					<asp:Label id="Label2" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Agencia de Destino</asp:Label></TD>
				<TD style="HEIGHT: 11px">
					<asp:DropDownList id="agencia" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="250px"
						AutoPostBack="True"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 158px; HEIGHT: 1px">
					<asp:Label id="Label3" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Tipo de Despacho</asp:Label></TD>
				<TD style="HEIGHT: 1px">
					<asp:DropDownList id="despacho" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="136px"
						AutoPostBack="True"></asp:DropDownList></TD>
			</TR>
			<TR>
				<td>
					<asp:Label id="Label22" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Secuencia Disponible</asp:Label></TD>
				<td>
					<asp:Label id="SecDispon" ForeColor="Red" Font-Bold="True" runat="server" Font-Size="XX-Small">Secuencia Disponible</asp:Label></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 12px">
					<asp:Label id="Label21" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Ultima Secuencia Asignada</asp:Label></TD>
				<TD style="HEIGHT: 12px">
					<asp:Label id="UltimaSecuencia" ForeColor="Red" Font-Bold="True" runat="server" Font-Size="XX-Small">Label</asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 158px">
					<asp:Label id="Label4" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Secuencia</asp:Label></TD>
				<td>
					<asp:Label id="Label5" Font-Bold="True" runat="server" Font-Size="XX-Small">Inicio</asp:Label>
					<asp:TextBox id="IniDes" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="40px">0</asp:TextBox>
					<asp:Label id="Label6" Font-Bold="True" runat="server" Font-Size="XX-Small">Fin</asp:Label>
					<asp:TextBox id="FinDes" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="40px">0</asp:TextBox></TD>
			</TR>
			<TR>
				<td>
					<asp:Label id="Label18" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Responsable</asp:Label></TD>
				<td>
					<asp:TextBox id="Responsable" runat="server" Font-Size="XX-Small" Width="280px"></asp:TextBox></TD>
			</TR>
		</TABLE>
		<asp:Button id="despachar" onclick="Despachar_OnClick" Font-Bold="True" runat="server" Font-Size="XX-Small"
			Text="Despachar"></asp:Button></FIELDSET>
</asp:panel><asp:panel id="Panel2" runat="server" Height="96px" Width="544px" Visible="False">
	<FIELDSET style="WIDTH: 552px; HEIGHT: 88px"><LEGEND>Recepcion</LEGEND>
		<TABLE id="Table3" style="WIDTH: 544px; HEIGHT: 57px" cellSpacing="0" cellPadding="0" width="544"
			border="0">
			<TR>
				<TD style="WIDTH: 158px; HEIGHT: 15px">
					<asp:Label id="Label11" Font-Bold="True" runat="server" Font-Size="XX-Small">Fecha Recepcion</asp:Label></TD>
				<TD style="HEIGHT: 15px">
					<asp:TextBox id="fechaRe" runat="server" Width="95px"></asp:TextBox></TD>
				<TD style="HEIGHT: 15px"></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 158px">
					<asp:Label id="Label10" Font-Bold="True" runat="server" Font-Size="XX-Small">Tipo de Documento</asp:Label></TD>
				<td>
					<asp:DropDownList id="DocRece" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="136px"></asp:DropDownList></TD>
				<td></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 158px">
					<asp:Label id="Label9" Font-Bold="True" runat="server" Font-Size="XX-Small">Secuencia</asp:Label></TD>
				<td>
					<asp:Label id="Label8" Font-Bold="True" runat="server" Font-Size="XX-Small">Inicio</asp:Label>
					<asp:TextBox id="IniSecRE" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="40px">0</asp:TextBox>
					<asp:Label id="Label7" Font-Bold="True" runat="server" Font-Size="XX-Small">Fin</asp:Label>
					<asp:TextBox id="FinSecRE" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="40px">0</asp:TextBox></TD>
				<td></TD>
			</TR>
		</TABLE>
		<asp:Button id="recepcionar" onclick="Recepcionar_OnClick" Font-Bold="True" runat="server" Font-Size="XX-Small"
			Text="Recepcionar"></asp:Button></FIELDSET>
</asp:panel><asp:panel id="Panel3" runat="server" Height="106px" Width="528px" Visible="False">
	<FIELDSET style="WIDTH: 552px; HEIGHT: 88px"><LEGEND>Retorno Papeleria</LEGEND>
		<TABLE id="Table4" style="WIDTH: 544px; HEIGHT: 57px" cellSpacing="0" cellPadding="0" width="544"
			border="0">
			<TR>
				<TD style="HEIGHT: 9px">
					<asp:Label id="Label20" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Agencia</asp:Label></TD>
				<TD style="HEIGHT: 9px">
					<asp:DropDownList id="agenciaRET" runat="server" Font-Size="XX-Small" Width="250px"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 158px; HEIGHT: 15px">
					<asp:Label id="Label16" Font-Bold="True" runat="server" Font-Size="XX-Small">Fecha Retorno</asp:Label></TD>
				<TD style="HEIGHT: 15px">
					<asp:TextBox id="fechaRetorno" runat="server" Width="95px"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 158px; HEIGHT: 9px">
					<asp:Label id="Label15" Font-Bold="True" runat="server" Font-Size="XX-Small">Tipo de Documento</asp:Label></TD>
				<TD style="HEIGHT: 9px">
					<asp:DropDownList id="DocRet" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="136px"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 158px">
					<asp:Label id="Label14" Font-Bold="True" runat="server" Font-Size="XX-Small">Secuencia</asp:Label></TD>
				<td>
					<asp:Label id="Label13" Font-Bold="True" runat="server" Font-Size="XX-Small">Inicio</asp:Label>
					<asp:TextBox id="IniSecRET" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="40px">0</asp:TextBox>
					<asp:Label id="Label12" Font-Bold="True" runat="server" Font-Size="XX-Small">Fin</asp:Label>
					<asp:TextBox id="FinSecRET" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="40px">0</asp:TextBox></TD>
			</TR>
			<TR>
				<td>
					<asp:Label id="Label19" ForeColor="Black" Font-Bold="True" runat="server" Font-Size="XX-Small">Responsable</asp:Label></TD>
				<td>
					<asp:TextBox id="responsableRET" runat="server" Font-Size="XX-Small" Width="192px"></asp:TextBox></TD>
			</TR>
		</TABLE>
		<asp:Button id="Button1" onclick="Recepcionar2_OnClick" Font-Bold="True" runat="server" Font-Size="XX-Small"
			Text="Recepcionar"></asp:Button></FIELDSET>
</asp:panel>
