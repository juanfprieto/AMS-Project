<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Contabilidad.ElimCom.ascx.cs" Inherits="AMS.Contabilidad.AMS_Contabilidad_ElimCom" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<script type="text/javascript">      
    function abrirEmergente() 
        {
        ModalDialog(<%=typeDoc2.ClientID%>, "select pdoc_codigo, pdoc_nombre from pdocumento where tvig_vigencia = 'V'", new Array());
    }

    function abrirEmergente1() 
        {
        ModalDialog(<%=typeDoc2.ClientID%>, "select pdoc_codigo, pdoc_nombre from pdocumento where tvig_vigencia = 'V'", new Array());
    }
    
</script>

<p>
	<table id="table1" class="filters">
		<tbody>
			<tr>
			<td class="tittleBox">
                Permite Modificar las imputaciones contables en los comprobantes
			  </td>
                </tr>
            <tr>
				<td>
    				<p>Año:
						<asp:dropdownlist id="yearEdit" class="dpequeno" runat="server" AutoPostBack="True" onselectedindexchanged="ChangeParametersEdit"></asp:dropdownlist>&nbsp;&nbsp; 
						Mes:
						<asp:dropdownlist id="monthEdit" class="dpequeno" runat="server" AutoPostBack="True" onselectedindexchanged="ChangeParametersEdit"></asp:dropdownlist>
                        Tipo:
						<asp:dropdownlist id="typeDoc2" runat="server" OnSelectedIndexChanged="ChangeParametersEdit" AutoPostBack="true"
							class="dmediano"></asp:dropdownlist>
                         <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente()"></asp:Image>
    				</p>
					<p>Comprobante:
						<asp:dropdownlist id="comprobante" class="dmediano" runat="server"></asp:dropdownlist>&nbsp;&nbsp;
                        <br />
						<asp:button id="Editar" onclick="EdiComp" runat="server" Text="Editar"></asp:button></p>
				</td>
			</tr> 
            </table>   
            </p>
            <table id="table2" class="filters">        
           	<tr>
                   
				<td class="tittleBox">
                    Elimina 1 o un grupo de comprobantes contables (Las borra)
			</td>
                </tr>
            <tr>
				<td>
    				<p>Año:
						<asp:dropdownlist id="yearElim" class="dpequeno" runat="server" AutoPostBack="True" onselectedindexchanged="ChangeParametersElim"></asp:dropdownlist>&nbsp;&nbsp; 
						Mes:
						<asp:dropdownlist id="monthElim" class="dpequeno" runat="server" AutoPostBack="True" onselectedindexchanged="ChangeParametersElim"></asp:dropdownlist>
                        Tipo:
						<asp:dropdownlist id="typeDoc3" class="dmediano" runat="server" AutoPostBack="true" onselectedindexchanged="ChangeParametersElim"></asp:dropdownlist>
                        <asp:Image id="imglupa2" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente1()"></asp:Image>
    				</p>
					<p>Comprobante Inicio:
						<asp:dropdownlist id="comprobanteInicio" class="dmediano" runat="server"></asp:dropdownlist><br />
                        Comprobante 
						Inicio:
						<asp:dropdownlist id="comprobanteFin" class="dmediano" runat="server"></asp:dropdownlist><br />
                        <asp:button id="Eliminar" runat="server" Text="Eliminar" onclick="Eliminar_Click" AutoPostBack="true"></asp:button>
                    </p>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p><asp:label id="infoLabel" runat="server"></asp:label></p>
<p></p>

