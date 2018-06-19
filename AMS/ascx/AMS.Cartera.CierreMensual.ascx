<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Cartera.CierreMensual.ascx.cs" Inherits="AMS.Finanzas.AMS_Cartera_CierreMensual" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<!--proceso Cartera -->
<fieldset id="fldCartera" runat="server" visible ="false">
    <table id="Table1" class="filtersIn">
        <tbody>
            <tr>
                <td>
                    <p>
                        El Cierre Mensual o Administrativo de Cartera, es el procedimiento para 
	                    finalizar la actividad de&nbsp;cartera del mes vigente y&nbsp;no&nbsp;permitir 
	                    la creación&nbsp;facturas&nbsp;con fecha diferente al año y mes vigente.
                    </p>
                </td>
            </tr>
            <tr>
		        <td>
                    Mes Vigente:&nbsp;
			        <b><asp:Label id="mesVigenteC" runat="server"></asp:Label>&nbsp;</b><br /><br />
                    Año Vigente:&nbsp;
			        <b><asp:Label id="anoVigenteC" runat="server"></asp:Label>&nbsp;</b>
		        </td>
	        </tr>
        </tbody>
    </table>
    <P><br />
	    <asp:Button id="btnProcesoC" runat="server" Text="Realizar Proceso" onclick="proceso_Cartera"></asp:Button>
    </P>
    <P>
        <asp:Label id="lbC" runat="server"></asp:Label>
    </P>
</fieldset>

<!--proceso Tesoreria -->
<fieldset id="fldTesoreria" runat="server" visible ="false">
    <table class="filtersIn">
        <tbody>
            <tr>
                <td>
                    <p>
                        El Cierre Mensual o Administrativo de Tesorería, es el procedimiento para 
	                    finalizar la actividad de&nbsp;tesorería del mes vigente y&nbsp;no&nbsp;permitir 
	                    la creación&nbsp;de facturas&nbsp;con fecha diferente al año y mes vigente.
                    </p>
                </td>
            </tr>
            <tr>
		        <td>
                    Mes Vigente:&nbsp;
                    <b><asp:Label id="mesVigenteT" runat="server"></asp:Label>&nbsp; </b><br /><br />
			        
                    Año Vigente:&nbsp;
                    <b><asp:Label id="anoVigenteT" runat="server"></asp:Label>&nbsp;</b>
			        
		        </td>
	        </tr>
        </tbody>
    </table>
    <P><br />
	    <asp:Button id="btnProcesoT" runat="server" Text="Realizar Proceso" onclick="proceso_Tesoreria"></asp:Button>
    </P>
    <P>
        <asp:Label id="lbT" runat="server"></asp:Label>
    </P>
</fieldset>

