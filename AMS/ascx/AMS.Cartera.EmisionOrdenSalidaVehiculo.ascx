<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.EmisionOrdenSalidaVehiculo.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.EmisionOrdenSalida" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>


<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>

<fieldset>
    <table class="filtersIn">
	    <tr>
		    <td>
                Escoja una opción de busqueda :
		    </td>
	    </tr>
	    <tr>
		    <td>
                <asp:radiobutton id="rbCat" OnCheckedChanged="mostrarPlaceholder" Text="Catálogo y VIN del Vehículo" GroupName="busqueda" AutoPostBack="true"
				    Runat="server"></asp:radiobutton></td>
	    </tr>
	    <tr>
		    <td>
                <asp:radiobutton id="rbPla" OnCheckedChanged="mostrarPlaceholder" Text="Placa del Vehículo" GroupName="busqueda" AutoPostBack="true"
				    Runat="server"></asp:radiobutton>
		    </td>
	    </tr>
	    <tr>
		    <td>
                <asp:radiobutton id="rbOT" OnCheckedChanged="mostrarPlaceholder" Text="Prefijo y Número de la OT" GroupName="busqueda" AutoPostBack="true"
				    Runat="server"></asp:radiobutton>
            </td>
	    </tr>
	    <tr>
		    <td>
                <asp:radiobutton id="rbNit" OnCheckedChanged="mostrarPlaceholder" Text="Nit del Propietario" GroupName="busqueda" AutoPostBack="true"
				    Runat="server"></asp:radiobutton>
		    </td>
	    </tr>
    </table>
    
    <asp:PlaceHolder ID="plhVinVehiculo" runat="server" visible="false">
        <table class="filtersIn">
		    <tr>
			    <td>
                    Catálogo del Vehículo :&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:dropdownlist id="ddlCatalogo" class="dmediano" runat="server" onChange="CambioCatalogo(this)"></asp:dropdownlist>
			    </td>
		    </tr>
		    <tr>
			    <td>
                    VIN del Vehículo :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:dropdownlist id="ddlVIN" class="dmediano" runat="server"></asp:dropdownlist>
			    </td>
		    </tr>
		    <tr>
			    <td>
                    <asp:button id="btnAceptarCat" Text="Aceptar" runat="server" CausesValidation="False" CommandName="CV" onclick="Ejecutar_Proceso"></asp:button>
			    </td>
		    </tr>
	    </table>
    </asp:PlaceHolder>
	
    <asp:PlaceHolder ID="plhPlacaVehiculo" runat="server" visible="false">
        <table class="filtersIn">
		    <tr>
			    <td>
                    Introduzca la placa del vehículo : &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:textbox id="tbPlaca" runat="server" Width="88px"></asp:textbox><asp:customvalidator id="cstPlaca" Runat="server" ErrorMessage="*" Display="Dynamic" ClientValidationFunction="Validar"></asp:customvalidator> &nbsp;&nbsp;&nbsp;&nbsp;
                    
			    </td>
		    </tr>
		    <tr>
			    <td>
                    <asp:button id="btnAceptarPla" Text="Aceptar" runat="server" CommandName="PV" onclick="Ejecutar_Proceso"></asp:button>
			    </td>
		    </tr>
	    </table>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plhOrden" runat="server" visible="false">
        <table class="filtersIn">
		    <tr>
			    <td>
                    Escoja el prefijo de la Orden de Trabajo :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:dropdownlist id="ddlPrefijo" class="dmediano" runat="server" onChange="CambioPrefijoOrden(this)"></asp:dropdownlist>
			    </td>
		    </tr>
		    <tr>
			    <td>
                    Escoja el número de la Orden de Trabajo :&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:dropdownlist id="ddlNumero" class="dpequeno" runat="server"></asp:dropdownlist>
			    </td>
		    </tr>
		    <tr>
			    <td>
                    <asp:button id="btnAceptarOT" Text="Aceptar" runat="server" CausesValidation="False" CommandName="OT" onclick="Ejecutar_Proceso"></asp:button>

			    </td>
		    </tr>
	    </table>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plhNitPropietario" runat="server" visible="false" >
        <table class="filtersIn">
		    <tr>
			    <td>
                    Identificación del Propietario :&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:textbox id="tbNit" ondblclick="ModalDialog(this, 'SELECT M.mnit_nit AS Nit,M.mnit_nombres CONCAT\' \'CONCAT M.mnit_apellidos AS Nombre,M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M,pciudad P WHERE M.pciu_codigo=P.pciu_codigo ORDER BY mnit_nit',1,new Array())"
				 	    class="tmediano" runat="server" ToolTip="Haga Doble Click para iniciar la busqueda"></asp:textbox>
			    </td>
		    </tr>
		    <tr>
                <td>
                    <asp:button id="btnAceptarProp" Text="Aceptar" runat="server" CommandName="NP" onclick="Ejecutar_Proceso"></asp:button>
                </td>
		    </tr>
	    </table>
    </asp:PlaceHolder>
	
    <p><asp:checkbox id="cbCartera" Text="Verificar cartera para hacer la emisión de la orden de salida?"
		    runat="server" Checked="True" onClick="MostrarCheckBox()"></asp:checkbox></p>
    <div id="divCb" style="display: none">
	    <asp:CheckBox id="cbMensaje" runat="server" Checked="True" Text="Mostrar advertencia de NO verificación de cartera en la orden de salida?"></asp:CheckBox>
    </div>
    <p>
        <asp:label id="lb" runat="server" forecolor="Black"></asp:label>

    </p>
