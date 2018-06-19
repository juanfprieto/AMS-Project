<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenesTaller.DatosPropietario.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.DatosPropietario" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Web.WizardDirection.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type="text/javascript">
    function abrirEmergente() {
        var nit = document.getElementById("<%=datos.ClientID%>");
        ModalDialog(nit, 'SELECT MN.mnit_nit AS CEDULA, MN.mnit_nombres concat \' \' concat coalesce(MN.mnit_nombre2,\'\') AS NOMBRES, MN.mnit_apellidos concat \' \' concat coalesce(MN.mnit_apellido2,\'\') AS APELLIDOS, MN.mnit_direccion AS DIRECCION, MN.pciu_codigo AS CIUDAD, MN.mnit_telefono AS TELEFONO, MN.mnit_celular AS MOVIL, MN.mnit_email AS EMAIL, MN.mnit_web AS WEBSITE FROM mnit MN ORDER BY MN.mnit_nit', new Array(), 1);
    }
</script>
<fieldset class="infield">
	<legend class="Legends">Datos Propietario</legend>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td colspan="2">
					CC&nbsp;:&nbsp;<asp:TextBox id="datos" class="tmediano" ondblclick="ModalDialog(this, 'SELECT MN.mnit_nit AS CEDULA, MN.mnit_nombres concat \' \' concat coalesce(MN.mnit_nombre2,\'\') AS NOMBRES, MN.mnit_apellidos concat \' \' concat coalesce(MN.mnit_apellido2,\'\') AS APELLIDOS, MN.mnit_direccion AS DIRECCION, MN.pciu_codigo AS CIUDAD, MN.mnit_telefono AS TELEFONO, MN.mnit_celular AS MOVIL, MN.mnit_email AS EMAIL, MN.mnit_web AS WEBSITE FROM mnit MN ORDER BY MN.mnit_nit', new Array(),1)"
						runat="server" onkeyup="verificar(this.value);" ReadOnly="True"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorDatos" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="datos">*</asp:RequiredFieldValidator>
                    <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente();"></asp:Image>
				</td>
                <td></td>
			</tr>
			<tr>
				<td colspan="2">
						Nombres&nbsp;:&nbsp;<asp:TextBox id="datosa" runat="server" ReadOnly="True"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorDatosa" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="datosa" >*</asp:RequiredFieldValidator>
				</td>
                <td></td>
            </tr>
            <tr>
				<td colspan="2">
						Apellidos&nbsp;:&nbsp;&nbsp;<asp:TextBox id="datosb" runat="server" ReadOnly="True"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorDatosb" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="datosb">*</asp:RequiredFieldValidator>
				</td>
                <td></td>
			</tr>
			<tr>
				<td>
					Dirección :<br>
					<asp:TextBox id="datosc" runat="server"  ReadOnly="True" onclick="WizardDirection(this);"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorDatosc" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="datosc">*</asp:RequiredFieldValidator>
				</td>
				<td colspan="2">
					Ciudad :<br>
					<asp:DropDownList id="datosd" runat="server" class="dmediano"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					Telefono :
					<asp:TextBox id="datose" runat="server" MaxLength="10"></asp:TextBox>
				</td>
				<td>
					Movil :<br>
					<asp:TextBox id="datosf" runat="server" MaxLength="12"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorDatosf" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="datosf">*</asp:RequiredFieldValidator>
				</td>
				<td>
					E-Mail :<br>
                    <asp:TextBox id="datosg" runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<p>
						Web Stie :
						<asp:TextBox id="datosh" runat="server"></asp:TextBox>
					</p>
				</td>
                <td></td>
			</tr>            
		</tbody>
	</table>
