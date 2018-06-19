<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.VIP.ManejoClientes.ascx.cs" Inherits="AMS.VIP.ManejoClientes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>
<asp:ScriptManager ID="ScriptManager" runat="server" />
<table>
    <tr>
        <td>
            <asp:Label ID="lblPadre" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Table ID="tblPadre" runat="server" Visible="false" BorderWidth="2" BorderStyle="Solid">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label runat="server" Text="Nombre: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNomPadre" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label runat="server" Text="Apellido: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtApePadre" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label1" runat="server" Text="Tipo de Afiliación: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlTipoAfilPadre" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label2" runat="server" Text="Código SAP: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSAP" runat="server" AutoPostBack="true" OnTextChanged="generarCodProdPadre" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label3" runat="server" Text="Cédula: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCedPadre" runat="server" AutoPostBack="true" OnTextChanged="generarCodProdPadre" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label4" runat="server" Text="Fecha de Expedición: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFechaExpPadre" runat="server" />
                        <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txtFechaExpPadre" Format="yyyy-MM-dd"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label runat="server" Text="Código de Producto: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCodProdPadre" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label5" runat="server" Text="Teléfono: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtTelfPadre" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label6" runat="server" Text="Dirección: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtDirPadre" runat="server" />
                    </asp:TableCell>
                     <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label34" runat="server" Text="Inactivo: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:checkbox id="chkInactivo" Checked="False" TextAlign="Left" Text="" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label7" runat="server" Text="Sobregiro: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCapEnd" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label8" runat="server" Text="Cupo Daimler: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCupoDai" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label9" runat="server" Text="Día de Corte para Pagos: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFechaCorte" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label32" runat="server" Text="Cupo Utilizado: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCupoUtil" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label33" runat="server" Text="Cupo Disponible: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCupoDisponible" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label10" runat="server" Text="Información Financiera: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFin" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lblPtsPadre" runat="server" Text="Puntos Acumulados: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPtsPadre" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblHabilitadoFacturacion" runat="server" Visible="false" Font-Size="Large"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblHijo1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Table ID="tblHijo1" runat="server" Visible="false" BorderWidth="2" BorderStyle="Solid">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label11" runat="server" Text="Nombre: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNomHijo1" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label12" runat="server" Text="Apellido: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtApeHijo1" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label13" runat="server" Text="Tipo de Afiliación: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlTipoAfilHijo1" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label14" runat="server" Text="Cédula: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCedHijo1" runat="server" AutoPostBack="true" OnTextChanged="generarCodProdHijo1" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label15" runat="server" Text="Fecha de Expedición: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFechaExpHijo1" runat="server" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaExpHijo1" Format="yyyy-MM-dd"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label16" runat="server" Text="Código de Producto: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCodProdHijo1" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label17" runat="server" Text="Teléfono: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtTelfHijo1" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label18" runat="server" Text="Dirección: " />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <asp:TextBox ID="txtDirHijo1" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lblPtsHijo1" runat="server" Text="Puntos Acumulados: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPtsHijo1" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblHijo2" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Table ID="tblHijo2" runat="server" Visible="false" BorderWidth="2" BorderStyle="Solid">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label19" runat="server" Text="Nombre: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNomHijo2" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label20" runat="server" Text="Apellido: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtApeHijo2" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label21" runat="server" Text="Tipo de Afiliación: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlTipoAfilHijo2" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label22" runat="server" Text="Cédula: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCedHijo2" runat="server" AutoPostBack="true" OnTextChanged="generarCodProdHijo2" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label23" runat="server" Text="Fecha de Expedición: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFechaExpHijo2" runat="server" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaExpHijo2" Format="yyyy-MM-dd"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label24" runat="server" Text="Código de Producto: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCodProdHijo2" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label25" runat="server" Text="Teléfono: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtTelfHijo2" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label26" runat="server" Text="Dirección: " />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <asp:TextBox ID="txtDirHijo2" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lblPtsHijo2" runat="server" Text="Puntos Acumulados: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPtsHijo2" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnAgregarNuevo" runat="server" Text="Agregar Nuevo" OnClick="guardarNuevo" Visible="false"/>
            <asp:Button ID="btnEditar" runat="server" Text="Guardar Cambios" OnClick="guardarEdicion" Visible="false"/>
            <asp:Button ID="btnActivarFacturacion" runat="server" Text="Pasar a Facturar" OnClick="activarFacturacion" Visible="false"/>
        </td>
    </tr>
</table>
<asp:Label ID="lblErrorClientes" runat="server" ForeColor="RosyBrown" Font-Size="10pt"></asp:Label>
<table>
    <tr>
        <td>
            <asp:Table ID="tblFactura" runat="server" Visible="false" BorderWidth="2" BorderStyle="Solid">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label27" runat="server" Text="Código de Factura: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCodFac" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label28" runat="server" Text="Tipo de Factura: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlTipoFac" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label31" runat="server" Text="Aval de Fenalco: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtAval" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label29" runat="server" Text="Valor: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtValorFac" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="Label30" runat="server" Text="Fecha de Pago: " />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFechaPagoFac" runat="server" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFechaPagoFac" Format="yyyy-MM-dd"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Button ID="btnFacturar" runat="server" Text="Facturar" OnClick="facturar"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
    </tr>
</table>
<asp:Label ID="lblErrorFacturacion" runat="server" ForeColor="RosyBrown" Font-Size="10pt"></asp:Label>
 