<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.RecepcionFisica.ascx.cs" Inherits="AMS.Vehiculos.RecepcionFisica" %>

<fieldset>
    <legend>Datos de Recepción</legend>
	<table class="filtersIn" >
		<tr>
		    <td>
                Almacén :<br /><asp:DropDownList id="ddlAlmacen" OnSelectedIndexChanged="CargarPrefijos" AutoPostBack="true" class="dmediano"  runat="server"></asp:DropDownList>
                <asp:Label id="lblAlm" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                Prefijo :<br /><asp:DropDownList id="ddlPrefijo" OnSelectedIndexChanged="CargaNumeroPrefijo" AutoPostBack="true" class="dmediano" runat="server"></asp:DropDownList>
                <asp:Label id="lblPref" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
		    <td>
                Número :<br /><asp:TextBox id="txtNumeroPrefijo" Enabled="false" class="tpequeno" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Prefijo OT :<br /><asp:DropDownList id="ddlPrefijoOT" AutoPostBack="false" onchange="cambioprefOt(this);" class="dmediano" runat="server"></asp:DropDownList>
                <asp:Label id="lblOT" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
		    <td>
                Número OT :<br /><asp:TextBox id="txtNumeroOT" Enabled="false" class="tpequeno" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Catálogo :<br /><asp:DropDownList id="ddlCatalogo" class="dmediano" runat="server" AutoPostBack="false" onChange="llenarVinBasico()"></asp:DropDownList>
                <asp:Label id="lblCat" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
		    <td>
                 VIN :<br /><asp:TextBox id="txtVIN" class="tmediano" runat="server" onblur="ValidarVIN1(this);" onKeyUp="changeUpper(this)"></asp:TextBox>
                <asp:Label id="lblVIN" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Motor :<br /><asp:TextBox id="txtMotor" class="tmediano" runat="server" onblur="ValidarVIN" ></asp:TextBox>
                                <%--<asp:CustomValidator id="CustomValidator1"
                                               ControlToValidate="txtMotor"
                                               ClientValidationFunction="ClientValidate"
                                               Display="Dynamic"
                                               ErrorMessage="prueba!!"          
                                               runat="server"
                                                ValidateEmptyText="true"/>--%>
                <%--<asp:Label id="lblMotor" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>--%>
            </td>
		    <td>
                Placa (Si aplica) :<br /><asp:TextBox id="txtplaca" class="tpequeno" runat="server" onblur="ValidarVIN"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Proveedor :<br /><asp:TextBox id="txtProveedor" class="tmediano" runat="server" placeholder="doble click!" ondblclick="mostrarEmergente()"></asp:TextBox>
                <%--<asp:Label id="lblProv" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>--%>
            </td>
		    <td>
                Color :<br /><asp:DropDownList id="ddlColor" class="dmediano" runat="server"></asp:DropDownList>
                <%--<asp:Label id="lblCol" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>--%>
            </td>
        </tr>
        <tr>
            <td>
                Serie :<br /><asp:TextBox id="txtSerie" class="tmediano" runat="server"></asp:TextBox>
                <%--<asp:Label id="lblSeri" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>--%>
            </td>
		    <td>
                Chasis :<br /><asp:TextBox id="txtChasis" class="tmediano" runat="server"></asp:TextBox>
                <%--<asp:Label id="lblChas" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>--%>
            </td>
        </tr>
        <tr>
            <td>
                Año modelo :<br /><asp:TextBox id="txtModelo" class="tpequeno" runat="server" onkeypress="return soloNumero(event, this)"></asp:TextBox>
                <asp:Label id="lblMod" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
		    <td>
                Kilometraje :<br /><asp:TextBox id="txtKilometros" class="tpequeno" runat="server" onkeypress="return soloNumero(event, this)"></asp:TextBox>Kms.
                <asp:Label id="lblKilo" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Tipo servicio :<br /><asp:DropDownList id="ddlServicio" class="dmediano" runat="server"></asp:DropDownList>
                <asp:Label id="lblSer" Runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
		    <td>
                Opción Vehículo :<br /><asp:DropDownList ID="ddlOpcion" runat="server" class="dmediano"></asp:DropDownList>
		    </td>
        </tr>
    </table>
</fieldset>

