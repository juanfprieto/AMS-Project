<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.EliminarRemesaGiro.ascx.cs" Inherits="AMS.Comercial.EliminarRemesaGiro" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Mask.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<div align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 439px" colSpan="2"><b>Informacion del giro o Encomienda</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 5px"><asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia :</asp:label></td>
			<td style="WIDTH: 281px; HEIGHT: 5px"><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small" Width="150px" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:dropdownlist id="ddlTipoAsociar" runat="server" Font-Size="XX-Small" Width="150px" AutoPostBack="True">
						<asp:ListItem Value="---seleccione---">---seleccione---</asp:ListItem>
						<asp:ListItem Value="Giros">Giros</asp:ListItem>
						<asp:ListItem Value="Remesas">Remesas</asp:ListItem>
					</asp:dropdownlist></TD>
			</TR>
				
	
			<TR>
				<td>
					<asp:label id="Label35" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero de Documento :</asp:label></TD>
				<TD style="WIDTH: 281px">
					<asp:textbox id="txtNumeroDocumento" runat="server" Font-Size="XX-Small" Width="80px" MaxLength="10"></asp:textbox></TD>
			</TR>
	
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 439px" colSpan="2"><b>Eliminar</b></td>
		</tr>
		
		
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrVentas" runat="server" Width="500" ShowFooter="False" AutoGenerateColumns="true">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					
						
					</asp:datagrid></TD>
			</TR>
		</TABLE>

			<TR>
				<TD style="WIDTH: 154px" vAlign="top" align="right"></TD>
				<TD style="WIDTH: 154px" vAlign="top" align="left"><BR>
					<asp:button id="btnEliminar" Font-Size="XX-Small" Font-Bold="True" Width="100px" Runat="server"
						Text="Eliminar"></asp:button><BR>
				</TD>
		
	</table>
</div>
<P>&nbsp;</P>
