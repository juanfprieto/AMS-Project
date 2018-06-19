<%@ Control Language="c#" codebehind="AMS.Inventarios.CfgUbicaciones.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ConfiguradorUbicaciones" %>
<link rel="stylesheet" href="../css/bodegas.css" TYPE="text/css">
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/tabpane.js" type="text/javascript"></script>
<script type="text/javascript">
function MostrarRefs(obTex,obCmbLin)
{
    ModalDialog(obTex, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE plin.plin_tipo = (SELECT PLIN_TIPO FROM PLINEAITEM AS PL WHERE PL.plin_codigo =\'' + (obCmbLin.value.split('-'))[0] + '\') AND MIT.plin_codigo=PLIN.plin_codigo ORDER By mite_codigo', new Array());
}
	
function EliminarItemUbi(strParamCont, strParamVal)
{
	if(!confirm('¿Esta seguro de eliminar esta referencia en esta ubicación?'))
		return false;
		
	__doPostBack(strParamCont,strParamVal);
}
	
function EliminarUbic(strParamCont, strParamVal)
{
	if(!confirm('¿Esta seguro de eliminar esta ubicación?'))
		return false;

	__doPostBack(strParamCont, strParamVal);

}

function cambio(obj) {
    obj.src = "../img/casillero2.png";
}

function espera() {
    var imaG = document.getElementById("contImg");
    imaG.style.visibility = "visible";
}
</script>
<div id="contImg">
    <img id="imagenCarga" src = "../img/Send3.gif" /><br>
    <label >Cargando...</label>
</div>
<p>

<fieldset >
	<legend>Información de Ubicaciones</legend>
	<asp:table id="tblInfo" runat="server" CssClass="filtersIn"></asp:table>
</fieldset>
</p>
<p>
<asp:placeholder id="plInfoUbiNiv2" runat="server">
    <FIELDSET >
        <legend>Datos Configuración de Ubicación Nivel 2</legend>
		<TABLE class="filtersIn">
			<tr>
				<td  align="justify" colspan="2">A continuación se solicitará el nombre del
					estante que se configurará, (un estante es una columna o módulo de una estantería) y luego escogera un espacio en la
					grilla de la parte inferior. El estante pertenece a
					<asp:Label id="lbNomUbiNiv1" runat="server" font-bold="True"></asp:Label>
                </td>
            </tr>
			<tr>
				<td>Nombre de Ubicación :</td>
				<td  align="right"><asp:TextBox id="tbNomUbiNiv2" CssClass="AlineacionDerecha" runat="server" MaxLength="30" Width="225px"></asp:TextBox>
                </td>
            </tr>
        </TABLE>
    </FIELDSET> 
</asp:placeholder>
</p>

<asp:placeholder id="plInfoUbiNiv3" runat="server">
<p>
    <FIELDSET>
        <legend>Manejo de Niveles</legend>
		<TABLE class="filtersIn">
			<tr>
				<td  colspan="3">
                    <asp:Label id="lbInfo1" runat="server" text="AGREGAR FILA" forecolor="RoyalBlue"></asp:Label>
                </td>
            </tr>
			<tr>
				<td>Nombre del Nivel : <asp:TextBox id="tbNomFil" runat="server" MaxLength="30"></asp:TextBox>&nbsp;</td>
				<td>Dividir en&nbsp; <asp:TextBox id="tbDivCeldas" CssClass="AlineacionDerecha" runat="server" Width="56px">1</asp:TextBox>&nbsp;cajones</td>
				<td  align="right"><asp:Button id="btnAgrFil" onclick="AgregarFilaNiv2" runat="server" Text="Agregar Fila"></asp:Button></td></tr>
			<tr>
				<td  colspan="3">
                    <asp:Label id="lbInfo2" runat="server" text="ELIMINAR FILA" forecolor="RoyalBlue"></asp:Label>
                </td>
            </tr>
			<tr>
				<td>
                    Nivel a Eliminar :&nbsp;&nbsp;&nbsp; <asp:DropDownList id="ddlFilElim" runat="server"></asp:DropDownList>
                </td>
				<td  align="right" colspan="2"><asp:Button id="btnEliFil" onclick="EliminarFila" runat="server" Text="Eliminar Fila"></asp:Button>
                </td>
            </tr>
			<tr>
				<td  align="justify" colspan="3">
					<p>
                        Si NO ingresa un nombre para la fila se le asignará automaticamente un valor Alfabético (A,B,C,etc), 
                        para facilidad defina nombres con prefijo del estante. El número de cajones debe ser mayor o igual a 1. 
                    </p>
                </td>
            </tr>
        </TABLE>
    </FIELDSET>
    </p>
    <FIELDSET >
        <legend>Datos Configuración del Cajón</legend>
		<TABLE class="filtersIn">
			<tr>
				<td  align="justify" colspan="2">
                    Los cajones son los sitios donde
					se almacenaran los ítems. Por lo general, corresponden a como se divide
					una estantería. Por favor ingrese el código del ítem a almacenar y
					seleccione un espacio en la grilla de la parte inferior.
                </td>
            </tr>
			<tr>
				<td>Código del Item :</td>
				<td  align="right">
                    <asp:TextBox id="tbCodigoItem" runat="server" Width="181px"></asp:TextBox>
                </td>
            </tr>
			<tr>
				<td>Línea de Bodega : </td>
				<td  align="right">
                    <asp:DropDownList id="ddlLineaBdg" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </TABLE>
    </FIELDSET> 
</asp:placeholder>

<p>
<div class="contenedor">
<table class="main">
	<tbody>
		<asp:placeholder id="plInfoDimBod" runat="server">
		    <tr>
				<td>
                    Cantidad Máxima de Filas o Calles : <asp:Label id="lbMaxFilas" runat="server"></asp:Label>&nbsp;
                </td>
				<td>
                    Cantidad Máxima de Columnas o Módulos : <asp:Label id="lbMaxCols" runat="server"></asp:Label>
                </td>
            </tr>
		</asp:placeholder>
        <asp:placeholder id="plInfoDimNiv2" runat="server">
			<tr>
				<td  colspan="2">
                    Cantidad Filas : <asp:Label id="lbFilas" runat="server"></asp:Label>
                </td>
            </tr>
		</asp:placeholder>
		<tr>
			<td  colspan="2">
                <asp:datagrid id="dgUbicaciones" runat="server" CssClass="tablaGrande">
					<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
				</asp:datagrid>
            </td>
		</tr>
	</tbody>
</table>
</p>

<asp:button id="btnVolver" onclick="VolverAnterior" runat="server" Text="Volver"></asp:button>
<asp:button id="btnretubi"  onclick="RetornarUbicacion" runat="server" Text="retornarubica" visible="false"></asp:button>
</div>
<div style="DISPLAY:none">
    <asp:linkbutton id="lnkEliminarItem" runat="server">Eliminar 1</asp:linkbutton>
    <asp:linkbutton id="lnkAgregarItem" runat="server">Agregar</asp:linkbutton>
    <asp:linkbutton id="lnkEliminarUbicacion" runat="server">Eliminar 2</asp:linkbutton>
</div>

<asp:label id="lb" runat="server"></asp:label>
