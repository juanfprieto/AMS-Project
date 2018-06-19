<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Produccion.ArbolEnsamble.ascx.cs" Inherits="AMS.Produccion.AMS_Produccion_ArbolEnsamble" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<style type="text/css">
	table
	{
		background: trasparent;
		border: none; 
		color: #2F2F45;
		margin: inherit;
		width: 100%;
		overflow: auto;
		padding-left: 3cm;
	}
	#Table 
	{
	    height:auto;
	    overflow:scroll;
	}
</style>
<table>
	<tbody>
		<tr>
			<td>
				<fieldset><legend>Ensamble:</legend>
					<table class="main">
						<tbody>
							<TR>
								<TD>&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlEnsambles" runat="server"></asp:dropdownlist></TD>
							</TR>
							<tr>
								<td align="right"><asp:button id="btnSeleccionar" runat="server" Text="Seleccionar" Enabled="True" onclick="btnSeleccionar_Click" onClientClick="espera();"></asp:button></td>
							</tr>
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<br>
<fieldset id="fldArbol" runat="server" visible="false">
	    <TABLE id="Table">
		    <TR>
			    <TD>
				    <FIELDSET><LEGEND>Costo Total:</LEGEND>
					    <TABLE class="main">
						    <TR>
							    <TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								    <asp:label id="lblCosto" runat="server" Font-Size="20px"></asp:label></TD>
						    </TR>
					    </TABLE>
				    </FIELDSET>
			    </TD>
		    </TR>
		    <TR>
			    <TD>
				    <FIELDSET><LEGEND>Arbol del Ensamble:</LEGEND>
					    <TABLE>
						    <TR>
							    <TD>&nbsp;</TD>
						    </TR>
						    <TR>
							    <TD>
								    <asp:TreeView ID="tvControl" runat="server" Height="100%" Width="30%"></asp:TreeView></TD>
						    </TR>
						    <TR>
							    <TD>&nbsp;</TD>
						    </TR>
						    <TR>
							    <TD><asp:Label ID="lblCosteo" Runat="Server"></asp:Label>
							    </TD>
						    </TR>
						    <TR>
							    <TD>&nbsp;</TD>
						    </TR>
					    </TABLE>
				    </FIELDSET>
			    </TD>
		    </TR>
	    </TABLE>
</fieldset>