<fieldset >
    <legend>Observaciones de la Recepción</legend>
    <asp:TextBox id="txtObservaciones" text="" widht="500" runat="server" TextMode="MultiLine" MaxLength="600" Height="80" style="width: 550px"></asp:TextBox>
</fieldset>

<fieldset>
    <legend>Accesorios</legend>
    <asp:DataGrid id="grdAccesorios" runat="server" cssclass="datagrid" AutoGenerateColumns="FALSE" GridLines="Vertical" ShowFooter="false">
        <FooterStyle CssClass="footer"></FooterStyle>
        <HeaderStyle CssClass="header"></HeaderStyle>
        <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
        <ItemStyle CssClass="item"></ItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="SELECCIONAR" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate > 
                        <asp:CheckBox id="chkAccesorios" Text="" runat="server" Checked="False" Visible="true" />
	                </ItemTemplate> 
				</asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CODIGO ACCESORIO">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="NOMBRE ACCESORIO">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="OBSERVACIÓN">
                    <ItemTemplate>
                        <asp:TextBox id="txtObservacionAccesorio" runat="server" TextMode="MultiLine" MaxLength="200" Height="30" style="width: 350px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
    </asp:DataGrid>
</fieldset>

<fieldset>
    <asp:Button ID="btnRegistrar" Text="Registrar Entrada" runat="server" OnClientClick="validateAll(); return false"/><!-- onclick="BtnRegistrar_Click" -->
</fieldset>
<asp:Label id="lblInfoSistema" Runat="server"></asp:Label>