</fieldset>
<p>
    
        <fieldset class="infield">
            <legend class="Legends">Contacto</legend>
            <table class="filtersIn">
            <tr>
			    <td>
					Nombre:&nbsp;<asp:TextBox id="contacto" class="tmediano" runat="server"></asp:TextBox>
				</td>
                <td>
					Teléfono fijo ó movil:&nbsp;<asp:TextBox id="telFijo" class="tmediano" runat="server"></asp:TextBox>
				</td>
                <td>
					Direccion:&nbsp;<asp:TextBox id="txtdireccion" class="tmediano" runat="server"></asp:TextBox>
				</td>
                </tr>
                <tr>
                <td>
					Tipo de Identificación :<br>
                    <asp:DropDownList id="tipid" class="tmediano"  runat="server" ></asp:DropDownList>
				</td>
                <td>
					Numero de identificación :<br>
                    <asp:TextBox id="numid" class="tmediano" runat="server" ></asp:TextBox>
				</td>
                <td>
					Email:<br>
                    <asp:TextBox id="txtemail" class="tmediano" runat="server"></asp:TextBox>
				</td>
                </tr>    
            </table>
         </fieldset>
		
</p>
<p>
<table class="filtersIn">
    <tr>
        <td>
            <fieldset class="infield">
	            <legend class="Legends">Tipo Cliente</legend>
	            <asp:RadioButtonList id="tipoCliente" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
            </fieldset>
        </td>
        <td>
            <fieldset class="infield">
	            <legend class="Legends">Pago</legend>
	            <asp:DropDownList id="tipoPago" runat="server"></asp:DropDownList>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button id="confirmar" onclick="Confirmar" Visible="true" text="Validar" runat="server" Width="138px" Enabled="true" class="noEspera"></asp:Button>
        </td>
    </tr>
</table>
</p>
<p>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
<script type ="text/javascript">

function verificar(obj)
{
	DatosPropietario.BuscarCedula(obj,BuscarCedula_CallBack);	
}
function BuscarCedula_CallBack(response)
{
	var respuesta=response.value;
	if(respuesta.Tables[0].Rows.length==0)
	{
		alert('No se encuentra Ninguna Cedula con este Numero... Si desea Agregarla por Favor Doble Click sobre el Campo CC');
		var ced=document.getElementById("<%=datos.ClientID%>");
		ced.value=ced.value.substring(0,ced.value.length-1);
	}
	if(respuesta.Tables[1].Rows.length!=0)
		{
			var nombre=document.getElementById("<%=datosa.ClientID%>");
			var apellido=document.getElementById("<%=datosb.ClientID%>");
			var direccion=document.getElementById("<%=datosc.ClientID%>");
			var telefono=document.getElementById("<%=datose.ClientID%>");
			var movil=document.getElementById("<%=datosf.ClientID%>");
			var mail=document.getElementById("<%=datosg.ClientID%>");
			var web=document.getElementById("<%=datosh.ClientID%>");
			var ciudad=document.getElementById("<%=datosd.ClientID%>");
			
			if(respuesta.Tables[1].Rows[0].NOMBRE!='')
			{
			  nombre.value=respuesta.Tables[1].Rows[0].NOMBRE;
			}
			if(respuesta.Tables[1].Rows[0].APELLIDOS!='')
			{
				apellido.value=respuesta.Tables[1].Rows[0].APELLIDOS;
			}
			if(respuesta.Tables[1].Rows[0].DIRECCION!='')
			{	
				direccion.value=respuesta.Tables[1].Rows[0].DIRECCION;
			}
			if(respuesta.Tables[1].Rows[0].CIUDAD!='')
			{
				ciudad.value=respuesta.Tables[1].Rows[0].CIUDAD;
			}
			if(respuesta.Tables[1].Rows[0].TELEFONO!='')
			{	
				telefono.value=respuesta.Tables[1].Rows[0].TELEFONO;
			}
			if(respuesta.Tables[1].Rows[0].CELULAR!='')
			{	
				movil.value=respuesta.Tables[1].Rows[0].CELULAR;
			}
			if(respuesta.Tables[1].Rows[0].EMAIL!='')
			{
				mail.value=respuesta.Tables[1].Rows[0].EMAIL;
			}
			if(respuesta.Tables[1].Rows[0].WEB!='')
			{	
				web.value=respuesta.Tables[1].Rows[0].WEB;
			}	
		}
}

</script>