</fieldset>
<script type ="text/javascript" language="javascript">

	function CambioCatalogo(obj)
	{
		EmisionOrdenSalida.CambiarCatalogo(obj.value,CambiarCatalogo_CallBack);
	}
	
	function CambiarCatalogo_CallBack(response)
	{
		if(response.error!=null)
		{
			alert(response.error);
			return;
		}
		var ddlvin=document.getElementById("<%=ddlVIN.ClientID%>");
		var btnace=document.getElementById("<%=btnAceptarCat.ClientID%>");
		var respuesta=response.value;
		ddlvin.options.length=0;
		if(respuesta.Tables.length!=0)
		{
			if(respuesta.Tables[0].Rows.length!=0)
			{
				for(var i=0;i<respuesta.Tables[0].Rows.length;i++)
				{
					ddlvin.options[ddlvin.options.length]=new Option(respuesta.Tables[0].Rows[i].VIN,respuesta.Tables[0].Rows[i].VIN);
				}
				btnace.disabled=false;
			}
			else
			{
				alert('No hay VINs asociados a este catálogo');
				btnace.disabled=true;
			}
		}
		else
		{
			alert('Error Interno de Consulta');
			btnace.disabled=true;
		}
	}
	
	function CambioPrefijoOrden(obj)
	{
		EmisionOrdenSalida.CambiarOrden(obj.value,CambiarOrden_CallBack);
	}
	
	function CambiarOrden_CallBack(response)
	{
		if(response.error!=null)
		{
			alert(response.error);
			return;
		}
		var ddlnumot=document.getElementById("<%=ddlNumero.ClientID%>");
		var btnace=document.getElementById("<%=btnAceptarOT.ClientID%>");
		ddlnumot.options.length=0;
		var respuesta=response.value;
		if(respuesta.Tables.length!=0)
		{
			if(respuesta.Tables[0].Rows.length!=0)
			{
				for(var i=0;i<respuesta.Tables[0].Rows.length;i++)
				{
					ddlnumot.options[ddlnumot.options.length]=new Option(respuesta.Tables[0].Rows[i].NUMERO,respuesta.Tables[0].Rows[i].NUMERO);
				}
				btnace.disabled=false;
			}
			else
			{
				alert('No hay OTs facturadas con este prefijo');
				btnace.disabled=true;
			}
		}
		else
		{
			alert('Error Interno de Consulta');
			btnace.disabled=true;
		}
	}
	
	function Validar(sender,args)
	{
		args.IsValid=true;
		var tb=document.getElementById("tabPlaca");
		var tb2=document.getElementById("tabPro");
		var pla=document.getElementById("<%=tbPlaca.ClientID%>");
		var nit=document.getElementById("<%=tbNit.ClientID%>");
		if(tb.style.display=='')
		{
			if(pla.value=='')
			{
				args.IsValid=false;
				return;
			}
		}
		else if(tb2.style.display=='')
		{
			if(nit.value=='')
			{
				args.IsValid=false;
				return;
			}
		}
	}
	
	function MostrarCheckBox()
	{
		var obj=document.getElementById("<%=cbCartera.ClientID%>");
		var dv=document.getElementById("divCb");
		if(obj.checked)
		    dv.style.display = 'none';
		else
			dv.style.display='';
	}
</script>