<script type ="text/javascript">
    var accion = 0;
    var par = 0;

    function cambioprefOt(obj)
    {
        RecepcionFisica.cargarNumOrden(obj.value, cambioPrefijoCallBack);
    }
    function cambioPrefijoCallBack(response)
    {
        var respuesta = response.value;
        
        document.getElementById("<%=txtNumeroOT.ClientID %>").value = respuesta;
    }

    function ValidarVIN1(obj) {
        RecepcionFisica.validar_VIN1(obj.value, ValidarVIN1_CallBack);
    }

    function ValidarVIN1_CallBack(response)
    {
        var respuesta = response.value;
        var catalogo = document.getElementById("<%=ddlCatalogo.ClientID %>");
        var motor = document.getElementById("<%=txtMotor.ClientID %>");
        var placa = document.getElementById("<%=txtplaca.ClientID %>");
        //var color = document.getElementById("<%=ddlColor.ClientID %>");
        var serie = document.getElementById("<%=txtSerie.ClientID %>");
        var chasis = document.getElementById("<%=txtChasis.ClientID %>");
        var anoModelo = document.getElementById("<%=txtModelo.ClientID %>");
        var kilometraje = document.getElementById("<%=txtKilometros.ClientID %>");
        var proovedor = document.getElementById("<%=txtProveedor.ClientID %>");
        var vin = document.getElementById("<%=txtVIN.ClientID%>"); 
        var servicio = document.getElementById("<%=ddlServicio.ClientID%>");
        var opcion = document.getElementById("<%=ddlOpcion.ClientID%>");
        if (respuesta.Tables[0].Rows.length > 0)
        {
            $("#<%=ddlCatalogo.ClientID %>").val(respuesta.Tables[0].Rows[0].PCAT_CODIGO).change();
            if ($("#<%=ddlCatalogo.ClientID %>").val() == "0")
            {
                alert('El vehículo con V.I.N: ' + vin.value + ' no se puede recepcionar porque el catálogo no figura en la lista de precios!');
                vin.value = "";

                
            }
            else
            {
                //vin.value = respuesta.Tables[0].Rows[0].MCAT_VIN;
                //catalogo.value = respuesta.Tables[0].Rows[0].PCAT_CODIGO;
                motor.value = respuesta.Tables[0].Rows[0].MCAT_MOTOR;
                placa.value = respuesta.Tables[0].Rows[0].MCAT_PLACA;
                proovedor.value = respuesta.Tables[0].Rows[0].MPRO_NIT;
                $("#<%=ddlColor.ClientID %>").val(respuesta.Tables[0].Rows[0].PCOL_CODIGO).change();
                serie.value = respuesta.Tables[0].Rows[0].MCAT_SERIE;
                chasis.value = respuesta.Tables[0].Rows[0].MCAT_CHASIS;
                anoModelo.value = respuesta.Tables[0].Rows[0].MCAT_ANOMODE;
                kilometraje.value = respuesta.Tables[0].Rows[0].MCAT_NUMEKILOVENT;
                servicio.value = respuesta.Tables[0].Rows[0].TSER_TIPOSERV;
                $(opcion).val(respuesta.Tables[0].Rows[0].POPC_OPCIVEHI).change();

                if (catalogo.value.trim() != "")                $(catalogo).prop("disabled", true);
                if (motor.value.trim() != "") $(motor).prop("disabled", true);
                if (placa.value.trim() != "") $(placa).prop("disabled", true);
                if (proovedor.value.trim() != "") $(proovedor).prop("disabled", true);
                if ($("#<%=ddlColor.ClientID %>").val().trim() != "0") $("#<%=ddlColor.ClientID %>").prop("disabled", true);
                if (serie.value.trim() != "") $(serie).prop("disabled", true);
                if (chasis.value.trim() != "") $(chasis).prop("disabled", true);
                if (anoModelo.value.trim() != "") $(anoModelo).prop("disabled", true);
                if (kilometraje.value.trim() != "") $(kilometraje).prop("disabled", true);
                if (servicio.value.trim() != "") $(servicio).prop("disabled", true);
                if ($(opcion).val().trim() != "") $(opcion).prop("disabled", true);
            }
            
        }
        else {
            if( $("#<%=txtMotor.ClientID %>").val() != ""
            || $("#<%=txtSerie.ClientID %>").val() != ""
            || $("#<%=txtChasis.ClientID %>").val() != ""
            || $("#<%=txtModelo.ClientID %>").val() != ""
            || $("#<%=txtKilometros.ClientID %>").val() != "")
            {

            }
            else
            {
                motor.value = "";
                placa.value = "";
                proovedor.value = "";
                $("#<%=ddlColor.ClientID %>").val(0).change();
                serie.value = "";
                chasis.value = "";
                anoModelo.value = "";
                kilometraje.value = "";
                $(servicio).val("0").change();
                $(opcion).val("0").change();

                $(catalogo).prop("disabled", false);
                $(motor).prop("disabled", false);
                $(placa).prop("disabled", false);
                $(proovedor).prop("disabled", false);
                $("#<%=ddlColor.ClientID %>").prop("disabled", false);
                $(serie).prop("disabled", false);
                $(chasis).prop("disabled", false);
                $(anoModelo).prop("disabled", false);
                $(kilometraje).prop("disabled", false);
                $(servicio).prop("disabled", false);
                $(opcion).prop("disabled", false);
            }
            
        }
        
    }

    function ValidarVIN()
    {
        var vin = document.getElementById("<%=txtVIN.ClientID%>"); 
        var motor = document.getElementById("<%=txtMotor.ClientID%>");
        var placa = document.getElementById("<%=txtplaca.ClientID%>"); 
                

        if (accion == 0) 
        {
            accion = 1;
            RecepcionFisica.ValidarVIN(vin.value, motor.value, placa.value, ValidarVIN_CallBack);
        }
    }

    function ValidarVIN_CallBack(response) 
    {
        if (response.value != "") 
        {
            alert(response.value);
        }
        accion = 0;
    }

    function llenarVinBasico()
    {
        var catalogo = document.getElementById("<%=ddlCatalogo.ClientID%>").value;
        RecepcionFisica.llenarVinBasico(catalogo, llenarVinBasico_CallBack);
    }

    function llenarVinBasico_CallBack(response)
    {
        var vin = document.getElementById("<%=txtVIN.ClientID%>"); 
        if(vin.value === "" || vin.value.length == 0)
            vin.value = response.value;
    }
    function mostrarEmergente()
    {
        var nit = document.getElementById("<%=txtProveedor.ClientID%>");
        ModalDialog(nit, "SELECT MPR.mnit_nit as NIT, MNI.mnit_apellidos CONCAT ' ' CONCAT coalesce(MNI.mnit_apellido2,'') CONCAT ' ' CONCAT MNI.mnit_nombres AS PROVEEDOR FROM mnit MNI, mproveedor MPR, PCASAMATRIZ CM WHERE MNI.mnit_nit = MPR.mnit_nit AND MNI.mnit_nit = CM.mnit_nit order by MNI.mnit_apellidos", new Array());
    }
   function ClientValidate(source, arguments)
   {
        if (arguments.Value % 2 == 0 ){
            arguments.IsValid = true;
        } else {
            arguments.IsValid = false;
        }
   }
   function validateAll()
   {
       if (!validarCamposNulos())
       {
           alert('No se puede continuar el proceso si hay campos vacios o listas sin elementos');
           $("#<%=btnRegistrar.ClientID %>").addClass("noEspera");
           agregarEspera();
       }
       else
       {
           $("#<%=btnRegistrar.ClientID %>").removeClass("noEspera");
           agregarEspera();
           var accesoriosElegidos = obtenerchecksDataGrid();
           checkPostback(accesoriosElegidos);
       }
           
   }
    function checkPostback(accesoriosElegidos)
    {
        //var greeting = "Good" + ((now.getHours() > 17) ? " evening." : " day.");
        __doPostBack(accesoriosElegidos, $("#<%=ddlAlmacen.ClientID %>").val() + '~' + $("#<%=ddlPrefijo.ClientID %>").val() + '~' + $("#<%=txtNumeroPrefijo.ClientID %>").val()
            + '~' + $("#<%=ddlPrefijoOT.ClientID %>").val() + '~' + $("#<%= txtNumeroOT.ClientID%>").val() + '~' + $("#<%=ddlCatalogo.ClientID %>").val()
            + '~' + $("#<%=txtVIN.ClientID %>").val() + '~' + $("#<%=txtMotor.ClientID %>").val() + '~' + $("#<%=txtplaca.ClientID %>").val()
            + '~' + $("#<%=txtProveedor.ClientID %>").val() + '~' + $("#<%=ddlColor.ClientID %>").val() + '~' + $("#<%=txtSerie.ClientID %>").val()
            + '~' + $("#<%=txtChasis.ClientID %>").val() + '~' + $("#<%=txtModelo.ClientID %>").val() + '~' + $("#<%=txtKilometros.ClientID %>").val()
            + '~' + $("#<%=ddlServicio.ClientID %>").val() + '~' +  $("#<%=ddlOpcion.ClientID %>").val() + '~' 
            + (
                (
                    $("#<%=txtObservaciones.ClientID %>").val() == ''
                ) ? 'Sin observación' : $("#<%=txtObservaciones.ClientID %>").val()
              )
        );
    }

    function validarCamposNulos()
    {
        var proceso = true;
        //alert($("#ddlTales option").length);
        if(    $("#<%=ddlCatalogo.ClientID %>").val() == "0"
            || $("#<%=ddlPrefijo.ClientID %> option").length == 0
            || $("#<%=ddlPrefijoOT.ClientID %> option").length == 0
            || $("#<%=ddlServicio.ClientID %>").val() == "0"
            || $("#<%=ddlOpcion.ClientID %>").val() == "0"
            || $("#<%=ddlColor.ClientID %>").val() == "0")
        {
            proceso = false;
        }
        if (   $("#<%=txtNumeroOT.ClientID%>").val() == ""
            || $("#<%=txtVIN.ClientID %>").val() == ""
            || $("#<%=txtMotor.ClientID %>").val() == ""
            || $("#<%=txtProveedor.ClientID %>").val() == ""
            || $("#<%=txtSerie.ClientID %>").val() == ""
            || $("#<%=txtChasis.ClientID %>").val() == ""
            || $("#<%=txtModelo.ClientID %>").val() == ""
            || $("#<%=txtKilometros.ClientID %>").val() == "")
        {
            proceso = false;
        }

        return proceso;
    }

    function obtenerchecksDataGrid()
    {
        var posiciones = [];
        var num = 0;
        var dataGrid = document.all['<%= grdAccesorios.ClientID %>'];
        var rows = dataGrid.rows;
        for (var index = 1; index < rows.length; index++)
        {
            var checkBox = rows[index].cells[0].childNodes[1];
            if (checkBox.checked)
            {
                posiciones[num] = rows[index].cells[1].innerText + '~' + rows[index].cells[3].childNodes[1].value;
                num++;
            }
                
        }
        return posiciones;
    }
    function changeUpper(obj)
    {
        document.getElementById("<%=txtVIN.ClientID%>").value = obj.value.toUpperCase();
    }
    //function soloNumero(evt,obj)
    //{
    //    var valor = evt.key;
    //    var esLetra = isNaN(valor);
    //    if (esLetra || valor == " ") {
    //        return false;
    //    } else
    //        return true;
    //}
</script>