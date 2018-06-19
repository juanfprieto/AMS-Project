<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.Seleccionar.ascx.cs" Inherits="AMS.Tools.Seleccionar"%>
<P>
	<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="1">
		<tr>
			<td>
				<asp:Label id="lbTitulo1" Runat="server" ForeColor="Navy" Font-Bold="True" Font-Size="Medium"></asp:Label>
			</td>
			<td>
			</td>
			<td>
				<asp:Label id="lbTitulo2" Runat="server" ForeColor="Navy" Font-Bold="True" Font-Size="Medium"></asp:Label>
			</td>
		</tr>
		<TR>
			<TD width="344"><asp:listbox id="Origen" SelectionMode="Multiple" BackColor="#E0E0E0" Height="200px" Width="368px"
					runat="server"></asp:listbox></TD>
			<TD align="center">
				<P><asp:button id="Todos" runat="server" Text=">>" CssClass="noEspera"></asp:button></P>
				<P><asp:button id="Pasar" runat="server" Text=">" CssClass="noEspera"></asp:button></P>
				<P><asp:button id="Quitar" runat="server" Text="<" CssClass="noEspera"></asp:button></P>
				<P><asp:button id="Limpiar" runat="server" Text="<<" CssClass="noEspera"></asp:button></P>
				<P><asp:button id="Button4" runat="server" Text="Actualizar" onclick="Button4_Click" onClientClick="espera()" CssClass="noEspera"></asp:button></P>
			</TD>
			<TD><asp:listbox id="Destino" SelectionMode="Multiple" BackColor="#E0E0E0" Height="200px" Width="374px"
					runat="server"></asp:listbox></TD>
		</TR>
	</TABLE>
</P>
<P></P>
<INPUT type="hidden" runat="server" id="hdn_values" NAME="hdn_values" />
<script language="javascript">
var l1_<%=ClientID%>=document.getElementById('<%=Origen.ClientID%>');
var l2_<%=ClientID%>=document.getElementById('<%=Destino.ClientID%>');
var lO_<%=ClientID%>,lD_<%=ClientID%>;
var hv_<%=ClientID%>=document.getElementById('<%=hdn_values.ClientID%>');
var bAc_<%=ClientID%>=document.getElementById('<%=Button4.ClientID%>');
function fnL_<%=ClientID%>(sels){
	lO_<%=ClientID%>=l2_<%=ClientID%>;
	lD_<%=ClientID%>=l1_<%=ClientID%>;
	fnM_<%=ClientID%>(sels);
	
	if(l2_<%=ClientID%>.length==0) bAc_<%=ClientID%>.disabled = true;
	else                           bAc_<%=ClientID%>.disabled = false;
}
function fnR_<%=ClientID%>(sels){
	lO_<%=ClientID%>=l1_<%=ClientID%>;
	lD_<%=ClientID%>=l2_<%=ClientID%>;
	fnM_<%=ClientID%>(sels);
	
	if(l2_<%=ClientID%>.length==0) bAc_<%=ClientID%>.disabled = true;
	else                           bAc_<%=ClientID%>.disabled = false;
}
function fnM_<%=ClientID%>(sel){
	var tot=lO_<%=ClientID%>.length;
	for(var i=0;i<tot;i++){
        if(sel==false || lO_<%=ClientID%>[i].selected){
            fnX_<%=ClientID%>(i);
            i--;tot--;
        }
    }
    hv_<%=ClientID%>.value='';
    for(var i=0;i<l2_<%=ClientID%>.length;i++)
		hv_<%=ClientID%>.value+=l2_<%=ClientID%>[i].value+',';
}
function fnX_<%=ClientID%>(iVal){
    lD_<%=ClientID%>.appendChild(lO_<%=ClientID%>[iVal].cloneNode(true));
    lO_<%=ClientID%>.removeChild(lO_<%=ClientID%>[iVal]);
}

</script>
