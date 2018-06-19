<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.Creditos.ascx.cs" Inherits="AMS.Vehiculos.Creditos" %>
<link rel="stylesheet" href="../css/tabber.css" TYPE="text/css" MEDIA="screen">
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type="text/javascript" src="../js/tabber.js"></script>
<fieldset>
<div class="tabber" id="mytab1">
    <asp:PlaceHolder ID="plcCredito" Visible="false" runat="server">
        <div class="tabbertab" title="Selección">
            <table id="Table" class="filtersIn">
                <tr>
                    <td>Número de Crédito:&nbsp;</td>
                    </tr>
                    <tr>
                    <td><asp:DropDownList ID="ddlCredito" class="dmediano" runat="server"></asp:DropDownList>&nbsp;&nbsp;</td>
                    <td><asp:button id="btnSeleccionar" onclick="Sel_Credito" runat="server" Text="Seleccionar"></asp:button></td>
                </tr>
            </table>
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="plcRegistro" Visible="false" runat="server">
        <div class="tabbertab" title="Datos Básicos">
            <table>
                <tr>
                    <td>Número Crédito : </td>
                    <td><asp:label id="lblNumero" class="lmediano" runat="server" Font-Bold="true"></asp:label></td>
                </tr>
                <tr>
			        <td>Prefijo Pedido :</td>
			        <td><asp:DropDownList id="ddlPrefPed" class="dmediano" runat="server" OnSelectedIndexChanged="CambioPrefijoPedido" AutoPostBack="True"></asp:DropDownList></td>
	            </tr>
	            <tr>
			        <td>Número Pedido :</td>
			        <td><asp:DropDownList id="ddlNumPed" class="dmediano" runat="server" OnSelectedIndexChanged="CambioNumeroPedido" AutoPostBack="True"></asp:DropDownList></td>
		        </tr>
                <tr>
                    <td>Entidad Financiera :</td>
			        <td><asp:label id="LabelFinanciera" class="lmediano" runat="server"  Font-Bold="true"></asp:label></td>
			        <td><asp:label id="LabelFinancieraa" class="lmediano" runat="server"  Font-Bold="true"></asp:label></td>
                </tr>
                
                                
                <tr>
                   <td>Nombre Cliente: </td>
                   <td><asp:label id="nom_cliente" class="lmediano" runat="server"  Font-Bold="true"></asp:label></td>
                </tr>
		        
		        <tr>
			        <td>Cédula o Nit :</td>
			        <td><asp:label id="cc_cliente" class="lmediano" runat="server" Font-Bold="true"></asp:label></td>
	            </tr>
	            
                <tr>
                   <td>Catálogo de Vehiculo : </td>
                   <td><asp:label id="catalogo_vehiculo" class="lmediano" runat="server"  Font-Bold="true"></asp:label></td>
                </tr>
		        
		        <tr>
                   <td>Teléfono Cliente : </td>
                   <td><asp:label id="tel_cliente" class="lmediano" runat="server"  Font-Bold="true"></asp:label></td>
                </tr>
                
		       <tr>
                   <td>Nombre del Vendedor :</td>
                   <td><asp:label id="nom_vendedor" class="lmediano" runat="server"  Font-Bold="true"></asp:label></td>
                </tr>
                
                
		        <tr>
		            <td>Valor Solicitado :</td>
			        <td><asp:TextBox id="txtValSolicitado" ReadOnly="true" runat="server" class="tmediano"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td>Número Meses :</td>
			        <td><asp:TextBox id="txtMeses" class="tmediano" runat="server"></asp:TextBox></td>
		        </tr>
		        <tr>
			        <td>Origen :</td>
			        <td><asp:DropDownList id="ddlOrigen" class="dmediano" runat="server"></asp:DropDownList></td>
		        </tr>
		        <tr>
		            <td></td>
		            <td><asp:button id="btnCrear" onclick="btnCrear_Click" runat="server" Text="Crear" Visible="False"></asp:button><asp:button id="btnActualizar" onclick="btnActualizar_Click" runat="server" Width="73px" Text="Actualizar" Visible="False"></asp:button></td>
		        </tr>
		        <asp:PlaceHolder runat="server" ID="plcAnular" Visible="false">
		            <tr>
		                <td>Denegado:</td>
		                <td>Detalles:&nbsp;<asp:TextBox id="txtDenegado" runat="server" Width="200px"></asp:TextBox>&nbsp;<asp:button id="btnDenegado" onclick="btnDenegado_Click" runat="server" Width="73px" Text="Denegado"></asp:button></td>
		            </tr>
		            <tr>
		                <td>Anular:</td>
		                <td>Razón:&nbsp;<asp:TextBox id="txtRazonAnula" runat="server" Width="200px"></asp:TextBox>&nbsp;<asp:button id="btnAnular" onclick="btnAnular_Click" runat="server" Width="73px" Text="Anular"></asp:button></td>
		            </tr>
		        </asp:PlaceHolder>
            </table>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcSolicitud" Visible="false" runat="server">
        <div class="tabbertab" title="Solicitud">
            <table>
		        <tr>
		            <td>Fecha Solicitud :</td>
			        <td><asp:TextBox id="txtFechaSolicitud" onkeyup="DateMask(this)" runat="server" CssClass="AlineacionDerecha" Width="80px"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td>Observaciones :</td>
			        <td><asp:TextBox id="txtObsSolicitud" runat="server" Width="400px" TextMode="MultiLine" Height="30px"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td></td>
		            <td><asp:button id="btnSolicitar" onclick="btnSolicitar_Click" runat="server" Text="Solicitar" Visible="false"></asp:button></td>
		        </tr>
            </table>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcAprobacion" Visible="false" runat="server">
        <div class="tabbertab" title="Aprobación">
            <table>
		        <tr>
		            <td>Valor Aprobado :</td>
			        <td><asp:TextBox id="txtValAprobado" onkeyup="NumericMaskE(this,event)" runat="server" Width="100px" CssClass="AlineacionDerecha"></asp:TextBox></td>
		        </tr>
		        <tr>
			        <td>Tipo Desembolso :</td>
			        <td><asp:DropDownList id="ddlDesembolso" runat="server"></asp:DropDownList></td>
		        </tr>
		        <tr>
			        <td>Número Aprobación :</td>
			        <td><asp:TextBox id="txtNumAprobacion" runat="server" Width="100px" CssClass="AlineacionDerecha"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td>Fecha Aprobación :</td>
			        <td><asp:TextBox id="txtFechaAprobacion" onkeyup="DateMask(this)" runat="server" CssClass="AlineacionDerecha" Width="80px"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td></td>
		            <td><asp:button id="btnAprobar" onclick="btnAprobar_Click" runat="server" Text="Aprobar" Visible="false"></asp:button></td>
		        </tr>
            </table>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcDesembolso" Visible="false" runat="server">
        <div class="tabbertab" title="Desembolso">
            <table>
		        <tr>
		            <td>Fecha Desembolso :</td>
			        <td><asp:TextBox id="txtFechaDesembolso" onkeyup="DateMask(this)" runat="server" CssClass="AlineacionDerecha" Width="80px"></asp:TextBox></td>
		            <td>Tipo de Desembolso</td>
                    <td><asp:DropDownList ID="ddlTipoDesembolso" runat="server"></asp:DropDownList></td>		        
		        </tr>
		        <tr>
		            <td>Número :</td>
			        <td><asp:TextBox id="txtCheque" runat="server" class="tmediano"></asp:TextBox></td>
			        <td>Cod y Banco del Cheque :</td>
			        <td><asp:DropDownList id="ddlBancoCheque" runat="server"></asp:DropDownList></td>
		        	
		        </tr>
		        <tr>
		            <td>Valor :</td>
			        <td><asp:TextBox id="txtValorDesembolso" onkeyup="NumericMaskE(this,event)" runat="server" Width="100px" CssClass="AlineacionDerecha"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td></td>
		            <td><asp:button id="btnDesembolso" onclick="btnDesembolso_Click" runat="server" Text="Desembolsar" Visible="false"></asp:button></td>
		        </tr>
            </table>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcLegalizacion" Visible="false" runat="server">
        <div class="tabbertab" title="Legalización">
            <table>
		        <tr>
		            <td>Fecha Legalización :</td>
			        <td><asp:TextBox id="txtFechaLegalizacion" onkeyup="DateMask(this)" runat="server" CssClass="AlineacionDerecha" Width="80px"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td>Número Carta Aval :</td>
			        <td><asp:TextBox id="txtNumCarta" runat="server" class="tpequeno"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td></td>
		            <td><asp:button id="btnLegalizar" onclick="btnLegalizar_Click" runat="server" Text="Aprobar" Visible="false"></asp:button></td>
		        </tr>
            </table>
        </div>
    </asp:PlaceHolder>
</div>
</fieldset>
<asp:label id="lblError" runat="server"></asp:label>
