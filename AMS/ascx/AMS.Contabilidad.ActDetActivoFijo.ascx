<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Contabilidad.ActDetActivoFijo.ascx.cs" Inherits="AMS.Contabilidad.ActDetActivoFijo" %>

<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type="text/javascript">
    function confirm()
    {
        var acepta = confirm('Esta seguro de guardar los cambios?');
        if(acepta == false)
        {
            return;
        }
    }

</script>

<fieldset>
    <asp:Label>Activo Fijo:</asp:Label>&nbsp;&nbsp;
    <asp:DropDownList id="ddlActivoFijo" runat="server"  CssClass="dmediano"></asp:DropDownList><br /><br />
    <asp:Button ID="btnCargar" runat="server" OnClick="cargaTexto" Text="Cargar"/>
</fieldset>
<br />

<fieldset id="fldActivos" runat="server" visible="false">
    <table>
        <tr>
            <td>
                <asp:Label>Actual Valor Deterioro NIIF</asp:Label>
            </td>
            <td>
                <asp:Label>Actual Valor Residual NIIF</asp:Label>
            </td>
            <td>
                <asp:Label>Actual Valor del Mercado</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtDeterioroActual"  runat="server" cssClass="tpequeno" Enabled="false"></asp:TextBox>
                <br /><br />
            </td>
            <td>
                <asp:TextBox ID="txtResidualActual" runat="server" cssClass="tpequeno" Enabled="false"></asp:TextBox>
                <br /><br />
            </td>
            <td>
                <asp:TextBox ID="txtMercado" runat="server" cssClass="tpequeno" Enabled="false"></asp:TextBox>
                <br /><br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label>Depreciación Acumulada: </asp:Label>
                
            </td>
            <td>
                <asp:Label>Avaluador: </asp:Label>

            </td>
            <td>
                <asp:Label>Fecha Avaluo: </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label id="lbDepreciacion" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label id="lbAvaluador" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label id="lbFechaA" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button id="btnEditar" runat="server" Text="Editar" OnClick="habilitaFld"/>
</fieldset>
<br />
<fieldset id="fldEdit" runat="server" visible="false">
    <table id="tablaNueva">
        <tr >
            <td colspan="2">
                <asp:Label>Nuevo Valor Deterioro NIIF: </asp:Label>
            </td>
            <td>
                <asp:Label>Nuevo Valor Residual NIIF: </asp:Label>
            </td>
            <td >
                <asp:Label>Nuevo valor del Mercado</asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtDeterioroNuevo"  runat="server" cssClass="tpequeno" onKeyUp="NumericMaskE(this,event)" required ="true" ></asp:TextBox>
                <br /><br />
            </td>
            <td >
                <asp:TextBox ID="txtResidualNuevo" runat="server" cssClass="tpequeno" onKeyUp="NumericMaskE(this,event)" required ="true" ></asp:TextBox>
                <br /><br />
            </td>
            <td >
                <asp:TextBox ID="txtNuevoMercado" runat="server" cssClass="tpequeno" onKeyUp="NumericMaskE(this,event)" required ="true" ></asp:TextBox>
                <br /><br />
            </td>
            
        </tr>
        <tr > 
             <td>
                <asp:Label>Nuevo Avaluador: </asp:Label>
            </td>
            <td>
                <asp:Label>Nueva Fecha Avaluo: </asp:Label>
            </td>
            <td>
               <asp:Label> Observaciones: </asp:Label>
            </td>
            <td>
               <asp:Label> Anexar Soporte: </asp:Label>
            </td>
        </tr>
        <tr>
             <td>
                <asp:TextBox ID="txtAvaluador" runat="server" cssClass="tpequeno" required ="true"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="TxtFechAval" runat="server" cssClass="tpequeno" onKeyUp="DateMask(this)" required ="true" placeholder="aaaa-mm-dd"></asp:TextBox>
            </td>
             <td>
                <textarea id="txtObs" runat="server" style="height:50px; width:250px;"></textarea>
            </td>
            <td>
                <INPUT id="uplFile" type="file" runat="server">
                <asp:Button id="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" CausesValidation="False" cssClass="noEspera"></asp:Button>
                <br />
                Usted ha agregado lo siguientes archivos:
	            <asp:Label id="lbArchivos" runat="server" forecolor="MidnightBlue"></asp:Label>       
            </td>
        </tr>
    </table><br />
    <asp:Button id="btnGuardar" runat="server" Text="Guardar" OnClick="guardarDatos" Visible="false" cssClass="noEspera"/>
</fieldset>